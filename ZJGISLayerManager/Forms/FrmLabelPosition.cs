//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：FrmLabelPosition
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：在该窗体中设置标注的位置
//    创建日期：
//    修改人：MXF
//    修改说明：btnAddAngle_Click()函数中修改一处
//    修改日期：11-10-2008
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
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using Microsoft.VisualBasic;
//using ErrorHandle;
//using ZJGISDialog;

namespace ZJGISLayerManager
{
    public partial class FrmLabelPosition : DevComponents.DotNetBar.Office2007Form
    {
        private IBasicOverposterLayerProperties4 m_pBasicOverposterLayerProperties;
        private IBasicOverposterLayerProperties4 m_pBasicOverposterLayerPropertiesOut;
        private ILabelEngineLayerProperties m_pLabeEnginProperty;
        private IGeoFeatureLayer m_pGeoFeatLayer;
        //private ClsErrorHandle m_pDisplayInformation = new ClsErrorHandle();
        private string[] strGroupBoxes = { "grpLineSet", "grpRegionSet", "grpPointSet" };

        public FrmLabelPosition()
        {
            InitializeComponent();
        }

        //用于向上一级窗体传值，获得字段
        public IGeoFeatureLayer GeoFeatureLayer
        {
            set
            {
                m_pGeoFeatLayer = value;
            }
        }

        //用于从外部获取标注属性
        public ILabelEngineLayerProperties LabelEngineLayerProperties
        {
            set
            {
                m_pLabeEnginProperty = value;
                m_pBasicOverposterLayerProperties = (IBasicOverposterLayerProperties4)m_pLabeEnginProperty.BasicOverposterLayerProperties;
            }
        }

        public IBasicOverposterLayerProperties4 BasicOverposterLayerProperties
        {
            get
            {
                return m_pBasicOverposterLayerPropertiesOut;
            }
        }

        private void FrmLabelPosition_Load(object sender, EventArgs e)
        {
            IClone pClone;
            IFields pFields;
            IField pField;

            //标识是否找到了数字型的字段
            bool bHasFindNumbericField;
            int i;

            try
            {
                if (m_pBasicOverposterLayerProperties == null)
                {
                    return;
                }

                //复制一份传入的 BasicOverposterLayerProperties
                pClone = (IClone)m_pBasicOverposterLayerProperties;
                m_pBasicOverposterLayerPropertiesOut = (IBasicOverposterLayerProperties4)pClone.Clone();

                //对所有的标注层的初始化(重复标注的处理)
                if (m_pBasicOverposterLayerProperties.NumLabelsOption == esriBasicNumLabelsOption.esriOneLabelPerName)
                {
                    optRemoveDoubleLable.Checked = true;        //移除值重复的标注
                }
                else if (m_pBasicOverposterLayerProperties.NumLabelsOption == esriBasicNumLabelsOption.esriOneLabelPerPart)
                {
                    optPerFeature.Checked = true;               //每个地物只有一个标注
                }
                else
                {
                    optPerPart.Checked = true;                  //每个几何图形有一个标注
                }

                //显示要素和标注的重要性
                //要素重要性
                switch ((int)m_pBasicOverposterLayerPropertiesOut.FeatureWeight)
                {
                    case 0:
                        OptionF1.Checked = false;
                        OptionF2.Checked = false;
                        OptionF3.Checked = false;
                        OptionF0.Checked = true;
                        break;
                    case 1:
                        OptionF1.Checked = false;
                        OptionF2.Checked = false;
                        OptionF3.Checked = true;
                        OptionF0.Checked = false;
                        break;
                    case 2:
                        OptionF1.Checked = false;
                        OptionF2.Checked = true;
                        OptionF3.Checked = false;
                        OptionF0.Checked = false;
                        break;
                    case 3:
                        OptionF1.Checked = true;
                        OptionF2.Checked = false;
                        OptionF3.Checked = false;
                        OptionF0.Checked = false;
                        break;
                    default:
                        break;
                }

                //标注重要性
                switch ((int)m_pBasicOverposterLayerPropertiesOut.LabelWeight)
                {
                    case 0:
                        OptionL1.Checked = false;
                        OptionL2.Checked = false;
                        OptionL3.Checked = false;
                        break;
                    case 1:
                        OptionL1.Checked = false;
                        OptionL2.Checked = false;
                        OptionL3.Checked = true;
                        break;
                    case 2:
                        OptionL1.Checked = false;
                        OptionL2.Checked = true;
                        OptionL3.Checked = false;
                        break;
                    case 3:
                        OptionL1.Checked = true;
                        OptionL2.Checked = false;
                        OptionL3.Checked = false;
                        break;
                    default:
                        break;
                }

                if (m_pBasicOverposterLayerProperties != null)
                {
                    //下面是标注层是线时的初始化
                    if (m_pBasicOverposterLayerProperties.FeatureType == esriBasicOverposterFeatureType.esriOverposterPolyline)
                    {
                        InitialGroupBox("grpLineSet");
                        //线框架初始化的函数
                        InitialLinePosition();
                    }

                    //面标注层的处理
                    else if (m_pBasicOverposterLayerProperties.FeatureType == esriBasicOverposterFeatureType.esriOverposterPolygon)
                    {
                        InitialGroupBox("grpRegionSet");

                        if (m_pBasicOverposterLayerProperties.PlaceOnlyInsidePolygon == true)
                        {
                            chkLabelInRegion.Checked = true;                //标注要位于多边形内
                        }
                        else
                        {
                            chkLabelInRegion.Checked = false;
                        }

                        if (m_pBasicOverposterLayerProperties.PolygonPlacementMethod == esriOverposterPolygonPlacementMethod.esriAlwaysHorizontal)
                        {
                            optHorizatalOrentation.Checked = true;          //总是水平
                        }
                        else if (m_pBasicOverposterLayerProperties.PolygonPlacementMethod == esriOverposterPolygonPlacementMethod.esriAlwaysStraight)
                        {
                            optRegionOrentation.Checked = true;             //与面的方向一致
                        }
                        else
                        {
                            optOptimizationOrentation.Checked = true;       //最优方向
                        }
                    }
                    //点标注图层的处理
                    else
                    {
                        InitialGroupBox("grpPointSet");

                        //查看是否存在数字型字段
                        pFields = m_pGeoFeatLayer.FeatureClass.Fields;
                        bHasFindNumbericField = false;

                        for (i = 0; i <= pFields.FieldCount - 1; i++)
                        {
                            pField = pFields.get_Field(i);
                            if ((pField.Type == esriFieldType.esriFieldTypeDouble) || (pField.Type == esriFieldType.esriFieldTypeInteger)
                                || (pField.Type == esriFieldType.esriFieldTypeSingle) || (pField.Type == esriFieldType.esriFieldTypeSmallInteger))
                            {
                                bHasFindNumbericField = true;
                                break;
                            }
                        }

                        //如果不存在数值型字段则依字段进行控制角度的控钮灰掉
                        if (bHasFindNumbericField == false)
                        {
                            optRotationField.Enabled = false;
                            this.btnChooseField.Enabled = false;
                        }

                        //初始化点标注类型
                        if (m_pBasicOverposterLayerProperties.PointPlacementMethod == esriOverposterPointPlacementMethod.esriRotationField)
                        {
                            optRotationField.Checked = true;        //指向某字段确定的特定方向
                            PntAngleChoiseEnbal(false, true);
                        }
                        else if (m_pBasicOverposterLayerProperties.PointPlacementMethod == esriOverposterPointPlacementMethod.esriAroundPoint)
                        {
                            PntAngleChoiseEnbal(false, false);
                            optAroundPoint.Checked = true;          //水平方向
                        }
                        else if (m_pBasicOverposterLayerProperties.PointPlacementMethod == esriOverposterPointPlacementMethod.esriOnTopPoint)
                        {
                            PntAngleChoiseEnbal(false, false);
                            optOnTopPoint.Checked = true;           //位于点的正上方(覆盖点)
                        }
                        else if (m_pBasicOverposterLayerProperties.PointPlacementMethod == esriOverposterPointPlacementMethod.esriSpecifiedAngles)
                        {
                            optSpecifiedAngles.Checked = true;      //指向特定的方向
                            PntAngleChoiseEnbal(true, false);
                        }
                        else
                        {
                            optAroundPoint.Checked = true;
                            PntAngleChoiseEnbal(false, false);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //g_ErrorHandler.DisplayInformation(ex.Message, false, "确定");
            }
        }

        private void InitialGroupBox(string vGroupBoxName)
        {
            int i;
            this.Width = 390;
            this.Height = 590;
            for (i = 0; i <= strGroupBoxes.Length - 1; i++)
            {
                if (strGroupBoxes[i] == vGroupBoxName)
                {
                    this.Controls[strGroupBoxes[i]].Visible = true;
                    this.Controls[vGroupBoxName].Left = this.grpPointSet.Left;
                    this.Controls[vGroupBoxName].Top = this.grpPointSet.Top;
                }
                else
                {
                    this.Controls[strGroupBoxes[i]].Visible = false;
                }
            }
        }

        //线设置初始化
        private void InitialLinePosition()
        {
            grpLineSet.Visible = true;

            ILineLabelPosition pLinePosition;
            pLinePosition = m_pBasicOverposterLayerProperties.LineLabelPosition;

            //参照系统的初始化
            cboOrentationSystem.Items.Add("页面");
            cboOrentationSystem.Items.Add("线");

            //线穿过标注的值的初始化
            if (pLinePosition.OnTop == true)
            {
                chkTop.Checked = true;          //穿过注记
            }
            else
            {
                chkTop.Checked = false;
            }

            //上下左右位置的处理
            if ((pLinePosition.Above == true) || (pLinePosition.Left == true))
            {
                chkAbove.Checked = true;
            }
            else
            {
                chkAbove.Checked = false;
            }

            if ((pLinePosition.Below == true) || (pLinePosition.Right == true))
            {
                chkBottom.Checked = true;
            }
            else
            {
                chkBottom.Checked = false;
            }

            if ((pLinePosition.Above == true) || (pLinePosition.Below == true))
            {
                cboOrentationSystem.Text = "页面";
            }
            else if ((pLinePosition.Left == true) || (pLinePosition.Right == true))
            {
                cboOrentationSystem.Text = "线";
            }

            //偏移量初始化
            txtOffSet.Text = m_pBasicOverposterLayerProperties.LineOffset.ToString();

            //沿线位置的初始化
            cboAlongPosition.Items.Add("起点");
            cboAlongPosition.Items.Add("终点");
            cboAlongPosition.Items.Add("最佳位置");

            if (pLinePosition.AtEnd == true)
            {
                cboAlongPosition.Text = "终点";
            }
            else if (pLinePosition.AtStart == true)
            {
                cboAlongPosition.Text = "起点";
            }
            else
            {
                cboAlongPosition.Text = "最佳位置";
            }
            grpTOP.Enabled = true;
            grpAlong.Enabled = true;

            //水平方向和沿线方向的初始化
            if (pLinePosition.Horizontal == true)
            {
                optHorizontal.Checked = true;       //水平方向
                SetFrameTOPEnableValue(false);      //设置上下左右框架上所有控件的Enbale值
                SetFrameAlongEnable(false);         //设置沿线段位置框架上所有控件的Enbale
            }
            else if (pLinePosition.ProduceCurvedLabels == true)
            {
                optCurved.Checked = true;           //沿线方向
                SetFrameTOPEnableValue(false);
                SetFrameAlongEnable(true);
            }
            else if (pLinePosition.Parallel == true)
            {
                optParalel.Checked = true;          //平行方向
            }
            else if (pLinePosition.Perpendicular == true)
            {
                optPerpendicurlar.Checked = true;   //正交方向
            }

        }

        //设置上下左右框架上所有控件的Enbale值
        private void SetFrameTOPEnableValue(bool bValue)
        {
            grpTOP.Enabled = bValue;
            chkAbove.Enabled = bValue;
            chkTop.Enabled = bValue;
            chkBottom.Enabled = bValue;
            cboOrentationSystem.Enabled = bValue;
            txtOffSet.Enabled = bValue;
        }

        //设置沿线段位置框架上所有控件的Enbale
        private void SetFrameAlongEnable(bool bValue)
        {
            grpAlong.Enabled = bValue;
            cboAlongPosition.Enabled = bValue;
        }

        //水平方向
        private void optAroundPoint_CheckedChanged(object sender, EventArgs e)
        {
            PntAngleChoiseEnbal(false, false);
        }

        private void PntAngleChoiseEnbal(bool bAngle, bool bChoise)
        {
            btnAddAngle.Enabled = bAngle;
            btnChooseField.Enabled = bChoise;
        }

        //指向特定的方向
        private void optSpecifiedAngles_CheckedChanged(object sender, EventArgs e)
        {
            PntAngleChoiseEnbal(optSpecifiedAngles.Checked, false);
        }

        //仅位于点的正上方(覆盖点)
        private void optOnTopPoint_CheckedChanged(object sender, EventArgs e)
        {
            PntAngleChoiseEnbal(false, false);
        }

        //指向某字段确定的特定方向
        private void optRotationField_CheckedChanged(object sender, EventArgs e)
        {
            PntAngleChoiseEnbal(false, optRotationField.Checked);
        }

        //线设置 水平方向
        private void optHorizontal_CheckedChanged(object sender, EventArgs e)
        {
            SetFrameTOPEnableValue(false);
            SetFrameAlongEnable(false);
        }

        //线设置 与线平行
        private void optParalel_CheckedChanged(object sender, EventArgs e)
        {
            SetFrameTOPEnableValue(true);
            SetFrameAlongEnable(true);
        }

        //线设置 沿线方向
        private void optCurved_CheckedChanged(object sender, EventArgs e)
        {
            SetFrameTOPEnableValue(true);
            SetFrameAlongEnable(false);
        }

        //线设置 正交方向
        private void optPerpendicurlar_CheckedChanged(object sender, EventArgs e)
        {
            SetFrameTOPEnableValue(true);
            SetFrameAlongEnable(true);
        }

        //上      上、下(左、右)的位置
        private void chkAbove_CheckedChanged(object sender, EventArgs e)
        {
            setValue();
        }

        //下      上、下(左、右)的位置
        private void chkBottom_CheckedChanged(object sender, EventArgs e)
        {
            setValue();
        }

        //参照物
        private void cboOrentationSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //参考物改变时,相对位置的名称也相应的改变
            if (cboOrentationSystem.Text == "页面")
            {
                chkAbove.Text = "上";
                chkBottom.Text = "下";
                setLineImageVisible(true, false);
            }
            else if (cboOrentationSystem.Text == "线")
            {
                chkAbove.Text = "左";
                chkBottom.Text = "右";
                setLineImageVisible(false, true);
            }
        }

        private void setLineImageVisible(bool bPage, bool bLine)
        {
            //一下为VB中代码

            //ImgLine.Visible = bLine
            //ImgPage.Visible = bPage

            //If bLine = True Then
            //    ImgLine.ZOrder(0)
            //    ImgPage.ZOrder(1)
            //End If

            //If bPage = True Then
            //    ImgPage.ZOrder(0)
            //    ImgLine.ZOrder(1)
            //End If
        }

        //设置参照物和偏距的Enable值
        private void setValue()
        {
            if ((chkAbove.Checked == true) || (chkBottom.Checked == true))
            {
                cboOrentationSystem.Enabled = true;
                txtOffSet.Enabled = true;
            }
            else
            {
                cboOrentationSystem.Enabled = false;
                txtOffSet.Enabled = false;
            }
        }

        //偏移量
        private void txtOffSet_TextChanged(object sender, EventArgs e)
        {
            if ((txtOffSet.Text + "").Trim() == "")
            {
                txtOffSet.Text = "0";
            }
        }

        //设置点的旋转类型为依某个具体的字段   选择字段
        private void btnChooseField_Click(object sender, EventArgs e)
        {
            try
            {
                FrmLabelPntChoiseField frmLabelPntChoiseField = new FrmLabelPntChoiseField();
                m_pBasicOverposterLayerPropertiesOut.PointPlacementMethod = esriOverposterPointPlacementMethod.esriRotationField;
                frmLabelPntChoiseField.BasicOverposterLayerProperties = m_pBasicOverposterLayerPropertiesOut;
                frmLabelPntChoiseField.GeoFeatureLayer = m_pGeoFeatLayer;

                frmLabelPntChoiseField.ShowDialog();
            }
            catch (Exception)
            {
                //m_pDisplayInformation.DisplayInformation("在显示选择字段窗体时出了错", false, , "退出");
            }
        }

        //添加角度   设置旋转依靠的字段
        private void btnAddAngle_Click(object sender, EventArgs e)
        {
            //设置点的旋转类型为依某个具体的值
            try
            {
                FrmLabelPntAddAngleValue frmLabelPntAddAngleValue = new FrmLabelPntAddAngleValue();
                double[] dArrPointPlacementAngles = (double[])m_pBasicOverposterLayerPropertiesOut.PointPlacementAngles;
                double[] aa = new double[] { 0, 30, 45, 60, 90, 120, 135, 150 };

                if (dArrPointPlacementAngles.GetLength(0) == 0)               //不包含角度值
                {
                    frmLabelPntAddAngleValue.AngelValueArray = aa;
                }
                else
                {
                    frmLabelPntAddAngleValue.AngelValueArray = (double[])m_pBasicOverposterLayerPropertiesOut.PointPlacementAngles;
                }
                frmLabelPntAddAngleValue.ShowDialog();
                m_pBasicOverposterLayerPropertiesOut.PointPlacementAngles = frmLabelPntAddAngleValue.AngelValueArray;

            }
            catch (Exception)
            {
                //m_pDisplayInformation.DisplayInformation("在显示设置旋转角度窗体时出了错", false, , "退出");
            }
        }

        //确定
        private void btnOk_Click(object sender, EventArgs e)
        {
            ILineLabelPosition pLinePosition;

            try
            {
                //对线图层的处理
                if (m_pBasicOverposterLayerProperties.FeatureType == esriBasicOverposterFeatureType.esriOverposterPolyline)
                {
                    if ((chkAbove.Checked == false) && (chkBottom.Checked == false) && (chkTop.Checked == false))
                    {
                        //m_pDisplayInformation.DisplayInformation("请在上、下（左、右）位置属性中选一个复选框!", false, , "退出");
                        return;
                    }
                    pLinePosition = new LineLabelPositionClass();
                    //得到新的线标属性的设置
                    setLinePosition(pLinePosition);
                    //应用设置
                    m_pBasicOverposterLayerPropertiesOut.LineLabelPosition = pLinePosition;
                }

                //对面图层的处理   
                else if (m_pBasicOverposterLayerProperties.FeatureType == esriBasicOverposterFeatureType.esriOverposterPolygon)
                {
                    //设置标注是否一定要仅次于多边形内
                    if (chkLabelInRegion.Checked == true)
                    {
                        m_pBasicOverposterLayerPropertiesOut.PlaceOnlyInsidePolygon = true;
                    }
                    else
                    {
                        m_pBasicOverposterLayerPropertiesOut.PlaceOnlyInsidePolygon = false;
                    }

                    //设置标注的方向
                    if (optHorizatalOrentation.Checked == true)
                    {
                        m_pBasicOverposterLayerPropertiesOut.PolygonPlacementMethod = esriOverposterPolygonPlacementMethod.esriAlwaysHorizontal;
                    }
                    else if (optRegionOrentation.Checked == true)
                    {
                        m_pBasicOverposterLayerPropertiesOut.PolygonPlacementMethod = esriOverposterPolygonPlacementMethod.esriAlwaysStraight;
                    }
                    else
                    {
                        m_pBasicOverposterLayerPropertiesOut.PolygonPlacementMethod = esriOverposterPolygonPlacementMethod.esriMixedStrategy;
                    }
                }

                //对点图层的处理
                else if (m_pBasicOverposterLayerProperties.FeatureType == esriBasicOverposterFeatureType.esriOverposterPoint)
                {
                    if (optRotationField.Checked == true)
                    {
                        m_pBasicOverposterLayerPropertiesOut.PointPlacementMethod = esriOverposterPointPlacementMethod.esriRotationField;
                        m_pBasicOverposterLayerPropertiesOut.PointPlacementOnTop = false;
                    }
                    else if (optAroundPoint.Checked == true)
                    {
                        m_pBasicOverposterLayerPropertiesOut.PointPlacementMethod = esriOverposterPointPlacementMethod.esriAroundPoint;
                        m_pBasicOverposterLayerPropertiesOut.PointPlacementOnTop = false;
                    }
                    else if (optOnTopPoint.Checked == true)
                    {
                        m_pBasicOverposterLayerPropertiesOut.PointPlacementMethod = esriOverposterPointPlacementMethod.esriOnTopPoint;
                        m_pBasicOverposterLayerPropertiesOut.PointPlacementOnTop = true;
                    }
                    else if (optSpecifiedAngles.Checked == true)
                    {
                        m_pBasicOverposterLayerPropertiesOut.PointPlacementMethod = esriOverposterPointPlacementMethod.esriSpecifiedAngles;
                        m_pBasicOverposterLayerPropertiesOut.PointPlacementOnTop = false;
                    }
                }

                //对所有的标注层的重新设置(重复标注的处理)
                if (optRemoveDoubleLable.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerName;
                }
                else if (optPerFeature.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerPart;
                }
                else if (optPerPart.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.NumLabelsOption = esriBasicNumLabelsOption.esriOneLabelPerShape;
                }

                //对要素重要性进行设置
                if (OptionF1.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.FeatureWeight = esriBasicOverposterWeight.esriHighWeight;
                }
                else if (OptionF2.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.FeatureWeight = esriBasicOverposterWeight.esriMediumWeight;
                }
                else if (OptionF3.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.FeatureWeight = esriBasicOverposterWeight.esriLowWeight;
                }
                else if (OptionF0.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.FeatureWeight = esriBasicOverposterWeight.esriNoWeight;
                }

                //对标注重要性进行设置
                if (OptionL1.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.LabelWeight = esriBasicOverposterWeight.esriHighWeight;
                }
                else if (OptionL2.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.LabelWeight = esriBasicOverposterWeight.esriMediumWeight;
                }
                else if (OptionL3.Checked == true)
                {
                    m_pBasicOverposterLayerPropertiesOut.LabelWeight = esriBasicOverposterWeight.esriLowWeight;
                }

                this.Close();

            }
            catch (Exception)
            {
                //m_pDisplayInformation.DisplayInformation("在处理标注位置时出了错!", false, , "退出");
                this.Close();
            }
        }


        //** 功能描述 :  获得对线图层的标注设置信息
        private void setLinePosition(ILineLabelPosition pLinePosition)
        {
            //标的方向的处理
            pLinePosition.Horizontal = optHorizontal.Checked;
            pLinePosition.ProduceCurvedLabels = optCurved.Checked;
            pLinePosition.Perpendicular = optPerpendicurlar.Checked;
            pLinePosition.Parallel = optParalel.Checked;

            //不同参照物下上下左右位置的不同组合,(上下,左右是互斥的,不是即有下上,又包含有左右)
            //除不是平行方向的处理
            if (optHorizontal.Checked == false)
            {
                //只有复选框上下(左右)有一个选中时才进行上下(左右)的设置
                if ((chkAbove.Checked == true) || (chkBottom.Checked == true))
                {
                    if (cboOrentationSystem.Text == "页面")
                    {
                        //如果是相对于"页面"的上下操作时,则左右的值都应该为false
                        pLinePosition.Right = false;
                        pLinePosition.Left = false;

                        if (chkAbove.Checked == true)
                        {
                            pLinePosition.Above = true;
                        }
                        else
                        {
                            pLinePosition.Above = false;
                        }

                        if (chkBottom.Checked == true)
                        {
                            pLinePosition.Below = true;
                        }
                        else
                        {
                            pLinePosition.Below = false;
                        }

                        //偏移量的处理
                        m_pBasicOverposterLayerPropertiesOut.LineOffset = Conversion.Val(txtOffSet.Text.Trim());
                    }
                    else if (cboOrentationSystem.Text == "线")
                    {
                        //如果是相对于"线"的左右操作时,则上下的值都应该为false
                        pLinePosition.Above = false;
                        pLinePosition.Below = false;

                        if (chkAbove.Checked == true)
                        {
                            pLinePosition.Left = true;
                        }
                        else
                        {
                            pLinePosition.Left = false;
                        }

                        if (chkBottom.Checked == true)
                        {
                            pLinePosition.Right = true;
                        }
                        else
                        {
                            pLinePosition.Right = false;
                        }
                    }
                }

                //穿过标注的设置
                if (chkTop.Checked == true)
                {
                    pLinePosition.OnTop = true;
                }
                else
                {
                    pLinePosition.OnTop = false;
                }
            }

            //平行方向的处理
            else if (optHorizontal.Checked == true)
            {
                pLinePosition.OnTop = true;
            }

            //沿线位置的处理
            //水平方向和沿线方向的处理
            if ((optHorizontal.Checked == true) || (optCurved.Checked == true))
            {
                pLinePosition.InLine = true;
            }
            else
            {
                if (cboAlongPosition.Text == "起点")
                {
                    pLinePosition.AtStart = true;
                }
                else if (cboAlongPosition.Text == "终点")
                {
                    pLinePosition.AtEnd = true;
                }
                else
                {
                    pLinePosition.InLine = true;
                }
            }
        }

        private void chkTop_CheckedChanged(object sender, EventArgs e)
        {
            setValue();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
