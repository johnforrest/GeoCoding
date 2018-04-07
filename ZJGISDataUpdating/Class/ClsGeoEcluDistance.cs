/*-------------------------------------------------------------------------
 * 版权所有：武汉大学
 * 创建人：johnforrest
 * 联系方式：571716604@qq.com
 * 创建时间： 4/7/2018 9:09:01 AM
 * 类名称：ClsGeoEcluDistance
 * 本类主要用途描述：
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ZJGISDataUpdating.Class
{
    public static class ClsGeoEcluDistance
    {
        public static double HaverSin(double theta)
        {
            var v = Math.Sin(theta / 2);
            return v * v;
        }


        static double EARTH_RADIUS = 6371.0;//km 地球半径 平均值，千米

        /// <summary>
        /// 给定经纬度计算其距离
        /// </summary>
        /// <param name="lat1">纬度1</param>
        /// <param name="lon1">经度1</param>
        /// <param name="lat2">纬度2</param>
        /// <param name="lon2">经度2</param>
        /// <returns>返回米</returns>
        public static double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            //用haversine公式计算球面两点间的距离。
            //经纬度转换成弧度
            lat1 = ConvertDegreesToRadians(lat1);
            lon1 = ConvertDegreesToRadians(lon1);
            lat2 = ConvertDegreesToRadians(lat2);
            lon2 = ConvertDegreesToRadians(lon2);

            //差值
            double vLon = Math.Abs(lon1 - lon2);
            double vLat = Math.Abs(lat1 - lat2);

            //h is the great circle distance in radians, great circle就是一个球体上的切面，它的圆心即是球心的一个周长最大的圆。
            double h = HaverSin(vLat) + Math.Cos(lat1) * Math.Cos(lat2) * HaverSin(vLon);

            double distance = 2 * EARTH_RADIUS * Math.Asin(Math.Sqrt(h));
            distance = Math.Round(distance * 10000) / 10000;
            return distance*1000.0;
        }

        /// <summary>
        /// 将角度换算为弧度。
        /// </summary>
        /// <param name="degrees">角度</param>
        /// <returns>弧度</returns>
        public static double ConvertDegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double ConvertRadiansToDegrees(double radian)
        {
            return radian * 180.0 / Math.PI;
        }



    }
}
