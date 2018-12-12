<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowFollowUp.aspx.cs"
    Inherits="MRPWorkflow_WorkflowFollowUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
 
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Panel ID="pnlFollowUp" runat="server" BackColor="Gainsboro" BorderColor="#C0C0FF"
                BorderWidth="1px" ForeColor="black" Style="z-index: 100;" Width="100%">
                <asp:GridView ID="grdFollowUp" CssClass="Gridview" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="PROPOSAL_NO,STATUS_CODE" Width="100%" OnRowDataBound="grdFollowUp_RowDataBound">
                    <HeaderStyle BackColor="#236790" ForeColor="white" />
                    <Columns>
                       
                        <asp:BoundField ItemStyle-Width="150px"  DataField="STATUS_NAME" HeaderText="Status" />
                        <asp:BoundField ItemStyle-Width="150px"  DataField="SYS_DATE" HeaderText="Date and Time" />
                        <asp:BoundField ItemStyle-Width="150px"  DataField="REMARKS" HeaderText="Remarks" />
                        <asp:BoundField ItemStyle-Width="150px"  DataField="USER_NAME" HeaderText="User" />
                        
                         <asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:GridView ID="grdReminders" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="150px" DataField="reminder_no" HeaderText="Reminder No." />
                                        <asp:BoundField ItemStyle-Width="150px" DataField="sys_date" HeaderText="Date" />
                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
