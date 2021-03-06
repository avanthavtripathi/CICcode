<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SIMS/MasterPages/SIMSCICPage.master"
    CodeFile="DefectiveSpareChallanGeneration.aspx.cs" Inherits="SIMS_Pages_DefectiveSpareChallanGeneration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainConHolder" runat="Server">
<script type="text/javascript">
    //Check Checkbox 
    function CheckForEmptyTextbox() {
        var challan = document.getElementById("<%=txtchallan.ClientID%>").value;

        if (challan == "") {
            alert("Challan No is Required");
            return false;
        }
        else {
            return ClientCheck();

        }
    }
    //End

    //added for reprint 4 nov bp
    function BtnRePrint_Click() {
        var challan = document.getElementById("<%=ddlChallans.ClientID%>");
        var challanno = challan.options[challan.selectedIndex].text;
        var pdiv = challan.options[challan.selectedIndex].value;

        if (pdiv == 0) {
            alert("Challan No is Required");
            return false;
        }
        else {
            window.open("PrintChallanScreen.aspx?Rep=true&ChNo=" + challanno + "&PDiv=" + pdiv);
            return false;

        }
    }
    //End

    function ClientCheck() {
        var flag = false;
        for (var i = 0; i < document.forms[0].length; i++) {
            if (document.forms[0].elements[i].id.indexOf('chk') != -1) {
                if (document.forms[0].elements[i].checked) {
                    flag = true
                }
            }
        }
        if (flag == false) {
            alert('Please select at least one Complaint.')
            return false

        }
        else {
            return true
        }
    }
    
    // Added By Ashok for check uncheck of grid checkbox on 7.11.2014
    function CheckUncheckAll(evt)
    {
        var grd=document.getElementById("<%=gvChallanDetail.ClientID %>");
        var input=grd.getElementsByTagName('input');
        
        if(evt.id.indexOf('chAllChall')>=0)
        {
            for (var i = 0; i < input.length; i++) {
                if (input[i].type=="checkbox" && input[i].id!=evt.id) {
                        input[i].checked = evt.checked;
                }
            }
        }
        else if(evt.id.indexOf('chk')>=0)
        {
            var count=0;
            for (var i = 0; i < input.length; i++) {
                if (input[i].type=="checkbox" && input[i].id.indexOf('chAllChall')<0 && input[i].checked) {
                        count=count+1;
                }
            }
            var chkHd=input[0].checked=count==input.length-1;
        }
    }
 
</script>

    <asp:UpdatePanel ID="updatepnl" runat="server">
        <ContentTemplate>
            <table width="100%" cellpadding="2" cellspacing="0">
               
                <tr>
                    <td class="headingred">
                        Defective Spare Chalan Generation
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;padding-right: 10px;">
                      <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="updatepnl"  >
                            <ProgressTemplate>
                                <img src="<%=ConfigurationManager.AppSettings["SIMSAjaxLoadingImageName"]%>" alt="" /></ProgressTemplate>
                      </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 10px" align="center">
                        <table width="100%" border="0">
                            <tr>
                                <td width="100%" align="left" class="bgcolorcomm">
                                    <table border="0" style="width: 100%">
                                        <tr>
                                            <%--   <td align="right" colspan="2">
                                                <font style="color: #FF0000">*</font>
                                                Mandatory fields
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td style="width: 9%">
                                                ASC:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblASCName" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnASC_Id" runat="server" />
                                            </td>
                                            <td>
                        Generated Challans:
                    </td>
                                            <td>
											<%--4 nov added bp--%>
                                              <asp:DropDownList ID="ddlChallans" runat="server" CssClass="simpletxt1" ValidationGroup="RePrint"
                            Width="206px">
                        </asp:DropDownList>    
                                                </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 9%">
                                                Division:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDivision" runat="server" CssClass="simpletxt1" Width="185px"
                                                    OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
											<%--add reprint button 4 nov bp--%>
                                                          <asp:Button ID="BtnRePrint" runat="server" CssClass="btn" 
                                                         OnClientClick="javascript:return BtnRePrint_Click();"                              Text="Re-Print" Width="78px" ValidationGroup="RePrint" /><br />
                            </td>
                                        </tr>
                                        <tr id="tblchallan" runat="server" visible="false">
                                            <td style="width: 9%">
                                                Challan No<font style="color: #FF0000">*</font>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtchallan" runat="server" CssClass="simpletxt1" Width="180px"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtchallan"
                                                    runat="server" ErrorMessage="Challan Number Required"></asp:RequiredFieldValidator>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td visible="false" style="width: 9%">
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlwarranty" runat="server" CssClass="simpletxt1" Width="185px"
                                                    AutoPostBack="true" Visible="false" OnSelectedIndexChanged="ddlwarranty_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Selected="True">Select</asp:ListItem>
                                                    <asp:ListItem Value="Y">Under Warranty</asp:ListItem>
                                                    <asp:ListItem Value="N">Out Of Warranty</asp:ListItem>
                                                </asp:DropDownList>
                                               <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlwarranty"
                                                    runat="server" ErrorMessage="Required" InitialValue="0"></asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:GridView ID="gvChallanDetail" runat="server" AllowPaging="True" AllowSorting="True"
                                                    AlternatingRowStyle-CssClass="fieldName" AutoGenerateColumns="False" GridGroups="both"
                                                    HeaderStyle-CssClass="fieldNamewithbgcolor" HorizontalAlign="Left" PageSize="50"
                                                    RowStyle-CssClass="gridbgcolor" Width="100%" EnableSortingAndPagingCallbacks="True"
                                                    OnPageIndexChanging="gvChallanDetail_PageIndexChanging" 
                                                    >
                                                    <RowStyle CssClass="gridbgcolor" />
                                                    <Columns>
                                                    <asp:TemplateField HeaderText="Sno"  HeaderStyle-HorizontalAlign="Left"
                                                            ItemStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Product_Division" HeaderText="Product Division" ItemStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Complaint No.">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkcomplaint" CausesValidation="false" 
                                                                    CommandArgument='<%#Eval("BASELINEID")%>'  CommandName="stage" runat="server" 
                                                                    Text='<%#Eval("complaint_no") %>' onclick="lnkcomplaint_Click"></asp:LinkButton>
                                                                
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbldefid" runat="server" Text='<%#Eval("DefId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Spare" ItemStyle-Width="210px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblspare" runat="server" Text='<%#Eval("spare") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="210px" />
                                                        </asp:TemplateField>
                                                        <%-- <asp:TemplateField HeaderText="Spare" ItemStyle-Width="210px" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblspareId" runat="server" Text='<%#Eval("spare_id") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:BoundField DataField="UOM" HeaderText="UOM" ItemStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <%--  <asp:BoundField DataField="SalesOrder" HeaderText="Sales Order No." ItemStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>--%>
                                                        <asp:BoundField DataField="qty" HeaderText="Returnable Defective Qty" ItemStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="qty" HeaderText="Qty Being Returned" ItemStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <%--<asp:TemplateField HeaderText="Claim No.">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClaimNo" runat="server" Text='<%#Eval("Claim_No") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Approved Flag" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                Y
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <%-- <asp:BoundField DataField="claim_date" HeaderText="Date Of Claim Approval" ItemStyle-HorizontalAlign="Center">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>--%>
                                                        <asp:TemplateField>
                                                        <HeaderTemplate>
                                                        <asp:CheckBox ID="chAllChall" runat="server"  Text="All/None" onclick="CheckUncheckAll(this);"/>
                                                        </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chk" runat="server" onclick="CheckUncheckAll(this);"/>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td align="center" style="padding-top: 50px; padding-bottom: 50px; padding-left: 60px;">
                                                                    <img src="<%=ConfigurationManager.AppSettings["simsUserMessage"]%>" alt="" />
                                                                    <b>No Record found</b>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                                    <AlternatingRowStyle CssClass="fieldName" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                          
                                        <tr id="trButton" runat="server" visible="false">
                                         <td align="center">
                                                Transportation Details :
                                                <asp:TextBox ID="txtcomments" runat="server"></asp:TextBox>
                                              </td>
                                            <td align="center">
                                                <asp:Button ID="btngeneratechallan" runat="server" Text="Generate Challan" CssClass="btn"
                                                    OnClientClick="javascript:return CheckForEmptyTextbox();" OnClick="btngeneratechallan_Click" />
                                                <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn" CausesValidation="False"
                                                    OnClick="btncancel_Click" />
                                            </td>
                                            <td align="center">
                                                &nbsp;</td>
                                            <td align="center">
                                                &nbsp;</td>
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
