﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OutBoundRpt.ascx.cs" Inherits="UC_OutBoundRpt" %>
<%@ Register assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
          
        </Triggers>
        <ContentTemplate>
            <table width="100%" border="0">
                <tr>
                    <td class="headingred" style="width: 40%">
                        OutBound Feedback Report</td>
                    <td align="<%=ConfigurationManager.AppSettings["AjaxLoadingAlign"]%>" style="padding-right: 10px;">
                        <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                            <ProgressTemplate>
                                <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Region:<span style="color:Red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRegion" Width="175" AutoPostBack="true" runat="server" CssClass="simpletxt1"
                            OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvRegion" runat="server" 
                            ControlToValidate="ddlRegion" Display="Dynamic" InitialValue="0" 
                            ValidationGroup="a">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Branch:<span style="color:Red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" Width="175" runat="server" CssClass="simpletxt1"  >
                            <asp:ListItem Value="0">Select</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvBranch" runat="server" 
                            ControlToValidate="ddlBranch" Display="Dynamic" InitialValue="0" 
                            ValidationGroup="a">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        Product Division:<span style="color:Red">*</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProductDivision" Width="175" runat="server" CssClass="simpletxt1">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="ddlProductDivision" Display="Dynamic" InitialValue="0" 
                            ValidationGroup="a">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                    <tr>
                    <td align="right">
                    Year:<span style="color:Red">*</span>
                    </td>
                    <td>
                    <asp:DropDownList ID="DDlYear" Width="175" runat="server" CssClass="simpletxt1">
                    </asp:DropDownList>
                    </td>
                    <td>
                    &nbsp;</td>
                    </tr>
                <tr>
                <td align="right">
                     Month:<span style="color:Red">*</span>
                    </td>
                    <td>
                      <asp:DropDownList ID="DDLMonth" Width="175" runat="server" CssClass="simpletxt1">
                      </asp:DropDownList>
                         <asp:RequiredFieldValidator ID="rfvMonth1" runat="server" 
                            ControlToValidate="DDLMonth" Display="Dynamic" InitialValue="0" 
                            ValidationGroup="a">*</asp:RequiredFieldValidator>
                      </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn" Text="Search" Width="100"
                            OnClick="btnSearch_Click" ValidationGroup="a" />
            <%--            <asp:Button ID="btnExport" runat="server" Visible="false" CssClass="btn" Text="Export To Execl"
                            Width="100" OnClick="btnExport_Click" />--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
<asp:Chart ID="Chart1" runat="server" Palette="None" PaletteCustomColors="255, 128, 0" >
</asp:Chart>
<asp:Chart ID="Chart2" runat="server" Palette="None" PaletteCustomColors="CornflowerBlue" >
</asp:Chart>
<asp:Chart ID="Chart3" runat="server" Palette="None" PaletteCustomColors="HotPink" >
</asp:Chart>
<asp:Chart ID="Chart4" runat="server" Palette="None" PaletteCustomColors="128, 255, 128"  >
</asp:Chart>
<asp:Chart ID="Chart5" runat="server" Palette="None" PaletteCustomColors="255, 192, 255"  >
</asp:Chart>

                        
                    </td>
                </tr>
              
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>



