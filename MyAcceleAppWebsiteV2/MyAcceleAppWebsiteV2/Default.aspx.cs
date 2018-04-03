using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataPointViewModel;
using DataWebApi;

public partial class _Default : System.Web.UI.Page
{
    DataWebApiAccess access = new DataWebApiAccess();
    int timecount = 0;
    int fftcount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataPointFFTChart.Visible = false;
            DataPointTimeChart.Visible = false;

            access.Initialize("http://myacceleappwebserver.us-west-2.elasticbeanstalk.com/");
            List<string> datelist = new List<string>();
            List<DataPointModel> pointlist = new List<DataPointModel>();
            string temp;

            pointlist = access.GetRawData();
            foreach (DataPointModel element in pointlist)
            {
                temp = element.DataPointDateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                datelist.Add(temp);
            }
            DropDownList1.DataSource = datelist;
            DropDownList2.DataSource = datelist;
            DropDownList1.DataBind();
            DropDownList2.DataBind();
        }
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        access.Initialize("http://myacceleappwebserver.us-west-2.elasticbeanstalk.com/");

        List<DataPointModel> pointtimelist = new List<DataPointModel>();
        List<DataPointModel> pointfftlist = new List<DataPointModel>();

        DataPointFFTChart.Visible = true;
        DataPointTimeChart.Visible = true;

        pointtimelist = access.GetRawDataByDates(DropDownList1.SelectedItem.ToString(), DropDownList2.SelectedValue.ToString());
        pointfftlist = access.GetFFTDataByDates(DropDownList1.SelectedItem.ToString(), DropDownList2.SelectedValue.ToString());

        foreach (DataPointModel point in pointtimelist)
        {
            DataPointTimeChart.Series["DataPointSeriesX"].Points.AddXY(timecount, point.DataPointX);
            DataPointTimeChart.Series["DataPointSeriesY"].Points.AddXY(timecount, point.DataPointY);
            DataPointTimeChart.Series["DataPointSeriesZ"].Points.AddXY(timecount, point.DataPointZ);
            timecount++;
        }

        foreach (DataPointModel point in pointfftlist)
        {
            DataPointFFTChart.Series["DataPointSeriesX"].Points.AddXY(fftcount, point.DataPointX);
            DataPointFFTChart.Series["DataPointSeriesY"].Points.AddXY(fftcount, point.DataPointY);
            DataPointFFTChart.Series["DataPointSeriesZ"].Points.AddXY(fftcount, point.DataPointZ);
            fftcount++;
        }
    }
}