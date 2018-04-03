using DataPointViewModelServerWebsite;
using DataWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VibrationSensorAgent;

public partial class _Default : System.Web.UI.Page
{
    DataWebApiAccess access = new DataWebApiAccess();
    Agent agent = new Agent();
    int timecount = 0;
    int fftcount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        DataPointTimeChart.Visible = false;
        DataPointFFTChart.Visible = false;
        DataPointRMSChart.Visible = false;

        if (!IsPostBack)
        {
            CalendarFrom.Visible = false;
            CalendarTo.Visible = false;
        }
    }

    protected void CalendarFrom_SelectionChanged(object sender, EventArgs e)
    {
        CalendarFrom.Visible = false;
        StartDate.Text = CalendarFrom.SelectedDate.ToString();
    }

    protected void CalendarTo_SelectionChanged(object sender, EventArgs e)
    {
        CalendarTo.Visible = false;
        EndDate.Text = CalendarTo.SelectedDate.ToString();
    }

    protected void ImageStartDate_Click(object sender, ImageClickEventArgs e)
    {
        CalendarFrom.Visible = true;
    }

    protected void ImageEndDate_Click(object sender, ImageClickEventArgs e)
    {
        CalendarTo.Visible = true;
    }


    protected void SelectDates_Click(object sender, EventArgs e)
    {
        DataPointRMSChart.Visible = true;

        access.Initialize("http://myacceleappwebserver.us-west-2.elasticbeanstalk.com/");

        List<DataPointTimeModel> pointtimelist = new List<DataPointTimeModel>();
        List<DataPointRMSModel> pointrmslist = new List<DataPointRMSModel>();

        VibrationSensorAgent.Action action = new VibrationSensorAgent.Action();

        List<string> grouplist = new List<string>();

        
        pointtimelist = access.GetTimeDataByDates(CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        pointrmslist = access.GetRMSDataByDates(CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        action = agent.ProcessRMS(pointrmslist);

        //Print to user
        AgentTextBox.Text = action.PrintAction(action);

        foreach (DataPointTimeModel element in pointtimelist)
        {

            //temp = element.DataPointDateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            grouplist.Add(element.DataPointGroupID.ToString());
            grouplist = grouplist.Distinct().ToList();//Remmove Duplicates group numbers
        }

        foreach(DataPointRMSModel point in pointrmslist)
        {
            DataPointRMSChart.Series["X-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointXRMS);
            //DataPointRMSChart.Series["Y-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointYRMS);
            //DataPointRMSChart.Series["Z-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointZRMS);
        }

        DropDownList1.DataSource = grouplist;
        DropDownList1.DataBind();
    }

    protected void SelectGroup_Click(object sender, EventArgs e)
    {
        DataPointTimeChart.Visible = true;
        DataPointFFTChart.Visible = true;
        DataPointRMSChart.Visible = true;
        access.Initialize("http://myacceleappwebserver.us-west-2.elasticbeanstalk.com/");

        List<DataPointTimeModel> pointtimelist = new List<DataPointTimeModel>();
        List<DataPointFFTModel> pointfftlist = new List<DataPointFFTModel>();
        List<DataPointRMSModel> pointrmslist = new List<DataPointRMSModel>();

        pointtimelist = access.GetTimeDataByGrouptIDAndDates(Convert.ToInt32(DropDownList1.SelectedValue), CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        pointfftlist = access.GetFFTDataByGroupIDAndDates(Convert.ToInt32(DropDownList1.SelectedValue), CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        pointrmslist = access.GetRMSDataByDates(CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());

        foreach (DataPointTimeModel point in pointtimelist)
        {
            DataPointTimeChart.Series["X-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointX);
            //DataPointTimeChart.Series["Y-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointY);
            //DataPointTimeChart.Series["Z-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointZ);
            timecount++;
        }

        foreach (DataPointFFTModel point in pointfftlist)
        {
            DataPointFFTChart.Series["X-Axis"].Points.AddXY(point.DataPointFreq, point.DataPointX);
            //DataPointFFTChart.Series["Y-Axis"].Points.AddXY(point.DataPointFreq, point.DataPointY);
            //DataPointFFTChart.Series["Z-Axis"].Points.AddXY(point.DataPointFreq, point.DataPointZ);
            fftcount++;
        }

        foreach (DataPointRMSModel point in pointrmslist)
        {
            DataPointRMSChart.Series["X-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointXRMS);
            //DataPointRMSChart.Series["Y-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointYRMS);
            //DataPointRMSChart.Series["Z-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointZRMS);
        }

    }
}