<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ViewMRPWorkflow.aspx.cs" Inherits="ViewMRPWorkflow" Title="MRP Workflow" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>


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
	


        function jsViewDocuments(){
                     
             if(document.getElementById('<%= ddlStatus.ClientID %>').value != ""){
                //alert(document.getElementById('<%= txtProposal.ClientID %>').value);
                windowprop = "toolbar=1,Location=1,menubar=1,scrollbars=1,status=1,resizable=1,width='100%',height='100%'";
	            //RefLink = "Documents/WorkflowDocuments.aspx?ProposalNo=" +document.getElementById('<%= txtProposal.ClientID %>').value;
	            RefLink = "Documents/WorkflowDocuments.aspx?StatusCode=" +document.getElementById('<%= ddlStatus.ClientID %>').value+"&"+"ProposalNo=" +document.getElementById('<%= txtProposal.ClientID %>').value;
	            Child = open(RefLink,"Documents",windowprop);
	        }
        }
        
        function jsViewFollowup(){
             if(document.getElementById('<%= txtProposal.ClientID %>').value != ""){
                windowprop = "toolbar=1,Location=1,menubar=1,scrollbars=1,status=1,resizable=1,width='100%',height='100%'";
	            RefLink = "WorkflowFollowUp.aspx?ProposalNo=" +document.getElementById('<%= txtProposal.ClientID %>').value;
	            Child = open(RefLink,"FollowUp",windowprop);
	        }
        }
      
    </script>

    <table style="z-index: 103; left: 0px; position: absolute; top: -54px">
        <asp:Button ID="ButtonTarget" runat="server" Text="Button" Style="display: none" /><ajaxToolkit:ModalPopupExtender
            ID="mpeMsgBox" BackgroundCssClass="modalBackground" runat="server" PopupControlID="PanelMsgBox"
            TargetControlID="ButtonTarget" OkControlID="ButtonOK" CancelControlID="ButtonCancel">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="PanelConfirmBox" runat="server" Style="display: none" CssClass="modalPopup">
            <asp:Label ID="LabelMsg" runat="server" Text="Payment has updated in MRP System, Do you need to change the status to Payment Updated in Workflow too?"></asp:Label>
            <br />
            <br />
            <br />
            <asp:Button ID="ButtonOK" runat="server" Text="OK" OnClick="btnMsgOK_Click" CausesValidation="false"
                UseSubmitBehavior="false" />&nbsp;
            <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />
        </asp:Panel>
        <asp:Panel ID="PanelMsgBox" runat="server" Style="display: none" CssClass="modalPopup">
            <asp:Label ID="lblPanelMsgBox" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <br />
            <asp:Button ID="btnPanelMsgBox" runat="server" Text="OK" />
        </asp:Panel>
        <asp:Button ID="btnDeleteOnCondition" runat="server" Text="Delete" Style="display: none" /><asp:ScriptManager
            ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <tr>
            <td>
                <asp:Panel ID="pnlSearch" runat="server" BackColor="Gainsboro" BorderColor="#C0C0FF"
                    BorderWidth="1px" ForeColor="Gainsboro" Style="z-index: 100;" Width="750px">
                    <table style="width: 562px">
                        <tr>
                            <td>
                                <asp:Label ID="Label36" runat="server" ForeColor="Gray" Style="text-align: left"
                                    Text="Job Number" Width="111px"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtSearchJobNumber" runat="server" Width="189px" TabIndex="200"
                                    CssClass="input"></asp:TextBox>
                            </td>
                            <td style="width: 140px; text-align: left" rowspan="8">
                                &nbsp;</td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" ForeColor="Gray" Style="text-align: left" Text="Proposal"
                                    Width="111px"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtSearchProposal" runat="server" Width="189px" TabIndex="200" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" ForeColor="Gray" Style="text-align: left" Text="NIC 1"
                                    Width="111px"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtSearchNIC1" runat="server" Width="189px" TabIndex="201" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" ForeColor="Gray" Style="text-align: left" Text="NIC 2"
                                    Width="111px"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtSearchNIC2" runat="server" Width="189px" TabIndex="201" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" ForeColor="Gray" Style="text-align: left" Text="Assigned To"
                                    Width="111px"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlSearchAssignedTo" runat="server" Style="z-index: 109;" Width="188px"
                                    TabIndex="205" CssClass="dropDown">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label40" runat="server" ForeColor="Gray" Style="text-align: left"
                                    Text="Status" Width="111px"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlSearchStatus" runat="server" Style="z-index: 109;" Width="188px"
                                    TabIndex="205" CssClass="dropDown" OnSelectedIndexChanged="ddlSearchStatus_SelectedIndexChange"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlSearchReminderStage" runat="server" Style="z-index: 109;"
                                    Width="40px" TabIndex="205" CssClass="dropDown" Visible="false">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label46" runat="server" ForeColor="Gray" Style="text-align: left"
                                    Text="Pending Cleared" Width="111px"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:CheckBox ID="chkSearchFaxCleared" runat="server" Text=" Fax" ForeColor="black" />
                                <asp:CheckBox ID="chkSearchOriginalsCleared" runat="server" Text=" Originals" ForeColor="black" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <asp:Label ID="Label47" runat="server" ForeColor="Gray" meta:resourcekey="Label1Resource1"
                                    Style="text-align: left" Text="Start Date." Width="111px"></asp:Label></td>
                            <td style="text-align: left">
                                <asp:TextBox runat="server" ID="txtSearchfromDate" AutoCompleteType="None" Width="75px"
                                    AutoPostBack="false" />
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtSearchfromDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                    ControlToValidate="txtSearchfromDate" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid"
                                    Display="Dynamic" TooltipMessage="" EmptyValueBlurredText="Date is invalid" InvalidValueBlurredMessage="Date is invalid"
                                    ValidationGroup="MKE" />
                                <ajaxToolkit:CalendarExtender ID="txtfromDateExtender" runat="server" TargetControlID="txtSearchfromDate"
                                    CssClass="MyCalendar" Format="dd/MM/yyyy" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <asp:Label ID="Label48" runat="server" ForeColor="Gray" meta:resourcekey="Label1Resource1"
                                    Text="End Date." Width="111px"></asp:Label></td>
                            <td style="text-align: left">
                                <asp:TextBox runat="server" ID="txtSearchToDate" AutoCompleteType="None" Width="75px"
                                    AutoPostBack="false" />
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" TargetControlID="txtSearchToDate"
                                    Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                                    OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                                    ErrorTooltipEnabled="True" />
                                <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MaskedEditExtender2"
                                    ControlToValidate="txtSearchToDate" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid"
                                    Display="Dynamic" TooltipMessage="" EmptyValueBlurredText="Date is invalid" InvalidValueBlurredMessage="Date is invalid"
                                    ValidationGroup="MKE" />
                                <ajaxToolkit:CalendarExtender ID="txtToDateExtender" runat="server" TargetControlID="txtSearchToDate"
                                    CssClass="MyCalendar" Format="dd/MM/yyyy" />
                            </td>
                            <td style="width: 140px">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="text-align: left">
                                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" TabIndex="206"
                                    Text="Search" Width="75px" CssClass="button" />
                                <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" TabIndex="207"
                                    Text="Clear" Width="75px" CssClass="button" /></td>
                            <td style="width: 140px">
                                &nbsp;
                                
                            </td>
                            <td>
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
                <asp:Panel ID="pnlPolicyGrid" runat="server" Height="113px" ScrollBars="Vertical"
                    Style="z-index: 102;" Width="1043px" BorderColor="#C0C0FF" BorderWidth="1px">
                    <asp:GridView ID="grdPolicies" runat="server" BackColor="White" BorderColor="#CCCCCC"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="8pt" Style="z-index: 102;" Width="995px" OnSelectedIndexChanged="grdPolicies_SelectedIndexChanged"
                        OnRowDataBound="grdPolicies_RowDataBound">
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
                    <asp:Panel ID="Panel3" runat="server" Height="950px" Style="z-index: 104;" Width="1038px"
                        BorderColor="#C0C0FF" BorderWidth="1px">
                        &nbsp; &nbsp;&nbsp;
                        <table style="width: 950px; height: 188px" cellpadding="2" cellspacing="5">
                            <tr>
                                <td>
                                    <asp:Label ID="lblIsFastTrackJob" runat="server" Font-Names="Verdana" Font-Size="14pt"
                                        Style="z-index: 106;" Text="" Width="150px" ForeColor="red"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="up_Timer" runat="server" RenderMode="Inline" UpdateMode="Always">
                                        <triggers>
                                            <asp:AsyncPostBackTrigger ControlID="tmrCountdown" EventName="Tick" />
                                        </triggers>
                                        <contenttemplate>
                                            <asp:Timer ID="tmrCountdown" runat="server" Interval="1000" OnTick="tmrCountdown_Tick" enabled="false"/>
                                            <asp:Literal ID="lit_Timer" runat="server" /><br />
                                            <asp:HiddenField ID="hid_Ticker" runat="server" Value="0" />
                                        </contenttemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label37" runat="server" Font-Names="Verdana" Font-Size="9pt" Style="z-index: 106;"
                                        Text="Job Number" Width="150px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtJobNumber" runat="server" MaxLength="250" Style="z-index: 105;"
                                        Width="184px" TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblCourierServiceLabel" runat="server" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Courier Service" Width="150px"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:Label ID="lblCourierService" runat="server" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="" Width="150px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Policy"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPolicy" runat="server" Style="z-index: 105;" Width="184px" TabIndex="6"
                                        CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label18" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Proposal"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtProposal" runat="server" Style="z-index: 105;" Width="184px"
                                        TabIndex="6" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Medical/Non-Medical"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMedicalNonMedical" runat="server" Style="z-index: 109;"
                                        Width="188px" TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 111;" Text="Status"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:CheckBox ID="chkSkipToCertificateIssued" runat="server" Text=" Skip To Certificate Issued" />
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td colspan="2">
                                    <asp:CheckBox ID="chkExcessPremiumReimbursementDone" runat="server" Text=" Excess Premium Reimbursement Done" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkMedicalReimbursementDone" runat="server" Text="  Medical Reimbursement Done" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Sum Insured"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSumInsured" runat="server" Style="z-index: 108;" Width="184px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label45" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Commencement Date"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtCommencementDate" runat="server" Style="z-index: 108;" Width="150px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReInsurer" runat="server" Font-Bold="False" Font-Names="Verdana"
                                        Font-Size="9pt" Style="z-index: 124;" Text="Re Insurer"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReInsurer" runat="server" Style="z-index: 108;" Width="184px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Life Insured 1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLifeInsured1" runat="server" Style="z-index: 108;" Width="300px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Life Insured 2"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtLifeInsured2" runat="server" Style="z-index: 108;" Width="300px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="NIC1"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNIC1" runat="server" Style="z-index: 108;" Width="100px" TabIndex="8"
                                        CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label19" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="NIC2"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtNIC2" runat="server" Style="z-index: 108;" Width="100px" TabIndex="8"
                                        CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label15" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Age"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAge1" runat="server" Style="z-index: 108;" Width="100px" TabIndex="8"
                                        CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label30" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Age"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtAge2" runat="server" Style="z-index: 108;" Width="100px" TabIndex="8"
                                        CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label20" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Life assured covers"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkLifeassured1coversNaturalOrAccDeath" runat="server" Text=" Natural or accidental death" /><br />
                                    <asp:CheckBox ID="chkLifeassured1coversTPD" runat="server" Text=" Total permanent disability due to accident or sickness" /><br />
                                    <asp:CheckBox ID="chkLifeassured1BeneficiaryCover" runat="server" Text=" Beneficiary cover"
                                        AutoPostBack="True" /><br />
                                </td>
                                <td>
                                    <asp:Label ID="Label21" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Life assured covers"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:CheckBox ID="chkLifeassured2coversNaturalOrAccDeath" runat="server" Text=" Natural or accidental death" /><br />
                                    <asp:CheckBox ID="chkLifeassured2coversTPD" runat="server" Text=" Total permanent disability due to accident or sickness" /><br />
                                    <asp:CheckBox ID="chkLifeassured2BeneficiaryCover" runat="server" Text=" Beneficiary cover"
                                        AutoPostBack="True"  Visible="false"/><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updPnl1" runat="server" UpdateMode="Conditional">
                                        <contenttemplate>
                                            <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLifeassured1BeneficiaryName" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 124;" Text="Name" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLifeassured1BeneficiaryName" runat="server" Style="z-index: 108;"
                                                    Width="200px" TabIndex="8" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLifeassured1BeneficiaryNIC" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 124;" Text="NIC" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLifeassured1BeneficiaryNIC" runat="server" Style="z-index: 108;"
                                                    Width="100px" TabIndex="8" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLifeassured1BeneficiaryAddress" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 124;" Text="Address" Width="100px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLifeassured1BeneficiaryAddress" runat="server" Style="z-index: 108;"
                                                    Width="200px" TabIndex="8" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                        </contenttemplate>
                                        <triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chkLifeassured1BeneficiaryCover" />
                                        </triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="updPnl2" runat="server" UpdateMode="Conditional">
                                        <contenttemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLifeassured2BeneficiaryName" runat="server" Font-Bold="False" Font-Names="Verdana"
                                                    Font-Size="9pt" Style="z-index: 124;" Text="Name" Width="100px" visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLifeassured2BeneficiaryName" runat="server" Style="z-index: 108;"
                                                    Width="200px" TabIndex="8" CssClass="input" visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLifeassured2BeneficiaryNIC" runat="server" Font-Bold="False" Font-Names="Verdana"
                                                    Font-Size="9pt" Style="z-index: 124;" Text="NIC" Width="100px" visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLifeassured2BeneficiaryNIC" runat="server" Style="z-index: 108;"
                                                    Width="100px" TabIndex="8" CssClass="input" visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLifeassured2BeneficiaryAddress" runat="server" Font-Bold="False"
                                                    Font-Names="Verdana" Font-Size="9pt" Style="z-index: 124;" Text="Address" Width="100px" visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLifeassured2BeneficiaryAddress" runat="server" Style="z-index: 108;"
                                                    Width="200px" TabIndex="8" CssClass="input" visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                     </contenttemplate>
                                        <triggers>
                                            <asp:AsyncPostBackTrigger ControlID="chkLifeassured2BeneficiaryCover" />
                                        </triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblLifeAssured1HNBARefNo" runat="server" Font-Bold="False" Font-Names="Verdana"
                                        Font-Size="9pt" Style="z-index: 124;" Text="HNBA Ref No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLifeAssured1HNBARefNo" runat="server" Style="z-index: 108;" Width="100px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblLifeAssured2HNBARefNo" runat="server" Font-Bold="False" Font-Names="Verdana"
                                        Font-Size="9pt" Style="z-index: 124;" Text="HNBA Ref No "></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtLifeAssured2HNBARefNo" runat="server" Style="z-index: 108;" Width="100px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblLifeAssured1RIRefNo" runat="server" Font-Bold="False" Font-Names="Verdana"
                                        Font-Size="9pt" Style="z-index: 124;" Text="RI Ref No"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLifeAssured1RIRefNo" runat="server" Style="z-index: 108;" Width="100px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblLifeAssured2RIRefNo" runat="server" Font-Bold="False" Font-Names="Verdana"
                                        Font-Size="9pt" Style="z-index: 124;" Text="RI Ref No"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtLifeAssured2RIRefNo" runat="server" Style="z-index: 108;" Width="100px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Bank Code"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBank" runat="server" Style="z-index: 108;" Width="184px" TabIndex="8"
                                        CssClass="input"></asp:TextBox>
                                    <asp:TextBox ID="txtBankType" runat="server" Width="184px" Visible="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Agent Code"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtAgentCode" runat="server" Style="z-index: 108;" Width="184px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label12" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBranch" runat="server" Style="z-index: 108;" Width="184px" TabIndex="8"
                                        CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label29" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Assurance Code"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtAssuranceCode" runat="server" Style="z-index: 108;" Width="184px"
                                        TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label14" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Medical"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMedical" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label50" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124" Text="Broker Code"></asp:Label></td>
                                <td style="width: 338px">
                                    <asp:DropDownList ID="ddlBrokerCode" runat="server" Style="z-index: 109;" Width="188px"
                                        TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label28" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Premium" Enabled="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPremium" runat="server" Style="z-index: 108;" Width="184px" TabIndex="8"
                                        CssClass="input" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label38" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Currency"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtCurrency" runat="server" Style="z-index: 108;" Width="184px"
                                        Enabled="false" TabIndex="8" CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label22" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 106;" Text="Loan Amount 1"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLoanAmount1" runat="server" Style="z-index: 105;" Width="184px"
                                                    TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label23" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 106;" Text="Interest Rate 1"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInterestRate1" runat="server" Style="z-index: 105;" Width="184px"
                                                    TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label33" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 106;" Text="Term 1"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTerm1" runat="server" Style="z-index: 105;" Width="184px" TabIndex="6"
                                                    CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLoanAmount2" runat="server" Font-Bold="False" Font-Names="Verdana"
                                                    Font-Size="9pt" Style="z-index: 106;" Text="Loan Amount 2"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLoanAmount2" runat="server" Style="z-index: 105;" Width="184px"
                                                    TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInterestRate2" runat="server" Font-Bold="False" Font-Names="Verdana"
                                                    Font-Size="9pt" Style="z-index: 106;" Text="Interest Rate 2"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInterestRate2" runat="server" Style="z-index: 105;" Width="184px"
                                                    TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTerm2" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 106;" Text="Term 2"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTerm2" runat="server" Style="z-index: 105;" Width="184px" TabIndex="6"
                                                    CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblLoanAmount3" runat="server" Font-Bold="False" Font-Names="Verdana"
                                                    Font-Size="9pt" Style="z-index: 106;" Text="Loan Amount 3"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLoanAmount3" runat="server" Style="z-index: 105;" Width="184px"
                                                    TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblInterestRate3" runat="server" Font-Bold="False" Font-Names="Verdana"
                                                    Font-Size="9pt" Style="z-index: 106;" Text="Interest Rate 3"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInterestRate3" runat="server" Style="z-index: 105;" Width="184px"
                                                    TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblTerm3" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 106;" Text="Term 3"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTerm3" runat="server" Style="z-index: 105;" Width="184px" TabIndex="6"
                                                    CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label55" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 106;" Text="Fixed Interest Term"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFixedInterestTerm" runat="server" Style="z-index: 105;" Width="184px"
                                                    TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label56" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 106;" Text="Addition to AWPLR"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAdditionToAWPLR" runat="server" Style="z-index: 105;" Width="184px"
                                                    TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label57" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                    Style="z-index: 106;" Text="Interest rate type"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInterestRateType" runat="server" Style="z-index: 105;" Width="184px"
                                                    TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label53" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Full Term"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFullTerm" runat="server" Style="z-index: 105;" Width="184px"
                                        TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkTPDMarketLimit" runat="server" Text=" TPPD market limit" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label31" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Approved User"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApprovedUser" runat="server" Style="z-index: 105;" Width="184px"
                                        TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label32" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106;" Text="Assigned User"></asp:Label>
                                </td>
                                <td style="width: 338px">
                                    <asp:TextBox ID="txtAssignedUser" runat="server" Style="z-index: 105;" Width="184px"
                                        TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtAssignedUserCode" runat="server" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkPPI" runat="server" Text=" PPI" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkScanned" runat="server" Text=" Scanned" />
                                </td>
                                <td>
                                    <asp:Label ID="Label51" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 106" Text="Other Policies"></asp:Label></td>
                                <td colspan="2" rowspan="2">
                                    <asp:Panel ID="Panel1" runat="server" Height="100px" ScrollBars="Auto" Style="z-index: 102;"
                                        Width="250px" BorderColor="#C0C0FF" BorderWidth="1px" BackColor="Gainsboro" ForeColor="Gainsboro">
                                        <asp:GridView ID="GrdOtherPolicies" runat="server" BackColor="White" BorderColor="#CCCCCC"
                                            BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                                            Font-Size="8pt" Style="z-index: 102;" Width="98%">
                                            <RowStyle ForeColor="Black" BackColor="White" Height="15px" />
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Tahoma" Font-Size="Larger"
                                                ForeColor="White" Height="20px" />
                                            <AlternatingRowStyle BackColor="WhiteSmoke" Font-Names="Tahoma" Font-Size="8pt" Height="15px" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label52" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Clauses"></asp:Label>
                                </td>
                                <td>
                                    <asp:CheckBoxList ID="chkLstClauses" runat="server">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label13" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Remarks"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" Style="z-index: 108;" Width="184px" Height="60px"
                                        TabIndex="8" CssClass="input" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label54" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                        Style="z-index: 124;" Text="Cover Note Validity Period"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCoverNoteValidityPeriod" runat="server" Style="z-index: 109;"
                                        Width="188px" TabIndex="205" CssClass="dropDown">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCurDate" runat="server" Width="184px" Visible="false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" align="center">
                                   
                                    <asp:Button ID="btnFollowup" runat="server" TabIndex="25" Text="Followup" Width="100px"
                                        CssClass="button" />
                                   
                                    <div>
                                    </div>
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
