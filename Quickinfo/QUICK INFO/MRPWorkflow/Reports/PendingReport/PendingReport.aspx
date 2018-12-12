<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PendingReport.aspx.cs" Inherits="PendingReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pending Report</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnExport" runat="server" Text="Export To Excel" OnClick="ExportToExcel" />
            <asp:GridView ID="grdPendings" runat="server">
            </asp:GridView>
        </div>
    </form>
</body>
</html>
