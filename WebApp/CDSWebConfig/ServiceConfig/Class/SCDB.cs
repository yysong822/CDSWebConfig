using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using CDSWebConfig.DB;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;

namespace CDSWebConfig.ServiceConfig.Class
{
    public class SCDB
    {
        //public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        protected OracleConnection Connection;
        private string connectionString;

        public SCDB()
        {
            string connStr;
            connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ToString();
            connectionString = connStr;
            Connection = new OracleConnection(connectionString);
        }

        #region 打开数据库
        /// 打开数据库
        public void OpenConn()
        {
            if (this.Connection.State != ConnectionState.Open)
                this.Connection.Open();
        }
        #endregion

        #region 关闭数据库联接
        /// 关闭数据库联接
        public void CloseConn()
        {
            if (Connection.State == ConnectionState.Open)
                Connection.Close();
        }
        #endregion

        public DataTable GetSCTypeList()
        {
            OpenConn();

            string sql = "select * from dictionary.pmsc_business_type";

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql);
            dt = ds.Tables[0];

            CloseConn();

            return dt;
        }

        /// <summary>
        /// 获得最大业务ID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int GetMaxBID(string code)
        {
            OpenConn();

            string bid = "";
            int bidi = -1;
            string sql = "select Max(business_id) from dictionary.pmsc_business_info Where code=:code";

            OracleParameter[] parameters = {
                        new OracleParameter(":code", code),                     
                };

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql, parameters);
            dt = ds.Tables[0];

            if (dt != null && dt.Rows.Count != 0)
            {
                bid = dt.Rows[0][0].ToString();
                try
                {
                    bidi = Convert.ToInt32(bid);
                }
                catch
                {
                    bidi = -1;
                }
                
            }

            CloseConn();

            return bidi;
        }

        /// <summary>
        /// 插入采集信息
        /// </summary>
        /// <param name="scModel">scModel</param>
        /// <returns></returns>
        public bool InsertSCCollectionModel(SCModel scModel, User user)
        {
            OpenConn();

            bool result = false;

            OracleTransaction trans = Connection.BeginTransaction();

            List<string> strSql = new List<string> ();
            List<OracleParameter[]> parameters = new List<OracleParameter[]> ();

            StringBuilder strSqlService = new StringBuilder();
            strSqlService.Append("INSERT INTO dictionary.pmsc_business_info(");
            strSqlService.Append(" code, business_id, business_name, data_type, data_desc, start_time, period_time, pre_source, business_rac_info");
            strSqlService.Append(") VALUES (");
            strSqlService.Append(" :code, :business_id, :business_name, :data_type, :data_desc, :start_time, :period_time, :pre_source, :business_rac_info");
            strSqlService.Append(") ");

            OracleParameter[] parametersService = {
                        new OracleParameter(":code",  scModel.ServiceInfo.ServiceType),           
                        new OracleParameter(":business_id",  scModel.ServiceInfo.ServiceID.Substring(2)),           
                        new OracleParameter(":business_name",  scModel.ServiceInfo.ServiceName),    
                        new OracleParameter(":data_type",  scModel.DataSourceType), 
                        new OracleParameter(":data_desc",  ((DataSourceCollection)scModel.DataSource).CollectionDescription), 
                        new OracleParameter(":start_time",  ((DataSourceCollection)scModel.DataSource).StartTimeValue), 
                        new OracleParameter(":period_time", ((DataSourceCollection)scModel.DataSource).PeriodValue), 
                        new OracleParameter(":pre_source", scModel.ServiceInfo.SourceID),
                        new OracleParameter(":business_rac_info", scModel.ServiceInfo.ServiceGroup),
                };

            strSql.Add(strSqlService.ToString());
            parameters.Add(parametersService);


            foreach(SCLink scLink in scModel.SCLinks)
            {
                StringBuilder strSqlLink = new StringBuilder();
                strSqlLink.Append("INSERT INTO dictionary.pmsc_business_link(");
                strSqlLink.Append(" link_id, business_describe, cmd_info, topic_info");
                strSqlLink.Append(") VALUES (");
                strSqlLink.Append(" :link_id, :business_describe, :cmd_info, :topic_info");
                strSqlLink.Append(") ");

                OracleParameter[] parametersLink = {
                        new OracleParameter(":link_id",  scLink.LinkID),           
                        new OracleParameter(":business_describe",  scLink.Description),           
                        new OracleParameter(":cmd_info",  scLink.Order),   
                        new OracleParameter(":topic_info",  scLink.TopicLink),    
                };

                strSql.Add(strSqlLink.ToString());
                parameters.Add(parametersLink);
            }

            //20160808 syy 增加操作记录
            //User user = new User();
            this.InsertOptRecord(strSql, parameters, scModel.ServiceInfo.ServiceID, "Insert", user);

            result =  OracleHelper.ExecuteNonQuery(trans, CommandType.Text, strSql, parameters);

            CloseConn();

            return result;
        }

        /// <summary>
        /// 更新采集信息
        /// </summary>
        /// <param name="scModel">scModel</param>
        /// <returns></returns>
        public bool UpdateSCCollectionModel(SCModel scModel, User user)
        {
            OpenConn();

            bool result = false;

            OracleTransaction trans = Connection.BeginTransaction();

            List<string> strSql = new List<string>();
            List<OracleParameter[]> parameters = new List<OracleParameter[]>();

            StringBuilder strSqlService = new StringBuilder();
            strSqlService.Append("UPDATE dictionary.pmsc_business_info SET");
            strSqlService.Append(" business_name = :business_name, data_desc = :data_desc, start_time = :start_time, period_time = :period_time, pre_source = :pre_source, business_rac_info=:business_rac_info");
            strSqlService.Append(" WHERE");
            strSqlService.Append(" code= :code AND business_id = :business_id AND data_type=:data_type");

            OracleParameter[] parametersService = {
                        new OracleParameter(":business_name",  scModel.ServiceInfo.ServiceName),    
                        new OracleParameter(":data_desc",  ((DataSourceCollection)scModel.DataSource).CollectionDescription), 
                        new OracleParameter(":start_time",  ((DataSourceCollection)scModel.DataSource).StartTimeValue), 
                        new OracleParameter(":period_time", ((DataSourceCollection)scModel.DataSource).PeriodValue), 
                        new OracleParameter(":pre_source", scModel.ServiceInfo.SourceID),
                        new OracleParameter(":business_rac_info", scModel.ServiceInfo.ServiceGroup),
                        new OracleParameter(":code",  scModel.ServiceInfo.ServiceType),           
                        new OracleParameter(":business_id",  scModel.ServiceInfo.ServiceID.Substring(2)),           
                        new OracleParameter(":data_type",  scModel.DataSourceType), 
                        
                };

            strSql.Add(strSqlService.ToString());
            parameters.Add(parametersService);

            StringBuilder strSqlLinkDelete = new StringBuilder();

            strSqlLinkDelete.Append("DELETE FROM dictionary.pmsc_business_link");
            strSqlLinkDelete.Append(" WHERE SUBSTR(link_id,0,7) = :service_id");
            OracleParameter[] parametersDelete = {
                        new OracleParameter(":service_id",  scModel.ServiceInfo.ServiceID ),    
                };

            strSql.Add(strSqlLinkDelete.ToString());
            parameters.Add(parametersDelete);

            foreach (SCLink scLink in scModel.SCLinks)
            {
                StringBuilder strSqlLink = new StringBuilder();
                strSqlLink.Append("INSERT INTO dictionary.pmsc_business_link(");
                strSqlLink.Append(" link_id, business_describe, cmd_info, topic_info");
                strSqlLink.Append(") VALUES (");
                strSqlLink.Append(" :link_id, :business_describe, :cmd_info, :topic_info");
                strSqlLink.Append(") ");

                OracleParameter[] parametersLink = {
                        new OracleParameter(":link_id",  scLink.LinkID),           
                        new OracleParameter(":business_describe",  scLink.Description),           
                        new OracleParameter(":cmd_info",  scLink.Order),   
                        new OracleParameter(":topic_info",  scLink.TopicLink),    
                };

                strSql.Add(strSqlLink.ToString());
                parameters.Add(parametersLink);
            }

            //20160808 syy 增加操作记录
            //User user = new User();
            this.InsertOptRecord(strSql, parameters, scModel.ServiceInfo.ServiceID, "Update", user);

            result = OracleHelper.ExecuteNonQuery(trans, CommandType.Text, strSql, parameters);

            CloseConn();

            return result;
        }

        /// <summary>
        /// 插入监控信息
        /// </summary>
        /// <param name="scModel">scModel</param>
        /// <returns></returns>
        public bool InsertSCNotifyModel(SCModel scModel, User user)
        {
            OpenConn();

            bool result = false;

            OracleTransaction trans = Connection.BeginTransaction();

            List<string> strSql = new List<string>();
            List<OracleParameter[]> parameters = new List<OracleParameter[]>();

            StringBuilder strSqlService = new StringBuilder();
            strSqlService.Append("INSERT INTO dictionary.pmsc_business_info(");
            strSqlService.Append(" code, business_id, business_name, data_type, data_desc, monitor_dir, monitor_file, pre_source, business_rac_info");
            strSqlService.Append(") VALUES (");
            strSqlService.Append(" :code, :business_id, :business_name, :data_type, :data_desc, :monitor_dir, :monitor_file, :pre_source, :business_rac_info");
            strSqlService.Append(") ");

            OracleParameter[] parametersService = {
                        new OracleParameter(":code",  scModel.ServiceInfo.ServiceType),           
                        new OracleParameter(":business_id",  scModel.ServiceInfo.ServiceID.Substring(2)),           
                        new OracleParameter(":business_name",  scModel.ServiceInfo.ServiceName),    
                        new OracleParameter(":data_type",  scModel.DataSourceType), 
                        new OracleParameter(":data_desc",  ((DataSourceNotify)scModel.DataSource).NotifyDescription), 
                        new OracleParameter(":monitor_dir",  ((DataSourceNotify)scModel.DataSource).PathsValue), 
                        new OracleParameter(":monitor_file", ((DataSourceNotify)scModel.DataSource).FilesValue), 
                        new OracleParameter(":pre_source", scModel.ServiceInfo.SourceID),
                        new OracleParameter(":business_rac_info", scModel.ServiceInfo.ServiceGroup),
                };

            strSql.Add(strSqlService.ToString());
            parameters.Add(parametersService);


            foreach (SCLink scLink in scModel.SCLinks)
            {
                StringBuilder strSqlLink = new StringBuilder();
                strSqlLink.Append("INSERT INTO dictionary.pmsc_business_link(");
                strSqlLink.Append(" link_id, business_describe, cmd_info, topic_info");
                strSqlLink.Append(") VALUES (");
                strSqlLink.Append(" :link_id, :business_describe, :cmd_info, :topic_info");
                strSqlLink.Append(") ");

                OracleParameter[] parametersLink = {
                        new OracleParameter(":link_id",  scLink.LinkID),           
                        new OracleParameter(":business_describe",  scLink.Description),           
                        new OracleParameter(":cmd_info",  scLink.Order),   
                        new OracleParameter(":topic_info",  scLink.TopicLink),    
                };

                strSql.Add(strSqlLink.ToString());
                parameters.Add(parametersLink);
            }

            //20160808 syy 增加操作记录
            //User user = new User();
            this.InsertOptRecord(strSql, parameters, scModel.ServiceInfo.ServiceID, "Insert", user);

            result = OracleHelper.ExecuteNonQuery(trans, CommandType.Text, strSql, parameters);

            CloseConn();

            return result;
        }

        /// <summary>
        /// 更新监控信息
        /// </summary>
        /// <param name="scModel">scModel</param>
        /// <returns></returns>
        public bool UpdateSCNotifyModel(SCModel scModel, User user)
        {
            OpenConn();

            bool result = false;

            OracleTransaction trans = Connection.BeginTransaction();

            List<string> strSql = new List<string>();
            List<OracleParameter[]> parameters = new List<OracleParameter[]>();

            StringBuilder strSqlService = new StringBuilder();
            strSqlService.Append("UPDATE dictionary.pmsc_business_info SET");
            strSqlService.Append(" business_name = :business_name, data_desc = :data_desc, monitor_dir = :monitor_dir, monitor_file = :monitor_file, pre_source = :pre_source, business_rac_info=:business_rac_info");
            strSqlService.Append(" WHERE");
            strSqlService.Append(" code= :code AND business_id = :business_id AND data_type=:data_type");

            OracleParameter[] parametersService = {
                        new OracleParameter(":business_name",  scModel.ServiceInfo.ServiceName),    
                        new OracleParameter(":data_desc",  ((DataSourceNotify)scModel.DataSource).NotifyDescription), 
                        new OracleParameter(":monitor_dir",  ((DataSourceNotify)scModel.DataSource).PathsValue), 
                        new OracleParameter(":monitor_file", ((DataSourceNotify)scModel.DataSource).FilesValue), 
                        new OracleParameter(":pre_source", scModel.ServiceInfo.SourceID),
                        new OracleParameter(":business_rac_info", scModel.ServiceInfo.ServiceGroup),
                        new OracleParameter(":code",  scModel.ServiceInfo.ServiceType),           
                        new OracleParameter(":business_id",  scModel.ServiceInfo.ServiceID.Substring(2)),           
                        new OracleParameter(":data_type",  scModel.DataSourceType), 
                };

            strSql.Add(strSqlService.ToString());
            parameters.Add(parametersService);

            StringBuilder strSqlLinkDelete = new StringBuilder();

            strSqlLinkDelete.Append("DELETE FROM dictionary.pmsc_business_link");
            strSqlLinkDelete.Append(" WHERE SUBSTR(link_id,0,7) = :service_id");
            OracleParameter[] parametersDelete = {
                        new OracleParameter(":service_id",  scModel.ServiceInfo.ServiceID ),    
                };

            strSql.Add(strSqlLinkDelete.ToString());
            parameters.Add(parametersDelete);

            foreach (SCLink scLink in scModel.SCLinks)
            {
                StringBuilder strSqlLink = new StringBuilder();
                strSqlLink.Append("INSERT INTO dictionary.pmsc_business_link(");
                strSqlLink.Append(" link_id, business_describe, cmd_info, topic_info");
                strSqlLink.Append(") VALUES (");
                strSqlLink.Append(" :link_id, :business_describe, :cmd_info, :topic_info");
                strSqlLink.Append(") ");

                OracleParameter[] parametersLink = {
                        new OracleParameter(":link_id",  scLink.LinkID),           
                        new OracleParameter(":business_describe",  scLink.Description),           
                        new OracleParameter(":cmd_info",  scLink.Order),   
                        new OracleParameter(":topic_info",  scLink.TopicLink),    
                };

                strSql.Add(strSqlLink.ToString());
                parameters.Add(parametersLink);
            }

            //20160808 syy 增加操作记录
            //User user = new User();
            this.InsertOptRecord(strSql, parameters, scModel.ServiceInfo.ServiceID, "Update", user);

            result = OracleHelper.ExecuteNonQuery(trans, CommandType.Text, strSql, parameters);

            CloseConn();

            return result;
        }

        /// <summary>
        /// 插入外部信息
        /// </summary>
        /// <param name="scModel">scModel</param>
        /// <returns></returns>
        public bool InsertSCOuterModel(SCModel scModel, User user)
        {
            OpenConn();

            bool result = false;

            OracleTransaction trans = Connection.BeginTransaction();

            List<string> strSql = new List<string>();
            List<OracleParameter[]> parameters = new List<OracleParameter[]>();

            StringBuilder strSqlService = new StringBuilder();
            strSqlService.Append("INSERT INTO dictionary.pmsc_business_info(");
            strSqlService.Append(" code, business_id, business_name, data_type, data_desc, monitor_dir, monitor_file, pre_source, business_rac_info");
            strSqlService.Append(") VALUES (");
            strSqlService.Append(" :code, :business_id, :business_name, :data_type, :data_desc, :monitor_dir, :monitor_file, :pre_source, :business_rac_info");
            strSqlService.Append(") ");

            OracleParameter[] parametersService = {
                        new OracleParameter(":code",  scModel.ServiceInfo.ServiceType),           
                        new OracleParameter(":business_id",  scModel.ServiceInfo.ServiceID.Substring(2)),           
                        new OracleParameter(":business_name",  scModel.ServiceInfo.ServiceName),    
                        new OracleParameter(":data_type",  scModel.DataSourceType), 
                        new OracleParameter(":data_desc",  ((DataSourceOuter)scModel.DataSource).OuterDescription), 
                        new OracleParameter(":monitor_dir",  ((DataSourceOuter)scModel.DataSource).OuterPathValue), 
                        new OracleParameter(":monitor_file", ((DataSourceOuter)scModel.DataSource).OuterFileValue), 
                        new OracleParameter(":pre_source", scModel.ServiceInfo.SourceID),
                        new OracleParameter(":business_rac_info", scModel.ServiceInfo.ServiceGroup),
                };

            strSql.Add(strSqlService.ToString());
            parameters.Add(parametersService);


            /*foreach (SCLink scLink in scModel.SCLinks)
            {
                StringBuilder strSqlLink = new StringBuilder();
                strSqlLink.Append("INSERT INTO dictionary.pmsc_business_link(");
                strSqlLink.Append(" link_id, business_describe, cmd_info, topic_info");
                strSqlLink.Append(") VALUES (");
                strSqlLink.Append(" :link_id, :business_describe, :cmd_info, :topic_info");
                strSqlLink.Append(") ");

                OracleParameter[] parametersLink = {
                        new OracleParameter(":link_id",  scLink.LinkID),           
                        new OracleParameter(":business_describe",  scLink.Description),           
                        new OracleParameter(":cmd_info",  scLink.Order),   
                        new OracleParameter(":topic_info",  scLink.TopicLink),    
                };

                strSql.Add(strSqlLink.ToString());
                parameters.Add(parametersLink);
            }*/


            //20160808 syy 增加操作记录
            //User user = new User();
            this.InsertOptRecord(strSql, parameters, scModel.ServiceInfo.ServiceID, "Insert", user);

            result = OracleHelper.ExecuteNonQuery(trans, CommandType.Text, strSql, parameters);

            CloseConn();

            return result;
        }

        /// <summary>
        /// 更新外部信息
        /// </summary>
        /// <param name="scModel">scModel</param>
        /// <returns></returns>
        public bool UpdateSCOuterModel(SCModel scModel, User user)
        {
            OpenConn();

            bool result = false;

            OracleTransaction trans = Connection.BeginTransaction();

            List<string> strSql = new List<string>();
            List<OracleParameter[]> parameters = new List<OracleParameter[]>();

            StringBuilder strSqlService = new StringBuilder();
            strSqlService.Append("UPDATE dictionary.pmsc_business_info SET");
            strSqlService.Append(" business_name = :business_name, data_desc = :data_desc, monitor_dir = :monitor_dir, monitor_file = :monitor_file, pre_source = :pre_source, business_rac_info = :business_rac_info");
            //strSqlService.Append(" business_name = :business_name, data_desc = :data_desc, monitor_dir = :monitor_dir, monitor_file = :monitor_file");
            strSqlService.Append(" WHERE");
            strSqlService.Append(" code = :code AND business_id = :business_id AND data_type = :data_type");

            OracleParameter[] parametersService = {
                        new OracleParameter(":business_name",  scModel.ServiceInfo.ServiceName),    
                        new OracleParameter(":data_desc",  ((DataSourceOuter)scModel.DataSource).OuterDescription), 
                        new OracleParameter(":monitor_dir",  ((DataSourceOuter)scModel.DataSource).OuterPathValue), 
                        new OracleParameter(":monitor_file", ((DataSourceOuter)scModel.DataSource).OuterFileValue), 
                        new OracleParameter(":pre_source", scModel.ServiceInfo.SourceID),
                        new OracleParameter(":business_rac_info", scModel.ServiceInfo.ServiceGroup),
                        new OracleParameter(":code",  scModel.ServiceInfo.ServiceType),           
                        new OracleParameter(":business_id",  scModel.ServiceInfo.ServiceID.Substring(2)),           
                        new OracleParameter(":data_type",  scModel.DataSourceType), 
                };

            strSql.Add(strSqlService.ToString());
            parameters.Add(parametersService);

            /*StringBuilder strSqlLinkDelete = new StringBuilder();

            strSqlLinkDelete.Append("DELETE FROM dictionary.pmsc_business_link");
            strSqlLinkDelete.Append(" WHERE SUBSTR(link_id,0,7) = :service_id");
            OracleParameter[] parametersDelete = {
                        new OracleParameter(":service_id",  scModel.ServiceInfo.ServiceID ),    
                };

            strSql.Add(strSqlLinkDelete.ToString());
            parameters.Add(parametersDelete);

            foreach (SCLink scLink in scModel.SCLinks)
            {
                StringBuilder strSqlLink = new StringBuilder();
                strSqlLink.Append("INSERT INTO dictionary.pmsc_business_link(");
                strSqlLink.Append(" link_id, business_describe, cmd_info, topic_info");
                strSqlLink.Append(") VALUES (");
                strSqlLink.Append(" :link_id, :business_describe, :cmd_info, :topic_info");
                strSqlLink.Append(") ");

                OracleParameter[] parametersLink = {
                        new OracleParameter(":link_id",  scLink.LinkID),           
                        new OracleParameter(":business_describe",  scLink.Description),           
                        new OracleParameter(":cmd_info",  scLink.Order),   
                        new OracleParameter(":topic_info",  scLink.TopicLink),    
                };

                strSql.Add(strSqlLink.ToString());
                parameters.Add(parametersLink);
            }*/

            //20160808 syy 增加操作记录
            //User user = new User();
            this.InsertOptRecord(strSql, parameters, scModel.ServiceInfo.ServiceID, "Update", user);

            result = OracleHelper.ExecuteNonQuery(trans, CommandType.Text, strSql, parameters);
            CloseConn();

            return result;
        }
        
        /// <summary>
        /// 获得业务列表
        /// </summary>
        /// <param name="code"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public DataTable GetSCListByCodeType(string code, string datatype)
        {
            string sql = string.Empty;
            OpenConn();

            sql = "select * from dictionary.pmsc_business_info where code = :code and data_type = :datatype order by business_id asc";

            OracleParameter[] parameters = {
                        new OracleParameter(":code",  code),         
                        new OracleParameter(":datatype",  datatype),    
                };

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql, parameters);
            dt = ds.Tables[0];

            CloseConn();

            return dt;
        }

        /// <summary>
        /// 获得业务细节
        /// </summary>
        /// <param name="code"></param>
        /// <param name="business_id"></param>
        /// <returns></returns>
        public DataTable GetSCDetailByBusinessCode(string code, string business_id)
        {
            string sql = string.Empty;
            OpenConn();

            sql = "select * from dictionary.pmsc_business_info where (code=:code and business_id=:business_id) ";

            OracleParameter[] parameters = {
                        new OracleParameter(":code",  code),      
                        new OracleParameter(":business_id",  business_id),  
                        
                };

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql, parameters);
            dt = ds.Tables[0];

            CloseConn();

            return dt;
        }

        /// <summary>
        /// 获得业务名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetBTNameByBusinessCode(string code)
        {
            string sql = string.Empty;
            OpenConn();

            sql = "select * from dictionary.pmsc_business_type where code=:code";

            OracleParameter[] parameters = {
                        new OracleParameter(":code",  code),                           
                };

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql, parameters);
            dt = ds.Tables[0];

            CloseConn();

            return dt;
        }

        /// <summary>
        /// 获得环节信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="business_id"></param>
        /// <returns></returns>
        public DataTable GetSCLinksByServiceID(string code, string business_id)
        {
            string serviceid = code + business_id;
            string sql = string.Empty;
            OpenConn();

            sql = "select * from dictionary.pmsc_business_link where substr(link_id,0,7)=:serviceid order by link_id asc";

            OracleParameter[] parameters = {
                        new OracleParameter(":serviceid",  serviceid),                
                };

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql, parameters);
            dt = ds.Tables[0];

            CloseConn();

            return dt;
        }
        
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public DataTable GetListForwhere(string sql, params OracleParameter[] commandParameters)
        {
            //string sql = string.Empty;
            OpenConn();

            //sql = "select * from dictionary.pmsc_dispatch_infomation";
            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql, commandParameters);
            dt = ds.Tables[0];

            CloseConn();

            return dt;
        }


        public bool DeleteBusiness(string code, string business_id, User user)
        {

            OpenConn();

            OracleTransaction trans = Connection.BeginTransaction();

            List<string> strSql = new List<string>();
            List<OracleParameter[]> parameters = new List<OracleParameter[]>();
            
            StringBuilder strSqlBusinessDelete = new StringBuilder();
            strSqlBusinessDelete.Append("DELETE FROM dictionary.pmsc_business_info WHERE");
            strSqlBusinessDelete.Append(" code=:code and business_id=:business_id");

            OracleParameter[] parm = { 
                            new OracleParameter(":business_id", code),
                            new OracleParameter(":business_id", business_id)};

            strSql.Add(strSqlBusinessDelete.ToString());
            parameters.Add(parm);

            StringBuilder strSqlLinkDelete = new StringBuilder();

            strSqlLinkDelete.Append("DELETE FROM dictionary.pmsc_business_link");
            strSqlLinkDelete.Append(" WHERE SUBSTR(link_id,0,7) = :business_id");
            OracleParameter[] parametersDelete = {
                        new OracleParameter(":business_id",  code + business_id),    
                };

            strSql.Add(strSqlLinkDelete.ToString());
            parameters.Add(parametersDelete);


            StringBuilder strSqlMonitorCollectDelete = new StringBuilder();

            strSqlMonitorCollectDelete.Append("DELETE FROM monitor.collect_log");
            strSqlMonitorCollectDelete.Append(" WHERE business_id = :business_id");
            OracleParameter[] parametersCollectMonitor = {
                        new OracleParameter(":business_id",  code + business_id),    
                };

            strSql.Add(strSqlMonitorCollectDelete.ToString());
            parameters.Add(parametersCollectMonitor);


            StringBuilder strSqlMonitorDetailDelete = new StringBuilder();

            strSqlMonitorDetailDelete.Append("DELETE FROM monitor.detail_log");
            strSqlMonitorDetailDelete.Append(" WHERE business_id = :business_id");
            OracleParameter[] parametersMonitorDetail = {
                        new OracleParameter(":business_id",  code + business_id),    
                };

            strSql.Add(strSqlMonitorDetailDelete.ToString());
            parameters.Add(parametersMonitorDetail);

            //20160808 syy 增加操作记录
            //User user = new User();
            this.InsertOptRecord(strSql, parameters, code + business_id, "Delete", user);

            bool result = OracleHelper.ExecuteNonQuery(trans, CommandType.Text, strSql, parameters);

            CloseConn();

            return result;
        }



        /// <summary>
        /// 插入记录信息
        /// </summary>
        /// <param name="optrecord">optrecord</param>
        /// <returns></returns>
        public void InsertOptRecord(List<string> strSql, List<OracleParameter[]> parameters, string businessID, string optName, User user)
        {

            OptRecord optrecord = new OptRecord();
            //optrecord.OptID = this.GetMaxOptID().ToString();
            optrecord.OptBusinessID = businessID;
            optrecord.OptName = optName;

            optrecord.OptUserID = user.UserID;
            optrecord.OptUser = user.UserName;

            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                optrecord.OptUserIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
            else
                optrecord.OptUserIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; 
            //optrecord.OptUserIP = "10.16.37.19";

            for (int i = 0; i < strSql.Count; i++)
            {
                string sql = strSql[i].ToString();
                string para = "";

                foreach (OracleParameter[] oraparas in parameters)
                {
                    foreach (OracleParameter orapara in oraparas)
                    {
                        para += orapara.Value + ";";
                    }
                }

                optrecord.OptSQL += sql + "---" + para + "------";
            }

            //optrecord.OptDate = DateTime.Now.ToShortDateString();
            

            StringBuilder strSqlOpt = new StringBuilder();
            strSqlOpt.Append("INSERT INTO monitor.syslog(");
            strSqlOpt.Append(" OperateBusinessID, OperateName, OperateUserID, OperateUser, OperateUserIP, OperateSQL, OperateDate");
            strSqlOpt.Append(") VALUES (");
            strSqlOpt.Append(" :OperateBusinessID, :OperateName, :OperateUserID, :OperateUser, :OperateUserIP, :OperateSQL, :OperateDate");
            strSqlOpt.Append(") ");

            OracleParameter[] parametersService = {
                        //new OracleParameter(":OperateID",  optrecord.OptID),           
                        new OracleParameter(":OperateBusinessID",  optrecord.OptBusinessID),           
                        new OracleParameter(":OperateName",  optrecord.OptName),    
                        new OracleParameter(":OperateUserID",  optrecord.OptUserID), 
                        new OracleParameter(":OperateUser",  optrecord.OptUser), 
                        new OracleParameter(":OperateUserIP",  optrecord.OptUserIP), 
                        new OracleParameter(":OperateSQL", optrecord.OptSQL), 
                        new OracleParameter(":OperateDate", DateTime.Now),
                };

            strSql.Add(strSqlOpt.ToString());
            parameters.Add(parametersService);
        }



        /// <summary>
        /// 获得最大操作ID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int GetMaxOptID()
        {
            //OpenConn();

            string optid = "";
            int optidi = 1;
            string sql = "select Max(OperateID) from monitor.syslog";

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql);
            dt = ds.Tables[0];

            if (dt != null && dt.Rows.Count != 0)
            {
                optid = dt.Rows[0][0].ToString();
                try
                {
                    optidi = Convert.ToInt32(optid) + 1;
                }
                catch
                {
                    optidi = 1;
                }
            }

            //CloseConn();

            return optidi;
        }



        /// <summary>
        /// 获得某一条操作记录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public DataTable GetOptByBusinessID(string business_id)
        {
            string sql = string.Empty;
            OpenConn();

            sql = "select * from monitor.syslog where OperateBusinessID = :OperateBusinessID order by OperateDate desc";

            OracleParameter[] parameters = {
                        new OracleParameter(":OperateBusinessID",  business_id),           
                };

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql, parameters);
            dt = ds.Tables[0];

            CloseConn();

            return dt;
        }

        /// <summary>
        /// 获得全部操作记录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public DataTable GetOptListAll()
        {
            string sql = string.Empty;
            OpenConn();

            sql = "select * from monitor.syslog order by OperateDate desc";

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql);
            dt = ds.Tables[0];

            CloseConn();

            return dt;
        }


















        //测试，大于0，则成功
        public bool InsertNonQuery()
        {
            OpenConn();
            //sql = "INSERT INTO dictionary.pmsc_dispatch_infomation(business_id, link_id,topic_name,group_name,server_ip,username,password,process_name,parameter_info) VALUES ('1', '1','ss','ss','10.16.36.17','ss','ss','ss','ss')";

            StringBuilder strSqlLink = new StringBuilder();
            strSqlLink.Append("INSERT INTO dictionary.pmsc_business_link(");
            strSqlLink.Append(" link_id, business_describe, cmd_info, topic_info");
            strSqlLink.Append(") VALUES (");
            strSqlLink.Append(" :link_id, :business_describe, :cmd_info, :topic_info");
            strSqlLink.Append(") ");

            OracleParameter[] parametersLink = {
                        new OracleParameter(":link_id",  "1000001"),           
                        new OracleParameter(":business_describe",  "test"),           
                        new OracleParameter(":cmd_info",  "test"),   
                        new OracleParameter(":topic_info",  "(01/02)"),    
                };
            string ss = strSqlLink.ToString();

            int result = OracleHelper.ExecuteNonQuery(Connection, CommandType.Text, strSqlLink.ToString(), parametersLink);

            CloseConn();

            return result > 0;
        }

        public bool Update()
        {
            OpenConn();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE dictionary.pmsc_dispatch_infomation SET ");
            //strSql.Append(" business_id = :business_id , ");
            strSql.Append(" link_id = :link_id , ");
            strSql.Append(" topic_name = :topic_name , ");
            strSql.Append(" group_name = :group_name , ");
            strSql.Append(" server_ip = :server_ip , ");
            strSql.Append(" username = :username ");
            strSql.Append(" password = :password , ");
            strSql.Append(" process_name = :process_name , ");
            strSql.Append(" parameter_info = :parameter_info ");
            strSql.Append(" where business_id=:business_id  ");

            OracleParameter[] parameters = {
                        new OracleParameter(":link_id",  "1"),           
                        new OracleParameter(":topic_name",  "sss"),           
                        new OracleParameter(":group_name",  "sss"),           
                        new OracleParameter(":server_ip",  "10.16..36.17"),           
                        new OracleParameter(":username", "sss"),           
                        new OracleParameter(":password",  "sss"), 
                        new OracleParameter(":process_name",  "sss"),           
                        new OracleParameter(":parameter_info",  "sss"),    
                        new OracleParameter(":business_id",  "1"),   
                };

            int result = OracleHelper.ExecuteNonQuery(Connection, CommandType.Text, strSql.ToString(), parameters);
            
            CloseConn();

            return result > 0;
        }



        public bool Delete(string ID)
        {
            OpenConn();

            string sql = "delete from dictionary.pmsc_dispatch_infomation where business_id=:business_id";

            OracleParameter[] parm = { new OracleParameter(":business_id", ID) };

            int result = OracleHelper.ExecuteNonQuery(Connection, CommandType.Text, sql, parm);

            CloseConn();

            return result > 0;
        }

        public void InsterHistory(string username, string operation,string operTime)
        {
            OpenConn();
            string strSql = "insert into"; //插入操作记录SQL语句
            OracleParameter[] parameters = {
                                               new OracleParameter(":username",username),
                                               new OracleParameter(":operation",operation),
                                               new OracleParameter(":opertime",operTime), 
                                           };
            int result = OracleHelper.ExecuteNonQuery(Connection, CommandType.Text, strSql.ToString(), parameters);
            CloseConn();
        }
    }
}