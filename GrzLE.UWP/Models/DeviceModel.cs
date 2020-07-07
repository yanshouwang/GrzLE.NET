using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Security.Cryptography;

namespace GrzLE.UWP.Models
{
    class DeviceModel : BaseModel
    {
        const string APC2_1_COMMUNICATION_SERVICE = "41444449-5445-4C42-4C45-4D4F44554C49";
        const string APC2_1_NOTIFY_CHARACTERISTIC = "41444449-5445-4C42-4C45-4D4F44554C44";
        const string APC2_1_WRITE_CHARACTERISTIC = "41444449-5445-4C42-4C45-4D4F44554C44";

        const string PGC_COMMUNICATION_SERVICE = "AF661820-D14A-4B21-90F8-54D58F8614F0";
        const string PGC_NOTIFY_CHARACTERISTIC = "1B6B9415-FF0D-47C2-9444-A5032F727B2D";
        const string PGC_WRITE_CHARACTERISTIC = "1B6B9415-FF0D-47C2-9444-A5032F727B2D";

        readonly ulong _address;
        BluetoothLEDevice _device;
        GattDeviceService _communicationService;
        GattCharacteristic _notifyCharacteristic;
        GattCharacteristic _writeCharacteristic;

        bool CanWriteWithResponse
            => _writeCharacteristic?.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Write) ?? false;
        bool CanWriteWithoutResponse
            => _writeCharacteristic?.CharacteristicProperties.HasFlag(GattCharacteristicProperties.WriteWithoutResponse) ?? false;
        bool CanWrite
            => CanWriteWithResponse || CanWriteWithoutResponse;
        Guid CommunicationId
            => VId == 0x2E19 && PId == 0x02A0
            ? Guid.Parse(PGC_COMMUNICATION_SERVICE)
            : Guid.Parse(APC2_1_COMMUNICATION_SERVICE);
        Guid NotifyId
            => VId == 0x2E19 && PId == 0x02A0
            ? Guid.Parse(PGC_NOTIFY_CHARACTERISTIC)
            : Guid.Parse(APC2_1_NOTIFY_CHARACTERISTIC);
        Guid WriteId
            => VId == 0x2E19 && PId == 0x02A0
            ? Guid.Parse(PGC_WRITE_CHARACTERISTIC)
            : Guid.Parse(APC2_1_WRITE_CHARACTERISTIC);

        public IList<LogModel> Logs { get; }

        public short VId { get; }
        public short PId { get; }
        public byte MId { get; }
        public string MAC { get; }

        public byte MTU => 20;

        private short _rssi;
        public short RSSI
        {
            get { return _rssi; }
            set { SetProperty(ref _rssi, value); }
        }

        private bool _connected;
        public bool Connected
        {
            get { return _connected; }
            set { SetProperty(ref _connected, value); }
        }

        public DeviceModel(ulong address, short vid, short pid, byte mid, string mac, short rssi)
        {
            _address = address;

            VId = vid;
            PId = pid;
            MId = mid;
            MAC = mac;
            RSSI = rssi;

            Logs = new ObservableCollection<LogModel>();
        }

        DelegateCommand _connectCommand;
        public DelegateCommand ConnectCommand
            => _connectCommand ?? (_connectCommand = new DelegateCommand(ExecuteConnectCommand, CanExecuteConnectCommand))
            .ObservesProperty(() => Connected);

        bool CanExecuteConnectCommand()
            => !Connected;

        async void ExecuteConnectCommand()
        {
            _device = await BluetoothLEDevice.FromBluetoothAddressAsync(_address);
            Connected = true;
            //var r1 = await _device.GetGattServicesForUuidAsync(CommunicationId);
            var r1 = await _device.GetGattServicesAsync();
            if (r1.Status != GattCommunicationStatus.Success)
            {
                throw new NotImplementedException();
            }
            _communicationService = r1.Services.FirstOrDefault(i => i.Uuid == CommunicationId);
            //var r2 = await _communicationService.GetCharacteristicsForUuidAsync(NotifyId);
            var r2 = await _communicationService.GetCharacteristicsAsync();
            if (r2.Status != GattCommunicationStatus.Success)
            {
                throw new NotImplementedException();
            }
            _notifyCharacteristic = r2.Characteristics.FirstOrDefault(i => i.Uuid == NotifyId);
            _notifyCharacteristic.ValueChanged += OnValueChanged;
            //var r3 = await _communicationService.GetCharacteristicsForUuidAsync(WriteId);
            //if (r2.Status != GattCommunicationStatus.Success)
            //{
            //    throw new NotImplementedException();
            //}
            _writeCharacteristic = r2.Characteristics.FirstOrDefault(i => i.Uuid == WriteId);
            RaisePropertyChanged(nameof(CanWrite));
            // 开启通知
            var status = await _notifyCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
            if (status != GattCommunicationStatus.Success)
            {
                throw new NotImplementedException();
            }
        }

        byte[] _cache;

        void OnValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            CryptographicBuffer.CopyToByteArray(args.CharacteristicValue, out var value);
            if (_cache == null)
            {
                _cache = value;
            }
            else
            {
                var values = new List<byte>(_cache);
                values.AddRange(value);
                _cache = values.ToArray();
            }
            if (_cache.Length < 2)
                return;
            var code1 = _cache[_cache.Length - 2];
            var code2 = _cache[_cache.Length - 1];
            if (code1 != 0x0D || code2 != 0x0A)
                return;
            var str = Encoding.ASCII.GetString(_cache).TrimEnd();
            _cache = null;

            DealWithStr(str);
        }

        private async void DealWithStr(string str)
        {
            var log = new LogModel("RECEIVE", str);
            DispatcherRun(() => Logs.Add(log));

            // 握手
            if (str == "CODE?")
            {
                await WriteAsync("@GrzBLE");
            }
        }

        DelegateCommand _disconnectCommand;
        public DelegateCommand DisconnectCommand
            => _disconnectCommand ?? (_disconnectCommand = new DelegateCommand(ExecuteDisconnectCommand, CanExecuteDisconnectCommand))
            .ObservesProperty(() => Connected);

        bool CanExecuteDisconnectCommand()
            => Connected;

        void ExecuteDisconnectCommand()
        {
            _notifyCharacteristic = null;
            _writeCharacteristic = null;
            _communicationService.Dispose();
            _communicationService = null;
            _device.Dispose();
            _device = null;
            Connected = false;
        }

        DelegateCommand<string> _writeCommand;
        public DelegateCommand<string> WriteCommand
            => _writeCommand ?? (_writeCommand = new DelegateCommand<string>(ExecuteWriteCommand, CanExecuteWriteCommand))
            .ObservesProperty(() => CanWrite);

        bool CanExecuteWriteCommand(string str)
            => CanWrite && str != null;

        async void ExecuteWriteCommand(string str)
            => await WriteAsync(str);

        async Task<bool> WriteAsync(string str)
        {
            var log = new LogModel("SEND", str);
            DispatcherRun(() => Logs.Add(log));

            var data = Encoding.ASCII.GetBytes($"{str}\r\n");

            var option = CanWriteWithoutResponse ? GattWriteOption.WriteWithoutResponse : GattWriteOption.WriteWithResponse;
            // 大于 20 字节分包发送（最大可以支持 512 字节）
            // https://stackoverflow.com/questions/53313117/cannot-write-large-byte-array-to-a-ble-device-using-uwp-apis-e-g-write-value
            var count = data.Length / MTU;
            var remainder = data.Length % MTU;
            var carriage = new byte[MTU];
            for (int i = 0; i < count; i++)
            {
                Array.Copy(data, i * MTU, carriage, 0, MTU);
                var value = CryptographicBuffer.CreateFromByteArray(carriage);
                var status = await _writeCharacteristic.WriteValueAsync(value, option);
                //var result = await mCharacteristic.WriteValueWithResultAsync(value, option);
                //var status = result.Status;
                if (status != GattCommunicationStatus.Success)
                    return false;
            }
            if (remainder > 0)
            {
                carriage = new byte[remainder];
                Array.Copy(data, count * MTU, carriage, 0, remainder);
                var value = CryptographicBuffer.CreateFromByteArray(carriage);
                var status = await _writeCharacteristic.WriteValueAsync(value, option);
                var isWritten = status == GattCommunicationStatus.Success;
                return isWritten;
            }

            return true;
        }
    }
}
