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
    public class DataController : ApiController
    {
        SQLDataAccess access = new SQLDataAccess(ConfigurationManager.ConnectionStrings["CSQLDB"].ConnectionString);
        // GET: api/Data
        public List<DataPointModel> Get()
        {
            List<DataPointModel> datalist = new List<DataPointModel>();
            datalist = access.GetDataPoints();
            return datalist;
        }

        // GET: api/Data/5
        public DataPointModel Get(int id)
        {
            DataPointModel data = new DataPointModel();
            data = access.GetDataPointByID(id);
            return data;
        }

        public List<DataPointModel> Get(string startdate, string enddate)
        {
            List<DataPointModel> datalist = new List<DataPointModel>();
            datalist = access.GetDataPointsByDate(startdate,enddate);
            return datalist;
        }

        // POST: api/Data
        public HttpResponseMessage Post([FromBody]DataPointModel value)
        { 
            int id;
            id = access.AddDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("data/{0}", id));
            return response;

        }

        // PUT: api/Data/5
        public HttpResponseMessage Put([FromBody]DataPointModel value)
        {
            access.EditDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }

        // DELETE: api/Data/5
        public HttpResponseMessage Delete(int id)
        {
            access.DeleteDataPointByID(id);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}
