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
using System.Web.Services;

public partial class pages_MTOComplainDetails : System.Web.UI.Page
{
    #region Global Variables and Objects

    CommonClass objCommonClass = new CommonClass();
    SqlDataAccessLayer objSqlDataAccessLayer = new SqlDataAccessLayer();
    MTOComplainDetails objServiceContractor = new MTOComplainDetails();
    RequestRegistration_MTO objRequestReg = new RequestRegistration_MTO();
    DataTable dtTemp = new DataTable();
    DataTable dtTempDefect = new DataTable();
    DataSet ds = new DataSet();
    ListItem lstItem = new ListItem("Select", "0");
    int j = 0;
    string strVar = "";
    string strSpareCharges = "";
    string strTravelCharges = "";
    string strSms = "";
    string strFullSms = "";
    string atrAppflag = "";

    #endregion Global Variables and Objects

    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ddlStatus.Attributes.Add("OnChange", "javascript:return DisAppDDl();");
            //Add New Code By Binay-03-12-2009
            objServiceContractor.userName = Membership.GetUser().UserName.ToString();
            //End
            if (!IsPostBack)
            {
                GetUserType();
                if (objServiceContractor.UserType != 3)
                {
                    trchkSelfAllocatopn.Visible = true;
                    gvFresh.Columns[9].Visible = false;
                    gvFresh.Columns[10].Visible = true;
                }
                else
                {
                    trchkSelfAllocatopn.Visible = false;
                    gvFresh.Columns[10].Visible = false;
                    gvFresh.Columns[9].Visible = true;
                }
                //Logic For Paging Gv Fresh////
                ViewState["FirstRow"] = 1;
                ViewState["LastRow"] = 10;
                ViewState["PageLoadFlag"] = true;
                ///////////////////////////////

                // Logic for the batch code validation
                DataSet dsBatch = objServiceContractor.GetBatch();
                if (dsBatch.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow drw in dsBatch.Tables[0].Rows)
                    {
                        hdnValidBatch.Value = hdnValidBatch.Value + drw["Batch_Code"].ToString() + "|";
                    }
                }
                // To get the login SC's ID
                //GetSCNo();
                //To set all action time ddls
                SetAllActionTime();
                hdnGlobalDate.Value = DateTime.Today.Date.ToString("MM/dd/yyyy");//("MM/dd/yyyy");dd/MM/yyyy
                hdnDefectCurrentDate.Value = DateTime.Today.Date.ToString("MM/dd/yyyy");
                hdnActionCurrentDate.Value = DateTime.Today.Date.ToString("MM/dd/yyyy");
                hdnInitDate.Value = DateTime.Today.Date.ToString("MM/dd/yyyy");
                txtInitialActionDate.Text = DateTime.Today.Date.ToString("MM/dd/yyyy");
                btnAdd.Enabled = true;
                btnSave.Enabled = true;

                PageLoadSearch();

                if (gvFresh.Rows.Count == 0)
                    tbIntialization.Visible = false;
                else
                    tbIntialization.Visible = true;
                ViewState["Grid1"] = ds;
                objServiceContractor.BindSCCity(ddlCity);

                objServiceContractor.BindSCProductDivision(ddlSearchProductDivision);
                objServiceContractor.BindServiceEngineer(ddlServiceEngg);
                //New Code Binay(Bind CG Type User
                // objServiceContractor.BindCGEmployee_MTOComplaintDetails(ddlCGExcutive);//For CG User
                objServiceContractor.BindCGEmployee_MTOComplaintDetails(ddlCGExcutive);//For CG User
                //objServiceContractor.BindCGContract(ddlCGEng);
                objServiceContractor.BindCGContract_MTOComplaintDetails(ddlCGEng);
                //objServiceContractor.BindSCName(ddlSCName);
                objServiceContractor.BindSCName_MTOComplaintDetails(ddlSCName);
                //End
                //Change Flow Code By Binay-07-12-2009

                //End
                ViewState["SeviceEnggDetail"] = objServiceContractor.SeviceEnggDetail;
                //Temporary table creation for the temporary grid binding used in FIR add product and Add defect
                CreatTempTable();
                CreatTempDefectTable();

                if (gvFresh.Rows.Count != 0)
                {
                    tbIntialization.Visible = true;
                }
                else
                {
                    tbIntialization.Visible = false;
                }
                CountForSearchButton();
                if (int.Parse(lblRowCount.Text.ToString()) > 10)
                {
                    btnPre.Visible = true;
                    btnNxt.Visible = true;
                }
                else
                {
                    btnPre.Visible = false;
                    btnNxt.Visible = false;
                }
            }
            System.Threading.Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["AjaxPleaseWaitTime"]));
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    #endregion

    #region PageLoadSearch
    protected void PageLoadSearch()
    {

        //New Code Add By Binay Find UserType
        GetUserType();
        if (objServiceContractor.UserType != 3)
        {
            ddlOtherAction.Items.RemoveAt(1);
            ddlAllocateAction.Items.RemoveAt(1);
        }
        else
        {
            ddlOtherAction.Items.RemoveAt(2);
            ddlAllocateAction.Items.RemoveAt(2);
        }
        //End
        // To get the login SC's ID
        GetSCNo();
        //New Add By Binay For MTO
        objServiceContractor.userName = Membership.GetUser().UserName.ToString();
        //End
        //objServiceContractor.CallStatus = 2;
        //Code Modify Binay(MTO)

        objServiceContractor.CallStatus = 47;//47 For Complaint Logged Type Status
        //END
        objServiceContractor.City_SNo = 0;
        objServiceContractor.Territory_SNo = 0;
        objServiceContractor.AppointmentFlag = 0;
        objServiceContractor.PageLoad = "PageLoad";
        ds = objServiceContractor.BindCompGrid(gvFresh);

    }
    #endregion

    #region Search Area
    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(lstItem);
            if (ddlStage.SelectedIndex != 0)
            {
                objServiceContractor.CallStage = ddlStage.SelectedValue.ToString();
                objServiceContractor.BindStatusDdl(ddlStatus);
            }
        }
        catch (Exception ex)
        {
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            //New Code Add By Binay Find UserType
            trCGExecutiveType.Visible = false;
            ddlCGExType.SelectedIndex = 0;
            RFVCGExType.Enabled = false;
            trInitEngineer.Visible = false;
            ddlServiceEngg.SelectedIndex = 0;
            trCGExecutive.Visible = false;
            ddlCGExcutive.SelectedIndex = 0;
            trCGEngineer.Visible = false;
            ddlCGEng.SelectedIndex = 0;
            trSCName.Visible = false;
            ddlSCName.SelectedIndex = 0;
            cmpDdlEngg.Enabled = false;
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;
            txtInitializationActionRemarks.Text = "";
            //End

            tbLifted.Visible = false;
            //Logic For Paging Gv Fresh////
            ViewState["FirstRow"] = 1;
            ViewState["LastRow"] = 10;
            ViewState["PageLoadFlag"] = false;
            ///////////////////////////////
            SearchButton();
            CountForSearchButton();
            btnNxt.Enabled = true;
            btnPre.Enabled = true;
            if (int.Parse(lblRowCount.Text.ToString()) > 10)
            {
                btnPre.Visible = true;
                btnNxt.Visible = true;
            }
            else
            {
                btnPre.Visible = false;
                btnNxt.Visible = false;
            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    protected void SearchButton()
    {
        //To refresh View Defect Grid//////
        trInitAppointDateTime.Visible = false;
        trInitEngineer.Visible = false;
        trCGEngineer.Visible = false;
        gvViewDefects.DataSource = null;
        gvViewDefects.DataBind();
        ///////////////////////////////////
        lblMsg.Text = "";
        //To manage the visiblity of the action options according to the search critaria
        #region Initialization
        if (ddlStage.SelectedItem.Text.ToString() == "Initialization")
        {
            tbIntialization.Visible = true;
            if (ddlStatus.SelectedIndex != 0)
            {
                // if (int.Parse(ddlStatus.SelectedValue.ToString()) == 2 || int.Parse(ddlStatus.SelectedValue.ToString()) == 44)
                if (int.Parse(ddlStatus.SelectedValue.ToString()) == 43 || int.Parse(ddlStatus.SelectedValue.ToString()) == 44)
                {
                    if (ddlAppointment.SelectedValue.ToString() == "Y")
                    {
                        ddlTakeAppointment.Visible = true;
                        RequiredFieldValidatorddlTakeAppointment.Enabled = true;

                        ddlAllocateAction.Visible = false;
                        RequiredFieldValidatorddlAllocateAction.Enabled = false;

                        ddlOtherAction.Visible = false;
                        RequiredFieldValidatorddlOtherAction.Enabled = false;
                    }
                    else if (ddlAppointment.SelectedValue.ToString() == "N")
                    {

                        ddlAllocateAction.Visible = true;
                        RequiredFieldValidatorddlAllocateAction.Enabled = true;

                        ddlOtherAction.Visible = false;
                        RequiredFieldValidatorddlOtherAction.Enabled = false;
                        ddlTakeAppointment.Visible = false;
                        RequiredFieldValidatorddlTakeAppointment.Enabled = false;
                    }
                    else if (ddlAppointment.SelectedIndex == 0)
                    {

                        ddlAllocateAction.Visible = true;
                        RequiredFieldValidatorddlAllocateAction.Enabled = true;

                        ddlOtherAction.Visible = false;
                        RequiredFieldValidatorddlOtherAction.Enabled = false;
                        ddlTakeAppointment.Visible = false;
                        RequiredFieldValidatorddlTakeAppointment.Enabled = false;
                    }
                }
                else if (ddlAppointment.SelectedValue.ToString() == "Y")
                {
                    ddlTakeAppointment.Visible = true;
                    RequiredFieldValidatorddlTakeAppointment.Enabled = true;

                    ddlAllocateAction.Visible = false;
                    RequiredFieldValidatorddlAllocateAction.Enabled = false;
                    ddlOtherAction.Visible = false;
                    RequiredFieldValidatorddlOtherAction.Enabled = false;
                }
                else
                {
                    ddlOtherAction.Visible = true;
                    RequiredFieldValidatorddlOtherAction.Enabled = true;

                    ddlAllocateAction.Visible = false;
                    RequiredFieldValidatorddlAllocateAction.Enabled = false;
                    ddlTakeAppointment.Visible = false;
                    RequiredFieldValidatorddlTakeAppointment.Enabled = false;
                }
            }
            else if (ddlStatus.SelectedIndex == 0)
            {
                tbIntialization.Visible = false;
            }
            else
            {
                if (ddlAppointment.SelectedValue.ToString() == "Y")
                {
                    ddlTakeAppointment.Visible = true;
                    RequiredFieldValidatorddlTakeAppointment.Enabled = true;

                    ddlAllocateAction.Visible = false;
                    RequiredFieldValidatorddlAllocateAction.Enabled = false;
                    ddlOtherAction.Visible = false;
                    RequiredFieldValidatorddlOtherAction.Enabled = false;
                }
            }

        }
        #endregion Initialization
        else if (ddlStage.SelectedItem.Text.ToString() == "WIP" || ddlStage.SelectedItem.Text.ToString() == "Closure" || ddlStage.SelectedItem.Text.ToString() == "TempClosed")
        {
            objServiceContractor.WipFlag = ddlStage.SelectedItem.Text.ToString();
            tbIntialization.Visible = false;
        }
        else
        {
            tbIntialization.Visible = false;
        }
        tbDefect.Visible = false;
        tbAction.Visible = false;
        tbBasicRegistrationInformation.Visible = false;
        tbViewAttribute.Visible = false;
        gvCustDetail.Visible = false;
        tbTempGrid.Visible = false;
        gvCustDetail.Visible = false;

        BindGvFreshOnSearchBtnClick();
    }

    protected void CountForSearchButton()
    {

        if (ddlStage.SelectedItem.Text.ToString() == "WIP" || ddlStage.SelectedItem.Text.ToString() == "Closure" || ddlStage.SelectedItem.Text.ToString() == "TempClosed")
        {
            objServiceContractor.WipFlag = ddlStage.SelectedItem.Text.ToString();

        }

        CountGvFreshOnSearchBtnClick();
    }
    protected void CountGvFreshOnSearchBtnClick()
    {
        try
        {
            ////New Add By Binay For MTO         
            GetSCNo();
            objServiceContractor.Complaint_RefNo = "";
            objServiceContractor.Stage = "";
            objServiceContractor.CallStatus = 0;
            objServiceContractor.City_SNo = 0;
            objServiceContractor.Territory_SNo = 0;
            objServiceContractor.AppointmentFlag = 0;
            objServiceContractor.ProductDivision_Sno = 0;
            if (txtComplaintRefNo.Text != "")
            {
                objServiceContractor.Complaint_RefNo = txtComplaintRefNo.Text.ToString();
                objServiceContractor.txtComplaint = "SHOWALLSPLIT";
                tbIntialization.Visible = true;
            }
            else
            {
                if (ddlStage.SelectedIndex != 0)
                {
                    objServiceContractor.Stage = ddlStage.SelectedValue.ToString();
                }
                if (ddlStatus.SelectedIndex != 0)
                {
                    objServiceContractor.CallStatus = int.Parse(ddlStatus.SelectedValue.ToString());
                }
                if (ddlSearchProductDivision.SelectedIndex != 0)
                {
                    objServiceContractor.ProductDivision_Sno = int.Parse(ddlSearchProductDivision.SelectedValue.ToString());
                }
                if (ddlCity.SelectedIndex != 0)
                {
                    objServiceContractor.City_SNo = int.Parse(ddlCity.SelectedValue.ToString());
                    objServiceContractor.State_SNo = 0;
                }
                if (ddlTeritory.SelectedIndex != 0)
                {
                    objServiceContractor.Territory_SNo = int.Parse(ddlTeritory.SelectedValue.ToString());
                    objServiceContractor.City_SNo = 0;
                }
                if (ddlAppointment.SelectedIndex != 0)
                {
                    objServiceContractor.AppointmentFlag = 1;
                    if (ddlAppointment.SelectedIndex == 1)
                        objServiceContractor.AppointmentReq = true;
                    else
                        objServiceContractor.AppointmentReq = false;
                }
                if (ddlSrf.SelectedIndex != 0)
                {
                    objServiceContractor.SRF = ddlSrf.SelectedValue.ToString();
                }
            }

            DataSet ds = objServiceContractor.CountCompGrid();

            lblRowCount.Text = ds.Tables[0].Rows[0]["Tot"].ToString();
        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }

    //New Code By Binay(MTO)
    protected void ddlCGExType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (int.Parse(ddlCGExType.SelectedValue.ToString()) == 2)
        {
            //CG Excutive(CG)
            trCGExecutive.Visible = true;
            trCGEngineer.Visible = false;
            rfvCGEng.Enabled = false;
            if (chkSelfAllocatopn.Checked)
            {
                RFVCGEmployee.Enabled = false;
            }
            else
            {
                RFVCGEmployee.Enabled = true;
            }

        }
        else if (int.Parse(ddlCGExType.SelectedValue.ToString()) == 5)
        {
            //CG Engg.(CG Contracted Employee)
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = true;
            RFVCGEmployee.Enabled = false;

            if (chkSelfAllocatopn.Checked)
            {
                rfvCGEng.Enabled = false;
            }
            else
            {
                rfvCGEng.Enabled = true;
            }
        }
        else
        {
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;
        }
    }
    //End
    protected void ddlAllocateAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        //To make engineer list visible 
        //when allocation or reallocation is required
        if (int.Parse(ddlAllocateAction.SelectedValue.ToString()) == 42)
        {
            trInitEngineer.Visible = true;
            cmpDdlEngg.Enabled = true;
            ddlServiceEngg.SelectedIndex = 0;
            txtInitializationActionRemarks.Text = "";
        }
        else
        {
            trInitEngineer.Visible = false;
            cmpDdlEngg.Enabled = false;
        }
        if (int.Parse(ddlAllocateAction.SelectedValue.ToString()) == 41)
        {
            trCGExecutiveType.Visible = true;
            RFVCGExType.Enabled = true;
            ddlCGEng.SelectedIndex = 0;
            ddlCGExcutive.SelectedIndex = 0;
            ddlCGExType.SelectedIndex = 0;
            chkSelfAllocatopn.Checked = false;
            txtInitializationActionRemarks.Text = "";

            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
            trSCName.Visible = false;
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;

        }
        else if (int.Parse(ddlAllocateAction.SelectedValue.ToString()) == 43)
        {
            trSCName.Visible = true;
            rfvSCName.Enabled = true;
            ddlSCName.SelectedIndex = 0;
            //Other Control Visible Fale
            trCGExecutiveType.Visible = false;
            RFVCGExType.Enabled = false;
            trInitEngineer.Visible = false;
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
            cmpDdlEngg.Enabled = false;
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;
            txtInitializationActionRemarks.Text = "";

        }
        else
        {
            trSCName.Visible = false;
            rfvSCName.Enabled = false;
            trCGExecutiveType.Visible = false;
            RFVCGExType.Enabled = false;

            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;
        }
        if (int.Parse(ddlAllocateAction.SelectedValue.ToString()) == 44)
        {
            trInitAppointDateTime.Visible = true;
            txtInitializationActionRemarks.Text = "";

        }
        else
        {
            trInitAppointDateTime.Visible = false;

        }


    }
    protected void ddlTakeAppointment_SelectedIndexChanged(object sender, EventArgs e)
    {
        //To handle visiblity of the appointment date time option visible 
        //when take appointment action is being performed
        if (int.Parse(ddlTakeAppointment.SelectedValue.ToString()) == 44)
        {
            trInitAppointDateTime.Visible = true;

        }
        else
        {
            trInitAppointDateTime.Visible = false;
        }
        if (int.Parse(ddlTakeAppointment.SelectedValue.ToString()) == 41)
        {
            trCGEngineer.Visible = true;

        }
        else
        {
            trCGEngineer.Visible = false;
        }
    }
    protected void ddlOtherAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (int.Parse(ddlOtherAction.SelectedValue.ToString()) == 4)
        //Code By Binay
        if (int.Parse(ddlOtherAction.SelectedValue.ToString()) == 42)
        {
            trInitEngineer.Visible = true;
            cmpDdlEngg.Enabled = true;
            //Othere Control Visible False
            trCGExecutiveType.Visible = false;
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
            trSCName.Visible = false;

            //ddlSCName.SelectedIndex = 0;
            //ddlCGExType.SelectedIndex = 0;
            //ddlCGExcutive.SelectedIndex = 0;
            //ddlCGEng.SelectedIndex = 0;

            RFVCGExType.Enabled = true;
            rfvSCName.Enabled = false;
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;

        }
        else if (int.Parse(ddlOtherAction.SelectedValue.ToString()) == 43)
        {
            trSCName.Visible = true;
            rfvSCName.Enabled = true;
            trInitEngineer.Visible = false;
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
            trCGExecutiveType.Visible = false;

            RFVCGExType.Enabled = false;
            cmpDdlEngg.Enabled = false;
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;

        }
        else if (int.Parse(ddlOtherAction.SelectedValue.ToString()) == 41)
        {
            trCGExecutiveType.Visible = true;
            RFVCGExType.Enabled = true;
            //Other Control Visible Fale
            trInitEngineer.Visible = false;
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
            trSCName.Visible = false;


            cmpDdlEngg.Enabled = false;
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;
            rfvSCName.Enabled = false;

        }
        else
        {
            trInitEngineer.Visible = false;
            trSCName.Visible = false;
            trCGExecutiveType.Visible = false;
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;

            rfvCGEng.Enabled = false;
            RFVCGEmployee.Enabled = false;
            RFVCGExType.Enabled = false;
            rfvSCName.Enabled = false;
            cmpDdlEngg.Enabled = false;


        }

    }
    protected void BindGvFreshOnSearchBtnClick()
    {
        try
        {
            GetSCNo();
            objServiceContractor.userName = Membership.GetUser().UserName.ToString();
            objServiceContractor.Complaint_RefNo = "";
            objServiceContractor.Stage = "";
            objServiceContractor.CallStatus = 0;
            objServiceContractor.City_SNo = 0;
            objServiceContractor.Territory_SNo = 0;
            objServiceContractor.AppointmentFlag = 0;
            objServiceContractor.ProductDivision_Sno = 0;
            if (txtComplaintRefNo.Text != "")
            {
                objServiceContractor.Complaint_RefNo = txtComplaintRefNo.Text.ToString();
                objServiceContractor.txtComplaint = "SHOWALLSPLIT";
                tbIntialization.Visible = true;
            }
            else
            {
                if (ddlStage.SelectedIndex != 0)
                {
                    objServiceContractor.Stage = ddlStage.SelectedValue.ToString();
                }
                if (ddlStatus.SelectedIndex != 0)
                {
                    objServiceContractor.CallStatus = int.Parse(ddlStatus.SelectedValue.ToString());
                }
                if (ddlSearchProductDivision.SelectedIndex != 0)
                {
                    objServiceContractor.ProductDivision_Sno = int.Parse(ddlSearchProductDivision.SelectedValue.ToString());
                }
                if (ddlCity.SelectedIndex != 0)
                {
                    objServiceContractor.City_SNo = int.Parse(ddlCity.SelectedValue.ToString());
                    objServiceContractor.State_SNo = 0;
                }
                if (ddlTeritory.SelectedIndex != 0)
                {
                    objServiceContractor.Territory_SNo = int.Parse(ddlTeritory.SelectedValue.ToString());
                    objServiceContractor.City_SNo = 0;
                }
                if (ddlAppointment.SelectedIndex != 0)
                {
                    objServiceContractor.AppointmentFlag = 1;
                    if (ddlAppointment.SelectedIndex == 1)
                        objServiceContractor.AppointmentReq = true;
                    else
                        objServiceContractor.AppointmentReq = false;
                }
                if (ddlSrf.SelectedIndex != 0)
                {
                    objServiceContractor.SRF = ddlSrf.SelectedValue.ToString();
                }
            }

            ////New Add By Binay For MTO         
            DataSet ds = objServiceContractor.BindCompGrid(gvFresh);
            ViewState["Grid1"] = ds;
            //lblRowCount.Text = ds.Tables[0].Rows.Count.ToString();
            if (gvFresh.Rows.Count == 0)
                tbIntialization.Visible = false;
        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }
    #endregion

    #region Grid No 1 Area
    // Button for the initialization section action
    protected void btnAllocate_Click(object sender, EventArgs e)
    {
        try
        {


            lblMsg.Text = "";
            GetSCNo();
            int count = 0;
            #region For Loop
            foreach (GridViewRow gvRow in gvFresh.Rows)
            {
                if (((CheckBox)gvRow.FindControl("chkChild")).Checked == true)
                {
                    objServiceContractor.BaseLineId = int.Parse(((HiddenField)gvRow.FindControl("hdnBaselineID")).Value.ToString());
                    objServiceContractor.Complaint_RefNo = ((HiddenField)gvRow.FindControl("hdnComplaint_RefNo")).Value.ToString();
                    if (ddlTakeAppointment.Visible == true)
                    {
                        if (ddlTakeAppointment.SelectedIndex != 0)
                            objServiceContractor.CallStatus = int.Parse(ddlTakeAppointment.SelectedValue.ToString());
                    }
                    else if (ddlAllocateAction.Visible == true)
                    {
                        if (ddlAllocateAction.SelectedIndex != 0)
                            objServiceContractor.CallStatus = int.Parse(ddlAllocateAction.SelectedValue.ToString());

                    }
                    else if (ddlOtherAction.Visible == true)
                    {
                        if (ddlOtherAction.SelectedIndex != 0)
                            objServiceContractor.CallStatus = int.Parse(ddlOtherAction.SelectedValue.ToString());
                    }

                    if (chkSelfAllocatopn.Checked)
                    {
                        objServiceContractor.CallStatus = 41;
                    }
                    if (ddlServiceEngg.SelectedIndex != 0)
                    {
                        objServiceContractor.ServiceEng_SNo = int.Parse(ddlServiceEngg.SelectedValue.ToString());
                    }
                    //New Code Add By Binay
                    if (ddlCGExType.SelectedIndex != 0)
                    {
                        objServiceContractor.CGType = int.Parse(ddlCGExType.SelectedValue.ToString());
                    }
                    if (ddlCGExcutive.SelectedIndex != 0)
                    {
                        objServiceContractor.CG = ddlCGExcutive.SelectedValue.ToString();
                    }
                    if (ddlCGEng.SelectedIndex != 0)
                    {
                        objServiceContractor.CG = ddlCGEng.SelectedValue.ToString();
                    }
                    // End

                   
                    objServiceContractor.LastComment = txtInitializationActionRemarks.Text;
                    //Modify By Binay--23/11/2009  
                  
                        objServiceContractor.EmpID = Membership.GetUser().UserName.ToString();
                    
                    //End
                    //if (objServiceContractor.CallStatus == 4 || objServiceContractor.CallStatus == 5)
                    //Code Modify By Binay(MTO)
                        if (objServiceContractor.CallStatus == 42 || objServiceContractor.CallStatus == 41 || objServiceContractor.CallStatus == 43)
                        //Add New Code By Binay
                        if (chkSelfAllocatopn.Checked)
                        {                          
                            GetUserType();                            
                            objServiceContractor.CGType = objServiceContractor.UserType;
                            objServiceContractor.CG = Membership.GetUser().UserName.ToString();
                            objServiceContractor.InsertAllocation();
                        }
                        else if(ddlActionStatus.SelectedValue.ToString()=="41")
                        {
                            GetUserType();                           
                            objServiceContractor.CGType = objServiceContractor.UserType;
                            objServiceContractor.CG = Membership.GetUser().UserName.ToString();
                            objServiceContractor.InsertAllocation();
                        }
                        else
                        {
                           
                            //Add New Code By Binay-23-11-2009
                            if (ddlSCName.SelectedIndex != 0)
                            {
                                objServiceContractor.GetSC_ID(Convert.ToInt32(ddlSCName.SelectedValue.ToString()));
                                objServiceContractor.SC_SNo = Convert.ToInt32(ddlSCName.SelectedValue.ToString());
                                objServiceContractor.CG = objServiceContractor.userName;
                                objServiceContractor.CGType = objServiceContractor.UserType;
                                objServiceContractor.InsertAllocation();
                            }
                            else
                            {
                                objServiceContractor.InsertAllocation();
                            }
                        }
                        //End
                    if (txtInitialActionDate.Text != "")
                        objServiceContractor.ActionDate = Convert.ToDateTime(txtInitialActionDate.Text.ToString());
                    GetActionTime(ddlInitHr, ddlInitMin, ddlInitAm);
                                  
                    //Modify Binay(MTO)
                     if (objServiceContractor.CallStatus == 44)
                    {
                        GetAppTime(ddlInitAppHr, ddlInitAppMin, ddlInitAppAm);
                        objServiceContractor.SLADate = Convert.ToDateTime(txtAppointMentDate.Text.ToString());
                        objServiceContractor.UpdateCallStatusOnApp();
                    }
                    else
                    {
                        objServiceContractor.UpdateCallStatus();

                        ///Send SMS on allocation and reallocation
                        if (((HiddenField)gvRow.FindControl("hdnAppointmentFlag")).Value.ToString() == "True")
                        {
                            atrAppflag = "Y";
                        }
                        else
                        {
                            atrAppflag = "N";
                        }
                        strFullSms = objServiceContractor.Complaint_RefNo + "-" + ((HiddenField)gvRow.FindControl("hdnUnit_Desc")).Value.ToString() + "-" + atrAppflag + "-" + ((HiddenField)gvRow.FindControl("gvFreshhdnLastName")).Value.ToString() + "-" + ((HiddenField)gvRow.FindControl("gvFreshhdnFullAddress")).Value.ToString() + "-" + txtInitializationActionRemarks.Text.Trim();
                        strSms = strFullSms;
                        if (strFullSms.Length > 157)
                            strSms = strFullSms.Substring(0, 158);
                        if (chkSMS.Checked)
                        {
                           // if (objServiceContractor.CallStatus == 42 || objServiceContractor.CallStatus == 4)
                            //Code Modify Binay(MTO)
                           if (objServiceContractor.CallStatus == 42 )
                            {
                                DataRow[] drtemp;
                                string strVaildNumber = "";
                                objServiceContractor.SeviceEnggDetail = (DataSet)ViewState["SeviceEnggDetail"];

                                drtemp = objServiceContractor.SeviceEnggDetail.Tables[0].Select("ServiceEng_SNo=" + ddlServiceEngg.SelectedValue.ToString());
                                if (drtemp.Length > 0)
                                {
                                    if (CommonClass.ValidateMobileNumber(drtemp[0]["PhoneNo"].ToString(), ref strVaildNumber))
                                    {
                                        //CommonClass.SendSMS(strVaildNumber, objServiceContractor.Complaint_RefNo, ddlServiceEngg.SelectedValue.ToString(), DateTime.Now.Date.ToString("yyyyMMdd"), "CGL", strSms, strFullSms, "ASC");
                                    }
                                }
                                drtemp = null;
                            }
                            //End
                        }
                    }
                    count = count + 1;
                }
            }
            #endregion For Loop
            // Refreshing the grid upon taking the Action
            if (ddlStage.SelectedIndex == 0 && ddlStatus.SelectedIndex == 0 && ddlAppointment.SelectedIndex == 0
               && ddlCity.SelectedIndex == 0 && ddlSearchProductDivision.SelectedIndex == 0 && txtComplaintRefNo.Text == "")
            {
                objServiceContractor.Complaint_RefNo = "";
                objServiceContractor.BaseLineId = 0;

                objServiceContractor.CallStatus = 42;
                GetUserType();
                if (objServiceContractor.UserType != 3)
                {
                    objServiceContractor.SC_SNo = 0;
                }
                //END
                objServiceContractor.City_SNo = 0;
                objServiceContractor.Territory_SNo = 0;
                objServiceContractor.AppointmentFlag = 0;
                objServiceContractor.PageLoad = "PageLoad";
                //New Add By Binay For MTO
                objServiceContractor.userName = Membership.GetUser().UserName.ToString();
                //End
                DataSet ds = objServiceContractor.BindCompGrid(gvFresh);
                gvFresh.DataSource = ds;
                gvFresh.DataBind();
                if (gvFresh.Rows.Count == 0)
                    tbIntialization.Visible = false;
                ViewState["Grid1"] = ds;
            }
            else
            {
                BindGvFreshOnSearchBtnClick();
                if (gvFresh.Rows.Count == 0)
                    tbIntialization.Visible = false;
            }
            CountForSearchButton();
            if (count != 0)
            {
                ScriptManager.RegisterClientScriptBlock(btnAllocate, GetType(), "NoCompSelected2", "alert('" + count + " Complaint(s) Allocated');", true);
                ClearinitializeCotrols();
            }
            if (count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(btnAllocate, GetType(), "NoCompSelected", "alert('No Complaint Selected to Proceed');", true);
            }

        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    protected void ClearinitializeCotrols()
    {
        ddlAllocateAction.SelectedIndex = 0;
        ddlServiceEngg.SelectedIndex = 0;
        ddlCGExType.SelectedIndex = 0;
        ddlCGExcutive.SelectedIndex = 0;
        ddlCGEng.SelectedIndex = 0;
        chkSelfAllocatopn.Checked = false;
        ddlAllocateAction.SelectedIndex = 0;
        if (ddlAllocateAction.SelectedIndex == 0)
        {
            trCGExecutiveType.Visible = false;
            RFVCGExType.Enabled = false;
            trInitEngineer.Visible = false;
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
            cmpDdlEngg.Enabled = false;
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;
            txtInitializationActionRemarks.Text = "";
        }
        ddlTakeAppointment.SelectedIndex = 0;
        ddlOtherAction.SelectedIndex = 0;
        txtInitializationActionRemarks.Text = "";
    }
    protected void btnInitialiseClose_Click(object sender, EventArgs e)
    {
        try
        {
            ClearinitializeCotrols();
            tbIntialization.Visible = false;
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }

    //Grid no. 1 Next btn Click
    protected void lnkBtnNext_Click(object sender, EventArgs e)
    {
        try
        {
            txtFirDate.Text = DateTime.Today.Date.ToString("MM/dd/yyyy");
             ClearFirControls();
            //Add Cod By Binay MTO--hide Control according to click next Button
            trInitEngineer.Visible = false;
            trSCName.Visible = false;
            trCGExecutiveType.Visible = false;
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
            rfvCGEng.Enabled = false;
            RFVCGEmployee.Enabled = false;
            RFVCGExType.Enabled = false;
            rfvSCName.Enabled = false;
            cmpDdlEngg.Enabled = false;
            //End

            lblMsg.Text = "";
            lblSave.Text = "";
            btnSaveDefect.Enabled = true;
            tbTempGrid.Visible = false;
            tbViewAttribute.Visible = false;
            GetSCNo();
            ds = (DataSet)ViewState["Grid1"];
            LinkButton lnk = (LinkButton)sender;
            objServiceContractor.Complaint_RefNo = lnk.CommandArgument.ToString();
            objServiceContractor.BaseLineId = int.Parse(lnk.ToolTip.ToString());
            int callStatus = int.Parse(lnk.CommandName.ToString());
            //if (callStatus == 4 || callStatus == 4 || callStatus == 12 || callStatus == 13 || callStatus == 17 || callStatus == 18 || callStatus == 44 || callStatus == 31 || callStatus == 33)
            //Code Modify By Binay(MTO)
            if (callStatus == 41 || callStatus == 42 || callStatus == 43 || callStatus == 44 || callStatus == 46 || callStatus == 47 || callStatus == 48 || callStatus == 49)
            {
                // to decide which ddl to show in initialise tab

               // if (callStatus != 44 && callStatus != 33)
               if (callStatus != 43)
                {
                    ddlOtherAction.Visible = true;
                    ddlAllocateAction.Visible = false;
                    trInitEngineer.Visible = false;

                    //ddlServiceEngg.Visible = false;
                }
                else if (callStatus == 44)
                {
                    ddlOtherAction.Visible = false;
                    ddlTakeAppointment.Visible = false;
                    ddlAllocateAction.Visible = true;
                    trInitEngineer.Visible = false;
                    //ddlServiceEngg.Visible = false;
                }               

                gvCustDetail.Visible = false;
                ds = (DataSet)ViewState["Grid1"];
                if (ds.Tables[0].Rows.Count != 0)
                {
                    tbBasicRegistrationInformation.Visible = true;
                    tbTempGrid.Visible = true;
                    gvCustDetail.Visible = false;
                    tbIntialization.Visible = true;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (objServiceContractor.Complaint_RefNo == (ds.Tables[0].Rows[i]["Complaint_RefNo"]).ToString())
                        {
                            objServiceContractor.ProductDivision_Sno = int.Parse((ds.Tables[0].Rows[i]["ProductDivision_Sno"]).ToString());
                            objServiceContractor.BindMfgDdl(ddlMfgUnit); 
                           //Add Code By Binay-25-11-2009
                            if (ddlMfgUnit.Items.Count == 0)
                            {

                                objServiceContractor.Manufacture_Unit = (ds.Tables[0].Rows[i]["Unit_Desc"]).ToString();
                                objServiceContractor.InsertManifature();
                                objServiceContractor.BindMfgDdl(ddlMfgUnit);
                            }
                            //End
                            lblUnit.Text = (ds.Tables[0].Rows[i]["Unit_Desc"]).ToString();
                            //Binding the Product Division ddl in FIR section
                            //according to the city , state and sc
                            hdnProductDvNo.Value = (ds.Tables[0].Rows[i]["ProductDivision_Sno"]).ToString();
                            hdnFirState.Value = (ds.Tables[0].Rows[i]["State_Sno"]).ToString();
                            hdnFirCity.Value = (ds.Tables[0].Rows[i]["City_SNo"]).ToString();
                            txtInvoiceNo.Text = ds.Tables[0].Rows[i]["InvoiceNo"].ToString();
                            string ProductlineSno = ds.Tables[0].Rows[i]["ProductLine_Sno"].ToString();
                            txtCommissionDate.Text = Convert.ToString(ds.Tables[0].Rows[i]["DtofCommission"]);
                            // RSD change MTO
                            if (hdnProductDvNo.Value == "31") // 31 live,53 test
                            {
                                tr1.Visible = true;
                                tr2.Visible = true;
                                txtcoachNo.Text = ds.Tables[0].Rows[i]["coachNo"].ToString();
                                txtTrainNo.Text = ds.Tables[0].Rows[i]["TrainNo"].ToString();
                                txtAvailblty.Text = ds.Tables[0].Rows[i]["AvailabilityDepot"].ToString();
                                txtequipname.Text = ds.Tables[0].Rows[i]["EquipmentName"].ToString();
                            }
                            else
                            {
                                tr1.Visible = false;
                                tr2.Visible = false;
                            }

                            if (txtInvoiceNo.Text != "")
                            {
                                objRequestReg.Invoice_No = txtInvoiceNo.Text;
                                objRequestReg.Type = "SELECT_CUSTOMER_ON_INVOICE";
                                DataSet dsSAP =new DataSet();
                                dsSAP = objRequestReg.GetPartyCodeOnInvoiceNoProductSRNo();

                                if (dsSAP.Tables[0].Rows.Count > 0)
                                {
                                    objRequestReg.LocCode = dsSAP.Tables[0].Rows[0]["LocCode"].ToString();
                                    objRequestReg.Materialcode =  dsSAP.Tables[0].Rows[0]["Materialcode"].ToString();
                                    
                                    objRequestReg.BindProductDivision(ddlProductDiv);

                                    if (ddlProductDiv.Items.Count != 1)
                                    {
                                        ddlProductDiv.Items.RemoveAt(0);
                                    }

                                    if (ddlProductDiv.SelectedValue != "0")
                                    {
                                        objRequestReg.BindProduct_Line(ddlProductLine, Convert.ToInt32(ddlProductDiv.SelectedValue.ToString()));

                                        if (int.Parse(ProductlineSno) != 0)
                                        {
                                            if (ddlProductLine.Items.Count != 0)
                                            {
                                                for (j = 0; j < ddlProductLine.Items.Count; j++)
                                                {
                                                    if (ddlProductLine.Items[j].Value == (ProductlineSno))
                                                        ddlProductLine.SelectedIndex = j;
                                                 
                                                }
                                            }

                                        }

                                        if (ddlProductLine.SelectedValue != "")
                                        {
                                            objRequestReg.BindProduct_Detail(ddlProduct, Convert.ToInt32(ddlProductLine.SelectedValue.ToString()), objRequestReg.Materialcode);
                                        }
                                    }
                                }
                                else
                                {
                                    objServiceContractor.BindProductLineDdl(ddlProductLine);
                                    try
                                    {
                                        if (int.Parse((ds.Tables[0].Rows[i]["ProductLine_Sno"]).ToString()) != 0)
                                        {
                                            ddlProductLine.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[i]["ProductLine_Sno"])).Selected = true;
                                        }
                                    }
                                    catch (Exception e1)
                                    { 
                                       
                                    }

                                    ddlProductLine_SelectedIndexChanged(ddlProductLine, null);
                                    try
                                    {
                                        if (int.Parse((ds.Tables[0].Rows[i]["Product_Sno"]).ToString()) != 0)
                                        {
                                           ddlProduct.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[i]["Product_Sno"])).Selected = true;
                                        }
                                    }
                                    catch (Exception e2)
                                    {

                                    }
                                } // End of --if (ds.Tables[0].Rows.Count > 0)
                            } 
                            else
                            {
                                objServiceContractor.BindProductLineDdl(ddlProductLine);
                                try
                                { 
                                    if (int.Parse((ds.Tables[0].Rows[i]["ProductLine_Sno"]).ToString()) != 0)
                                    {
                                    ddlProductLine.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[i]["ProductLine_Sno"])).Selected = true;

                                     }
                                   
                                }
                                catch (Exception e1)
                                {

                                }
                               
                               ddlProductLine_SelectedIndexChanged(ddlProductLine, null);

                                try
                                {
                                    if (int.Parse((ds.Tables[0].Rows[i]["Product_Sno"]).ToString()) != 0)
                                    {
                                        ddlProduct.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[i]["Product_Sno"])).Selected = true;
                                    }
                                }
                                catch (Exception e2)
                                {

                                }

                            }
                            //End Code
                            hdnNatureOfComplaint.Value = (ds.Tables[0].Rows[i]["NatureOfComplaint"]).ToString();
                            lblComplainDate.Text = ds.Tables[0].Rows[i]["ComplaintDate"].ToString();
                            lblComplainNo.Text = ds.Tables[0].Rows[i]["Complaint_RefNo"].ToString();
                            hdnComplaintRef.Value = ds.Tables[0].Rows[i]["Complaint_RefNo"].ToString();
                           // hdnBaseLineID.Value = lnk.CommandArgument.ToString();
                            hdnBaseLineID.Value = ds.Tables[0].Rows[i]["BaseLineId"].ToString();
                            hdnCustmerID.Value = ds.Tables[0].Rows[i]["CustomerID"].ToString();
                            hdnCallStatus.Value = ds.Tables[0].Rows[i]["CallStatus"].ToString();
                              txtProductRefNo.Text = ds.Tables[0].Rows[i]["Product_SRNo"].ToString();
                                txtWarranty.Text = Convert.ToString(ds.Tables[0].Rows[i]["Natureofcomplaint"]); //?

                            txtInvoiceDate.Text = Convert.ToString(ds.Tables[0].Rows[i]["InvoiceDate"]);
                            txtDispatchDate.Text = Convert.ToString(ds.Tables[0].Rows[i]["DispatchDate"]);
                            hdnSLADate.Value = Convert.ToString(ds.Tables[0].Rows[i]["SLADate"]);
                            txtPoDate.Text = Convert.ToString(ds.Tables[0].Rows[i]["ManufactureDate"]);
                            txtWarrantyDate.Text = Convert.ToString(ds.Tables[0].Rows[i]["WarrantyExpirydate"]); // ??
                            txtCompalaintDate.Text = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[i]["SLADATE"])).ToString("d");
                            txtCommissionDate.Text = Convert.ToString(ds.Tables[0].Rows[i]["DtofCommission"]); 
                           
                        }
                    }
                }
            }
            // Wip Section

            else if (callStatus == 50 || callStatus == 51 || callStatus == 52 || callStatus == 53 || callStatus == 54 || callStatus == 55 || callStatus == 56 || callStatus == 75)//Add New Status 75 callstatus-06-04-2010 by binay
            {
                gvCustDetail.Visible = true;
                tbBasicRegistrationInformation.Visible = false;
                tbIntialization.Visible = false;
                DataSet dsGvCust = objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvCustDetail);
                ViewState["dsGvCust"] = dsGvCust;

                ddlActionStatus.Items.Clear();
                ddlActionStatus.Items.Insert(0, new ListItem("Select", "0"));

                string DefectenteredFlag = objRequestReg.CheckDefectApproveFlagforComplaint(objServiceContractor.Complaint_RefNo.ToString());
                if (DefectenteredFlag != "y")
                {
                    ddlActionStatus.Items.Insert(1, new ListItem("(WIP)Repair at Division", "50"));
                    ddlActionStatus.Items.Insert(2, new ListItem("(WIP)Repair In Progress at Site", "52"));
                    ddlActionStatus.Items.Insert(3, new ListItem("(WIP)Waiting for Spares", "53"));
                    ddlActionStatus.Items.Insert(4, new ListItem("(WIP)Pending for replacement", "55"));
                    ddlActionStatus.Items.Insert(5, new ListItem("(WIP)Complaint disposed by temporary arrangement", "75"));    //Add By Binay 06-04-2010       
                }
                else
                {
                    ddlActionStatus.Items.Insert(1, new ListItem("(WIP)Repair at Division", "50"));
                    ddlActionStatus.Items.Insert(2, new ListItem("(WIP)Repair In Progress at Site", "52"));
                    ddlActionStatus.Items.Insert(3, new ListItem("(WIP)Waiting for Spares", "53"));
                    ddlActionStatus.Items.Insert(4, new ListItem("(WIP)Pending for replacement", "55"));
                    ddlActionStatus.Items.Insert(5, new ListItem("(WIP)Complaint disposed by temporary arrangement", "75"));  //Add By Binay 06-04-2010

                    ddlActionStatus.Items.Insert(6, new ListItem("(Closure)Resolved by Replacement", "57"));
                    ddlActionStatus.Items.Insert(7, new ListItem("(Closure)Resolved by Repair", "58"));
                    ddlActionStatus.Items.Insert(8, new ListItem("(Closure)Complaint Cancelled", "59"));
                    ddlActionStatus.Items.Insert(9, new ListItem("(Closure)Resolved by repair with replacement of spare", "60"));
                    ddlActionStatus.Items.Insert(10, new ListItem("(Closure)Complaint Resolved and Completed", "61"));
                }
            }
            //Equipment Lift Section
            //else if (callStatus == 32)
            //code by Modify Binay in MTO
            else if (callStatus == 60)
            {
                tbLifted.Visible = true;
                if (ds.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (objServiceContractor.BaseLineId == int.Parse((ds.Tables[0].Rows[i]["BaseLineId"]).ToString()))
                        {
                            eqlblComplaint.Text = ds.Tables[0].Rows[i]["Complaint_RefNo"].ToString() + "/" + ds.Tables[0].Rows[i]["split"].ToString();
                            eqphdnBaselineID.Value = (ds.Tables[0].Rows[i]["BaseLineId"]).ToString();
                        }
                    }
                }
            }
            else
            {
                tbIntialization.Visible = true;
                tbBasicRegistrationInformation.Visible = false;
                trInitEngineer.Visible = false;

                //ddlServiceEngg.Visible = false;
                //ddlInitTempClosedAction.Visible = false;
                ddlTakeAppointment.Visible = false;
                ddlOtherAction.Visible = true;
                ddlAllocateAction.Visible = false;
                gvCustDetail.Visible = false;
                lblMsg.Text = "";
                btnSave.Visible = false;

                tbLifted.Visible = false;
            }

            foreach (GridViewRow grv in gvFresh.Rows)
            {
                if (((HiddenField)grv.FindControl("hdnComplaint_RefNo")).Value.ToString() == lnk.CommandArgument.ToString())
                {
                    ((CheckBox)grv.FindControl("chkChild")).Checked = true;
                }
                else
                {
                    ((CheckBox)grv.FindControl("chkChild")).Checked = false;
                }
            }

            rdbWarranty_SelectedIndexChanged(null, null);
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    protected void gvFresh_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvFresh.PageIndex = e.NewPageIndex;
            gvFresh.DataSource = (DataSet)ViewState["Grid1"];
            gvFresh.DataBind();
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    protected void gvFresh_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string str = "False", CallStage = "";
            int CallStatusID = -1;
            int splitNo = 0;
            string strASCReAllocFlag = "";
            string strComplaint_RefNo = "";
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Add Binay --24-11-2009
                strComplaint_RefNo = ((HiddenField)e.Row.FindControl("hdnComplaint_RefNo")).Value.ToString();
                objServiceContractor.GetCreatedBy(strComplaint_RefNo);
                //END
                str = ((HiddenField)e.Row.FindControl("hdnAppointmentFlag")).Value.ToString();
                CallStatusID = int.Parse(((HiddenField)e.Row.FindControl("gvFreshhdnCallStatus")).Value.ToString());
                CallStage = ((HiddenField)e.Row.FindControl("gvFreshhdnCallStage")).Value.ToString();
                strASCReAllocFlag = ((HiddenField)e.Row.FindControl("gvFreshhdnASCReAllocFlag")).Value.ToString();
                if (str == "True")
                {
                    e.Row.CssClass = "hgridbgcolor";
                }
                if (strASCReAllocFlag == "N")
                {
                    e.Row.CssClass = "hgridbgcolorred";
                }
                // if (CallStatusID == 2 || CallStatusID == 14 || CallStatusID == 15 || CallStatusID == 16 || CallStatusID == 23 || CallStatusID == 9 || CallStatusID == 28 || CallStatusID == 34)
                if (CallStatusID == 47 || CallStatusID == 57 || CallStatusID == 58 || CallStatusID == 59 || CallStatusID == 60 || CallStatusID == 61)
                {
                    ((LinkButton)e.Row.FindControl("lnkBtnNext")).Enabled = false;
                }

                if (txtComplaintRefNo.Text != "")
                {
                    if (CallStage != "Initialization")
                    {
                        tbIntialization.Visible = false;
                    }
                    else if (CallStage == "Initialization")
                    {
                        tbIntialization.Visible = true;
                    }
                }

                //Add Binay-24-11-2009 
                if ((objServiceContractor.userName == objServiceContractor.CreatedBy) && ((CallStatusID != 57) && (CallStatusID != 58) && (CallStatusID != 59) && (CallStatusID != 60) && (CallStatusID != 61) && (CallStatusID == 47)))
                {
                    ((LinkButton)e.Row.FindControl("lnkBtnNext")).Enabled = true;
                }

                //Add By Binay -03-12-2009
                if (((objServiceContractor.userName != objServiceContractor.CreatedBy) && (CallStatusID == 47)) || ((CallStatusID == 43) || (CallStatusID == 41)))
                {
                    e.Row.CssClass = "hgridbgcolorredMTO";
                }
                else if ((objServiceContractor.userName != objServiceContractor.CreatedBy) && (CallStatusID == 41))
                {
                    e.Row.CssClass = "";
                }

            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    #endregion

    #region Common Functions
    //To get the login id of the logged SC
    protected void GetSCNo()
    {
        try
        {
            string SCUserName = Membership.GetUser().ToString();
            SqlParameter[] sqlparam = {
                               new SqlParameter("@Type","SELECT_SC_SNO"),
                               new SqlParameter("@SC_UserName",SCUserName)
                           };
            DataSet ds = objSqlDataAccessLayer.ExecuteDataset(CommandType.StoredProcedure, "uspBaseComDet_MTO", sqlparam);
            if (ds.Tables[0].Rows.Count != 0)
            {
                objServiceContractor.SC_SNo = int.Parse(ds.Tables[0].Rows[0]["SC_SNo"].ToString());
                objServiceContractor.SC_Name = ds.Tables[0].Rows[0]["SC_Name"].ToString();
            }
            else
            {
                objServiceContractor.SC_SNo = 0;
            }
        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }
    protected void GetUserType()
    {
        try
        {
            string UserName = Membership.GetUser().ToString();
            SqlParameter[] sqlparam = {
                               new SqlParameter("@Type","CHECK USER TYPE"),
                               new SqlParameter("@Username",UserName)
                           };
            DataSet ds = objSqlDataAccessLayer.ExecuteDataset(CommandType.StoredProcedure, "uspBaseComDet_MTO", sqlparam);
            if (ds.Tables[0].Rows.Count != 0)
            {
                objServiceContractor.UserType = int.Parse(ds.Tables[0].Rows[0]["Usertype"].ToString());

            }

        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }
 
    //To get attributes from the attribute mapping table based on product line : bhawesh 2 march 12 Updated
    protected void GetDefectAttributes()
    {
        trAttAvailability.Visible = false;
        trAttCoachNo.Visible = false;
        trAttEquipmentName.Visible = false;
        trAttTrainNo.Visible = false;

        rfvCoach.Enabled = false;
        rfvTrain.Enabled = false;
        rfvAvailability.Enabled = false;
        rfvEquipName.Enabled = false;

        DataSet ds = objServiceContractor.GetAttrbuteMapping();
        if (ds.Tables[0].Rows.Count != 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                // Name of Equipment
                if (int.Parse(dr["Attribute_Sno"].ToString()) == 20)
                {
                    trAttEquipmentName.Visible = true;
                    txtAttEquipmentName.Text = dr["AttrDefaultValue"].ToString();
                    if (dr["AttrRequired"].ToString() == "N")
                        rfvEquipName.Enabled = false;
                    else if (dr["AttrRequired"].ToString() == "Y")
                        rfvEquipName.Enabled = true;
                }
                // Coach Number
                if (int.Parse(dr["Attribute_Sno"].ToString()) == 21)
                {
                    trAttCoachNo.Visible = true;
                    txtAttCoachNo.Text = dr["AttrDefaultValue"].ToString();
                    if (dr["AttrRequired"].ToString() == "N")
                        rfvCoach.Enabled = false;
                    else if (dr["AttrRequired"].ToString() == "Y")
                        rfvCoach.Enabled = true;
                }
                // Train Number
                if (int.Parse(dr["Attribute_Sno"].ToString()) == 22)
                {
                    trAttTrainNo.Visible = true;
                    txtAttTrainNo.Text = dr["AttrDefaultValue"].ToString();
                    if (dr["AttrRequired"].ToString() == "N")
                        rfvTrain.Enabled = false;
                    else if (dr["AttrRequired"].ToString() == "Y")
                        rfvTrain.Enabled = true;
                }
                // Availability of Coach/BLDC Fan at depot
                if (int.Parse(dr["Attribute_Sno"].ToString()) == 23 )
                {
                   trAttAvailability.Visible = true;
                   txtAttAvailability.Text = dr["AttrDefaultValue"].ToString();
                   if (dr["AttrRequired"].ToString() == "N")
                       rfvAvailability.Enabled = false;
                   else if (dr["AttrRequired"].ToString() == "Y")
                       rfvAvailability.Enabled = true;
               }
            }
        }
    }

    //To get the action time
    protected void GetActionTime(DropDownList ddlHR, DropDownList ddlMIN, DropDownList ddlAM)
    {
        //if (ddlHR.SelectedIndex != 0 && ddlMIN.SelectedIndex != 0 && ddlAM.SelectedIndex != 0)
        objServiceContractor.ActionTime = ddlHR.SelectedValue.ToString() + ":" + ddlMIN.SelectedValue.ToString() + ":00" + " " + ddlAM.SelectedValue.ToString();
        //else
        //  objServiceContractor.ActionTime = "12:00:00 AM";
    }
    //To set the appointment time
    protected void GetAppTime(DropDownList ddlHR, DropDownList ddlMIN, DropDownList ddlAM)
    {
        //if (ddlHR.SelectedIndex != 0 && ddlMIN.SelectedIndex != 0 && ddlAM.SelectedIndex != 0)
        objServiceContractor.SLATime = ddlHR.SelectedValue.ToString() + ":" + ddlMIN.SelectedValue.ToString() + ":00" + " " + ddlAM.SelectedValue.ToString();
        //else
        //objServiceContractor.SLATime = "12:00:00 AM";
    }

    //To set the action time in the ddls
    protected void SetActionTime(DropDownList ddlHR, DropDownList ddlMIN, DropDownList ddlAM)
    {
        int hr=0;
        string ampm, strHr, min;
        //Comment By Binay   
        ////DateTime dtNow= DateTime.Now;
        //////string dtTest = dtNow.ToString("dd-MM-yyyy hh:mm tt");

        ////////dtNow = DateTime.Parse(dtTest);
        ////strHr = dtNow.ToString("h");
        ////min = dtNow.ToString("mm");
        ////ampm = dtNow.ToString("tt");
        //////DateTime dtNow = DateTime.ParseExact(strTime, "dd/MM/yyyy hh:mm", null);

        hr = int.Parse(DateTime.Now.Hour.ToString());
        min = DateTime.Now.Minute.ToString();
        if (hr > 12)
        {
            hr = hr - 12;
            ampm = "PM";
        }
        else
        {
            ampm = "AM";
        }
        strHr = hr.ToString();

        ddlHR.Items.FindByValue(strHr).Selected = true;
        ddlMIN.Items.FindByValue(min).Selected = true;
        ddlAM.Items.FindByValue(ampm).Selected = true;
    }
    //Single function to set action time in all ddls once
    protected void SetAllActionTime()
    {
        SetActionTime(ddlInitHr, ddlInitMin, ddlInitAm);
        SetActionTime(ddlFirHr, ddlFirMin, ddlFirAM);
        SetActionTime(ddlDefHr, ddlDefMin, ddlDefAM);
        SetActionTime(ddlActHr, ddlActMin, ddlActAM);
    }

    //To view Attributes after approval
    protected void GetDefectAttributesDataFromPPRAfterApproval()
    {
        // Attribute aaded for MTO, 1 march 12 : For railway signaling RSD
        trAttr_Availability.Visible = false;
        trAttr_CoachNo.Visible = false;
        trAttr_EquipmentName.Visible = false;
        trAttr_TrainNo.Visible = false;
      
        DataSet ds = objServiceContractor.GetAttrbuteMapping();
        if (ds.Tables[0].Rows.Count != 0)
        {
            tbViewAttribute.Visible = true;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //Name of Equipment
                if (int.Parse(dr["Attribute_Sno"].ToString()) == 20)
                {
                    trAttr_EquipmentName.Visible = true;
                }

                //Coach Number
                if (int.Parse(dr["Attribute_Sno"].ToString()) == 21)
                {
                   trAttr_CoachNo.Visible = true;
                }

                //Train Number
                if (int.Parse(dr["Attribute_Sno"].ToString()) == 22)
                {
                    trAttr_TrainNo.Visible = true;
                }

                //Availability of Coach/BLDC Fan at depot
                if (int.Parse(dr["Attribute_Sno"].ToString()) == 23)
                {
                   trAttr_Availability.Visible = true;
                }
           }
        }

        ds = objServiceContractor.GetAttrbuteDataFromPPR_MTO();

        if (ds.Tables[0].Rows.Count != 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                txtAttr_Availability.Text = dr["AVAILABILITY_DEPOT"].ToString();
                txtAttr_CoachNo.Text = dr["COACH_NO"].ToString();
                txtAttr_EquipmentName.Text = dr["EQUIPMENT_NAME"].ToString();
                txtAttr_TrainNo.Text = dr["TRAIN_NO"].ToString();
            }
        }


    }

    //To retrive data from ppr table for the attribute
    protected void GetDefectAttributesDataFromPPR()
    {
        trAttr_Availability.Visible = false;
        trAttr_CoachNo.Visible = false;
        trAttr_EquipmentName.Visible = false;
        trAttr_TrainNo.Visible = false;

        rfvCoach.Enabled = false;
        rfvTrain.Enabled = false;
        rfvAvailability.Enabled = false;
        rfvEquipName.Enabled = false;

        DataSet ds = objServiceContractor.GetAttrbuteMapping();
        if (ds.Tables[0].Rows.Count != 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (int.Parse(dr["Attribute_Sno"].ToString()) == 20)
                {
                    trAttr_EquipmentName.Visible = true;
                     if (dr["AttrRequired"].ToString() == "Y")
                             rfvEquipName.Enabled = true;
                }

                if (int.Parse(dr["Attribute_Sno"].ToString()) == 21)
                {
                    trAttr_CoachNo.Visible = true;
                     if (dr["AttrRequired"].ToString() == "Y")
                         rfvCoach.Enabled = true;
                }

                if (int.Parse(dr["Attribute_Sno"].ToString()) == 22)
                {
                   trAttr_TrainNo.Visible = true;
                    if (dr["AttrRequired"].ToString() == "Y")
                        rfvTrain.Enabled = true;
                }

                if (int.Parse(dr["Attribute_Sno"].ToString()) == 23)
                {
                    trAttr_Availability.Visible = true;
                      if (dr["AttrRequired"].ToString() == "Y")
                          rfvAvailability.Enabled = true;
                }
               
            }
       }
       
        ds = objServiceContractor.GetAttrbuteDataFromPPR_MTO();

        if (ds.Tables[0].Rows.Count != 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                txtAttEquipmentName.Text = dr["EQUIPMENT_NAME"].ToString();
                txtAttAvailability.Text = dr["AVAILABILITY_DEPOT"].ToString();
                txtAttTrainNo.Text = dr["TRAIN_NO"].ToString();
                txtAttCoachNo.Text = dr["COACH_NO"].ToString();
                txtDefectServiceActionRemarks.Text = dr["OBSERV"].ToString();
            }
        }


    }

    #endregion Common Functions

    #region Grid No. 2

    // Add defect link button
    protected void lnkCustGvDefect_Click(object sender, EventArgs e)
    {
        try
        {
            gvViewDefects.Visible = false;
            txtDefectDate.Text = DateTime.Today.Date.ToString("MM/dd/yyyy");
            lblDefectMsg.Text = "";

            ClearDefectControls();
            LinkButton lnk = (LinkButton)sender;
            objServiceContractor.Complaint_RefNo = lnk.CommandArgument.ToString();
            objServiceContractor.SplitComplaint_RefNo = int.Parse(lnk.CommandName.ToString());


            //to view the entered defects
            DataSet dsPPR = objServiceContractor.GetPPRDefect();
            txtDefectQty.Text = dsPPR.Tables[0].Rows.Count.ToString();
            gvDefectTemp.DataSource = dsPPR;
            gvDefectTemp.DataBind();
            if (dsPPR.Tables[0].Rows.Count != 0)
            {
                DataTable dt = dsPPR.Tables[0];
                ViewState["dtTempDefect"] = dt;
                RequiredFieldValidatorddlDefect.Enabled = false;
                RegularExpressionValidatorddlDefectCat.Enabled = false;
            }
            else
            {
                RequiredFieldValidatorddlDefect.Enabled = true;
                RegularExpressionValidatorddlDefectCat.Enabled = true;
            }
            //To set values to the controls in the defect section
            foreach (GridViewRow grv in gvCustDetail.Rows)
            {
                if (objServiceContractor.Complaint_RefNo == ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value.ToString()
                    && objServiceContractor.SplitComplaint_RefNo == int.Parse(((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value.ToString()))
                {
                    lblDefectProductLine.Text = ((Label)grv.FindControl("lblgvProductLine")).Text;
                    objServiceContractor.ProductLine_Sno = int.Parse(((HiddenField)grv.FindControl("hdngvProductLineSno")).Value.ToString());
                    //Line added By Mukesh TO set ProductLineSno for PPR_Code in ProdutLineMaster
                    hdnDefectProductLine_Sno.Value = ((HiddenField)grv.FindControl("hdngvProductLineSno")).Value;
                    hdnDefectBatch.Value = ((Label)grv.FindControl("lblgvBatch")).Text.ToString();
                    hdnDefectProductGroup_Sno.Value = ((HiddenField)grv.FindControl("hdngvProductGroupSno")).Value;
                    hdnDefectProduct_Sno.Value = ((HiddenField)grv.FindControl("hdngvProductSno")).Value;
                    hdnDefectComplaintLoggDate.Value = ((HiddenField)grv.FindControl("hdngvComplaintDate")).Value;
                    hdnDefectCustomerID.Value = ((HiddenField)grv.FindControl("hdngvCustomerID")).Value;
                    hdnDefectComplaintRef_No.Value = ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value;
                    hdnDefectSliptComplaint.Value = ((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value;
                    hdnDefectProductDiv_Sno.Value = ((HiddenField)grv.FindControl("hdngvProductDivisionSno")).Value;
                    hdnDefectCallStatus.Value = ((HiddenField)grv.FindControl("hdngvCallStatus")).Value;
                    lblDefComp.Text = ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value + "/" + ((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value;
                    lblDefProductDiv.Text = ((Label)grv.FindControl("lblgvProductDivision")).Text.ToString();
                    if (int.Parse(((HiddenField)grv.FindControl("hdngvCallStatus")).Value.ToString()) == 27)
                    {
                        txtDefectServiceActionRemarks.Text = ((HiddenField)grv.FindControl("hdngvLastComment")).Value.ToString();
                    }
                    //New Change for setting MFGUNIT
                    hdnDefectMFGUNIT.Value = ((HiddenField)grv.FindControl("hdngvManufacture_SNo")).Value;
                    hdnDefectProductSrNo.Value = ((HiddenField)grv.FindControl("hdngvProduct_SerialNo")).Value;

                }
            }
            ddlDefectCat.Items.Clear();
            ddlDefectCat.Items.Add(lstItem);
            objServiceContractor.BindDefectCatDdl(ddlDefectCat);

            tbBasicRegistrationInformation.Visible = false;
            tbDefect.Visible = true;
            tbAction.Visible = false;
            tbViewAttribute.Visible = false;
            //Attribute Handling for the defect section for PPR entry
            GetDefectAttributes();

            GetDefectAttributesDataFromPPR();
        }




        catch (Exception ex)
        {
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    // Action link button in grid no 2
    protected void lnkCustGvAction_Click(object sender, EventArgs e)
    {
        try
        {
            //Modified Date : 4 May 2009//
            txtServiceDate.Text = "";
            txtServiceAmount.Text = "";
            txtServiceNumber.Text = "";
            txtActionDetails.Text = "";
            //////////////////////////////
            //hdnActionCurrentDate ADDED BY SUJIT ON 19-05-2010 
            hdnActionCurrentDate.Value = DateTime.Today.Date.ToString("MM/dd/yyyy");
            txtActionEntryDate.Text = DateTime.Today.Date.ToString("MM/dd/yyyy");
            lblActionMsg.Text = "";
            tbAction.Visible = true;
            tbDefect.Visible = false;
            LinkButton lnk = (LinkButton)sender;
            objServiceContractor.Complaint_RefNo = lnk.CommandArgument.ToString();
            objServiceContractor.SplitComplaint_RefNo = int.Parse(lnk.CommandName.ToString());
            foreach (GridViewRow grv in gvCustDetail.Rows)
            {
                if (objServiceContractor.Complaint_RefNo == ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value.ToString()
                    && objServiceContractor.SplitComplaint_RefNo == int.Parse(((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value.ToString()))
                {
                    lblActionProductDiv.Text = ((Label)grv.FindControl("lblgvProductDivision")).Text.ToString();
                    lblActionComplaintRefNo.Text = ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value.ToString() + "/" + ((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value.ToString();
                    lblActionProduct.Text = ((Label)grv.FindControl("lblgvProduct")).Text.ToString();
                    lblActionBatch.Text = ((Label)grv.FindControl("lblgvBatch")).Text.ToString();
                    hdnActionSplitNo.Value = ((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value;
                    hdnActionCallStatusID.Value = ((HiddenField)grv.FindControl("hdngvCallStatus")).Value;
                    hdnActionSLADate.Value = ((HiddenField)grv.FindControl("hdngvSLADate")).Value;
                    hdnActionWarrantyStatus.Value = ((Label)grv.FindControl("lblgvWarrantyStatus")).Text.ToString();
                    lblActionWarranty.Text = ((Label)grv.FindControl("lblgvWarrantyStatus")).Text.ToString();
                }
            }
            string strWarrantyStatus;
            strWarrantyStatus = hdnActionWarrantyStatus.Value.ToString();
            if (strWarrantyStatus.ToUpper() == "Y")
            {
                rqftxtServiceAmount.Enabled = false;
                rqftxtServiceDate.Enabled = false;
                rqftxtServiceNumber.Enabled = false;
                trServiceDate.Visible = false;
                trServiceNumber.Visible = false;
                trServiceAmount.Visible = false;

            }
            else if (strWarrantyStatus.ToUpper() == "N")
            {
                rqftxtServiceAmount.Enabled = true;
                rqftxtServiceDate.Enabled = true;
                rqftxtServiceNumber.Enabled = true;
                trServiceDate.Visible = true;
                trServiceNumber.Visible = true;
                trServiceAmount.Visible = true;
            }

        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }


    }
    // View link button in Grid no 2
    protected void lnkCustGvViewDefect_Click(object sender, EventArgs e)
    {
        try
        {
            tbAction.Visible = false;
            tbDefect.Visible = false;
            gvViewDefects.Visible = true;
            LinkButton lnk = (LinkButton)sender;
            objServiceContractor.Complaint_RefNo = lnk.CommandArgument.ToString();
            objServiceContractor.SplitComplaint_RefNo = int.Parse(lnk.CommandName.ToString());
            //To view the attributes added in ppr
            //To set values to the controls in the defect section
            foreach (GridViewRow grv in gvCustDetail.Rows)
            {
                if (objServiceContractor.Complaint_RefNo == ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value.ToString()
                    && objServiceContractor.SplitComplaint_RefNo == int.Parse(((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value.ToString()))
                {
                    objServiceContractor.ProductLine_Sno = int.Parse(((HiddenField)grv.FindControl("hdngvProductLineSno")).Value.ToString());
                }
            }

            DataSet dsPPR = objServiceContractor.ViewPPRDefect();
            gvViewDefects.DataSource = dsPPR;
            gvViewDefects.DataBind();
            ViewState["dsPPR"] = dsPPR;


            GetDefectAttributesDataFromPPRAfterApproval();
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }

    // View link button in Temp grid during fir
    protected void lnkTempGvViewDefect_Click(object sender, EventArgs e)
    {
        try
        {
            tbAction.Visible = false;
            tbDefect.Visible = false;
            gvViewDefects.Visible = true;
            LinkButton lnk = (LinkButton)sender;
            objServiceContractor.Complaint_RefNo = lnk.CommandArgument.ToString();
            objServiceContractor.SplitComplaint_RefNo = int.Parse(lnk.CommandName.ToString());
            //To view the attributes added in ppr
            //To set values to the controls in the defect section
            foreach (GridViewRow grv in gvCustDetail.Rows)
            {
                if (objServiceContractor.Complaint_RefNo == ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value.ToString()
                    && objServiceContractor.SplitComplaint_RefNo == int.Parse(((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value.ToString()))
                {
                    objServiceContractor.ProductLine_Sno = int.Parse(((HiddenField)grv.FindControl("hdngvProductLineSno")).Value.ToString());
                }
            }

            DataSet dsPPR = objServiceContractor.ViewPPRDefect();
            gvViewDefects.DataSource = dsPPR;
            gvViewDefects.DataBind();
            ViewState["dsPPR"] = dsPPR;

            GetDefectAttributesDataFromPPRAfterApproval();
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    protected void gvViewDefects_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvViewDefects.PageIndex = e.NewPageIndex;
            gvViewDefects.DataSource = (DataSet)ViewState["dsPPR"];
            gvViewDefects.DataBind();
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }

    protected void gvCustDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            gvViewDefects.Visible = false;
            //To toggle visiblity of the link button add defect and view defect
            // based on whether defect entred or approved
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Modifyed By Binay
                //if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 11 || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "Y")
                if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 54 || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "Y")
                {
                    ((LinkButton)e.Row.FindControl("lnkCustGvDefect")).Visible = false;
                    ((LinkButton)e.Row.FindControl("lnkCustGvViewDefect")).Visible = true;
                }
                //else if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 27 || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "N" || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "")
                else if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 56 || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "N" || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "")
                {
                    ((LinkButton)e.Row.FindControl("lnkCustGvDefect")).Visible = true;
                    ((LinkButton)e.Row.FindControl("lnkCustGvViewDefect")).Visible = false;
                }
                // if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 14 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 15 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 28 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 32 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 23)
                if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 57 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 58 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 60 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 32 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 59)
                {
                    ((LinkButton)e.Row.FindControl("lnkCustGvDefect")).Visible = false;
                    ((LinkButton)e.Row.FindControl("lnkCustGvAction")).Visible = false;

                    ((LinkButton)e.Row.FindControl("lnkCustGvViewDefect")).Visible = true;

                }
            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }

    }
    protected void gvCustDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvCustDetail.PageIndex = e.NewPageIndex;
            gvCustDetail.DataSource = (DataSet)ViewState["dsGvCust"];
            gvCustDetail.DataBind();
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    //Second Gried Row Data Bind--2
    protected void gvAddTemp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //To toggle visiblity of the link button add defect and view defect
        // based on whether defect entred or approved
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 11 || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "Y") for MTS
            //For MTO Modify By Binay
           if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 54 || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "Y") 
            {
                ((LinkButton)e.Row.FindControl("lnkTempGvDefect")).Visible = false;
                ((LinkButton)e.Row.FindControl("lnkTempGvViewDefect")).Visible = true;
                ////
                ((LinkButton)e.Row.FindControl("lnkTempGvAction")).Visible = true;
                ((LinkButton)e.Row.FindControl("lnkTempGv")).Visible = false;
            }
            //End
            //else if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 27 || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "N" || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "")
            else if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 56 || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "N" || ((HiddenField)e.Row.FindControl("hdngvDefectAccFlag")).Value.ToString() == "")
            {
                //if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 4 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 17 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 18 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 13 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 12 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 24 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 33)
                if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 62 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 45 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 46)
                {
                    ((LinkButton)e.Row.FindControl("lnkTempGvDefect")).Visible = false;
                    ((LinkButton)e.Row.FindControl("lnkTempGvViewDefect")).Visible = false;
                }
                else
                {
                    ((LinkButton)e.Row.FindControl("lnkTempGvDefect")).Visible = true;
                    ((LinkButton)e.Row.FindControl("lnkTempGvViewDefect")).Visible = false;
                    ////
                    ((LinkButton)e.Row.FindControl("lnkTempGvAction")).Visible = true;
                    ((LinkButton)e.Row.FindControl("lnkTempGv")).Visible = false;
                }

            }
            //if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 14 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 15 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 28)
            // Added By Gaurav Garg on 27 NOV (59/61)
            if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 57 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 58 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 60 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 61 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 59)
            {
                ((LinkButton)e.Row.FindControl("lnkTempGvDefect")).Visible = false;
                ((LinkButton)e.Row.FindControl("lnkTempGvAction")).Visible = false;

                ((LinkButton)e.Row.FindControl("lnkTempGvViewDefect")).Visible = true;

            }

            //Code By Binay FOR MTO Hide Link Button
            if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 41 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 42 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 43 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 44 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 45 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 46 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 47 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 48 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 49)
            {
                ((LinkButton)e.Row.FindControl("lnkTempGvDefect")).Visible = false; //Add Defect
                ((LinkButton)e.Row.FindControl("lnkTempGvViewDefect")).Visible = false; //View Defect
                ((LinkButton)e.Row.FindControl("lnkTempGvAction")).Visible = false; ///ACTION
                ((LinkButton)e.Row.FindControl("lnkTempGv")).Visible = true;//Remove

            }
            else if (int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 51 || int.Parse(((HiddenField)e.Row.FindControl("hdngvCallStatus")).Value.ToString()) == 47)
            {
                ((LinkButton)e.Row.FindControl("lnkTempGvDefect")).Visible = true; //Add Defect
                ((LinkButton)e.Row.FindControl("lnkTempGvViewDefect")).Visible = false; //View Defect
                ((LinkButton)e.Row.FindControl("lnkTempGvAction")).Visible = false; ///ACTION
                ((LinkButton)e.Row.FindControl("lnkTempGv")).Visible = false;//Remove
            }
            //END

        }
    }
    #endregion Grid No. 2

    #region FIR
    //////FIR Related////////
    // Link button in temporary grid to remove the grid row
    protected void llnkTempGv_Click(object sender, EventArgs e)
    {
        try
        {

            tbBasicRegistrationInformation.Visible = true;
            tbTempGrid.Visible = true;
            LinkButton lnk = (LinkButton)sender;
            int rowNo = int.Parse(lnk.CommandArgument.ToString());
            DataTable dtTemp = (DataTable)ViewState["dtTemp"];
            dtTemp.Rows[rowNo - 1].Delete();
            dtTemp.AcceptChanges();
            ViewState["dtTemp"] = dtTemp;
            CheckCallSplit();
            BindTempGrid();
            // Added By Gaurav Garg on 26 Nov
            if (dtTemp.Rows.Count == 0)
            {
                btnSave.Visible = false;
            }

        }
        catch (Exception ex)
        {
            //lblSave.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    // Button to add product to the temporary grid before commiting fir
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        try
        {
            btnSave.CausesValidation = false;
            lblSave.Text = "";
            CheckCallSplit();
            CreatRow();
            BindTempGrid();
            //Add Code By Binay--MTO(Product Serial no

            ViewState["PSNO"] = txtProductRefNo.Text;
            //End
            ClearFirControlsOnAdd();
            //txtProductRefNo.Text=ViewState["PSNO"].ToString(); 
            if (gvAddTemp.Rows.Count != 0)
            {
                btnSave.Visible = true;
                ddlProduct.SelectedIndex = 0;
            }
            else
            { btnSave.Visible = false; }
        }
        catch (Exception ex)
        {
            //lblSave.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }

    //Function to check the split sequence
    protected void CheckCallSplit()
    {
        try
        {
            SqlParameter[] param ={
                                 new SqlParameter("@Type","CHECKCALLSPLIT"),
                                 new SqlParameter("@Complaint_RefNo",hdnComplaintRef.Value.ToString())
                             };
            DataSet ds = objSqlDataAccessLayer.ExecuteDataset(CommandType.StoredProcedure, "uspBaseComDet_MTO", param);
            if (ds.Tables[0].Rows.Count != 0)
                objServiceContractor.SplitComplaint_RefNo = int.Parse(ds.Tables[0].Rows[0]["SplitCount"].ToString());
        }
        catch (Exception) { }
    }

    //Function to create a row in temporary datatable
    protected void CreatRow()
    {

        DataTable dtTemp = (DataTable)ViewState["dtTemp"];
        DataRow drw = dtTemp.NewRow();
        drw["CustomerID"] = hdnCustmerID.Value.ToString();
        drw["Complaint_RefNo"] = lblComplainNo.Text;
        drw["ComplaintDate"] = lblComplainDate.Text;
        drw["ProductDivisionSno"] = int.Parse(hdnProductDvNo.Value.ToString());
        drw["ProductDivision"] = lblUnit.Text.ToString();
        //drw["ProductDivision"] = ddlFirProductDiv.SelectedItem.Text.ToString();
        drw["ProductLine"] = ddlProductLine.SelectedItem.Text;
        drw["ProductLineSno"] = int.Parse(ddlProductLine.SelectedValue.ToString());
        drw["ProductLine"] = ddlProductLine.SelectedItem.Text;
        drw["ProductGroupSno"] = int.Parse(ddlProductGroup.SelectedValue.ToString());
        drw["ProductGroup"] = ddlProductGroup.SelectedItem.Text;
        drw["Product"] = ddlProduct.SelectedItem.Text;
        drw["ProductSno"] = int.Parse(ddlProduct.SelectedValue.ToString());
        if (txtInvoiceDate.Text != "")
            drw["InvoiceDate"] = txtInvoiceDate.Text;
        else
            drw["InvoiceDate"] = "01/01/1900";
        drw["InvoiceNo"] = txtInvoiceNo.Text;
        drw["Batch"] = txtBatchNo.Text;
        drw["AdditionalInfo"] = txtWarranty.Text;
        drw["WarrantyDate"] = txtFirDate.Text;
        if (hdnSLADate.Value != null)
            drw["SLADate"] = hdnSLADate.Value.ToString();
        GetActionTime(ddlFirHr, ddlFirMin, ddlFirAM);
        drw["ActionTime"] = objServiceContractor.ActionTime;
        drw["CallStatus"] = int.Parse(hdnCallStatus.Value.ToString());
        drw["Product_SerialNo"] = txtProductRefNo.Text.Trim();
        //Add Code By Binay-25-11-2009
        //if (ddlMfgUnit.SelectedIndex != 0) Prev CodeFor MTS
        if (ddlMfgUnit.SelectedValue.ToString() != "0")
            drw["Manufacture_SNo"] = int.Parse(ddlMfgUnit.SelectedValue.ToString());
        if (hdnNatureOfComplaint.Value.ToString() != "")
            drw["NatureOfComplaint"] = hdnNatureOfComplaint.Value.ToString();
        if (txtVisitCharges.Text != "")
            drw["VisitCharges"] = System.Convert.ToDecimal(txtVisitCharges.Text.ToString());
        else
            drw["VisitCharges"] = 0;
        //Code add By Binay(MTO)
        if (txtSpareCharges.Text != "")
            drw["SpareCharges"] = System.Convert.ToDecimal(txtSpareCharges.Text.ToString());
        else
            drw["SpareCharges"] = 0;
        if (txtTravelCharges.Text != "")
            drw["TravelCharges"] = System.Convert.ToDecimal(txtTravelCharges.Text.ToString());
        else
            drw["TravelCharges"] = 0;
        //End
        if (rdbWarranty.SelectedIndex == 0)
            drw["WarrantyStatus"] = "Y";
        else
            drw["WarrantyStatus"] = "N";
        drw["DefectAccFlag"] = "N";
        //SUJIT 
        drw["DateOfReporting"] = txtCompalaintDate.Text.ToString();
        // added By Guarav Garg on 2 Dec
        if (txtDispatchDate.Text != "")
            drw["DispatchDate"] = txtDispatchDate.Text;
        else
            drw["DispatchDate"] = "01/01/1900";
        //End

        // added By Naveen on 18 jan 2010
        if (txtPoDate.Text != "")
            drw["ManufactureDate"] = txtPoDate.Text.Trim();
        else
            drw["ManufactureDate"] = "01/01/1900";
        //End

        // added By Naveen on 18 jan 2010
        if (txtWarrantyDate.Text != "")
            drw["WarrantyExpirydate"] = txtWarrantyDate.Text.Trim();
        else
            drw["WarrantyExpirydate"] = "01/01/1900";
        //End

        // added By Naveen on 18 jan 2010
        if (txtCommissionDate.Text != "")
            drw["DtofCommission"] = txtCommissionDate.Text.Trim();
        else
            drw["DtofCommission"] = "01/01/1900";
        //End
        // RSD chanbge
        drw["EquipmentName"] = txtequipname.Text;
        drw["CoachNo"] = txtcoachNo.Text;
        drw["TrainNo"] = txtTrainNo.Text;
        drw["AvailabilityDepot"] = txtAvailblty.Text;

        // Added by Gaurav Garg on 27 NOV
        bool blnIsValid = true;
        foreach (DataRow dr in dtTemp.Rows)
        {
            if (dr["ProductSno"].ToString().Trim() == drw["ProductSno"].ToString().Trim())
            {
                blnIsValid = false;
                break;
            }
        }
        if (blnIsValid)
        {
            dtTemp.Rows.Add(drw);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(btnAdd, GetType(), "Confirm", "alert('Product already added');", true);
        }
        //END

        ViewState["dtTemp"] = dtTemp;
    }
    //Function to create temporary datatable
    // increase size of int16 to int32 by VT 
    protected void CreatTempTable()
    {
        DataColumn Sno = new DataColumn("Sno", System.Type.GetType("System.Int32"));
        DataColumn SplitComplaint_RefNo = new DataColumn("SplitComplaint_RefNo", System.Type.GetType("System.Int32"));
        DataColumn CallStatus = new DataColumn("CallStatus", System.Type.GetType("System.Int32"));
        DataColumn Complaint_RefNo = new DataColumn("Complaint_RefNo", System.Type.GetType("System.String"));
        DataColumn CustomerID = new DataColumn("CustomerID", System.Type.GetType("System.String"));
        DataColumn ComplaintDate = new DataColumn("ComplaintDate", System.Type.GetType("System.DateTime"));
        DataColumn ProductDivisionSno = new DataColumn("ProductDivisionSno", System.Type.GetType("System.Int32"));
        DataColumn ProductDivision = new DataColumn("ProductDivision", System.Type.GetType("System.String"));
        DataColumn ProductLineSno = new DataColumn("ProductLineSno", System.Type.GetType("System.Int32"));
        DataColumn Manufacture_SNo = new DataColumn("Manufacture_SNo", System.Type.GetType("System.Int32"));
        DataColumn ProductLine = new DataColumn("ProductLine", System.Type.GetType("System.String"));
        DataColumn ProductGroup = new DataColumn("ProductGroup", System.Type.GetType("System.String"));
        DataColumn ProductGroupSno = new DataColumn("ProductGroupSno", System.Type.GetType("System.Int32"));
        DataColumn Product = new DataColumn("Product", System.Type.GetType("System.String"));
        DataColumn ProductSno = new DataColumn("ProductSno", System.Type.GetType("System.Int32"));
        DataColumn Batch = new DataColumn("Batch", System.Type.GetType("System.String"));
        DataColumn InvoiceDate = new DataColumn("InvoiceDate", System.Type.GetType("System.DateTime"));
        DataColumn InvoiceNo = new DataColumn("InvoiceNo", System.Type.GetType("System.String"));
        DataColumn AdditionalInfo = new DataColumn("AdditionalInfo", System.Type.GetType("System.String"));
        DataColumn WarrantyDate = new DataColumn("WarrantyDate", System.Type.GetType("System.DateTime"));
        DataColumn ActionTime = new DataColumn("ActionTime", System.Type.GetType("System.String"));
        DataColumn WarrantyStatus = new DataColumn("WarrantyStatus", System.Type.GetType("System.String"));
        DataColumn NatureOfComplaint = new DataColumn("NatureOfComplaint", System.Type.GetType("System.String"));
        DataColumn VisitCharges = new DataColumn("VisitCharges", System.Type.GetType("System.String"));
        //Add Code Binay For MTO
        DataColumn SpareCharges = new DataColumn("SpareCharges", System.Type.GetType("System.String"));
        DataColumn TravelCharges = new DataColumn("TravelCharges", System.Type.GetType("System.String"));
        // Added By Gaurav Garg on 2 Dec
        DataColumn DispatchDate = new DataColumn("DispatchDate", System.Type.GetType("System.DateTime"));
        //End
        DataColumn DefectAccFlag = new DataColumn("DefectAccFlag", System.Type.GetType("System.String"));
        DataColumn Product_SerialNo = new DataColumn("Product_SerialNo", System.Type.GetType("System.String"));
        DataColumn SLADate = new DataColumn("SLADate", System.Type.GetType("System.DateTime"));
        //SUJIT DATE OF REPORTING
        DataColumn DateOfReporting = new DataColumn("DateOfReporting", System.Type.GetType("System.DateTime"));
        DataColumn dColEmpManufactureDate = new DataColumn("ManufactureDate", System.Type.GetType("System.String"));
        DataColumn dColEmpDtofCommissionDate = new DataColumn("DtofCommission", System.Type.GetType("System.String"));
        DataColumn dtEmpWarrantyExpiryDate = new DataColumn("WarrantyExpiryDate", System.Type.GetType("System.String"));

        // for RSD
        DataColumn dcEquipmentName = new DataColumn("EquipmentName", System.Type.GetType("System.String"));
        DataColumn dcCoachNo = new DataColumn("CoachNo", System.Type.GetType("System.String"));
        DataColumn dcTrainNo = new DataColumn("TrainNo", System.Type.GetType("System.String"));
        DataColumn dcAvailability = new DataColumn("AvailabilityDepot", System.Type.GetType("System.String"));


        dtTemp.Columns.Add(Sno);
        dtTemp.Columns.Add(SplitComplaint_RefNo);
        dtTemp.Columns.Add(CallStatus);
        dtTemp.Columns.Add(Complaint_RefNo);
        dtTemp.Columns.Add(CustomerID);
        dtTemp.Columns.Add(ComplaintDate);
        dtTemp.Columns.Add(ProductDivisionSno);
        dtTemp.Columns.Add(ProductDivision);
        dtTemp.Columns.Add(ProductLineSno);
        dtTemp.Columns.Add(ProductLine);
        dtTemp.Columns.Add(ProductGroup);
        dtTemp.Columns.Add(ProductGroupSno);
        dtTemp.Columns.Add(Product);
        dtTemp.Columns.Add(ProductSno);
        dtTemp.Columns.Add(Batch);
        dtTemp.Columns.Add(InvoiceDate);
        dtTemp.Columns.Add(InvoiceNo);
        dtTemp.Columns.Add(AdditionalInfo);
        dtTemp.Columns.Add(WarrantyDate);
        dtTemp.Columns.Add(ActionTime);
        dtTemp.Columns.Add(WarrantyStatus);
        dtTemp.Columns.Add(NatureOfComplaint);
        dtTemp.Columns.Add(Manufacture_SNo);
        dtTemp.Columns.Add(VisitCharges);
        //Add By Binay (MTO)
        dtTemp.Columns.Add(SpareCharges);
        dtTemp.Columns.Add(TravelCharges);
        // Added By Gaurav Garg on 2 Dec
        dtTemp.Columns.Add(DispatchDate);
        //END
        dtTemp.Columns.Add(DefectAccFlag);
        dtTemp.Columns.Add(Product_SerialNo);
        dtTemp.Columns.Add(SLADate);
        //sujit on  11-05-2010
        dtTemp.Columns.Add(DateOfReporting);

        dtTemp.Columns.Add(dColEmpManufactureDate);
        dtTemp.Columns.Add(dColEmpDtofCommissionDate);
        dtTemp.Columns.Add(dtEmpWarrantyExpiryDate);

        dtTemp.Columns.Add(dcEquipmentName);
        dtTemp.Columns.Add(dcCoachNo);
        dtTemp.Columns.Add(dcTrainNo);
        dtTemp.Columns.Add(dcAvailability);
        ViewState["dtTemp"] = dtTemp;
    }
    //Function to Bind the teporary grid in FIR with temporary datatable
    protected void BindTempGrid()
    {
        DataTable dtTemp = (DataTable)ViewState["dtTemp"];
        if (objServiceContractor.SplitComplaint_RefNo != 1)
        {

            for (int intCounter = objServiceContractor.SplitComplaint_RefNo; intCounter < dtTemp.Rows.Count + objServiceContractor.SplitComplaint_RefNo; intCounter++)
            {
                dtTemp.Rows[intCounter - objServiceContractor.SplitComplaint_RefNo]["SplitComplaint_RefNo"] = intCounter + 1;
                dtTemp.Rows[intCounter - objServiceContractor.SplitComplaint_RefNo]["Sno"] = intCounter - objServiceContractor.SplitComplaint_RefNo + 1;
            }
        }
        else
        {
            for (int intCounter = 0; intCounter < dtTemp.Rows.Count; intCounter++)
            {
                dtTemp.Rows[intCounter]["SplitComplaint_RefNo"] = intCounter + 1;
                dtTemp.Rows[intCounter]["Sno"] = intCounter + 1;
            }
        }


        gvAddTemp.DataSource = dtTemp;
        gvAddTemp.DataBind();

    }
    // Button to save FIR
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            //ScriptManager.RegisterClientScriptBlock(btnSave, GetType(), "Confirm", "confirm('Are You Sure');", true);
            lblSave.Text = "";
            if (lblComplainDate.Text != "")
                objServiceContractor.LoggedDate = Convert.ToDateTime(lblComplainDate.Text.Trim());
            else
                objServiceContractor.LoggedDate = DateTime.Now.Date;
            if (gvAddTemp.Rows.Count != 0)
            {
                objServiceContractor.EmpID = Membership.GetUser().UserName.ToString();
                foreach (GridViewRow grv in gvAddTemp.Rows)
                {
                    objServiceContractor.SplitComplaint_RefNo = int.Parse(((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value.ToString());
                    objServiceContractor.Complaint_RefNo = ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value.ToString();
                    objServiceContractor.LoggedBY = Membership.GetUser().UserName.ToString();
                    objServiceContractor.LoggedDate = Convert.ToDateTime(((HiddenField)grv.FindControl("hdngvComplaintDate")).Value.ToString());
                    objServiceContractor.ProductLine_Sno = int.Parse(((HiddenField)grv.FindControl("hdngvProductLineSno")).Value.ToString());
                    objServiceContractor.ProductGroup_SNo = int.Parse(((HiddenField)grv.FindControl("hdngvProductGroupSno")).Value.ToString());
                    objServiceContractor.BaseLineId = int.Parse(hdnBaseLineID.Value.ToString());
                    objServiceContractor.CustomerId = int.Parse(hdnCustmerID.Value.ToString());
                    objServiceContractor.Product_SNo = int.Parse(((HiddenField)grv.FindControl("hdngvProductSno")).Value.ToString());
                    objServiceContractor.Batch_Code = ((Label)grv.FindControl("lblgvBatch")).Text.ToString();
                    objServiceContractor.ProductSerial_No = ((HiddenField)grv.FindControl("hdngvProduct_SerialNo")).Value.ToString();
                    objServiceContractor.ProductDivision_Sno = int.Parse(hdnProductDvNo.Value.ToString());
                    objServiceContractor.InvoiceDate = Convert.ToDateTime(((HiddenField)grv.FindControl("hdngvInvoiceDate")).Value.ToString());
                    objServiceContractor.InvoiceNo = ((HiddenField)grv.FindControl("hdngvInvoiceNo")).Value.ToString();
                    objServiceContractor.WarrantyStatus = ((Label)grv.FindControl("lblgvWarrantyStatus")).Text.ToString();
                    objServiceContractor.ActionDate = DateTime.Now;
                    objServiceContractor.ActionTime = ((HiddenField)grv.FindControl("hdngvActionTime")).Value.ToString();
                    strVar = ((HiddenField)grv.FindControl("hdngvVisitCharges")).Value.ToString();
                    objServiceContractor.VisitCharges = System.Convert.ToDecimal(strVar);
                    //Add Code By Binay
                    strSpareCharges = ((HiddenField)grv.FindControl("hdnSpareCharges")).Value.ToString();
                    objServiceContractor.SpareCharges = System.Convert.ToDecimal(strSpareCharges);
                    strTravelCharges = ((HiddenField)grv.FindControl("hdnTravelCharges")).Value.ToString();
                    objServiceContractor.TravelCharges = System.Convert.ToDecimal(strTravelCharges);
                    //Added by gaurav garg on 2 Dec
                    objServiceContractor.DispatchDate = Convert.ToDateTime(((HiddenField)grv.FindControl("hdnDispatchDate")).Value.ToString());

                    objServiceContractor.Manufacture_Date = Convert.ToDateTime(((HiddenField)grv.FindControl("hdnManufactureDate")).Value.ToString());
                    objServiceContractor.Warranty_Expiry_date = Convert.ToDateTime(((HiddenField)grv.FindControl("hdnWarrantyExpirydate")).Value.ToString());
                    objServiceContractor.SLADate = Convert.ToDateTime(((HiddenField)grv.FindControl("hdngvSLADate")).Value.ToString());
                    objServiceContractor.Date_of_Reporting = Convert.ToDateTime(((HiddenField)grv.FindControl("hdDateOfReporting")).Value.ToString());
                    objServiceContractor.Date_of_Commission = Convert.ToDateTime(((HiddenField)grv.FindControl("hdnDateofCommission")).Value.ToString());

                    //End
                    objServiceContractor.NatureOfComplaint = ((HiddenField)grv.FindControl("hdngvNatureOfComplaint")).Value.ToString();
                    if (((HiddenField)grv.FindControl("hdngvManufacture_SNo")).Value.ToString() != "")
                        objServiceContractor.Manufacture_SNo = int.Parse(((HiddenField)grv.FindControl("hdngvManufacture_SNo")).Value.ToString());
                    objServiceContractor.LastComment = ((Label)grv.FindControl("lblgvAdditionalInfo")).Text.ToString();
                    //objServiceContractor.CallStatus = 6;
                    //Code Binay
                    //Add Two charges Spare and Travel In 
                    objServiceContractor.CallStatus = 51;//FIR DONE
                    //End
                    objServiceContractor.EmpID = Membership.GetUser().UserName.ToString();
                    objServiceContractor.Quantity = 1;
                    if (objServiceContractor.SplitComplaint_RefNo == 1)
                    { objServiceContractor.Type = "UPD_PRODETAIL"; }
                    else { objServiceContractor.Type = "INS_PRODETAIL"; }
                    GetSCNo();
                // RSD Change
                    objServiceContractor.EQUIPMENT_NAME = ((HiddenField)grv.FindControl("hdnequipname")).Value;
                    objServiceContractor.TRAIN_NO = ((HiddenField)grv.FindControl("hdntrainno")).Value;
                    objServiceContractor.COACH_NO = ((HiddenField)grv.FindControl("hdncoachno")).Value;
                    objServiceContractor.AVAILABILITY_DEPOT = ((HiddenField)grv.FindControl("hdnAvailblty")).Value;


                    objServiceContractor.SaveData();
                }
                if (gvAddTemp.Rows.Count != 0)
                {
                    //lblSave.Text = "FIR Completed";
                    ScriptManager.RegisterClientScriptBlock(btnSave, GetType(), "AddedProd", "alert('Fir Completed');", true);
                    foreach (GridViewRow grv in gvAddTemp.Rows)
                    {
                        ((LinkButton)grv.FindControl("lnkTempGvDefect")).Visible = true;
                        ((LinkButton)grv.FindControl("lnkTempGvAction")).Visible = true;
                        ((LinkButton)grv.FindControl("lnkTempGv")).Visible = false;
                    }
                    btnAdd.Enabled = false;
                    btnSave.Enabled = false;
                    objServiceContractor.SplitComplaint_RefNo = 1;
                }
            }
            else if (gvAddTemp.Rows.Count == 0)
            {

                ScriptManager.RegisterClientScriptBlock(btnSave, GetType(), "AddProd", "alert('First Add Product');", true);
            }
        }
        catch (Exception ex)
        {
            //lblSave.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }

    //Fill ddlFirProductDiv
    protected void lnkFirEditProdDiv_Click(object sender, EventArgs e)
    {

        objServiceContractor.State_SNo = int.Parse(hdnFirState.Value.ToString());
        objServiceContractor.City_SNo = int.Parse(hdnFirCity.Value.ToString());
        GetSCNo();
        objServiceContractor.BindProductDivDdl(ddlFirProductDiv);


    }
    
    protected void ddlFirProductDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtProductRefNo.Text = "";
            txtBatchNo.Text = "";
            if (ddlFirProductDiv.SelectedIndex != 0)
            {
                ddlProductLine.Items.Clear();
                if (ddlProductLine.Items.Count == 0)
                    ddlProductLine.Items.Add(lstItem);
                ddlProductGroup.Items.Clear();
                if (ddlProductGroup.Items.Count == 0)
                    ddlProductGroup.Items.Add(lstItem);
                ddlProduct.Items.Clear();
                if (ddlProduct.Items.Count == 0)
                    ddlProduct.Items.Add(lstItem);

                ddlMfgUnit.Items.Clear();
                if (ddlMfgUnit.Items.Count == 0)
                    ddlMfgUnit.Items.Add(lstItem);

                objServiceContractor.ProductDivision_Sno = int.Parse(ddlFirProductDiv.SelectedValue.ToString());
                lblUnit.Text = ddlFirProductDiv.SelectedItem.Text;
                //ModiFied By Pravesh To Remove AppLIANCE from PL list
                //objServiceContractor.BindProductLineDdl(ddlProductLine); 
                objServiceContractor.BindProductLineDdlRemoveAppliance(ddlProductLine);
                hdnProductDvNo.Value = ddlFirProductDiv.SelectedValue.ToString();

                //Updating MfgUnit on div change
                objServiceContractor.ProductDivision_Sno = int.Parse(hdnProductDvNo.Value.ToString());
                objServiceContractor.BindMfgDdl(ddlMfgUnit);
                if (ddlMfgUnit.Items.Count == 1)
                {
                    RequiredFieldValidatorddlMfgUnit.Enabled = false;
                }
                else
                {
                    RequiredFieldValidatorddlMfgUnit.Enabled = true;
                }
            }
        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }

    }
    
    protected void ddlProductLine_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProductLine.SelectedIndex != 0)
            {
                ddlProductGroup.Items.Clear();
                if (ddlProductGroup.Items.Count == 0)
                    ddlProductGroup.Items.Add(lstItem);
                ddlProduct.Items.Clear();
                if (ddlProduct.Items.Count == 0)
                    ddlProduct.Items.Add(lstItem);
                objServiceContractor.ProductLine_Sno = int.Parse(ddlProductLine.SelectedValue.ToString());
                //objServiceContractor.BindProductGroupDdl(ddlProductGroup);
                objServiceContractor.BindProduct(ddlProduct);

            }
        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }
    
    protected void ddlProductGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlProductGroup.SelectedIndex != 0)
            {
                ddlProduct.Items.Clear();
                if (ddlProduct.Items.Count == 0)
                    ddlProduct.Items.Add(lstItem);
                objServiceContractor.ProductGroup_SNo = int.Parse(ddlProductGroup.SelectedValue.ToString());
                objServiceContractor.BindProductDdl(ddlProduct);
            }
        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }

    protected void rdbWarranty_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdbWarranty.SelectedIndex == 1)
            {
                objServiceContractor.ProductDivision_Sno = int.Parse(hdnProductDvNo.Value.ToString());
                objServiceContractor.GetVisitChargesOnProductLineBasis(txtVisitCharges);
                ///To disable invoice date and number required field validation
                //RequiredFieldValidatortxtInvoiceDate.Enabled = false;
                RequiredFieldValidatortxtInvoiceNo.Enabled = false;
                RequiredFieldValidatortxtInvoiceDate.Enabled = false;
                lblStarInvDt.Visible = false;
                lblStarInvNum.Visible = false;
                //Add Code Binay(MTO)
                txtVisitCharges.Visible = true;
                tdVisitCharge.Visible = true;
                tdVisitAmount.Visible = true;
                trCharges.Visible = true;
            }
            else
            {
                txtVisitCharges.Text = "";
                ///To enable invoice date and number required field validation
                //RequiredFieldValidatortxtInvoiceDate.Enabled = true;
                RequiredFieldValidatortxtInvoiceNo.Enabled = true;
                RequiredFieldValidatortxtInvoiceDate.Enabled = true;
                lblStarInvDt.Visible = true;
                lblStarInvNum.Visible = true;
                //ADD Code By Binay (MTO)
                tdVisitCharge.Visible = false;
                tdVisitAmount.Visible = false;
                txtVisitCharges.Visible = false;
                trCharges.Visible = false;
            }
        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }
    
    protected void btnFirClose_Click(object sender, EventArgs e)
    {
        try
        {

            ClearFirControls();
            tbBasicRegistrationInformation.Visible = false;
            tbTempGrid.Visible = false;
        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }
    
    protected void ClearFirControls()
    {
        tbDefect.Visible = false;
        tbAction.Visible = false;
        hdnCustmerID.Value = "";
        hdnProductDvNo.Value = "";
        hdnComplaintRef.Value = "";

        lblComplainDate.Text = "";

        ddlFirProductDiv.Items.Clear();
        if (ddlFirProductDiv.Items.Count == 0)
            ddlFirProductDiv.Items.Add(lstItem);
        ddlProductLine.Items.Clear();
        if (ddlProductLine.Items.Count == 0)
            ddlProductLine.Items.Add(lstItem);
        ddlProductGroup.Items.Clear();
        if (ddlProductGroup.Items.Count == 0)
            ddlProductGroup.Items.Add(lstItem);
        ddlProduct.Items.Clear();
        if (ddlProduct.Items.Count == 0)

            ddlMfgUnit.Items.Clear();
        if (ddlMfgUnit.Items.Count == 0)
            ddlMfgUnit.Items.Add(lstItem);

        ddlProduct.Items.Add(lstItem);
        txtProductRefNo.Text = "";
        txtBatchNo.Text = "";
        txtInvoiceDate.Text = "";

        //txtWarranty.Text = "";

        hdnBaseLineID.Value = "";
        txtVisitCharges.Text = "";
        DataTable dt = (DataTable)ViewState["dtTemp"];
        dt.Rows.Clear();
        gvAddTemp.DataSource = dt;
        gvAddTemp.DataBind();
        if (dt.Rows.Count > 0)
        {
            btnSave.Enabled = true;
            btnSave.Visible = true;
            btnAdd.Enabled = true;
        }
        else
        {
            btnSave.Enabled = false;
            btnSave.Visible = false;
            btnAdd.Enabled = true;
        }
        rdbWarranty.SelectedIndex = 0;
    }
    
    protected void ClearFirControlsOnAdd()
    {
        //ddlProductLine.SelectedIndex = 0;
        //ddlProductGroup.Items.Clear();
        // if (ddlProductGroup.Items.Count == 0)
        //    ddlProductGroup.Items.Add(lstItem);
        //ddlProduct.Items.Clear();
        // if (ddlProduct.Items.Count == 0)
        //     ddlProduct.Items.Add(lstItem);
        //ddlMfgUnit.SelectedIndex = 0;
        // txtProductRefNo.Text = "";  //ProductSrno
        txtBatchNo.Text = "";
        // txtInvoiceDate.Text = "";

        // txtWarranty.Text = "";

        txtVisitCharges.Text = "";
        btnSave.Enabled = true;
        btnAdd.Enabled = true;
        //Add Code By Binay MTO
        tdVisitCharge.Visible = false;
        tdVisitAmount.Visible = false;
        txtVisitCharges.Visible = false;
        trCharges.Visible = false;
        txtSpareCharges.Text = "";
        txtTravelCharges.Text = "";
        //End
        rdbWarranty.SelectedIndex = 0;

        // RSD
        txtAvailblty.Text = "";
        txtcoachNo.Text = "";
        txtTrainNo.Text = "";
        txtequipname.Text = "";
    }
    
    //Full text search on Product Line
    protected void btnGoPL_Click(object sender, EventArgs e)
    {
        objServiceContractor.ProductDivision_Sno = int.Parse(hdnProductDvNo.Value.ToString());
        objServiceContractor.ProductLine_Desc = txtFindPL.Text.ToString();
        objServiceContractor.BindProductLineDdlForFind(ddlProductLine);

        ////To fill PL
        if (txtFindPL.Text == "")
        {
            objServiceContractor.ProductDivision_Sno = int.Parse(hdnProductDvNo.Value.ToString());
            objServiceContractor.BindProductLineDdl(ddlProductLine);
        }
        //Code Modify By Binay


    }
  
    //Full text search on Product Group
    protected void btnGoPG_Click(object sender, EventArgs e)
    {
        if (ddlProductLine.SelectedIndex != 0)
        {
            objServiceContractor.ProductLine_Sno = int.Parse(ddlProductLine.SelectedValue.ToString());
            objServiceContractor.ProductGroup_Desc = txtFindPG.Text.ToString();
            objServiceContractor.BindProductGroupDdlForFind(ddlProductGroup);
        }

        ////To fill PG
        if (txtFindPG.Text == "")
        {
            if (ddlProductLine.SelectedIndex != 0)
                objServiceContractor.ProductLine_Sno = int.Parse(ddlProductLine.SelectedValue.ToString());
            objServiceContractor.BindProductGroupDdl(ddlProductGroup);
        }

    }
    //Full text search on Product 
    protected void btnGoP_Click(object sender, EventArgs e)
    {
        if (ddlProductGroup.SelectedIndex != 0)
        {
            objServiceContractor.ProductGroup_SNo = int.Parse(ddlProductGroup.SelectedValue.ToString());
            objServiceContractor.Product_Desc = txtFindP.Text.ToString();
            objServiceContractor.BindProductDdlForFind(ddlProduct);
        }

        ////To fill P
        if (txtFindP.Text == "")
        {
            if (ddlProductGroup.SelectedIndex != 0)
                objServiceContractor.ProductGroup_SNo = int.Parse(ddlProductGroup.SelectedValue.ToString());
            objServiceContractor.BindProductDdl(ddlProduct);
        }
    }

    #endregion FIR

    #region Defect
    //////Defect Related////////
    protected void btnAddTempDefect_Click(object sender, EventArgs e)
    {
        try
        {
            if (trDefectMake.Visible == true)
            {
                if (txtDefectMakeCapcitor.Text == "")
                    ScriptManager.RegisterClientScriptBlock(btnAddTempDefect, GetType(), "MakeCap", "alert('Enter Make Capcitor');", true);
                else
                {
                    CreatDefectRow();
                    BindTempDefectGrid();
                    ddlDefectCat.SelectedIndex = 0;
                    if (gvDefectTemp.Rows.Count != 0)
                    {
                        RequiredFieldValidatorddlDefect.Enabled = false;
                        RegularExpressionValidatorddlDefectCat.Enabled = false;
                    }
                    else
                    {
                        RequiredFieldValidatorddlDefect.Enabled = true;
                        RegularExpressionValidatorddlDefectCat.Enabled = true;
                    }
                }
            }
            else
            {
                CreatDefectRow();
                BindTempDefectGrid();
                ddlDefectCat.SelectedIndex = 0;
                //Add code By Binay-03-12-2009
                ddlDefectCat_SelectedIndexChanged(null, null);
                //end
                ddlDefect.SelectedIndex = 0;
                if (gvDefectTemp.Rows.Count != 0)
                {
                    RequiredFieldValidatorddlDefect.Enabled = false;
                    RegularExpressionValidatorddlDefectCat.Enabled = false;
                }
                else
                {
                    RequiredFieldValidatorddlDefect.Enabled = true;
                    RegularExpressionValidatorddlDefectCat.Enabled = true;
                }
            }
            //txtDefectQty.Text = (int.Parse(txtDefectQty.Text.ToString()) + 1).ToString();//Addition on instruction from Seema
        }
        catch (Exception ex)
        {
            //lblDefectMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }

    protected void CreatDefectRow()
    {

        DataTable dtTempDefect = (DataTable)ViewState["dtTempDefect"];
        DataRow drw = dtTempDefect.NewRow();
        drw["DefectCategory"] = ddlDefectCat.SelectedItem.Text.ToString();
        drw["DefectCategory_Sno"] = int.Parse(ddlDefectCat.SelectedValue.ToString());
        drw["Defect_Desc"] = ddlDefect.SelectedItem.Text.ToString();
        drw["Defect_Sno"] = int.Parse(ddlDefect.SelectedValue.ToString());

        drw["MAKE_CAP"] = txtDefectMakeCapcitor.Text.Trim();
        txtDefectMakeCapcitor.Text = "";
        drw["NUM_OF_DEF"] = 1;//int.Parse(txtDefectQty.Text.ToString());//modification on instruction from Seema
        drw["SRNO"] = 0;
        int flag = 0;
        int count = dtTempDefect.Rows.Count;
        if (dtTempDefect.Rows.Count != 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (int.Parse(dtTempDefect.Rows[i]["DefectCategory_Sno"].ToString()) == int.Parse(ddlDefectCat.SelectedValue.ToString()) && int.Parse(dtTempDefect.Rows[i]["Defect_Sno"].ToString()) == int.Parse(ddlDefect.SelectedValue.ToString()))
                {
                    flag = 1;
                    goto a;
                }
            }

            dtTempDefect.Rows.Add(drw);
            ViewState["dtTempDefect"] = dtTempDefect;
            btnSaveDefect.Enabled = true;

        }
        else
        {
            dtTempDefect.Rows.Add(drw);
            ViewState["dtTempDefect"] = dtTempDefect;
            btnSaveDefect.Enabled = true;
        }
        dtTempDefect.AcceptChanges();
    a: if (flag == 1)
            ScriptManager.RegisterClientScriptBlock(btnAddTempDefect, GetType(), "defectAddScript", "alert('Defect Already Exists');", true); ;
    }
    // incraese size of int16 to Int32 
    protected void CreatTempDefectTable()
    {
        DataColumn Sno = new DataColumn("Sno", System.Type.GetType("System.Int32"));
        DataColumn DefectCategory = new DataColumn("DefectCategory", System.Type.GetType("System.String"));
        DataColumn DefectCategory_Sno = new DataColumn("DefectCategory_Sno", System.Type.GetType("System.Int32"));
        DataColumn Defect_Sno = new DataColumn("Defect_Sno", System.Type.GetType("System.Int32"));
        DataColumn Defect_Desc = new DataColumn("Defect_Desc", System.Type.GetType("System.String"));

        DataColumn MAKE_CAP = new DataColumn("MAKE_CAP", System.Type.GetType("System.String"));
        DataColumn NUM_OF_DEF = new DataColumn("NUM_OF_DEF", System.Type.GetType("System.Int32"));
        DataColumn SRNO = new DataColumn("SRNO", System.Type.GetType("System.Int32"));
        dtTempDefect.Columns.Add(Sno);
        dtTempDefect.Columns.Add(SRNO);
        dtTempDefect.Columns.Add(DefectCategory);
        dtTempDefect.Columns.Add(DefectCategory_Sno);
        dtTempDefect.Columns.Add(Defect_Sno);
        dtTempDefect.Columns.Add(Defect_Desc);

        dtTempDefect.Columns.Add(MAKE_CAP);
        dtTempDefect.Columns.Add(NUM_OF_DEF);
        ViewState["dtTempDefect"] = dtTempDefect;
    }
    protected void BindTempDefectGrid()
    {
        DataTable dtTempDefect = (DataTable)ViewState["dtTempDefect"];
        for (int intCounter = 0; intCounter < dtTempDefect.Rows.Count; intCounter++)
        {
            dtTempDefect.Rows[intCounter]["Sno"] = intCounter + 1;
        }
        gvDefectTemp.DataSource = dtTempDefect;
        gvDefectTemp.DataBind();

        txtDefectQty.Text = dtTempDefect.Rows.Count.ToString();

    }
    protected void ddlDefectCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlDefect.Items.Clear();
            ddlDefect.Items.Add(lstItem);
            if (ddlDefectCat.SelectedIndex != 0)
            {
                objServiceContractor.Defect_Category_SNo = int.Parse(ddlDefectCat.SelectedValue.ToString());
                objServiceContractor.BindDefectDdl(ddlDefect);
            }
        }
        catch (Exception ex)
        {
            //lblDefectMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }

    protected void ddlDefect_SelectedIndexChanged(object sender, EventArgs e)
    {
        objServiceContractor.ProductDivision_Sno = int.Parse(hdnDefectProductDiv_Sno.Value.ToString());
        ds = objServiceContractor.GetProductDivCode();
        if (ds.Tables[0].Rows.Count != 0)
        {
            objServiceContractor.MFGUNIT = ds.Tables[0].Rows[0]["unit_code"].ToString();
            ds.Tables[0].Rows.Clear();
        }

        if (ddlDefectCat.SelectedIndex != 0)
        {
            objServiceContractor.Defect_Category_SNo = int.Parse(ddlDefectCat.SelectedValue.ToString());
            ds = objServiceContractor.GetDefectCatCode();
        }
        if (ds.Tables[0].Rows.Count != 0)
        {
            objServiceContractor.DEF_CAT_CODE = ds.Tables[0].Rows[0]["Defect_Category_Code"].ToString();
            ds.Tables[0].Rows.Clear();
        }

        if (ddlDefect.SelectedIndex != 0)
            objServiceContractor.Defect_SNo = int.Parse(ddlDefect.SelectedValue.ToString());
        ds = objServiceContractor.GetDefectCode();
        if (ds.Tables[0].Rows.Count != 0)
        {
            objServiceContractor.DEFCD = ds.Tables[0].Rows[0]["Defect_Code"].ToString();
            ds.Tables[0].Rows.Clear();
        }

        if (objServiceContractor.MFGUNIT == "BW")
        {
            if (objServiceContractor.DEF_CAT_CODE == "fan-cf-ss" && objServiceContractor.DEFCD == "FAN-CF-SS001")
            {
                trDefectMake.Visible = true;
                RequiredFieldValidatortxtDefectMakeCapcitor.Enabled = true;
            }
            else if (objServiceContractor.DEF_CAT_CODE == "fan-tpwo-ss" && objServiceContractor.DEFCD == "FAN-TPWO-SS001")
            {
                trDefectMake.Visible = true;
                RequiredFieldValidatortxtDefectMakeCapcitor.Enabled = true;
            }

            else
            {
                trDefectMake.Visible = false;
                RequiredFieldValidatortxtDefectMakeCapcitor.Enabled = false;
            }
        }
        else
        {
            trDefectMake.Visible = false;
            RequiredFieldValidatortxtDefectMakeCapcitor.Enabled = false;
        }
        txtDefectMakeCapcitor.Text = "";
    }

    protected void llnkTempGvDefect_Click(object sender, EventArgs e)
    {
        try
        {
            txtDefectDate.Text = DateTime.Today.Date.ToString("MM/dd/yyyy");
            lblDefectMsg.Text = "";

            ClearDefectControls();
            LinkButton lnk = (LinkButton)sender;
            objServiceContractor.Complaint_RefNo = lnk.CommandArgument.ToString();
            objServiceContractor.SplitComplaint_RefNo = int.Parse(lnk.CommandName.ToString());

            //To view the attributes added in ppr
            //objServiceContractor.FillAttributes(gvAttributeInDefectSection);
            //gvAttribute.Visible = false;


            //to view the entered defects
            DataSet dsPPR1 = objServiceContractor.GetPPRDefect();
            gvDefectTemp.DataSource = dsPPR1;
            gvDefectTemp.DataBind();
            if (dsPPR1.Tables[0].Rows.Count != 0)
            {
                DataTable dt = dsPPR1.Tables[0];
                ViewState["dtTempDefect"] = dt;
                RequiredFieldValidatorddlDefect.Enabled = false;
                RegularExpressionValidatorddlDefectCat.Enabled = false;
            }
            else
            {
                RequiredFieldValidatorddlDefect.Enabled = true;
                RegularExpressionValidatorddlDefectCat.Enabled = true;
            }

            foreach (GridViewRow grv in gvAddTemp.Rows)
            {
                if (objServiceContractor.Complaint_RefNo == ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value.ToString()
                        && objServiceContractor.SplitComplaint_RefNo == int.Parse(((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value.ToString()))
                {
                    lblDefectProductLine.Text = ((Label)grv.FindControl("lblgvProductLine")).Text;
                    objServiceContractor.ProductLine_Sno = int.Parse(((HiddenField)grv.FindControl("hdngvProductLineSno")).Value.ToString());

                    //Line added By Mukesh TO set ProductLineSno for PPR_Code in ProdutLineMaster
                    hdnDefectProductLine_Sno.Value = ((HiddenField)grv.FindControl("hdngvProductLineSno")).Value;



                    hdnDefectBatch.Value = ((Label)grv.FindControl("lblgvBatch")).Text.ToString();
                    hdnDefectProductGroup_Sno.Value = ((HiddenField)grv.FindControl("hdngvProductGroupSno")).Value;
                    hdnDefectProduct_Sno.Value = ((HiddenField)grv.FindControl("hdngvProductSno")).Value;
                    hdnDefectComplaintLoggDate.Value = ((HiddenField)grv.FindControl("hdngvComplaintDate")).Value;
                    hdnDefectCustomerID.Value = ((HiddenField)grv.FindControl("hdngvCustomerID")).Value;
                    hdnDefectComplaintRef_No.Value = ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value;
                    hdnDefectProductDiv_Sno.Value = ((HiddenField)grv.FindControl("hdngvProductDivisionSno")).Value;
                    hdnDefectSliptComplaint.Value = ((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value;
                    hdnDefectCallStatus.Value = ((HiddenField)grv.FindControl("hdngvCallStatus")).Value;
                    lblDefComp.Text = ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value + "/" + ((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value;
                    lblDefProductDiv.Text = ((Label)grv.FindControl("lblgvProductDivision")).Text.ToString();

                    //New Change for setting MFGUNIT
                    hdnDefectMFGUNIT.Value = ((HiddenField)grv.FindControl("hdngvManufacture_SNo")).Value;
                    hdnDefectProductSrNo.Value = ((HiddenField)grv.FindControl("hdngvProduct_SerialNo")).Value;

                }
            }
            ddlDefectCat.Items.Clear();
            ddlDefectCat.Items.Add(lstItem);
            objServiceContractor.BindDefectCatDdl(ddlDefectCat);
            tbIntialization.Visible = false;
            tbBasicRegistrationInformation.Visible = false;
            tbDefect.Visible = true;
            tbAction.Visible = false;
            tbViewAttribute.Visible = false;
            //defect attribute handling
            GetDefectAttributes();

            GetDefectAttributesDataFromPPR();
        }
        catch (Exception ex)
        {
            //lblDefectMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    protected void lnkDefectGv_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        int rowNo = int.Parse(lnk.CommandArgument.ToString());
        objServiceContractor.SRNO = int.Parse(lnk.CommandName.ToString());
        DataTable dtTempDefect = (DataTable)ViewState["dtTempDefect"];
        objServiceContractor.DeleteDefect();
        dtTempDefect.Rows[rowNo - 1].Delete();
        dtTempDefect.AcceptChanges();
        ViewState["dtTempDefect"] = dtTempDefect;
        BindTempDefectGrid();
    }

    protected void InsertDefectInPPR(string strFlag, string strDefect_Approval_Or_Entry)
    {
        //if (int.Parse(hdnDefectCallStatus.Value.ToString()) != 23)
        //{
        objServiceContractor.ServiceDate = DateTime.Parse("1/1/1900 12:00:00 AM");
        objServiceContractor.ServiceAmt = 0;
        objServiceContractor.ServiceNumber = "";
        DataSet ds;

        #region set values
        objServiceContractor.SplitComplaint_RefNo = int.Parse(hdnDefectSliptComplaint.Value.ToString());
        string[] strarrComplainNo;
        strarrComplainNo = hdnDefectComplaintRef_No.Value.ToString().Split('/');
        objServiceContractor.Complaint_RefNo = strarrComplainNo[0];

        //objServiceContractor.MTH_NAME = DateTime.Now.Month.ToString();
        objServiceContractor.MTH_NAME = String.Format("{0:MMMM}", DateTime.Now).ToString();

        objServiceContractor.MANF_PERIOD = hdnDefectBatch.Value.ToString();
        if (objServiceContractor.MANF_PERIOD.Length > 2)
        {
            objServiceContractor.MANF_PERIOD = objServiceContractor.MANF_PERIOD.Substring(0, 2);
        }
        //*****Code added By Mukesh To set PPR_Code Of product Line
        objServiceContractor.ProductLine_Sno = int.Parse(hdnDefectProductLine_Sno.Value.ToString());
        ds = objServiceContractor.GetProductLineCode();
        if (ds.Tables[0].Rows.Count != 0)
        {
            objServiceContractor.PRDCD = ds.Tables[0].Rows[0]["PPR_code"].ToString();

        }
        //objServiceContractor.PRDCD = "sdafsd";
        //*****Code added By Mukesh To set PPR_Code Of product Line

        if (strDefect_Approval_Or_Entry == "APP")
        {
            objServiceContractor.REMARK = txtDefectServiceActionRemarks.Text.Trim();
        }
        else if (strDefect_Approval_Or_Entry == "ENT")
        {
            objServiceContractor.OBSERV = txtDefectServiceActionRemarks.Text;
        }

        /////Get Product Group Code
        objServiceContractor.ProductGroup_SNo = int.Parse(hdnDefectProductGroup_Sno.Value.ToString());
        ds = objServiceContractor.GetProductGroupCode();
        if (ds.Tables[0].Rows.Count != 0)
        {
            objServiceContractor.SPCODE = ds.Tables[0].Rows[0]["ProductGroup_Code"].ToString();
            objServiceContractor.SPNAME = ds.Tables[0].Rows[0]["ProductGroup_Desc"].ToString();
        }


        if (hdnDefectComplaintLoggDate.Value != null)
            objServiceContractor.REP_DAT = DateTime.Parse(hdnDefectComplaintLoggDate.Value.ToString());
        GetSCNo();

        objServiceContractor.CONTRA_NAME = objServiceContractor.SC_Name;
        objServiceContractor.EmpID = Membership.GetUser().ToString();
        ds = objServiceContractor.GetDefectBranchCode();
        if (ds.Tables[0].Rows.Count != 0)
        {
            objServiceContractor.BRCD = ds.Tables[0].Rows[0]["Branch_Name"].ToString();//(After Change) Branch_Code
        }

        ds = objServiceContractor.GetDefectRegionCode();
        if (ds.Tables[0].Rows.Count != 0)
            objServiceContractor.RGNCD = ds.Tables[0].Rows[0]["Region_Desc"].ToString();// (After change)Region_Code
        if (objServiceContractor.RGNCD == null)
        { objServiceContractor.RGNCD = ""; }

        objServiceContractor.ORIGIN = objServiceContractor.BRCD;

        //**** Change as per CG
        //if (txtRating.Visible == true)
        //objServiceContractor.RATING = txtRating.Text.ToString();
        objServiceContractor.RATING = "";
        objServiceContractor.CustomerId = int.Parse(hdnDefectCustomerID.Value.ToString());
        ds = objServiceContractor.GetCustomerName();
        if (ds.Tables[0].Rows.Count != 0)
            objServiceContractor.CUST_NAME = ds.Tables[0].Rows[0]["FirstName"].ToString();

        objServiceContractor.MODEL = "";
        objServiceContractor.SERIAL_NUM = "";
       

        objServiceContractor.SUPP_CD = "";  // Not Applicable
        objServiceContractor.TYP = ""; //Not Applicable

        objServiceContractor.SOMA_SRNO = hdnDefectComplaintRef_No.Value.ToString();

            /////Get Product  Code
        objServiceContractor.CATREF_NO = "";
        objServiceContractor.CATREF_DESC = "";
        objServiceContractor.RATING_STATUS = "";

        objServiceContractor.Product_SNo = int.Parse(hdnDefectProduct_Sno.Value.ToString());
        ds = objServiceContractor.GetProductCode();
        if (ds.Tables[0].Rows.Count != 0)
        {
            objServiceContractor.CATREF_NO = ds.Tables[0].Rows[0]["Product_Code"].ToString();
            objServiceContractor.CATREF_DESC = ds.Tables[0].Rows[0]["Product_Desc"].ToString();
            objServiceContractor.RATING_STATUS = ds.Tables[0].Rows[0]["Rating_Status"].ToString();
        }
        if (objServiceContractor.RATING_STATUS == null || objServiceContractor.RATING_STATUS == " ")
        { objServiceContractor.RATING_STATUS = ""; }

        objServiceContractor.AVR_SRNO = "";


        //objServiceContractor.MAKE_CAP = txtDefectMakeCapcitor.Text;

        if (hdnDefectMFGUNIT.Value.ToString() != "")
            objServiceContractor.Manufacture_SNo = int.Parse(hdnDefectMFGUNIT.Value.ToString());
        ds = objServiceContractor.GETMFG();
        if (ds.Tables[0].Rows.Count != 0)
        {
            objServiceContractor.MFGUNIT = ds.Tables[0].Rows[0]["Manufacture_Unit"].ToString();
            if (objServiceContractor.MFGUNIT == null)
                objServiceContractor.MFGUNIT = "";
        }
        else
        {
            if (objServiceContractor.MFGUNIT == null)
                objServiceContractor.MFGUNIT = "";
        }
        ////Seting ProductSerial_No/////
        objServiceContractor.ProductSerial_No = hdnDefectProductSrNo.Value.ToString();



       // bhawesh added mto RSD
        objServiceContractor.EQUIPMENT_NAME = txtAttEquipmentName.Text.Trim();
        objServiceContractor.AVAILABILITY_DEPOT = txtAttAvailability.Text.Trim();
        objServiceContractor.COACH_NO =  txtAttCoachNo.Text.Trim();
        objServiceContractor.TRAIN_NO =  txtAttTrainNo.Text.Trim();
    
     
        #endregion set values

        if (gvDefectTemp.Rows.Count != 0)
        {
            #region gvDefectTemp.Rows.Count not zero
            foreach (GridViewRow grv in gvDefectTemp.Rows)
            {
                objServiceContractor.Defect_Category_SNo = int.Parse(((HiddenField)grv.FindControl("hdngvDefectCategory_Sno")).Value.ToString());
                ds = objServiceContractor.GetDefectCatCode();
                if (ds.Tables[0].Rows.Count != 0)
                { objServiceContractor.DEF_CAT_CODE = ds.Tables[0].Rows[0]["Defect_Category_Code"].ToString(); }

                objServiceContractor.Defect_SNo = int.Parse(((HiddenField)grv.FindControl("hdngvDefect_Sno")).Value.ToString());
                ds = objServiceContractor.GetDefectCode();
                if (ds.Tables[0].Rows.Count != 0)
                { objServiceContractor.DEFCD = ds.Tables[0].Rows[0]["Defect_Code"].ToString(); }
                if (((Label)grv.FindControl("txtQty")).Text.ToString() != "")
                    objServiceContractor.NUM_OF_DEF = int.Parse(((Label)grv.FindControl("txtQty")).Text.ToString());

                objServiceContractor.MAKE_CAP = "";
                objServiceContractor.MAKE_CAP = ((Label)grv.FindControl("lblgvMAKE_CAP")).Text.ToString();

                if (int.Parse(((HiddenField)grv.FindControl("hdnGvDefectSRNO")).Value.ToString()) == 0)
                {
                    objServiceContractor.InsertDefect();

                }
                //updating attributes in ppr
                objServiceContractor.UpdateAttributes();

            }


            GetSCNo();
            objServiceContractor.LastComment = txtDefectServiceActionRemarks.Text;
            objServiceContractor.EmpID = Membership.GetUser().UserName.ToString();
            if (txtDefectDate.Text != "")
                objServiceContractor.ActionDate = Convert.ToDateTime(txtDefectDate.Text.ToString());
            GetActionTime(ddlDefHr, ddlDefMin, ddlDefAM);
            objServiceContractor.DefectAccFlag = "N";
            objServiceContractor.ActionEntry();
            if (strFlag == "Y")
            {
                ScriptManager.RegisterClientScriptBlock(btnSaveDefect, GetType(), "DefEntryDone", "alert('Defect Entry Saved & Completed');", true);
                ClearDefectControls();
            }
            //lblDefectMsg.Text = "Defect Entry Completed";
            #endregion gvDefectTemp.Rows.Count
        }
        else
        {
            // ScriptManager.RegisterClientScriptBlock(btnSaveDefect, GetType(), "AddDef", "alert('You have not added any defects to proceed');", true);
            //lblDefectMsg.Text = "First Add Defect";
            #region gvDefectTemp.Rows.Count zero
            //Add By Binay FOR MTO 16-11-2009
            if (gvDefectTemp.Rows.Count != 0)
            {
                if (ddlDefectCat.SelectedIndex == 0)
                {
                    lblDefectMsg.Text = "Select Defect Category to Proceed";
                }
                else if (ddlDefect.SelectedIndex == 0)
                {
                    lblDefectMsg.Text = "Select Defect to Proceed";
                }
                else
                {
                    if (ddlDefectCat.SelectedIndex != 0)
                    {
                        objServiceContractor.Defect_Category_SNo = int.Parse(ddlDefectCat.SelectedValue.ToString());
                        ds = objServiceContractor.GetDefectCatCode();
                    }
                    if (ds.Tables[0].Rows.Count != 0)
                    { objServiceContractor.DEF_CAT_CODE = ds.Tables[0].Rows[0]["Defect_Category_Code"].ToString(); }

                    if (ddlDefect.SelectedIndex != 0)
                        objServiceContractor.Defect_SNo = int.Parse(ddlDefect.SelectedValue.ToString());
                    ds = objServiceContractor.GetDefectCode();
                    if (ds.Tables[0].Rows.Count != 0)
                    { objServiceContractor.DEFCD = ds.Tables[0].Rows[0]["Defect_Code"].ToString(); }


                    if (txtDefectQty.Text.ToString() != "")
                        objServiceContractor.NUM_OF_DEF = int.Parse(txtDefectQty.Text.ToString());



                    objServiceContractor.InsertDefect();
                    //////Changing Call Status in Base Line
                    GetSCNo();
                    objServiceContractor.LastComment = txtDefectServiceActionRemarks.Text;
                    objServiceContractor.EmpID = Membership.GetUser().UserName.ToString();
                    if (txtDefectDate.Text != "")
                        objServiceContractor.ActionDate = Convert.ToDateTime(txtDefectDate.Text.ToString());
                    GetActionTime(ddlDefHr, ddlDefMin, ddlDefAM);
                    objServiceContractor.DefectAccFlag = "N";
                    objServiceContractor.ActionEntry();
                    lblDefectMsg.Text = "Defect Entry Completed";
                    ScriptManager.RegisterClientScriptBlock(btnSaveDefect, GetType(), "DefEntryDone", "alert('Defect Entry Completed');", true);

                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(btnSaveDefect, GetType(), "AddDef", "alert('You have not added any defects to proceed');", true);
            }
            //END
            #endregion gvDefectTemp.Rows.Count zero
        }

        //refreshing defect grid
        /////************************//////////////
        //to view the entered defects
        if (strDefect_Approval_Or_Entry == "APP")
        {
            DataSet dsPPR1 = objServiceContractor.GetPPRDefectAfterApproval();
            gvDefectTemp.DataSource = dsPPR1;
            gvDefectTemp.DataBind();
        }
        ////////////////////////////////////////////          
    }
    protected void btnSaveDefect_Click(object sender, EventArgs e)
    {
        try
        {
            //Logic for just entering the defect without Approving The defect
            objServiceContractor.Ready_For_Push = 0;
            // objServiceContractor.CallStatus = 27;---MTS
            //MTO Case Code By Binay
            objServiceContractor.CallStatus = 56;
            //END
            InsertDefectInPPR("Y", "ENT");
            txtDefectQty.Text = "0";

            //Changes to show defect after saving - bhuwanesh 08-Feb 2010-START
            DataSet dsPPR = objServiceContractor.GetPPRDefect();
            txtDefectQty.Text = dsPPR.Tables[0].Rows.Count.ToString();
            gvDefectTemp.DataSource = dsPPR;
            gvDefectTemp.DataBind();
            if (dsPPR.Tables[0].Rows.Count != 0)
            {
                DataTable dt = dsPPR.Tables[0];
                ViewState["dtTempDefect"] = dt;
                RequiredFieldValidatorddlDefect.Enabled = false;
                RegularExpressionValidatorddlDefectCat.Enabled = false;
            }
            else
            {
                RequiredFieldValidatorddlDefect.Enabled = true;
                RegularExpressionValidatorddlDefectCat.Enabled = true;
            }
            BindTempDefectGrid();
            btnSaveDefect.Enabled = false;
            //Changes to show defect after saving - bhuwanesh 08-Feb 2010-END

        }


        catch (Exception ex)
        {
            //lblDefectMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }

    protected void btnDefectApprove_Click(object sender, EventArgs e)
    {
        try
        {
            objServiceContractor.ServiceDate = DateTime.Parse("1/1/1900 12:00:00 AM");
            objServiceContractor.ServiceAmt = 0;
            objServiceContractor.ServiceNumber = "";
            // Logic for approving the defect
            objServiceContractor.Ready_For_Push = 1;
            //objServiceContractor.CallStatus = 11;
            //Code By Binay-MTO
            objServiceContractor.CallStatus = 54;
            //END
            if (gvDefectTemp.Rows.Count != 0)
            {
                foreach (GridViewRow grv in gvDefectTemp.Rows)
                {
                    int i = int.Parse(((HiddenField)grv.FindControl("hdnGvDefectSRNO")).Value.ToString());
                    if (int.Parse(((HiddenField)grv.FindControl("hdnGvDefectSRNO")).Value.ToString()) == 0)
                    { InsertDefectInPPR("N", "APP"); }
                }
            }
            else if (gvDefectTemp.Rows.Count == 0)
            {
                InsertDefectInPPR("N", "APP");
            }
            objServiceContractor.DefectAccFlag = "Y";

            //////Changing Call Status in Base Line
            if (gvDefectTemp.Rows.Count != 0)
            {
                GetSCNo();

                objServiceContractor.SplitComplaint_RefNo = int.Parse(hdnDefectSliptComplaint.Value.ToString());
                string[] strarrComplainNo;
                strarrComplainNo = hdnDefectComplaintRef_No.Value.ToString().Split('/');
                objServiceContractor.Complaint_RefNo = strarrComplainNo[0];
                objServiceContractor.REMARK = txtDefectServiceActionRemarks.Text.Trim();
                objServiceContractor.LastComment = txtDefectServiceActionRemarks.Text;
                objServiceContractor.EmpID = Membership.GetUser().UserName.ToString();
                if (txtDefectDate.Text != "")
                    objServiceContractor.ActionDate = Convert.ToDateTime(txtDefectDate.Text.ToString());
                GetActionTime(ddlDefHr, ddlDefMin, ddlDefAM);
                objServiceContractor.ActionEntry();
                objServiceContractor.UpdateReadyFlag();
                //updating attributes in ppr
                objServiceContractor.RATING = "";
              
                //if (txtAt_TrainNo.Visible == true)
                //    objServiceContractor.APPL = txtAt_TrainNo.Text;
                //objServiceContractor.LOAD = "";
                //if (txtLOAD.Visible == true)
                //    objServiceContractor.LOAD = txtLOAD.Text;
                objServiceContractor.MODEL = "";
                objServiceContractor.SERIAL_NUM = "";

                objServiceContractor.AVR_SRNO = "";
          
                //objServiceContractor.UpdateAttributes();
                ////////////////////////////////////////
                ScriptManager.RegisterClientScriptBlock(btnDefectApprove, GetType(), "DefApprov", "alert('Defect(s) Approved');", true);

                ClearDefectControls();
                tbDefect.Visible = false;

                //To refresh grid 2 on defect approval///
                objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvCustDetail);

                if (gvAddTemp.Rows.Count != 0)
                {
                    objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvAddTemp);

                }
                /////////////////////////////////////////

                //lblDefectMsg.Text = "Defect Approval Completed";
                ClearDefectControls();
                txtDefectQty.Text = "0";
                Boolean flag = false;

                for (j = 0; j < ddlActionStatus.Items.Count; j++)
                {
                    if (ddlActionStatus.Items[j].Value == "57")
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag == false)
                {

                    ddlActionStatus.Items.Insert(6, new ListItem("(Closure)Resolved by Replacement", "57"));
                    ddlActionStatus.Items.Insert(7, new ListItem("(Closure)Resolved by Repair", "58"));
                    ddlActionStatus.Items.Insert(8, new ListItem("(Closure)Complaint Cancelled", "59"));
                    ddlActionStatus.Items.Insert(9, new ListItem("(Closure)Resolved by repair with replacement of spare", "60"));
                    ddlActionStatus.Items.Insert(10, new ListItem("(Closure)Complaint Resolved and Completed", "61"));
                }
            }
        }
        catch (Exception ex)
        {
            //lblDefectMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }

    }

    protected void btnDefectClose_Click(object sender, EventArgs e)
    {
        try
        {
            ClearDefectControls();
            tbDefect.Visible = false;
            txtDefectQty.Text = "0";
        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }

    protected void ClearDefectControls()
    {
        ddlDefectCat.SelectedIndex = 0;
        ddlDefect.Items.Clear();
        ddlDefect.Items.Add(lstItem);
        txtDefectMakeCapcitor.Text = "";
        DataTable ds = (DataTable)ViewState["dtTempDefect"];
        ds.Rows.Clear();
        gvDefectTemp.DataSource = ds;
        gvDefectTemp.DataBind();
        txtDefectServiceActionRemarks.Text = "";
   }

    #endregion Defect

    #region Action
    //////Action Related////////

    protected void llnkTempGvAction_Click(object sender, EventArgs e)
    {
        try
        {
            txtActionEntryDate.Text = DateTime.Today.Date.ToString("MM/dd/yyyy");
            lblActionMsg.Text = "";
            tbIntialization.Visible = false;
            tbBasicRegistrationInformation.Visible = false;
            tbAction.Visible = true;
            tbDefect.Visible = false;
            LinkButton lnk = (LinkButton)sender;
            objServiceContractor.Complaint_RefNo = lnk.CommandArgument.ToString();
            objServiceContractor.SplitComplaint_RefNo = int.Parse(lnk.CommandName.ToString());
            foreach (GridViewRow grv in gvAddTemp.Rows)
            {
                if (objServiceContractor.Complaint_RefNo == ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value.ToString()
                    && objServiceContractor.SplitComplaint_RefNo == int.Parse(((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value.ToString()))
                {
                    lblActionProductDiv.Text = ((Label)grv.FindControl("lblgvProductDivision")).Text.ToString();
                    lblActionComplaintRefNo.Text = ((HiddenField)grv.FindControl("hdngvComplaint_RefNo")).Value.ToString() + "/" + ((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value.ToString();
                    lblActionProduct.Text = ((Label)grv.FindControl("lblgvProduct")).Text.ToString();
                    lblActionBatch.Text = ((Label)grv.FindControl("lblgvBatch")).Text.ToString();
                    hdnActionCallStatusID.Value = ((HiddenField)grv.FindControl("hdngvCallStatus")).Value;
                    hdnActionSplitNo.Value = ((HiddenField)grv.FindControl("hdngvSplitComplaint_RefNo")).Value;
                    hdnActionWarrantyStatus.Value = ((Label)grv.FindControl("lblgvWarrantyStatus")).Text.ToString();
                    hdnActionSLADate.Value = ((HiddenField)grv.FindControl("hdngvSLADate")).Value;
                    lblActionWarranty.Text = ((Label)grv.FindControl("lblgvWarrantyStatus")).Text.ToString();

                }
            }
            string strWarrantyStatus;
            strWarrantyStatus = hdnActionWarrantyStatus.Value.ToString();
            if (strWarrantyStatus.ToUpper() == "Y")
            {
                rqftxtServiceAmount.Enabled = false;
                rqftxtServiceDate.Enabled = false;
                rqftxtServiceNumber.Enabled = false;

                trServiceDate.Visible = false;
                trServiceNumber.Visible = false;
                trServiceAmount.Visible = false;
            }
            else if (strWarrantyStatus.ToUpper() == "N")
            {
                rqftxtServiceAmount.Enabled = true;
                rqftxtServiceDate.Enabled = true;
                rqftxtServiceNumber.Enabled = true;

                trServiceDate.Visible = true;
                trServiceNumber.Visible = true;
                trServiceAmount.Visible = true;
            }
        }
        catch (Exception ex)
        {
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
   
    protected void btnSaveAction_Click(object sender, EventArgs e)
    {
        try
        {
            //To maitain paging in GV Fresh/////////////////
            objServiceContractor.FirstRow = int.Parse(ViewState["FirstRow"].ToString());
            objServiceContractor.LastRow = int.Parse(ViewState["LastRow"].ToString());
            ///////////////////////////////////////////////
            string strSLADate = hdnActionSLADate.Value.ToString();
            ///Testing/////
            DateTime dtSLADate = Convert.ToDateTime(strSLADate);
            strSLADate = dtSLADate.ToString("MM/dd/yyyy");
            strSLADate = strSLADate + " 12:00:00 AM";
            ///Testing/////
            objServiceContractor.ServiceDate = DateTime.Parse("1/1/1900 12:00:00 AM");
            objServiceContractor.ServiceAmt = 0;
            objServiceContractor.ServiceNumber = "";
            objServiceContractor.SplitComplaint_RefNo = int.Parse(hdnActionSplitNo.Value.ToString());

            //To make Service attributes optional when complaint under warranty
            string strWarrantyStatus;
            strWarrantyStatus = hdnActionWarrantyStatus.Value.ToString();
            if (strWarrantyStatus.ToUpper() == "Y")
            {
                rqftxtServiceAmount.Enabled = false;
                rqftxtServiceDate.Enabled = false;
                rqftxtServiceNumber.Enabled = false;
                lblStarServiceAmount.Visible = false;
                lblStarServiceDate.Visible = false;
                lblStarServiceNumber.Visible = false;
            }
            else if (strWarrantyStatus.ToUpper() == "N")
            {
                rqftxtServiceAmount.Enabled = true;
                rqftxtServiceDate.Enabled = true;
                rqftxtServiceNumber.Enabled = true;
                lblStarServiceAmount.Visible = true;
                lblStarServiceDate.Visible = true;
                lblStarServiceNumber.Visible = true;
            }
            string[] strarrComplainNo;
            strarrComplainNo = lblActionComplaintRefNo.Text.ToString().Split('/');
            objServiceContractor.Complaint_RefNo = strarrComplainNo[0];
            objServiceContractor.SplitComplaint_RefNo = int.Parse(strarrComplainNo[1]);
            GetSCNo();
            ds = objServiceContractor.GetDefectFlag();
            if (ds.Tables[0].Rows.Count != 0)
                objServiceContractor.DefectAccFlag = ds.Tables[0].Rows[0]["DefectAccFlag"].ToString();
            if (ddlActionStatus.SelectedIndex != 0)
            {
                objServiceContractor.CallStatus = int.Parse(ddlActionStatus.SelectedValue.ToString());
            }
            objServiceContractor.LastComment = txtActionDetails.Text;
            objServiceContractor.EmpID = Membership.GetUser().UserName.ToString();
            if (txtActionEntryDate.Text != "")
                objServiceContractor.ActionDate = Convert.ToDateTime(txtActionEntryDate.Text.ToString());
            GetActionTime(ddlActHr, ddlActMin, ddlActAM);
            //if (objServiceContractor.CallStatus == 14 || objServiceContractor.CallStatus == 15 || objServiceContractor.CallStatus == 28 || objServiceContractor.CallStatus == 32)
            //Modify By Binay--MTO
            // Added By Gaurav Garg on 27 Nov --- In If conditions (|| objServiceContractor.CallStatus == 61)
            if (objServiceContractor.CallStatus == 57 || objServiceContractor.CallStatus == 58 || objServiceContractor.CallStatus == 60 || objServiceContractor.CallStatus == 61)// || objServiceContractor.CallStatus == 59)
            {
                if (txtServiceAmount.Text != "")
                    objServiceContractor.ServiceAmt = decimal.Parse(txtServiceAmount.Text);
                if (txtServiceDate.Text != "")
                    objServiceContractor.ServiceDate = DateTime.Parse(txtServiceDate.Text);
                if (txtServiceNumber.Text != "")
                    objServiceContractor.ServiceNumber = txtServiceNumber.Text;

                if (objServiceContractor.DefectAccFlag != "Y")
                {
                    ScriptManager.RegisterClientScriptBlock(btnSaveAction, GetType(), "ApproveDefect", "alert('Please Approve Defect First');", true);
                    //lblActionMsg.Text = "Please Approve Defect First"; 
                }

                else
                {
                    //Checking SLADAte with Service Action Date
                    if (txtServiceDate.Text != "")
                    {
                        if (objServiceContractor.ServiceDate != null && strSLADate != null)
                        {
                            if (DateTime.Parse(strSLADate) < (objServiceContractor.ServiceDate) || DateTime.Parse(strSLADate) == (objServiceContractor.ServiceDate))
                            {
                                ////Putting Action remarks in the PPR_Trans table in OBSERV col//////
                                objServiceContractor.EnterOBSERVInPPR();
                                ////////////////////////////////////////////////////////////////////
                                objServiceContractor.ActionEntry();
                                //lblActionMsg.Text = "Action Completed";
                                ScriptManager.RegisterClientScriptBlock(btnSaveAction, GetType(), "ActComp", "alert('Action Completed');", true);
                                //To refresh grid 2 on defect approval///
                                objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvCustDetail);

                                if (gvAddTemp.Rows.Count != 0)
                                {
                                    objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvAddTemp);
                                }
                                /////////////////////////////////////////
                                tbAction.Visible = false;
                                ClearActionCotrols();
                                //To refresh grid1 on action
                                BindGvFreshOnSearchBtnClick();
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(btnSaveAction, GetType(), "ActComp1", "alert('Service Invoice Date cannot be less than SLA Date');", true);
                            }
                        }
                        else
                        {
                            ////Putting Action remarks in the PPR_Trans table in OBSERV col//////
                            objServiceContractor.EnterOBSERVInPPR();
                            ////////////////////////////////////////////////////////////////////
                            objServiceContractor.ActionEntry();
                            //lblActionMsg.Text = "Action Completed";
                            ScriptManager.RegisterClientScriptBlock(btnSaveAction, GetType(), "ActComp", "alert('Action Completed');", true);
                            //To refresh grid 2 on defect approval///
                            objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvCustDetail);

                            if (gvAddTemp.Rows.Count != 0)
                            {
                                objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvAddTemp);
                            }
                            /////////////////////////////////////////
                            tbAction.Visible = false;
                            ClearActionCotrols();
                            //To refresh grid1 on action
                            BindGvFreshOnSearchBtnClick();
                        }
                    }
                    else
                    {
                        ////Putting Action remarks in the PPR_Trans table in OBSERV col//////
                        objServiceContractor.EnterOBSERVInPPR();
                        ////////////////////////////////////////////////////////////////////
                        objServiceContractor.ActionEntry();
                        //lblActionMsg.Text = "Action Completed";
                        ScriptManager.RegisterClientScriptBlock(btnSaveAction, GetType(), "ActComp", "alert('Action Completed');", true);
                        //To refresh grid 2 on defect approval///
                        objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvCustDetail);

                        if (gvAddTemp.Rows.Count != 0)
                        {
                            objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvAddTemp);
                        }
                        /////////////////////////////////////////
                        tbAction.Visible = false;
                        ClearActionCotrols();
                        //To refresh grid1 on action
                        BindGvFreshOnSearchBtnClick();
                    }
                }
            }
            else
            {

                //Checking SLADAte with Service Action Date
                if (txtServiceDate.Text != "")
                {
                    if (objServiceContractor.ServiceDate != null && strSLADate != null)
                    {
                        if (DateTime.Parse(strSLADate) < (objServiceContractor.ServiceDate))
                        {
                            ////Putting Action remarks in the PPR_Trans table in OBSERV col//////
                            objServiceContractor.EnterOBSERVInPPR();
                            ////////////////////////////////////////////////////////////////////
                            objServiceContractor.ActionEntry();
                            //lblActionMsg.Text = "Action Completed";
                            ScriptManager.RegisterClientScriptBlock(btnSaveAction, GetType(), "ActComp", "alert('Action Completed');", true);
                            //To refresh grid 2 on defect approval///
                            objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvCustDetail);

                            if (gvAddTemp.Rows.Count != 0)
                            {
                                objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvAddTemp);
                            }
                            /////////////////////////////////////////
                            tbAction.Visible = false;
                            ClearActionCotrols();
                            //To refresh grid1 on action
                            BindGvFreshOnSearchBtnClick();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(btnSaveAction, GetType(), "ActComp1", "alert('Service Invoice Date cannot be less than SLA Date');", true);
                        }
                    }
                    else
                    {
                        ////Putting Action remarks in the PPR_Trans table in OBSERV col//////
                        objServiceContractor.EnterOBSERVInPPR();
                        ////////////////////////////////////////////////////////////////////
                        objServiceContractor.ActionEntry();
                        //lblActionMsg.Text = "Action Completed";
                        ScriptManager.RegisterClientScriptBlock(btnSaveAction, GetType(), "ActComp", "alert('Action Completed');", true);
                        //To refresh grid 2 on defect approval///
                        objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvCustDetail);

                        if (gvAddTemp.Rows.Count != 0)
                        {
                            objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvAddTemp);
                        }
                        /////////////////////////////////////////
                        tbAction.Visible = false;
                        ClearActionCotrols();
                        //To refresh grid1 on action
                        BindGvFreshOnSearchBtnClick();
                    }
                }
                else
                {
                    ////Putting Action remarks in the PPR_Trans table in OBSERV col//////
                    objServiceContractor.EnterOBSERVInPPR();
                    ////////////////////////////////////////////////////////////////////
                    objServiceContractor.ActionEntry();
                    ScriptManager.RegisterClientScriptBlock(btnSaveAction, GetType(), "ActComp2", "alert('Action Completed');", true);
                    //To refresh grid 2 on defect approval///
                    objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvCustDetail);

                    //added by Gaurav Garg on 27 Nov 
                    if (gvAddTemp.Rows.Count != 0)
                    {
                        objServiceContractor.BindGridOngvFreshSelectIndexChanged(gvAddTemp);
                    }


                    /////////////////////////////////////////
                    tbAction.Visible = false;
                    ClearActionCotrols();
                    //To refresh grid1 on action
                    BindGvFreshOnSearchBtnClick();
                }
            }

            //Modified Date : 4 May 2009//
            txtServiceDate.Text = "";
            txtServiceAmount.Text = "";
            txtServiceNumber.Text = "";
            txtActionDetails.Text = "";
            //////////////////////////////


        }
        catch (Exception ex)
        {
            //lblActionMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
  
    protected void ddlActionStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Modified Date : 4 May 2009//
            txtServiceDate.Text = "";
            txtServiceAmount.Text = "";
            txtServiceNumber.Text = "";
            string strWarrantyStatus;
            strWarrantyStatus = hdnActionWarrantyStatus.Value.ToString();
            //////////////////////////////
            // if (int.Parse(ddlActionStatus.SelectedValue.ToString()) == 32 || int.Parse(ddlActionStatus.SelectedValue.ToString()) == 15 || int.Parse(ddlActionStatus.SelectedValue.ToString()) == 14 || int.Parse(ddlActionStatus.SelectedValue.ToString()) == 28)
            if (int.Parse(ddlActionStatus.SelectedValue.ToString()) == 57 || int.Parse(ddlActionStatus.SelectedValue.ToString()) == 58 || int.Parse(ddlActionStatus.SelectedValue.ToString()) == 60)
            {
                trServiceDate.Visible = true;
                trServiceNumber.Visible = true;
                trServiceAmount.Visible = true;

                rqftxtServiceAmount.Enabled = true;
                rqftxtServiceDate.Enabled = true;
                rqftxtServiceNumber.Enabled = true;



                if (strWarrantyStatus.ToUpper() == "Y")
                {
                    rqftxtServiceAmount.Enabled = false;
                    rqftxtServiceDate.Enabled = false;
                    rqftxtServiceNumber.Enabled = false;

                    trServiceDate.Visible = false;
                    trServiceNumber.Visible = false;
                    trServiceAmount.Visible = false;

                }
                else if (strWarrantyStatus.ToUpper() == "N")
                {
                    rqftxtServiceAmount.Enabled = true;
                    rqftxtServiceDate.Enabled = true;
                    rqftxtServiceNumber.Enabled = true;

                    trServiceDate.Visible = true;
                    trServiceNumber.Visible = true;
                    trServiceAmount.Visible = true;

                }

            }
            else
            {
                trServiceDate.Visible = false;
                trServiceNumber.Visible = false;
                trServiceAmount.Visible = false;

                rqftxtServiceAmount.Enabled = false;
                rqftxtServiceDate.Enabled = false;
                rqftxtServiceNumber.Enabled = false;
            }


            // if (int.Parse(ddlActionStatus.SelectedValue.ToString()) == 8)
            if (int.Parse(ddlActionStatus.SelectedValue.ToString()) == 53)
            {
                trSpare.Visible = false;
            }
            else
            {
                trSpare.Visible = false;
            }
        }
        catch (Exception ex)
        {
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }

    }
   
    protected void btnCloseAction_Click(object sender, EventArgs e)
    {
        try
        {
            tbAction.Visible = false;
            ClearActionCotrols();
            //Modified Date : 4 May 2009//
            txtServiceDate.Text = "";
            txtServiceAmount.Text = "";
            txtServiceNumber.Text = "";
            txtActionDetails.Text = "";
            //////////////////////////////

        }
        catch (Exception ex) { CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString()); }
    }
   
    protected void ClearActionCotrols()
    {
        lblActionProductDiv.Text = "";
        lblActionComplaintRefNo.Text = "";
        lblActionProduct.Text = "";
        lblActionBatch.Text = "";
        ddlActionStatus.SelectedIndex = 0;
        txtActionDetails.Text = "";

    }
    
    #endregion Action

    #region PagingGvFresh Gried No.1

    protected void btnPre_Click(object sender, EventArgs e)
    {
        if (btnNxt.Enabled == false)
        {
            btnNxt.Enabled = true;
        }
        // if (int.Parse(ViewState["FirstRow"].ToString()) != 1 && int.Parse(ViewState["LastRow"].ToString()) != 10)
        //Code By Binay For MTO
        if (int.Parse(ViewState["FirstRow"].ToString()) != 47)//&& int.Parse(ViewState["LastRow"].ToString()) != 10)
        {
            objServiceContractor.FirstRow = int.Parse(ViewState["FirstRow"].ToString()) - 10;
            ViewState["FirstRow"] = objServiceContractor.FirstRow;
            objServiceContractor.LastRow = int.Parse(ViewState["LastRow"].ToString()) - 10;
            ViewState["LastRow"] = objServiceContractor.LastRow;

            if (Convert.ToBoolean(ViewState["PageLoadFlag"].ToString()))
            {
                PageLoadSearch();
            }
            else
            {
                SearchButton();
            }
            btnPre.Enabled = true;
            //btnSearch_Click(null, null);
        }
        else
        {
            btnPre.Enabled = false;
        }
    }
    protected void btnNxt_Click(object sender, EventArgs e)
    {
        if (btnPre.Enabled == false)
        {
            btnPre.Enabled = true;
        }
        if (gvFresh.Rows.Count != 0)
        {
            objServiceContractor.FirstRow = int.Parse(ViewState["FirstRow"].ToString()) + 10;
            ViewState["FirstRow"] = objServiceContractor.FirstRow;
            objServiceContractor.LastRow = int.Parse(ViewState["LastRow"].ToString()) + 10;
            ViewState["LastRow"] = objServiceContractor.LastRow;
            if (Convert.ToBoolean(ViewState["PageLoadFlag"].ToString()))
            {
                PageLoadSearch();
            }
            else
            {
                SearchButton();
            }
            btnNxt.Enabled = true;
        }
        else
        {
            btnNxt.Enabled = false;
        }

        //btnSearch_Click(null, null);
    }
    #endregion PagingGvFresh

    #region eqbtnLifted_Click
    protected void eqbtnLifted_Click(object sender, EventArgs e)
    {
        try
        {
            objServiceContractor.BaseLineId = int.Parse(eqphdnBaselineID.Value.ToString());
            objServiceContractor.LastComment = eqtxtRemarks.Text.Trim();
            objServiceContractor.EmpID = Membership.GetUser().UserName.ToString();
            objServiceContractor.EquiptActionEntry();
            ScriptManager.RegisterClientScriptBlock(eqbtnLifted, GetType(), "Eqp", "alert('Action Completed');", true);
            BindGvFreshOnSearchBtnClick();
            eqtxtRemarks.Text = "";
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message.ToString();
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    #endregion

    #region chkSelfAllocatopn
    protected void chkSelfAllocatopn_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelfAllocatopn.Checked)
        {
            ddlCGExType.SelectedIndex = 0;
            ddlCGExcutive.SelectedIndex = 0;
            ddlCGEng.SelectedIndex = 0;
            RFVCGExType.Enabled = false;
            txtInitializationActionRemarks.Text = "";
            // Added By Gaurav Garg on 28 NOV
            RFVCGEmployee.Enabled = false;
            rfvCGEng.Enabled = false;
            RequiredFieldValidatorddlAllocateAction.Enabled = false;
            RequiredFieldValidatorddlOtherAction.Enabled = false;
            ddlOtherAction.SelectedIndex = 0;
            ddlAllocateAction.SelectedIndex = 0;
            trSCName.Visible = false;
            trCGExecutiveType.Visible = false;
            trCGExecutive.Visible = false;
            trCGEngineer.Visible = false;
        }
        else
        {
            RFVCGExType.Enabled = true;
            // Added By Gaurav Garg on 28 NOV
            RFVCGEmployee.Enabled = true;
            rfvCGEng.Enabled = true;
            RequiredFieldValidatorddlOtherAction.Enabled = true;
            RequiredFieldValidatorddlAllocateAction.Enabled = true;
            //trSCName.Visible = true;
            //trCGExecutiveType.Visible = true;
            //trCGExecutive.Visible = true;
            //trCGEngineer.Visible = true;
        }


    }
    #endregion

    //Add New Code By Binay--04-12-2009
    protected void txtProductRefNo_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNo.Text != "")
        {

            string strMaterialCode = txtProductRefNo.Text;
            objRequestReg.BindProduct_Detail_MTO(ddlProduct, Convert.ToInt32(ddlProductLine.SelectedValue.ToString()), strMaterialCode, txtInvoiceNo.Text.Trim());

        }
        //else
        //{
        //    ddlProductLine_SelectedIndexChanged(ddlProductLine, null);
        //}
    }

}
