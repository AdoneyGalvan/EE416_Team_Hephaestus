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
    public class RMSDataController : ApiController
    {
        SQLDataBaseAccess access = new SQLDataBaseAccess(ConfigurationManager.ConnectionStrings["CSQLDB"].ConnectionString);

        // GET: api/RMSData
        public List<DataPointRMSModel> Get()
        {
            List<DataPointRMSModel> datalist = new List<DataPointRMSModel>();
            datalist = access.GetRMSDataPoints();
            return datalist;
        }

        // GET: api/RMSData/5
        public List<DataPointRMSModel> Get(int id)
        {
            List<DataPointRMSModel> data = new List<DataPointRMSModel>();
            data = access.GetRMSDataPointByGroupID(id);
            return data;
        }

        public List<DataPointRMSModel> Get(string startdate, string enddate)
        {
            List<DataPointRMSModel> datalist = new List<DataPointRMSModel>();
            datalist = access.GetRMSDataPointsByDate(startdate, enddate);
            return datalist;
        }

        public List<DataPointRMSModel> Get(int ID, string startdate, string enddate)
        {
            List<DataPointRMSModel> datalist = new List<DataPointRMSModel>();
            datalist = access.GetRMSDataPointsByGroupIDAndDate(ID, startdate, enddate);
            return datalist;
        }

        // POST: api/RMSData
        public HttpResponseMessage Post([FromBody]DataPointRMSModel value)
        {
            int id;
            id = access.AddRMSDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format("FFTData/{0}", id));
            return response;

        }

        // PUT: api/RMSData/5
        public HttpResponseMessage Put([FromBody]DataPointRMSModel value)
        {
            access.EditRMSDataPoint(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
        // DELETE: api/RMSData/5
        public HttpResponseMessage Delete(int id)
        {
            access.DeleteRMSDataPointByID(id);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
            return response;
        }
    }
}
