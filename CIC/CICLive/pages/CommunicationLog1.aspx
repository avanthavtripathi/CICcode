﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommunicationLog1.aspx.cs"
    Inherits="pages_CommunicationLog1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<head runat="server">
    <title>Crompton Greaves :: Customer Interaction Center</title>
    <link href="../css/style.css" rel="stylesheet" type="text/css" />
    <link href="../css/global.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatepnl" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr bgcolor="white">
                        <td class="headingred">
                            Communication Details
                        </td>
                        <td>
                            <a href="javascript:void(0)" class="links" onclick="window.close();">Close</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 10px" align="center" colspan="2" class="bgcolorcomm">
                            <table width="99%" border="0">
                                <tr>
                                    <td width="18%" align="left">
                                        Complaint Ref No.
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblComplaintRefNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id="dvGrid" style="width: 860px; overflow: auto;">
                                            <!-- Communication Details for Complaint Ref No Listing   -->
                                            <asp:GridView PageSize="15" RowStyle-CssClass="gridbgcolor" AlternatingRowStyle-CssClass="fieldName"
                                                HeaderStyle-CssClass="fieldNamewithbgcolor" GridLines="both" AllowPaging="True"
                                                AutoGenerateColumns="False" ID="gvCommunication" runat="server" OnPageIndexChanging="gvCommunication_PageIndexChanging"
                                                Width="98%" HorizontalAlign="Left">
                                                <RowStyle CssClass="gridbgcolor" />
                                                <Columns>
                                                    <asp:BoundField DataField="Sno" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderText="SNo">
                                                        <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="CommentDate" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Date">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Name" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" HeaderText="User">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="RoleName" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left"
                                                        HeaderStyle-HorizontalAlign="Left" HeaderText="Role">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Remarks" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                        HeaderText="Remarks">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <img src="<%=ConfigurationManager.AppSettings["UserMessage"]%>" alt="" /><b>No Records
                                                        found.</b>
                                                </EmptyDataTemplate>
                                                <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                                <AlternatingRowStyle CssClass="fieldName" />
                                            </asp:GridView>
                                            <!-- End Communication Details for Complaint Ref No Listing -->
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <div id="dvComments" runat="server">
                        <tr>
                            <td style="padding: 10px" align="left" colspan="2">
                                <b>Comment</b>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 10px" align="center" colspan="2">
                                <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="6" Columns="50"
                                    ValidationGroup="Comment"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtComment"
                                    ErrorMessage="Required." ValidationGroup="Comment"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding: 10px" align="center" colspan="2" valign="top">
                            Comment Date<font color='red'>*</font>
                                <asp:TextBox ID="txtComplaintLoggedDate" MaxLength="10" runat="server" CssClass="txtboxtxt"
                                    Width="100px"></asp:TextBox>
                                &nbsp;(mm/dd/yyyy)
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtComplaintLoggedDate"
                                    ErrorMessage="Required." ValidationGroup="Comment"></asp:RequiredFieldValidator>
                                <cc1:calendarextender id="CalendarExtender6" runat="server" targetcontrolid="txtComplaintLoggedDate">
                                                            </cc1:calendarextender>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:400px" align="left" colspan="2">
                                <asp:Button ID="btnSubmitComment" runat="server" CssClass="btn" Text="Submit" OnClick="btnSubmitComment_Click"
                                    ValidationGroup="Comment" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </div>
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
