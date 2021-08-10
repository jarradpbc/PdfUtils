using DevExpress.Pdf;
using System;
using System.Windows.Input;

namespace PdfModify.ViewModels
{
    class SplitViewModel : ViewModelBase
    {
        private string _selectedFilepath;
        public string SelectedFilepath
        {
            get => _selectedFilepath;
            set
            {
                _selectedFilepath = value;
                OnPropertyChanged(nameof(SelectedFilepath));
            }
        }

        private int _splitAt = 1;
        public int SplitAt
        {
            get => _splitAt;
            set
            {
                _splitAt = value;
                OnPropertyChanged(nameof(SplitAt));
            }
        }

        private int _filePageCount = 1;
        public int FilePageCount
        {
            get => _filePageCount;
            set
            {
                _filePageCount = value;
                OnPropertyChanged(nameof(FilePageCount));
            }
        }

        private ICommand _fileSelectCommand;
        public ICommand FileSelectCommand
        {
            get
            {
                return _fileSelectCommand ??= (_fileSelectCommand = new CommandHandler(() => FileSelect(), () => true));
            }
        }

        public void FileSelect()
        {
            // create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // set filter for file extension and default file extension 
            dlg.DefaultExt = ".pdf";
            dlg.Filter = "PDF Files (*.pdf)|*.pdf";

            // display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // get the selected file name
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;

                SelectedFilepath = filename;

                Console.WriteLine(filename);
            }
            GetPageCount();
        }

        private void GetPageCount()
        {
            using (PdfDocumentProcessor source = new PdfDocumentProcessor())
            {
                source.LoadDocument(SelectedFilepath);

                FilePageCount = source.Document.Pages.Count - 1;
            }
        }

        private ICommand _splitCommand;
        public ICommand SplitCommand
        {
            get
            {
                return _splitCommand ??= (_splitCommand = new CommandHandler(() => SplitPdf(), () => true));
            }
        }

        public void SplitPdf()
        {
            using (PdfDocumentProcessor source = new PdfDocumentProcessor())
            {
                try
                {
                    source.LoadDocument(SelectedFilepath);

                    using (PdfDocumentProcessor target = new PdfDocumentProcessor())
                    {
                        Console.WriteLine("Split at: " + SplitAt);

                        target.CreateEmptyDocument("..\\..\\SplitFirstHalf.pdf");
                        int index = 0;
                        for (int i = 0; i < SplitAt; i++)
                        {
                            target.Document.Pages.Insert(i, source.Document.Pages[i]);
                            index++;
                        }
                        target.SaveDocument("..\\..\\SplitFirstHalf.pdf");


                        target.CreateEmptyDocument("..\\.." + "\\SplitSecondHalf.pdf");
                        //target.Document.Pages.Insert(atPage, source.Document.Pages[0]);
                        index = 0;
                        for (int i = SplitAt; i < source.Document.Pages.Count; i++)
                        {
                            target.Document.Pages.Insert(index, source.Document.Pages[i]);
                            index++;
                        }

                        target.SaveDocument("..\\..\\SplitSecondHalf.pdf");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
