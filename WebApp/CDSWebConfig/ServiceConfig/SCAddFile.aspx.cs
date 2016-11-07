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
    public partial class SCAddFile : System.Web.UI.Page
    {


        //string s_StartTime_Label = "采集开始时间：";
        //string s_StartTime_Tip = "hh:mm:ss";
        //string s_StartTime_Verify = "输入格式为：hh:mm:ss";
        //string s_StartTime_VerifyExperssion = "^([0-1]?[0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$";
        //string s_PeriodTime_Label = "采集轮询间隔：";
        //string s_PeriodTime_Tip = "second,0s means just once";
        //string s_PeriodTime_Verify = "只能输入0或者正整数";
        //string s_PeriodTime_VerifyExpression = "^(0|[1-9][0-9]*)$";

        //string s_FileDir_Label = "监控文件目录：";
        //string s_FileDir_Tip = "要监控的文件目录";
        //string s_FileDir_Verify = "不允许为空！";
        //string s_FileDir_VerifyExperssion = "^[^\\s][\\w\\s-]+$";
        //string s_File_Label = "监控文件名称：";
        //string s_File_Tip = "需要的数据源文件，可用${yyyyMMddHH}时间通配符";
        //string s_File_Verify = "不允许为空！";
        //string s_File_VerifyExperssion = "^[^\\s][\\w\\s-]+$";

        string s_StartTime_Label = System.Configuration.ConfigurationManager.AppSettings["StartTime_Label"].ToString();
        string s_StartTime_Tip = System.Configuration.ConfigurationManager.AppSettings["StartTime_Tip"].ToString();
        string s_StartTime_Verify = System.Configuration.ConfigurationManager.AppSettings["StartTime_Verify"].ToString();
        string s_StartTime_VerifyExperssion = System.Configuration.ConfigurationManager.AppSettings["StartTime_VerifyExperssion"].ToString();
        string s_PeriodTime_Label = System.Configuration.ConfigurationManager.AppSettings["PeriodTime_Label"].ToString();
        string s_PeriodTime_Tip = System.Configuration.ConfigurationManager.AppSettings["PeriodTime_Tip"].ToString();
        string s_PeriodTime_Verify = System.Configuration.ConfigurationManager.AppSettings["PeriodTime_Verify"].ToString();
        string s_PeriodTime_VerifyExpression = System.Configuration.ConfigurationManager.AppSettings["PeriodTime_VerifyExpression"].ToString();

        string s_FileDir_Label = System.Configuration.ConfigurationManager.AppSettings["FileDir_Label"].ToString();
        string s_FileDir_Tip = System.Configuration.ConfigurationManager.AppSettings["FileDir_Tip"].ToString();
        string s_File_Label = System.Configuration.ConfigurationManager.AppSettings["File_Label"].ToString();
        string s_File_Tip = System.Configuration.ConfigurationManager.AppSettings["File_Tip"].ToString();
        

        //string xml_BusinessConfig_Path = "d:/template/";
        string xml_BusinessConfig_Path = System.Configuration.ConfigurationManager.AppSettings["ConfigSavePath"].ToString();
        User user = new User();

        protected void Page_Load(object sender, EventArgs e)
        {

            //1 超级管理员，可以进行查看、增加、修改、删除操作
            //2 高级用户，可以进行查看、增加、修改操作
            //3 普通用户
            if (Session["RoleID"] == null || Session["RoleID"].ToString() != "1"
             || Session["UserID"] == null || Session["UserName"] == null)
            {
                Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('请您先登录！');window.location.href='/Account/Login.aspx';</script>");
                return;
            }
            else
            {
                user.UserID = Session["UserID"].ToString();
                user.UserName = Session["UserName"].ToString();
            }

            if (!IsPostBack)
            {
                //xml_BusinessConfig_Path 

                //填充业务种类
                FillDDLBusinessType();
                //获取根据业务种类当前业务ID
                string code = ddl_SCType.Items[0].Value;
                int bussiness_id = currentBID(code);
                //填充当前业务IDLabel
                lb_BID.Text = ddl_SCType.SelectedValue.Trim() + bussiness_id.ToString();
                
            }
        }

        /// <summary>
        /// 从数据库中读取文件，填充业务种类ddl
        /// </summary>
        protected void FillDDLBusinessType()
        {
            SCDB scDB = new SCDB();
            DataTable dt = scDB.GetSCTypeList();
            ddl_SCType.DataSource = dt;
            ddl_SCType.DataBind();
        }

        /// <summary>
        /// 读取数据库，获取当前业务ID（5位）
        /// 读取数据库中该业务种类的最大值，如果有值，则+1
        /// 如果为-1，则该业务ID赋值为10000
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected int currentBID(string code)
        {
            SCDB scDB = new SCDB();
            int bussiness_id = scDB.GetMaxBID(code);
            if (bussiness_id == -1)
            {
                bussiness_id = 10000;
            }
            else
            {
                bussiness_id = bussiness_id + 1;
            }
            return bussiness_id;
        }

        /// <summary>
        /// 随选择变更当前业务ID
        /// 业务号：业务种类+业务ID（2位+5位）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_SCType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = ddl_SCType.SelectedValue;
            //currentBID,获取当前业务ID
            int bussiness_id = currentBID(code);
            //业务号：业务种类+业务ID（2位+5位）
            lb_BID.Text = code + bussiness_id.ToString();
        }

        protected void b_upload_Click(object sender, EventArgs e)
        {
            if (fu_serviceconfig.FileName != "")
            {
                int index = fu_serviceconfig.FileName.LastIndexOf('.');

                if (fu_serviceconfig.FileName.Substring(0, index) == lb_BID.Text)
                {
                    fu_serviceconfig.SaveAs(xml_BusinessConfig_Path + fu_serviceconfig.FileName);
                    SCXML scXML = new SCXML();
                    SCModel scModel = scXML.GetSCModel(xml_BusinessConfig_Path, fu_serviceconfig.FileName);
                    bool dbsave = false;
                    SCDB scDB = new SCDB();
                    //采集数据
                    if (scModel.DataSourceType == "0")
                    {
                        User user = new Class.User();
                        dbsave = scDB.InsertSCCollectionModel(scModel, user);
                    }
                    //监控数据
                    else
                    {
                        dbsave = scDB.InsertSCNotifyModel(scModel, user);
                    }
                    if (dbsave)
                    {
                        Label1.Text = "上传成功！";
                    }
                }
                else
                {
                    Label1.Text = "文件名须与业务ID一致！";
                }
            }
            else
            {
                Label1.Text = "请选择需上传的业务配置文件！";
            }
        }
    }
}