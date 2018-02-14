using MyAcceleAppSQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    SQLDataAccess access = new SQLDataAccess(ConfigurationManager.ConnectionStrings["CSQLDB"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            List<string> pointlist = new List<string>();

            pointlist = access.GetDataPointsDates();
            DropDownList1.DataSource = pointlist;
            DropDownList2.DataSource = pointlist;
            DropDownList1.DataBind();
            DropDownList2.DataBind();
        }
    }


    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        List<DataPointModel> pointlist = new List<DataPointModel>();
        pointlist = access.GetDataPointsByDate(DropDownList1.SelectedItem.ToString(), DropDownList2.SelectedValue.ToString());
        GridView1.DataSource = pointlist;
        GridView1.DataBind();
    }
}