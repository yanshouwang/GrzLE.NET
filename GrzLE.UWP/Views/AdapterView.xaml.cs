using GrzLE.UWP.Models;
using GrzLE.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace GrzLE.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AdapterView : Page
    {
        AdapterViewModel ViewModel => (AdapterViewModel)DataContext;

        public AdapterView()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var device = (DeviceModel)e.ClickedItem;
            ViewModel.NavigateToDeviceCommand.Execute(device);
        }
    }
}
