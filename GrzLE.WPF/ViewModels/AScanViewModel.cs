using DryIoc;
using GrzLE.WPF.Models;
using GrzLE.WPF.Services;
using GrzLE.WPF.Views;
using ImTools;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Security.Cryptography;

namespace GrzLE.WPF.ViewModels
{
    class AScanViewModel : BaseViewModel
    {
        readonly BluetoothLEAdvertisementWatcher _watcher;

        public IList<ADeviceModel> DeviceModels { get; }

        public AScanViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _watcher = new BluetoothLEAdvertisementWatcher();
            _watcher.Received += OnWatcherReceived;

            DeviceModels = new ObservableCollection<ADeviceModel>();

            _watcher.Start();
        }

        public override void OnNavigatedTo(NavigationContext context)
        {
            base.OnNavigatedTo(context);

            _watcher.Start();
        }

        public override void OnNavigatedFrom(NavigationContext context)
        {
            base.OnNavigatedFrom(context);

            _watcher.Stop();
        }

        private void OnWatcherReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var nameSection = args.Advertisement.DataSections.FirstOrDefault(i => i.DataType == 0x09);
            if (nameSection == null)
                return;
            var uuidSection = args.Advertisement.DataSections.FirstOrDefault(i => i.DataType == 0x07);
            var vendorSection = args.Advertisement.DataSections.FirstOrDefault(i => i.DataType == 0xFF);
            if (uuidSection == null && vendorSection == null)
                return;
            short vid, pid; byte mid; string mac;
            if (uuidSection != null)
            {
                CryptographicBuffer.CopyToByteArray(uuidSection.Data, out var data1);
                Array.Reverse(data1);
                var uuidStr = BitConverter.ToString(data1).Replace("-", string.Empty);
                var uuid = Guid.Parse(uuidStr);
                if (uuid != UUID.ATC_CUSTOM_SERVICE && uuid != UUID.UTC_CUSTOM_SERVICE)
                    return;
                CryptographicBuffer.CopyToByteArray(nameSection.Data, out var data2);
                var mixed = Encoding.ASCII.GetString(data2);
                var pattern = @"(?<LETTER>[ACQRab])(?<NUMBER>\d{3})(?<MAC>(?i)[A-F|0-9]{4})";
                var match = Regex.Match(mixed, pattern);
                if (!match.Success)
                    return;
                vid = 0x2E19;
                mid = 0;
                var letter = match.Groups["LETTER"].Value.ToUpper();
                var number = match.Groups["NUMBER"].Value;
                // 兼容老版本
                if (letter.Equals("A", StringComparison.OrdinalIgnoreCase) || letter.Equals("C", StringComparison.OrdinalIgnoreCase))
                {
                    pid = 0x036B;
                    //name = letter == "A" ? $"Additel {number}" : $"ConST {number}";
                }
                else
                {
                    var firstByte = Encoding.ASCII.GetBytes(letter)[0];
                    //var higher = firstByte.GetHigher();
                    var lower = firstByte.GetLower();
                    pid = (short)(lower == 1 ? 0x036B : 0x518B);
                    //name = higher == 5 ? $"ConST {number}" : $"Additel {number}";
                }
                mac = match.Groups["MAC"].Value;
            }
            else
            {
                CryptographicBuffer.CopyToByteArray(vendorSection.Data, out var data);
                if (data.Length < 11)
                    return;
                // VId
                var vidArray = new byte[2];
                Array.Copy(data, 0, vidArray, 0, vidArray.Length);
                vid = BitConverter.ToInt16(vidArray, 0);
                if (vid != 0x2E19 && vid != 0x045E)
                    return;
                // PId
                var pidArray = new byte[2];
                Array.Copy(data, 2, pidArray, 0, pidArray.Length);
                pid = BitConverter.ToInt16(pidArray, 0);
                // MId
                var midArray = new byte[1];
                Array.Copy(data, 4, midArray, 0, midArray.Length);
                mid = midArray[0];
                // MAC
                var macArray = new byte[6];
                Array.Copy(data, 5, macArray, 0, macArray.Length);
                mac = BitConverter.ToString(macArray).Replace('-', ':');
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                var deviceModel = DeviceModels.FirstOrDefault(i => i.MAC == mac);
                if (deviceModel != null)
                {
                    deviceModel.RSSI = args.RawSignalStrengthInDBm;
                }
                else
                {
                    deviceModel = new ADeviceModel(args.BluetoothAddress, vid, pid, mid, mac, args.RawSignalStrengthInDBm);
                    DeviceModels.Add(deviceModel);
                }
            });
        }

        private DelegateCommand<ADeviceModel> _navigateToDeviceCommand;
        public DelegateCommand<ADeviceModel> NavigateToDeviceCommand
            => _navigateToDeviceCommand ??= new DelegateCommand<ADeviceModel>(ExecuteNavigateToDeviceCommand);

        void ExecuteNavigateToDeviceCommand(ADeviceModel device)
        {
            var args = new NavigationParameters { { "Device", device } };
            NavigationService.Navigate<ADeviceView>(args);
        }
    }
}
