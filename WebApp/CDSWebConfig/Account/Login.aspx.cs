using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CDSWebConfig.Account.Class;

namespace CDSWebConfig.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "UserAdd.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = UserName.Text.Trim();
            string password = Password.Text.Trim();
            string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5");
            UserDB odb = new UserDB();
            bool result = odb.Login(username, pwd);
            if (result)
            {
                FailureText.Text = "登录成功!!!";
                FormsAuthentication.SetAuthCookie(username, false);
                string[] userinfo = odb.getUserInfo(username);
                Session["UserName"] = username;
                Session["UserID"] = userinfo[0];
                Session["RoleID"] = userinfo[1];

                //FormsAuthentication.RedirectFromLoginPage(username, false);
            }
            else
            {
                FailureText.Text = "登录失败!!!";
            }
            
        }
    }
}
