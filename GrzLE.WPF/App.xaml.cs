using GrzLE.WPF.Native;
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
using Windows.Management.Deployment;

namespace GrzLE.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //var r = Ole32.CoInitializeSecurity(IntPtr.Zero, -1, IntPtr.Zero, IntPtr.Zero, RpcAuthnLevel.Default, RpcImpLevel.Identify, IntPtr.Zero, EoAuthnCap.None, IntPtr.Zero);
            base.OnStartup(e);
        }

        protected override Window CreateShell()
            => Container.Resolve<ShellView>();

        protected override void RegisterTypes(IContainerRegistry cr)
        {
            cr.Register<INavigationService, ShellNavigationService>();
            cr.RegisterForNavigation<AdapterView>();
            cr.RegisterForNavigation<AScanView>();
            cr.RegisterForNavigation<ADeviceView>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            var navigationService = Container.Resolve<INavigationService>();
            navigationService.Navigate<AScanView>();
        }
    }
}
