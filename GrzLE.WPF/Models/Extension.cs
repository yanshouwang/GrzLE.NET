using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;

namespace GrzLE.WPF.Models
{
    static class Extension
    {
        public static DeviceModel ToModel(this BluetoothLEDevice device, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            return new DeviceModel(device, args);
        }
    }
}
