using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.VisualBasic;

namespace ZJGISLayerManager
{
    public partial class FrmRotateRenderer : DevComponents.DotNetBar.Office2007Form
    {
        public IFeatureLayer m_FeatureLayer;
        private IFeatureClass m_FeatureClass;
        private IRotationRenderer m_RotationRenderer;

        public FrmRotateRenderer()
        {
            InitializeComponent();
        }

        private void FrmRotateRenderer_Load(object sender, EventArgs e)
        {
            m_FeatureClass = m_FeatureLayer.FeatureClass;
            FunAddNumberFieldToCombo();
            FunReadRotation();
        }
        //*********************************************************************************
        //** 函 数 名：FunAddNumberFieldToCombo
        //** 输    入：
        //** 输    出：
        //** 功能描述：读取数值型字段
        //** 全局变量：
        //** 调用模块：
        //*********************************************************************************
        private void FunAddNumberFieldToCombo()
        {
            IFields pFlds;
            pFlds = m_FeatureClass.Fields;
            IField pFld;
            int i;

            this.CmbField.Items.Clear();
            for (i = 0; i <= pFlds.FieldCount-1; i++)
            {
                pFld = pFlds.get_Field(i);
                if ((pFld.VarType == Convert.ToInt32(Constants.vbLong)) || (pFld.VarType == Convert.ToInt32(Constants.vbInteger)) 
                    || (pFld.VarType == Convert.ToInt32(Constants.vbSingle)) || (pFld.VarType == Convert.ToInt32(Constants.vbDouble)))
                {
                    this.CmbField.Items.Add(pFld.Name);
                }
            }
            this.CmbField.Items.Add("<无>");
        }
        //*********************************************************************************
        //** 函 数 名：FunReadRotation
        //** 输    入：
        //** 输    出：
        //** 功能描述：读取RotationRenderer
        //** 全局变量：
        //** 调用模块：
        //*********************************************************************************
        private void FunReadRotation()
        {
            IFeatureRenderer pRenderer;
            IGeoFeatureLayer pGeoFeatureLayer;
            pGeoFeatureLayer = (IGeoFeatureLayer)m_FeatureLayer;
            pRenderer = pGeoFeatureLayer.Renderer;
            m_RotationRenderer = (IRotationRenderer)pRenderer;

            if (m_RotationRenderer!=null)
            {
                try
                {
                    if (m_RotationRenderer.RotationField!="")
                    {
                        this.CmbField.Text = m_RotationRenderer.RotationField;
                    }
                    else
                    {
                        this.CmbField.Text = "<无>";
                    }

                    if (m_RotationRenderer.RotationField==esriSymbolRotationType.esriRotateSymbolGeographic.ToString())
                    {
                        this.rdoGeographic.Checked = true;
                    }
                    else
                    {
                        this.rdoMath.Checked = true;
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.CmbField.Text!="<无>")
            {
                m_RotationRenderer.RotationField = this.CmbField.Text;
                m_RotationRenderer.RotationType = (esriSymbolRotationType)Interaction.IIf(this.rdoGeographic.Checked = true, esriSymbolRotationType.esriRotateSymbolGeographic, esriSymbolRotationType.esriRotateSymbolArithmetic);
            }
            else
            {
                m_RotationRenderer.RotationField = "";
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }


    }
}
