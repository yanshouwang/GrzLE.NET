using GrzLE.WPF.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Bluetooth;

namespace GrzLE.WPF.ViewModels
{
    class DeviceViewModel : BaseViewModel
    {
        public DeviceViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}
