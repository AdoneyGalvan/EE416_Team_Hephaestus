﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataPointViewModelServerWebsite;

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

        public List<DataPointTimeModel> GetTimeData()
        {
            List<DataPointTimeModel> temp = new List<DataPointTimeModel>();
            HttpResponseMessage response = client.GetAsync("api/TimeData").Result;
            return temp = response.Content.ReadAsAsync<List<DataPointTimeModel>>().Result;
        }

        public List<DataPointTimeModel> GetTimeDataByDates(string startdate, string enddate)
        {
            List<DataPointTimeModel> temp = new List<DataPointTimeModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/TimeData?startdate={0}&enddate={1}", startdate, enddate)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointTimeModel>>().Result;
        }

        public List<DataPointTimeModel> GetTimeDataByGrouptIDAndDates(int ID, string startdate, string enddate)
        {
            List<DataPointTimeModel> temp = new List<DataPointTimeModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/TimeData?ID={0}&startdate={1}&enddate={2}",ID, startdate, enddate)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointTimeModel>>().Result;
        }

        public List<DataPointTimeModel> GetTimeDataByGroupID(int ID)
        {
            List<DataPointTimeModel> temp = new List<DataPointTimeModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/TimeData?id={0}", ID)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointTimeModel>>().Result;
        }

        public List<DataPointFFTModel> GetFFTData()
        {
            List<DataPointFFTModel> temp = new List<DataPointFFTModel>();
            HttpResponseMessage response = client.GetAsync("api/FFTData").Result;
            return temp = response.Content.ReadAsAsync<List<DataPointFFTModel>>().Result;
        }

        public List<DataPointFFTModel> GetFFTDataByDates(string startdate, string enddate)
        {
            List<DataPointFFTModel> temp = new List<DataPointFFTModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/FFTData?startdate={0}&enddate={1}", startdate, enddate)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointFFTModel>>().Result;
        }

        public List<DataPointFFTModel> GetFFTDataByGroupIDAndDates(int ID, string startdate, string enddate)
        {
            List<DataPointFFTModel> temp = new List<DataPointFFTModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/FFTData?ID={0}&startdate={1}&enddate={2}",ID, startdate, enddate)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointFFTModel>>().Result;
        }

        public List<DataPointFFTModel> GetFFTDataByGroupID(int ID)
        {
            List<DataPointFFTModel> temp = new List<DataPointFFTModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/FFTData?id={0}", ID)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointFFTModel>>().Result;
        }

        public List<DataPointRMSModel> GetRMSData()
        {
            List<DataPointRMSModel> temp = new List<DataPointRMSModel>();
            HttpResponseMessage response = client.GetAsync("api/RMSData").Result;
            return temp = response.Content.ReadAsAsync<List<DataPointRMSModel>>().Result;
        }

        public List<DataPointRMSModel> GetRMSDataByDates(string startdate, string enddate)
        {
            List<DataPointRMSModel> temp = new List<DataPointRMSModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/RMSData?startdate={0}&enddate={1}", startdate, enddate)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointRMSModel>>().Result;
        }

        public List<DataPointRMSModel> GetRMSDataByGroupIDAndDates(int ID, string startdate, string enddate)
        {
            List<DataPointRMSModel> temp = new List<DataPointRMSModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/RMSData?ID={0}&startdate={1}&enddate={2}",ID, startdate, enddate)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointRMSModel>>().Result;
        }

        public List<DataPointRMSModel> GetRMSDataByGroupID(int ID)
        {
            List<DataPointRMSModel> temp = new List<DataPointRMSModel>();
            HttpResponseMessage response = client.GetAsync(String.Format("api/RMSData?id={0}", ID)).Result;
            return temp = response.Content.ReadAsAsync<List<DataPointRMSModel>>().Result;
        }
    }
}
