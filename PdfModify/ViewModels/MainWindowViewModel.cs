using System.Windows.Input;

namespace PdfModify.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        public MergeViewModel MergeViewModel { get; } = new MergeViewModel();

        public SplitViewModel SplitViewModel { get; } = new SplitViewModel();

        public AppendUtilViewModel AppendUtilViewModel { get; } = new AppendUtilViewModel();

        public enum ViewTypes
        {
            Merge,
            Split,
            AppendUtil
        }

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView ?? AppendUtilViewModel;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        private ICommand _mergeCommand;
        public ICommand MergeCommand
        {
            get
            {
                return _mergeCommand ??
                       (_mergeCommand = new CommandHandler(() => ChangeView(ViewTypes.Merge), () => true));
            }
        }

        private ICommand _splitCommand;
        public ICommand SplitCommand
        {
            get
            {
                return _splitCommand ??
                       (_splitCommand = new CommandHandler(() => ChangeView(ViewTypes.Split), () => true));
            }
        }

        private ICommand _appendUtilCommand;
        public ICommand AppendUtilCommand
        {
            get
            {
                return _appendUtilCommand ??
                       (_appendUtilCommand = new CommandHandler(() => ChangeView(ViewTypes.AppendUtil), () => true));
            }
        }

        private void ChangeView(ViewTypes viewType)
        {
            switch (viewType)
            {
                case ViewTypes.Merge:
                    CurrentView = MergeViewModel;
                    break;
                case ViewTypes.Split:
                    CurrentView = SplitViewModel;
                    break;
                case ViewTypes.AppendUtil:
                    CurrentView = AppendUtilViewModel;
                    break;
            }
        }
    }
}
