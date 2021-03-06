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
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class pages_PrintCallSlipCP : System.Web.UI.Page
{
    PrintCallSlip objPrint = new PrintCallSlip();
    CommonPopUp objCommonPopUp = new CommonPopUp();
    public StringBuilder strBody = new StringBuilder();
    SqlDataAccessLayer objSql = new SqlDataAccessLayer();
    string strSplitNo;
    string strBid = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Unit"] = null;
        CHECKCPISDivision();

    }

    private void CHECKCPISDivision()
    {
        objCommonPopUp.UserName = Membership.GetUser().UserName.ToString();
        objCommonPopUp.Type = "Check_CP_And_IS_ForBlankCallSlip";
        objCommonPopUp.CPIS = "CP";
        DataSet ds = objCommonPopUp.BIND_CPAndISDivision();
        if (ds.Tables[0].Rows.Count == 0)
        {
            Divmsg.Visible = true;
            DivContent.Visible = false;
        }
        else
        {
            Divmsg.Visible = false;
            DivContent.Visible = true;
            BindRecord();
        }
    }

    private void BindRecord()
    {
        try
        {
            XmlDocument xmlDoc = null;
            strBid = Convert.ToString(Request.QueryString["BaseLineId"]);
            if (Session["xmlData"] != null)
            {
                xmlDoc = (XmlDocument)Session["xmlData"];
            }
            else if (Request.QueryString["BaseLineId"] != null)
            {
                xmlDoc = new XmlDocument();
                XmlNode root = xmlDoc.CreateElement("Root");
                xmlDoc.AppendChild(root);
                XmlNode rootNode = xmlDoc.CreateElement("ComplaintBaseline");
                root.AppendChild(rootNode);
                XmlNode ComplaintId = xmlDoc.CreateElement("ComplaintId");
                XmlNode BaseLineId = xmlDoc.CreateElement("BaseLineId");

                ComplaintId.AppendChild(xmlDoc.CreateTextNode("0"));
                BaseLineId.AppendChild(xmlDoc.CreateTextNode(strBid));
                rootNode.AppendChild(ComplaintId);
                rootNode.AppendChild(BaseLineId);
            }
            string invoiceDt = "";


            if (string.IsNullOrEmpty(strBid)) strBid = "0";
            if (strBid != "0" || xmlDoc != null)
            {
                DataSet dsuser = new DataSet();
                objCommonPopUp.Type = "POPUP_COMMON_DETAILS_BASELINEID";
                objCommonPopUp.EmpCode = Membership.GetUser().UserName.ToString();
                objCommonPopUp.xmlData = xmlDoc;
                objCommonPopUp.BaseLineId = int.Parse(strBid);
                dsuser = objCommonPopUp.GetCommonDetails();

                for (int i = 0; i < dsuser.Tables[0].Rows.Count; i++)
                //if (dsuser.Tables[0].Rows.Count > 0)
                { 
                    //by @VT
                    ViewState["Unit"] = dsuser.Tables[0].Rows[i]["Unit_Desc"].ToString();

                    strSplitNo = "1";
                    strSplitNo = dsuser.Tables[0].Rows[i]["SplitComplaint_RefNo"].ToString();
                    if (strSplitNo.Length == 1)
                    {
                        strSplitNo = "0" + strSplitNo;
                    }
                    if (Convert.ToString(dsuser.Tables[0].Rows[i]["InvoiceDate"]) != "")
                        invoiceDt = dsuser.Tables[0].Rows[i]["InvoiceDate"].ToString().Substring(0, 10);
                    if (i == 0)
                        strBody = strBody.Append("<div class='page page1'>" +
                            "<table width='700' border='0' align='center' cellpadding='0' cellspacing='0'><tr>" +
                            "<td align='right'><a class='links' href='JavaScript:Printfile()'>Print </a></td></tr></table>");
                    else
                        strBody = strBody.Append("<div class='page'><table style='height:100%!important;'><tr><td>");
                    strBody = strBody.Append("<table width='750' border='0' align='center' cellpadding='0' cellspacing='0' class='table'>");
                    strBody = strBody.Append("<tr><td><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td width='25%'" +
                    "class='TdRightBorder'><!--Column Name--><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>" +
                    "<td>&nbsp;<asp:HiddenField ID='hdnBaseLineId' runat='server' /></td></tr><tr><td align='center'><img alt='' " +
                    "src='../images/CromptonLogo.jpg'></td></tr><tr><td height='22'>&nbsp;<asp:HiddenField ID='hdnSplitNo' runat='server' />" +
                    "</td></tr><tr><td class='boldTxt'>For Product Service Needs</td></tr><tr><td>" +
                    "Call Toll Free - <font class='boldTxt'>1-800-419-0505</font></td></tr></table><!--//Column Name--></td>" +
                    "<td width='50%' class='TdRightBorder'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>" +
                    "<td colspan='4' align='center' class='TableHead' style='font-size:12px'>Crompton Greaves Consumer Electricals Limited" +
                    "<br/>Authorized Service Center</td>" +
                    "</tr><tr><td width='17%'>&nbsp;</td><td width='36%'>&nbsp;<asp:HiddenField ID='hdnComNo' runat='server' /></td>" +
                    "<td width='15%'>&nbsp;</td><td width='32%'>&nbsp;</td></tr><tr><td colspan='4'>Managed By <span>" +
                    " " + dsuser.Tables[0].Rows[i]["Sc_Name"].ToString() + "</span></td></tr><tr><td colspan='4' height='26'>&nbsp;</td></tr><tr><td>Address:</td><td colspan='3'>" +
                    "<span CssClass='label'>" + dsuser.Tables[0].Rows[i]["ASCAdd"].ToString() + "</span></td></tr><tr><td>Place:</td><td>" +
                    "<span CssClass='label'>" + dsuser.Tables[0].Rows[i]["City_Desc"].ToString() + "</span></td><td>State:</td><td>" +
                    "<span CssClass='label'>" + dsuser.Tables[0].Rows[i]["State_Desc"].ToString() + "</span></td></tr><tr><td colspan='4' height='20'>&nbsp;" +
                    "</td></tr><tr><td>Ph. Nos.</td><td colspan='3'><span CssClass='label'>" + Convert.ToString(dsuser.Tables[0].Rows[i]["ASCPHONE1"].ToString()) + "</span>," +
                    "<span CssClass='label'>" + Convert.ToString(dsuser.Tables[0].Rows[i]["ASCPHONE2"].ToString()) + "</span></td></tr><tr><td colspan='4'>" +
                    "Work Timings: <font class='boldTxt'>9.30 A.M. - 5.30 P.M.</font> , Weekly Closed Day: <font class='boldTxt'>" +
                    "<span CssClass='label'>" + Convert.ToString(dsuser.Tables[0].Rows[i]["ASCW"].ToString()) + "</span></font> </td></tr></table></td>" +
                    "<td width='25%' class='TdStyle'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>" +
                    "<td colspan='4' class='TableHead'>Job Sheet - Sr. No.:</td></tr><tr><td width='27%'>&nbsp;</td><td width='31%'>" +
                    "&nbsp;</td><td width='16%'>&nbsp;</td><td width='26%'>&nbsp;</td></tr><tr><td>Job No.:</td><td colspan='3'>");

                    strBody = strBody.Append("<span CssClass='label'>" + dsuser.Tables[0].Rows[i]["Complaint_RefNo"].ToString() + "/" + strSplitNo + "</span></td></tr><tr><td>&nbsp;</td><td>&nbsp;" +
                    "</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>Date:</td><td colspan='3'><span " +
                    " CssClass='label'>" + dsuser.Tables[0].Rows[i]["LoggedDate"].ToString() + "</span></td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>" +
                    " Onsite:</td><td><input type='checkbox' name='checkbox' id='chkJobonsite'></td><td>W/S:</td><td>" +
                    "<input type='checkbox' name='checkbox2' id='chkJobWS'></td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
                    "<td>&nbsp;</td></tr><tr><td colspan='4'>(Call Type)</td></tr><tr><td>Warranty:</td><td><input type='checkbox' name='checkbox3'" +
                    "id='chkJobWarranty'></td><td>O/W:</td><td><input type='checkbox' name='checkbox4' id='chkJobOW'></td></tr>" +
                    "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr>" +
                    "<td colspan='4'>Source Of Complaint:</td></tr><tr>" +
                    "<td colspan='4'><span CssClass='label'>" + Convert.ToString(dsuser.Tables[0].Rows[i]["SourceOfComplaint"].ToString()) + "</span></td></tr><tr>" +
                    "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td colspan='4'>" +
                    "Type Of Complaint:</td></tr>" +

                 

                    "<tr><td colspan='4'><span " +
                    " CssClass='label'>" + dsuser.Tables[0].Rows[i]["TypeOfComplaint"].ToString() + "</span></td></tr></table></td></tr></table></td></tr></table><table width='750' border='0'" +
                    " align='center' cellpadding='0' cellspacing='0'><tr><td width='50%' class='Tdleft' valign='top'>" +
                    "<table width='100%' border='0' cellspacing='1' cellpadding='1' class='table'>" +
                    "<tr><td width='26%'>Company Name:</td><td colspan='3'><span CssClass='label'>" + dsuser.Tables[0].Rows[i]["Company_name"].ToString() + "</span>" +
                    "</td></tr><tr><td width='26%'>Customer Name:</td><td colspan='3'><span CssClass='label'>" + dsuser.Tables[0].Rows[i]["FirstName"].ToString() + "</span>" +
                    "</td></tr><tr><td>Address:</td><td colspan='3'><span CssClass='label'>" + dsuser.Tables[0].Rows[i]["Address1"].ToString() + "</span></td></tr>" +
                    "<tr><td>&nbsp;</td><td colspan='3'><span CssClass='label'>" + dsuser.Tables[0].Rows[i]["Address2"].ToString() + "</span></td></tr>" +
                    "<tr><td colspan='4' height='14'></td></tr><tr><td>Landmark:</td><td colspan='3'><span " +
                    "CssClass='label'>" + dsuser.Tables[0].Rows[i]["Landmark"].ToString() + "</span></td></tr><tr><td>City:</td><td width='29%'><span CssClass='label'" +
                    ">" + dsuser.Tables[0].Rows[i]["City_Desc"].ToString() + "</span></td><td width='24%'>State:</td><td width='24%'><span CssClass='label'>" + dsuser.Tables[0].Rows[i]["State_Desc"].ToString() + "</span>");

                    strBody = strBody.Append("</td></tr><tr><td>Pin:</td><td><span CssClass='label'>" + dsuser.Tables[0].Rows[i]["PinCode"].ToString() + "</span></td><td>&nbsp;</td>" +
                    "<td>&nbsp;</td></tr></table><!--//Column Name--></td>" +
                    "<td width='50%' class='Tdright' valign='top'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>" +
                    "<td width='50%' class='Tdleft' style='border-top: 1px solid #000000'><table width='100%' border='0' cellspacing='1'" +
                    " cellpadding='1'><tr><td width='40%'>Ph. 1:</td><td width='60%'><span CssClass='label'" +
                    ">" + dsuser.Tables[0].Rows[i]["UniqueContact_No"].ToString() + "</span></td></tr><tr><td>Ph. 2:</td><td><span CssClass='label'>" + dsuser.Tables[0].Rows[i]["AltTelNumber"].ToString() + "</span></td></tr>" +
                    "<tr><td colspan='2' height='10'></td></tr><tr><td>E-mail:</td><td><span CssClass='label'>" + dsuser.Tables[0].Rows[i]["Email"].ToString() + "</span>" +
                    "</td></tr><tr><td>Fax:</td><td><span CssClass='label' Text=''>" + dsuser.Tables[0].Rows[i]["Fax"].ToString() + "</span></td></tr><tr><td>Appointment:" +
                    "</td><td>(Date)<span id='appval' CssClass='label'></span></td></tr><tr><td></td><td>(Day)" +
                    "<span id='lblAppReqValue1' CssClass='label'></span></td></tr></table></td>" +
                    "<td width='50%' class='Tdright' style='border-top: 1px solid #000000'><table width='100%' border='0' cellspacing='1' cellpadding='1'>" +
                    "<tr><td colspan='2'>Invoice No :<span CssClass='label'>" + dsuser.Tables[0].Rows[i]["InvoiceNo"].ToString() + "</span></td></tr>" +
                    "<tr><td colspan='2'>Invoice / Dt. Of Purchase:<span id='invDt' CssClass='label'></span></td></tr>" +
                    "<tr><td colspan='2'><span CssClass='label'>" + invoiceDt + "</span></td></tr><tr><td colspan='2' height='13'>" +
                    "</td></tr><tr><td width='44%'>Dealer Name:</td><td width='56%'>&nbsp;</td></tr><tr><td colspan='2'><span " +
                    "CssClass='label'>" + dsuser.Tables[0].Rows[i]["PurchasedFrom"].ToString() + "</span></td></tr><tr><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr></table></td></tr></table>" +
                    "<table width='750' border='0' align='center' cellpadding='0' cellspacing='0'><tr><td class='Tdleft'><!--Column Name-->" +
                    "<table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td height='22' colspan='3' align='center' bgcolor='#CCCCCC'" +
                    " class='TableHead'>Product Information</td></tr><tr><td width='33%' valign='top'><table width='99%' border='0'" +
                    " cellspacing='0' cellpadding='1' class='table' height='70'><tr><td width='38%'>Model No.:</td><td width='62%'>" +
                    "<span id='lblPInfoModelNo' CssClass='label'></span></td></tr><tr><td>Ref. No.:</td><td>" +
                    "<span id='lblPInfoRefNo' CssClass='label'></span></td></tr><tr><td>(Product Sr. No.)</td>" +
                    "<td>&nbsp;</td></tr></table></td><td width='34%' valign='top'><table width='99%' border='0' cellspacing='0'" +
                    " cellpadding='1' class='table' height='70'><tr><td width='38%'>Product Division:</td><td width='62%'>" +
                    "<span CssClass='label'>" + dsuser.Tables[0].Rows[i]["Unit_Desc"].ToString() + "</span></td></tr><tr><td>Product Line:" +
                    "</td><td><span CssClass='label'>" + Convert.ToString(dsuser.Tables[0].Rows[i]["Productline_desc"].ToString()) + "</span></td></tr><tr><td>Product Group:</td>" +
                    "<td><span CssClass='label'>" + Convert.ToString(dsuser.Tables[0].Rows[i]["ProductGroup_desc"].ToString()) + "</span></td></tr></table></td>" +
                    "<td width='33%' valign='top'><table width='100%' border='0' cellspacing='1' cellpadding='1' class='table'>" +
                    "<tr><td colspan='4'><b>Condition of Equipment</b></td></tr><tr><td width='27%'>Tampered:</td><td width='20%'>" +
                    "<input type='checkbox' name='checkbox5' id='chkCETampered'></td><td width='25%'>Used:</td><td width='28%'>" +
                    "<input type='checkbox' name='checkbox7' id='chkCEUsed'></td></tr><tr><td>Broken:</td><td>" +
                    "<input type='checkbox' name='checkbox6' id='chkCEBroken'></td><td>New:</td><td><input type='checkbox'" +
                    " name='checkbox8' id='chkCENew'></td></tr></table></td></tr><tr><td height='20' colspan='3'>Accessories Received:</td></tr> </table>" +
                    "<!--//Column Name--></td></tr></table>");
                    MeargeHTMLBody(dsuser.Tables[0].Rows[i]["NatureOfComplaint"].ToString());

                

                }

                if (objCommonPopUp.ReturnValue == -1)
                {
                    CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), objCommonPopUp.MessageOut);
                }
            }
            else
            {
                strBody = strBody.Append("<div style='height: 28cm!important;'>" +
                    "<table width='700' border='0' align='center' cellpadding='0' cellspacing='0'><tr>" +
                    "<td align='right'><a class='links' href='JavaScript:Printfile()'>Print </a></td></tr></table>");
                strBody = strBody.Append("<div class='page' style='margin:auto;'><table style='height:100%!important;'><tr><td>");
                strBody = strBody.Append("<table width='750' border='0' align='center' cellpadding='0' cellspacing='0' class='table'>");
                strBody = strBody.Append("<tr><td><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td width='25%'" +
                    "class='TdRightBorder'><!--Column Name--><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>" +
                    "<td>&nbsp;<asp:HiddenField ID='hdnBaseLineId' runat='server' /></td></tr><tr><td align='center'><img alt='' " +
                    "src='../images/CromptonLogo.jpg'></td></tr><tr><td height='22'>&nbsp;<asp:HiddenField ID='hdnSplitNo' runat='server' />" +
                    "</td></tr><tr><td class='boldTxt'>For Product Service Needs</td></tr><tr><td>" +
                    "Call Toll Free - <font class='boldTxt'>1-800-419-0505</font></td></tr></table><!--//Column Name--></td>" +
                    "<td width='50%' class='TdRightBorder'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>" +
                    "<td colspan='4' align='center' class='TableHead' style='font-size:12px'>Crompton Greaves Consumer Electricals Limited" +
                    "<br/>Authorized Service Center</td>" +
                    "</tr><tr><td width='17%'>&nbsp;</td><td width='36%'>&nbsp;<asp:HiddenField ID='hdnComNo' runat='server' /></td>" +
                    "<td width='15%'>&nbsp;</td><td width='32%'>&nbsp;</td></tr><tr><td colspan='4'>Managed By <span>" +
                    " " + " " + "</span></td></tr><tr><td colspan='4' height='26'>&nbsp;</td></tr><tr><td>Address:</td><td colspan='3'>" +
                    "<span CssClass='label'>" + " " + "</span></td></tr><tr><td>Place:</td><td>" +
                    "<span CssClass='label'>" + " " + "</span></td><td>State:</td><td>" +
                    "<span CssClass='label'>" + " " + "</span></td></tr><tr><td colspan='4' height='20'>&nbsp;" +
                    "</td></tr><tr><td>Ph. Nos.</td><td colspan='3'><span CssClass='label'>" + " " + "</span>," +
                    "<span CssClass='label'>" + " " + "</span></td></tr><tr><td colspan='4'>" +
                    "Work Timings: <font class='boldTxt'>9.30 A.M. - 5.30 P.M.</font> , Weekly Closed Day: <font class='boldTxt'>" +
                    "<span CssClass='label'>" + " " + "</span></font> </td></tr></table></td>" +
                    "<td width='25%' class='TdStyle'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>" +
                    "<td colspan='4' class='TableHead'>Job Sheet - Sr. No.:</td></tr><tr><td width='27%'>&nbsp;</td><td width='31%'>" +
                    "&nbsp;</td><td width='16%'>&nbsp;</td><td width='26%'>&nbsp;</td></tr><tr><td>Job No.:</td><td colspan='3'>");

                   strBody = strBody.Append("<span CssClass='label'>" + " " + "/" + strSplitNo + "</span></td></tr><tr><td>&nbsp;</td><td>&nbsp;" +
                    "</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>Date:</td><td colspan='3'><span " +
                    " CssClass='label'>" + " " + "</span></td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>" +
                    " Onsite:</td><td><input type='checkbox' name='checkbox' id='chkJobonsite'></td><td>W/S:</td><td>" +
                    "<input type='checkbox' name='checkbox2' id='chkJobWS'></td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>" +
                    "<td>&nbsp;</td></tr><tr><td colspan='4'>(Call Type)</td></tr><tr><td>Warranty:</td><td><input type='checkbox' name='checkbox3'" +
                    "id='chkJobWarranty'></td><td>O/W:</td><td><input type='checkbox' name='checkbox4' id='chkJobOW'></td></tr>" +
                    "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr>" +
                    "<td colspan='4'>Source Of Complaint:</td></tr><tr>" +
                    "<td colspan='4'><span CssClass='label'>" + " " + "</span></td></tr><tr>" +
                    "<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td colspan='4'>" +
                    "Type Of Complaint:</td></tr>" +


                    //"<tr><td colspan='4'>Happy Code:</td></tr><tr><td><table border='2'><tbody><tr colspan='10'>" +
                    //"<td height='20' width='15'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                    //"<td height='20' width='15'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                    //"<td height='20' width='15'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                    //"<td height='20' width='15'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                    //"<td height='20' width='15'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                    //"</tr></tbody></table></td></tr>" +




                    "<tr><td colspan='4'><span " +
                    " CssClass='label'>" + " " + "</span></td></tr></table></td></tr></table></td></tr></table><table width='750' border='0'" +
                    " align='center' cellpadding='0' cellspacing='0'><tr><td width='50%' class='Tdleft' valign='top'>" +
                    "<table width='100%' border='0' cellspacing='1' cellpadding='1' class='table'>" +
                    "<tr><td width='26%'>Company Name:</td><td colspan='3'><span CssClass='label'>" + " " + "</span>" +
                    "</td></tr><tr><td width='26%'>Customer Name:</td><td colspan='3'><span CssClass='label'>" + " " + "</span>" +
                    "</td></tr><tr><td>Address:</td><td colspan='3'><span CssClass='label'>" + " " + "</span></td></tr>" +
                    "<tr><td>&nbsp;</td><td colspan='3'><span CssClass='label'>" + " " + "</span></td></tr>" +
                    "<tr><td colspan='4' height='14'></td></tr><tr><td>Landmark:</td><td colspan='3'><span " +
                    "CssClass='label'>" + " " + "</span></td></tr><tr><td>City:</td><td width='29%'><span CssClass='label'" +
                    ">" + " " + "</span></td><td width='24%'>State:</td><td width='24%'><span CssClass='label'>" + " " + "</span>");

                strBody = strBody.Append("</td></tr><tr><td>Pin:</td><td><span CssClass='label'>" + " " + "</span></td><td>&nbsp;</td>" +
                    "<td>&nbsp;</td></tr></table><!--//Column Name--></td>" +
                    "<td width='50%' class='Tdright' valign='top'><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>" +
                    "<td width='50%' class='Tdleft' style='border-top: 1px solid #000000'><table width='100%' border='0' cellspacing='1'" +
                    " cellpadding='1'><tr><td width='40%'>Ph. 1:</td><td width='60%'><span CssClass='label'" +
                    ">" + " " + "</span></td></tr><tr><td>Ph. 2:</td><td><span CssClass='label'>" + " " + "</span></td></tr>" +
                    "<tr><td colspan='2' height='10'></td></tr><tr><td>E-mail:</td><td><span CssClass='label'>" + " " + "</span>" +
                    "</td></tr><tr><td>Fax:</td><td><span CssClass='label' Text=''>" + " " + "</span></td></tr><tr><td>Appointment:" +
                    "</td><td>(Date)<span id='appval' CssClass='label'></span></td></tr><tr><td></td><td>(Day)" +
                    "<span id='lblAppReqValue1' CssClass='label'></span></td></tr></table></td>" +
                    "<td width='50%' class='Tdright' style='border-top: 1px solid #000000'><table width='100%' border='0' cellspacing='1' cellpadding='1'>" +
                    "<tr><td colspan='2'>Invoice No :<span CssClass='label'>" + " " + "</span></td></tr>" +
                    "<tr><td colspan='2'>Invoice / Dt. Of Purchase:<span id='invDt' CssClass='label'></span></td></tr>" +
                    "<tr><td colspan='2'><span CssClass='label'>" + invoiceDt + "</span></td></tr><tr><td colspan='2' height='13'>" +
                    "</td></tr><tr><td width='44%'>Dealer Name:</td><td width='56%'>&nbsp;</td></tr><tr><td colspan='2'><span " +
                    "CssClass='label'>" + " " + "</span></td></tr><tr><td>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr></table></td></tr></table>" +
                    "<table width='750' border='0' align='center' cellpadding='0' cellspacing='0'><tr><td class='Tdleft'><!--Column Name-->" +
                    "<table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td height='22' colspan='3' align='center' bgcolor='#CCCCCC'" +
                    " class='TableHead'>Product Information</td></tr><tr><td width='33%' valign='top'><table width='99%' border='0'" +
                    " cellspacing='0' cellpadding='1' class='table' height='70'><tr><td width='38%'>Model No.:</td><td width='62%'>" +
                    "<span id='lblPInfoModelNo' CssClass='label'></span></td></tr><tr><td>Ref. No.:</td><td>" +
                    "<span id='lblPInfoRefNo' CssClass='label'></span></td></tr><tr><td>(Product Sr. No.)</td>" +
                    "<td>&nbsp;</td></tr></table></td><td width='34%' valign='top'><table width='99%' border='0' cellspacing='0'" +
                    " cellpadding='1' class='table' height='70'><tr><td width='38%'>Product Division:</td><td width='62%'>" +
                    "<span CssClass='label'>" + " " + "</span></td></tr><tr><td>Product Line:" +
                    "</td><td><span CssClass='label'>" + " " + "</span></td></tr><tr><td>Product Group:</td>" +
                    "<td><span CssClass='label'>" + " " + "</span></td></tr></table></td>" +
                    "<td width='33%' valign='top'><table width='100%' border='0' cellspacing='1' cellpadding='1' class='table'>" +
                    "<tr><td colspan='4'><b>Condition of Equipment</b></td></tr><tr><td width='27%'>Tampered:</td><td width='20%'>" +
                    "<input type='checkbox' name='checkbox5' id='chkCETampered'></td><td width='25%'>Used:</td><td width='28%'>" +
                    "<input type='checkbox' name='checkbox7' id='chkCEUsed'></td></tr><tr><td>Broken:</td><td>" +
                    "<input type='checkbox' name='checkbox6' id='chkCEBroken'></td><td>New:</td><td><input type='checkbox'" +
                    " name='checkbox8' id='chkCENew'></td></tr></table></td></tr><tr><td height='20' colspan='3'>Accessories Received:</td></tr> </table>" +
                    "<!--//Column Name--></td></tr></table>");
                MeargeHTMLBody("");
            }
        }
        catch (Exception ex)
        {
            //Writing Error message to File using CommonClass WriteErrorErrFile method taking arguments as URL of page
            // trace, error message 
            CommonClass.WriteErrorErrFile(Request.RawUrl.ToString(), ex.StackTrace.ToString() + "-->" + ex.Message.ToString());
        }
    }

    private void MeargeHTMLBody(string natureOfComplaint)
    {
        

       string Unit = Convert.ToString(ViewState["Unit"]);

       string productdefect =Defecttype(Unit);
        strBody = strBody.Append("<table width='750' border='0' align='center' cellpadding='0' cellspacing='0'><tr><td class='Tdleft'>" +
                    "<!--Column Name--><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td valign='top'>" +
                    "<table width='99%' border='0' cellspacing='0' cellpadding='1' class='table'><tr><td width='25%' valign='top'>" +
                    "Fault Reported :<br>(By Customer)</td><td width='75%' height='60' valign='top'><span" +
                    " CssClass='label'>" + natureOfComplaint + "</span></td></tr></table></td><td width='50%' valign='top'>" +
                    "<table width='100%' border='0' cellspacing='4' cellpadding='1' class='table'><tr><td width='50%' valign='top'>" +
                    "&nbsp;</td><td width='50%' height='30' valign='top'>&nbsp;</td></tr><tr><td align='center' valign='top'" +
                    " style='border-top: 1px solid #000000'>Customer Signature and Date</td><td align='center'" +
                    " valign='top' style='border-top: 1px solid #000000'>ASC Signature and Date</td></tr></table></td></tr></table>" +
                    "<!--//Column Name--></td></tr></table><table width='750' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                    "<tr><td class='Tdleft'><!--Column Name--><table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                    "<tr><td height='20' colspan='3' align='center' bgcolor='#CCCCCC' class='TableHead'>Technician's Report" +
                    "</td></tr>" +

                    "<tr><td width='50%' valign='top'><table width='99%' border='0' cellspacing='0' cellpadding='1' class='table' height='70'>" +
                    "<tr><td width='38%' valign='top'>Defect Observed:</td><td width='62%' height='40' valign='top'>&nbsp;" +
                    "</td></tr><tr><td height='20'>Site Condition: (volt / amp)</td><td>&nbsp;</td></tr><tr><td height='20'>" +
                    "Status of Installation:</td><td>OK<input type='checkbox' name='checkbox5' id='CHKOK' />&nbsp; Not OK" +
                    "<input type='checkbox' name='checkbox7' id='Checkbox2' /></td></tr><tr><td height='20'>Cable Used:</td>" +
                    "<td>&nbsp;</td></tr><tr><td height='20'>Whether Starter Used:</td><td>&nbsp;</td></tr></table></td>" +
                    "<td width='50%'  valign='top'><table width='100%' border='0' cellspacing='1' cellpadding='1' class='table'>" +
                    "<tr><td colspan='3'>Action Taken / Repair Report:</td></tr><tr><td height='45' colspan='3'>&nbsp;</td></tr>" +
                    "<tr><td height='44' colspan='3'>&nbsp;</td></tr><tr><td width='25%' align='center' style='border-top: 1px solid #000000'>" +
                    "Date:</td><td width='40%' align='center' style='border-top: 1px solid #000000'>Technician's Name</td>" +
                    "<td width='35%' align='center' style='border-top: 1px solid #000000'>Signature and Date</td></tr></table></td></tr></table>" +
                    "<!--//Column Name--></td></tr></table>" +



                    "<table width='750' border='0' align='center' cellpadding='0' cellspacing='0'><tbody><tr><td class='Tdleft'>" +
                    "<table width='100%' border='1' cellspacing='0' cellpadding='0'>" +
                    "<tbody>" +
                     productdefect +
                  

                    "</tbody></table></td></tr></tbody></table>" +


                    "<table width='750' border='0' align='center' cellpadding='0' cellspacing='0'><tr><td class='Tdleft'>" +
                    "<!--Column Name--><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr>" +
                    "<td height='20' colspan='2' align='center' bgcolor='#CCCCCC' class='TableHead'>Parts Used</td></tr>" +
                    "<tr><td valign='top' bgcolor='#000000'><table width='100%' border='1' cellspacing='0' cellpadding='0'>" +
                    "<tr><td width='4%' height='18' align='center' bgcolor='#FFFFFF'>Sr.</td><td width='21%' align='center' bgcolor='#FFFFFF'>" +
                    "Description</td><td width='15%' align='center' bgcolor='#FFFFFF'>Part Code</td>" +
                    "<td width='6%' align='center' bgcolor='#FFFFFF'>Qty</td>" +
                    "<td width='26%' align='center' bgcolor='#FFFFFF'>Location</td>" +
                    "<td colspan='2' align='center' bgcolor='#FFFFFF'>Unit Price</td>" +
                    "<td colspan='2' align='center' bgcolor='#FFFFFF'>Total</td></tr>" +
                    "<tr><td height='19' bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>" +
                    "&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td width='7%' bgcolor='#FFFFFF'>&nbsp;Rs" +
                    "</td><td width='7%' bgcolor='#FFFFFF'>&nbsp;Ps</td><td width='7%' bgcolor='#FFFFFF'>" +
                    "&nbsp;Rs</td><td width='7%' bgcolor='#FFFFFF'>&nbsp;Ps</td></tr><tr>" +
                    "<td height='19' bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td></tr><tr><td height='19' bgcolor='#FFFFFF'>&nbsp;" +
                    "</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td></tr>" +
                    "<tr><td height='19' bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td></tr></table></td></tr></table><!--//Column Name--></td></tr></table>" +
                    "<table width='750' border='0' align='center' cellpadding='0' cellspacing='0'><tr>" +
                    "<td class='Tdleft'><!--Column Name--><table width='100%' border='0' cellspacing='0' cellpadding='0'>" +
                    "<tr><td width='33%' height='22' align='center'  bgcolor='#CCCCCC' class='TableHead'>Happy Code</td>" +
                    "<td width='67%' height='22' align='center' bgcolor='#CCCCCC' class='TableHead'>Provisional Receipt</td></tr>" +
                    "<tr><td width='33%' height='20' align='center' valign='bottom'>" +
                    "<table width='96%' border='0' align='center' cellpadding='0' cellspacing='0'><tr><td>" +
                                    "<table border='2'><tbody><tr>" +
                   "<td height='20'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                   "<td height='20'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                   "<td height='20'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                   "<td height='20'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                   "<td height='20'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                   "</tr> </tbody></table>" +
                    "</td></tr> " +

                    "<tr height='25px'></tr>"+

                    "<tr><td align='center' style='border-top: 1px solid #000000'>Customer's Signature &amp; Date</td></tr>" +
                    "<tr><td>&nbsp;</td></tr>"+
                    "<tr height='25px'></tr>"+
                    "<tr><td>I hereby acknowledge that I am satisfied with the repair of my equipment</td>" +
                    "</tr></table></td><td width='67%' height='20' align='center' bgcolor='#000000'>" +
                    "<table width='100%' border='.5' cellspacing='0' cellpadding='1'><tr>" +
                    "<td width='26%' align='center' bgcolor='#FFFFFF'>(A)</td><td width='10%' align='center' bgcolor='#FFFFFF'>" +
                    "Rs</td><td width='10%' align='center' bgcolor='#FFFFFF'>Ps</td><td width='34%' align='center' bgcolor='#FFFFFF'>" +
                    "(B)</td><td width='10%' align='center' bgcolor='#FFFFFF'>Rs</td><td width='10%' align='center' bgcolor='#FFFFFF'>" +
                    "Ps</td></tr><tr><td bgcolor='#FFFFFF'>Service Charges:<br>(incldg. Serv. Tax)</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>Material Charges</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td></tr><tr><td bgcolor='#FFFFFF'>Auxillary Charges</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>VAT</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td></tr><tr><td bgcolor='#FFFFFF'>" +
                    "&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>Material Charges with VAT</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td></tr>" +
                    "<tr><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td></tr>" +
                    "<tr><td align='center' bgcolor='#FFFFFF'>Total (A)</td><td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>" +
                    "&nbsp;</td><td align='center' bgcolor='#FFFFFF'>Total (B)</td><td bgcolor='#FFFFFF'>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td></tr><tr><td height='15' colspan='4' align='right'" +
                    " style='background-color: Gray' bgcolor='#CCCCCC'><b>TOTAL REPAIR CHARGES (A+B)</b>&nbsp;</td>" +
                    "<td bgcolor='#FFFFFF'>&nbsp;</td><td bgcolor='#FFFFFF'>&nbsp;</td></tr></table></td></tr></table>" +
                    "<!--//Column Name--></td></tr></table><table width='800' border='0' align='center' cellpadding='0' cellspacing='0'>" +
                    "<tr><td height='22' align='center' valign='middle'><font size='2'>" +
                    "Registered Office: Equinox Business Park, 1st Floor, Tower 3, LBS Marg, Kurla (W), Mumbai – 400070, INDIA " +
               // "Registered Office: CG House, 6th Floor, Dr. Annie Besant Road. Worli,Mumbai - 400 030, INDIA "+ 
                    "<a href='http://www.crompton.co.in'>www.crompton.co.in</a></font></td>" +
                    "</tr></table></td></tr></table></div><br style='page-break-after: always!important' />");
        Session.Remove("xmlData");
    }
    // by  @VT 
    private string Defecttype(string Unit)
    {
        string txt = "";
        string header = "<tr><td height='20' colspan='5' align='center' bgcolor='#CCCCCC' class='TableHead'>" + Unit + " Defect</td></tr>";

        if (Unit.ToLower()=="fans")
        {
            txt = header + " <tr><td width='33%' valign='top'>Noise</td><td width='33%' valign='top'>Winding</td><td width='33%' valign='top'>Finish Defects</td></tr>" +
                    "<tr><td width='33%' valign='top'>Transit Damage</td><td width='33%' valign='top'>Slow Speed</td><td width='33%' valign='top'>Other</td></tr>";

        }
        //if (Unit.ToLower() == "appliances")
        //{
        //    txt = header + " <tr><td width='33%' valign='top'> </td><td width='33%' valign='top'> </td><td width='33%' valign='top'> </td></tr>" +
        //           "<tr><td width='33%' valign='top'> </td><td width='33%' valign='top'> </td><td width='33%' valign='top'></td></tr>";

        //}
        if (Unit.ToLower() == "pumps") 
        {
            txt = header + " <tr><td width='33%' valign='top'>Winding Failure</td><td width='33%' valign='top'>Pump Jam</td><td width='33%' valign='top'>Motor Defects</td></tr>" +
                    "<tr><td width='33%' valign='top'>Performance Problems</td><td width='33%' valign='top'>Application Error</td><td width='33%' valign='top'>Other</td></tr>";

        }
        return txt;
    }
}
