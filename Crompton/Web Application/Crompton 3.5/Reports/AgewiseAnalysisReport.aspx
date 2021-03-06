﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CICPage.master" AutoEventWireup="true"
    CodeFile="AgewiseAnalysisReport.aspx.cs" Inherits="Reports_AgewiseAnalysisReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainConHolder" runat="Server">
    <table width="100%">
        <tr>
            <td class="headingred">
                Agewise Analysis
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="padding: 10px" align="center" colspan="2" class="bgcolorcomm">
                <table width="98%" border="0">
                    <tr>
                        <td>
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                                Width="680px" AsyncRendering="False" ProcessingMode="Remote">
                                <ServerReport ReportPath="/Report Project1/Agewise Analysis" DisplayName="Agewise Analysis"
                                    ReportServerUrl="http://192.168.10.61/cicreport" />
                            </rsweb:ReportViewer>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
