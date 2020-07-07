using GrzLE.UWP.Models;
using GrzLE.UWP.Views;
using Prism.Commands;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Security.Cryptography;

namespace GrzLE.UWP.ViewModels
{
    class AdapterViewModel : BaseViewModel
    {
        readonly BluetoothLEAdvertisementWatcher _watcher;

        public IList<DeviceModel> DeviceModels { get; }

        public AdapterViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _watcher = new BluetoothLEAdvertisementWatcher();
            _watcher.Received += OnWatcherReceived;

            DeviceModels = new ObservableCollection<DeviceModel>();

            _watcher.Start();
        }

        private void OnWatcherReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            var section = args.Advertisement.DataSections.FirstOrDefault(i => i.DataType == 0xFF);
            if (section == null)
                return;
            CryptographicBuffer.CopyToByteArray(section.Data, out var data);
            if (data.Length < 11)
                return;
            // VId
            var vidArray = new byte[2];
            Array.Copy(data, 0, vidArray, 0, vidArray.Length);
            var vid = BitConverter.ToInt16(vidArray, 0);
            if (vid != 0x2E19 && vid != 0x045E)
                return;
            // PId
            var pidArray = new byte[2];
            Array.Copy(data, 2, pidArray, 0, pidArray.Length);
            var pid = BitConverter.ToInt16(pidArray, 0);
            // MId
            var midArray = new byte[1];
            Array.Copy(data, 4, midArray, 0, midArray.Length);
            var mid = midArray[0];
            // MAC
            var mac = new byte[6];
            Array.Copy(data, 5, mac, 0, mac.Length);
            var macStr = BitConverter.ToString(mac).Replace('-', ':');

            var deviceModel = DeviceModels.FirstOrDefault(i => i.MAC == macStr);
            if (deviceModel != null)
            {
                DispatcherRunAsync(() => deviceModel.RSSI = args.RawSignalStrengthInDBm);
            }
            else
            {
                deviceModel = new DeviceModel(args.BluetoothAddress, vid, pid, mid, macStr, args.RawSignalStrengthInDBm);
                DispatcherRunAsync(() => DeviceModels.Add(deviceModel));
            }
        }

        DelegateCommand<DeviceModel> _navigateToDeviceCommand;
        public DelegateCommand<DeviceModel> NavigateToDeviceCommand
            => _navigateToDeviceCommand ?? (_navigateToDeviceCommand = new DelegateCommand<DeviceModel>(ExecuteNavigateToDeviceCommand));

        void ExecuteNavigateToDeviceCommand(DeviceModel device)
        {
            NavigationService.Navigate<DeviceView>(device);
        }
    }
}
