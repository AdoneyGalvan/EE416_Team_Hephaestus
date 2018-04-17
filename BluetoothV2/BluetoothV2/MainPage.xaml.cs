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
    public enum ADXL345Commands : byte
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
        DUMMY = 13,
    };

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //Sensor 
        const int ACCEL_RES = 1024;/* The ADXL345 has 10 bit resolution giving 1024 unique values*///Dont Change!!!!
        int ACCEL_DYN_RANGE_G; //Example: -+4G has a dynamic range of 8
        int sample_rate;
        const int number_of_samples = 1024;
        const int number_of_bytes = 6144;
        const int command_buffer_size = 6;

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
            webapiaccess.Initialize("http://myacceleappwebserver-env.us-west-2.elasticbeanstalk.com/");
            //webapiaccess.Initialize("http://localhost:58768/");
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
            btnDisConnect.IsEnabled = false;

            //Group Count
            int groupnumber;

            //RMS Datapoints
            List<DataPointRMSModel> rmsdates = new List<DataPointRMSModel>();

            Update_UI("Retriving current data set group number for: " + DateTime.Today.ToString());

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

            List<Acceleration> rawdata = new List<Acceleration>();

            double[] x_time_samples = new double[number_of_samples];
            double[] x_fft_samples = new double[number_of_samples];
            double[] y_time_samples = new double[number_of_samples];
            double[] y_fft_samples = new double[number_of_samples];
            double[] z_time_samples = new double[number_of_samples];
            double[] z_fft_samples = new double[number_of_samples];
            double[] freq = new double[number_of_samples];

            signalprocessingaccess.GenerateIndeces(ref freq, number_of_samples);//Generate Ftrequency Range
            signalprocessingaccess.IndexScaling(ref freq, number_of_samples, sample_rate);

            Update_UI("Reading from ADXL345...");

            rawdata = await ReadData();//Read the sensor 

            for (int i = 0; i < number_of_samples; i++)
            {
                x_time_samples[i] = rawdata[i].X;//Store the raw values of the X Axis, this buffer will passed to the FFT algorthim
                y_time_samples[i] = rawdata[i].Y;//Store the raw values of the Y Axis, this buffer will passed to the FFT algorthim
                z_time_samples[i] = rawdata[i].Z;//Store the raw values of the Z Axis, this buffer will passed to the FFT algorthim

                //Add a new datapoint to the raw time series list
                rawdatapointlist.Add(new DataPointTimeModel()
                {
                    DataPointDateTime = DateTime.Now,
                    DataPointGroupID = groupnumber,
                    DataPointX = rawdata[i].X,
                    DataPointY = rawdata[i].Y,
                    DataPointZ = rawdata[i].Z,
                    DataPointUniqueID = 0
                });

                //Pre allocate the space for the fft series list
                fftdatapointlist.Add(new DataPointFFTModel()
                {
                    DataPointDateTime = rawdatapointlist[i].DataPointDateTime,
                    DataPointGroupID = groupnumber,
                    DataPointX = rawdata[i].X,
                    DataPointY = rawdata[i].Y,
                    DataPointZ = rawdata[i].Z,
                    DataPointFreq = freq[i],
                });
            }

            Update_UI("Performing Root Mean Squared Analysis on dataset...");

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

            Update_UI("Performing Fast Fourier Analysis on dataset...");

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

            Update_UI("Uploading dataset to cloud database...");

            webapiaccess.SendFFTData(fftdatapointlist);

            webapiaccess.SendTimeData(rawdatapointlist);

            webapiaccess.SendRMSData(rmsdatapointlist);

            Update_UI("Read completed...");

            btnRead.IsEnabled = true;
            btnDisConnect.IsEnabled = true;

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

        public ADXL345Commands data_rate(int index)
        {
            switch (index)
            {
                case 0:
                    return ADXL345Commands.SETDATARATE100HZ;
                case 1:
                    return ADXL345Commands.SETDATARATE200HZ;
                case 2:
                    return ADXL345Commands.SETDATARATE400HZ;
                case 3:
                    return ADXL345Commands.SETDATARATE800HZ;
                case 4:
                    return ADXL345Commands.SETDATARATE1600HZ;
                default:
                    return ADXL345Commands.SETDATARATE1600HZ;
            }
        }

        public ADXL345Commands sensitivity(int index)
        {
            switch(index)
            {
                case 0:
                    return ADXL345Commands.SETRANGE2G;
                case 1:
                    return ADXL345Commands.SETRANGE4G;
                case 2:
                    return ADXL345Commands.SETRANGE8G;
                case 3:
                    return ADXL345Commands.SETRANGE16G;
                default:
                    return ADXL345Commands.SETRANGE2G;
            }
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            Update_UI("Attemping to Connect to ADXL345....");
            byte[] buffer = new byte[command_buffer_size];//Standard response for a command is 6 bytes

            tbError.Text = string.Empty;

            try
            {
                var devices = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
                var device = devices.Single(x => x.Name == "RNI-SPP");
                service = await RfcommDeviceService.FromIdAsync(device.Id);
                socket = new StreamSocket();
                await socket.ConnectAsync(service.ConnectionHostName, service.ConnectionServiceName, SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

                buffer = await ExecuteCommand(ADXL345Commands.BEGIN);

                buffer = await ExecuteCommand(data_rate(comboBox1.SelectedIndex));
                
                buffer = await ExecuteCommand(sensitivity(comboBox2.SelectedIndex));
   

                btnConnect.IsEnabled = false;
                btnDisConnect.IsEnabled = true;
                btnRead.IsEnabled = true;
                Update_UI("Connected to ADXL345....");
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
                Update_UI("Failed to Connect to ADXL345....");
            }
        }
        
        public async Task<byte[]> ExecuteCommand(ADXL345Commands command)
        {
            byte[] command_buffer = new byte[command_buffer_size];//Standard response for a command is 6 bytes
            byte[] data_buffer = new byte[number_of_bytes];//1024 samples 1024*6 bytes = 6144

            switch (command)
            {
                case ADXL345Commands.BEGIN:
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    return command_buffer;
                case ADXL345Commands.SETRANGE2G:
                    ACCEL_DYN_RANGE_G = 4;
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    return command_buffer;
                case ADXL345Commands.SETRANGE4G:
                    ACCEL_DYN_RANGE_G = 8;
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    return command_buffer;
                case ADXL345Commands.SETRANGE8G:
                    ACCEL_DYN_RANGE_G = 16;
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    return command_buffer;
                case ADXL345Commands.SETRANGE16G:
                    ACCEL_DYN_RANGE_G = 32;
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    return command_buffer;
                case ADXL345Commands.SETDATARATE100HZ:
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    sample_rate = 100;
                    return command_buffer;
                case ADXL345Commands.SETDATARATE200HZ:
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    sample_rate = 200;
                    return command_buffer;
                case ADXL345Commands.SETDATARATE400HZ:
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    sample_rate = 400;
                    return command_buffer;
                case ADXL345Commands.SETDATARATE800HZ:
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    sample_rate = 800;
                    return command_buffer;
                case ADXL345Commands.SETDATARATE1600HZ:
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    sample_rate = 1600;
                    return command_buffer;
                case ADXL345Commands.SETDATARATE3200HZ:
                    command_buffer = await SendCommand(command, command_buffer, command_buffer_size);
                    sample_rate = 3200;
                    return command_buffer;
                case ADXL345Commands.READDATA:
                    data_buffer = await SendCommand(command, data_buffer, number_of_bytes);
                    return data_buffer;
                case ADXL345Commands.DUMMY:
                    data_buffer = await SendCommand(command, data_buffer, number_of_bytes);
                    return data_buffer;
                default:
                    return data_buffer;
            }
        }

        public async Task<byte[]> SendCommand(ADXL345Commands command, byte[] buffer, int buffer_size)
        {
            //Initialize the buffer
            byte[] commands = { (byte)command };
            uint temp;

            try
            {
                using (var writer = new DataWriter(socket.OutputStream))
                {
                    
                    writer.WriteBytes(commands);
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
                    var data = await reader.LoadAsync((uint)buffer_size);
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

        public async Task<List<Acceleration>> ReadData()
        {
            int UNITS_PER_G = ACCEL_RES / ACCEL_DYN_RANGE_G;  /* Ratio of raw int values to G units                          */
            byte[] buffer = new byte[number_of_bytes];
            int index;
            List<Acceleration> rawdata = new List<Acceleration>();
            Acceleration temprawdata = new Acceleration();

            buffer = await ExecuteCommand(ADXL345Commands.READDATA);

            for(int i = 0; i < number_of_samples; i++)
            {
                index = i * 6;
                temprawdata.X = (double)((short)((buffer[index] << 8) + buffer[index+1])) / UNITS_PER_G;
                temprawdata.Y = (double)((short)((buffer[index+2] << 8) + buffer[index+3])) / UNITS_PER_G;
                temprawdata.Z = (double)((short)((buffer[index+4] << 8) + buffer[index+5])) / UNITS_PER_G;
                rawdata.Add(temprawdata);
            }
            return rawdata;
        }
        
        public async void Update_UI(string msg)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                tbOutput.Text = msg; 
            });
        }
    }
}
