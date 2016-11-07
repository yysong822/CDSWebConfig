<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="CDSWebConfig._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        欢迎使用核心数据平台业务配置系统
    </h2>
    <ul style=" position: absolute; ">
       <li style=" height: 50px; "> 增加业务配置 <a href="ServiceConfig/SCAdd.aspx" 
            title="Add Config">增加</a>.
       </li>

       <li style=" height: 50px; "> 增加业务配置（文件） <a href="ServiceConfig/SCAddFile.aspx"
            title="Add Config With File">增加（文件）</a>.
        </li>

       <li style=" height: 50px; "> 业务配置列表 <a href="ServiceConfig/SCList.aspx"
            title="Config List">列表</a>.
        </li>

        <li style=" height: 50px; "> 修改业务配置 <a href="ServiceConfig/SCDetail.aspx"
            title="Config Detail">详情</a>.
        </li>
    </ul>
    
</asp:Content>
