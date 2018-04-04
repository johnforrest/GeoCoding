using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using ZJGIS;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geodatabase;
using ZJGISCommon;
using ZJGISCommon.Classes;
using SelectQuery;
using ZJGISOpenData.Forms;
using ZJGISFinishTool;
using ZJGISEnvirConfig;
using PluginFramework;
using ZJGISGCoding;
using ZJGISDataUpdating;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ZJGISHistory;
using ESRI.ArcGIS.esriSystem;
using ZJGISDataMatch.Classes;
using ZJGISDataMatch.Forms;
using ZJGISGCoding.Class;
using ZJGISEntiTable.Froms;
using ZJGISGCoding.Forms;
using ZJGISDataUpdating.Class;
using ZJGISXMLOperation;
using ZJGISXMLOperation.Forms;
using ZJGIS.Class;
using ZJGIS.Classes;
namespace ZJGIS
{
    public partial class frmMain
    {
        private Dictionary<string, string> dic_fileFullPath = new Dictionary<string, string>(); //已打开文档路径
        private frmIdxMap m_frmIdxMap;//鹰眼地图
        private frmDataTree m_frmDataTree;//目录树
        private clsFrmMain m_clsFrmMain; //frmMain类
        private IMapControl3 m_MapControl;//主地图
        private Map m_MapEvent;

        private IApplication mainApplication = new PluginFramework.Application();
        private PluginService pluginService;

        //private IAoInitialize m_AoInitialize = new AoInitializeClass();
        ZJGISGCoding.Class.ClsCommon pClsCom = new ZJGISGCoding.Class.ClsCommon();

        #region mapbrowse tools
        private ESRI.ArcGIS.SystemUI.ICommand commandFixedZoomIn;
        private ESRI.ArcGIS.SystemUI.ICommand commandFixedZoomOut;
        private ESRI.ArcGIS.SystemUI.ICommand toolZoomIn;
        private ESRI.ArcGIS.SystemUI.ICommand toolZoomOut;
        private ESRI.ArcGIS.SystemUI.ICommand commandFullExtent;
        private ESRI.ArcGIS.SystemUI.ICommand commandBack;
        private ESRI.ArcGIS.SystemUI.ICommand commandFore;
        private ESRI.ArcGIS.SystemUI.ICommand toolPan;
        private ESRI.ArcGIS.SystemUI.ICommand commandRefresh;

        #endregion
        public frmMain()
        {
            InitializeComponent();

            InitialMapStorage();

            this.ribbonControl1.SelectedRibbonTabItem = ribbonTabItemWJ;

            //匹配结果窗体加载控件
            ClsControl.MapControlMain = this.mapMain.Object as IMapControl4;
            ClsControl.m_MapControlFrom = this.MapFrom.Object as IMapControl4;
            ClsControl.m_MapControlTo = this.MapTo.Object as IMapControl4;
            ClsControl.m_MapControlOverlap = this.MapOverlapping.Object as IMapControl4;
            ClsControl.m_MainTabControl = this.tabControlMain;

            //插件
            mainApplication.RibbonControlMain = this.ribbonControl1;
            mainApplication.MapControMain = this.mapMain.Object as IMapControl4;

            pluginService = new PluginService(mainApplication);
            pluginService.LoadAllPlugin();


            //20170313
            //this.Load += new System.EventHandler(this.frmMain_Load);


        }
        #region 图库管理
        /// <summary>
        /// 初始化图库管理
        /// </summary>
        private void InitialMapStorage()
        {
            m_MapControl = mapMain.Object as IMapControl3;
            XmlDocument pXmlDocument = new XmlDocument();
            string strXML = System.Windows.Forms.Application.StartupPath + "\\..\\Res\\Xml\\DataView.xml";
            if (!string.IsNullOrEmpty(strXML))
            {
                pXmlDocument.Load(strXML);

            }
            //加载图库数据
            bool LoadOK;
            XmlElement pXmlElement = (XmlElement)pXmlDocument.DocumentElement;
            LoadOK = ClsDataTree.LoadLyrTreeFromXML(ref pXmlElement, ref this.tvwXMGL);
            if ((LoadOK == true) && (tvwXMGL.Nodes.Count > 0))
            {
                //设置节点图片
                ClsDataTree.SetNodePic(tvwXMGL);
            }
        }
        #endregion

        #region  设置样式
        private void buttonStyleOffice2007Blue_Click(System.Object sender, System.EventArgs e)
        {

            //ribbonControl1.Office2007ColorTable = eOffice2007ColorScheme.Blue;
            this.styleManager.ManagerStyle = eStyle.Office2007Blue; ;
        }

        private void buttonStyleOffice2007Black_Click(System.Object sender, System.EventArgs e)
        {

            //ribbonControl1.Office2007ColorTable = eOffice2007ColorScheme.Black;
            this.styleManager.ManagerStyle = eStyle.Office2007Black;
        }

        private void buttonStyleOffice2007Silver_Click(System.Object sender, System.EventArgs e)
        {

            //ribbonControl1.Office2007ColorTable = eOffice2007ColorScheme.Silver;
            this.styleManager.ManagerStyle = eStyle.Office2007Silver;
        }

        private bool m_ColorSelected = false;
        private eOffice2007ColorScheme m_BaseColorScheme = eOffice2007ColorScheme.Blue;
        private void buttonStyleCustom_ExpandChange(object sender, System.EventArgs e)
        {
            if (buttonStyleCustom.Expanded)
            {
                // Remember the starting color scheme to apply if no color is selected during live-preview
                m_ColorSelected = false;
                m_BaseColorScheme = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.InitialColorScheme;
            }
            else
            {
                if (!m_ColorSelected)
                {
                    this.styleManager.ManagerStyle = (eStyle)m_BaseColorScheme;
                }
            }
        }

        private void buttonStyleCustom_ColorPreview(object sender, DevComponents.DotNetBar.ColorPreviewEventArgs e)
        {
            RibbonPredefinedColorSchemes.ChangeOffice2007ColorTable(this, m_BaseColorScheme, e.Color);
        }

        private void buttonStyleCustom_SelectedColorChanged(object sender, System.EventArgs e)
        {
            m_ColorSelected = true; // Indicate that color was selected for buttonStyleCustom_ExpandChange method
            RibbonPredefinedColorSchemes.ChangeOffice2007ColorTable(this, m_BaseColorScheme, buttonStyleCustom.SelectedColor);
        }
        #endregion


        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                //20170313 注释掉
                //IWorkspace workSpace = ClsDBInfo.SdeWorkspace;
                m_clsFrmMain = new clsFrmMain();

                m_clsFrmMain.FrmActive = this;
                m_clsFrmMain.MapControl = m_MapControl;
                //初始化
                SelectQuery.ClsDeclare.g_Sys = new ZJGISCommon.ClsSysSet();
                SelectQuery.ClsDeclare.g_Sys.FrmMain = this;
                SelectQuery.ClsDeclare.g_Sys.MapControl = mapMain.Object as IMapControl3;
                SelectQuery.ClsDeclare.g_pMap = mapMain.Map;

                System.Windows.Forms.Application.DoEvents();

                //控制鹰眼图、树图等
                m_frmDataTree = new frmDataTree();
                m_frmDataTree.BuddyControl = m_MapControl;
                m_frmDataTree.FrmActive = this;
                m_frmDataTree.MapMain = (IMapControl4)mapMain.Object;
                this.pdcDataTree.Controls.Add(m_frmDataTree);

                m_frmDataTree.Visible = true;
                m_frmDataTree.Dock = DockStyle.Fill;
                System.Windows.Forms.Application.DoEvents();

                ////20170504注释掉鹰眼
                //m_frmIdxMap = new frmIdxMap();
                //this.pdcIdxMap.Controls.Add(m_frmIdxMap);
                //m_frmIdxMap.mainForm = this;
                //m_frmIdxMap.Visible = true;
                //m_frmIdxMap.Dock = DockStyle.Fill;

                ////匹配结果表
                //frmShowMatchedResult = new FrmShowMatchedResult();
                //pFrmResult = new FrmResult();
                //frmShowMatchedResult.TopLevel = false;
                //frmShowMatchedResult.Size = pFrmResult.panelDockContainerMatchedResult.Size;

                //frmShowMatchedResult.MapControlFrom = ClsControl.m_MapControlFrom;
                //frmShowMatchedResult.MapControlTo = ClsControl.m_MapControlTo;
                //frmShowMatchedResult.MapControlOverlap = ClsControl.m_MapControlOverlap;
                //frmShowMatchedResult.tabControl = ClsControl.m_MainTabControl;
                //frmShowMatchedResult.DGVFrom = ClsControl.m_DGVFrom;
                //frmShowMatchedResult.DGVTo = ClsControl.m_DGVTo;

                //pFrmResult.panelDockContainerMatchedResult.Controls.Clear();
                //pFrmResult.panelDockContainerMatchedResult.Controls.Add(frmShowMatchedResult);
                //pFrmResult.tabControl = ClsControl.m_MainTabControl;
                //pFrmResult.m_BottomStandBar = ClsControl.m_BottomStandBar;
                //frmShowMatchedResult.Show();


                m_MapEvent = (Map)this.mapMain.ActiveView.FocusMap; //初始化Map事件
                m_MapEvent.AfterDraw += new IActiveViewEvents_AfterDrawEventHandler(this.m_MapEvent_AfterDraw);
                //初始化comboBox下拉菜单
                m_MapEvent.ItemAdded += new IActiveViewEvents_ItemAddedEventHandler(this.m_MapEvent_ItemAdded);
                m_MapEvent.ItemDeleted += new IActiveViewEvents_ItemDeletedEventHandler(m_MapEvent_ItemDeleted);
                this.WindowState = FormWindowState.Maximized;
                System.Windows.Forms.Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "数据库未连接！");
            }

            string startPath = System.Windows.Forms.Application.StartupPath;
            string textFullPath = startPath + "\\..\\Res\\path\\openedPath.txt";

            ReadText(textFullPath);

            //初试化时选择一个图层
            comboBoxItemToMapControl.SelectedIndex = 0;
        }
        private void m_MapEvent_AfterDraw(IDisplay Display, esriViewDrawPhase phase)
        {
            IEnvelope pRect;
            ISimpleFillSymbol pSym;
            pRect = m_frmIdxMap.EyeRect;
            pSym = m_frmIdxMap.EyeRectSym;
            object obj = pSym;
            if (pRect != null && pSym != null)
            {
                m_frmIdxMap.MapIndex.DrawShape(pRect, ref obj);//改
            }
            //m_frmIdxMap.MapIndex.ActiveView.Refresh()
            m_frmIdxMap.MapIndex.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);

        }
        /// <summary>
        /// 初始化comboBox下拉菜单
        /// </summary>
        /// <param name="Item"></param>
        private void m_MapEvent_ItemAdded(object Item)
        {
            cbxCodeLayer.Items.Clear();
            comboBoxItemCheck.Items.Clear();
            ClsCheckData.AddDataToCom(mapMain.Map, cbxCodeLayer);
            ClsCheckData.AddDataToCom(mapMain.Map, comboBoxItemCheck);
        }
        private void m_MapEvent_ItemDeleted(object Item)
        {
            cbxCodeLayer.Items.Clear();
            comboBoxItemCheck.Items.Clear();
            ClsCheckData.AddDataToCom(mapMain.Map, cbxCodeLayer);
            ClsCheckData.AddDataToCom(mapMain.Map, comboBoxItemCheck);
        }



        #region Basic tool
        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomIn_Click(object sender, EventArgs e)
        {

            toolZoomIn = new ControlsMapZoomInToolClass();
            toolZoomIn.OnCreate(mapMain.Object);
            this.mapMain.CurrentTool = (ITool)toolZoomIn;
        }
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            toolZoomOut = new ControlsMapZoomOutToolClass();
            toolZoomOut.OnCreate(mapMain.Object);
            this.mapMain.CurrentTool = (ITool)toolZoomOut;
        }
        /// <summary>
        /// 固定放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFixedZoomIn_Click(object sender, EventArgs e)
        {
            commandFixedZoomIn = new ControlsMapZoomInFixedCommandClass();
            commandFixedZoomIn.OnCreate(mapMain.Object);
            commandFixedZoomIn.OnClick();
        }
        /// <summary>
        /// 固定缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFixedZoomOut_Click(object sender, EventArgs e)
        {
            commandFixedZoomOut = new ControlsMapZoomOutFixedCommandClass();
            commandFixedZoomOut.OnCreate(mapMain.Object);
            commandFixedZoomOut.OnClick();
        }
        /// <summary>
        /// 漫游
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPan_Click(object sender, EventArgs e)
        {
            toolPan = new ControlsMapPanToolClass();
            toolPan.OnCreate(mapMain.Object);
            this.mapMain.CurrentTool = (ITool)toolPan;
        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, EventArgs e)
        {
            commandBack = new ControlsMapZoomToLastExtentForwardCommandClass();
            commandBack.OnCreate(mapMain.Object);
            commandBack.OnClick();
        }
        /// <summary>
        /// 向前
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFore_Click(object sender, EventArgs e)
        {
            commandFore = new ControlsMapZoomToLastExtentBackCommandClass();
            commandFore.OnCreate(mapMain.Object);
            commandFore.OnClick();

        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            commandRefresh = new ControlsMapRefreshViewCommandClass();
            commandRefresh.OnCreate(mapMain.Object);
            commandRefresh.OnClick();
        }
        /// <summary>
        /// 全图显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFullExtent_Click(object sender, EventArgs e)
        {
            commandFullExtent = new ControlsMapFullExtentCommandClass();
            commandFullExtent.OnCreate(mapMain.Object);
            commandFullExtent.OnClick();
        }
        /// <summary>
        /// 初始状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefault_Click(object sender, EventArgs e)
        {
            this.mapMain.CurrentTool = null;
            IActiveView pActiveView = (IActiveView)(mapMain.Map);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, mapMain.get_Layer(0), null);
            this.mapMain.Map.ClearSelection();
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, mapMain.get_Layer(0), null);
        }
        #region measure
        /// <summary>
        /// 量算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMeasure_Click(object sender, EventArgs e)
        {
            //使用自定义封装类ClsMeasure

            ClsMeasure toolMeasure = new ClsMeasure();
            toolMeasure.OnCreate(this.mapMain.Object);
            toolMeasure.OnClick();
        }
        #endregion
        #region QueryOperation

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryByAttribute_Click(object sender, EventArgs e)
        {
            //使用自定义封装类ClsQueryByAttribute
            ClsQueryByAttribute toolQueryByAttribute = new ClsQueryByAttribute();
            toolQueryByAttribute.OnCreate(this.mapMain.Object);
            //toolQueryByAttribute.FrmActive = this;
            toolQueryByAttribute.OnClick();
        }
        /// <summary>
        /// 模糊查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            //使用自定义封装类ClsFind
            ClsFind toolFind = new ClsFind();
            toolFind.OnCreate(this.mapMain.Object);
            toolFind.OnClick();
        }
        #endregion
        #region selection

        /// <summary>
        /// 点选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPointSel_Click(object sender, EventArgs e)
        {
            //使用自定义封装类ClsSelectByPoint
            ClsSelectByPoint toolSelectByPoint = new ClsSelectByPoint();
            toolSelectByPoint.OnCreate(this.mapMain.Object);
            mapMain.CurrentTool = (ITool)toolSelectByPoint;
        }



        /// <summary>
        /// 线选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLineSel_Click(object sender, EventArgs e)
        {
            //使用自定义封装类ClsSelectByLine
            ClsSelectByLine toolSelectByLine = new ClsSelectByLine();
            toolSelectByLine.OnCreate(this.mapMain.Object);
            this.mapMain.CurrentTool = toolSelectByLine;
        }



        /// <summary>
        /// 圆选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCircleSel_Click(object sender, EventArgs e)
        {
            //使用自定义封装类ClsSelectByCircle

            ClsSelectByCircle toolSelectByCircle = new ClsSelectByCircle();
            toolSelectByCircle.OnCreate(this.mapMain.Object);

            this.mapMain.CurrentTool = toolSelectByCircle;
        }



        /// <summary>
        /// 多边形选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPolygonSel_Click(object sender, EventArgs e)
        {
            //使用自定义封装类ClsSelectByPolygon

            ClsSelectByPolygon toolSelectByPolygon = new ClsSelectByPolygon();
            toolSelectByPolygon.OnCreate(this.mapMain.Object);

            this.mapMain.CurrentTool = (ITool)toolSelectByPolygon;
        }


        /// <summary>
        /// 矩形选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRectangleSel_Click(object sender, EventArgs e)
        {
            //使用自定义封装类ClsSelectByRectangle

            ClsSelectByRectangle toolSelectByRectangle = new ClsSelectByRectangle();
            toolSelectByRectangle.OnCreate(this.mapMain.Object);

            this.mapMain.CurrentTool = (ITool)toolSelectByRectangle;
        }
        /// <summary>
        /// 缩放至选择区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZoomToSel_Click(object sender, EventArgs e)
        {
            //使用自定义封装类ClsZoomToSelection
            ClsZoomToSelection toolZoomToSelection = new ClsZoomToSelection();
            toolZoomToSelection.OnCreate(this.mapMain.Object);
            toolZoomToSelection.OnClick();
        }
        /// <summary>
        /// 清空选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearSel_Click(object sender, EventArgs e)
        {

            ClsClearFeatureSelection toolClearSelection = new ClsClearFeatureSelection();
            toolClearSelection.OnCreate(this.mapMain.Object);
            toolClearSelection.OnClick();
        }

        #endregion

        #endregion
        #region Document operations
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {

            OpenFileDialog openMapFileDialog = new System.Windows.Forms.OpenFileDialog();

            openMapFileDialog.Title = "打开地图文档！";
            openMapFileDialog.Filter = "Map Documents (*.mxd)|*.mxd";
            openMapFileDialog.ShowDialog();
            string mxFilePath = openMapFileDialog.FileName;
            if (mxFilePath == "")
            {
                return;
            }
            if (mapMain.CheckMxFile(mxFilePath) == true)
            {


                mapMain.LoadMxFile(mxFilePath, Type.Missing, Type.Missing);
                mapMain.Enabled = true;

            }
            else
            {
                MessageBox.Show("错误", mxFilePath + "不是合法的ArcMap文件！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            WriteText(mxFilePath);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                IMxdContents pMxdC;
                pMxdC = mapMain.Map as IMxdContents;

                IMapDocument pMapDocument = new MapDocumentClass();

                if (mapMain.DocumentFilename == null)
                {
                    SaveFileDialog saveMapFileDialog = new SaveFileDialog();
                    saveMapFileDialog.Title = "保存地图文件";
                    saveMapFileDialog.Filter = "Map Documents (*.mxd)|*.mxd";
                    if (saveMapFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        pMapDocument.New(saveMapFileDialog.FileName);
                        pMapDocument.ReplaceContents(pMxdC);
                        pMapDocument.Save(true, true);
                        MessageBox.Show("保存成功！");
                    }
                }
                else
                {
                    pMapDocument.Open(mapMain.DocumentFilename, "");

                    pMapDocument.ReplaceContents(pMxdC);
                    pMapDocument.Save(true, true);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            IMxdContents pMxdC;
            pMxdC = mapMain.Map as IMxdContents;
            IMapDocument pMapDocument = new MapDocumentClass();
            SaveFileDialog saveMapFileDialog = new SaveFileDialog();
            saveMapFileDialog.Title = "保存地图文件";
            saveMapFileDialog.Filter = "Map Documents (*.mxd)|*.mxd";
            if (saveMapFileDialog.ShowDialog() == DialogResult.OK)
            {
                pMapDocument.New(saveMapFileDialog.FileName);

                pMapDocument.ReplaceContents(pMxdC);
                pMapDocument.Save(true, true);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmOpenData loadDatatoMap = new FrmOpenData((IBasicMap)this.mapMain.Map);
            if (loadDatatoMap.ShowDialog() == DialogResult.OK)
            {
                mapMain.ActiveView.Refresh();
            }
        }

        #endregion
        #region Environment configuration
        private void btnSDEConnection_Click(object sender, EventArgs e)
        {
            FrmSDE pFrmSDE = new FrmSDE();
            pFrmSDE.ShowDialog();
        }
        private void btnMaintain_Click(object sender, EventArgs e)
        {
            FormMaintain pFormMaintain = new FormMaintain();
            pFormMaintain.ShowDialog();
        }
        #endregion
        #region adddataby map
        private void tvwXMGL_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.tvwXMGL.SelectedNode = e.Node;
        }

        private void tvwXMGL_AfterCheck(object sender, TreeViewEventArgs e)
        {
            int i = 0;

            if (e.Node.Checked == true)  //选中则添加
            {
                if (e.Node.Tag.ToString() == "FDC")
                {
                    //this.axTOCControlLayers.Enabled = false;
                    if (!LoadFeatFDC(e.Node))
                    {
                        e.Node.Checked = false;
                    }
                    //this.axTOCControlLayers.Enabled = true;
                }
                else if (e.Node.Tag.ToString() == "FDS")
                {
                    if (e.Node.Nodes.Count > 10)
                    {
                        if (tvwXMGL.Tag == null)
                        {
                            if (MessageBox.Show("将要加载的图层数大于10，是否继续加载？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                            {
                                e.Node.Checked = false;
                                return;
                            }
                        }
                        else
                        {
                            tvwXMGL.Tag = null;
                        }
                    }
                    //this.axTOCControlLayers.Enabled = false;
                    for (i = 0; i <= e.Node.Nodes.Count - 1; i++)
                    {
                        if (e.Node.Nodes[i].Checked == false)
                        {
                            e.Node.Nodes[i].Checked = true;
                        }
                    }
                    //this.axTOCControlLayers.Enabled = true;
                }
                else if (e.Node.Tag.ToString() == "RC")
                {
                    if (!LoadFeatFDC(e.Node))
                    {
                        e.Node.Checked = false;
                    }
                }
                else if (e.Node.Tag.ToString() == "RD")
                {
                    if (!LoadFeatFDC(e.Node))
                    {
                        e.Node.Checked = false;
                    }
                }
            }
            else           //否则移除
            {
                if (e.Node.Tag.ToString() == "FDC")
                {
                    UnloadLayer(e.Node);
                }
                else if (e.Node.Tag.ToString() == "FDS")
                {
                    // this.axTOCControlLayers.Enabled = false;
                    for (i = 0; i <= e.Node.Nodes.Count - 1; i++)
                    {
                        e.Node.Nodes[i].Checked = false;
                    }
                    //this.axTOCControlLayers.Enabled = true;
                }
                else if (e.Node.Tag.ToString() == "RC")
                {
                    UnloadLayer(e.Node);
                }
                else if (e.Node.Tag.ToString() == "RD")
                {
                    UnloadLayer(e.Node);
                }
            }

        }

        private bool LoadFeatFDC(TreeNode vNode)
        {
            TreeNode pTreeNode = null;
            IFeatureWorkspace pFeatWorkspace = null;
            IFeatureLayer pFeatLayer = null;

            XmlNodeList pXmlNodeList = null;
            int i = 0;
            IEnumDataset pEnumDs = null;
            IDataset pDs = null;

            bool bFind = false;
            IRasterWorkspaceEx pRasterWs = null;
            IRasterDataset pRasterDs = null;
            IRasterLayer pRasterLyr = null;
            IWorkspace pWorkspace = null;

            System.Xml.XmlElement vNodeMeXML = null;

            ClsString pSplitName = new ClsString();
            XmlDocument pXmlDocument = new XmlDocument();
            string strXML = System.Windows.Forms.Application.StartupPath + "\\..\\Res\\Xml\\DataView.xml";
            if (!string.IsNullOrEmpty(strXML))
            {
                pXmlDocument.Load(strXML);
            }
            //加载图库数据
            vNodeMeXML = (XmlElement)pXmlDocument.DocumentElement;


            //得到数据库工作区
            pWorkspace = ClsDBInfo.SdeWorkspace;//获取一次，下次赋值即可。

            try
            {
                pTreeNode = vNode;
                if ((pWorkspace != null)) //判断是否连上SDE
                {
                    if (pTreeNode.Tag.ToString() == "FDC")
                    {
                        pFeatWorkspace = (IFeatureWorkspace)pWorkspace;
                        LoadFeatCls(vNodeMeXML, pFeatWorkspace, vNode.Text);
                    }
                    else if (pTreeNode.Tag.ToString() == "FDS")
                    {
                        pFeatWorkspace = (IFeatureWorkspace)pWorkspace;
                        pXmlNodeList = vNodeMeXML.ChildNodes;
                        if (pXmlNodeList.Count > 10)
                        {
                            if (MessageBox.Show("将要加载的图层数大于10，是否继续加载？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                            {
                                return false;
                            }
                        }
                        for (i = 0; i <= pXmlNodeList.Count - 1; i++)
                        {
                            XmlElement pXmlElement = (XmlElement)pXmlNodeList[i];
                            LoadFeatCls(pXmlElement, pFeatWorkspace, vNode.Text);
                        }
                    }
                    else if (pTreeNode.Tag.ToString() == "RC")
                    {
                        pEnumDs = pWorkspace.get_Datasets(esriDatasetType.esriDTRasterCatalog);
                        bFind = false;
                        pDs = pEnumDs.Next();
                        while ((pDs != null))
                        {
                            if (pDs.Name == vNodeMeXML.GetAttribute("CODE"))
                            {
                                bFind = true;
                                break;
                            }

                            pDs = pEnumDs.Next();
                        }

                        if (bFind == false)
                        {

                            MessageBox.Show("找不到该栅格目录，请重新生成数据源XML", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                        else
                        {
                            pFeatLayer = new GdbRasterCatalogLayerClass();
                            pFeatLayer.FeatureClass = (IFeatureClass)pDs;
                            pFeatLayer.Name = vNodeMeXML.GetAttribute("TEXT");
                            mapMain.AddLayer(pFeatLayer, 0);

                            mapMain.Refresh(esriViewDrawPhase.esriViewGraphics, Type.Missing, Type.Missing);
                        }
                    }
                    else if (pTreeNode.Tag.ToString() == "RD")
                    {
                        pRasterWs = (IRasterWorkspaceEx)pWorkspace;
                        pRasterDs = pRasterWs.OpenRasterDataset(vNodeMeXML.GetAttribute("CODE"));
                        pRasterLyr = new RasterLayer();
                        pRasterLyr.Name = vNodeMeXML.GetAttribute("TEXT");
                        pRasterLyr.CreateFromDataset(pRasterDs);
                        mapMain.AddLayer(pRasterLyr, 0);
                        mapMain.Refresh(esriViewDrawPhase.esriViewGraphics, Type.Missing, Type.Missing);
                    }
                    return true;
                }
                else
                {

                    MessageBoxEx.Show("无法打开对应工作区，加载图层失败! 请先连接SDE !", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            catch (Exception)
            {

                MessageBoxEx.Show("当前连接的SDE中没有找到" + vNode.Text + "的数据，" + "" + "请更改SDE连接设置！");
                return false;
            }
            finally
            {
                pTreeNode = null;
                pFeatWorkspace = null;

                pFeatLayer = null;
            }


        }

        private bool LoadFeatCls(XmlElement vXmlElement, IFeatureWorkspace pFeatWorkspace, string sLayerName)
        {
            XmlElement pXmlElement = null;

            if (vXmlElement.HasChildNodes == true)
            {

                for (int i = 0; i < vXmlElement.ChildNodes.Count; i++)
                {
                    pXmlElement = (XmlElement)vXmlElement.ChildNodes[i];
                    if (pXmlElement.HasChildNodes == true)
                    {
                        LoadFeatCls(pXmlElement, pFeatWorkspace, sLayerName);
                    }
                    else
                    {
                        for (int j = 0; j < vXmlElement.ChildNodes.Count; j++)
                        {
                            IFeatureClass pFeatCls = null;
                            IFeatureLayer pFeatLayer = null;
                            string pFeatClsName = null;
                            XmlElement pXmlElement2 = (XmlElement)vXmlElement.ChildNodes[j];
                            if (sLayerName == pXmlElement2.GetAttribute("TEXT"))
                            {
                                pFeatClsName = pXmlElement2.GetAttribute("CODE");
                                if (pFeatClsName == null || pFeatClsName == "")
                                {
                                    return false;
                                }
                                pFeatCls = pFeatWorkspace.OpenFeatureClass(pFeatClsName);
                                pFeatLayer = new FeatureLayer();
                                pFeatLayer.FeatureClass = pFeatCls;
                                pFeatLayer.Name = pXmlElement2.GetAttribute("TEXT"); //给图层付名字。否则会显示Code
                                mapMain.Map.AddLayer(pFeatLayer);
                                return true;

                            }
                        }
                    }
                }
            }
            mapMain.ActiveView.Refresh();
            return true;
        }

        private bool UnloadLayer(TreeNode vNode)
        {
            string strLyrName = vNode.Text;
            ILayer pLayer = default(ILayer);
            try
            {
                pLayer = GetLyrByName(mapMain.Map, strLyrName);
                if ((pLayer != null))
                {
                    mapMain.Map.DeleteLayer(pLayer);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;

            }
        }
        private void GetSDENodeFromChildNode(TreeNode vNode, ref XmlElement vParentXMlElement)
        {
            XmlElement pXmlElement = default(XmlElement);

            pXmlElement = (XmlElement)vNode.Tag;

            if (pXmlElement == null)
                return;
            if (pXmlElement.GetAttribute("TYPE") == "SDE")
            {
                vParentXMlElement = pXmlElement;
            }
            else
            {
                if (vNode.Parent != null)
                    GetSDENodeFromChildNode(vNode.Parent, ref vParentXMlElement);
            }
        }

        public ILayer GetLyrByName(IMap vMap, string vLayerName)
        {
            int i = 0;

            for (i = 0; i <= vMap.LayerCount - 1; i++)
            {
                if (vMap.get_Layer(i).Name == vLayerName)
                {
                    return vMap.get_Layer(i);
                }
            }

            return null;
        }
        #endregion
        #region project transformation
        /// <summary>
        /// 投影转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProtrans_Click(object sender, EventArgs e)
        {
            ClsExtractAfterProjection toolExtractAfterProjection = new ClsExtractAfterProjection();
            toolExtractAfterProjection.OnCreate(mapMain.Object);
            toolExtractAfterProjection.OnClick();
        }
        #endregion
        #region 地理编码
        /// <summary>
        /// 空值检查(检查的是FCode字段是否存在)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataCheck_Click(object sender, EventArgs e)
        {
            FrmDataCheck pFrmDataCheck = new FrmDataCheck();
            ClsCheckData.CheckCode(mapMain.Map, cbxCodeLayer, pFrmDataCheck.GetDataGridView);
            pFrmDataCheck.ShowDialog();

        }
        //TODO :20180103合并
        /// <summary>
        /// 生成格网（如果有GridCode就用GridCode，没有的话就新建一个GridCode字段，并加入格网信息）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreatGrid_Click(object sender, EventArgs e)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(this.mapMain.Map, this.cbxCodeLayer.Text);

            if (pFeatureLayer != null)
            {
                if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    ClsCommonEnti pcommonEnti = new ClsCommonEnti();
                    pcommonEnti.CreatGridCode(mapMain.Map, cbxCodeLayer);
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    ClsRoadEnti pRoadEnti = new ClsRoadEnti();
                    pRoadEnti.CreatGridCodeRoad(mapMain.Map, cbxCodeLayer);
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    ClsResEnti pResEnti = new ClsResEnti();
                    pResEnti.CreatGridCodeRES(mapMain.Map, cbxCodeLayer);
                }
            }
            else
            {
                MessageBox.Show("没有选中任何图层，请选择图层！");
            }

        }


        /// <summary>
        /// 实体编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(this.mapMain.Map, this.cbxCodeLayer.Text);
            if (pFeatureLayer != null)
            {
                if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    ClsCommonEnti pcommonEnti = new ClsCommonEnti();
                    pcommonEnti.CommonEntiCode(mapMain.Map, cbxCodeLayer);
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    ClsRoadEnti pRoadEnti = new ClsRoadEnti();
                    pRoadEnti.RoadCode(mapMain.Map, cbxCodeLayer);
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    ClsResEnti pResEnti = new ClsResEnti();
                    pResEnti.RESCode(mapMain.Map, cbxCodeLayer);
                }
            }
            else
            {
                MessageBox.Show("没有选中任何图层，请选择图层！");
            }

        }
        /// <summary>
        /// 补充格网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestGrid_Click(object sender, EventArgs e)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(this.mapMain.Map, this.cbxCodeLayer.Text);
            if (pFeatureLayer != null)
            {
                if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    ClsCommonEnti pCommonEnti = new ClsCommonEnti();
                    pCommonEnti.CreatGridCodeRest(mapMain.Map, cbxCodeLayer);
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    ClsRoadEnti pRoadEnti = new ClsRoadEnti();
                    pRoadEnti.RestRoadGrid(mapMain.Map, cbxCodeLayer);
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    ClsResEnti pResEnti = new ClsResEnti();
                    pResEnti.RestRESGridCode(mapMain.Map, cbxCodeLayer);
                }
            }
            else
            {
                MessageBox.Show("没有选中任何图层，请选择图层！");
            }

        }
        /// <summary>
        /// 补充编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestCode_Click(object sender, EventArgs e)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(this.mapMain.Map, this.cbxCodeLayer.Text);
            if (pFeatureLayer != null)
            {
                if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    ClsCommonEnti pCommonEnti = new ClsCommonEnti();
                    pCommonEnti.CodeRest(mapMain.Map, cbxCodeLayer);
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    ClsRoadEnti pRoadEnti = new ClsRoadEnti();
                    pRoadEnti.RestRoadCode(mapMain.Map, cbxCodeLayer);
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    ClsResEnti pResEnti = new ClsResEnti();
                    pResEnti.RestRESCode(mapMain.Map, cbxCodeLayer);
                }
            }
            else
            {
                MessageBox.Show("没有选中任何图层，请选择图层！");
            }
        }

        #region 20180103注释
        ///// <summary>
        ///// 道路编码
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnRoad_Click(object sender, EventArgs e)
        //{
        //    ClsRoadEnti pRoadEnti = new ClsRoadEnti();
        //    pRoadEnti.RoadCode(mapMain.Map, cbxCodeLayer);
        //}
        ///// <summary>
        ///// 道路格网
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnGridRoad_Click(object sender, EventArgs e)
        //{
        //    ClsRoadEnti pRoadEnti = new ClsRoadEnti();
        //    pRoadEnti.CreatGridCodeRoad(mapMain.Map, cbxCodeLayer);
        //}
        ///// <summary>
        ///// 补充道路格网
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnRestRoadGrid_Click(object sender, EventArgs e)
        //{
        //    ClsRoadEnti pRoadEnti = new ClsRoadEnti();
        //    pRoadEnti.CreatRestRoadGrid(mapMain.Map, cbxCodeLayer);
        //}
        ///// <summary>
        ///// 补充道路编码
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnRestRoadCode_Click(object sender, EventArgs e)
        //{
        //    ClsRoadEnti pRoadEnti = new ClsRoadEnti();
        //    pRoadEnti.CreatRestRoadCode(mapMain.Map, cbxCodeLayer);
        //}


        ///// <summary>
        ///// 房屋编码
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnHouse_Click(object sender, EventArgs e)
        //{
        //    ClsResEnti pResEnti = new ClsResEnti();
        //    pResEnti.RESCode(mapMain.Map, cbxCodeLayer);

        //}
        ///// <summary>
        ///// 房屋格网编码
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnRESGrid_Click(object sender, EventArgs e)
        //{
        //    ClsResEnti pResEnti = new ClsResEnti();
        //    pResEnti.CreatGridCodeRES(mapMain.Map, cbxCodeLayer);
        //}
        ///// <summary>
        ///// 补充房屋格网编码
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnRESPYGrid_Click(object sender, EventArgs e)
        //{
        //    ClsResEnti pResEnti = new ClsResEnti();
        //    pResEnti.CreatRestGridCodeRES(mapMain.Map, cbxCodeLayer);
        //}
        ///// <summary>
        ///// 补充房屋编码
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btnRestHouse_Click(object sender, EventArgs e)
        //{
        //    ClsResEnti pResEnti = new ClsResEnti();
        //    pResEnti.RestRESCode(mapMain.Map, cbxCodeLayer);
        //}
        #endregion

        /// <summary>
        /// POI补充格网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPOIGrid_Click(object sender, EventArgs e)
        {
            ClsPOIEnti pClsPOIEnti = new ClsPOIEnti();
            pClsPOIEnti.RestPOIGrid(mapMain.Map, cbxCodeLayer);
        }
        /// <summary>
        /// POI补充编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPOICode_Click(object sender, EventArgs e)
        {
            ClsPOIEnti pClsPOIEnti = new ClsPOIEnti();
            pClsPOIEnti.RestPOICode(mapMain.Map, cbxCodeLayer);
        }

        /// <summary>
        /// 图元编码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrimitive_Click(object sender, EventArgs e)
        {
            ClsTuYuanEnti pTuYuanEnti = new ClsTuYuanEnti();
            pTuYuanEnti.PrimitiveCode(mapMain.Map, cbxCodeLayer);

        }
        #endregion


        #region  多尺度联动更新

        /// <summary>
        /// 联动更新：大比例尺更新小比例尺
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxItem1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxItem2.Items.Clear();

            //if (comboBoxItem1.SelectedIndex == 0)
            //{
            //    comboBoxItem2.Items.Add("1:5万");
            //    comboBoxItem2.Items.Add("1:25万");
            //}
            //else if (comboBoxItem1.SelectedIndex == 1)
            //{
            //    comboBoxItem2.Items.Add("1:25万");
            //}
            if (comboBoxItem1.SelectedIndex == 0)
            {
                comboBoxItem2.Items.Add("1:2000");
                comboBoxItem2.Items.Add("1:10000");
            }
            else if (comboBoxItem1.SelectedIndex == 1)
            {
                //comboBoxItem2.Items.Add("1:500");
                comboBoxItem2.Items.Add("1:10000");
            }
        }
        /// <summary>
        /// 变化检测（同比例尺）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateCheck_Click(object sender, EventArgs e)
        {

            FrmMatchSet frmMatchSet = new FrmMatchSet();
            ZJGISDataUpdating.Class.ClsDeclare.g_SameScaleMatch = true;
            ZJGISDataUpdating.Class.ClsDeclare.g_DifScaleMatch = false;
            frmMatchSet.ShowDialog();
        }
        /// <summary>
        /// 编辑匹配列表（同比例尺）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditMathTab_Click(object sender, EventArgs e)
        {

            FrmResult pFrmResult;
            FrmShowMatchedResult frmShowMatchedResult;
            //匹配结果表
            frmShowMatchedResult = new FrmShowMatchedResult();
            pFrmResult = new FrmResult();
            frmShowMatchedResult.TopLevel = false;
            frmShowMatchedResult.Size = pFrmResult.panelDockContainerMatchedResult.Size;

            frmShowMatchedResult.MapControlFrom = ClsControl.m_MapControlFrom;
            frmShowMatchedResult.MapControlTo = ClsControl.m_MapControlTo;
            frmShowMatchedResult.MapControlOverlap = ClsControl.m_MapControlOverlap;
            frmShowMatchedResult.tabControl = ClsControl.m_MainTabControl;
            frmShowMatchedResult.DGVFrom = ClsControl.m_DGVFrom;
            frmShowMatchedResult.DGVTo = ClsControl.m_DGVTo;

            pFrmResult.panelDockContainerMatchedResult.Controls.Clear();
            pFrmResult.panelDockContainerMatchedResult.Controls.Add(frmShowMatchedResult);
            pFrmResult.tabControl = ClsControl.m_MainTabControl;
            pFrmResult.m_BottomStandBar = ClsControl.m_BottomStandBar;
            frmShowMatchedResult.Show();

            //打开数据表项
            ZJGISOpenData.Forms.FrmOpenData frmOpenData = new ZJGISOpenData.Forms.FrmOpenData();
            frmOpenData.IsShowTable = true;
            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string pathName = frmOpenData.PathName;
            Collection<object> tableClsCol = new Collection<object>();
            tableClsCol = frmOpenData.TableCollection;
            string tableName = string.Empty;
            if (tableClsCol[0] != null)
            {

                IDataset dataset = tableClsCol[0] as IDataset;
                tableName = dataset.Name;
                IWorkspaceFactory workspaceFactory = new FileGDBWorkspaceFactory();
                string workspacePath = frmOpenData.PathName;
                //workspacePath = workspacePath.Substring(0,workspacePath.LastIndexOf(@"\"));

                IWorkspace2 workspace = workspaceFactory.OpenFromFile(workspacePath, 0) as IWorkspace2;
                ITable tableSetting = null;
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                IFeatureClass tuFeatCls = null;
                IFeatureClass teFeatCls = null;
                IFeatureLayer tuFeatLyr = new FeatureLayerClass();
                IFeatureLayer teFeatLyr = new FeatureLayerClass();

                if (tableName.Contains("_PyTable") || tableName.Contains("_py") || tableName.Contains("_Py") || tableName.Contains("_pY"))
                {
                    if (workspace.get_NameExists(esriDatasetType.esriDTTable, "MatchedPolygonFCSetting"))
                    {
                        featureWorkspace = workspace as IFeatureWorkspace;
                        tableSetting = featureWorkspace.OpenTable("MatchedPolygonFCSetting");
                    }
                    else
                    {
                        MessageBoxEx.Show("匹配设置表不存在，无法加载数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ICursor cursor = tableSetting.Search(null, false);
                    IRow row = new RowClass();
                    row = cursor.NextRow();
                    string matchedFCName = tableName.Substring(0, tableName.LastIndexOf("_"));//由于创建表时加了_table
                    while (row != null)
                    {
                        if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)//判断是否为工作层的表
                        {
                            string sourceName = row.get_Value(row.Fields.FindField("SourceFCName")).ToString(); //通过设置表找到图层路径与图层名
                            string sourcePath = row.get_Value(row.Fields.FindField("SourcePath")).ToString();
                            //添加图层到MapControl中
                            if (sourcePath.Contains(".gdb"))
                            {
                                IWorkspaceFactory sourceWF = new FileGDBWorkspaceFactoryClass();
                                if (sourcePath.Substring(sourcePath.LastIndexOf(".")) == ".gdb")
                                {
                                    IFeatureWorkspace sourceFeatWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                    tuFeatCls = sourceFeatWorkspace.OpenFeatureClass(sourceName);
                                    tuFeatLyr.FeatureClass = tuFeatCls;
                                    frmShowMatchedResult.TUFeatCls = tuFeatCls;

                                    tuFeatLyr.Name = tuFeatCls.AliasName;
                                    ILayer slayer = tuFeatLyr as ILayer;
                                    MapFrom.ActiveView.FocusMap.ClearLayers();
                                    MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                    MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                    teFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);//？注意
                                    teFeatLyr.FeatureClass = teFeatCls;
                                    frmShowMatchedResult.TEFeatCls = teFeatCls;

                                    teFeatLyr.Name = teFeatCls.AliasName;
                                    ILayer tlayer = teFeatLyr as ILayer;
                                    MapTo.ActiveView.FocusMap.ClearLayers();
                                    MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                    MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);

                                    break;
                                }
                                else
                                {
                                    string featureDataset = sourcePath.Substring(sourcePath.LastIndexOf(@"\") + 1);
                                    sourcePath = sourcePath.Substring(0, sourcePath.LastIndexOf(@"\"));
                                    IFeatureWorkspace sourceFeatureWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                    tuFeatCls = sourceFeatureWorkspace.OpenFeatureClass(sourceName);
                                    tuFeatLyr.FeatureClass = tuFeatCls;
                                    frmShowMatchedResult.TUFeatCls = tuFeatCls;

                                    tuFeatLyr.Name = tuFeatCls.AliasName;
                                    ILayer slayer = tuFeatLyr as ILayer;
                                    MapFrom.ActiveView.FocusMap.ClearLayers();
                                    MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                    MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                    teFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName + "_TE");
                                    teFeatLyr.FeatureClass = teFeatCls;
                                    frmShowMatchedResult.TEFeatCls = teFeatCls;

                                    teFeatLyr.Name = teFeatCls.AliasName;
                                    ILayer tlayer = teFeatLyr as ILayer;
                                    MapTo.ActiveView.FocusMap.ClearLayers();
                                    MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                    MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);


                                    break;
                                }
                            }
                            else
                            {
                                IWorkspaceFactory shpWorkspaceFactory = new ShapefileWorkspaceFactory();
                                IFeatureWorkspace shpFeatWorkspace = shpWorkspaceFactory.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                tuFeatCls = shpFeatWorkspace.OpenFeatureClass(sourceName);
                                tuFeatLyr.FeatureClass = tuFeatCls;
                                frmShowMatchedResult.TUFeatCls = tuFeatCls;
                                tuFeatLyr.Name = tuFeatCls.AliasName;
                                ILayer slayer = tuFeatLyr as ILayer;
                                MapFrom.ActiveView.FocusMap.ClearLayers();
                                MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);


                                teFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                teFeatLyr.FeatureClass = teFeatCls;
                                frmShowMatchedResult.TEFeatCls = teFeatCls;

                                teFeatLyr.Name = teFeatCls.AliasName;
                                ILayer tlayer = teFeatLyr as ILayer;
                                MapTo.ActiveView.FocusMap.ClearLayers();
                                MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);



                                break;

                            }
                        }
                        row = cursor.NextRow();
                    }
                }
                else if (tableName.Contains("_LnTable") || tableName.Contains("_ln") || tableName.Contains("_Ln") || tableName.Contains("_lN"))
                {
                    if (workspace.get_NameExists(esriDatasetType.esriDTTable, ClsConstant.lineSettingTable))
                    {
                        featureWorkspace = workspace as IFeatureWorkspace;
                        tableSetting = featureWorkspace.OpenTable(ClsConstant.lineSettingTable);
                    }
                    else
                    {
                        MessageBoxEx.Show("匹配设置表不存在，无法加载数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ICursor cursor = tableSetting.Search(null, false);
                    IRow row = new RowClass();
                    row = cursor.NextRow();
                    string matchedFCName = tableName.Substring(0, tableName.LastIndexOf("_"));
                    while (row != null)
                    {
                        if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)
                        {
                            string sourceName = row.get_Value(row.Fields.FindField("SourceFCName")).ToString();
                            string sourcePath = row.get_Value(row.Fields.FindField("SourcePath")).ToString();

                            if (sourcePath.Contains(".gdb"))
                            {
                                IWorkspaceFactory sourceWF = new FileGDBWorkspaceFactoryClass();
                                if (sourcePath.Substring(sourcePath.LastIndexOf(".")) == ".gdb")
                                {
                                    IFeatureWorkspace sourceFeatWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                    tuFeatCls = sourceFeatWorkspace.OpenFeatureClass(sourceName);
                                    tuFeatLyr.FeatureClass = tuFeatCls;
                                    frmShowMatchedResult.TUFeatCls = tuFeatCls;

                                    tuFeatLyr.Name = tuFeatCls.AliasName;
                                    ILayer slayer = tuFeatLyr as ILayer;
                                    MapFrom.ActiveView.FocusMap.ClearLayers();
                                    MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                    MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                    teFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                    teFeatLyr.FeatureClass = teFeatCls;
                                    frmShowMatchedResult.TEFeatCls = teFeatCls;

                                    teFeatLyr.Name = teFeatCls.AliasName;
                                    ILayer tlayer = teFeatLyr as ILayer;
                                    MapTo.ActiveView.FocusMap.ClearLayers();
                                    MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                    MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                    break;
                                }
                                else
                                {
                                    string featureDataset = sourcePath.Substring(sourcePath.LastIndexOf(@"\") + 1);
                                    sourcePath = sourcePath.Substring(0, sourcePath.LastIndexOf(@"\"));
                                    IFeatureWorkspace sourceFeatureWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                    tuFeatCls = sourceFeatureWorkspace.OpenFeatureClass(sourceName);
                                    tuFeatLyr.FeatureClass = tuFeatCls;
                                    frmShowMatchedResult.TUFeatCls = tuFeatCls;

                                    tuFeatLyr.Name = tuFeatCls.AliasName;
                                    ILayer slayer = tuFeatLyr as ILayer;
                                    MapFrom.ActiveView.FocusMap.ClearLayers();
                                    MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                    MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                    teFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName + "_TE");
                                    teFeatLyr.FeatureClass = teFeatCls;
                                    frmShowMatchedResult.TEFeatCls = teFeatCls;

                                    teFeatLyr.Name = teFeatCls.AliasName;
                                    ILayer tlayer = teFeatLyr as ILayer;
                                    MapTo.ActiveView.FocusMap.ClearLayers();
                                    MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                    MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);

                                    break;
                                }
                            }
                            else
                            {
                                IWorkspaceFactory shpWorkspaceFactory = new ShapefileWorkspaceFactory();
                                IFeatureWorkspace shpFeatWorkspace = shpWorkspaceFactory.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                tuFeatCls = shpFeatWorkspace.OpenFeatureClass(sourceName);
                                tuFeatLyr.FeatureClass = tuFeatCls;
                                frmShowMatchedResult.TUFeatCls = tuFeatCls;

                                tuFeatLyr.Name = tuFeatCls.AliasName;
                                ILayer slayer = tuFeatLyr as ILayer;
                                MapFrom.ActiveView.FocusMap.ClearLayers();
                                MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                teFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                teFeatLyr.FeatureClass = teFeatCls;
                                frmShowMatchedResult.TEFeatCls = teFeatCls;

                                teFeatLyr.Name = teFeatCls.AliasName;
                                ILayer tlayer = teFeatLyr as ILayer;
                                MapTo.ActiveView.FocusMap.ClearLayers();
                                MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                break;

                            }
                        }
                        row = cursor.NextRow();
                    }
                }
                else if (tableName.Contains("_PtTable") || tableName.Contains("_ln") || tableName.Contains("_Ln") || tableName.Contains("_lN"))
                {
                    if (workspace.get_NameExists(esriDatasetType.esriDTTable, ClsConstant.pointSettingTable))
                    {
                        featureWorkspace = workspace as IFeatureWorkspace;
                        tableSetting = featureWorkspace.OpenTable(ClsConstant.pointSettingTable);
                    }
                    else
                    {
                        MessageBoxEx.Show("匹配设置表不存在，无法加载数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ICursor cursor = tableSetting.Search(null, false);
                    IRow row = new RowClass();
                    row = cursor.NextRow();
                    string matchedFCName = tableName.Substring(0, tableName.LastIndexOf("_"));
                    while (row != null)
                    {
                        if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)
                        {
                            string sourceName = row.get_Value(row.Fields.FindField("SourceFCName")).ToString();
                            string sourcePath = row.get_Value(row.Fields.FindField("SourcePath")).ToString();

                            if (sourcePath.Contains(".gdb"))
                            {
                                IWorkspaceFactory sourceWF = new FileGDBWorkspaceFactoryClass();
                                if (sourcePath.Substring(sourcePath.LastIndexOf(".")) == ".gdb")
                                {
                                    IFeatureWorkspace sourceFeatWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                    tuFeatCls = sourceFeatWorkspace.OpenFeatureClass(sourceName);
                                    tuFeatLyr.FeatureClass = tuFeatCls;
                                    frmShowMatchedResult.TUFeatCls = tuFeatCls;

                                    tuFeatLyr.Name = tuFeatCls.AliasName;
                                    ILayer slayer = tuFeatLyr as ILayer;
                                    MapFrom.ActiveView.FocusMap.ClearLayers();
                                    MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                    MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                    teFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                    teFeatLyr.FeatureClass = teFeatCls;
                                    frmShowMatchedResult.TEFeatCls = teFeatCls;

                                    teFeatLyr.Name = teFeatCls.AliasName;
                                    ILayer tlayer = teFeatLyr as ILayer;
                                    MapTo.ActiveView.FocusMap.ClearLayers();
                                    MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                    MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                    break;
                                }
                                else
                                {
                                    string featureDataset = sourcePath.Substring(sourcePath.LastIndexOf(@"\") + 1);
                                    sourcePath = sourcePath.Substring(0, sourcePath.LastIndexOf(@"\"));
                                    IFeatureWorkspace sourceFeatureWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                    tuFeatCls = sourceFeatureWorkspace.OpenFeatureClass(sourceName);
                                    tuFeatLyr.FeatureClass = tuFeatCls;
                                    frmShowMatchedResult.TUFeatCls = tuFeatCls;

                                    tuFeatLyr.Name = tuFeatCls.AliasName;
                                    ILayer slayer = tuFeatLyr as ILayer;
                                    MapFrom.ActiveView.FocusMap.ClearLayers();
                                    MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                    MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                    teFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                    teFeatLyr.FeatureClass = teFeatCls;
                                    frmShowMatchedResult.TEFeatCls = teFeatCls;

                                    teFeatLyr.Name = teFeatCls.AliasName;
                                    ILayer tlayer = teFeatLyr as ILayer;
                                    MapTo.ActiveView.FocusMap.ClearLayers();
                                    MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                    MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);

                                    break;
                                }
                            }
                            else
                            {
                                IWorkspaceFactory shpWorkspaceFactory = new ShapefileWorkspaceFactory();
                                IFeatureWorkspace shpFeatWorkspace = shpWorkspaceFactory.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                tuFeatCls = shpFeatWorkspace.OpenFeatureClass(sourceName);
                                tuFeatLyr.FeatureClass = tuFeatCls;
                                frmShowMatchedResult.TUFeatCls = tuFeatCls;

                                tuFeatLyr.Name = tuFeatCls.AliasName;
                                ILayer slayer = tuFeatLyr as ILayer;
                                MapFrom.ActiveView.FocusMap.ClearLayers();
                                MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                teFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                teFeatLyr.FeatureClass = teFeatCls;
                                frmShowMatchedResult.TEFeatCls = teFeatCls;

                                teFeatLyr.Name = teFeatCls.AliasName;
                                ILayer tlayer = teFeatLyr as ILayer;
                                MapTo.ActiveView.FocusMap.ClearLayers();
                                MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                break;

                            }
                        }
                        row = cursor.NextRow();
                    }
                }


                if (pFrmResult.IsDisposed)
                {
                    frmShowMatchedResult = new FrmShowMatchedResult();
                    pFrmResult = new FrmResult();
                    frmShowMatchedResult.TopLevel = false;
                    frmShowMatchedResult.Size = pFrmResult.panelDockContainerMatchedResult.Size;

                    frmShowMatchedResult.MapControlFrom = ClsControl.m_MapControlFrom;
                    frmShowMatchedResult.MapControlTo = ClsControl.m_MapControlTo;
                    frmShowMatchedResult.MapControlOverlap = ClsControl.m_MapControlOverlap;
                    frmShowMatchedResult.tabControl = ClsControl.m_MainTabControl;
                    frmShowMatchedResult.DGVFrom = ClsControl.m_DGVFrom;
                    frmShowMatchedResult.DGVTo = ClsControl.m_DGVTo;

                    pFrmResult.panelDockContainerMatchedResult.Controls.Clear();
                    pFrmResult.panelDockContainerMatchedResult.Controls.Add(frmShowMatchedResult);
                    pFrmResult.tabControl = ClsControl.m_MainTabControl;
                    pFrmResult.m_BottomStandBar = ClsControl.m_BottomStandBar;
                    frmShowMatchedResult.Show();
                }
                if (tableName.Contains("Table"))
                {

                    //if (barStandBottom.Visible == false)
                    //{
                    //    barStandBottom.Visible = true;
                    //}
                    frmShowMatchedResult.DGVX.DataSource = null;
                    frmShowMatchedResult.Table = tableClsCol[0] as ITable;
                    frmShowMatchedResult.AddITableToDGV(frmShowMatchedResult.Table, null);
                    pFrmResult.Show();
                }
                else
                {
                    MessageBoxEx.Show("请加载正确的匹配结果表！");
                    return;
                }
            }


        }
        /// <summary>
        /// 变化检测（多尺度）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateCheckDif_Click(object sender, EventArgs e)
        {
            FrmMatchSet frmMatchSet = new FrmMatchSet();
            //20170612注释
            //             if (comboBoxItem1.SelectedItem == null || comboBoxItem2.SelectedItem == null)
            //             {
            //                 MessageBoxEx.Show("请选择更新数据比例尺！");
            //                 return;
            //             }
            ZJGISDataUpdating.Class.ClsDeclare.g_SameScaleMatch = false;
            ZJGISDataUpdating.Class.ClsDeclare.g_DifScaleMatch = true;
            //             ZJGISDataUpdating.ClsDeclare.strFrom = comboBoxItem1.SelectedItem.ToString();
            //             ZJGISDataUpdating.ClsDeclare.strTo = comboBoxItem2.SelectedItem.ToString();
            frmMatchSet.ShowDialog();
        }
        /// <summary>
        /// 编辑匹配结果表（只有线和面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditMathTabDif_Click(object sender, EventArgs e)
        {
            FrmResult pFrmResult;
            FrmShowMatchedResult frmShowMatchedResult;
            //匹配结果表
            pFrmResult = new FrmResult();
            frmShowMatchedResult = new FrmShowMatchedResult();

            frmShowMatchedResult.TopLevel = false;
            frmShowMatchedResult.Size = pFrmResult.panelDockContainerMatchedResult.Size;

            //源图层、待匹配图层和待叠加图层的MapControl
            //所以要查看结果表，必须加载两个图层
            frmShowMatchedResult.MapControlFrom = ClsControl.m_MapControlFrom;
            frmShowMatchedResult.MapControlTo = ClsControl.m_MapControlTo;

            frmShowMatchedResult.MapControlOverlap = ClsControl.m_MapControlOverlap;
            frmShowMatchedResult.tabControl = ClsControl.m_MainTabControl;

            frmShowMatchedResult.DGVFrom = ClsControl.m_DGVFrom;
            frmShowMatchedResult.DGVTo = ClsControl.m_DGVTo;

            pFrmResult.panelDockContainerMatchedResult.Controls.Clear();
            //把结果表窗体添加到结果结果中
            pFrmResult.panelDockContainerMatchedResult.Controls.Add(frmShowMatchedResult);
            pFrmResult.tabControl = ClsControl.m_MainTabControl;
            pFrmResult.m_BottomStandBar = ClsControl.m_BottomStandBar;
            frmShowMatchedResult.Show();

            //打开结果表
            ZJGISOpenData.Forms.FrmOpenData frmOpenData = new ZJGISOpenData.Forms.FrmOpenData();
            frmOpenData.IsShowTable = true;
            if (frmOpenData.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string pathName = frmOpenData.PathName;

            Collection<object> tableClsCol = new Collection<object>();
            tableClsCol = frmOpenData.TableCollection;
            if (tableClsCol[0] != null)
            {
                IDataset dataset = tableClsCol[0] as IDataset;
                string tableName = dataset.Name;
                IWorkspaceFactory workspaceFactory = new FileGDBWorkspaceFactory();
                string workspacePath = frmOpenData.PathName;
                //workspacePath = workspacePath.Substring(0,workspacePath.LastIndexOf(@"\"));

                IWorkspace2 workspace = workspaceFactory.OpenFromFile(workspacePath, 0) as IWorkspace2;
                ITable tableSetting = null;
                IFeatureWorkspace featureWorkspace = workspace as IFeatureWorkspace;
                IFeatureClass sourceFeatCls = null;
                IFeatureClass targetFeatCls = null;
                IFeatureLayer sourceFeatLyr = new FeatureLayerClass();
                IFeatureLayer targetFeatLyr = new FeatureLayerClass();
                if (this.mapMain.LayerCount > 0)
                {
                    //如果为面匹配结果表
                    if (tableName.Contains("_DifPyTable") || tableName.Contains("_py") || tableName.Contains("_Py") || tableName.Contains("_pY"))
                    {
                        if (workspace.get_NameExists(esriDatasetType.esriDTTable, ClsConstant.polygonSettingTable))
                        {
                            featureWorkspace = workspace as IFeatureWorkspace;
                            tableSetting = featureWorkspace.OpenTable(ClsConstant.polygonSettingTable);
                        }
                        else
                        {
                            MessageBox.Show("匹配设置表不存在，无法加载数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        ICursor cursor = tableSetting.Search(null, false);
                        IRow row = new RowClass();
                        row = cursor.NextRow();
                        string matchedFCName = tableName.Substring(0, tableName.LastIndexOf("_"));//由于创建表时加了_table
                        while (row != null)
                        {
                            if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)//判断是否为工作层的表
                            {
                                string sourceName = row.get_Value(row.Fields.FindField("SourceFCName")).ToString(); //通过设置表找到图层路径与图层名
                                string sourcePath = row.get_Value(row.Fields.FindField("SourcePath")).ToString();

                                if (sourcePath.Contains(".gdb"))
                                {
                                    IWorkspaceFactory sourceWF = new FileGDBWorkspaceFactoryClass();
                                    if (sourcePath.Substring(sourcePath.LastIndexOf(".")) == ".gdb")
                                    {
                                        IFeatureWorkspace sourceFeatWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                        sourceFeatCls = sourceFeatWorkspace.OpenFeatureClass(sourceName);
                                        sourceFeatLyr.FeatureClass = sourceFeatCls;
                                        frmShowMatchedResult.TUFeatCls = sourceFeatCls;

                                        sourceFeatLyr.Name = sourceFeatCls.AliasName;
                                        ILayer slayer = sourceFeatLyr as ILayer;
                                        MapFrom.ActiveView.FocusMap.ClearLayers();
                                        MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                        MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                        MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                        targetFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);//？注意
                                        targetFeatLyr.FeatureClass = targetFeatCls;
                                        frmShowMatchedResult.TEFeatCls = targetFeatCls;

                                        targetFeatLyr.Name = targetFeatCls.AliasName;
                                        ILayer tlayer = targetFeatLyr as ILayer;
                                        MapTo.ActiveView.FocusMap.ClearLayers();
                                        MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                        MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);

                                        break;
                                    }
                                    else
                                    {
                                        string featureDataset = sourcePath.Substring(sourcePath.LastIndexOf(@"\") + 1);
                                        sourcePath = sourcePath.Substring(0, sourcePath.LastIndexOf(@"\"));
                                        IFeatureWorkspace sourceFeatureWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                        sourceFeatCls = sourceFeatureWorkspace.OpenFeatureClass(sourceName);
                                        sourceFeatLyr.FeatureClass = sourceFeatCls;
                                        frmShowMatchedResult.TUFeatCls = sourceFeatCls;

                                        sourceFeatLyr.Name = sourceFeatCls.AliasName;
                                        ILayer slayer = sourceFeatLyr as ILayer;
                                        MapFrom.ActiveView.FocusMap.ClearLayers();
                                        MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                        MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                        MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                        targetFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                        targetFeatLyr.FeatureClass = targetFeatCls;
                                        frmShowMatchedResult.TEFeatCls = targetFeatCls;

                                        targetFeatLyr.Name = targetFeatCls.AliasName;
                                        ILayer tlayer = targetFeatLyr as ILayer;
                                        MapTo.ActiveView.FocusMap.ClearLayers();
                                        MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                        MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                        break;
                                    }
                                }
                                else
                                {
                                    IWorkspaceFactory shpWorkspaceFactory = new ShapefileWorkspaceFactory();
                                    IFeatureWorkspace shpFeatWorkspace = shpWorkspaceFactory.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                    sourceFeatCls = shpFeatWorkspace.OpenFeatureClass(sourceName);
                                    sourceFeatLyr.FeatureClass = sourceFeatCls;
                                    frmShowMatchedResult.TUFeatCls = sourceFeatCls;
                                    sourceFeatLyr.Name = sourceFeatCls.AliasName;
                                    ILayer slayer = sourceFeatLyr as ILayer;
                                    MapFrom.ActiveView.FocusMap.ClearLayers();
                                    MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                    MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);


                                    targetFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                    targetFeatLyr.FeatureClass = targetFeatCls;
                                    frmShowMatchedResult.TEFeatCls = targetFeatCls;

                                    targetFeatLyr.Name = targetFeatCls.AliasName;
                                    ILayer tlayer = targetFeatLyr as ILayer;
                                    MapTo.ActiveView.FocusMap.ClearLayers();
                                    MapTo.ActiveView.FocusMap.AddLayer(tlayer);
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                    break;
                                }
                            }
                            row = cursor.NextRow();
                        }
                    }
                    //如果为线匹配结果表                
                    else if (tableName.Contains("_DifLnTable") || tableName.Contains("_ln") || tableName.Contains("_Ln") || tableName.Contains("_lN"))
                    {
                        //必须存在表——MatchedPolylineFCSetting，还要遍历这个表呢
                        if (workspace.get_NameExists(esriDatasetType.esriDTTable, ClsConstant.lineSettingTable))
                        {
                            featureWorkspace = workspace as IFeatureWorkspace;
                            tableSetting = featureWorkspace.OpenTable(ClsConstant.lineSettingTable);
                        }
                        else
                        {
                            MessageBox.Show("匹配设置表不存在，无法加载数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //遍历MatchedPolylineFCSetting
                        ICursor cursor = tableSetting.Search(null, false);
                        IRow row = new RowClass();
                        row = cursor.NextRow();
                        string matchedFCName = tableName.Substring(0, tableName.LastIndexOf("_"));
                        while (row != null)
                        {
                            if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)
                            {
                                string sourceName = row.get_Value(row.Fields.FindField("SourceFCName")).ToString();
                                string sourcePath = row.get_Value(row.Fields.FindField("SourcePath")).ToString();

                                if (sourcePath.Contains(".gdb"))
                                {
                                    IWorkspaceFactory sourceWF = new FileGDBWorkspaceFactoryClass();
                                    if (sourcePath.Substring(sourcePath.LastIndexOf(".")) == ".gdb")
                                    {
                                        IFeatureWorkspace sourceFeatWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                        sourceFeatCls = sourceFeatWorkspace.OpenFeatureClass(sourceName);
                                        sourceFeatLyr.FeatureClass = sourceFeatCls;
                                        frmShowMatchedResult.TUFeatCls = sourceFeatCls;

                                        sourceFeatLyr.Name = sourceFeatCls.AliasName;
                                        ILayer slayer = sourceFeatLyr as ILayer;
                                        MapFrom.ActiveView.FocusMap.ClearLayers();
                                        MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                        MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                        MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                        targetFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                        targetFeatLyr.FeatureClass = targetFeatCls;
                                        frmShowMatchedResult.TEFeatCls = targetFeatCls;

                                        targetFeatLyr.Name = targetFeatCls.AliasName;
                                        ILayer tlayer = targetFeatLyr as ILayer;
                                        MapTo.ActiveView.FocusMap.ClearLayers();
                                        MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                        MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                        break;
                                    }
                                    else
                                    {
                                        string featureDataset = sourcePath.Substring(sourcePath.LastIndexOf(@"\") + 1);
                                        sourcePath = sourcePath.Substring(0, sourcePath.LastIndexOf(@"\"));
                                        IFeatureWorkspace sourceFeatureWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                        sourceFeatCls = sourceFeatureWorkspace.OpenFeatureClass(sourceName);
                                        sourceFeatLyr.FeatureClass = sourceFeatCls;
                                        frmShowMatchedResult.TUFeatCls = sourceFeatCls;

                                        sourceFeatLyr.Name = sourceFeatCls.AliasName;
                                        ILayer slayer = sourceFeatLyr as ILayer;
                                        MapFrom.ActiveView.FocusMap.ClearLayers();
                                        MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                        MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                        MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                        targetFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                        targetFeatLyr.FeatureClass = targetFeatCls;
                                        frmShowMatchedResult.TEFeatCls = targetFeatCls;

                                        targetFeatLyr.Name = targetFeatCls.AliasName;
                                        ILayer tlayer = targetFeatLyr as ILayer;
                                        MapTo.ActiveView.FocusMap.ClearLayers();
                                        MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                        MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);

                                        break;
                                    }
                                }
                                else
                                {
                                    IWorkspaceFactory shpWorkspaceFactory = new ShapefileWorkspaceFactory();
                                    IFeatureWorkspace shpFeatWorkspace = shpWorkspaceFactory.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                    sourceFeatCls = shpFeatWorkspace.OpenFeatureClass(sourceName);
                                    sourceFeatLyr.FeatureClass = sourceFeatCls;
                                    frmShowMatchedResult.TUFeatCls = sourceFeatCls;

                                    sourceFeatLyr.Name = sourceFeatCls.AliasName;
                                    ILayer slayer = sourceFeatLyr as ILayer;
                                    MapFrom.ActiveView.FocusMap.ClearLayers();
                                    MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                    MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                    targetFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                    targetFeatLyr.FeatureClass = targetFeatCls;
                                    frmShowMatchedResult.TEFeatCls = targetFeatCls;

                                    targetFeatLyr.Name = targetFeatCls.AliasName;
                                    ILayer tlayer = targetFeatLyr as ILayer;
                                    MapTo.ActiveView.FocusMap.ClearLayers();
                                    MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                    MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                    break;


                                }
                            }
                            row = cursor.NextRow();
                        }
                    }
                    //如果为点匹配结果表                
                    else if (tableName.Contains("_PtTable") || tableName.Contains("_pt") || tableName.Contains("_Pt") || tableName.Contains("_pT"))
                    {
                        //必须存在表——MatchedPointFCSetting，还要遍历这个表呢
                        //if (workspace.get_NameExists(esriDatasetType.esriDTTable, ClsConstant.lineSettingTable))
                        if (workspace.get_NameExists(esriDatasetType.esriDTTable, ClsConstant.pointSettingTable))
                        {
                            featureWorkspace = workspace as IFeatureWorkspace;
                            //tableSetting = featureWorkspace.OpenTable(ClsConstant.lineSettingTable);
                            tableSetting = featureWorkspace.OpenTable(ClsConstant.pointSettingTable);
                        }
                        else
                        {
                            MessageBox.Show("匹配设置表不存在，无法加载数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //遍历MatchedPolylineFCSetting
                        ICursor cursor = tableSetting.Search(null, false);
                        IRow row = new RowClass();
                        row = cursor.NextRow();
                        string matchedFCName = tableName.Substring(0, tableName.LastIndexOf("_"));
                        while (row != null)
                        {
                            if (row.get_Value(row.Fields.FindField("MatchedFCName")).ToString() == matchedFCName)
                            {
                                string sourceName = row.get_Value(row.Fields.FindField("SourceFCName")).ToString();
                                string sourcePath = row.get_Value(row.Fields.FindField("SourcePath")).ToString();

                                if (sourcePath.Contains(".gdb"))
                                {
                                    IWorkspaceFactory sourceWF = new FileGDBWorkspaceFactoryClass();
                                    if (sourcePath.Substring(sourcePath.LastIndexOf(".")) == ".gdb")
                                    {
                                        IFeatureWorkspace sourceFeatWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                        sourceFeatCls = sourceFeatWorkspace.OpenFeatureClass(sourceName);
                                        sourceFeatLyr.FeatureClass = sourceFeatCls;
                                        frmShowMatchedResult.TUFeatCls = sourceFeatCls;

                                        sourceFeatLyr.Name = sourceFeatCls.AliasName;
                                        ILayer slayer = sourceFeatLyr as ILayer;
                                        MapFrom.ActiveView.FocusMap.ClearLayers();
                                        MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                        MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                        MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                        targetFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                        targetFeatLyr.FeatureClass = targetFeatCls;
                                        frmShowMatchedResult.TEFeatCls = targetFeatCls;

                                        targetFeatLyr.Name = targetFeatCls.AliasName;
                                        ILayer tlayer = targetFeatLyr as ILayer;
                                        MapTo.ActiveView.FocusMap.ClearLayers();
                                        MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                        MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                        break;
                                    }
                                    else
                                    {
                                        string featureDataset = sourcePath.Substring(sourcePath.LastIndexOf(@"\") + 1);
                                        sourcePath = sourcePath.Substring(0, sourcePath.LastIndexOf(@"\"));
                                        IFeatureWorkspace sourceFeatureWorkspace = sourceWF.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                        sourceFeatCls = sourceFeatureWorkspace.OpenFeatureClass(sourceName);
                                        sourceFeatLyr.FeatureClass = sourceFeatCls;
                                        frmShowMatchedResult.TUFeatCls = sourceFeatCls;

                                        sourceFeatLyr.Name = sourceFeatCls.AliasName;
                                        ILayer slayer = sourceFeatLyr as ILayer;
                                        MapFrom.ActiveView.FocusMap.ClearLayers();
                                        MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                        MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                        MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                        targetFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                        targetFeatLyr.FeatureClass = targetFeatCls;
                                        frmShowMatchedResult.TEFeatCls = targetFeatCls;

                                        targetFeatLyr.Name = targetFeatCls.AliasName;
                                        ILayer tlayer = targetFeatLyr as ILayer;
                                        MapTo.ActiveView.FocusMap.ClearLayers();
                                        MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                        MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);

                                        break;
                                    }
                                }
                                else
                                {
                                    IWorkspaceFactory shpWorkspaceFactory = new ShapefileWorkspaceFactory();
                                    IFeatureWorkspace shpFeatWorkspace = shpWorkspaceFactory.OpenFromFile(sourcePath, 0) as IFeatureWorkspace;
                                    sourceFeatCls = shpFeatWorkspace.OpenFeatureClass(sourceName);
                                    sourceFeatLyr.FeatureClass = sourceFeatCls;
                                    frmShowMatchedResult.TUFeatCls = sourceFeatCls;

                                    sourceFeatLyr.Name = sourceFeatCls.AliasName;
                                    ILayer slayer = sourceFeatLyr as ILayer;
                                    MapFrom.ActiveView.FocusMap.ClearLayers();
                                    MapFrom.ActiveView.FocusMap.AddLayer(slayer);

                                    MapOverlapping.ActiveView.FocusMap.ClearLayers();
                                    MapOverlapping.ActiveView.FocusMap.AddLayer(slayer);

                                    targetFeatCls = featureWorkspace.OpenFeatureClass(matchedFCName);
                                    targetFeatLyr.FeatureClass = targetFeatCls;
                                    frmShowMatchedResult.TEFeatCls = targetFeatCls;

                                    targetFeatLyr.Name = targetFeatCls.AliasName;
                                    ILayer tlayer = targetFeatLyr as ILayer;
                                    MapTo.ActiveView.FocusMap.ClearLayers();
                                    MapTo.ActiveView.FocusMap.AddLayer(tlayer);

                                    MapOverlapping.ActiveView.FocusMap.AddLayer(tlayer);
                                    break;


                                }
                            }
                            row = cursor.NextRow();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先添加已匹配和待匹配的图层！");
                }
                //if (barStandBottom.Visible == false)
                //{
                //    barStandBottom.Visible = true;
                //}

                if (pFrmResult.IsDisposed)
                {
                    frmShowMatchedResult = new FrmShowMatchedResult();
                    pFrmResult = new FrmResult();
                    frmShowMatchedResult.TopLevel = false;
                    frmShowMatchedResult.Size = pFrmResult.panelDockContainerMatchedResult.Size;

                    frmShowMatchedResult.MapControlFrom = ClsControl.m_MapControlFrom;
                    frmShowMatchedResult.MapControlTo = ClsControl.m_MapControlTo;
                    frmShowMatchedResult.MapControlOverlap = ClsControl.m_MapControlOverlap;
                    frmShowMatchedResult.tabControl = ClsControl.m_MainTabControl;
                    frmShowMatchedResult.DGVFrom = ClsControl.m_DGVFrom;
                    frmShowMatchedResult.DGVTo = ClsControl.m_DGVTo;

                    pFrmResult.panelDockContainerMatchedResult.Controls.Clear();
                    pFrmResult.panelDockContainerMatchedResult.Controls.Add(frmShowMatchedResult);
                    pFrmResult.tabControl = ClsControl.m_MainTabControl;
                    pFrmResult.m_BottomStandBar = ClsControl.m_BottomStandBar;
                    frmShowMatchedResult.Show();
                }
                frmShowMatchedResult.DGVX.DataSource = null;
                frmShowMatchedResult.Table = tableClsCol[0] as ITable;
                frmShowMatchedResult.TableName = tableName;
                frmShowMatchedResult.AddITableToDGV(frmShowMatchedResult.Table, null);
                pFrmResult.Show();
            }

        }
        /// <summary>
        /// 更新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDUpdate_Click(object sender, EventArgs e)
        {
            FrmUpdate pFrmUpdate = new FrmUpdate();
            pFrmUpdate.Show();

        }
        private void checkBoxItemSourceLayer_CheckedChanged(object sender, CheckBoxChangeEventArgs e)
        {
            if (checkBoxItemSourceLayer.Checked)
            {
                if (!this.MapOverlapping.ActiveView.FocusMap.get_Layer(1).Visible)
                {
                    this.MapOverlapping.ActiveView.FocusMap.get_Layer(1).Visible = true;
                    this.MapOverlapping.ActiveView.Refresh();
                    this.MapFrom.ActiveView.Refresh();
                }
            }
            else if (!checkBoxItemSourceLayer.Checked)
            {
                if (this.MapOverlapping.ActiveView.FocusMap.get_Layer(1).Visible)
                {
                    this.MapOverlapping.ActiveView.FocusMap.get_Layer(1).Visible = false;
                    this.MapOverlapping.ActiveView.Refresh();
                    this.MapFrom.ActiveView.Refresh();
                }
            }
        }

        private void checkBoxItemUpdatedLayer_CheckedChanged(object sender, CheckBoxChangeEventArgs e)
        {
            if (checkBoxItemUpdatedLayer.Checked)
            {
                if (!this.MapOverlapping.ActiveView.FocusMap.get_Layer(0).Visible)
                {
                    this.MapOverlapping.ActiveView.FocusMap.get_Layer(0).Visible = true;
                    this.MapTo.ActiveView.Refresh();
                    this.MapOverlapping.ActiveView.Refresh();
                }
            }
            if (!checkBoxItemUpdatedLayer.Checked)
            {
                if (this.MapOverlapping.ActiveView.FocusMap.get_Layer(0).Visible)
                {
                    this.MapOverlapping.ActiveView.FocusMap.get_Layer(0).Visible = false;
                    this.MapTo.ActiveView.Refresh();
                    this.MapOverlapping.ActiveView.Refresh();
                }
            }
        }
        /// <summary>
        ///分屏对比-放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemZoomIn_Click(object sender, EventArgs e)
        {
            //ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapZoomInToolClass();
            //ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapZoomInFixedCommandClass();
            if (this.comboBoxItemToMapControl.SelectedItem != null)
            {
                if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "源图层")
                {
                    pMouseOperate = "ZoomIn";

                    //pCmd.OnCreate(this.MapFrom.Object);
                    //pCmd.OnClick();
                    //this.MapFrom.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;
                }
                else if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "待匹配图层")
                {
                    pMouseOperate = "ZoomIn";

                    //pCmd.OnCreate(this.MapTo.Object);
                    //pCmd.OnClick();
                    //this.MapTo.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;
                }
            }
            else
            {
                MessageBoxEx.Show("请选择图层！");
            }
        }
        /// <summary>
        /// 分屏对比-缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemZoomOut_Click(object sender, EventArgs e)
        {
            //ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapZoomOutToolClass();
            //ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapZoomOutFixedCommandClass();
            if (this.comboBoxItemToMapControl.SelectedItem != null)
            {
                if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "源图层")
                {
                    pMouseOperate = "ZoomOut";

                    //pCmd.OnCreate(this.MapFrom.Object);
                    //pCmd.OnClick();
                    //this.MapFrom.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;
                }
                else if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "待匹配图层")
                {
                    pMouseOperate = "ZoomOut";

                    //pCmd.OnCreate(this.MapTo.Object);
                    //pCmd.OnClick();
                    //this.MapTo.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;
                }
            }
            else
            {
                MessageBoxEx.Show("请选择图层！");
            }
        }
        /// <summary>
        /// 分屏对比-漫游
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemPan_Click(object sender, EventArgs e)
        {
            //ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapPanToolClass();
            if (this.comboBoxItemToMapControl.SelectedItem != null)
            {
                if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "源图层")
                {
                    pMouseOperate = "Pan";

                    //pCmd.OnCreate(this.MapFrom.Object);
                    //pCmd.OnClick();
                    //this.MapFrom.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;
                }
                else if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "待匹配图层")
                {
                    pMouseOperate = "Pan";

                    //pCmd.OnCreate(this.MapTo.Object);
                    //pCmd.OnClick();
                    //this.MapTo.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;
                }
            }
            else
            {
                MessageBoxEx.Show("请选择图层！");
            }
        }
        /// <summary>
        /// 分屏对比-选择图层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemSelectFeat_Click(object sender, EventArgs e)
        {
            pMouseOperate = "";
            ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsSelectFeaturesToolClass();
            if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "源图层")
            {
                pCmd.OnCreate(this.MapFrom.Object);
                pCmd.OnClick();
                this.MapFrom.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;
            }
            else if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "待匹配图层")
            {
                pCmd.OnCreate(this.MapTo.Object);
                pCmd.OnClick();
                this.MapTo.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;
            }
        }
        /// <summary>
        /// 分屏对比-全图显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemFullExtent_Click(object sender, EventArgs e)
        {
            if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "源图层")
            {
                commandFullExtent = new ControlsMapFullExtentCommandClass();
                commandFullExtent.OnCreate(MapFrom.Object);
                commandFullExtent.OnClick();
            }
            else if (this.comboBoxItemToMapControl.SelectedItem.ToString() == "待匹配图层")
            {
                commandFullExtent = new ControlsMapFullExtentCommandClass();
                commandFullExtent.OnCreate(MapTo.Object);
                commandFullExtent.OnClick();
            }
        }
        /// <summary>
        /// 叠加对比-放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOverlapZoomIn_Click(object sender, EventArgs e)
        {
            //ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapZoomInToolClass();
            ////ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapZoomInFixedCommandClass();
            //pCmd.OnCreate(this.MapOverlapping.Object);
            //pCmd.OnClick();
            //this.MapOverlapping.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;

            pMouseOperate = "ZoomIn";


            this.btnOverlapZoomIn.Checked = true;
            this.btnOverlapZoomOut.Checked = false;
            this.btnOverlapPan.Checked = false;
            this.btnOverlapSelectFeat.Checked = false;

        }
        /// <summary>
        /// 叠加对比-缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOverlapZoomOut_Click(object sender, EventArgs e)
        {
            //ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapZoomOutToolClass();
            ////ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapZoomOutFixedCommandClass();
            //pCmd.OnCreate(this.MapOverlapping.Object);
            //pCmd.OnClick();
            //this.MapOverlapping.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;

            pMouseOperate = "ZoomOut";

            //pMouseOperate = "ZoomIn";

            this.btnOverlapZoomIn.Checked = false;
            this.btnOverlapZoomOut.Checked = true;
            this.btnOverlapPan.Checked = false;
            this.btnOverlapSelectFeat.Checked = false;
        }

        /// <summary>
        /// 叠加对比-漫游
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOverlapPan_Click(object sender, EventArgs e)
        {

            pMouseOperate = "Pan";

            //ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsMapPanToolClass();
            //pCmd.OnCreate(this.MapOverlapping.Object);
            //pCmd.OnClick();
            //this.MapOverlapping.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;

            this.btnOverlapZoomIn.Checked = false;
            this.btnOverlapZoomOut.Checked = false;
            this.btnOverlapPan.Checked = true;
            this.btnOverlapSelectFeat.Checked = false;

        }
        /// <summary>
        /// 叠加对比-选择要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOverlapSelectFeat_Click(object sender, EventArgs e)
        {
            pMouseOperate = "";
            ESRI.ArcGIS.SystemUI.ICommand pCmd = new ControlsSelectFeaturesToolClass();
            //ESRI.ArcGIS.SystemUI.ICommand pCmd = new ClsSelectFeature();
            pCmd.OnCreate(this.MapOverlapping.Object);
            pCmd.OnClick();
            this.MapOverlapping.CurrentTool = pCmd as ESRI.ArcGIS.SystemUI.ITool;

            this.btnOverlapZoomIn.Checked = false;
            this.btnOverlapZoomOut.Checked = false;
            this.btnOverlapPan.Checked = false;
            this.btnOverlapSelectFeat.Checked = true;
        }
        /// <summary>
        /// 叠加对比-全屏显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOverlapFullExtent_Click(object sender, EventArgs e)
        {
            commandFullExtent = new ControlsMapFullExtentCommandClass();
            commandFullExtent.OnCreate(MapOverlapping.Object);
            commandFullExtent.OnClick();
        }

        string mapName = "";
        IPoint pPoint = null;
        string pMouseOperate = "";
        /// <summary>
        /// 源图层鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapFrom_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            mapName = (sender as AxMapControl).Name;

        }
        /// <summary>
        /// 待匹配图层鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapTo_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            mapName = (sender as AxMapControl).Name;
        }

        private void MapFrom_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            mapName = (sender as AxMapControl).Name;

            //pMouseOperate = "Pan";
            //pMouseOperate = "ZoomOut";
            //屏幕坐标点转化为地图坐标点
            //pPoint = (MapFrom.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            if (e.button == 1)
            {
                switch (pMouseOperate)
                {
                    case "ZoomIn":
                        ClsMapOperation zoomIn = new ClsMapOperation();
                        zoomIn.ZoomIn(MapFrom, MapFrom.TrackRectangle());
                        break;
                    case "ZoomOut":
                        ClsMapOperation zoomOut = new ClsMapOperation();
                        zoomOut.ZoomOut(MapFrom, MapFrom.TrackRectangle());
                        break;
                    case "Pan":
                        ClsMapOperation pan = new ClsMapOperation();
                        pan.Pan(MapFrom);
                        break;
                    default:
                        break;
                }
            }
        }

        private void MapTo_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            mapName = (sender as AxMapControl).Name;

            //pMouseOperate = "Pan";
            //pMouseOperate = "ZoomIn";

            //屏幕坐标点转化为地图坐标点
            //pPoint = (MapTo.Map as IActiveView).ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
            if (e.button == 1)
            {
                switch (pMouseOperate)
                {
                    case "ZoomIn":
                        ClsMapOperation zoomIn = new ClsMapOperation();
                        zoomIn.ZoomIn(MapTo, MapTo.TrackRectangle());
                        break;
                    case "ZoomOut":
                        ClsMapOperation zoomOut = new ClsMapOperation();
                        zoomOut.ZoomOut(MapTo, MapTo.TrackRectangle());
                        break;
                    case "Pan":
                        ClsMapOperation pan = new ClsMapOperation();
                        pan.Pan(MapTo);
                        break;
                    default:
                        break;
                }
            }
        }

        private void MapOverlapping_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 1)
            {
                switch (pMouseOperate)
                {
                    case "ZoomIn":
                        ClsMapOperation zoomIn = new ClsMapOperation();
                        zoomIn.ZoomIn(MapOverlapping, MapOverlapping.TrackRectangle());
                        break;
                    case "ZoomOut":
                        ClsMapOperation zoomOut = new ClsMapOperation();
                        zoomOut.ZoomOut(MapOverlapping, MapOverlapping.TrackRectangle());
                        break;
                    case "Pan":
                        ClsMapOperation pan = new ClsMapOperation();
                        pan.Pan(MapOverlapping);
                        break;
                    default:
                        break;
                }
            }
        }
        private void MapFrom_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            if (mapName == "MapFrom")
            {
                MapTo.Extent = MapFrom.Extent;
                MapTo.ActiveView.Refresh();
            }
        }

        private void MapTo_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            if (mapName == "MapTo")
            {
                MapFrom.Extent = MapTo.Extent;
                MapFrom.ActiveView.Refresh();
            }
        }
        #endregion

        #region The document that already opened.
        #region write text
        private void WriteText(string text)
        {
            string startPath = System.Windows.Forms.Application.StartupPath;
            string textPath = startPath + "\\..\\Res\\path\\openedPath.txt";
            string mxdPath = System.IO.Path.GetFileName(text);

            if (File.Exists(textPath))
            {
                if (!dic_fileFullPath.ContainsKey(text))
                {
                    StreamWriter sw = new StreamWriter(textPath, true);
                    dic_fileFullPath.Add(text, mxdPath);
                    sw.WriteLine(text);
                    sw.Close();

                }
                else
                {
                    return;
                }

            }
            else
            {
                StreamWriter sw = new StreamWriter(textPath, true);
                mxdPath = System.IO.Path.GetFileName(text);
                dic_fileFullPath.Add(text, mxdPath);
                sw.WriteLine(text);
                sw.Close();
            }
        }
        #endregion

        #region read text

        private int i = 0;
        ButtonItem buttonItemFile;
        /// <summary>
        /// 打开txt文件路径
        /// </summary>
        /// <param name="path"></param>
        private void ReadText(string path)
        {
            string read;
            // int i = 0;

            if (File.Exists(path))
            {
                //获取文本中已打开文档数
                StreamReader readCount = new StreamReader(path);
                while (readCount.ReadLine() != null)
                {
                    i++;
                }

                readCount.Close();

                //读取文本
                StreamReader streamReader = new StreamReader(path);
                while ((read = streamReader.ReadLine()) != null)
                {
                    //int i = 1;
                    string mxdName = System.IO.Path.GetFileName(read);

                    //创建类ButtonItem的对象
                    buttonItemFile = new ButtonItem();
                    galleryContainer1.SubItems.Add(buttonItemFile, 1);
                    buttonItemFile.Text = "&" + i + ". " + mxdName;
                    buttonItemFile.Name = "doc";
                    //在控件中galleryContainer添加
                    //galleryContainer2.SubItems.Add(buttonItem,i);
                    i--;
                    dic_fileFullPath.Add(read, mxdName);
                    //strArrBtn.Add(buttonItemFile.Text);
                    buttonItemFile.Click += new EventHandler(buttonItemFile_Click);
                }
                streamReader.Close();
            }
        }
        #endregion

        #region openfile
        void buttonItemFile_Click(object sender, EventArgs e)
        {
            ButtonItem btn = sender as ButtonItem;

            string temp = btn.Text.Substring(4);
            if (dic_fileFullPath.ContainsValue(temp))
            {
                foreach (var item in dic_fileFullPath)
                {
                    if (item.Value == temp)
                    {
                        if (mapMain.CheckMxFile(item.Key) == true)
                        {
                            mapMain.LoadMxFile(item.Key, Type.Missing, Type.Missing);
                            mapMain.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("该文件已不存在\n" + item.Key);
                        }
                    }
                }
            }
        }
        #endregion

        private void labelItem5_Click(object sender, EventArgs e)
        {

        }

        #endregion
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FrmUpdate pFrmUpdate = new FrmUpdate();
            pFrmUpdate.ShowDialog();
        }
        /// <summary>
        /// 历史对照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHistoryLook_Click(object sender, EventArgs e)
        {
            ClsHistory.Map = mapMain.Map;
            ClsHistory.Connection = ClsDBInfo.OracleConn;
            FrmHistoryView historyViewer = new FrmHistoryView();
            historyViewer.ShowDialog();
        }


        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBufferAnalysis_Click(object sender, EventArgs e)
        {
            ClsBufferSelectedLayerCmd pBufferselect = new ClsBufferSelectedLayerCmd();
            pBufferselect.OnCreate(this.mapMain.Object);
            //pBufferselect.
            pBufferselect.OnClick();
        }

        private void btnIntersectAnalysis_Click(object sender, EventArgs e)
        {
            FrmIntersect pIntersect = new FrmIntersect(this.mapMain.Object as IMapControl2);
            pIntersect.Show();
        }

        private void btnIdentityAnalysis_Click(object sender, EventArgs e)
        {
            FrmIdentity pIndentity = new FrmIdentity(this.mapMain.Object as IMapControl2);
            pIndentity.Show();
        }

        private void btnPointMatchCoding_Click(object sender, EventArgs e)
        {
            FrmOneKeyCoding pIndentity = new FrmOneKeyCoding(this.mapMain.Object as IMapControl2);
            pIndentity.Show();
        }


        /// <summary>
        /// 创建实体表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEntiDB_Click(object sender, EventArgs e)
        {
            FrmEntiDB pFrmEntiDB = new FrmEntiDB();
            pFrmEntiDB.ShowDialog();
        }

        FrmEntitySearch pFrmEntiSearch = new FrmEntitySearch();

        /// <summary>
        /// 实体查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEntitySearch_Click(object sender, EventArgs e)
        {
            if (pFrmEntiSearch.IsDisposed)
            {
                pFrmEntiSearch = new FrmEntitySearch();
            }
            pFrmEntiSearch.Show();
        }
        /// <summary>
        /// 按实体查询实体表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckEntiTable_Click(object sender, EventArgs e)
        {
            //FrmQueryByEntiTable frmQueryByEntiTable = new FrmQueryByEntiTable();
            //frmQueryByEntiTable.Show();

            //使用自定义封装类ClsQueryByAttribute
            ClsQueryByEntiTable toolQueryByEntiTable = new ClsQueryByEntiTable();
            toolQueryByEntiTable.OnCreate(this.mapMain.Object);
            //toolQueryByAttribute.FrmActive = this;
            toolQueryByEntiTable.OnClick();
        }

        FrmEntiUpdate pFrmEntiUpdate = new FrmEntiUpdate();
        /// <summary>
        /// 驱动更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEntityUpdate_Click(object sender, EventArgs e)
        {
            if (pFrmEntiUpdate.IsDisposed)
            {
                pFrmEntiUpdate = new FrmEntiUpdate();
            }
            pFrmEntiUpdate.Show();
        }

        /// <summary>
        /// 空值检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommonNullCheck_Click(object sender, EventArgs e)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(this.mapMain.Map, this.comboBoxItemCheck.Text);
            if (pFeatureLayer != null)
            {
                FrmResultDGV frmResult = new ZJGISGCoding.Forms.FrmResultDGV();
                //BindingSource bs = new BindingSource();
                if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                {
                    ClsCommonEnti pcommonEnti = new ClsCommonEnti();
                    //bs.DataSource = pcommonEnti.CheckCommonEnti(pFeatureLayer);
                    frmResult.LoadData(pcommonEnti.CheckCommonEnti(pFeatureLayer));
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    ClsRoadEnti pRoadEnti = new ClsRoadEnti();
                    frmResult.LoadData(pRoadEnti.CheckRoadEnti(pFeatureLayer));
                    //pRoadEnti.CreatGridCodeRoad(mapMain.Map, cbxCodeLayer);
                }
                else if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    ClsResEnti pResEnti = new ClsResEnti();
                    frmResult.LoadData(pResEnti.CheckRESEnti(pFeatureLayer));
                    //pResEnti.CreatGridCodeRES(mapMain.Map, cbxCodeLayer);
                }
                //frmResult.dataChild.DataSource = bs;
                frmResult.ShowDialog();

            }
            else
            {
                MessageBox.Show("没有选中任何图层，请选择图层！");
            }
        }
        /// <summary>
        /// 实体表唯一性检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEntiTableCheck_Click(object sender, EventArgs e)
        {
            FrmEntiTalbeCheck frmEntiTableCheck = new FrmEntiTalbeCheck();
            frmEntiTableCheck.ShowDialog();
        }

        /// <summary>
        /// 配置图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfigLayer_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(ClsConfig.path))
            {
                //ClsXmlOperation xmlop = new ClsXmlOperation("Layer.xml");
                ClsXmlOperation xmlop = new ClsXmlOperation(ClsConfig.path);
                FrmLayerConfig frmLayerConfig = new FrmLayerConfig(xmlop);
                frmLayerConfig.Show();

            }
        }

     






    }
}
