using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

namespace ZJGISXMLOperation
{
    public enum XDirection
    { Up = -2, Down = 0 }

    public class ClsXmlOperation
    {
        XmlDocument doc;
        public XmlDocument Doc
        { get { return doc; } }
        String xmlfile;

        public String Xmlfile
        {
            get { return xmlfile; }
            set { xmlfile = value; }
        }

        public ClsXmlOperation()
        {

        }
        public ClsXmlOperation(String xmlfile)
        {
            this.xmlfile = xmlfile;
            doc = new XmlDocument();
        }

        /// <summary>
        /// 获取节点的所有子节点
        /// </summary>
        /// <param name="elementname"></param>
        /// <returns></returns>
        public string[] GetInnerXMLValue(XmlElement element)
        {
            string[] array = new string[9];
            int i = 0;

            XmlDocument doc = new XmlDocument();
            string root = "<root>" + element.InnerXml + "</root>";
            doc.LoadXml(root);
            XmlNode newNode = doc.DocumentElement;
            foreach (XmlNode node in newNode.ChildNodes)
            {
                array[i] = node.InnerText;
                i++;
            }
            return array;
        }

        /// <summary>
        /// 获取节点的所有子节点
        /// </summary>
        /// <param name="elementname"></param>
        /// <returns></returns>
        public XmlNodeList GetChildrenNodes(string element)
        {
            doc.Load(Xmlfile);
            //XmlElement el = null;
            XmlNodeList topM = doc.DocumentElement.ChildNodes;
            return topM;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementname"></param>
        /// <returns></returns>
        public XmlNode GetTopNode(String elementname)
        {
            doc.Load(Xmlfile);
            XmlNode topM = doc.DocumentElement.FirstChild.ParentNode;
            return topM;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementname"></param>
        /// <returns></returns>
        public XmlElement GetTopElement(String elementname)
        {
            doc.Load(Xmlfile);
            XmlElement el = null;
            XmlNodeList topM = doc.DocumentElement.ChildNodes;
            foreach (XmlElement element in topM)
            {
                if (element.Name == elementname)
                {
                    el = element;
                }
            }
            return el;
        }
        /// <summary>
        /// 获得对应名字的父节点元素
        /// </summary>
        /// <param name="element"></param>
        /// <param name="keyattriname"></param>
        /// <param name="keyattrivalue"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public XmlElement GetQryElement(XmlElement element, String keyattriname, String keyattrivalue, XDirection direction)     //向上或向下查找对应父元素下符合单个属性值的的第一个二级节点元素并返回
        {
            XmlElement el = null;
            XmlNodeList nodelist = element.ChildNodes;
            int k = nodelist.Count;
            for (int i = 0; i < k; i++)
            {
                int x = (direction == XDirection.Down) ? i : (k - i - 1);
                if ((((XmlElement)nodelist[x]).Attributes[keyattriname].Value == keyattrivalue) ||
                    ((keyattriname == "Date") && DateTime.Parse(((XmlElement)nodelist[x]).Attributes[keyattriname].Value) <= DateTime.Parse(keyattrivalue))
                    )
                {
                    el = (XmlElement)nodelist[x];
                    break;
                }
            }
            return el;
        }

        /// <summary>
        /// 向上或向下查找对应父元素下符合单个属性值的的第一个二级节点元素并返回
        /// </summary>
        /// <param name="elementname"></param>
        /// <param name="keyattriname"></param>
        /// <param name="keyattrivalue"></param>
        /// <returns></returns>
        public int GetQryElementNub(String elementname, String keyattriname, String keyattrivalue)
        {
            int m = 0;
            XmlElement element = GetTopElement(elementname);
            XmlNodeList nodelist = element.ChildNodes;
            int k = nodelist.Count;
            for (int i = 0; i < k; i++)
            {
                if (((XmlElement)nodelist[i]).Attributes[keyattriname].Value == keyattrivalue)
                {
                    m++;
                }
            }
            return m;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementname"></param>
        /// <param name="keyattriname"></param>
        /// <param name="keyattrivalue"></param>
        /// <param name="attriname"></param>
        /// <returns></returns>
        public String GetAttrivalue(String elementname, String keyattriname, String keyattrivalue, String attriname)
        {
            String attrivalue = "";
            doc.Load(Xmlfile);
            XmlElement element = GetTopElement(elementname);
            XmlNodeList nodelist = element.ChildNodes;
            foreach (XmlElement el in nodelist)
            {
                if (el.Attributes[keyattriname].Value == keyattrivalue)
                {

                    attrivalue = el.Attributes[attriname].Value;
                }
            }
            return attrivalue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementname"></param>
        /// <param name="nodename"></param>
        /// <param name="attrinamelist"></param>
        /// <param name="attrivaluelist"></param>
        public void Addinnode(String elementname, String nodename, String[] attrinamelist, String[] attrivaluelist)
        {
            doc.Load(Xmlfile);
            XmlElement element = GetTopElement(elementname);
            XmlElement el = doc.CreateElement(nodename);
            for (int i = 0; i < attrinamelist.Length; i++)
            {
                el.SetAttribute(attrinamelist[i], attrivaluelist[i]);
            }
            element.AppendChild(el);
            doc.Save(Xmlfile);
        }
        /// <summary>
        /// 添加一个节点
        /// </summary>
        /// <param name="elementname"></param>
        /// <param name="nodename"></param>
        /// <param name="keyattriname"></param>
        /// <param name="keyattrivalue"></param>
        /// <param name="attrinamelist"></param>
        /// <param name="attrivaluelist"></param>
        public void InsetAfterEl(String elementname, String nodename, String keyattriname, String keyattrivalue, String[] attrinamelist, String[] attrivaluelist)
        {
            //doc.Load(xmlfile);
            XmlElement element = GetTopElement(elementname);
            XmlElement elx = GetQryElement(element, keyattriname, keyattrivalue, XDirection.Up);
            XmlElement el = doc.CreateElement(nodename);
            for (int i = 0; i < attrinamelist.Length; i++)
            {
                el.SetAttribute(attrinamelist[i], attrivaluelist[i]);
            }
            //重设index
            if (elx == null || elx.Attributes["Date"].Value != el.Attributes["Date"].Value)
                el.SetAttribute("Index", el.Attributes["Index"].Value + "-1");
            else
                el.SetAttribute("Index", el.Attributes["Index"].Value + "-" + (Int32.Parse((elx.Attributes["Index"].Value).Substring(9)) + 1).ToString());
            element.InsertAfter(el, elx);
            //element.AppendChild(el);
            doc.Save(Xmlfile);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementname"></param>
        /// <param name="keyattriname"></param>
        /// <param name="keyattrivalue"></param>
        /// <param name="attrinamelist"></param>
        /// <param name="attrivaluelist"></param>
        public void AttriValueEdit(String elementname, String keyattriname, String keyattrivalue, String[] attrinamelist, String[] attrivaluelist)
        {
            doc.Load(Xmlfile);
            XmlElement element = GetTopElement(elementname);
            XmlNodeList nodelist = element.ChildNodes;
            foreach (XmlElement el in nodelist)
            {
                if (el.Attributes[keyattriname].Value == keyattrivalue)
                {
                    for (int i = 0; i < attrinamelist.Length; i++)
                    {
                        el.SetAttribute(attrinamelist[i], attrivaluelist[i]);
                    }
                    break;
                }
            }
            doc.Save(Xmlfile);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementname"></param>
        /// <param name="index"></param>
        /// <param name="direction"></param>
        public void MoveUpDown(String elementname, int index, XDirection direction)
        {
            doc.Load(Xmlfile);
            XmlElement element = GetTopElement(elementname);
            XmlNodeList nodelist = element.ChildNodes;
            XmlElement el = (XmlElement)nodelist[index];
            if ((index != 0 && direction == XDirection.Up) || (index != nodelist.Count - 1 && direction == XDirection.Down))
            {
                element.RemoveChild(el);
                element.InsertAfter(el, (XmlElement)nodelist[(index + (int)direction)]);
                doc.Save(Xmlfile);
            }
        }
        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="elementname"></param>
        /// <param name="attriname"></param>
        /// <returns></returns>
        public Object[] GetAttriList(String elementname, String attriname)
        {
            XmlElement element = GetTopElement(elementname);
            XmlNodeList nodelist = element.ChildNodes;
            int k = nodelist.Count;
            Object[] obtlist = null;
            if (k > 0)
            {
                obtlist = new Object[k];
                for (int i = 0; i < k; i++)
                {
                    obtlist[i] = ((XmlElement)nodelist[i]).Attributes[attriname].Value;
                }
            }
            return obtlist;
        }

        /// <summary>
        /// 插入节点
        /// </summary>
        /// <param name="LayerConfig"></param>
        /// <param name="name"></param>
        /// <param name="attrinamelist"></param>
        /// <param name="attrivaluelist"></param>
        internal void InsetAfterEl(string LayerConfig, string name, string[] attrinamelist, string[] attrivaluelist)
        {
            doc.Load(Xmlfile);
            //XmlNodeList topM = doc.DocumentElement.ChildNodes;

            string str = null;
            for (int i = 0; i < attrinamelist.Length; i++)
            {
                str += "<" + attrinamelist[i] + ">" + attrivaluelist[i] + "</" + attrinamelist[i] + ">";
            }

            //XmlDocument doc = new XmlDocument();
            //string root = "<layer>" + str + "</layer>";
            //doc.LoadXml(root);
            //XmlNode newNode = doc.CreateElement(root);
            //doc.DocumentElement.AppendChild(newNode);
            //doc.AppendChild(newNode);
            //创建节点
            XmlElement xmlElement = doc.CreateElement("layer");
            //添加属性
            //xmlElement.SetAttribute("ID", "21");
            xmlElement.SetAttribute("name", name);
            xmlElement.InnerXml = str;
            //将节点加入到指定的节点下
            XmlNode xml = doc.DocumentElement.PrependChild(xmlElement);
            //doc.Save(path);
            doc.Save(Xmlfile);

        }
        /// <summary>
        /// 编辑修改
        /// </summary>
        /// <param name="LayerConfig"></param>
        /// <param name="name"></param>
        /// <param name="attrinamelist"></param>
        /// <param name="attrivaluelist"></param>
        internal void AttriValueEdit(string LayerConfig, string name, string[] attrinamelist, string[] attrivaluelist)
        {
            doc.Load(Xmlfile);
            XmlNodeList nodelist = doc.DocumentElement.ChildNodes;
            foreach (XmlElement el in nodelist)
            {
                //if (el.Attributes["Name"].Value == name)
                string test = el.GetAttribute("name");
                if (el.GetAttribute("name") == name)
                {
                    string str = null;
                    for (int i = 0; i < attrinamelist.Length; i++)
                    {
                        str += "<" + attrinamelist[i] + ">" + attrivaluelist[i] + "</" + attrinamelist[i] + ">";
                    }
                    //创建节点
                    XmlElement xmlElement = doc.CreateElement("layer");
                    //添加属性
                    //xmlElement.SetAttribute("ID", "21");
                    xmlElement.SetAttribute("name", name);
                    xmlElement.InnerXml = str;
                    //将节点加入到指定的节点下
                    //XmlNode xml = doc.DocumentElement.PrependChild(xmlElement);
                    el.ParentNode.ReplaceChild(xmlElement, el);
                }
            }
            doc.Save(Xmlfile);
        }

   

    }
}
