using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CDSWebConfig.ServiceConfig.Class;
using System.Data;

namespace CDSWebConfig.ServiceConfig
{
    public partial class OptList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string ss = Session["RoleID"].ToString();
            //1 超级管理员，可以进行查看、增加、修改、删除操作
            //2 高级用户，可以进行查看、增加、修改操作
            //3 普通用户
            if (Session["RoleID"] == null || Session["RoleID"].ToString() != "1"
             || Session["UserID"] == null || Session["UserName"] == null)
            {
                //string ss = Session["RoleID"].ToString();
                Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('您没有权限，请联系最高管理员！');window.location.href='/Account/Login.aspx';</script>");
                return;
            }

            if (!IsPostBack)
            {
                fillGVList();
            }
        }

        protected void b_Select_Click(object sender, EventArgs e)
        {
            string businessid = tb_Bid.Text.Trim();
            fillGVListByBusinessID(businessid);
        }

        /// <summary>
        /// 填充gridview
        /// </summary>
        /// <param name="code"></param>
        /// <param name="datatype"></param>
        protected void fillGVList()
        {
            SCDB scDB = new SCDB();
            DataTable dt = new DataTable();
            dt = scDB.GetOptListAll();
            gvItem.DataSource = dt;
            gvItem.DataBind();
        }

        /// <summary>
        /// 填充gridview
        /// </summary>
        /// <param name="code"></param>
        /// <param name="datatype"></param>
        protected void fillGVListByBusinessID(string businessid)
        {
            SCDB scDB = new SCDB();
            DataTable dt = new DataTable();
            dt = scDB.GetOptByBusinessID(businessid);
            gvItem.DataSource = dt;
            gvItem.DataBind();
        }


        protected void gvItem_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("style", "word-break:break-all;word-warp:break-word");
                e.Row.Cells[1].Attributes.Add("style", "word-break:break-all;word-warp:break-word");
                e.Row.Cells[2].Attributes.Add("style", "word-break:break-all;word-warp:break-word");
                e.Row.Cells[3].Attributes.Add("style", "word-break:break-all;word-warp:break-word");
                e.Row.Cells[4].Attributes.Add("style", "word-break:break-all;word-warp:break-word");
                e.Row.Cells[5].Attributes.Add("style", "word-break:break-all;word-warp:break-word");
                e.Row.Cells[6].Attributes.Add("style", "word-break:break-all;word-warp:break-word");
            }
            
        }

        protected void gvItem_PageIndexChanged(object sender, EventArgs e)
        {
            string businessid = tb_Bid.Text.Trim();
            if (businessid != "" && businessid.Length == 7)
            {
                try
                {
                    fillGVListByBusinessID(businessid);
                }
                catch
                {
                    fillGVList();
                }
            }
            else
            {
                fillGVList();
            }
        }

        protected void gvItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //分页完成之前
            gvItem.PageIndex = e.NewPageIndex;
        }
    }
}