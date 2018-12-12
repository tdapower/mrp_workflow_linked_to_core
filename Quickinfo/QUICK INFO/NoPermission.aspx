<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NoPermission.aspx.cs" Inherits="NoPermission" Title="No Permission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table style="z-index: 112; left: 1px; width: 1024px; top: 146px; border: 4px; border-color: Red;">
        <tr align="center" style="width: 100%">
            <td>
                <asp:Label ID="lblError1" runat="server" Font-Bold="true" Font-Names="Arial" Font-Size="10pt"
                    ForeColor="Red">Sorry, You don't Have Privileges to access this page. Please Contact Your system Administrator........!</asp:Label>
            </td>
        </tr>
        <tr align="center" style="width: 100%">
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/warning.gif" />
            </td>
        </tr>
    </table>
</asp:Content>
