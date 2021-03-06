﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class SIMS_Admin_SpareConsumeMaster : System.Web.UI.Page
{
    SIMSCommonClass objCommonClass = new SIMSCommonClass();
    SpareConsumeMaster objSpareConsumeMaster = new SpareConsumeMaster();
    int intCnt = 0;

    SqlParameter[] sqlParamSrh =
        {
            new SqlParameter("@Type","SEARCH"),
            new SqlParameter("@Column_name",""),
            new SqlParameter("@SearchCriteria",""),
            new SqlParameter("@Active_Flag","")
            
        };

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        imgBtnAdd.Visible = false;
        sqlParamSrh[3].Value = int.Parse(rdoboth.SelectedValue);
        if (!Page.IsPostBack)
        {
            objCommonClass.BindDataGrid(gvComm, "uspSpareConsume", true, sqlParamSrh, lblRowCount);
            imgBtnUpdate.Visible = false;
            ViewState["Column"] = "SpareConsume_Id";
            ViewState["Order"] = "ASC";
        }
        System.Threading.Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["AjaxPleaseWaitTime"]));
    }
    protected void Page_UnLoad(object sender, EventArgs e)
    {
        objCommonClass = null;
        objSpareConsumeMaster = null;

    }
    #endregion

    #region Add Button
    protected void imgBtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
                objSpareConsumeMaster.SpareConsume_Code = txtcode.Text.Trim();
                objSpareConsumeMaster.SpareConsume_Desc = txtDescription.Text.Trim();
                objSpareConsumeMaster.Action_Type =Convert.ToInt32(ddlActionType.SelectedValue);
                objSpareConsumeMaster.EmpCode = Membership.GetUser().UserName.ToString();
                objSpareConsumeMaster.ActiveFlag = rdoStatus.SelectedValue.ToString();
                string strMsg = objSpareConsumeMaster.SaveData("INSERT_SPARE_CONSUME");
                if (objSpareConsumeMaster.ReturnValue == -1)
                {
                    lblMessage.Text = SIMSCommonClass.getErrorWarrning(SIMSenuErrorWarrning.ErrorInStoreProc, SIMSenuMessageType.Error, false, "");
                }
                else
                {
                    if (strMsg == "Exists")
                    {
                        lblMessage.Text = SIMSCommonClass.getErrorWarrning(SIMSenuErrorWarrning.ActivateStatusNotChange, SIMSenuMessageType.UserMessage, false, "");
                    }
                    else
                    {
                        lblMessage.Text = SIMSCommonClass.getErrorWarrning(SIMSenuErrorWarrning.RecordUpdated, SIMSenuMessageType.UserMessage, false, "");
                    }
                }
        }

        catch (Exception ex)
        {
            //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            SIMSCommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
        objCommonClass.BindDataGrid(gvComm, "uspSpareConsume", true, sqlParamSrh, lblRowCount);
        ClearControls();
    }
    #endregion

    #region Update Button
    protected void imgBtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnSpareConsume_Id.Value != "")
            {
                //Assigning values to properties
                objSpareConsumeMaster.SpareConsume_Id = Convert.ToInt32(hdnSpareConsume_Id.Value);
                objSpareConsumeMaster.SpareConsume_Code = txtcode.Text.Trim();
                objSpareConsumeMaster.SpareConsume_Desc = txtDescription.Text.Trim();
                objSpareConsumeMaster.Action_Type =Convert.ToInt32(ddlActionType.SelectedValue);
                objSpareConsumeMaster.EmpCode = Membership.GetUser().UserName.ToString();
                objSpareConsumeMaster.ActiveFlag = rdoStatus.SelectedValue.ToString();
                string strMsg = objSpareConsumeMaster.SaveData("UPDATE_SPARE_CONSUME");
                if (objSpareConsumeMaster.ReturnValue == -1)
                {
                    lblMessage.Text = SIMSCommonClass.getErrorWarrning(SIMSenuErrorWarrning.ErrorInStoreProc, SIMSenuMessageType.Error, false, "");
                }
                else
                {
                    if (strMsg == "Exists")
                    {
                        lblMessage.Text = SIMSCommonClass.getErrorWarrning(SIMSenuErrorWarrning.ActivateStatusNotChange, SIMSenuMessageType.UserMessage, false, "");
                    }
                    else
                    {
                        lblMessage.Text = SIMSCommonClass.getErrorWarrning(SIMSenuErrorWarrning.RecordUpdated, SIMSenuMessageType.UserMessage, false, "");
                    }
                }
            }

        }

        catch (Exception ex)
        {
            SIMSCommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
        objCommonClass.BindDataGrid(gvComm, "uspSpareConsume", true, sqlParamSrh, lblRowCount);
        ClearControls();
    }
    #endregion

    #region Cancel Button
    protected void imgBtnCancel_Click(object sender, EventArgs e)
    {
        ClearControls();
        lblMessage.Text = "";
    }
    #endregion

    #region GO Button
    protected void imgBtnGo_Click(object sender, EventArgs e)
    {
        if (gvComm.PageIndex != -1)
            gvComm.PageIndex = 0;

        sqlParamSrh[1].Value = ddlSearch.SelectedValue.ToString();
        sqlParamSrh[2].Value = txtSearch.Text.Trim();
        objCommonClass.BindDataGrid(gvComm, "uspSpareConsume", true, sqlParamSrh, lblRowCount);
    
    }
    #endregion

    #region Radio Button
    protected void rdoboth_SelectedIndexChanged(object sender, EventArgs e)
    {
        imgBtnGo_Click(null, null);
    }
    #endregion

    #region Gried View Event
    protected void gvComm_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvComm.PageIndex = e.NewPageIndex;
        BindData(Convert.ToString(ViewState["Column"]) + " " + Convert.ToString(ViewState["Order"]));
    }
      
    protected void gvComm_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        imgBtnUpdate.Visible = true;
        imgBtnAdd.Visible = false;      
        hdnSpareConsume_Id.Value = gvComm.DataKeys[e.NewSelectedIndex].Value.ToString();
        BindSelected(Convert.ToInt32(hdnSpareConsume_Id.Value));
    }
    protected void gvComm_Sorting(object sender, GridViewSortEventArgs e)
    {
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
        gvComm.PageIndex = 0;
    }

    #endregion

    #region ClearControl Method
    private void ClearControls()
    {
        txtcode.Enabled = true;
        imgBtnAdd.Visible = false;
        imgBtnUpdate.Visible = false;
        rdoStatus.SelectedIndex = 0;
        txtcode.Text = "";
        txtDescription.Text = "";
        ddlActionType.SelectedIndex = 0;
    }
    #endregion

    #region Bind Data to Sorting Order
    private void BindData(string strOrder)
    {
        DataSet dstData = new DataSet();
        sqlParamSrh[1].Value = ddlSearch.SelectedValue.ToString();
        sqlParamSrh[2].Value = txtSearch.Text.Trim();

        dstData = objCommonClass.BindDataGrid(gvComm, "uspSpareConsume", true, sqlParamSrh, true);

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
    #endregion

    #region Gried Select VendorId
    private void BindSelected(int intSpareConsumeId)
    {
        lblMessage.Text = "";
        txtcode.Enabled = false;
        objSpareConsumeMaster.BindSpareConsumeDetails(intSpareConsumeId, "SELECT_ON_SPARE_CONSUME_ID");
        txtcode.Text = objSpareConsumeMaster.SpareConsume_Code;
        txtDescription.Text = objSpareConsumeMaster.SpareConsume_Desc;
        for (int i = 0; i < ddlActionType.Items.Count; i++)
        {
            if (ddlActionType.Items[i].Value == Convert.ToString(objSpareConsumeMaster.Action_Type))
                ddlActionType.SelectedIndex = i;
        }
        
        for (intCnt = 0; intCnt < rdoStatus.Items.Count; intCnt++)
        {
            if (rdoStatus.Items[intCnt].Value.ToString().Trim() == objSpareConsumeMaster.ActiveFlag.ToString().Trim())
            {
                rdoStatus.Items[intCnt].Selected = true;
            }
            else
            {
                rdoStatus.Items[intCnt].Selected = false;
            }
        }

    }
    #endregion
}
