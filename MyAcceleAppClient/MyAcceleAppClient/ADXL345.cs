using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;

namespace MyAcceleAppClient
{
    public struct Acceleration
    {
        public double X;
        public double Y;
        public double Z;
    };

    public class ADXL345
    {
        private StreamSocket _socket; //Bluetooth connection socket
        private RfcommDeviceService _service;//Device information collection
        private enum commands
        {
            BEGIN = 1,
            SETRANGE2G = 2,
            SETRANGE4G = 3,
            SETRANGE8G = 4,
            SETRANGE16G = 5,
            SETDATARATE100HZ = 6,
            SETDATARATE200HZ = 7,
            SETDATARATE400HZ = 8,
            SETDATARATE800HZ = 9,
            SETDATARATE1600HZ = 10,
            SETDATARATE3200HZ = 11,
            READDATA = 12,
        };

        public async void InitializeAccele()
        {
            byte[] test = new byte[6];

            tbError.Text = string.Empty;

            byte[] command = { BEGIN };
            uint temp = await Write(command);

            test = await Read(6);

            var taskbegin = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                tbOutput.Text = Encoding.ASCII.GetString(test);
            });

            command[0] = 3;

            temp = await Write(command);
            test = await Read(6);

            var task2G = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                tbOutput.Text = Encoding.ASCII.GetString(test);
            });
        }

        public Acceleration ReadAccele()
        {
            const int ACCEL_RES = 1024;         /* The ADXL345 has 10 bit resolution giving 1024 unique values                     */
            const int ACCEL_DYN_RANGE_G = 8;    /* The ADXL345 had a total dynamic range of 8G, since we're configuring it to +-4G */
            const int UNITS_PER_G = ACCEL_RES / ACCEL_DYN_RANGE_G;  /* Ratio of raw int values to G units                          */

            byte[] ReadBuf = new byte[6];
            byte[] RegAddrBuf = new byte[] { ACCEL_REG_X };
            IC2Accele.WriteRead(RegAddrBuf, ReadBuf);

            //Check the Endianess 
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(ReadBuf, 0, 2);
                Array.Reverse(ReadBuf, 2, 2);
                Array.Reverse(ReadBuf, 4, 2);
            }

            /* In order to get the raw 16-bit data values, we need to concatenate two 8-bit bytes for each axis */
            short AccelerationRawX = BitConverter.ToInt16(ReadBuf, 0);
            short AccelerationRawY = BitConverter.ToInt16(ReadBuf, 2);
            short AccelerationRawZ = BitConverter.ToInt16(ReadBuf, 4);

            /* Convert raw values to G's */
            Acceleration accel;
            accel.X = (double)AccelerationRawX / UNITS_PER_G;
            accel.Y = (double)AccelerationRawY / UNITS_PER_G;
            accel.Z = (double)AccelerationRawZ / UNITS_PER_G;
            return accel;
        }

    }
}
