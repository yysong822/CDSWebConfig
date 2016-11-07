using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CDSWebConfig.ServiceConfig.Class;

namespace CDSWebConfig.ServiceConfig
{
    public partial class SCList : System.Web.UI.Page
    {
        //title1, 开始时间or文件目录
        protected string NameA = "";
        //title2, 间隔时间or文件名
        protected string NameB = "";
        //业务种类名称，暂时没有用到
        protected string BusinessName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //1 超级管理员，可以进行查看、增加、修改、删除操作
            //2 高级用户，可以进行查看、增加、修改操作
            //3 普通用户
            if (Session["UserID"] == null || Session["UserName"] == null || Session["RoleID"] == null)
            {
                Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('请您先登录！');window.location.href='/Account/Login.aspx';</script>");
                return;
            }

            if (!IsPostBack)
            {
                //填充业务种类
                fillDDLType();
                //填充列名
                GetColumnTitle();

                //
                string code = ddl_SCType.SelectedValue;
                string datatype = rbl_datatype.SelectedValue;
                //根据选择的业务种类和数据源填充gridview
                fillGVList(code, datatype);
            }
        }

        /// <summary>
        /// 填充列名
        /// NameA, 开始时间or监控目录
        /// NameB, 轮询间隔or监控文件
        /// CodeName, 当前业务种类ddl选择text
        /// </summary>
        protected void GetColumnTitle()
        {
            if (rbl_datatype.SelectedValue == "0")
            {
                NameA = "开始时间";
                NameB = "轮询间隔";
            }
            else if (rbl_datatype.SelectedValue == "1")
            {
                NameA = "监控目录";
                NameB = "监控文件";
            }
            else
            {
                NameA = "监控目录";
                NameB = "监控文件";
            }
            BusinessName = ddl_SCType.SelectedItem.Text;
        }

        /// <summary>
        /// 从数据库中提取数据种类
        /// </summary>
        protected void fillDDLType()
        {
            SCDB scDB = new SCDB();
            DataTable dt = scDB.GetSCTypeList();
            ddl_SCType.DataSource = dt;
            ddl_SCType.DataBind();
        }

        /// <summary>
        /// 填充gridview
        /// </summary>
        /// <param name="code"></param>
        /// <param name="datatype"></param>
        protected void fillGVList(string code, string datatype)
        {
            SCDB scDB = new SCDB();
            DataTable dt = new DataTable();
            dt = scDB.GetSCListByCodeType(code, datatype);
            gvItem.DataSource = dt;
            gvItem.DataBind();
        }

        /// <summary>
        /// 根据选择的ddl，填充gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_SCType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = ddl_SCType.SelectedValue;
            string datatype = rbl_datatype.SelectedValue;
            GetColumnTitle();
            fillGVList(code, datatype);
        }

        /// <summary>
        /// 根据选择的radio，填充gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbl_datatype_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = ddl_SCType.SelectedValue;
            string datatype = rbl_datatype.SelectedValue;
            GetColumnTitle();
            fillGVList(code, datatype);
        }

        /// <summary>
        /// 给链接增加业务种类名称，以减少详细信息中的数据库提取操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink h = e.Row.Cells[5].Controls[0] as HyperLink;
                h.NavigateUrl += ddl_SCType.SelectedItem.Text;
            }
        }

        protected void b_select_Click(object sender, EventArgs e)
        {

        }
    }
}