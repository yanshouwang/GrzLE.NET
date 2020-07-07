using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace GrzLE.WPF.Services
{
    interface INavigationService
    {
        void Navigate<T>() where T : Page;
        void Navigate<T>(NavigationParameters args) where T : Page;
    }
}
