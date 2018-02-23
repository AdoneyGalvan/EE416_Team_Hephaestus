using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyAcceleAppClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ADXL345 mysensor = new ADXL345();
        private HttpResponseMessage response = new HttpResponseMessage();
        private Acceleration rawdata = new Acceleration();
        private DataPointModel dataPoint = new DataPointModel();
        private Timer periodicTimer;

        public MainPage()
        {
            this.InitializeComponent();
            mysensor.InitializeIC2();
            InitializeTimer();
        }

        public async Task Post()
        {
            rawdata = mysensor.ReadAccele();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://myacceleappwebserver.us-west-2.elasticbeanstalk.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                dataPoint.DataPointDateTime = DateTime.Now;
                dataPoint.DataPointX = rawdata.X;
                dataPoint.DataPointY = rawdata.Y;
                dataPoint.DataPointZ = rawdata.Z;

                response = await client.PostAsJsonAsync("api/data", dataPoint);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Post();
        }

        private void InitializeTimer()
        {
            /* Now that everything is initialized, create a timer so we read data every 100mS */
            periodicTimer = new Timer(this.TimerCallback, null, 5000, 60000);
        }

        private void TimerCallback(object state)
        {
            Post();
        }
    }
}
