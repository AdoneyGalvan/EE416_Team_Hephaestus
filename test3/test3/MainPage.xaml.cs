﻿using System;
using LiveCharts;
using LiveCharts.Uwp;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Spi;
using Windows.Devices.Enumeration;
using System.Threading;
using Windows.UI.Xaml.Media;
using LiveCharts.Defaults;

//All Credit given to Mircosoft  
//The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace test3
{

    struct Acceleration
    {
        public double X;
        public double Y;
        public double Z;
    };

    public sealed partial class MainPage : Page
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

        private double count = 0;
        private SpiDevice SPIAccel;
        private Timer periodicTimer;

        public ChartValues<ObservablePoint> xValues { get; set; }
        public ChartValues<ObservablePoint> yValues { get; set; }
        public ChartValues<ObservablePoint> zValues { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            /* Register for the unloaded event so we can clean up upon exit */
            Unloaded += MainPage_Unloaded;
            InitAccel();
            xValues = new ChartValues<ObservablePoint> { };
            yValues = new ChartValues<ObservablePoint> { };
            zValues = new ChartValues<ObservablePoint> { };


            //SeriesCollection = new SeriesCollection{ new LineSeries { Values = new ChartValues<double> { } } };
            DataContext = this;
        }
          
        public void InitAccel()
        {
            InitSPIAccel();
        }

        private async void InitSPIAccel()
        {
            try
            {
                var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE);
                settings.ClockFrequency = 5000000;
                settings.Mode = SpiMode.Mode3;

                string aqs = SpiDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs);
                SPIAccel = await SpiDevice.FromIdAsync(dis[0].Id, settings);
                if(SPIAccel == null)
                {
                    Text_Status.Text = string.Format("SPI Controller {0} is currently in use by " +
                        "another application. Please ensure that no other applications are using SPI.",
                        dis[0].Id);
                    return;
                }
            }
            catch (Exception ex)
            {
                Text_Status.Text = "SPI Initialization failed. Exception: " + ex.Message;
            }

            /* 
            * Initialize the accelerometer:
            *
            * For this device, we create 2-byte write buffers:
            * The first byte is the register address we want to write to.
            * The second byte is the contents that we want to write to the register. 
            */
            byte[] WriteBuf_DataFormat = new byte[] { ACCEL_REG_DATA_FORMAT, 0x01 };        /* 0x01 sets range to +- 4Gs                         */
            byte[] WriteBuf_PowerControl = new byte[] { ACCEL_REG_POWER_CONTROL, 0x08 };    /* 0x08 puts the accelerometer into measurement mode */

            /* Write the register settings */
            try
            {
                SPIAccel.Write(WriteBuf_DataFormat);
                SPIAccel.Write(WriteBuf_PowerControl);
            }
            /* If the write fails display the error and stop running */
            catch (Exception ex)
            {
                Text_Status.Text = "Failed to communicate with device: " + ex.Message;
                return;
            }
            /* Now that everything is initialized, create a timer so we read data every 100mS */
            periodicTimer = new Timer(this.TimerCallback, null, 0, 100);
        }

        private void MainPage_Unloaded(object sender, object args)
        {
            SPIAccel.Dispose();
        }

        private void TimerCallback(object state)
        {
            string xText, yText, zText;
            string statusText;
            double xData, yData, zData;


            /* Read and format accelerometer data */
            try
            {
                Acceleration accel = ReadAccel();
                xText = String.Format("X Axis: {0:F3}G", accel.X);
                xData = accel.X;
                yText = String.Format("Y Axis: {0:F3}G", accel.Y);
                yData = accel.Y;
                zText = String.Format("Z Axis: {0:F3}G", accel.Z);
                zData = accel.Z;
                statusText = "Status: Running";
            }
            catch (Exception ex)
            {
                xText = "X Axis: Error";
                xData = 0;
                yText = "Y Axis: Error";
                yData = 0;
                zText = "Z Axis: Error";
                zData = 0;
                statusText = "Failed to read from Accelerometer: " + ex.Message;
            }

            /* UI updates must be invoked on the UI thread */
            var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if(count > 100)
                {
                    xValues.RemoveAt(0);
                    yValues.RemoveAt(0);
                    zValues.RemoveAt(0);
                }
                xValues.Add(new ObservablePoint(count, xData));
                yValues.Add(new ObservablePoint(count, yData));
                zValues.Add(new ObservablePoint(count, zData));

                Text_X_Axis.Text = xText;
                Text_Y_Axis.Text = yText;
                Text_Z_Axis.Text = zText;
                Text_Status.Text = statusText;
            });
        }

        private Acceleration ReadAccel()
        {
            const int ACCEL_RES = 1024;         /* The ADXL345 has 10 bit resolution giving 1024 unique values                     */
            const int ACCEL_DYN_RANGE_G = 8;    /* The ADXL345 had a total dynamic range of 8G, since we're configuring it to +-4G */
            const int UNITS_PER_G = ACCEL_RES / ACCEL_DYN_RANGE_G;  /* Ratio of raw int values to G units                          */

            byte[] ReadBuf;
            byte[] RegAddrBuf;

  
            ReadBuf = new byte[6 + 1];      /* Read buffer of size 6 bytes (2 bytes * 3 axes) + 1 byte padding */
            RegAddrBuf = new byte[1 + 6];   /* Register address buffer of size 1 byte + 6 bytes padding        */
            /* Register address we want to read from with read and multi-byte bit set                          */
            RegAddrBuf[0] = ACCEL_REG_X | ACCEL_SPI_RW_BIT | ACCEL_SPI_MB_BIT;
            SPIAccel.TransferFullDuplex(RegAddrBuf, ReadBuf);
            Array.Copy(ReadBuf, 1, ReadBuf, 0, 6);  /* Discard first dummy byte from read                      */


            /* Check the endianness of the system and flip the bytes if necessary */
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

            count++;
            return accel;
        }
    }
}


