//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：FrmLabelPntChoiseField
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：在该窗体中选择字段设置标注的旋转角度
//    创建日期：
//    修改人：
//    修改说明：
//    修改日期：
//-------------------------------------------------------------------------------------
//</CSCC>


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
//using ErrorHandle;
//using ZJGISDialog;

namespace ZJGISLayerManager
{
    public partial class FrmLabelPntChoiseField :DevComponents.DotNetBar.Office2007Form
    {

        private IGeoFeatureLayer m_pGeoFeatLayer;
        private IBasicOverposterLayerProperties4 m_pBasicOverposterLayerProperties;
        //private ClsErrorHandle m_pDisplayInformation = new ClsErrorHandle();

        public FrmLabelPntChoiseField()
        {
            InitializeComponent();
        }

        //获得设置的图层
        public IGeoFeatureLayer GeoFeatureLayer
        {
            set
            {
                m_pGeoFeatLayer = value;
            }
        }

        //获得设置图层的位置相关的属性
        public IBasicOverposterLayerProperties4 BasicOverposterLayerProperties
        {
            set
            {
                m_pBasicOverposterLayerProperties = value;
            }
        }

        private void FrmLabelPntChoiseField_Load(object sender, EventArgs e)
        {
            IField pField;
            IFields pFields;
            bool bHasFindOneNumbricField;
            string sValue = "";         //如果该图层没有设置过控置角度的字段的话，则记录第一个数字型字段的名称赋给combox
            int i;
            int iFieldIndex;

            pFields = m_pGeoFeatLayer.FeatureClass.Fields;
            bHasFindOneNumbricField = false;
            //向字段下拉框中添加字段名称及其别名
            cboFieldsName.Items.Clear();

            for (i = 0; i <= pFields.FieldCount-1; i++)
            {
                pField = pFields.get_Field(i);

                if ((pField.Type==esriFieldType.esriFieldTypeDouble)||(pField.Type==esriFieldType.esriFieldTypeInteger)
                    ||(pField.Type==esriFieldType.esriFieldTypeSingle)||(pField.Type==esriFieldType.esriFieldTypeSmallInteger))
                {
                    cboFieldsName.Items.Add(pField.Name + "[" + pField.AliasName + "]");
                    if (bHasFindOneNumbricField==false)
                    {
                        sValue = pField.Name + "[" + pField.AliasName + "]";
                        bHasFindOneNumbricField = true;
                    }
                }
            }

            //初始化字段下拉框
            if (m_pBasicOverposterLayerProperties.FeatureType==esriBasicOverposterFeatureType.esriOverposterPoint)
            {
                //如果找到控制字段的话
                if ((m_pBasicOverposterLayerProperties.RotationField+"").Trim()!="")
                {
                    iFieldIndex = pFields.FindField(m_pBasicOverposterLayerProperties.RotationField);
                    if (iFieldIndex>0)
                    {
                        pField = pFields.get_Field(iFieldIndex);
                        cboFieldsName.Text = pField.Name + "[" + pField.AliasName + "]";
                    }
                }
                //否则，则使用一个数字型的字段进行初始化
                else
                {
                    cboFieldsName.Text = sValue;
                }
            }

            //初始化旋转坐标系
            if (m_pBasicOverposterLayerProperties.RotationType==esriLabelRotationType.esriRotateLabelArithmetic)
            {
                optArithmetic.Checked = true;
            }
            else
            {
                optArithmetic.Checked = true;
            }

            //初始化旋转角度是否在原有字段的基础上+90度
            if (m_pBasicOverposterLayerProperties.PerpendicularToAngle==true)
            {
                chkRation90.Checked = true;
            }
            else
            {
                chkRation90.Checked = false;
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string[] sFieldName;

            //设置控制旋转角度的字段
            if ((cboFieldsName.Text+"").Trim()=="")
            {
                //m_pDisplayInformation.DisplayInformation("请选择字段名!", false, null, "退出");
                MessageBox.Show("请选择字段名!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }
            else
            {
                sFieldName=cboFieldsName.Text.Split(new char[] {'['});
                m_pBasicOverposterLayerProperties.RotationField = sFieldName[0];
            }

            //设置施转时的参照坐标系
            if (optGeographic.Checked==true)
            {
                m_pBasicOverposterLayerProperties.RotationType= esriLabelRotationType.esriRotateLabelGeographic;
            }
            else
            {
                m_pBasicOverposterLayerProperties.RotationType = esriLabelRotationType.esriRotateLabelArithmetic;
            }

            //设置是否加90度
            if (chkRation90.Checked==true)
            {
                m_pBasicOverposterLayerProperties.PerpendicularToAngle = true;
            }
            else
            {
                m_pBasicOverposterLayerProperties.PerpendicularToAngle = false;
            }

            this.Close();
        }

    }
}
