using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NVelocity.App;
using Commons.Collections;
using NVelocity.Runtime;
using NVelocity;
using System.IO;
using System.Xml;
using System.Text;

namespace CDSWebConfig.ServiceConfig.Class
{
    public class SCCollectionXML
    {
        public void CreateXML(SCModel scModel, string xmlPath)
        {
            VelocityEngine ve = new VelocityEngine();//模板引擎实例化
            ExtendedProperties ep = new ExtendedProperties();//模板引擎参数实例化
            ep.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");//指定资源的加载类型
            string ss = HttpContext.Current.Server.MapPath(".");
            ep.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, HttpContext.Current.Server.MapPath("."));//指定资源的加载路径
            //props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, Path.GetDirectoryName(HttpContext.Current.Request.PhysicalPath));
            ep.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");//输入格式
            ep.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");//输出格式

            //模板的缓存设置
            ep.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE, true); //是否缓存
            ep.AddProperty("file.resource.loader.modificationCheckInterval", (Int64)300); //缓存时间(秒)
            ve.Init(ep);

            //一、加载模板 
            Template template = ve.GetTemplate("config/XMLTemplate/collection.xml");//加载模板
            VelocityContext vltContext = new VelocityContext(); //当前的数据信息载体集合

            //二、填充值
            string serviceID = scModel.ServiceInfo.ServiceID;

            vltContext.Put("servicename", scModel.ServiceInfo.ServiceName);
            vltContext.Put("collectiondes", ((DataSourceCollection)scModel.DataSource).CollectionDescription);
            vltContext.Put("starttimedes", ((DataSourceCollection)scModel.DataSource).StartTimeDescription);
            vltContext.Put("starttime", ((DataSourceCollection)scModel.DataSource).StartTimeValue);
            vltContext.Put("perioddes", ((DataSourceCollection)scModel.DataSource).PeriodDescription);
            vltContext.Put("period", ((DataSourceCollection)scModel.DataSource).PeriodValue);
            vltContext.Put("SCLinks", scModel.SCLinks);
            vltContext.Put("sourceid", scModel.ServiceInfo.SourceID);
            vltContext.Put("servicegroup", scModel.ServiceInfo.ServiceGroup);

            //三、合并模板输出内容
            var vltWriter = new StringWriter();
            template.Merge(vltContext, vltWriter);//合并数据集合对象到输出流.
            string innerxml = vltWriter.GetStringBuilder().ToString();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.InnerXml = innerxml;
            string filexml = xmlPath + serviceID + ".xml";
            xmlDoc.Save(filexml);
        }
    }
}