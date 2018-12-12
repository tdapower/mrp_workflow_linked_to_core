<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ReInsurance.aspx.cs" Inherits="ReInsurance" Title="Re Insurance" %>

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
                    BorderWidth="1px" ForeColor="Gainsboro" Height="150px" Style="z-index: 100;"
                    Width="489px">
                    <table>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" ForeColor="Gray" Style="text-align: left" Text="HNBA Ref. No."
                                    Width="111px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchHNBARefNo" runat="server" Width="189px" TabIndex="200"
                                    CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label17" runat="server" ForeColor="Gray" Style="text-align: left"
                                    Text="Department" Width="111px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSearchDepartment" runat="server" Style="z-index: 109;" Width="188px"
                                    TabIndex="205" CssClass="dropDown">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" ForeColor="Gray" Style="text-align: left" Text="Proposal No."
                                    Width="111px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchProposalNo" runat="server" Width="189px" TabIndex="200"
                                    CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label16" runat="server" ForeColor="Gray" Style="text-align: left"
                                    Text="Re-Insurer" Width="111px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSearchReInsurer" runat="server" Style="z-index: 109;" Width="188px"
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
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" TabIndex="206"
                                    Text="Search" Width="75px" CssClass="button" />
                                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" TabIndex="207"
                                    Text="Clear" Width="75px" CssClass="button" />
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
                <asp:Panel ID="pnlSearchResult" runat="server" Height="113px" ScrollBars="Vertical"
                    Style="z-index: 102;" Width="1043px" BorderColor="#C0C0FF" BorderWidth="1px">
                    <asp:GridView ID="grdSearchResult" runat="server" BackColor="White" BorderColor="#CCCCCC"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="8pt" Style="z-index: 102;" Width="995px" OnSelectedIndexChanged="grdSearchResult_SelectedIndexChanged"
                        OnRowDataBound="grdSearchResult_RowDataBound">
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
                    <asp:Panel ID="Panel3" runat="server" Height="422px" Style="z-index: 104;" Width="1038px"
                        BorderColor="#C0C0FF" BorderWidth="1px">
                        &nbsp; &nbsp;&nbsp;
                        <table style="width: 464px; height: 100px">
                            <tr>
                                <td colspan="4">
                                    <asp:TextBox ID="txtSeqId" runat="server" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="HNBA Ref. No." Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHNBARefNo" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="250px" TabIndex="6" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" ForeColor="Gray" Style="text-align: left" Text="Department"
                                        Width="150px"></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label19" runat="server" ForeColor="Gray" Style="text-align: left"
                                        Text="Sending Reason" Width="150px"></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlSendingReason" runat="server" CssClass="dropDown" Style="z-index: 109;"
                                        TabIndex="205" Width="188px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Date of Sending" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDateOfSending" AutoCompleteType="None" Width="75px"
                                        AutoPostBack="false" />
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDateOfSending"
                                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                        ErrorTooltipEnabled="True" />
                                    <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                        ControlToValidate="txtDateOfSending" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid"
                                        Display="Dynamic" TooltipMessage="" EmptyValueBlurredText="Date is invalid" InvalidValueBlurredMessage="Date is invalid"
                                        ValidationGroup="MKE" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderDateOfSending" runat="server" TargetControlID="txtDateOfSending"
                                        CssClass="MyCalendar" Format="dd/MM/yyyy" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Proposal No." Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProposalNo" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnLoadDataFromProposalNo" runat="server" CssClass="button" OnClick="btnLoadDataFromProposalNo_Click"
                                        TabIndex="25" Text="Load" Width="200px" />
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Life Assure 1 Name" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLifeAssure1Name" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Life Assure 2 Name" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLifeAssure2Name" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Life Assure 1 NIC" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLifeAssure1NIC" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Life Assure 2 NIC" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLifeAssure2NIC" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Policy No." Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPolicyNo" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Sum Insured" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSumInsured" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label12" runat="server" ForeColor="Gray" Style="text-align: left"
                                        Text="Re-Insurer" Width="150px"></asp:Label>
                                </td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlReInsurer" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="RI Ref. No." Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRIRefNo" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="250px" TabIndex="6" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="RI Decision" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRIDecision" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="250px" TabIndex="6" CssClass="input" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label15" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Date of Decision" Width="150px"></asp:Label>
                                </td>
                                <td>
                              
                                    <asp:TextBox runat="server" ID="txtDateOfDecision" AutoCompleteType="None" Width="75px"
                                        AutoPostBack="false" />
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtDateOfDecision"
                                        Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                        OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                        ErrorTooltipEnabled="True" />
                                    <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MaskedEditExtender1"
                                        ControlToValidate="txtDateOfDecision" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid"
                                        Display="Dynamic" TooltipMessage="" EmptyValueBlurredText="Date is invalid" InvalidValueBlurredMessage="Date is invalid"
                                        ValidationGroup="MKE" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtenderDateOfDecision" runat="server"
                                        TargetControlID="txtDateOfDecision" CssClass="MyCalendar" Format="dd/MM/yyyy" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
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
