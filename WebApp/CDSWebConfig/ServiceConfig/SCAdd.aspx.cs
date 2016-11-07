using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using CDSWebConfig.ServiceConfig.Class;
using System.IO;

namespace CDSWebConfig.ServiceConfig
{
    public partial class SCAdd : System.Web.UI.Page
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
        //string s_File_Label = "监控文件名称：";
        //string s_File_Tip = "需要的数据源文件，可用${yyyyMMddHH}时间通配符";

        ////string s_FileDir_Verify = "不允许为空！";
        ////string s_FileDir_VerifyExperssion = "^[^\\s][\\w\\s-]+$";
        ////string s_File_Verify = "不允许为空！";
        ////string s_File_VerifyExperssion = "^[^\\s][\\w\\s-]+$";

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

        string s_OuterDir_Label = System.Configuration.ConfigurationManager.AppSettings["OuterDir_Label"].ToString();
        string s_OuterDir_Tip = System.Configuration.ConfigurationManager.AppSettings["OuterDir_Tip"].ToString();
        string s_Outer_Label = System.Configuration.ConfigurationManager.AppSettings["Outer_Label"].ToString();
        string s_Outer_Tip = System.Configuration.ConfigurationManager.AppSettings["Outer_Tip"].ToString();

        //string xml_BusinessConfig_Path = "d:/template/";
        string xml_BusinessConfig_Path = System.Configuration.ConfigurationManager.AppSettings["ConfigSavePath"].ToString();
        SCModel scModel = new SCModel();

        User user = new User();
        protected void Page_Load(object sender, EventArgs e)
        {
            //1 超级管理员，可以进行查看、增加、修改、删除操作
            //2 高级用户，可以进行查看、增加、修改操作
            //3 普通用户
            if (Session["RoleID"] == null || Session["RoleID"].ToString() == "3"
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
                //Cache["nameID"] = "0001";
                
                //xml_BusinessConfig_Path 
                
                //填充业务种类
                FillDDLBusinessType();
                //获取根据业务种类当前业务ID
                string code = ddl_SCType.Items[0].Value;
                int bussiness_id = currentBID(code);


                //20160329 从cache中取业务号Business—ID
                string cache_business_id = (string)Cache["businessid"];
                //20160329 如果为空，则证明当前没有用户占用业务号Business—ID
                //20160329 将申请的业务号Business—ID插入cache中，占用该ID
                if (cache_business_id == null)
                {
                    Cache.Insert("businessid", bussiness_id.ToString());
                }
                else 
                {
                    //20160329 cache中存储的是最后一次的业务号，当前需要的 业务号在此基础上+1
                    bussiness_id = Convert.ToInt32(cache_business_id) + 1;
                    //20160329 将新的业务号插入
                    Cache.Insert("businessid", bussiness_id.ToString());

                }


                //填充当前业务IDLabel
                lb_BID.Text = ddl_SCType.SelectedValue.Trim() + bussiness_id.ToString();
                //根据数据源种类填充描述信息、开始时间、间隔时间、文件目录、文件等
                CreateCorN(ddl_Data_Type.SelectedValue);

                //将scModel存入ViewState
                ViewState["scModel"] = scModel; // 定义的开始.
            }
        }

        /// <summary>
        /// 将除Link以外的信息保存至变量scModel中，包括ServiceInfo，和DataSource
        /// 初始化LinkID TextBox，将要显示的ID存于隐藏label中
        /// scModel 存入ViewState
        /// Enable Link 环节
        /// Disable ServiceInfo和DataSource 环节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void b_SaveName_Click(object sender, EventArgs e)
        {
            ServiceInfo si = new ServiceInfo();
            si.ServiceName = tb_ServiceName.Text.Trim();
            si.ServiceID = lb_BID.Text.Trim();
            si.ServiceType = ddl_SCType.SelectedValue;
            //上游业务ID
            si.SourceID = tb_SourceID.Text.Trim();
            //所属业务群
            string servicegroup = "";
            string sort="";
            for (int i = 0; i < chkblServiceGroup.Items.Count; i++)
            {
                if (chkblServiceGroup.Items[i].Selected)
                {
                    sort = i.ToString();
                    if (i < 10)
                    {
                        sort = "0" + sort;
                    }
                    servicegroup = servicegroup + sort + ",";
                }
            }
            if (servicegroup.Length > 0)
            {
                servicegroup = servicegroup.Substring(0, servicegroup.LastIndexOf(','));
            }
            si.ServiceGroup = servicegroup;
            scModel.ServiceInfo = si;
            
            //采集类数据源
            if (ddl_Data_Type.SelectedValue == "0")
            {
                scModel.DataSourceType = ddl_Data_Type.SelectedValue;
                DataSourceCollection dsc = new DataSourceCollection();
                //dsc.DataSourceType = ddl_Data_Type.SelectedValue;  //采集，则存0
                dsc.CollectionDescription = tb_DataSourceDescription.Text.Trim();
                dsc.StartTimeValue = tb_Des_1.Text.Trim();
                dsc.PeriodValue = tb_Des_2.Text.Trim();
                dsc.StartTimeDescription = s_StartTime_Tip;    //"hh:mm:ss";
                dsc.PeriodDescription = s_PeriodTime_Tip;      // "second,0s means just once";
                scModel.DataSource = dsc;
            }
            //监控类数据源
            else if (ddl_Data_Type.SelectedValue == "1")
            {
                scModel.DataSourceType = ddl_Data_Type.SelectedValue;
                DataSourceNotify dsn = new DataSourceNotify();
                //dsn.DataSourceType = ddl_Data_Type.SelectedValue;//监控，则存1
                dsn.NotifyDescription = tb_DataSourceDescription.Text.Trim();
                dsn.PathsValue = tb_Des_1.Text.Trim();
                dsn.FilesValue = tb_Des_2.Text.Trim();
                dsn.PathDescription = s_FileDir_Tip;     //"要监控的文件目录";
                dsn.FilesDescription = s_File_Tip;      // "需要的数据源文件，可用${yyyyMMddHH}时间通配符";

                //20160308 syy 增加多目录，多文件，用；隔开，有空，用null表示；
                dsn.GetPathFileMateFromString(dsn.PathsValue, dsn.FilesValue, dsn.PathDescription, dsn.FilesDescription); //20160308 syy 
                
                //上游业务ID和所属业务群
                

                scModel.DataSource = dsn;
            }
            //外部类数据源
            else if (ddl_Data_Type.SelectedValue == "2")
            {
                scModel.DataSourceType = ddl_Data_Type.SelectedValue;
                DataSourceOuter dso = new DataSourceOuter();
                //dsn.DataSourceType = ddl_Data_Type.SelectedValue;//监控，则存1
                dso.OuterDescription = tb_DataSourceDescription.Text.Trim();
                dso.OuterPathValue = tb_Des_1.Text.Trim();
                dso.OuterFileValue = tb_Des_2.Text.Trim();
                dso.OuterPathDes = s_FileDir_Tip;     //"要监控的文件目录";
                dso.OuterFileDes = s_File_Tip;      // "需要的数据源文件，可用${yyyyMMddHH}时间通配符";

                //20160308 syy 增加多目录，多文件，用；隔开，有空，用null表示；
                //dso.GetPathFileMateFromString(dso.PathsValue, dso.FilesValue, dso.PathDescription, dso.FilesDescription); //20160308 syy 

                scModel.DataSource = dso;
            }

            ViewState["scModel"] = scModel; // scModel 存入ViewState

            //初始化LinkID TextBox，将要显示的ID存于隐藏label中s
            lb_hide_linkid.Text = (lb_BID.Text.Trim()) + "000";


            if (ddl_Data_Type.SelectedValue == "2")
            {
                b_Save.Enabled = true;
            }
            //Enable Link 环节
            else
            {
                EnabledLinkTB(true);
                b_Add.Enabled = true;
                ResetLinkTB(lb_hide_linkid.Text);
            }
            //Disable ServiceInfo和DataSource 环节
            EnabledSITB(false);
        }

        /// <summary>
        /// 增加一个Link环节
        /// //命令行没有进行Trim处理，因为命令中就可能存在空格
        /// 绑定gridview数据源
        /// 修改隐藏label，+1，以便赋值linkID
        /// 重置link系列TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void b_Add_Click(object sender, EventArgs e)
        {
            lb_LinkExist.Visible = false;

            scModel = ViewState["scModel"] as SCModel;

            if(scModel.SCLinks == null)
            {
                scModel.SCLinks = new List<SCLink>();
            }

            foreach (SCLink var in scModel.SCLinks)
            {
                if (var.LinkID == tb_LinkID.Text.Trim())
                {
                    lb_LinkExist.Visible = true;
                    return;
                }
            }

            SCLink scLink = new SCLink();
            scLink.LinkID = tb_LinkID.Text.Trim(); //20160201LinkID改为3位，增加
            scLink.Description = tb_Description.Text.Trim();
            scLink.Order = tb_Order.Text;  //命令行没有进行Trim处理，因为命令中就可能存在空格
            scLink.TopicLink = tb_TopicLink.Text.Trim();
            scLink.NextLinks = scLink.ChangeToTopicLinkList(); //变成list，以便生成xml
            scModel.SCLinks.Add(scLink);

            //绑定gridview数据源
            gvItem.DataSource = scModel.SCLinks;
            gvItem.DataBind();

            //修改隐藏label，+1，以便赋值linkID
            lb_hide_linkid.Text = (Convert.ToDouble(lb_hide_linkid.Text.Trim()) + 1).ToString();
            //重置link系列TextBox,为“”
            ResetLinkTB(lb_hide_linkid.Text);

            if (lb_hide_linkid.Text.Substring(7, 3) != "000")
            {
                b_Save.Enabled = true;
            }
        }

        /// <summary>
        /// 选择某一个link环节，进行修改操作
        /// 将信息填充至Link环节的各个TextBox中
        /// 增加按钮设置为Disable，修改按钮设置为Enable，保存按钮设置为Disable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            scModel = ViewState["scModel"] as SCModel;

            //将信息填充至Link环节的各个TextBox中
            int index = gvItem.SelectedIndex;
            string linkid = gvItem.SelectedDataKey.Values["LinkID"].ToString(); 

            List<SCLink> scLinks = scModel.SCLinks;
            
            int scLinkIndex = 0;

            foreach (SCLink scLink in scLinks)
            {
                if (scLink.LinkID == linkid)
                {
                    tb_LinkID.Text = scLink.LinkID;
                    tb_Description.Text = scLink.Description;
                    tb_Order.Text = scLink.Order;
                    tb_TopicLink.Text = scLink.TopicLink;

                    break;
                }
                scLinkIndex++;
            }

            lb_UpdateIndex.Text = scLinkIndex.ToString();

            if (linkid.EndsWith("000"))
            {
                tb_LinkID.Enabled = false;
            }
            else
            {
                tb_LinkID.Enabled = true;
            }


            //增加按钮设置为Disable，修改按钮设置为Enable，保存按钮设置为Disable
            ChangeUpdateToAdd(false);
        }

        /// <summary>
        /// 删除某一个Link
        /// 自动修正其他Link的ID顺序
        /// 修改隐藏label，+1，以便赋值linkID
        /// //绑定gridview
        /// //重置link系列TextBox,为“”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            scModel = ViewState["scModel"] as SCModel;

            string linkid = gvItem.DataKeys[e.RowIndex].Value.ToString();

            if (linkid.Substring(7, 3) == "000")
            {
                return;
            }
            else
            {
                List<SCLink> scLinks = scModel.SCLinks;

                //从对象中删除Link
                int linkid_1 = 0;
                for (int i = 0; i < scLinks.Count; i++)
                {
                    if (scLinks[i].LinkID == linkid)
                    {
                        scModel.SCLinks.Remove(scLinks[i]);
                        linkid_1 = i;
                        break;
                    }
                }

                //自动修正其他Link的ID顺序
                for (int i = 0; i < scLinks.Count; i++)
                {
                    if (i >= linkid_1)
                    {
                        scLinks[i].LinkID = (Convert.ToDouble(scLinks[i].LinkID) - 1).ToString();
                    }
                }

                //修改隐藏label，+1，以便赋值linkID
                lb_hide_linkid.Text = (Convert.ToDouble(lb_hide_linkid.Text) - 1).ToString();

                ////重置link系列TextBox,为“”
                ResetLinkTB(lb_hide_linkid.Text);

                //绑定gridview
                gvItem.DataSource = scModel.SCLinks;
                gvItem.DataBind();
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
            lb_BID.Text = code +  bussiness_id.ToString() ;
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
        /// 保存按钮
        /// 真正与数据库进行交互
        /// 保存至数据库，事务提交，更改两个表，业务表和环节表
        /// 生成XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void b_Save_Click(object sender, EventArgs e)
        {

            try
            {
                scModel = ViewState["scModel"] as SCModel;
                bool dbsave = false;

                

                //采集数据
                if (scModel.DataSourceType == "0")
                {
                    SCCollectionXML sccxml = new SCCollectionXML();
                    sccxml.CreateXML(scModel, xml_BusinessConfig_Path);// "d:/template/"

                    SCDB scDB = new SCDB();
                    dbsave = scDB.InsertSCCollectionModel(scModel, user);

                }
                //监控数据
                else if (scModel.DataSourceType == "1")
                {
                    SCNotifyXML scnxml = new SCNotifyXML();
                    scnxml.CreateXML(scModel, xml_BusinessConfig_Path); // "d:/template/"

                    SCDB scDB = new SCDB();
                    dbsave = scDB.InsertSCNotifyModel(scModel, user);

                }
                //外部数据
                else
                {
                    SCOuterXML scoxml = new SCOuterXML();
                    scoxml.CreateXML(scModel, xml_BusinessConfig_Path); // "d:/template/"

                    SCDB scDB = new SCDB();
                    dbsave = scDB.InsertSCOuterModel(scModel, user);
                }

                //判断是否成功，跳转页面
                string filesave = xml_BusinessConfig_Path + scModel.ServiceInfo.ServiceID + ".xml";
                if (dbsave && File.Exists(filesave))
                {
                    Response.Write("<script language=javascript>alert('业务保存成功！');window.location='/ServiceConfig/SCAdd.aspx'</script>");
                }
                else
                {
                    Response.Write("<script language=javascript>alert('业务保存失败！');window.location='/ServiceConfig/SCAdd.aspx'</script>");
                }

                //Cache.Remove("businessid");

                //Response.Redirect("/ServiceConfig/SCAdd.aspx");
                ////Disable Link 环节
                //EnabledLinkTB(false);
                //ResetLinkTB("");
                ////Enable ServiceInfo和DataSource 环节
                //EnabledSITB(true);
            }
            catch(Exception ex)
            {
                Server.Transfer("/Error.aspx");
                Server.ClearError();
            }
            
        }

        

        /// <summary>
        /// Enable Link 环节TextB
        /// </summary>
        protected void EnabledLinkTB(bool flag)
        {
            //resetLinkTB();
            tb_Description.Enabled = flag;
            tb_LinkID.Enabled = flag;
            tb_Order.Enabled = flag;
            tb_TopicLink.Enabled = flag;
        }

        /// <summary>
        /// 重置Link 环节TextB为“”
        /// </summary>
        protected void ResetLinkTB(string link_id)
        {
            tb_LinkID.Text = lb_hide_linkid.Text;
            tb_Description.Text = "";
            tb_Order.Text = "";
            tb_TopicLink.Text = "";
        }

        /// <summary>
        /// 设置ServiceInfo,和数据源，Disable
        /// </summary>
        protected void EnabledSITB(bool flag)
        {
            tb_ServiceName.Enabled = flag;
            ddl_SCType.Enabled = flag;
            b_SaveName.Enabled = flag;

            ddl_Data_Type.Enabled = flag;
            tb_DataSourceDescription.Enabled = flag;
            tb_Des_1.Enabled = flag;
            tb_Des_2.Enabled = flag;
            tb_SourceID.Enabled = flag;
            chkblServiceGroup.Enabled = flag;
        }

        /// <summary>
        /// 随数据源（采集或者监控）变更数据源Label和Tip等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddl_Data_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            string value = ddl_Data_Type.SelectedValue;
            CreateCorN(value);
        }

        
        /// <summary>
        /// 修改按钮
        /// 重新赋值某一Link
        /// 更新gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void b_Update_Click(object sender, EventArgs e)
        {

            scModel = ViewState["scModel"] as SCModel;

            SCLink scLinkUpdate = new SCLink();
            
            scLinkUpdate.LinkID = tb_LinkID.Text.Trim();
            scLinkUpdate.Description = tb_Description.Text.Trim();
            scLinkUpdate.Order = tb_Order.Text.Trim();
            scLinkUpdate.TopicLink = tb_TopicLink.Text.Trim();
            scLinkUpdate.NextLinks = scLinkUpdate.ChangeToTopicLinkList();//变成list，以便生成xml

            List<SCLink> scLinks = scModel.SCLinks;

            int updateindex = Convert.ToInt32(lb_UpdateIndex.Text);
            scModel.SCLinks[updateindex] = scLinkUpdate;//用的新的Link覆盖原有Link
            //for (int i = 0; i < scLinks.Count; i++)
            //{
            //    if (scLinks[i].LinkID == scLinkUpdate.LinkID)
            //    {
            //        scModel.SCLinks[i] = scLinkUpdate;     
            //        break;
            //    }
            //}
            
            //绑定girview
            gvItem.DataSource = scModel.SCLinks;
            gvItem.DataBind();

            //变为增加，true，增加按钮设置为Enable，修改按钮设置为Disable，保存按钮设置为Enable
            ChangeUpdateToAdd(true);

            //重置Link //从隐藏的label中取出linkID
            ResetLinkTB(lb_hide_linkid.Text);

        }

        /// <summary>
        /// 修改，false，增加按钮设置为Disable，修改按钮设置为Enable，保存按钮设置为Disable
        /// 增加，true，增加按钮设置为Enable，修改按钮设置为Disable，保存按钮设置为Enable
        /// </summary>
        /// <param name="flag"></param>
        protected void ChangeUpdateToAdd(bool flag)
        {
            b_Add.Enabled = flag;
            b_Update.Enabled = !flag;
            b_Save.Enabled = flag;
        }

        /// <summary>
        /// 根据数据源（采集、监控）填充各TextBox和Tip信息
        /// 0，采集
        /// 1，监控
        /// </summary>
        /// <param name="flag"></param>
        protected void CreateCorN(string flag)
        {
            //0，采集
            if (flag == "0")
            {
                lb_Des_1.Text = s_StartTime_Label;
                lb_Des_1_Tip.Text = s_StartTime_Tip;
                r_Des_1_Verify.Enabled = true;
                r_Des_1_Verify.ValidationExpression = s_StartTime_VerifyExperssion;
                r_Des_1_Verify.ErrorMessage = s_StartTime_Verify;
                lb_Des_2.Text = s_PeriodTime_Label;
                lb_Des_2_Tip.Text = s_PeriodTime_Tip;
                r_Des_2_Verify.Enabled = true;
                r_Des_2_Verify.ValidationExpression = s_PeriodTime_VerifyExpression;
                r_Des_2_Verify.ErrorMessage = s_PeriodTime_Verify;
                //20160727 增加采集不允许为空判断
                r_Des_2_Verify_Null.Enabled = true;
                r_Des_1_Verify_Null.Enabled = true;
            }
            //1,监控
            else if (flag == "1")
            {
                r_Des_1_Verify.Enabled = false;
                r_Des_2_Verify.Enabled = false;

                lb_Des_1.Text = s_FileDir_Label;
                lb_Des_1_Tip.Text = s_FileDir_Tip;
                //r_Des_1_Verify.ValidationExpression = s_FileDir_VerifyExperssion;
                //r_Des_1_Verify.ErrorMessage = s_FileDir_Verify;

                lb_Des_2.Text = s_File_Label;
                lb_Des_2_Tip.Text = s_File_Tip;
                //r_Des_2_Verify.ValidationExpression = s_File_VerifyExperssion;
                //r_Des_2_Verify.ErrorMessage = s_File_Verify;

                //20160727 增加监控不允许为空判断
                r_Des_2_Verify_Null.Enabled = true;
                r_Des_1_Verify_Null.Enabled = true;
            }
            //2,外部
            else
            {
                r_Des_1_Verify.Enabled = false;
                r_Des_2_Verify.Enabled = false;

                //20160727 取消外部业务不能为空校验，可以为0个，即为空
                r_Des_2_Verify_Null.Enabled = false;
                r_Des_1_Verify_Null.Enabled = false;

                lb_Des_1.Text = s_OuterDir_Label;
                lb_Des_1_Tip.Text = s_OuterDir_Tip;
                lb_Des_2.Text = s_Outer_Label;
                lb_Des_2_Tip.Text = s_Outer_Tip;

            }
        }

        //protected void b_upload_Click(object sender, EventArgs e)
        //{
        //    fu_serviceconfig.SaveAs(xml_BusinessConfig_Path + fu_serviceconfig.FileName);
        //    Label1.Text = "上传成功！";  
        //}


    }
}