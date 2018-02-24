using MyAcceleAppSQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyAcceleAppWebServer.Controllers
{
    public class FFTDataController : ApiController
    {
        SQLDataAccess access = new SQLDataAccess(ConfigurationManager.ConnectionStrings["CSQLDB"].ConnectionString);
        // GET: api/FFTData
        public List<DataPointModel> Get()
        {
            List<DataPointModel> datalist = new List<DataPointModel>();
            datalist = access.GetFFTDataPoints();
            return datalist;
        }

        // GET: api/FFTData/5
        public DataPointModel Get(int id)
        {
            DataPointModel data = new DataPointModel();
            data = access.GetFFTDataPointByID(id);
            return data;
        }

        public List<DataPointModel> Get(string startdate, string enddate)
        {
            List<DataPointModel> datalist = new List<DataPointModel>();
            datalist = access.GetFFTDataPointsByDate(startdate, enddate);
            return datalist;
        }

        // POST: api/FFTData
        public HttpResponseMessage Post([FromBody]DataPointModel value)
        {
            int id;
            id = access.AddFFTDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("FFTData/{0}", id));
            return response;

        }

        // PUT: api/FFTData/5
        public HttpResponseMessage Put([FromBody]DataPointModel value)
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
