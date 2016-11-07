//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Oracle.ManagedDataAccess.Client;
//using System.Data;
//using System.Text;

//namespace CDSWebConfig.DB
//{
//    public class OracleDBConn
//    {
//        protected OracleConnection Connection;
//        private string connectionString;
//        public OracleDBConn()
//        {
//            string connStr;
//            connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ToString();
//            connectionString = connStr;
//            Connection = new OracleConnection(connectionString);
//        }

//        #region 带参数的构造函数
//        /// 带参数的构造函数
//        /// 数据库联接字符串
//        public OracleDBConn(string ConnString)
//        {
//            string connStr;
//            connStr = System.Configuration.ConfigurationManager.ConnectionStrings[ConnString].ToString();
//            Connection = new OracleConnection(connStr);
//        }
//        #endregion

//        #region 打开数据库
//        /// 打开数据库
//        public void OpenConn()
//        {
//            if (this.Connection.State != ConnectionState.Open)
//                this.Connection.Open();
//        }
//        #endregion

//        #region 关闭数据库联接
//        /// 关闭数据库联接
//        public void CloseConn()
//        {
//            if (Connection.State == ConnectionState.Open)
//                Connection.Close();
//        }
//        #endregion

//        public DataTable GetListForwhere(string sql, params OracleParameter[] commandParameters)
//        {
//            OpenConn();
//            //string sql = string.Empty;
//            sql = "select * from dictionary.pmsc_dispatch_infomation";
//            DataTable dt = new DataTable();
//            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, sql, commandParameters);
//            dt = ds.Tables[0];
//            CloseConn();
//            return dt;
//        }

//        //返回收影响的行数，不为0，则成功
//        public int InsertNonQuery(string sql, params OracleParameter[] commandParameters)
//        {
//            OpenConn();
//            //string sql = string.Empty;
//            sql = "INSERT INTO dictionary.pmsc_dispatch_infomation(business_id, link_id,topic_name,group_name,server_ip,username,password,process_name,parameter_info) VALUES ('1', '1','ss','ss','10.16.36.17','ss','ss','ss','ss')";

//            StringBuilder strSql = new StringBuilder();
//            strSql.Append("INSERT INTO dictionary.pmsc_dispatch_infomation(");
//            strSql.Append(" business_id, link_id,topic_name,group_name,server_ip,username,password,process_name,parameter_info");
//            strSql.Append(") VALUES (");
//            strSql.Append(" :business_id,:link_id,:topic_name,:group_name,:server_ip,:username,:password,:process_name,parameter_info");
//            strSql.Append(") ");
//            OracleParameter[] parameters = {
//                        new OracleParameter(":business_id",  "1"),           
//                        new OracleParameter(":link_id",  "1"),           
//                        new OracleParameter(":topic_name",  "sss"),           
//                        new OracleParameter(":group_name",  "sss"),           
//                        new OracleParameter(":server_ip",  "10.16..36.17"),           
//                        new OracleParameter(":username", "sss"),           
//                        new OracleParameter(":password",  "sss"), 
//                        new OracleParameter(":process_name",  "sss"),           
//                        new OracleParameter(":parameter_info",  "sss"),           
//                };

//            OracleParameter[] parameters={new OracleParameter(":a", OracleType.VarChar,50),};

//            parameters[0].Value = 赋值;
//            int result = OracleHelper.ExecuteNonQuery(Connection, CommandType.Text, sql, commandParameters);
//            CloseConn();
//            return result;
//        }
//    }
//}