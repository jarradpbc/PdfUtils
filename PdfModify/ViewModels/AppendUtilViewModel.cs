using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using PdfAppendUtil;

namespace PdfModify.ViewModels
{
    class AppendUtilViewModel : ViewModelBase
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

        private ICommand _runUtilCommand;
        public ICommand RunUtilCommand
        {
            get
            {
                return _runUtilCommand ??= (_runUtilCommand = new CommandHandler(() => RunUtil(), () => true));
            }
        }

        public void RunUtil()
        {
            MainClass pdfUtil = new();
            pdfUtil.OutPath = OutputPath;
            pdfUtil.BsnCreateCombinedPdf(SelectedFilepath);
            // open output folder in explorer
            System.Diagnostics.Process.Start(@OutputPath);
        }
    }
}
