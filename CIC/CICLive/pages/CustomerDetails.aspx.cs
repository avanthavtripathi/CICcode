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

public partial class pages_CustomerDetails : System.Web.UI.Page
{
    CommonClass objCommonClass = new CommonClass();
    //clsCallRegistration objCommonPopUp = new clsCallRegistration();
    CommonPopUp objCommonPopUp = new CommonPopUp();
    string strCustNo = "", strComplaintNo="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindRecord();
        }
    }

    private void BindRecord()
    {
        try
        {
            DataSet dsuser = new DataSet();
            strCustNo = Request.QueryString["cid"].ToString();
            //strComplaintNo = Request.QueryString["compNo"].ToString();
            //lblComRefNo.Text = Request.QueryString["CompNo"].ToString();

            objCommonPopUp.Type = "SELECT_CUSTOMER_CUSTOMERID";
            //objCommonPopUp.EmpCode = Membership.GetUser().UserName.ToString();
            //objCommonPopUp.Emp = Membership.GetUser().UserName.ToString();
            objCommonPopUp.CustomerId = Int32.Parse(strCustNo);

            dsuser = objCommonPopUp.GetCustomerOnCustomerId();
            if (dsuser.Tables[0].Rows.Count > 0)
            {             
              
            lblUserName.Text = dsuser.Tables[0].Rows[0]["FirstName"].ToString();
            lblPriPhone.Text = dsuser.Tables[0].Rows[0]["UniqueContact_No"].ToString();
            lblAltPhone.Text = dsuser.Tables[0].Rows[0]["AltTelNumber"].ToString();
            lblEmail.Text = dsuser.Tables[0].Rows[0]["Email"].ToString();
            if (dsuser.Tables[0].Rows[0]["Extension"].ToString() != "0")
            { 
                lblExt.Text = dsuser.Tables[0].Rows[0]["Extension"].ToString();
            }
            lblFax.Text = dsuser.Tables[0].Rows[0]["Fax"].ToString();
            lblAddress.Text = dsuser.Tables[0].Rows[0]["Address1"].ToString();
            lblAddress2.Text = dsuser.Tables[0].Rows[0]["Address2"].ToString();
            lblLandmark.Text = dsuser.Tables[0].Rows[0]["Landmark"].ToString();
            lblCountry.Text = dsuser.Tables[0].Rows[0]["Country_Desc"].ToString();
            lblState.Text = dsuser.Tables[0].Rows[0]["State_Desc"].ToString();
            lblCity.Text = dsuser.Tables[0].Rows[0]["City_Desc"].ToString();
            lblPinCode.Text = dsuser.Tables[0].Rows[0]["PinCode"].ToString();
            lblCompany.Text = dsuser.Tables[0].Rows[0]["Company_name"].ToString();
            }
            //gvHistory.DataSource =
            //gvHistory.DataBind();
            if (objCommonPopUp.ReturnValue == -1)
            {
                //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
                // trace, error message 
                CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), objCommonPopUp.MessageOut);
            }
        }
        catch (Exception ex)
        {
            //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
}

