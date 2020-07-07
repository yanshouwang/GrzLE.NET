using GrzLE.WPF.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrzLE.WPF.ViewModels
{
    class ShellViewModel : BaseViewModel
    {
        public ShellViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}
