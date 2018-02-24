using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Timers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using SignalAnalytics;
using Windows.UI.Xaml.Media;
using Windows.UI;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyAcceleAppClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //FTT Classes/Variables and Settings
        private const int samplenum = 4096;
        private SigFunctions sig = new SigFunctions();
        private double[] x_axis = new double[samplenum];
        private double[] y_axis = new double[samplenum];
        private double[] z_axis = new double[samplenum];

        //Sensor Classes/Variables and Settings
        private ADXL345 mysensor = new ADXL345();
        private Acceleration rawdata = new Acceleration();

        //Http Classes/Variables and Settings
        private HttpResponseMessage response = new HttpResponseMessage();
        private HttpClient client = new HttpClient();
        private DataPointModel[] rawdatapointlist = new DataPointModel[samplenum];
        private DataPointModel[] fftdatapointlist = new DataPointModel[samplenum];

        //
        private Timer period = new Timer();
        private int delaytime = 100;
        private int globalcount = 0;

        public MainPage()
        {
            this.InitializeComponent();
            mysensor.InitializeIC2();
            InitializeHttp();
            InitializeTimer();
        }

        private void InitializeHttp()
        {
            client.BaseAddress = new Uri("http://myacceleappwebserver.us-west-2.elasticbeanstalk.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void InitializeTimer()
        {
            period.Interval = delaytime;
            period.AutoReset = true;
            period.Elapsed += GetData;
        }
 
        private void GetData(Object source, ElapsedEventArgs e)
        {
            rawdata = mysensor.ReadAccele();

            //Use to perform the signal analysis
            x_axis[globalcount] = rawdata.X;
            y_axis[globalcount] = rawdata.Y;
            z_axis[globalcount] = rawdata.Z;


            rawdatapointlist[globalcount] = new DataPointModel {
                DataPointDateTime = DateTime.Now,
                DataPointX = rawdata.X,
                DataPointY = rawdata.Y,
                DataPointZ = rawdata.Z,
            };

            //Adding the tempdatapoint to FFT list to ensure that the fft and raw have the same timestamp.
            //The actual x,y, and z values will be overwritten
            fftdatapointlist[globalcount] = new DataPointModel
            {
                DataPointDateTime = DateTime.Now,
                DataPointX = rawdata.X,
                DataPointY = rawdata.Y,
                DataPointZ = rawdata.Z,
            };

            
            if (globalcount >= samplenum - 1)
            {
                globalcount = 0;
                period.Enabled = false;

                x_axis = sig.ComplexFFT(x_axis, samplenum);
                y_axis = sig.ComplexFFT(x_axis, samplenum);
                z_axis = sig.ComplexFFT(x_axis, samplenum);

                for (int i = 0; i < samplenum; i++)
                {
                    fftdatapointlist[i].DataPointX = x_axis[i];
                    fftdatapointlist[i].DataPointY = y_axis[i];
                    fftdatapointlist[i].DataPointZ = z_axis[i];

                    response = client.PostAsJsonAsync("api/FFTData", fftdatapointlist[i]).Result;
                }

                for (int i = 0; i < samplenum; i++)
                {
                    response = client.PostAsJsonAsync("api/RawData", rawdatapointlist[i]).Result;
                }

                
            }
            globalcount++;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            period.Enabled = true;
        }

    }
}
