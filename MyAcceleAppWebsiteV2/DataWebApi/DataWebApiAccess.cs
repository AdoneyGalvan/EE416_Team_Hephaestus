using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataPointViewModel;

namespace DataWebApi
{
    public class DataWebApiAccess
    {
        private HttpClient client = new HttpClient();

        public void Initialize(string uri)
        {
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public List<DataPointModel> GetRawData()
        {
            List<DataPointModel> temp = new List<DataPointModel>();
            HttpResponseMessage response = client.GetAsync("api/RawData").Result;
            return temp = response.Content.ReadAsAsync<List<DataPointModel>>().Result;
        }

        public List<DataPointModel> GetRawDataByDates(string startdate, string enddate)
        {
            List<DataPointModel> temp = new List<DataPointModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/RawData?startdate={0}&enddate={1}", startdate, enddate)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointModel>>().Result;
        }

        public DataPointModel GetRawDataByID(int ID)
        {
            DataPointModel temp = new DataPointModel();
            HttpResponseMessage response = client.GetAsync(String.Format("api/RawData?id={0}", ID)).Result;
            return temp = response.Content.ReadAsAsync<DataPointModel>().Result;
        }

        public List<DataPointModel> GetFFTData()
        {
            List<DataPointModel> temp = new List<DataPointModel>();
            HttpResponseMessage response = client.GetAsync("api/FFTData").Result;
            return temp = response.Content.ReadAsAsync<List<DataPointModel>>().Result;
        }

        public List<DataPointModel> GetFFTDataByDates(string startdate, string enddate)
        {
            List<DataPointModel> temp = new List<DataPointModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/FFTData?startdate={0}&enddate={1}", startdate, enddate)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointModel>>().Result;
        }

        public DataPointModel GetFFTDataByID(int ID)
        {
            DataPointModel temp = new DataPointModel();
            HttpResponseMessage response = client.GetAsync(String.Format("api/FFTData?id={0}", ID)).Result;
            return temp = response.Content.ReadAsAsync<DataPointModel>().Result;
        }
    }
}
