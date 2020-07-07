using GrzLE.UWP.Views;
using Prism.Unity.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GrzLE.UWP
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate<AdapterView>();
            return Task.CompletedTask;
        }

        protected override Type GetPageType(string pageToken)
        {
            var name = GetType().AssemblyQualifiedName;
            var format = name.Replace(GetType().FullName, $"{GetType().Namespace}.Views.{{0}}");
            var typeName = string.Format(CultureInfo.InvariantCulture, format, pageToken);
            var type = Type.GetType(typeName) ?? base.GetPageType(pageToken);
            return type;
        }
    }
}
