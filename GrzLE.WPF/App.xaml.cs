using GrzLE.WPF.Services;
using GrzLE.WPF.ViewModels;
using GrzLE.WPF.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GrzLE.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
            => Container.Resolve<ShellView>();

        protected override void RegisterTypes(IContainerRegistry cr)
        {
            cr.Register<INavigationService, ShellNavigationService>();
            cr.RegisterForNavigation<AdapterView>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var navigationService = Container.Resolve<INavigationService>();
            navigationService.Navigate("AdapterView");
        }
    }
}
