using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.I2c;

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
        private const byte ACCEL_REG_POWER_CONTROL = 0x2D;  /* Address of the Power Control register                */
        private const byte ACCEL_REG_DATA_FORMAT = 0x31;    /* Address of the Data Format register                  */
        private const byte ACCEL_REG_X = 0x32;              /* Address of the X Axis data register                  */
        private const byte ACCEL_REG_Y = 0x34;              /* Address of the Y Axis data register                  */
        private const byte ACCEL_REG_Z = 0x36;              /* Address of the Z Axis data register                  */

        private const byte ACCEL_I2C_ADDR = 0x53;           /* 7-bit I2C address of the ADXL345 with SDO pulled low */

        private const byte SPI_CHIP_SELECT_LINE = 0;        /* Chip select line to use                              */
        private const byte ACCEL_SPI_RW_BIT = 0x80;         /* Bit used in SPI transactions to indicate read/write  */
        private const byte ACCEL_SPI_MB_BIT = 0x40;         /* Bit used to indicate multi-byte SPI transactions     */

        private I2cDevice IC2Accele;
        public string comfailed;

        public async void InitializeIC2()
        {
            //Create an instance of I2cConnectionSettings, allowing to configure the I2c settings
            I2cConnectionSettings settings = new I2cConnectionSettings(ACCEL_I2C_ADDR);
            settings.BusSpeed = I2cBusSpeed.FastMode;

            //Create and intance of I2cController with the I2cConnectingSettings
            I2cController controller = await I2cController.GetDefaultAsync();
            IC2Accele = controller.GetDevice(settings);

            byte[] WriteBuf_DataFormat = new byte[] { ACCEL_REG_DATA_FORMAT, 0x01 };        /* 0x01 sets range to +- 4Gs*/
            byte[] WriteBuf_PowerControl = new byte[] { ACCEL_REG_POWER_CONTROL, 0x08 };    /* 0x08 puts the accelerometer into measuremen*/

            //Format the ADXL345 
            try
            {
                IC2Accele.Write(WriteBuf_DataFormat);
                IC2Accele.Write(WriteBuf_PowerControl);
            }
            catch (Exception ex)
            {
                comfailed = "Failed to communicate with device" + ex.Message;
                return;
            }
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
