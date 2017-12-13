using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace SelectQuery
{
    public static class ClsMeasureSnap
    {
        public static IPoint g_pLastSnapPnt;
        public static IPoint g_pNowSnapPnt;
        public static ISymbol g_pSnapSymbol;
        public static IFeatureCache2 m_pFeatureCache;

        public static IPoint MeasureSnapPoint(ref IPoint pPoint)
        {
            IActiveView pActiveView;
            IEnvelope pEnvlope;
            int pixel;
            double dCacheradius;
            double dRangle;
            IPoint pSnapPpt;

            pActiveView = (IActiveView)ClsDeclare.g_pMap;
            pixel = 5;
            dCacheradius = ConvertPixelDistanceToMapDistance(pActiveView, pixel);
            pEnvlope = GetCurrentEnvelope(pPoint, dCacheradius);
            m_pFeatureCache = new FeatureCache() as IFeatureCache2;
            dRangle = pEnvlope.Width;
            m_pFeatureCache.Initialize(pPoint, dRangle);
            FillCache(pPoint, pEnvlope);
            if (m_pFeatureCache.Count == 0)
                return null;

            pSnapPpt = GetSnapPoint(pPoint, m_pFeatureCache, dCacheradius);
            if (pSnapPpt != null)
            {
                g_pLastSnapPnt = g_pNowSnapPnt;
                g_pNowSnapPnt = pSnapPpt;
                DrawRectangle(pActiveView, g_pLastSnapPnt);
                DrawRectangle(pActiveView, g_pNowSnapPnt);
                return pSnapPpt;

            }
            else
            {
                g_pLastSnapPnt = g_pNowSnapPnt;
                g_pNowSnapPnt = null;
                DrawRectangle(pActiveView, g_pLastSnapPnt);
                return null;///可能存在错误！！！！！！！！！！！！
            }
        }
        public static double ConvertPixelDistanceToMapDistance(IActiveView vActiveView, double vPixelDistance)
        {
            tagPOINT tagP;
            WKSPoint WKSP=new WKSPoint();
            tagP.x = Convert.ToInt32(vPixelDistance);
            tagP.y = Convert.ToInt32(vPixelDistance);
            vActiveView.ScreenDisplay.DisplayTransformation.TransformCoords(ref WKSP, ref tagP, 1, 6);
            return WKSP.X;
        
        }
        public static IEnvelope GetCurrentEnvelope(IPoint pPoint, double dCacheradius)
        {
            IEnvelope pEnvelope;
            pEnvelope = new EnvelopeClass();
            pEnvelope.XMax = pPoint.X + dCacheradius / 2;
            pEnvelope.YMax = pPoint.Y + dCacheradius / 2;
            pEnvelope.XMin = pPoint.X - dCacheradius / 2;
            pEnvelope.YMin = pPoint.Y - dCacheradius / 2;
            return pEnvelope;
        
        }
        public static void FillCache(IPoint pPoint, IEnvelope pEnvlope)
        {
            IGeoFeatureLayer pGeoLyr;
            IEnumLayer pEnumLyr;
            IEnumLayer pTemEnumLyr;
            IMap pMap;
            UIDClass pUID=new UIDClass();
            IMap pNewMap;
            pNewMap = new MapClass();
            //IEnvelope pEnvlope=new EnvelopeClass();

            pUID.Value = "{E156D7E5-22AF-11D3-9F99-00C04F6BC78E}";
            pMap = ClsDeclare.g_pMap;
            pEnumLyr = pMap.get_Layers(pUID, true);
            pGeoLyr = pEnumLyr.Next() as IGeoFeatureLayer;
            while (pGeoLyr != null)
            {
                if (pGeoLyr.FeatureClass != null)
                    pNewMap.AddLayer(pGeoLyr);
                pGeoLyr = pEnumLyr.Next() as IGeoFeatureLayer;
            }
            pTemEnumLyr = pNewMap.get_Layers(null,false);
            if (pNewMap.LayerCount != 0)
                m_pFeatureCache.AddLayers(pTemEnumLyr, pEnvlope);
        }
        public static IPoint GetSnapPoint(IPoint pPoint, IFeatureCache2 pFtCache, double Dist)
        {
            IHitTest pHitTest;
            IPoint pHitPt;
            bool bHit;
            double dDist;
            esriGeometryHitPartType hitType;
            int lPartIndex;
            int lSegIndex;
            IGeometry pGeom;
            pHitPt = new PointClass();

            bool bRightSide=false;
            bHit = false;
            for (int i = 0; i < pFtCache.Count; i++)
            { 
                lPartIndex = 0;
                lSegIndex = 0;
                dDist = 0;
                bHit = false;

                pGeom = pFtCache.get_Feature(i).ShapeCopy;
                pHitTest = (IHitTest)pGeom;

                if (pGeom.Dimension == esriGeometryDimension.esriGeometry0Dimension)
                    hitType = esriGeometryHitPartType.esriGeometryPartVertex;
                else
                    hitType = esriGeometryHitPartType.esriGeometryPartBoundary;
                bHit = pHitTest.HitTest(pPoint, Dist, hitType, pHitPt, ref dDist, ref lPartIndex, ref lSegIndex, ref bRightSide);
                if (bHit == true)
                    break;   
            }
            if (bHit == true)
                return pHitPt;
            else
                return null;
                
        }
        public static void DrawRectangle(IActiveView pActiveView, IPoint pPoint)
        {
            if (pPoint == null)
                return;
            pActiveView.ScreenDisplay.StartDrawing(pActiveView.ScreenDisplay.hDC,-1);
            pActiveView.ScreenDisplay.SetSymbol(g_pSnapSymbol);
            pActiveView.ScreenDisplay.DrawPoint(pPoint);
            pActiveView.ScreenDisplay.FinishDrawing();
            
        
        }
        public static ISimpleMarkerSymbol SetMeasureSnapSymbol()
        {
            ISimpleMarkerSymbol pMarkerSymbol;
            ISymbol pSymbol;
            IRgbColor pRgbColor;

            pRgbColor = new RgbColorClass();
            pRgbColor.Transparency = 0;

            pMarkerSymbol = new SimpleMarkerSymbolClass();
            pSymbol = (ISymbol)pMarkerSymbol;
            pSymbol.ROP2 = esriRasterOpCode.esriROPMaskNotPen;
            pMarkerSymbol.Color = pRgbColor;
            pMarkerSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle;

            pRgbColor.Red = 255;
            pRgbColor.Blue = 0;
            pRgbColor.Green = 0;

            pRgbColor.Transparency = 255;
            pMarkerSymbol.Outline = true;
            pMarkerSymbol.OutlineColor = pRgbColor;

            return pMarkerSymbol;

        }
        public static string GetMapUnits(IMap pMap)
        {
            switch (pMap.MapUnits)
            { 
                case esriUnits.esriCentimeters:
                    return "厘米";
                case esriUnits.esriFeet:
                    return "英尺";
                case esriUnits.esriKilometers:
                    return "公里";
                case esriUnits.esriMeters:
                    return "米";
                case esriUnits.esriMiles:
                    return "英里";
                case esriUnits.esriDecimalDegrees:
                    return "度";
                case esriUnits.esriUnknownUnits:
                    return "未知";
                default:
                    return "未知";

            }
        }

        
    }
}
