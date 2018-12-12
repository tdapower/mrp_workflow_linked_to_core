<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewPendingDocuments.aspx.cs"
    Inherits="ViewPendingDocuments" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
    <link id="Link1" runat="server" rel="stylesheet" type="text/css" ie:href="~/default.css"
        href="~/Styles/StyleSheet.css" media="screen" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </cc1:ToolkitScriptManager>
            <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Font-Names="Segoe UI"
                Font-Size="9pt" Height="656px" Width="1081px">
                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        <span style="font-size: 9pt; font-family: Segoe UI">Pending Documents</span>&nbsp;
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table style="z-index: 103; left: 50px; position: absolute; top: 50px">
                            <tr>
                                <td>
                                    <div align="left" class="listunlinkstyle">
                                        <asp:Panel ID="Panel3" runat="server" Style="z-index: 104;" Width="1000px" BorderColor="#C0C0FF"
                                            BorderWidth="1px">
                                            &nbsp; &nbsp;&nbsp;
                                            <table>
                                                <tr>
                                                    <td align="center">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblMsg" runat="server" Font-Bold="False" Font-Names="Franklin Gothic Book"
                                                                    Font-Size="11pt" ForeColor="Red" Style="z-index: 101;" Width="800px" Height="22px"></asp:Label>
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
                                            <table style="width: 900px;">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblNote" runat="server" Text="Pending Documents of MRP Workflow" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label16" runat="server" Font-Bold="False" Font-Names="Verdana" Font-Size="9pt"
                                                                        Style="z-index: 106;" Text="Document Type"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlDocumentType" runat="server" Style="z-index: 109;" Width="188px"
                                                                        TabIndex="205" CssClass="dropDown" OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged"
                                                                        AutoPostBack="true">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="txtProposalNo" runat="server" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label1" runat="server" Text="Life Assured 1" Font-Bold="true"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label2" runat="server" Text="Life Assured 2" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td>
                                                        <asp:TreeView ID="tvPendingsLifeAssured1" runat="server">
                                                        </asp:TreeView>
                                                    </td>
                                                    <td>
                                                        <asp:TreeView ID="tvPendingsLifeAssured2" runat="server">
                                                        </asp:TreeView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Button ID="btnAlter" runat="server" TabIndex="23" Text="Alter" Width="100px"
                                                            OnClick="btnAlter_Click" CssClass="button" Visible="false" />
                                                        <asp:Button ID="btnSave" runat="server" TabIndex="24" Text="Save" Width="100px" OnClick="btnSave_Click"
                                                            CssClass="button" />
                                                        <asp:Button ID="btnCancel" runat="server" TabIndex="25" Text="Close" Width="100px"
                                                            OnClientClick="window.close(); return false;" CssClass="button" />
                                                        <asp:Button ID="btnSendPendingLetter" runat="server" TabIndex="25" Text="Send Pending Letter"
                                                            Width="200px" CssClass="button" OnClick="btnSendPendingLetter_Click" />
                                                        <asp:Button ID="btnSendOriginalsPendingLetter" runat="server" TabIndex="25" Text="Send Originals Pending Letter"
                                                            Width="250px" CssClass="button" OnClick="btnSendOriginalsPendingLetter_Click" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                    <HeaderTemplate>
                        <span style="font-size: 9pt; font-family: Segoe UI">E-Mail Generation</span>&nbsp;
                    </HeaderTemplate>
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" ForeColor="Gray" Style="text-align: left" Text="E-mail To:-"
                                        Width="111px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailToAddresses" runat="server" Width="400px" TabIndex="200"
                                        CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" ForeColor="Gray" Style="text-align: left" Text="E-mail Cc:-"
                                        Width="111px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmailCcAddresses" runat="server" Width="400px" TabIndex="200"
                                        CssClass="input"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" ForeColor="Gray" Style="text-align: left" Text="Custom Note:-"
                                        Width="111px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCustomNote" runat="server" Width="400px" TabIndex="200" CssClass="input"
                                        TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" ForeColor="Gray" Style="text-align: left" Text="Additional Attachments:-"
                                        Width="111px"></asp:Label>
                                </td>
                                <td>
                                    <asp:FileUpload ID="fileUploadAdditionalAttachments"  Multiple="Multiple runat="server"   />
                                </td>
                            </tr>
                                 <tr>
                                <td>
                                    
                                </td>
                                <td>
                                   <asp:Button ID="btnSendEmail" runat="server" TabIndex="25" Text="Send Email" Width="100px"
                                        OnClick="btnSendEmail_Click" CssClass="button" />
                                </td>
                            </tr>
                            
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
        </div>
      
        
        
    </form>
</body>
</html>
