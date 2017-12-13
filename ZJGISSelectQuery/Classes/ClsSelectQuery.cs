using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using Microsoft.VisualBasic;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ZJGISCommon;

namespace SelectQuery 
{
    public static class ClsSelectQuery
    {
        /// <summary>
        /// 选择几何图形
        /// </summary>
        /// <param name="blnSelect"></param>
        /// <param name="pGeometry"></param>
        /// <param name="pselecttype"></param>
        /// <param name="m_pQueryResultCln"></param>
        public static void SelectGeometry(ref bool blnSelect, IGeometry pGeometry, esriSelectionResultEnum pselecttype,ref Collection m_pQueryResultCln)
        {
            ClsMapLayer pQueryResult;
            try
            {
                if (pGeometry == null)
                    return;
                
                if (!(pGeometry is IEnvelope))
                {
                    ITopologicalOperator pTop;
                    pTop = (ITopologicalOperator)pGeometry;
                    pTop.Simplify();
                }
                ILayer pLayer;
                IFeatureLayer pFeatLayer;
                IFeatureClass pFeatCls;
                int i;
                IFeatureLayerDefinition pFeaLyrDefine;

                Collection pLyrCol;
                Collection LyrCol=null;
                pLyrCol = ClsSelectQuery.FunGetFeaLyrCol(ClsDeclare.g_pMap, null, LyrCol);
                if (pLyrCol == null)
                    return;
                for (i = 1; i <= pLyrCol.Count; i++)
                {
                    pLayer = pLyrCol[i] as ILayer;
                    if (pLayer is IFeatureLayer)
                    {
                        pFeatLayer = (IFeatureLayer)pLayer;
                        bool bOutOfRange;
                        bOutOfRange = false;
                        if ((ClsDeclare.g_Sys.MapControl) != null)
                        {
                            IActiveView pActiveView;
                            ESRI.ArcGIS.Display.IDisplayTransformation pDisplayTransform;
                            pActiveView = ClsDeclare.g_Sys.MapControl.ActiveView.FocusMap as IActiveView;
                            pDisplayTransform = pActiveView.ScreenDisplay.DisplayTransformation;

                            if (pFeatLayer.MaximumScale == 0 && pFeatLayer.MinimumScale == 0)
                                bOutOfRange = false;
                            if (pFeatLayer.MaximumScale != 0 && pFeatLayer.MinimumScale != 0)
                            {
                                if (pDisplayTransform.ScaleRatio > pFeatLayer.MaximumScale && pDisplayTransform.ScaleRatio < pFeatLayer.MinimumScale)
                                    bOutOfRange = false;
                                else
                                    bOutOfRange = true;
                            }
                            if (pFeatLayer.MaximumScale == 0 && pFeatLayer.MinimumScale != 0)
                            {
                                if (pDisplayTransform.ScaleRatio < pFeatLayer.MinimumScale)
                                    bOutOfRange = false;
                                else
                                    bOutOfRange = true;


                            }
                            if (pFeatLayer.MaximumScale != 0 && pFeatLayer.MinimumScale == 0)
                            {
                                if (pDisplayTransform.ScaleRatio > pFeatLayer.MaximumScale)
                                    bOutOfRange = false;
                                else
                                    bOutOfRange = true;

                            }
                        }
                        if (bOutOfRange == false && pFeatLayer.Selectable)
                        {
                           ClsSelectQuery.RemoveSelection(pFeatLayer);
                            if (pFeatLayer.Visible == true)
                            {
                                pFeatCls = pFeatLayer.FeatureClass;
                                pFeaLyrDefine = (IFeatureLayerDefinition)pFeatLayer;

                                if (pFeaLyrDefine.DefinitionSelectionSet != null || pFeaLyrDefine.DefinitionExpression != "")
                                    blnSelect = true;
                                else
                                    blnSelect = false;
                                pQueryResult =ClsSelectQuery.QueryByGeometry(blnSelect, pGeometry, pFeatCls, pselecttype, pFeatLayer);

                                if (pQueryResult != null)
                                {
                                    m_pQueryResultCln.Add(pQueryResult, null, null, null);
                                }
                            }

                        }

                    }

                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
            finally
            {
                IActiveView pActiveView;
                pActiveView = (IActiveView)ClsDeclare.g_pMap;
                //pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, pActiveView.Extent);
                pActiveView.Refresh();
            }
        }

        //private static Collection FunGetFeaLyrCol(IMap iMap)
        //{
        //    throw new NotImplementedException();
        //}
        /// <summary>
        /// 通过几何图形查询
        /// </summary>
        /// <param name="blnSelect"></param>
        /// <param name="pGeometry"></param>
        /// <param name="pFeatCls"></param>
        /// <param name="pselecttype"></param>
        /// <param name="pFeatlayer"></param>
        /// <returns></returns>
        public static ClsMapLayer QueryByGeometry(bool blnSelect, IGeometry pGeometry, IFeatureClass pFeatCls, esriSelectionResultEnum pselecttype,IFeatureLayer pFeatlayer)
        {
            ClsMapLayer pQueryByGeometry = new ClsMapLayer();
            pQueryByGeometry.FeatCls = pFeatCls;

            IDataset pDataset;
            pDataset=(IDataset)pFeatCls;
            IFeatureSelection pFeatSelection;
            ICursor pCursor = null;

            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pGeometry;

            switch (pFeatCls.ShapeType)
            { 
                case esriGeometryType.esriGeometryPoint:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                    break;
                default:
                    pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelUndefined;
                    break;

            }
            pSpatialFilter.GeometryField = pFeatCls.ShapeFieldName;

            ISpatialFilter pFilter;
            pFilter = pSpatialFilter;

            if (blnSelect == true)
            {
                pFeatSelection = (IFeatureSelection)pFeatlayer;
                pFeatSelection.SelectFeatures(pFilter, pselecttype, false);

                pQueryByGeometry.FeatLayer = pFeatlayer;
                pQueryByGeometry.FeatSelectionSet = pFeatSelection.SelectionSet as ISelectionSet2;
                pQueryByGeometry.FeatSelectionSet.Search(null, false,out pCursor);

                if (pCursor is IFeatureCursor)
                    pQueryByGeometry.FeatCur = (IFeatureCursor)pCursor;
                return pQueryByGeometry;
            }
            else
            {
                pFeatSelection = (IFeatureSelection)pFeatlayer;
                pFeatSelection.SelectFeatures(pFilter, pselecttype, false);

                //esriSelectionOption pSelectionOption;
                //pSelectionOption = pselecttype as esriSelectionResultEnum;

                pQueryByGeometry.FeatLayer = pFeatlayer;
                pQueryByGeometry.FeatSelectionSet = pQueryByGeometry.FeatCls.Select(pFilter, esriSelectionType.esriSelectionTypeHybrid, (esriSelectionOption)((int)pselecttype), pDataset.Workspace) as ISelectionSet2;
                pQueryByGeometry.FeatSelectionSet.Search(null, false,out pCursor);
                if (pCursor is IFeatureCursor)
                    pQueryByGeometry.FeatCur = (IFeatureCursor)pCursor;
                return pQueryByGeometry;

            }
        }
        public static void RemoveSelection(IFeatureLayer pFeatlayer)
        {

            if (pFeatlayer.Selectable == false || pFeatlayer.Visible == false)
            {
                IFeatureSelection pFeatureSelection;
                pFeatureSelection = (IFeatureSelection)pFeatlayer;
                if (pFeatureSelection.SelectionSet != null)
                {
                    if (pFeatureSelection.SelectionSet.Count > 0)
                        pFeatureSelection.Clear();
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="pMap"></param>
        /// <returns></returns>
        public static IFeatureLayer FunFindFeatureLayerByName(string strName,IMap pMap)
        {
            Collection pLyrCol;
            ILayer pLayer;
            IFeatureLayer pFeatLyr;
            int i;
            Collection LyrCol=null;
            pLyrCol = FunGetFeaLyrCol(pMap, null, LyrCol);
            for (i = 1; i <= pLyrCol.Count; i++)
            {
                pLayer = pLyrCol[i] as ILayer;
                if (pLayer is IGeoFeatureLayer)
                {
                    pFeatLyr = pLyrCol[i] as IFeatureLayer;
                    if (pFeatLyr.Name == strName)
                        return pFeatLyr;
                }
            }
            return null;
        }
        public static Collection FunGetFeaLyrCol(IMap pMap, ILayer pLayer,Collection LyrCol)
        {
            ILayer pTemLyr;
            ICompositeLayer pCompositeLayer;
            if (pMap == null)
            {
                if (pLayer == null)
                    return null;
                else
                {
                    if (pLayer is IGroupLayer)
                    {
                        pCompositeLayer = (ICompositeLayer)pLayer;
                        for (int j = 0; j < pCompositeLayer.Count; j++)
                            FunGetFeaLyrCol(null, pCompositeLayer.get_Layer(j), LyrCol);
                    }
                    else
                        LyrCol.Add(pLayer, null, null, null);
                }               
            }
            else
            {
                LyrCol = new Collection();
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    
                    pTemLyr = pMap.get_Layer(i);
                    if (pTemLyr is IGroupLayer)
                    {
                        if (pTemLyr.Name != "示意图")
                            FunGetFeaLyrCol(null, pTemLyr, LyrCol);
                    }
                    else
                        LyrCol.Add(pTemLyr,null,null,null);
                }
            }

            return LyrCol;

        }
        public static ILayer FunFindLayer(IFeatureClass pFeatCls,IMap pMap)
        {
            int i;
            IDataset pDataset;
            IDataset pFCDataset;
            IFeatureLayer pFeatLayer;
            IWorkspace pWorkspace;

            pFCDataset = (IDataset)pFeatCls;

            for (i = 0; i < pMap.LayerCount; i++)
            {
                if (pMap.get_Layer(i) is IFeatureLayer)
                {
                    pFeatLayer = (IFeatureLayer)pMap.get_Layer(i);
                    pDataset = (IDataset)pFeatLayer.FeatureClass;
                    pWorkspace = pDataset.Workspace;
                    if (pWorkspace == pFCDataset.Workspace)
                    {
                        if (pFeatLayer.FeatureClass.ObjectClassID == pFeatCls.ObjectClassID)
                            return pFeatLayer;

                    }
                }
                 
            }
            return null;
        
        }
        public static ClsMapLayer QueryByAttribute(bool blnSelect, string whereClause, esriSelectionResultEnum pOperator, IFeatureClass pFeatCls, IFeatureLayer pFeatlayer)
        {

             ClsMapLayer pQueryByAttribute = new  ClsMapLayer();

            //IActiveView pActiveView;

            IQueryFilter pFilter=new QueryFilterClass();
            pFilter.SubFields = "*";
            pFilter.WhereClause = whereClause;

            pQueryByAttribute.FeatCls = pFeatCls;
            IDataset pDataset;
            pDataset = (IDataset)pFeatCls;

            IFeatureSelection pFeatSelection;
            ICursor pCursor;

            if (blnSelect == true)
            {

                pQueryByAttribute.FeatLayer = pFeatlayer;
                pFeatSelection = (IFeatureSelection)pFeatlayer;
                pFeatSelection.SelectFeatures(pFilter, pOperator, false);
                pQueryByAttribute.FeatSelectionSet = pFeatSelection.SelectionSet as ISelectionSet2;
                pQueryByAttribute.FeatSelectionSet.Search(null, false, out pCursor);
                if (pCursor is IFeatureCursor)
                    pQueryByAttribute.FeatCur = (IFeatureCursor)pCursor;
                return pQueryByAttribute;

                //pActiveView = ClsDeclare.g_pMap as IActiveView;
                //pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            }
            else
            {
                pQueryByAttribute.FeatLayer = pFeatlayer;
                //esriSelectionOption pSelectionOption=new esriSelectionOption();
                pQueryByAttribute.FeatSelectionSet = pQueryByAttribute.FeatCls.Select(pFilter, esriSelectionType.esriSelectionTypeHybrid, (esriSelectionOption)(int)pOperator, pDataset.Workspace) as ISelectionSet2;
                pQueryByAttribute.FeatSelectionSet.Search(null, false,out pCursor);

                if (pCursor is IFeatureCursor)
                    pQueryByAttribute.FeatCur = (IFeatureCursor)pCursor;
                return pQueryByAttribute;

            }

        }
        public static IRaster SelectRaster(IPoint pPnt,IMap pMap)
        {

            ILayer pLayer;
            IFeatureLayer pFeatlayer;
            IRasterLayer pRasterLayer;
            //IRasterCatalogItem pRasCatalogItem = null;
            //IFeatureCursor pFeatCursor = null;
            int i;
            IRaster pSelectRaster = null;
             
            for (i = 0; i < pMap.LayerCount; i++)
            {
                pLayer = pMap.get_Layer(i);
                if (pLayer is IGdbRasterCatalogLayer)
                {
                    pFeatlayer = (IFeatureLayer)pLayer;
                    pSelectRaster = QryRasterByPnt(pPnt, pFeatlayer);
                    return pSelectRaster;
                }
                else if (pLayer is IRasterLayer)
                {
                    pRasterLayer = (IRasterLayer)pLayer;
                    pSelectRaster = pRasterLayer.Raster;
                    return pSelectRaster;

                }
            }
            return null;
        }
        public static IRaster QryRasterByPnt(IPoint pPnt,IFeatureLayer pFeatlayer)
        {
            ISpatialFilter pSpatialFilter;
            IRasterCatalogItem pRasCatalogItem;
            ITopologicalOperator pTop;
            IRaster pQryRasterByPnt = null;

            try
            {
                pTop = (ITopologicalOperator)pPnt;
                pTop.Buffer(1);
                pSpatialFilter = new SpatialFilterClass();
                pSpatialFilter.Geometry = pPnt;
                pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pSpatialFilter.GeometryField = pFeatlayer.FeatureClass.ShapeFieldName;

                IFeatureCursor pFeatCursor;
                pFeatCursor = pFeatlayer.Search(pSpatialFilter,false);
                pRasCatalogItem = pFeatCursor.NextFeature() as IRasterCatalogItem;
                if (pRasCatalogItem != null)
                {
                    pQryRasterByPnt = pRasCatalogItem.RasterDataset.CreateDefaultRaster();
                    return pQryRasterByPnt;
                }
                return null;


            }
            catch (Exception)
            {
                //ClsDeclare.g_ErrorHandler.DisplayInformation("错误信息：" + ex.Message,false,null,null);
                throw;
            }

        }
        public static double GetPixelVal(IRaster pRaster,IPoint pPoint)
        {

            IPixelBlock pPixelBlock;
            IPnt pPnt;
            IPnt pCurrentPnt;
            ISpatialReference pSR;
            pSR = pPoint.SpatialReference;
            pPnt = new DblPnt();
            pPnt.SetCoords(1.0,1.0);

            pPixelBlock = pRaster.CreatePixelBlock(pPnt);
            pCurrentPnt = GetCurrPnt(pRaster, pPoint.X, pPoint.Y, pSR);
            pRaster.Read(pCurrentPnt, pPixelBlock);

            return Convert.ToDouble(string.Format("{0,10:###0.0000}", pPixelBlock.GetVal(0, 0, 0)));


        }
        public static IPnt GetCurrPnt(IRaster pRaster,double x,double y,ISpatialReference pSR)
        {
            IRasterProps pRasterProps;
            IEnvelope pExtent;
            IPnt pPnt;
            double dbWidth, dbHight;

            pRasterProps = (IRasterProps)pRaster;
            pExtent = pRasterProps.Extent;
            pExtent.Project(pSR);
            pPnt = pRasterProps.MeanCellSize();

            dbWidth = pPnt.X;
            dbHight = pPnt.Y;

            IPnt pTemPnt=new DblPntClass();
            pTemPnt.X = (x - pExtent.XMin) / dbWidth;
            pTemPnt.Y = (pExtent.YMax - y) / dbHight;

            return pTemPnt;


        }
    }
}
