using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace CDSWebConfig.ServiceConfig.Class
{
    [SerializableAttribute]
    public class SCModel
    {
        private ServiceInfo _ServiceInfo;

        public ServiceInfo ServiceInfo
        {
            get { return _ServiceInfo; }
            set { _ServiceInfo = value; }
        }

        private object _DataSource;

        public object DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }

        private string _DataSourceType;

        public string DataSourceType
        {
            get { return _DataSourceType; }
            set { _DataSourceType = value; }
        }

        private string _DataSourceName;

        public string DataSourceName
        {
            get { return _DataSourceName; }
            set { _DataSourceName = value; }
        }

        private List<SCLink> _SCLinks;

        public List<SCLink> SCLinks
        {
            get { return _SCLinks; }
            set { _SCLinks = value; }
        }

        
    }

    [SerializableAttribute]
    public class ServiceInfo
    {
        private string _ServiceType;
        private string _ServiceTypeName;
        private string _ServiceName;
        private string _ServiceID;

        public string ServiceType
        {
            get { return _ServiceType; }
            set { _ServiceType = value; }
        }
        public string ServiceTypeName
        {
            get { return _ServiceTypeName; }
            set { _ServiceTypeName = value; }
        }
        public string ServiceName
        {
            get { return _ServiceName; }
            set { _ServiceName = value; }
        }
        public string ServiceID
        {
            get { return _ServiceID; }
            set { _ServiceID = value; }
        }

        //上游ID和所属业务群
        public string SourceID { get; set; }
        public string ServiceGroup { get; set; }

        public void  GetServiceInfoFromDT(DataTable dt)
        {
            if (dt != null && dt.Rows.Count == 1)
            {
                ServiceType = dt.Rows[0][0].ToString();
                ServiceID = dt.Rows[0][0].ToString() + dt.Rows[0][1].ToString();
                ServiceName = dt.Rows[0][2].ToString();
                SourceID = dt.Rows[0][9].ToString();
                ServiceGroup = dt.Rows[0][10].ToString();
            }
        }
    }
    [SerializableAttribute]
    public class DataSourceNotify
    {
        private string _NotifyDescription;

        public string NotifyDescription
        {
            get { return _NotifyDescription; }
            set { _NotifyDescription = value; }
        }
        private string _PathDescription;

        public string PathDescription
        {
            get { return _PathDescription; }
            set { _PathDescription = value; }
        }
        private string _PathsValue;

        public string PathsValue
        {
            get { return _PathsValue; }
            set { _PathsValue = value; }
        }
        private string _FilesDescription;

        public string FilesDescription
        {
            get { return _FilesDescription; }
            set { _FilesDescription = value; }
        }
        private string _FilesValue;

        public string FilesValue
        {
            get { return _FilesValue; }
            set { _FilesValue = value; }
        }

        private List<NotifyPathFile> _PathFileMate;

        public List<NotifyPathFile> PathFileMate
        {
            get { return _PathFileMate; }
            set { _PathFileMate = value; }
        }

        public void GetPathFileMateFromString(string PathsValue, string FilesValue, string PathsDes, string FilesDes)
        {
            _PathFileMate = new List<NotifyPathFile>();

            string[] paths = PathsValue.Split(';');
            string[] files = FilesValue.Split(';');

            for (int i = 0; i < paths.Length; i++)
            {
                NotifyPathFile npf = new NotifyPathFile();
                npf.Path = paths[i];
                npf.Files = files[i];
                npf.PathDes = PathsDes;
                npf.FilesDes = FilesDes;
                _PathFileMate.Add(npf);
            }

            //return _PathFileMate;
        }
        public void  GetDataSourceFromDT(DataTable dt)
        {
            if (dt != null && dt.Rows.Count == 1)
            {
                //DataSourceType = dt.Rows[0][3].ToString();
                NotifyDescription = dt.Rows[0][4].ToString();
                PathsValue = dt.Rows[0][7].ToString();
                FilesValue = dt.Rows[0][8].ToString();
            }
        }
    }

    [SerializableAttribute]
    public class NotifyPathFile
    {
        private string _Path;

        public string Path
        {
            get { return _Path; }
            set { _Path = value; }
        }
        private string _Files;

        public string Files
        {
            get { return _Files; }
            set { _Files = value; }
        }

        private string _PathDes;

        public string PathDes
        {
            get { return _PathDes; }
            set { _PathDes = value; }
        }
        private string _FilesDes;

        public string FilesDes
        {
            get { return _FilesDes; }
            set { _FilesDes = value; }
        }

    }

    [SerializableAttribute]
    public class DataSourceCollection
    {
        private string _CollectionDescription;

        public string CollectionDescription
        {
            get { return _CollectionDescription; }
            set { _CollectionDescription = value; }
        }
        private string _StartTimeDescription;

        public string StartTimeDescription
        {
            get { return _StartTimeDescription; }
            set { _StartTimeDescription = value; }
        }
        private string _StartTimeValue;

        public string StartTimeValue
        {
            get { return _StartTimeValue; }
            set { _StartTimeValue = value; }
        }
        private string _PeriodDescription;

        public string PeriodDescription
        {
            get { return _PeriodDescription; }
            set { _PeriodDescription = value; }
        }
        private string _PeriodValue;

        public string PeriodValue
        {
            get { return _PeriodValue; }
            set { _PeriodValue = value; }
        }

        public void GetDataSourceFromDT(DataTable dt)
        {
            if (dt != null && dt.Rows.Count == 1)
            {
                //DataSourceType = dt.Rows[0][3].ToString();
                CollectionDescription = dt.Rows[0][4].ToString();
                StartTimeValue = dt.Rows[0][5].ToString();
                PeriodValue = dt.Rows[0][6].ToString();
            }
        }
    }

    //外部业务分类
    [SerializableAttribute]
    public class DataSourceOuter
    {
        public string OuterDescription { get; set; }
        public string OuterPathValue { get; set; }
        public string OuterPathDes { get; set; }
        public string OuterFileValue { get; set; }
        public string OuterFileDes { get; set; }
        public void GetDataSourceFromDT(DataTable dt)
        {
            if (dt != null && dt.Rows.Count == 1)
            {
                OuterDescription = dt.Rows[0][4].ToString();
                //DataSourceType = dt.Rows[0][3].ToString();
                OuterPathValue = dt.Rows[0][7].ToString();
                OuterFileValue = dt.Rows[0][8].ToString();
            }
        }
    }

    [SerializableAttribute]
    public class SCLink
    {
        private string _LinkID;
        private string _Description;
        private string _Order;
        private string _TopicLink;

        private List<SCNextLink> _NextLinks;

        public string LinkID
        {
            get { return _LinkID; }
            set { _LinkID = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public string Order
        {
            get { return _Order; }
            set { _Order = value; }
        }

        public string TopicLink
        {
            get { return _TopicLink; }
            set { _TopicLink = value; }
        }

        public List<SCNextLink> NextLinks
        {
            get { return _NextLinks; }
            set { _NextLinks = value; }
        }

        public List<SCNextLink> ChangeToTopicLinkList()
        {
            List<SCNextLink> scNextLinks = new List<SCNextLink>();
            if (TopicLink != "")
            {
                try
                {
                    string[] topiclinks = TopicLink.Split(',');
                    foreach (string var in topiclinks)
                    {
                        string[] topiclink = var.Split('/');
                        SCNextLink scNextLink = new SCNextLink();
                        scNextLink.Topic = topiclink[0].Substring(1);
                        scNextLink.LinkID = topiclink[1].Substring(0, topiclink[1].Length - 1);
                        scNextLinks.Add(scNextLink);
                    }
                }
                catch(Exception ex)
                {
                    //ex.Message;
                }
            }
            return scNextLinks;
        }
    }

    [SerializableAttribute]
    public class SCNextLink
    {
        private string _Topic;

        private string _LinkID;

        public string Topic
        {
            get { return _Topic; }
            set { _Topic = value; }
        }

        public string LinkID
        {
            get { return _LinkID; }
            set { _LinkID = value; }
        }
    }

    public class SCType
    {
        private string _TypeID;

        public string TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }

        private string _TypeName;

        public string TypeName
        {
            get { return _TypeName; }
            set { _TypeName = value; }
        }
    }
}