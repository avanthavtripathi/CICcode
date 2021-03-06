﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CICPage.master" AutoEventWireup="true" CodeFile="FeedBackQuestionMaster.aspx.cs" Inherits="Admin_FeedBackQuestionMaster"%>

<asp:Content ID="ContentFeedBackQues" ContentPlaceHolderID="MainConHolder" Runat="Server">

<asp:UpdatePanel ID="updatepnl" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td class="headingred">
                        Feedback Question Master
                    </td>
                    <td align="<%=ConfigurationManager.AppSettings["AjaxLoadingAlign"]%>" style="padding-right:10px;">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                            <ProgressTemplate>
                                <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
               <tr>
                    <td style="padding: 10px" align="center" colspan="2">
                        <table border="0" width="100%">
                               <tr>
                                <td align="right">Search For <asp:DropDownList ID="ddlSearch" runat="server" Width="130px" CssClass="simpletxt1">
                                <asp:ListItem Text="Question Code" Value="Fb_Ques_Code"></asp:ListItem>
                                <asp:ListItem Text="Question" Value="Fb_Question_Desc"></asp:ListItem>
                                
                                </asp:DropDownList>
                                
                                With <asp:TextBox ID="txtSearch" runat="server" CssClass="txtboxtxt" Width="100px" Text=""></asp:TextBox>
                                
                                  <asp:Button Text="Go" Width="25px" CssClass="btn" ID="imgBtnGo" runat="server"
                                    CausesValidation="False" ValidationGroup="editt" OnClick="imgBtnGo_Click"  />
                                </td>
                            </tr>
                        </table>
                        <table width="100%" border="0">
                            <tr>
                                <td>
                                    <!-- FeedBack Question Listing -->
                                    <asp:GridView RowStyle-CssClass="gridbgcolor" AlternatingRowStyle-CssClass="fieldName"
                                        HeaderStyle-CssClass="fieldNamewithbgcolor" GridGroups="both" AllowPaging="True"
                                        AllowSorting="True" DataKeyNames="Fb_Ques_SNo" AutoGenerateColumns="False" ID="gvComm"
                                        runat="server" OnPageIndexChanging="gvComm_PageIndexChanging" Width="100%" OnSelectedIndexChanging="gvComm_SelectedIndexChanging"
                                        HorizontalAlign="Left" onsorting="gvComm_Sorting">
                                        <RowStyle CssClass="gridbgcolor" />
                                        <Columns>
                                            <asp:BoundField DataField="Sno" HeaderStyle-Width="40px" HeaderText="SNo">
                                                <HeaderStyle Width="40px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Fb_Ques_Code" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-HorizontalAlign="Left" HeaderText="Ques Code" 
                                                SortExpression="Fb_Ques_Code">
                                                <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Fb_Question_Desc" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                HeaderText="Question" SortExpression="Fb_Question_Desc">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Active_Flag" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                HeaderText="Status" SortExpression="Active_Flag">
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>
                                            <asp:CommandField ShowSelectButton="True"  HeaderText="Edit">
                                               
                                            </asp:CommandField>
                                        </Columns>
                                        <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                        <AlternatingRowStyle CssClass="fieldName" />
                                    </asp:GridView>
                                    <!-- End Feed Back Ques Listing -->
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                    
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
                                                <asp:HiddenField ID="hdnFbQuesSNo" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                FeedBack Question Code<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="txtboxtxt" ID="txtFeedBackQuesCode" runat="server" MaxLength="10" Width="170px"
                                                    Text="" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                        SetFocusOnError="true" ErrorMessage="FeedBack Ques Code is required." ControlToValidate="txtFeedBackQuesCode"
                                                        ValidationGroup="editt" ToolTip="FeedBack Ques Code is required."></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="30%">
                                                FeedBack Question<font color='red'>*</font>
                                            </td>
                                            <td>
                                                <asp:TextBox CssClass="txtboxtxt" ID="txtFeedBackQuestion" runat="server" MaxLength="100" Width="170px"
                                                    Text="" /><asp:RequiredFieldValidator ID="reqvalDeptIname" runat="server" SetFocusOnError="true"
                                                       ToolTip="Feedback Question is required." ErrorMessage="FeedBack Question is required." ControlToValidate="txtFeedBackQuestion" ValidationGroup="editt"></asp:RequiredFieldValidator>
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
                                            <td height="25" align="left">&nbsp;
                                                
                                            </td>
                                            <td>
                                                <!-- For button portion update -->
                                                <table>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Button  Text="Add" Width="70px" CssClass="btn" ID="imgBtnAdd" runat="server" CausesValidation="True"
                                                                ValidationGroup="editt" OnClick="imgBtnAdd_Click" />
                                                            <asp:Button Text="Save" Width="70px" ID="imgBtnUpdate" CssClass="btn" runat="server" CausesValidation="True"
                                                                ValidationGroup="editt" OnClick="imgBtnUpdate_Click" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="imgBtnCancel"  Width="70px"  runat="server" CausesValidation="false" CssClass="btn"
                                                                Text="Cancel" OnClick="imgBtnCancel_Click" />
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

