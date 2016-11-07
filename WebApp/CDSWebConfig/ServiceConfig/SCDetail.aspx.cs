using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CDSWebConfig.ServiceConfig.Class;
using System.IO;

namespace CDSWebConfig.ServiceConfig
{
    public partial class SCDetail : System.Web.UI.Page
    {
        //string xml_BusinessConfig_Path = System.Configuration.ConfigurationManager.AppSettings["ConfigSavePath"].ToString();
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
            if (Session["UserID"] == null || Session["UserName"] == null || Session["RoleID"] == null)
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
                //code参数
                string  code = Request.QueryString["code"];
                //id参数
                string business_id = Request.QueryString["id"];
                //codename参数
                string business_name = Request.QueryString["codename"];

                if (code != null && business_id != null)
                {
                    GetSCDetail(code, business_id);
                }

                ViewState["scModel"] = scModel; // 定义的开始.

                if (Session["RoleID"] != null)
                {
                    string roleid = Session["RoleID"].ToString();
                    FillOptByRole(roleid);
                }
                else
                {
                    FillOptByRole("3");
                }
                
                //填充页面
                FillWebPage(scModel);
            }
        }

        protected void b_Select_Click(object sender, EventArgs e)
        {
            lb_updatesuccess.Text = "";

            string code = tb_Bid.Text.Trim().Substring(0, 2);
            string business_id = tb_Bid.Text.Trim().Substring(2);

            //数据库提取信息
            GetSCDetail(code, business_id);

            ViewState["scModel"] = scModel; // 定义的开始.

            if (Session["RoleID"] != null)
            {
                string roleid = Session["RoleID"].ToString();
                FillOptByRole(roleid);
            }
            else
            {
                FillOptByRole("3");
            }

            //填充页面
            FillWebPage(scModel);
        }

        //1 超级管理员，可以进行查看、增加、修改、删除操作
        //2 高级用户，可以进行查看、增加、修改操作
        //3 普通用户
        protected void FillOptByRole(string role)
        {
            switch (role)
            {
                case "1":
                    rbl_ops.Items[0].Enabled = true;
                    rbl_ops.Items[1].Enabled = true;
                    b_delete.Visible = true;
                    break;

                case "2":
                    rbl_ops.Items[0].Enabled = true;
                    rbl_ops.Items[1].Enabled = true;
                    b_delete.Visible = false;
                    break;

                case "3":
                    rbl_ops.Items[0].Enabled = true;
                    rbl_ops.Items[1].Enabled = false;
                    b_delete.Visible = false;
                    break;
            }
        }


        /// <summary>
        /// 填充页面
        /// </summary>
        /// <param name="scModel"></param>
        protected void FillWebPage(SCModel scModel)
        {
            if (scModel.ServiceInfo == null)
            {
                FillWebPanel("-1");
            }
            else
            {
                tb_Bid.Text = scModel.ServiceInfo.ServiceID;
                lb_BType.Text = scModel.ServiceInfo.ServiceTypeName;
                tb_ServiceName.Text = scModel.ServiceInfo.ServiceName;
                tb_SourceID.Text = scModel.ServiceInfo.SourceID;
                //填充所属业务群
                //先取消所有选中项
                chkblServiceGroup.ClearSelection();
                //获取数据库中业务群信息
                string servicegroup = scModel.ServiceInfo.ServiceGroup;
                //若不为空,则进行填充
                if (servicegroup.Length > 0)
                {
                    string[] servicegroupsingle = servicegroup.Split(',');
                    for (int i = 0; i < servicegroupsingle.Length; i++)
                    {
                        if (servicegroupsingle[i].Substring(0, 1) == "0")
                        {
                            servicegroupsingle[i] = servicegroupsingle[i].Substring(1);
                        }
                        chkblServiceGroup.Items[int.Parse(servicegroupsingle[i])].Selected = true;
                    }

                }

                //数据源类型
                if (scModel.DataSourceType == "0")
                {
                    DataSourceCollection dsc = (DataSourceCollection)scModel.DataSource;
                    lb_datasource.Text = scModel.DataSourceName;
                    tb_DataSourceDescription.Text = dsc.CollectionDescription;
                    tb_Des_1.Text = dsc.StartTimeValue;
                    tb_Des_2.Text = dsc.PeriodValue;
                    CreateCorN(scModel.DataSourceType);
                }
                //数据源类型
                else if (scModel.DataSourceType == "1")
                {
                    DataSourceNotify dsn = (DataSourceNotify)scModel.DataSource;
                    lb_datasource.Text = scModel.DataSourceName;
                    tb_DataSourceDescription.Text = dsn.NotifyDescription;
                    tb_Des_1.Text = dsn.PathsValue;
                    tb_Des_2.Text = dsn.FilesValue;
                    CreateCorN(scModel.DataSourceType);
                }
                else
                {
                    DataSourceOuter dso = (DataSourceOuter)scModel.DataSource;
                    lb_datasource.Text = scModel.DataSourceName;
                    tb_DataSourceDescription.Text = dso.OuterDescription;
                    tb_Des_1.Text = dso.OuterPathValue;
                    tb_Des_2.Text = dso.OuterFileValue;
                    CreateCorN(scModel.DataSourceType);
                }

                int link_count = scModel.SCLinks.Count;

                if (link_count != 0)
                {
                    tb_LinkID.Text = (Convert.ToDouble(scModel.SCLinks[link_count - 1].LinkID) + 1).ToString();
                }
                else
                {
                    tb_LinkID.Text = scModel.ServiceInfo.ServiceID + "000";
                }
                
                //tb_Description.Text = scModel.SCLinks[0].Description;
                //tb_Order.Text = scModel.SCLinks[0].Order;
                //tb_TopicLink.Text = scModel.SCLinks[0].TopicLink;
                tb_Description.Text = "";
                tb_Order.Text = "";
                tb_TopicLink.Text = "";
                gvItem.DataSource = scModel.SCLinks;
                gvItem.DataBind();

                lb_hide_linkid.Text = tb_LinkID.Text;

                //FillOptByRole("0");


                FillWebPanel(rbl_ops.SelectedValue);
                EnableTextBox(rbl_ops.SelectedValue);
                VisibleGridViewOpt(rbl_ops.SelectedValue);
            }
        }

        /// <summary>
        /// 填充页面
        /// 空，没用对象填充
        /// 有值，查看
        /// 有值，修改
        /// </summary>
        /// <param name="flag"></param>
        protected void FillWebPanel(string flag)
        {
            scModel = ViewState["scModel"] as SCModel;

            switch (flag)
            {
                //空，没用对象填充
                case "-1":
                    //p_opt.Visible = false;
                    p_info.Visible = false;
                    p_LinkList.Visible = false;
                    p_Link.Visible = false;
                    p_Button.Visible = false;
                    break;
                //有值，查看
                case "0":
                    //p_opt.Visible = true;
                    p_info.Visible = true;
                    p_LinkList.Visible = true;
                    p_Link.Visible = false;
                    p_Button.Visible = false;
                    break;
                //有值，修改
                case "1":
                    //p_opt.Visible = true;
                    p_info.Visible = true;
                    p_Button.Visible = true;
                    //外部
                    if (scModel.DataSourceType == "2")
                    {
                        p_Link.Visible = false;
                        p_LinkList.Visible = false;
                        b_Save.Enabled = true;
                        b_Add.Enabled = false;
                        b_Update.Enabled = false;
                    }
                    //非外部(采集和监控)
                    else
                    {
                        p_LinkList.Visible = true;
                        p_Link.Visible = true;
                        b_Save.Enabled = true;
                        b_Add.Enabled = true;
                    }
                    break;
            }
        }
        /// <summary>
        /// 根据数据源（采集、监控）填充各TextBox和Tip信息
        /// 0，采集
        /// 1，监控
        /// 2, 外部
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
                lb_Des_1.Text = s_OuterDir_Label;
                lb_Des_1_Tip.Text = s_OuterDir_Tip;
                lb_Des_2.Text = s_Outer_Label;
                lb_Des_2_Tip.Text = s_Outer_Tip;

                //20160727 取消外部业务不能为空校验，可以为0个，即为空
                r_Des_2_Verify_Null.Enabled = false;
                r_Des_1_Verify_Null.Enabled = false;
            }
        }

        /// <summary>
        /// 增加按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void b_Add_Click(object sender, EventArgs e)
        {
            lb_LinkExist.Visible = false;

            scModel = ViewState["scModel"] as SCModel;

            if (scModel.SCLinks == null)
            {
                scModel.SCLinks = new List<SCLink>();
            }

            SCLink scLink = new SCLink();
            scLink.LinkID = tb_LinkID.Text.Trim();
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
        /// 更新按钮事件
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
            //        scModel.SCLinks[i] = scLinkUpdate;     //用的新的Link覆盖原有Link
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
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void b_Save_Click(object sender, EventArgs e)
        {
            scModel = ViewState["scModel"] as SCModel;


            ServiceInfo si = new ServiceInfo();
            si.ServiceName = tb_ServiceName.Text.Trim();
            si.ServiceID = scModel.ServiceInfo.ServiceID;
            si.ServiceType = scModel.ServiceInfo.ServiceID.Substring(0, 2) ;
            //上游业务ID
            si.SourceID = tb_SourceID.Text.Trim();
            //所属业务群
            string servicegroup = "";
            string sort = "";
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
            if (scModel.DataSourceType == "0")
            {
                //scModel.DataSourceType = scModel.DataSourceType;
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
            else if (scModel.DataSourceType == "1")
            {
                //scModel.DataSourceType = scModel.DataSourceType;
                DataSourceNotify dsn = new DataSourceNotify();
                //dsn.DataSourceType = ddl_Data_Type.SelectedValue;//监控，则存1
                dsn.NotifyDescription = tb_DataSourceDescription.Text.Trim();
                dsn.PathsValue = tb_Des_1.Text.Trim();
                dsn.FilesValue = tb_Des_2.Text.Trim();
                dsn.PathDescription = s_FileDir_Tip;     //"要监控的文件目录";
                dsn.FilesDescription = s_File_Tip;      // "需要的数据源文件，可用${yyyyMMddHH}时间通配符";

                //20160308 syy 增加多目录，多文件，用；隔开，有空，用null表示；
                dsn.GetPathFileMateFromString(dsn.PathsValue, dsn.FilesValue, dsn.PathDescription, dsn.FilesDescription); //20160308 syy 
                
                scModel.DataSource = dsn;
            }
            //外部类数据源
            else if (scModel.DataSourceType == "2")
            {
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
            
            bool dbsave = false;

            
            //采集数据
            if (scModel.DataSourceType == "0")
            {
                SCDB scDB = new SCDB();
                dbsave = scDB.UpdateSCCollectionModel(scModel, user);
                SCCollectionXML sccxml = new SCCollectionXML();
                sccxml.CreateXML(scModel, xml_BusinessConfig_Path);// "d:/template/"
            }
            //监控数据
            else if (scModel.DataSourceType == "1")
            {
                SCDB scDB = new SCDB();
                dbsave = scDB.UpdateSCNotifyModel(scModel, user);
                SCNotifyXML scnxml = new SCNotifyXML();
                scnxml.CreateXML(scModel, xml_BusinessConfig_Path); // "d:/template/"
            }
            //外部数据
            else if (scModel.DataSourceType == "2")
            {
                SCDB scDB = new SCDB();
                dbsave = scDB.UpdateSCOuterModel(scModel, user);
                SCOuterXML scnxml = new SCOuterXML();
                scnxml.CreateXML(scModel, xml_BusinessConfig_Path); // "d:/template/"
            }

            //判断是否成功，跳转页面
            string filesave = xml_BusinessConfig_Path + scModel.ServiceInfo.ServiceID + ".xml";
            if (dbsave && File.Exists(filesave))
            {
                lb_updatesuccess.Text = "业务更新成功！";
            }
            else
            {
                lb_updatesuccess.Text = "业务更新失败！";
            }
        }

        /// <summary>
        /// 删除事件
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

                for (int i = 0; i < scLinks.Count; i++)
                {
                    if (i >= linkid_1)
                    {
                        scLinks[i].LinkID = (Convert.ToDouble(scLinks[i].LinkID) - 1).ToString();
                    }
                }

                lb_hide_linkid.Text = (Convert.ToDouble(lb_hide_linkid.Text) - 1).ToString();
                //resetLinkTB();

                gvItem.DataSource = scModel.SCLinks;
                gvItem.DataBind();
            }
            
        }

        protected void EnableTextBox(string flag)
        {
            switch (flag)
            {
                //有值，查看
                case "0":
                    EnabledTB(false);
                    break;
                //有值，修改
                case "1":
                    EnabledTB(true);
                    break;
            }
        }

        protected void EnabledTB(bool flag)
        {
            tb_ServiceName.Enabled = flag;
            tb_DataSourceDescription.Enabled = flag;
            tb_Des_1.Enabled = flag;
            tb_Des_2.Enabled = flag;
            tb_Description.Enabled = flag;
            tb_LinkID.Enabled = flag;
            tb_Order.Enabled = flag;
            tb_TopicLink.Enabled = flag;
            tb_SourceID.Enabled = flag;
            chkblServiceGroup.Enabled = flag;
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

        protected void rbl_ops_SelectedIndexChanged(object sender, EventArgs e)
        {
            lb_updatesuccess.Text = "";
            scModel = ViewState["scModel"] as SCModel;
            if (scModel !=null && scModel.ServiceInfo != null)
            {
                FillWebPanel(rbl_ops.SelectedValue);
                EnableTextBox(rbl_ops.SelectedValue);
                VisibleGridViewOpt(rbl_ops.SelectedValue);
            }
            
        }

        protected void VisibleGridViewOpt(string flag)
        {
            switch (flag)
            {
                //有值，查看
                case "0":
                    gvItem.Columns[4].Visible = false;
                    gvItem.Columns[5].Visible = false;
                    break;
                //有值，修改
                case "1":
                    gvItem.Columns[4].Visible = true;
                    gvItem.Columns[5].Visible = true;
                    break;
            }
        }

        /// <summary>
        /// 数据库信息提取
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="business_id">业务id</param>
        protected void GetSCDetail(string code, string business_id)
        {
            //SCModel scModel = new SCModel();
            SCDB odb = new SCDB();
            DataTable dt_businesstype = new DataTable();
            dt_businesstype = odb.GetBTNameByBusinessCode(code);

            DataTable dt_business = new DataTable();
            dt_business = odb.GetSCDetailByBusinessCode(code, business_id);

            if (dt_business != null && dt_business.Rows.Count == 1)
            {
                ServiceInfo si = new ServiceInfo();
                si.GetServiceInfoFromDT(dt_business);
                si.ServiceTypeName = dt_businesstype.Rows[0][1].ToString();
                scModel.ServiceInfo = si;
                scModel.DataSourceType = dt_business.Rows[0][3].ToString();
                if (scModel.DataSourceType == "0")
                {
                    DataSourceCollection dsc = new DataSourceCollection();
                    dsc.GetDataSourceFromDT(dt_business);
                    scModel.DataSource = dsc;
                    scModel.DataSourceName = "采集";
                }
                else if (scModel.DataSourceType == "1")
                {
                    DataSourceNotify dsn = new DataSourceNotify();
                    dsn.GetDataSourceFromDT(dt_business);
                    scModel.DataSource = dsn;
                    scModel.DataSourceName = "监控";
                }
                else
                {
                    DataSourceOuter dso = new DataSourceOuter();
                    dso.GetDataSourceFromDT(dt_business);
                    scModel.DataSource = dso;
                    scModel.DataSourceName = "外部";
                }
            }

            DataTable dt_links = new DataTable();
            dt_links = odb.GetSCLinksByServiceID(code, business_id);

            List<SCLink> scList = new List<SCLink>();
            for (int i = 0; i < dt_links.Rows.Count; i++)
            {
                SCLink scLink = new SCLink();
                scLink.LinkID = dt_links.Rows[i][0].ToString();
                scLink.Description = dt_links.Rows[i][1].ToString();
                scLink.Order = dt_links.Rows[i][2].ToString();
                scLink.TopicLink = dt_links.Rows[i][3].ToString();
                scLink.NextLinks = scLink.ChangeToTopicLinkList(); //变成list，以便生成xml
                scList.Add(scLink);
            }
            scModel.SCLinks = scList;

            //return scModel;
        }


        /// <summary>
        /// 每条信息选择后填充textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvItem_SelectedIndexChanged1(object sender, EventArgs e)
        {
            scModel = ViewState["scModel"] as SCModel;

            //将信息填充至Link环节的各个TextBox中
            int index = gvItem.SelectedIndex;
            string linkid = gvItem.SelectedDataKey.Values["LinkID"].ToString();

            int scLinkIndex = 0;

            List<SCLink> scLinks = scModel.SCLinks;
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

        protected void b_delete_Click(object sender, EventArgs e)
        {
            lb_updatesuccess.Text = "";
            string bid = tb_Bid.Text.Trim();
            if (bid != "")
            {
                try
                {
                    string code = bid.Substring(0, 2);
                    string business_id = bid.Substring(2, bid.Length - 2);
                    SCDB odb = new SCDB();
                    bool success = odb.DeleteBusiness(code, business_id, user);

                    //string filename = xml_BusinessConfig_Path + bid + ".xml";

                    if (success)
                    {
                        //删除文件
                        DeleteBFile(bid);

                        FillWebPanel("-1");
                        ViewState["scModel"] = null;
                        lb_updatesuccess.Text = bid + "删除成功！";
                    }
                }
                catch
                {
                    lb_updatesuccess.Text = "删除失败！";
                }
            }
            
        }

        protected void DeleteBFile(string businessID)
        {
            string filename = xml_BusinessConfig_Path + businessID + ".xml";
            if (File.Exists(filename))
            {
                //如果存在则删除
                File.Delete(filename);
            } 
        }

        //protected void tb_LinkID_TextChanged(object sender, EventArgs e)
        //{
        //    foreach (SCLink var in scModel.SCLinks)
        //    {
        //        if (var.LinkID == tb_LinkID.Text.Trim())
        //        {
        //            lb_LinkExist.Visible = true;
        //            return;
        //        }
        //    }
        //}

    }

    class BR : Control
    {
        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<br />");
        }

    }
}