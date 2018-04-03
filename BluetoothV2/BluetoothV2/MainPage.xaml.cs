using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DataWebApi;
using DataPointViewModelHardware;
using SignalProcessing;
using System.Threading;
using System.Net.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BluetoothV2
{
    //Temp view model for acceleration data
    public struct Acceleration { public double X; public double Y; public double Z; };

    //Settings/Commands to the Arduino to configure ADXL345
    public enum ADXL345Settings : byte
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

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //Sensor 
        const int ACCEL_RES = 1024;/* The ADXL345 has 10 bit resolution giving 1024 unique values*///Dont Change!!!!
        int ACCEL_DYN_RANGE_G; //Example: -+4G has a dynamic range of 8
        const int sample_rate = 3200;
        const int buffersize = 6;//Dont Change!!!!!
        byte[] buffer = new byte[buffersize];
        const int number_of_samples = 1024;
        const int delaytime = 100;

        //Acceleration data
        Acceleration tempdata = new Acceleration();

        //WebApi
        DataWebApiAccess webapiaccess = new DataWebApiAccess();

        //Bluetooth Device
        RfcommDeviceService service;

        //Serial Socket
        StreamSocket socket;

        //Signal Processing
        SignalProcessingAccess signalprocessingaccess = new SignalProcessingAccess();

        public MainPage()
        {
            this.InitializeComponent();
            webapiaccess.Initialize("http://myacceleappwebserver.us-west-2.elasticbeanstalk.com/");
            btnDisConnect.IsEnabled = false;
            btnConnect.IsEnabled = true;
            btnRead.IsEnabled = false;
        }
        

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            Read();
        }

        private async void Read()
        {
            btnRead.IsEnabled = false;

            //Group Count
            int groupnumber;

            //RMS Datapoints
            List<DataPointRMSModel> rmsdates = new List<DataPointRMSModel>();
            var today = DateTime.Today;//Todays Date
            var tomorrow = today.AddDays(1);//Increase by one date

            //Used to get the current group number for the speficied data
            rmsdates = webapiaccess.GetRMSDataByDates(today.ToString(),tomorrow.ToString());

            if(!rmsdates.Any())
            {
                groupnumber = 1;//If there is no data for the date start group number at 1
            }
            else
            {
                groupnumber = rmsdates.Last().DataPointGroupID + 1;//Get the last data point and increase the group number
            }

            //FFT Datapoints
            List<DataPointFFTModel> fftdatapointlist = new List<DataPointFFTModel>();

            //Time Series Datapoints
            List<DataPointTimeModel> rawdatapointlist = new List<DataPointTimeModel>();

            //RMS Datapoints
            List<DataPointRMSModel> rmsdatapointlist = new List<DataPointRMSModel>();

            double[] x_time_samples = new double[number_of_samples];
            double[] x_fft_samples = new double[number_of_samples];
            double[] y_time_samples = new double[number_of_samples];
            double[] y_fft_samples = new double[number_of_samples];
            double[] z_time_samples = new double[number_of_samples];
            double[] z_fft_samples = new double[number_of_samples];
            double[] freq = new double[number_of_samples];

            signalprocessingaccess.GenerateIndeces(ref freq, number_of_samples);//Generate Ftrequency Range
            signalprocessingaccess.IndexScaling(ref freq, number_of_samples, sample_rate);


            for (int i = 0; i < number_of_samples; i++)
            {
                tempdata = await ReadData();//Read the sensor 

                x_time_samples[i] = tempdata.X;//Store the raw values of the X Axis, this buffer will passed to the FFT algorthim
                y_time_samples[i] = tempdata.Y;//Store the raw values of the Y Axis, this buffer will passed to the FFT algorthim
                z_time_samples[i] = tempdata.Z;//Store the raw values of the Z Axis, this buffer will passed to the FFT algorthim

                //Add a new datapoint to the raw time series list
                rawdatapointlist.Add(new DataPointTimeModel()
                {
                    DataPointDateTime = DateTime.Now,
                    DataPointGroupID = groupnumber,
                    DataPointX = tempdata.X,
                    DataPointY = tempdata.Y,
                    DataPointZ = tempdata.Z,
                    DataPointUniqueID = 0
                });

                //Pre allocate the space for the fft series list
                fftdatapointlist.Add(new DataPointFFTModel()
                {
                    DataPointDateTime = rawdatapointlist[i].DataPointDateTime,
                    DataPointGroupID = groupnumber,
                    DataPointX = tempdata.X,
                    DataPointY = tempdata.Y,
                    DataPointZ = tempdata.Z,
                    DataPointFreq = freq[i],
                });

                Thread.Sleep(100);
            }

            //Perform RMS
            rmsdatapointlist.Add(new DataPointRMSModel()
            {
                DataPointDateTime = rawdatapointlist[0].DataPointDateTime,
                DataPointGroupID = groupnumber,
                DataPointXRMS = signalprocessingaccess.RMS(x_time_samples, number_of_samples),
                DataPointYRMS = signalprocessingaccess.RMS(y_time_samples, number_of_samples),
                DataPointZRMS = signalprocessingaccess.RMS(z_time_samples, number_of_samples),
                DataPointUniqueID = 0,
            });

            
            //Perform FFT
            x_fft_samples = signalprocessingaccess.ComplexFFT(x_time_samples, number_of_samples);//X Axis
            signalprocessingaccess.FFT_shift(ref x_fft_samples, number_of_samples);
            y_fft_samples = signalprocessingaccess.ComplexFFT(y_time_samples, number_of_samples);//Y Axis
            signalprocessingaccess.FFT_shift(ref y_fft_samples, number_of_samples);
            z_fft_samples = signalprocessingaccess.ComplexFFT(z_time_samples, number_of_samples);//Z Axis
            signalprocessingaccess.FFT_shift(ref z_fft_samples, number_of_samples);


            for (int i = 0; i < number_of_samples; i++)
            {
                fftdatapointlist[i].DataPointX = x_fft_samples[i];//Update to the correct fft value calculated for X Axis
                fftdatapointlist[i].DataPointY = y_fft_samples[i];//Update to the correct fft value calculated for Y Axis
                fftdatapointlist[i].DataPointZ = z_fft_samples[i];//Update to the correct fft value calculated for Z Axis
            }

            //Send the data to the database
            webapiaccess.SendFFTData(fftdatapointlist);
            webapiaccess.SendTimeData(rawdatapointlist);
            webapiaccess.SendRMSData(rmsdatapointlist);

            //Increase the global count 
            groupnumber++;

            btnRead.IsEnabled = true;

        }

        private async void btnDisConnect_Click(object sender, RoutedEventArgs e)
        {
            tbError.Text = string.Empty;

            try
            {
                await socket.CancelIOAsync();
                socket.Dispose();
                socket = null;
                service.Dispose();
                service = null;

                btnConnect.IsEnabled = true;
                btnDisConnect.IsEnabled = false;
                btnRead.IsEnabled = false;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
            }
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            tbError.Text = string.Empty;

            try
            {
                var devices = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
                var device = devices.Single(x => x.Name == "RNI-SPP");
                service = await RfcommDeviceService.FromIdAsync(device.Id);
                socket = new StreamSocket();
                await socket.ConnectAsync(service.ConnectionHostName, service.ConnectionServiceName, SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);
                ADXL345_Settings(ADXL345Settings.SETRANGE4G, ADXL345Settings.SETDATARATE3200HZ);

                btnConnect.IsEnabled = false;
                btnDisConnect.IsEnabled = true;
                btnRead.IsEnabled = true;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
            }

        }

        public async void ADXL345_Settings(ADXL345Settings range, ADXL345Settings samplefreq)
        {
            switch (range)
            {
                case ADXL345Settings.SETRANGE2G:
                    ACCEL_DYN_RANGE_G = 4;
                    break;
                case ADXL345Settings.SETRANGE4G:
                    ACCEL_DYN_RANGE_G = 8;
                    break;
                case ADXL345Settings.SETRANGE8G:
                    ACCEL_DYN_RANGE_G = 16;
                    break;
                case ADXL345Settings.SETRANGE16G:
                    ACCEL_DYN_RANGE_G = 32;
                    break;
            }

            buffer = await SendCommand(ADXL345Settings.BEGIN);
            buffer = await SendCommand(range);
            buffer = await SendCommand(samplefreq);
        }

        public async Task<byte[]> SendCommand(ADXL345Settings comtemp)
        {
            //Initialize the buffer
            byte[] command = { (byte)comtemp };
            uint temp;

            try
            {
                using (var writer = new DataWriter(socket.OutputStream))
                {
                    
                    writer.WriteBytes(command);
                    temp = await writer.StoreAsync().AsTask();
                    writer.DetachStream();
                }
            }
            catch(Exception ex)
            {
                tbError.Text = ex.Message;
            }

            try
            {
                using (var reader = new DataReader(socket.InputStream))
                {
                    var data = await reader.LoadAsync(buffersize);
                    reader.ReadBytes(buffer);
                    reader.DetachStream();
                }
                return buffer;
            }
            catch(Exception ex)
            {
                tbError.Text = ex.Message;
                return buffer;
            }
        }

        public async Task<Acceleration> ReadData()
        {
            int UNITS_PER_G = ACCEL_RES / ACCEL_DYN_RANGE_G;  /* Ratio of raw int values to G units                          */
            Acceleration rawdata = new Acceleration();

            buffer = await SendCommand(ADXL345Settings.READDATA);
            /* In order to get the raw 16-bit data values, we need to concatenate two 8-bit bytes for each axis */
            rawdata.X = (double)((short)((buffer[0] << 8) + buffer[1])) / UNITS_PER_G;
            rawdata.Y = (double)((short)((buffer[2] << 8) + buffer[3])) / UNITS_PER_G;
            rawdata.Z = (double)((short)((buffer[4] << 8) + buffer[5])) / UNITS_PER_G;

            return rawdata;
        }
        //private void Test()
        //{
        //    const int sample_rate = 1024;
        //    const int number_of_samples = 1024;
        //    const int amplitude = 1;
        //    const int frequency_in_hz = 60;
        //    double time_in_seconds = 0;

        //    double[] sample_buffer_time = new double[number_of_samples];
        //    double[] sample_buffer_fft = new double[number_of_samples];

        //    for (int sample_number = 0; sample_number < number_of_samples; sample_number++)
        //    {
        //        time_in_seconds = (double)sample_number / sample_rate;
        //        sample_buffer_time[sample_number] = amplitude * Math.Sin(2 * Math.PI * frequency_in_hz * time_in_seconds);
        //    }

        //    //Perform FFT
        //    sample_buffer_fft = processingaccess.ComplexFFT(sample_buffer_time, number_of_samples);


        //    for (int sample_number = 0; sample_number < number_of_samples; sample_number++)
        //    {
        //        fftdatapointlist.Add(new DataPointModel()
        //        {
        //            DataPointDateTime = DateTime.Now,
        //            DataPointGroupID = groupnumber,
        //            DataPointX = sample_buffer_fft[sample_number],
        //            DataPointY = sample_buffer_fft[sample_number],
        //            DataPointZ = sample_buffer_fft[sample_number],
        //            DataPointUniqueID = 0
        //        });

        //        rawdatapointlist.Add(new DataPointModel()
        //        {
        //            DataPointDateTime = fftdatapointlist[sample_number].DataPointDateTime,
        //            DataPointGroupID = groupnumber,
        //            DataPointX = sample_buffer_time[sample_number],
        //            DataPointY = sample_buffer_time[sample_number],
        //            DataPointZ = sample_buffer_time[sample_number],
        //            DataPointUniqueID = 0
        //        });
        //    }

        //    //Send the data to the database
        //    webapiaccess.SendFFTData(fftdatapointlist);
        //    webapiaccess.SendRawData(rawdatapointlist);

        //    //Increase the global count 
        //    groupnumber++;

        //}
        //private async void btnRead_Click(object sender, RoutedEventArgs e)
        //{
        //    List<DataPointModel> datapoints = new List<DataPointModel>();
        //    DataPointModel datapoint = new DataPointModel();

        //    tempdata = await mysensor.ReadData();
        //    datapoint.DataPointDateTime = DateTime.Now;
        //    datapoint.DataPointX = tempdata.X;
        //    datapoint.DataPointY = tempdata.Y;
        //    datapoint.DataPointZ = tempdata.Z;
        //    datapoint.DataPointGroupID = 1;

        //    datapoints.Add(datapoint);
        //    var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        //    {
        //        tbOutput.Text = " X: " + tempdata.X + " Y: " + tempdata.Y + " Z: " + tempdata.Z;
        //    });
        //    webapiaccess.SendRawData(datapoints);
        //    webapiaccess.SendFFTData(datapoints);

        //}
        //private async void btnRead_Click(object sender, RoutedEventArgs e)
        //{
        //    List<DataPointModel> datapoints = new List<DataPointModel>();
        //    DataPointModel datapoint = new DataPointModel();

        //    tempdata = await mysensor.ReadData();
        //    datapoint.DataPointDateTime = DateTime.Now;
        //    datapoint.DataPointX = tempdata.X;
        //    datapoint.DataPointY = tempdata.Y;
        //    datapoint.DataPointZ = tempdata.Z;
        //    datapoint.DataPointGroupID = 1;

        //    datapoints.Add(datapoint);
        //    var task = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
        //    {
        //        tbOutput.Text = " X: " + tempdata.X + " Y: " + tempdata.Y + " Z: " + tempdata.Z;
        //    });
        //    webapiaccess.SendRawData(datapoints);
        //    webapiaccess.SendFFTData(datapoints);

        //}
    }
}
