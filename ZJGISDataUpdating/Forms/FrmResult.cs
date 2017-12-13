using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ZJGISCommon;
using DevComponents.DotNetBar;
namespace ZJGISDataUpdating
{
    public partial class FrmResult : DevComponents.DotNetBar.Office2007Form
    {
        DevComponents.DotNetBar.TabControl m_TabControl;
        DevComponents.DotNetBar.Bar BottomStandBar; 
        public DevComponents.DotNetBar.TabControl tabControl
        {
            get
            {
                return m_TabControl;
            }
            set
            {
                m_TabControl = value;
            }
        }
        public  DevComponents.DotNetBar.Bar m_BottomStandBar
        {
            get
            {
                return BottomStandBar;
            }
            set
            {
                BottomStandBar = value;
            }
        }
        public FrmResult()
        {
            InitializeComponent();
        }

        private void FrmResult_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!m_TabControl.Tabs[0].Visible)
                m_TabControl.Tabs[0].Visible = true;

            if (m_TabControl.Tabs[1].Visible)
                m_TabControl.Tabs[1].Visible = false;
            if (m_TabControl.Tabs[2].Visible)
                m_TabControl.Tabs[2].Visible = false;
            //if (BottomStandBar.Visible)
            //{
            //    m_BottomStandBar.Visible = false;
            //}
        }

        private void FrmResult_Load(object sender, EventArgs e)
        {
            //if (!BottomStandBar.Visible)
            //{
            //    m_BottomStandBar.Visible = true;
            //}
        }
    }
}
