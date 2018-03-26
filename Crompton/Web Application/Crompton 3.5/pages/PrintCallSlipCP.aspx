﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintCallSlipCP.aspx.cs" Inherits="pages_PrintCallSlipCP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Print Call Slip</title>
    <link href="../css/CallSlip.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">

 .page {
    width: 21cm!important;
    min-height: 28cm!important; 
    background: white;
    position:relative;
    overflow: visible !important;    
    }
    .page1 {
        max-height:28cm!important;
    }

</style>

    <script language="javascript" type="text/javascript">
    
    function fnOpenCommPopUp()
    {
      funCommLog("" + document.getElementById('hdnComNo').value + "",document.getElementById('hdnBaseLineId').value);
    }
    function fnOpenCommPopUp()
    {
      funCommLog("" + document.getElementById('hdnComNo').value + "",document.getElementById('hdnSplitNo').value);
    }
    function fnOpenHistPopUp()
    {
      funHistoryLog("" + document.getElementById('hdnComNo').value + "",document.getElementById('hdnSplitNo').value);
    }
    
    function fnOpenViewDefectPopUp()
    {
        funDefectEntry("" + document.getElementById('hdnComNo').value + "",document.getElementById('hdnSplitNo').value);
    }
    </script>
<script type="text/javascript" language="javascript" src="../scripts/Common.js"></script>
<script type="text/javascript">
</script>
</head>
<body onload="popup();">
    <form id="form1" runat="server" >
    <div id="Divmsg" runat="server"  style="padding-top:20px; text-align:center; font-size:13px; font-weight:bold">
        <asp:Label ID="lblmsg" runat="server">
         Sorry! Yor are not authorized for CP division blank call slip.
        </asp:Label>
         <a href="" onclick="window.history.back()">Back To Home</a>
    </div>
    
    <div id="DivContent" runat="server">
    <div>
    <%=strBody.ToString() %>
    </div>  
    <div style="height: 10px">
    </div>
    <div>
        <table width="680" align="center" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="4" style="padding: 40px; font-size: 12px">
                    <p align="center">
                        <u> <br /><br /><br /><br /><b>Terms and Conditions</b></u></p>
                    <p>
                        &nbsp;</p>
                    <ul style="list-style: decimal; line-height: 25px">
                        <li>An equipment or accessory received for repairs vide this Job Sheet will only be
                            delivered to the customer against production of the customer’s copy (first copy
                            of this jobsheet).</li>
                        <li>Equipment tampered with or misused or mishandled or subjected to voltage higher
                            than the rated voltage will not be covered under warranty.</li>
                        <li>Estimate for repairs to an equipment, received vide this jobsheet, will only be
                            provided to the customer on request.</li>
                        <li>In case the estimate for repairs to a product is not approved 50% of the standard
                            labour charges will be levied.</li>
                        <li>An advance of 50% of the estimated repair cost will be taken before commencing actual
                            repairs to the equipment.</li>
                        <li>The Labour Charges for tampered equipment will be two times the standard charges.</li>
                        <li>Defective material, replaced in warranty jobs, will be retained by Crompton Greaves
                            Ltd. and not be returned to the customer.</li>
                        <li>Repaired equipment not collected by the customer for over 4 days from the date of
                            intimation will invite a storage charge of Rs. 50/ per day (applicable for workshop
                            jobs). Further, equipment not collected for 30 days from the date of intimation
                            for delivery will be disposed off at the customer’s risk.</li>
                        <li>Utmost care will be taken to have the equipment repaired and kept ready for delivery
                            by the due date. However, in case of delay due to non availability of spare parts
                            or other reasons beyond our control we shall not be responsible for any consequential
                            losses or damages.</li>
                        <li>We shall not be responsible for any losses or damages caused to the equipment due
                            to force majeure conditions or riots.</li>
                        <li>Defective parts of out of warranty jobs are destroyed and accordingly not returned.
                            However, if required by the customer, the same may be requested at the time of delivery
                            of the equipment to the service center and be collected.</li>
                        <li>The equipment repaired vide this jobsheet (except for tampered and misused equipment)
                            is guaranteed for 1 month for labour charges.</li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
     </div>
    </form>
    <link href="../css/CallSlipIE.css" rel="Stylesheet" type="text/css" />
   
</body>
</html>

<script language="javascript" type="text/javascript">

    function Printfile()
    {
        window.print();
    }
    
</script>

