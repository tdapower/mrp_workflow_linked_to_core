<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TotalBencmarkReport.aspx.cs"
    Inherits="TotalBencmarkReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
   .grdPendingsBenchmark th  {
   background-color:#79d4ff
   }
    
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 300px">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Year"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlYears" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnLoadData" runat="server" Text="Load Data" OnClick="btnLoadData_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Pending letters" Style="font-size: large;
                font-weight: bolder"></asp:Label>
            <asp:GridView ID="grdPendingsBenchmark" runat="server" Style="background-color: #79d4ff;">
            </asp:GridView>
            <asp:Label ID="Label3" runat="server" Text="Cover Notes" Style="font-size: large;
                font-weight: bolder;"></asp:Label>
            <asp:GridView ID="grdCoverBenchmark" runat="server" Style="background-color: #fff579;">
            </asp:GridView>
            <asp:Label ID="Label4" runat="server" Text="Policy issuance" Style="font-size: large;
                font-weight: bolder;"></asp:Label>
            <asp:GridView ID="grdPolicyBenchmark" runat="server" Style="background-color: #79ff90;">
            </asp:GridView>
            <asp:Label ID="Label5" runat="server" Text="Individual weighted Average" Style="font-size: large;
                font-weight: bolder;"></asp:Label>
            <asp:GridView ID="grdSummary" runat="server" Style="background-color: #ea79ff;">
            </asp:GridView>
        </div>
    </form>
</body>
</html>
