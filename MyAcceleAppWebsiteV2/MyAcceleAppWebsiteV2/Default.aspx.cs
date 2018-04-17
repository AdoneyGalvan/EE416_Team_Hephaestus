using DataPointViewModelServerWebsite;
using DataWebApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VibrationSensorAgent;

public partial class _Default : System.Web.UI.Page
{
    DataWebApiAccess access = new DataWebApiAccess();
    Agent agent = new Agent();

    protected void Page_Load(object sender, EventArgs e)
    {
        DataPointTimeChart.Visible = false;
        DataPointFFTChart.Visible = false;
        DataPointRMSChart.Visible = false;
        DataPointRMSChart.Series["X-Axis"].Enabled = false;
        DataPointTimeChart.Series["X-Axis"].Enabled = false;
        DataPointFFTChart.Series["X-Axis"].Enabled = false;
        DataPointRMSChart.Series["Y-Axis"].Enabled = false;
        DataPointTimeChart.Series["Y-Axis"].Enabled = false;
        DataPointFFTChart.Series["Y-Axis"].Enabled = false;
        DataPointRMSChart.Series["Z-Axis"].Enabled = false;
        DataPointTimeChart.Series["Z-Axis"].Enabled = false;
        DataPointFFTChart.Series["Z-Axis"].Enabled = false;

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
        check_axis();
        access.Initialize("http://myacceleappwebserver-env.us-west-2.elasticbeanstalk.com/");

        List<DataPointTimeModel> pointtimelist = new List<DataPointTimeModel>();
        List<DataPointRMSModel> pointrmslist = new List<DataPointRMSModel>();

        //VibrationSensorAgent.Action action = new VibrationSensorAgent.Action();

        List<string> grouplist = new List<string>();

        
        pointtimelist = access.GetTimeDataByDates(CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        pointrmslist = access.GetRMSDataByDates(CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        //action = agent.ProcessRMS(pointrmslist);

        //Print to user
        //AgentTextBox.Text = action.PrintAction(action);

        foreach (DataPointTimeModel element in pointtimelist)
        {

            //temp = element.DataPointDateTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
            grouplist.Add(element.DataPointGroupID.ToString());
            grouplist = grouplist.Distinct().ToList();//Remmove Duplicates group numbers
        }

        foreach(DataPointRMSModel point in pointrmslist)
        {
            DataPointRMSChart.Series["X-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointXRMS);
            DataPointRMSChart.Series["Y-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointYRMS);
            DataPointRMSChart.Series["Z-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointZRMS);
        }

        DropDownList1.DataSource = grouplist;
        DropDownList1.DataBind();
    }

    protected void SelectGroup_Click(object sender, EventArgs e)
    {
        update_page();
    }

    public void update_page()
    {
        DataPointTimeChart.Visible = true;
        DataPointFFTChart.Visible = true;
        DataPointRMSChart.Visible = true;
        check_axis();
        access.Initialize("http://myacceleappwebserver-env.us-west-2.elasticbeanstalk.com/");

        List<DataPointTimeModel> pointtimelist = new List<DataPointTimeModel>();
        List<DataPointFFTModel> pointfftlist = new List<DataPointFFTModel>();
        List<DataPointRMSModel> pointrmslist = new List<DataPointRMSModel>();

        pointtimelist = access.GetTimeDataByGrouptIDAndDates(Convert.ToInt32(DropDownList1.SelectedValue), CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        pointfftlist = access.GetFFTDataByGroupIDAndDates(Convert.ToInt32(DropDownList1.SelectedValue), CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        pointrmslist = access.GetRMSDataByDates(CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());

        foreach (DataPointTimeModel point in pointtimelist)
        {
            DataPointTimeChart.Series["X-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointX);
            DataPointTimeChart.Series["Y-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointY);
            DataPointTimeChart.Series["Z-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointZ);
        }

        foreach (DataPointFFTModel point in pointfftlist)
        {
            DataPointFFTChart.Series["X-Axis"].Points.AddXY(point.DataPointFreq, point.DataPointX);
            DataPointFFTChart.Series["Y-Axis"].Points.AddXY(point.DataPointFreq, point.DataPointY);
            DataPointFFTChart.Series["Z-Axis"].Points.AddXY(point.DataPointFreq, point.DataPointZ);
        }

        foreach (DataPointRMSModel point in pointrmslist)
        {
            DataPointRMSChart.Series["X-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointXRMS);
            DataPointRMSChart.Series["Y-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointYRMS);
            DataPointRMSChart.Series["Z-Axis"].Points.AddXY(point.DataPointDateTime.TimeOfDay.ToString(), point.DataPointZRMS);
        }
    }

    protected void SelectAxis_Click(object sender, EventArgs e)
    {
        update_page();
    }


    public void check_axis()
    {
        if (CheckBoxY.Checked)
        {
            DataPointRMSChart.Series["Y-Axis"].Enabled = true;
            DataPointTimeChart.Series["Y-Axis"].Enabled = true;
            DataPointFFTChart.Series["Y-Axis"].Enabled = true;
        }
        else
        {
            DataPointRMSChart.Series["Y-Axis"].Enabled = false;
            DataPointTimeChart.Series["Y-Axis"].Enabled = false;
            DataPointFFTChart.Series["Y-Axis"].Enabled = false;
        }

        if (CheckBoxX.Checked)
        {
            DataPointRMSChart.Series["X-Axis"].Enabled = true;
            DataPointTimeChart.Series["X-Axis"].Enabled = true;
            DataPointFFTChart.Series["X-Axis"].Enabled = true;
        }
        else
        {
            DataPointRMSChart.Series["X-Axis"].Enabled = false;
            DataPointTimeChart.Series["X-Axis"].Enabled = false;
            DataPointFFTChart.Series["X-Axis"].Enabled = false;
        }

        if (CheckBoxZ.Checked)
        {
            DataPointRMSChart.Series["Z-Axis"].Enabled = true;
            DataPointTimeChart.Series["Z-Axis"].Enabled = true;
            DataPointFFTChart.Series["Z-Axis"].Enabled = true;
        }
        else
        {
            DataPointRMSChart.Series["Z-Axis"].Enabled = false;
            DataPointTimeChart.Series["Z-Axis"].Enabled = false;
            DataPointFFTChart.Series["Z-Axis"].Enabled = false;
        }
    }

    public void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
    {
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        foreach (PropertyDescriptor prop in props)
        {
            output.Write(prop.DisplayName); // header
            output.Write("\t");
        }
        output.WriteLine();
        foreach (T item in data)
        {
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.Converter.ConvertToString(
                     prop.GetValue(item)));
                output.Write("\t");
            }
            output.WriteLine();
        }
    }

    public void ExportListFromTsv(string filename)
    {
        List<DataPointTimeModel> pointtimelist = new List<DataPointTimeModel>();
        List<DataPointFFTModel> pointfftlist = new List<DataPointFFTModel>();
        List<DataPointRMSModel> pointrmslist = new List<DataPointRMSModel>();

        //"attachment;filename=FFT.xls"
        string _filename = "attachment;filename=" + filename + ".xls";

        access.Initialize("http://myacceleappwebserver-env.us-west-2.elasticbeanstalk.com/");

        pointtimelist = access.GetTimeDataByGrouptIDAndDates(Convert.ToInt32(DropDownList1.SelectedValue), CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        pointfftlist = access.GetFFTDataByGroupIDAndDates(Convert.ToInt32(DropDownList1.SelectedValue), CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());
        pointrmslist = access.GetRMSDataByDates(CalendarFrom.SelectedDate.ToString(), CalendarTo.SelectedDate.ToString());


        Response.ClearContent();
        Response.AddHeader("content-disposition", _filename);
        Response.AddHeader("Content-Type", "application/vnd.ms-excel");
        WriteTsv(pointtimelist, Response.Output);
        WriteTsv(pointfftlist, Response.Output);
        WriteTsv(pointrmslist, Response.Output);
        Response.End();
    }


    protected void Export_Click(object sender, EventArgs e)
    {
        ExportListFromTsv(ExportTextBox.Text);
    }
}
