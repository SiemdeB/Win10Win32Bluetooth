using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth;
using System.Diagnostics;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace ConsoleApplication1
{
    class Program
    {
        static async Task Main(string[] args)
        {


            //DeviceInformationCollection deviceInformationCollection = await DeviceInformation.FindAllAsync();

            // Start the program
            var program = new Program();

            // Close on key press
            Console.ReadLine();
        }

        public Program()
        {
            // Create Bluetooth Listener
            var watcher = new BluetoothLEAdvertisementWatcher();
            //watcher.AdvertisementFilter.BytePatterns
            // Query for extra properties you want returned
            //string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            watcher.ScanningMode = BluetoothLEScanningMode.Active;

            watcher.AdvertisementFilter.Advertisement.LocalName = "Bangle.js 23b0";
            //watcher.AdvertisementFilter.Advertisement.ServiceUuids.Add(new Guid("6e400001b5a3f393e0a9e50e24dcca9e"));
            //watcher.AdvertisementFilter.Advertisement.ServiceUuids.Add(new Guid("6e400002b5a3f393e0a9e50e24dcca9e"));
            //watcher.AdvertisementFilter.Advertisement.ServiceUuids.Add(new Guid("6e400003b5a3f393e0a9e50e24dcca9e"));

            // Only activate the watcher when we're recieving values >= -80
            // watcher.SignalStrengthFilter.InRangeThresholdInDBm = -99;

            // Stop watching if the value drops below -90 (user walked away)
            //watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -10;

            // Register callback for when we see an advertisements
            // watcher.Received += OnAdvertisementReceived;

            // Wait 5 seconds to make sure the device is really out of range
            watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);

            // Starting watching for advertisements
            

            watcher.Received += async (w, btAdv) => {
                var device = await BluetoothLEDevice.FromBluetoothAddressAsync(btAdv.BluetoothAddress);
                Debug.WriteLine($"BLEWATCHER Found: {device.DeviceInformation.Name}");

                // SERVICES!!
                var gatt = device.GattServices.FirstOrDefault();
                //Debug.WriteLine($"{device.Name} Services: {gatt.`.Count}, {gatt.Status}, {gatt.ProtocolError}");

                //// CHARACTERISTICS!!
                //var characs = device.GattServices.Single(s => s.Uuid == new Guid("6e400003b5a3f393e0a9e50e24dcca9e")).GetCharacteristics(new Guid("6e400003b5a3f393e0a9e50e24dcca9e"));
                var characs2 = device.GattServices.Single(s => s.Uuid == new Guid("6e400001b5a3f393e0a9e50e24dcca9e"));


                var cah2 = characs2.GetCharacteristics(new Guid("6e400002b5a3f393e0a9e50e24dcca9e"));

                GattCharacteristic vvv = cah2.FirstOrDefault();
                GattCharacteristicProperties properties = vvv.CharacteristicProperties;

                //var charac = characs.Single(c => c.Uuid == SAMPLECHARACUUID);


                var writer = new DataWriter();
                writer.WriteString("LED1.reset();\n");
                var res = await vvv.WriteValueAsync(writer.DetachBuffer(), GattWriteOption.WriteWithoutResponse);
                //writer.WriteString("LED2.set();\n");
                //res = await vvv.WriteValueAsync(writer.DetachBuffer(), GattWriteOption.WriteWithoutResponse);

            };


            watcher.Start();

        }



        //    {
        //          var btUARTService = findByUUID(services, "6e400001b5a3f393e0a9e50e24dcca9e");
        //    txCharacteristic = findByUUID(characteristics, "6e400002b5a3f393e0a9e50e24dcca9e");
        //    rxCharacteristic = findByUUID(characteristics, "6e400003b5a3f393e0a9e50e24dcca9e");
        //  if (error || !btUARTService || !txCharacteristic || !rxCharacteristic) {
        //    console.log("BT> ERROR getting services/characteristics");
        //    console.log("Service "+btUARTService);
        //    console.log("TX "+txCharacteristic);
        //    console.log("RX "+rxCharacteristic);
        //    btDevice.disconnect();
        //    txCharacteristic = undefined;
        //    rxCharacteristic = undefined;
        //    btDevice = undefined;
        //    return openCallback();
        //}

        //    UARTService
        //}

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            // Tell the user we see an advertisement and print some properties
            Console.WriteLine(String.Format("Advertisement:"));
            Console.WriteLine(String.Format("  BT_ADDR: {0}", eventArgs.BluetoothAddress));
            Console.WriteLine(String.Format("  FR_NAME: {0}", eventArgs.Advertisement.LocalName));
            Console.WriteLine();
        }
    }
}
 