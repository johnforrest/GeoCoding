/*-------------------------------------------------------------------------
 * 版权所有：武汉大学
 * 创建人：johnforrest
 * 联系方式：571716604@qq.com
 * 创建时间： 4/27/2018 9:24:04 AM
 * 类名称：ClsGeoManhattanDistance
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
using System.Threading.Tasks;

namespace ZJGISDataUpdating.Class
{

    public  class ClsGeoManhattanDistance
    {
         double EARTH_RADIUS = 6371.0;//km 地球半径 平均值，千米
        /// <summary>
        /// 给定经纬度计算其曼哈顿距离(只适用于二者都在北半球)
        /// </summary>
        /// <param name="lat1">纬度1</param>
        /// <param name="lon1">经度1</param>
        /// <param name="lat2">纬度2</param>
        /// <param name="lon2">经度2</param>
        /// <returns>返回米</returns>
        public double ManhattanDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double dLatDis = 0;
            double dLonDis = 0;
            double latMax = 0;

            dLatDis = (Math.Abs(lat1 - lat2)/360)*2*Math.PI*EARTH_RADIUS; //单位千米
            if (lat1>lat2)
            {
                latMax = lat1;
            } 
            else
            {
                latMax = lat2;
            }
            dLonDis = (Math.Abs(lon1 - lon2) / 360) * 2 * Math.PI*Math.Sin(90 - latMax) * EARTH_RADIUS;

            return (Math.Round((dLonDis + dLatDis) * 10000) / 10000)*1000.0;
        }
    }
}
