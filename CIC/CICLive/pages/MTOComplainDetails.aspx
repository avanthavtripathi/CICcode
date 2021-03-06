﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/CICPage.master" AutoEventWireup="true"
    CodeFile="MTOComplainDetails.aspx.cs" Inherits="pages_MTOComplainDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainConHolder" runat="Server">

    <script language="javascript" type="text/javascript">
        window.onload = function() {
            Counter = 0;
        }
        function SetCounter() {
            Counter = 0;
        }

        function ChildClick(CheckBox) {
            var HeaderCheckBox = document.getElementById("ctl00_MainConHolder_gvFresh_ctl01_chkHeader");
            TotalChkBx = document.getElementById("ctl00_MainConHolder_gvFresh").rows.length;

            TotalChkBx = TotalChkBx - 2;
            //alert(TotalChkBx);
            if (CheckBox.checked && Counter < TotalChkBx) {
                Counter++;
            }
            else if (CheckBox.checked && Counter == 0)
            { Counter++; }
            else if (Counter > 0)
            { Counter--; }
            //alert("counter" + Counter);
            if (Counter < TotalChkBx) {
                HeaderCheckBox.checked = false;
            }
            else if (Counter == TotalChkBx) {
                HeaderCheckBox.checked = true;
            }
        }

        function SelectAllCheckboxes(spanChk) {
            // Added as ASPX uses SPAN for checkbox
            TotalChkBx = document.getElementById("ctl00_MainConHolder_gvFresh").rows.length;
            TotalChkBx = TotalChkBx - 1;
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;
            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" && elm[i].id != theBox.id) {//elm[i].click();
                if (elm[i].checked != xState)
                    elm[i].click();
                //elm[i].checked=xState;
            }
        }
    </script>

    <script type="text/javascript" language="javascript">

        function CheckRow() {
            alert('Defect Already Exists');
            return false;
        }


        function validateBatchCode(oSrc, args) {

            //var varBatchYear = (document.getElementById('ctl00_MainConHolder_hdnValidBatch').value);
            var varBatchYear = (document.getElementById('<% = hdnValidBatch.ClientID %>').value);

            var varBatchMonth = 'A|B|C|D|E|F|G|H|I|J|K|L';
            var arrayBatchYear = varBatchYear.split('|');
            var arrayBatchMonth = varBatchMonth.split('|');
            var bolBatchYearExit = new Boolean(false);
            var inputProdSer = document.getElementById('ctl00_MainConHolder_txtProductRefNo').value.toUpperCase();
            var inputBatchYear;
            var inputBatchMonth;
            //var ddlProductDivFir= document.getElementById('ctl00_MainConHolder_ddlFirProductDiv')
            //var productDiv=ddlProductDivFir.options[ddlProductDivFir.selectedIndex].text.toUpperCase();
            var productDiv = $get('ctl00_MainConHolder_lblUnit').innerHTML.toUpperCase();
            if (productDiv == 'APPLIANCES' || productDiv == 'APPLIANCE') {
                inputProdSer = inputProdSer.substring(2, 4);
                inputBatchYear = inputProdSer.substring(0, 1);
                inputBatchMonth = inputProdSer.substring(1, 2);
            }
            else {
                inputProdSer = inputProdSer.substring(0, 2);
                inputBatchYear = inputProdSer.substring(0, 1);
                inputBatchMonth = inputProdSer.substring(1, 2);
            }


            for (BatchYear in arrayBatchYear) {
                if (arrayBatchYear[BatchYear] == inputBatchYear) {
                    bolBatchYearExit = true;
                    break;
                }
            }
            if (bolBatchYearExit == true) {
                for (BatchMonth in arrayBatchMonth) {
                    if (arrayBatchMonth[BatchMonth] == inputBatchMonth) {
                        args.IsValid = true;
                        if (document.getElementById('ctl00_MainConHolder_txtBatchNo') != null) {
                            document.getElementById('ctl00_MainConHolder_txtBatchNo').value = inputProdSer;
                            break;
                        }
                    }
                    else {
                        args.IsValid = false;
                        if (document.getElementById('ctl00_MainConHolder_txtBatchNo') != null) {
                            document.getElementById('ctl00_MainConHolder_txtBatchNo').value = "Not a Vaild Serial No";
                        }
                    }

                }
            }
            else {
                args.IsValid = false

                if (document.getElementById('ctl00_MainConHolder_txtBatchNo') != null) {
                    document.getElementById('ctl00_MainConHolder_txtBatchNo').value = "Not a Vaild Serial No";
                }
            }
        }

        function CheckDefect() {

            var strValidChars = "0123456789";
            var strChar;
            var blnResult = true;
            var ddlCat = $get('ctl00_MainConHolder_ddlDefectCat').value;
            var ddlDef = $get('ctl00_MainConHolder_ddlDefect').value;
            var txtQty = $get('ctl00_MainConHolder_txtDefectQty');
            var strString = txtQty.value;
            if (ddlCat == 0) {
                alert("Select Defect Category");
                return false;
            }
            else if (ddlDef == 0) {
                alert("Select Defect");
                return false;
            }
            else if (txtQty.value == '') {
                alert("Enter Defect Quantity");
                return false;
            }
            else if (txtQty.value != '') {
                for (i = 0; i < strString.length && blnResult == true; i++) {
                    strChar = strString.charAt(i);
                    if (strValidChars.indexOf(strChar) == -1) {
                        blnResult = false;
                        alert("Enter Number Only in Quantity");
                    }
                }
                return blnResult;
            }

            else {
                return true;
            }
        }




        function validateDefectQty(oSrc, args) {

            var txtQty = $get('ctl00_MainConHolder_txtDefectQty');
            var strString = txtQty.value;
            var strValidChars = "0123456789";
            var strChar;

            args.IsValid = true;
            //  test strString consists of valid characters listed above
            for (i = 0; i < strString.length && args.IsValid == true; i++) {
                strChar = strString.charAt(i);
                if (strValidChars.indexOf(strChar) == -1) {
                    args.IsValid = false;

                }
                else {
                    args.IsValid = true;
                }
            }

        }

        function IsNumeric()
        //  check for valid numeric strings	
        {
            var txtQty = $get('ctl00_MainConHolder_txtDefectQty');
            var strString = txtQty.value;
            var strValidChars = "0123456789";
            var strChar;
            var blnResult = true;

            if (strString.length == 0) {
                alert("Enter Defect Quantity");
                return false;
            }

            //  test strString consists of valid characters listed above
            for (i = 0; i < strString.length && blnResult == true; i++) {
                strChar = strString.charAt(i);
                if (strValidChars.indexOf(strChar) == -1) {
                    blnResult = false;
                    alert("Enter Number Only in Quantity");
                }
            }
            return blnResult;
        }
        function validateloggedDate(oSrc, args) {
            var varActiondate = (document.getElementById('ctl00_MainConHolder_txtFirDate').value);
            var varServerDate = (document.getElementById('ctl00_MainConHolder_hdnGlobalDate').value);

            var arrayAction = varActiondate.split('/');
            var arrayServer = varServerDate.split('/');

            var actionDate = new Date();
            var serverDate = new Date();


            actionDate.setFullYear(arrayAction[2], (arrayAction[0] - 1), arrayAction[1]);
            actionDate.setHours(0, 0, 59, 0);
            serverDate.setFullYear(arrayServer[2], (arrayServer[0] - 1), arrayServer[1]);
            serverDate.setHours(0, 0, 59, 0);

            if (actionDate < serverDate) {
                args.IsValid = false;
            }
            else {
                args.IsValid = true
            }


        }
        // FOR CHECK FIR DATE CAN NOT BE FUTURE DATE
        
        function validateloggedDate1(oSrc, args) {
            var varActiondate = (document.getElementById('ctl00_MainConHolder_txtFirDate').value);
            var varServerCurrentDate = (document.getElementById('ctl00_MainConHolder_hdnGlobalDate').value);

            var arrayAction = varActiondate.split('/');
            var aarayCurrentdate = varServerCurrentDate.split('/');

            var actionDate = new Date();
            var serverDate = new Date();
            var curr_date = new Date();

            actionDate.setFullYear(arrayAction[2], (arrayAction[0] - 1), arrayAction[1]);
            actionDate.setHours(0, 0, 59, 0);

            curr_date.setFullYear(aarayCurrentdate[2], (aarayCurrentdate[0] - 1), aarayCurrentdate[1]);
            curr_date.setHours(0, 0, 59, 0);



            if (actionDate > curr_date) {
                args.IsValid = false;

            }
            else {
                args.IsValid = true;
            }
        }
        function validateActionloggedDate1(oSrc, args) {
            var varActiondate = (document.getElementById('ctl00_MainConHolder_txtInitialActionDate').value);
            var varServerCurrentDate = (document.getElementById('ctl00_MainConHolder_hdnGlobalDate').value);

            var arrayAction = varActiondate.split('/');
            var aarayCurrentdate = varServerCurrentDate.split('/');

            var actionDate = new Date();
            var serverDate = new Date();
            var curr_date = new Date();

            actionDate.setFullYear(arrayAction[2], (arrayAction[0] - 1), arrayAction[1]);
            actionDate.setHours(0, 0, 59, 0);

            curr_date.setFullYear(aarayCurrentdate[2], (aarayCurrentdate[0] - 1), aarayCurrentdate[1]);
            curr_date.setHours(0, 0, 59, 0);



            if (actionDate > curr_date) {
                args.IsValid = false;

            }
            else {
                args.IsValid = true;
            }
        }
        function validateDate(oSrc, args) {

            var varActiondate = (document.getElementById('ctl00_MainConHolder_txtFirDate').value);
            var varServerDate = (document.getElementById('ctl00_MainConHolder_hdnGlobalDate').value);
            var arrayAction = varActiondate.split('/');
            var arrayServer = varServerDate.split('/');
            var actionDate = new Date();
            var serverDate = new Date();
            actionDate.setFullYear(arrayAction[2], (arrayAction[0] - 1), arrayAction[1]);
            actionDate.setHours(0, 0, 59, 0);
            serverDate.setFullYear(arrayServer[2], (arrayServer[0] - 1), arrayServer[1]);
            serverDate.setHours(0, 0, 59, 0);
            serverDate.setDate(serverDate.getDate() - 2);
            if (actionDate < serverDate) {
                args.IsValid = false
            }
            else {
                args.IsValid = true
            }

        }
        function validateInvoiceDate(oSrc, args) {

            var varInvoicedate = (document.getElementById('ctl00_MainConHolder_txtInvoiceDate').value);
            var varServerDate = (document.getElementById('ctl00_MainConHolder_hdnGlobalDate').value);
            var arrayInvoice = varInvoicedate.split('/');
            var arrayServer = varServerDate.split('/');
            var InvoiceDate = new Date();
            var serverDate = new Date();
            InvoiceDate.setFullYear(arrayInvoice[2], (arrayInvoice[0] - 1), arrayInvoice[1]);
            InvoiceDate.setHours(0, 0, 59, 0);
            serverDate.setFullYear(arrayServer[2], (arrayServer[0] - 1), arrayServer[1]);
            serverDate.setHours(0, 0, 59, 0);
            //serverDate.setDate(serverDate.getDate() - 2);
            if (InvoiceDate > serverDate) {
                args.IsValid = false
            }
            else {
                args.IsValid = true
            }

        }
        function validateCompalaintDate(oSrc, args) {

            var varActiondate = (document.getElementById('ctl00_MainConHolder_txtCompalaintDate').value);
            var varServerCurrentDate = (document.getElementById('ctl00_MainConHolder_hdnGlobalDate').value);

            var arrayAction = varActiondate.split('/');
            var aarayCurrentdate = varServerCurrentDate.split('/');

            var actionDate = new Date();
            var serverDate = new Date();
            var curr_date = new Date();

            actionDate.setFullYear(arrayAction[2], (arrayAction[0] - 1), arrayAction[1]);
            actionDate.setHours(0, 0, 59, 0);

            curr_date.setFullYear(aarayCurrentdate[2], (aarayCurrentdate[0] - 1), aarayCurrentdate[1]);
            curr_date.setHours(0, 0, 59, 0);



            if (actionDate > curr_date) {
                args.IsValid = false;

            }
            else {
                args.IsValid = true;
            }

        }
        function validateInitializeDate(oSrc, args) {

            var varActiondate = (document.getElementById('ctl00_MainConHolder_txtInitialActionDate').value);
            var varServerDate = (document.getElementById('ctl00_MainConHolder_hdnInitDate').value);
            var arrayAction = varActiondate.split('/');
            var arrayServer = varServerDate.split('/');
            var actionDate = new Date();
            var serverDate = new Date();
            actionDate.setFullYear(arrayAction[2], (arrayAction[0] - 1), arrayAction[1]);
            actionDate.setHours(0, 0, 59, 0);
            serverDate.setFullYear(arrayServer[2], (arrayServer[0] - 1), arrayServer[1]);
            serverDate.setHours(0, 0, 59, 0);
            serverDate.setDate(serverDate.getDate() - 2);
            if (actionDate < serverDate) {
                args.IsValid = false
            }
            else {
                args.IsValid = true
            }

        }


        function validateDefectDate(oSrc, args) {
            var varActiondate = (document.getElementById('ctl00_MainConHolder_txtDefectDate').value);
            var varServerDate = (document.getElementById('ctl00_MainConHolder_hdnDefectCurrentDate').value);
            var arrayAction = varActiondate.split('/');
            var arrayServer = varServerDate.split('/');
            var actionDate = new Date();
            var serverDate = new Date();

            actionDate.setFullYear(arrayAction[2], (arrayAction[0] - 1), arrayAction[1]);
            actionDate.setHours(0, 0, 59, 0);
            serverDate.setFullYear(arrayServer[2], (arrayServer[0] - 1), arrayServer[1]);
            serverDate.setHours(0, 0, 59, 0);
            serverDate.setDate(serverDate.getDate() - 2);
            if (actionDate < serverDate) {
                args.IsValid = false
            }
            else {
                args.IsValid = true
            }

        }

        //MODIFY BY SUJIT OM 19-05-2010
        function validateActionDate(oSrc, args) {

            var varActiondate = (document.getElementById('ctl00_MainConHolder_txtActionEntryDate').value);
            var varServerDate = (document.getElementById('ctl00_MainConHolder_hdnActionCurrentDate').value);         
            var arrayAction = varActiondate.split('/');
            var arrayServer = varServerDate.split('/');
            var actionDate = new Date();
            var serverDate = new Date();
            actionDate.setFullYear(arrayAction[2], (arrayAction[0] - 1), arrayAction[1]);
            actionDate.setHours(0, 0, 59, 0);
            serverDate.setFullYear(arrayServer[2], (arrayServer[0] - 1), arrayServer[1]);
            serverDate.setHours(0, 0, 59, 0);
            serverDate.setDate(serverDate.getDate() - 2);
            if (actionDate < serverDate) {
                args.IsValid = false
            }
            else {
                args.IsValid = true
            }


        }
        //validate future date
         function validateActionFutureDate(oSrc, args) {

            var varActiondate = (document.getElementById('ctl00_MainConHolder_txtActionEntryDate').value);
            var varServerDate = (document.getElementById('ctl00_MainConHolder_hdnActionCurrentDate').value);         
           var arrayAction = varActiondate.split('/');
            var arrayServer = varServerDate.split('/');
            var actionDate = new Date();
            var serverDate = new Date();
            actionDate.setFullYear(arrayAction[2], (arrayAction[0] - 1), arrayAction[1]);
            actionDate.setHours(0, 0, 59, 0);
            serverDate.setFullYear(arrayServer[2], (arrayServer[0] - 1), arrayServer[1]);
            serverDate.setHours(0, 0, 59, 0);
           
            if (actionDate > serverDate) {
                args.IsValid = false
            }
            else {
                args.IsValid = true
            }

        }
        // End By sujit

        function funUploadPopUp(CRefNo) {
            var strUrl = '../Pages/UploadedFilePopUp.aspx?CompNo=' + CRefNo;
            custWin = window.open(strUrl, 'SCPopup', 'height=550,width=750,left=20,top=30,scrollbars=1');
            if (window.focus) { custWin.focus() }
        }



        function funCustomerMaster(cid, CRefNo) {
            var strUrl = '../Reports/CustomerDetail.aspx?CustNo=' + cid + '&CompNo=' + CRefNo;
            custWin = window.open(strUrl, 'SCPopup', 'height=550,width=750,left=20,top=30,scrollbars=1');
            if (window.focus) { custWin.focus() }
        }

        function funCommLog(compNo, splitNo) {
            var strUrl = 'CommunicationLog.aspx?CompNo=' + compNo + '&SplitNo=' + splitNo;
            logWin = window.open(strUrl, 'CommunicationLog', 'height=550,width=750,left=20,top=30,scrollbars=1');
            if (window.focus) { logWin.focus() }
        }
        function funHistoryLog(compNo, splitNo) {
            var strUrl = 'HistoryLog.aspx?CompNo=' + compNo + '&SplitNo=' + splitNo;
            hisWin = window.open(strUrl, 'History', 'height=550,width=750,left=20,top=30,scrollbars=1');
            if (window.focus) { hisWin.focus() }
        }
        function funPrint(compNo, baseID) {
            var strUrl = 'PrintCallSlip.aspx?qsCompNo=' + compNo + '&BaseLineId=' + baseID;
            prWin = window.open(strUrl, 'Print', 'height=650,width=850,left=20,top=30,resizable=1,scrollbars=1');
            if (window.focus) { prWin.focus() }
        }
        function OpenFindProduct() {
            //$get('dvFindProduct').style.display="block";
            //document.getElementById("dvFindProduct").style.display="block";
        }

        function funSpare() {
            var arrCompNo = document.getElementById('<% = lblActionComplaintRefNo.ClientID %>').innerHTML;
            arrCompNo = arrCompNo.split('/');
            var compNo = arrCompNo[0];
            var splitNo = (document.getElementById('<% = hdnActionSplitNo.ClientID %>').value);
            var strUrl = 'SpareReq.aspx?CompNo=' + compNo + '&SplitNo=' + splitNo;
            hisWin = window.open(strUrl, 'Spare', 'scrollbars=1');
            if (window.focus) { hisWin.focus() }
        }

        function DisAppDDl() {

            var ddlApp = $get('ctl00_MainConHolder_ddlAppointment');
            var ddlStatusValue = $get('ctl00_MainConHolder_ddlStatus');
            ddlApp.selectedIndex = 0;
            //alert("hi");
            if (ddlStatusValue.value == 2) {
                ddlApp.disabled = false;
            }
            else {
                ddlApp.disabled = true;
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        function FillDdlStatus() {
            var callStatus = ($get('<% = ddlStage.ClientID %>').value);
            PageMethods.BindingStatusByAjax(callStatus, callBackFunc);
        }

        function callBackFunc(dataTable) {
            var callTable = dataTable.value;
            var ddlcallStatus = $get('<% = ddlStatus.ClientID %>');
            ddlcallStatus.options.lenght = 0;
            ddlcallStatus.options[ddlcallStatus.options.lenght] = new Option("Select", 0);


        }
    </script>

    <asp:UpdatePanel ID="updateSC" runat="server">
        <ContentTemplate>
        
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="headingred" style="width: 30%">
                        MTO Complaint Details
                        <asp:HiddenField ID="hdnGlobalDate" runat="server" />
                    </td>
                    <td align="center" class="headingred" style="width: 40%">
                    </td>
                    <td align="<%=ConfigurationManager.AppSettings["AjaxLoadingAlign"]%>" style="padding-right: 10px;"
                        style="width: 30%">
                        <asp:UpdateProgress AssociatedUpdatePanelID="updateSC" ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 10px" align="center" class="bgcolorcomm" colspan="3">
                        <table width="100%">
                            <tr>
                                <td align="left" width="20%">
                                    Stage:
                                </td>
                                <td align="left" width="30%">
                                    <asp:DropDownList ID="ddlStage" CssClass="simpletxt1" Width="175" runat="server"
                                        AutoPostBack="true" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="Initialization">Initialization</asp:ListItem>
                                        <asp:ListItem Value="WIP">WIP</asp:ListItem>
                                        <asp:ListItem Value="Closure">Closure</asp:ListItem>
                                        <%--<asp:ListItem Value="TempClosed">TempClosed</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td align="left" width="20%">
                                    City:
                                </td>
                                <td align="left" width="30%">
                                    <asp:DropDownList ID="ddlCity" runat="server" CssClass="simpletxt1" Width="175" AutoPostBack="false">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    Status:
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlStatus" CssClass="simpletxt1" Width="400" runat="server"
                                        AppendDataBoundItems="True" AutoPostBack="False">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                    Product Division:
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSearchProductDivision" runat="server" AutoPostBack="True"
                                        CssClass="simpletxt1" Width="175">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <!--<td align="left">Territory:</td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTeritory" runat="server" AutoPostBack="True" CssClass="simpletxt1"
                                        Width="175">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                </td> -->
                            </tr>
                            <tr>
                                <td align="left">
                                    Appointment:
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlAppointment" runat="server" Enabled="False" CssClass="simpletxt1"
                                        Width="175">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Value="N">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                    Complaint Ref. No.:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtComplaintRefNo" Width="175" MaxLength="11" runat="server" CssClass="txtboxtxt"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" height="10px" class="MsgTDCount">
                                    Total Number of Records :
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblRowCount" Text="0" CssClass="MsgTotalCount" runat="server"></asp:Label>
                                </td>
                                <td align="left">
                                    SRF:
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSrf" CssClass="simpletxt1" Width="175" runat="server">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="N">No</asp:ListItem>
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                </td>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click"
                                        OnClientClick="javascript:SetCounter()" CausesValidation="false" Text="Search"
                                        Width="70px" />
                                </td>
                                <td align="left">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:GridView ID="gvFresh" runat="server" AllowPaging="True" AllowSorting="True"
                                        AlternatingRowStyle-CssClass="fieldName" AutoGenerateColumns="False" DataKeyNames="BaseLineId"
                                        GridGroups="both" HeaderStyle-CssClass="fieldNamewithbgcolor" Width="100%" RowStyle-CssClass="gridbgcolor"
                                        OnPageIndexChanging="gvFresh_PageIndexChanging" OnRowDataBound="gvFresh_RowDataBound">
                                        <RowStyle CssClass="gridbgcolor" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:SelectAllCheckboxes(this);" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkChild" onclick="javascript:ChildClick(this);" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RowNumber" HeaderText="Sno." />
                                            <asp:TemplateField HeaderText="Complaint RefNo" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnBaselineID" runat="server" Value='<%#Eval("BaseLineId")%>' />
                                                    <asp:HiddenField ID="hdnProductDivision_Sno" runat="server" Value='<%#Eval("ProductDivision_Sno")%>' />
                                                    <asp:HiddenField ID="hdnUnit_Desc" runat="server" Value='<%#Eval("Unit_Desc")%>' />
                                                    <asp:HiddenField ID="hdnComplaint_RefNo" runat="server" Value='<%#Eval("Complaint_RefNo")%>' />
                                                    <asp:HiddenField ID="hdnAppointmentFlag" runat="server" Value='<%#Eval("AppointmentReq")%>' />
                                                    <asp:HiddenField ID="gvFreshhdnCallStatus" runat="server" Value='<%#Eval("CallStatus")%>' />
                                                    <asp:HiddenField ID="gvFreshhdnCallStage" runat="server" Value='<%#Eval("CallStage")%>' />
                                                    <asp:HiddenField ID="gvFreshhdnNatureOfComplaint" runat="server" Value='<%#Eval("NatureOfComplaint")%>' />
                                                    <asp:HiddenField ID="gvFreshhdnASCReAllocFlag" runat="server" Value='<%#Eval("ASCReAllocFlag")%>' />
                                                    <asp:HiddenField ID="gvFreshhdnSLADate" runat="server" Value='<%#Eval("SLADate")%>' />
                                                    <asp:HiddenField ID="hdFreshDateOfReporting" runat="server" Value='<%#Eval("SLADate")%>' />
                                                    <asp:HiddenField ID="gvFreshSplit" runat="server" Value='<%#Eval("split")%>' />
                                                    <%--<asp:HiddenField ID="gvFreshhdnInvoiceDate" runat="server" Value='<%#Eval("InvoiceDate")%>' />--%>
                                                    <%--<a href="Javascript:void(0);" onclick="funComplaintDetails(<%#Eval("BaseLineId")%>)">--%>
                                                    <a href="Javascript:void(0);" onclick="funCommonPopUp(<%#Eval("BaseLineId")%>)">
                                                        <%#Eval("Complaint_RefNo")%>/<%#Eval("split")%></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("FormatedLoggedDate")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <a href="Javascript:void(0);" onclick="funCustomerMaster('<%#Eval("CustomerId")%>','<%#Eval("Complaint_RefNo")%>')">
                                                        <%#Eval("CustName")%></a>
                                                    <asp:HiddenField ID="gvFreshhdnLastComment" runat="server" Value='<%#Eval("LastComment")%>' />
                                                    <asp:HiddenField ID="gvFreshhdnLastName" runat="server" Value='<%#Eval("CustName")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="gvFreshhdnFullAddress" runat="server" Value='<%#Eval("FullAddress")%>' />
                                                    <%#Eval("Address1")%><%#Eval("Address2")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Contact No." HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("UniqueContact_No")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("Unit_Desc")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("Quantity")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Engineer Name" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("SEName")%>
                                                    <%--<%#Eval("NatureOfComplaint")%>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Engineer Name" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("CGUser")%>
                                                    <%--<%#Eval("NatureOfComplaint")%>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField HeaderText="SRF" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" Visible="false">
                                                <ItemTemplate>
                                                    <%#Eval("SRF") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comm. History" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <a href="Javascript:void(0);" onclick="funCommLog('<%#Eval("Complaint_RefNo")%>',<%#Eval("SplitComplaint_RefNo")%>)">
                                                        <%#Eval("LastComment")%></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stage" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <a href="Javascript:void(0);" onclick="funHistoryLog('<%#Eval("Complaint_RefNo")%>',<%#Eval("SplitComplaint_RefNo")%>)">
                                                        <%#Eval("CallStage")%></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <a href="Javascript:void(0);" onclick="funUploadPopUp('<%#Eval("Complaint_RefNo")%>')">
                                                        Click</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnNext" CommandArgument='<%#Eval("Complaint_RefNo")%>' ToolTip='<%#Eval("BaseLineId")%>'
                                                        CausesValidation="false" runat="server" CommandName='<%#Eval("CallStatus")%>'
                                                        Text="Next" OnClick="lnkBtnNext_Click">
                                                    </asp:LinkButton>
                                                    <a href="Javascript:void(0);" onclick="funPrint('<%#Eval("Complaint_RefNo")%>','<%#Eval("BaseLineId")%>')">
                                                        / Print</a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                        <AlternatingRowStyle CssClass="fieldName" />
                                        <EmptyDataTemplate>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center" style="padding-top: 50px; padding-bottom: 50px; padding-left: 60px;">
                                                        <b>No Complaints found</b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <asp:Button ID="btnPre" runat="server" CssClass="btn" OnClick="btnPre_Click" Text="<<" />
                                    <asp:Button ID="btnNxt" runat="server" CssClass="btn" OnClick="btnNxt_Click" Text=">>" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%" class="bgcolorcomm" cellpadding="3" id="tbLifted" runat="server"
                            visible="false" border="0">
                            <tr>
                                <td height="20" colspan="2" align="left" valign="bottom" style="border-bottom: 1px solid #396870">
                                    <b>Action: Equipment Lifted by Customer</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Complaint RefNo:
                                </td>
                                <td align="left">
                                    <asp:Label ID="eqlblComplaint" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Action Remarks:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="eqtxtRemarks" CssClass="txtboxtxt" ValidationGroup="eqp" Width="200px"
                                        runat="server">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="eqphdnBaselineID" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtInitialActionDate"
                                        ErrorMessage="Enter Action Remarks" Display="None" ValidationGroup="eqp">
                                    </asp:RequiredFieldValidator>
                                    <asp:ValidationSummary ID="eqpValidSummary" ShowSummary="False" runat="server" ValidationGroup="eqp"
                                        ShowMessageBox="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="eqbtnLifted" ValidationGroup="eqp" runat="server" Text="Equipment Lifted"
                                        CssClass="btn" OnClick="eqbtnLifted_Click" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%" class="bgcolorcomm" cellpadding="3" id="tbIntialization" runat="server"
                            visible="true" border="0">
                            <tr>
                                <td colspan="3" height="20" align="left" valign="bottom" style="border-bottom: 1px solid #396870">
                                    <b>Initialization</b>
                                </td>
                            </tr>
                             <tr id="trchkSelfAllocatopn" runat="server" visible="false">
                                <td align="left" style="padding-left: 200px">
                                    Allocate to Myself<asp:CheckBox ID="chkSelfAllocatopn" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelfAllocatopn_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" height="20" align="left" style="padding-left: 200px">
                                    Action:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <%--<asp:DropDownList ID="ddlInitTempClosedAction" runat="server" Visible="false" Width="300"
                                        CssClass="simpletxt1" AutoPostBack="True" 
                                        onselectedindexchanged="ddlTakeAppointment_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="20">(WIP)Equipment received at ASC</asp:ListItem>
                                        </asp:DropDownList>--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlTakeAppointment"
                                        InitialValue="0" ErrorMessage="Select Action" Display="None" ValidationGroup="init">
                                    </asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="ddlTakeAppointment" runat="server" Visible="false" Width="490"
                                        CssClass="simpletxt1" AutoPostBack="True" OnSelectedIndexChanged="ddlTakeAppointment_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <%--<asp:ListItem Value="41">(Initialization)Complaint Logged by division and forwarded to CGL Branch</asp:ListItem>--%>
                                        <asp:ListItem Value="41">(Initialization) Allocated to CGL Service Engineer</asp:ListItem>
                                        <asp:ListItem Value="44">(Initialization)Appointment taken/Shutdown requested</asp:ListItem>
                                        <asp:ListItem Value="59">(Closure)Complaint Cancelled</asp:ListItem>
                                        <asp:ListItem Value="42">(Initialization)Allocated to Service Engineer of ASC</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlTakeAppointment" runat="server"
                                        ControlToValidate="ddlTakeAppointment" InitialValue="0" ErrorMessage="Select Action"
                                        Display="None" ValidationGroup="init">
                                    </asp:RequiredFieldValidator>
                                    <%--First Time Bind--%>
                                    <asp:DropDownList ID="ddlAllocateAction" runat="server" Visible="true" Width="490"
                                        CssClass="simpletxt1" AutoPostBack="True" OnSelectedIndexChanged="ddlAllocateAction_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="42">(Initialization)Allocate to Service Engineer</asp:ListItem>
                                        <asp:ListItem Value="43">(Initialization)Allocated TO ASC</asp:ListItem>
                                        <asp:ListItem Value="44">(Initialization)Take Appointment/request shut down</asp:ListItem>
                                        <asp:ListItem Value="41">(Initialization)Allocate CG User</asp:ListItem>
                                        <asp:ListItem Value="59">(Closure)Complaint Cancelled</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlAllocateAction" runat="server"
                                        ControlToValidate="ddlAllocateAction" InitialValue="0" ErrorMessage="Select Action"
                                        Display="None" ValidationGroup="init">
                                    </asp:RequiredFieldValidator>
                                    <%--Second Time Bind--%>
                                    <asp:DropDownList ID="ddlOtherAction" runat="server" Visible="false" Width="490"
                                        CssClass="simpletxt1" AutoPostBack="True" OnSelectedIndexChanged="ddlOtherAction_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="42">(Initialization)Reallocate to Service Engineer</asp:ListItem>
                                        <asp:ListItem Value="43">(Initialization)Allocated TO ASC</asp:ListItem>
                                        <asp:ListItem Value="45">(Initialization)Power Outage at Customer site</asp:ListItem>
                                        <asp:ListItem Value="46">(Initialization)Responsible person not available</asp:ListItem>
                                        <asp:ListItem Value="41">(Initialization)Reallocate to CG User</asp:ListItem>
                                        <asp:ListItem Value="59">(Closure)Complaint Cancelled</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlOtherAction" runat="server"
                                        ControlToValidate="ddlOtherAction" InitialValue="0" ErrorMessage="Select Action"
                                        Display="None" ValidationGroup="init">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>                            
                            <tr id="trInitAppointDateTime" runat="server" visible="false">
                                <td width="100%" colspan="3" height="20" align="left" style="padding-left: 260px">
                                    <table align="left" border="0">
                                        <tr>
                                            <td colspan="3" align="left" width="75%">
                                                Appointment Date:
                                                <asp:TextBox ID="txtAppointMentDate" runat="server" Width="100" CssClass="txtboxtxt"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender3" TargetControlID="txtAppointMentDate"
                                                    runat="server">
                                                </cc1:CalendarExtender>
                                                Appointment Time:<asp:DropDownList ID="ddlInitAppHr" runat="server" ValidationGroup="init"
                                                    CssClass="simpletxt1">
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>4</asp:ListItem>
                                                    <asp:ListItem>5</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>7</asp:ListItem>
                                                    <asp:ListItem>8</asp:ListItem>
                                                    <asp:ListItem>9</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                </asp:DropDownList>
                                                :
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15"
                                     runat="server" ControlToValidate="ddlInitHr" InitialValue="0"
                                        ErrorMessage="Enter Action Hour" Display="None" ValidationGroup="init" >
                                    </asp:RequiredFieldValidator>--%>
                                                <asp:DropDownList ID="ddlInitAppMin" runat="server" ValidationGroup="init" CssClass="simpletxt1">
                                                    <asp:ListItem Value="0">00</asp:ListItem>
                                                    <asp:ListItem Value="1">01</asp:ListItem>
                                                    <asp:ListItem Value="2">02</asp:ListItem>
                                                    <asp:ListItem Value="3">03</asp:ListItem>
                                                    <asp:ListItem Value="4">04</asp:ListItem>
                                                    <asp:ListItem Value="5">05</asp:ListItem>
                                                    <asp:ListItem Value="6">06</asp:ListItem>
                                                    <asp:ListItem Value="7">07</asp:ListItem>
                                                    <asp:ListItem Value="8">08</asp:ListItem>
                                                    <asp:ListItem Value="9">09</asp:ListItem>
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>11</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                    <asp:ListItem>13</asp:ListItem>
                                                    <asp:ListItem>14</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>16</asp:ListItem>
                                                    <asp:ListItem>17</asp:ListItem>
                                                    <asp:ListItem>18</asp:ListItem>
                                                    <asp:ListItem>19</asp:ListItem>
                                                    <asp:ListItem>20</asp:ListItem>
                                                    <asp:ListItem>21</asp:ListItem>
                                                    <asp:ListItem>22</asp:ListItem>
                                                    <asp:ListItem>23</asp:ListItem>
                                                    <asp:ListItem>24</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>26</asp:ListItem>
                                                    <asp:ListItem>27</asp:ListItem>
                                                    <asp:ListItem>28</asp:ListItem>
                                                    <asp:ListItem>29</asp:ListItem>
                                                    <asp:ListItem>30</asp:ListItem>
                                                    <asp:ListItem>31</asp:ListItem>
                                                    <asp:ListItem>32</asp:ListItem>
                                                    <asp:ListItem>33</asp:ListItem>
                                                    <asp:ListItem>34</asp:ListItem>
                                                    <asp:ListItem>35</asp:ListItem>
                                                    <asp:ListItem>36</asp:ListItem>
                                                    <asp:ListItem>37</asp:ListItem>
                                                    <asp:ListItem>38</asp:ListItem>
                                                    <asp:ListItem>39</asp:ListItem>
                                                    <asp:ListItem>40</asp:ListItem>
                                                    <asp:ListItem>41</asp:ListItem>
                                                    <asp:ListItem>42</asp:ListItem>
                                                    <asp:ListItem>43</asp:ListItem>
                                                    <asp:ListItem>44</asp:ListItem>
                                                    <asp:ListItem>45</asp:ListItem>
                                                    <asp:ListItem>46</asp:ListItem>
                                                    <asp:ListItem>47</asp:ListItem>
                                                    <asp:ListItem>48</asp:ListItem>
                                                    <asp:ListItem>49</asp:ListItem>
                                                    <asp:ListItem>50</asp:ListItem>
                                                    <asp:ListItem>51</asp:ListItem>
                                                    <asp:ListItem>52</asp:ListItem>
                                                    <asp:ListItem>53</asp:ListItem>
                                                    <asp:ListItem>54</asp:ListItem>
                                                    <asp:ListItem>55</asp:ListItem>
                                                    <asp:ListItem>56</asp:ListItem>
                                                    <asp:ListItem>57</asp:ListItem>
                                                    <asp:ListItem>58</asp:ListItem>
                                                    <asp:ListItem>59</asp:ListItem>
                                                    <asp:ListItem>60</asp:ListItem>
                                                </asp:DropDownList>
                                                :
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator16"
                                     runat="server" ControlToValidate="ddlInitMin" InitialValue="0"
                                        ErrorMessage="Enter Action Minute" Display="None" ValidationGroup="init" >
                                    </asp:RequiredFieldValidator>--%>
                                                <asp:DropDownList ID="ddlInitAppAm" runat="server" ValidationGroup="init" CssClass="simpletxt1">
                                                    <asp:ListItem>AM</asp:ListItem>
                                                    <asp:ListItem>PM</asp:ListItem>
                                                </asp:DropDownList>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator17"
                                     runat="server" ControlToValidate="ddlInitAm" InitialValue="0"
                                        ErrorMessage="Enter Action AM/PM" Display="None" ValidationGroup="init" >
                                    </asp:RequiredFieldValidator>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trInitEngineer" visible="false" runat="server">
                                <td width="100%" colspan="3" height="20" align="left" style="padding-left: 260px">
                                    <table align="left" border="0">
                                        <tr>
                                            <td align="left" width="75%">
                                                Engineer:<asp:DropDownList ID="ddlServiceEngg" runat="server" Width="200" CssClass="simpletxt1"
                                                    ValidationGroup="init">
                                                    <asp:ListItem Value="0" Text="Select"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:CompareValidator ID="cmpDdlEngg" runat="server" ControlToValidate="ddlServiceEngg"
                                                    ErrorMessage="Select Engineer" ValueToCompare="Select" ValidationGroup="init"
                                                    Operator="NotEqual" Display="none"></asp:CompareValidator>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorddlServiceEngg"
                                                 runat="server" ControlToValidate="ddlServiceEngg" InitialValue="0" 
                                                    ErrorMessage="Select Engineer" Display="None" ValidationGroup="init" >
                                                </asp:RequiredFieldValidator>--%>
                                            </td>
                                            <td align="right" width="20%">
                                                <asp:Label ID="lblSMS" runat="server" Visible="false" Text="Send SMS"></asp:Label>
                                            </td>
                                            <td width="5%">
                                                <asp:CheckBox ID="chkSMS" runat="server" Visible="false" Checked="True" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trSCName" visible="false" runat="server">
                                <td width="100%" height="20" align="left" style="padding-left: 260px">
                                    <table align="left" border="0">
                                        <tr>
                                            <td width="100">
                                                SC Name:
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlSCName" runat="server" Width="400" CssClass="simpletxt1"
                                                    ValidationGroup="init">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvSCName" runat="server" ControlToValidate="ddlSCName"
                                                    InitialValue="0" ErrorMessage="Select SC Name" Display="None" ValidationGroup="init">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>                           
                            <tr id="trCGExecutiveType" visible="false" runat="server">
                                <td width="100%" height="20" align="left" style="padding-left: 260px">
                                    <table align="left" border="0">
                                        <tr>
                                            <td width="180">
                                                CG Employee Type:
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlCGExType" runat="server" Width="200" CssClass="simpletxt1"
                                                    ValidationGroup="init" AutoPostBack="True" OnSelectedIndexChanged="ddlCGExType_SelectedIndexChanged">
                                                    <asp:ListItem Value="0" Text="Select">Select</asp:ListItem>
                                                    <asp:ListItem Value="2" Text="Select">CG Employee</asp:ListItem>
                                                    <asp:ListItem Value="5" Text="Select">CG Contracted Employee</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVCGExType" runat="server" ControlToValidate="ddlCGExType"
                                                    InitialValue="0" ErrorMessage="Select CG Excutive Type" Display="None" ValidationGroup="init">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trCGExecutive" visible="false" runat="server">
                                <td width="100%" colspan="3" height="20" align="left" style="padding-left: 260px">
                                    <table align="left" border="0">
                                        <tr>
                                            <td width="180">
                                                CG Employee:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlCGExcutive" runat="server" Width="200" CssClass="simpletxt1"
                                                    ValidationGroup="init">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RFVCGEmployee" runat="server" ControlToValidate="ddlCGExcutive"
                                                    InitialValue="0" ErrorMessage="Select CG Employee" Display="None" ValidationGroup="init">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trCGEngineer" visible="false" runat="server">
                                <td width="100%" colspan="3" height="20" align="left" style="padding-left: 260px">
                                    <table align="left" border="0">
                                        <tr>
                                            <td width="180">
                                                CG Contracted Employee:
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlCGEng" runat="server" Width="200" CssClass="simpletxt1"
                                                    ValidationGroup="init">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCGEng" runat="server" ControlToValidate="ddlCGEng"
                                                    InitialValue="0" ErrorMessage="Select CG Contracted Employee" Display="None" ValidationGroup="init">
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left" style="padding-left: 200px">
                                    Remarks:<font color="red">*</font> &nbsp;<asp:TextBox ID="txtInitializationActionRemarks" Text="" runat="server"
                                        Width="490" CssClass="txtboxtxt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtInitializationActionRemarks"
                                        ErrorMessage="Enter Action Remarks" Display="None" ValidationGroup="init">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="left" style="padding-left: 180px">
                                    Action Date:<font color="red">*</font>
                                    <asp:TextBox ID="txtInitialActionDate" CssClass="txtboxtxt" runat="server">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtInitialActionDate"
                                        ErrorMessage="Enter Action Date" Display="None" ValidationGroup="init">
                                    </asp:RequiredFieldValidator>
                                    <cc1:CalendarExtender ID="CalendarExtender4" TargetControlID="txtInitialActionDate"
                                        runat="server">
                                    </cc1:CalendarExtender>
                                    <asp:CompareValidator Operator="DataTypeCheck" Type="Date" ControlToValidate="txtInitialActionDate"
                                        Display="None" ErrorMessage="Not a vaild Date" runat="server" ID="CompareValidator1"
                                        ValidationGroup="init">
                                    </asp:CompareValidator>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Action Time:<font color="red">*</font>
                                    <asp:DropDownList ID="ddlInitHr" runat="server" ValidationGroup="init" CssClass="simpletxt1">
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>
                                    :
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15"
                                     runat="server" ControlToValidate="ddlInitHr" InitialValue="0"
                                        ErrorMessage="Enter Action Hour" Display="None" ValidationGroup="init" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:DropDownList ID="ddlInitMin" runat="server" ValidationGroup="init" CssClass="simpletxt1">
                                        <asp:ListItem Value="0">00</asp:ListItem>
                                        <asp:ListItem Value="1">01</asp:ListItem>
                                        <asp:ListItem Value="2">02</asp:ListItem>
                                        <asp:ListItem Value="3">03</asp:ListItem>
                                        <asp:ListItem Value="4">04</asp:ListItem>
                                        <asp:ListItem Value="5">05</asp:ListItem>
                                        <asp:ListItem Value="6">06</asp:ListItem>
                                        <asp:ListItem Value="7">07</asp:ListItem>
                                        <asp:ListItem Value="8">08</asp:ListItem>
                                        <asp:ListItem Value="9">09</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                        <asp:ListItem>24</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>26</asp:ListItem>
                                        <asp:ListItem>27</asp:ListItem>
                                        <asp:ListItem>28</asp:ListItem>
                                        <asp:ListItem>29</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>31</asp:ListItem>
                                        <asp:ListItem>32</asp:ListItem>
                                        <asp:ListItem>33</asp:ListItem>
                                        <asp:ListItem>34</asp:ListItem>
                                        <asp:ListItem>35</asp:ListItem>
                                        <asp:ListItem>36</asp:ListItem>
                                        <asp:ListItem>37</asp:ListItem>
                                        <asp:ListItem>38</asp:ListItem>
                                        <asp:ListItem>39</asp:ListItem>
                                        <asp:ListItem>40</asp:ListItem>
                                        <asp:ListItem>41</asp:ListItem>
                                        <asp:ListItem>42</asp:ListItem>
                                        <asp:ListItem>43</asp:ListItem>
                                        <asp:ListItem>44</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                        <asp:ListItem>46</asp:ListItem>
                                        <asp:ListItem>47</asp:ListItem>
                                        <asp:ListItem>48</asp:ListItem>
                                        <asp:ListItem>49</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>51</asp:ListItem>
                                        <asp:ListItem>52</asp:ListItem>
                                        <asp:ListItem>53</asp:ListItem>
                                        <asp:ListItem>54</asp:ListItem>
                                        <asp:ListItem>55</asp:ListItem>
                                        <asp:ListItem>56</asp:ListItem>
                                        <asp:ListItem>57</asp:ListItem>
                                        <asp:ListItem>58</asp:ListItem>
                                        <asp:ListItem>59</asp:ListItem>
                                        <asp:ListItem>60</asp:ListItem>
                                    </asp:DropDownList>
                                    :
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator16"
                                     runat="server" ControlToValidate="ddlInitMin" InitialValue="0"
                                        ErrorMessage="Enter Action Minute" Display="None" ValidationGroup="init" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:DropDownList ID="ddlInitAm" runat="server" ValidationGroup="init" CssClass="simpletxt1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator17"
                                     runat="server" ControlToValidate="ddlInitAm" InitialValue="0"
                                        ErrorMessage="Enter Action AM/PM" Display="None" ValidationGroup="init" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:ValidationSummary ID="valsuminitial" runat="server" ValidationGroup="init" ShowMessageBox="True"
                                        ShowSummary="False" />
                                    <asp:CustomValidator ValidationGroup="init" ID="CustomValidator2" runat="server"
                                        ControlToValidate="txtInitialActionDate" Display="None" ErrorMessage="Action Date cannot be less then two days from current date"
                                        ClientValidationFunction="validateInitializeDate">
                                    </asp:CustomValidator>
                                    <asp:HiddenField ID="hdnInitDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" height="10" align="left" valign="bottom" style="border-top: 1px solid #396870">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%" align="center" colspan="3">
                                    <asp:Button ID="btnAllocate" runat="server" Width="70px" Text="Save" CssClass="btn"
                                        ValidationGroup="init" CausesValidation="true" OnClick="btnAllocate_Click" />
                                    <asp:Button ID="btnInitialiseClose" runat="server" Width="70px" Text="Close" CssClass="btn"
                                        OnClick="btnInitialiseClose_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%" visible="false" runat="server" id="tbBasicRegistrationInformation"
                            style="padding: 3px">
                            <tr>
                                <td colspan="4" height="20" align="left" valign="bottom" style="border-bottom: 1px solid #396870">
                                    <b>Products Details Entry</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="bottom" style="border-bottom: 1px solid #396870" colspan="4"
                                    height="20">
                                    <asp:UpdateProgress AssociatedUpdatePanelID="updateSC" ID="UpdateProgress2" runat="server">
                                        <ProgressTemplate>
                                            <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                            <tr id="trProductDiv" runat="server" visible="false">
                                <td colspan="4">
                                    <asp:DropDownList ID="ddlProductDiv" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Complaint Ref No.:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblComplainNo" runat="server" Text="">
                                    </asp:Label>
                                </td>
                                <td align="right">
                                    Complaint Date:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblComplainDate" runat="server" Text="">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%" align="right">
                                    Product Division:
                                </td>
                                <td align="left" width="35%">
                                    <asp:Label ID="lblUnit" runat="server" Text="">
                                    </asp:Label>
                                    <asp:DropDownList ID="ddlFirProductDiv" Visible="false" Width="120px" CssClass="simpletxt1"
                                        runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFirProductDiv_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:LinkButton ID="lnkFirEditProdDiv" Visible="false" runat="server" OnClick="lnkFirEditProdDiv_Click">Edit Product Division</asp:LinkButton>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="AddTempGv"
                                        runat="server" ErrorMessage="Select Product Division" InitialValue="0" 
                                        ControlToValidate="ddlFirProductDiv" Display="None"></asp:RequiredFieldValidator>--%>
                                    <asp:HiddenField ID="hdnCustmerID" runat="server" />
                                    <asp:HiddenField ID="hdnProductDvNo" runat="server" />
                                    <asp:HiddenField ID="hdnProductDvCode" runat="server" />
                                    <asp:HiddenField ID="hdnComplaintRef" runat="server" />
                                    <asp:HiddenField ID="hdnCallStatus" runat="server" />
                                    <asp:HiddenField ID="hdnNatureOfComplaint" runat="server" />
                                    <asp:HiddenField ID="hdnFirState" runat="server" />
                                    <asp:HiddenField ID="hdnFirCity" runat="server" />
                                    <asp:HiddenField ID="hdnSLADate" runat="server" />
                                </td>
                                <td width="20%">
                                    &nbsp;
                                </td>
                                <td width="25%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Product Line:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ValidationGroup="AddTempGv" ID="ddlProductLine" CssClass="simpletxt1"
                                        Width="400px" runat="server" OnSelectedIndexChanged="ddlProductLine_SelectedIndexChanged"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="AddTempGv"
                                        runat="server" ErrorMessage="Select Product Line" InitialValue="0" ControlToValidate="ddlProductLine"
                                        Display="None"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtFindPL" ValidationGroup="ProductRef" CssClass="txtboxtxt" runat="server"
                                        Visible="false" Width="70" CausesValidation="True"></asp:TextBox>
                                    <asp:Button ID="btnGoPL" Visible="false" runat="server" Width="20px" Text="Go" CssClass="btn"
                                        OnClick="btnGoPL_Click" />
                                </td>
                                <td valign="top" align="right">
                                    Product Serial No:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtProductRefNo" ValidationGroup="AddTempGv" MaxLength="20" CssClass="txtboxtxt"
                                        runat="server" AutoPostBack="true" 
                                        ontextchanged="txtProductRefNo_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtProductRefNo"
                                        ErrorMessage="Enter Product Serial No." ValidationGroup="AddTempGv" Display="None"></asp:RequiredFieldValidator>
                                    <%--<asp:CustomValidator ValidationGroup="AddTempGv" ID="CustomValidator7" runat="server"
                                        ControlToValidate="txtProductRefNo" Display="None" ErrorMessage="Product Serial No is not Valid"
                                        ClientValidationFunction="validateBatchCode">
                                    </asp:CustomValidator>--%>
                                    <asp:HiddenField ID="hdnValidBatch" runat="server" />
                                </td>
                            </tr>
                            <tr id="trProductGroup" runat="server" visible="false">
                                <td align="right">
                                    Product Group:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ValidationGroup="AddTempGv" ID="ddlProductGroup" CssClass="simpletxt1"
                                        Width="250px" runat="server" AppendDataBoundItems="false" OnSelectedIndexChanged="ddlProductGroup_SelectedIndexChanged"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" InitialValue="0"
                                        ControlToValidate="ddlProductGroup" ErrorMessage="Select Product Group" ValidationGroup="AddTempGv"
                                        Display="None"></asp:RequiredFieldValidator>--%>
                                    <asp:TextBox ID="txtFindPG" ValidationGroup="ProductRef" Width="70" CssClass="txtboxtxt"
                                        runat="server"></asp:TextBox>
                                    <asp:Button ID="btnGoPG" Enabled="false" runat="server" Width="20px" Text="Go" CssClass="btn"
                                        OnClick="btnGoPG_Click" />
                                </td>
                                <td align="right">
                                    Batch No:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtBatchNo" CssClass="txtboxtxt" runat="server" Enabled="False"></asp:TextBox>
                                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtBatchNo"
                                        ValidationGroup="AddTempGv" ErrorMessage="First two letters should alphabatic in Product Serial No."
                                        ValidationExpression="[a-zA-Z]{2,}.*" Display="None"></asp:RegularExpressionValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Product:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ValidationGroup="AddTempGv" ID="ddlProduct" AppendDataBoundItems="false"
                                        CssClass="simpletxt1" Width="400px" runat="server">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlProduct"
                                        ErrorMessage="Select Product" InitialValue="0" ValidationGroup="AddTempGv" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td align="left" valign="top">
                                    <asp:TextBox ID="txtFindP" Visible="false" ValidationGroup="ProductRef" CssClass="txtboxtxt"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td align="left" valign="top">
                                    <asp:Button ID="btnGoP" Visible="false" runat="server" Width="20px" Text="Go" CssClass="btn"
                                        OnClick="btnGoP_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Invoice Date:<asp:Label ID="lblStarInvDt" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtInvoiceDate" ValidationGroup="AddTempGv" CssClass="txtboxtxt"
                                        runat="server">
                                    </asp:TextBox>
                                    <asp:CustomValidator ValidationGroup="AddTempGv" ID="CustomValidator6" runat="server"
                                        ControlToValidate="txtInvoiceDate" Display="None" ErrorMessage="Invoice date should be less than current date"
                                        ClientValidationFunction="validateInvoiceDate">
                                    </asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtInvoiceDate" runat="server"
                                        ControlToValidate="txtInvoiceDate" ErrorMessage="Enter Invoice Date" ValidationGroup="AddTempGv"
                                        Display="None"></asp:RequiredFieldValidator>
                                    <cc1:CalendarExtender ID="CalendarExtender1" TargetControlID="txtInvoiceDate" runat="server">
                                    </cc1:CalendarExtender>
                                </td>
                                <td align="right">
                                    Invoice Number:<asp:Label ID="lblStarInvNum" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtInvoiceNo" MaxLength="25" ValidationGroup="AddTempGv" CssClass="txtboxtxt"
                                        runat="server">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtInvoiceNo" runat="server"
                                        ControlToValidate="txtInvoiceNo" ErrorMessage="Enter Invoice Number" ValidationGroup="AddTempGv"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%--Added By Gaurav Garg on 2 Dec--%>
                            <tr>
                                <td align="right">
                                    Dispatch Date:<asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDispatchDate" ValidationGroup="AddTempGv" CssClass="txtboxtxt"
                                        runat="server">
                                    </asp:TextBox>
                                    <asp:CustomValidator ValidationGroup="AddTempGv" ID="CustomValidator7" runat="server"
                                        ControlToValidate="txtDispatchDate" Display="None" ErrorMessage="Dispatch date should be less than current date"
                                        ClientValidationFunction="validateInvoiceDate">
                                    </asp:CustomValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDispatchDate"
                                        ErrorMessage="Enter Dispatch Date" ValidationGroup="AddTempGv" Display="None"></asp:RequiredFieldValidator>
                                    <cc1:CalendarExtender ID="CalendarExtender8" TargetControlID="txtDispatchDate" runat="server">
                                    </cc1:CalendarExtender>
                                </td>
                                 <td align="right">
                                                            Manufacture Date <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox MaxLength="10" ValidationGroup="AddTempGv" runat="server" ID="txtPoDate"
                                        CssClass="txtboxtxt" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtPoDate"
                                        ErrorMessage="Enter Manufacture Date" ValidationGroup="AddTempGv" Display="None"></asp:RequiredFieldValidator>
                                    <cc1:CalendarExtender ID="CalendarExtender9" TargetControlID="txtPoDate" runat="server">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Warranty Expiry Date
                                    <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="left">
                                    <%-- </td>
                                                        <td style="width: 21%">--%>
                                    <asp:TextBox MaxLength="10" ValidationGroup="AddTempGv" runat="server" ID="txtWarrantyDate"
                                        CssClass="txtboxtxt" />
                                    <cc1:CalendarExtender ID="CalendarExtender30" runat="server" TargetControlID="txtWarrantyDate" >
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtWarrantyDate"
                                        ErrorMessage="Enter Warranty Expiry Date" ValidationGroup="AddTempGv" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td align="right">
                                    Date of Commission
                                </td>
                                <td align="left">
                                    <asp:TextBox ValidationGroup="AddTempGv" MaxLength="10" runat="server" ID="txtCommissionDate"
                                        CssClass="txtboxtxt" />
                                    <cc1:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txtCommissionDate"  >
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Date Of Reporting
                                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox MaxLength="10" ValidationGroup="AddTempGv" runat="server" ID="txtCompalaintDate"
                                        CssClass="txtboxtxt" />
                                    <asp:CustomValidator ValidationGroup="AddTempGv" ID="CustomValidator8" runat="server"
                                        ControlToValidate="txtCompalaintDate" Display="None" ErrorMessage="Compalaint date can not be greater than current date"
                                        ClientValidationFunction="validateCompalaintDate"> </asp:CustomValidator>
                                    <cc1:CalendarExtender ID="CaltxtCompalaintDate" runat="server" TargetControlID="txtCompalaintDate"  >
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="ReqtxtCompalaintDate" runat="server" ControlToValidate="txtCompalaintDate"
                                        ErrorMessage="Enter Coplaint Date" ValidationGroup="AddTempGv" Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td align="right">
                                </td>
                                <td align="left">
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Additional Information:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtWarranty" ValidationGroup="AddTempGv" Text="" CssClass="txtboxtxt"
                                        Height="50px" Width="250px" TextMode="MultiLine" runat="server">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtWarranty"
                                        ErrorMessage="Enter Additional Information" ValidationGroup="AddTempGv" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hdnBaseLineID" runat="server" />
                                    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="AddTempGv" runat="server"
                                        ShowMessageBox="True" ShowSummary="False" />
                                </td>
                                <td align="right" valign="top">
                                    Call Under Warranty:
                                </td>
                                <td align="left" valign="top">
                                    <asp:RadioButtonList ID="rdbWarranty" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="True" OnSelectedIndexChanged="rdbWarranty_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Yes</asp:ListItem>
                                        <asp:ListItem Value="1">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    FIR Date:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtFirDate" CssClass="txtboxtxt" runat="server">
                                    </asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFirDate" runat="server">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtFirDate"
                                        ErrorMessage="Enter FIR Date" ValidationGroup="AddTempGv" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator Operator="DataTypeCheck" Type="Date" ControlToValidate="txtFirDate" Enabled="false" 
                                        Display="None" ErrorMessage="Not a vaild Date" runat="server" ID="cmptxtFromDate"
                                        ValidationGroup="AddTempGv">
                                    </asp:CompareValidator>
                                    <asp:CustomValidator ValidationGroup="AddTempGv" ID="CustomValidator1" runat="server"
                                        ControlToValidate="txtFirDate" Display="None" ErrorMessage="FIR Date cannot be less then two days from current date"
                                        ClientValidationFunction="validateDate">
                                    </asp:CustomValidator>
                                      
                                    
                                    <asp:DropDownList ID="ddlFirHr" runat="server" ValidationGroup="AddTempGv" CssClass="simpletxt1">
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>
                                    :
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator18"
                                     runat="server" ControlToValidate="ddlFirHr" InitialValue="0"
                                        ErrorMessage="Enter Action Hour" Display="None" ValidationGroup="AddTempGv" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:DropDownList ID="ddlFirMin" runat="server" ValidationGroup="AddTempGv" CssClass="simpletxt1">
                                        <asp:ListItem Value="0">00</asp:ListItem>
                                        <asp:ListItem Value="1">01</asp:ListItem>
                                        <asp:ListItem Value="2">02</asp:ListItem>
                                        <asp:ListItem Value="3">03</asp:ListItem>
                                        <asp:ListItem Value="4">04</asp:ListItem>
                                        <asp:ListItem Value="5">05</asp:ListItem>
                                        <asp:ListItem Value="6">06</asp:ListItem>
                                        <asp:ListItem Value="7">07</asp:ListItem>
                                        <asp:ListItem Value="8">08</asp:ListItem>
                                        <asp:ListItem Value="9">09</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                        <asp:ListItem>24</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>26</asp:ListItem>
                                        <asp:ListItem>27</asp:ListItem>
                                        <asp:ListItem>28</asp:ListItem>
                                        <asp:ListItem>29</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>31</asp:ListItem>
                                        <asp:ListItem>32</asp:ListItem>
                                        <asp:ListItem>33</asp:ListItem>
                                        <asp:ListItem>34</asp:ListItem>
                                        <asp:ListItem>35</asp:ListItem>
                                        <asp:ListItem>36</asp:ListItem>
                                        <asp:ListItem>37</asp:ListItem>
                                        <asp:ListItem>38</asp:ListItem>
                                        <asp:ListItem>39</asp:ListItem>
                                        <asp:ListItem>40</asp:ListItem>
                                        <asp:ListItem>41</asp:ListItem>
                                        <asp:ListItem>42</asp:ListItem>
                                        <asp:ListItem>43</asp:ListItem>
                                        <asp:ListItem>44</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                        <asp:ListItem>46</asp:ListItem>
                                        <asp:ListItem>47</asp:ListItem>
                                        <asp:ListItem>48</asp:ListItem>
                                        <asp:ListItem>49</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>51</asp:ListItem>
                                        <asp:ListItem>52</asp:ListItem>
                                        <asp:ListItem>53</asp:ListItem>
                                        <asp:ListItem>54</asp:ListItem>
                                        <asp:ListItem>55</asp:ListItem>
                                        <asp:ListItem>56</asp:ListItem>
                                        <asp:ListItem>57</asp:ListItem>
                                        <asp:ListItem>58</asp:ListItem>
                                        <asp:ListItem>59</asp:ListItem>
                                        <asp:ListItem>60</asp:ListItem>
                                    </asp:DropDownList>
                                    :
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator19"
                                     runat="server" ControlToValidate="ddlFirMin" InitialValue="0"
                                        ErrorMessage="Enter Action Minute" Display="None" ValidationGroup="AddTempGv" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:DropDownList ID="ddlFirAM" runat="server" ValidationGroup="AddTempGv" CssClass="simpletxt1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator20"
                                     runat="server" ControlToValidate="ddlFirAM" InitialValue="0"
                                        ErrorMessage="Enter Action AM/PM" Display="None" ValidationGroup="AddTempGv" >
                                    </asp:RequiredFieldValidator>--%>
                                </td>
                                <td align="right" visible="false" runat="server" id="tdVisitCharge">
                                    Visit Charges:
                                </td>
                                <td align="left" id="tdVisitAmount" runat="server">
                                    <asp:TextBox ValidationGroup="AddTempGv" ID="txtVisitCharges" Enabled="true" Visible="false"
                                        CssClass="txtboxtxt" MaxLength="20" runat="server" Width="80px">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr visible="false" runat="server" id="trCharges">
                                <td align="right">
                                    Spare Charges:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSpareCharges" Width="80px" MaxLength="20" CssClass="txtboxtxt"
                                        runat="server"></asp:TextBox>
                                </td>
                                <td align="right">
                                    Travel Charges:
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtTravelCharges" CssClass="txtboxtxt" MaxLength="20" runat="server"
                                        Width="80px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Mfg Unit:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ValidationGroup="AddTempGv" ID="ddlMfgUnit" CssClass="simpletxt1"
                                        Width="200px" runat="server" AppendDataBoundItems="false">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlMfgUnit" ValidationGroup="AddTempGv"
                                        runat="server" ErrorMessage="Select Manufacturing Unit" InitialValue="0" ControlToValidate="ddlMfgUnit"
                                        Display="None"></asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                              <tr id="tr1" runat="server">
                                                              <td>
                                                              Equipment Name
                                                              </td>
                                                              <td align="left">
                                                                 <asp:TextBox ID="txtequipname" runat="server" CssClass="txtboxtxt" 
                                                                      MaxLength="150"></asp:TextBox>
                                                              </td>
                                                              <td style="width: 230px" align="right">
                                                                  Coach No. :</td>
                                                              <td align="left">
                                                                <asp:TextBox ID="txtcoachNo" runat="server" CssClass="txtboxtxt" MaxLength="150" 
                                                                      Width="80px"></asp:TextBox>
                                                              </td>
                                                              </tr>
                                                             <tr id="tr2" runat="server">
                                                              <td>
                                                              Train No.
                                                              </td>
                                                              <td align="left">
                                                                <asp:TextBox ID="txtTrainNo" runat="server" CssClass="txtboxtxt" MaxLength="150"></asp:TextBox>
                                                              </td>
                                                              <td style="width: 230px" align="right">
                                                                  Availability of Coach /BLDC Fan at depot :
                                                              </td>
                                                              <td align="left">
                                                                <asp:TextBox ID="txtAvailblty" runat="server" CssClass="txtboxtxt" 
                                                                      MaxLength="150" Width="80px"></asp:TextBox>
                                                              </td>
                                                              </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Label ID="lblSave" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table width="99%" style="border: solid 1px #eeeeee;" visible="false" runat="server"
                            id="tbTempGrid">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnAdd" runat="server" Width="82px" Text="Save/Add More" ValidationGroup="AddTempGv"
                                        CssClass="btn" OnClick="btnAdd_Click" CausesValidation="true" />
                                    <asp:Button ID="btnFirClose" runat="server" Width="70px" Text="Close" CssClass="btn"
                                        OnClick="btnFirClose_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvAddTemp" runat="server" AllowPaging="True" DataKeyNames="Sno"
                                        AllowSorting="True" AlternatingRowStyle-CssClass="fieldName" AutoGenerateColumns="False"
                                        GridGroups="both" HeaderStyle-CssClass="fieldNamewithbgcolor" Width="100%" RowStyle-CssClass="gridbgcolor"
                                        OnRowDataBound="gvAddTemp_RowDataBound">
                                        <RowStyle CssClass="gridbgcolor" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Eval("Sno")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Complaint RefNo" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdngvCustomerID" Value='<%#Bind("CustomerID")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvComplaint_RefNo" Value='<%#Bind("Complaint_RefNo")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvSplitComplaint_RefNo" Value='<%#Bind("SplitComplaint_RefNo")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvComplaintDate" Value='<%#Bind("ComplaintDate")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvNatureOfComplaint" Value='<%#Bind("NatureOfComplaint")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvCallStatus" Value='<%#Bind("CallStatus")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvManufacture_SNo" Value='<%#Bind("Manufacture_SNo")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvDefectAccFlag" Value='<%#Bind("DefectAccFlag")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvProduct_SerialNo" Value='<%#Bind("Product_SerialNo")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvSLADate" runat="server" Value='<%#Eval("SLADate")%>' />
                                                    <asp:HiddenField ID="hdDateOfReporting" runat="server" Value='<%#Eval("DateOfReporting")%>' />
                                                    <%--  RSD Change          --%>                                                       
                                                        <asp:HiddenField ID="hdnequipname" Value='<%#Bind("AvailabilityDepot")%>' runat="server" />
                                                        <asp:HiddenField ID="hdncoachno" Value='<%#Bind("CoachNo")%>' runat="server" />
                                                        <asp:HiddenField ID="hdntrainno" Value='<%#Bind("TrainNo")%>' runat="server" />
                                                        <asp:HiddenField ID="hdnAvailblty" Value='<%#Bind("EquipmentName")%>' runat="server" />
                                                                    
                                                    <%#Eval("Complaint_RefNo")%>/<%#Eval("SplitComplaint_RefNo")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Division" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvProductDivision" runat="server" Text='<%#Eval("ProductDivision")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdngvProductDivisionSno" Value='<%#Bind("ProductDivisionSno")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Line" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvProductLine" runat="server" Text='<%#Eval("ProductLine")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdngvProductLineSno" Value='<%#Bind("ProductLineSno")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvProductGroupSno" Value='<%#Bind("ProductGroupSno")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvProduct" runat="server" Text='<%#Eval("Product")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdngvProductSno" Value='<%#Bind("ProductSno")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvInvoiceNo" Value='<%#Bind("InvoiceNo")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvInvoiceDate" Value='<%#Bind("InvoiceDate")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvActionTime" Value='<%#Bind("ActionTime")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvVisitCharges" Value='<%#Bind("VisitCharges")%>' runat="server" />
                                                    <asp:HiddenField ID="hdnSpareCharges" Value='<%#Bind("SpareCharges")%>' runat="server" />
                                                    <asp:HiddenField ID="hdnTravelCharges" Value='<%#Bind("TravelCharges")%>' runat="server" />
                                                    <asp:HiddenField ID="hdnDispatchDate" Value='<%#Bind("DispatchDate")%>' runat="server" />
                                                     <asp:HiddenField ID="hdnManufactureDate" Value='<%#Bind("ManufactureDate")%>' runat="server" />
                                                       <asp:HiddenField ID="hdnWarrantyExpirydate" Value='<%#Bind("WarrantyExpirydate")%>' runat="server" />
                                                    <asp:HiddenField ID="HiddenField1" Value='<%#Bind("SLADATE")%>' runat="server" />
                                                    <asp:HiddenField ID="hdnDateofCommission" Value='<%#Bind("DtofCommission")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Batch" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvBatch" runat="server" Text='<%#Eval("Batch")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Additional Info" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvAdditionalInfo" runat="server" Text='<%#Eval("AdditionalInfo")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Warranty Status" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvWarrantyStatus" runat="server" Text='<%#Eval("WarrantyStatus")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkTempGvDefect" Visible="false" OnClick="llnkTempGvDefect_Click"
                                                        runat="server" Text="Add Defect" CausesValidation="false" CommandArgument='<%#Eval("Complaint_RefNo")%>'
                                                        CommandName='<%#Eval("SplitComplaint_RefNo")%>'>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkTempGvViewDefect" Visible="false" OnClick="lnkTempGvViewDefect_Click"
                                                        runat="server" Text="View Defect" CausesValidation="false" CommandArgument='<%#Eval("Complaint_RefNo")%>'
                                                        CommandName='<%#Eval("SplitComplaint_RefNo")%>'>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkTempGvAction" Visible="false" OnClick="llnkTempGvAction_Click"
                                                        runat="server" CausesValidation="false" Text=" / Action" CommandArgument='<%#Eval("Complaint_RefNo")%>'
                                                        CommandName='<%#Eval("SplitComplaint_RefNo")%>'>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkTempGv" OnClick="llnkTempGv_Click" runat="server" Text="Remove"
                                                        CausesValidation="false" CommandArgument='<%#Eval("Sno")%>'>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                        <AlternatingRowStyle CssClass="fieldName" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSave" runat="server" Width="70px" Visible="false" Text="Save FIR"
                                        ValidationGroup="AddTempGv" CssClass="btn" OnClick="btnSave_Click" />
                                </td>
                            </tr>
                        </table>
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvCustDetail" runat="server" AllowPaging="true" AllowSorting="True"
                                        OnPageIndexChanging="gvCustDetail_PageIndexChanging" AlternatingRowStyle-CssClass="fieldName"
                                        AutoGenerateColumns="False" DataKeyNames="BaseLineId" GridGroups="both" HeaderStyle-CssClass="fieldNamewithbgcolor"
                                        Width="100%" RowStyle-CssClass="gridbgcolor" OnRowDataBound="gvCustDetail_RowDataBound">
                                        <RowStyle CssClass="gridbgcolor" />
                                        <Columns>
                                            <asp:BoundField DataField="SNo" HeaderText="Sno." />
                                            <asp:TemplateField HeaderText="Complaint RefNo" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdngvCustomerID" Value='<%#Bind("CustomerID")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvComplaint_RefNo" Value='<%#Bind("Complaint_RefNo")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvCallStatus" Value='<%#Bind("CallStatus")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvSplitComplaint_RefNo" Value='<%#Bind("SplitComplaint_RefNo")%>'
                                                        runat="server" />
                                                    <asp:HiddenField ID="hdngvComplaintDate" Value='<%#Bind("ComplaintDate")%>' runat="server" /><%--LoggedDate--%>
                                                    <asp:HiddenField ID="hdngvLastComment" Value='<%#Bind("LastComment")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvDefectAccFlag" Value='<%#Bind("DefectAccFlag")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvManufacture_SNo" Value='<%#Bind("Manufacture_SNo")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvWarrantyStatus" Value='<%#Bind("WarrantyStatus")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvSLADate" runat="server" Value='<%#Eval("SLADate")%>' />
                                                    <asp:HiddenField ID="hdngvProduct_SerialNo" Value='<%#Bind("ProductSerial_No")%>'
                                                        runat="server" />
                                                    <a href="Javascript:void(0);" onclick="funCommonPopUp(<%#Eval("BaseLineId")%>)">
                                                        <%#Eval("Complaint_RefNo")%>/<%#Eval("split")%></a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%#Eval("CustName")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Division" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvProductDivision" runat="server" Text='<%#Eval("Unit_Desc")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdngvProductDivisionSno" Value='<%#Bind("ProductDivision_Sno")%>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Line" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvProductLine" runat="server" Text='<%#Eval("ProductLine_Desc")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdngvProductLineSno" Value='<%#Bind("ProductLine_Sno")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvProductGroupSno" Value='<%#Bind("ProductGroup_SNo")%>'
                                                        runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvProduct" runat="server" Text='<%#Eval("Product_Desc")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdngvProductSno" Value='<%#Bind("Product_SNo")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvInvoiceNo" Value='<%#Bind("InvoiceNo")%>' runat="server" />
                                                    <asp:HiddenField ID="hdngvInvoiceDate" Value='<%#Bind("InvoiceDate")%>' runat="server" />
                                                    <%--<asp:HiddenField ID="hdngvWarrantyDate" Value='<%#Bind("WarrantyDate")%>' runat="server" />--%>
                                                    <asp:HiddenField ID="hdngvVisitCharges" Value='<%#Bind("VisitCharges")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Batch" Visible="false" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvBatch" runat="server" Text='<%#Eval("Batch_Code")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Warranty Status" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvWarrantyStatus" runat="server" Text='<%#Eval("WarrantyStatus")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkCustGvDefect" OnClick="lnkCustGvDefect_Click" runat="server"
                                                        Text="Add Defect " CausesValidation="false" CommandArgument='<%#Eval("Complaint_RefNo")%>'
                                                        CommandName='<%#Eval("SplitComplaint_RefNo")%>'>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCustGvViewDefect" OnClick="lnkCustGvViewDefect_Click" runat="server"
                                                        Visible="false" Text="View Defect " CausesValidation="false" CommandArgument='<%#Eval("Complaint_RefNo")%>'
                                                        CommandName='<%#Eval("SplitComplaint_RefNo")%>'>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="lnkCustGvAction" OnClick="lnkCustGvAction_Click" runat="server"
                                                        CausesValidation="false" Text="/ Action" CommandArgument='<%#Eval("Complaint_RefNo")%>'
                                                        CommandName='<%#Eval("SplitComplaint_RefNo")%>'>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                        <AlternatingRowStyle CssClass="fieldName" />
                                        <EmptyDataTemplate>
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center" style="padding-top: 50px; padding-bottom: 50px; padding-left: 60px;">
                                                        <b>Selected Complain is Unallocated</b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <asp:GridView ID="gvViewDefects" runat="server" AllowPaging="True" DataKeyNames="Sno"
                                        AllowSorting="True" AlternatingRowStyle-CssClass="fieldName" AutoGenerateColumns="False"
                                        GridGroups="both" HeaderStyle-CssClass="fieldNamewithbgcolor" Width="100%" RowStyle-CssClass="gridbgcolor">
                                        <RowStyle CssClass="gridbgcolor" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Eval("Sno")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Defect Category" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdngvDefectCategory_Sno" Value='<%#Bind("DefectCategory_Sno")%>'
                                                        runat="server" />
                                                    <asp:Label ID="lblgvDefectCategory" runat="server" Text='<%#Eval("DefectCategory")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Defect" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvDefect_Desc" runat="server" Text='<%#Eval("Defect_Desc")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdngvDefect_Sno" Value='<%#Bind("Defect_Sno")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Make Capacitor" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvMAKE_CAP" runat="server" Text='<%#Eval("MAKE_CAP")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtQty" runat="server" Text='<%#Bind("NUM_OF_DEF") %>' CssClass="txtboxtxt"
                                                        Width="40"></asp:Label>
                                                    <asp:HiddenField ID="hdnGvDefectSRNO" runat="server" Value='<%#Eval("SRNO") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                        <AlternatingRowStyle CssClass="fieldName" />
                                    </asp:GridView>
                                    <table id="tbViewAttribute" runat="server" width="100%" visible="false">
                                        <tr>
                                            <td>
                                                <tr>
                                                    <td colspan="4" height="20" align="left" valign="bottom" style="border-bottom: 1px solid #396870">
                                                        <b>Attribute Details</b>
                                                    </td>
                                                </tr>
                                                <tr id="trAttr_EquipmentName" runat="server" visible="false">
                                                    <td align="right" valign="top">
                                                       Name of Equipment:<font color="red">*</font>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtAttr_EquipmentName" runat="server" Enabled="false" Width="250px" CssClass="txtboxtxt"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="trAttr_CoachNo" runat="server" visible="false">
                                                    <td align="right" valign="top">
                                                        Coach Number:<font color="red">*</font>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtAttr_CoachNo" runat="server" Enabled="false" Width="250px" CssClass="txtboxtxt"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="trAttr_TrainNo" runat="server" visible="false">
                                                    <td align="right" valign="top">
                                                       Train Number:<font color="red">*</font>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtAttr_TrainNo" runat="server" Enabled="false" Width="250px"
                                                            CssClass="txtboxtxt"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="trAttr_Availability" runat="server" visible="false">
                                                    <td align="right" valign="top">
                                                        Availability of Coach/BLDC Fan at depot.:<font color="red">*</font>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtAttr_Availability" runat="server" Enabled="false" Width="250px" CssClass="txtboxtxt"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%" visible="false" runat="server" id="tbDefect" style="padding: 3px">
                            <tr>
                                <td colspan="4" height="20" align="left" valign="bottom" style="border-bottom: 1px solid #396870">
                                    <b>Defect Details</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="bottom" style="border-bottom: 1px solid #396870" colspan="4"
                                    height="20">
                                    <asp:UpdateProgress AssociatedUpdatePanelID="updateSC" ID="UpdateProgress3" runat="server">
                                        <ProgressTemplate>
                                            <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Complaint Refrence No.:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblDefComp" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Product Division:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblDefProductDiv" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Product Line:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblDefectProductLine" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField ID="hdnProductLineCode" runat="server" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Defect Category:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlDefectCat" CssClass="simpletxt1" Width="500px" runat="server"
                                        AppendDataBoundItems="True" AutoPostBack="True" ValidationGroup="SaveDefect"
                                        OnSelectedIndexChanged="ddlDefectCat_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RegularExpressionValidatorddlDefectCat" Display="None"
                                        InitialValue="0" ValidationGroup="SaveDefect" ControlToValidate="ddlDefectCat"
                                        runat="server" ErrorMessage="Select Defect Category">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Select Defect:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlDefect" CssClass="simpletxt1" Width="500px" ValidationGroup="SaveDefect"
                                        runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDefect_SelectedIndexChanged"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorddlDefect" Display="None" ValidationGroup="SaveDefect"
                                        ControlToValidate="ddlDefect" InitialValue="0" runat="server" ErrorMessage="Select Defect">
                                    </asp:RequiredFieldValidator>
                                    <asp:ValidationSummary ID="validAddDefect" runat="server" ValidationGroup="AddDefect"
                                        ShowSummary="false" ShowMessageBox="true" />
                                    <asp:Button ID="btnAddTempDefect" runat="server" Width="50px" Text="Add" OnClientClick="javascript:return CheckDefect();"
                                        CssClass="btn" OnClick="btnAddTempDefect_Click" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Defect No.:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDefectQty" Text="0" MaxLength="3" ReadOnly="true" runat="server"
                                        CssClass="txtboxtxt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ReqtxtDefectQty" runat="server" ValidationGroup="SaveDefect"
                                        ErrorMessage="Enter Defect Quantity" ControlToValidate="txtDefectQty" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator5" runat="server" ControlToValidate="txtDefectQty"
                                        Display="None" ErrorMessage="Enter Number Only in Quantity" ClientValidationFunction="validateDefectQty">
                                    </asp:CustomValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trDefectMake" runat="server" visible="false">
                                <td align="right" valign="top">
                                    Make Capacitor:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDefectMakeCapcitor" runat="server" CssClass="txtboxtxt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtDefectMakeCapcitor" Display="None"
                                        ControlToValidate="txtDefectMakeCapcitor" runat="server" ErrorMessage="Enter Make Capcitor"
                                        Enabled="false">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                    <asp:HiddenField ID="hdnDefectCustomerID" runat="server" />
                                    <asp:HiddenField ID="hdnDefectSliptComplaint" runat="server" />
                                    <asp:HiddenField ID="hdnDefectProductSrNo" runat="server" />
                                    <asp:HiddenField ID="hdnDefectComplaintRef_No" runat="server" />
                                    <asp:HiddenField ID="hdnDefectBatch" runat="server" />
                                    <asp:HiddenField ID="hdnDefectComplaintLoggDate" runat="server" />
                                    <asp:HiddenField ID="hdnDefectProductDiv_Sno" runat="server" />
                                    <asp:HiddenField ID="hdnDefectProductGroup_Sno" runat="server" />
                                    <asp:HiddenField ID="hdnDefectProductLine_Sno" runat="server" />
                                    <asp:HiddenField ID="hdnDefectCallStatus" runat="server" />
                                    <asp:HiddenField ID="hdnDefectProduct_Sno" runat="server" />
                                    <asp:HiddenField ID="hdnDefectProductCode" runat="server" />
                                    <asp:HiddenField ID="hdnDefectMFGUNIT" runat="server" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:GridView ID="gvDefectTemp" runat="server" AllowPaging="True" DataKeyNames="Sno"
                                        AllowSorting="True" AlternatingRowStyle-CssClass="fieldName" AutoGenerateColumns="False"
                                        GridGroups="both" HeaderStyle-CssClass="fieldNamewithbgcolor" Width="100%" RowStyle-CssClass="gridbgcolor">
                                        <RowStyle CssClass="gridbgcolor" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Eval("Sno")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Defect Category" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdngvDefectCategory_Sno" Value='<%#Bind("DefectCategory_Sno")%>'
                                                        runat="server" />
                                                    <asp:Label ID="lblgvDefectCategory" runat="server" Text='<%#Eval("DefectCategory")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Defect" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvDefect_Desc" runat="server" Text='<%#Eval("Defect_Desc")%>'></asp:Label>
                                                    <asp:HiddenField ID="hdngvDefect_Sno" Value='<%#Bind("Defect_Sno")%>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Make Capacitor" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgvMAKE_CAP" runat="server" Text='<%#Eval("MAKE_CAP")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="txtQty" runat="server" Text='<%#Bind("NUM_OF_DEF") %>' CssClass="txtboxtxt"
                                                        Width="40"></asp:Label>
                                                    <asp:HiddenField ID="hdnGvDefectSRNO" runat="server" Value='<%#Eval("SRNO") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDefectGv" OnClick="lnkDefectGv_Click" CommandName='<%#Eval("SRNO")%>'
                                                        runat="server" Text="Remove" CommandArgument='<%#Eval("Sno")%>'>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="fieldNamewithbgcolor" />
                                        <AlternatingRowStyle CssClass="fieldName" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" height="20" align="left" valign="bottom" style="border-bottom: 1px solid #396870">
                                    <b>Attribute Details</b>
                                </td>
                            </tr>
                            <tr id="trAttEquipmentName" runat="server" visible="false">
                                <td align="right" valign="top">
                                    Name of Equipment:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAttEquipmentName" runat="server" Width="250px" CssClass="txtboxtxt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEquipName" runat="server" ControlToValidate="txtAttEquipmentName"
                                        Display="None" ErrorMessage="Enter Equipment Name" ValidationGroup="SaveDefect"> </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trAttCoachNo" runat="server" visible="false">
                                <td align="right" valign="top">
                                    Coach Number:<font color="red">*</font></td>
                                <td align="left">
                                    <asp:TextBox ID="txtAttCoachNo" runat="server" Width="250px" CssClass="txtboxtxt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCoach" runat="server"
                                        ControlToValidate="txtAttCoachNo" Display="None" ErrorMessage="Enter Coach Number" ValidationGroup="SaveDefect"> </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trAttTrainNo" runat="server" visible="false">
                                <td align="right" valign="top">
                                   Train Number:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAttTrainNo" runat="server" Width="250px" CssClass="txtboxtxt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvTrain" runat="server"
                                        ControlToValidate="txtAttTrainNo" Display="None" ErrorMessage="Enter Train Number"
                                        ValidationGroup="SaveDefect"> </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr id="trAttAvailability" runat="server" visible="false">
                                <td align="right" valign="top">
                                    Availability of Coach/BLDC Fan at depot.:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAttAvailability" runat="server" Width="250px" CssClass="txtboxtxt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAvailability" runat="server"
                                        ControlToValidate="txtAttAvailability" Display="None" ErrorMessage="Enter Availability of Coach/BLDC Fan at depot."
                                        ValidationGroup="SaveDefect"> </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Action Remarks:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDefectServiceActionRemarks" Text="" ValidationGroup="SaveDefect" MaxLength="250" 
                                        runat="server" Width="250px" Height="40" CssClass="txtboxtxt"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtDefectServiceActionRemarks"
                                        Display="None" ErrorMessage="Enter Defect Action remarks" ValidationGroup="SaveDefect"> </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    Defect Entry Date:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDefectDate" ValidationGroup="SaveDefect" runat="server" CssClass="txtboxtxt"
                                        Height="16px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtDefectDate"
                                        Display="None" ErrorMessage="Enter Defect Entry Date" ValidationGroup="SaveDefect">
                                    </asp:RequiredFieldValidator>
                                    <cc1:CalendarExtender ID="CalendarExtender5" TargetControlID="txtDefectDate" runat="server">
                                    </cc1:CalendarExtender>
                                    <asp:CompareValidator Operator="DataTypeCheck" Type="Date" ControlToValidate="txtDefectDate"
                                        Display="None" ErrorMessage="Not a vaild Date" runat="server" ID="CompareValidator2"
                                        ValidationGroup="SaveDefect">
                                    </asp:CompareValidator>
                                    <asp:DropDownList ID="ddlDefHr" runat="server" ValidationGroup="SaveDefect" CssClass="simpletxt1">
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>
                                    :
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator21"
                                     runat="server" ControlToValidate="ddlDefHr" InitialValue="0"
                                        ErrorMessage="Enter Action Hour" Display="None" ValidationGroup="SaveDefect" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:DropDownList ID="ddlDefMin" runat="server" ValidationGroup="SaveDefect" CssClass="simpletxt1">
                                        <asp:ListItem Value="0">00</asp:ListItem>
                                        <asp:ListItem Value="1">01</asp:ListItem>
                                        <asp:ListItem Value="2">02</asp:ListItem>
                                        <asp:ListItem Value="3">03</asp:ListItem>
                                        <asp:ListItem Value="4">04</asp:ListItem>
                                        <asp:ListItem Value="5">05</asp:ListItem>
                                        <asp:ListItem Value="6">06</asp:ListItem>
                                        <asp:ListItem Value="7">07</asp:ListItem>
                                        <asp:ListItem Value="8">08</asp:ListItem>
                                        <asp:ListItem Value="9">09</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                        <asp:ListItem>24</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>26</asp:ListItem>
                                        <asp:ListItem>27</asp:ListItem>
                                        <asp:ListItem>28</asp:ListItem>
                                        <asp:ListItem>29</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>31</asp:ListItem>
                                        <asp:ListItem>32</asp:ListItem>
                                        <asp:ListItem>33</asp:ListItem>
                                        <asp:ListItem>34</asp:ListItem>
                                        <asp:ListItem>35</asp:ListItem>
                                        <asp:ListItem>36</asp:ListItem>
                                        <asp:ListItem>37</asp:ListItem>
                                        <asp:ListItem>38</asp:ListItem>
                                        <asp:ListItem>39</asp:ListItem>
                                        <asp:ListItem>40</asp:ListItem>
                                        <asp:ListItem>41</asp:ListItem>
                                        <asp:ListItem>42</asp:ListItem>
                                        <asp:ListItem>43</asp:ListItem>
                                        <asp:ListItem>44</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                        <asp:ListItem>46</asp:ListItem>
                                        <asp:ListItem>47</asp:ListItem>
                                        <asp:ListItem>48</asp:ListItem>
                                        <asp:ListItem>49</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>51</asp:ListItem>
                                        <asp:ListItem>52</asp:ListItem>
                                        <asp:ListItem>53</asp:ListItem>
                                        <asp:ListItem>54</asp:ListItem>
                                        <asp:ListItem>55</asp:ListItem>
                                        <asp:ListItem>56</asp:ListItem>
                                        <asp:ListItem>57</asp:ListItem>
                                        <asp:ListItem>58</asp:ListItem>
                                        <asp:ListItem>59</asp:ListItem>
                                        <asp:ListItem>60</asp:ListItem>
                                    </asp:DropDownList>
                                    :
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator22"
                                     runat="server" ControlToValidate="ddlDefMin" InitialValue="0"
                                        ErrorMessage="Enter Action Minute" Display="None" ValidationGroup="SaveDefect" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:DropDownList ID="ddlDefAM" runat="server" ValidationGroup="SaveDefect" CssClass="simpletxt1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator23"
                                     runat="server" ControlToValidate="ddlDefAM" InitialValue="0"
                                        ErrorMessage="Enter Action AM/PM" Display="None" ValidationGroup="SaveDefect" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="SaveDefect"
                                        ShowMessageBox="True" ShowSummary="False" />
                                    <asp:CustomValidator ValidationGroup="SaveDefect" ID="CustomValidator3" runat="server"
                                        ControlToValidate="txtDefectDate" Display="None" ErrorMessage="Defect Date cannot be less then two days from current date"
                                        ClientValidationFunction="validateDefectDate">
                                    </asp:CustomValidator>
                                    <asp:HiddenField ID="hdnDefectCurrentDate" runat="server" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Label ID="lblDefectMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Button ID="btnSaveDefect" runat="server" Width="70px" Text="Save" CssClass="btn"
                                        OnClick="btnSaveDefect_Click" ValidationGroup="SaveDefect" />
                                    <asp:Button ID="btnDefectApprove" runat="server" Width="70px" Text="Approve" CssClass="btn"
                                        ValidationGroup="SaveDefect" OnClick="btnDefectApprove_Click" />
                                    <asp:Button ID="btnDefectClose" runat="server" Width="70px" Text="Close" CssClass="btn"
                                        OnClick="btnDefectClose_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="99%" visible="false" runat="server" id="tbAction" style="padding: 3px">
                            <tr>
                                <td colspan="4" height="20" align="left" valign="bottom" style="border-bottom: 1px solid #396870">
                                    <b>Service Action Entry</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="bottom" style="border-bottom: 1px solid #396870" colspan="4"
                                    height="20">
                                    <asp:UpdateProgress AssociatedUpdatePanelID="updateSC" ID="UpdateProgress4" runat="server">
                                        <ProgressTemplate>
                                            <img src="<%=ConfigurationManager.AppSettings["AjaxLoadingImageName"]%>" alt="" />
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%" align="right">
                                    Product Division:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblActionProductDiv" runat="server" Text="">
                                    </asp:Label><asp:HiddenField ID="hdnActionSplitNo" runat="server" />
                                    <asp:HiddenField ID="hdnActionCallStatusID" runat="server" />
                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                    <asp:HiddenField ID="hdnActionWarrantyStatus" runat="server" />
                                    <asp:HiddenField ID="hdnActionSLADate" runat="server" />
                                </td>
                            </tr>
                            <%--<tr>
                                <td style="width: 30%" align="right">
                                   Customer Ref No.:
                                </td>
                                <td>
                                    <asp:Label ID="lblCustRefNo" Visible="false" runat="server" Text="">
                                    </asp:Label>
                                   
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="width: 30%" align="right">
                                    Complaint Ref No:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblActionComplaintRefNo" runat="server" Text="">
                                    </asp:Label>
                                </td>
                                <td align="right">
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%" align="right">
                                    Product:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblActionProduct" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="trbatch" runat="server" visible="false">
                                <td style="width: 30%" align="right">
                                    Batch No:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblActionBatch" runat="server" Text="">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%" align="right">
                                    Warranty Status:
                                </td>
                                <td align="left">
                                    <asp:Label ID="lblActionWarranty" runat="server" Text="">
                                    </asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top" style="width: 30%">
                                    Action:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlActionStatus" runat="server" CssClass="simpletxt1" Width="500px"
                                        AppendDataBoundItems="false" OnSelectedIndexChanged="ddlActionStatus_SelectedIndexChanged"
                                        AutoPostBack="True">
                                        <asp:ListItem Value="0">Select</asp:ListItem>
                                        <asp:ListItem Value="50">(WIP)Repair at Division</asp:ListItem>
                                        <%--<asp:ListItem Value="51">(WIP)FIR Done</asp:ListItem>--%>
                                        <asp:ListItem Value="52">(WIP)Repair In Progress at Site</asp:ListItem>
                                        <asp:ListItem Value="53">(WIP)Waiting for Spares</asp:ListItem>
                                        <%--<asp:ListItem Value="54">(WIP)Defect Approved</asp:ListItem>--%>
                                        <asp:ListItem Value="55">(WIP)Pending for replacement</asp:ListItem>
                                        <asp:ListItem Value="75">(WIP)Complaint disposed by temporary arrangement</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlActionStatus"
                                        InitialValue="0" Display="None" ErrorMessage="Select Action" ValidationGroup="SaveAction">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trSpare" runat="server" visible="false">
                                <td colspan="2" align="center">
                                    <a href="Javascript:void(0);" onclick="funSpare()">Click Here To Add Spare</a>
                                </td>
                            </tr>
                            <tr id="trServiceDate" runat="server" visible="false">
                                <td align="right">
                                    Service Invoice Date:
                                    <asp:Label ID="lblStarServiceDate" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtServiceDate" Width="175" CssClass="txtboxtxt" runat="server">
                                    </asp:TextBox>
                                    <cc1:CalendarExtender ID="CalendarExtender7" TargetControlID="txtServiceDate" runat="server">
                                    </cc1:CalendarExtender>
                                    <asp:RequiredFieldValidator ID="rqftxtServiceDate" ControlToValidate="txtServiceDate"
                                        runat="server" ErrorMessage="Enter Service Invoice Date" ValidationGroup="SaveAction"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trServiceNumber" runat="server" visible="false">
                                <td align="right">
                                    Service Invoice Number:
                                    <asp:Label ID="lblStarServiceNumber" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtServiceNumber" Width="175" MaxLength="15" CssClass="txtboxtxt"
                                        runat="server">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rqftxtServiceNumber" ControlToValidate="txtServiceNumber"
                                        runat="server" ErrorMessage="Enter Service Invoice Number" ValidationGroup="SaveAction"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trServiceAmount" runat="server" visible="false">
                                <td align="right">
                                    Service Amount:
                                    <asp:Label ID="lblStarServiceAmount" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtServiceAmount" Width="175" MaxLength="10" CssClass="txtboxtxt"
                                        runat="server">
                                    </asp:TextBox><%--onkeypress="javascript:return checkNumberOnly(event);"--%>
                                    <asp:RangeValidator ID="regValidateAmount" runat="server" ControlToValidate="txtServiceAmount"
                                        ValidationGroup="SaveAction" Display="None" ErrorMessage="Not a Valid Amount"
                                        Type="Double"></asp:RangeValidator>
                                    <asp:RequiredFieldValidator ID="rqftxtServiceAmount" ControlToValidate="txtServiceAmount"
                                        runat="server" ErrorMessage="Enter Service Amount" ValidationGroup="SaveAction"
                                        Display="None">
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%" align="right">
                                    Action Details:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtActionDetails" Width="250" Text="" Height="40" CssClass="txtboxtxt"
                                        runat="server">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtActionDetails"
                                        Display="None" ErrorMessage="Enter Action Remarks" ValidationGroup="SaveAction">
                                    </asp:RequiredFieldValidator>
                                </td>
                                <td>
                                </td>
                                <td align="right">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%" align="right">
                                    Action Date:<font color="red">*</font>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtActionEntryDate" CssClass="txtboxtxt" runat="server">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtActionEntryDate"
                                        Display="None" ErrorMessage="Enter Action Date" ValidationGroup="SaveAction">
                                    </asp:RequiredFieldValidator>
                                    <cc1:CalendarExtender ID="CalendarExtender6" TargetControlID="txtActionEntryDate"
                                        runat="server">
                                    </cc1:CalendarExtender>
                                    <asp:CompareValidator Operator="DataTypeCheck" Type="Date" ControlToValidate="txtActionEntryDate"
                                        Display="None" ErrorMessage="Not a vaild Date" runat="server" ID="CompareValidator3"
                                        ValidationGroup="SaveAction">
                                    </asp:CompareValidator>
                                    <asp:DropDownList ID="ddlActHr" runat="server" ValidationGroup="SaveAction" CssClass="simpletxt1">
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>
                                    :
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator24"
                                     runat="server" ControlToValidate="ddlActHr" InitialValue="0"
                                        ErrorMessage="Enter Action Hour" Display="None" ValidationGroup="SaveAction" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:DropDownList ID="ddlActMin" runat="server" ValidationGroup="SaveAction" CssClass="simpletxt1">
                                        <asp:ListItem Value="0">00</asp:ListItem>
                                        <asp:ListItem Value="1">01</asp:ListItem>
                                        <asp:ListItem Value="2">02</asp:ListItem>
                                        <asp:ListItem Value="3">03</asp:ListItem>
                                        <asp:ListItem Value="4">04</asp:ListItem>
                                        <asp:ListItem Value="5">05</asp:ListItem>
                                        <asp:ListItem Value="6">06</asp:ListItem>
                                        <asp:ListItem Value="7">07</asp:ListItem>
                                        <asp:ListItem Value="8">08</asp:ListItem>
                                        <asp:ListItem Value="9">09</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                        <asp:ListItem>24</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>26</asp:ListItem>
                                        <asp:ListItem>27</asp:ListItem>
                                        <asp:ListItem>28</asp:ListItem>
                                        <asp:ListItem>29</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>31</asp:ListItem>
                                        <asp:ListItem>32</asp:ListItem>
                                        <asp:ListItem>33</asp:ListItem>
                                        <asp:ListItem>34</asp:ListItem>
                                        <asp:ListItem>35</asp:ListItem>
                                        <asp:ListItem>36</asp:ListItem>
                                        <asp:ListItem>37</asp:ListItem>
                                        <asp:ListItem>38</asp:ListItem>
                                        <asp:ListItem>39</asp:ListItem>
                                        <asp:ListItem>40</asp:ListItem>
                                        <asp:ListItem>41</asp:ListItem>
                                        <asp:ListItem>42</asp:ListItem>
                                        <asp:ListItem>43</asp:ListItem>
                                        <asp:ListItem>44</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                        <asp:ListItem>46</asp:ListItem>
                                        <asp:ListItem>47</asp:ListItem>
                                        <asp:ListItem>48</asp:ListItem>
                                        <asp:ListItem>49</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>51</asp:ListItem>
                                        <asp:ListItem>52</asp:ListItem>
                                        <asp:ListItem>53</asp:ListItem>
                                        <asp:ListItem>54</asp:ListItem>
                                        <asp:ListItem>55</asp:ListItem>
                                        <asp:ListItem>56</asp:ListItem>
                                        <asp:ListItem>57</asp:ListItem>
                                        <asp:ListItem>58</asp:ListItem>
                                        <asp:ListItem>59</asp:ListItem>
                                        <asp:ListItem>60</asp:ListItem>
                                    </asp:DropDownList>
                                    :
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator25"
                                     runat="server" ControlToValidate="ddlActMin" InitialValue="0"
                                        ErrorMessage="Enter Action Minute" Display="None" ValidationGroup="SaveAction" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:DropDownList ID="ddlActAM" runat="server" ValidationGroup="SaveAction" CssClass="simpletxt1">
                                        <asp:ListItem>AM</asp:ListItem>
                                        <asp:ListItem>PM</asp:ListItem>
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator26"
                                     runat="server" ControlToValidate="ddlActAM" InitialValue="0"
                                        ErrorMessage="Enter Action AM/PM" Display="None" ValidationGroup="SaveAction" >
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="SaveAction"
                                        ShowMessageBox="True" ShowSummary="False" />
                                    <asp:CustomValidator ValidationGroup="SaveAction" ID="CustomValidator4" runat="server"
                                        ControlToValidate="txtActionEntryDate" Display="None" ErrorMessage="Action Date cannot be less then two days from current date"
                                        ClientValidationFunction="validateActionDate">
                                    </asp:CustomValidator>
                                    <asp:HiddenField ID="hdnActionCurrentDate" runat="server" />
                                </td>
                                <td>
                                </td>
                                <td align="right">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4">
                                    <asp:Label ID="lblActionMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                    <asp:Button ID="btnSaveAction" runat="server" Width="70px" Text="Save" ValidationGroup="SaveAction"
                                        CssClass="btn" OnClick="btnSaveAction_Click" />
                                    <asp:Button ID="btnCloseAction" runat="server" Width="70px" Text="Close" CssClass="btn"
                                        OnClick="btnCloseAction_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
