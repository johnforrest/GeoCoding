using System;
using System.Xml;
using System.Collections.ObjectModel;

namespace ZJGISCommon
{
    public class ClsXml
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public string GetMaxElementID(XmlDocument xmlDoc)
        {
            XmlNode CEn = null;
            string str = null;
            string tmp = null;
            str = "N-1";

            for (int i = 0; i < xmlDoc.ChildNodes.Count; i++)
            {
                CEn = xmlDoc.ChildNodes[i];

                if (Convert.ToInt64(str.Substring(1))<Convert.ToInt64(CEn.Name.Substring(1)))
                {
                    str = CEn.Name;
                }
                if (CEn.ChildNodes.Count>0)
                {
                    tmp = GetMaxElementIDByNode(CEn);

                    if (Convert.ToInt64(str.Substring(1))<Convert.ToInt64(tmp.Substring(1)))
                    {
                        str = tmp;
                    }
                }
            }

            return "N" + (Convert.ToInt64(str.Substring(1)) + 1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RootElementNode"></param>
        /// <param name="vElementCol"></param>
        /// <param name="vLayerName"></param>
        /// <param name="vFindText"></param>
        public void FindElementByProperty( XmlElement RootElementNode,
            Collection<object> vElementCol, string vLayerName, string vFindText)
        {
            //从一个xml的根节点生成树
            XmlElement CEn = null;

            if (RootElementNode == null)
            {
                return;
            }
            try
            {
                if (RootElementNode.HasChildNodes == true)
                {
                    for (int i = 0; i < RootElementNode.ChildNodes.Count; i++)
                    {
                        CEn = RootElementNode.ChildNodes[i] as XmlElement;

                        if (CEn.HasChildNodes == true)
                        {
                            FindElementByProperty(CEn, vElementCol, vLayerName, vFindText);
                        }
                        if (CEn.GetAttribute(vFindText).ToUpper() == vLayerName.ToUpper())
                        {
                            if (!vElementCol.Contains(CEn.LocalName))
                            {
                                vElementCol.Add(CEn);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public string GetMaxElementIDByNode(XmlNode en)
        {
            XmlNode CEn = null;
            string str = null;
            string tmp = null;
            str = "N-1";

            for (int i = 0; i <= en.ChildNodes.Count - 1; i++)
            {
                CEn = en.ChildNodes[i];

                if (Convert.ToInt64(str.Substring(1)) < Convert.ToInt64(CEn.Name.Substring(1)))
                {
                    str = CEn.Name;
                }
                if (CEn.ChildNodes.Count > 0)
                {
                    tmp = GetMaxElementIDByNode(CEn);

                    if (Convert.ToInt64(str.Substring(1)) < Convert.ToInt64(tmp.Substring(1)))
                    {
                        str = tmp;
                    }
                }
            }

            return str;
        }

        /// <summary>
        /// 功能:
        /// 读取指定节点的指定属性值
        /// </summary>
        /// <param name="strNode">节点路径</param>
        /// <param name="strAttribute">此节点的属性</param>
        /// <returns></returns>
        public static string GetXmlNodeAttribute(string xmlPath, string strNode, string strAttribute)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string strReturn = "";
            xmlDoc.Load(xmlPath);
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                //获取节点的属性，并循环取出需要的属性值
                XmlAttributeCollection xmlAttr = xmlNode.Attributes;

                for (int i = 0; i < xmlAttr.Count; i++)
                {
                    if (xmlAttr.Item(i).Name == strAttribute)
                        strReturn = xmlAttr.Item(i).Value;
                }
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
            return strReturn;
        }

        public static void SetXmlNodeAttribute(string xmlPath, string strNode, string strAttribute,string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);
            try
            {
                //根据指定路径获取节点
                XmlNode xmlNode = xmlDoc.SelectSingleNode(strNode);
                //获取节点的属性，并循环取出需要的属性值
                XmlAttributeCollection xmlAttr = xmlNode.Attributes;

                for (int i = 0; i < xmlAttr.Count; i++)
                {
                    if (xmlAttr.Item(i).Name == strAttribute)
                         xmlAttr.Item(i).Value =value;
                }
                xmlDoc.Save(xmlPath);
            }
            catch (XmlException xmle)
            {
                throw xmle;
            }
        }
    }
}
