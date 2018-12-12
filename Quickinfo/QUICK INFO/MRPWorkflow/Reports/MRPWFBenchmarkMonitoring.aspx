<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="MRPWFBenchmarkMonitoring.aspx.cs" Inherits="MRPWFBenchmarkMonitoring"
    Title="MRP Workflow Reports" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="Panel1" runat="server" BackColor="Gainsboro" BorderColor="#C0C0FF"
        BorderStyle="Solid" BorderWidth="1px" Height="534px" Style="z-index: 100; left: 2px;
        position: absolute; top: -51px" Width="999px">
        <asp:Label ID="Label9" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Gray"
            meta:resourcekey="Label1Resource1" Style="z-index: 104; left: 26px; position: absolute;
            top: 11px; text-align: left" Text="Major Criteria." Width="131px"></asp:Label>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
        <asp:Label ID="lblError" runat="server" Style="z-index: 101; left: 28px; position: absolute;
            top: -16px" Width="904px" Font-Names="Verdana" Font-Size="8pt" ForeColor="Red"></asp:Label>
        &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
        &nbsp; &nbsp; &nbsp;&nbsp;
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Panel ID="Panel3" runat="server" BackColor="White" Height="317px" Style="z-index: 102;
            left: 22px; position: absolute; top: 30px" Width="431px" BorderColor="#C0C0FF"
            BorderWidth="1px">
            <table style="width: 484px; height: 94px">
                <tr>
                    <td style="width: 2px; height: 5px; text-align: left">
                    </td>
                    <td style="width: 133px; height: 5px; text-align: left">
                    </td>
                </tr>
                <tr>
                    <td style="width: 2px; height: 30px; text-align: left">
                        &nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Gray"
                            meta:resourcekey="Label1Resource1" Style="text-align: left" Text="Select Report."
                            Width="92px"></asp:Label></td>
                    <td style="width: 133px; height: 30px; text-align: left">
                        <asp:DropDownList ID="ddlReport" runat="server" Width="215px" ForeColor="Gray" Font-Size="9pt"
                            Font-Names="Verdana" Font-Italic="False" OnSelectedIndexChanged="ddlReport_SelectedIndexChanged1"
                            AutoPostBack="True">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="height: 30px; text-align: left">
                        &nbsp;
                        <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Gray"
                            meta:resourcekey="Label1Resource1" Style="text-align: left" Text="Start Date."
                            Width="69px"></asp:Label></td>
                    <td style="width: 133px; height: 30px; text-align: left">
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
                </tr>
                <tr>
                    <td style="width: 2px; height: 30px; text-align: left">
                        <asp:Label ID="lblFrom" runat="server" Text="Time (hh : mm)" Font-Names="Verdana"
                            Font-Size="9pt" ForeColor="Gray" meta:resourcekey="Label1Resource1"></asp:Label></td>
                    <td style="width: 2px; height: 30px; text-align: left">
                        <asp:DropDownList ID="ddlFromHour" runat="server" Width="50px" Enabled="false">
                        </asp:DropDownList>&nbsp;
                        <asp:DropDownList ID="ddlFromMin" runat="server" Width="50px" Enabled="false">
                        </asp:DropDownList>
                        &nbsp;<asp:DropDownList ID="ddlFromAMPM" runat="server" Width="50px" Enabled="false">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="height: 30px; text-align: left">
                        &nbsp;
                        <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Gray"
                            meta:resourcekey="Label1Resource1" Style="text-align: left" Text="End Date."
                            Width="97px"></asp:Label></td>
                    <td style="width: 133px; height: 30px; text-align: left">
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
                </tr>
                <tr>
                    <td style="width: 2px; height: 30px; text-align: left">
                        <asp:Label ID="lblTo" runat="server" Text="Time (hh : mm)" Font-Names="Verdana" Font-Size="9pt"
                            ForeColor="Gray" meta:resourcekey="Label1Resource1"></asp:Label></td>
                    <td style="width: 2px; height: 30px; text-align: left">
                        <asp:DropDownList ID="ddlToHour" runat="server" Width="50px" Enabled="false">
                        </asp:DropDownList>
                        &nbsp;<asp:DropDownList ID="ddlToMin" runat="server" Width="50px" Enabled="false">
                        </asp:DropDownList>
                        &nbsp;<asp:DropDownList ID="ddlToAMPM" runat="server" Width="50px" Enabled="false">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 2px; height: 30px; text-align: left">
                        &nbsp;
                        <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Gray"
                            meta:resourcekey="Label1Resource1" Style="text-align: left" Text="Assigned User."
                            Width="100px"></asp:Label></td>
                    <td style="width: 133px; height: 30px; text-align: left">
                        <asp:DropDownList ID="ddlAssinedUser" runat="server" Width="215px" ForeColor="Gray"
                            Font-Size="9pt" Font-Names="Verdana" Font-Italic="False">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 2px; height: 30px; text-align: left">
                        &nbsp;
                        <asp:Label ID="Label8" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Gray"
                            meta:resourcekey="Label1Resource1" Style="text-align: left" Text="Approved User."
                            Width="100px"></asp:Label></td>
                    <td style="width: 133px; height: 30px; text-align: left">
                        <asp:DropDownList ID="ddlApprovedUser" runat="server" Width="215px" ForeColor="Gray"
                            Font-Size="9pt" Font-Names="Verdana" Font-Italic="False">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 2px; height: 30px; text-align: left">
                        &nbsp;
                        <asp:Label ID="lblPendingClearedUser" runat="server" Font-Names="Verdana" Font-Size="9pt"
                            ForeColor="Gray" Style="text-align: left" Text="Pending Cleared User." Width="100px"
                            Visible="false"></asp:Label></td>
                    <td style="width: 133px; height: 30px; text-align: left">
                        <asp:DropDownList ID="ddlPendingClearedUser" runat="server" Width="215px" ForeColor="Gray"
                            Font-Size="9pt" Font-Names="Verdana" Font-Italic="False" Visible="false">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 2px; height: 30px; text-align: left">
                        &nbsp;
                        <asp:Label ID="lblPaymentMode" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Gray"
                            meta:resourcekey="Label1Resource1" Style="text-align: left" Text="Payment Mode"
                            Width="92px"></asp:Label></td>
                    <td style="width: 133px; height: 30px; text-align: left">
                        <asp:DropDownList ID="ddlPaymentMode" runat="server" Width="215px" ForeColor="Gray"
                            Font-Size="9pt" Font-Names="Verdana" Font-Italic="False">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="width: 2px;">
                    </td>
                    <td style="width: 133px; text-align: left">
                        <asp:Button ID="btn1ViewReport" runat="server" OnClick="btn1ViewReport_Click" Text="View Report"
                            BackColor="#6699CC" BorderColor="White" BorderStyle="Solid" BorderWidth="1px"
                            Font-Names="Verdana" Font-Size="8.5pt" ForeColor="White" Height="20px" Width="105px" />&nbsp;
                        <asp:Button ID="btnViewBmOutProposals" runat="server" OnClick="btnViewBmOutProposals_Click"
                            Visible="false" Text="View BM OUT Proposals" BackColor="#6699CC" BorderColor="White"
                            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="8.5pt"
                            ForeColor="White" Height="20px" Width="155px" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 2px; height: 20px">
                    </td>
                    <td style="width: 133px; height: 20px; text-align: left">
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <br />
        <br />
        <asp:Panel ID="Panel2" runat="server" BackColor="White" Height="143px" Style="z-index: 103;
            left: 23px; position: absolute; top: 304px" Width="337px" BorderColor="#C0C0FF"
            BorderWidth="1px" Visible="false">
            <table style="width: 328px; height: 127px">
                <tbody>
                    <tr>
                        <td style="width: 25px; height: 6px">
                        </td>
                        <td style="width: 104px; height: 6px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 25px; height: 30px; text-align: left;">
                            &nbsp;
                            <asp:Label Style="text-align: left" ID="Label3" runat="server" Width="69px" Text="Policy No."
                                ForeColor="Gray" Font-Size="9pt" Font-Names="Verdana" meta:resourcekey="Label1Resource1"></asp:Label></td>
                        <td style="width: 104px; height: 30px">
                            <asp:TextBox ID="txtPolicyNo" runat="server" Width="202px" ForeColor="Gray" Font-Size="9pt"
                                Font-Names="Verdana" BorderWidth="1px" BorderColor="#C0C0FF" BorderStyle="Solid"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 25px; height: 30px; text-align: left;">
                            &nbsp;
                            <asp:Label Style="text-align: left" ID="Label4" runat="server" Width="85px" Text="Proposal No."
                                ForeColor="Gray" Font-Size="9pt" Font-Names="Verdana" meta:resourcekey="Label1Resource1"></asp:Label></td>
                        <td style="width: 104px; height: 30px">
                            <asp:TextBox ID="txtPropNo" runat="server" Width="202px" ForeColor="Gray" Font-Size="9pt"
                                Font-Names="Verdana" BorderWidth="1px" BorderColor="#C0C0FF" BorderStyle="Solid"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 25px; height: 30px; text-align: left;">
                            &nbsp;
                            <asp:Label Style="text-align: left" ID="Label7" runat="server" Width="87px" Text="Branch Code."
                                ForeColor="Gray" Font-Size="9pt" Font-Names="Verdana" meta:resourcekey="Label1Resource1"></asp:Label></td>
                        <td style="width: 104px; height: 30px">
                            <asp:DropDownList ID="ddlBranch" runat="server" Width="202px" ForeColor="Gray" Font-Size="9pt"
                                Font-Names="Verdana" Font-Italic="False">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td style="width: 25px; text-align: left">
                        </td>
                        <td style="width: 104px; text-align: left">
                            <asp:Button ID="BtnView2" runat="server" BackColor="#6699CC" BorderColor="White"
                                BorderWidth="1px" Font-Names="Verdana" Font-Size="8.5pt" ForeColor="White" Height="20px"
                                OnClick="BtnView2_Click1" Text="View Report" Width="97px" /></td>
                    </tr>
                </tbody>
            </table>
            &nbsp;
        </asp:Panel>
        <asp:Label ID="Label10" runat="server" Font-Names="Verdana" Font-Size="9pt" ForeColor="Gray"
            meta:resourcekey="Label1Resource1" Style="z-index: 104; left: 27px; position: absolute;
            top: 287px; text-align: left" Text="Sub Criteria." Width="131px" Visible="false"></asp:Label>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
    </asp:Panel>
</asp:Content>
