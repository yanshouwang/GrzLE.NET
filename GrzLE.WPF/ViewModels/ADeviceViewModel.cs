using GrzLE.WPF.Models;
using GrzLE.WPF.Services;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrzLE.WPF.ViewModels
{
    class ADeviceViewModel : BaseViewModel
    {
        private ADeviceModel _model;
        public ADeviceModel Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public ADeviceViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);

            Model = (ADeviceModel)context.Parameters["Device"];
        }
    }
}
