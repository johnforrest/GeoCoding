using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic;
using System.Data.OracleClient;
using ZJGISCommon;

namespace ZJGIS
{
    class ClsDataTree
    {
        
        /// <summary>
        /// 从lyrxml的根节点生成树
        /// </summary>
        /// <param name="RootElementNode"></param>
        /// <param name="vTreeView"></param>
        /// <returns></returns>
        public static bool LoadLyrTreeFromXML(ref System.Xml.XmlElement RootElementNode, ref TreeView vTreeView)
        {
            System.Xml.XmlElement Cen = null;
            int i = 0;
            TreeNode pRootNode = null;
            TreeNode tNode = null;
            try
            {
                if (RootElementNode.HasChildNodes == true)
                {
                    for (i = 0; i <= RootElementNode.ChildNodes.Count - 1; i++)
                    {
                        XmlElement tempElement = RootElementNode.ChildNodes[i] as XmlElement;
                        pRootNode = vTreeView.Nodes.Add(tempElement.Name, tempElement.GetAttribute("TEXT"));
                        pRootNode.Tag = tempElement.GetAttribute("TYPE");
                   
                        XmlNode pXmlNode = RootElementNode.ChildNodes[i];
                        if (pXmlNode.HasChildNodes == true)
                        {
                            for (int j = 0; j <= pXmlNode.ChildNodes.Count - 1; j++)
                            {
                                Cen = pXmlNode.ChildNodes[j] as XmlElement;
                                if (Strings.LCase(Cen.GetAttribute("EDITABLE")) == "true")
                                {
                                    tNode = pRootNode.Nodes.Add(Cen.Name, Cen.GetAttribute("TEXT"));
                                    tNode.Tag = Cen.GetAttribute("TYPE");
                                    tNode.ToolTipText = Cen.GetAttribute("CODE");
                                    LoadLyrTreeFromXML2(Cen, tNode);
                                }

                            }

                        }
                    }
                }
                return true;
            }
            catch (Exception )
            {
                return true;
            }
        }
        private static void LoadLyrTreeFromXML2(System.Xml.XmlElement xmlElement, TreeNode treeNode)
        {
            System.Xml.XmlElement Cen = null;
            int i = 0;
            TreeNode tNode = default(TreeNode);

            if (xmlElement.HasChildNodes == true)
            {
                for (i = 0; i <= xmlElement.ChildNodes.Count - 1; i++)
                {
                    Cen = xmlElement.ChildNodes[i] as XmlElement;
                    if (Strings.LCase(Cen.GetAttribute("EDITABLE")) == "true")
                    {
                        tNode = treeNode.Nodes.Add(Cen.Name, Cen.GetAttribute("TEXT"));
                        tNode.ToolTipText = Cen.GetAttribute("CODE");
                        tNode.Tag = Cen.GetAttribute("TYPE");
                        LoadLyrTreeFromXML2(Cen, tNode);
                    }
                }
            }
        }

        /// <summary>
        /// 设置图库管理树的节点图片和判断节点是否展开
        /// </summary>
        /// <param name="vTreeView"></param>
        public static void SetNodePic(TreeView vTreeView)
        {
            int i = 0;
            TreeNode pNode = null;
            System.Xml.XmlDocument xmlDoc = null;
            try
            {

                //初始化图库管理
                XmlDocument pXmlDocument = new XmlDocument();
                string strXML = System.Windows.Forms.Application.StartupPath + "\\..\\Res\\Xml\\DataView.xml";
                if (!string.IsNullOrEmpty(strXML))
                {
                    pXmlDocument.Load(strXML);
                }
                xmlDoc = pXmlDocument;
                for (i = 0; i <= vTreeView.Nodes.Count - 1; i++)
                {
                    pNode = vTreeView.Nodes[i];
                    pNode.Expand();
                    SetPic(pNode, xmlDoc);
                }
                ////释放一些变量
                xmlDoc = null;
                pNode = null;
            }
            catch (Exception)
            {
                xmlDoc = null;
                pNode = null;
            }
        }

        private static void SetPic(TreeNode pNode, System.Xml.XmlDocument xmlDoc)
        {
            try
            {
                string strCode = null;
                System.Xml.XmlElement vNode = null;
                TreeNode pNode1 = default(TreeNode);
                int i = 0;

                vNode = (XmlElement)xmlDoc.GetElementsByTagName(pNode.Name).Item(0);

                switch (pNode.Tag.ToString())
                {
                    case "ROOT":
                        pNode.Expand();
                        break;
                    case "SDE":
                        pNode.Expand();
                        break;
                    case "SCALE":
                        pNode.Expand();
                        break;
                    case "STYLE":
                        pNode.Expand();
                        break;
                }

                if (pNode.Nodes.Count == 0)
                {
                    strCode = vNode.GetAttribute("CODE");
                    if (Strings.InStr(strCode, "PT", CompareMethod.Text) != 0)
                    {
                        pNode.SelectedImageKey = "Point";
                        pNode.ImageKey = "Point";
                    }
                    else if (Strings.InStr(strCode, "LN", CompareMethod.Text) != 0)
                    {
                        pNode.SelectedImageKey = "Polyline";
                        pNode.ImageKey = "Polyline";
                    }
                    else if (Strings.InStr(strCode, "PY", CompareMethod.Text) != 0)
                    {
                        pNode.SelectedImageKey = "Polygon";
                        pNode.ImageKey = "Polygon";
                    }
                    else if (Strings.InStr(strCode, "AN", CompareMethod.Text) != 0)
                    {
                        pNode.SelectedImageKey = "AN";
                        pNode.ImageKey = "AN";
                    }
                    else
                    {
                        pNode.SelectedImageKey = pNode.Tag.ToString();
                        pNode.ImageKey = pNode.Tag.ToString();

                    }
                }
                else
                {
                    pNode.SelectedImageKey = pNode.Tag.ToString();
                    pNode.ImageKey = pNode.Tag.ToString();

                }

                if (pNode.Nodes.Count != 0)
                {
                    for (i = 0; i <= pNode.Nodes.Count - 1; i++)
                    {
                        pNode1 = pNode.Nodes[i];
                        SetPic(pNode1, xmlDoc);
                    }
                }

            }
            catch (Exception )
            {
            }
        }

        /// <summary>
        /// 获取当前数据流的Unicode种类，返回对应的string值
        /// </summary>
        /// <param name="pBytes"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetUnicodeString(byte[] pBytes)
        {
            string strXML = null;

            if (pBytes == null)
            {
                return null;
            }
            if (pBytes.Length < 4)
            {
                return null;
            }
            //EF BB BF　　　 UTF-8
            if (pBytes[0] == 239 & pBytes[1] == 187 & pBytes[2] == 191)
            {
                strXML = System.Text.Encoding.UTF8.GetString(pBytes);
                strXML = strXML.Substring(1, strXML.Length - 1);
                return strXML;
            }
            //FE FF　　　　　UTF-16/UCS-2, little endian
            if (pBytes[0] == 254 & pBytes[1] == 255)
            {
                strXML = System.Text.Encoding.Unicode.GetString(pBytes);
                strXML = strXML.Substring(1, strXML.Length - 1);
                return strXML;
            }
            //FF FE　　　　　UTF-16/UCS-2, big endian
            if (pBytes[0] == 255 & pBytes[1] == 254)
            {
                strXML = System.Text.Encoding.Unicode.GetString(pBytes);
                strXML = strXML.Substring(1, strXML.Length - 1);
                return strXML;
            }
            //FF FE 00 00　　 UTF-32/UCS-4, little endian.
            if (pBytes[0] == 255 & pBytes[1] == 254 & pBytes[2] == 0 & pBytes[3] == 0)
            {
                strXML = System.Text.Encoding.UTF32.GetString(pBytes);
                strXML = strXML.Substring(1, strXML.Length - 1);
                return strXML;
            }
            //00 00 FE FF　　 UTF-32/UCS-4, big-endian. 
            if (pBytes[0] == 0 & pBytes[1] == 0 & pBytes[2] == 254 & pBytes[3] == 255)
            {
                strXML = System.Text.Encoding.UTF32.GetString(pBytes);
                strXML = strXML.Substring(1, strXML.Length - 1);
                return strXML;
            }
            strXML = System.Text.Encoding.Unicode.GetString(pBytes);
            //默认返回Unicode编码的字符串
            return strXML;
        }

        private static TreeNode FindRecursive(TreeNode treeNode,string text)
        {
            // Print the node.
            if (treeNode.Text==text)
            {
                return treeNode;
            }
            // Print each node recursively.
            foreach (TreeNode tn in treeNode.Nodes)
            {
                TreeNode temp = FindRecursive(tn, text);
                if (temp != null)
                {
                    return temp;
                }
                else
                {
                    continue;
                }
            }
            return null;
        }

        // Call the procedure using the TreeView.
        public static TreeNode CallRecursive(TreeView treeView,string text)
        {
            // Print each node recursively.
            TreeNodeCollection nodes = treeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                TreeNode temp=FindRecursive(n, text) ;
                if (temp== null)
                {
                    continue;
                }
                else
                {
                    return temp;
                }
            }
            return null;
        }

    }
}
