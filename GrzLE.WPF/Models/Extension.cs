using System;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace GrzLE.WPF.Models
{
    static class Extension
    {
        public static DeviceModel ToModel(this BluetoothLEDevice device, BluetoothLEAdvertisementReceivedEventArgs args)
            => new DeviceModel(device, args);

        public static byte GetHigher(this byte value)
            => (byte)((value & 0xF0) >> 4);

        public static byte GetLower(this byte value)
            => (byte)(value & 0x0F);
    }
}
