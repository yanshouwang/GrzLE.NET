using GrzLE.WPF.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrzLE.WPF.ViewModels
{
    class BaseViewModel : BindableBase, INavigationAware
    {
        private IRegionNavigationJournal _journal;
        public IRegionNavigationJournal Journal
        {
            get { return _journal; }
            set { SetProperty(ref _journal, value); }
        }

        protected INavigationService NavigationService { get; }

        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        private DelegateCommand _goBackCommand;
        public DelegateCommand GoBackCommand
            => _goBackCommand ??= new DelegateCommand(ExecuteGoBackCommand, CanExecuteGoBackCommand)
            .ObservesProperty(() => Journal);

        bool CanExecuteGoBackCommand()
           => Journal != null && Journal.CanGoBack;

        void ExecuteGoBackCommand()
            => Journal.GoBack();

        private DelegateCommand _goForwardCommand;
        public DelegateCommand GoForwardCommand
            => _goForwardCommand ??= new DelegateCommand(ExecuteGoForwardCommand, CanExecuteGoForwardCommand)
            .ObservesProperty(() => Journal);

        bool CanExecuteGoForwardCommand()
           => Journal != null && Journal.CanGoForward;

        void ExecuteGoForwardCommand()
            => Journal.GoForward();

        public virtual void OnNavigatedTo(NavigationContext context)
            => Journal = context.NavigationService.Journal;

        public virtual bool IsNavigationTarget(NavigationContext context)
            => true;

        public virtual void OnNavigatedFrom(NavigationContext context) { }
    }
}
