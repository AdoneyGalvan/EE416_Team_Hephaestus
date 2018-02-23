using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class _Default : System.Web.UI.Page
{
    //SQLDataAccess access = new SQLDataAccess(ConfigurationManager.ConnectionStrings["CSQLDB"].ConnectionString);
    WebApiAccess access = new WebApiAccess();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            List<string> datelist = new List<string>();
            List<DataPointModel> pointlist = new List<DataPointModel>();
            string temp;

            pointlist = access.WebApiGetData();
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
        List<DataPointModel> pointlist = new List<DataPointModel>();
        pointlist = access.WebApiGetDataByDates(DropDownList1.SelectedItem.ToString(), DropDownList2.SelectedValue.ToString());
        GridView1.DataSource = pointlist;
        GridView1.DataBind();
    }
}