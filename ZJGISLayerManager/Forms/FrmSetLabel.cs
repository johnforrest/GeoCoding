//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：FrmSetLabel
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：在该窗体中设置标注的相关属性
//    创建日期：
//    修改人：
//    修改说明：
//    修改日期：
//-------------------------------------------------------------------------------------
//</CSCC>


//本模块中在System.Drawing.Font()函数中的某一参数为OR或AND时只选取了其中一个值

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
using ESRI.ArcGIS.Geometry;

using stdole;
using System.Xml;
using ESRI.ArcGIS.Display;
using Microsoft.VisualBasic;
//using ZJGISDialog;
using ZJGISLayerManager;
using ESRI.ArcGIS.Controls;

namespace ZJGISLayerManager
{
    public partial class FrmSetLabel : DevComponents.DotNetBar.Office2007Form
    {
        private IMapControl4 m_pMapControl;

        private IGeoFeatureLayer m_pGeoFeatLayer;
        private ISymbol m_pSymbol;
        private ITextSymbol m_pTextSymbol;
        private IAnnotateLayerProperties m_pAnnoLayerProperties;
        private IBasicOverposterLayerProperties4 m_pNewBasicOverposterLayerProperties;      //记录位置相关属性
        private ILabelEngineLayerProperties m_pNewLabelEngineLayerProperties;               //记录表达式
        //private ClsErrorHandle m_pDisplayInformation;
        private bool m_bOK;                                                                 //用于取消按钮
        private XmlElement m_XMLElement;                                                    //保存标注的各种属性信息

        public IGeoFeatureLayer GeoFeatLayer
        {
            set
            {
                m_pGeoFeatLayer = value;
            }
        }

        //记录是否按下了OK键
        public bool bOK
        {
            get
            {
                return m_bOK;
            }
        }

        //记录当前设置的各种标注信息
        public XmlElement XMLElenment
        {
            get
            {
                return m_XMLElement;
            }
        }
        
        public FrmSetLabel(IMapControl4 pMapControl)
        {
            m_pMapControl = pMapControl;
            InitializeComponent();
        }

        //初始化
        private void FrmSetLabel_Load(object sender, EventArgs e)
        {
            IFields pFields;
            IFeatureClass pFeatCls;
            int i;
            bool bIsComplexExpression;          //标识是否是复杂表达式

            //清空列表
            cmbFieldName.Items.Clear();
            cmbFontName.Items.Clear();
            cmbFontSize.Items.Clear();
            
            //初始化字体
            //FontFamily FontF;
            foreach (FontFamily FontF in FontFamily.Families)
            {
                cmbFontName.Items.Add(FontF.Name);
            }

            //初始化字体的大小
            int k;
            for (k = 5; k <= 11; k++)
            {
                cmbFontSize.Items.Add(k);
            }
            for (k = 12; k <= 72; k+=2)
            {
                cmbFontSize.Items.Add(k);
            }

            //初始化点击了OK键为false
            m_bOK = false;

            //初始化传出参数
            m_XMLElement = null;

            //获得当前图层的标注的属性
            ILabelEngineLayerProperties pLabelEngineLayerProper;
            IAnnotateLayerPropertiesCollection pAnnoLayerProperCollection;
            pAnnoLayerProperCollection = m_pGeoFeatLayer.AnnotationProperties;

            //得到当前图层的当前标注属性
            IElementCollection pElementCollection1, pElementCollection2;

            pAnnoLayerProperCollection.QueryItem(0, out m_pAnnoLayerProperties, out pElementCollection1, out pElementCollection2);
            pLabelEngineLayerProper = (ILabelEngineLayerProperties)m_pAnnoLayerProperties;
            m_pSymbol = (ISymbol)pLabelEngineLayerProper.Symbol;
            m_pTextSymbol = (ITextSymbol)m_pSymbol;

            //初始化该图层的字段，并初始化为该层的标注字段
            ILabelEngineLayerProperties pLableEngine;
            IField pField;
            IField pDisplayField = null;
            string sDisplayFieldName;
            int iStrLeng, iFieldIndex;

            pLableEngine = (ILabelEngineLayerProperties)m_pAnnoLayerProperties;
            //判断是否是复杂表达式
            sDisplayFieldName = pLableEngine.Expression.Trim();
            bIsComplexExpression = JudgeIsComplexExpression(sDisplayFieldName);

            //if (bIsComplexExpression==false)
            //{
            //    //获得表达式的长度
            //    iStrLeng = pLableEngine.Expression.Length;
            //    //去除[]符号
            //    sDisplayFieldName = Strings.Left(Strings.Right(pLableEngine.Expression, iStrLeng - 1), iStrLeng - 2);
            //    sDisplayFieldName = sDisplayFieldName.ToUpper();
            //}

            //如果图层不为空
            if (m_pGeoFeatLayer!=null)
            {
                pFeatCls = m_pGeoFeatLayer.FeatureClass;
                pFields = pFeatCls.Fields;
                //IDataset pDs = pFeatCls;
                for (i = 0; i <= pFields.FieldCount-1; i++)
                {
                    pField = pFields.get_Field(i);
                    if ((pField.Type!=esriFieldType.esriFieldTypeGeometry)&&pField.Type!=esriFieldType.esriFieldTypeBlob)
                    {
                        cmbFieldName.Items.Add(pField.Name + "[" + pField.AliasName + "]");
                    }
                }

                //设置当前层的标注字段
                if (bIsComplexExpression==false)
                {
                    //获得表达式的长度
                    iStrLeng = Strings.Len(pLableEngine.Expression);
                    //去除[]符号
                    sDisplayFieldName = Strings.Left(Strings.Right(pLableEngine.Expression, iStrLeng - 1), iStrLeng - 2);
                    iFieldIndex = pFields.FindField(sDisplayFieldName);

                    if (iFieldIndex>-1)
                    {
                        pDisplayField = pFields.get_Field(iFieldIndex);
                    }

                    cmbFieldName.Text = pDisplayField.Name + "[" + pDisplayField.AliasName + "]";

                }
                else
                {
                    cmbFieldName.Items.Add("<复杂表达式>");
                    cmbFieldName.Text = "复杂表达式";
                    m_pNewLabelEngineLayerProperties = (ILabelEngineLayerProperties)pLabelEngineLayerProper;
                    cmbFieldName.Enabled = false;
                }
                
            }
            else
            {
                cmbFieldName.Enabled = false;
            }

            //初始化颜色
            IFormattedTextSymbol pFormattedTextSymbol;
            pFormattedTextSymbol = (IFormattedTextSymbol)m_pTextSymbol;
            //设置初始颜色盒控件的颜色
            this.btnColorPick.SelectedColor = GetSystemColor(pFormattedTextSymbol.Color);
            //当前图层的当前标注的字体
            cmbFontName.Text = m_pTextSymbol.Font.Name;
            //当前图层的当前标注字体的大小
            cmbFontSize.Text = m_pTextSymbol.Font.Size.ToString();
            //初始化当前字体的样式
            btnFontBold.Checked = m_pTextSymbol.Font.Bold;
            btnFontUnderline.Checked = m_pTextSymbol.Font.Underline;
            btnFontItalic.Checked = m_pTextSymbol.Font.Italic;

            if (m_pTextSymbol.Font.Italic)
            {
                if (m_pTextSymbol.Font.Bold)
                {
                    if (m_pTextSymbol.Font.Underline)
                    {
                        //lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name, m_pTextSymbol.Font.Size, FontStyle.Bold || FontStyle.Italic || FontStyle.Underline);
                        lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name,(float)m_pTextSymbol.Font.Size, FontStyle.Bold);
                    }
                    else
                    {
                        //lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name, m_pTextSymbol.Font.Size, FontStyle.Bold || FontStyle.Italic);
                        lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name, (float)m_pTextSymbol.Font.Size, FontStyle.Italic);
                    }
                }
                else
                {
                    if (m_pTextSymbol.Font.Underline)
                    {
                        lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name, (float)m_pTextSymbol.Font.Size, FontStyle.Underline);
                    }
                    else
                    {
                        lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name, (float)m_pTextSymbol.Font.Size, FontStyle.Italic);
                    }
                }
            }
            else
            {
                if (m_pTextSymbol.Font.Bold)
                {
                    if (m_pTextSymbol.Font.Underline)
                    {
                        lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name, (float)m_pTextSymbol.Font.Size, FontStyle.Underline);
                    }
                    else
                    {
                        lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name, (float)m_pTextSymbol.Font.Size, FontStyle.Bold);
                    }
                }
                else
                {
                    if (m_pTextSymbol.Font.Underline)
                    {
                        lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name, (float)m_pTextSymbol.Font.Size, FontStyle.Underline);
                    }
                    else
                    {
                        lblText.Font = new System.Drawing.Font(m_pTextSymbol.Font.Name, (float)m_pTextSymbol.Font.Size, FontStyle.Regular);
                    }
                }
            }

            lblText.ForeColor = GetSystemColor(m_pTextSymbol.Color);

            //初始化标注显示的比例范围
            if ((m_pAnnoLayerProperties.AnnotationMaximumScale!=m_pGeoFeatLayer.MaximumScale)||(m_pAnnoLayerProperties.AnnotationMinimumScale!=m_pGeoFeatLayer.MinimumScale))
            {
                optUserDefined.CheckValue = "Y";
                txtMinScale.Text = m_pAnnoLayerProperties.AnnotationMinimumScale.ToString();
                txtMaxScale.Text = m_pAnnoLayerProperties.AnnotationMaximumScale.ToString();
            }
            else
            {
                optScaleNameWithLayer.CheckValue = "Y";
            }

            //如果显示最大最小比例尺都是0的话，则雨当前地图的参考比例尺相同
            if ((m_pAnnoLayerProperties.AnnotationMaximumScale==0)&&(m_pAnnoLayerProperties.AnnotationMinimumScale==0))
            {
                optScaleNameWithLayer.CheckValue = "Y";
            }

        }

        //判断表达式是否为复杂表达式
        private bool JudgeIsComplexExpression(string sExpression)
        {

            int i;
            string sValue;
            bool bIsComplexExpression = false;

            sValue = sExpression.Trim();
            
            //如果含有下面的字符串时都认为是复杂表达式
            if (Strings.InStr(sValue, "&", CompareMethod.Text) > 0 && Strings.InStr(sValue, "+", CompareMethod.Text) > 0
                && Strings.InStr(sValue, "And", CompareMethod.Text) > 0 && Strings.InStr(sValue, "Or", CompareMethod.Text) > 0 
                && Strings.InStr(sValue, "=", CompareMethod.Text) > 0 && Strings.InStr(sValue, ">", CompareMethod.Text) > 0 
                && Strings.InStr(sValue, "<", CompareMethod.Text) > 0)
            {
                bIsComplexExpression = true;
            }
            else
            {
                i = Strings.InStr(sValue, "[",CompareMethod.Text);
                if (i > 1)                            //如果不位于第一个的话,说明表达式中包涵有函数
                {
                    bIsComplexExpression = true;
                }
                else                                  //如果位于第一位,剩余的字符串中还有[时,说明是复杂表达式
                {
                    sValue = Strings.Mid(sValue, i + 1, Strings.Len(sValue) - i);
                    i = Strings.InStr(sValue, "[", CompareMethod.Text);

                    if (i > 0)                        //剩余的字符串中还有[时
                    {
                        bIsComplexExpression = true;
                    }
                }
            }
            return bIsComplexExpression;
        }

        //转换IColor到Color
        private Color GetSystemColor(IColor pColor)
        {
            RgbColor pRgbColor = new RgbColor();
            pRgbColor.RGB = pColor.RGB;

            Color pSysColor;
            pSysColor = Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);

            return pSysColor;
        }

        //改变文字颜色
        private void btnColorPick_SelectedColorChanged(object sender, EventArgs e)
        {
            lblText.ForeColor = btnColorPick.SelectedColor;
        }

        private void cmbFieldName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFieldName.Text.Trim()=="")
            {
                cmbFieldName.Text = m_pGeoFeatLayer.DisplayField;
            }
        }

        private void cmbFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFontName.Text.Trim()=="")
            {
                cmbFontName.Text = m_pTextSymbol.Font.Name;
            }
        }

        private void cmbFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFontSize.Text.Trim()=="")
            {
                cmbFontSize.Text = "8";
            }
            if (Conversion.Val(cmbFontSize.Text.Trim())>2001)
            {
                cmbFontSize.Text = "2000";
            }
        }

        private void cmbFieldName_Click(object sender, EventArgs e)
        {
            m_pGeoFeatLayer.DisplayField = cmbFieldName.Text.Trim();
        }

        private void cmbFieldName_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((cmbFieldName.Text+"").Trim()=="")
            {
                cmbFieldName.Text = m_pGeoFeatLayer.DisplayField;
            }
        }

        //改变字体
        private void cmbFontName_Click(object sender, EventArgs e)
        {
            lblText.Font = new System.Drawing.Font(cmbFontName.Text.Trim(), lblText.Font.Size);
        }

        //改变字体大小
        private void cmbFontSize_Click(object sender, EventArgs e)
        {
            lblText.Font=new System.Drawing.Font(lblText.Font.Name,Convert.ToSingle(cmbFontSize.Text));
        }

        private void cmbFontName_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((cmbFontName.Text+"").Trim()=="")
            {
                cmbFontName.Text = m_pTextSymbol.Font.Name;
            }
            lblText.Font = new System.Drawing.Font(cmbFontName.Text.Trim(), lblText.Font.Size);
        }

        private void cmbFontSize_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((cmbFontSize.Text+"").Trim()=="")
            {
                cmbFontSize.Text = "8";
            }
            if (Conversion.Val(Convert.ToUInt32(cmbFontSize.Text.Trim()))>2001)
            {
                cmbFontSize.Text = "2000";
            }
            lblText.Font = new System.Drawing.Font(lblText.Font.Name, Convert.ToSingle(cmbFontSize.Text));
        }

        //设置是否为粗体
        private void btnFontBold_Click(object sender, EventArgs e)
        {
            if (lblText.Font.Bold)              //取消字体加粗
            {
                btnFontBold.Checked = false;
                lblText.Font = new System.Drawing.Font(lblText.Font.Name, lblText.Font.Size, lblText.Font.Style);
            }
            else                               //字体加粗
            {
                btnFontBold.Checked = true;
                lblText.Font = new System.Drawing.Font(lblText.Font.Name, lblText.Font.Size, FontStyle.Bold);
            }
        }

        //设置是否为斜体
        private void btnFontItalic_Click(object sender, EventArgs e)
        {
            if (lblText.Font.Italic)            //取消字体倾斜
            {
                this.btnFontItalic.Checked = false;
                lblText.Font = new System.Drawing.Font(lblText.Font.Name, lblText.Font.Size, lblText.Font.Style);
            }
            else                                //字体倾斜
            {
                this.btnFontItalic.Checked = true;
                lblText.Font = new System.Drawing.Font(lblText.Font.Name, lblText.Font.Size, FontStyle.Italic);
            }
        }

        //设置文本是否含有下划线
        private void btnFontUnderline_Click(object sender, EventArgs e)
        {
            if (lblText.Font.Underline)         //取消字体下划线
            {
                this.btnFontUnderline.Checked = false;
                lblText.Font = new System.Drawing.Font(lblText.Font.Name, lblText.Font.Size, lblText.Font.Style);
            }
            else                               //字体下划线
            {
                this.btnFontUnderline.Checked = true;
                lblText.Font = new System.Drawing.Font(lblText.Font.Name, lblText.Font.Size, FontStyle.Underline);
            }
        }

        private void txtMinScale_TextChanged(object sender, EventArgs e)
        {
            if ((txtMinScale.Text+"").Trim()=="")
            {
                txtMinScale.Text = "0";
            }
        }

        private void txtMaxScale_TextChanged(object sender, EventArgs e)
        {
            if ((txtMaxScale.Text+"").Trim()=="")
            {
                txtMaxScale.Text = "0";
            }
        }

        //自定义显示注记的参考比例
        private void optUserDefined_CheckedChanged(object sender, EventArgs e)
        {
            txtMinScale.Enabled = true;
            txtMaxScale.Enabled = true;

            if ((txtMaxScale.Text+"").Trim()=="")
            {
                txtMaxScale.Text = "0";
            }
            if ((txtMinScale.Text+"").Trim()=="")
            {
                txtMinScale.Text = "0";
            }
        }

        //与当前图层的参考比例尺相同
        private void optScaleNameWithLayer_CheckedChanged(object sender, EventArgs e)
        {
            if (optScaleNameWithLayer.Checked)
            {
                txtMinScale.Enabled = false;
                txtMaxScale.Enabled = false;
            }
        }

        //设置属性相关
        private void btnPosition_Click(object sender, EventArgs e)
        {
            try
            {
                if ((m_pAnnoLayerProperties!=null)&&(m_pGeoFeatLayer!=null))
                {
                    FrmLabelPosition FrmLabelPosition = new FrmLabelPosition();
                    FrmLabelPosition.LabelEngineLayerProperties = (ILabelEngineLayerProperties)m_pAnnoLayerProperties;
                    FrmLabelPosition.GeoFeatureLayer = m_pGeoFeatLayer;
                    FrmLabelPosition.ShowDialog();

                    m_pNewBasicOverposterLayerProperties = FrmLabelPosition.BasicOverposterLayerProperties;
                }
            }
            catch (Exception)
            {
                //m_pDisplayInformation.DisplayInformation("在显示设置标注位置的窗体时候错误发生!", false, null, "退出");
                MessageBox.Show("在显示设置标注位置的窗体时候错误发生!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        //表达式
        private void btnExpresion_Click(object sender, EventArgs e)
        {
            bool bIsComplexExpression = false;              //标识是否是复杂表达式
            bool bExistComplexExpression=false;            //标识,"<复杂表达式>"是否在下拉框中存在
            int iComplexExpressionIndex=-1;                    //记录<复杂表达式>在下拉框中的索引
            int i, j;
            string sValue;
            IField pField;
            IFields pFields;

            FrmLabelExpression FrmLabelExpression = new FrmLabelExpression();
            FrmLabelExpression.GeoFeatureLayer = m_pGeoFeatLayer;
            FrmLabelExpression.LabelEngineLayerProperties = (ILabelEngineLayerProperties)m_pAnnoLayerProperties;
            FrmLabelExpression.ShowDialog();

            m_pNewLabelEngineLayerProperties = FrmLabelExpression.LabelEngineLayerProperties;
            bIsComplexExpression = JudgeIsComplexExpression(m_pNewLabelEngineLayerProperties.Expression);

            //检查<复杂表达式>在下拉框中是否已经存在
            for (i = 0; i <= cmbFieldName.Items.Count-1; i++)
            {
                if (cmbFieldName.Items[i].ToString().Trim()=="<复杂表达式>")
                {
                    bExistComplexExpression = true;
                    iComplexExpressionIndex = i;
                    break;
                }
            }

            //初始化下拉框   如果是复杂表达式
            if (bIsComplexExpression)
            {
                if (bExistComplexExpression==true)
                {
                    cmbFieldName.Text = "<复杂表达式>";
                }
                else
                {
                    cmbFieldName.Items.Add("<复杂表达式>");
                    cmbFieldName.Text = "<复杂表达式>";
                }
                cmbFieldName.Enabled = false;
            }
            else
            {
                //如果<复杂表达式>存在于下拉框中的话,则移除它,注意:在添加前移除
                if (bExistComplexExpression)
                {
                    cmbFieldName.Items.RemoveAt(iComplexExpressionIndex);
                }

                //获得[]之间的字段名称
                sValue = m_pNewLabelEngineLayerProperties.Expression;
                i = Strings.InStr(sValue, "[", CompareMethod.Text);
                j = Strings.InStr(sValue, "]", CompareMethod.Text);
                sValue = Strings.Mid(sValue, i + 1, j - i - 1);
            
                //找到对应的字段名称，并初始化
                pFields = m_pGeoFeatLayer.FeatureClass.Fields;

                for (i = 0; i <= pFields.FieldCount-1; i++)
                {
                    pField = pFields.get_Field(i);
                    if (Strings.UCase(pField.Name)==Strings.UCase(sValue))
                    {
                        cmbFieldName.Text = pField.Name + "[" + pField.AliasName + "]";
                        break;
                    }
                }
                cmbFieldName.Enabled = true;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_bOK = false;
            this.Close();
        }

        //应用所有设置的标注的属性
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbFieldName.Text.Trim()=="")
                {
                    return;
                }

                //设置显示的参考比例尺
                if (optUserDefined.Checked)
                {
                    m_pAnnoLayerProperties.AnnotationMaximumScale = Conversion.Val(txtMaxScale.Text);
                    m_pAnnoLayerProperties.AnnotationMinimumScale = Conversion.Val(txtMinScale.Text);
                }
                else if (optScaleNameWithLayer.Checked)
                {
                    m_pAnnoLayerProperties.AnnotationMaximumScale = m_pGeoFeatLayer.MaximumScale;
                    m_pAnnoLayerProperties.AnnotationMinimumScale = m_pGeoFeatLayer.MinimumScale;
                }

                ILabelEngineLayerProperties pLabelEngine;
                //获得解析表达式的解析器
                IAnnotationExpressionEngine pAnnoVBScriptEngine;
                pAnnoVBScriptEngine = new AnnotationVBScriptEngineClass();
                pLabelEngine = (ILabelEngineLayerProperties)m_pAnnoLayerProperties;
                pLabelEngine.ExpressionParser = pAnnoVBScriptEngine;

                //设置显示的表达式
                string sDisplayFieldName;
                string[] vFieldNameArray;

                if (cmbFieldName.Text.Contains("复杂表达式"))
                {
                    pLabelEngine.Expression = m_pNewLabelEngineLayerProperties.Expression;
                }
                else
                {
                    vFieldNameArray = cmbFieldName.Text.Split(new char[] { '[' });
                    sDisplayFieldName = vFieldNameArray[0];
                    pLabelEngine.Expression = "[" + sDisplayFieldName + "]";
                }

                //设置字体
                IFontDisp pFontDisp;
                pFontDisp = m_pTextSymbol.Font;
                pFontDisp.Name = lblText.Font.Name;
                pFontDisp.Size = (decimal)lblText.Font.Size;
                pFontDisp.Bold = lblText.Font.Bold;
                pFontDisp.Italic = lblText.Font.Italic;
                pFontDisp.Underline = lblText.Font.Underline;

                m_pTextSymbol.Font = pFontDisp;

                //设置颜色
                IRgbColor pRgbColor = new RgbColorClass();
                Color pForeColor;
                pForeColor = lblText.ForeColor;
                pRgbColor.Red = pForeColor.R;
                pRgbColor.Green = pForeColor.G;
                pRgbColor.Blue = pForeColor.B;

                m_pTextSymbol.Color = pRgbColor;

                //设置位置及其它属性
                if (m_pNewBasicOverposterLayerProperties!=null)
                {
                    pLabelEngine.BasicOverposterLayerProperties = (IBasicOverposterLayerProperties)m_pNewBasicOverposterLayerProperties;
                }

                //将属性设置写入XML
                m_XMLElement = WriteLabelInfoToXML(m_pAnnoLayerProperties);

                IActiveView pActiveView;
                //pActiveView = (IActiveView)ModDeclare.g_Sys.MapControl.Map;
                pActiveView = (IActiveView)m_pMapControl.Map;
                pActiveView.Refresh();

                m_bOK = true;
                this.Close();

            }
            catch (Exception)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("在显示设置标注时候发生错误,请检查标注表达式！", false, null, "退出");
                MessageBox.Show("在显示设置标注时候发生错误,请检查标注表达式！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        //把当前层的标注设置保存成xml节点
        private XmlElement WriteLabelInfoToXML(IAnnotateLayerProperties pAnnoLayerProper)
        {
            XmlDocument xml = new XmlDocument();
            XmlElement pElement;

            //标注的属性
            ILabelEngineLayerProperties pLabelEngineLayerProper;
            //标注的位置相关属性
            IBasicOverposterLayerProperties4 pBasicOverposterLayerProper;
            ILineLabelPosition pLinePosition;
            //标注的字体
            ITextSymbol pTextSymbol;
            IFontDisp pFont;
            int i;

            double[] dArray;
            string sAngle = string.Empty;
            pElement = xml.CreateElement("LABEL");

            pLabelEngineLayerProper = (ILabelEngineLayerProperties)pAnnoLayerProper;
            pBasicOverposterLayerProper = (IBasicOverposterLayerProperties4)pLabelEngineLayerProper.BasicOverposterLayerProperties;
            pTextSymbol = pLabelEngineLayerProper.Symbol;
            pFont = pTextSymbol.Font;

            //记录字体
            pElement.SetAttribute(Strings.UCase("FontName"),pFont.Name);
            pElement.SetAttribute(Strings.UCase("FontSize"), pFont.Size.ToString());
            pElement.SetAttribute(Strings.UCase("FontItalic"), pFont.Italic.ToString());
            pElement.SetAttribute(Strings.UCase("FontUnderline"), pFont.Underline.ToString());
            pElement.SetAttribute(Strings.UCase("FontBold"), pFont.Bold.ToString());

            //记录颜色信息
            pElement.SetAttribute(Strings.UCase("Color"), pTextSymbol.Color.RGB.ToString());

            //记录表达式信息
            pElement.SetAttribute(Strings.UCase("Expression"), pLabelEngineLayerProper.Expression.ToString());

            //记录显示比例尺信息
            pElement.SetAttribute(Strings.UCase("MaximumScale"), pAnnoLayerProper.AnnotationMaximumScale.ToString());
            pElement.SetAttribute(Strings.UCase("MinimumScale"), pAnnoLayerProper.AnnotationMinimumScale.ToString());

            //标注与要素类对应关系的信息
            if (pBasicOverposterLayerProper.NumLabelsOption==esriBasicNumLabelsOption.esriOneLabelPerName)
            {
                pElement.SetAttribute(Strings.UCase("NumLabelsOption"), "esriOneLabelPerName");
            }
            else if (pBasicOverposterLayerProper.NumLabelsOption==esriBasicNumLabelsOption.esriOneLabelPerPart)
            {
                pElement.SetAttribute(Strings.UCase("NumLabelsOption"), "esriOneLabelPerPart");
            }
            else
            {
                pElement.SetAttribute(Strings.UCase("NumLabelsOption"), "esriOneLabelPerShape");
            }

            //下面分要素类开始记录不同的信息
            //记录点类型图层的信息
            if (pBasicOverposterLayerProper.FeatureType==esriBasicOverposterFeatureType.esriOverposterPoint)
            {
                pElement.SetAttribute(Strings.UCase("FeatType"), "esriOverposterPoint");

                if (pBasicOverposterLayerProper.PointPlacementMethod==esriOverposterPointPlacementMethod.esriAroundPoint)
                {
                    pElement.SetAttribute(Strings.UCase("PointPlacementMethod"), "esriAroundPoint");
                    pElement.SetAttribute(Strings.UCase("PointPlacementOnTop"), pBasicOverposterLayerProper.PointPlacementOnTop.ToString());
                }
                else if (pBasicOverposterLayerProper.PointPlacementMethod==esriOverposterPointPlacementMethod.esriOnTopPoint)
                {
                    pElement.SetAttribute(Strings.UCase("PointPlacementMethod"), "esriOnTopPoint");
                    pElement.SetAttribute(Strings.UCase("PointPlacementOnTop"), pBasicOverposterLayerProper.PointPlacementOnTop.ToString());
                }
                //某个字段决定旋转角度
                else if (pBasicOverposterLayerProper.PointPlacementMethod==esriOverposterPointPlacementMethod.esriRotationField)
                {
                    pElement.SetAttribute(Strings.UCase("PointPlacementMethod"), "esriRotationField");
                    pElement.SetAttribute(Strings.UCase("RotationField"), pBasicOverposterLayerProper.RotationField);

                    //参照旋转坐标系
                    if (pBasicOverposterLayerProper.RotationType == esriLabelRotationType.esriRotateLabelArithmetic)
                    {
                        pElement.SetAttribute(Strings.UCase("RotationType"), "esriRotateLabelArithmetic");
                    }
                    else if (pBasicOverposterLayerProper.RotationType==esriLabelRotationType.esriRotateLabelGeographic)
                    {
                        pElement.SetAttribute(Strings.UCase("RotationType"), "esriRotateLabelGeographic");
                    }

                    //是否在原旋转基础上加90度
                    pElement.SetAttribute(Strings.UCase("PerpendicularToAngle"), pBasicOverposterLayerProper.PerpendicularToAngle.ToString());
                    pElement.SetAttribute(Strings.UCase("PointPlacementOnTop"), pBasicOverposterLayerProper.PointPlacementOnTop.ToString());
                }

                //特定的角度数组来决定旋转角度
                else if (pBasicOverposterLayerProper.PointPlacementMethod==esriOverposterPointPlacementMethod.esriSpecifiedAngles)
                {
                    pElement.SetAttribute(Strings.UCase("PointPlacementMethod"), "esriSpecifiedAngles");
                    dArray = (double[])pBasicOverposterLayerProper.PointPlacementAngles;

                    //最后一维默认的是零
                    for (i = 0; i <= dArray.Length-1; i++)
                    {
                        if (i==0)
                        {
                            sAngle = dArray[i].ToString();
                        }
                        else
                        {
                            sAngle = sAngle + "" + dArray[i];
                        }
                    }

                    pElement.SetAttribute(Strings.UCase("PointPlacementAngles"), sAngle);
                    pElement.SetAttribute(Strings.UCase("PointPlacementOnTop"), pBasicOverposterLayerProper.PointPlacementOnTop.ToString());
                }

            }

            //记录线类型要素类的标注的信息
            else if (pBasicOverposterLayerProper.FeatureType==esriBasicOverposterFeatureType.esriOverposterPolyline)
            {
                pElement.SetAttribute(Strings.UCase("FeatType"), "esriOverposterPolyline");
                pLinePosition = pBasicOverposterLayerProper.LineLabelPosition;

                //记录位置的所有信息
                if (pLinePosition!=null)
                {
                    pElement.SetAttribute(Strings.UCase("Above"), pLinePosition.Above.ToString());
                    pElement.SetAttribute(Strings.UCase("AtEnd"), pLinePosition.AtEnd.ToString());
                    pElement.SetAttribute(Strings.UCase("AtStart"), pLinePosition.AtStart.ToString());
                    pElement.SetAttribute(Strings.UCase("Below"), pLinePosition.Below.ToString());
                    pElement.SetAttribute(Strings.UCase("Horizontal"), pLinePosition.Horizontal.ToString());
                    pElement.SetAttribute(Strings.UCase("InLine"), pLinePosition.InLine.ToString());
                    pElement.SetAttribute(Strings.UCase("Left"), pLinePosition.Left.ToString());
                    pElement.SetAttribute(Strings.UCase("Offset"), pLinePosition.Offset.ToString());
                    pElement.SetAttribute(Strings.UCase("OnTop"), pLinePosition.OnTop.ToString());
                    pElement.SetAttribute(Strings.UCase("Parallel"), pLinePosition.Parallel.ToString());
                    pElement.SetAttribute(Strings.UCase("Perpendicular"), pLinePosition.Perpendicular.ToString());
                    pElement.SetAttribute(Strings.UCase("ProduceCurvedLabels"), pLinePosition.ProduceCurvedLabels.ToString());
                    pElement.SetAttribute(Strings.UCase("Right"), pLinePosition.Right.ToString());
                }
            }

            else if (pBasicOverposterLayerProper.FeatureType==esriBasicOverposterFeatureType.esriOverposterPolygon)
            {
                pElement.SetAttribute(Strings.UCase("FeatType"), "esriOverposterPolygon");
 
                //方向信息
                if (pBasicOverposterLayerProper.PolygonPlacementMethod == esriOverposterPolygonPlacementMethod.esriAlwaysHorizontal)
                {
                    pElement.SetAttribute(Strings.UCase("PolygonPlacementMethod"), "esriAlwaysHorizontal");
                }
                else if (pBasicOverposterLayerProper.PolygonPlacementMethod == esriOverposterPolygonPlacementMethod.esriAlwaysStraight)
                {
                    pElement.SetAttribute(Strings.UCase("PolygonPlacementMethod"), "esriAlwaysStraight");
                }
                else
                {
                    pElement.SetAttribute(Strings.UCase("PolygonPlacementMethod"), "esriMixedStrategy");
                }

                pElement.SetAttribute(Strings.UCase("PlaceOnlyInsidePolygon"), pBasicOverposterLayerProper.PlaceOnlyInsidePolygon.ToString());
            }
            return pElement;
        }

        private void txtMinScale_Leave(object sender, EventArgs e)
        {
            try
            {
                double pdouble;
                pdouble = double.Parse(txtMinScale.Text);
            }
            catch (Exception)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("请输入半角数字", false, null, null);
                MessageBox.Show("请输入半角数字", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void txtMaxScale_Leave(object sender, EventArgs e)
        {
            try
            {
                double pdouble;
                pdouble = double.Parse(txtMaxScale.Text);
            }
            catch (Exception)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("请输入半角数字", false, null, null);
                MessageBox.Show("请输入半角数字", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

    }
}
