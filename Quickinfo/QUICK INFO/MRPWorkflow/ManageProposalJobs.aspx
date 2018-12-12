<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageProposalJobs.aspx.cs" Inherits="ManageProposalJobs" Title="Manage Proposal Jobs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
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
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" BackColor="Gainsboro" BorderColor="#C0C0FF"
                                BorderWidth="1px" ForeColor="Gainsboro" Height="200px" Style="z-index: 100;"
                                Width="350px">
                                <table>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" ForeColor="Gray" Style="text-align: left" Text="Job Number"
                                                Width="111px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchJobNumber" runat="server" Width="189px" TabIndex="200"
                                                CssClass="input"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" ForeColor="Gray" Style="text-align: left" Text="NIC 1"
                                                Width="111px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchNIC1" runat="server" Width="189px" TabIndex="200" CssClass="input"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" ForeColor="Gray" Style="text-align: left" Text="NIC 2"
                                                Width="111px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchNIC2" runat="server" Width="189px" TabIndex="200" CssClass="input"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" ForeColor="Gray" Style="text-align: left" Text="Assigned To"
                                                Width="111px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSearchAssignedTo" runat="server" Style="z-index: 109;" Width="188px"
                                                TabIndex="205" CssClass="dropDown">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label46" runat="server" ForeColor="Gray" meta:resourcekey="Label1Resource1"
                                                Style="text-align: left" Text="Start Date." Width="111px"></asp:Label></td>
                                        <td style="text-align: left">
                                            <asp:TextBox runat="server" ID="txtfromDate" AutoCompleteType="None" Width="75px"
                                                AutoPostBack="false" />
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtfromDate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                                ControlToValidate="txtfromDate" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid"
                                                Display="Dynamic" TooltipMessage="" EmptyValueBlurredText="Date is invalid" InvalidValueBlurredMessage="Date is invalid"
                                                ValidationGroup="MKE" />
                                            <ajaxToolkit:CalendarExtender ID="txtfromDateExtender" runat="server" TargetControlID="txtfromDate"
                                                CssClass="MyCalendar" Format="dd/MM/yyyy" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="Label47" runat="server" ForeColor="Gray" meta:resourcekey="Label1Resource1"
                                                Text="End Date." Width="111px"></asp:Label></td>
                                        <td style="text-align: left">
                                            <asp:TextBox runat="server" ID="txtToDate" AutoCompleteType="None" Width="75px" AutoPostBack="false" />
                                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtToDate"
                                                Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                                OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                                ErrorTooltipEnabled="True" />
                                            <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MaskedEditExtender2"
                                                ControlToValidate="txtToDate" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid"
                                                Display="Dynamic" TooltipMessage="" EmptyValueBlurredText="Date is invalid" InvalidValueBlurredMessage="Date is invalid"
                                                ValidationGroup="MKE" />
                                            <ajaxToolkit:CalendarExtender ID="txtToDateExtender" runat="server" TargetControlID="txtToDate"
                                                CssClass="MyCalendar" Format="dd/MM/yyyy" />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
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
                        <td>
                            <asp:Panel ID="Panel2" runat="server" BackColor="Gainsboro" BorderColor="#C0C0FF"
                                BorderWidth="1px" ForeColor="Gainsboro" Height="200px" Style="z-index: 100;"
                                Width="489px" ScrollBars="none" Visible="false">
                                <asp:GridView ID="grdJobSummary" runat="server" BackColor="White" BorderColor="#CCCCCC"
                                    BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                                    Font-Size="8pt" Style="z-index: 102;" Width="500px">
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
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
                </table>
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
                <asp:Panel ID="pnlProposalJobsGrid" runat="server" Height="113px" ScrollBars="Vertical"
                    Style="z-index: 102;" Width="1043px" BorderColor="#C0C0FF" BorderWidth="1px">
                    <asp:GridView ID="grdProposalJobs" runat="server" BackColor="White" BorderColor="#CCCCCC"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="8pt" Style="z-index: 102;" Width="995px" OnSelectedIndexChanged="grdProposalJobs_SelectedIndexChanged"
                        OnRowDataBound="grdProposalJobs_RowDataBound">
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
                    <asp:Panel ID="Panel3" runat="server" Height="350px" Style="z-index: 104;" Width="1038px"
                        BorderColor="#C0C0FF" BorderWidth="1px">
                        &nbsp; &nbsp;&nbsp;
                        <table style="width: 700px; height: 100px">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblJobStatus" runat="server" Font-Bold="true" Font-Names="Franklin Gothic Book"
                                        Font-Size="14pt" ForeColor="Red" Style="z-index: 101;" Height="22px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                        Text="Job Number" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtJobNumber" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="250px" TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                        Text="Proposal Number" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtProposalNumber" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                        Text="NIC 1" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNIC1" runat="server" MaxLength="250" Style="z-index: 105;" Width="184px"
                                        TabIndex="6" CssClass="input" AutoPostBack="True" OnTextChanged="txtNIC1_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                        Text="NIC 2" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNIC2" runat="server" MaxLength="250" Style="z-index: 105;" Width="184px"
                                        TabIndex="6" CssClass="input" AutoPostBack="True" OnTextChanged="txtNIC2_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                        Text="Bank" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBankType" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown" OnSelectedIndexChanged="ddlBankType_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnRefreshBanks" runat="server" TabIndex="22" Text="Refresh Banks"
                                        Width="97px" CssClass="button" OnClick="btnRefreshBanks_Click" Enabled="false" />
                                    <asp:Button ID="BtnCreateBanks" runat="server" TabIndex="22" Text="Update Email"
                                        Width="97px" CssClass="button" OnClick="BtnCreateBanks_Click" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                        Text="Bank Name" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBankName" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown" OnSelectedIndexChanged="ddlBankName_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                        Text="Branch" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBranch" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                        Text="Assigned To" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAssignedTo" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="Label12" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106"
                                        Text="Broker Code" Width="150px"></asp:Label></td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlBrokerCode" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList></td>
                            </tr>
                            
                               <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="Label14" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106"
                                        Text="Mode of Proposal" Width="150px"></asp:Label></td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlModeOfProposal" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="Label15" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106"
                                        Text="Business Channel" Width="150px"></asp:Label></td>
                                <td style="text-align: left">
                                    <asp:DropDownList ID="ddlBusinessChannel" runat="server" CssClass="dropDown" Style="z-index: 109;"
                                        TabIndex="205" Width="188px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="Label13" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106"
                                        Text="Fast Track Job" Width="150px"></asp:Label></td>
                                <td style="text-align: left">
                                    <asp:CheckBox ID="chkIsFastTrack" runat="server" OnCheckedChanged="chkIsFastTrack_CheckedChanged"
                                        AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: left">
                                    <asp:Label ID="Label16" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106"
                                        Text="Is Free Cover Limit Proposal" Width="180px"></asp:Label></td>
                                <td style="text-align: left">
                                    <asp:CheckBox ID="chkIsFreeCoverLimitProposal" runat="server"/>
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:Button ID="btnAddNew" runat="server" TabIndex="22" Text="Add New" Width="100px"
                                        OnClick="btnAddNew_Click" CssClass="button" />
                                    <asp:Button ID="btnAlter" runat="server" TabIndex="23" Text="Alter" Width="100px"
                                        OnClick="btnAlter_Click" CssClass="button" />
                                    <asp:Button ID="btnSave" runat="server" TabIndex="24" Text="Save" Width="100px" OnClick="btnSave_Click"
                                        CssClass="button" />
                                    <asp:Button ID="btnCancelJob" runat="server" TabIndex="25" Text="Cancel Job" Width="100px"
                                        OnClick="btnCancelJob_Click" CssClass="button" />
                                    <asp:Button ID="btnCancel" runat="server" TabIndex="25" Text="Cancel" Width="100px"
                                        OnClick="btnCancel_Click" CssClass="button" />
                                    <asp:Button ID="btnRemoveFST" runat="server" TabIndex="23" Text="Remove From FST" Width="150px"
                                        OnClick="btnRemoveFST_Click" CssClass="button" />
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
