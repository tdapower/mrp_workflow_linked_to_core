<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageMedicalPayments.aspx.cs"
    Inherits="ManageMedicalPayments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Medical Payments</title>

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

    <script type="text/javascript">
                function checkDate(sender, args) {
                sender._selectedDate.setHours(0,0,0,0);
            if (sender._selectedDate < new Date().setHours(0,0,0,0)) {
                alert("You cannot select a day earlier than today!");
                sender._selectedDate = new Date();
                // set the date back to the current date
               sender._textbox.set_Value(sender._selectedDate.format(sender._format))
               
               sender._textbox.set_Value('')
                        }

        }
    </script>

    <link id="Link1" runat="server" rel="stylesheet" type="text/css" ie:href="~/default.css"
        href="~/Styles/StyleSheet.css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <asp:Panel ID="pnlMedical" runat="server" BackColor="Gainsboro" BorderColor="#C0C0FF"
                BorderWidth="1px" ForeColor="black" Style="z-index: 100;" Width="100%">
                <table>
                    <tr>
                        <td>
                            <div align="left" class="listunlinkstyle">
                                &nbsp; &nbsp;&nbsp;
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtMedicalPaymentID" runat="server" Visible="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label18" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                                Text="Proposal No." Width="200px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProposalNo" runat="server" MaxLength="250" Style="z-index: 105;"
                                                Width="250px" TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlSearchResults" runat="server" ScrollBars="Vertical" Style="z-index: 102;"
                                                Width="1043px" BorderColor="#C0C0FF" BorderWidth="1px">
                                                <asp:GridView ID="grdSearchResults" runat="server" BackColor="White" BorderColor="#CCCCCC"
                                                    BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                                                    Font-Size="8pt" Style="z-index: 102;" Width="995px" OnSelectedIndexChanged="grdSearchResults_SelectedIndexChanged"
                                                    OnRowDataBound="grdSearchResults_RowDataBound">
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
                                            <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Font-Size="9pt" Text="Bill Received Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBillReceivedDate" Width="75px" ReadOnly="True" />
                                            <asp:Button ID="btnAddBillReceivedDate" runat="server" TabIndex="24" Text="Add Date"
                                                Width="100px" CssClass="button" OnClick="btnAddBillReceivedDate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Font-Size="9pt" Text="Payment Voucher Sent Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPaymentVoucherSentDate" Width="75px" ReadOnly="True" />
                                            <asp:Button ID="btnAddPaymentVoucherSentDate" runat="server" TabIndex="24" Text="Add Date"
                                                Width="100px" CssClass="button" OnClick="btnAddPaymentVoucherSentDate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Font-Size="9pt" Text="Cheque Received Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtChequeReceivedDate" Width="75px" ReadOnly="True" />
                                            <asp:Button ID="btnAddChequeReceivedDate" runat="server" TabIndex="24" Text="Add Date"
                                                Width="100px" CssClass="button" OnClick="btnAddChequeReceivedDate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                                Text="Cheque No." Width="150px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChequeNo" runat="server" MaxLength="250" Style="z-index: 105;"
                                                Width="184px" TabIndex="6" CssClass="input"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Font-Size="9pt" Text="Mailed Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtMailedDate" Width="75px" ReadOnly="True" />
                                            <asp:Button ID="btnAddMailedDate" runat="server" TabIndex="24" Text="Add Date" Width="100px"
                                                CssClass="button" OnClick="btnAddMailedDate_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                                Text="Payment Mode" Width="150px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPaymentMode" runat="server" Style="z-index: 109;" Width="150px"
                                                TabIndex="205" CssClass="dropDown" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label9" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                                Text="Remarks" Width="150px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRemarks" runat="server" MaxLength="250" Style="z-index: 105;"
                                                Width="184px" TabIndex="6" CssClass="input" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="background-color: rgb(138, 138, 138);">
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Font-Names="Verdana" Font-Size="9pt" Text="Payment Details"></asp:Label>
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                            Style="z-index: 106;" Text="Medical Lab"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlMedicalLab" runat="server" Style="z-index: 109;" Width="388px"
                                                            TabIndex="205" CssClass="dropDown" AutoPostBack="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label11" runat="server" Font-Names="Verdana" Font-Size="9pt" Text="Paid Amount"
                                                            Width="88px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPaidAmount" runat="server" MaxLength="250" Style="z-index: 105;"
                                                            Width="184px" TabIndex="6" CssClass="input"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnAddToMedicalPaymentsGrid" runat="server" Text="Add" OnClick="btnAddToMedicalPaymentsGrid_Click"
                                                            CssClass="button" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:GridView ID="grdMedicalPayments" runat="server" HorizontalAlign="Center" Width="100%"
                                                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                                                            CellPadding="3" Font-Bold="False" Font-Names="Tahoma" Font-Size="8pt">
                                                            <RowStyle ForeColor="Black" BackColor="White" Height="15px" />
                                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                            <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Tahoma" Font-Size="Larger"
                                                                ForeColor="White" Height="20px" />
                                                            <AlternatingRowStyle BackColor="WhiteSmoke" Font-Names="Tahoma" Font-Size="8pt" Height="15px" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Button ID="btnNew" runat="server" TabIndex="24" Text="New" Width="100px" OnClick="btnNew_Click"
                                                CssClass="button" />
                                            <asp:Button ID="btnAlter" runat="server" TabIndex="24" Text="Alter" Width="100px"
                                                OnClick="btnAlter_Click" CssClass="button" />
                                            <asp:Button ID="btnSave" runat="server" TabIndex="24" Text="Save" Width="100px" OnClick="btnSave_Click"
                                                CssClass="button" />
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
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
