﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CICPage.master" AutoEventWireup="true"
    CodeFile="CityMaster.aspx.cs" Inherits="Admin_CityMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainConHolder" runat="Server">
    <asp:UpdatePanel ID="updatepnl" runat="server">
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="headingred">
                        City Master
                    </td>
                    <td align="<%=ConfigurationManager.AppSettings["AjaxLoadingAlign"]%>" style="padding-right: 10px;">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                            <ProgressTemplate>
                                <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" />
                            </ProgressTemplate>
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
                    <td style="padding: 10px" colspan="2">
                        <table border="0" width="100%" align="center">
                            <tr>
                                <td align="left" class="MsgTDCount">
                                    Total Number of Records :
                                    <asp:Label ID="lblRowCount" CssClass="MsgTotalCount" runat="server"></asp:Label>
                                </td>
                                <td align="right">
                                    Search For
                                    <asp:DropDownList ID="ddlSearch" runat="server" Width="130px" CssClass="simpletxt1">
                                        <asp:ListItem Text="Code" Value="City_Code"></asp:ListItem>
                                        <asp:ListItem Text="Description" Value="City_Desc"></asp:ListItem>
                                        <asp:ListItem Text="Branch Name" Value="Branch_Name"></asp:ListItem>
                                        <asp:ListItem Text="State" Value="state_desc"></asp:ListItem>
                                        <asp:ListItem Text="Country" Value="country_desc"></asp:ListItem>
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
                                    <!-- City Listing   -->
                                    <asp:GridView PageSize="10" RowStyle-CssClass="gridbgcolor" AlternatingRowStyle-CssClass="fieldName"
                                        HeaderStyle-CssClass="fieldNamewithbgcolor" GridLines="both" AllowPaging="True"
                                        PagerStyle-HorizontalAlign="Center" AllowSorting="True" DataKeyNames="city_sno"
                                        AutoGenerateColumns="False" ID="gvComm" runat="server" OnPageIndexChanging="gvComm_PageIndexChanging"
                                        Width="100%" OnSelectedIndexChanging="gvComm_SelectedIndexChanging" HorizontalAlign="Left"
                                        EnableSortingAndPagingCallbacks="True" OnSorting="gvComm_Sorting">
                                        <RowStyle CssClass="gridbgcolor" />
                                        <Columns>
                                            <asp:BoundField DataField="Sno" HeaderStyle-Width="40px" HeaderText="SNo" ItemStyle-HorizontalAlign="Center">
                                                <HeaderStyle Width="40px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="City_Code" SortExpression="City_Code" HeaderStyle-Width="60px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Code">
                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="City_Desc" SortExpression="City_Desc" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Description">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Branch_Name" SortExpression="Branch_Name" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Branch Name">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="State_Desc" SortExpression="State_Desc" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="State">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Country_Desc" SortExpression="Country_Desc" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Country">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Active_Flag" SortExpression="Active_Flag" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Status">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:CommandField ShowSelectButton="True" HeaderStyle-Width="60px" HeaderText="Edit"
                                                ItemStyle-HorizontalAlign="Center">
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
                                        <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                        <AlternatingRowStyle CssClass="fieldName" />
                                    </asp:GridView>
                                    <!-- End City Listing -->
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
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
                                                <asp:HiddenField ID="hdnCitySNo" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                City Code<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="txtboxtxt" ID="txtCityCode" runat="server" Width="170px" Text=""
                                                    MaxLength="10" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                        SetFocusOnError="true" ErrorMessage="City Code is required." ControlToValidate="txtCityCode"
                                                        ValidationGroup="editt" ToolTip="City Code is required."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Country <font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCountry" AutoPostBack="true" runat="server" CssClass="simpletxt1"
                                                    Width="175px" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCountry"
                                                    ErrorMessage="Country is required." ValidationGroup="editt" ToolTip="Country is required."
                                                    InitialValue="Select"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                State <font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="simpletxt1" Width="175px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlState"
                                                    ErrorMessage="State is required." ValidationGroup="editt" ToolTip="State is required."
                                                    InitialValue="Select"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                Branch Name<font color="red">*</font>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlBranchName" runat="server" CssClass="simpletxt1" Width="175px">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlBranchName"
                                                    ErrorMessage="Branch Name is required." InitialValue="0" ValidationGroup="editt"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                City Description<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="txtboxtxt" ID="txtCityDesc" runat="server" Width="170px" Text="" />&nbsp;<asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" ErrorMessage="City Description is required."
                                                    ControlToValidate="txtCityDesc" ToolTip="City Desc is required." ValidationGroup="editt"></asp:RequiredFieldValidator>
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
                                                            <asp:Button Text="Add" Width="70px" CssClass="btn" ID="imgBtnAdd" runat="server"
                                                                CausesValidation="True" ValidationGroup="editt" OnClick="imgBtnAdd_Click" />
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
    </asp:UpdatePanel>
</asp:Content>
