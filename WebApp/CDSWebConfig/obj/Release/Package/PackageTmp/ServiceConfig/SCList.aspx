<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SCList.aspx.cs" Inherits="CDSWebConfig.ServiceConfig.SCList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<table style="width:100%">
    <tr>
        <td  colspan="2">
            <table>
                <tr>
                    
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="业务种类："></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_SCType" AutoPostBack="True"  runat="server" DataTextField="name" 
                                DataValueField="code" 
                                onselectedindexchanged="ddl_SCType_SelectedIndexChanged">
                            </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="数据源类型："></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rbl_datatype" runat="server" Height="16px" 
                            RepeatDirection="Horizontal" AutoPostBack="True" 
                            onselectedindexchanged="rbl_datatype_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True">采集</asp:ListItem>
                            <asp:ListItem Value="1">监控</asp:ListItem>
                            <asp:ListItem Value="2">外部</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <%--<td>
                        <asp:Label ID="Label3" runat="server" Text="业务号：（7位数字）"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_serviceid" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="业务名称"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tb_servicename" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="b_select" runat="server" Text="查询" onclick="b_select_Click" />
                    </td>--%>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="width:90%;vertical-align:top; border:1px">
            <asp:GridView ID="gvItem" Width="100%" DataKeyNames="business_id"
                runat="server" AutoGenerateColumns="False" BackColor="White" 
                BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                ForeColor="Black" GridLines="Vertical" 
                onrowdatabound="gvItem_RowDataBound" >
                <AlternatingRowStyle BackColor="#CCCCCC" />
                <Columns>
                    <%--<asp:TemplateField HeaderText="业务种类" HeaderStyle-Width="10%">
                        <ItemTemplate >
                            <asp:Label ID="lb_code1" runat="server" Text='<%# BusinessName%>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> --%>
                    <asp:TemplateField  HeaderText="业务号" SortExpression="Title">
                    <HeaderStyle Width="15%"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lb_code" runat="server" Text='<%# Bind("code") %>'></asp:Label>
                            <asp:Label  ID="lb_bid" runat="server" Text='<%# Bind("business_id") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:BoundField DataField="business_id" HeaderStyle-Width="15%" HeaderText="业务号">
                        <HeaderStyle Width="15%" />
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="business_name" HeaderStyle-Width="20%" HeaderText="业务名称">
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="data_type" HeaderStyle-Width="8%" HeaderText="数据来源">
                    </asp:BoundField>--%>
                    <asp:BoundField DataField="data_desc" HeaderStyle-Width="20%" HeaderText="数据源描述">
                    </asp:BoundField>
                    <asp:TemplateField  >
                       <HeaderStyle Width="20%"></HeaderStyle>
                        <HeaderTemplate >
                            <%=NameA%>
                        </HeaderTemplate>
                        <ItemTemplate >
                        
                            <asp:Label style=" OVERFLOW-X: hidden; WORD-BREAK: break-all;  WORD-WRAP: break-word"  
                               Width="190px" ID="lb_1" runat="server" 
                                Text='<%#((string)DataBinder.Eval(Container.DataItem, "data_type ")) == "0" ? DataBinder.Eval(Container.DataItem, "start_time ") : DataBinder.Eval(Container.DataItem, "monitor_dir ")%>'>
                            </asp:Label> 
                       
                            
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField >
                        <HeaderStyle Width="20%"></HeaderStyle>
                        <HeaderTemplate >
                            <%=NameB%>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label style="OVERFLOW-X: hidden; WORD-BREAK: break-all;  WORD-WRAP: break-word"  
                                Width="190px" ID="lb_2" runat="server" 
                                Text='<%#((string)DataBinder.Eval(Container.DataItem, "data_type ")) == "0" ? DataBinder.Eval(Container.DataItem, "period_time ") : DataBinder.Eval(Container.DataItem, "monitor_file ")%>'>
                            </asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:HyperLinkField HeaderStyle-Width="5%" DataNavigateUrlFields="code,business_id" DataNavigateUrlFormatString="SCDetail.aspx?code={0}&id={1}&bname="
                    HeaderText="详情" Text="详情" />
                    
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
