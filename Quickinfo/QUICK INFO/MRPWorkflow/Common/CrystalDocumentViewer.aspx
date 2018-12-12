<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrystalDocumentViewer.aspx.cs"
    Inherits="MRPWorkflow_Common_CrystalDocumentViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <CR:CrystalReportViewer ID="LetterViewer" runat="server" AutoDataBind="True" DisplayGroupTree="False"
                Height="799px" Width="1095px" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False"
                ReuseParameterValuesOnRefresh="True"></CR:CrystalReportViewer>
        </div>
    </form>
</body>
</html>
