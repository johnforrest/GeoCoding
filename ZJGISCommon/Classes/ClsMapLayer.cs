using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.Display;
using System.Threading;
using ESRI.ArcGIS.esriSystem;
using Microsoft.VisualBasic;
using ESRI.ArcGIS.Analyst3D;
using ESRI.ArcGIS.DataSourcesFile;

namespace ZJGISCommon
{
    public class ClsMapLayer
    {
        private Collection vFeatCln;
        private IFeatureLayer vFeatLayer;
        private IFeatureClass vFeatCls;
        private ITable vTable;

    
        private IFeatureCursor vFeatCur;
        private ISelectionSet2 vFeatSelectionSet;
        private static bool blnEditMapSpatial;

        public static bool BlnEditMapSpatial
        {
            get
            {
                return blnEditMapSpatial;
            }
            set
            {
                blnEditMapSpatial = value;
            }
        }

        public Collection FeatCln
        {
            get
            {
                return vFeatCln;
            }
            set
            {
                vFeatCln = value;
            }

        }

        public IFeatureLayer FeatLayer
        {
            get
            {
                return vFeatLayer;
            }
            set
            {
                vFeatLayer = value;
            }
        }

        public IFeatureClass FeatCls
        {
            get
            {
                return vFeatCls;
            }
            set
            {
                vFeatCls = value;
            }
        }
        public ITable VTable
        {
            get { return vTable; }
            set { vTable = value; }
        }
        public IFeatureCursor FeatCur
        {
            get
            {
                return vFeatCur;
            }
            set
            {
                vFeatCur = value;
            }
        }

        public ISelectionSet2 FeatSelectionSet
        {
            get
            {
                return vFeatSelectionSet;
            }
            set
            {
                vFeatSelectionSet = value;
            }
        }

        /// <summary>
        /// 查找图层
        /// </summary>
        /// <param name="pFeatCls">要素类</param>
        /// <param name="pMap">所查地图</param>
        /// <returns>找到图层</returns>
        public ILayer FindLayer(IFeatureClass pFeatCls, IMap pMap)
        {
            IDataset pDataset = null;
            IDataset pFCDataset = null;
            IFeatureLayer pFeatLayer = null;
            IWorkspace pWorkspace = null;

            pFCDataset = pFeatCls as IDataset;

            //遍历地图中的层
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                //是要素层
                if (pMap.get_Layer(i) is IFeatureLayer)
                {
                    pFeatLayer = pMap.get_Layer(i) as IFeatureLayer;
                    pDataset = pFeatLayer.FeatureClass as IDataset;
                    pWorkspace = pDataset.Workspace;

                    if (pWorkspace==pFCDataset.Workspace)
                    {
                        if (pFeatLayer.FeatureClass==pFeatCls)
                        {
                            //如果所查要素类与该层工作空间相同且为同一要素类，返回该层
                            return pFeatLayer; 
                        }
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// 获取选择的要素（以cusor形式遍历）
        /// </summary>
        /// <param name="pFeatureLayer">预分析要素层</param>
        /// <returns>选择要素的游标</returns>
        public IFeatureCursor GetSelectedFeatures(IFeatureLayer pFeatureLayer)
        {
            if (pFeatureLayer==null)
            {
                //分析图层为空
                return null;
            }
            else
            {
                IFeatureSelection pFeatSel = null;
                ISelectionSet pSelectionSet = null;
                ICursor pCursor = null;

                pFeatSel = pFeatureLayer as IFeatureSelection;
                pSelectionSet = pFeatSel.SelectionSet;

                if (pSelectionSet.Count==0)
                {
                    MessageBox.Show("No features are selected in the '" + pFeatureLayer.Name + "' layer");
                    return null;
                }

                pSelectionSet.Search(null, false, out pCursor);

                //返回游标
                return pCursor as IFeatureCursor;
            }
        }


        /// <summary>
        /// 获取图层类型
        /// </summary>
        /// <param name="pLayer">预分析图层</param>
        /// <returns>图层类型</returns>
        public static string GetLayerType(ILayer pLayer)
        {
            IFeatureClass pFeatureClass=null;
            IFeatureLayer pFeatureLayer=null;

            if ((pLayer is IAnnotationLayer)||(pLayer is IDimensionLayer)||(pLayer is ICadLayer))
            {
                return "Anno";
            }
            else
            {
                if (pLayer is FeatureLayer)
                {
                    pFeatureLayer = pLayer as FeatureLayer;
                    pFeatureClass = pFeatureLayer.FeatureClass;

                    if (pFeatureLayer is GdbRasterCatalogLayer)
                    {
                        return "Raster";//栅格
                    }
                    else
                    {
                        if (pFeatureClass.ShapeType==esriGeometryType.esriGeometryPoint)
                        {
                            return "Point";
                        }
                        if (pFeatureClass.ShapeType==esriGeometryType.esriGeometryPolyline)
                        {
                            return "Polyline";
                        }
                        if (pFeatureClass.ShapeType==esriGeometryType.esriGeometryPolygon)
                        {
                            return "Polygon";
                        }
                    }
                }

            }

            return null;
        }


        public static long SortIndex(ILayer pSortLayer, IBasicMap pMap)
        {
            string LayerType = null;
            string tmpLayerType = null;

            int index = 0;
            int oldIndex = 0;

            bool bExist = false;

            ILayer pLayer  = pSortLayer;
            LayerType = GetLayerType(pLayer);

            index = pMap.LayerCount;

            switch (LayerType)
            {
                case "Point":
                    {
                        for (int i = 0; i < pMap.LayerCount; i++)
                        {
                            pLayer = pMap.get_Layer(i);
                            if (pLayer.Name != pSortLayer.Name)
                            {
                                tmpLayerType = GetLayerType(pLayer);

                                if (tmpLayerType != "Anno")
                                {
                                    index = i;
                                    break;
                                }
                            }
                            else
                            {
                                oldIndex = i;
                                bExist = true;
                            }

                            index = i + 1;
                        }
                        break;
                    }

                case "Anno":
                    {
                        index = 0;
                        break;
                    }

                case "Polyline":
                    {
                        for (int i = 0; i < pMap.LayerCount; i++)
                        {
                            pLayer = pMap.get_Layer(i);
                            if (pLayer.Name != pSortLayer.Name)
                            {
                                tmpLayerType = GetLayerType(pLayer);

                                if (tmpLayerType != "Anno" && tmpLayerType != "Point")
                                {
                                    index = i;
                                    break;
                                }
                            }
                            else
                            {
                                oldIndex = i;
                                bExist = true;
                            }

                            index = i + 1;
                        }
                        break;
                    }

                case "Polygon":
                    {
                        for (int i = 0; i < pMap.LayerCount; i++)
                        {
                            pLayer = pMap.get_Layer(i);
                            if (pLayer.Name != pSortLayer.Name)
                            {
                                tmpLayerType = GetLayerType(pLayer);

                                if (tmpLayerType != "Anno" && tmpLayerType != "Point"
                                    && tmpLayerType != "Polyline")
                                {
                                    index = i;
                                    break;
                                }
                            }
                            else
                            {
                                oldIndex = i;
                                bExist = true;
                            }

                            index = i + 1;
                        }
                        break;
                    }

                case "Raster":
                    {

                        for (int i = 0; i < pMap.LayerCount; i++)
                        {
                            pLayer = pMap.get_Layer(i);
                            if (pLayer.Name != pSortLayer.Name)
                            {
                                tmpLayerType = GetLayerType(pLayer);

                                if (tmpLayerType != "Anno" && tmpLayerType != "Point"
                                    && tmpLayerType != "Polyline" && tmpLayerType != "Polygon")
                                {
                                    index = i;
                                    break;
                                }
                            }
                            else
                            {
                                oldIndex = i;
                                bExist = true;
                            }

                            index = i + 1;
                        }
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            if (bExist==true)
            {
                if (oldIndex<index)
                {
                    index = index - 1;
                }
            }

            if (index<0)
            {
                index = 0;
            }

            return index;
        }

        /// <summary>
        /// 获取要素的集合
        /// </summary>
        /// <param name="pMap">地图要素</param>
        /// <param name="pLayer">图层</param>
        /// <param name="LyrCol">定义的集合类型</param>
        /// <returns></returns>
        public static Collection<object> GetFeaLyrCol(IMap pMap, ILayer pLayer,Collection<object> LyrCol)
        {
            ILayer pTempLyr = null;
            ICompositeLayer pCompositeLayer = null;
            
            if (LyrCol==null)
            {
                LyrCol = new Collection<object>();
            }

            if (pMap==null)
            {
                if (pLayer == null)
                {
                    return null;
                }
                else
                {
                    if (pLayer is IGroupLayer)
                    {
                        pCompositeLayer = pLayer as ICompositeLayer;

                        for (int i = 0; i < pCompositeLayer.Count; i++)
                        {
                            GetFeaLyrCol(null, pCompositeLayer.get_Layer(i),LyrCol);
                        }
                    }
                    else
                    {                           
                        LyrCol.Add(pLayer);
                    }
                }
            }
            else
            {
                for (int i = 0; i < pMap.LayerCount; i++)
                {
                    pTempLyr = pMap.get_Layer(i);

                    if (pTempLyr is IGroupLayer)
                    {
                        if (pTempLyr.Name!="示意图")
                        {
                            GetFeaLyrCol(null, pTempLyr,LyrCol);
                        }
                    }
                    else
                    {
                        LyrCol.Add(pTempLyr);
                    }
                }
            }

            return LyrCol;
        }


        /// <summary>
        /// 按名称获取图层
        /// </summary>
        /// <param name="strName">图层名</param>
        /// <param name="pMap">预分析地图</param>
        /// <returns>要素层</returns>
        public IFeatureLayer FindFeatureLayerByName(string strName, IMap pMap)
        {
            Collection<object> LyrCol=null;
            //ILayer pLayer = null;
            IFeatureLayer pFeatLyr = null;

            LyrCol = GetFeaLyrCol(pMap,null,null);
            if (LyrCol == null)
            {
                return null;
            }
            else
            {
                for (int i = 0; i < LyrCol.Count; i++)
                {
                    pFeatLyr = LyrCol[i] as IFeatureLayer;

                    if (pFeatLyr.Name == strName)
                    {
                        return pFeatLyr;
                    }
                }
                return null;
            }    
        }

        
        /// <summary>
        /// 通过要素获得其Map中的FeatureLayer
        /// </summary>
        /// <param name="pFeature">要素</param>
        /// <param name="pMap">工作map</param>
        /// <returns>要素层</returns>
        public IFeatureLayer GetFeatLayerByFeat(IFeature pFeature, IMap pMap)
        {
            ILayer pLayer = null;
            IDataset pFeaDataSet = null;
            IDataset pDataSet = null;
            IFeatureLayer pFeatureLayer = null;
            IFeatureClass pFeatureClass = null;
            pFeatureClass = pFeature.Class as IFeatureClass;
            pFeaDataSet = pFeatureClass as IFeatureDataset;

            for (int i = 0; i <pMap.LayerCount; i++)
            {
                pLayer = pMap.get_Layer(i);

                if (pLayer is IFeatureLayer)
                {
                    pFeatureLayer = pLayer as IFeatureLayer;
                    pDataSet = pFeatureLayer.FeatureClass as IDataset;
                    ////判断是否出于同一个工作空间
                    if (object.ReferenceEquals(pDataSet.Workspace, pFeaDataSet.Workspace))
                    {
                        if (object.ReferenceEquals(pFeatureLayer.FeatureClass, pFeatureClass))
                        {
                            return pFeatureLayer;
                        }
                    }
                }
            }

            return null;

        }


        /// <summary>
        /// 获得图层索引号
        /// </summary>
        /// <param name="strName">图层名字</param>
        /// <param name="pMap">欲检索的地图</param>
        /// <returns>图层索引</returns>
        public long LayerIndex(string strName, IMap pMap)
        {
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                if (pMap.get_Layer(i).Name == strName)
                {
                    return i;
                }
            }
            return -1;
        }



        /// <summary>
        /// 获取单位名称
        /// </summary>
        /// <param name="vSpatialReference">空间参考</param>
        /// <returns>单位名称</returns>
        public string GetUnitName(ISpatialReference vSpatialReference)
        {
            IGeographicCoordinateSystem pGeographicCoordinateSystem = null;
            IProjectedCoordinateSystem pProjectedCoordinateSystem = null;

            string sUnitName = null;

            if ((vSpatialReference != null))
            {
                if (vSpatialReference is IGeographicCoordinateSystem)//如果是大地坐标系则获得其对应的名称
                {
                    pGeographicCoordinateSystem = vSpatialReference as IGeographicCoordinateSystem;
                    sUnitName = pGeographicCoordinateSystem.CoordinateUnit.Name;
                }
                else if (vSpatialReference is IProjectedCoordinateSystem)//如果是投影坐标则获得对应的名称
                {
                    pProjectedCoordinateSystem = vSpatialReference as IProjectedCoordinateSystem;
                    sUnitName = pProjectedCoordinateSystem.CoordinateUnit.Name;
                }
                else if (vSpatialReference is IUnknownCoordinateSystem)
                {
                    sUnitName = "未知";
                }
            }
            else
            {
                sUnitName = "未知";
            }
            switch (sUnitName.ToUpper())//将获得的英文名称转换成其对应的中文单位名称
            {
                case "METER":
                    sUnitName = "米";
                    break;
                case "CENTIMETRE":
                    sUnitName = "厘米";
                    break;
                case "KILOMETRES":
                    sUnitName = "千米";
                    break;
                case "DEGREE":
                    sUnitName = "度";
                    break;
            }

            return sUnitName;
        }





        /// <summary>
        /// 高亮显示地物
        /// </summary>
        /// <param name="pGeometry">几何图形</param>
        /// <param name="pActiveView">活动视图</param>
        public void FlashGeometry(IGeometry pGeometry, IActiveView pActiveView)
        {
            if (pGeometry == null || pActiveView == null)
            {
                return;
            }

            //设置与视图相同的空间参考
            pGeometry.Project(pActiveView.FocusMap.SpatialReference);
            pActiveView.ScreenDisplay.StartDrawing(0, (short)esriScreenCache.esriNoScreenCache);

            switch (pGeometry.GeometryType)//根据不同的几何类型作不同的高亮显示
            {
                case esriGeometryType.esriGeometryPolyline:
                    {
                        FlashLine(pActiveView.ScreenDisplay, pGeometry);
                        break;
                    }
                case esriGeometryType.esriGeometryPolygon:
                    {
                        FlashPolygon(pActiveView.ScreenDisplay, pGeometry);
                        break;
                    }
                case esriGeometryType.esriGeometryPoint:
                    {
                        FlashPoint(pActiveView.ScreenDisplay, pGeometry);
                        break;
                    }
                default: break;
            }

            pActiveView.ScreenDisplay.FinishDrawing();

        }


        /// <summary>
        /// 高亮显示线
        /// </summary>
        /// <param name="pDisplay">用于操作的显示屏幕</param>
        /// <param name="pGeometry">欲高亮显示的几何图形</param>
        private void FlashLine(IScreenDisplay pDisplay, IGeometry pGeometry)
        {
            if (pGeometry == null)
            {
                return;
            }

            //设置显示格式
            ISimpleLineSymbol pLineSymbol = null;
            pLineSymbol = new SimpleLineSymbol();
            pLineSymbol.Width = 4;
            pLineSymbol.Color = GetRGBColor(255, 150, 150);

            ISymbol pSymbol = null;
            pSymbol = pLineSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            //用所设置的格式绘制几何图形
            pDisplay.SetSymbol((ISymbol)pLineSymbol);
            pDisplay.DrawPolyline(pGeometry);
            Thread.Sleep(100);
            pDisplay.DrawPolyline(pGeometry);

        }


        /// <summary>
        /// 高亮显示多边形
        /// </summary>
        /// <param name="pDisplay">用于操作的显示屏幕</param>
        /// <param name="pGeometry">欲高亮显示的几何图形</param>
        private void FlashPolygon(IScreenDisplay pDisplay, IGeometry pGeometry)
        {
            if (pGeometry == null)
            {
                return;
            }

            //设置显示格式
            ISimpleFillSymbol pFillSymbol = null;
            pFillSymbol = new SimpleFillSymbol();
            pFillSymbol.Outline = null;
            pFillSymbol.Color = GetRGBColor(255, 150, 150);

            ISymbol pSymbol = default(ISymbol);
            pSymbol = pFillSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            //用所设置的格式绘制几何图形
            pDisplay.SetSymbol((ISymbol)pFillSymbol);
            pDisplay.DrawPolygon(pGeometry);
            Thread.Sleep(100);
            pDisplay.DrawPolygon(pGeometry);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDisplay">用于操作的显示屏幕</param>
        /// <param name="pGeometry">欲高亮显示的几何图形</param>
        private void FlashPoint(IScreenDisplay pDisplay, IGeometry pGeometry)
        {
            if (pGeometry == null)
            {
                return;
            }

            //设置显示格式
            ISimpleMarkerSymbol pMarkerSymbol = null;
            pMarkerSymbol = new SimpleMarkerSymbol();
            pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
            pMarkerSymbol.Color = GetRGBColor(255, 150, 150);

            ISymbol pSymbol = null;
            pSymbol = pMarkerSymbol as ISymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            //用所设置的格式绘制几何图形
            pDisplay.SetSymbol((ISymbol)pMarkerSymbol);
            pDisplay.DrawPoint(pGeometry);
            Thread.Sleep(100);
            pDisplay.DrawPoint(pGeometry);

        }

        /// <summary>
        /// 实现几个图形聚焦
        /// </summary>
        /// <param name="pActiveView">操作的活动视图</param>
        /// <param name="pGeometry">几何图形</param>
        public void ZoomToGeo(IActiveView pActiveView, IGeometry pGeometry)
        {
            switch (pGeometry.GeometryType)//根据不同图形作不同设置
            {
                case esriGeometryType.esriGeometryPoint:
                    {
                        IEnvelope pEnv = default(IEnvelope);
                        pEnv = pActiveView.Extent;
                        pEnv.CenterAt((IPoint)pGeometry);//改变外界矩形封套的中心锚点位置（点的特殊操作）
                        pActiveView.Extent = pEnv;
                        break;
                    }
                case esriGeometryType.esriGeometryPolyline:
                case esriGeometryType.esriGeometryPolygon:
                    {
                        pActiveView.Extent = pGeometry.Envelope;
                        break;
                    }
                default:
                    {
                        pActiveView.Extent = pGeometry.Envelope;
                        break;
                    }
            }

            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, pActiveView.Extent);

        }

        /// <summary>
        /// 将像素转换成地图单元
        /// </summary>
        /// <param name="pActiveView">提供各种转换信息</param>
        /// <param name="pixelUnits">像素单元</param>
        /// <returns>像素单元大小</returns>
        public static double ConvertPixelsToMapUnits(IActiveView pActiveView, double pixelUnits)
        {
            if (pActiveView == null)
            {
                return -1;
            }
            //获取 the ScreenDisplay
            IScreenDisplay screenDisplay = pActiveView.ScreenDisplay;

            //获取 DisplayTransformation 
            IDisplayTransformation displayTransformation = screenDisplay.DisplayTransformation;

            //获取一个设备帧并用它来获得x轴方向上的像素数
            tagRECT deviceRECT = displayTransformation.get_DeviceFrame();
            int pixelExtent = (deviceRECT.right - deviceRECT.left);

            //获取当前可是区域的地图范围
            IEnvelope envelope = displayTransformation.VisibleBounds;
            double realWorldDisplayExtent = envelope.Width;

            //计算一个像素大小
            if (pixelExtent == 0)
            {
                return -1;
            }
            double sizeOfOnePixel = (realWorldDisplayExtent / pixelExtent);

            //得出像素单元大小
            return (pixelUnits * sizeOfOnePixel);

        }


        /// <summary>
        /// 把像素(屏幕)距离转化成为地图上的距离
        /// </summary>
        /// <param name="vActiveView">用于构造转换器</param>
        /// <param name="vPixelDistance">像素距离</param>
        /// <returns>地图距离</returns>
        public double ConvertPixelDistanceToMapDistance(IActiveView vActiveView, double vPixelDistance)
        {
            tagPOINT tagPOINT;
            WKSPoint WKSPoint = new WKSPoint();

            //依据距离构造一个点
            tagPOINT.x = (int)vPixelDistance;
            tagPOINT.y = (int)vPixelDistance;

            //转换点的信息
            vActiveView.ScreenDisplay.DisplayTransformation.TransformCoords(ref WKSPoint, ref tagPOINT, 1, 6);

            //返回点x即距离
            return WKSPoint.X;

        }


        /// <summary>
        /// 把Map中的选择要素转到Collection中
        /// </summary>
        /// <param name="pMap">选择对象</param>
        /// <returns>要素集合</returns>
        public Collection<object> GetSelectFeature(IMap pMap)
        {
            //获取选择集
            IEnumFeature pEnumFeature = null;
            pEnumFeature = pMap.FeatureSelection as IEnumFeature;

            if (pEnumFeature == null)
            {
                return null;
            }

            IFeature pFeature = null;

            pEnumFeature.Reset();

            //遍历所选中的要素并将其放入一集合中
            Collection<object> pFeatureCol = new Collection<object>();
            while ((pFeature = pEnumFeature.Next()) != null)
            {
                pFeatureCol.Add(pFeature);
            }

            return pFeatureCol;
        }


        /// <summary>
        /// 根据不同原色构造Color
        /// </summary>
        /// <param name="pRed">红</param>
        /// <param name="pGreen">绿</param>
        /// <param name="pBlue">蓝</param>
        /// <returns>颜色</returns>
        public static IRgbColor GetRGBColor(int pRed, int pGreen, int pBlue)
        {
            IRgbColor pRGB = new RgbColor();

            //构建RGB
            pRGB.Red = pRed;
            pRGB.Green = pGreen;
            pRGB.Blue = pBlue;
            pRGB.UseWindowsDithering = true;

            return pRGB;
        }

        public static void SortLayer(IBasicMap pBasicMap, ILayer pLayer)
        {
            if (pBasicMap is IMap)
            {
                IMap pTmpMap = pBasicMap as IMap;
                pTmpMap.MoveLayer(pLayer, (int)ClsMapLayer.SortIndex(pLayer, pBasicMap));

            }
            else if (pBasicMap is IScene)
            {
                IScene pScene = null;
                pScene = pBasicMap as IScene;
                pScene.MoveLayer(pLayer, (int)ClsMapLayer.SortIndex(pLayer, pBasicMap));
            }
        }

        public static bool AddDataset(IBasicMap pBasicMap, IDatasetName pDatasetName, Collection<object> m_DatasetCol, bool blnAddData)
        {
            bool functionReturnValue = false;
            functionReturnValue = false;
            ////如果是特征数据集,则添加里边的所有要素类
            IFeatureDataset pFeatDS = default(IFeatureDataset);
            IEnumDataset pEnumDataSet = default(IEnumDataset);
            IDataset pDataset = default(IDataset);
            IFeatureClass pFeatCls = default(IFeatureClass);
            IFeatureLayer pFeatLayer = default(IFeatureLayer);
            IName pName = default(IName);
            ILayer pLayer = default(ILayer);
            ITopologyLayer pTopoLayer = default(ITopologyLayer);
            //Dim pEnumLyr As IEnumLayer
            ITinWorkspace pTinWS = default(ITinWorkspace);
            IEnumFeatureClass pEnumFeatCls = null;
            IRasterCatalogDisplayProps pRasterCatalogPro = default(IRasterCatalogDisplayProps);

            //ClsErrorHandle pfrmError = new ClsErrorHandle();
            //ClsLayerRender pLayerRender = null;


            if (pDatasetName is IFeatureDatasetName)
            {
                pName = pDatasetName as IName;
                pFeatDS = pName.Open() as IFeatureDataset;

                pEnumDataSet = pFeatDS.Subsets;
                pDataset = pEnumDataSet.Next();

                m_DatasetCol.Add(pDataset);
                if (pDataset == null)
                {
                    return false;
                }
                ////根据数据集的类型,添加特征数据集中的所有要素类(拓扑,一般的,栅格目录,网络)
                while ((pDataset != null))
                {
                    if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        //pLayerRender = new ClsLayerRender();
                        //// 陈昉  2008-12-3  修改 修改原因 添加SDE图层时从数据库读取符号化信息
                        //pFeatLayer = null;
                        //if (pDataset.Workspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                        //{
                        //    pFeatLayer = pLayerRender.GetRendererLayer((IFeatureClass)pDataset);
                        //}
                        if (pFeatLayer == null)
                        {
                            pFeatLayer = new FeatureLayer();
                            pFeatCls = pDataset as IFeatureClass;
                            pFeatLayer.Name = pFeatCls.AliasName;
                            pFeatLayer.FeatureClass = pFeatCls;
                        }

                        if (pDataset.Type == esriDatasetType.esriDTRasterCatalog)
                        {
                            // Dim pRaster
                        }

                        if (blnAddData == true)
                        {
                            //pMap.AddLayer pFeatLayer
                            AddLyrToBasicMap(pBasicMap, pFeatLayer);
                            SortLayer(pBasicMap, pFeatLayer);
                        }
                        // pSelectedCln.Add(pFeatLayer)

                    }
                    else if (pDataset.Type == esriDatasetType.esriDTTopology)
                    {
                        pTopoLayer = new TopologyLayer() as ITopologyLayer;
                        pTopoLayer.Topology = pDataset as ITopology;
                        pLayer = pTopoLayer as ILayer;
                        pLayer.Name = pDataset.Name;

                        if (blnAddData == true)
                        {
                            //pMap.AddLayer pLayer
                            AddLyrToBasicMap(pBasicMap, pLayer);
                            SortLayer(pBasicMap, pLayer);
                        }
                        //pSelectedCln.Add(pFeatLayer)
                    }

                    pDataset = pEnumDataSet.Next();
                }
                functionReturnValue = true;


                ////添加拓扑图层
            }
            else if (pDatasetName is ITopologyName)
            {
                ITopology pTopo = null;
                pName = pDatasetName as IName;
                pDataset = pName.Open() as IDataset;
                pTopoLayer = new TopologyLayer() as ITopologyLayer;
                pTopoLayer.Topology = pDataset as ITopology;
                pLayer = pTopoLayer as ILayer;
                pLayer.Name = pDataset.Name;
                m_DatasetCol.Add(pDataset);
                if (blnAddData == true)
                {
                    //pMap.AddLayer pLayer
                    AddLyrToBasicMap(pBasicMap, pLayer);
                    SortLayer(pBasicMap, pLayer);
                }
                //pSelectedCln.Add(pLayer)

                if (MessageBox.Show("要把拓扑里边的所有要素类也添加到当前地图中吗?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
                    == DialogResult.Yes)
                {
                    IFeatureClassContainer pFeatClsContainer = default(IFeatureClassContainer);
                    pFeatClsContainer = pTopo as IFeatureClassContainer;
                    pEnumFeatCls = pFeatClsContainer.Classes;
                    pFeatCls = pEnumFeatCls.Next();
                    pFeatLayer = new FeatureLayer();

                    ////循环拓扑中的每个要素类,并添加到当前地图中
                    while ((pFeatCls != null))
                    {
                        pFeatLayer.FeatureClass = pFeatCls;
                        pFeatLayer.Name = pFeatCls.AliasName;

                        if (blnAddData == true)
                        {
                            //pMap.AddLayer pFeatLayer
                            AddLyrToBasicMap(pBasicMap, pFeatLayer);
                            SortLayer(pBasicMap, pFeatLayer);
                        }
                        //pSelectedCln.Add(pFeatLayer)

                        pFeatCls = pEnumFeatCls.Next();
                    }

                }
                functionReturnValue = true;
                ////添加网络数据
            }
            else if (pDatasetName is IGeometricNetworkName)
            {
                INetworkCollection pNetworkCollection = default(INetworkCollection);
                IGeometricNetwork pGeometricNetwork = default(IGeometricNetwork);

                IDataset pGeoDataset = default(IDataset);

                pName = pDatasetName as IName;
                pGeoDataset = pName.Open() as IDataset;
                m_DatasetCol.Add(pGeoDataset);
                if (pGeoDataset.Type == esriDatasetType.esriDTGeometricNetwork)
                {
                    ////这里对网络数据进行处理  
                    IFeatureClassContainer pFeatureClassContainer = null;
                    pGeometricNetwork = pGeoDataset as IGeometricNetwork;
                    pFeatureClassContainer = pGeometricNetwork as IFeatureClassContainer;

                    for (int i = 0; i < pFeatureClassContainer.ClassCount; i++)
                    {
                        pFeatCls = pFeatureClassContainer.get_Class(i);
                        pFeatLayer = new FeatureLayer();
                        pFeatLayer.Name = pFeatCls.AliasName;
                        pFeatLayer.FeatureClass = pFeatCls;

                        if (blnAddData == true)
                        {
                            // pMap.AddLayer pFeatLayer
                            AddLyrToBasicMap(pBasicMap, pFeatLayer);
                            SortLayer(pBasicMap, pFeatLayer);
                        }
                        //pSelectedCln.Add(pFeatLayer)
                    }
                }
                else
                {
                    pFeatDS = pGeoDataset as IFeatureDataset;
                    pNetworkCollection = pFeatDS as INetworkCollection;
                    ////如果是用户选择一个网络技术打开的话,肯定只有一个网络在里边,其实
                    ////可以不需要循环,而用GeometricNetwork(0)代替循环
                    for (int j = 0; j < pNetworkCollection.GeometricNetworkCount; j++)
                    {
                        pGeometricNetwork = pNetworkCollection.get_GeometricNetwork(j);
                        for (int i = 0; i <= 3; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    pEnumFeatCls = pGeometricNetwork.get_ClassesByType(esriFeatureType.esriFTSimpleJunction);
                                    break;
                                case 1:
                                    pEnumFeatCls = pGeometricNetwork.get_ClassesByType(esriFeatureType.esriFTSimpleEdge);
                                    break;
                                case 2:
                                    pEnumFeatCls = pGeometricNetwork.get_ClassesByType(esriFeatureType.esriFTComplexJunction);
                                    break;
                                case 3:
                                    pEnumFeatCls = pGeometricNetwork.get_ClassesByType(esriFeatureType.esriFTComplexEdge);
                                    break;
                            }
                            pFeatCls = pEnumFeatCls.Next();
                            while ((pFeatCls != null))
                            {
                                pFeatLayer = new FeatureLayer();
                                pFeatLayer.Name = pFeatCls.AliasName;
                                pFeatLayer.FeatureClass = pFeatCls;
                                pFeatCls = pEnumFeatCls.Next();

                                if (blnAddData == true)
                                {
                                    //pMap.AddLayer pFeatLayer
                                    AddLyrToBasicMap(pBasicMap, pFeatLayer);
                                    SortLayer(pBasicMap, pFeatLayer);
                                }
                                // pSelectedCln.Add(pFeatLayer)

                                functionReturnValue = true;
                            }
                        }
                    }
                }

                ////添加栅格目录，并设置为显示最新时相
            }
            else if (pDatasetName is IRasterCatalogName)
            {
                pName = pDatasetName as IName;
                pDataset = pName.Open() as IDataset;
                m_DatasetCol.Add(pDataset);
                pFeatLayer = new GdbRasterCatalogLayer() as IFeatureLayer;
                pFeatLayer.FeatureClass = pDataset as IFeatureClass;
                pFeatLayer.Name = pDataset.Name;

                //'//如果是SDE的栅格目录
                //If pFeatLayer.DataSourceType = "SDE Raster Catalog" Then

                //    Dim pFeatLayerDef As IFeatureLayerDefinition
                //    pFeatLayerDef = pFeatLayer

                //    '//设置最初显示地图范围内最近时相的数据
                //    pFeatLayerDef.DefinitionExpression = "objectid in (select objectid from" & vbNewLine & _
                //        "(select a.objectid, b.receive_date,rank()" & vbNewLine & _
                //        "over(partition by a.name,a.resolution order by b.receive_date desc) as cid" & vbNewLine & _
                //        "from " & pFeatLayer.Name & " a, sj_t_tense b" & vbNewLine & _
                //        "where a.tense = b.tense" & vbNewLine & "and b.online_state = 1) t2" & vbNewLine & _
                //        "where " & pFeatLayer.Name & ".objectid=t2.objectid and t2.cid = 1)"

                //End If
                ////设置当栅格目录中的图幅在地图上超过16个的时候，以格网来显示，而不显示栅格本身
                pRasterCatalogPro = pFeatLayer as IRasterCatalogDisplayProps;
                ////不用数量来控制了,而以比例尺来控制
                pRasterCatalogPro.DisplayRasters = 16;
                pRasterCatalogPro.UseScale = true;
                ////设置一个比例,在此临界栅格数据将会在框架显示与实际栅格显示之间转换
                pRasterCatalogPro.TransitionScale = 50000;

                if (blnAddData == true)
                {
                    //pMap.AddLayer pFeatLayer
                    AddLyrToBasicMap(pBasicMap, pFeatLayer);
                    //SortLayer(pBasicMap, pFeatLayer)
                }
                //pSelectedCln.Add(pFeatLayer)
                functionReturnValue = true;


            }
            else if (pDatasetName is IRasterDatasetName)
            {
                IRasterLayer pRasterLayer = default(IRasterLayer);
                pName = pDatasetName as IName;
                pDataset = pName.Open() as IDataset;
                m_DatasetCol.Add(pDataset);
                pRasterLayer = new RasterLayer();
                pRasterLayer.CreateFromDataset((IRasterDataset)pDataset);
                pRasterLayer.Name = pDataset.Name;
                AddLyrToBasicMap(pBasicMap, pRasterLayer);
                functionReturnValue = true;
                ////添加TIN图层
            }
            else if (pDatasetName is ITinWorkspace)
            {
                pTinWS = pDatasetName as ITinWorkspace;
                ITinLayer pTinLyr = default(ITinLayer);
                pTinLyr = new TinLayer();
                pTinLyr.Dataset = pTinWS.OpenTin(pDatasetName.Name);
                pTinLyr.Name = pDatasetName.Name;

                if (blnAddData == true)
                {
                    //pMap.AddLayer pTinLyr
                    AddLyrToBasicMap(pBasicMap, pTinLyr);
                    SortLayer(pBasicMap, pTinLyr);
                }
                //pSelectedCln.Add(pTinLyr)
                functionReturnValue = true;



            }
            else if (pDatasetName is INetworkDatasetName)
            {
                //Dim pNetWorkDS As INetworkDataset
                try
                {
                    INetworkLayer pNetLayer = default(INetworkLayer);

                    pName = pDatasetName as IName;
                    pNetLayer = new NetworkLayer() as INetworkLayer;
                    pNetLayer.NetworkDataset = pName.Open() as INetworkDataset;
                    pLayer = pNetLayer as ILayer;
                    pLayer.Name = pName.NameString;
                    if (blnAddData == true)
                    {
                        AddLyrToBasicMap(pBasicMap, pLayer);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                ////添加一般的要素类,未写完。。。。。。
            }
            else
            {
                pName = pDatasetName as IName;
                pDataset = pName.Open() as IDataset;
                pFeatCls = pDataset as IFeatureClass;
                m_DatasetCol.Add(pDataset);
                if (pFeatCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatLayer = new FDOGraphicsLayer() as IFeatureLayer;
                }
                else if (pFeatCls.FeatureType == esriFeatureType.esriFTDimension)
                {
                    pFeatLayer = new DimensionLayer() as IFeatureLayer;

                }
                else
                {
                    pFeatLayer = new FeatureLayer();
                    //pLayerRender = new ClsLayerRender();
                    //// 陈昉  2008-12-3  修改 修改原因 添加SDE图层时从数据库读取符号化信息
                    //if (pDataset.Workspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                    //{
                    //    pFeatLayer = pLayerRender.GetRendererLayer((IFeatureClass)pDataset);
                    //}
                }
                //印骅 20081205 添加"Not"
                if ((pFeatLayer != null))
                {
                    //pFeatLayer.Name = pDataset.Name
                    pFeatLayer.Name = pFeatCls.AliasName;
                    pFeatLayer.FeatureClass = pDataset as IFeatureClass;
                }
                if (blnAddData == true)
                {
                    //pMap.AddLayer pFeatLayer

                    AddLyrToBasicMap(pBasicMap, pFeatLayer);
                    SortLayer(pBasicMap, pFeatLayer);
                }
                //pSelectedCln.Add(pFeatLayer)

                functionReturnValue = true;
            }

            return functionReturnValue;

            //'//添加Coverage图层
            //ElseIf vItem.SmallIcon = "Coverage" Then
            //AddSelectedLayer = ADDCoverageLayer(pMap, pCurrentFilePath.Path, _
            //vItem.Text, pSelectedCln, blnAddData)


        }

        public static void AddLyrToBasicMap(IBasicMap basicMap, ILayer addedLayer)
        {
            try
            {
                //判断空间参考是否一致
                IDataset dataSet = addedLayer as IDataset;
                if (dataSet is IGeoDataset)
                {
                    IGeoDataset geoDataset = dataSet as IGeoDataset;
                    if (geoDataset.SpatialReference != null && basicMap.SpatialReference != null)
                    {
                        //这里只是通过名称进行下比较  如果说想要比较的严格的话 可以通过克隆的方法进行
                        if (blnEditMapSpatial == true && geoDataset.SpatialReference.Name != basicMap.SpatialReference.Name)
                        {
                            if (MessageBox.Show("当前加载图层的空间参考与地图空间参考不一致，是否更改地图空间参考？",
                                "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                basicMap.SpatialReference = geoDataset.SpatialReference;
                            }
                            else
                            {
                                //不进行比较了
                                blnEditMapSpatial = false;
                            }
                        }
                    }
                    basicMap.AddLayer(addedLayer);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }

        }
        /// <summary>
        /// 添加选中的图层
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="pSuffix"></param>
        /// <param name="pListCol"></param>
        /// <param name="m_DatasetCol"></param>
        /// <param name="pCurrentFilePath"></param>
        /// <param name="blnAddData"></param>
        /// <returns></returns>
        public static bool AddSelectedLayer(IBasicMap pMap, string pSuffix, Collection<object> pListCol,
                Collection<object> m_DatasetCol, string pCurrentFilePath, bool blnAddData)
        {
            bool functionReturnValue = false;
            System.Windows.Forms.Cursor pCursor = Cursors.Default;

            //Dim pFileName As IFileName
            IWorkspaceFactory pWorkspaceFactory = null;

            IFeatureWorkspace pFeatWorkspace = null;
            //Dim pFeatDS As IFeatureDataset
            IDataset pDataset = null;
            //Dim pFeatCls As IFeatureClass
            IFeatureLayer pFeatLayer = null;
            //Dim pLayer As ILayer
            ////以下三个变量是用来获得工作区的路径和文件的名称
            //Dim sPath() As String
            string sWorkSpacePath = pCurrentFilePath;
            string sFileName = null;

            if (pListCol.Count == 0)
            {
                return false;
            }

            //要进行空间参考的比较
            blnEditMapSpatial = true;
            try
            {
                switch (pSuffix)
                {
                    case ".shp":
                        {
                            //shp文件的处理
                            //获得工作区,并通过shp文件的名称打开文件
                            pWorkspaceFactory = new ShapefileWorkspaceFactory() as IWorkspaceFactory;
                            if (pWorkspaceFactory.IsWorkspace(sWorkSpacePath))
                            {
                                pFeatWorkspace = pWorkspaceFactory.OpenFromFile(sWorkSpacePath, 0) as IFeatureWorkspace;
                                for (int i = 0; i < pListCol.Count; i++)
                                {
                                    sFileName = pListCol[i].ToString();
                                    pDataset = pFeatWorkspace.OpenFeatureClass(sFileName) as IDataset;
                                    m_DatasetCol.Add(pDataset);
                                    pFeatLayer = new FeatureLayer();
                                    //设置图层的名字和要素类
                                    pFeatLayer.FeatureClass = pDataset as IFeatureClass;
                                    pFeatLayer.Name = sFileName.Substring(0, sFileName.Length - 4);
                                    if (blnAddData == true)
                                    {
                                        //pMap.AddLayer pFeatLayer
                                        ClsMapLayer.AddLyrToBasicMap(pMap, pFeatLayer);
                                        ClsMapLayer.SortLayer(pMap, pFeatLayer);
                                    }
                                    //pSelectedCln.Add(pFeatLayer)
                                }
                                functionReturnValue = true;
                            }

                            break;
                        }
                    case "IDatasetName":

                        IDatasetName pDSN = null;
                        for (int i = 0; i < pListCol.Count; i++)
                        {
                            pDSN = pListCol[i] as IDatasetName;
                            functionReturnValue = ClsMapLayer.AddDataset(pMap, pDSN, m_DatasetCol, blnAddData);
                        }

                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            return functionReturnValue;
        }
    }
}
