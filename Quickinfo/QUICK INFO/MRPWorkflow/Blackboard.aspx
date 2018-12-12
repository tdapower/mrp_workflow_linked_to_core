<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Blackboard.aspx.cs" Inherits="MRPWorkflow_Blackboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style>
	body{
	background-color:black;
	}
*{
				padding: 0;
				margin: 0;
				box-sizing: border-box;
}
.container{
  //width: 800px;
  margin: 30px auto 0 50px;
}
.line{
  padding: 5px 0 5px 30px;
  border-left: 3px solid #ffffff;
}
p{
  text-align: left;
  width:700px;
  padding: 10px;
  border:1px solid #ffffff;
  color: #ffffff;
  margin: 20px 0;
  position: relative;
   Font-Names="Verdana";
   Font-Size="9pt";
}
p:before{
  content: '';
  position: absolute;
  left: -40px;
  top: 7px;
  width: 10px;
  height: 10px;
  border: 3px solid #ffffff;
  border-radius: 50%;
  background-color: #ffffff;
}
p:after{
  content: '';
  position: absolute;
  left: -18px;
  top: 7px;
  width: 0;
  height: 0;
  border-top: 8px solid transparent;
  border-left: 8px solid transparent;
  border-bottom: 8px solid transparent;
  border-right: 10px solid #ffffff;
}

	<%--tr{
	background-color:black;
	
	}--%>
	
	
	.topbox {
    background-color: lightgrey;
    width: 800px;
    padding: 15px;
    margin: 15px;
}



/* For Calendar Control */
/* ==========================================================*/
.MyCalendar .ajax__calendar_container 
{
    border:1px solid #646464;
    background-color: #EDF5FA;
    color: blue;
    position: absolute;
    z-index:500;
	   
}
.MyCalendar .ajax__calendar_other .ajax__calendar_day,
.MyCalendar .ajax__calendar_other .ajax__calendar_year 
{
    color: black;
}
.MyCalendar .ajax__calendar_hover .ajax__calendar_day,
.MyCalendar .ajax__calendar_hover .ajax__calendar_month,
.MyCalendar .ajax__calendar_hover .ajax__calendar_year 
{
    color: black;
}
.MyCalendar .ajax__calendar_active .ajax__calendar_day,
.MyCalendar .ajax__calendar_active .ajax__calendar_month,
.MyCalendar .ajax__calendar_active .ajax__calendar_year 
{
    color: black;
    font-weight:bold;
}


/* End Calendar Control */
/*==========================================================================*/


	</style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div class="topbox">
            <table style="width: 700px; height: 100px; border-color: Red">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" ForeColor="black" Style="text-align: Left;
                            font-weight: bold;" Font-Names="Verdana" Font-Size="9pt" Text="Proposal No."
                            Width="111px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProposalNo" runat="server" MaxLength="250" Style="z-index: 105;
                            text-align: Left;" Width="250px" TabIndex="6" CssClass="input" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label36" runat="server" ForeColor="black" Style="text-align: Left;
                            font-weight: bold;" Font-Names="Verdana" Font-Size="9pt" Text="New Comment" Width="111px"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtNewComment" runat="server" TabIndex="200" Width="100%" CssClass="input"
                            TextMode="MultiLine" Rows="5">
                        </asp:TextBox>
                    </td>
                    <td style="width: 140px; text-align: left">
                        <asp:Button ID="btnSaveComment" runat="server" CssClass="button" OnClick="btnSaveComment_Click"
                            TabIndex="25" Text="Add Comment" Font-Names="Verdana" Width="200px" Height="50px" /></td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" ForeColor="black" Style="text-align: Left;
                            font-weight: bold;" Font-Names="Verdana" Font-Size="9pt" Text="New Reminder"
                            Width="111px"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReminderText" runat="server" TabIndex="200" Width="100%" CssClass="input"
                            TextMode="MultiLine" Rows="3">
                        </asp:TextBox>
                    </td>
                    <td style="text-align: left">
                       <asp:Label ID="Label3" runat="server" ForeColor="black" Style="text-align: Left;
                            font-weight: bold;" Font-Names="Verdana" Font-Size="9pt" Text="Reminder Date"
                            Width="111px"></asp:Label>
                        <asp:TextBox runat="server" ID="txtReminderDate" AutoCompleteType="None" Width="75px"
                            AutoPostBack="false" />
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtReminderDate"
                            Mask="99/99/9999" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                            OnInvalidCssClass="MaskedEditError" MaskType="Date" DisplayMoney="Left" AcceptNegative="Left"
                            ErrorTooltipEnabled="True" />
                        <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                            ControlToValidate="txtReminderDate" EmptyValueMessage="Date is required" InvalidValueMessage="Date is invalid"
                            Display="Dynamic" TooltipMessage="" EmptyValueBlurredText="Date is invalid" InvalidValueBlurredMessage="Date is invalid"
                            ValidationGroup="MKE" />
                        <ajaxToolkit:CalendarExtender ID="CalendarExtenderDateOfSending" runat="server" TargetControlID="txtReminderDate"
                            CssClass="MyCalendar" Format="dd/MM/yyyy" />
                    </td>
                    <td style="width: 140px; text-align: left">
                        <asp:Button ID="btnSaveReminder" runat="server" CssClass="button" OnClick="btnSaveReminder_Click"
                            TabIndex="25" Text="Add Reminder" Font-Names="Verdana" Width="200px" Height="50px" /></td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Label ID="lblMsg" runat="server" Font-Bold="False" Font-Names="Franklin Gothic Book"
                                    Font-Size="11pt" ForeColor="Red" Style="z-index: 101;" Height="22px"></asp:Label>
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
        <div style="text-align: center">
            <div class="container">
                <div class="line">
                    <asp:Literal ID="ltrlOldComments" runat="server" /><br />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
