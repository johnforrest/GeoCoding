using System;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ZJGISCommon;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;

namespace ZJGISCommon
{
    public class ClsConnectInfo
    {
        private string server;
        private string service;
        private string user;
        private string password;
        private string version;
        private string datasource;
        private Collection<object> collection;
        string m_strFile;

        private Dictionary<string, string> m_dicDLG = new Dictionary<string, string>();
        private Dictionary<string, string> m_dicSDE = new Dictionary<string, string>();
        private string oUser;
        private string oPass;

        private XmlDocument xmlDoc = new XmlDocument();
        private XmlNode xmlNode;
        public string OUser
        {
            get
            {
                return oUser;
            }
            set
            {
                oUser = value;
            }
        }

        public string OPass
        {
            get
            {
                return oPass;
            }
            set
            {
                oPass = value;
            }
        }

        public string Server
        {
            get
            {
                return server;
            }
            set
            {
                server = value;
            }
        }


        public string Service
        {
            get
            {
                return service;
            }
            set
            {
                service = value;
            }
        }


        public string User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }


        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }


        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }


        public string Datasource
        {
            get
            {
                return datasource;
            }
            set
            {
                datasource = value;
            }
        }


        public Collection<object> Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
            }
        }



        /// <summary>
        /// 通过权限信息取得IWorkspace
        /// </summary>
        /// <returns>工作空间</returns>
        public IWorkspace GetWorkspace()
        {
            IWorkspaceFactory pWorkSpaceFactory=null;
            IPropertySet pPropertySet=null;

            try
            {
                pWorkSpaceFactory = new SdeWorkspaceFactory();
                pPropertySet = new PropertySet();

                //设置属性集参数
                pPropertySet.SetProperty("Server", Server);
                pPropertySet.SetProperty("Instance", Service);
                pPropertySet.SetProperty("Database",Datasource);
                pPropertySet.SetProperty("User", User);
                pPropertySet.SetProperty("Password", Password);
                pPropertySet.SetProperty("Version", Version);

                //返回打开的工作空间
                return pWorkSpaceFactory.Open(pPropertySet, 0);
            }
            catch (Exception )
            {
                pWorkSpaceFactory = null;
                pPropertySet = null;

                //异常后返回空值
                return null;
            }
        }


        /// <summary>
        /// 获取属性集
        /// </summary>
        /// <returns>属性集</returns>
        public IPropertySet GetPropertySet()
        {
            IPropertySet pPropertySet = null;

            try
            {
                pPropertySet = new PropertySet();

                //设置一个新的属性集
                pPropertySet.SetProperty("Server", server);
                pPropertySet.SetProperty("Instance", service);
                pPropertySet.SetProperty("User", user);
                pPropertySet.SetProperty("Password", password);
                pPropertySet.SetProperty("Version", version);

                //返回属性集
                return pPropertySet;
            }
            catch (Exception)
            {
                pPropertySet = null;

                //如果产生异常返回空值
                return null;
            }
        }


        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        public string GetAdoConn()
        {
            return "Data Source=" + datasource + ";User ID=" + oUser + ";Password=" + oPass;
        }


        /// <summary>
        /// 获取一个要素类的连接信息
        /// </summary>
        /// <param name="pFeatureClass">欲查询的要素类</param>
        /// <param name="vName">连接属性</param>
        /// <returns>查询到的连接信息</returns>
        public string GetConnectProperty(IFeatureClass pFeatureClass, string vName)
        {

            try
            {
                IWorkspace pWorkspace = null;
                IDataset pDataSet = null;
                pDataSet = pFeatureClass as IDataset;
                pWorkspace = pDataSet.Workspace;

                object sPropNames = null;
                object vPropValues = null;

                //得到此工作空间的连接信息
                pWorkspace.ConnectionProperties.GetAllProperties(out sPropNames, out vPropValues);

                //得到对应属性的连接信息值
                string[] temName = sPropNames as string[];
                string[] temValue = vPropValues as string[];
                for (int i = 0; i < temName.Length; i++)
                {
                    if (temName[i].ToUpper() == vName.ToUpper())
                    {
                        return temValue[i];
                    }
                }
            }
            catch (Exception)
            {
            }

            return null;
        }


        /// <summary>
        /// 根据连接信息获取工作空间
        /// </summary>
        /// <param name="clsConninfo">用于提供连接信息的类</param>
        /// <returns>工作空间</returns>
        public IWorkspace GetWorkspace(ClsConnectInfo clsConninfo)
        {
            IWorkspaceFactory pWorkSpaceFactory = null;
            IPropertySet pPropertySet = null;

            pPropertySet = new PropertySet();
            pWorkSpaceFactory = new SdeWorkspaceFactory();

            //根据连接信息的对应信息设置属性集
            pPropertySet.SetProperty("Server", clsConninfo.Server);
            pPropertySet.SetProperty("Instance", clsConninfo.Service);
            pPropertySet.SetProperty("User", clsConninfo.User);
            pPropertySet.SetProperty("Password", clsConninfo.Password);
            pPropertySet.SetProperty("Version", clsConninfo.Version);

            //返回打开的工作空间
            return pWorkSpaceFactory.Open(pPropertySet, 0);

        }
        /// <summary>
        /// 读取DBConfig.xml文件中的配置信息
        /// </summary>
        public void GetInfoFromXml()
        {
            string strXML = System.Windows.Forms.Application.StartupPath + "\\..\\Res\\xml\\DBConfig.xml";
            datasource = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//OracleConnect", "DataSource");
            oUser = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//OracleConnect", "UserID");
            oPass = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//OracleConnect", "password");
            server = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "SERVER");
            service = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "SERVICE");
            user = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "USER");
            password = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "PASSWORD");
            version = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "VERSION");
        }

        private void ReadParaFromXmlToDic()
        {
            m_strFile = Application.StartupPath + @"\..\Res\Xml\DBConnInfo.xml";
            if (System.IO.File.Exists(m_strFile) == false)
            {
                MessageBox.Show("文件DBConnInfo.xml不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                xmlDoc.Load(m_strFile);
                xmlNode = xmlDoc.DocumentElement["DLG"];
                foreach (XmlAttribute xmlAttr in xmlNode.Attributes)
                {
                    string strName = xmlAttr.Name;
                    if (m_dicDLG.ContainsKey(strName) == false)
                    {
                        m_dicDLG.Add(strName, xmlNode.Attributes[strName].Value.ToString());
                    }
                }
                xmlNode = xmlDoc.DocumentElement["SDE"];
                foreach (XmlAttribute xmlAttr in xmlNode.Attributes)
                {
                    string strName = xmlAttr.Name;
                    if (m_dicSDE.ContainsKey(strName) == false)
                    {
                        m_dicSDE.Add(strName, xmlNode.Attributes[strName].Value.ToString());
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
