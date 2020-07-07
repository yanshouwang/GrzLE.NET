using GrzLE.UWP.Models;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrzLE.UWP.ViewModels
{
    class DeviceViewModel : BaseViewModel
    {
        private DeviceModel _model;
        public DeviceModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public DeviceViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            Model = (DeviceModel)e.Parameter;
        }
    }
}
