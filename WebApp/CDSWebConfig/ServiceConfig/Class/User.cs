using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDSWebConfig.ServiceConfig.Class
{
    public class User
    {
        private string _UserID;

        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _UserIP;

        public string UserIP
        {
            get { return _UserIP; }
            set { _UserIP = value; }
        }
    }
}