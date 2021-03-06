﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;


public partial class pages_ScDashBoard : System.Web.UI.Page
{
    SqlDataAccessLayer objSqlDataAccessLayer = new SqlDataAccessLayer();
    SqlParameter[] sqlParamS =  {
                                    new SqlParameter("@UserName",SqlDbType.VarChar)
                                };


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindChart();
            BindRenkoChart();
        }
    }

    public DataTable GetData()
    {
        sqlParamS[0].Value = Membership.GetUser().UserName;
        DataSet ds = new DataSet();


        ds = objSqlDataAccessLayer.ExecuteDataset(CommandType.StoredProcedure, "usp_dashBoardBranchSCWise", sqlParamS);
        DataTable dt = ds.Tables[0];
        return dt;
    }

    public void BindChart()
    {
        DataTable dt = GetData();

        if (HttpContext.Current.User.IsInRole("SC"))
        {

            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[dt.Rows.Count]; // X point
            int[] YPointMember = new int[dt.Rows.Count];  // Y point 
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = dt.Rows[count]["Name"].ToString();
                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(dt.Rows[count]["TotalPending"]);

            }
            //binding chart control  
            scchart.Series[0].Points.DataBindXY(XPointMember, YPointMember);
            //Setting width of line  
            scchart.Series[0].BorderWidth = 20;
            //setting Chart type   
            scchart.Series[0].ChartType = SeriesChartType.Doughnut;
            foreach (Series charts in scchart.Series)
            {
                int i = 0;
                foreach (DataPoint point in charts.Points)
                {
                    string[] colors = Enum.GetNames(typeof(System.Drawing.KnownColor));
                    Color color = Color.FromName(colors[i]);
                    point.Label = string.Format("{0:0} - {1}", point.YValues[0], point.AxisLabel, point.Color = color);
                    i++;
                }
            }
        }
        scchart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
    }
    public void BindRenkoChart()
    {
        DataTable dt = GetData();
        if (HttpContext.Current.User.IsInRole("SC"))
        {

            //storing total rows count to loop on each Record  
            string[] XPointMember = new string[dt.Rows.Count]; // X point
            int[] YPointMember = new int[dt.Rows.Count];  // Y point 
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = dt.Rows[count]["Name"].ToString();
                //storing values for Y Axis  
                YPointMember[count] = Convert.ToInt32(dt.Rows[count]["TotalPending"]);

            }
            //binding chart control  

            Chart1.Series[0].Points.DataBindXY(XPointMember, YPointMember);
            //Setting width of line  
            Chart1.Series[0].BorderWidth = 20;
            //setting Chart type   
            Chart1.Series[0].ChartType = SeriesChartType.Pie;
            foreach (Series charts in Chart1.Series)
            {
                int i = 0;
                foreach (DataPoint point in charts.Points)
                {
                    string[] colors = Enum.GetNames(typeof(System.Drawing.KnownColor));
                    Color color = Color.FromName(colors[i+1]);
                    point.Label = string.Format("{0:0} - {1}", point.YValues[0], point.AxisLabel, point.Color = color);
                    i++;
                }
            }
        }
      
       
    }
}

