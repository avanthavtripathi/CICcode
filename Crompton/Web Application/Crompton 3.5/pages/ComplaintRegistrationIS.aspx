﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComplaintRegistrationIS.aspx.cs" Inherits="pages_ComplaintRegistrationIS" %>
<%@ Register Assembly="AjaxControlToolkit"   Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Complaint Registration Form</title>
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>

    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
 </asp:ScriptManager>
 <script language="javascript" type="text/javascript">
     function funOpenDivSC() 
	 {
         var strProductDivision = document.getElementById('<%=ddlProductDiv.ClientID %>');
         var strState = document.getElementById('<%=ddlState.ClientID %>');
         var strCity = document.getElementById('<%=ddlCity.ClientID %>');
         if (strState.value == "Select") 
		 {
             alert("Please select State");
             strState.focus();
             return false;
         }
         if (strCity.value == "Select") 
		 {
             alert("Please select City");
             strCity.focus();
             return false;
         }
         if (strProductDivision.value == "0") 
		 {
             alert("Please select Product Division");
             strProductDivision.focus();
             return false;
         }
         document.getElementById("dvSearch").style.display = 'block';

     }
     function funConfirm() 
	 {
         if (confirm('Are you sure to cancel this request?')) 
		 {
             return true;
         }
         return false;
     }


     function goBack() 
	 {
         window.history.back()
     }

</script>

<table width="1000px" align="center" style="border-left: 1px Solid #080000;border-right: 1px Solid #080000">
        
          <tr>
            <td class="bgcolorcomm" colspan="2">
                    <table width="99%">
                        <tr style="margin-top:5px;">
                            <td class="headingred" align="center" style="font-size:15px">
                            Customer Feedback Registration</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="pnl" runat="server">
                                    <ContentTemplate>
                                        <table width="100%">
                                     
                                            <tr height="20">
                                                <td align="right">
                                                   <asp:HiddenField ID="hdnCustomerId" Value="0" runat="server" />
                                                </td>
                                                <td align="<%=ConfigurationManager.AppSettings["AjaxLoadingAlign"]%>" style="padding-right: 10px;">
                                                    <asp:UpdateProgress AssociatedUpdatePanelID="pnl" ID="UpdateProgress1" runat="server">
                                                        <ProgressTemplate>
                                                            <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" /></ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table width="100%" border="0" cellpadding="1" cellspacing="0" class="bgcolorcomm">
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;</td>
                                                            <td align="<%=ConfigurationManager.AppSettings["MandatoryTextAlign"]%>">
                                                               
                                                                <font color="red">*</font>&nbsp;<%=ConfigurationManager.AppSettings["MandatoryText"]%>
                                                            </td>
                                                        </tr>
                                                        <tr style="font-weight:bold;font-size:14px;line-height:40px;background-color:#dddddd">
                                                            <td align="left" style="padding-left:10px">
                                                                Service Request Type<font color="red">*</font></td>
                                                            <td colspan="2">
                                                                <asp:DropDownList ID="ddlFeedbackType" runat="server" AutoPostBack="True" 
                                                                    CssClass="simpletxt1" 
                                                                    OnSelectedIndexChanged="ddlFeedbackType_SelectedIndexChanged" Width="172px">
                                                                    <asp:ListItem Selected="True" Text="Select" Value="0" />
                                                            <asp:ListItem Text="Query" Value="3" />
                                                            <asp:ListItem Text="Feedback" Value="2" />
                                                            <asp:ListItem Text="Installation/Demonstration" Value="6" />
                                                            <asp:ListItem Text="Breakdown/Complaint" Value="1" />
                                                                </asp:DropDownList>
                                                                <asp:CompareValidator ID="CVFeedbackType" runat="server" 
                                                                    ControlToValidate="ddlFeedbackType" ErrorMessage="Feedback Type is required." Operator="NotEqual" 
                                                                    SetFocusOnError="true" ValueToCompare="0"></asp:CompareValidator>
                                                            </td>
                                                        </tr>
                                                        <tr style="background-color:#dddddd">
                                                            <td align="left" colspan="3">
                                                            
                                                                <asp:RadioButtonList ID="ChkLst" runat="server" Enabled="False" 
                                                                    AutoPostBack="True" 
                                                                    onselectedindexchanged="ChkLst_SelectedIndexChanged"  >
                                                                    <asp:ListItem Selected="True" Text="Customer" Value="1" ></asp:ListItem>
                                                                    <asp:ListItem Text="Authorized Service Center" Value="2" ></asp:ListItem>
                                                                </asp:RadioButtonList> 
                                                            </td>
                                                        </tr>
                                                        <tr id="trlogin" style="background-color:#dddddd" runat="server" visible="false" >
                                                            <td colspan="3" style="padding:5px" >
                                                            UserName : <asp:TextBox ID="TxtSCName" runat="server" CssClass="txtboxtxt" Width="150px"
                                                                    MaxLength="30"></asp:TextBox>
                                                                &nbsp; Password : <asp:TextBox ID="TxtPwd" runat="server" CssClass="txtboxtxt" Width="150px"
                                                        MaxLength="30" TextMode="Password"></asp:TextBox>
                                                                &nbsp;
                                                                <asp:Button ID="BtnGo" runat="server" CssClass="btn" CausesValidation="false" 
                                                                    Text="Go" onclick="BtnGo_Click" />
                                                             &nbsp;<asp:Label ID="LblLogin" runat="server"> </asp:Label>
                                                          </td>
                                                           
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                &nbsp;</td>
                                                            <td align='<%=ConfigurationManager.AppSettings["MandatoryTextAlign"]%>'>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                     <td colspan="3">
                                                  <asp:Panel id="tblenable" runat="server" >
                                                     <table width="100%" border="0">
                                                        <tr>
                                                            <td colspan="3">
                                                                <b>Customer Information:</b> &nbsp;
                                                            </td>
                                                            <td align='<%=ConfigurationManager.AppSettings["MandatoryTextAlign"]%>'>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td width="120px">
                                                                Salutation
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:DropDownList ID="ddlPrefix" runat="server" CssClass="simpletxt1">
                                                                    <asp:ListItem Selected="True" Value="Mr">Mr.</asp:ListItem>
                                                                    <asp:ListItem Value="Ms">Ms.</asp:ListItem>
                                                                    <asp:ListItem Value="Mrs">Mrs.</asp:ListItem>
                                                                    <asp:ListItem Value="Dr">Dr.</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Name<font color="red">*</font>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="txtboxtxt" Width="140px" MaxLength="15"></asp:TextBox>
                                                                &nbsp; <asp:TextBox ID="txtMiddleName" runat="server" CssClass="txtboxtxt" Width="140" MaxLength="15"></asp:TextBox>
                                                                &nbsp;&nbsp;<asp:TextBox ID="txtLastName" runat="server" CssClass="txtboxtxt" MaxLength="15" Width="140px"></asp:TextBox>
                                                                &nbsp;<div>
                                                                <cc1:TextBoxWatermarkExtender ID="exn1" runat="server" TargetControlID="txtFirstName" WatermarkCssClass="GrayCss"  WatermarkText="First Name" />
                                                                <cc1:TextBoxWatermarkExtender ID="exn2" runat="server" TargetControlID="txtMiddleName" WatermarkCssClass="GrayCss" WatermarkText="Middle Name" />
                                                                <cc1:TextBoxWatermarkExtender ID="exn3" runat="server" TargetControlID="txtLastName" WatermarkCssClass="GrayCss" WatermarkText="Last Name" />
                                                                    <asp:RequiredFieldValidator ID="rfvFname" runat="server" 
                                                                        ControlToValidate="txtFirstName" Display="Dynamic" 
                                                                        ErrorMessage="First Name is required." SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                    <asp:RequiredFieldValidator ID="rfvLname" runat="server" 
                                                                        ControlToValidate="txtLastName" Display="Dynamic" 
                                                                        ErrorMessage="Last Name is required." SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Company Name
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox runat="server" MaxLength="150" Width="350Px" CssClass="txtboxtxt" ID="txtCompanyName"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Address 1<font color='red'>*</font>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox runat="server" MaxLength="50" Width="350Px" CssClass="txtboxtxt" ID="txtAdd1"></asp:TextBox>
                                                                <div>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" SetFocusOnError="true"
                                                                    ControlToValidate="txtAdd1" ErrorMessage="Address1 is required." Display="Dynamic"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                              Address 2
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtAdd2" runat="server" CssClass="txtboxtxt" MaxLength="50" Width="350px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr id="trLandMark" runat="server">
                                                            <td>
                                                                Landmark
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtLandmark" runat="server" CssClass="txtboxtxt" MaxLength="150" Width="350px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" style="width:24%"> State<font color='red'>*</font>
                                                            </td>
                                                            <td style="width:26%" >
                                                                <asp:DropDownList Width="170px" runat="server" CssClass="simpletxt1" AutoPostBack="true"
                                                                    ID="ddlState" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" >
                                                                </asp:DropDownList>
                                                                <div>
                                                                   <asp:CompareValidator SetFocusOnError="true" ID="CompareValidator1" runat="server"
                                                                    ControlToValidate="ddlState" ErrorMessage="State is required." Operator="NotEqual" ValueToCompare="Select" Display="Dynamic"></asp:CompareValidator>
                                                                </div>
                                                            </td>
                                                            <td valign="top" style="width:15%"> City<font color='red'>*</font>
                                                            </td>
                                                            <td valign="top" style="width:35%">
                                                                <asp:DropDownList Width="170px" ID="ddlCity" AutoPostBack="true" runat="server" CssClass="simpletxt1"
                                                                    OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                                                                    <asp:ListItem Text="Select" Value="Select" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                                <div>
                                                                    <asp:CompareValidator SetFocusOnError="true" ID="CompareValidator2" runat="server"
                                                                    ControlToValidate="ddlCity" ErrorMessage="City is required." Operator="NotEqual" ValueToCompare="Select" Display="Dynamic"></asp:CompareValidator>
                                                                </div>
                                                                <asp:TextBox ID="txtOtherCity" Width="100px" MaxLength="30" runat="server" CssClass="txtboxtxt"></asp:TextBox>
                                                                <div>
                                                                   <asp:RequiredFieldValidator ID="reqCityOther" runat="server" SetFocusOnError="true" ErrorMessage="Other City is required."
                                                                    ControlToValidate="txtOtherCity" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPin" runat="server">
                                                            <td>
                                                                Pin Code
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtPinCode" EnableViewState="true" runat="server" CssClass="txtboxtxt" MaxLength="6" ></asp:TextBox>
                                                                <div>
                                                                    <asp:RegularExpressionValidator ID="RfvPin" runat="server" ErrorMessage="Valid pin is required."
                                                                    ControlToValidate="txtPinCode" SetFocusOnError="True" ValidationExpression="\d{6}" Display="Dynamic" ></asp:RegularExpressionValidator>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr><%--id="trProductN" runat="server"--%>
                                                         <td>
                                                            Product 
                                                         </td>
                                                         <td colspan="3">
                                                           <asp:Label ID="lblpname" runat="server" ></asp:Label>
                                                            </td>
                                                        </tr>
                                                         <tr id="tr1" runat="server">
                                                         <td>
                                                            Product Line
                                                         </td>
                                                         <td colspan="3">
                                                           <asp:DropDownList ID="DDlPL" runat="server" CssClass="simpletxt1" Width="350px" >
                                                                <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                           </asp:DropDownList>
                                                        </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td colspan="3">
                                                               
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td colspan="4" height="24">
                                                                <b>Contact Information:</b>
                                                            </td>
                                                        </tr>
                                                        <tr style="display:none">
                                                            <td>
                                                                Mode Of Receipt
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:DropDownList ID="ddlModeOfRec" Enabled="false" runat="server" CssClass="simpletxt1">
                                                                </asp:DropDownList>
                                                            </td>
                                                            
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                Contact No.<font color='red'>*</font>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtContactNo" MaxLength="11" runat="server" CssClass="txtboxtxt"></asp:TextBox>
                                                                <div>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" SetFocusOnError="true" ControlToValidate="txtContactNo"
                                                                    runat="server" ErrorMessage="Contact Number is required." Display="Dynamic" ></asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Valid Contact Number is required."
                                                                    ControlToValidate="txtContactNo" SetFocusOnError="True" ValidationExpression="\d{10,11}" Display="Dynamic" ></asp:RegularExpressionValidator>
                                                                </div>    
                                                            </td>
                                                        </tr>
                                                        <tr id="trAlternate" runat="server">
                                                            <td>
                                                                Alternate Contact No.
                                                            </td>
                                                            <td >
                                                                <asp:TextBox runat="server" MaxLength="11" ID="txtAltConatctNo" CssClass="txtboxtxt"></asp:TextBox>
                                                                <div>
                                                                   <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Valid Alternate Contact Number is required."
                                                                    ControlToValidate="txtAltConatctNo" SetFocusOnError="True" ValidationExpression="\d{10,11}"  Display="Dynamic"></asp:RegularExpressionValidator>
                                                                 </div>
                                                            </td>
                                                            <td >
                                                                Extension
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtExtension" MaxLength="5" runat="server" CssClass="txtboxtxt"></asp:TextBox>
                                                                <div>
                                                                   <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="Valid Extension is required."
                                                                    Operator="DataTypeCheck" ControlToValidate="txtExtension" Type="Integer" Display="Dynamic" ></asp:CompareValidator>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                E-Mail<font color='red'>*</font>
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtEmail" MaxLength="60" runat="server" CssClass="txtboxtxt" Width="213px"></asp:TextBox>
                                                                 <div>
                                                                     <asp:RequiredFieldValidator ID="RFVEmail" runat="server" SetFocusOnError="true" ErrorMessage="Email is required."
                                                                    ControlToValidate="txtEmail" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Valid Email is required."
                                                                    ControlToValidate="txtEmail" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                                                                 </div>
                                                            </td>
                                                        </tr>
                                                        <tr id="trAppointment" runat="server">
                                                            <td>
                                                                Appointment Req.
                                                            </td>
                                                            <td colspan="3">
                                                                <asp:CheckBox ID="chkAppointment" runat="server" />
                                                            </td>
                                                            
                                                        </tr>
                                                        <tr id="trFeedBack" runat="server" >
                                                            <td>
                                                                Details</td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="txtFeedbackDetails" runat="server" CssClass="txtboxtxt" 
                                                                    Height="40px" MaxLength="500" TextMode="MultiLine" Width="380px"></asp:TextBox>
                                                                    <div>
                                                                      <asp:RequiredFieldValidator ID="RFVFeedback" runat="server" 
                                                                       ControlToValidate="txtFeedbackDetails" SetFocusOnError="true">*</asp:RequiredFieldValidator>
                                                                    </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                        <td colspan="4">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnFileUpload" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                            <table id="tblupfile" runat="server" width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                        <tr height="20">
                                                                            <td style="width:23%">
                                                                                Upload File
                                                                            </td>
                                                                            <td style="width:65%; padding-left:10px" >
                                                                                <input type="file" class="btn" id="flUpload" runat="server" onkeydown="if(event.keyCode==9){return true;}else{return false;}" />&nbsp;<asp:Button
                                                                                    ID="btnFileUpload" runat="server" CssClass="btn" CausesValidation="false" Text="Upload"
                                                                                    OnClick="btnFileUpload_Click" />
                                                                            </td>
                                                                            <td  style="width:12%">
                                                                                <asp:UpdateProgress AssociatedUpdatePanelID="UpdatePanel1" ID="UpdateProgress2" runat="server">
                                                                                    <ProgressTemplate>
                                                                                        <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" /></ProgressTemplate>
                                                                                </asp:UpdateProgress>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3">
                                                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <asp:GridView ID="gvFiles" runat="server" RowStyle-CssClass="gridbgcolor" AlternatingRowStyle-CssClass="fieldName"
                                                                                            HeaderStyle-CssClass="fieldNamewithbgcolor" AutoGenerateColumns="False" BorderStyle="None"
                                                                                            GridLines="none" Width="100%" OnPageIndexChanging="gvFiles_PageIndexChanging"
                                                                                            OnRowDeleting="gvFiles_RowDeleting">
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="File Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="25px">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("FileName") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" HeaderText="Action"
                                                                                                    HeaderStyle-HorizontalAlign="Left" />
                                                                                            </Columns>
                                                                                            <AlternatingRowStyle BorderStyle="None" />
                                                                                        </asp:GridView>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                        <td colspan="3"></td>
                                                                        </tr>
                                                                    </table>
                                                       <br />
                                                            <table id="tblcomplaint" runat="server" width="100%" border="0" cellpadding="2" cellspacing="0">
                                                                        <tr>
                                                                            <td colspan="4">
                                                                                <b>Product Complaint / Feedback Information:</b> 
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                        <td colspan="4"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td >
                                                                                Product Division<font color='red'>*</font>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:DropDownList Width="175px" ID="ddlProductDiv" AutoPostBack="true" runat="server"
                                                                                    CssClass="simpletxt1" OnSelectedIndexChanged="ddlProductDiv_SelectedIndexChanged">
                                                                                </asp:DropDownList>
                                                                                <div>
                                                                                   <asp:CompareValidator SetFocusOnError="true" ID="compValProdDiv" runat="server" ControlToValidate="ddlProductDiv"
                                                                                    ErrorMessage="Product Division is required." Operator="NotEqual" ValueToCompare="0" Display="Dynamic"></asp:CompareValidator>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trFrames" runat="server">
                                                                            <td >
                                                                                Frames<font color='red'>*</font>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:TextBox ID="txtFrames" runat="server" CssClass="txtboxtxt"  MaxLength="3" Text="" Width="50px"></asp:TextBox>
                                                                                <div>
                                                                                 <asp:RequiredFieldValidator ID="reqValFrames" SetFocusOnError="true" runat="server" ControlToValidate="txtFrames"
                                                                                        ErrorMessage="Frames is required." Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                 <asp:RegularExpressionValidator ID="CompareValidator4" runat="server" SetFocusOnError="true" ErrorMessage="Incorrect Format."
                                                                                      ControlToValidate="txtFrames" ValidationExpression="\d{1,3}" Display="Dynamic"></asp:RegularExpressionValidator> 
                                                                                </div>
                                                                                      </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td >
                                                                                Product Line
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:DropDownList ID="ddlProductLine" runat="server" CssClass="simpletxt1" style="width:65%;"  >
                                                                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td >
                                                                                Quantity<font color='red'>*</font>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="txtboxtxt" MaxLength="2" Text="1"
                                                                                    Width="50px"></asp:TextBox>
                                                                                   <div>
                                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtQuantity" ErrorMessage="Quantity is required."
                                                                                         SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                        <asp:CompareValidator ID="CompareValidator6" runat="server" ErrorMessage="Valid Quantity is required."
                                                                                            Operator="DataTypeCheck" ControlToValidate="txtQuantity" Type="Integer" SetFocusOnError="true" Display="Dynamic"></asp:CompareValidator>
                                                                                   </div> 
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" > Details<font color='red'>*</font>
                                                                            </td>
                                                                            <td colspan="3">
                                                                                <asp:TextBox ID="txtComplaintDetails" runat="server" CssClass="txtboxtxt" Width="380px"
                                                                                    TextMode="MultiLine" MaxLength="500" Height="72px"></asp:TextBox>
                                                                                 <div>
                                                                                   <asp:RequiredFieldValidator SetFocusOnError="true" ID="reqValComplaint" runat="server"
                                                                                    ControlToValidate="txtComplaintDetails" ErrorMessage="Complaint Details is required." Display="Dynamic"></asp:RequiredFieldValidator>
                                                                                 </div>   
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width:24%">
                                                                                Invoice No.
                                                                            </td>
                                                                            <td style="width:26%">
                                                                                <asp:TextBox runat="server" ID="txtInvoiceNum" CssClass="txtboxtxt" MaxLength="50"
                                                                                    ValidationGroup="Report" />
                                                                            </td>
                                                                            <td style="width:15%">
                                                                                Purchase Date <div>(dd/mm/yyyy)</div>
                                                                            </td>
                                                                            <td style="width:35%">
                                                                                <asp:TextBox MaxLength="10" runat="server" ID="txtxPurchaseDate"
                                                                                    CssClass="txtboxtxt" />
                                                                                    <div>
                                                                                       <%-- <asp:RequiredFieldValidator ID="CompareValidator7" runat="server"
                                                                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtxPurchaseDate" 
                                                                                        ErrorMessage="Valid Purchase date is required." Display="Dynamic"></asp:CompareValidator>--%>
                                                                                    </div>
                                                                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                                                                                    TargetControlID="txtxPurchaseDate" Format="dd/MM/yyyy">
                                                                                </cc1:CalendarExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td >Purchased from</td>
                                                                            <td colspan="3">
                                                                                <asp:TextBox ID="txtDealerName" runat="server" CssClass="txtboxtxt" 
                                                                                    MaxLength="50" ValidationGroup="Report" />
                                                                            </td>
                                                                            
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4" align="left"></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4" align="center">
                                                                                <!-- Product Division details Start-->
                                                                                <table width="98%" border="0" cellpadding="0" cellspacing="0">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <!-- Action Listing -->
                                                                                            <asp:GridView PageSize="10" DataKeyNames="SnoDisp" RowStyle-CssClass="gridbgcolor"
                                                                                                AlternatingRowStyle-CssClass="fieldName" HeaderStyle-CssClass="fieldNamewithbgcolor"
                                                                                                GridGroups="both" AllowPaging="True" AutoGenerateColumns="False" ID="gvComm"
                                                                                                runat="server" OnPageIndexChanging="gvComm_PageIndexChanging" Width="98%" Visible="true"
                                                                                                OnRowDeleting="gvComm_RowDeleting" >
                                                                                                <Columns>
                                                                                                    <asp:BoundField DataField="SnoDisp" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left"
                                                                                                        HeaderStyle-HorizontalAlign="Left" HeaderText="SNo"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="ProductDivision" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left"
                                                                                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Product Division"></asp:BoundField>
                                                                                                    <asp:BoundField DataField="ProductLine" HeaderStyle-Width="150px" ItemStyle-Wrap="true"
                                                                                                        ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Product Line">
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="QuantityDisp" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left"
                                                                                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Quantity">
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="InvoiceNumDisp" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left"
                                                                                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Invoice Number">
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="PurchasedFromDisp" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                                                                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Purchased From">
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:TemplateField HeaderText="Edit/Delete">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton ID="lnkBtnEdit" CommandArgument='<%#Eval("SnoDisp")%>' CommandName="Change"
                                                                                                                CausesValidation="false" runat="server">Edit</asp:LinkButton>
                                                                                                            /<asp:LinkButton ID="lnkBtnDelete" CausesValidation="false" CommandName="Delete"
                                                                                                                runat="server">Delete</asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                            <!-- End Action Listing -->
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                                <!-- Product Division End -->
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                               
                                                            <table width="100%" border="0" cellpadding="1" cellspacing="0">
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:Button ID="btnSubmit" runat="server" CssClass="btn" OnClick="btnSubmit_Click"
                                                                                    Text="Submit" Style="width: 200px" />
                                                                               <asp:Button ID="btnSubmitFeedBck" runat="server" CssClass="btn" 
                                                                                    Text="Submit" Style="width:150px" Visible="false" 
                                                                                    onclick="btnSubmitFeedBck_Click" />
                                                                                <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="btn"
                                                                                    OnClick="btnCancel_Click" OnClientClick="return funConfirm();" Text="Cancel" />
                                                                          </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                    
                                                                    <table id="tableResult" runat="server" width="100%" border="0" cellpadding="1" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%" border="0" cellpadding="1" cellspacing="0">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                        <div>
                                                                                               <asp:Label ID="LblMsgg" runat="server" CssClass="MsgTDCount"></asp:Label>
                                                                                         </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:GridView PageSize="15" DataKeyNames="Sno" RowStyle-CssClass="gridbgcolor" AlternatingRowStyle-CssClass="fieldName"
                                                                                                HeaderStyle-CssClass="fieldNamewithbgcolor" GridGroups="both" 
                                                                                                AutoGenerateColumns="False" ID="gvFinal" runat="server" Width="60%" 
                                                                                                Visible="true">
                                                                                                <RowStyle CssClass="gridbgcolor" />
                                                                                                <Columns>
                                                                                                    <asp:BoundField DataField="Sno" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left"
                                                                                                        HeaderStyle-HorizontalAlign="Left" HeaderText="SNo">
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:BoundField>
                                                                                                    <asp:TemplateField HeaderStyle-Width="160px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                                                        HeaderText="Product Division">
                                                                                                        <ItemTemplate>
                                                                                                            <%#Eval("ProductDivision")%>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="160px" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderStyle-Width="280px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                                                        HeaderText="Complaint Ref No">
                                                                                                        <ItemTemplate>
                                                                                                            <%#Eval("ComplaintRefNo")%>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" Width="280px" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                                <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                                                                                <AlternatingRowStyle CssClass="fieldName" />
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            &nbsp;</td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                              
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                </td>
                          
                                                        </tr>
                                                     </table> 
                                                  </asp:Panel>
                                                     </td>
                                                     </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                    
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    
                    </table>
            </td>
         </tr>
    </table>
    </form>
</body>
</html>
