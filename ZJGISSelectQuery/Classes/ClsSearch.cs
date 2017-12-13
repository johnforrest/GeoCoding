using System;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using Microsoft.VisualBasic;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ZJGISCommon;

namespace SelectQuery 
{
    public static class ClsSearch
    {
        public static void PfgisGetClosestFeature(IMap pMap, IPoint pSearchPt,ref Collection pSelected,ref IFeature pFeatureClosest)
        {
            ClsMapLayer pConvert = new ClsMapLayer();
            //初始化返回值
            pSelected = new Collection();
            pFeatureClosest = null;
            //设置搜索范围
            IEnvelope pSrchEnv;
            pSrchEnv = pSearchPt.Envelope;
            double SearchDist;
            SearchDist = 4d;
            IActiveView pActiveView;
            pActiveView = pMap as IActiveView;
            SearchDist = ClsMapLayer.ConvertPixelsToMapUnits(pActiveView, SearchDist);
            pSrchEnv.Width = SearchDist;
            pSrchEnv.Height = SearchDist;
            pSrchEnv.CenterAt(pSearchPt);

            UIDClass pUID = new UIDClass();
            pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
            IEnumLayer pLayers;
            pLayers = pMap.get_Layers(pUID,true);

            ILayer pLayer;
            pLayer = pLayers.Next();

            while(pLayer!=null)
            {
                IGeoFeatureLayer pGeoLayer;
                pGeoLayer = (IGeoFeatureLayer)pLayer;

                if (pGeoLayer.Selectable && pGeoLayer.Visible && pGeoLayer.FeatureClass != null)
                {
                    IIdentify2 pID;
                    pID = (IIdentify2)pGeoLayer;
                    //根据查询范围执行查询操作
                    IArray pArray;
                    pArray = pID.Identify(pSrchEnv, null);

                    IFeatureIdentifyObj pFeatIdObj;
                    IRowIdentifyObject pRowObj;
                    IFeature pFeature;

                    if (pArray != null)
                    {
                        for (int j = 0; j < pArray.Count; j++)
                        {
                            if (pArray.get_Element(j) is IFeatureIdentifyObj)
                            {
                                pFeatIdObj = pArray.get_Element(j) as IFeatureIdentifyObj;
                                pRowObj = pFeatIdObj as IRowIdentifyObject;
                                pFeature = pRowObj.Row as IFeature;
                                pSelected.Add(pFeature,null,null,null);
                            }
                        }
                        pArray.RemoveAll();

                    }
                }
                pLayer = pLayers.Next();

            }
            ClsSearch.PfgisGetClosestFeatureInCollection(SearchDist, pSelected, pSearchPt, ref pFeatureClosest);
        }
        private static void PfgisGetClosestFeatureInCollection(double SearchDist, Collection searchCollection, IPoint pPoint, ref IFeature pFeature)
        {
            IProximityOperator pProximity;
            IFeature pTestFeature;
            IFeature pPointFeature=null;
            IFeature pLineFeature=null;
            IFeature pAreaFeature = null;
            IGeometry pGeom;

            double pointTestDistance;
            double lineTestDistance;
            double areaTestDistance;
            double testDistance;

            double tempDist;
            long i;

            pointTestDistance = -1;
            lineTestDistance = -1;
            areaTestDistance = -1;
            testDistance = -1;

            pProximity =pPoint as IProximityOperator;

            for (i = 1; i <=searchCollection.Count; i++)
            {
                pTestFeature = searchCollection[i] as IFeature;
                pGeom = pTestFeature.Shape;
                tempDist = pProximity.ReturnDistance(pGeom);
                if (tempDist < SearchDist)
                {

                    switch (pGeom.GeometryType)
                    {
                        case esriGeometryType.esriGeometryPoint:
                            {
                                if (pointTestDistance < 0)
                                    pointTestDistance = tempDist + 1;
                                if (tempDist < pointTestDistance)
                                {
                                    pointTestDistance = tempDist;
                                    pPointFeature = pTestFeature;
                                }
                                break;
                            }
                        case esriGeometryType.esriGeometryPolygon:
                            {
                                if (areaTestDistance < 0)
                                    areaTestDistance = tempDist + 1;
                                if (tempDist < areaTestDistance)
                                {
                                    areaTestDistance = tempDist;
                                    pAreaFeature = pTestFeature;

                                }
                                break;
                            }
                        case esriGeometryType.esriGeometryPolyline:
                            {
                                if (lineTestDistance < 0)
                                    lineTestDistance = tempDist + 1;
                                if (tempDist < lineTestDistance)
                                {
                                    lineTestDistance = tempDist;
                                    pLineFeature = pTestFeature;
                                }
                                break;
                            }
                    }
                }
                else
                {
                    if (testDistance < 0)
                        testDistance = tempDist + 1;
                    if (tempDist < testDistance)
                    {
                        testDistance = tempDist;
                        pFeature = pTestFeature;
                    }
                }
            }
            if (pPointFeature != null)
                pFeature = pPointFeature;
            else if (pLineFeature != null)
                pFeature = pLineFeature;
            else if (pAreaFeature != null)
                pFeature = pAreaFeature;
        }
        public static void FlashFeature(IActiveView pActiveView,IFeature pFeature)
        {
            pActiveView.ScreenDisplay.StartDrawing(0,Convert.ToInt16(esriScreenCache.esriNoScreenCache));

            switch (pFeature.Shape.GeometryType)
            {
                case esriGeometryType.esriGeometryPolyline:
                    FlashLine(pActiveView.ScreenDisplay, pFeature.Shape);
                    break;
                case esriGeometryType.esriGeometryPolygon:
                    FlashPolygon(pActiveView.ScreenDisplay, pFeature.Shape);
                    break;
                case esriGeometryType.esriGeometryPoint:
                    FlashPoint(pActiveView.ScreenDisplay, pFeature.Shape);
                    break;

            }
            pActiveView.ScreenDisplay.FinishDrawing();

        
        }
        private static void FlashLine(IScreenDisplay pDisplay,IGeometry pGeometry)
        {
            ISimpleLineSymbol pLineSymbol;
            ISymbol pSymbol;
            IRgbColor pRGBColor;

            pRGBColor = new RgbColorClass();
            pRGBColor.Red = 255;
            pRGBColor.Green = 150;
            pRGBColor.Blue = 150;

            pLineSymbol = new SimpleLineSymbolClass();
            pLineSymbol.Width = 4;
            pLineSymbol.Color = pRGBColor;

            pSymbol = (ISymbol)pLineSymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPolyline(pGeometry);
            System.Threading.Thread.Sleep(300);
            pDisplay.DrawPolyline(pGeometry);

        }
        public static void FlashPoint(IScreenDisplay pDisplay,IGeometry pGeometry)
        {
            try
            {
                ISimpleMarkerSymbol pMarkerSymbol;
                ISymbol pSymbol;
                IRgbColor pRGBColor;

                pRGBColor = new RgbColorClass();
                pRGBColor.Red = 255;
                pRGBColor.Green = 150;
                pRGBColor.Blue = 150;

                pMarkerSymbol = new SimpleMarkerSymbolClass();
                pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;
                pMarkerSymbol.Color = pRGBColor;

                pSymbol = (ISymbol)pMarkerSymbol;
                pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

                pDisplay.SetSymbol(pSymbol);
                pDisplay.DrawPoint(pGeometry);
                System.Threading.Thread.Sleep(300);
                pDisplay.DrawPoint(pGeometry);

            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public static void FlashPolygon(IScreenDisplay pDisplay,IGeometry pGeometry)
        {
            ISimpleFillSymbol pFillSymbol;
            ISymbol pSymbol;
            IRgbColor pRGBColor;

            pRGBColor = new RgbColorClass();
            pRGBColor.Red = 255;
            pRGBColor.Green = 150;
            pRGBColor.Blue = 150;

            pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Outline = null;
            pFillSymbol.Color = pRGBColor;

            pSymbol = (ISymbol)pFillSymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPolygon(pGeometry);
            System.Threading.Thread.Sleep(300);
            pDisplay.DrawPolygon(pGeometry);
        }
        public static void PfgisFlashPolygon(IActiveView pActiveView,IGeometry pGeometry)
        {

            if (pGeometry == null)
                return;
            pActiveView.ScreenDisplay.StartDrawing(0, Convert.ToInt16(esriScreenCache.esriNoScreenCache));

            IRgbColor pRgbColor;
            pRgbColor = new RgbColorClass();
            pRgbColor.Red = 255;
            pRgbColor.Green = 150;
            pRgbColor.Blue = 150;

            ISimpleFillSymbol pFillSymbol;
            pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Outline = null;
            pFillSymbol.Color = pRgbColor;

            ISymbol pSymbol;
            pSymbol = (ISymbol)pFillSymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPNotXOrPen;

            IScreenDisplay pDisplay;
            pDisplay = pActiveView.ScreenDisplay;

            pDisplay.SetSymbol(pSymbol);
            pDisplay.DrawPolygon(pGeometry);
            System.Threading.Thread.Sleep(150);
            pDisplay.DrawPolygon(pGeometry);

            pActiveView.ScreenDisplay.FinishDrawing();
        }

      
    }
}
