using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using Microsoft.VisualBasic;
using ZJGISCommon;

namespace ZJGISCommon
{
    public partial class FrmSDE : DevComponents.DotNetBar.Office2007Form
    {
        public IWorkspace m_pSDEWorkspace;
        public IPropertySet m_SDEPropertSet;
        private string[] pStrProp = new string[7];

        public IWorkspace SDEWorkspace
        {
            get
            {
                return m_pSDEWorkspace;
            }
        }

        //属性值数组
        public string[] Properties
        {
            get
            {
                return pStrProp;
            }
            set
            {
                pStrProp = value;
            }
        }

        public FrmSDE()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 连接按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSDE_Click(object sender, EventArgs e)
        {
            //修改服务器SDE链接方式，无需单独测试链接，点击连接时自动测试
            if (SDEConnectTest() == true)
            {
                //pStrProp[5] = cboVersion.Text;
                m_SDEPropertSet = ClsSDE.GetPropSetFromArr(pStrProp);
                ClsSDE.CheckTxtComplete(pStrProp);
                m_pSDEWorkspace = ClsSDE.TestSDELinkState(m_SDEPropertSet);
                if ((m_pSDEWorkspace != null))
                {
                    Properties = pStrProp;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    ClsDBInfo.SdeWorkspace = m_pSDEWorkspace;
                }
            }
        }

        private void FrmSDE_Load(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            //获取上次连接信息
            GetPropSetting();
        }

        /// <summary>
        /// 测试SDE链接
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool SDEConnectTest()
        {
            ////把界面中的信息作为参数放在数组pStrProp中
            pStrProp[0] = this.txtSet1.Text;
            pStrProp[1] = this.txtSet2.Text;
            pStrProp[2] = this.txtSet3.Text;
            pStrProp[3] = this.txtSet4.Text;
            pStrProp[4] = this.txtSet5.Text;
            pStrProp[5] = this.comboBoxEx1.Text;
            pStrProp[6] = this.comboBoxEx2.Text;

            //主要为了测试连接用，测试连接函数里面用的是IproperSet
            m_SDEPropertSet = ClsSDE.GetPropSetFromArr(pStrProp);

            ////判断是否已经填写完毕
            if (ClsSDE.CheckTxtComplete(pStrProp) == true)
            {
                ////测试连接
                m_pSDEWorkspace = ClsSDE.TestSDELinkState(m_SDEPropertSet);
                if ((m_pSDEWorkspace != null))
                {
                    ClsSDE.SavePropSetting(pStrProp);
                    AddVersionsToComb(m_pSDEWorkspace);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private void AddVersionsToComb(IWorkspace pWorkSpace)
        {
            IVersionedWorkspace pVerWorkspace = null;
            IEnumVersionInfo pEnumVersionInfo = null;
            pVerWorkspace = (IVersionedWorkspace)pWorkSpace;
            pEnumVersionInfo = pVerWorkspace.Versions;
            pEnumVersionInfo.Reset();

            IVersionInfo pVersionInfo = default(IVersionInfo);
            pVersionInfo = pEnumVersionInfo.Next();

            ////在组合框中加载sde的所有版本
            while (!(pVersionInfo == null))
            {
                cboVersion.Items.Add(pVersionInfo.VersionName);
                pVersionInfo = pEnumVersionInfo.Next();
            }
            cboVersion.Text = "SDE.DEFAULT";
        }
        /// <summary>
        /// 得到上次的SDE连接设置
        /// </summary>
        private void GetPropSetting()
        {
            ////得到上次的SDE连接设置
            this.txtSet1.Text = Interaction.GetSetting(Application.CompanyName, "SDESeting", "Server", "");
            this.txtSet2.Text = Interaction.GetSetting(Application.CompanyName, "SDESeting", "Instance", "");
            this.txtSet3.Text = Interaction.GetSetting(Application.CompanyName, "SDESeting", "Database", "");
            this.txtSet4.Text = Interaction.GetSetting(Application.CompanyName, "SDESeting", "user", "");
            this.txtSet5.Text = Interaction.GetSetting(Application.CompanyName, "SDESeting", "password", "");
            //if (txtSet1.Text == "" && txtSet2.Text == "" && txtSet3.Text == "" && txtSet4.Text == "")
            if (txtSet1.Text == "" || txtSet2.Text == "" || txtSet3.Text == "" || txtSet4.Text == "" || txtSet5.Text == "")
            {
                ClsConnectInfo connectInfo = new ClsConnectInfo();
                connectInfo.GetInfoFromXml();
                txtSet1.Text = connectInfo.Server;
                txtSet2.Text = connectInfo.Service;

                txtSet3.Text = connectInfo.Database;

                txtSet4.Text = connectInfo.User;
                txtSet5.Text = connectInfo.Password;
                comboBoxEx1.Text = connectInfo.Version;
                comboBoxEx2.Text = connectInfo.Authenication_mode;

            }
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
