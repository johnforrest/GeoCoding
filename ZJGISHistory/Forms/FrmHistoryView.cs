using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using ESRI.ArcGIS.Geodatabase;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ZJGISCommon;
using ESRI.ArcGIS.Geometry;

namespace ZJGISHistory
{
    public partial class FrmHistoryView : DevComponents.DotNetBar.Office2007Form
    {
        private IHistoricalWorkspace m_pHistoricalWorkspace;
        private IHistoricalVersion m_pHistoricalVersion;
        private Collection<object> m_pHisLayerColl = new Collection<object>();          //记录MapCtl中现有的历史图层
        //Private m_frmAddHisData As frmAddHisData             '添加图层窗体
        private DataSet m_featClsDS = new DataSet();

        //侯晨 20081118修改 改变窗体大小时需判断是新建窗体还是已加载
        private bool m_bHasLoad = false;                //判断是否加载窗体
        private string m_sMorTSelected;              //记录查看方式,0表示按历史标记查看,1表示按时间点查看
        private string m_sfunLR;                           //判断对加载图层的窗体对象，O表示对左窗体添加图层，1表示对右窗体添加

        public FrmHistoryView()
        {
            InitializeComponent();
        }

        private void FrmHistoryView_Load(object sender, EventArgs e)
        {


            ClsHistory.WorkSpace = ClsDBInfo.SdeWorkspace;
          
            m_pHistoricalWorkspace = ClsHistory.WorkSpace as IHistoricalWorkspace;
            ClsHistory.OpenConn();


            DTPickerL.Format = DateTimePickerFormat.Custom;
            DTPickerL.CustomFormat = "MM/dd/yyy HH:mm:ss tt";
            DTPickerL.Value = ClsHistory.GetSystemTime();

            DTPickerR.Format = DateTimePickerFormat.Custom;
            DTPickerR.CustomFormat = "MM/dd/yyy HH:mm:ss tt";
            DTPickerR.Value = ClsHistory.GetSystemTime();

            ////分别添加工作空间中现有历史标记到左右Cbo
            ClsHistory.AddHisMarkerToCbo(ref CboMarkerL);
            ClsHistory.AddHisMarkerToCbo(ref CboMarkerR);
            rdoTimeL.Checked = true;
            rdoTimeR.Checked = true;
            m_bHasLoad = true;
        }

        /// <summary>
        /// 判断MapMain中的图层是否为注册后的归档数据，否则没有历史 
        /// </summary>
        /// <param name="pWorkspace"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool HasArch(IWorkspace pWorkspace, string strName)
        {

            IVersionedObject3 pVersionedObject = default(IVersionedObject3);
            IArchivableObject pArchivableObject = default(IArchivableObject);
            IFeatureClass pFeatClass = default(IFeatureClass);

            IFeatureWorkspace pFeatWorkspace = default(IFeatureWorkspace);
            IWorkspace2 pWorkspace2 = default(IWorkspace2);
            try
            {
                //只对SDE的数据执行历史数据的操作
                if (pWorkspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    pWorkspace2 = pWorkspace as IWorkspace2;
                    if (pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, strName))
                    {
                        pFeatWorkspace = pWorkspace as IFeatureWorkspace;
                        pFeatClass = pFeatWorkspace.OpenFeatureClass(strName);
                        pVersionedObject = pFeatClass as IVersionedObject3;
                        if (pVersionedObject == null)
                            return false;
                        pArchivableObject = pVersionedObject as IArchivableObject;
                        if (pArchivableObject.IsArchiving == false)
                            return false;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception )
            {
                return false;
            }
        }

        /// <summary>
        /// 获取当前MapCtl里所有注册版本且归档的历史图层集合
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool GetHisLayerColl()
        {
            string strName = null;
            FeatureClass pFeatCls = default(FeatureClass);

            IFeatureLayer pFeatLayer = default(IFeatureLayer);
            int i = 0;
            if (ClsHistory.Map.LayerCount == 0)
            {
                MessageBoxEx.Show("当前图层中没有可查看的图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            for (i = 0; i <= ClsHistory.Map.LayerCount - 1; i++)
            {
                if (ClsHistory.Map.get_Layer(i) is IFeatureLayer)
                {
                    pFeatLayer = ClsHistory.Map.get_Layer(i) as IFeatureLayer;
                    pFeatCls = pFeatLayer.FeatureClass as FeatureClass;
                    strName = pFeatCls.BrowseName;
                    if (HasArch(ClsHistory.WorkSpace, strName) == true)
                    {
                        m_pHisLayerColl.Add(pFeatLayer);
                      
                    }
                    else
                    {
                        MessageBoxEx.Show("不存在'" + strName + "'图层的历史数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                        return false;
                    }
                }

            }
            return true;
        }

        /// <summary>
        /// 判断当前地图中有无指定Layer
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool LayerExist(Map pMap, string strName)
        {
            int i = 0;
            for (i = 0; i <= pMap.LayerCount - 1; i++)
            {
                if (pMap.get_Layer(i).Name == strName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 删除地图中指定Layer
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="strName"></param>
        /// <remarks></remarks>
        private void DelLayerExist(ref Map pMap, string strName)
        {
            int i = 0;
            for (i = 0; i <= pMap.LayerCount - 1; i++)
            {
                if (pMap.get_Layer(i).Name == strName)
                {
                    pMap.DeleteLayer(pMap.get_Layer(i));
                }
            }
        }


        /// <summary>
        /// 加载历史数据 
        /// </summary>
        /// <param name="vData"></param>
        /// <remarks></remarks>
        private void AddHisLayer(object vData, string sfunLR, Collection<object> pHisLyrColl)
        {
            string dTimeStamp = string.Empty;
            string strName = null;
            string sHTableName = null;
            string sName = null;
            int i = 0;

            IFeatureWorkspace pFeatureWorkspace = default(IFeatureWorkspace);
            IFeatureClass pFeatCls = default(IFeatureClass);
            //Dim pFeatLayer As IFeatureLayer
            IFeatureLayer pFeatLyr = default(IFeatureLayer);
            string strCondition = null;

            if (ClsHistory.WorkSpace == null)
                return;
            pFeatureWorkspace = ClsHistory.WorkSpace as IFeatureWorkspace;
            if (pHisLyrColl.Count == 0)
            {
                if (GetHisLayerColl() == false)
                    return;
            }

            try
            {
                if (m_sMorTSelected == "0")
                {
                    m_pHistoricalVersion = m_pHistoricalWorkspace.FindHistoricalVersionByName(vData.ToString());
                    dTimeStamp = String.Format(m_pHistoricalVersion.TimeStamp.ToString(), "yyyy-MM-dd HH:mm:ss");
                    //格式化时间
                }
                else if (m_sMorTSelected == "1")
                {
                    dTimeStamp = String.Format(vData.ToString(), "yyyy-MM-dd HH:mm:ss");
                }

                //FrmProgressBar progressBar = new FrmProgressBar(true);
                //progressBar.SetMax(pHisLyrColl.Count);
                //progressBar.SetStep(1);
                //progressBar.ShowNew();
                //progressBar.SetInfo("正在加载历史图层...");
                for (i = 0; i < pHisLyrColl.Count; i++)
                {
                    pFeatLyr = pHisLyrColl[i] as IFeatureLayer;
                    strName = ((IDataset)pFeatLyr.FeatureClass).Name;

                    //查找对应的历史图层名_H%
                    try
                    {
                        sHTableName = ClsHistory.GetTableNameRec(strName, ClsHistory.Connection);
                        if (string.IsNullOrEmpty(sHTableName))
                        {
                            MessageBoxEx.Show("不存在'" + strName + "'图层的历史数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //progressBar.PerformOneStep();
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        //progressBar.PerformOneStep();
                        continue;
                    }

                    //按时间戳进行点查询
                    strCondition = "GDB_FROM_DATE <= TO_DATE('" + Convert.ToString(dTimeStamp) + "','yyyy-MM-dd HH24:mi:ss') AND GDB_TO_DATE > TO_DATE('" + Convert.ToString(dTimeStamp) + "','yyyy-MM-dd HH24:mi:ss')";

                    IQueryFilter pFilter = default(IQueryFilter);
                    IFeatureLayer pTmpLayer = default(IFeatureLayer);
                    IFeatureLayer pNewFeatLyr = default(IFeatureLayer);
                    IFeatureLayerDefinition2 pFeatLayerDef = default(IFeatureLayerDefinition2);
                    IFeatureSelection pFeatSel = default(IFeatureSelection);

                    pFilter = new QueryFilter();
                    pFilter.SubFields = "*";
                    pFilter.WhereClause = strCondition;

                    pFeatCls = pFeatureWorkspace.OpenFeatureClass(sHTableName);
                    pTmpLayer = new FeatureLayer();
                    pTmpLayer.FeatureClass = pFeatCls;
                    pFeatSel = pTmpLayer as IFeatureSelection;
                    pFeatSel.SelectFeatures(pFilter, esriSelectionResultEnum.esriSelectionResultNew, false);
                    pFeatLayerDef = pTmpLayer as IFeatureLayerDefinition2;

                    pFeatLayerDef.DefinitionExpression = strCondition;

                    //IFields fields = pTmpLayer.FeatureClass.Fields;
                    //int indext = fields.FindField("GDB_FROM_DATE");

                    //object str = pTmpLayer.FeatureClass.GetFeature(1).get_Value(indext);
                   
                    //string  str=pTmpLayer.FeatureClass.get
                    //对历史图层命名
                    if (m_sMorTSelected == "0")
                    {
                        sName = strName + "[" + vData + "]";
                    }
                    else if (m_sMorTSelected == "1")
                    {
                        sName = strName + "[" + Convert.ToString(dTimeStamp) + "]";
                    }
                    else
                    {
                        //progressBar.CloseBar();
                        return;
                    }

                    //按照选择集生成新的图层pNewFeatLyr
                    pNewFeatLyr = pFeatLayerDef.CreateSelectionLayer(sName, true, "", "");

                    //添加历史图层到对应的MapCtl中
                    if (sfunLR == "0")
                    {
                        if (this.MapLeft.Map != null)
                        {
                            if (LayerExist((Map)this.MapLeft.Map, sName) == false)
                            {
                                //MapLeft.ClearLayers();
                                MapLeft.Map.AddLayer(pNewFeatLyr);
                            }
                        }
                    }
                    else if (sfunLR == "1")
                    {
                        if (this.MapRight.Map != null)
                        {
                            if (LayerExist((Map)this.MapRight.Map, sName) == false)
                            {
                                //MapRight.ClearLayers();
                                MapRight.Map.AddLayer(pNewFeatLyr);
                            }
                        }
                    }
                    //progressBar.PerformOneStep();
                }
            }
            catch (Exception )
            {
                //g_ErrorHandle.HandleError(true, "错误文件名：" + "WHFHistory_frmHisDoubView" + "；错误过程名：AddHisLayer" + "；错误行数： " + g_ErrorHandle.GetErrorLineNumberString(Erl()), Err().Number, Err().Source, Err().Description);

            }
            finally
            {
                //if (frmCnProgress.IsDisposed == false)
                //    frmCnProgress.Close();
            }
        }

        private void CboMarkerL_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.CboMarkerL.SelectedItem.ToString() == "请选择")
            {
                return;
            }
            if (m_bHasLoad == true)
            {
                rdoMarkerL.Checked = true;
                rdoTimeL.Checked = false;
                m_sMorTSelected = "0";
                m_sfunLR = "0";
                this.MapLeft.ClearLayers();
                AddHisLayer(CboMarkerL.SelectedItem, m_sfunLR, m_pHisLayerColl);
                this.MapLeft.Refresh();
            }
        }

        private void CboMarkerR_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.CboMarkerR.SelectedItem.ToString() == "请选择")
            {
                return;
            }
            if (m_bHasLoad == true)
            {
                rdoMarkerR.Checked = true;
                rdoTimeR.Checked = false;
                m_sMorTSelected = "0";
                m_sfunLR = "1";
                this.MapRight.ClearLayers();
                AddHisLayer(CboMarkerR.SelectedItem, m_sfunLR, m_pHisLayerColl);
                this.MapRight.Refresh();
            }
        }

        private void DTPickerL_ValueChanged(object sender, EventArgs e)
        {
            if (m_bHasLoad == true)
            {
                rdoMarkerL.Checked = false;
                rdoTimeL.Checked = true;
                m_sMorTSelected = "1";
                m_sfunLR = "0";
                AddHisLayer(DTPickerL.Value, m_sfunLR, m_pHisLayerColl);
                this.MapLeft.Refresh();
            }
        }

        private void DTPickerR_ValueChanged(object sender, EventArgs e)
        {
            if (m_bHasLoad == true)
            {
                rdoMarkerR.Checked = false;
                rdoTimeR.Checked = true;
                m_sMorTSelected = "1";
                m_sfunLR = "1";
                AddHisLayer(DTPickerR.Value, m_sfunLR, m_pHisLayerColl);
                this.MapRight.Refresh();
            }
        }

        private void MapLeft_OnAfterDraw(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnAfterDrawEvent e)
        {
            IGeometry pGeom = null;
            if (this.MapRight.Map == null)
            {
                return;
            }
            pGeom = MapLeft.ActiveView.Extent;
            MapRight.ActiveView.Extent = pGeom as IEnvelope;
            MapRight.ActiveView.Refresh();
        }
    }
}