using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;

namespace SelectQuery
{
    public class ClsConvertUnit
    {
        esriUnits m_DefauleMapUnit, m_CurrentUnit;
        string m_CurrentUnitName;

        public ClsConvertUnit(esriUnits vDefaultMapUnit) //以坐标单位为参数的构造函数
        {
            m_DefauleMapUnit = vDefaultMapUnit;
            m_CurrentUnit = vDefaultMapUnit;
            m_CurrentUnitName = GetUnitNameChn(m_CurrentUnit);
        }
        public ClsConvertUnit(string vDefaultMapUnitName) //以坐标单位名为参数的构造函数
        {
            m_DefauleMapUnit = GetUnitNameFromChn(vDefaultMapUnitName);
            m_CurrentUnit = m_DefauleMapUnit;
            m_CurrentUnitName = vDefaultMapUnitName;
        }
        public void SetCurrentMapUnit(esriUnits vCurMapUnit)//设置当前坐标
        {
            m_CurrentUnit = vCurMapUnit;
            m_CurrentUnitName = GetUnitNameChn(m_CurrentUnit);
        }
        public void SetCurrentMapUnit(string vCurMapUnitName)//设置当前坐标
        {
            m_CurrentUnit = GetUnitNameFromChn(vCurMapUnitName);
            m_CurrentUnitName = vCurMapUnitName;
        }
        //转换点坐标，从默认坐标转换当前坐标
        public IPoint ConvertPointCoordinate(IPoint vPoint)
        {
            IUnitConverter pUnitConverter = new UnitConverter();
            IPoint pPt = new PointClass();
            pPt.X = pUnitConverter.ConvertUnits(vPoint.X,m_DefauleMapUnit,m_CurrentUnit);
            pPt.Y = pUnitConverter.ConvertUnits(vPoint.Y,m_DefauleMapUnit,m_CurrentUnit);
            return pPt;
        }
        //转换线坐标，从默认坐标转换当前坐标
        public IPolyline ConvertLineCoordinate(IPolyline vPolyline)
        {
            IPointCollection pPointColl;
            IPolyline pNewPolyline;
            IPointCollection pNewPointColl;
       
            pNewPolyline = new PolylineClass();
            pNewPointColl= (IPointCollection)pNewPolyline;
            pPointColl = (IPointCollection)vPolyline;
            object Missing = Type.Missing;
            object Missing1 = Type.Missing;
            for (int i = 0; i < pPointColl.PointCount; i++)
            {
                pNewPointColl.AddPoint(ConvertPointCoordinate(pPointColl.get_Point(i)),ref Missing,ref Missing1);
            }
            pNewPolyline = (IPolyline)pNewPointColl;
            return pNewPolyline;
        }
        //转换面坐标，从默认坐标转换当前坐标
        public IPolygon ConvertPolyonCoordinate(IPolygon vPolygon)
        {
            IPointCollection pPointColl;
            IPolygon pNewPolygon;
            IPointCollection pNewPointColl;

            pNewPolygon = new PolygonClass();
            pNewPointColl = (IPointCollection)pNewPolygon;
            pPointColl = (IPointCollection)vPolygon;
            object Missing = Type.Missing;
            object Missing1 = Type.Missing;
            for (int i = 0; i < pPointColl.PointCount; i++)
            {
                pNewPointColl.AddPoint(ConvertPointCoordinate(pPointColl.get_Point(i)), ref Missing, ref Missing1);
            }
            pNewPolygon = (IPolygon)pNewPointColl;
            return pNewPolygon;
        }
        public IGeometry ConvertGeometryCoordinate(IGeometry vGeometry)
        {
            if (vGeometry.GeometryType == esriGeometryType.esriGeometryPoint)
                return ConvertPointCoordinate((IPoint)(vGeometry));
            else if (vGeometry.GeometryType == esriGeometryType.esriGeometryPolyline)
                return ConvertLineCoordinate((IPolyline)vGeometry);
            else if (vGeometry.GeometryType == esriGeometryType.esriGeometryPolygon)
                return ConvertPolyonCoordinate((IPolygon)vGeometry);
            else
                return null;
        }

        public esriUnits DefaultMapUnit
        {
            get
            {
                return m_DefauleMapUnit;
            }
            set
            {
                m_DefauleMapUnit = value;
            }
        }
        public esriUnits CurrentUnit
        {
            get
            {
                return m_CurrentUnit;
            }
            set
            {
                m_CurrentUnit = value;
            }
        }
        public string CurrentUnitName
        {
            get
            {
                return m_CurrentUnitName;
            }
            set
            {
                m_CurrentUnitName = value;
            }
        }
        //坐标单位转换为中文
        public string GetUnitNameChn(esriUnits vUnit)
        { 
        switch(vUnit)
        {
            case esriUnits.esriKilometers:
                return "公里";
            case esriUnits.esriMeters:
                return "米";
            case esriUnits.esriCentimeters:
                return "厘米";
            case esriUnits.esriDecimeters:
                return "分米";
            case esriUnits.esriMillimeters:
                return "毫米";
            case esriUnits.esriDecimalDegrees:
                return "度";
            default:
                return "未知";

        }
            
             
        }
        public esriUnits GetUnitNameFromChn(string vUnitName)
        {
            switch (vUnitName)
            { 
                case "公里":
                    return esriUnits.esriKilometers;
                case "米":
                    return esriUnits.esriMeters;
                case "厘米":
                    return esriUnits.esriCentimeters;
                case "分米":
                    return esriUnits.esriDecimeters;
                case "毫米":
                    return esriUnits.esriMillimeters;
                case "度":
                    return esriUnits.esriDecimalDegrees;
                default:
                    return esriUnits.esriUnknownUnits;
            }
        }

        //internal object ConvertPolygonCoordinate(IPolygon pPolygon)
        //{
        //    throw new NotImplementedException();
        //}

      
    }
}
