using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Xml;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ZJGISCommon;

namespace ZJGISCommon
{
    public class ClsSysSet
    {
        private OracleConnection m_SysCn;//与系统维护库之间的连接字符
        private string m_SysSQL;
        private OracleConnection m_SysCnMetaData;//与元数据库之间的连接
        private string m_SysSQLMetaData; //与元数据库之间的连接字符串

        private string m_sSDEServer;//SDE库服务名         
        private string m_sSDEPort;//SDE库端口号
        private string m_sSDEUser; //SDE库用户名
        private string m_sSDEPSW;//SDE库用户密码
        private string m_sSDEVersion;//SDE库用户版本

        private string m_DbName;//系统维护库用户连接名称      
        private string m_DbUser;//系统维护库用户名        
        private string m_DbPass;//系统维护库用户密码
        private int m_SavePass;//是否保存系统维护库密码

        private string m_SysUser; //用户名

        private string m_MDDbName; //元数据库用户连接名称      
        private string m_MDDbUser; //元数据库用户名        
        private string m_MDDbPass;//元数据库用户密码    
        private int m_MDSavePass;//是否保存元数据库密码

        private bool m_SuperUser;//是否超级用户
        private string m_SuperUserName;//超级用户名
        private string m_SuperUserCode;//超级用户名代码
        private string m_SuperUserPassword;//超级用户密码

        private Collection<object> m_QueryResultCln;        //查询结果     
        private Collection<object> m_SelectedItem; //点击Toc获得的图层
        private long m_MaxSelectCount;  //最大选择个数

        ////other
        private Form m_CurClientForm;
        private Form m_frmMain;
        private Collection<object> m_ConnInfoCol;//图层集合、连接信息集合
        private Collection<object> m_ADOConnCol;//基础库的ADO.Net连接集合
        private double m_dblDefMapScale;//基础库的定义显示比例尺 -1 通比例尺显示 0 全比例尺数据显示  》0 指定比例尺显示          
        private XmlDocument m_vMapViewXMLDoc;//用于数据显示的XML文档 在系统加载时放入系统内存中 以便放大缩小时快速查询

        ////主界面控件
        private IMapControl3 m_MapControl;
        private IPageLayoutControl3 m_PageLayoutControl;
        private ITOCControl m_TocControl;

        ////与业务系统的交互相关XML变量
        private XmlDocument m_LyrXMLDoc;  //数据xml，在系统维护系统中管理
        private XmlDocument m_CodeXMLDoc;//代码对应关系xml，在系统维护系统中管理
        private XmlDocument m_RightXMLDoc;//权限xml，在系统维护系统中管理
        private XmlDocument m_RuleXmlDoc; //规则xml，在数据管理系统中管理
        private XmlDocument m_TransXMLDoc;//

        private short m_SelectStyle;

        private ClsUser vUser; //登录用户信息
        private ClsRole vRole; //登录角色信息
        //private ClsPrivilege vPriv;//登录权限信息
        private string vInstance; //数据库实例名
        private IEnvelope m_FullPageExtent;//制图视图范围



        public double DblDefMapScale
        {
            get
            {
                return m_dblDefMapScale;
            }
            set
            {
                m_dblDefMapScale = value;
            }
        }

        public XmlDocument RuleXmlDoc
        {
            get
            {
                return m_RuleXmlDoc;
            }
            set
            {
                m_RuleXmlDoc = value;
            }
        }

        public IEnvelope FullPageExtent
        {
            get
            {
                return m_FullPageExtent;
            }
            set
            {
                m_FullPageExtent = value;
            }
        }

        public ClsUser User
        {
            get
            {
                return vUser;
            }
            set
            {
                vUser = value;
            }
        }

        public ClsRole Role
        {
            get
            {
                return vRole;
            }
            set
            {
                vRole = value;
            }
        }

        //public ClsPrivilege Priv
        //{
        //    get
        //    {
        //        return vPriv;
        //    }
        //    set
        //    {
        //        vPriv = value;
        //    }
        //}

        public string Instance
        {
            get
            {
                return vInstance;
            }
            set
            {
                vInstance = value;
            }
        }

        public short SelectStyle
        {
            get
            {
                return m_SelectStyle;
            }

            set
            {
                m_SelectStyle = value;
            }
        }

        ////系统连接、连接字符串、用户等属性
        ////与系统维护库之间的连接
        public OracleConnection SysCn
        {
            get
            {
                return m_SysCn;
            }

            set
            {
                m_SysCn = value;
            }
        }

        ////与系统维护库之间的连接字符串
        public string SysSQL
        {
            get
            {
                return m_SysSQL;
            }
            set
            {
                m_SysSQL = value;
            }
        }

        ////与元数据库之间的连接
        public OracleConnection SysCnMetaData
        {
            get
            {
                return m_SysCnMetaData;
            }

            set
            {
                m_SysCnMetaData = value;
            }
        }

        ////与元数据库之间的连接字符串
        public string SysSQLMetaData
        {

            get
            {
                return m_SysSQLMetaData;
            }

            set
            {
                m_SysSQLMetaData = value;
            }
        }

        /// <summary>
        /// SDE库服务名
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SDEServer
        {
            get
            {
                return m_sSDEServer;
            }

            set
            {
                m_sSDEServer = value;
            }
        }

        /// <summary>
        /// SDE端口号
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SDEPort
        {
            get
            {
                return m_sSDEPort;
            }

            set
            {
                m_sSDEPort = value;
            }
        }

        /// <summary>
        /// SDE版本信息
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SDEVersion
        {
            get
            {
                return m_sSDEVersion;
            }

            set
            {
                m_sSDEVersion = value;
            }
        }

        /// <summary>
        /// SDE用户名
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SDEUser
        {
            get
            {
                return m_sSDEUser;
            }

            set
            {
                m_sSDEUser = value;
            }
        }

        /// <summary>
        /// SDE用户密码
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SDEPSW
        {
            get
            {
                return m_sSDEPSW;
            }

            set
            {
                m_sSDEPSW = value;
            }
        }


        ////系统维护库用户连接名称
        public string DbName
        {
            get
            {
                return m_DbName;
            }

            set
            {
                m_DbName = value;
            }
        }

        ////系统维护库用户名
        public string DbUser
        {
            get
            {
                return m_DbUser;
            }

            set
            {
                m_DbUser = value;
            }
        }

        ////系统维护库用户密码
        public string DbPass
        {

            get
            {
                return m_DbPass;
            }

            set
            {
                m_DbPass = value;
            }
        }

        ////是否保存系统维护库密码
        public int SavePass
        {
            get
            {
                return m_SavePass;
            }

            set
            {
                m_SavePass = value;
            }
        }

        ////系统用户信息
        ////用户名
        public string SysUser
        {
            get
            {
                return m_SysUser;
            }

            set
            {
                m_SysUser = value;
            }
        }

        ////元数据连接用户信息
        public string MDDbName
        {
            ////元数据库用户连接名称
            get
            {
                return m_MDDbName;
            }

            set
            {
                m_MDDbName = value;
            }
        }

        ////元数据库用户名
        public string MDDbUser
        {
            get
            {
                return m_MDDbUser;
            }

            set
            {
                m_MDDbUser = value;
            }
        }

        ////元数据库用户密码
        public string MDDbPass
        {
            get
            {
                return m_MDDbPass;
            }

            set
            {
                m_MDDbPass = value;
            }
        }

        ////是否保存元数据库密码
        public int MDSavePass
        {
            get
            {
                return m_MDSavePass;
            }

            set
            {
                m_MDSavePass = value;
            }
        }

        ////系统用户信息

        ////是否超级用户
        public bool SuperUser
        {
            get
            {
                return m_SuperUser;
            }

            set
            {
                m_SuperUser = value;
            }
        }

        ////元数据连接用户信息
        ////超级用户名
        public string SuperUserName
        {
            get
            {
                return m_SuperUserName;
            }

            set
            {
                m_SuperUserName = value;
            }
        }

        ////超级用户名代码
        public string SuperUserCode
        {
            get
            {
                return m_SuperUserCode;
            }

            set
            {
                m_SuperUserCode = value;
            }
        }

        ////超级用户密码
        public string SuperUserPassword
        {
            get
            {
                return m_SuperUserPassword;
            }

            set
            {
                m_SuperUserPassword = value;
            }
        }

        ////other属性
        public Form CurClientForm
        {
            get
            {
                return m_CurClientForm;
            }

            set
            {
                m_CurClientForm = value;
            }
        }

        ////查询结果
        public Collection<object> QueryResultCln
        {
            get
            {
                return m_QueryResultCln;
            }

            set
            {
                m_QueryResultCln = value;
            }
        }

        ////最大选择个数
        public long MaxSelectCount
        {
            get
            {
                return m_MaxSelectCount;
            }

            set
            {
                m_MaxSelectCount = value;
            }
        }


        ////Toc中选择的图层
        public Collection<object> SelectedItem
        {
            get
            {
                return m_SelectedItem;
            }

            set
            {
                m_SelectedItem = value;
            }
        }

        ////MapControl与PageLayoutControl
        public IMapControl3 MapControl
        {
            get
            {
                return m_MapControl;
            }

            set
            {
                m_MapControl = value;
            }
        }

        public IPageLayoutControl3 PageLayoutControl
        {
            get
            {
                return m_PageLayoutControl;
            }

            set
            {
                m_PageLayoutControl = value;
            }
        }

        ////TocControl
        public ITOCControl TocControl
        {
            get
            {
                return m_TocControl;
            }

            set
            {
                m_TocControl = value;
            }
        }

        ////frmMain
        public Form FrmMain
        {
            get
            {
                return m_frmMain;
            }

            set
            {
                m_frmMain = value;
            }
        }

        ////与业务系统的交互相关XML属性
        //数据xml，在系统维护系统中管理
        public XmlDocument LyrXMLDoc
        {
            get
            {
                return m_LyrXMLDoc;
            }

            set
            {
                m_LyrXMLDoc = value;
            }
        }

        //权限xml，在系统维护系统中管理
        public XmlDocument RightXMLDoc
        {
            get
            {
                return m_RightXMLDoc;
            }

            set
            {
                m_RightXMLDoc = value;
            }
        }

        //规则xml，在数据管理系统中管理
        public XmlDocument CodeXMLDoc
        {
            get
            {
                return m_CodeXMLDoc;
            }

            set
            {
                m_CodeXMLDoc = value;
            }
        }

        //代码对应关系xml，在系统维护系统中管理
        public XmlDocument TransXMLDoc
        {
            get
            {
                return m_TransXMLDoc;
            }

            set
            {
                m_TransXMLDoc = value;
            }
        }

        ////显示规则
        public XmlDocument MapViewXMLDoc
        {
            get
            {
                return m_vMapViewXMLDoc;
            }

            set
            {
                m_vMapViewXMLDoc = value;
            }
        }

        ////从用户权限xml中解析出来的连接信息集合、连接信息下的图层集合
        public Collection<object> ConnInfo
        {
            get
            {
                return m_ConnInfoCol;
            }

            set
            {
                m_ConnInfoCol = value;
            }
        }

        ////从用户权限xml中解析出来的ORACLE的 ADO.NET连接信息
        public Collection<object> ADOConnInfo
        {
            get
            {
                return m_ADOConnCol;
            }

            set
            {
                m_ADOConnCol = value;
            }
        }

    }
}
