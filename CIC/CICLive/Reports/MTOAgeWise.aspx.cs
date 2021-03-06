﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Microsoft.Reporting.WebForms;

public partial class Reports_MTOAgeWise : System.Web.UI.Page
{
    MTOReports objMTOReport = new MTOReports();
    //CommonClass objCommonClass = new CommonClass();

    //For Searching
    SqlParameter[] sqlParamSrh =
        {
            new SqlParameter("@Type","SELECT_AGEWISE"),
            new SqlParameter("@ProductDivision_Sno",0), 
            new SqlParameter("@FromDate",""),
            new SqlParameter("@ToDate","")
        };


    protected void Page_Load(object sender, EventArgs e)
    {
        //Reports/MTOGuaranteeStatus.aspx
        if (!Page.IsPostBack)
        {
            txtFromDate.Text = DateTime.Today.ToShortDateString();
            txtToDate.Text = DateTime.Today.ToShortDateString();
            // Added By Gaurav Garg on 18 DEC For using username to get product division.
            objMTOReport.UserName = Membership.GetUser().UserName.ToString();
            objMTOReport.GetProductDivisions(ddlProductDivison);
            
        }
        System.Threading.Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["AjaxPleaseWaitTime"]));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        rvMisDeatil.Visible = false;
        lblMessage.Visible = false;

        lblMessage.Text = "";
        objMTOReport.ProductDivision = ddlProductDivison.SelectedValue.ToString();
        objMTOReport.FromDate = txtFromDate.Text.Trim();
        objMTOReport.ToDate = txtToDate.Text.Trim();
        // Added By Gaurav Garg on 18 DEC 
        objMTOReport.UserName = Membership.GetUser().UserName.ToString();
        

        DataSet dsReport;
        rvMisDeatil.Visible = true;
        lblMessage.Visible = true;

        rvMisDeatil.ProcessingMode = ProcessingMode.Local;

        rvMisDeatil.Reset();
        rvMisDeatil.LocalReport.Refresh();
        rvMisDeatil.LocalReport.DataSources.Clear();



        dsReport = objMTOReport.ReportData("AGEWISE_REPORT");
        rvMisDeatil.LocalReport.ReportPath = "Reports\\BarReport\\AgeWiseReport.rdlc";
        if (dsReport.Tables[0].Rows.Count > 0)
        {
            ReportDataSource datasource = new ReportDataSource("DSMTOReport", dsReport.Tables[0]);
            rvMisDeatil.LocalReport.DataSources.Add(datasource);
            btnExport.Visible = true;
            ViewState["MTOReport"] = dsReport; 
        }
        else
        {
            lblMessage.Visible = true;
            rvMisDeatil.Visible = false;
            btnExport.Visible = false;
            lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.RecordNotFound, enuMessageType.Warrning, true, "No record found");
        }


    }

    protected void rvMisDeatil_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        btnSearch_Click(null, null);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            Response.ClearContent();
            Response.AddHeader("Content-Disposition", "attachment;filename=MTOAgeWise.xls");
            Response.ContentType = "application/ms-excel";
            // SearchData();  
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            ds = (DataSet)ViewState["MTOReport"];
            gvExport.DataSource = ds;
            gvExport.DataBind();
            gvExport.RenderControl(htmlWrite);
            Response.Write(stringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {//Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
   
}
