using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace GrzLE.UWP
{
    static class Extensions
    {
        public static bool Navigate<T>(this INavigationService navigationService, object paramters) where T : Page
        {
            var uri = typeof(T).Name;
            return navigationService.Navigate(uri, paramters);
        }

        public static bool Navigate<T>(this INavigationService navigationService) where T : Page
            => navigationService.Navigate<T>(null);
    }
}
