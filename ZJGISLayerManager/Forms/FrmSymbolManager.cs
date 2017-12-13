using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using System.Xml;
using System.IO;
using ZJGISLayerManager;
using ESRI.ArcGIS.Controls;
using Microsoft.VisualBasic;

namespace ZJGISLayerManager
{
    //管理符号库
    public partial class FrmSymbolManager :DevComponents.DotNetBar.Office2007Form
    {
        private ISymbol m_CurSymbol;                //当前符号
        private XmlElement m_SymbolXmlElement;  //当前符号XML
        public string m_SymbolType;                //符号类型

        private IStyleGalleryItem m_SelStyleGalleryItem;    //当前选中的StyleGalleryItem
        private UseType m_UseType;

        public ISymbol m_InSymbol;                  //传入的符号
        public ISymbol m_Symbol;                    //传出的符号
        private int pPtNum;

        public enum UseType
	    {
            General=1,
            Renderer=2
	    }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vType">1.通用 2.符号方案管理</param>
        public FrmSymbolManager(UseType vType)
        {
            //此调用是 Windows 窗体设计器所必需的
            InitializeComponent();

            //在 InitializeComponent() 调用之后添加任何初始化
            m_UseType = vType;
        }

        //当前符号
        public ISymbol CurSymbol
        {
            get
            {
                return m_CurSymbol;
            }
            set
            {
                m_CurSymbol = value;
            }
        }

        public XmlElement SymbolXmlElment
        {
            get
            {
                return m_SymbolXmlElement;
            }
            set
            {
                m_SymbolXmlElement = value;
            }
        }

        //符号类型
        public string SymbolType
        {
            set
            {
                m_SymbolType = value;
            }
        }

        /// <summary>
        /// Form初始化
        /// </summary>
        private void Initial()
        {
            string m_SymbolFileName;

            try
            {
                if (m_SymbolXmlElement==null)
                {
                    return;
                }
                m_SymbolFileName = m_SymbolXmlElement.GetAttribute("文件名");
                if (m_SymbolFileName=="")
                {
                    //ModDeclare.g_ErrorHandler.DisplayInformation("找不到该符号文件！", false, null, null);
                    MessageBox.Show("找不到该符号文件!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return;
                }
                

                switch (m_SymbolType)
                {
                    case "点状地物符号":
                        this.SymbolCtrl1.StyleClass = esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
                        ShowStyleItem(esriSymbologyStyleClass.esriStyleClassMarkerSymbols);
                        InitialGroupBox(esriSymbologyStyleClass.esriStyleClassMarkerSymbols);
                        break;
                    case "线状地物符号":
                        this.SymbolCtrl1.StyleClass = esriSymbologyStyleClass.esriStyleClassLineSymbols;
                        ShowStyleItem(esriSymbologyStyleClass.esriStyleClassLineSymbols);
                        InitialGroupBox(esriSymbologyStyleClass.esriStyleClassLineSymbols);
                        break;
                    case "面状地物符号":
                        this.SymbolCtrl1.StyleClass = esriSymbologyStyleClass.esriStyleClassFillSymbols;
                        ShowStyleItem(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                        InitialGroupBox(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                        break;
                    default:
                        break;
                }
                this.SymbolCtrl1.LoadStyleFile(ClsDeclare.g_StyleFolder + m_SymbolFileName);
                //初始化符号文件ComboBox
                this.cboSymbolFile.Items.Add(m_SymbolFileName);
                this.cboSymbolFile.Text = m_SymbolFileName;
                //初始化符号类型ComboBox
                this.cboSymbolType.Items.Add(m_SymbolType);
                this.cboSymbolType.Text = m_SymbolType;

            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("frmSymboManager的Initial出错:" + ex.Message, false, null, null);
                MessageBox.Show("frmSymboManager的Initial出错:" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 在SymbolControl里显示当前Symbol所在的目录，并预览当前Symbol
        /// </summary>
        /// <param name="vSymbologyStyleClass"></param>
        private void ShowStyleItem(esriSymbologyStyleClass vSymbologyStyleClass)
        {
            IStyleGalleryItem pStyleGalleryItem;
            ISymbologyStyleClass pSymbologyStyleClass;

            try
            {
                //定义新的ServerStyleGalleryItem,将当前符号赋值给它
                pStyleGalleryItem = new ServerStyleGalleryItemClass();
                pStyleGalleryItem.Item = m_CurSymbol;
                pStyleGalleryItem.Name = m_SymbolXmlElement.GetAttribute("符号名");
                m_SelStyleGalleryItem = pStyleGalleryItem;
                //ISymbologyStyleClass,得到SymbolControl的StyleClass
                pSymbologyStyleClass = this.SymbolCtrl1.GetStyleClass(vSymbologyStyleClass);

                pSymbologyStyleClass.AddItem(pStyleGalleryItem, 0);
                pSymbologyStyleClass.SelectItem(0);
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("frmSymboManager的ShowStyleItem出错:" + ex.Message, false, null, null);
                MessageBox.Show("frmSymboManager的ShowStyleItem出错:" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 初始化点、线、面GroupBox中的项
        /// </summary>
        /// <param name="vSymbologyStyleClass"></param>
        private void InitialGroupBox(esriSymbologyStyleClass vSymbologyStyleClass)
        {
            switch (vSymbologyStyleClass)
            {
                case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                    this.gbPoint.Visible = true;
                    this.gbLine.Visible = false;
                    this.gbArea.Visible = false;
                    this.gbPoint.Left = this.gbArea.Left;
                    this.gbPoint.Top = this.gbArea.Top;
                    break;
                case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                    this.gbPoint.Visible = false;
                    this.gbLine.Visible = true;
                    this.gbArea.Visible = false;
                    this.gbLine.Left = this.gbArea.Left;
                    this.gbLine.Top = this.gbArea.Top;
                    break;
                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    this.gbPoint.Visible = false;
                    this.gbLine.Visible = false;
                    this.gbArea.Visible = true;
                    //this.gbArea.Left = this.gbPoint.Left;
                    //this.gbArea.Top = this.gbPoint.Top;
                    break;
                default:
                    break;
            }
        }

        private void FrmSymbolManager_Load(object sender, EventArgs e)
        {
            if (m_UseType==UseType.Renderer)
            {
                Initial();
            }
            if (m_UseType==UseType.General)
            {
                InitialGeneral();
            }
            this.SymbolCtrl1.Width = 333;
            this.SymbolCtrl1.Height = 157;
        }

        private void SymbolCtrl1_OnItemSelected(object sender, ISymbologyControlEvents_OnItemSelectedEvent e)
        {
            m_SelStyleGalleryItem = (IStyleGalleryItem)e.styleGalleryItem;
            m_CurSymbol = (ISymbol)m_SelStyleGalleryItem.Item;
            if (m_SymbolXmlElement!=null)
            {
                m_SymbolXmlElement.SetAttribute("符号名", m_SelStyleGalleryItem.Name);
            }

            PreviewSymbol(m_CurSymbol);

            ChangeControls();
            
        }
        /// <summary>
        /// 预览符号，根据Symbol
        /// </summary>
        /// <param name="vSymbol"></param>
        private void PreviewSymbol(ISymbol vSymbol)
        {
            ISymbologyStyleClass pSymbologyStyleClass;
            IStyleGalleryItem pStyleGalleryItem;
            stdole.IPictureDisp ppicdisp;
            Image pImage;

            try
            {
                pSymbologyStyleClass = this.SymbolCtrl1.GetStyleClass(this.SymbolCtrl1.StyleClass);
                pStyleGalleryItem = new ServerStyleGalleryItemClass();
                pStyleGalleryItem.Item = vSymbol;
                this.SymbolCtrl1.Update();
                ppicdisp = pSymbologyStyleClass.PreviewItem(pStyleGalleryItem, this.pbPreview.Width, this.pbPreview.Height);
                pImage = Image.FromHbitmap(new System.IntPtr(ppicdisp.Handle));
                this.pbPreview.Image = null;
                this.pbPreview.Image = pImage;
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("frmSymboManager的PreviewSymbol出错:" + ex.Message, false, null, null);
                MessageBox.Show("frmSymboManager的PreviewSymbol出错:" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }

        }
        /// <summary>
        /// 改变控件值
        /// </summary>
        private void ChangeControls()
        {
            IRgbColor pColor;
            IRgbColor pRGBColor = new RgbColorClass();
            try
            {
                switch (this.SymbolCtrl1.StyleClass)
                {
                    case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                        IMarkerSymbol pMarkerSymbol;
                        pMarkerSymbol = (IMarkerSymbol)m_CurSymbol;
                        pColor = (IRgbColor)pMarkerSymbol.Color;
                        pRGBColor.RGB = pColor.RGB;
                        pRGBColor.UseWindowsDithering = true;
                        this.cpbPtColor.SelectedColor = Color.FromArgb(pRGBColor.Red, pRGBColor.Green, pRGBColor.Blue);
                        this.upPointSize.Value = Convert.ToDecimal(pMarkerSymbol.Size);
                        this.upPointAngle.Value = Convert.ToDecimal(pMarkerSymbol.Angle);
                        if (m_SymbolXmlElement != null)
                        {
                            this.m_SymbolXmlElement.SetAttribute("颜色", pColor.RGB.ToString());
                            this.m_SymbolXmlElement.SetAttribute("大小", pMarkerSymbol.Size.ToString());
                            this.m_SymbolXmlElement.SetAttribute("角度", pMarkerSymbol.Angle.ToString());
                        }
                        break;
                    case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                        ISimpleLineSymbol pSimpleLineSymbol;
                        IMultiLayerLineSymbol pMultiLayerLineSymbol;

                        if (m_CurSymbol is ISimpleLineSymbol)
                        {
                            pSimpleLineSymbol = (ISimpleLineSymbol)m_CurSymbol;
                            pRGBColor = (IRgbColor)pSimpleLineSymbol.Color;
                            this.cpbLnColor.SelectedColor = Color.FromArgb(pRGBColor.Red, pRGBColor.Green, pRGBColor.Blue);
                            this.upLnWidth.Value = Convert.ToDecimal(pSimpleLineSymbol.Width);
                        }
                        else if (m_CurSymbol is IMultiLayerLineSymbol)
                        {
                            pMultiLayerLineSymbol = (IMultiLayerLineSymbol)m_CurSymbol;
                            pRGBColor = (IRgbColor)pMultiLayerLineSymbol.Color;
                            this.cpbLnColor.SelectedColor = Color.FromArgb(pRGBColor.Red, pRGBColor.Green, pRGBColor.Blue);
                            this.upLnWidth.Value = Convert.ToDecimal(pMultiLayerLineSymbol.Width);
                        }

                        //'Dim pLineSymbol As ILineSymbol   
                        //'pLineSymbol = m_CurSymbol
                        //'pRGBColor = pLineSymbol.Color
                        //'Me.cpbLnColor.SelectedColor = Color.FromArgb(pRGBColor.Red, pRGBColor.Green, pRGBColor.Blue)
                        //'Me.upLnWidth.Value = pLineSymbol.Width

                        if (m_SymbolXmlElement != null)
                        {
                            this.m_SymbolXmlElement.SetAttribute("颜色", pRGBColor.RGB.ToString());
                            this.m_SymbolXmlElement.SetAttribute("线宽", this.upLnWidth.Value.ToString());
                        }
                        break;
                    case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                        IMultiLayerFillSymbol pMultiLayerFillSymbol;
                        //'Dim pLineSymbol As ISimpleLineSymbol
                        pMultiLayerFillSymbol = (IMultiLayerFillSymbol)m_CurSymbol;
                        pColor = (IRgbColor)pMultiLayerFillSymbol.Color;
                        if (m_SymbolXmlElement != null)
                        {
                            this.m_SymbolXmlElement.SetAttribute("填充颜色", pColor.RGB.ToString());
                        }
                        if (pColor != null)
                        {
                            this.cpbAreaColor.SelectedColor = Color.FromArgb(pColor.Transparency, pColor.Red, pColor.Green, pColor.Blue);
                        }

                        if (pMultiLayerFillSymbol.Outline is ISimpleLineSymbol)        //轮廓线
                        {
                            ISimpleLineSymbol pLineSymbol;
                            pLineSymbol = (ISimpleLineSymbol)pMultiLayerFillSymbol.Outline;
                            pColor = (IRgbColor)pLineSymbol.Color;
                            if (m_SymbolXmlElement != null)
                            {
                                this.m_SymbolXmlElement.SetAttribute("轮廓线颜色", pColor.RGB.ToString());
                            }
                            if (pColor != null)
                            {
                                this.cpbAreaBoardWidth.SelectedColor = Color.FromArgb(pColor.Red, pColor.Green, pColor.Blue);
                            }

                            this.upAreaBoardWidth.Value = Convert.ToDecimal(pLineSymbol.Width);
                            if (m_SymbolXmlElement != null)
                            {
                                this.m_SymbolXmlElement.SetAttribute("轮廓线宽度", this.upAreaBoardWidth.Value.ToString());
                            }
                        }
                        else if (pMultiLayerFillSymbol.Outline is IMultiLayerLineSymbol)
                        {
                            IMultiLayerLineSymbol pLineSymbol;
                            pLineSymbol = (IMultiLayerLineSymbol)pMultiLayerFillSymbol.Outline;
                            pColor = (IRgbColor)pLineSymbol.Color;
                            if (m_SymbolXmlElement != null)
                            {
                                this.m_SymbolXmlElement.SetAttribute("轮廓线颜色", pColor.RGB.ToString());
                            }
                            if (pColor != null)
                            {
                                this.cpbAreaBoardWidth.SelectedColor = Color.FromArgb(pColor.Red, pColor.Green, pColor.Blue);
                            }

                            this.upAreaBoardWidth.Value = Convert.ToDecimal(pLineSymbol.Width);
                            if (m_SymbolXmlElement != null)
                            {
                                this.m_SymbolXmlElement.SetAttribute("轮廓线宽度", this.upAreaBoardWidth.Value.ToString());
                            }
                        }
                        else
                        {
                            //IMultiLayerLineSymbol pLineSymbol;
                            //pLineSymbol = (IMultiLayerLineSymbol)pMultiLayerFillSymbol.Outline;
                            //pColor = (IRgbColor)pLineSymbol.Color;
                            //if (m_SymbolXmlElement != null)
                            //{
                            //    this.m_SymbolXmlElement.SetAttribute("轮廓线颜色", pColor.RGB.ToString());
                            //}
                            //if (pColor != null)
                            //{
                            //    this.cpbAreaBoardWidth.SelectedColor = Color.FromArgb(pColor.Red, pColor.Green, pColor.Blue);
                            //}

                            //this.upAreaBoardWidth.Value = Convert.ToDecimal(pLineSymbol.Width);
                            //if (m_SymbolXmlElement != null)
                            //{
                            //    this.m_SymbolXmlElement.SetAttribute("轮廓线宽度", this.upAreaBoardWidth.Value.ToString());
                            //}

                            //20110721  HSh 解决切换符号时候出现bug
                            IFillSymbol pFillSymbol = (IFillSymbol)m_CurSymbol;
                            pColor = (IRgbColor)pFillSymbol.Color;
                            pColor.RGB = pFillSymbol.Color.RGB;
                            //pRGBColor = Color.FromArgb(pColor.Red, pColor.Green, pColor.Blue);

                            if (m_SymbolXmlElement != null)
                            {
                                this.m_SymbolXmlElement.SetAttribute("轮廓线颜色", pColor.RGB.ToString());
                            }
                            if (pColor != null)
                            {
                                this.cpbAreaBoardWidth.SelectedColor = Color.FromArgb(pColor.Red, pColor.Green, pColor.Blue);
                            }

                            this.upAreaBoardWidth.Value = Convert.ToDecimal(pFillSymbol.Outline.Width);
                            if (m_SymbolXmlElement != null)
                            {
                                this.m_SymbolXmlElement.SetAttribute("轮廓线宽度", this.upAreaBoardWidth.Value.ToString());
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// 点符号改变颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cpbPtColor_SelectedColorChanged(object sender, EventArgs e)
        {
            IMarkerSymbol pMarkerSymbol;
            IRgbColor pColor;

            try
            {
                pColor = new RgbColorClass();
                pColor.Red = cpbPtColor.SelectedColor.R;
                pColor.Green = cpbPtColor.SelectedColor.G;
                pColor.Blue = cpbPtColor.SelectedColor.B;

                ChangeMarkerSymboColor(m_CurSymbol, pColor);

                PreviewSymbol(m_CurSymbol);
            }
            catch (Exception)
            {
                //g_ErrorHandler.DisplayInformation(ex.Message, false);
            }
            finally
            {
                pMarkerSymbol = null;
                pColor = null;
            }
        }

        /// <summary>
        /// 改变点大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upPointSize_ValueChanged(object sender, EventArgs e)
        {
            IMarkerSymbol pMarkerSymbol;

            pMarkerSymbol = (IMarkerSymbol)m_CurSymbol;
            if (upPointSize.Value==0)
            {
                return;
            }
            pMarkerSymbol.Size = Convert.ToDouble(upPointSize.Value);
            PreviewSymbol(m_CurSymbol);
            if (m_SymbolXmlElement!=null)
            {
                this.m_SymbolXmlElement.SetAttribute("大小", upPointSize.Value.ToString());
            }
            pMarkerSymbol = null;
        }

        /// <summary>
        /// 改变点角度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upPointAngle_ValueChanged(object sender, EventArgs e)
        {
            IMarkerSymbol pMarkerSymbol;
            pMarkerSymbol = (IMarkerSymbol)m_CurSymbol;

            pMarkerSymbol.Angle = Convert.ToDouble(upPointAngle.Value);
            PreviewSymbol(m_CurSymbol);
            if (m_SymbolXmlElement!=null)
            {
                this.m_SymbolXmlElement.SetAttribute("角度", upPointAngle.Value.ToString());
            }
            pMarkerSymbol = null;
        }

        /// <summary>
        /// 改变线颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cpbLnColor_SelectedColorChanged(object sender, EventArgs e)
        {
            ILineSymbol pLineSymbol;
            IRgbColor pColor;

            try
            {
                pColor = new RgbColorClass();
                pColor.Red = cpbLnColor.SelectedColor.R;
                pColor.Green = cpbLnColor.SelectedColor.G;
                pColor.Blue = cpbLnColor.SelectedColor.B;

                ChangeLineSymboColor(m_CurSymbol, pColor);

                if (m_SymbolXmlElement != null)
                {
                    this.m_SymbolXmlElement.SetAttribute("颜色", pColor.RGB.ToString());
                }
                PreviewSymbol(m_CurSymbol);
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation(ex.Message, false, null, null);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                pLineSymbol = null;
                pColor = null;
            }
        }

        /// <summary>
        /// 改变线宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upLnWidth_ValueChanged(object sender, EventArgs e)
        {
            ILineSymbol pLineSymbol;

            pLineSymbol = (ILineSymbol)m_CurSymbol;

            pLineSymbol.Width = Convert.ToDouble(upLnWidth.Value);
            PreviewSymbol(m_CurSymbol);

            if (this.m_SymbolXmlElement!=null)
            {
                this.m_SymbolXmlElement.SetAttribute("线宽", upLnWidth.Value.ToString());
            }
            pLineSymbol = null;
        }

        /// <summary>
        /// 面填充颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cpbAreaColor_SelectedColorChanged(object sender, EventArgs e)
        {
            //if (cpbAreaColor.Focused==false)
            //{
            //    return;
            //}
            IRgbColor pColor;
            try
            {
                pColor = new RgbColorClass();
                pColor.Red = cpbAreaColor.SelectedColor.R;
                pColor.Green = cpbAreaColor.SelectedColor.G;
                pColor.Blue = cpbAreaColor.SelectedColor.B;

                ChangeAreaSymboColor(m_CurSymbol, pColor);
                PreviewSymbol(m_CurSymbol);

                if (m_SymbolXmlElement != null)
                {
                    this.m_SymbolXmlElement.SetAttribute("填充颜色", pColor.RGB.ToString());
                }
                //Common.ModMain.g_Sys.MapControl.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation(ex.Message, false, null, null);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                pColor = null;
            }
        }

        /// <summary>
        /// 面轮廓线颜色改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cpbAreaBoardWidth_SelectedColorChanged(object sender, EventArgs e)
        {
            ICartographicLineSymbol pLineSymbol;
            IColor pColor;
            IRgbColor pRgbColor;

            try
            {
                pRgbColor = new RgbColorClass();
                pRgbColor.Red = cpbAreaBoardWidth.SelectedColor.R;
                pRgbColor.Green = cpbAreaBoardWidth.SelectedColor.G;
                pRgbColor.Blue = cpbAreaBoardWidth.SelectedColor.B;
                pColor = pRgbColor;

                ChangeAreaSymboOutlineColor(m_CurSymbol, pColor);
                PreviewSymbol(m_CurSymbol);

                if (m_SymbolXmlElement != null)
                {
                    this.m_SymbolXmlElement.SetAttribute("轮廓线颜色", pRgbColor.RGB.ToString());
                }
                //Common.ModMain.g_Sys.MapControl.ActiveView.Refresh();
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation(ex.Message, false, null, null);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                pLineSymbol = null;
                pColor = null;
            }

        }

        /// <summary>
        /// 面轮廓线宽度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upAreaBoardWidth_ValueChanged(object sender, EventArgs e)
        {
            IMultiLayerFillSymbol pMultiLayerFillSymbol=new MultiLayerFillSymbolClass();
            ILineSymbol pLineSymbol=null;

            try
            {
                //pMultiLayerFillSymbol = (IMultiLayerFillSymbol)m_CurSymbol;
                //pLineSymbol = pMultiLayerFillSymbol.Outline;
                ChangeAreaSymboOutlinewWidth(m_CurSymbol, Convert.ToDouble(upAreaBoardWidth.Value));
                PreviewSymbol(m_CurSymbol);

                if (m_SymbolXmlElement != null)
                {
                    this.m_SymbolXmlElement.SetAttribute("轮廓线宽度", upAreaBoardWidth.Value.ToString());
                }
                //Common.ModMain.g_Sys.MapControl.ActiveView.Refresh();
            }
                
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation(ex.Message, false, null, null);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
            finally
            {
                pMultiLayerFillSymbol = null;
                pLineSymbol = null;
            }
        }

        /// <summary>
        /// 改变点颜色
        /// </summary>
        /// <param name="vSymbol"></param>
        /// <param name="pColor"></param>
        private void ChangeMarkerSymboColor(ISymbol vSymbol, IColor pColor)
        {
            IMultiLayerMarkerSymbol pMultiLayerMarkerSymbol;
            ISimpleMarkerSymbol pSimpleMarkerSymbol;

            if (vSymbol is ISimpleMarkerSymbol)
            {
                pSimpleMarkerSymbol = (ISimpleMarkerSymbol)vSymbol;
                pSimpleMarkerSymbol.Color = pColor;
            }
            else if (vSymbol is IMultiLayerMarkerSymbol)
            {
                pMultiLayerMarkerSymbol = (IMultiLayerMarkerSymbol)vSymbol;
                pMultiLayerMarkerSymbol.Color = pColor;
            }

            if (m_SymbolXmlElement!=null)
            {
                this.m_SymbolXmlElement.SetAttribute("颜色", pColor.RGB.ToString());
            }
        }

        /// <summary>
        /// 改变线颜色
        /// </summary>
        /// <param name="vSymbol"></param>
        /// <param name="pColor"></param>
        private void ChangeLineSymboColor(ISymbol vSymbol, IColor pColor)
        {
            IMultiLayerLineSymbol pMultiLayerLineSymbol;
            ISimpleLineSymbol pSimpleLineSymbol;

            if (vSymbol is ISimpleLineSymbol)
            {
                pSimpleLineSymbol = (ISimpleLineSymbol)vSymbol;
                pSimpleLineSymbol.Color = pColor;
            }
            else if (vSymbol is IMultiLayerLineSymbol)
            {
                pMultiLayerLineSymbol = (IMultiLayerLineSymbol)vSymbol;
                pMultiLayerLineSymbol.Color = pColor;
            }
        }

        /// <summary>
        /// 改变面填充颜色
        /// </summary>
        /// <param name="vSymbol"></param>
        /// <param name="pColor"></param>
        private void ChangeAreaSymboColor(ISymbol vSymbol, IColor pColor)
        {
            IMultiLayerFillSymbol pMultiLayerFillSymbol;
            ISimpleFillSymbol pSimpleFillSymbol;

            if (vSymbol is ISimpleFillSymbol)
            {
                pSimpleFillSymbol = (ISimpleFillSymbol)vSymbol;
                pSimpleFillSymbol.Color = pColor;
            }
            else if (vSymbol is IMultiLayerFillSymbol)
            {
                pMultiLayerFillSymbol = (IMultiLayerFillSymbol)vSymbol;
                pMultiLayerFillSymbol.Color = pColor;
            }
        }

        /// <summary>
        /// 改变面轮廓宽度
        /// </summary>
        /// <param name="vSymbol"></param>
        /// <param name="vWidth"></param>
        private void ChangeAreaSymboOutlinewWidth(ISymbol vSymbol, double vWidth)
        {
            IMultiLayerFillSymbol pMultiLayerFillSymbol;
            ISimpleFillSymbol pSimpleFillSymbol;
            ICartographicLineSymbol pLineSymbol;
            int i;

            if (vSymbol is ISimpleFillSymbol)
            {
                pSimpleFillSymbol = (ISimpleFillSymbol)vSymbol;
                pLineSymbol = new CartographicLineSymbolClass();
                pLineSymbol.Width = vWidth;
                pLineSymbol.Color = pSimpleFillSymbol.Outline.Color;
                pSimpleFillSymbol.Outline = pLineSymbol;
            }
            else if (vSymbol is IMultiLayerFillSymbol)
            {
                pMultiLayerFillSymbol = (IMultiLayerFillSymbol)vSymbol;
                for (i = 0; i <= pMultiLayerFillSymbol.LayerCount-1; i++)
                {
                    pLineSymbol = new CartographicLineSymbolClass();
                    pLineSymbol.Width = vWidth;
                    pLineSymbol.Color = pMultiLayerFillSymbol.get_Layer(i).Outline.Color;
                    pMultiLayerFillSymbol.get_Layer(i).Outline = pLineSymbol;
                }
            }
        }

        /// <summary>
        /// 改变面轮廓颜色
        /// </summary>
        /// <param name="vSymbol"></param>
        /// <param name="pColor"></param>
        private void ChangeAreaSymboOutlineColor(ISymbol vSymbol, IColor pColor)
        {
            IMultiLayerFillSymbol pMultiLayerFillSymbol;
            ISimpleFillSymbol pSimpleFillSymbol;
            ICartographicLineSymbol pLineSymbol;
            int i;

            if (vSymbol is ISimpleFillSymbol)
            {
                pSimpleFillSymbol = (ISimpleFillSymbol)vSymbol;
                pLineSymbol = new CartographicLineSymbolClass();
                pLineSymbol.Color = pColor;
                pLineSymbol.Width = pSimpleFillSymbol.Outline.Width;
                pSimpleFillSymbol.Outline = pLineSymbol;
            }
            else if (vSymbol is IMultiLayerFillSymbol)
            {
                pMultiLayerFillSymbol = (IMultiLayerFillSymbol)vSymbol;
                for (i = 0; i <= pMultiLayerFillSymbol.LayerCount-1; i++)
                {
                    pLineSymbol = new CartographicLineSymbolClass();
                    pLineSymbol.Color = pColor;
                    pLineSymbol.Width = pMultiLayerFillSymbol.get_Layer(i).Outline.Width;
                    pMultiLayerFillSymbol.get_Layer(i).Outline = pLineSymbol;
                }
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.chkNullColor.Checked==true)
            {
                RgbColor pRgbColor = new RgbColorClass();
                pRgbColor.Red = 0;
                pRgbColor.Green = 0;
                pRgbColor.Blue = 0;
                pRgbColor.NullColor = true;
                IMultiLayerMarkerSymbol pMultiLayerMarkerSymbol;
                ISimpleMarkerSymbol pSimpleMarkerSymbol;
                int i;
                if (m_CurSymbol is ISimpleMarkerSymbol)
                {
                    pSimpleMarkerSymbol = (ISimpleMarkerSymbol)m_CurSymbol;
                    pSimpleMarkerSymbol.Color = pRgbColor;
                }
                else if (m_CurSymbol is IMultiLayerMarkerSymbol)
                {
                    pMultiLayerMarkerSymbol = (IMultiLayerMarkerSymbol)m_CurSymbol;
                    pMultiLayerMarkerSymbol.Color = pRgbColor;
                    for (i = 0; i <= pMultiLayerMarkerSymbol.LayerCount-1; i++)
                    {
                        pMultiLayerMarkerSymbol.get_Layer(i).Color = pRgbColor;
                    }
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        //*****************************通用****************************************************
        private void InitialGeneral()
        {
            string[] strFiles;
            int i;

            if (Directory.Exists(ClsDeclare.g_StyleFolder)==false)
            {
                Directory.CreateDirectory(ClsDeclare.g_StyleFolder);
            }
            strFiles = Directory.GetFiles(ClsDeclare.g_StyleFolder);
            //int l = strFiles.Length;
            for (i = 0; i <= strFiles.Length-1; i++)
            {
                string fileName;
                int nPos;
                //string str = strFiles[i];

                nPos = Strings.InStrRev(strFiles[i], ".",-1, CompareMethod.Binary);
                fileName = Strings.Right(strFiles[i], Strings.Len(strFiles[i]) - nPos);

                if (Strings.StrComp(fileName, "ServerStyle",CompareMethod.Text)==0)
                {
                    int pathLen = Strings.Len(Path.GetFileName(strFiles[i]));
                    cboSymbolFile.Items.Add(Path.GetFileName(strFiles[i]).Substring(0, pathLen - Strings.Len(".serverstyle")));
                }
            }
            cboSymbolFile.SelectedIndex = 0;

            if (cboSymbolFile.Items.Count<=0)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("无法找到符号文件，请重新安装或与管理员联系。", false, null, null);
                MessageBox.Show("无法找到符号文件，请重新安装或与管理员联系。", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }

            switch (m_SymbolType)
            {
                case "点状地物符号":
                    cboSymbolType.Items.Add("点状地物符号");
                    break;
                case "线状地物符号":
                    cboSymbolType.Items.Add("线状地物符号");
                    break;
                case "面状地物符号":
                    cboSymbolType.Items.Add("面状地物符号");
                    break;
                default:
                    break;
            }
            cboSymbolType.SelectedIndex = 0;

            if (m_InSymbol==null)
            {
                m_InSymbol = ClsFunctions.GetASymbolBySymbolType(m_SymbolType,null);
            }

            IColor pColor = null;
            IRgbColor pRgbColor;

            if (m_InSymbol!=null)
            {
                m_Symbol = m_InSymbol;
                m_CurSymbol = m_InSymbol;
                if (m_Symbol is ILineSymbol)
                {
                    ILineSymbol pLineSymbol;
                    pLineSymbol = (ILineSymbol)m_Symbol;

                    this.upLnWidth.Value = Convert.ToDecimal(string.Format("{0:F2}", pLineSymbol.Width));

                    if (pLineSymbol.Color is IRgbColor)
                    {
                        pColor = pLineSymbol.Color;
                    }
                    else if (pLineSymbol.Color is ICmykColor)
                    {
                        pColor = new RgbColorClass();
                        pColor.RGB = ((IHsvColor)pLineSymbol.Color).RGB;
                    }
                    else if (pLineSymbol.Color is IGrayColor)
                    {
                        pColor = new RgbColorClass();
                        pColor.RGB = ((IHsvColor)pLineSymbol.Color).RGB;
                    }
                    else if (pLineSymbol.Color is IHlsColor)
                    {
                        pColor = new RgbColorClass();
                        pColor.RGB = ((IHsvColor)pLineSymbol.Color).RGB;
                    }
                    else if (pLineSymbol.Color is IHsvColor)
                    {
                        pColor = new RgbColorClass();
                        pColor.RGB = ((IHsvColor)pLineSymbol.Color).RGB;
                    }
                    pRgbColor = (IRgbColor)pColor;
                    pRgbColor.UseWindowsDithering = true;

                    this.cpbLnColor.SelectedColor=Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);
                    
                    InitialGroupBox(esriSymbologyStyleClass.esriStyleClassLineSymbols);
                    PreviewSymbol((ISymbol)pLineSymbol);
                }
                else if (m_Symbol is IFillSymbol)
                {
                    IFillSymbol pFillSymbol;
                    pFillSymbol = (IFillSymbol)m_Symbol;

                    this.upAreaBoardWidth.Value = Convert.ToDecimal(string.Format("{0:F2}", pFillSymbol.Outline.Width));

                    if (pFillSymbol.Color!=null)
                    {
                        if (pFillSymbol.Color is IRgbColor)
                        {
                            pColor = pFillSymbol.Color;
                        }
                        else if (pFillSymbol.Color is ICmykColor)
                        {
                            pColor = new RgbColorClass();
                            pColor.RGB = ((IHsvColor)pFillSymbol.Color).RGB;
                        }
                        else if (pFillSymbol.Color is IGrayColor)
                        {
                            pColor = new RgbColorClass();
                            pColor.RGB = ((IHsvColor)pFillSymbol.Color).RGB;
                        }
                        else if (pFillSymbol.Color is IHlsColor)
                        {
                            pColor = new RgbColorClass();
                            pColor.RGB = ((IHsvColor)pFillSymbol.Color).RGB;
                        }
                        else if (pFillSymbol.Color is IHsvColor)
                        {
                            pColor = new RgbColorClass();
                            pColor.RGB = ((IHsvColor)pFillSymbol.Color).RGB;
                        }
                        pRgbColor = (IRgbColor)pColor;
                        pRgbColor.UseWindowsDithering = true;
                        this.cpbAreaColor.SelectedColor = Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);

                        pRgbColor = null;

                        pRgbColor = (IRgbColor)pFillSymbol.Outline.Color;
                        pRgbColor.UseWindowsDithering = true;
                        this.cpbAreaBoardWidth.SelectedColor = Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);
                    }
                    InitialGroupBox(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                    PreviewSymbol((ISymbol)pFillSymbol);
                }
                else if (m_Symbol is IMarkerSymbol)
                {
                    IMarkerSymbol pMarkSymbol;
                    pMarkSymbol = (IMarkerSymbol)m_Symbol;

                    this.upPointSize.Value = Convert.ToInt32(pMarkSymbol.Size);
                    this.upPointAngle.Value = Convert.ToInt32(pMarkSymbol.Angle);

                    if (pMarkSymbol.Color is IRgbColor)
                    {
                        pColor = pMarkSymbol.Color;
                    }
                    else if (pMarkSymbol.Color is ICmykColor)
                    {
                        pColor = new RgbColorClass();
                        pColor.RGB = ((IHsvColor)pMarkSymbol.Color).RGB;
                    }
                    else if (pMarkSymbol.Color is IGrayColor)
                    {
                        pColor = new RgbColorClass();
                        pColor.RGB = ((IHsvColor)pMarkSymbol.Color).RGB;
                    }
                    else if (pMarkSymbol.Color is IHlsColor)
                    {
                        pColor = new RgbColorClass();
                        pColor.RGB = ((IHsvColor)pMarkSymbol.Color).RGB;
                    }
                    else if (pMarkSymbol.Color is IHsvColor)
                    {
                        pColor = new RgbColorClass();
                        pColor.RGB = ((IHsvColor)pMarkSymbol.Color).RGB;
                    }
                    pRgbColor = (IRgbColor)pColor;
                    pRgbColor.UseWindowsDithering = true;

                    this.cpbPtColor.SelectedColor = Color.FromArgb(pRgbColor.Red, pRgbColor.Green, pRgbColor.Blue);

                    InitialGroupBox(esriSymbologyStyleClass.esriStyleClassMarkerSymbols);
                    PreviewSymbol(m_CurSymbol);

                }
            }
            pPtNum = 2;
        }

        //*************************************************************************************
        private void cboSymbolFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SymbolCtrl1.Clear();

            string strStyleFilePath = ClsDeclare.g_StyleFolder + this.cboSymbolFile.Text;
            if (Strings.Right(strStyleFilePath,11)=="ServerStyle")          
            {
                //???
            }
            else
            {
                strStyleFilePath = ClsDeclare.g_StyleFolder + this.cboSymbolFile.Text + ".ServerStyle";
            }

            this.SymbolCtrl1.LoadStyleFile(strStyleFilePath);
            switch (m_SymbolType)
            {
                case "点状地物符号":
                    this.SymbolCtrl1.StyleClass = esriSymbologyStyleClass.esriStyleClassMarkerSymbols;
                    break;
                case "线状地物符号":
                    this.SymbolCtrl1.StyleClass = esriSymbologyStyleClass.esriStyleClassLineSymbols;
                    break;
                case "面状地物符号":
                    this.SymbolCtrl1.StyleClass = esriSymbologyStyleClass.esriStyleClassFillSymbols;
                    break;
                default:
                    break;
            }

        }

        private void chkNoColor_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkNoColor.Checked==true)
            {
                this.cpbAreaColor.Enabled = false;
                IRgbColor pColor;

                try
                {
                    pColor = new RgbColorClass();
                    pColor.NullColor = true;
                    ChangeAreaSymboColor(m_CurSymbol, pColor);
                    PreviewSymbol(m_CurSymbol);
                    if (m_SymbolXmlElement != null)
                    {
                        this.m_SymbolXmlElement.SetAttribute("填充颜色", pColor.RGB.ToString());
                    }
                }
                catch (Exception ex)
                {
                    //ModDeclare.g_ErrorHandler.DisplayInformation(ex.Message, false, null, null);
                    MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
                finally
                {
                    pColor = null;
                }
            }
            else
            {
                this.cpbAreaColor.Enabled = true;
            }
        }









    }
}
