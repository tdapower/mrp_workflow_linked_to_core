<%@ Page Language="C#" MasterPageFile="../MasterPage.master" AutoEventWireup="true"
    CodeFile="EmailHeader.aspx.cs" Inherits="SubMenuSetup" Title="Config E-Mail Header" %>

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
                    BorderWidth="1px" ForeColor="Gainsboro" Height="120px" Style="z-index: 100;"
                    Width="600px">
                    <table>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" ForeColor="Gray" Style="text-align: left" Text="Main Menu Name"
                                    Width="111px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSearchMainMenu" runat="server" Style="z-index: 109;" Width="328px"
                                    TabIndex="205"   CssClass="dropDown">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 103px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td style="width: 103px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align=right>
                                <asp:Button ID="btnSearch1" runat="server" OnClick="btnSearch_Click" TabIndex="206"
                                    Text="Search" Width="75px" Height="25px" CssClass="button" />
                                <asp:Button ID="btnClear1" runat="server" OnClick="btnClear_Click" TabIndex="207"
                                    Text="Clear" Width="75px" Height="25px" CssClass="button" /></td>
                            <td style="width: 103px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblError" runat="server" Font-Bold="False" Font-Names="Franklin Gothic Book"
                    Font-Size="11pt" ForeColor="Red" Style="z-index: 101;" Width="1039px" Height="22px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="pnlSubMenuGrid" runat="server" Height="113px" ScrollBars="Vertical"
                    Style="z-index: 102;" Width="1043px" BorderColor="#C0C0FF" BorderWidth="1px">
                    <asp:GridView ID="grdSubMenus" runat="server" BackColor="White" BorderColor="#CCCCCC"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="8pt" Style="z-index: 102;" Width="995px" OnSelectedIndexChanged="grdSubMenus_SelectedIndexChanged"
                        OnRowDataBound="grdSubMenus_RowDataBound">
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
                        <table style="width: 464px; height: 100px">
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtSubMenuCode" runat="server" Visible="false"></asp:TextBox>
                                    <asp:Label ID="lblSortId" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106" Text="Status" Visible="False" Width="150px"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="E-Mail Desc" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMainMenu" runat="server" Style="z-index: 109;" Width="500px"
                                        TabIndex="205"   CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 22px">
                                    <asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="E-Mail Header" Width="150px"></asp:Label>
                                </td>
                                <td style="height: 22px">
                                    <asp:TextBox ID="txtSubMenuName" runat="server"
                                         MaxLength="250" Style="z-index: 105;"
                                        Width="209px" TabIndex="6"  CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Status" Width="150px"></asp:Label>
                                </td>
                                <td><asp:DropDownList ID="cboStatus" runat="server" Style="z-index: 109;" Width="73px"
                                        TabIndex="205"   CssClass="dropDown">
                                    <asp:ListItem>0</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                </asp:DropDownList></td>
                                <td>
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
                                        <ContentTemplate>
                                            <asp:Label ID="lblMsg" runat="server" Font-Bold="False" Font-Names="Franklin Gothic Book"
                                                Font-Size="11pt" ForeColor="Red" Style="z-index: 101;" Width="1039px" Height="22px"></asp:Label>
                                            <asp:Timer ID="Timer1" runat="server" Enabled="False" OnTick="Timer1_Tick">
                                            </asp:Timer>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="Timer1" />
                                        </Triggers>
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
