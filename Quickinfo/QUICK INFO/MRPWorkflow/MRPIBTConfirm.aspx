<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MRPIBTConfirm.aspx.cs" Inherits="MRPDetails" Title="Quickinfo - MRP/MCR IBT Confirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server" BackColor="Gainsboro" Height="150px" Style="z-index: 100;
        left: 0px; position: absolute; top: -47px" Width="430px">
        <asp:TextBox ID="txtSeaProposalNo" runat="server" BorderColor="#C0C0FF" BorderWidth="1px"
            Style="z-index: 100; left: 84px; position: absolute; top: 28px" Width="176px"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" Style="z-index: 101; left: 32px; position: absolute;
            top: 48px; text-align: left" Text="Product" Width="45px"></asp:Label>
        <asp:DropDownList ID="cboSearchStatus" runat="server" AutoPostBack="True" Font-Names="Verdana"
            Style="z-index: 102; left: 85px; position: absolute; top: 49px" Width="181px">
            <asp:ListItem>-</asp:ListItem>
            <asp:ListItem>MRP</asp:ListItem>
            <asp:ListItem>MCR</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label6" runat="server" Style="z-index: 103; left: 32px; position: absolute;
            top: 72px; text-align: left" Text="Reason" Width="45px"></asp:Label>
        <asp:DropDownList ID="cboReason" runat="server" AutoPostBack="True" Font-Names="Verdana"
            Style="z-index: 104; left: 85px; position: absolute; top: 73px" Width="181px">
            <asp:ListItem>-</asp:ListItem>
            <asp:ListItem>NEW PROPOSAL</asp:ListItem>
            <asp:ListItem>RENEWAL</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label7" runat="server" Style="z-index: 105; left: 12px; position: absolute;
            top: 96px; text-align: left" Text="Policy Year" Width="69px"></asp:Label>
        <asp:DropDownList ID="cboPolicyYear" runat="server" AutoPostBack="True" Font-Names="Verdana"
            Style="z-index: 106; left: 85px; position: absolute; top: 97px" Width="181px">
            <asp:ListItem>-</asp:ListItem>
            <asp:ListItem>2011</asp:ListItem>
            <asp:ListItem>2012</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="Label1" runat="server" ForeColor="Gray" Style="z-index: 107; left: 28px;
            position: absolute; top: 29px; text-align: left" Text="To Date." Width="53px"></asp:Label>
        <asp:Button ID="btnImport" runat="server" BorderColor="White" BorderWidth="1px" ForeColor="White"
            Height="20px" Style="font-weight: normal; font-size: 12px;
            z-index: 108; left: 188px; font-family: Tahoma; position: absolute; top: 125px;
            background-color: #6699cc" TabIndex="7" Text="Import" Width="75px" OnClick="btnImport_Click" />
        &nbsp;
        <asp:Button ID="btnClear" runat="server" BorderColor="White" BorderWidth="1px" ForeColor="White"
            Height="20px" Style="font-weight: normal; font-size: 12px;
            z-index: 110; left: 268px; font-family: Tahoma; position: absolute; top: 125px;
            background-color: #6699cc" TabIndex="7" Text="Clear" Width="75px" OnClick="btnClear_Click" />
        <asp:TextBox ID="txtSeaPol_no" runat="server" BorderColor="#C0C0FF" BorderWidth="1px"
            Style="z-index: 111; left: 84px; position: absolute; top: 8px" Width="176px"></asp:TextBox>
        <asp:Label ID="Label2" runat="server" ForeColor="Gray" Style="z-index: 112; left: 12px;
            position: absolute; top: 9px; text-align: left" Text="From Date." Width="68px"></asp:Label>
        <asp:Label ID="Label4" runat="server" ForeColor="Red" Style="z-index: 113; left: 266px;
            position: absolute; top: 10px; text-align: left" Text="DD/MM/YYYY" Width="68px"></asp:Label>
        <asp:Label ID="Label5" runat="server" ForeColor="Red" Style="z-index: 114; left: 266px;
            position: absolute; top: 31px; text-align: left" Text="DD/MM/YYYY" Width="68px"></asp:Label>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Height="28px" Style="z-index: 115;
            left: 435px; position: absolute; top: 120px; text-align: left" Width="602px"></asp:Label>
        <asp:Button ID="btnSearch1" runat="server" BorderColor="White" BorderWidth="1px"
            Font-Bold="False" ForeColor="White" Height="20px"
            Style="font-weight: normal; font-size: 12px; z-index: 116; left: 108px; font-family: Tahoma;
            position: absolute; top: 125px; background-color: #6699cc" TabIndex="7" Text="Search"
            Width="75px" OnClick="btnSearch1_Click" />
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" BorderColor="#C0C0FF" BorderWidth="1px" Height="495px"
        ScrollBars="Vertical" Style="z-index: 101; left: 2px; position: absolute; top: 109px"
        Width="1043px">
        <asp:GridView ID="grdNewBAC" runat="server" BackColor="White" BorderColor="#CCCCCC"
            BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="8pt" Style="z-index: 100;
            left: 5px; position: absolute; top: 6px" Width="1016px" OnSelectedIndexChanged="grdNewBAC_SelectedIndexChanged1">
            <RowStyle BackColor="White" ForeColor="Black" Height="20px" />
            <Columns>
                <asp:TemplateField >
                    <ItemTemplate>
                        <asp:CheckBox ID="myCheckBox" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Tahoma" Font-Size="8pt"
                ForeColor="White" Height="20px" />
            <AlternatingRowStyle BackColor="Gainsboro" Font-Names="Tahoma" Font-Size="8pt" Height="20px" />
        </asp:GridView>
        <asp:Panel ID="Panel3" runat="server" Style="z-index: 102; left: 449px; position: absolute;
            top: -96px">
        </asp:Panel>
        &nbsp;&nbsp;

        <script type="text/javascript">
                function uppercase()
                {
                  key = window.event.keyCode;
                  if ((key > 0x60) && (key < 0x7B))
                  window.event.keyCode = key-0x20;
                }
                
                function NumberFilter(e)
                {  /*Grabbing the unicode value of the key that was pressed*/
                    var unicode;
                    try
                    {   /*IE*/
                        unicode = e.keyCode; 
                    }
                    catch(err)
                    {   
                        try
                        { /*Netscape, Mozilla, FireFox...*/
                             unicode = event.keyCode;
                        }
                        catch(error)
                        {  /*Other*/
                            unicode = e.which;
                         }
                     }
                    /*if the value entered is not a unicode value between 48 and 57 return false*/
                    if(unicode < 48 || unicode > 57)
                    {  
                      return false;
                    } 
                    return true;
                }
        </script>

    </asp:Panel>
    <asp:Panel ID="pnlmessage" runat="server" BackColor="Gainsboro" Height="72px" Style="z-index: 103;
        left: 257px; position: absolute; top: 160px" Width="478px">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Height="35px" Style="z-index: 115;
            left: 9px; position: absolute; top: 7px; text-align: left" Width="462px"></asp:Label>
        <asp:Button ID="btnmsgYes" runat="server" BorderColor="White" BorderWidth="1px" ForeColor="White"
            Height="20px" Style="font-weight: normal; font-size: 12px;
            z-index: 108; left: 145px; font-family: Tahoma; position: absolute; top: 46px;
            background-color: #6699cc" TabIndex="7" Text="Yes" Width="75px" OnClick="btnmsgYes_Click" />
        <asp:Button ID="btnmsgNo" runat="server" BorderColor="White" BorderWidth="1px" ForeColor="White"
            Height="20px" Style="font-weight: normal; font-size: 12px;
            z-index: 110; left: 225px; font-family: Tahoma; position: absolute; top: 46px;
            background-color: #6699cc" TabIndex="7" Text="No" Width="75px" OnClick="btnmsgNo_Click" />
    </asp:Panel>
</asp:Content>

