using Prism.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace GrzLE.UWP.ViewModels
{
    abstract class BaseViewModel : BindableBase, INavigationAware
    {
        protected INavigationService NavigationService { get; }

        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        protected static Task DispatcherRunAsync(Action action)
        {
            var handler = new DispatchedHandler(action);
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                handler).AsTask();
        }

        public virtual void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {

        }

        public virtual void OnNavigatingFrom(NavigatingFromEventArgs e, Dictionary<string, object> viewModelState, bool suspending)
        {

        }
    }
}
