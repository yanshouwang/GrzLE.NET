using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace GrzLE.WPF.Models
{
    class DeviceModel : BaseModel
    {
        public BluetoothLEDevice Device { get; }
        public BluetoothLEAdvertisement Advertisement { get; }

        private short _rssi;
        public short RSSI
        {
            get { return _rssi; }
            set { SetProperty(ref _rssi, value); }
        }

        public DeviceModel(BluetoothLEDevice device, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            Device = device;
            Advertisement = args.Advertisement;
            RSSI = args.RawSignalStrengthInDBm;
        }

        private DelegateCommand _connectCommand;
        public DelegateCommand ConnectCommand
            => _connectCommand ??= new DelegateCommand(ExecuteConnectCommand);

        void ExecuteConnectCommand()
        {

        }
    }
}
