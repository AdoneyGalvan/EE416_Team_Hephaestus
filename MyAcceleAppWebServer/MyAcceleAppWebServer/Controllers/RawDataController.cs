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
    public class RawDataController : ApiController
    {
        SQLDataAccess access = new SQLDataAccess(ConfigurationManager.ConnectionStrings["CSQLDB"].ConnectionString);
        // GET: api/RawData
        public List<DataPointModel> Get()
        {
            List<DataPointModel> datalist = new List<DataPointModel>();
            datalist = access.GetRawDataPoints();
            return datalist;
        }

        // GET: api/RawData/5
        public DataPointModel Get(int id)
        {
            DataPointModel data = new DataPointModel();
            data = access.GetRawDataPointByID(id);
            return data;
        }

        public List<DataPointModel> Get(string startdate, string enddate)
        {
            List<DataPointModel> datalist = new List<DataPointModel>();
            datalist = access.GetRawDataPointsByDate(startdate, enddate);
            return datalist;
        }

        // POST: api/RawData
        public HttpResponseMessage Post([FromBody]DataPointModel value)
        {
            int id;
            id = access.AddRawDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("RawData/{0}", id));
            return response;

        }

        // PUT: api/RawData/5
        public HttpResponseMessage Put([FromBody]DataPointModel value)
        {
            access.EditRawDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }

        // DELETE: api/RawData/5
        public HttpResponseMessage Delete(int id)
        {
            access.DeleteRawDataPointByID(id);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }

    }
}
