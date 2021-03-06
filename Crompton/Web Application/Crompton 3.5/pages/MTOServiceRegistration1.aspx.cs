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
using System.IO;
using System.Text;
using System.Data.SqlClient;


public partial class pages_MTOServiceRegistration1 : System.Web.UI.Page
{
    int intCnt = 0, intCommon = 0;
    string strFileName = "", strvFileName = "", strExt = "", strFileSavePath = "", strLandmark = "", strPhone = "";
    RequestRegistration_MTO objRequestReg = new RequestRegistration_MTO();
    RequestRegistration objCallRegistration = new RequestRegistration();
    CommonClass objCommonClass = new CommonClass();
    UserMaster objUserMaster = new UserMaster();
    DataSet dsProduct = new DataSet();
    DataTable dTableFile = new DataTable();
    bool blnFlag = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //MTOServiceRegistration1.aspx
        if (!Page.IsPostBack)
        {
            //objRequestReg.BindDdl(ddlproductDivision, "0", "SELECT_MTO_UNIT");
            objRequestReg.BindDdl(ddlPMCountry, "0", "SELECT_COUNTRY");
            objRequestReg.BindCommonDDL(ddlCountry, "0", "SELECT_COUNTRY_FOR_SITE");
            // Added By Gaurav Garg on 30 Nov for default "India" selected -- 
            ddlCountry_SelectedIndexChanged(null, null);
            //ddlState.Items.Insert(0, new ListItem("Select", "Select"));
            objCommonClass.BindModeOfReciept(ddlModeOfRec);
            objCommonClass.BindLanguage(ddlLanguage);

            objRequestReg.UserName = Membership.GetUser().UserName.ToString();
            tableResult.Visible = false;

            // Added for Complaint div
            objRequestReg.LocCode = "";
            objRequestReg.BindProductDivision(ddlProductDiv);

            //Create table to bind product
            CreateTable();
            DataTable dTableF = new DataTable("tblInsert");
            DataColumn dClFileName = new DataColumn("FileName", System.Type.GetType("System.String"));
            dTableF.Columns.Add(dClFileName);
            ViewState["dTableFile"] = dTableF;
            ViewState["ds"] = dsProduct;

            // added By Gaurav Garg on 11 NOV 09
            hdnGlobalDate.Value = DateTime.Today.Date.ToString("MM/dd/yyyy");

        // Added By Gaurav Garg on 08 Dec for adding Region 
            objRequestReg.BindRegion(ddlRegion);

        }
        System.Threading.Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["AjaxPleaseWaitTime"]));
    }

    #region Fill ddl for site Details
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCountry.SelectedIndex != 0)
        {
            objRequestReg.Country_Sno = ddlCountry.SelectedValue;
            objRequestReg.BindCommonDDL(ddlState, objRequestReg.Country_Sno, "SELECT_STATE_ON_COUNTRY_FOR_SITE");
        }
        else
        {
            ddlState.Items.Clear();
            ddlState.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlState.SelectedIndex != 0)
        {
            objRequestReg.State_Sno = ddlState.SelectedValue;
            objRequestReg.BindCommonDDL(ddlCity, objRequestReg.State_Sno, "SELECT_CITY_ON_STATE_FOR_SITE");
        }
        else
        {
            ddlCity.Items.Clear();
            ddlCity.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlCity.SelectedValue.ToString() != "0")
        //{
        //    trAllocateToMySelf.Visible = true;
        //    trAllocateTo.Visible = true;
        //}
        //else
        //{
        //    trAllocateToMySelf.Visible = false;
        //    trAllocateTo.Visible = false;
        //}
    }
    

    #endregion Fill ddl for site Details

    #region Set Default Mode and Language
    protected void chkDefautMode_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Updating Mode of reciept and Language
            objUserMaster.UserName = Membership.GetUser().UserName.ToString();
            objUserMaster.ModeOfRecipt = ddlModeOfRec.SelectedValue.ToString();
            objUserMaster.Language = ddlLanguage.SelectedValue.ToString();
            objUserMaster.UpdateLanguageModeOfRecipt("UPDATE_LANGUAGE_MODEOfRECIPT");
        }
        catch (Exception ex)
        {
            //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }

    }
    protected void chkLanguage_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            //Updating Mode of reciept and Language
            objUserMaster.UserName = Membership.GetUser().UserName.ToString();
            objUserMaster.ModeOfRecipt = ddlModeOfRec.SelectedValue.ToString();
            objUserMaster.Language = ddlLanguage.SelectedValue.ToString();
            objUserMaster.UpdateLanguageModeOfRecipt("UPDATE_LANGUAGE_MODEOfRECIPT");
        }
        catch (Exception ex)
        {
            //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }
    #endregion Set Default Mode and Language
    
    protected void ddlPMCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPMCountry.SelectedIndex != 0)
        {
            string ContryDesc = ddlPMCountry.SelectedItem.ToString();
            string[] str1 = ContryDesc.Split('-');
            string Country_Sno = str1[1];

            objRequestReg.Country_Sno = Country_Sno; // ddlPMCountry.SelectedValue;
            objRequestReg.BindDdl(ddlPMState, objRequestReg.Country_Sno, "SELECT_STATE_ON_COUNTRY");
        }
        else
        {
            ddlPMState.Items.Clear();
            ddlPMState.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected void ddlPMState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPMState.SelectedIndex != 0)
        {
            string StateDesc = ddlPMState.SelectedItem.ToString();
            string[] str1 = StateDesc.Split('-');
            string State_Sno = str1[1];

            objRequestReg.State_Sno = State_Sno; // ddlPMState.SelectedValue;
            objRequestReg.BindDdl(ddlPMCity, objRequestReg.State_Sno, "SELECT_CITY_ON_STATE");
        }
        else
        {
            ddlPMCity.Items.Clear();
            ddlPMCity.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    #region  Commented on 5 Nov 09
    //protected void ddlproductDivision_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlproductDivision.SelectedIndex != 0)
    //    {
    //        string UnitDesc = ddlproductDivision.SelectedItem.ToString();
    //        string[] str1 = UnitDesc.Split('-');
    //        string UnitCode = str1[1];

    //        objRequestReg.Unit_Code = UnitCode;
    //        objRequestReg.BindDdl1(ddlProductInvoiceNo, objRequestReg.Unit_Code, "SELECT_INVOICE_UNIT_CODE");
    //        objRequestReg.Unit_Sno = ddlproductDivision.SelectedValue;
    //        objRequestReg.BindDdl(ddlProductLine, objRequestReg.Unit_Sno, "SELECT_PRODUCTLINE_ON_UNIT");
    //        objRequestReg.BindDdl(ddlProduct, objRequestReg.Unit_Sno, "SELECT_PRODUCT_ON_UNIT");
    //    }
    //    else
    //    {
    //        ddlProductInvoiceNo.Items.Clear();
    //        ddlProductInvoiceNo.Items.Insert(0, new ListItem("Select", "0"));
    //        ddlProductLine.Items.Clear();
    //        ddlProductLine.Items.Insert(0, new ListItem("Select", "0"));
    //        ddlProduct.Items.Clear();
    //        ddlProduct.Items.Insert(0, new ListItem("Select", "0"));
    //    }
    //}
    #endregion Commented on 5 Nov 09

    #region  Commented on 5 Nov 09
    //protected void ddlProductInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlProductInvoiceNo.SelectedIndex != 0)
    //    {
    //        objRequestReg.Invoice_No = ddlProductInvoiceNo.SelectedValue;
    //        objRequestReg.BindDdl(ddlFGCustomer, objRequestReg.Invoice_No, "SELECT_CUSTOMER_ON_INVOICE");

    //    }
    //    else
    //    {
    //        ddlFGCustomer.Items.Clear();
    //        ddlFGCustomer.Items.Insert(0, new ListItem("Select", "0"));

    //    }
    //}
    #endregion Commented on 5 Nov 09

    //protected void lnkbtnCustomerSearch_Click(object sender, EventArgs e)
    //{
    //    //ShowHideDivCustomerSearch(true);
    //    trCustomerSearch.Visible = true;
    //}

    //protected void BtnProcess_Click(object sender, EventArgs e)
    //{
    //    if (txtInvoiceNo.Text != "")
    //    {
    //        objRequestReg.Invoice_No = txtInvoiceNo.Text;
    //        DataSet ds;
    //        objRequestReg.Type = "SELECT_CUSTOMER_ON_INVOICE";
    //        ds = objRequestReg.GetPartyCodeOnInvoiceNoProductSRNo();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            objRequestReg.Party_Code = ds.Tables[0].Rows[0]["PartyCode"].ToString();
    //            objRequestReg.LocCode = ds.Tables[0].Rows[0]["LocCode"].ToString();
    //            txtInvoiceNum.Text = txtInvoiceNo.Text;
    //            txtxPurchaseDate.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
    //            txtPoDate.Text = ds.Tables[0].Rows[0]["PoDate"].ToString();
    //            txtDateofDispatch.Text = ds.Tables[0].Rows[0]["DateOfDispatch"].ToString();
    //            txtPrdSRNo.Text = ds.Tables[0].Rows[0]["ProductSRNo"].ToString();
    //            lblErrormsg.Text = "";
    //        }
    //        else
    //        {
    //            // mesg for not find any party for that invoice number.
    //            lblErrormsg.Text = "Record Not Found";
    //            divCustomerInfo.Visible = false;
    //            ClearControls();
    //        }
    //    }
    //    else
    //    {
    //        objRequestReg.Product_SRNo = txtProductSRNo.Text;
    //        DataSet ds;
    //        objRequestReg.Type = "SELECT_CUSTOMER_ON_PRODUCT_SRNO";
    //        ds = objRequestReg.GetPartyCodeOnInvoiceNoProductSRNo();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            objRequestReg.Party_Code = ds.Tables[0].Rows[0]["PartyCode"].ToString();
    //            objRequestReg.LocCode = ds.Tables[0].Rows[0]["LocCode"].ToString();
    //            txtInvoiceNum.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
    //            txtxPurchaseDate.Text = ds.Tables[0].Rows[0]["InoiceDate"].ToString();
    //            txtPrdSRNo.Text= txtProductSRNo.Text  ;
    //            lblErrormsg.Text = "";

    //        }
    //        else
    //        {
    //            // mesg for not find any party for that invoice number.
    //            lblErrormsg.Text = "Record Not Found";
    //            divCustomerInfo.Visible = false;
    //            ClearControls();
    //        }
    //        //On 5 Nov //objRequestReg.Party_Code = ddlFGCustomer.SelectedValue;
    //    }
        
    //    objRequestReg.BindProductDivision(ddlProductDiv);
    //    AddAllInDdl(ddlProductDiv);
    //    ddlProductDiv_SelectedIndexChanged(null, null);

    //    BindGridCustomers();
    //}

    protected void BtnProcess_Click(object sender, EventArgs e)
    {
        DataSet ds;
        if ((txtInvoiceNo.Text != "") && (ddlproductSRNO.SelectedValue != ""))
        {
            tdInvoiceNo.Visible = false;
            objRequestReg.Product_SRNo = ddlproductSRNO.SelectedValue.ToString();

            //DataSet ds;
            objRequestReg.Type = "SELECT_CUSTOMER_ON_PRODUCT_SRNO";
            ds = objRequestReg.GetPartyCodeOnInvoiceNoProductSRNo();
            if (ds.Tables[0].Rows.Count > 0)
            {
                objRequestReg.Party_Code = ds.Tables[0].Rows[0]["PartyCode"].ToString();
                objRequestReg.LocCode = ds.Tables[0].Rows[0]["LocCode"].ToString();
                txtInvoiceNum.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                txtxPurchaseDate.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                
                // Added Here
                txtPoDate.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                txtDateofDispatch.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                txtPrdSRNo.Text = ds.Tables[0].Rows[0]["ProductSRNo"].ToString();
                txtProductSRNo.Text = ds.Tables[0].Rows[0]["ProductSRNo"].ToString();
                txtCommissionDate.Text = ds.Tables[0].Rows[0]["DateOfCommission"].ToString();
                txtWarrantyDate.Text = ds.Tables[0].Rows[0]["WarrantyExpDate"].ToString();

                //txtPrdSRNo.Text = txtProductSRNo.Text;
                lblErrormsg.Text = "";
                objRequestReg.Materialcode = ds.Tables[0].Rows[0]["Materialcode"].ToString();
                Session["Materialcode"] = objRequestReg.Materialcode;
                // added here 
                objRequestReg.BindProductDivision(ddlProductDiv);
                if (ddlProductDiv.Items.Count != 1)
                {
                    AddAllInDdl(ddlProductDiv);
                }
                //Add Code By Binay-23-11-2009
                if (ddlProductDiv.SelectedValue != "0")
                {
                    
                    objRequestReg.BindProduct_Line(ddlProductLine, Convert.ToInt32(ddlProductDiv.SelectedValue.ToString()));
                    if (ddlProductLine.SelectedValue != "")
                    {
                        objRequestReg.BindProduct_Detail(ddlProduct, Convert.ToInt32(ddlProductLine.SelectedValue.ToString()), objRequestReg.Materialcode);
                    }
                }
                BindGridCustomers();
            }
            else
            {
                // mesg for not find any party for that invoice number.
                lblErrormsg.Text = "Record Not Found / Select Product SRNo ";
                divCustomerInfo.Visible = false;
                objRequestReg.Materialcode = "";
                ClearControls();
            }
        }
        else if (txtInvoiceNo.Text != "")
        {
            tdInvoiceNo.Visible = false;
            objRequestReg.Invoice_No = txtInvoiceNo.Text;
            //DataSet ds;
            objRequestReg.Type = "SELECT_CUSTOMER_ON_INVOICE";
            ds = objRequestReg.GetPartyCodeOnInvoiceNoProductSRNo();
            if (ds.Tables[0].Rows.Count > 1)
            {
                tdProdctSRNO.Visible = true;
                objRequestReg.BindProductSRNo_InvoiceNo(ddlproductSRNO);
                //txtInvoiceNo.Text = "";
            }

            else if (ds.Tables[0].Rows.Count == 1)
            {
                objRequestReg.Party_Code = ds.Tables[0].Rows[0]["PartyCode"].ToString();
                objRequestReg.LocCode = ds.Tables[0].Rows[0]["LocCode"].ToString();
                txtInvoiceNum.Text = txtInvoiceNo.Text;
                txtxPurchaseDate.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                //txtPoDate.Text = ds.Tables[0].Rows[0]["PoDate"].ToString();
                txtPoDate.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                //txtDateofDispatch.Text = ds.Tables[0].Rows[0]["DateOfDispatch"].ToString();
                txtDateofDispatch.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                txtPrdSRNo.Text = ds.Tables[0].Rows[0]["ProductSRNo"].ToString();
                txtProductSRNo.Text = ds.Tables[0].Rows[0]["ProductSRNo"].ToString();
                txtCommissionDate.Text = ds.Tables[0].Rows[0]["DateOfCommission"].ToString();
                txtWarrantyDate.Text = ds.Tables[0].Rows[0]["WarrantyExpDate"].ToString();
                lblErrormsg.Text = "";
                objRequestReg.Materialcode = ds.Tables[0].Rows[0]["Materialcode"].ToString();
                Session["Materialcode"] = objRequestReg.Materialcode;
                // added here 
                 objRequestReg.BindProductDivision(ddlProductDiv);
                if (ddlProductDiv.Items.Count != 1)
                {
                    AddAllInDdl(ddlProductDiv);
                }
                //Add Code By Binay-23-11-2009
                if (ddlProductDiv.SelectedValue != "0") 
                {
                    
                    objRequestReg.BindProduct_Line(ddlProductLine, Convert.ToInt32(ddlProductDiv.SelectedValue.ToString()));
                    if (ddlProductLine.SelectedValue != "")
                    {
                        objRequestReg.BindProduct_Detail(ddlProduct, Convert.ToInt32(ddlProductLine.SelectedValue.ToString()), objRequestReg.Materialcode);
                    }
                }
                
                    BindGridCustomers();
            }
            else
            {
                // mesg for not find any party for that invoice number.
                lblErrormsg.Text = "Record Not Found";
                divCustomerInfo.Visible = false;
                objRequestReg.Materialcode = "";
                ClearControls();
            }
        }
        else if ((txtProductSRNo.Text != "") && (ddlInvoiceNo.SelectedValue != ""))
            {
                tdProdctSRNO.Visible = false;
                objRequestReg.Invoice_No = ddlInvoiceNo.SelectedValue.ToString();
                //DataSet ds;
                objRequestReg.Type = "SELECT_CUSTOMER_ON_INVOICE";
                ds = objRequestReg.GetPartyCodeOnInvoiceNoProductSRNo();
                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    objRequestReg.Party_Code = ds.Tables[0].Rows[0]["PartyCode"].ToString();
                    objRequestReg.LocCode = ds.Tables[0].Rows[0]["LocCode"].ToString();
                    txtInvoiceNum.Text = ddlInvoiceNo.SelectedValue.ToString();
                    txtxPurchaseDate.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                    //txtPoDate.Text = ds.Tables[0].Rows[0]["PoDate"].ToString();
                    txtPoDate.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                    //txtDateofDispatch.Text = ds.Tables[0].Rows[0]["DateOfDispatch"].ToString();
                    txtDateofDispatch.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                    txtPrdSRNo.Text = ds.Tables[0].Rows[0]["ProductSRNo"].ToString();
                    txtProductSRNo.Text = ds.Tables[0].Rows[0]["ProductSRNo"].ToString();
                    txtCommissionDate.Text = ds.Tables[0].Rows[0]["DateOfCommission"].ToString();
                    txtWarrantyDate.Text = ds.Tables[0].Rows[0]["WarrantyExpDate"].ToString();

                    lblErrormsg.Text = "";
                    objRequestReg.Materialcode = ds.Tables[0].Rows[0]["Materialcode"].ToString();
                    Session["Materialcode"] = objRequestReg.Materialcode;
                    // added here 
                    objRequestReg.BindProductDivision(ddlProductDiv);
                    if (ddlProductDiv.Items.Count != 1)
                    {
                        AddAllInDdl(ddlProductDiv);
                    }
                    //Add Code By Binay-23-11-2009
                    if (ddlProductDiv.SelectedValue != "0")
                    {
                        
                        objRequestReg.BindProduct_Line(ddlProductLine, Convert.ToInt32(ddlProductDiv.SelectedValue.ToString()));
                        if (ddlProductLine.SelectedValue != "")
                        {
                            objRequestReg.BindProduct_Detail(ddlProduct, Convert.ToInt32(ddlProductLine.SelectedValue.ToString()), objRequestReg.Materialcode);
                        }
                    }
                    BindGridCustomers();
                }
                else
                {
                    // mesg for not find any party for that invoice number.
                    lblErrormsg.Text = "Record Not Found / Select Invoice No";
                    divCustomerInfo.Visible = false;
                    objRequestReg.Materialcode = "";
                    ClearControls();
                }
            }
        else if (txtProductSRNo.Text != "")
        {
            tdProdctSRNO.Visible = false;
            objRequestReg.Product_SRNo = txtProductSRNo.Text;
            objRequestReg.Type = "SELECT_CUSTOMER_ON_PRODUCT_SRNO";
            ds = objRequestReg.GetPartyCodeOnInvoiceNoProductSRNo();
            if (ds.Tables[0].Rows.Count > 1)
            {
                tdInvoiceNo.Visible = true;
                objRequestReg.BindInvoiceNo_ProductSRNo(ddlInvoiceNo);
                //txtInvoiceNo.Text = "";
            }

            else if (ds.Tables[0].Rows.Count > 0)
            {
                objRequestReg.Party_Code = ds.Tables[0].Rows[0]["PartyCode"].ToString();
                objRequestReg.LocCode = ds.Tables[0].Rows[0]["LocCode"].ToString();
                txtInvoiceNum.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                txtInvoiceNo.Text = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                txtxPurchaseDate.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();

                // Added Here
                txtPoDate.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                txtDateofDispatch.Text = ds.Tables[0].Rows[0]["invoiceDate"].ToString();
                txtPrdSRNo.Text = ds.Tables[0].Rows[0]["ProductSRNo"].ToString();
                txtProductSRNo.Text = ds.Tables[0].Rows[0]["ProductSRNo"].ToString();
                txtCommissionDate.Text = ds.Tables[0].Rows[0]["DateOfCommission"].ToString();
                txtWarrantyDate.Text = ds.Tables[0].Rows[0]["WarrantyExpDate"].ToString();


                //txtPrdSRNo.Text = txtProductSRNo.Text;
                lblErrormsg.Text = "";
                objRequestReg.Materialcode = ds.Tables[0].Rows[0]["Materialcode"].ToString();
                Session["Materialcode"] = objRequestReg.Materialcode;
                // added here 
                objRequestReg.BindProductDivision(ddlProductDiv);
                

                if (ddlProductDiv.Items.Count != 1)
                {
                    AddAllInDdl(ddlProductDiv);
                }
                
                //Add Code By Binay-23-11-2009
                if (ddlProductDiv.SelectedValue != "0")
                {
                    
                    objRequestReg.BindProduct_Line(ddlProductLine, Convert.ToInt32(ddlProductDiv.SelectedValue.ToString()));
                    
                    if (ddlProductLine.SelectedValue != "")
                    {
                        objRequestReg.BindProduct_Detail(ddlProduct, Convert.ToInt32(ddlProductLine.SelectedValue.ToString()), objRequestReg.Materialcode);
                    }
                }
                BindGridCustomers();
            }
            else
            {
                // mesg for not find any party for that invoice number.
                lblErrormsg.Text = "Record Not Found";
                divCustomerInfo.Visible = false;
                objRequestReg.Materialcode = "";
                ClearControls();
            }
            //On 5 Nov //objRequestReg.Party_Code = ddlFGCustomer.SelectedValue;
        }
        ////if (ds.Tables[0].Rows.Count > 0)
        ////{
        ////    objRequestReg.BindProductDivision(ddlProductDiv);
        ////    AddAllInDdl(ddlProductDiv);
        ////    //Add Code By Binay-23-11-2009
        ////    objRequestReg.BindProduct_Line(ddlProductLine, Convert.ToInt32(ddlProductDiv.SelectedValue.ToString()));
        ////    objRequestReg.BindProduct_Detail(ddlProduct, Convert.ToInt32(ddlProductLine.SelectedValue.ToString()), objRequestReg.Materialcode);
        ////    //End
        ////    //ddlProductLine_SelectedIndexChanged(null, null);
        ////}

        ////BindGridCustomers();
    }
    protected void AddAllInDdl(DropDownList ddl)
    {
        ddl.Items.RemoveAt(0);
    }

    private void BindGridCustomers()
    {
        objRequestReg.Type = "SEARCHDATA_ON_PARTYCODE";
        objRequestReg.EmpCode = Membership.GetUser().UserName.ToString();

        DataSet dsCustomers = new DataSet();
        dsCustomers = objRequestReg.GetCustomerOnPartyCode();
        if (objRequestReg.ReturnValue == -1)
        {
            //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), objRequestReg.MessageOut.ToString());
        }
        else
        {
            intCnt = dsCustomers.Tables[0].Rows.Count;
            if (intCnt > 0)
            {
                if (intCnt == 1)
                {
                    BindCustomerData(dsCustomers.Tables[0].Rows[0]["PARTYCODE"].ToString());
                    lblErrormsg.Text = "";
                }
                else
                {
                    //gvCustomerList.DataSource = dsCustomers;
                    //gvCustomerList.DataBind();
                    //lblCustMsg.Text = "Total Customers found: " + dsCustomers.Tables[0].Rows.Count + " (Please select below action for Complaint Registration.)";
                    //ShowHideDivCustomerSearch(true);
                }
            }
            else
            {

                //ClearControls();
                //txtContactNo.Text = strPhoneNumber;
                //gvCustomerList.DataSource = dsCustomers;
                //gvCustomerList.DataBind();
                //lblCustMsg.Text = "Total Customers found: " + dsCustomers.Tables[0].Rows.Count + " (Please select below action for Complaint Registration.)";
                //ShowHideDivCustomerSearch(false);
                lblErrormsg.Text = "Party detail not available in SAP Data.";//Record Not Found";
            }

        }
        dsCustomers = null;
    }

    private void BindCustomerData(string PartyCode)
    {
        //RequestRegistration objRequestRegistrationNew = new RequestRegistration();
        DataSet dsNewCust = new DataSet();
        objRequestReg.Party_Code = PartyCode;
        ViewState["Party_Code"] = PartyCode;
        objRequestReg.EmpCode = Membership.GetUser().UserName.ToString();
        objRequestReg.Type = "SEARCHDATA_ON_PARTYCODE";
        dsNewCust = objRequestReg.GetCustomerOnPartyCode();

        if (dsNewCust.Tables[1].Rows.Count > 0)
        {
            hdnCustomerId.Value = dsNewCust.Tables[1].Rows[0]["CustomerId"].ToString(); //PartyCode.ToString();

            txtFirstName.Text = dsNewCust.Tables[1].Rows[0]["FirstName"].ToString();
            txtLastName.Text = dsNewCust.Tables[1].Rows[0]["LastName"].ToString();
            txtAdd1.Text = dsNewCust.Tables[1].Rows[0]["Address1"].ToString();
            txtAdd2.Text = dsNewCust.Tables[1].Rows[0]["Address2"].ToString();
            txtAdd3.Text = dsNewCust.Tables[1].Rows[0]["Landmark"].ToString();
            txtCompanyName.Text = dsNewCust.Tables[1].Rows[0]["Company_Name"].ToString();
            txtPinCode.Text = dsNewCust.Tables[1].Rows[0]["PinCode"].ToString();
            txtContactNo.Text = dsNewCust.Tables[1].Rows[0]["UniqueContact_No"].ToString();
            txtAltConatctNo.Text = dsNewCust.Tables[1].Rows[0]["AltTelNumber"].ToString();
            txtExtension.Text = dsNewCust.Tables[1].Rows[0]["Extension"].ToString();
            txtEmail.Text = dsNewCust.Tables[1].Rows[0]["Email"].ToString();
            txtFaxNo.Text = dsNewCust.Tables[1].Rows[0]["Fax"].ToString();


            //ddlPMCountry.SelectedIndex = ddlPMCountry.Items.IndexOf(ddlPMCountry.Items.FindByValue(objAttributeMapMaster.BusinessLine_Sno.ToString()));
            for (intCnt = 0; intCnt < ddlCountry.Items.Count; intCnt++)
            {
                if (ddlCountry.Items[intCnt].Value.ToString() == dsNewCust.Tables[1].Rows[0]["Country_Sno"].ToString())
                {
                    ddlCountry.SelectedIndex = intCnt;
                }
            }
            ddlCountry_SelectedIndexChanged(null, null);

            for (intCnt = 0; intCnt < ddlState.Items.Count; intCnt++)
            {
                if (ddlState.Items[intCnt].Value.ToString() == dsNewCust.Tables[1].Rows[0]["State_Sno"].ToString())
                {
                    ddlState.SelectedIndex = intCnt;
                }
            }
            ddlState_SelectedIndexChanged(null, null);

            if (ddlState.SelectedIndex != 0)
            {
                objCommonClass.BindCity(ddlCity, int.Parse(ddlState.SelectedValue.ToString()));
                ddlCity.Items.Add(new ListItem("Other", "0"));
                ddlCity_SelectedIndexChanged(null, null);
            }
            

            for (intCnt = 0; intCnt < ddlCity.Items.Count; intCnt++)
            {
                if (ddlCity.Items[intCnt].Value.ToString() == dsNewCust.Tables[1].Rows[0]["City_Sno"].ToString())
                {
                    ddlCity.SelectedIndex = intCnt;
                }
            }

            for (intCnt = 0; intCnt < ddlLanguage.Items.Count; intCnt++)
            {
                if (ddlLanguage.Items[intCnt].Value.ToString() == dsNewCust.Tables[1].Rows[0]["Language_SNo"].ToString())
                {
                    ddlLanguage.SelectedIndex = intCnt;
                }
            }

        }

        if (dsNewCust.Tables[0].Rows.Count > 0)
        {
            //hdnCustomerId.Value = lngCustomerId.ToString();

            txtPMFirstName.Text = dsNewCust.Tables[0].Rows[0]["PartyName"].ToString();
            txtPMLastName.Text = dsNewCust.Tables[0].Rows[0]["PartyShortName"].ToString();
            //for (intCnt = 0; intCnt < ddlCustomerType.Items.Count; intCnt++)
            //{
            //    if (ddlCustomerType.Items[intCnt].Value.ToString() == dsNewCust.Tables[0].Rows[0]["Customer_type"].ToString())
            //    {
            //        ddlCustomerType.SelectedIndex = intCnt;
            //    }
            //}

            txtPMCompanyName.Text = dsNewCust.Tables[0].Rows[0]["PartyName"].ToString();
            txtPMAdd1.Text = dsNewCust.Tables[0].Rows[0]["Address1"].ToString();
            txtPMAdd2.Text = dsNewCust.Tables[0].Rows[0]["Address2"].ToString();
            txtPMAdd3.Text = dsNewCust.Tables[0].Rows[0]["Address3"].ToString();

            //ddlPMCountry.SelectedIndex = ddlPMCountry.Items.IndexOf(ddlPMCountry.Items.FindByValue(objAttributeMapMaster.BusinessLine_Sno.ToString()));
            for (intCnt = 0; intCnt < ddlPMCountry.Items.Count; intCnt++)
            {
                if (ddlPMCountry.Items[intCnt].Value.ToString() == dsNewCust.Tables[0].Rows[0]["CountryCode"].ToString())
                {
                    ddlPMCountry.SelectedIndex = intCnt;
                }
                else
                {
                    //ddlPMCountry.Items.Insert(0, new ListItem(dsNewCust.Tables[0].Rows[0]["CountryCode"].ToString(), "0"));
                }
            }
            ddlPMCountry_SelectedIndexChanged(null, null);

            //ddlPMState.Enabled = true;
            for (intCnt = 0; intCnt < ddlPMState.Items.Count; intCnt++)
            {
                if (ddlPMState.Items[intCnt].Value.ToString() == dsNewCust.Tables[0].Rows[0]["StateCode"].ToString())
                {
                    ddlPMState.SelectedIndex = intCnt;
                }
            }
            if (ddlPMState.SelectedIndex == 0)
            {
                ddlPMState.Items.Insert(0, new ListItem(dsNewCust.Tables[0].Rows[0]["StateCode"].ToString(),"0"));
                ddlPMState.SelectedIndex = 0;
            }
            ddlPMState_SelectedIndexChanged(null, null);

            //if (ddlPMState.SelectedIndex != 0)
            //{
            //    objCommonClass.BindCity(ddlPMCity, int.Parse(ddlPMState.SelectedValue.ToString()));
            //    ddlPMCity.Items.Add(new ListItem("Other", "0"));
            //}

            for (intCnt = 0; intCnt < ddlPMCity.Items.Count; intCnt++)
            {
                if (ddlPMCity.Items[intCnt].Value.ToString() == dsNewCust.Tables[0].Rows[0]["CityCode"].ToString())
                {
                    ddlPMCity.SelectedIndex = intCnt;
                }
            }
            if (ddlPMCity.SelectedIndex == 0)
            {
                ddlPMCity.Items.Insert(0, new ListItem(dsNewCust.Tables[0].Rows[0]["CityCode"].ToString(), "0"));
                ddlPMCity.SelectedIndex = 0;
            }

            txtPMPinCode.Text = dsNewCust.Tables[0].Rows[0]["PinCode"].ToString();
            txtPMPhone.Text = dsNewCust.Tables[0].Rows[0]["Phone"].ToString();

            txtPMEmail.Text = dsNewCust.Tables[0].Rows[0]["Email"].ToString();
            txtPMFaxNo.Text = dsNewCust.Tables[0].Rows[0]["Fax"].ToString();
        }



        //chkUpdateCustomerData.Visible = true;
        //chkUpdateCustomerData.Checked = true;
        objRequestReg = null;
        dsNewCust = null;
        divCustomerInfo.Visible = true;
        divSiteInfo.Visible = true;
    }

    // Added for Complaint div

    protected void ddlProductDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet dsTemp = (DataSet)ViewState["ds"];

        if (ddlProductDiv.SelectedIndex != 0)
        {
            btnSubmit.CausesValidation = true;
        }
        else
        {
            //if (dsTemp.Tables[0].Rows.Count > 0)
            //{
            //    btnSubmit.CausesValidation = false;
            //}
            //else
            //{
            //    btnSubmit.CausesValidation = true;
            //}
           
        }

        //binding ProductLine based on Product Division Sno
        objCommonClass.BindProductLine(ddlProductLine, int.Parse(ddlProductDiv.SelectedValue.ToString()));

        ScriptManager.RegisterClientScriptBlock(ddlProductDiv, GetType(), "MyScript11", "document.getElementById('ctl00_MainConHolder_ddlProductDiv').focus(); ", true);
    }

    protected void ddlProductLine_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProductLine.SelectedIndex != 0)
        {
            objCommonClass.BindProduct(ddlProduct, int.Parse(ddlProductLine.SelectedValue.ToString()));
        }
        else
        {
            ddlProduct.Items.Clear();
            ddlProduct.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected void ddlAllocateTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAllocateTo.SelectedIndex != 0)
        {
            tdAllocate.Visible = true;
            if (ddlAllocateTo.SelectedValue == "3") // 3 for SC
            {
                ddlCGExec.Visible = false;
                ddlCGContractEmp.Visible = false;
                ddlSC.Visible = true;
                //objRequestReg.State_Sno = ddlState.SelectedValue.ToString();
                //objRequestReg.BindDdl(ddlSC, "3", "SELECT_SC");
                //if(ddlCity.SelectedValue.ToString() !="0")
                    //objRequestReg.BindDdl(ddlSC, ddlCity.SelectedValue.ToString(), "SELECT_SC");
                objRequestReg.BindSCName_MTOServiceReg(ddlSC);
            }
            else if (ddlAllocateTo.SelectedValue == "2") // 2 CG Executive
            {
                
                ddlCGExec.Visible = true;
                ddlSC.Visible = false;
                ddlCGContractEmp.Visible = false;                
                objRequestReg.BindDdl(ddlCGExec, "2", "SELECT_CGEMPLOYEE");
                //Add By Binay-05-12-2009
                //objRequestReg.BindDdlCGUser(ddlCGExec, Convert.ToInt32(ddlCity.SelectedValue),Convert.ToInt32(ddlProductDiv.SelectedValue));
            }
            else  // 3 for CG Contract Emp
            {
                ddlCGExec.Visible = false;
                ddlSC.Visible = false;
                ddlCGContractEmp.Visible = true;
                objRequestReg.BindDdl(ddlCGContractEmp, "5", "SELECT_CG_CONTRACT");
                //Add By Binay -05-12-2009
                //objRequestReg.BindDdlCGContractUser(ddlCGContractEmp, Convert.ToInt32(ddlCity.SelectedValue), Convert.ToInt32(ddlProductDiv.SelectedValue));
            }
            if (chkSelfAllocatopn.Checked)
            {
                chkSelfAllocatopn.Checked = false;
            }
        }
        else
        {
            tdAllocate.Visible = false;
           
        }
    }

    private void CreateTable()
    {
        //Table for Data Insert to database based on Ids
        DataTable dTable = new DataTable("tblInsert");
        dsProduct.Tables.Add(dTable);
        DataColumn dColSno = new DataColumn("Sno", System.Type.GetType("System.Int16"));
        DataColumn dColProductDivision_Sno = new DataColumn("ProductDivision_Sno", System.Type.GetType("System.String"));
        DataColumn dColProductLine_Sno = new DataColumn("ProductLine_Sno", System.Type.GetType("System.String"));
        DataColumn dColLanguage_Sno = new DataColumn("Language_Sno", System.Type.GetType("System.String"));
        DataColumn dColFileName = new DataColumn("IsFiles", System.Type.GetType("System.String"));
        DataColumn dColFrames = new DataColumn("Frames", System.Type.GetType("System.String"));
        DataColumn dColModeOfReceipt_SNo = new DataColumn("ModeOfReceipt_SNo", System.Type.GetType("System.String"));
        DataColumn dColQuantity = new DataColumn("Quantity", System.Type.GetType("System.Int32"));
        DataColumn dColNatureOfComplaint = new DataColumn("NatureOfComplaint", System.Type.GetType("System.String"));
        DataColumn dColInvoiceDate = new DataColumn("InvoiceDate", System.Type.GetType("System.String"));
        DataColumn dColInvoiceNum = new DataColumn("InvoiceNum", System.Type.GetType("System.String"));
        DataColumn dColPurchasedDate = new DataColumn("PurchasedDate", System.Type.GetType("System.String"));
        DataColumn dColPurchasedFrom = new DataColumn("PurchasedFrom", System.Type.GetType("System.String"));
        DataColumn dColTerritory_SNo = new DataColumn("SC_SNo", System.Type.GetType("System.String"));
        DataColumn dColAppointmentReq = new DataColumn("AppointmentReq", System.Type.GetType("System.String"));
        DataColumn dColSRF = new DataColumn("SRF", System.Type.GetType("System.String"));
        DataColumn dColCGExec = new DataColumn("CGExec", System.Type.GetType("System.String"));
        DataColumn dColCGContEmp = new DataColumn("CGContEmp", System.Type.GetType("System.String"));
        dsProduct.Tables[0].Columns.Add(dColSno);
        dsProduct.Tables[0].Columns.Add(dColProductDivision_Sno);
        dsProduct.Tables[0].Columns.Add(dColProductLine_Sno);
        dsProduct.Tables[0].Columns.Add(dColLanguage_Sno);
        dsProduct.Tables[0].Columns.Add(dColPurchasedDate);
        dsProduct.Tables[0].Columns.Add(dColPurchasedFrom);
        dsProduct.Tables[0].Columns.Add(dColFileName);
        dsProduct.Tables[0].Columns.Add(dColFrames);
        dsProduct.Tables[0].Columns.Add(dColModeOfReceipt_SNo);
        dsProduct.Tables[0].Columns.Add(dColQuantity);
        dsProduct.Tables[0].Columns.Add(dColNatureOfComplaint);
        dsProduct.Tables[0].Columns.Add(dColInvoiceDate);
        dsProduct.Tables[0].Columns.Add(dColInvoiceNum);
        dsProduct.Tables[0].Columns.Add(dColTerritory_SNo);
        dsProduct.Tables[0].Columns.Add(dColAppointmentReq);
        dsProduct.Tables[0].Columns.Add(dColSRF);
        dsProduct.Tables[0].Columns.Add(dColCGExec);
        dsProduct.Tables[0].Columns.Add(dColCGContEmp);

        DataTable dTableDisp = new DataTable("tblDisplay");
        dsProduct.Tables.Add(dTableDisp);
        DataColumn dColSnoDisp = new DataColumn("SnoDisp", System.Type.GetType("System.Int16"));
        DataColumn dColProductDivision = new DataColumn("ProductDivision", System.Type.GetType("System.String"));
        DataColumn dColProductLine = new DataColumn("ProductLine", System.Type.GetType("System.String"));
        DataColumn dColLanguage = new DataColumn("Language", System.Type.GetType("System.String"));
        DataColumn dColModeOfReceipt = new DataColumn("ModeOfReceipt", System.Type.GetType("System.String"));
        DataColumn dColQuantityDisp = new DataColumn("QuantityDisp", System.Type.GetType("System.Int32"));
        DataColumn dColNatureOfComplaintDisp = new DataColumn("NatureOfComplaintDisp", System.Type.GetType("System.String"));
        DataColumn dColInvoiceDateDisp = new DataColumn("InvoiceDateDisp", System.Type.GetType("System.String"));
        DataColumn dColInvoiceNumDisp = new DataColumn("InvoiceNumDisp", System.Type.GetType("System.String"));
        DataColumn dColPurchasedDateDisp = new DataColumn("PurchasedDateDisp", System.Type.GetType("System.String"));
        DataColumn dColPurchasedFromDisp = new DataColumn("PurchasedFromDisp", System.Type.GetType("System.String"));
        DataColumn dColSC = new DataColumn("SC", System.Type.GetType("System.String"));
        DataColumn dColWarrantyStatusDisp = new DataColumn("WarrantyStatusDisp", System.Type.GetType("System.String"));
        DataColumn dColVisitChargeDisp = new DataColumn("VisitChargesDisp", System.Type.GetType("System.Decimal"));
        DataColumn dColAppointmentReqDisp = new DataColumn("AppointmentReqDisp", System.Type.GetType("System.String"));
        DataColumn dColSRFDisp = new DataColumn("SRFDisp", System.Type.GetType("System.String"));
        DataColumn dColCGExecDisp = new DataColumn("CGExecDisp", System.Type.GetType("System.String"));
        DataColumn dColCGContEmpDisp = new DataColumn("CGContEmpDisp", System.Type.GetType("System.String"));
        dsProduct.Tables[1].Columns.Add(dColSnoDisp);
        dsProduct.Tables[1].Columns.Add(dColProductDivision);
        dsProduct.Tables[1].Columns.Add(dColProductLine);
        dsProduct.Tables[1].Columns.Add(dColLanguage);
        dsProduct.Tables[1].Columns.Add(dColPurchasedDateDisp);
        dsProduct.Tables[1].Columns.Add(dColPurchasedFromDisp);
        dsProduct.Tables[1].Columns.Add(dColModeOfReceipt);
        dsProduct.Tables[1].Columns.Add(dColQuantityDisp);
        dsProduct.Tables[1].Columns.Add(dColNatureOfComplaintDisp);
        dsProduct.Tables[1].Columns.Add(dColInvoiceDateDisp);
        dsProduct.Tables[1].Columns.Add(dColInvoiceNumDisp);
        dsProduct.Tables[1].Columns.Add(dColVisitChargeDisp);
        dsProduct.Tables[1].Columns.Add(dColSC);
        dsProduct.Tables[1].Columns.Add(dColWarrantyStatusDisp);
        dsProduct.Tables[1].Columns.Add(dColAppointmentReqDisp);
        dsProduct.Tables[1].Columns.Add(dColSRFDisp);
        dsProduct.Tables[1].Columns.Add(dColCGExecDisp);
        dsProduct.Tables[1].Columns.Add(dColCGContEmpDisp);
    }
    private void CreateRow()
    {
        //Create DataRow for Product Division for Data insert based on ids
        DataSet dsTemp = (DataSet)ViewState["ds"];
        intCommon = dsTemp.Tables[0].Rows.Count;
        blnFlag = false;

        if (!blnFlag)    // If record is not already exist for correspodning product division then save it
        {
            #region Data for insert
            DataRow dRow = dsTemp.Tables[0].NewRow();
            dRow["Sno"] = dsTemp.Tables[0].Rows.Count + 1;
            dRow["ProductDivision_Sno"] = ddlProductDiv.SelectedValue.ToString();
            if (ddlProductLine.SelectedValue.ToString() != "0")
            {
                dRow["ProductLine_Sno"] = ddlProductLine.SelectedValue.ToString();
            }
            else
            {
                dRow["ProductLine_Sno"] = "0";
            }
            if (ddlLanguage.SelectedIndex != 0)
            {
                dRow["Language_Sno"] = ddlLanguage.SelectedValue.ToString();
            }
            else
            {
                dRow["Language_Sno"] = "";
            }
            dRow["ModeOfReceipt_SNo"] = ddlModeOfRec.SelectedValue.ToString();
            dRow["Quantity"] = txtQuantity.Text.Trim();
            //if (txtFrames.Text.Trim() == "")
            //{
            //    dRow["Frames"] = 0;
            //}
            //else
            //{
            //    dRow["Frames"] = txtFrames.Text.Trim();
            //}
            dRow["NatureOfComplaint"] = txtComplaintDetails.Text.Trim();
            dRow["InvoiceNum"] = txtInvoiceNum.Text.Trim();
            dRow["PurchasedDate"] = txtxPurchaseDate.Text.Trim();
            dRow["purchasedFrom"] = txtxPurchaseFrom.Text.Trim();
            //if (chkAppointment.Checked)
            //{
            //    dRow["AppointmentReq"] = "1";
            //}
            //else
            //{
            //    dRow["AppointmentReq"] = "0";
            //}
            //if (chkSRF.Checked)
            //{
            //    dRow["SRF"] = "Y";
            //}
            //else
            //{
            //    dRow["SRF"] = "N";
            //}
            dTableFile = (DataTable)ViewState["dTableFile"];
            if (dTableFile.Rows.Count > 0)
            {
                dRow["IsFiles"] = "Y";
            }
            else
            {
                dRow["IsFiles"] = "N";
            }
            //if (txtFrames.Text.Trim() != "")
            //{
            //    if (int.Parse(txtFrames.Text.Trim()) > 180)
            //    {
            //        dRow["SC_SNo"] = "0";
            //    }
            //    else
            //    {
            //        dRow["SC_SNo"] = hdnScId.Value;
            //    }
            //}
            //else
            //{
            //    
            //}
            if (ddlAllocateTo.SelectedValue.ToString() == "3") // 3 For SC
            {
                if (ddlSC.SelectedItem.Text.ToString() != "")
                {
                    dRow["SC_SNo"] = ddlSC.SelectedValue.ToString();
                }
                else
                {
                    dRow["SC_SNo"] = "0";
                }
            }
            else if (ddlAllocateTo.SelectedValue.ToString() == "2") // 2 For CG Exec
            {
                if (ddlCGExec.SelectedItem.Text.ToString() != "")
                {
                    dRow["CGExec"] = ddlCGExec.SelectedValue.ToString();
                }
                else
                {
                    dRow["CGExec"] = "0";
                }
                dRow["SC_SNo"] = "0";
            }
            //Add By Binay-23-11-2009
            else if (ddlAllocateTo.SelectedValue.ToString() == "0")
            {
                if (chkSelfAllocatopn.Checked)
                {
                    objCallRegistration.GetUserType();
                    if (objCallRegistration.AllocateTo == "2")
                    {
                        dRow["CGExec"] = Membership.GetUser().UserName.ToString();

                    }
                    else if (objCallRegistration.AllocateTo == "3")
                    {
                        dRow["SC_SNo"] = Membership.GetUser().UserName.ToString();

                    }
                    else if (objCallRegistration.AllocateTo == "5")
                    {
                        dRow["CGContEmp"] = Membership.GetUser().UserName.ToString();

                    }
                }
               
                    dRow["SC_SNo"] = "0";
                

            }
            //END
            else
            {
                if (ddlCGContractEmp.SelectedItem.Text.ToString() != "")
                {
                    dRow["CGContEmp"] = ddlCGContractEmp.SelectedValue.ToString();
                }
                else
                {
                    dRow["CGContEmp"] = "0";
                }
                dRow["SC_SNo"] = "0";
            }


            dsTemp.Tables[0].Rows.Add(dRow);
            #endregion Data for insert
            #region Data for Display on grid
            DataRow dRowDisp = dsTemp.Tables[1].NewRow();
            dRowDisp["SnoDisp"] = dsTemp.Tables[1].Rows.Count + 1;
            dRowDisp["ProductDivision"] = ddlProductDiv.SelectedItem.Text.ToString();
            if (ddlProductLine.SelectedValue.ToString() != "0")
            {
                dRowDisp["ProductLine"] = ddlProductLine.SelectedItem.Text.ToString();
            }
            else
            {
                dRowDisp["ProductLine"] = "";
            }
            if (ddlLanguage.SelectedIndex != 0)
            {
                dRowDisp["Language"] = ddlLanguage.SelectedItem.Text.ToString();
            }
            else
            {
                dRowDisp["Language"] = "";
            }
            dRowDisp["ModeOfReceipt"] = ddlModeOfRec.SelectedItem.Text.ToString();
            dRowDisp["QuantityDisp"] = txtQuantity.Text.Trim();
            dRowDisp["NatureOfComplaintDisp"] = txtComplaintDetails.Text.Trim();
            dRowDisp["InvoiceNumDisp"] = txtInvoiceNum.Text.Trim();
            dRowDisp["PurchasedDateDisp"] = txtxPurchaseDate.Text.Trim();
            dRowDisp["purchasedFromDisp"] = txtxPurchaseFrom.Text.Trim();
            //if (txtFrames.Text.Trim() != "")
            //{
            //    if (int.Parse(txtFrames.Text.Trim()) > 180)
            //    {
            //        dRowDisp["SC"] = "Suspense Account";
            //    }
            //    else
            //    {
            //        dRowDisp["SC"] = txtSc.Text;
            //    }
            //}
            //else
            //{
            //    dRowDisp["SC"] = txtSc.Text; 
            //}
            if (ddlAllocateTo.SelectedValue.ToString() == "3") // 3 For SC
            {
                if (ddlSC.SelectedItem.Text.ToString() != "")
                {
                    dRowDisp["SC"] = ddlSC.SelectedItem.Text.ToString();
                }
                else
                {
                    dRowDisp["SC"] = "";
                }
            }
            else if (ddlAllocateTo.SelectedValue.ToString() == "2") // 2 For CG Exec
            {
                if (ddlCGExec.SelectedItem.Text.ToString() != "")
                {
                    dRowDisp["CGExecDisp"] = ddlCGExec.SelectedItem.Text.ToString();
                }
                else
                {
                    dRowDisp["CGExecDisp"] = "0";
                }
                dRowDisp["SC"] = "";
            }
                //Add By Binay-23-11-2009
            else if (ddlAllocateTo.SelectedValue.ToString() == "0")
            {
                if (chkSelfAllocatopn.Checked)
                {                   
                    objCallRegistration.GetUserType();
                    if (objCallRegistration.AllocateTo == "2")
                    {
                        dRowDisp["CGExecDisp"] = Membership.GetUser().UserName.ToString(); 
                        
                    }
                    else if (objCallRegistration.AllocateTo == "3")
                    {
                       dRowDisp["SC"] = Membership.GetUser().UserName.ToString();
                          
                    }
                    else if (objCallRegistration.AllocateTo == "5")
                    {                        
                       dRowDisp["CGContEmpDisp"] = Membership.GetUser().UserName.ToString();
                       
                    }
                }

                dRowDisp["SC"] = "";
                
            }
                //END
            else
            {
                if (ddlCGContractEmp.SelectedItem.Text.ToString() != "")
                {
                    dRowDisp["CGContEmpDisp"] = ddlCGContractEmp.SelectedItem.Text.ToString();
                }
                else
                {
                    dRowDisp["CGContEmpDisp"] = "0";
                }
                dRowDisp["SC"] = "";
            }


            //if (chkAppointment.Checked)
            //{
            //    dRowDisp["AppointmentReqDisp"] = "1";
            //}
            //else
            //{
            //    dRowDisp["AppointmentReqDisp"] = "0";
            //}
            //if (chkSRF.Checked)
            //{
            //    dRowDisp["SRFDisp"] = "Y";
            //}
            //else
            //{
            //    dRowDisp["SRFDisp"] = "N";
            //}
            dsTemp.Tables[1].Rows.Add(dRowDisp);
            #endregion Data for insert
            ViewState["ds"] = dsTemp;
            BindGridView();
        }
        else
        {
            lblMsg.Text = "Product division " + ddlProductDiv.SelectedItem.Text + " is already added. You can update data from above listing.";
            ChangeButtonStatus();
        }
    }

    #region Save Customer data and Complaint Data
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if ((ddlAllocateTo.SelectedValue.ToString() != "0") || (chkSelfAllocatopn.Checked == true))
            {
                long lngCustomerId = 0;
                string strUniqueNo = "", strAltNo = "", strValidNumberCus = "", strValidNumberAsc = "";
                bool blnFlagSMSCus = false, blnFlagSMSASC = false;
                string strCustomerMessage = "", strASCMessage = "", strCity = "", strState = "", strTemAdd = "", strTotalSMS = "";

                // if ((ddlProductDiv.SelectedIndex != 0) && (txtQuantity.Text != "") && (txtComplaintDetails.Text != ""))
                if ((txtQuantity.Text != "") && (txtComplaintDetails.Text != ""))
                {
                    CreateRow();
                }
                if (!blnFlag)
                {
                    DataSet dsTemp = (DataSet)ViewState["ds"];
                    if (dsTemp.Tables[0].Rows.Count > 0)
                    {
                        //Assigning properties of clsRegistration class object to save data
                        objCallRegistration.Type = "INSERT_UPDAT_CUSTOMER_DATA";
                        objCallRegistration.CustomerId = int.Parse(hdnCustomerId.Value);
                        if ((ViewState["Party_Code"] != "") && (ViewState["Party_Code"] != null))
                        {
                            objCallRegistration.PartyCode = ViewState["Party_Code"].ToString();
                        }
                        objCallRegistration.UpdateCustDeatil = "Y";// "N";
                        //if (chkUpdateCustomerData.Checked)
                        // objCallRegistration.UpdateCustDeatil = "Y";
                        objCallRegistration.Active_Flag = "1";
                        objCallRegistration.Prefix = ddlPrefix.SelectedValue.ToString();
                        objCallRegistration.FirstName = txtFirstName.Text.Trim();
                        objCallRegistration.LastName = txtLastName.Text.Trim();
                        objCallRegistration.Customer_Type = ddlCustomerType.SelectedValue.ToString();
                        objCallRegistration.Company_Name = txtCompanyName.Text.Trim();
                        objCallRegistration.Address1 = txtAdd1.Text.ToString();
                        objCallRegistration.Address2 = txtAdd2.Text.ToString();
                        objCallRegistration.Landmark = txtAdd3.Text.Trim();
                        objCallRegistration.PinCode = txtPinCode.Text.Trim();
                        objCallRegistration.Country = ddlCountry.SelectedValue.ToString();// "1"; //For country india
                        objCallRegistration.State = ddlState.SelectedValue.ToString();
                        objCallRegistration.City = ddlCity.SelectedValue.ToString();
                        //if (ddlCity.SelectedValue.ToString() == "0")
                        //    objCallRegistration.CityOther = txtOtherCity.Text.Trim();
                        //else
                        //    objCallRegistration.CityOther = null;
                        objCallRegistration.Language = ddlLanguage.SelectedValue.ToString();
                        //objCallRegistration.Remarks = txtRemark.Text.Trim();
                        objCallRegistration.Email = txtEmail.Text.Trim();
                        objCallRegistration.Fax = txtFaxNo.Text.Trim();
                        if ((txtContactNo.Text.Trim().Substring(0, 1) != "0") && (txtContactNo.Text.Trim().Length == 10))
                        {
                            objCallRegistration.UniqueContact_No = "0" + txtContactNo.Text.Trim();
                        }
                        else
                        {
                            objCallRegistration.UniqueContact_No = txtContactNo.Text.Trim();
                        }
                        if (txtAltConatctNo.Text.Trim() == "")
                        {
                            txtAltConatctNo.Text = txtContactNo.Text.Trim();
                        }
                        if (txtAltConatctNo.Text.Trim() != "")
                        {
                            if ((txtAltConatctNo.Text.Trim().Substring(0, 1) != "0") && (txtAltConatctNo.Text.Trim().Length == 10))
                            {
                                objCallRegistration.AltTelNumber = "0" + txtAltConatctNo.Text.Trim();
                            }
                            else
                            {
                                objCallRegistration.AltTelNumber = txtAltConatctNo.Text.Trim();
                            }
                        }
                        else
                        {
                            objCallRegistration.AltTelNumber = txtAltConatctNo.Text.Trim();
                        }
                        if (txtExtension.Text.Trim() == "")
                        {
                            objCallRegistration.Extension = 0;
                        }
                        else
                        {
                            objCallRegistration.Extension = int.Parse(txtExtension.Text.Trim());
                        }
                        objCallRegistration.EmpCode = Membership.GetUser().UserName.ToString();
                        //Inserting customer data to MstCustomerMaster and get CustomerId
                        objCallRegistration.SaveCustomerData();
                        strUniqueNo = txtContactNo.Text.Trim();
                        strAltNo = txtAltConatctNo.Text.Trim();
                        if (CommonClass.ValidateMobileNumber(strUniqueNo, ref strValidNumberCus))
                        {
                            blnFlagSMSCus = true;
                        }
                        else if (CommonClass.ValidateMobileNumber(strAltNo, ref strValidNumberCus))
                        {
                            blnFlagSMSCus = true;
                        }
                        if (objCallRegistration.ReturnValue == 1)
                        {
                            lngCustomerId = objCallRegistration.CustomerId;

                        }
                        ArrayList arrListFiles = new ArrayList();
                        //Inserting DraftComplaintDetails
                        if (lngCustomerId > 0)
                        {
                            #region Upload file
                            //uploading Files to Server on Folder Docs/Customer

                            strFileSavePath = ConfigurationSettings.AppSettings["CustomerFilePath"].ToString();
                            dTableFile = (DataTable)ViewState["dTableFile"];
                            if (flUpload.Value != "")
                            {
                                try
                                {
                                    if (!Directory.Exists(strFileSavePath))
                                    {
                                        Directory.CreateDirectory(Server.MapPath(strFileSavePath));
                                    }
                                    strvFileName = flUpload.Value;
                                    strFileName = Path.GetFileName(strvFileName);
                                    strExt = Path.GetExtension(strvFileName);
                                    strFileName = Path.GetFileNameWithoutExtension(strvFileName) + "_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                                    strFileName = strFileName + strExt;
                                    DataRow dRow = dTableFile.NewRow();
                                    dRow["FileName"] = strFileName;
                                    dTableFile.Rows.Add(dRow);
                                    ViewState["dTableFile"] = dTableFile;
                                    flUpload.PostedFile.SaveAs(Server.MapPath(strFileSavePath + strFileName));
                                }
                                catch (Exception ex)
                                {
                                    //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
                                    // trace, error message 
                                    CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
                                }
                            }
                            dTableFile = (DataTable)ViewState["dTableFile"];
                            for (intCnt = 0; intCnt < dTableFile.Rows.Count; intCnt++)
                            {
                                arrListFiles.Add(dTableFile.Rows[intCnt]["FileName"].ToString());
                            }
                            //Uploading Files End
                            #endregion Upload file
                            //Create Temp table for Complaint + SC Name
                            DataTable dTableTemp = new DataTable();
                            DataColumn dColNo = new DataColumn("SNo");
                            DataColumn dColSCNo = new DataColumn("SC_SNo");
                            DataColumn dColSCName = new DataColumn("SCName");
                            DataColumn dColComplaintNo = new DataColumn("ComplaintRefNo");
                            DataColumn dColProd = new DataColumn("ProductDivision");
                            DataColumn dColCGExecuative = new DataColumn("CGExecuative");
                            DataColumn dColCGContractEmp = new DataColumn("CGContractEmp");
                            dTableTemp.Columns.Add(dColNo);
                            dTableTemp.Columns.Add(dColSCNo);
                            dTableTemp.Columns.Add(dColSCName);
                            dTableTemp.Columns.Add(dColComplaintNo);
                            dTableTemp.Columns.Add(dColProd);
                            dTableTemp.Columns.Add(dColCGExecuative);
                            dTableTemp.Columns.Add(dColCGContractEmp);

                            for (int intCnt = 0; intCnt < dsTemp.Tables[0].Rows.Count; intCnt++)
                            {
                                objCallRegistration.CustomerId = lngCustomerId;
                                objCallRegistration.ModeOfReceipt = dsTemp.Tables[0].Rows[intCnt]["ModeOfReceipt_SNo"].ToString();
                                objCallRegistration.ProductDivision = dsTemp.Tables[0].Rows[intCnt]["ProductDivision_Sno"].ToString();
                                objCallRegistration.ProductLine = dsTemp.Tables[0].Rows[intCnt]["ProductLine_Sno"].ToString();
                                objCallRegistration.Language = dsTemp.Tables[0].Rows[intCnt]["Language_Sno"].ToString();
                                objCallRegistration.IsFiles = dsTemp.Tables[0].Rows[intCnt]["IsFiles"].ToString();
                                objCallRegistration.Quantity = int.Parse(dsTemp.Tables[0].Rows[intCnt]["Quantity"].ToString());
                                objCallRegistration.NatureOfComplaint = dsTemp.Tables[0].Rows[intCnt]["NatureOfComplaint"].ToString();
                                objCallRegistration.InvoiceDate = dsTemp.Tables[0].Rows[intCnt]["InvoiceDate"].ToString();
                                objCallRegistration.PurchasedDate = dsTemp.Tables[0].Rows[intCnt]["PurchasedDate"].ToString();
                                objCallRegistration.PurchasedFrom = dsTemp.Tables[0].Rows[intCnt]["PurchasedFrom"].ToString();
                                //objCallRegistration.AppointmentReq = dsTemp.Tables[0].Rows[intCnt]["AppointmentReq"].ToString();
                                objCallRegistration.AppointmentReq = "0";
                                //objCallRegistration.IsSRF = dsTemp.Tables[0].Rows[intCnt]["SRF"].ToString();

                                objCallRegistration.Territory = dsTemp.Tables[0].Rows[intCnt]["SC_SNo"].ToString();

                                //objCallRegistration.Territory = "0";
                                objCallRegistration.EmpCode = Membership.GetUser().UserName.ToString();
                                //objCallRegistration.Frames = dsTemp.Tables[0].Rows[intCnt]["Frames"].ToString();
                                objCallRegistration.Frames = "0";
                                objCallRegistration.InvoiceNum = dsTemp.Tables[0].Rows[intCnt]["InvoiceNum"].ToString();
                                objCallRegistration.State = ddlState.SelectedValue;
                                objCallRegistration.City = ddlCity.SelectedValue;
                                //if (ddlCity.SelectedValue.ToString() == "0")
                                //    objCallRegistration.CityOther = txtOtherCity.Text.Trim();
                                //else
                                //    objCallRegistration.CityOther = null;
                                // Added by Gaurav on 9 NOV 
                                objCallRegistration.BusinessLine = "1";
                                objCallRegistration.ProductSRNo = txtPrdSRNo.Text.Trim();
                                objCallRegistration.DispatchDate = txtDateofDispatch.Text.Trim();
                                objCallRegistration.Product_Sno = ddlProduct.SelectedValue.ToString();
                                // Added by Gaurav on 8 Dec 
                                objCallRegistration.Branch_Sno = Convert.ToInt32(ddlBranch.SelectedValue.ToString());
                                objCallRegistration.Region_Sno = Convert.ToInt32(ddlRegion.SelectedValue.ToString());

                                // END Added by Gaurav on 8 Dec
                                //Add By Binay-09-12-2009
                                objCallRegistration.LoggedDate = Convert.ToDateTime(txtComplaintLoggedDate.Text.Trim()).ToString();    
                                //End
                                // Added by Gaurav on 10 NOV 
                                //Code Modify By Binay-23-11-2009

                                if (chkSelfAllocatopn.Checked)
                                {
                                    objCallRegistration.GetUserType();
                                    if (objCallRegistration.AllocateTo == "3")
                                    {
                                        objCallRegistration.UserName = Membership.GetUser().UserName.ToString();
                                    }
                                    else if (objCallRegistration.AllocateTo == "2") // 2 For CG EXEC
                                    {
                                        objCallRegistration.UserName = Membership.GetUser().UserName.ToString();
                                    }
                                    else
                                    {
                                        objCallRegistration.UserName = Membership.GetUser().UserName.ToString();
                                    }


                                }
                                else
                                {
                                    objCallRegistration.AllocateTo = ddlAllocateTo.SelectedValue.ToString();
                                    if (objCallRegistration.AllocateTo == "3") // 3 For SC
                                    {
                                        string SCDesc = ddlSC.SelectedItem.ToString();
                                        string[] str1 = SCDesc.Split('-');
                                        string SCUserName = str1[1];
                                        objCallRegistration.UserName = SCUserName;
                                    }
                                    else if (objCallRegistration.AllocateTo == "2") // 2 For CG EXEC
                                    {
                                        objCallRegistration.UserName = ddlCGExec.SelectedValue.ToString();
                                    }
                                    else
                                    {
                                        objCallRegistration.UserName = ddlCGContractEmp.SelectedValue.ToString();
                                    }
                                }

                                //End
                                objCallRegistration.Type = "INSERT_COMPLAINT_DATA";
                                //objCallRegistration.BusinessLine = "MTO";
                                objCallRegistration.SaveComplaintData_LoggedDate();
                                if (objCallRegistration.ReturnValue == -1)
                                {
                                    //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
                                    // trace, error message 
                                    CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), objCallRegistration.MessageOut.ToString());
                                }
                                if (objCallRegistration.ReturnValue == 1)
                                {
                                    #region Save FileData

                                    RequestRegistration objCallreg = new RequestRegistration();
                                    //Saving FileNames to DB
                                    for (int i = 0; i < arrListFiles.Count; i++)
                                    {
                                        objCallreg.EmpCode = Membership.GetUser().UserName.ToString();
                                        objCallreg.Type = "INSERT_COMPLAINT_FILES_DATA";
                                        objCallreg.SaveFilesWithComplaintno(objCallRegistration.Complaint_RefNoOUT, arrListFiles[i].ToString());
                                    }
                                    objCallreg = null;
                                    //End Saving
                                    #endregion Save FileData

                                    #region For Display Final Grid
                                    //Creating row for temp table
                                    DataRow dRowTEMP = dTableTemp.NewRow();

                                    dRowTEMP["SNo"] = dTableTemp.Rows.Count + 1;
                                    dRowTEMP["SC_SNo"] = objCallRegistration.Territory;
                                    dRowTEMP["SCName"] = dsTemp.Tables[1].Rows[intCnt]["SC"].ToString();
                                    dRowTEMP["ProductDivision"] = dsTemp.Tables[1].Rows[intCnt]["ProductDivision"].ToString();
                                    dRowTEMP["CGExecuative"] = dsTemp.Tables[1].Rows[intCnt]["CGExecDisp"].ToString();
                                    dRowTEMP["CGContractEmp"] = dsTemp.Tables[1].Rows[intCnt]["CGContEmpDisp"].ToString();
                                    dRowTEMP["ComplaintRefNo"] = objCallRegistration.Complaint_RefNoOUT;
                                    dTableTemp.Rows.Add(dRowTEMP);
                                    #endregion For Display Final Grid

                                    #region SMS Store to SMS_TRANS
                                    //try
                                    //{

                                    //    //If customer no is valid then send message End
                                    //    //If SC no is valid then send message start
                                    //    blnFlagSMSASC = false;
                                    //    strValidNumberAsc = "";
                                    //    if (int.Parse(objCallRegistration.Territory) > 0)
                                    //    {
                                    //        ServiceContractorMaster objSC = new ServiceContractorMaster();
                                    //        objSC.BindServiceContractorOnSNo(int.Parse(objCallRegistration.Territory), "SELECT_INDIVIDUAL_SC_BASED_ON_SCSNO");
                                    //        //If customer no is valid then send message start
                                    //        if (blnFlagSMSCus)
                                    //        {
                                    //            //Message for customer
                                    //            strCustomerMessage = "Your Complaint No:" + objCallRegistration.Complaint_RefNoOUT + " will be attended by " + objSC.SCName + "-" + objSC.PhoneNo;
                                    //            CommonClass.SendSMS(strValidNumberCus, objCallRegistration.Complaint_RefNoOUT, lngCustomerId.ToString(), DateTime.Now.Date.ToString("yyyyMMdd"), "CGL", strCustomerMessage, strCustomerMessage, "CUS");
                                    //        }
                                    //        if (objSC.MobileNo != "")
                                    //        {
                                    //            if (CommonClass.ValidateMobileNumber(objSC.MobileNo, ref strValidNumberAsc))
                                    //            {
                                    //                blnFlagSMSASC = true;
                                    //            }

                                    //            if (blnFlagSMSASC)
                                    //            {
                                    //                //Message for ASC
                                    //                strASCMessage = objCallRegistration.Complaint_RefNoOUT + "-" + dsTemp.Tables[1].Rows[intCnt]["ProductDivision"].ToString() + "-";
                                    //                if (dsTemp.Tables[1].Rows[intCnt]["AppointmentReqDisp"].ToString() == "1")
                                    //                    strASCMessage += "Y-";
                                    //                else
                                    //                    strASCMessage += "N-";
                                    //                strASCMessage += txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim() + "-" + txtContactNo.Text.Trim() + "-";



                                    //                if (ddlCity.SelectedIndex != 0)
                                    //                {
                                    //                    strCity = ddlCity.SelectedItem.Text.ToString();
                                    //                    strCity = strCity.Substring(0, strCity.IndexOf("("));
                                    //                    //strASCMessage += "-" + strCity;
                                    //                }

                                    //                if (ddlState.SelectedIndex != 0)
                                    //                {
                                    //                    strState = ddlState.SelectedItem.Text.ToString();
                                    //                    strState = strState.Substring(0, strState.IndexOf("("));
                                    //                    //strASCMessage += "-" + strState;
                                    //                }

                                    //                strTemAdd = txtAdd1.Text.Trim() + " " + txtAdd2.Text.Trim();
                                    //                strTotalSMS = strASCMessage + strTemAdd + "-" + strCity + "-" + strState;
                                    //                string strASCCityState = "";
                                    //                int intLen = 0;
                                    //                strASCCityState = strASCMessage + "-" + strCity + "-" + strState;
                                    //                intLen = strASCCityState.Length;
                                    //                intLen = 159 - intLen;
                                    //                if (strTemAdd.Length > intLen)
                                    //                    strTemAdd = strTemAdd.Substring(0, intLen);
                                    //                strASCMessage += strTemAdd + "-" + strCity + "-" + strState;

                                    //                CommonClass.SendSMS(strValidNumberAsc, objCallRegistration.Complaint_RefNoOUT, objSC.SCCode, DateTime.Now.Date.ToString("yyyyMMdd"), "CGL", strASCMessage, strTotalSMS, "ASC");
                                    //            }
                                    //        }
                                    //    }
                                    //    //If SC no is valid then send message end
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
                                    //    // trace, error message 
                                    //    CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
                                    //}
                                    #endregion SMS Store to SMS_TRANS
                                }
                            }
                            ////Assigning DataSource to Grid
                            gvFinal.DataSource = dTableTemp;
                            gvFinal.DataBind();

                            // Added By Gaurav Garg for Hiding the Column of Final Grid on 11 Nov
                            if (dsTemp.Tables[1].Rows[0]["CGExecDisp"].ToString() == "")
                            {
                                gvFinal.Columns[4].Visible = false;
                            }
                            if (dsTemp.Tables[1].Rows[0]["CGContEmpDisp"].ToString() == "")
                            {
                                gvFinal.Columns[5].Visible = false;
                            }
                            if (dsTemp.Tables[1].Rows[0]["SC"].ToString() == "")
                            {
                                gvFinal.Columns[3].Visible = false;
                            }

                            ////End
                            //Clear Files For File Upload Grid
                            dTableFile = (DataTable)ViewState["dTableFile"];
                            dTableFile.Rows.Clear();
                            ViewState["dTableFile"] = dTableFile;
                            ////BindGridFiles();
                            ////gvFiles.Visible = false;

                            //End
                            //Clear Product Grid 
                            dsProduct = (DataSet)ViewState["ds"];
                            dsProduct.Tables[1].Rows.Clear();
                            ViewState["ds"] = dsProduct;
                            BindGridView();
                            //gvComm.Visible = false;
                            //End
                            btnSubmit.CausesValidation = true;
                            tableResult.Visible = true;
                            btnSubmit.Enabled = false;
                            btnAddMore.Enabled = false;
                            btnCancel.Enabled = false;
                            // added by Gaurav Garg on 9 Nov
                            //btnNext.Enabled = false;
                            BtnProcess.Enabled = false;
                            //
                            ClearControls();
                            lblMsg.Text = "";
                        }
                    }
                }
                else
                {
                    lblMsg.Text = "Product division " + ddlProductDiv.SelectedItem.Text + " is already added. You can update data from above listing.";
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(btnSubmit, GetType(), "MyScript11", "alert('Please select at least one option - Allocate to/Allocate to myself');", true);
                //ScriptManager.RegisterClientScriptBlock(btnSave, GetType(), "AddedProd", "alert('Fir Completed');", true);

            }
        }
        catch (Exception ex)
        {
            //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }


    }
    #endregion Save Customer data and Complaint Data

    protected void btnFresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("MTOServiceRegistration.aspx");
    }

    private void BindGridView()
    {
        DataSet dsTemp = (DataSet)ViewState["ds"];
        //gvComm.DataSource = dsTemp.Tables[1];
        //gvComm.DataBind();
    }
    private void ChangeButtonStatus()
    {
        btnAddMore.Text = "Add more";
        btnAddMore.Enabled = true;
        btnSubmit.Enabled = true;
        btnCancel.Text = "Cancel";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (btnCancel.Text.ToLower() == "cancel update")
        {
            ClearComplaintDetails();
            btnCancel.Text = "Cancel";
            btnAddMore.Text = "Add more";
            btnSubmit.Enabled = true;
            lblMsg.Text = "";
        }
        else
        {
            Response.Redirect("MTOServiceRegistration.aspx");
            ClearControls();
            tableResult.Visible = false;
        }
    }

    private void ClearComplaintDetails()
    {
        ddlProductDiv.SelectedIndex = 0;
        txtComplaintDetails.Text = "";
        ddlProductLine.SelectedIndex = 0;
        txtQuantity.Text = "1";
        //ddlSC.SelectedIndex = 0;
        //txtInvoiceNum.Text = "";
        //txtxPurchaseDate.Text = "";
        //txtxPurchaseFrom.Text = "";
        hdnScId.Value = "";
        //if (chkSRF.Checked == true)
        //{
        //    chkSRF.Checked = true;
        //}
        //else
        //{
        //    chkSRF.Checked = false;
        //}
        lblVisitCharge.Text = "0";
    }
    private void ClearControls()
    {
        txtFirstName.Text = "";
        txtLastName.Text = "";
        ddlCustomerType.SelectedIndex = 0;
        txtCompanyName.Text = "";
        txtAdd1.Text = "";
        txtAdd2.Text = "";
        txtAdd3.Text = "";
        ddlState.SelectedIndex = 0;
        ddlCity.SelectedIndex = 0;
        txtPinCode.Text = "";
        //chkAppointment.Checked = false;
        ddlModeOfRec.SelectedIndex = 0;
        txtContactNo.Text = "";
        txtAltConatctNo.Text = "";
        txtExtension.Text = "";
        txtEmail.Text = "";
        txtFaxNo.Text = "";
        txtInvoiceNum.Text = "";
        ddlProductDiv.SelectedIndex = 0;
        ddlProductLine.SelectedIndex = 0;
        //ddlSC.SelectedIndex = 0;
        txtQuantity.Text = "1";
        txtComplaintDetails.Text = "";
        btnAddMore.Text = "Add More";
        txtxPurchaseDate.Text = "";
        txtxPurchaseFrom.Text = "";
        //txtSc.Text = "";
        hdnScId.Value = "";
        lblVisitCharge.Text = "0";
        hdnCustomerId.Value = "0";

        // Added by Gaurav Garg on 9 NOV 
        txtPrdSRNo.Text = "";
        txtDateofDispatch.Text = "";
        ddlAllocateTo.SelectedIndex = 0;
        ddlAllocateTo_SelectedIndexChanged(null, null);
        if(ddlProduct.SelectedValue != "")
        ddlProduct.SelectedIndex = 0;
    }

    protected void btnGoPL_Click(object sender, EventArgs e)
    {
        objRequestReg.ProductLine = ddlProductLine.SelectedValue.ToString();
        objRequestReg.Product_Desc = txtFindPL.Text.ToString();
        objRequestReg.BindProductDdlForFind(ddlProduct);

        ////To fill PL
        if ((txtFindPL.Text == "") && (ddlProductLine.SelectedIndex != 0))
        {
            objCommonClass.BindProduct(ddlProduct, int.Parse(ddlProductLine.SelectedValue.ToString()));
        }

    }
    protected void chkSelfAllocatopn_CheckedChanged(object sender, EventArgs e)
    {
        
        if (chkSelfAllocatopn.Checked)
        {
            ddlAllocateTo.SelectedIndex = 0;
            ddlAllocateTo_SelectedIndexChanged(null, null);
        }
    }
    protected void txtxPurchaseDate_TextChanged(object sender, EventArgs e)
    {
        DateTime dtdate = Convert.ToDateTime(txtxPurchaseDate.Text);
        DateTime dtdate1 = dtdate.AddMonths(12);
        txtPoDate.Text = txtxPurchaseDate.Text;
        txtDateofDispatch.Text = txtxPurchaseDate.Text;
        txtWarrantyDate.Text = dtdate1.ToShortDateString();
    }

    // Added By Gaurav Garg on 28 NOV for Invoice number dropdown which shows in search...
    protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        BtnProcess_Click(null, null);
    }
    // Added By Gaurav Garg on 28 NOV for Product SR number dropdown which shows in search...
    protected void ddlproductSRNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        BtnProcess_Click(null, null);
    }


    // Added By Gaurav Garg on 8 Dec -- As discussed with Mr. Srinivas
    protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRegion.SelectedIndex != 0)
        {
            objRequestReg.Region_Sno = Convert.ToInt32(ddlRegion.SelectedValue.ToString());
            objRequestReg.BindBranch_Region(ddlBranch);
        }
        else
        {
            ddlBranch.Items.Clear();
            ddlBranch.Items.Insert(0, new ListItem("Select", "0"));
        }
        
    }

    #region File Uploading
    protected void btnFileUpload_Click(object sender, EventArgs e)
    {
        if (flUpload.Value != "")
        {
            dTableFile = (DataTable)ViewState["dTableFile"];
            DataRow dRow = dTableFile.NewRow();
            //uploading Files to Server on Folder Docs/Customer
            strFileSavePath = ConfigurationSettings.AppSettings["CustomerFilePath"].ToString();
            strvFileName = flUpload.Value;
            strFileName = Path.GetFileName(strvFileName);
            strExt = Path.GetExtension(strvFileName);
            strFileName = Path.GetFileNameWithoutExtension(strvFileName) + "_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            strFileName = strFileName + strExt;
            flUpload.PostedFile.SaveAs(Server.MapPath(strFileSavePath + strFileName));
            dRow["FileName"] = strFileName;
            dTableFile.Rows.Add(dRow);
            ViewState["dTableFile"] = dTableFile;
            BindGridFiles();
        }
    }
    private void BindGridFiles()
    {
        dTableFile = (DataTable)ViewState["dTableFile"];
        gvFiles.DataSource = dTableFile;
        gvFiles.DataBind();
    }
    protected void gvFiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvFiles.PageIndex = e.NewPageIndex;
        BindGridFiles();
    }
    protected void gvFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        dTableFile = (DataTable)ViewState["dTableFile"];
        strFileSavePath = ConfigurationSettings.AppSettings["CustomerFilePath"].ToString();
        File.Delete(Server.MapPath(strFileSavePath + dTableFile.Rows[e.RowIndex]["FileName"].ToString()));
        dTableFile.Rows.RemoveAt(e.RowIndex);
        BindGridFiles();
    }
    #endregion File Uploading
}
