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

namespace CDSWebConfig.Account.Class
{
    public class UserDB
    {
        protected OracleConnection Connection;
        private string connectionString;
        public UserDB()
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

        public bool Register(string username, string password,DateTime createdate,string roleid)
        {
            string userid = (Convert.ToInt32(getMaxUserID()) + 1).ToString();
            OpenConn();
            StringBuilder strSql = new StringBuilder(); //数据库注册SQL语句
            strSql.Append("INSERT INTO monitor.userinfo (");
            strSql.Append("userid,username,password,roleid,createdate");
            strSql.Append(") VALUES(");
            strSql.Append(":userid,:username,:password,:roleid,:createdate");
            strSql.Append(")");
            OracleParameter[] parameters = {
                                               new OracleParameter(":userid",userid),
                                               new OracleParameter(":username",username),
                                               new OracleParameter(":password",password),
                                               new OracleParameter(":roleid",roleid),
                                               new OracleParameter(":createdate",createdate),
                                           };
            int result = OracleHelper.ExecuteNonQuery(Connection, CommandType.Text, strSql.ToString(), parameters);
            CloseConn();
            return result > 0;
        }

        public bool Login(string username, string password)
        {
            OpenConn();
            //string strSql = "select username,password from monitor.userinfo"; //数据库登录查询SQL语句
            OracleParameter[] parameters = {
                                               new OracleParameter(":username",username),
                                               new OracleParameter(":password",password),
                                           };
            //int result = OracleHelper.ExecuteNonQuery(Connection, CommandType.Text, strSql.ToString(), parameters);
            OracleCommand oracmd = Connection.CreateCommand();
            oracmd.CommandText="select username,password,roleid from monitor.userinfo where username=:username and password=:password";
            OracleDataReader odr = OracleHelper.ExecuteReader(Connection.ConnectionString, CommandType.Text, oracmd.CommandText, parameters);
            bool result = odr.HasRows;
            CloseConn();
            return result;
        }

        public string[] getUserInfo(string username)
        {
            OpenConn();
            OracleParameter[] parameters = {
                                               new OracleParameter(":username",username),
                                               };
            OracleCommand oracmd = Connection.CreateCommand();
            oracmd.CommandText = "select userid,username,roleid from monitor.userinfo where username=:username";

            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, oracmd.CommandText, parameters);
            dt = ds.Tables[0];
            string userid = dt.Rows[0]["userid"].ToString();
            string roleid = dt.Rows[0]["roleid"].ToString();

            string[] userinfo = new string[] { userid, roleid };
            CloseConn();
            return userinfo;
        }

        public string getMaxUserID()
        {
            OpenConn();
            //string strSql = "select * from monitor.userinfo t where userid=(select max(userid) from monitor.userinfo)";
            string strSql = "select max(userid) from monitor.userinfo";
            DataTable dt = new DataTable();
            DataSet ds = OracleHelper.ExecuteDataset(Connection, CommandType.Text, strSql);
            string maxID;
            dt = ds.Tables[0];
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                maxID = dt.Rows[0][0].ToString();
            }
            else
            {
                maxID = "0";
            }
            CloseConn();
            return maxID;
        }
    }
}