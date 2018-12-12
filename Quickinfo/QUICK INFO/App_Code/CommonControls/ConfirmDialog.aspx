<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfirmDialog.aspx.cs" Inherits="CommonControls_ConfirmDialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="300px">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblMessage" runat="server" Text="Message Here"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width:150px;">
                    </td>
                    <td>
                        <asp:Button ID="btn1" runat="server" Text="Button1" OnClick="btn1_Click" />
                        <asp:Button ID="btn2" runat="server" Text="Button2" OnClick="btn2_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
