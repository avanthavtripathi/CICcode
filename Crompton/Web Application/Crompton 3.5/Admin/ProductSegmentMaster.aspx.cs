﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Data;


public partial class Admin_ProductSegmentMaster : System.Web.UI.Page
{
    CommonClass objCommonClass = new CommonClass();
    ProductTypeMaster objProductType = new ProductTypeMaster();
    BusinessLine objBusinessLine = new BusinessLine();
    ProductMaster objProductMaster = new ProductMaster(); // to bind product division

    SqlParameter[] sqlParamSrh =
        {
            new SqlParameter("@Type","SEARCH_PRODUCT_SEGMENT"),
            new SqlParameter("@Column_name",""),
            new SqlParameter("@SearchCriteria",""),
            new SqlParameter("@Active_Flag","")
        };

    protected void Page_Load(object sender, EventArgs e)
    {
        sqlParamSrh[3].Value = int.Parse(rdoboth.SelectedValue);
        if (!Page.IsPostBack)
        {
            objCommonClass.BindDataGrid(gvComm, "[uspProductMaster]", true, sqlParamSrh, lblRowCount);
            imgBtnUpdate.Visible = false;

            ViewState["Column"] = "ProductSegment_Desc";
            ViewState["Order"] = "ASC";
        }
        System.Threading.Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["AjaxPleaseWaitTime"]));
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        objCommonClass = null;
        objProductType = null;

    }

    protected void imgBtnGo_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (gvComm.PageIndex != -1)
            gvComm.PageIndex = 0;
        sqlParamSrh[1].Value = ddlSearch.SelectedValue.ToString();
        sqlParamSrh[2].Value = txtSearch.Text.Trim();
        objCommonClass.BindDataGrid(gvComm, "uspProductMaster", true, sqlParamSrh, lblRowCount);

    }

    protected void rdoboth_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        imgBtnGo_Click(null, null);
    }

    protected void imgBtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            objProductType.ProductSegment_Desc = txtProductSegmentDesc.Text;
            objProductType.ProductSegment_Code = txtProductSegmentCode.Text;
            objProductType.EmpCode = Membership.GetUser().UserName.ToString();
            objProductType.ActiveFlag = rdoStatus.SelectedValue.ToString();
            string strMsg = objProductType.SaveProductSegment("INSERT_PRODUCT_SEGMENT");
            if (objProductType.ReturnValue == -1)
            {
                lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.ErrorInStoreProc, enuMessageType.Error, false, "");
            }
            else
            {
                if (strMsg == "Exists")
                {
                    lblMessage.Text = "Product Segment Code is already exits.";
                }
                else
                {
                    lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.AddRecord, enuMessageType.UserMessage, false, "");
                }
            }
        }
        catch (Exception ex)
        {
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
        objCommonClass.BindDataGrid(gvComm, "[uspProductMaster]", true, sqlParamSrh, lblRowCount);
        ClearField();
    }

    protected void imgBtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnProductSegmentId.Value != "")
            {
                objProductType.ProductSegment_Sno = int.Parse(hdnProductSegmentId.Value.ToString());
                objProductType.ProductSegment_Desc = txtProductSegmentDesc.Text;
                objProductType.ProductSegment_Code = txtProductSegmentCode.Text;
                objProductType.EmpCode = Membership.GetUser().UserName.ToString();
                objProductType.ActiveFlag = rdoStatus.SelectedValue.ToString();

                string strMsg = objProductType.SaveProductSegment("UPDATE_PRODUCT_SEGMENT");
                if (objProductType.ReturnValue == -1)
                {
                    lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.ErrorInStoreProc, enuMessageType.Error, false, "");
                }
                else
                {
                    if (strMsg == "Exists")
                    {
                        lblMessage.Text = "Product Segment Code is already used for another record.";
                    }
                    else
                    {
                        lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.RecordUpdated, enuMessageType.UserMessage, false, "");
                    }
                }
            }

        }

        catch (Exception ex)
        {
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
        objCommonClass.BindDataGrid(gvComm, "[uspProductMaster]", true, sqlParamSrh, lblRowCount);
        ClearField();
        imgBtnUpdate.Visible = false;
        imgBtnAdd.Visible = true;
        hdnProductSegmentId.Value = null;

    }

    protected void imgBtnCancel_Click(object sender, EventArgs e)
    {
        ClearField();
        lblMessage.Text = "";
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        imgBtnUpdate.Visible = true;
        imgBtnAdd.Visible = false;

        GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);

        string Status = ((Label)row.FindControl("lblStatus")).Text;
        if (Status == "True")
            rdoStatus.SelectedIndex = 0;
        else
            rdoStatus.SelectedIndex = 1;

        txtProductSegmentDesc.Text = ((Label)row.FindControl("lblProductSegment_Desc")).Text;
        txtProductSegmentCode.Text = ((Label)row.FindControl("lblSegmentCode")).Text;
        hdnProductSegmentId.Value = ((HiddenField)row.FindControl("hdnProductSegmentSno")).Value;
    }

    protected void gvComm_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvComm.PageIndex = e.NewPageIndex;
        BindData(Convert.ToString(ViewState["Column"]) + " " + Convert.ToString(ViewState["Order"]));
    }
    protected void gvComm_Sorting(object sender, GridViewSortEventArgs e)
    {
        lblMessage.Text = "";
        if (gvComm.PageIndex != -1)
            gvComm.PageIndex = 0;
        string strOrder;
        //if same column clicked again then change the order. 
        if (e.SortExpression == Convert.ToString(ViewState["Column"]))
        {
            if (Convert.ToString(ViewState["Order"]) == "ASC")
            {
                strOrder = e.SortExpression + " DESC";
                ViewState["Order"] = "DESC";
            }
            else
            {
                strOrder = e.SortExpression + " ASC";
                ViewState["Order"] = "ASC";
            }
        }
        else
        {
            //default to asc order. 
            strOrder = e.SortExpression + " ASC";
            ViewState["Order"] = "ASC";
        }
        //Bind the datagrid. 
        ViewState["Column"] = e.SortExpression;
        BindData(strOrder);
    }

    private void BindData(string strOrder)
    {
        DataSet dstData = new DataSet();
        sqlParamSrh[1].Value = ddlSearch.SelectedValue.ToString();
        sqlParamSrh[2].Value = txtSearch.Text.Trim();

        dstData = objCommonClass.BindDataGrid(gvComm, "[uspProductMaster]", true, sqlParamSrh, true);

        DataView dvSource = default(DataView);

        dvSource = dstData.Tables[0].DefaultView;
        dvSource.Sort = strOrder;

        if ((dstData != null))
        {
            gvComm.DataSource = dvSource;
            gvComm.DataBind();
        }
        dstData = null;
        dvSource.Dispose();
        dvSource = null;

    }

    private void ClearField()
    {
        txtProductSegmentDesc.Text = "";
        rdoStatus.SelectedIndex = 0;
        txtSearch.Text = "";
        txtProductSegmentCode.Text = "";
        imgBtnAdd.Visible = true;
        imgBtnUpdate.Visible = false;
    }
}
