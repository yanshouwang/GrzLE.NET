using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace GrzLE.WPF.Services
{
    class ShellNavigationService : INavigationService
    {
        readonly IRegionManager _rm;

        public ShellNavigationService(IRegionManager rm)
        {
            _rm = rm;
        }

        public void Navigate<T>() where T : Page
        {
            var uri = typeof(T).Name;
            _rm.RequestNavigate("Shell", uri);
        }

        public void Navigate<T>(NavigationParameters args) where T : Page
        {
            var uri = typeof(T).Name;
            _rm.RequestNavigate("Shell", uri, args);
        }
    }
}
