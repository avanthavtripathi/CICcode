﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CICPage.master" AutoEventWireup="true"
    CodeFile="ServiceContractorMaster.aspx.cs" Inherits="Admin_ServiceContractor"
    Title="Service Contractor" %>

<asp:Content ID="ContentServiceContractor" ContentPlaceHolderID="MainConHolder" runat="Server">
    <asp:UpdatePanel ID="updatepnl" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="headingred">
                        Service Contractor Master
                    </td>
                    <td align="<%=ConfigurationManager.AppSettings["AjaxLoadingAlign"]%>" style="padding-right: 10px;">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                            <ProgressTemplate>
                                <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" /></ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right" style="padding-right: 10px">
                        <asp:RadioButtonList ID="rdoboth" RepeatDirection="Horizontal" RepeatColumns="3"
                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdoboth_SelectedIndexChanged">
                            <asp:ListItem Value="1" Text="Active" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="0" Text="In-Active"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Both"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 10px" align="center" colspan="2">
                        <table border="0" width="100%">
                            <tr>
                                <td align="left" class="MsgTDCount">
                                    Total Number of Records :
                                    <asp:Label ID="lblRowCount" CssClass="MsgTotalCount" runat="server"></asp:Label>
                                </td>
                                <td align="right">
                                    Search For
                                    <asp:DropDownList ID="ddlSearch" runat="server" Width="130px" CssClass="simpletxt1">
                                        <asp:ListItem Text="SC Code" Value="SC_Username"></asp:ListItem>
                                        <asp:ListItem Text="SC Name" Value="SC_Name"></asp:ListItem>
                                        <asp:ListItem Text="Contact Person" Value="Contact_Person"></asp:ListItem>
                                        <asp:ListItem Text="Address" Value="Address1"></asp:ListItem>
                                        <asp:ListItem Text="EmailID" Value="EmailID"></asp:ListItem>
                                        <asp:ListItem Text="Phone No" Value="PhoneNo"></asp:ListItem>
                                        <asp:ListItem Text="Fax No" Value="FaxNo"></asp:ListItem>
                                        <asp:ListItem Text="City" Value="city_desc"></asp:ListItem>
                                        <asp:ListItem Text="State" Value="state_desc"></asp:ListItem>
                                        <asp:ListItem Text="Weekly Off Day" Value="Weekly_Off_Day"></asp:ListItem>
                                    </asp:DropDownList>
                                    With
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="txtboxtxt" Width="100px" Text=""></asp:TextBox>
                                    <asp:Button Text="Go" Width="25px" CssClass="btn" ID="imgBtnGo" runat="server" CausesValidation="False"
                                        ValidationGroup="editt" OnClick="imgBtnGo_Click" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%" border="0">
                            <tr>
                                <td>
                                    <!-- Service Contractor Listing   -->
                                    <asp:GridView PageSize="5" RowStyle-CssClass="gridbgcolor" AlternatingRowStyle-CssClass="fieldName"
                                        HeaderStyle-CssClass="fieldNamewithbgcolor" GridLines="both" AllowPaging="True"
                                        AllowSorting="True" DataKeyNames="SC_SNo" AutoGenerateColumns="False" ID="gvServiceContractor"
                                        runat="server" OnPageIndexChanging="gvServiceContractor_PageIndexChanging" Width="100%"
                                        OnSelectedIndexChanging="gvServiceContractor_SelectedIndexChanging" HorizontalAlign="Left"
                                        EnableSortingAndPagingCallbacks="True" OnSorting="gvServiceContractor_Sorting">
                                        <Columns>
                                            <asp:BoundField DataField="Sno" HeaderStyle-Width="40px" HeaderText="SNo">
                                                <HeaderStyle Width="40px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SC_Code" SortExpression="SC_Code" HeaderStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="SC Code">
                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SC_Name" SortExpression="SC_Name" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="SC Name">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Contact_Person" SortExpression="Contact_Person" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Contact Person">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Address1" SortExpression="Address1" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Address">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EmailID" SortExpression="EmailID" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="EmailID">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="PhoneNo" SortExpression="PhoneNo" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Phone No">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MobileNo" SortExpression="MobileNo" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Mobile No">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="city_desc" SortExpression="city_desc" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="City">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="state_desc" SortExpression="state_desc" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="State">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Weekly_Off_Day" SortExpression="Weekly_Off_Day" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Weekly Off Day">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Active_Flag" SortExpression="Active_Flag" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Status">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:CommandField ShowSelectButton="True" HeaderStyle-Width="60px" HeaderText="Edit">
                                                <HeaderStyle Width="60px" />
                                            </asp:CommandField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center" style="padding-top: 50px; padding-bottom: 50px; padding-left: 60px;">
                                                        <img src="<%=ConfigurationManager.AppSettings["UserMessage"]%>" alt="" />
                                                        <b>No Record found</b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <!-- End Service Contractor Listing -->
                                </td>
                            </tr>
                            <tr>
                                <td>
                                  <asp:Button Text="Excel DownLoad" CssClass="btn" ID="BtnExportExcel" 
                                        runat="server" CausesValidation="False" onclick="BtnExportExcel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td width="100%" align="left" class="bgcolorcomm">
                                    <table width="98%" border="0">
                                        <tr>
                                            <td colspan="2" align="<%=ConfigurationManager.AppSettings["MandatoryTextAlign"]%>">
                                                <font color='red'>*</font>
                                                <%=ConfigurationManager.AppSettings["MandatoryText"]%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:HiddenField ID="hdnSCSNo" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Service contractor Code<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="txtboxtxt" ID="txtScCode" runat="server" Width="170px" Text=""
                                                    MaxLength="10" /><asp:RequiredFieldValidator ID="RFVScCode" runat="server" SetFocusOnError="true"
                                                        ErrorMessage="Service Contractor code is required." ToolTip="Service Contractor is required."
                                                        ControlToValidate="txtScCode" ValidationGroup="editt"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Service Contractor Name<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="txtboxtxt" ID="txtScName" runat="server" Width="170px" Text=""
                                                    MaxLength="50" /><asp:RequiredFieldValidator ID="RFVScName" runat="server" SetFocusOnError="true"
                                                        ErrorMessage="Service Contractor Name is required." ControlToValidate="txtScName"
                                                        ValidationGroup="editt" ToolTip="Service Contractor Name is required."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Contact Person <font color="red">*</font>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactPerson" runat="server" CssClass="txtboxtxt" MaxLength="50"
                                                    Text="" Width="170px" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true"
                                                    ErrorMessage="Contact Person is required." ControlToValidate="txtContactPerson"
                                                    ValidationGroup="editt"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Address 1</font><font color="red">*</font>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="txtboxtxt" ID="txtAddOne" runat="server" Width="170px" Text=""
                                                    MaxLength="100" TextMode="MultiLine" />
                                                <asp:RequiredFieldValidator ID="RFVAddOne" runat="server" SetFocusOnError="true"
                                                    Display="Dynamic" ErrorMessage="Address One is required." ControlToValidate="txtAddOne"
                                                    ValidationGroup="editt" ToolTip="Address One is required."></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ValidationGroup="editt" ID="CustomValidator1" runat="server"
                                                    ControlToValidate="txtAddOne" Display="dynamic" ErrorMessage="Please enter max 100 characters."
                                                    ClientValidationFunction="validateAddress1"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                </font> Address2
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="txtboxtxt" ID="txtAddTwo" runat="server" Width="170px" Text=""
                                                    MaxLength="100" ValidationGroup="editt" TextMode="MultiLine" />
                                                <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtAddTwo"
                                                    ValidationGroup="editt" Display="dynamic" ErrorMessage="Please enter max 100 characters."
                                                    ClientValidationFunction="validateAddress2"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Email ID
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="txtboxtxt" MaxLength="100" Text=""
                                                    Width="170px" />
                                                <asp:RegularExpressionValidator ID="RegEmail" runat="server" ControlToValidate="txtEmail"
                                                    Display="dynamic" ErrorMessage="E-mail is not valid." ValidationGroup="editt"
                                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Phone Number
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="txtboxtxt" MaxLength="11" Text=""
                                                    Width="170px" onkeypress="javascript:return checkNumberOnly(event);" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Mobile Number
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMobileNo" runat="server" CssClass="txtboxtxt" MaxLength="11"
                                                    Text="" Width="170px" onkeypress="javascript:return checkNumberOnly(event);" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Preference
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPrefence" runat="server" CssClass="txtboxtxt" MaxLength="1" Text="0"
                                                    Width="170px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Special Remarks
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSpecialRemarks" runat="server" CssClass="txtboxtxt" MaxLength="50"
                                                    Text="" Width="170px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Fax No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFaxNo" runat="server" onkeypress="javascript:return checkNumberOnly(event);"
                                                    CssClass="txtboxtxt" MaxLength="20" Text="" Width="170px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Region<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRegion" AutoPostBack="true" runat="server" CssClass="simpletxt1"
                                                    Width="175px" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CVRegion" runat="server" ControlToValidate="ddlRegion"
                                                    ErrorMessage="Region is required." Operator="NotEqual" SetFocusOnError="True"
                                                    ValueToCompare="Select" ValidationGroup="editt"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Branch<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="simpletxt1" Width="175px">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CVBranch" runat="server" ControlToValidate="ddlBranch"
                                                    ErrorMessage="Branch is required." Operator="NotEqual" SetFocusOnError="True"
                                                    ValueToCompare="Select" ValidationGroup="editt"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                State<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlState" AutoPostBack="true" runat="server" CssClass="simpletxt1"
                                                    Width="175px" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CVState" runat="server" ControlToValidate="ddlState" ErrorMessage="State code is required."
                                                    Operator="NotEqual" SetFocusOnError="True" ValueToCompare="Select" ValidationGroup="editt"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                City<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCity" runat="server" CssClass="simpletxt1" Width="175px">
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CVCity" runat="server" ControlToValidate="ddlCity" ErrorMessage="City code is required."
                                                    Operator="NotEqual" SetFocusOnError="True" ValueToCompare="Select" ValidationGroup="editt"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Weekly Off Day<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlWeeklyOffDay" runat="server" CssClass="simpletxt1" Width="175px">
                                                    <asp:ListItem Selected="True">Select</asp:ListItem>
                                                    <asp:ListItem>SUN</asp:ListItem>
                                                    <asp:ListItem>MON</asp:ListItem>
                                                    <asp:ListItem>TUE</asp:ListItem>
                                                    <asp:ListItem>WED</asp:ListItem>
                                                    <asp:ListItem>THU</asp:ListItem>
                                                    <asp:ListItem>FRI</asp:ListItem>
                                                    <asp:ListItem>SAT</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="CVWeeklyOffDay" runat="server" ControlToValidate="ddlWeeklyOffDay"
                                                    ErrorMessage="Weekly Off Day is required." Operator="NotEqual" SetFocusOnError="True"
                                                    ValueToCompare="Select" ValidationGroup="editt"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Status
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdoStatus" RepeatDirection="Horizontal" RepeatColumns="2"
                                                    runat="server">
                                                    <asp:ListItem Value="1" Text="Active" Selected="True">
                                                    </asp:ListItem>
                                                    <asp:ListItem Value="0" Text="In-Active">
                                                    </asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="25" align="left">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <!-- For button portion update -->
                                                <table>
                                                    <tr>
                                                        <td align="right">
                                                            <%-- <asp:Button Text="Add" Width="70px" CssClass="btn" ID="imgBtnAdd" runat="server"
                                                                CausesValidation="True" ValidationGroup="editt" OnClick="imgBtnAdd_Click" />--%>
                                                            <asp:Button Text="Save" Width="70px" ID="imgBtnUpdate" CssClass="btn" runat="server"
                                                                CausesValidation="True" ValidationGroup="editt" OnClick="imgBtnUpdate_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="imgBtnCancel" Width="70px" runat="server" CausesValidation="false"
                                                                CssClass="btn" Text="Cancel" OnClick="imgBtnCancel_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <!-- For button portion update end -->
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
        <asp:PostBackTrigger ControlID ="BtnExportExcel" />
        </Triggers>
    </asp:UpdatePanel>

    <script language="javascript" type="text/javascript">
        
        function validateAddress1(oSrc, args)
        {
            
            var x = (document.getElementById('ctl00_MainConHolder_txtAddOne').value); 
            if(x.length > 100)
            
             {
                args.IsValid = false
             }
             else
             {
           
                args.IsValid = true
             }
           
             
        }
        
        function validateAddress2(oSrc, args)
        {
            
            var x = (document.getElementById('ctl00_MainConHolder_txtAddTwo').value); 
            if(x.length > 100)
            
             {
                args.IsValid = false
             }
             else
             {
                args.IsValid = true
             }
        }
        
    </script>

</asp:Content>
