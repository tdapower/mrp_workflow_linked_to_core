<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowDocuments.aspx.cs"
    Inherits="MRPWorkflow_Documents_WorkflowDocuments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow Documents</title>

    <script type="text/javascript">


        function jsViewDocuments(DocID){
        
             if(DocID != ""){
                windowprop = "toolbar=1,Location=1,menubar=0,scrollbars=1,status=1,resizable=1,width='100%',height='100%'";
	            RefLink = "DocumentViewer.aspx?DocID=" +DocID;
	            Child = open(RefLink,"Documents",windowprop);
	        }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <asp:Panel ID="pnlDocs" runat="server" BackColor="Gainsboro" BorderColor="#C0C0FF"
                BorderWidth="1px" ForeColor="black" Style="z-index: 100;" Width="100%">
                <table>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label16" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                Style="z-index: 106;" Text="Signing Person"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSigningPerson" runat="server" Style="z-index: 109;" Width="388px"
                                TabIndex="205" CssClass="dropDown" OnSelectedIndexChanged="ddlSigningPerson_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                Style="z-index: 106;" Text="Designation" Width="150px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDesignation" runat="server"  Style="z-index: 105;" Enabled="false"
                                Width="388px" TabIndex="6" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                Style="z-index: 106;" Text="Medical Lab"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMedicalLab" runat="server" Style="z-index: 109;" Width="388px"
                                TabIndex="205" CssClass="dropDown"  AutoPostBack="false">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
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
                <asp:GridView ID="grdDocuments" CssClass="Gridview" runat="server" AutoGenerateColumns="false"
                    DataKeyNames="DOCUMENT_ID" Width="100%">
                    <HeaderStyle BackColor="#236790" ForeColor="white" />
                    <Columns>
                        <asp:BoundField DataField="DOCUMENT_ID" HeaderText="Id" Visible="false" />
                        <asp:BoundField DataField="DOCUMENT_TITLE" HeaderText="Document" />
                        <asp:TemplateField HeaderText="Document">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkViewDocument" runat="server" Text="View" OnClick="lnkViewDocument_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <br />
            <br />
            <asp:Panel ID="pnlReport" runat="server">
                <CR:CrystalReportViewer ID="LetterViewer" runat="server" AutoDataBind="True" DisplayGroupTree="False"
                    Height="799px" Width="1095px" OnInit="LetterViewer_Init" EnableDatabaseLogonPrompt="False"
                    EnableParameterPrompt="False" ReuseParameterValuesOnRefresh="True"></CR:CrystalReportViewer>
            </asp:Panel>
            <asp:TextBox ID="txtDocumentId" runat="server" Visible="false"></asp:TextBox>
        </div>
    </form>
</body>
</html>
