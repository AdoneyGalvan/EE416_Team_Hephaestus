using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataPointViewModelHardware;

namespace DataWebApi
{
    public class DataWebApiAccess
    {
        private HttpResponseMessage response = new HttpResponseMessage();
        private HttpClient client = new HttpClient();

        public void Initialize(string uri)
        {
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SendFFTData(List<DataPointFFTModel> data)
        {
            foreach (DataPointFFTModel element in data)
            {
                response = client.PostAsJsonAsync("api/FFTData", element).Result;
            }
        }

        public void SendTimeData(List<DataPointTimeModel> data)
        {
            foreach (DataPointTimeModel element in data)
            {
                response = client.PostAsJsonAsync("api/TimeData", element).Result;
            }
        }

        public void SendRMSData(List<DataPointRMSModel> data)
        {
            foreach (DataPointRMSModel element in data)
            {
                response = client.PostAsJsonAsync("api/RMSData", element).Result;
            }
        }

        public List<DataPointRMSModel> GetRMSDataByDates(string startdate, string enddate)
        {
            List<DataPointRMSModel> temp = new List<DataPointRMSModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/RMSData?startdate={0}&enddate={1}", startdate, enddate)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointRMSModel>>().Result;
        }
    }
}
