using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZJGISCommon;
using DevComponents.DotNetBar;

namespace ZJGISEnvirConfig
{
    public partial class FormMaintain : DevComponents.DotNetBar.Office2007Form
    {
        string strXML = System.Windows.Forms.Application.StartupPath + "\\..\\Res\\xml\\DBConfig.xml";
        string datasource = null;
        string server = null;
        string service = null;
        string user = null;
        string password = null;
        string version = null;
        string oUser = null;
        string oPass = null;
        public FormMaintain()
        {
            InitializeComponent();
            datasource =ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//OracleConnect", "DataSource");
            oUser = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//OracleConnect", "UserID");
            oPass = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//OracleConnect", "password");
            server =ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "SERVER");
            service = ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "SERVICE");
            user =ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "USER");
            password =ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "PASSWORD");
            version =ClsXml.GetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "VERSION");
        }

        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void FormMaintain_Load(object sender, EventArgs e)
        {
            textBoxXOName.Text = oUser;
            textBoxXOPass.Text = oPass;
            textBoxXOService.Text = datasource;
            ipAddressInputS.Value = server;
            integerInputPort.Text = service;
            textBoxXSUser.Text = user;
            textBoxXSPass.Text = password;
            textBoxXVersion.Text = version;
        }

        private void buttonXOK_Click(object sender, EventArgs e)
        {
            oUser = textBoxXOName.Text;
            oPass = textBoxXOPass.Text;
            datasource = textBoxXOService.Text;
            server = ipAddressInputS.Value;
            service = integerInputPort.Text;
            user = textBoxXSUser.Text;
            password = textBoxXSPass.Text;
            version = textBoxXVersion.Text;
            try
            {
                ClsXml.SetXmlNodeAttribute(strXML, "ConnetInfo//OracleConnect", "DataSource", datasource);
                ClsXml.SetXmlNodeAttribute(strXML, "ConnetInfo//OracleConnect", "UserID", oUser);
                ClsXml.SetXmlNodeAttribute(strXML, "ConnetInfo//OracleConnect", "password", oPass);
                ClsXml.SetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "SERVER", server);
                ClsXml.SetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "SERVICE", service);
                ClsXml.SetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "USER", user);
                ClsXml.SetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "PASSWORD", password);
                ClsXml.SetXmlNodeAttribute(strXML, "ConnetInfo//SdeConnect", "VERSION", version);

                MessageBoxEx.Show("保存成功，下次启动时生效！");
                this.DialogResult = DialogResult.OK;
            }
            catch(Exception err)
            {
                MessageBoxEx.Show(err.ToString());
            }
            
        }
    }
}
