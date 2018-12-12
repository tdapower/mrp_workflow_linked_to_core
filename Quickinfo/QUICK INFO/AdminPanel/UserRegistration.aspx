<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="UserRegistration.aspx.cs" Inherits="UserRegistration" Title="User Registration" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
    function jsValidateNum(obj)
	{
	    if(isNaN(obj.value))
	    {	        
	        alert('Numeric Expected');
	        obj.value =''
	        obj.focus()
	    }
	}
    </script>

    <table style="z-index: 103; left: 0px; position: absolute; top: -54px">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BackColor="Gainsboro" BorderColor="#C0C0FF"
                    BorderWidth="1px" ForeColor="Gainsboro" Style="z-index: 100;" Width="489px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" ForeColor="Gray" Style="text-align: left" Text="Company"
                                    Width="111px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSearchCompany" runat="server" Style="z-index: 109;" Width="188px"
                                    TabIndex="205" CssClass="dropDown">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" ForeColor="Gray" Style="text-align: left" Text="User Code"
                                    Width="111px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchUserCode" runat="server" Width="189px" TabIndex="200" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" ForeColor="Gray" Style="text-align: left" Text="User Name"
                                    Width="111px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchUserName" runat="server" Width="189px" TabIndex="200" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" ForeColor="Gray" Style="text-align: left" Text="EPF"
                                    Width="111px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchEPF" runat="server" Width="189px" TabIndex="201" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" ForeColor="Gray" Style="text-align: left" Text="User Role"
                                    Width="112px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSearchUserRole" runat="server" Style="z-index: 109;" Width="188px"
                                    TabIndex="205" CssClass="dropDown">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSearch1" runat="server" OnClick="btnSearch_Click" TabIndex="206"
                                    Text="Search" Width="75px" CssClass="button" />
                                <asp:Button ID="btnClear1" runat="server" OnClick="btnClear_Click" TabIndex="207"
                                    Text="Clear" Width="75px" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblError1" runat="server" Font-Bold="False" Font-Names="Franklin Gothic Book"
                    Font-Size="11pt" ForeColor="Red" Style="z-index: 101;" Width="1039px" Height="22px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlUserGrid" runat="server" Height="113px" ScrollBars="Vertical" Style="z-index: 102;"
                    Width="1043px" BorderColor="#C0C0FF" BorderWidth="1px">
                    <asp:GridView ID="grdUsers" runat="server" BackColor="White" BorderColor="#CCCCCC"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="8pt" Style="z-index: 102;" Width="995px" OnSelectedIndexChanged="grdUsers_SelectedIndexChanged"
                        OnRowDataBound="grdUsers_RowDataBound">
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                        </Columns>
                        <RowStyle ForeColor="Black" BackColor="White" Height="15px" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Tahoma" Font-Size="Larger"
                            ForeColor="White" Height="20px" />
                        <AlternatingRowStyle BackColor="WhiteSmoke" Font-Names="Tahoma" Font-Size="8pt" Height="15px" />
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <div align="left" class="listunlinkstyle">
                    <asp:Panel ID="Panel3" runat="server" Height="222px" Style="z-index: 104;" Width="1038px"
                        BorderColor="#C0C0FF" BorderWidth="1px">
                        &nbsp; &nbsp;&nbsp;
                        <table style="width: 464px; height: 138px">
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="User Code" Width="97px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserCode" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="User Name" Width="97px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Company" Width="97px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCompany" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="EPF" Width="88px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEPF" runat="server" Style="z-index: 108;" Width="184px" TabIndex="8"
                                        MaxLength="6" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="e-mail" Width="88px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="input" MaxLength="6" Style="z-index: 108;"
                                        TabIndex="8" Width="184px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 111;" Text="User Role" Width="78px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlUserRole" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label35" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Height="16px" Style="z-index: 131;" Text="Status" Width="30px"></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rdbtnActive" runat="server" GroupName="grpStatus" Text="Active"
                                        Checked="true" />
                                    <asp:RadioButton ID="rdbtnInActive" runat="server" GroupName="grpStatus" Text="In-Active" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btnAddNew" runat="server" TabIndex="22" Text="AddNew" Width="100px"
                                        OnClick="btnAddNew_Click" CssClass="button" />
                                    <asp:Button ID="btnAlter" runat="server" TabIndex="23" Text="Alter" Width="100px"
                                        OnClick="btnAlter_Click" CssClass="button" />
                                    <asp:Button ID="btnSave" runat="server" TabIndex="24" Text="Save" Width="100px" OnClick="btnSave_Click"
                                        CssClass="button" />
                                    <asp:Button ID="btnCancel" runat="server" TabIndex="25" Text="Cancel" Width="100px"
                                        OnClick="btnCancel_Click" CssClass="button" />
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <contenttemplate>
                                            <asp:Label ID="lblMsg" runat="server" Font-Bold="False" Font-Names="Franklin Gothic Book"
                                                Font-Size="11pt" ForeColor="Red" Style="z-index: 101;" Width="1039px" Height="22px"></asp:Label>
                                            <asp:Timer ID="Timer1" runat="server" Enabled="False" OnTick="Timer1_Tick">
                                            </asp:Timer>
                                        </contenttemplate>
                                        <triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Timer1" />
                                        </triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
