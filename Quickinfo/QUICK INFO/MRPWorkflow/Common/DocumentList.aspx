<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentList.aspx.cs" Inherits="MRPWorkflow_Common_DocumentList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager2" runat="server">
        </asp:ScriptManager>
        
        <asp:TextBox ID="txtProposalNo" runat="server" Visible="false"></asp:TextBox>
        <div>
            <asp:GridView ID="grdUploadedDocs" runat="server" CssClass="myGridStyle-small" OnRowDataBound="grdUploadedDocs_RowDataBound" >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Document" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnViewDocument" runat="server">View</asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="mp2" runat="server" PopupControlID="Panl2" TargetControlID="lnkBtnViewDocument"
                                CancelControlID="Button2" BackgroundCssClass="Background">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="Panl2" runat="server" CssClass="Popup" align="center" Style="display: none">
                                <iframe style="width: 800px; height: 560px;" id="irm2" runat="server"></iframe>
                                <br />
                                <asp:Button ID="Button2" runat="server" Text="Close" CssClass="btn btn-apps" />
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnDeleteDocument" runat="server" 
                CommandName="DeleteDoc"  OnClick="lnkBtnDeleteDocument_Click">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle ForeColor="Black" BackColor="White" Height="15px" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Tahoma" Font-Size="Larger"
                    ForeColor="White" Height="20px" />
                <AlternatingRowStyle BackColor="WhiteSmoke" Font-Names="Tahoma" Font-Size="8pt" Height="15px" />
            </asp:GridView>
            
            
            
            
        </div>
        
        
        <div>
        Documents uploaded in Online MRP system
         <asp:GridView ID="grdOnlineMRPUploadedDocs" runat="server" CssClass="myGridStyle-small"  OnRowDataBound="grdOnlineMRPUploadedDocs_RowDataBound" >
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Document" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnViewDocument" runat="server">View</asp:LinkButton>
                            <ajaxToolkit:ModalPopupExtender ID="mp2" runat="server" PopupControlID="Panl2" TargetControlID="lnkBtnViewDocument"
                                CancelControlID="Button2" BackgroundCssClass="Background">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="Panl2" runat="server" CssClass="Popup" align="center" Style="display: none">
                                <iframe style="width: 800px; height: 560px;" id="irm2" runat="server"></iframe>
                                <br />
                                <asp:Button ID="Button2" runat="server" Text="Close" CssClass="btn btn-apps" />
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle ForeColor="Black" BackColor="White" Height="15px" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Tahoma" Font-Size="Larger"
                    ForeColor="White" Height="20px" />
                <AlternatingRowStyle BackColor="WhiteSmoke" Font-Names="Tahoma" Font-Size="8pt" Height="15px" />
            </asp:GridView>
            
        
        </div>
    </form>
</body>
</html>
