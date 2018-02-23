using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

/// <summary>
/// Summary description for WebApiAccess
/// </summary>
public class WebApiAccess
{
    private HttpClient client = new HttpClient();

    public WebApiAccess()
    {
        client.BaseAddress = new Uri("http://myacceleappwebserver.us-west-2.elasticbeanstalk.com/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
    }

    public List<DataPointModel> WebApiGetData()
    {
        List<DataPointModel> temp = new List<DataPointModel>();
        HttpResponseMessage response = client.GetAsync("api/Data").Result;
        return temp = response.Content.ReadAsAsync<List<DataPointModel>>().Result;
    }

    public List<DataPointModel> WebApiGetDataByDates(string startdate, string enddate)
    {
        List<DataPointModel> temp = new List<DataPointModel>();
        HttpResponseMessage response = client.GetAsync(String.Format("api/Data?startdate={0}&enddate={1}",startdate, enddate)).Result;
        return temp = response.Content.ReadAsAsync<List<DataPointModel>>().Result;
    }

    public DataPointModel WebApiGetDataByID(int ID)
    {
        DataPointModel temp = new DataPointModel();
        HttpResponseMessage response = client.GetAsync(String.Format("api/Data?id={0}",ID)).Result;
        return temp = response.Content.ReadAsAsync<DataPointModel>().Result;
    }


}