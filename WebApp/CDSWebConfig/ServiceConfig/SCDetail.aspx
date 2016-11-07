<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="SCDetail.aspx.cs" Inherits="CDSWebConfig.ServiceConfig.SCDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width: 100%; vertical-align: top;">
        <tr>
            <td>
                <table style="width: 100%;">
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
                        <td>
                            <asp:Label ID="lb_updatesuccess" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>

                        
                        <td style="text-align: right">
                            <asp:Label ID="Label13" runat="server" Text="操作方式：" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                        
                            <asp:RadioButtonList ID="rbl_ops" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"
                                Font-Bold="True" OnSelectedIndexChanged="rbl_ops_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="0">查看</asp:ListItem>
                                <asp:ListItem Value="1">修改</asp:ListItem>
                            </asp:RadioButtonList>
                            
                        </td>
                        <td>
                            <asp:Button ID="b_delete" runat="server" Text="删除" ForeColor="Red" 
                            Font-Bold="True"  OnClientClick="return confirm('该操作不可恢复，您确定要删除?')" 
                            OnClick="b_delete_Click" ValidationGroup="valGroup4"  />
                        </td>
                        
                        
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="p_info" runat="server">
                    <table style="width: 100%; vertical-align: top;">
                        <tr>
                            <td style="width: 15%;">
                                <asp:Label ID="Label6" runat="server" Text="业务种类："></asp:Label>
                            </td>
                            <td style="width: 50%;">
                                <asp:Label ID="lb_BType" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="业务名称："></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tb_ServiceName" runat="server" Width="98%"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="不允许为空！"
                                    ForeColor="Red" ControlToValidate="tb_ServiceName" ValidationGroup="valGroup3"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                <asp:Label ID="Label15" runat="server" Text="所属业务群："></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBoxList ID="chkblServiceGroup" runat="server" 
                                    RepeatDirection="Horizontal">
                                    <asp:ListItem>精细化预报</asp:ListItem>
                                    <asp:ListItem>站点实况</asp:ListItem>
                                    <asp:ListItem>指数</asp:ListItem>
                                    <asp:ListItem>其它</asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 15%;">
                                <asp:Label ID="lb_SourceID" runat="server" Text="上游业务ID："></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox Width="98%" ID="tb_SourceID" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="可0至多个，逗号分隔" ForeColor="Blue"></asp:Label>
                                </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="数据来源："></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_datasource" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="数据源信息描述："></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox Width="98%" ID="tb_DataSourceDescription" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="不允许为空！"
                                    ForeColor="Red" ValidationGroup="valGroup3" ControlToValidate="tb_DataSourceDescription"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lb_Des_1" runat="server" Text="采集开始时间："></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox Style="ime-mode: disabled;" Width="98%" ID="tb_Des_1" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lb_Des_1_Tip" runat="server" Text="hh:mm:ss" ForeColor="Blue"></asp:Label>
                                <asp:RequiredFieldValidator ID="r_Des_1_Verify_Null" runat="server" ErrorMessage="不允许为空！"
                                    ForeColor="Red" ValidationGroup="valGroup3" ControlToValidate="tb_Des_1"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="r_Des_1_Verify" runat="server" ControlToValidate="tb_Des_1"
                                    ErrorMessage="输入格式为：hh:mm:ss" ForeColor="Red" ValidationGroup="valGroup3" ValidationExpression="^([0-1]?[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lb_Des_2" runat="server" Text="采集轮询间隔："></asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <asp:TextBox Style="ime-mode: disabled;" Width="98%" ID="tb_Des_2" runat="server"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lb_Des_2_Tip" runat="server" Text="second,0s means just once" ForeColor="Blue"></asp:Label>
                                <asp:RequiredFieldValidator ID="r_Des_2_Verify_Null" runat="server" 
                            ErrorMessage="不允许为空！" ForeColor="Red" ValidationGroup="valGroup3"
                                ControlToValidate="tb_Des_2"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="r_Des_2_Verify" runat="server" ControlToValidate="tb_Des_2"
                                    ErrorMessage="只能输入0或者正整数" ForeColor="Red" ValidationGroup="valGroup3" ValidationExpression="^(0|[1-9][0-9]*)$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 50%; vertical-align: top">
                <asp:Panel ID="p_Link" runat="server">
                    <table style="width: 100%; vertical-align: top">
                        <tr>
                            <td style="width: 15%;">
                                <asp:Label ID="Label2" runat="server" Text="当前环节ID："></asp:Label>
                            </td>
                            <td style="width: 50%;">
                                <asp:TextBox Width="98%" ID="tb_LinkID" runat="server" Enabled="false" ></asp:TextBox>
                                <asp:Label ID="lb_hide_linkid" runat="server" Text="" Visible="false"></asp:Label>
                            </td>
                            <td style="width: 35%;">
                                <asp:Label ID="Label10" runat="server" Text="10位有效数字" ForeColor="Blue"></asp:Label>
                                &nbsp;&nbsp;&nbsp;
                                <asp:RegularExpressionValidator ID="r_linkid" runat="server" ControlToValidate="tb_LinkID"
                                    ErrorMessage="只能输入10位有效数字" ForeColor="Red" ValidationGroup="valGroup2" ValidationExpression="^\d{10}$"></asp:RegularExpressionValidator>
                                <asp:Label ID="lb_LinkExist" runat="server" Text="当前环节ID已存在！" ForeColor="Red" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="描述："></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox Width="98%" ID="tb_Description" runat="server" Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="不允许为空！"
                                    ForeColor="Red" ValidationGroup="valGroup2" ControlToValidate="tb_Description"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="命令行："></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox Width="98%" ID="tb_Order" runat="server" Enabled="false" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td>
                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                ControlToValidate="tb_Order" ErrorMessage="不允许为空！" 
                                ForeColor="Red" ValidationGroup="valGroup2"
                                ValidationExpression="^[^\s][\w\s-]+$"></asp:RegularExpressionValidator>--%>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="不允许为空！"
                                    ForeColor="Red" ValidationGroup="valGroup2" ControlToValidate="tb_Order"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="后续环节："></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox Style="ime-mode: disabled;" Width="98%" ID="tb_TopicLink" runat="server"
                                    Enabled="false"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="示例：(t2/1000000001),(t3/1000000002)"
                                    ForeColor="Blue"></asp:Label>
                                <br />
                                <asp:Label ID="Label12" runat="server" Text="结束时为空" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="p_Button" runat="server">
                <table style="width: 100%; vertical-align: top">
                    <tr>
                        <td style="width:15%">
                        </td>
                        <td style="width:50%">
                            <asp:Button ID="b_Add" runat="server" Text="增加" OnClick="b_Add_Click" Style="width: 40px"
                                ValidationGroup="valGroup2" />
                            <asp:Button ID="b_Update" runat="server" Text="修改" Enabled="false" Style="width: 40px"
                                OnClick="b_Update_Click" ValidationGroup="valGroup2" />
                            <asp:Button ID="b_Save" runat="server" Text="保存" OnClick="b_Save_Click" ValidationGroup="valGroup3" />
                        </td>
                        <td style="width:35%"></td>
                    </tr>
                </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="width: 50%; vertical-align: top">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Panel ID="p_LinkList" runat="server">
                                <asp:GridView ID="gvItem" Width="100%" DataKeyNames="LinkID" runat="server" AutoGenerateColumns="False"
                                    OnRowDeleting="gvItem_RowDeleting" BackColor="White" BorderColor="#999999" BorderStyle="Solid"
                                    BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" OnSelectedIndexChanged="gvItem_SelectedIndexChanged1">
                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                    <Columns>
                                        <asp:BoundField DataField="LinkID" HeaderStyle-Width="15%" HeaderText="当前环节ID">
                                            <HeaderStyle Width="10%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Description" HeaderStyle-Width="30%" HeaderText="描述">
                                            <HeaderStyle Width="20%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Order" HeaderStyle-Width="30%" HeaderText="命令行">
                                            <HeaderStyle Width="40%" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TopicLink" HeaderStyle-Width="30%" HeaderText="后续环节">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundField>
                                        <asp:CommandField SelectText="□" ShowSelectButton="True" />
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lb_delete" runat="server" CausesValidation="False" CommandName="Delete"
                                                    Text="×" OnClientClick='<%#  "if (!confirm(\"你确定要删除" + Eval("LinkID").ToString() + "吗?\")) return false;"%>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
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
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lb_UpdateIndex" runat="server" Text="0" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
