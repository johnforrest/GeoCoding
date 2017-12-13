using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

namespace ZJGISDataUpdating 
{
    public class ClsConvertUnit
    {
        /// <summary>
        /// 转换点坐标，从默认坐标转换当前坐标
        /// </summary>
        /// <param name="vPoint"></param>
        /// <param name="defaultUnit"></param>
        /// <param name="currentUnit"></param>
        /// <returns></returns>
        public IPoint ConvertPointCoordinate(IPoint vPoint, esriUnits defaultUnit, esriUnits currentUnit)
        {
            IUnitConverter pUnitConverter = new UnitConverter();
            IPoint pPt = new PointClass();

            pPt.X = pUnitConverter.ConvertUnits(vPoint.X, defaultUnit, currentUnit);
            pPt.Y = pUnitConverter.ConvertUnits(vPoint.Y, defaultUnit, currentUnit);
            return pPt;
        }
        /// <summary>
        /// 转换线坐标，从默认坐标转换当前坐标
        /// </summary>
        /// <param name="vPolyline"></param>
        /// <param name="defaultUnit"></param>
        /// <param name="currentUnit"></param>
        /// <returns></returns>
        public IPolyline ConvertLineCoordinate(IPolyline vPolyline, esriUnits defaultUnit, esriUnits currentUnit)
        {
            IPointCollection pPointColl;
            IPolyline pNewPolyline;
            IPointCollection pNewPointColl;

            pNewPolyline = new PolylineClass();
            pNewPointColl = (IPointCollection)pNewPolyline;
            pPointColl = (IPointCollection)vPolyline;

            object missing1 = Type.Missing;
            object missing2 = Type.Missing;

            for (int i = 0; i < pPointColl.PointCount; i++)
            {
                pNewPointColl.AddPoint(ConvertPointCoordinate(pPointColl.get_Point(i), defaultUnit, currentUnit), ref missing1, ref missing2);
            }
            pNewPolyline = (IPolyline)pNewPointColl;
            return pNewPolyline;
        }
        ////转换面坐标，从默认坐标转换当前坐标
        //public IPolygon ConvertPolyonCoordinate(IPolygon vPolygon)
        //{
        //    IPointCollection pPointColl;
        //    IPolygon pNewPolygon;
        //    IPointCollection pNewPointColl;

        //    pNewPolygon = new PolygonClass();
        //    pNewPointColl = (IPointCollection)pNewPolygon;
        //    pPointColl = (IPointCollection)vPolygon;
        //    object Missing = Type.Missing;
        //    object Missing1 = Type.Missing;
        //    for (int i = 0; i < pPointColl.PointCount; i++)
        //    {
        //        pNewPointColl.AddPoint(ConvertPointCoordinate(pPointColl.get_Point(i)), ref Missing, ref Missing1);
        //    }
        //    pNewPolygon = (IPolygon)pNewPointColl;
        //    return pNewPolygon;
        //}

        /// <summary>
        /// 检查坐标系统
        /// </summary>
        /// <param name="cFeatCls">源数据要素</param>
        /// <param name="tuFeatCls">待匹配数据要素</param>
        /// <returns></returns>
        public bool CheckCoordinateSystem(IFeatureClass cFeatCls, IFeatureClass tuFeatCls)
        {
            try
            {
                IDataset cDataset = cFeatCls as IDataset;
                IGeoDataset cGeoDataset = cDataset as IGeoDataset;
                ISpatialReference cSpatialReference = cGeoDataset.SpatialReference;

                IDataset tuDataset = tuFeatCls as IDataset;
                IGeoDataset tuGeoDataset = tuDataset as IGeoDataset;
                ISpatialReference tuSpatialReference = tuGeoDataset.SpatialReference;

                if (cSpatialReference is IGeographicCoordinateSystem)               
                {
                    if (tuSpatialReference is IGeographicCoordinateSystem)
                    {
                        IGeographicCoordinateSystem cGeoCS = cSpatialReference as IGeographicCoordinateSystem;
                        IGeographicCoordinateSystem tuGeoCS = tuSpatialReference as IGeographicCoordinateSystem;

                        if (cGeoCS.Name == tuGeoCS.Name)
                        {
                            if (cGeoCS.CoordinateUnit.Name == tuGeoCS.CoordinateUnit.Name)
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("坐标系统单位不一致！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("坐标系统投影不一致！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("坐标系统不一致！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                else if (cSpatialReference is IProjectedCoordinateSystem)
                {
                    if (tuSpatialReference is IProjectedCoordinateSystem)
                    {
                        IProjectedCoordinateSystem cProCS = cSpatialReference as IProjectedCoordinateSystem;
                        IProjectedCoordinateSystem tuProCS = tuSpatialReference as IProjectedCoordinateSystem;

                        if (cProCS.Name == tuProCS.Name)
                        {
                            if (cProCS.CoordinateUnit.Name == tuProCS.CoordinateUnit.Name)
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("坐标系统单位不一致！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("坐标系统投影不一致！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("坐标系统不一致！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("更新图层坐标系统未知！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 得到缓冲区范围
        /// </summary>
        /// <param name="vSpatialReference">空间参考</param>
        /// <param name="buffer">缓冲区距离</param>
        /// <returns></returns>
        public double GetBufferValueByUnit(ISpatialReference vSpatialReference,double buffer)
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
            //将获得的英文名称转换成其对应的中文单位名称
            double revalue = 0;
            switch (sUnitName.ToUpper())
            {
                case "METER":
                    return buffer;
                case "CENTIMETRE":
                    IUnitConverter pUnitConverter = new UnitConverter();
                    revalue=pUnitConverter.ConvertUnits(buffer,esriUnits.esriMeters,esriUnits.esriCentimeters);
                    return revalue;
                case "KILOMETRES":
                    IUnitConverter pUnitConverter2 = new UnitConverter();
                    revalue = pUnitConverter2.ConvertUnits(buffer, esriUnits.esriMeters, esriUnits.esriKilometers);
                    return revalue;
                case "DEGREE":
                    IUnitConverter pUnitConverter1 = new UnitConverter();
                    revalue = pUnitConverter1.ConvertUnits(buffer, esriUnits.esriMeters,esriUnits.esriDecimalDegrees);
                    return revalue;
                default:
                    MessageBox.Show("坐标系统单位未知！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return 0;
            }

        }

    }
}
