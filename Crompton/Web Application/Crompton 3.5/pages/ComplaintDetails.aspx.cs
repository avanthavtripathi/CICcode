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
using System.Text;
using System.IO;
public partial class pages_ComplaintDetails : System.Web.UI.Page
{
    CommonClass objCommonClass = new CommonClass();
    RequestRegistration objCallRegistration = new RequestRegistration();
    CommonPopUp objCommonPopUp = new CommonPopUp();
    DataSet dsCustomers = new DataSet();
    string strPhone,strComplaintRefNo;
    int intCommonCnt = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        #region Escalation portion only visible in callcenter , 20 march 12 bhawesh
        if (User.IsInRole("CCE") || User.IsInRole("CCAdmin"))
        {
            gvDetails.Columns[9].Visible = true;
            gvClosedCalls.Columns[9].Visible = true;
        }
        else
        {
            gvDetails.Columns[9].Visible = false;
            gvClosedCalls.Columns[9].Visible = false;
        }

      
        #endregion

        if (!Page.IsPostBack)
        {
            //Save Url End
            CommonClass.WriteToFile(Request.Url.ToString(), "InboundComplaintDeatils");
            //Stored Quering String value in Session Start 
            Session["userCrtObjectId"] = Convert.ToString(Request.QueryString["userCrtObjectId"]);
            Session["campaignId"] = Convert.ToString(Request.QueryString["campaignId"]);
            Session["phone"] = Convert.ToString(Request.QueryString["phone"]);
            Session["crm_push_generated_time"] = Convert.ToString(Request.QueryString["crm_push_generated_time"]);
            Session["sessionId"] = Convert.ToString(Request.QueryString["sessionId"]);
            Session["queueId"] = Convert.ToString(Request.QueryString["queueId"]);
            Session["dstPhone"] = Convert.ToString(Request.QueryString["dstPhone"]);
            Session["userId"] = Convert.ToString(Request.QueryString["userId"]);
            Session["queueName"] = Convert.ToString(Request.QueryString["queueName"]);
            Session["crtObjectId"] = Convert.ToString(Request.QueryString["crtObjectId"]);
            Session["crm-push-received-time"] = Convert.ToString(Request.QueryString["crm-push-received-time"]);
            Session["crm-push-received-time"] = Convert.ToString(Request.QueryString["crm-push-received-time"]);
            Session["network-latency"] = Convert.ToString(Request.QueryString["network-latency"]);


            if (!string.IsNullOrEmpty((string)Request.QueryString["phone"]))
            {
                if (Session["queueId"] != null)
                {
                    getLangauage(Convert.ToInt32(Session["queueId"].ToString()));
                }
            }

            //END



            //Selecting customers based on CTI phone number
            if ((Request.QueryString["phone"] != null) && ((Request.QueryString["phone"] != "")))
            {
                strPhone = Request.QueryString["phone"].ToString();// "9811413557";
                objCallRegistration.Type = "SELECT_CUSTOMER_PHONE";
                objCallRegistration.EmpCode = Membership.GetUser().UserName.ToString();
                if (strPhone.Length <= 10)
                {
                    strPhone = "0" + strPhone;
                }
                objCallRegistration.UniqueContact_No = strPhone;
                dsCustomers = objCallRegistration.GetCustomerOnPhone();
                if (objCallRegistration.ReturnValue == -1)
                {
                    //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
                    // trace, error message 
                    CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), objCallRegistration.MessageOut.ToString());

                }
                else
                {
                    if (dsCustomers.Tables[0].Rows.Count > 1)
                    {
                        ddlCustomers.DataSource = dsCustomers.Tables[0];
                        ddlCustomers.DataValueField = "CustomerId";
                        ddlCustomers.DataTextField = "FirstName";
                        ddlCustomers.DataBind();
                        hdnCustomerId.Value = ddlCustomers.SelectedValue;
                        //panOpenCalls.Visible = false;
                        // btnClosedCall.Visible = false;
                    }
                    else if (dsCustomers.Tables[0].Rows.Count == 1)
                    {
                        //pnlManyUsers.Visible = false;
                        //pnlSingle.Visible = true;
                        ddlCustomers.DataSource = dsCustomers.Tables[0];
                        ddlCustomers.DataValueField = "CustomerId";
                        ddlCustomers.DataTextField = "FirstName";
                        ddlCustomers.DataBind();
                        panOpenCalls.Visible = true;
                        btnClosedCall.Visible = true;
                        hdnCustomerId.Value = dsCustomers.Tables[0].Rows[0]["CustomerId"].ToString();
                        lblName.Text = dsCustomers.Tables[0].Rows[0]["FirstName"].ToString();
                        lblAddress.Text = dsCustomers.Tables[0].Rows[0]["Address1"].ToString() + " " + dsCustomers.Tables[0].Rows[0]["Address2"].ToString();
                        lblLandmark.Text = dsCustomers.Tables[0].Rows[0]["Landmark"].ToString();
                        lblCountry.Text = dsCustomers.Tables[0].Rows[0]["Country_Desc"].ToString();
                        lblState.Text = dsCustomers.Tables[0].Rows[0]["State_Desc"].ToString();
                        lblCity.Text = dsCustomers.Tables[0].Rows[0]["City_Desc"].ToString();
                        lblPinCode.Text = dsCustomers.Tables[0].Rows[0]["PinCode"].ToString();
                        lblCompany.Text = dsCustomers.Tables[0].Rows[0]["Company_Name"].ToString();
                        txtUnique.Text = dsCustomers.Tables[0].Rows[0]["UniqueContact_No"].ToString();
                        txtAltPhone.Text = dsCustomers.Tables[0].Rows[0]["AltTelNumber"].ToString();
                        if (dsCustomers.Tables[0].Rows[0]["Extension"].ToString() != "0")
                            lblExt.Text = dsCustomers.Tables[0].Rows[0]["Extension"].ToString();
                        lblEmail.Text = dsCustomers.Tables[0].Rows[0]["Email"].ToString();
                        lblFax.Text = dsCustomers.Tables[0].Rows[0]["Fax"].ToString();

                    }
                    else if (dsCustomers.Tables[0].Rows.Count == 0)
                    {
                        //pnlManyUsers.Visible = false;
                        //pnlSingle.Visible = true;
                        Response.Redirect("RequestRegistration.aspx");
                        ClearControls();
                        lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.RecordNotFound, enuMessageType.UserMessage, false, "");
                    }
                    if ((hdnCustomerId.Value != "") && (hdnCustomerId.Value != "0"))
                    {
                        FillGrid(gvDetails, Convert.ToInt64(hdnCustomerId.Value.ToString()), "N");
                    }

                }


            }
            panClosedCall.Visible = false;
        }
        else 
        {
            Session["tvtcampaignId"] = Convert.ToString(Request.QueryString["campaignId"]);
            Session["tvtphone"] = Convert.ToString(Request.QueryString["phone"]);
            Session["tvtcustomerId"] = Convert.ToString(Request.QueryString["customerId"]);        
            Session["tvtsessionId"] = Convert.ToString(Request.QueryString["sessionId"]);
        }
        DefaultButton(ref txtUnique, ref btnSearch);
        DefaultButton(ref txtAltPhone, ref btnSearch);
        DefaultButton(ref txtComplaintNo, ref btnSearch);
    }
    #region Setting default button on Enter key
    public void DefaultButton(ref TextBox objTextControl, ref Button objDefaultButton)
    {
        // The DefaultButton method set default button on enter pressed 
        StringBuilder sScript = new StringBuilder();
        sScript.Append("<SCRIPT language='javascript' type='text/javascript'>");
        sScript.Append("function fnTrapKD(btn){");
        sScript.Append(" if (document.all){");
        sScript.Append(" if (event.keyCode == 13)");
        sScript.Append(" {");
        sScript.Append(" event.returnValue=false;");
        sScript.Append(" event.cancel = true;");
        sScript.Append(" btn.click();");
        sScript.Append(" } ");
        sScript.Append(" } ");
        sScript.Append("return true;}");
        sScript.Append("<" + "/" + "SCRIPT" + ">");

        objTextControl.Attributes.Add("onkeydown", "return fnTrapKD(document.all." + objDefaultButton.ClientID + ")");
        if (!Page.IsStartupScriptRegistered("ForceDefaultToScript"))
        {
            Page.RegisterStartupScript("ForceDefaultToScript", sScript.ToString());
        }
    }
    #endregion Setting default button
   
    private void FillGrid(GridView gvComm,long lngCustomerId,string strIsClose)
    {
        try
        {
            objCallRegistration.CustomerId = lngCustomerId;
            objCallRegistration.Type = "SELECT_COMPLAINT_BASED_CUSTOMERID";
            objCallRegistration.BindComplaintDetailsCustomer(gvComm, strIsClose);
            if (objCallRegistration.ReturnValue == -1)
            {
                //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
                // trace, error message 
                CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), objCallRegistration.MessageOut.ToString());
             }
        }
        catch (Exception ex)
        {
            //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    protected void gvDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDetails.PageIndex = e.NewPageIndex;
        if ((hdnCustomerId.Value != "") && (hdnCustomerId.Value != "0"))
        {
            FillGrid(gvDetails, Convert.ToInt64(hdnCustomerId.Value.ToString()), "N");
        }

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        if (ddlCustomers.SelectedIndex != -1)
        {
            FillData(ddlCustomers.SelectedValue.ToString());
        }
        
    }
    private void FillData(string strCustomerId)
    {
        panOpenCalls.Visible = true;
        btnClosedCall.Visible = true;
        panClosedCall.Visible = false;
        objCallRegistration.Type = "SELECT_CUSTOMER_CUSTOMERID";
        objCallRegistration.EmpCode = Membership.GetUser().UserName.ToString();
        objCallRegistration.CustomerId = Convert.ToInt64(strCustomerId);
        dsCustomers = objCallRegistration.GetCustomerOnCustomerId();
        if (objCallRegistration.ReturnValue == -1)
        {
            //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), objCallRegistration.MessageOut.ToString());

        }
        else
        {
         if (dsCustomers.Tables[0].Rows.Count >0)
            {
                hdnCustomerId.Value = dsCustomers.Tables[0].Rows[0]["CustomerId"].ToString();
                lblName.Text = dsCustomers.Tables[0].Rows[0]["FirstName"].ToString();
                lblAddress.Text = dsCustomers.Tables[0].Rows[0]["Address1"].ToString() + " " + dsCustomers.Tables[0].Rows[0]["Address2"].ToString();
                lblLandmark.Text = dsCustomers.Tables[0].Rows[0]["Landmark"].ToString();
                lblCountry.Text = dsCustomers.Tables[0].Rows[0]["Country_Desc"].ToString();
                lblState.Text = dsCustomers.Tables[0].Rows[0]["State_Desc"].ToString();
                lblCity.Text = dsCustomers.Tables[0].Rows[0]["City_Desc"].ToString();
                lblPinCode.Text = dsCustomers.Tables[0].Rows[0]["PinCode"].ToString();
                lblCompany.Text = dsCustomers.Tables[0].Rows[0]["Company_Name"].ToString();
                txtUnique.Text = dsCustomers.Tables[0].Rows[0]["UniqueContact_No"].ToString();
                txtAltPhone.Text = dsCustomers.Tables[0].Rows[0]["AltTelNumber"].ToString();
                if (dsCustomers.Tables[0].Rows[0]["Extension"].ToString() != "0")
                lblExt.Text = dsCustomers.Tables[0].Rows[0]["Extension"].ToString();
                lblEmail.Text = dsCustomers.Tables[0].Rows[0]["Email"].ToString();
                lblFax.Text = dsCustomers.Tables[0].Rows[0]["Fax"].ToString();
                lblMessage.Text = "";
            }
            else
            {
                ClearControls();
            }
            if ((hdnCustomerId.Value != "") && (hdnCustomerId.Value != "0"))
            {
                FillGrid(gvDetails, Convert.ToInt64(hdnCustomerId.Value.ToString()), "N");
            }

        }
    }
    private void ClearControls()
    {
        txtComplaintNo.Text = "";
        txtAltPhone.Text = "";
        lblEmail.Text = "";
        lblFax.Text = "";
        lblName.Text = "";
        lblAddress.Text = "";
        lblExt.Text = "";
        lblLandmark.Text = "";
        lblPinCode.Text = "";
        lblState.Text = "";
        lblCountry.Text = "";
        lblCity.Text = "";
        lblCompany.Text = "";
        lblMessage.Text = "";
        panClosedCall.Visible = false;
        panOpenCalls.Visible = false;
        btnClosedCall.Visible = false;

        TxtCustID.Text = ""; //bhawesh 21 feb 12
        ddlCustomers.Items.Clear();
        txtUnique.Text = "";
    }
  
    protected void btnClosedCall_Click(object sender, EventArgs e)
    {
        panClosedCall.Visible = true;
        if ((hdnCustomerId.Value != "") && (hdnCustomerId.Value != "0"))
        {
            FillGrid(gvClosedCalls, Convert.ToInt64(hdnCustomerId.Value.ToString()), "Y");
        }
    }
    protected void gvClosedCalls_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvClosedCalls.PageIndex = e.NewPageIndex;
        if ((hdnCustomerId.Value != "") && (hdnCustomerId.Value != "0"))
        {
            FillGrid(gvClosedCalls,Convert.ToInt64(hdnCustomerId.Value.ToString()), "Y");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
      if (txtUnique.Text.Trim() != "")
        {
            strPhone = txtUnique.Text.Trim();
            if (strPhone.Length <= 10)
            {
                strPhone = "0" + strPhone;
            }
            
            objCallRegistration.UniqueContact_No = strPhone;
        }
        else
        {
            objCallRegistration.UniqueContact_No = "";
        }
        if (txtAltPhone.Text.Trim() != "")
        {
            strPhone = txtAltPhone.Text.Trim();
            if (strPhone.Length <= 10)
            {
                strPhone = "0" + strPhone;
            }
            objCallRegistration.AltTelNumber = strPhone;
        }
        else
        {
            objCallRegistration.AltTelNumber = "";
        }
        strComplaintRefNo = txtComplaintNo.Text.Trim();
        objCallRegistration.CustomerId = 0 ;
     

        //if (strComplaintRefNo != "")
        //{
        //    objCallRegistration.UniqueContact_No = "";
        //    objCallRegistration.AltTelNumber = "";
        //}
        if ((txtAltPhone.Text.Trim() == "") && (txtUnique.Text.Trim() == "") && (txtComplaintNo.Text.Trim() == ""))
        {
            lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.other, enuMessageType.Warrning, true, "Please enter Search Criteria");
        }
        else
        {
            ClearControls();
            objCallRegistration.Type = "SELECT_CUSTOMER_PHONE_COMPLAINT_EMAIL_FAX";
            dsCustomers = objCallRegistration.GetCustomerOnPhone(strComplaintRefNo);
            intCommonCnt = dsCustomers.Tables[0].Rows.Count;
           
            if (intCommonCnt > 0)
            {
                ddlCustomers.DataSource = dsCustomers.Tables[0];
                ddlCustomers.DataValueField = "CustomerId";
                ddlCustomers.DataTextField = "FirstName";
                ddlCustomers.DataBind();
                FillData(ddlCustomers.SelectedValue.ToString());
            }
            else
            {
               // ClearControls();
                FillGrid(gvDetails, 0, "N");
                FillGrid(gvClosedCalls, 0, "N");
                lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.RecordNotFound, enuMessageType.UserMessage, false, ""); 
                
            }
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        if (ddlCustomers.SelectedIndex != -1)
        {
            Session["btn"] = "btn";
            Response.Redirect("RequestRegistration.aspx?CustomerId=" + ddlCustomers.SelectedValue);
        }
        else
        {
            Session["btn"] = "btn";
            Response.Redirect("RequestRegistration.aspx");
        }
           // bhawesh 28 feb 12 , Call Center SLA need 
           // lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.AddRecord, enuMessageType.UserMessage, true, "Please Select Customer for New Request Registration"); 
    }
   
    protected void BtnSearchCustID_Click(object sender, EventArgs e)
    {

        if (TxtCustID.Text != "")
        {
            objCallRegistration.UniqueContact_No = "";
            objCallRegistration.AltTelNumber = "";
            objCallRegistration.CustomerId = Convert.ToInt64(TxtCustID.Text.Trim());
            ClearControls();
            objCallRegistration.Type = "SELECT_CUSTOMER_CUSTOMERID";
            dsCustomers = objCallRegistration.GetCustomerOnPhone("");
            intCommonCnt = dsCustomers.Tables[0].Rows.Count;

            if (intCommonCnt > 0)
            {

                ddlCustomers.DataSource = dsCustomers.Tables[0];
                ddlCustomers.DataValueField = "CustomerId";
                ddlCustomers.DataTextField = "FirstName";
                ddlCustomers.DataBind();
                FillData(ddlCustomers.SelectedValue.ToString());
            }
            else
            {
                // ClearControls();
                FillGrid(gvDetails, 0, "N");
                FillGrid(gvClosedCalls, 0, "N");
                lblMessage.Text = CommonClass.getErrorWarrning(enuErrorWarrning.RecordNotFound, enuMessageType.UserMessage, false, "");

            }
        }
    }

    protected void gvDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        objCommonPopUp.EmpCode = Membership.GetUser().UserName; 
        Button BtnSubmit = e.CommandSource as Button;
        TextBox txtescalate = null;
        if(BtnSubmit != null)
           txtescalate = BtnSubmit.NamingContainer.FindControl("txtescalate") as TextBox;
        if (BtnSubmit != null)
        {
              //  objCommonPopUp.IsEscalated = true;
                objCommonPopUp.Type = "UPDATE_COMPLAINT_COMMENT_BASELINE_ESCALATE";
                objCommonPopUp.UpdateComment(BtnSubmit.CommandName, BtnSubmit.CommandArgument, "Escalation :" + txtescalate.Text.Trim());
                if (objCommonPopUp.ReturnValue == -1)
                {
                    //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
                    // trace, error message 
                    CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), objCommonPopUp.MessageOut);
                }
                else
                {
                   FillGrid(gvDetails, Convert.ToInt64(hdnCustomerId.Value.ToString()), "N");
                    BtnSubmit.Enabled = false;
                }

        
        
        }


    }
    
    protected void chkescalate_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = sender as CheckBox;
        TextBox txtesc = chk.NamingContainer.FindControl("txtescalate") as TextBox;
        Button btnSave = chk.NamingContainer.FindControl("btnSave") as Button;
        if (chk.Checked)
        {
            txtesc.Enabled = true;
            btnSave.Enabled = true;
        }
        else
        {
            txtesc.Enabled = false;
            btnSave.Enabled = false;
        }

    }

    protected void BtnRegister_Click(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        objCallRegistration.EmpCode = Membership.GetUser().UserName.ToString();
        objCallRegistration.ReRegisterComplaint(btn.CommandArgument);
        btn.Enabled = false;
        string msg = "Old Complaint No : " + btn.CommandArgument + "\tNew Complaint No : " +  objCallRegistration.Complaint_RefNoOUT;
        ScriptManager.RegisterClientScriptBlock(btn, GetType(), "REREGISTER", "alert('" + msg + "');", true);
    }
    public void getLangauage(int queueId)
    {
        if (queueId == 26)
        {
            lblLanguage.Text = " Tamil-Inbound Call";
        }
        else if (queueId == 25)
        {
            lblLanguage.Text = " Malyalam-Inbound Call";
        }
        else if (queueId == 24)
        {
            lblLanguage.Text = " Hindi-Inbound Call ";
        }
        else if (queueId == 23)
        {
            lblLanguage.Text = " English-Inbound Call ";
        }
        else if (queueId == 21)
        {
            lblLanguage.Text = " Bengali-Inbound Call  ";
        }
        else if (queueId == 22)
        {
            lblLanguage.Text = " Default-Inbound Call  ";
        }

    }

  
}
