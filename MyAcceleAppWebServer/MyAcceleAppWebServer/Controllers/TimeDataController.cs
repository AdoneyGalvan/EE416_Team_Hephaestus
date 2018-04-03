using DataPointViewModelServerWebsite;
using SQLDataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyAcceleAppWebServer.Controllers
{
    public class TimeDataController : ApiController
    {
        SQLDataBaseAccess access = new SQLDataBaseAccess(ConfigurationManager.ConnectionStrings["CSQLDB"].ConnectionString);

        // GET: api/TimeData
        public List<DataPointTimeModel> Get()
        {
            List<DataPointTimeModel> datalist = new List<DataPointTimeModel>();
            datalist = access.GetTimeDataPoints();
            return datalist;
        }

        // GET: api/TimeData/5
        public List<DataPointTimeModel> Get(int id)
        {
            List<DataPointTimeModel> data = new List<DataPointTimeModel>();
            data = access.GetTimeDataPointsByGroupID(id);
            return data;
        }

        public List<DataPointTimeModel> Get(string startdate, string enddate)
        {
            List<DataPointTimeModel> datalist = new List<DataPointTimeModel>();
            datalist = access.GetTimeDataPointsByDate(startdate, enddate);
            return datalist;
        }

        public List<DataPointTimeModel> Get(int ID, string startdate, string enddate)
        {
            List<DataPointTimeModel> datalist = new List<DataPointTimeModel>();
            datalist = access.GetTimeDataPointsByGroupIDAndDate(ID, startdate, enddate);
            return datalist;
        }

        // POST: api/TimeData
        public HttpResponseMessage Post([FromBody]DataPointTimeModel value)
        {
            int id;
            id = access.AddTimeDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("FFTData/{0}", id));
            return response;

        }

        // PUT: api/TimeData/5
        public HttpResponseMessage Put([FromBody]DataPointTimeModel value)
        {
            access.EditTimeDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }

        // DELETE: api/TimeData/5
        public HttpResponseMessage Delete(int id)
        {
            access.DeleteTimeDataPointByID(id);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}
