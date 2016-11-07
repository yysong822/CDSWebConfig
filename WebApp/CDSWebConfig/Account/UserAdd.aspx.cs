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
    public partial class UserAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //1 超级管理员
            //2 高级用户
            //3 普通用户
            if (Session["RoleID"] == null || Session["RoleID"].ToString() == "3" || Session["RoleID"].ToString()=="2"
             || Session["UserID"] == null || Session["UserName"] == null)
            {
                Response.Write("<script language=\"javascript\" type=\"text/javascript\">alert('权限不足，请您使用超级管理员帐号添加！');window.location.href='/Account/Login.aspx';</script>");
                return;
            }
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);
            /*MembershipUser user = Membership.GetUser(RegisterUser.UserName);
            if (user != null)
            {
                throw new ApplicationException("找不到用户.");
            }*/
            //Guid userId = (Guid)user.ProviderUserKey;   //获取CreateUserWizard控件中的额外用户信息
            TextBox UserName = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("UserName");
            string username = UserName.Text;
            TextBox Password = (TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Password");
            string password = Password.Text;
            string pwd = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5");
            //user.
            DateTime createdate = new DateTime();
            createdate = DateTime.Now;
            DropDownList Role = (DropDownList)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Role");
            string role = Role.SelectedIndex.ToString();
            string roleid="3";
            if (role == "0")
            {
                roleid = "2";
            }
            else if (role == "1")
            {
                roleid = "3";
            }
            UserDB odb = new UserDB();
            odb.Register(username, pwd, createdate, roleid);
            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (String.IsNullOrEmpty(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);
        }
    }
}