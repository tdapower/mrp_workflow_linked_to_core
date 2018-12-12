<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="MRPWorkflow_Documents_test" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export To PDF" />
            <asp:Button ID="btnSaveToDB" runat="server" OnClick="btnSaveToDB_Click" Text="Save to DB" />
            <asp:FileUpload ID="fileUploadDocument" runat="server" />
            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Save to DB" />
            <asp:Button ID="btnSendSms" runat="server" Text="SMS" OnClick="btnSendSms_Click" />
            <br />
            <br />
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
                DisplayGroupTree="False" Height="799px" ReportSourceID="CrystalReportSource1"
                Width="1095px" OnInit="CrystalReportViewer1_Init"></CR:CrystalReportViewer>
            <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                <Report FileName="~/MRPWorkflow/Documents/test.rpt">
                </Report>
            </CR:CrystalReportSource>
            <asp:GridView ID="gridDownload" CssClass="Gridview" runat="server" AutoGenerateColumns="false"
                DataKeyNames="DOCUMENT_ID">
                <HeaderStyle BackColor="#df5015" />
                <Columns>
                    <asp:BoundField DataField="DOCUMENT_ID" HeaderText="Id" />
                    <asp:BoundField DataField="DOCUMENT_TYPE" HeaderText="Type" />
                    <asp:TemplateField HeaderText="FilePath">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="lnkDownload_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
