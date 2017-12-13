using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using DevComponents.DotNetBar;
using System.IO;
namespace ZJGISLayerManager
{
    public partial class FrmSymbolSelect : DevComponents.DotNetBar.Office2007Form
    {
        private IStyleGalleryItem pStyleGalleryItem;
        private ILegendClass pLegendClass;
        private ILayer pLayer;
        public ISymbol pSymbol;
        public Image pSymbolImage;
        bool contextMenuStripMoreSymbolInitialed = false;

        public FrmSymbolSelect(ILegendClass tempLegendClass, ILayer tempLayer)
        {
            InitializeComponent();
            this.pLegendClass = tempLegendClass;
            this.pLayer = tempLayer;
        }
        private void SetFeatureClassStyle(esriSymbologyStyleClass pesriSymbollogyStyleClass)
        {
            this.SymbologyControl.StyleClass = pesriSymbollogyStyleClass;
            ISymbologyStyleClass pSymbologyStyleClass = this.SymbologyControl.GetStyleClass(pesriSymbollogyStyleClass);
            if (pLegendClass != null)
            {
                IStyleGalleryItem currentStyleGalleryItem = new ServerStyleGalleryItem();
                currentStyleGalleryItem.Name = "当前符号";
                currentStyleGalleryItem.Item = pLegendClass.Symbol;
                pSymbologyStyleClass.AddItem(currentStyleGalleryItem, 0);
                this.pStyleGalleryItem = currentStyleGalleryItem;
            }
            pSymbologyStyleClass.SelectItem(0);
        }

        private string ReadRegistry(string sKey)
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(sKey, true);
            if (rk == null)
            {
                return "";

            }
            return (string)rk.GetValue("InstallDir");
        }

        private void FrmSymbolSelect_Load(object sender, EventArgs e)
        {
            //string sInstall = ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
            string sInstall = ReadRegistry("SOFTWARE\\ESRI\\Engine10.2\\CoreRuntime");
            if (sInstall == null)
            {
                return;
            }
            //test
            string test = sInstall + @"Styles\ESRI.ServerStyle";
            //this.SymbologyControl.LoadStyleFile(sInstall + @"Styles\ESRI.ServerStyle");
            //string stylesPath = new DirectoryInfo("").FullName + @"Res\Styles\ESRI.ServerStyle";
            string stylesPath = Application.StartupPath.Substring(0, Application.StartupPath.LastIndexOf(@"\")) + @"\Res\Styles\ESRI.ServerStyle";
           
            this.SymbologyControl.LoadStyleFile(stylesPath);

            IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
            IFeatureLayer pFeature = pLayer as IFeatureLayer;
            switch (pFeature.FeatureClass.ShapeType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                    SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassMarkerSymbols);
                    lblAngle.Visible = true;
                    nudAngle.Visible = true;
                    lblSize.Visible = true;
                    nudSize.Visible = true;
                    lblWidth.Visible = false;
                    nudWidth.Visible = false;
                    lblOutlineColor.Visible = false;
                    btnOutlineColor.Visible = false;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                    SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassLineSymbols);
                    lblAngle.Visible = false;
                    nudAngle.Visible = false;
                    lblSize.Visible = false;
                    nudSize.Visible = false;
                    lblWidth.Visible = true;
                    nudWidth.Visible = true;
                    lblOutlineColor.Visible = false;
                    btnOutlineColor.Visible = false;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                    SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                    lblAngle.Visible = false;
                    nudAngle.Visible = false;
                    lblSize.Visible = false;
                    nudSize.Visible = false;
                    lblWidth.Visible = true;
                    nudWidth.Visible = true;
                    lblOutlineColor.Visible = true;
                    btnOutlineColor.Visible = true;
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultiPatch:
                    SetFeatureClassStyle(esriSymbologyStyleClass.esriStyleClassFillSymbols);
                    lblAngle.Visible = false;
                    nudAngle.Visible = false;
                    lblSize.Visible = false;
                    nudSize.Visible = false;
                    lblWidth.Visible = true;
                    nudWidth.Visible = true;
                    lblOutlineColor.Visible = true;
                    btnOutlineColor.Visible = true;
                    break;
                default:
                    this.Close();
                    break;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.pSymbol = pStyleGalleryItem.Item as ISymbol;
            this.pSymbolImage = this.ptnPreview.Image;
            this.Close();//dialgresult默认设置为OK
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PreviewImage()
        {
            stdole.IPictureDisp picture = this.SymbologyControl.GetStyleClass(SymbologyControl.StyleClass).PreviewItem(pStyleGalleryItem, ptnPreview.Width, ptnPreview.Height);
            System.Drawing.Image Image = System.Drawing.Image.FromHbitmap(new System.IntPtr(picture.Handle));
            this.ptnPreview.Image = Image;
        }

        private void SymbologyControl_OnDoubleClick(object sender, ISymbologyControlEvents_OnDoubleClickEvent e)
        {
            this.btnApply.PerformClick();
        }

        private void SymbologyControl_OnStyleClassChanged(object sender, ISymbologyControlEvents_OnStyleClassChangedEvent e)
        {
            //switch((esriSymbologyStyleClass)e.symbologyStyleClass)//书上Debug
            switch (this.SymbologyControl.StyleClass)
            {
                case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:

                    lblAngle.Visible = true;
                    nudAngle.Visible = true;
                    lblSize.Visible = true;
                    nudSize.Visible = true;
                    lblWidth.Visible = false;
                    nudWidth.Visible = false;
                    lblOutlineColor.Visible = false;
                    btnOutlineColor.Visible = false;
                    break;
                case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                    lblAngle.Visible = false;
                    nudAngle.Visible = false;
                    lblSize.Visible = false;
                    nudSize.Visible = false;
                    lblWidth.Visible = true;
                    nudWidth.Visible = true;
                    lblOutlineColor.Visible = false;
                    btnOutlineColor.Visible = false;
                    break;
                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    lblAngle.Visible = false;
                    nudAngle.Visible = false;
                    lblSize.Visible = false;
                    nudSize.Visible = false;
                    lblWidth.Visible = true;
                    nudWidth.Visible = true;
                    lblOutlineColor.Visible = true;
                    btnOutlineColor.Visible = true;
                    break;
            }

        }

        private void SymbologyControl_OnItemSelected(object sender, ISymbologyControlEvents_OnItemSelectedEvent e)
        {
            pStyleGalleryItem = e.styleGalleryItem as IStyleGalleryItem;
            Color color;
            try
            {
                switch (SymbologyControl.StyleClass)
                {
                    case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                        color = this.ConvertIRgbColorToColor(((IMarkerSymbol)pStyleGalleryItem.Item).Color as IRgbColor);
                        this.nudAngle.Value = (decimal)((IMarkerSymbol)this.pStyleGalleryItem.Item).Angle;
                        this.nudSize.Value = (decimal)((IMarkerSymbol)this.pStyleGalleryItem.Item).Size;
                        break;
                    case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                        color = this.ConvertIRgbColorToColor(((ILineSymbol)pStyleGalleryItem.Item).Color as IRgbColor);
                        this.nudWidth.Value = (decimal)((ILineSymbol)this.pStyleGalleryItem.Item).Width;
                        break;
                    case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                        color = this.ConvertIRgbColorToColor(((IFillSymbol)pStyleGalleryItem.Item).Color as IRgbColor);
                        btnOutlineColor.BackColor = this.ConvertIRgbColorToColor(((IFillSymbol)pStyleGalleryItem.Item).Outline.Color as IRgbColor);
                        this.nudWidth.Value = (decimal)((IFillSymbol)this.pStyleGalleryItem.Item).Outline.Width;
                        break;
                    default:
                        color = Color.Black;
                        break;
                }
                this.btnColor.BackColor = color;
                this.PreviewImage();
            }
            catch (Exception err)
            {
                //MessageBoxEx.Show(err.ToString());解决NULL问题
            }
        }
        public IColor ConvertColorToIColor(Color color)
        {

            IColor pColor = new RgbColorClass();

            pColor.RGB = color.B * 65536 + color.G * 256 + color.R;

            return pColor;

        }

        //调整点大小
        private void nudSize_ValueChanged(object sender, EventArgs e)
        {
            ((IMarkerSymbol)this.pStyleGalleryItem.Item).Size = (double)this.nudSize.Value;

            this.PreviewImage();

        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            switch (this.SymbologyControl.StyleClass)
            {
                case esriSymbologyStyleClass.esriStyleClassLineSymbols:

                    ((ILineSymbol)this.pStyleGalleryItem.Item).Width = Convert.ToDouble(this.nudWidth.Value);
                    break;
                case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                    //取得面符号的轮廓线符号
                    ILineSymbol pLineSymbol = ((IFillSymbol)this.pStyleGalleryItem.Item).Outline;
                    pLineSymbol.Width = Convert.ToDouble(this.nudWidth.Value);
                    ((IFillSymbol)this.pStyleGalleryItem.Item).Outline = pLineSymbol;
                    break;
            }
            this.PreviewImage();
        }

        private void nudAngle_ValueChanged(object sender, EventArgs e)
        {
            ((IMarkerSymbol)this.pStyleGalleryItem.Item).Angle = (double)this.nudAngle.Value;
            this.PreviewImage();
        }

        public Color ConvertIRgbColorToColor(IRgbColor pRgbColor)
        {

            return ColorTranslator.FromOle(pRgbColor.RGB);
            //return ColorTranslator.FromWin32(pRgbColor.RGB);
        }

        private void btnOutlineColor_Click(object sender, EventArgs e)
        {
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                //取得面符号中的外框线符号
                ILineSymbol pLineSymbol = ((IFillSymbol)this.pStyleGalleryItem.Item).Outline;
                //设置外框线颜色
                pLineSymbol.Color = this.ConvertColorToIColor(this.colorDialog.Color);
                //重新设置面符号中的外框线符号
                ((IFillSymbol)this.pStyleGalleryItem.Item).Outline = pLineSymbol;
                //设置按钮背景颜色
                this.btnOutlineColor.BackColor = this.colorDialog.Color;
                //更新符号预览
                this.PreviewImage();
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            //调用系统颜色对话框
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                //将颜色按钮的背景颜色设置为用户选定的颜色
                this.btnColor.BackColor = this.colorDialog.Color;
                switch (this.SymbologyControl.StyleClass)
                {
                    case esriSymbologyStyleClass.esriStyleClassMarkerSymbols:
                        ((IMarkerSymbol)this.pStyleGalleryItem.Item).Color = this.ConvertColorToIColor(this.colorDialog.Color);
                        break;
                    case esriSymbologyStyleClass.esriStyleClassLineSymbols:
                        ((ILineSymbol)this.pStyleGalleryItem.Item).Color = this.ConvertColorToIColor(this.colorDialog.Color);
                        break;
                    case esriSymbologyStyleClass.esriStyleClassFillSymbols:
                        ((IFillSymbol)this.pStyleGalleryItem.Item).Color = this.ConvertColorToIColor(this.colorDialog.Color);
                        break;
                }
                this.PreviewImage();
            }
        }

        private void btnMoreSymbols_Click(object sender, EventArgs e)
        {
            if (this.contextMenuStripMoreSymbolInitialed == false)
            {
                //string sInstall = ReadRegistry("SOFTWARE\\ESRI\\Engine10.0\\CoreRuntime");
                string sInstall = ReadRegistry("SOFTWARE\\ESRI\\Engine10.2\\CoreRuntime");
                string path = System.IO.Path.Combine(sInstall, "Styles");
                //取得菜单项数量
                string[] styleNames = System.IO.Directory.GetFiles(path, "*.ServerStyle");
                ToolStripMenuItem[] symbolContextMenuItem = new ToolStripMenuItem[styleNames.Length + 1];
                //循环添加其它符号菜单项到菜单
                for (int i = 0; i < styleNames.Length; i++)
                {
                    symbolContextMenuItem[i] = new ToolStripMenuItem();
                    symbolContextMenuItem[i].CheckOnClick = true;
                    symbolContextMenuItem[i].Text = System.IO.Path.GetFileNameWithoutExtension(styleNames[i]);
                    if (symbolContextMenuItem[i].Text == "ESRI")
                    {
                        symbolContextMenuItem[i].Checked = true;
                    }
                    symbolContextMenuItem[i].Name = styleNames[i];
                }

                //添加“更多符号”菜单项到菜单最后一项
                symbolContextMenuItem[styleNames.Length] = new ToolStripMenuItem();
                symbolContextMenuItem[styleNames.Length].Text = "添加符号";
                symbolContextMenuItem[styleNames.Length].Name = "AddMoreSymbol";
                //添加所有的菜单项到菜单
                this.contextMenuStripMoreSymbols.Items.AddRange(symbolContextMenuItem);
                this.contextMenuStripMoreSymbolInitialed = true;

            }

            //显示菜单

            this.contextMenuStripMoreSymbols.Show(this.btnMoreSymbols.Location);


        }

        private void contextMenuStripMoreSymbol_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem pToolStripMenuItem = (ToolStripMenuItem)e.ClickedItem;
            if (pToolStripMenuItem.Name == "AddMoreSymbol")
            {
                if (this.openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.SymbologyControl.LoadStyleFile(this.openFileDialog.FileName);
                    this.SymbologyControl.Refresh();

                }

            }
            else
            {
                if (pToolStripMenuItem.Checked == false)
                {
                    this.SymbologyControl.LoadStyleFile(pToolStripMenuItem.Name);
                    this.SymbologyControl.Refresh();
                }
                else
                {
                    this.SymbologyControl.RemoveFile(pToolStripMenuItem.Name);
                    this.SymbologyControl.Refresh();

                }

            }

        }

    }
}
