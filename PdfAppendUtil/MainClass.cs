using System;
using System.IO;
using System.Text.RegularExpressions;
using DevExpress.Pdf;

namespace PdfAppendUtil
{
    public class MainClass
    {
        private string _outPath = "..\\..";
        public string OutPath
        {
            set => _outPath = value;
        }
        public void BsnCreateCombinedPdf(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;
            // get file name for saving new pdf
            string fullFileName = Path.GetFileName(filePath);
            string fileName = fullFileName.Substring(0, fullFileName.Length-4);

            // regex pattern to match the following strings
            // (#INSERT:pdffilepath#)
            string regexPattern = "(\\(#INSERT:)(.+)(#\\))";
            Regex rx = new Regex(regexPattern);

            // source document is the base pdf to regex over
            using (PdfDocumentProcessor source = new PdfDocumentProcessor())
            {
                try
                {
                    source.LoadDocument(filePath);

                    // iterate through every page to find regex pattern
                    for (int currentSourcePage = 0; currentSourcePage < source.Document.Pages.Count; currentSourcePage++)
                    {
                        // get all text on current page
                        string pageText = source.GetPageText(currentSourcePage + 1, new PdfTextExtractionOptions { ClipToCropBox = false });

                        Match match = rx.Match(pageText);
                        // if regex pattern found
                        if (match.Success)
                        {
                            // get file path to the target pdf to insert
                            string foundPdf = match.Groups[2].Value;
                            Console.WriteLine("page {0} found pdf file path to insert: {1}", currentSourcePage, foundPdf);

                            // target document to insert into source document
                            using (PdfDocumentProcessor target = new PdfDocumentProcessor())
                            {
                                try
                                {
                                    if (string.IsNullOrEmpty(foundPdf) || !File.Exists(foundPdf)) continue;
                                    target.LoadDocument(foundPdf);

                                    // add every page of target pdf to source
                                    for (int currentTargetPage = 0; currentTargetPage < target.Document.Pages.Count; currentTargetPage++)
                                    {
                                        source.Document.Pages.Insert(currentSourcePage + 1, target.Document.Pages[currentTargetPage]);
                                        // iterate current source page index as a new page has just been inserted
                                        currentSourcePage++;
                                    }
                                    target.CloseDocument();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Exception thrown in " + GetType().Name);
                                    Console.WriteLine("Unable to load target file, incorrect file path?");
                                    Console.WriteLine("File path: " + foundPdf);
                                    Console.WriteLine("Exception log:");
                                    Console.WriteLine(ex);
                                }
                            }
                        }
                    }
                    // save source document
                    source.SaveDocument(_outPath + "\\" + fileName + " COMBINED.pdf", true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception thrown in " + GetType().Name);
                    Console.WriteLine("Unable to load file, incorrect file path?");
                    Console.WriteLine("File path: " + filePath);
                    Console.WriteLine("Exception log:");
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
