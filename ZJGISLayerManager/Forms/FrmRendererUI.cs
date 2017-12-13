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
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using DevComponents.DotNetBar.Controls;
using System.IO;
using Microsoft.VisualBasic;
using ZJGISLayerManager;
using System.Collections;

namespace ZJGISLayerManager
{
    public partial class FrmRendererUI : DevComponents.DotNetBar.Office2007Form
    {
        #region "变量"

        public string m_RendererType;
        public bool m_OK;

        private IMapControl4 m_pMapControl;
        private IFeatureRenderer m_OldRenderer;
        private IFeatureRenderer m_Renderer;
        private ISymbol m_Symbol;
        private String m_SymbolType;
        private String m_SymbolInfo;

        private String m_RenderSavePath;
        private ISymbol m_GraduatedSymbol;
        private ISymbol m_GraduatedBackSymbol;

        private ISymbol m_ProportionalMinSymbol;
        private ISymbol m_ProportionalBackSymbol;
        private Collection m_ColorScheme = new Collection();
        private Collection m_ColorName = new Collection();

        private ISymbol m_ChartSymbol;
        private IFeatureLayer m_FeatureLayer;
        private ISymbol m_InSymbol;

        private ImageCombo m_UniqueSymboImageCombo = new ImageCombo();
        private ImageCombo m_UniqueSymbolManyValueImageCombo = new ImageCombo();
        private ImageCombo m_GraduatedColor = new ImageCombo();
        private ImageCombo m_BarColor = new ImageCombo();
        private int lPicWidth = 32;
        private int lPicHeight = 16;

        private bool m_bIsChanged = false;
        private string[] strGroupBoxes = { "FrameSimple", "FrameUniqueValue", "FrameURM", "FrameGC", "FrameGS", "FramePS", "FrameChart" };

        #endregion

        #region "属性"

        //传入MapControl
        public IMapControl4 MapControl
        {
            set
            {
                m_pMapControl = value;
            }
        }
        //符号化是否更改
        public bool IsChanged
        {
            get
            {
                return m_bIsChanged;
            }
        }

        //传入的符号
        public ISymbol InSymbol
        {
            get
            {
                return m_Symbol;
            }
            set
            {
                m_Symbol = value;
            }
        }

        //传入的符号类型  点状、线状、面状
        public string SymbolType
        {
            get
            {
                return m_SymbolType;
            }
            set
            {
                m_SymbolType = value;
            }
        }

        public IFeatureLayer FeatureLayer
        {
            get
            {
                return m_FeatureLayer;
            }
            set
            {
                m_FeatureLayer = value;
                IGeoFeatureLayer pGeoFeatLyr;
                pGeoFeatLyr = (IGeoFeatureLayer)m_FeatureLayer;
                m_OldRenderer = pGeoFeatLyr.Renderer;
            }
        }

        public IFeatureRenderer RendererUI
        {
            get
            {
                return m_Renderer;
            }
            set
            {
                m_Renderer = value;
            }
        }

        #endregion

        #region "方法、响应事件"

        public FrmRendererUI()
        {
            InitializeComponent();

            this.m_UniqueSymboImageCombo.SelectedIndexChanged += new EventHandler(m_UniqueSymboImageCombo_SelectedIndexChanged);
            this.m_UniqueSymbolManyValueImageCombo.SelectedIndexChanged += new EventHandler(m_UniqueSymbolManyValueImageCombo_SelectedIndexChanged);
            this.m_GraduatedColor.SelectedIndexChanged += new EventHandler(m_GraduatedColor_SelectedIndexChanged);
            this.m_BarColor.SelectedIndexChanged += new EventHandler(m_BarColor_SelectedIndexChanged);
        }

        //选择一个符号
        private void picSymbol_Click(object sender, EventArgs e)
        {
            FrmSymbolManager frm = new FrmSymbolManager(FrmSymbolManager.UseType.General);

            if (m_Symbol == null)
            {
                m_Symbol = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, null);
            }
            frm.m_InSymbol = m_Symbol;
            frm.m_SymbolType = m_SymbolType;

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_Symbol = frm.CurSymbol;
                picSymbol.Image = CreatePictureFromSymbol(m_Symbol, picSymbol.Width, picSymbol.Height);
            }

        }

        private void FrmRendererUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            IGeoFeatureLayer pGeoFeatLyr;
            if (m_bIsChanged == false)
            {
                pGeoFeatLyr = (IGeoFeatureLayer)m_FeatureLayer;
                pGeoFeatLyr.Renderer = m_OldRenderer;
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void FrmRendererUI_Load(object sender, EventArgs e)
        {
            this.progressbarRenderer.Visible = false;

            //得到符号类型
            GetSymbolType();

            //添加符号设置的类型到树图
            FunAddRendererType();

            DataGridViewImageColumn pImageColumn;
            pImageColumn = new DataGridViewImageColumn(false);
            pImageColumn.Name = "Symbol";
            pImageColumn.HeaderText = "符号";



            dgvUR.Columns.Add(pImageColumn);
            dgvUR.Columns.Add("Value", "属性值");
            dgvUR.Columns.Add("Label", "标签");
            dgvUR.Columns.Add("Count", "数量");
            dgvUR.RowHeadersVisible = false;
            dgvUR.AllowUserToAddRows = false;
            dgvUR.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            pImageColumn = new DataGridViewImageColumn(false);
            pImageColumn.Name = "Symbol";
            pImageColumn.HeaderText = "符号";
            dgvURM.Columns.Add(pImageColumn);

            dgvURM.Columns.Add("Value", "属性值");
            dgvURM.Columns.Add("Label", "标签");
            dgvURM.RowHeadersVisible = false;
            dgvURM.AllowUserToAddRows = false;
            dgvURM.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            pImageColumn = new DataGridViewImageColumn(false);
            pImageColumn.Name = "Symbol";
            pImageColumn.HeaderText = "符号";
            dgvGC.Columns.Add(pImageColumn);

            dgvGC.Columns.Add("Value", "范围");
            dgvGC.Columns.Add("Label", "标签");
            dgvGC.RowHeadersVisible = false;
            dgvGC.AllowUserToAddRows = false;
            dgvGC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            pImageColumn = new DataGridViewImageColumn(false);
            pImageColumn.Name = "Symbol";
            pImageColumn.HeaderText = "符号";
            dgvGS.Columns.Add(pImageColumn);

            dgvGS.Columns.Add("Value", "范围");
            dgvGS.Columns.Add("Label", "标签");
            dgvGS.RowHeadersVisible = false;
            dgvGS.AllowUserToAddRows = false;
            dgvGS.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            pImageColumn = new DataGridViewImageColumn(false);
            pImageColumn.Name = "Symbol";
            pImageColumn.HeaderText = "符号";
            dgvBarFields.Columns.Add(pImageColumn);

            dgvBarFields.Columns.Add("Value", "字段");
            dgvBarFields.RowHeadersVisible = false;
            dgvBarFields.AllowUserToAddRows = false;
            dgvBarFields.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            FunLoadLayer(m_FeatureLayer);

        }

        //得到符号类型
        private void GetSymbolType()
        {
            if (m_FeatureLayer == null)
            {
                return;
            }
            IFeatureClass pFeatureClass;
            pFeatureClass = m_FeatureLayer.FeatureClass;
            int lGeomIndex;
            string sShpName;
            IFields pFields;
            IField pField;
            IGeometryDef pGeometryDef;

            sShpName = pFeatureClass.ShapeFieldName;
            pFields = pFeatureClass.Fields;
            lGeomIndex = pFields.FindField(sShpName);
            pField = pFields.get_Field(lGeomIndex);
            pGeometryDef = pField.GeometryDef;

            switch (pGeometryDef.GeometryType)
            {
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint:
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultipoint:
                    m_SymbolType = "点状地物符号";
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline:
                    m_SymbolType = "线状地物符号";
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine:
                    m_SymbolType = "线状地物符号";
                    break;
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon:
                case ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryMultiPatch:
                    m_SymbolType = "面状地物符号";
                    break;
                default:
                    break;
            }

        }

        //添加符号设置的类型到树图
        private void FunAddRendererType()
        {
            TvwRenderType.Nodes.Clear();
            TvwRenderType.Nodes.Add("Features", "要素集").Expand();
            TvwRenderType.Nodes[0].Name = "Features";
            TvwRenderType.Nodes[0].Nodes.Add("SimpleSymbol", "简单符号");
            TvwRenderType.Nodes[0].Nodes[0].Name = "SimpleSymbol";

            TvwRenderType.Nodes.Add("Categories", "目录集").Expand();
            TvwRenderType.Nodes[1].Name = "Categories";
            TvwRenderType.Nodes[1].Nodes.Add("UniqueSymbol", "质地填充");
            TvwRenderType.Nodes[1].Nodes[0].Name = "UniqueSymbol";
            TvwRenderType.Nodes[1].Nodes.Add("UniqueSymbolManyValue", "多值质地填充");
            TvwRenderType.Nodes[1].Nodes[1].Name = "UniqueSymbolManyValue";

            TvwRenderType.Nodes.Add("Quantities", "数量集").Expand();
            TvwRenderType.Nodes[2].Name = "Quantities";
            TvwRenderType.Nodes[2].Nodes.Add("GraduatedColor", "分级颜色");
            TvwRenderType.Nodes[2].Nodes[0].Name = "GraduatedColor";
            TvwRenderType.Nodes[2].Nodes.Add("GraduatedSymbol", "分级符号");
            TvwRenderType.Nodes[2].Nodes[1].Name = "GraduatedSymbol";
            TvwRenderType.Nodes[2].Nodes.Add("ProportionalSymbol", "比例符号");
            TvwRenderType.Nodes[2].Nodes[2].Name = "ProportionalSymbol";

            TvwRenderType.Nodes.Add("Charts", "图表类").Expand();
            TvwRenderType.Nodes[3].Name = "Charts";
            TvwRenderType.Nodes[3].Nodes.Add("Pie", "饼图");
            TvwRenderType.Nodes[3].Nodes[0].Name = "Pie";
            TvwRenderType.Nodes[3].Nodes.Add("Bar", "柱图");
            TvwRenderType.Nodes[3].Nodes[1].Name = "Bar";
            TvwRenderType.Nodes[3].Nodes.Add("Stack", "堆图");
            TvwRenderType.Nodes[3].Nodes[2].Name = "Stack";
        }

        private void FunLoadLayer(IFeatureLayer pFeatureLayer)
        {
            TreeNode nodeX = null;
            IFeatureRenderer pRenderer;
            IGeoFeatureLayer pGeoFeatureLayer;

            pGeoFeatureLayer = (IGeoFeatureLayer)pFeatureLayer;
            pRenderer = pGeoFeatureLayer.Renderer;

            //简单填充
            if (pRenderer is ISimpleRenderer)
            {
                ISimpleRenderer pSimpleRenderer;

                pSimpleRenderer = (ISimpleRenderer)pRenderer;
                m_InSymbol = pSimpleRenderer.Symbol;
                m_Symbol = m_InSymbol;
                nodeX = TvwRenderType.Nodes[0].Nodes["SimpleSymbol"];
                nodeX.Tag = pRenderer;
            }
            //质地填充  单一值渲染
            else if (pRenderer is IUniqueValueRenderer)
            {
                IUniqueValueRenderer pUniqueValueRenderer;
                pUniqueValueRenderer = (IUniqueValueRenderer)pRenderer;
                if (pUniqueValueRenderer.FieldCount <= 1)
                {
                    nodeX = TvwRenderType.Nodes[1].Nodes["UniqueSymbol"];
                }
                else
                {
                    nodeX = TvwRenderType.Nodes[1].Nodes["UniqueSymbolManyValue"];
                }
                nodeX.Tag = pRenderer;
            }
            //分级  分类等级渲染
            else if (pRenderer is IClassBreaksRenderer)
            {
                IClassBreaksRenderer pClassBreaksRenderer;
                pClassBreaksRenderer = (IClassBreaksRenderer)pRenderer;
                ILegendInfo pLegendInfo;
                pLegendInfo = (ILegendInfo)pClassBreaksRenderer;

                if (pLegendInfo.SymbolsAreGraduated == false)
                {
                    nodeX = TvwRenderType.Nodes[2].Nodes["GraduatedColor"];
                }
                else
                {
                    nodeX = TvwRenderType.Nodes[2].Nodes["GraduatedSymbol"];
                }
                nodeX.Tag = pRenderer;
                if (pRenderer is IProportionalSymbolRenderer)
                {
                    nodeX = TvwRenderType.Nodes[2].Nodes["ProportionalSymbol"];
                    nodeX.Tag = pRenderer;
                }
            }
            //比例  根据属性值设置符号大小
            else if (pRenderer is IProportionalSymbolRenderer)
            {
                nodeX = TvwRenderType.Nodes[2].Nodes["ProportionalSymbol"];
                nodeX.Tag = pRenderer;
            }
            //图表  图表符号化
            else if (pRenderer is IChartRenderer)
            {
                IChartRenderer pChartRenderer;
                pChartRenderer = (IChartRenderer)pRenderer;

                IChartSymbol pChartSymbol;
                pChartSymbol = pChartRenderer.ChartSymbol;

                if (pChartSymbol is IBarChartSymbol)
                {
                    nodeX = TvwRenderType.Nodes[3].Nodes["Bar"];
                    nodeX.Tag = pRenderer;
                }
                else if (pChartSymbol is IPieChartRenderer)
                {
                    nodeX = TvwRenderType.Nodes[3].Nodes["Pie"];
                    nodeX.Tag = pRenderer;
                }
                else if (pChartSymbol is IStackedChartSymbol)
                {
                    nodeX = TvwRenderType.Nodes[3].Nodes["Stack"];
                    nodeX.Tag = pRenderer;
                }
            }
            else
            {
                return;
            }

            if (nodeX != null)
            {
                TvwRenderType.SelectedNode = nodeX;
                TvwRenderType_NodeMouseClick(nodeX, null);
            }

        }

        private void TvwRenderType_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {


            TreeNode Node;

            if (e != null)
            {
                Node = e.Node;
            }
            else
            {
                Node = this.TvwRenderType.SelectedNode;
            }

            IGeoFeatureLayer pGeoFeatureLayer;
            pGeoFeatureLayer = (IGeoFeatureLayer)m_FeatureLayer;

            m_Renderer = null;
            m_Renderer = (IFeatureRenderer)Node.Tag;

            //if (Node.Level==0)
            //{
            //    return;                 //点击首级节点不用新建Render
            //}
            //else
            //{
            switch (Node.Name)
            {
                //要素集
                case "SimpleSymbol":        //简单符号
                    InitialGroupBox("FrameSimple");
                    m_RendererType = Node.Name;
                    if (m_Renderer == null)
                    {
                        m_Renderer = new SimpleRendererClass();
                    }
                    switch (m_SymbolType)
                    {
                        case "点状地物符号":
                            btnSimpleOption.Visible = true;
                            break;
                        case "线状地物符号":
                            btnSimpleOption.Visible = false;
                            break;
                        case "面状地物符号":
                            btnSimpleOption.Visible = true;
                            break;
                        default:
                            break;
                    }
                    break;
                //目录集
                case "UniqueSymbol":        //质地填充
                    InitialGroupBox("FrameUniqueValue");
                    m_RendererType = Node.Name;

                    InitialComboBox(ClsDeclare.g_ColorFile, CreateImageCombo("UniqueSymbol"));
                    if (m_Renderer == null)
                    {
                        m_Renderer = new UniqueValueRendererClass();
                    }
                    switch (m_SymbolType)
                    {
                        case "点状地物符号":
                            CmdURAdvance.Visible = true;
                            break;
                        case "线状地物符号":
                            CmdURAdvance.Visible = false;
                            break;
                        case "面状地物符号":
                            CmdURAdvance.Visible = true;
                            break;
                        default:
                            break;
                    }
                    break;
                //
                case "UniqueSymbolManyValue":       //多值质地填充
                    InitialGroupBox("FrameURM");
                    m_RendererType = Node.Name;
                    InitialComboBox(ClsDeclare.g_ColorFile, CreateImageCombo("UniqueSymbolManyValue"));
                    if (m_Renderer == null)
                    {
                        m_Renderer = new UniqueValueRendererClass();
                    }
                    switch (m_SymbolType)
                    {
                        case "点状地物符号":
                            CmdURMAdvance.Visible = true;
                            break;
                        case "线状地物符号":
                            CmdURMAdvance.Visible = false;
                            break;
                        case "面状地物符号":
                            CmdURMAdvance.Visible = true;
                            break;
                        default:
                            break;
                    }
                    break;
                //数量集
                case "GraduatedColor":              //分级颜色
                    InitialGroupBox("FrameGC");
                    m_RendererType = Node.Name;
                    InitialComboBox(ClsDeclare.g_ColorFile, CreateImageCombo("GraduatedColor"));

                    if (m_Renderer == null)
                    {
                        m_Renderer = new ClassBreaksRendererClass();
                        ILegendInfo pLegendInfo1;
                        pLegendInfo1 = (ILegendInfo)m_Renderer;
                        pLegendInfo1.SymbolsAreGraduated = false;
                    }
                    break;
                case "GraduatedSymbol":             //分级符号
                    InitialGroupBox("FrameGS");
                    m_RendererType = Node.Name;

                    if (m_Renderer == null)
                    {
                        m_Renderer = new ClassBreaksRendererClass();
                        ILegendInfo pLegendInfo2;
                        pLegendInfo2 = (ILegendInfo)m_Renderer;
                        pLegendInfo2.SymbolsAreGraduated = true;
                    }
                    break;
                case "ProportionalSymbol":          //比例符号
                    InitialGroupBox("FramePS");
                    m_RendererType = Node.Name;
                    if (m_Renderer == null)
                    {
                        m_Renderer = new ProportionalSymbolRendererClass();
                    }
                    break;
                //图表类
                case "Pie":                         //饼图
                case "Bar":                         //柱图
                case "Stack":                       //直方图
                    InitialGroupBox("FrameChart");
                    m_RendererType = Node.Name;
                    if (m_Renderer == null)
                    {
                        m_Renderer = new ChartRendererClass();
                    }
                    break;
                default:
                    return;
            }
            //}

            pGeoFeatureLayer.Renderer = m_Renderer;

            //加载Renderer信息到对应的表中
            FunLoadLayerRenderer(m_FeatureLayer);

            PicRenderType.Image = ImgLstRenderType.Images[Node.Name];

        }

        //加载Renderer信息到对应的表中
        private void FunLoadLayerRenderer(IFeatureLayer pFeatureLayer)
        {
            IFeatureRenderer pRenderer;
            IGeoFeatureLayer pGeoFeatureLayer;
            string pLable, strLegendDesc;
            int i;
            string value;

            pGeoFeatureLayer = (IGeoFeatureLayer)pFeatureLayer;
            pRenderer = pGeoFeatureLayer.Renderer;
            m_Renderer = pRenderer;

            if (pRenderer is ISimpleRenderer)
            {
                //简单符号化
                //界面控制
                //FrameSimple.ZOrder 0
                //读取符号

                ISimpleRenderer pSimpleRenderer;
                pSimpleRenderer = (ISimpleRenderer)pRenderer;
                if (pSimpleRenderer.Symbol == null)
                {
                    m_Symbol = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, null);
                }
                else
                {
                    m_Symbol = pSimpleRenderer.Symbol;
                }
                pLable = pSimpleRenderer.Label;
                strLegendDesc = pSimpleRenderer.Description;

                Image pPicture;
                pPicture = CreatePictureFromSymbol(m_Symbol, picSymbol.Width, picSymbol.Height);
                picSymbol.Image = pPicture;

                txtLegend.Text = pLable;
                txtLegendDesc.Text = strLegendDesc;

            }
            else if (pRenderer is IUniqueValueRenderer)
            {
                //质地填充
                IUniqueValueRenderer pUniqueValueRenderer;
                pUniqueValueRenderer = (IUniqueValueRenderer)pRenderer;
                if (pUniqueValueRenderer.FieldCount > 0)
                {
                    if (pUniqueValueRenderer.FieldCount == 1)
                    {
                        //单值质地填充
                        //FrameUniqueValue.ZOrder 0
                        //FillImageColor CmbColor
                        //读取所有字段名
                        FunAddFieldToCombo(m_FeatureLayer, CmbUniqueValueField, "");

                        if (pUniqueValueRenderer.get_Field(0) != "")
                        {
                            CmbUniqueValueField.Text = GetAliasNameByName(pUniqueValueRenderer.get_Field(0), m_FeatureLayer);
                        }
                        this.dgvUR.Rows.Clear();
                        //************************************************************************
                        //获取分类代码为某值的要素的个数的条件
                        int FeatCount;
                        IQueryFilter pQueryFilter = new QueryFilterClass();

                        //************************************************************************

                        for (i = 0; i <= pUniqueValueRenderer.ValueCount - 1; i++)
                        {
                            value = pUniqueValueRenderer.get_Value(i);
                            pLable = pUniqueValueRenderer.get_Label(value);
                            pQueryFilter.WhereClause = GetWhereClause(m_FeatureLayer.FeatureClass, pUniqueValueRenderer.get_Field(0), value);
                            FeatCount = m_FeatureLayer.FeatureClass.FeatureCount(pQueryFilter);     //获得要素个数

                            try
                            {
                                this.dgvUR.Rows.Add();
                                this.dgvUR.Rows[i].Tag = pUniqueValueRenderer.get_Symbol(value);
                                this.dgvUR.Rows[i].Cells[0].Value = CreatePictureFromSymbol(pUniqueValueRenderer.get_Symbol(value), lPicWidth, lPicHeight);
                                this.dgvUR.Rows[i].Cells[1].Value = value;
                                this.dgvUR.Rows[i].Cells[2].Value = pLable;
                                this.dgvUR.Rows[i].Cells[3].Value = FeatCount;

                            }
                            catch (Exception ex)
                            {
                                //ModDeclare.g_ErrorHandler.DisplayInformation(ex.Message, false, "确定", null);
                                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            }

                        }
                    }
                    else
                    {
                        //多值质地填充
                        //FrameURM.ZOrder 0
                        //FillImageColor CmbURMColor
                        //读取所有字段名
                        FunAddFieldToCombo(m_FeatureLayer, CmbURMField1, "");
                        FunAddFieldToCombo(m_FeatureLayer, CmbURMField2, "");
                        FunAddFieldToCombo(m_FeatureLayer, CmbURMField3, "");

                        if (pUniqueValueRenderer.FieldCount == 2)         //双值质地填充
                        {
                            if (pUniqueValueRenderer.get_Field(0) != "")
                            {
                                CmbURMField1.Text = GetAliasNameByName(pUniqueValueRenderer.get_Field(0), m_FeatureLayer);
                            }
                            if (pUniqueValueRenderer.get_Field(1) != "")
                            {
                                CmbURMField2.Text = GetAliasNameByName(pUniqueValueRenderer.get_Field(1), m_FeatureLayer);
                            }
                        }
                        else                                             //三值质地填充
                        {
                            if (pUniqueValueRenderer.get_Field(0) != "")
                            {
                                CmbURMField1.Text = GetAliasNameByName(pUniqueValueRenderer.get_Field(0), m_FeatureLayer);
                            }
                            if (pUniqueValueRenderer.get_Field(1) != "")
                            {
                                CmbURMField2.Text = GetAliasNameByName(pUniqueValueRenderer.get_Field(1), m_FeatureLayer);
                            }
                            if (pUniqueValueRenderer.get_Field(2) != "")
                            {
                                CmbURMField3.Text = GetAliasNameByName(pUniqueValueRenderer.get_Field(2), m_FeatureLayer);
                            }
                        }

                        this.dgvURM.Rows.Clear();
                        for (i = 0; i <= pUniqueValueRenderer.ValueCount - 1; i++)
                        {
                            value = pUniqueValueRenderer.get_Value(i);
                            pLable = pUniqueValueRenderer.get_Label(value);

                            this.dgvURM.Rows.Add();
                            this.dgvURM.Rows[i].Tag = pUniqueValueRenderer.get_Symbol(value);
                            this.dgvURM.Rows[i].Cells[0].Value = CreatePictureFromSymbol(pUniqueValueRenderer.get_Symbol(value), lPicWidth, lPicHeight);
                            this.dgvURM.Rows[i].Cells[1].Value = value;
                            this.dgvURM.Rows[i].Cells[2].Value = pLable;
                        }
                    }
                }
                else
                {
                    FunAddFieldToCombo(m_FeatureLayer, CmbUniqueValueField, "");
                    this.CmbUniqueValueField.SelectedIndex = 0;
                    FunAddFieldToCombo(m_FeatureLayer, CmbURMField1, "");
                    this.CmbURMField1.SelectedIndex = 0;
                    FunAddFieldToCombo(m_FeatureLayer, CmbURMField2, "");
                    this.CmbURMField2.SelectedIndex = 0;
                    FunAddFieldToCombo(m_FeatureLayer, CmbURMField3, "");
                    this.CmbURMField3.SelectedIndex = 0;
                }

            }
            else if (pRenderer is IClassBreaksRenderer)
            {
                //分级显示
                ILegendInfo pLegendInfo;
                pLegendInfo = (ILegendInfo)pRenderer;

                if (pLegendInfo.SymbolsAreGraduated == false)
                {
                    CmbGCLevel.Items.Clear();
                    CmbGCLevel.Items.Add("2");
                    CmbGCLevel.Items.Add("3");
                    CmbGCLevel.Items.Add("4");
                    CmbGCLevel.Items.Add("5");
                    CmbGCLevel.Items.Add("6");
                    CmbGCLevel.Items.Add("7");
                    CmbGCLevel.Items.Add("8");
                    CmbGCLevel.Items.Add("9");
                    CmbGCLevel.Items.Add("10");
                    CmbGCLevel.Items.Add("11");
                    CmbGCLevel.Items.Add("12");

                    CmbGCLevel.SelectedIndex = 3;
                    IClassBreaksRenderer pClassBreaksRendererGC;
                    pClassBreaksRendererGC = (IClassBreaksRenderer)pRenderer;
                    //加载适当类型的属性字段到下拉框
                    FunAddFieldToCombo(m_FeatureLayer, CmbGCField, "num");

                    if (pClassBreaksRendererGC.Field != "")
                    {
                        CmbGCField.Text = GetAliasNameByName(pClassBreaksRendererGC.Field, m_FeatureLayer);
                    }
                    else
                    {
                        CmbGCField.SelectedIndex = 0;
                    }
                    CmbGCLevel.Text = pClassBreaksRendererGC.BreakCount.ToString();

                    this.dgvGC.Rows.Clear();
                    try
                    {
                        for (i = 0; i <= pClassBreaksRendererGC.BreakCount - 1; i++)
                        {
                            value = pClassBreaksRendererGC.get_Break(i).ToString();
                            pLable = pClassBreaksRendererGC.get_Label(i);
                            this.dgvGC.Rows.Add();
                            this.dgvGC.Rows[this.dgvGC.RowCount - 1].Tag = pClassBreaksRendererGC.get_Symbol(i);
                            this.dgvGC.Rows[this.dgvGC.RowCount - 1].Cells[0].Value = CreatePictureFromSymbol(pClassBreaksRendererGC.get_Symbol(i), lPicWidth, lPicHeight);
                            this.dgvGC.Rows[this.dgvGC.RowCount - 1].Cells[1].Value = value;
                            this.dgvGC.Rows[this.dgvGC.RowCount - 1].Cells[2].Value = pLable;
                        }
                    }
                    catch (Exception ex)
                    {
                        //ModDeclare.g_ErrorHandler.DisplayInformation(ex.Message, false, "确定", null);
                        MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    string strSymbolType = "";
                    switch (m_SymbolType)
                    {
                        case "面状地物符号":
                            strSymbolType = "点状地物符号";
                            PicPSym.Visible = true;
                            FrameSSym.Visible = true;
                            break;
                        case "线状地物符号":
                            strSymbolType = "线状地物符号";
                            PicPSym.Visible = true;
                            FrameSSym.Visible = false;
                            break;
                        case "点状地物符号":
                            strSymbolType = "点状地物符号";
                            PicPSym.Visible = true;
                            FrameSSym.Visible = false;
                            break;
                        default:
                            break;
                    }

                    this.CmbGSLevel.Items.Clear();
                    this.CmbGSLevel.Items.Add("2");
                    this.CmbGSLevel.Items.Add("3");
                    this.CmbGSLevel.Items.Add("4");
                    this.CmbGSLevel.Items.Add("5");
                    this.CmbGSLevel.Items.Add("6");
                    this.CmbGSLevel.Items.Add("7");
                    this.CmbGSLevel.Items.Add("8");
                    this.CmbGSLevel.Items.Add("9");
                    this.CmbGSLevel.Items.Add("10");
                    this.CmbGSLevel.Items.Add("11");
                    this.CmbGSLevel.Items.Add("12");

                    this.CmbGSLevel.SelectedIndex = 4;
                    IClassBreaksRenderer pClassBreaksRendererGS;
                    pClassBreaksRendererGS = (IClassBreaksRenderer)pRenderer;

                    //加载适当类型的属性字段到下拉框
                    FunAddFieldToCombo(m_FeatureLayer, CmbGSField, "num");
                    if (pClassBreaksRendererGS.Field != "")
                    {
                        CmbGSField.Text = GetAliasNameByName(pClassBreaksRendererGS.Field, m_FeatureLayer);
                    }
                    else
                    {
                        CmbGSField.SelectedIndex = 0;
                    }
                    this.dgvGS.Rows.Clear();

                    for (i = 0; i <= pClassBreaksRendererGS.BreakCount - 1; i++)
                    {
                        value = pClassBreaksRendererGS.get_Break(i).ToString();
                        pLable = pClassBreaksRendererGS.get_Label(i);

                        this.dgvGS.Rows.Add();
                        this.dgvGS.Rows[dgvGS.RowCount - 1].Tag = pClassBreaksRendererGS.get_Symbol(i);
                        IMarkerSymbol pMS = (IMarkerSymbol)pClassBreaksRendererGS.get_Symbol(i);

                        this.dgvGS.Rows[dgvGS.RowCount - 1].Cells[0].Value = CreatePictureFromSymbol2((ISymbol)pMS, lPicWidth, lPicHeight, strSymbolType);
                        this.dgvGS.Rows[dgvGS.RowCount - 1].Cells[1].Value = value;
                        this.dgvGS.Rows[dgvGS.RowCount - 1].Cells[2].Value = pLable;
                    }
                    if (!(pClassBreaksRendererGS.BreakCount == 0))
                    {
                        this.PicPSym.Image = CreatePictureFromSymbol2(pClassBreaksRendererGS.get_Symbol(0), PicPSym.Width, PicPSym.Height, strSymbolType);
                        this.m_GraduatedSymbol = pClassBreaksRendererGS.get_Symbol(0);
                    }

                    this.PicSSym.Image = CreatePictureFromSymbol2((ISymbol)pClassBreaksRendererGS.BackgroundSymbol, PicSSym.Width, PicSSym.Height, "面状地物符号");
                    this.m_GraduatedBackSymbol = (ISymbol)pClassBreaksRendererGS.BackgroundSymbol;
                }
            }
            else if (pRenderer is IProportionalSymbolRenderer)
            {
                string strSymbolType = "";
                switch (m_SymbolType)
                {
                    case "面状地物符号":
                    case "点状地物符号":
                        strSymbolType = "点状地物符号";
                        break;
                    case "线状地物符号":
                        strSymbolType = "线状地物符号";
                        break;
                    default:
                        break;
                }
                //比例符号
                IProportionalSymbolRenderer pProportionalSymbolRenderer;
                pProportionalSymbolRenderer = (IProportionalSymbolRenderer)pRenderer;

                FunAddFieldToCombo(m_FeatureLayer, CmbPSField, "num");

                CmbPSField.Text = GetAliasNameByName(pProportionalSymbolRenderer.Field, m_FeatureLayer);
                if (CmbPSField.Text == "")
                {
                    CmbPSField.Text = "NULL";
                }
                IMarkerSymbol pMS;
                m_ProportionalMinSymbol = pProportionalSymbolRenderer.MinSymbol;
                pMS = (IMarkerSymbol)m_ProportionalMinSymbol;
                m_ProportionalBackSymbol = (ISymbol)pProportionalSymbolRenderer.BackgroundSymbol;

                if (m_ProportionalMinSymbol == null)
                {
                    m_ProportionalMinSymbol = ClsFunctions.GetASymbolBySymbolType(strSymbolType, null);
                }
                if (m_ProportionalBackSymbol == null)
                {
                    m_ProportionalBackSymbol = ClsFunctions.GetASymbolBySymbolType("面状地物符号", null);
                }
                PicPSSymMin.Image = CreatePictureFromSymbol2(m_ProportionalMinSymbol, PicPSSymMin.Width, PicPSSymMin.Height, strSymbolType);
                PicPSSymBack.Image = CreatePictureFromSymbol2(m_ProportionalBackSymbol, PicPSSymMin.Width, PicPSSymMin.Height, "面状地物符号");
            }
            else if (pRenderer is IChartRenderer)
            {
                IChartRenderer pChartRenderer;
                pChartRenderer = (IChartRenderer)pRenderer;

                if (pChartRenderer.BaseSymbol == null)
                {
                    pChartRenderer.BaseSymbol = ClsFunctions.GetASymbolBySymbolType("面状地物符号", null);
                }
                PicBackGround.Image = CreatePictureFromSymbol(pChartRenderer.BaseSymbol, PicBackGround.Width, PicBackGround.Height);
                PicBackGround.Tag = pChartRenderer.BaseSymbol;

                if (pChartRenderer.UseOverposter == false)
                {
                    ChkBarCover.Checked = false;
                }
                else
                {
                    ChkBarCover.Checked = true;
                }

                ISymbolArray pSymbolArray;
                IRendererFields pRendererFields;
                IFillSymbol pFillSymbol;
                IChartSymbol pChartSymbol;
                pChartSymbol = pChartRenderer.ChartSymbol;

                if (pChartSymbol is IBarChartSymbol)
                {
                    m_ChartSymbol = pChartRenderer.BaseSymbol;
                    pRendererFields = (IRendererFields)pChartRenderer;

                    IBarChartSymbol pBarChartSymbol;
                    pBarChartSymbol = (IBarChartSymbol)pChartRenderer.ChartSymbol;

                    pSymbolArray = (ISymbolArray)pBarChartSymbol;
                    this.dgvBarFields.Rows.Clear();
                    for (i = 0; i <= pRendererFields.FieldCount - 1; i++)
                    {
                        this.dgvBarFields.Rows.Add();
                        this.dgvBarFields.Rows[dgvBarFields.RowCount - 1].Cells[1].Value = GetAliasNameByName(pRendererFields.get_Field(i), m_FeatureLayer);
                        pFillSymbol = (IFillSymbol)pSymbolArray.get_Symbol(i);
                        this.dgvBarFields.Rows[dgvBarFields.RowCount - 1].Cells[0].Value = CreatePictureFromSymbol2((ISymbol)pFillSymbol, lPicWidth, lPicHeight, "面状地物符号");
                        this.dgvBarFields.Rows[dgvBarFields.RowCount - 1].Tag = pFillSymbol;
                    }
                }
                else if (pChartSymbol is IPieChartSymbol)
                {
                    m_ChartSymbol = pChartRenderer.BaseSymbol;
                    pRendererFields = (IRendererFields)pChartRenderer;

                    IPieChartSymbol pPieChartSymbol;
                    pPieChartSymbol = (IPieChartSymbol)pChartRenderer.ChartSymbol;

                    pSymbolArray = (ISymbolArray)pPieChartSymbol;
                    this.dgvBarFields.Rows.Clear();
                    for (i = 0; i <= pRendererFields.FieldCount - 1; i++)
                    {
                        this.dgvBarFields.Rows.Add();
                        this.dgvBarFields.Rows[this.dgvBarFields.Rows.Count - 1].Cells[1].Value = GetAliasNameByName(pRendererFields.get_Field(i), m_FeatureLayer);
                        pFillSymbol = (IFillSymbol)pSymbolArray.get_Symbol(i);
                        this.dgvBarFields.Rows[this.dgvBarFields.Rows.Count - 1].Cells[0].Value = CreatePictureFromSymbol2((ISymbol)pFillSymbol, lPicWidth, lPicHeight, "面状地物符号");
                        this.dgvBarFields.Rows[this.dgvBarFields.Rows.Count - 1].Tag = pFillSymbol;
                    }
                }
                else if (pChartSymbol is IStackedChartSymbol)
                {
                    m_ChartSymbol = pChartRenderer.BaseSymbol;
                    pRendererFields = (IRendererFields)pChartRenderer;

                    IStackedChartSymbol pStackedChartSymbol;
                    pStackedChartSymbol = (IStackedChartSymbol)pChartRenderer.ChartSymbol;
                    pSymbolArray = (ISymbolArray)pStackedChartSymbol;

                    this.dgvBarFields.Rows.Clear();
                    for (i = 0; i <= pRendererFields.FieldCount - 1; i++)
                    {
                        this.dgvBarFields.Rows.Add();
                        this.dgvBarFields.Rows[this.dgvBarFields.Rows.Count - 1].Cells[1].Value = GetAliasNameByName(pRendererFields.get_Field(i), m_FeatureLayer);
                        pFillSymbol = (IFillSymbol)pSymbolArray.get_Symbol(i);
                        this.dgvBarFields.Rows[this.dgvBarFields.Rows.Count - 1].Cells[0].Value = CreatePictureFromSymbol2((ISymbol)pFillSymbol, lPicWidth, lPicHeight, "面状地物符号");
                        this.dgvBarFields.Rows[this.dgvBarFields.Rows.Count - 1].Tag = pFillSymbol;
                    }
                }

                //图表符号（饼图，柱图，直方图）
                FunAddFieldToListbox(m_FeatureLayer, LstBarFields, "num");
                InitialComboBox(ClsDeclare.g_ColorFile, CreateImageCombo("BarColor"));

            }

        }

        private Image CreatePictureFromSymbol(ISymbol vSymbol, int lWidth, int lHeight)
        {
            ISymbologyStyleClass pSymbologyStyleClass = null;
            IStyleGalleryItem pStyleGalleryItem;
            stdole.IPictureDisp ppicdisp;
            Image pImage;

            try
            {
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
                pSymbologyStyleClass = this.SymbolCtrl1.GetStyleClass(this.SymbolCtrl1.StyleClass);
                pStyleGalleryItem = new ServerStyleGalleryItemClass();
                if (vSymbol == null)
                {
                    vSymbol = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, null);
                }
                pStyleGalleryItem.Item = (object)vSymbol;
                this.SymbolCtrl1.Update();
                ppicdisp = pSymbologyStyleClass.PreviewItem(pStyleGalleryItem, lWidth, lHeight);
                pImage = Image.FromHbitmap(new System.IntPtr(ppicdisp.Handle));

                return pImage;
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("frmRendererUI的CreatePictureFromSymbol出错:" + ex.Message, false, "确定", null);
                MessageBox.Show("frmRendererUI的CreatePictureFromSymbol出错:" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return null;
            }

        }

        private Image CreatePictureFromSymbol2(ISymbol vSymbol, int lWidth, int lHeight, string strSymbolType)
        {
            ISymbologyStyleClass pSymbologyStyleClass = null;
            IStyleGalleryItem pStyleGalleryItem;
            stdole.IPictureDisp ppicdisp;
            Image pImage;

            try
            {
                switch (strSymbolType)
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
                pSymbologyStyleClass = this.SymbolCtrl1.GetStyleClass(this.SymbolCtrl1.StyleClass);
                pStyleGalleryItem = new ServerStyleGalleryItemClass();
                if (vSymbol == null)
                {
                    vSymbol = ClsFunctions.GetASymbolBySymbolType(strSymbolType, null);
                }
                pStyleGalleryItem.Item = vSymbol;
                this.SymbolCtrl1.Update();
                ppicdisp = pSymbologyStyleClass.PreviewItem(pStyleGalleryItem, lWidth, lHeight);
                pImage = Image.FromHbitmap(new System.IntPtr(ppicdisp.Handle));

                return pImage;
            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("frmRendererUI的CreatePictureFromSymbol2出错:" + ex.Message, false, "确定", null);
                MessageBox.Show("frmRendererUI的CreatePictureFromSymbol2出错:" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return null;
            }
        }

        private void SaveRenderer()
        {
            string m_Directory;
            IGeoFeatureLayer pGeoFeatureLayer;
            IFeatureRenderer pRenderer;
            IAnnotateLayerPropertiesCollection pAnnoLayerPropsCol;
            IPersistStream pPersistStream;
            IPersistStream pPersistStream1;
            IMemoryBlobStream pMemoryStream;
            IMemoryBlobStream pMemoryStream1;
            string pFileName;
            string pFileName1;
            string RenderInfo;
            IDataset pDataset;

            if (m_FeatureLayer == null)
            {
                return;
            }
            m_Directory = System.Windows.Forms.Application.StartupPath + @"\符号化信息\";

            try
            {
                pGeoFeatureLayer = (IGeoFeatureLayer)m_FeatureLayer;
                pRenderer = pGeoFeatureLayer.Renderer;

                pAnnoLayerPropsCol = pGeoFeatureLayer.AnnotationProperties;
                pPersistStream = (IPersistStream)pRenderer;

                pPersistStream1 = (IPersistStream)pAnnoLayerPropsCol;

                pMemoryStream = new MemoryBlobStreamClass();
                pPersistStream.Save(pMemoryStream, 0);

                pMemoryStream1 = new MemoryBlobStreamClass();
                pPersistStream1.Save(pMemoryStream1, 0);

                if (System.IO.Directory.Exists(m_Directory) == false)
                {
                    System.IO.Directory.CreateDirectory(m_Directory);
                }

                pFileName = m_Directory + "Rendererer";
                pFileName1 = m_Directory + "Annotaion";
                pMemoryStream.SaveToFile(pFileName);
                pMemoryStream1.SaveToFile(pFileName1);

                pDataset = (IDataset)pGeoFeatureLayer.FeatureClass;
                RenderInfo = "LayerName = " + pDataset.Name + "\n";
                if (pRenderer is ISimpleRenderer)
                {
                    RenderInfo = RenderInfo + "Render Type = SimpleRenderer" + "\n";
                }
                else if (pRenderer is IUniqueValueRenderer)
                {
                    RenderInfo = RenderInfo + "Render Type = UniqueValueRenderer" + "\n";
                }
                else if (pRenderer is IClassBreaksRenderer)
                {
                    RenderInfo = RenderInfo + "Render Type = ClassBreaksRenderer" + "\n";
                }
                else if (pRenderer is IProportionalSymbolRenderer)
                {
                    RenderInfo = RenderInfo + "Render Type = ProportionalSymbolRenderer" + "\n";
                }
                else if (pRenderer is IChartRenderer)
                {
                    RenderInfo = RenderInfo + "Render Type = ChartRenderer" + "\n";
                }
                else
                {
                    RenderInfo = RenderInfo + "Render Type = ?" + "\n";
                }

                RenderInfo = RenderInfo + "MinimumScale = " + pGeoFeatureLayer.MinimumScale.ToString() + "\n" + "MaximumScale = " + pGeoFeatureLayer.MaximumScale.ToString() + "\n";
                pFileName = m_Directory + "RenderInfo.txt";
                System.IO.File.WriteAllText(pFileName, RenderInfo, System.Text.Encoding.Unicode);

            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("frmRendererUI的SaveRenderer出错:" + ex.Message, false, "确定", null);
                MessageBox.Show("frmRendererUI的SaveRenderer出错:" + ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_bIsChanged = true;
            FunSetLayerRenderer();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            //Common.ModMain.g_Sys.MapControl.ActiveView.Refresh();
            m_pMapControl.ActiveView.Refresh();
            //SaveRenderer();
        }

        private void btnSimpleOption_PopupOpen(object sender, EventArgs e)
        {
            switch (m_SymbolType)
            {
                case "点状地物符号":
                    mnuTrans.Visible = false;
                    mnuRotation.Visible = true;
                    break;
                case "线状地物符号":
                    mnuTrans.Visible = false;
                    mnuRotation.Visible = false;
                    return;
                case "面状地物符号":
                    mnuTrans.Visible = true;
                    mnuRotation.Visible = false;
                    break;
                default:
                    break;
            }
        }

        //旋转设置
        private void mnuRotation_Click(object sender, EventArgs e)
        {
            FrmRotateRenderer frm = new FrmRotateRenderer();
            frm.m_FeatureLayer = m_FeatureLayer;
            frm.ShowDialog();
            frm = null;
        }

        //*********************************************************************************
        //** 函 数 名：FunSetLayerRenderer
        //** 输    入：
        //** 输    出：
        //** 功能描述：设置图层的符号
        //** 全局变量：
        //** 调用模块：
        //*********************************************************************************
        private bool FunSetLayerRenderer()
        {
            int i;
            string Value, pLable;

            //return false;

            IFeatureClass pFeatureClass;
            ITable pTable;
            IQueryFilter pQueryFilter;
            ICursor pCursor;
            IDataStatistics pDataStatistics;
            IStatisticsResults pStatisticsResult;

            double MaxValue = 0;
            string FieldText1, FieldText2, FieldText3;

            if (m_Renderer is ISimpleRenderer)
            {
                //简单符号化
                ISimpleRenderer pSimpleRenderer;
                pSimpleRenderer = (ISimpleRenderer)m_Renderer;
                pSimpleRenderer.Symbol = m_Symbol;
                pSimpleRenderer.Label = txtLegend.Text;
                pSimpleRenderer.Description = txtLegendDesc.Text;
            }
            else if (m_Renderer is IUniqueValueRenderer)
            {
                //生成质地填充
                IUniqueValueRenderer pUniqueValueRenderer;
                pUniqueValueRenderer = (IUniqueValueRenderer)m_Renderer;
                pUniqueValueRenderer.DefaultLabel = "唯一字段分类信息";
                FieldText1 = GetItemData(CmbUniqueValueField, m_FeatureLayer);          //以哪个字段名为标准，如：NAME

                if (TvwRenderType.SelectedNode.Name == "UniqueSymbol")
                {
                    pUniqueValueRenderer.RemoveAllValues();
                    pUniqueValueRenderer.DefaultSymbol = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, null);
                    pUniqueValueRenderer.UseDefaultSymbol = true;
                    pUniqueValueRenderer.FieldCount = 1;
                    pUniqueValueRenderer.set_Field(0, FieldText1);

                    for (i = 0; i <= this.dgvUR.Rows.Count - 1; i++)
                    {
                        Value = this.dgvUR.Rows[i].Cells[1].Value.ToString();
                        pLable = this.dgvUR.Rows[i].Cells[2].Value.ToString();
                        m_Symbol = (ISymbol)this.dgvUR.Rows[i].Tag;
                        pUniqueValueRenderer.AddValue(Value, FieldText1, m_Symbol);
                        pUniqueValueRenderer.set_Label(Value, pLable);
                    }
                }
                else if (TvwRenderType.SelectedNode.Name == "UniqueSymbolManyValue")
                {
                    pUniqueValueRenderer.RemoveAllValues();
                    pUniqueValueRenderer.DefaultSymbol = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, null);
                    pUniqueValueRenderer.UseDefaultSymbol = true;
                    pUniqueValueRenderer.FieldCount = 1;
                    FieldText1 = GetItemData(CmbURMField1, m_FeatureLayer);
                    FieldText2 = GetItemData(CmbURMField2, m_FeatureLayer);
                    FieldText3 = GetItemData(CmbURMField3, m_FeatureLayer);

                    string strValue;
                    if (FieldText1 == "")
                    {
                        return false;
                    }
                    pUniqueValueRenderer.set_Field(0, FieldText1);
                    strValue = FieldText1;
                    if (FieldText2 != "")
                    {
                        pUniqueValueRenderer.FieldCount = 2;
                        pUniqueValueRenderer.set_Field(1, FieldText2);
                        strValue = strValue + "," + FieldText2;
                    }
                    if (FieldText3 != "")
                    {
                        pUniqueValueRenderer.FieldCount = 3;
                        pUniqueValueRenderer.set_Field(2, FieldText3);
                        strValue = strValue + "," + FieldText3;
                    }

                    if (FieldText2 != "")
                    {
                        pUniqueValueRenderer.FieldDelimiter = ",";
                    }
                    else
                    {
                        pUniqueValueRenderer.FieldDelimiter = "";
                    }

                    for (i = 0; i <= dgvURM.Rows.Count - 1; i++)
                    {
                        Value = this.dgvURM.Rows[i].Cells[1].Value.ToString();
                        pLable = this.dgvURM.Rows[i].Cells[2].Value.ToString();
                        m_Symbol = (ISymbol)this.dgvURM.Rows[i].Tag;
                        pUniqueValueRenderer.AddValue(Value, strValue, m_Symbol);
                        pUniqueValueRenderer.set_Label(Value, pLable);
                    }

                }
            }
            else if (m_Renderer is IClassBreaksRenderer)
            {
                //生成分级符号或分级颜色
                IClassBreaksRenderer pClassBreaksRenderer;
                pClassBreaksRenderer = (IClassBreaksRenderer)m_Renderer;

                ILegendInfo pLegendInfo;

                if (TvwRenderType.SelectedNode.Name == "GraduatedColor")
                {
                    FieldText1 = GetItemData(CmbGCField, m_FeatureLayer);
                    pClassBreaksRenderer.Field = FieldText1;
                    pClassBreaksRenderer.BreakCount = this.dgvGC.RowCount;
                    pClassBreaksRenderer.SortClassesAscending = true;

                    for (i = 0; i <= this.dgvGC.RowCount - 1; i++)
                    {
                        Value = this.dgvGC.Rows[i].Cells[1].Value.ToString();
                        pLable = this.dgvGC.Rows[i].Cells[2].Value.ToString();
                        m_Symbol = (ISymbol)this.dgvGC.Rows[i].Tag;
                        pClassBreaksRenderer.set_Symbol(i, m_Symbol);
                        pClassBreaksRenderer.set_Break(i, Convert.ToDouble(Value));
                        pClassBreaksRenderer.set_Label(i, pLable);
                    }

                    pLegendInfo = (ILegendInfo)pClassBreaksRenderer;
                    pLegendInfo.SymbolsAreGraduated = false;
                }
                else if (TvwRenderType.SelectedNode.Name == "GraduatedSymbol")
                {
                    if ((m_SymbolType == "面状地物符号") && (m_GraduatedBackSymbol == null))
                    {
                        //ModDeclare.g_ErrorHandler.DisplayInformation("分级符号方案生成失败,请设置背景符号!", false, "确定", null);
                        MessageBox.Show("分级符号方案生成失败,请设置背景符号!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        return false;
                    }
                    FieldText1 = GetItemData(CmbGSField, m_FeatureLayer);
                    pClassBreaksRenderer.Field = FieldText1;
                    pClassBreaksRenderer.BreakCount = this.dgvGS.RowCount;
                    pClassBreaksRenderer.SortClassesAscending = true;

                    for (i = 0; i <= this.dgvGS.RowCount - 1; i++)
                    {
                        Value = this.dgvGS.Rows[i].Cells[1].Value.ToString();
                        pLable = this.dgvGS.Rows[i].Cells[2].Value.ToString();
                        m_Symbol = (ISymbol)this.dgvGS.Rows[i].Tag;
                        pClassBreaksRenderer.set_Symbol(i, m_Symbol);
                        pClassBreaksRenderer.set_Break(i, Convert.ToDouble(Value));
                        pClassBreaksRenderer.set_Label(i, pLable);
                    }

                    pClassBreaksRenderer.BackgroundSymbol = (IFillSymbol)m_GraduatedBackSymbol;

                    pLegendInfo = (ILegendInfo)pClassBreaksRenderer;
                    pLegendInfo.SymbolsAreGraduated = true;
                }

            }
            else if (m_Renderer is IProportionalSymbolRenderer)
            {
                //生成比例符号
                IProportionalSymbolRenderer pProportionalSymbolRenderer;
                pProportionalSymbolRenderer = (IProportionalSymbolRenderer)m_Renderer;

                pFeatureClass = m_FeatureLayer.FeatureClass;
                pTable = (ITable)pFeatureClass;

                pQueryFilter = new QueryFilterClass();
                FieldText1 = GetItemData(CmbPSField, m_FeatureLayer);
                if (FieldText1 == "")
                {
                    return false;
                }
                pQueryFilter.AddField(FieldText1);

                pCursor = pTable.Search(pQueryFilter, true);

                pDataStatistics = new DataStatisticsClass();
                pDataStatistics.Cursor = pCursor;
                pDataStatistics.Field = FieldText1;

                pStatisticsResult = pDataStatistics.Statistics;
                if (pStatisticsResult == null)
                {
                    //ModDeclare.g_ErrorHandler.DisplayInformation("计算属性最大值或最小值失败,请检查数据是否正确。", false, "确定", null);
                    MessageBox.Show("计算属性最大值或最小值失败,请检查数据是否正确", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return false;
                }

                pProportionalSymbolRenderer.ValueUnit = esriUnits.esriUnknownUnits;
                pProportionalSymbolRenderer.Field = FieldText1;
                pProportionalSymbolRenderer.FlanneryCompensation = false;
                //最小值不能小于等于0
                if (pStatisticsResult.Minimum <= 0)
                {
                    pProportionalSymbolRenderer.MinDataValue = 1;
                }
                else
                {
                    pProportionalSymbolRenderer.MinDataValue = pStatisticsResult.Minimum;
                }
                //最大值不能小于等于0
                if (pStatisticsResult.Maximum <= 0)
                {
                    pProportionalSymbolRenderer.MaxDataValue = 1;
                }
                else
                {
                    pProportionalSymbolRenderer.MaxDataValue = pStatisticsResult.Maximum;
                }
                pProportionalSymbolRenderer.BackgroundSymbol = (IFillSymbol)m_ProportionalBackSymbol;
                pProportionalSymbolRenderer.MinSymbol = m_ProportionalMinSymbol;
            }
            else if (m_Renderer is IChartRenderer)
            {
                //生成图表符号
                IChartRenderer pChartRenderer = new ChartRendererClass();
                pChartRenderer = (IChartRenderer)m_Renderer;

                IRendererFields pRendererFields;
                pRendererFields = (IRendererFields)pChartRenderer;

                pFeatureClass = m_FeatureLayer.FeatureClass;
                pTable = (ITable)pFeatureClass;
                //显示的是字段的别名，使用时要用字段名
                for (i = 0; i <= this.dgvBarFields.RowCount - 1; i++)
                {
                    pRendererFields.AddField(pFeatureClass.Fields.get_Field(pFeatureClass.Fields.FindFieldByAliasName(this.dgvBarFields.Rows[i].Cells[1].Value.ToString())).Name, this.dgvBarFields.Rows[i].Cells[1].Value.ToString());
                    pQueryFilter = new QueryFilterClass();
                    pQueryFilter.AddField(pFeatureClass.Fields.get_Field(pFeatureClass.Fields.FindFieldByAliasName(this.dgvBarFields.Rows[i].Cells[1].Value.ToString())).Name);
                    pCursor = pTable.Search(pQueryFilter, true);
                    pDataStatistics = new DataStatisticsClass();
                    pDataStatistics.Cursor = pCursor;
                    pDataStatistics.Field = pFeatureClass.Fields.get_Field(pFeatureClass.Fields.FindFieldByAliasName(this.dgvBarFields.Rows[i].Cells[1].Value.ToString())).Name;
                    pStatisticsResult = pDataStatistics.Statistics;

                    if (pStatisticsResult == null)
                    {
                        //ModDeclare.g_ErrorHandler.DisplayInformation("计算属性最大值或最小值失败,请检查数据是否正确。", false, "确定", null);
                        MessageBox.Show("计算属性最大值或最小值失败,请检查数据是否正确。", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        return false;
                    }

                    if (MaxValue < pStatisticsResult.Maximum)
                    {
                        MaxValue = pStatisticsResult.Maximum;
                    }
                }
                if (MaxValue <= 0)
                {
                    //ModDeclare.g_ErrorHandler.DisplayInformation("计算选定的属性的最大值失败或最大值为0,请检查数据是否正确。", false, "确定", null);
                    MessageBox.Show("计算选定的属性的最大值失败或最大值为0,请检查数据是否正确。", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    return false;
                }
                IChartSymbol pChartSymbol;
                IMarkerSymbol pMarkerSymbol;
                ISymbolArray pSymbolArray;
                IFillSymbol pFillSymbol;
                IColor pColor;

                switch (this.TvwRenderType.SelectedNode.Name)
                {
                    case "Pie":
                        IPieChartSymbol pPieChartSymbol;
                        pPieChartSymbol = new PieChartSymbolClass();

                        pChartSymbol = (IChartSymbol)pPieChartSymbol;
                        pMarkerSymbol = (IMarkerSymbol)pPieChartSymbol;
                        pSymbolArray = (ISymbolArray)pPieChartSymbol;
                        break;
                    case "Bar":
                        IBarChartSymbol pBarChartSymbol;
                        pBarChartSymbol = new BarChartSymbolClass();
                        pBarChartSymbol.Width = 6;

                        pChartSymbol = (IChartSymbol)pBarChartSymbol;
                        pMarkerSymbol = (IMarkerSymbol)pBarChartSymbol;
                        pSymbolArray = (ISymbolArray)pBarChartSymbol;
                        break;
                    case "Stack":
                        IStackedChartSymbol pStackedChartSymbol;
                        pStackedChartSymbol = new StackedChartSymbolClass();
                        pStackedChartSymbol.Width = 6;

                        pChartSymbol = (IChartSymbol)pStackedChartSymbol;
                        pMarkerSymbol = (IMarkerSymbol)pStackedChartSymbol;
                        pSymbolArray = (ISymbolArray)pStackedChartSymbol;
                        break;
                    default:
                        return false;
                }

                pChartSymbol.MaxValue = MaxValue;
                pMarkerSymbol.Size = (double)this.udBarSize.Value;

                for (i = 0; i <= dgvBarFields.Rows.Count - 1; i++)
                {
                    pFillSymbol = (IFillSymbol)this.dgvBarFields.Rows[i].Tag;
                    pColor = pFillSymbol.Color;
                    pSymbolArray.AddSymbol((ISymbol)pFillSymbol);
                }

                pChartRenderer.ChartSymbol = pChartSymbol;

                if (m_ChartSymbol != null)
                {
                    pChartRenderer.BaseSymbol = m_ChartSymbol;
                }

                if (ChkBarCover.Checked == false)
                {
                    pChartRenderer.UseOverposter = false;
                }
                else
                {
                    pChartRenderer.UseOverposter = true;
                }

                pChartRenderer.BaseSymbol = (ISymbol)this.PicBackGround.Tag;

            }
            else
            {
                return false;
            }

            IGeoFeatureLayer pGeoFeatureLayer;
            pGeoFeatureLayer = (IGeoFeatureLayer)m_FeatureLayer;
            pGeoFeatureLayer.Renderer = m_Renderer;

            return true;
        }
        //*********************************************************************************
        //** 函 数 名：FunAddFieldToCombo
        //** 输    入：   ByVal pFeatureLayer    ByRef Cmb
        //** 输    出：
        //** 功能描述：加载字段到Combo
        //** 全局变量：
        //** 调用模块：
        //** 作    者：steve
        //** 日    期：2005-06-09
        //** 修 改 者：steve
        //** 日    期：2005-06-09
        //** 版    本：1.0
        //*********************************************************************************
        private void FunAddFieldToCombo(IFeatureLayer pFeatureLayer, ComboBoxEx Cmb, string vFieldType)
        {
            IFields pFlds;
            IFeatureClass pFeatureClass;
            ClsField pField;

            pFeatureClass = pFeatureLayer.FeatureClass;
            pFlds = pFeatureClass.Fields;

            IField pFld;
            int i;
            string ShapeFieldName, OidFieldName;

            ShapeFieldName = pFeatureClass.ShapeFieldName;
            OidFieldName = pFeatureClass.OIDFieldName;
            Cmb.Items.Clear();

            vFieldType = Strings.UCase(vFieldType);
            Cmb.Items.Add("NULL");

            for (i = 0; i <= pFlds.FieldCount - 1; i++)
            {
                pFld = pFlds.get_Field(i);
                if ((pFld.Name != ShapeFieldName) && (pFld.Name != OidFieldName))
                {
                    switch (pFlds.get_Field(i).Type)
                    {
                        case esriFieldType.esriFieldTypeString:
                            if ((vFieldType == "STR") || (vFieldType == ""))
                            {
                                pField = new ClsField();
                                pField.Name = pFlds.get_Field(i).Name;
                                pField.AliasName = pFlds.get_Field(i).AliasName;
                                Cmb.Items.Add(pField);
                            }
                            break;
                        case esriFieldType.esriFieldTypeSmallInteger:
                        case esriFieldType.esriFieldTypeInteger:
                        case esriFieldType.esriFieldTypeSingle:
                        case esriFieldType.esriFieldTypeDouble:
                            if ((vFieldType == "NUM") || (vFieldType == ""))
                            {
                                pField = new ClsField();
                                pField.Name = pFlds.get_Field(i).Name;
                                pField.AliasName = pFlds.get_Field(i).AliasName;
                                Cmb.Items.Add(pField);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            Cmb.DisplayMember = "AliasName";
        }

        //*********************************************************************************
        //** 函 数 名：GetAliasNameByName
        //** 输    入：   ByVal mText    ByVal pFeatureLayer
        //** 输    出：
        //** 功能描述：通过字段名称得到字段的别名
        //** 全局变量：
        //** 调用模块：
        //*********************************************************************************
        private string GetAliasNameByName(string mText, IFeatureLayer pFeatureLayeer)
        {
            IFields pFields;
            IFeatureClass pFeatureClass;
            pFeatureClass = pFeatureLayeer.FeatureClass;
            if (pFeatureClass != null)
            {
                pFields = pFeatureClass.Fields;
                int l;
                for (l = 0; l <= pFields.FieldCount - 1; l++)
                {
                    if (pFields.get_Field(l).Name == mText)
                    {
                        return pFields.get_Field(l).AliasName;
                    }
                }
            }
            return "";
        }

        //检查当前GroupBox中是否有ImageCombo存在，是则删除，否则会出现点击颜色方案不响应的情况
        private void CheckImageCombo(GroupBox pGroupBox)
        {
            int i;
            for (i = 0; i <= pGroupBox.Controls.Count - 1; i++)
            {
                if (pGroupBox.Controls[i] is ImageCombo)
                {
                    pGroupBox.Controls[i].Dispose();
                }
            }
        }

        //创建ImageCombo的实例
        private ImageCombo CreateImageCombo(string vType)
        {
            ImageCombo pImageCombo = null;

            switch (vType)
            {
                case "UniqueSymbol":
                    CheckImageCombo(this.GroupBox12);
                    m_UniqueSymboImageCombo = new ImageCombo();
                    this.GroupBox12.Controls.Add(m_UniqueSymboImageCombo);
                    m_UniqueSymboImageCombo.Left = 6;
                    m_UniqueSymboImageCombo.Top = 12;
                    m_UniqueSymboImageCombo.Width = 150;
                    m_UniqueSymboImageCombo.Height = 22;

                    pImageCombo = m_UniqueSymboImageCombo;

                    this.ComboBoxEx5.Visible = false;
                    break;
                case "UniqueSymbolManyValue":
                    CheckImageCombo(this.GroupBox14);
                    m_UniqueSymbolManyValueImageCombo = new ImageCombo();
                    this.GroupBox14.Controls.Add(m_UniqueSymbolManyValueImageCombo);
                    m_UniqueSymbolManyValueImageCombo.Left = 6;
                    m_UniqueSymbolManyValueImageCombo.Top = 12;
                    m_UniqueSymbolManyValueImageCombo.Width = 150;
                    m_UniqueSymbolManyValueImageCombo.Height = 22;

                    pImageCombo = m_UniqueSymbolManyValueImageCombo;
                    this.CmbURMColor.Visible = false;
                    break;
                case "GraduatedColor":
                    CheckImageCombo(this.GroupBox18);
                    m_GraduatedColor = new ImageCombo();
                    this.GroupBox18.Controls.Add(m_GraduatedColor);
                    m_GraduatedColor.Left = 6;
                    m_GraduatedColor.Top = 12;
                    m_GraduatedColor.Width = 150;
                    m_GraduatedColor.Height = 22;

                    pImageCombo = m_GraduatedColor;
                    this.CmbGCColor.Visible = false;
                    break;
                case "BarColor":
                    CheckImageCombo(this.GroupBox20);
                    m_BarColor = new ImageCombo();
                    this.GroupBox20.Controls.Add(m_BarColor);
                    m_BarColor.Left = 6;
                    m_BarColor.Top = 12;
                    m_BarColor.Width = 150;
                    m_BarColor.Height = 22;

                    pImageCombo = m_BarColor;
                    this.CmbBarColor.Visible = false;
                    break;
                default:
                    break;
            }
            return pImageCombo;
        }

        //读取颜色方案
        private void InitialComboBox(string vPath, ImageCombo pImageCombo)
        {
            ClsImageComboItem ImageComboItem;
            string strTextLine;
            TextReader pTextReader;
            int Count = 0;
            string keyStr;
            int i;
            Image pImage;
            Graphics pGraphics;
            string[] pStrings;
            Color pColor;
            Color[] aColor = null;
            int Width;

            m_ColorScheme.Clear();
            m_ColorName.Clear();
            this.ImageList1.Images.Clear();
            pImageCombo.ImageList = this.ImageList1;
            pImageCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            pTextReader = new StreamReader(vPath);
            strTextLine = pTextReader.ReadLine();
            pImage = new Bitmap(this.PicDraw.Width, this.PicDraw.Height);
            pGraphics = Graphics.FromImage(pImage);

            while (strTextLine != null)
            {
                if (Strings.UCase(Strings.Left(strTextLine, 4)) == "RAMP")
                {
                    if (Count > 1)
                    {
                        Width = PicDraw.Width / Count - 1;
                        for (i = 0; i <= Count - 1; i++)
                        {
                            pGraphics.FillRectangle(new SolidBrush(aColor[i]), new Rectangle(i * Width, 0, Width, PicDraw.Height));
                        }
                        keyStr = Strings.Right(strTextLine, Strings.Len(strTextLine) - 5).Trim();
                        this.ImageList1.Images.Add(pImage);

                        m_ColorScheme.Add(aColor, keyStr, null, null);
                        m_ColorName.Add(keyStr, null, null, null);
                        Count = 0;

                        aColor = null;
                        pImage = new Bitmap(this.PicDraw.Width, this.PicDraw.Height);
                        pGraphics = Graphics.FromImage(pImage);
                    }
                    else
                    {
                        keyStr = Strings.Right(strTextLine, Strings.Len(strTextLine) - 5).Trim();

                        m_ColorName.Add(keyStr, null, null, null);
                    }
                }
                else
                {
                    pStrings = strTextLine.Split(new char[] { ',' });
                    pColor = Color.FromArgb(Convert.ToInt32(pStrings[1].Trim()), Convert.ToInt32(pStrings[2].Trim()), Convert.ToInt32(pStrings[3].Trim()));
                    //aColor = new Color[Count + 1];
                    System.Array.Resize(ref aColor, Count + 1);
                    aColor[Count] = pColor;
                    Count = Count + 1;
                }
                strTextLine = pTextReader.ReadLine();
            }

            for (i = 0; i <= this.ImageList1.Images.Count - 1; i++)
            {
                ImageComboItem = new ClsImageComboItem();
                ImageComboItem.Text = m_ColorName[i + 1].ToString();
                ImageComboItem.ImageIndex = i;
                pImageCombo.Items.Add(ImageComboItem);
            }
            pImageCombo.DisplayMember = "Text";
            pImageCombo.SelectedIndex = 0;

            ImageComboItem = null;
            pTextReader.Close();
            pTextReader = null;
            pImage = null;
            pGraphics = null;
            //pColor = null;
            aColor = null;
        }

        //添加值
        private void CmdURAdd_Click(object sender, EventArgs e)
        {
            IFeatureClass pFeatureClass;
            pFeatureClass = m_FeatureLayer.FeatureClass;

            IQueryFilter pQueryFilter = null;
            string OIDName, FeatureClassName, valueName;
            int i;
            IDataset pDataSet;
            pDataSet = (IDataset)pFeatureClass;

            OIDName = pFeatureClass.OIDFieldName;
            FeatureClassName = pDataSet.Name;
            valueName = CmbUniqueValueField.Text;

            IDataStatistics pDataStat;
            IEnumerator pEnumVar;               //字段唯一值的集合
            IFeatureCursor pFeatCursor;

            pFeatCursor = m_FeatureLayer.Search(null, false);
            pDataStat = new DataStatisticsClass();
            int lCount;

            valueName = GetItemData(CmbUniqueValueField, m_FeatureLayer);
            if (valueName == "")
            {
                return;
            }
            pDataStat.Field = valueName;
            pDataStat.Cursor = (ICursor)pFeatCursor;
            pEnumVar = pDataStat.UniqueValues;        //得到所有唯一值
            lCount = pDataStat.UniqueValueCount;
            if (lCount == 0)
            {
                return;
            }
            FeatureClassName = pDataSet.Name;

            int FeatureCount = lCount;

            IEnumColors pEnumColors;
            ISymbol pSymbol;
            IRgbColor pRGBColor;
            Color[] aColor;
            aColor = (Color[])m_ColorScheme[m_UniqueSymboImageCombo.SelectedIndex + 1];

            string[] ValueList = null;

            //找出所有未在列表中出现的值
            string pCurStr;
            bool bIsExist = false;
            int ValueCount = 0;
            int RowCount;
            pEnumVar.MoveNext();
            pCurStr = pEnumVar.Current.ToString();

            while (pCurStr != null)
            {
                for (i = 0; i <= this.dgvUR.Rows.Count - 1; i++)
                {
                    if (this.dgvUR.Rows[i].Cells[1].Value.ToString() == pCurStr)
                    {
                        bIsExist = true;
                    }
                }
                if (bIsExist == false)
                {
                    //ValueList = new string[ValueCount];
                    System.Array.Resize(ref ValueList, ValueCount + 1);
                    ValueList[ValueCount] = pCurStr;
                    ValueCount = ValueCount + 1;
                }

                pEnumVar.MoveNext();
                if (pEnumVar.Current != null)
                {
                    pCurStr = pEnumVar.Current.ToString();
                }
                else
                {
                    pCurStr = null;
                }

                bIsExist = false;
            }

            //下面是加值到列表中
            FrmRendererAddValue frm = new FrmRendererAddValue(ValueList);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.ReturnValues.Length != 0)
                {
                    pEnumColors = ReadEnumColor(aColor, this.dgvUR.Rows.Count + frm.ReturnValues.Length);
                    pQueryFilter = new QueryFilterClass();
                    for (i = 0; i <= frm.ReturnValues.Length - 1; i++)
                    {
                        pRGBColor = (IRgbColor)pEnumColors.Next();
                        pSymbol = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, pRGBColor);
                        this.dgvUR.Rows.Add();
                        RowCount = this.dgvUR.Rows.Count;
                        this.dgvUR.Rows[RowCount - 1].Tag = pSymbol;
                        this.dgvUR.Rows[RowCount - 1].Cells[0].Value = CreatePictureFromSymbol(pSymbol, lPicWidth, lPicHeight);
                        this.dgvUR.Rows[RowCount - 1].Cells[1].Value = frm.ReturnValues[i];
                        this.dgvUR.Rows[RowCount - 1].Cells[2].Value = frm.ReturnValues[i];
                        pQueryFilter.WhereClause = GetWhereClause(m_FeatureLayer.FeatureClass, valueName, frm.ReturnValues[i]);
                        this.dgvUR.Rows[RowCount - 1].Cells[3].Value = m_FeatureLayer.FeatureClass.FeatureCount(pQueryFilter);
                    }
                }
            }
        }

        private IEnumColors ReadEnumColor(Color[] aColor, int size)
        {
            IPresetColorRamp pColorRamp;
            int i;
            IColor pColor;
            IRgbColor pRGBColor;
            bool ICreate = true;

            pColorRamp = new PresetColorRampClass();
            if (size == 0)
            {
                pColorRamp.Size = 13;
            }
            else
            {
                pColorRamp.Size = size;
            }

            for (i = 0; i <= aColor.Length - 1; i++)
            {
                pRGBColor = new RgbColorClass();
                pRGBColor.Red = aColor[i].R;
                pRGBColor.Green = aColor[i].G;
                pRGBColor.Blue = aColor[i].B;
                pColor = pRGBColor;
                pColorRamp.set_PresetColor(i, pColor);
            }
            pColorRamp.CreateRamp(out ICreate);

            return pColorRamp.Colors;

        }

        //*********************************************************************************
        //** 函 数 名：GetItemData
        //** 输    入：   ByVal Cmb    ByVal pFeatureCl
        //** 输    出：
        //** 功能描述：通过ComboBox中选中的别名得到字段名称
        //** 全局变量：
        //** 调用模块：
        //*********************************************************************************
        private string GetItemData(ComboBox Cmb, IFeatureLayer pFeatureLayer)
        {
            IFields pFields;
            IFeatureClass pFeatureClass;
            ClsField pField;

            pFeatureClass = pFeatureLayer.FeatureClass;
            if (pFeatureClass == null)
            {
                return "";
            }
            pFields = pFeatureClass.Fields;

            int l = -1;
            l = Cmb.SelectedIndex;

            if (Cmb.Text == "NULL")
            {
                return "";
            }
            if (l != -1)
            {
                pField = (ClsField)Cmb.SelectedItem;
                return pField.Name;
            }
            return "";
        }

        private void dgvUR_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)       //点击Symbol列
            {
                FrmSymbolManager FrmSymbol = new FrmSymbolManager(FrmSymbolManager.UseType.General);
                ISymbol pSymbol = (ISymbol)this.dgvUR.Rows[e.RowIndex].Tag;
                if (pSymbol == null)
                {
                    pSymbol = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, null);
                }

                FrmSymbol.m_InSymbol = pSymbol;
                FrmSymbol.m_SymbolType = m_SymbolType;
                if (FrmSymbol.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pSymbol = FrmSymbol.CurSymbol;
                    this.dgvUR.Rows[e.RowIndex].Tag = pSymbol;
                    this.dgvUR.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = CreatePictureFromSymbol(pSymbol, lPicWidth, lPicHeight);
                }
            }
        }

        private void m_UniqueSymboImageCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //改变单值分级符号的符号颜色
            int l;
            int FeatureCount;
            FeatureCount = this.dgvUR.Rows.Count;

            if (FeatureCount == 0)
            {
                return;
            }
            IEnumColors pEnumColors;
            ISymbol pSymbol;
            IRgbColor pRGBColor;
            Color[] aColor;
            aColor = (Color[])m_ColorScheme[m_UniqueSymboImageCombo.SelectedIndex + 1];
            pEnumColors = ReadEnumColor(aColor, FeatureCount);

            for (l = 0; l <= FeatureCount - 1; l++)
            {
                pSymbol = (ISymbol)this.dgvUR.Rows[l].Tag;
                pRGBColor = (IRgbColor)pEnumColors.Next();
                SetASymbolColor(pSymbol, pRGBColor);
                this.dgvUR.Rows[l].Cells[0].Value = CreatePictureFromSymbol(pSymbol, lPicWidth, lPicHeight);
            }

        }

        //*********************************************************************************
        //** 函 数 名：SetASymbolColor
        //** 输    入：   ByVal pSymbol    ByVal aColor
        //** 输    出：
        //** 功能描述：设置符号颜色
        //** 全局变量：
        //** 调用模块：
        //*********************************************************************************
        private void SetASymbolColor(ISymbol pSymbol, IColor aColor)
        {
            if (pSymbol is ISimpleFillSymbol)
            {
                ISimpleFillSymbol pFillSymbol;
                pFillSymbol = (ISimpleFillSymbol)pSymbol;
                pFillSymbol.Color = aColor;
            }
            else if (pSymbol is ILineSymbol)
            {
                ILineSymbol pLineSymbol;
                pLineSymbol = (ILineSymbol)pSymbol;
                pLineSymbol.Color = aColor;
            }
            else if (pSymbol is IMarkerSymbol)
            {
                IMarkerSymbol pMarkerSymbol;
                pMarkerSymbol = (IMarkerSymbol)pSymbol;
                pMarkerSymbol.Color = aColor;
            }
        }

        //添加所有值
        private void CmdURAddAllValue_Click(object sender, EventArgs e)
        {
            IFeatureClass pFeatureClass;
            pFeatureClass = m_FeatureLayer.FeatureClass;
            IQueryFilter pQueryFilter = null;
            IFeatureCursor pFeatureCursor;
            IFeature pFeature;
            string OIDName, FeatureClassName, valueName;
            int lCount;
            IDataset pDataSet;
            pDataSet = (IDataset)pFeatureClass;

            IDataStatistics pDataStat;
            IEnumerator pEnumVar;               //字段唯一值的集合
            object pValue;
            IFeatureCursor pFeatCursor = null;

            try
            {
                pFeatureCursor = m_FeatureLayer.Search(pQueryFilter, false);
                pDataStat = new DataStatisticsClass();

                valueName = GetItemData(CmbUniqueValueField, m_FeatureLayer);
                if (valueName == "")
                {
                    return;
                }
                pDataStat.Field = valueName;
                pDataStat.Cursor = (ICursor)pFeatureCursor;
                pEnumVar = pDataStat.UniqueValues;       //得到所有唯一值
                lCount = pDataStat.UniqueValueCount;
                if (lCount == 0)
                {
                    return;
                }
                FeatureClassName = pDataSet.Name;

                int FeatureCount;
                FeatureCount = lCount;

                IEnumColors pEnumColors;
                ISymbol pSymbol;
                IRgbColor pRGBColor;
                Color[] aColor;
                aColor = (Color[])m_ColorScheme[this.m_UniqueSymboImageCombo.SelectedIndex + 1];
                pEnumColors = ReadEnumColor(aColor, FeatureCount);

                ////代码屏蔽部分是用作显示要素的数量，由于统计比较慢，暂时屏蔽
                //ICursor pCuror;
                //IDataStatistics pData;
                //IStatisticsResults pStatResults;
                //Set pData = new DataStatisticsClass();

                pQueryFilter = new QueryFilterClass();

                pEnumVar.MoveNext();
                string pCurStr;
                pCurStr = pEnumVar.Current.ToString();
                int RowCount = 0;

                this.dgvUR.Rows.Clear();
                this.progressbarRenderer.Visible = true;
                this.progressbarRenderer.Minimum = 0;
                this.progressbarRenderer.Maximum = FeatureCount - 1;

                while (pCurStr != null)
                {
                    this.dgvUR.Rows.Add();
                    pRGBColor = (IRgbColor)pEnumColors.Next();
                    this.dgvUR.Rows[RowCount].Tag = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, pRGBColor);
                    this.dgvUR.Rows[RowCount].Cells[0].Value = CreatePictureFromSymbol((ISymbol)this.dgvUR.Rows[RowCount].Tag, lPicWidth, lPicHeight);
                    this.dgvUR.Rows[RowCount].Cells[1].Value = pCurStr;
                    this.dgvUR.Rows[RowCount].Cells[2].Value = pCurStr;
                    pQueryFilter.WhereClause = GetWhereClause(m_FeatureLayer.FeatureClass, valueName, pCurStr);
                    this.dgvUR.Rows[RowCount].Cells[3].Value = m_FeatureLayer.FeatureClass.FeatureCount(pQueryFilter);

                    pEnumVar.MoveNext();
                    //if (pEnumVar.Current != null)
                    //{
                    if (pEnumVar.Current != null)
                    {
                        pCurStr = pEnumVar.Current.ToString();
                    }
                    else
                    {
                        pCurStr = null;
                    }

                    this.progressbarRenderer.Value = RowCount;
                    RowCount = RowCount + 1;
                    //}

                }

                this.progressbarRenderer.Visible = false;
                return;

            }
            catch (Exception ex)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation(ex.Message, false, null, null);
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }

        }

        private void CmbUniqueValueField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvUR.Rows.Clear();
        }

        //移除
        private void CmdURRemove_Click(object sender, EventArgs e)
        {
            int i;
            if (this.dgvUR.SelectedRows.Count != 0)
            {
                for (i = this.dgvUR.SelectedRows.Count - 1; i >= 0; i--)
                {
                    this.dgvUR.Rows.Remove(this.dgvUR.SelectedRows[i]);
                }
            }
        }

        //全部移除
        private void CmdURRemoveAll_Click(object sender, EventArgs e)
        {
            if (this.dgvUR.Rows.Count != 0)
            {
                this.dgvUR.Rows.Clear();
            }
        }

        private void CmdURAdvance_PopupOpen(object sender, EventArgs e)
        {
            switch (m_SymbolType)
            {
                case "点状地物符号":
                    this.btnURTrans.Visible = false;
                    btnURRotate.Visible = true;
                    break;
                case "线状地物符号":
                    //ModDeclare.g_ErrorHandler.DisplayInformation("线状符号无该设置", false, null, null);
                    MessageBox.Show("线状符号无该设置", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    btnURTrans.Visible = false;
                    btnURRotate.Visible = false;
                    return;
                case "面状地物符号":
                    btnURTrans.Visible = true;
                    btnURRotate.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void dgvURM_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                //选择符号
                FrmSymbolManager frm = new FrmSymbolManager(FrmSymbolManager.UseType.General);
                ISymbol pSymbol;

                pSymbol = (ISymbol)this.dgvURM.Rows[e.RowIndex].Tag;
                frm.m_InSymbol = pSymbol;
                frm.m_SymbolType = m_SymbolType;

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.dgvURM.Rows[e.RowIndex].Tag = frm.CurSymbol;
                    this.dgvURM.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = CreatePictureFromSymbol((ISymbol)this.dgvURM.Rows[e.RowIndex].Tag, lPicWidth, lPicHeight);
                }
                frm = null;
            }
        }

        private void m_UniqueSymbolManyValueImageCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //改变多值分级符号的符号颜色
            int l;
            int FeatureCount;
            FeatureCount = this.dgvURM.RowCount;

            if (FeatureCount == 0)
            {
                return;
            }
            IEnumColors pEnumColors;
            ISymbol pSymbol;
            IRgbColor pRGBColor;
            Color[] aColor;
            aColor = (Color[])m_ColorScheme[m_UniqueSymbolManyValueImageCombo.SelectedIndex + 1];

            pEnumColors = ReadEnumColor(aColor, FeatureCount);

            for (l = 0; l <= dgvURM.Rows.Count - 1; l++)
            {
                pRGBColor = (IRgbColor)pEnumColors.Next();
                pSymbol = (ISymbol)this.dgvURM.Rows[l].Tag;
                if (pSymbol == null)
                {
                    continue;
                }
                else
                {
                    SetASymbolColor(pSymbol, pRGBColor);
                    this.dgvURM.Rows[l].Cells[0].Value = CreatePictureFromSymbol(pSymbol, lPicWidth, lPicHeight);
                }
            }

        }

        //添加值
        private void CmdURMAdd_Click(object sender, EventArgs e)
        {
            IFeatureClass pFeatureClass;
            pFeatureClass = m_FeatureLayer.FeatureClass;
            IQueryFilter pQueryFilter;
            int l, m, i;
            IDataset pDataSet;
            pDataSet = (IDataset)pFeatureClass;

            IEnumerator pEnumVar1;              //字段唯一值的集合
            IEnumerator pEnumVar2;              //字段唯一值的集合
            IEnumerator pEnumVar3;              //字段唯一值的集合

            IFeatureCursor pFeatCursor1;
            IFeatureCursor pFeatCursor2;
            IFeatureCursor pFeatCursor3;

            pFeatCursor1 = m_FeatureLayer.Search(null, false);

            int lCount;

            string pValueName1;
            string pValueName2;
            string pValueName3;

            pValueName1 = GetItemData(this.CmbURMField1, m_FeatureLayer);
            pValueName2 = GetItemData(this.CmbURMField2, m_FeatureLayer);
            pValueName3 = GetItemData(this.CmbURMField3, m_FeatureLayer);

            IDataStatistics pDataStat1;
            IDataStatistics pDataStat2;
            IDataStatistics pDataStat3;

            lCount = 0;
            string[] ValueList = null;
            string pValue1;
            string pValue2;
            string pValue3;

            if (pValueName1 != "")
            {
                pDataStat1 = new DataStatisticsClass();
                pDataStat1.Field = pValueName1;
                pDataStat1.Cursor = (ICursor)pFeatCursor1;
                pEnumVar1 = pDataStat1.UniqueValues;       //得到所有唯一值

                pEnumVar1.MoveNext();
                pValue1 = pEnumVar1.Current.ToString();

                while (pValue1 != null)
                {
                    if (pValueName2 != "")
                    {
                        pDataStat2 = new DataStatisticsClass();
                        pDataStat2.Field = pValueName2;
                        pQueryFilter = new QueryFilterClass();

                        pQueryFilter.WhereClause = GetWhereClause(pFeatureClass, pValueName1, pValue1);
                        pFeatCursor2 = pFeatureClass.Search(pQueryFilter, false);

                        pDataStat2.Cursor = (ICursor)pFeatCursor2;
                        pEnumVar2 = pDataStat2.UniqueValues;        //得到所有唯一值
                        pEnumVar2.MoveNext();
                        pValue2 = pEnumVar2.Current.ToString();

                        while (pValue2 != null)
                        {
                            if (pValueName3 != "")
                            {
                                pDataStat3 = new DataStatisticsClass();
                                pDataStat3.Field = pValueName3;
                                pQueryFilter = new QueryFilterClass();
                                pQueryFilter.WhereClause = GetWhereClause(pFeatureClass, pValueName1, pValue1) + " and " + GetWhereClause(pFeatureClass, pValueName1, pValue1);
                                pFeatCursor3 = m_FeatureLayer.Search(pQueryFilter, false);
                                pDataStat3.Cursor = (ICursor)pFeatCursor3;
                                pEnumVar3 = pDataStat3.UniqueValues;        //得到所有唯一值
                                pEnumVar3.MoveNext();
                                pValue3 = pEnumVar3.Current.ToString();

                                while (pValue3 != null)
                                {
                                    //ValueList = new string[lCount];
                                    System.Array.Resize(ref ValueList, lCount + 1);
                                    ValueList[lCount] = pValue1 + ", " + pValue2 + ", " + pValue3;
                                    lCount = lCount + 1;
                                    pEnumVar3.MoveNext();
                                    pValue3 = pEnumVar3.Current.ToString();
                                }
                            }
                            else            //只有字段1和字段2的情况
                            {
                                //ValueList = new string[lCount];
                                System.Array.Resize(ref ValueList, lCount + 1);
                                ValueList[lCount] = pValue1 + ", " + pValue2;
                                lCount = lCount + 1;
                            }
                            pEnumVar2.MoveNext();
                            pValue2 = pEnumVar2.Current.ToString();
                        }
                    }
                    else                        //只有字段1的情况
                    {
                        //ValueList = new string[lCount];
                        System.Array.Resize(ref ValueList, lCount + 1);
                        ValueList[lCount] = pValue1;
                        lCount = lCount + 1;
                    }
                    pEnumVar1.MoveNext();
                    pValue1 = pEnumVar1.Current.ToString();
                }
            }
            int FeatureCount;
            FeatureCount = lCount;

            if (FeatureCount == 0)
            {
                return;
            }
            IEnumColors pEnumColors;
            Color[] aColor;
            aColor = (Color[])m_ColorScheme[this.m_UniqueSymbolManyValueImageCombo.SelectedIndex + 1];

            string[] vValueList = null;
            bool bIsExist = false;
            int Count = 0;
            //找出所有未在列表中出现的值
            for (l = 0; l <= ValueList.Length - 1; l++)
            {
                for (m = 0; m <= this.dgvURM.Rows.Count - 1; m++)
                {
                    if (this.dgvURM.Rows[m].Cells[1].Value.ToString() == ValueList[l])
                    {
                        bIsExist = true;
                        break;
                    }
                }
                if (bIsExist == false)
                {
                    //vValueList = new string[Count];
                    System.Array.Resize(ref vValueList, Count + 1);
                    vValueList[Count] = ValueList[l];
                    Count = Count + 1;
                }
                bIsExist = false;
            }

            //下面是加值到列表中
            FrmRendererAddValue frm = new FrmRendererAddValue(vValueList);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.ReturnValues.Length != 0)
                {
                    pEnumColors = ReadEnumColor(aColor, FeatureCount + 1);
                    for (i = 0; i <= frm.ReturnValues.Length - 1; i++)
                    {
                        this.dgvURM.Rows.Add();
                        this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Tag = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, pEnumColors.Next());
                        this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Cells[0].Value = CreatePictureFromSymbol((ISymbol)this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Tag, lPicWidth, lPicHeight);
                        this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Cells[1].Value = frm.ReturnValues[i];
                        this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Cells[2].Value = frm.ReturnValues[i];
                    }
                }
            }

        }

        //添加所有值
        private void CmdURMAddAllValue_Click(object sender, EventArgs e)
        {
            if ((this.CmbURMField1.SelectedIndex == 0) && (CmbURMField2.SelectedIndex == 0) && (CmbURMField3.SelectedIndex == 0))
            {
                return;
            }
            //Utilities.FrmProgress pfrmProcess = new Utilities.FrmProgress();
            //pfrmProcess.PrgCaption = "正在读取数据中，请稍候...";
            //pfrmProcess.Show(this);
            System.Windows.Forms.Application.DoEvents();

            IFeatureClass pFeatureClass;
            pFeatureClass = m_FeatureLayer.FeatureClass;
            IQueryFilter pQueryFilter;
            string OIDName, FeatureClassName, ValueName1, ValueName2, ValueName3;
            int l;
            IDataset pDataSet;
            pDataSet = (IDataset)pFeatureClass;

            OIDName = pFeatureClass.OIDFieldName;
            FeatureClassName = pDataSet.Name;
            ValueName1 = CmbURMField1.Text;
            ValueName2 = CmbURMField2.Text;
            ValueName3 = CmbURMField3.Text;

            IEnumerator pEnumVar1;              //字段唯一值的集合
            IEnumerator pEnumVar2;              //字段唯一值的集合
            IEnumerator pEnumVar3;              //字段唯一值的集合

            IFeatureCursor pFeatCursor1;
            IFeatureCursor pFeatCursor2;
            IFeatureCursor pFeatCursor3;

            pFeatCursor1 = m_FeatureLayer.Search(null, false);

            int lCount;

            string pValueName1;
            string pValueName2;
            string pValueName3;

            pValueName1 = GetItemData(this.CmbURMField1, m_FeatureLayer);
            pValueName2 = GetItemData(this.CmbURMField2, m_FeatureLayer);
            pValueName3 = GetItemData(this.CmbURMField3, m_FeatureLayer);

            IDataStatistics pDataStat1;
            IDataStatistics pDataStat2;
            IDataStatistics pDataStat3;

            lCount = 0;
            string[] ValueList = null;
            string pValue1;
            string pValue2;
            string pValue3;

            if (pValueName1 != "")
            {
                pDataStat1 = new DataStatisticsClass();
                pDataStat1.Field = pValueName1;
                pDataStat1.Cursor = (ICursor)pFeatCursor1;
                pEnumVar1 = pDataStat1.UniqueValues;       //得到所有唯一值

                pEnumVar1.MoveNext();
                if (pEnumVar1.Current != null)
                {
                    pValue1 = pEnumVar1.Current.ToString();
                }
                else
                {
                    pValue1 = null;
                }

                while (pValue1 != null)
                {
                    if (pValueName2 != "")
                    {
                        pDataStat2 = new DataStatisticsClass();
                        pDataStat2.Field = pValueName2;
                        pQueryFilter = new QueryFilterClass();

                        pQueryFilter.WhereClause = GetWhereClause(pFeatureClass, pValueName1, pValue1);
                        pFeatCursor2 = pFeatureClass.Search(pQueryFilter, false);

                        pDataStat2.Cursor = (ICursor)pFeatCursor2;
                        pEnumVar2 = pDataStat2.UniqueValues;        //得到所有唯一值
                        pEnumVar2.MoveNext();
                        if (pEnumVar2.Current != null)
                        {
                            pValue2 = pEnumVar2.Current.ToString();
                        }
                        else
                        {
                            pValue2 = null;
                        }

                        while (pValue2 != null)
                        {
                            if (pValueName3 != "")
                            {
                                pDataStat3 = new DataStatisticsClass();
                                pDataStat3.Field = pValueName3;
                                pQueryFilter = new QueryFilterClass();
                                pQueryFilter.WhereClause = GetWhereClause(pFeatureClass, pValueName1, pValue1) + " and " + GetWhereClause(pFeatureClass, pValueName1, pValue1);
                                pFeatCursor3 = m_FeatureLayer.Search(pQueryFilter, false);
                                pDataStat3.Cursor = (ICursor)pFeatCursor3;
                                pEnumVar3 = pDataStat3.UniqueValues;        //得到所有唯一值
                                pEnumVar3.MoveNext();
                                if (pEnumVar3.Current != null)
                                {
                                    pValue3 = pEnumVar3.Current.ToString();
                                }
                                else
                                {
                                    pValue3 = null;
                                }

                                while (pValue3 != null)
                                {
                                    //ValueList = new string[lCount];
                                    System.Array.Resize(ref ValueList, lCount + 1);
                                    ValueList[lCount] = pValue1 + ", " + pValue2 + ", " + pValue3;
                                    lCount = lCount + 1;
                                    pEnumVar3.MoveNext();
                                    if (pEnumVar3.Current != null)
                                    {
                                       pValue3 = pEnumVar3.Current.ToString(); 
                                    }
                                    else
                                    {
                                        pValue3 = null;
                                    }
                                    
                                    Application.DoEvents();
                                }
                            }
                            else            //只有字段1和字段2的情况
                            {
                                //ValueList = new string[lCount];
                                System.Array.Resize(ref ValueList, lCount + 1);
                                ValueList[lCount] = pValue1 + ", " + pValue2;
                                lCount = lCount + 1;
                            }
                            pEnumVar2.MoveNext();
                            if (pEnumVar2.Current != null)
                            {
                                pValue2 = pEnumVar2.Current.ToString();
                            }
                            else
                            {
                                pValue2 = null;
                            }

                            Application.DoEvents();
                        }
                    }
                    else                        //只有字段1的情况
                    {
                        //ValueList = new string[lCount];
                        System.Array.Resize(ref ValueList, lCount + 1);
                        ValueList[lCount] = pValue1;
                        lCount = lCount + 1;
                    }
                    pEnumVar1.MoveNext();
                    if (pEnumVar1.Current != null)
                    {
                        pValue1 = pEnumVar1.Current.ToString();
                    }
                    else
                    {
                        pValue1 = null;
                    }
                    
                    Application.DoEvents();
                }
            }
            int FeatureCount;
            FeatureCount = lCount;

            if (FeatureCount == 0)
            {
                return;
            }
            IEnumColors pEnumColors;
            Color[] aColor;
            aColor = (Color[])m_ColorScheme[this.m_UniqueSymbolManyValueImageCombo.SelectedIndex + 1];
            pEnumColors = ReadEnumColor(aColor, FeatureCount);


            //下面是加值到列表中

            this.dgvURM.Rows.Clear();
            for (l = 0; l <= ValueList.Length - 1; l++)
            {
                this.dgvURM.Rows.Add();
                this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Tag = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, pEnumColors.Next());
                this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Cells[0].Value = CreatePictureFromSymbol((ISymbol)this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Tag, lPicWidth, lPicHeight);
                this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Cells[1].Value = ValueList[l];
                this.dgvURM.Rows[this.dgvURM.Rows.Count - 1].Cells[2].Value = ValueList[l];
                Application.DoEvents();
            }
            //pfrmProcess.Close();
            //pfrmProcess = null;

            return;
        }

        //移除
        private void CmdURMRemove_Click(object sender, EventArgs e)
        {
            int i;

            if (this.dgvURM.SelectedRows.Count != 0)
            {
                for (i = this.dgvURM.SelectedRows.Count - 1; i >= 0; i--)
                {
                    this.dgvURM.Rows.Remove(this.dgvURM.SelectedRows[i]);
                }
            }
        }

        private void CmdURMRemoveAll_Click(object sender, EventArgs e)
        {
            this.dgvURM.Rows.Clear();
        }

        private void CmdURMAdvance_PopupOpen(object sender, EventArgs e)
        {
            switch (m_SymbolType)
            {
                case "点状地物符号":
                    this.btnURMTrans.Visible = false;
                    btnURMRotate.Visible = true;
                    break;
                case "线状地物符号":
                    //ModDeclare.g_ErrorHandler.DisplayInformation("线状符号无该设置", false, null, null);
                    MessageBox.Show("线状符号无该设置", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    btnURMTrans.Visible = false;
                    btnURMRotate.Visible = false;
                    return;
                case "面状地物符号":
                    btnURMTrans.Visible = true;
                    btnURMRotate.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void CmbURMField1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvURM.Rows.Clear();
        }

        private void CmbURMField2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvURM.Rows.Clear();
        }

        private void CmbGCField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvGC.Rows.Clear();
        }

        private void m_GraduatedColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //改变分级颜色的颜色
            int l;
            int FeatureCount;
            FeatureCount = this.dgvGC.RowCount;

            if (FeatureCount == 0)
            {
                return;
            }
            IEnumColors pEnumColors;
            ISymbol pSymbol;
            IRgbColor pRGBColor;
            Color[] aColor;
            aColor = (Color[])m_ColorScheme[this.m_GraduatedColor.SelectedIndex + 1];
            pEnumColors = ReadEnumColor(aColor, FeatureCount);

            for (l = 0; l <= this.dgvGC.RowCount - 1; l++)
            {
                pRGBColor = (IRgbColor)pEnumColors.Next();
                pSymbol = (ISymbol)this.dgvGC.Rows[l].Tag;
                if (pSymbol == null)
                {
                    return;
                }
                else
                {
                    SetASymbolColor(pSymbol, pRGBColor);
                }
                this.dgvGC.Rows[l].Cells[0].Value = CreatePictureFromSymbol(pSymbol, lPicWidth, lPicHeight);
            }
        }

        private void CmbGCLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvGC.Rows.Clear();
        }

        //生成分级颜色列表
        private void btnGC_Click(object sender, EventArgs e)
        {
            IGeoFeatureLayer pGeoFeatureLayer;
            ITable pTable;
            IQueryFilter pQueryFilter;
            ICursor pCursor;
            IDataStatistics pDataStatistics;
            IStatisticsResults pStatisticsResult;

            pGeoFeatureLayer = (IGeoFeatureLayer)m_FeatureLayer;
            pTable = (ITable)m_FeatureLayer;

            pQueryFilter = new QueryFilterClass();
            pQueryFilter.AddField(this.CmbGCField.Text);

            pCursor = pTable.Search(pQueryFilter, true);

            pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Cursor = pCursor;
            string valueName;
            valueName = GetItemData(CmbGCField, m_FeatureLayer);
            if (valueName == "")
            {
                return;
            }
            pDataStatistics.Field = valueName;

            pStatisticsResult = pDataStatistics.Statistics;
            if (pStatisticsResult == null)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("计算属性最大值或最小值失败,请检查数据是否正确。", false, "确定", null);
                MessageBox.Show("计算属性最大值或最小值失败,请检查数据是否正确。", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }

            double pStep;
            pStep = (pStatisticsResult.Maximum - pStatisticsResult.Minimum) / Convert.ToInt64(CmbGCLevel.Text);

            IEnumColors pEnumColors;
            ISymbol pSymbol;
            IRgbColor pRGBColor;
            Color[] aColor;
            aColor = (Color[])m_ColorScheme[this.m_GraduatedColor.SelectedIndex + 1];
            pEnumColors = ReadEnumColor(aColor, Convert.ToInt32(CmbGCLevel.Text));

            this.dgvGC.Rows.Clear();

            int i;
            for (i = 0; i <= Convert.ToInt32(CmbGCLevel.Text) - 1; i++)
            {
                this.dgvGC.Rows.Add();
                pRGBColor = (IRgbColor)pEnumColors.Next();
                pSymbol = ClsFunctions.GetASymbolBySymbolType(m_SymbolType, pRGBColor);
                this.dgvGC.Rows[this.dgvGC.Rows.Count - 1].Tag = pSymbol;
                this.dgvGC.Rows[this.dgvGC.Rows.Count - 1].Cells[0].Value = CreatePictureFromSymbol(pSymbol, lPicWidth, lPicHeight);
                this.dgvGC.Rows[this.dgvGC.Rows.Count - 1].Cells[1].Value = pStatisticsResult.Minimum + pStep * (i + 1);
                this.dgvGC.Rows[this.dgvGC.Rows.Count - 1].Cells[2].Value = pStatisticsResult.Minimum + pStep * (i + 1);
            }
        }

        private void dgvGC_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                //选择符号
                FrmSymbolManager frm = new FrmSymbolManager(FrmSymbolManager.UseType.General);
                ISymbol pSymbol;
                pSymbol = (ISymbol)this.dgvGC.Rows[e.RowIndex].Tag;
                frm.m_InSymbol = pSymbol;
                frm.m_SymbolType = m_SymbolType;

                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.dgvGC.Rows[e.RowIndex].Tag = frm.CurSymbol;
                    this.dgvGC.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = CreatePictureFromSymbol(frm.CurSymbol, lPicWidth, lPicHeight);
                }
                frm = null;
            }
            else
            {
                if (e.ColumnIndex != 1)
                {
                    return;
                }
            }
        }

        private void CmbGSField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvGS.Rows.Clear();
        }

        private void CmbGSLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvGS.Rows.Clear();
        }

        //生成分级符号列表
        private void cmdGS_Click(object sender, EventArgs e)
        {
            if (m_GraduatedSymbol == null)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("标准符号设置有误!请设置标准符号!", false, "确定", null);
                MessageBox.Show("标准符号设置有误!请设置标准符号!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }

            if ((CmbGSField.Text == "") || (CmbGSLevel.Text == ""))
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("请选择分级字段!", false, "确定", null);
                MessageBox.Show("请选择分级字段!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }

            IGeoFeatureLayer pGeoFeatureLayer;
            ITable pTable;
            IQueryFilter pQueryFilter;
            ICursor pCursor;
            IDataStatistics pDataStatistics;
            IStatisticsResults pStatisticsResult;
            string strSymbolType = "";

            pGeoFeatureLayer = (IGeoFeatureLayer)m_FeatureLayer;
            pTable = (ITable)m_FeatureLayer;

            pQueryFilter = new QueryFilterClass();
            pQueryFilter.AddField(CmbGSField.Text);

            pCursor = pTable.Search(pQueryFilter, true);

            pDataStatistics = new DataStatisticsClass();
            pDataStatistics.Cursor = pCursor;

            string valueName;
            valueName = GetItemData(this.CmbGSField, m_FeatureLayer);
            if (valueName == "")
            {
                return;
            }
            pDataStatistics.Field = valueName;

            pStatisticsResult = pDataStatistics.Statistics;
            if (pStatisticsResult == null)
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("计算属性最大值或最小值失败,请检查数据是否正确。", false, "确定", null);
                MessageBox.Show("计算属性最大值或最小值失败,请检查数据是否正确。", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }

            double pStep;
            pStep = (pStatisticsResult.Maximum - pStatisticsResult.Minimum) / Convert.ToInt32(CmbGSLevel.Text);

            this.dgvGS.Rows.Clear();

            double MinSymbolSize;
            MinSymbolSize = Convert.ToDouble(txtMinSize.Text);
            double MaxSymbolSize;
            MaxSymbolSize = Convert.ToDouble(txtMaxSize.Text);
            double Step1;
            Step1 = (MaxSymbolSize - MinSymbolSize) / (Convert.ToDouble(CmbGSLevel.Text) - 1);
            double SymbolSize;
            SymbolSize = MinSymbolSize;

            int i;
            for (i = 0; i <= Convert.ToInt32(CmbGSLevel.Text) - 1; i++)
            {
                dgvGS.Rows.Add();

                ISymbol pSymbol = null;
                IClone pClone;
                switch (m_SymbolType)
                {
                    case "面状地物符号":
                    case "点状地物符号":
                        strSymbolType = "点状地物符号";
                        IMarkerSymbol pMarkerSymbol;
                        pMarkerSymbol = (IMarkerSymbol)m_GraduatedSymbol;

                        SymbolSize = MinSymbolSize + i * Step1;
                        pClone = (IClone)pMarkerSymbol;
                        pSymbol = (ISymbol)pClone.Clone();
                        pMarkerSymbol = (IMarkerSymbol)pSymbol;
                        pMarkerSymbol.Size = SymbolSize;
                        pSymbol = (ISymbol)pMarkerSymbol;
                        break;

                    case "线状地物符号":
                        strSymbolType = "线状地物符号";
                        ILineSymbol pLineSymbol;
                        pLineSymbol = (ILineSymbol)m_GraduatedSymbol;

                        SymbolSize = MinSymbolSize + i * Step1;

                        //SymbolElement.setAttribute "线宽", SymbolSize

                        pClone = (IClone)pLineSymbol;
                        pSymbol = (ISymbol)pClone.Clone();
                        pLineSymbol = (ILineSymbol)pSymbol;
                        pLineSymbol.Width = SymbolSize;
                        pSymbol = (ISymbol)pLineSymbol;
                        break;
                    default:
                        break;
                }

                this.dgvGS.Rows[this.dgvGS.Rows.Count - 1].Tag = pSymbol;
                this.dgvGS.Rows[this.dgvGS.Rows.Count - 1].Cells[0].Value = CreatePictureFromSymbol2(pSymbol, lPicWidth, lPicHeight, strSymbolType);
                this.dgvGS.Rows[this.dgvGS.Rows.Count - 1].Cells[1].Value = pStatisticsResult.Minimum + pStep * (i + 1);
                this.dgvGS.Rows[this.dgvGS.Rows.Count - 1].Cells[2].Value = pStatisticsResult.Minimum + pStep * (i + 1);
            }

        }

        private void PicPSym_Click(object sender, EventArgs e)
        {
            //选择标准符号
            FrmSymbolManager frm = new FrmSymbolManager(FrmSymbolManager.UseType.General);
            ISymbol pSymbol = null;
            string strSymbolType = "";
            if (this.dgvGS.Rows.Count != 0)
            {
                pSymbol = (ISymbol)this.dgvGS.Rows[0].Tag;
            }
            frm.m_InSymbol = pSymbol;
            switch (m_SymbolType)
            {
                case "面状地物符号":
                case "点状地物符号":
                    frm.m_SymbolType = "点状地物符号";
                    strSymbolType = "点状地物符号";
                    break;
                case "线状地物符号":
                    frm.m_SymbolType = "线状地物符号";
                    strSymbolType = "线状地物符号";
                    break;
                default:
                    break;
            }

            frm.ShowDialog();

            m_GraduatedSymbol = frm.CurSymbol;
            PicPSym.Image = CreatePictureFromSymbol2(m_GraduatedSymbol, PicPSym.Width, PicPSym.Height, strSymbolType);

        }

        private void PicSSym_Click(object sender, EventArgs e)
        {
            //选择背景符号
            FrmSymbolManager frm = new FrmSymbolManager(FrmSymbolManager.UseType.General);
            ISymbol pSymbol = null;
            if (this.dgvGS.Rows.Count != 0)
            {
                pSymbol = (ISymbol)this.dgvGS.Rows[0].Tag;
            }
            frm.m_InSymbol = pSymbol;

            frm.m_SymbolType = "面状地物符号";
            frm.ShowDialog();
            m_GraduatedBackSymbol = frm.CurSymbol;

            PicSSym.Image = CreatePictureFromSymbol2(m_GraduatedBackSymbol, this.PicSSym.Width, this.PicSSym.Height, "面状地物符号");
            frm = null;
        }

        private void PicPSSymMin_Click(object sender, EventArgs e)
        {
            //比例符号化
            //选择标准符号（最小）
            if (CmbPSField.Text == "")
            {
                //ModDeclare.g_ErrorHandler.DisplayInformation("属性字段有误,请用户选择可使用的属性字段。", false, "确定", null);
                MessageBox.Show("属性字段有误,请用户选择可使用的属性字段。", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                return;
            }
            FrmSymbolManager frm = new FrmSymbolManager(FrmSymbolManager.UseType.General);
            ISymbol pSymbol = null;
            string strSymbolType = "";

            frm.m_InSymbol = pSymbol;
            switch (m_SymbolType)
            {
                case "面状地物符号":
                case "点状地物符号":
                    frm.m_SymbolType = "点状地物符号";
                    strSymbolType = "点状地物符号";
                    break;
                case "线状地物符号":
                    frm.m_SymbolType = "线状地物符号";
                    strSymbolType = "线状地物符号";
                    break;
                default:
                    break;
            }

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            m_ProportionalMinSymbol = frm.CurSymbol;
            PicPSSymMin.Image = CreatePictureFromSymbol2(m_ProportionalMinSymbol, PicPSSymMin.Width, PicPSSymMin.Height, strSymbolType);

            frm = null;
        }

        private void PicPSSymBack_Click(object sender, EventArgs e)
        {
            //选择背景符号
            FrmSymbolManager frm = new FrmSymbolManager(FrmSymbolManager.UseType.General);
            ISymbol pSymbol = null;

            frm.m_InSymbol = pSymbol;

            frm.m_SymbolType = "面状地物符号";
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            m_ProportionalBackSymbol = frm.CurSymbol;
            frm = null;

            this.PicPSSymBack.Image = CreatePictureFromSymbol2(m_ProportionalBackSymbol, PicPSSymBack.Width, PicPSSymBack.Height, "面状地物符号");
        }

        //*********************************************************************************
        //** 函 数 名：FunAddFieldToCombo
        //** 输    入：   ByVal pFeatureLayer    ByRef Lst
        //** 输    出：
        //** 功能描述：加载字段到ListBox
        //*********************************************************************************
        private void FunAddFieldToListbox(IFeatureLayer pFeatureLayer, ListBox lst, string vFieldType)
        {
            IFields pFlds;
            IFeatureClass pFeatureClass;
            ClsField pFieldName;
            pFeatureClass = pFeatureLayer.FeatureClass;
            pFlds = pFeatureClass.Fields;

            IField pFld;
            int i;
            string ShapeFieldName, OidFieldName;
            ShapeFieldName = pFeatureClass.ShapeFieldName;
            OidFieldName = pFeatureClass.OIDFieldName;
            lst.Items.Clear();

            vFieldType = Strings.UCase(vFieldType);

            for (i = 0; i <= pFlds.FieldCount - 1; i++)
            {
                pFld = pFlds.get_Field(i);

                if ((pFld.Name != ShapeFieldName) && (pFld.Name != OidFieldName))
                {
                    pFieldName = new ClsField();
                    switch (pFlds.get_Field(i).Type)
                    {
                        case esriFieldType.esriFieldTypeString:
                            if ((vFieldType == "STR") || (vFieldType == ""))
                            {
                                pFieldName.AliasName = pFlds.get_Field(i).AliasName;
                                pFieldName.Name = pFlds.get_Field(i).Name;
                                if (IsExistInDataGridView(this.dgvBarFields, 1, pFieldName.AliasName) == false)
                                {
                                    lst.Items.Add(pFieldName);
                                }
                            }
                            break;
                        case esriFieldType.esriFieldTypeSmallInteger:
                        case esriFieldType.esriFieldTypeInteger:
                        case esriFieldType.esriFieldTypeSingle:
                        case esriFieldType.esriFieldTypeDouble:
                            if ((vFieldType == "NUM") || (vFieldType == ""))
                            {
                                pFieldName.AliasName = pFlds.get_Field(i).AliasName;
                                pFieldName.Name = pFlds.get_Field(i).Name;
                                if (IsExistInDataGridView(this.dgvBarFields, 1, pFieldName.AliasName) == false)
                                {
                                    lst.Items.Add(pFieldName);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            lst.DisplayMember = "AliasName";
        }

        private bool IsExistInDataGridView(DataGridView vDataGridView, int vFieldIndex, string vFieldValue)
        {
            if (vDataGridView.RowCount == 0)
            {
                return false;
            }
            int i;
            for (i = 0; i <= vDataGridView.RowCount - 1; i++)
            {
                if (vDataGridView.Rows[i].Cells[vFieldIndex].Value.ToString() == vFieldValue)
                {
                    return true;
                }
            }
            return false;
        }

        private void LstBarFields_DoubleClick(object sender, EventArgs e)
        {
            if (this.LstBarFields.SelectedIndex < 0)
            {
                return;
            }
            int i;
            IFillSymbol pFillSymbol;
            if (IsExistInDataGridView(this.dgvBarFields, 1, this.LstBarFields.Text) == true)
            {
                return;
            }

            IEnumColors pEnumColors;
            IRgbColor pRGBColor = null;
            Color[] aColor;
            aColor = (Color[])m_ColorScheme[m_BarColor.SelectedIndex + 1];
            pEnumColors = ReadEnumColor(aColor, this.LstBarFields.Items.Count);

            for (i = 0; i <= LstBarFields.SelectedIndex; i++)
            {
                pRGBColor = (IRgbColor)pEnumColors.Next();
            }
            pRGBColor.UseWindowsDithering = true;

            pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pRGBColor;
            this.dgvBarFields.Rows.Add();
            this.dgvBarFields.Rows[this.dgvBarFields.Rows.Count - 1].Cells[1].Value = LstBarFields.Text;
            this.dgvBarFields.Rows[this.dgvBarFields.Rows.Count - 1].Cells[0].Value = CreatePictureFromSymbol2((ISymbol)pFillSymbol, lPicWidth, lPicHeight, "面状地物符号");
            this.dgvBarFields.Rows[this.dgvBarFields.Rows.Count - 1].Tag = pFillSymbol;

            this.LstBarFields.Items.RemoveAt(this.LstBarFields.SelectedIndex);
        }

        private void cmdBarAddAll_Click(object sender, EventArgs e)
        {
            //添加全部
            int i;
            for (i = LstBarFields.Items.Count - 1; i >= 0; i--)
            {
                LstBarFields.SelectedIndex = i;
                LstBarFields_DoubleClick(this.LstBarFields, null);
            }
        }

        private void cmdBarRemoveAll_Click(object sender, EventArgs e)
        {
            //移除全部
            this.dgvBarFields.Rows.Clear();

            FunAddFieldToListbox(m_FeatureLayer, LstBarFields, "num");
        }

        private void m_BarColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            //改变分级颜色的颜色
            int l;
            int FeatureCount;
            IFillSymbol pFillSymbol;
            FeatureCount = this.dgvBarFields.RowCount;

            IEnumColors pEnumColors;
            IRgbColor pRGBColor = null;
            Color[] aColor;
            aColor = (Color[])m_ColorScheme[m_BarColor.SelectedIndex + 1];
            pEnumColors = ReadEnumColor(aColor, this.dgvBarFields.RowCount);

            for (l = 0; l <= this.dgvBarFields.RowCount - 1; l++)
            {
                pFillSymbol = (IFillSymbol)this.dgvBarFields.Rows[l].Tag;
                pRGBColor = (IRgbColor)pEnumColors.Next();
                pFillSymbol.Color = pRGBColor;
                this.dgvBarFields.Rows[l].Cells[0].Value = CreatePictureFromSymbol2((ISymbol)pFillSymbol, lPicWidth, lPicHeight, "面状地物符号");
            }
        }

        private void PicBackGround_Click(object sender, EventArgs e)
        {
            //选择标准符号
            FrmSymbolManager frm = new FrmSymbolManager(FrmSymbolManager.UseType.General);
            ISymbol pSymbol = null;
            string pSymbolType = null;

            frm.m_InSymbol = pSymbol;
            switch (m_SymbolType)
            {
                case "点状地物符号":
                    return;
                case "面状地物符号":
                    frm.m_SymbolType = "面状地物符号";
                    pSymbolType = "面状地物符号";
                    break;
                case "线状地物符号":
                    frm.m_SymbolType = "线状地物符号";
                    pSymbolType = "线状地物符号";
                    break;
                default:
                    break;
            }
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            m_ChartSymbol = frm.CurSymbol;

            PicBackGround.Tag = frm.CurSymbol;
            picSymbol.Image = CreatePictureFromSymbol2(m_ChartSymbol, PicBackGround.Width, PicBackGround.Height, pSymbolType);

            frm = null;
        }

        private void InitialGroupBox(string vGroupBoxName)
        {
            int i;

            this.Width = 491;
            this.Height = 362;
            for (i = 0; i <= strGroupBoxes.Length - 1; i++)
            {
                if (strGroupBoxes[i] == vGroupBoxName)
                {
                    this.Controls[strGroupBoxes[i]].Visible = true;
                    this.Controls[vGroupBoxName].Left = this.FrameSimple.Left;
                    this.Controls[vGroupBoxName].Top = this.FrameSimple.Top;
                }
                else
                {
                    this.Controls[strGroupBoxes[i]].Visible = false;
                }
            }
        }
        //根据FeatureClass,Field和FieldValue获得查询表达式
        //不同的WorkSpace构造查询表达式不同
        private string GetWhereClause(IFeatureClass vFeatCls, string vFieldName, string vFieldValue)
        {
            string strWhereClause = "";
            IDataset pDataSet;
            IWorkspace pWorkspace;

            if (vFeatCls == null)
            {
                return strWhereClause;
            }

            //获得workspace的type
            pDataSet = (IDataset)vFeatCls;
            pWorkspace = pDataSet.Workspace;

            if ((pWorkspace.Type == esriWorkspaceType.esriFileSystemWorkspace) || (pWorkspace.Type == esriWorkspaceType.esriLocalDatabaseWorkspace))
            {
                //shape,pdb,fdb等
                switch (vFeatCls.Fields.get_Field(vFeatCls.Fields.FindField(vFieldName)).Type)
                {
                    case esriFieldType.esriFieldTypeSmallInteger:
                    case esriFieldType.esriFieldTypeInteger:
                    case esriFieldType.esriFieldTypeSingle:
                    case esriFieldType.esriFieldTypeDouble:
                        strWhereClause = "" + vFieldName + "" + "=" + vFieldValue;
                        break;
                    case esriFieldType.esriFieldTypeString:
                        strWhereClause = "" + vFieldName + "" + "=" + "'" + vFieldValue + "'";
                        break;
                    default:
                        break;
                }
            }
            else if (pWorkspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                //sde等
                switch (vFeatCls.Fields.get_Field(vFeatCls.Fields.FindField(vFieldName)).Type)
                {
                    case esriFieldType.esriFieldTypeSmallInteger:
                    case esriFieldType.esriFieldTypeInteger:
                    case esriFieldType.esriFieldTypeSingle:
                    case esriFieldType.esriFieldTypeDouble:
                        strWhereClause = vFieldName + "=" + vFieldValue;
                        break;
                    case esriFieldType.esriFieldTypeString:
                        strWhereClause = vFieldName + "='" + vFieldValue + "'";
                        break;
                    default:
                        break;
                }
            }
            return strWhereClause;

        }

        //透明设置
        private void mnuTrans_Click(object sender, EventArgs e)
        {
            FrmTransSetup frm = new FrmTransSetup();
            frm.m_FeatureLayer = m_FeatureLayer;
            frm.ShowDialog();
            frm = null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_bIsChanged = false;
            IGeoFeatureLayer pGeoFeatLyr;
            pGeoFeatLyr = (IGeoFeatureLayer)m_FeatureLayer;
            pGeoFeatLyr.Renderer = m_OldRenderer;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

        }

        //透明设置
        private void btnURTrans_Click(object sender, EventArgs e)
        {
            FrmTransSetup frm = new FrmTransSetup();
            frm.m_FeatureLayer = m_FeatureLayer;
            frm.ShowDialog();
            frm = null;
        }

        //选择设置
        private void btnURRotate_Click(object sender, EventArgs e)
        {
            FrmRotateRenderer frm = new FrmRotateRenderer();
            frm.m_FeatureLayer = m_FeatureLayer;
            frm.ShowDialog();
            frm = null;
        }

        private void btnURMRotate_Click(object sender, EventArgs e)
        {
            FrmRotateRenderer frm = new FrmRotateRenderer();
            frm.m_FeatureLayer = m_FeatureLayer;
            frm.ShowDialog();
            frm = null;
        }

        private void btnURMTrans_Click(object sender, EventArgs e)
        {
            FrmTransSetup frm = new FrmTransSetup();
            frm.m_FeatureLayer = m_FeatureLayer;
            frm.ShowDialog();
            frm = null;
        }

        private void CmbURMField3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvURM.Rows.Clear();
        }

        #endregion
    }
}
