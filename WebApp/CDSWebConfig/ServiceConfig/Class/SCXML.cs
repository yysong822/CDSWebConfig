using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace CDSWebConfig.ServiceConfig.Class
{
    public class SCXML
    {
        public SCModel GetSCModel(string path, string file)
        {

            SCModel scModel = new SCModel();

            XDocument xdoc = XDocument.Load(path + file);
            IEnumerable<XElement> services = from service in xdoc.Descendants("service") select service;

            foreach (var service in services)
            {
                ServiceInfo si = new ServiceInfo();
                si.ServiceName = service.Attribute("name").Value;
                si.ServiceID = file.Substring(0, 7);
                si.ServiceType = file.Substring(0, 2);
                scModel.ServiceInfo = si;

                string servicetype = service.Attribute("type").Value;
                if (servicetype == "collection")
                {
                    scModel.DataSourceType = "0";
                    scModel.DataSourceName = "采集";

                    DataSourceCollection dsc = new DataSourceCollection();

                    XElement collection = service.Element("collection");

                    dsc.CollectionDescription = collection.Attribute("description").Value;
                    dsc.StartTimeValue = collection.Element("start-time").Value;
                    dsc.PeriodValue = collection.Element("period").Value;
                    dsc.StartTimeDescription = collection.Element("start-time").Attribute("description").Value;    //"hh:mm:ss";
                    dsc.PeriodDescription = collection.Element("period").Attribute("description").Value;     // "second,0s means just once";
                    scModel.DataSource = dsc;
                }
                else if (servicetype=="notify")
                {
                    scModel.DataSourceType = "1";
                    scModel.DataSourceName = "监控";

                    DataSourceNotify dsn = new DataSourceNotify();

                    XElement notifyInfo = service.Element("notify");

                    dsn.NotifyDescription = notifyInfo.Attribute("description").Value;
                    dsn.PathDescription = notifyInfo.Element("paths").Element("path").Attribute("description").Value;
                    dsn.PathsValue = notifyInfo.Element("paths").Element("path").Attribute("value").Value;
                    dsn.FilesDescription = notifyInfo.Element("paths").Element("path").Element("files").Attribute("description").Value;
                    dsn.FilesValue = notifyInfo.Element("paths").Element("path").Element("files").Value;
                    scModel.DataSource = dsn;
                }
                else
                {
                    scModel.DataSourceType = "2";
                    scModel.DataSourceName = "外部";

                    DataSourceOuter dso = new DataSourceOuter();

                    XElement outerInfo = service.Element("outer");

                    dso.OuterDescription = outerInfo.Attribute("description").Value;
                    dso.OuterPathDes = outerInfo.Element("path").Attribute("description").Value;
                    dso.OuterPathValue = outerInfo.Element("path").Attribute("value").Value;
                    dso.OuterFileDes = outerInfo.Element("path").Element("files").Attribute("description").Value;
                    dso.OuterFileValue = outerInfo.Element("path").Element("files").Value;
                    scModel.DataSource = dso;
                }

                IEnumerable<XElement> links = service.Descendants("link");
                List<SCLink> scLinks = new List<SCLink>();
                foreach (var link in links)
                {
                    SCLink scLink = new SCLink();

                    scLink.LinkID = link.Attribute("id").Value;
                    scLink.Description = link.Attribute("description").Value;
                    scLink.Order = link.Element("order").Value;

                    IEnumerable<XElement> nextlinks = link.Descendants("next-links");
                    List<SCNextLink> scNextLinks = new List<SCNextLink>();
                    foreach (var nextlink in nextlinks)
                    {

                        IEnumerable<XElement> topiclinks = nextlink.Descendants("next-link");
                        foreach (var topiclink in topiclinks)
                        {
                            SCNextLink scNextLink = new SCNextLink();

                            scNextLink.Topic = topiclink.Element("topic").Value;
                            scNextLink.LinkID = topiclink.Element("link-id").Value;

                            scNextLinks.Add(scNextLink);

                            scLink.TopicLink += "(" + scNextLink.Topic + "/" + scNextLink.LinkID + ")" + ",";
                        }
                    }

                    scLink.NextLinks = scNextLinks;
                    if (scLink.TopicLink != null && scLink.TopicLink != "")
                    {
                        scLink.TopicLink = scLink.TopicLink.Substring(0, scLink.TopicLink.Length - 1);
                    }
                    else
                    {
                        scLink.TopicLink = "";
                    }
                    

                    scLinks.Add(scLink);
                }

                scModel.SCLinks = scLinks;
            }

            return scModel;
        }
    }
}