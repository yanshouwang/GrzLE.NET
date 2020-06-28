using GrzLE.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace GrzLE.WPF.ViewModels
{
    class AdapterViewModel : BaseViewModel
    {
        readonly BluetoothLEAdvertisementWatcher _watcher;

        public IList<DeviceModel> Devices { get; }

        public AdapterViewModel()
        {
            _watcher = new BluetoothLEAdvertisementWatcher();
            _watcher.Received += OnReceived;
            Devices = new ObservableCollection<DeviceModel>();
            _watcher.Start();
        }

        private void OnReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            Application.Current.Dispatcher.Invoke(async () =>
            {
                var deviceModel = Devices.FirstOrDefault(i => i.Device.BluetoothAddress == args.BluetoothAddress);
                if (deviceModel != null)
                {
                    deviceModel.RSSI = args.RawSignalStrengthInDBm;
                }
                else
                {
                    var device = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);
                    if (device == null)
                        return;
                    deviceModel = device.ToModel(args);
                    Devices.Add(deviceModel);
                }
            });
        }
    }
}
