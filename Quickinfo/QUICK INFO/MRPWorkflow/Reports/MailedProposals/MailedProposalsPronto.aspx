<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MailedProposalsPronto.aspx.cs" Inherits="MailedProposalsPronto" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>MRP Mail Register Report</title>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
            DisplayGroupTree="False" Height="1039px" OnInit="CrystalReportViewer1_Init" ReportSourceID="CrystalReportSource1"
            Width="773px" />
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="MailedProposalsPronto.rpt">
            </Report>
        </CR:CrystalReportSource>
    
    </div>
    </form>
</body>
</html>