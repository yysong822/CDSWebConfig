<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SCAddFile.aspx.cs" Inherits="CDSWebConfig.ServiceConfig.SCAddFile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%; vertical-align:top;" >

        <tr>
            <td style="width:15%;">
                <asp:Label ID="Label6" runat="server" Text="业务种类："></asp:Label>
            </td>
            <td style="width:35%;">
                <asp:DropDownList ID="ddl_SCType" AutoPostBack="True"  runat="server" DataTextField="name" 
                    DataValueField="code" 
                    onselectedindexchanged="ddl_SCType_SelectedIndexChanged">
                </asp:DropDownList>

                <asp:Label ID="Label7" runat="server" Text="当前业务号：" ForeColor="Red"></asp:Label>
                <asp:Label ID="lb_BID" runat="server" Text="" ForeColor="Red"></asp:Label>
                    &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label13" runat="server" Text="业务配置文件："></asp:Label>
            </td>
            <td>
                <asp:FileUpload ID="fu_serviceconfig" Width="98%" runat="server" />
            </td>
            <td>
                <asp:Button ID="b_upload" runat="server" Text="上传" onclick="b_upload_Click" />
            </td>
            <td>
                <asp:Label ID="Label14" runat="server"  ForeColor="Blue" Text="文件名:7位业务号.xml"></asp:Label>
            </td>
        </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
</asp:Content>
