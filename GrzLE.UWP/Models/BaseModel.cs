using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace GrzLE.UWP.Models
{
    class BaseModel : BindableBase
    {
        protected async void DispatcherRun(Action action)
            => await DispatcherRunAsync(action);

        protected Task DispatcherRunAsync(Action action)
        {
            var handler = new DispatchedHandler(action);
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                handler).AsTask();
        }
    }
}
