<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MRPBankEmailDetails.aspx.cs" Inherits="MRPWorkflow_MRPBankEmailDetails" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="z-index: 103; left: 0px; position: absolute; top: -52px">
        <asp:ScriptManager id="ScriptManager1" runat="server">
        </asp:ScriptManager><tr>
            <td style="height: 248px">
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" BackColor="Gainsboro" BorderColor="#C0C0FF"
                                BorderWidth="1px" ForeColor="Gainsboro" Height="220px" Style="z-index: 100" Width="900px">
                                <table style="width: 741px; height: 203px;">
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; width: 176px;">
                                            <asp:Label ID="Label2" runat="server" ForeColor="Gray" Style="text-align: left" Text="Bank Code"
                                                Width="111px"></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtBankCode" runat="server" CssClass="input" TabIndex="200"
                                                Width="300px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBankCode"
                                                ErrorMessage="Cannot be empty" ValidationGroup="INSERT"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtBankCode"
                                                ErrorMessage="Cannot be empty" ValidationGroup="UPDATE"></asp:RequiredFieldValidator></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; width: 176px;">
                                            <asp:Label ID="Label1" runat="server" ForeColor="Gray" Style="text-align: left" Text="Hnb Email"
                                                Width="111px"></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtHNBEmail" runat="server" CssClass="input" TabIndex="200" Width="300px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtHNBEmail"
                                                ErrorMessage="Cannot be empty" ValidationGroup="INSERT"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtHNBEmail"
                                                ErrorMessage="Cannot be empty" ValidationGroup="UPDATE"></asp:RequiredFieldValidator></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; width: 176px;">
                                            <asp:Label ID="Label4" runat="server" ForeColor="Gray" Style="text-align: left" Text="Bank Assurance Email"
                                                Width="165px"></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtBanAssuranceEmail" runat="server" CssClass="input" TabIndex="200" Width="300px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtBanAssuranceEmail"
                                                ErrorMessage="Cannot be empty" ValidationGroup="INSERT"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtBanAssuranceEmail"
                                                ErrorMessage="Cannot be empty" ValidationGroup="UPDATE"></asp:RequiredFieldValidator></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; width: 176px;">
                                            <asp:Label ID="Label5" runat="server" ForeColor="Gray" Style="text-align: left" Text="HNBA Branch Code"
                                                Width="169px"></asp:Label>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtHNBABranchCode" runat="server" CssClass="input" TabIndex="200" Width="300px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtHNBABranchCode"
                                                ErrorMessage="Cannot be empty" ValidationGroup="INSERT"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtHNBABranchCode"
                                                ErrorMessage="Cannot be empty" ValidationGroup="UPDATE"></asp:RequiredFieldValidator></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 176px; text-align: left">
                                            <asp:Label ID="Label3" runat="server" ForeColor="Gray" Style="text-align: left" Text="HNBA Email"
                                                Width="169px"></asp:Label></td>
                                        <td style="text-align: left">
                                            <asp:TextBox ID="txtHNBAEmail" runat="server" CssClass="input" TabIndex="200" Width="300px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtHNBAEmail"
                                                ErrorMessage="Cannot be empty" ValidationGroup="INSERT"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtHNBAEmail"
                                                ErrorMessage="Cannot be empty" ValidationGroup="UPDATE"></asp:RequiredFieldValidator></td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 176px">
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Button ID="btnInsert" runat="server" CssClass="button" TabIndex="206" Text="Add"
                                                Width="75px" OnClick="btnInsert_Click" ValidationGroup="INSERT" />
                                            <asp:Button ID="btnUpdate" runat="server" CssClass="button" TabIndex="206" Text="Update"
                                                Width="75px" Enabled="False" OnClick="btnUpdate_Click" ValidationGroup="UPDATE" />
                                            <asp:Button ID="btnClear" runat="server" CssClass="button" OnClick="btnClear_Click"
                                                TabIndex="207" Text="Clear" Width="75px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlPolicyGrid" runat="server" BorderColor="#C0C0FF" BorderWidth="1px"
                    Height="613px" ScrollBars="Auto" Style="z-index: 102" Width="1043px">
                    <asp:GridView ID="grdEmailList" runat="server" BackColor="White" BorderColor="#CCCCCC"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="8pt" OnSelectedIndexChanged="grdEmailList_SelectedIndexChanged" Style="z-index: 102"
                        Width="99%">
                        <RowStyle BackColor="White" ForeColor="Black" Height="15px" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                        </Columns>
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Tahoma" Font-Size="Larger"
                            ForeColor="White" Height="20px" />
                        <AlternatingRowStyle BackColor="WhiteSmoke" Font-Names="Tahoma" Font-Size="8pt" Height="15px" />
                    </asp:GridView>
                </asp:Panel>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <div align="left" class="listunlinkstyle">
                    &nbsp;</div>
            </td>
        </tr>
    </table>
</asp:Content>

