using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDSWebConfig.ServiceConfig.Class
{
    public class OptRecord
    {
        private string _OptID;

        public string OptID
        {
            get { return _OptID; }
            set { _OptID = value; }
        }

        private string _OptBusinessID;

        public string OptBusinessID
        {
            get { return _OptBusinessID; }
            set { _OptBusinessID = value; }
        }

        private string _OptName;

        public string OptName
        {
            get { return _OptName; }
            set { _OptName = value; }
        }

        private string _OptUserID;

        public string OptUserID
        {
            get { return _OptUserID; }
            set { _OptUserID = value; }
        }

        private string _OptUser;

        public string OptUser
        {
            get { return _OptUser; }
            set { _OptUser = value; }
        }

        private string _OptUserIP;

        public string OptUserIP
        {
            get { return _OptUserIP; }
            set { _OptUserIP = value; }
        }

        private string _OptSQL;

        public string OptSQL
        {
            get { return _OptSQL; }
            set { _OptSQL = value; }
        }

        private string _OptDate;

        public string OptDate
        {
            get { return _OptDate; }
            set { _OptDate = value; }
        }
    }
}