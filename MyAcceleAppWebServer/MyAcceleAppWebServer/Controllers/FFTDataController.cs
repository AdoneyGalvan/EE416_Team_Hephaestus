using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SQLDataBase;
using DataPointViewModelServerWebsite;

namespace MyAcceleAppWebServer.Controllers
{
    public class FFTDataController : ApiController
    {
        SQLDataBaseAccess access = new SQLDataBaseAccess(ConfigurationManager.ConnectionStrings["CSQLDB"].ConnectionString);
        // GET: api/FFTData
        public List<DataPointFFTModel> Get()
        {
            List<DataPointFFTModel> datalist = new List<DataPointFFTModel>();
            datalist = access.GetFFTDataPoints();
            return datalist;
        }
        
        // GET: api/FFTData/5
        public List<DataPointFFTModel> Get(int id)
        {
            List<DataPointFFTModel> data = new List<DataPointFFTModel>();
            data = access.GetFFTDataPointByGroupID(id);
            return data;
        }

        public List<DataPointFFTModel> Get(string startdate, string enddate)
        {
            List<DataPointFFTModel> datalist = new List<DataPointFFTModel>();
            datalist = access.GetFFTDataPointsByDate(startdate, enddate);
            return datalist;
        }

        public List<DataPointFFTModel> Get(int ID, string startdate, string enddate)
        {
            List<DataPointFFTModel> datalist = new List<DataPointFFTModel>();
            datalist = access.GetFFTDataPointsByGroupIDAndDate(ID, startdate, enddate);
            return datalist;
        }

        // POST: api/FFTData
        public HttpResponseMessage Post([FromBody]DataPointFFTModel value)
        {
            int id;
            id = access.AddFFTDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("FFTData/{0}", id));
            return response;

        }

        // PUT: api/FFTData/5
        public HttpResponseMessage Put([FromBody]DataPointFFTModel value)
        {
            access.EditFFTDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }

        // DELETE: api/FFTData/5
        public HttpResponseMessage Delete(int id)
        {
            access.DeleteFFTDataPointByID(id);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}
