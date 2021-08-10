using DevExpress.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PdfModify.ViewModels
{
    class MergeViewModel : ViewModelBase
    {
        private ObservableCollection<string> _pdfFiles = new ObservableCollection<string>();
        public ObservableCollection<string> PdfFiles
        {
            get => _pdfFiles;
            set
            {
                _pdfFiles = value;
                OnPropertyChanged(nameof(PdfFiles));
            }
        }

        private string _outputPath = "..\\..";
        public string OutputPath
        {
            get => _outputPath;
            set
            {
                _outputPath = value;
                OnPropertyChanged(nameof(OutputPath));
            }
        }

        private ICommand _fileSearchCommand;
        public ICommand FileSearchCommand
        {
            get
            {
                return _fileSearchCommand ??= (_fileSearchCommand = new CommandHandler(() => FileSearch(), () => true));
            }
        }

        public void FileSearch()
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

                PdfFiles.Add(filename);

                Console.WriteLine(filename);
            }
        }

        private ICommand _outputFolderCommand;
        public ICommand OutputFolderCommand
        {
            get
            {
                return _outputFolderCommand ??= (_outputFolderCommand = new CommandHandler(() => FolderSelect(), () => true));
            }
        }

        public void FolderSelect()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result.ToString().Equals("OK"))
                {
                    string filePath = dialog.SelectedPath;
                    OutputPath = filePath;
                    Console.WriteLine(OutputPath);
                }
            }
        }

        private ICommand _mergeFilesCommand;
        public ICommand MergeFilesCommand
        {
            get
            {
                return _mergeFilesCommand ??= (_mergeFilesCommand = new CommandHandler(() => MergeFiles(), () => true));
            }
        }

        public void MergeFiles()
        {
            using (PdfDocumentProcessor pdfDocumentProcessor = new PdfDocumentProcessor())
            {
                pdfDocumentProcessor.CreateEmptyDocument(_outputPath + "\\Merged.pdf");

                foreach (string s in PdfFiles)
                {
                    pdfDocumentProcessor.AppendDocument(s);
                }
            }
        }
    }
}