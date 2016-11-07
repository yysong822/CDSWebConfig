<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OptList.aspx.cs" Inherits="CDSWebConfig.ServiceConfig.OptList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<table style="width:100%">
    <tr>
        <td style="width: 15%;">
            <asp:Label ID="Label7" runat="server" Text="业务号：" Font-Bold="True"></asp:Label>
            <asp:Label ID="Label14" runat="server" Text="（7位数字）" ForeColor="Blue"></asp:Label>
        </td>
        <td style="width: 50%;">
            <asp:TextBox ID="tb_Bid" runat="server" Width="98%"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="b_Select" runat="server" Text="查询" ValidationGroup="valGroup4" OnClick="b_Select_Click" />
        </td>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="不允许为空！"
                ForeColor="Red" ValidationGroup="valGroup4" ControlToValidate="tb_Bid"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="r_bid" runat="server" ControlToValidate="tb_Bid"
                ErrorMessage="只能输入7位有效数字" ForeColor="Red" ValidationGroup="valGroup4" ValidationExpression="^\d{7}$"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td style="vertical-align:top; border:1px" colspan="4" >
            <asp:GridView ID="gvItem"  EnableModelValidation="True"
                runat="server" AutoGenerateColumns="False" BackColor="White" 
                BorderColor="#999999" BorderStyle="Solid" 
                ForeColor="Black" GridLines="Vertical" onrowcreated="gvItem_RowCreated" 
                AllowPaging="True" onpageindexchanged="gvItem_PageIndexChanged" 
                onpageindexchanging="gvItem_PageIndexChanging" PageSize="10" 
                >
                <AlternatingRowStyle BackColor="#CCCCCC" />
                <Columns>
                    <asp:BoundField DataField="OperateBusinessID" HeaderText="业务ID">
                    <ItemStyle Width="8%" Wrap="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OperateName" HeaderText="操作名称">
                    <ItemStyle Width="7%" Wrap="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OperateUserID" HeaderText="用户ID">
                    <ItemStyle Width="5%" Wrap="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OperateUser" HeaderText="用户名称">
                    <ItemStyle Width="10%" Wrap="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OperateUserIP" HeaderText="用户IP">
                    <ItemStyle Width="8%" Wrap="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OperateSQL" HeaderText="SQL详情">
                    <ItemStyle Width="55%" Wrap="True" />
                    </asp:BoundField>
                    <asp:BoundField DataField="OperateDate" HeaderText="操作日期">
                    <ItemStyle Width="7%" Wrap="True" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>
        </td>
    </tr>
</table>

</asp:Content>
