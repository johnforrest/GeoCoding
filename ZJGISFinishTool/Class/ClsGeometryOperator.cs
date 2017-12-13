using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ZJGISFinishTool
{
    class ClsGeometryOperator
    {
        //<CSCM>
        //********************************************************************************
        //** 函 数 名: UnionTwoGeometry
        //** 版    权: CopyRight (C)  
        //** 创 建 人: 杨旭斌
        //** 功能描述: 合并二个图形成一个图形
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间: 20070818
        //** 参数列表: pOriGeometry (IGeometry)需要合并的二个图形
        //             pDesOrigeometry (IGeometry)
        //             bHasErr合并过程中是否产生了错误
        //             sErrDes 错误描述
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static IGeometry UnionTwoGeometry(IGeometry pOriGeometry, IGeometry pDesGeometry, ref bool bHasErr, ref string sErrDes)
        {
            //bHasErr = false;
            //sErrDes = "";

            IGeometry functionReturnValue = default(IGeometry);
            ITopologicalOperator2 pTopoOperator = default(ITopologicalOperator2);
            IGeometry pGeometry = default(IGeometry);
            //如果图形为空,则置为nothing
            bHasErr = false;
            sErrDes = "";
            if (pOriGeometry != null && pDesGeometry != null)
            {
                if (pOriGeometry.SpatialReference == null & pDesGeometry.SpatialReference != null)
                {
                    if (pOriGeometry.SpatialReference.FactoryCode != pDesGeometry.SpatialReference.FactoryCode)
                    {
                        bHasErr = true;
                        sErrDes = "二个图形的投影坐标系不相同,不能进行求交。";
                        return null;
                    }
                }
            }
            if (pOriGeometry != null)
            {
                if (pOriGeometry.IsEmpty == true)
                    pOriGeometry = null;
            }
            if (pDesGeometry != null)
            {
                if (pDesGeometry.IsEmpty == true)
                    pDesGeometry = null;
            }
            try
            {
                if (pOriGeometry != null && pDesGeometry != null)
                {
                    pTopoOperator = (ITopologicalOperator2)pOriGeometry;
                    pGeometry = pTopoOperator.Union(pDesGeometry);
                    pTopoOperator = (ITopologicalOperator2)pGeometry;
                    pTopoOperator.IsKnownSimple_2 = false;
                    pTopoOperator.Simplify();
                    functionReturnValue = (IGeometry)pTopoOperator;
                    return functionReturnValue;
                }
                else if (pOriGeometry != null)
                {
                    return pOriGeometry;
                }
                else if (pDesGeometry != null)
                {
                    return pDesGeometry;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                functionReturnValue = null;
            }
            return functionReturnValue;
        }

        //<CSCM>
        //********************************************************************************
        //** 函 数 名: IntersectTwoGeometry
        //** 版    权: CopyRight (C) 
        //** 创 建 人: 杨旭斌
        //** 功能描述: 二个图形求差
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间: 20070818
        //** 参数列表: pOriGeometry (IGeometry)
        //         pDesGeometry (IGeometry)
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static IGeometry IntersectTwoGeometry(IGeometry pOriGeometry, IGeometry pDesGeometry)
        {
            IGeometry functionReturnValue = default(IGeometry);
            ITopologicalOperator2 pTopoOperator = default(ITopologicalOperator2);
            ITopologicalOperator2 pTopoOperator2 = default(ITopologicalOperator2);
            IGeometry pGeometry = default(IGeometry);

            // ERROR: Not supported in C#: OnErrorStatement
            try
            {
                pTopoOperator = (ITopologicalOperator2)pOriGeometry;
                pTopoOperator.IsKnownSimple_2 = false;
                pTopoOperator.Simplify();

                pTopoOperator2 = (ITopologicalOperator2)pDesGeometry;
                pTopoOperator2.IsKnownSimple_2 = false;
                pTopoOperator2.Simplify();
                pGeometry = pTopoOperator.Intersect((IGeometry)pTopoOperator2, esriGeometryDimension.esriGeometry2Dimension);
                pTopoOperator = (ITopologicalOperator2)pGeometry;
                pTopoOperator.IsKnownSimple_2 = false;
                pTopoOperator.Simplify();
                functionReturnValue = (IGeometry)pTopoOperator;
                return functionReturnValue;
            }
            catch (Exception)
            {
                functionReturnValue = null;
                return functionReturnValue;
            }

        }

        //<CSCM>
        //********************************************************************************
        //** 函 数 名: CutFeatByGeometry
        //** 版    权: CopyRight (C) 
        //** 创 建 人: 杨旭斌
        //** 功能描述: 根据传入的图形,剪切需要被剪切的要素
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间: 2006-10-13
        //** 参数列表: pOriGeometry (IGeometry)'需要被剪切的要素
        //             pRefGeometry (IGeometry)'参照要素
        //             EnumGeometryDimension (esriGeometryDimension)'返回的图形的维数
        //               线:esriGeometry1Dimension 用于面剪切线;
        //               面:esriGeometry2Dimension 用于面剪切面
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static IGeometry CutFeatByGeometry(ref IGeometry pOriGeometry, ref IGeometry pRefGeometry, ref esriGeometryDimension EnumGeometryDimension)
        {
            IGeometry functionReturnValue = default(IGeometry);
            ITopologicalOperator2 pTopoOperator = default(ITopologicalOperator2);
            ITopologicalOperator2 pTopoOperator2 = default(ITopologicalOperator2);
            //原要素和参照要素相交的部分
            IGeometry pGeometry = default(IGeometry);

            // ERROR: Not supported in C#: OnErrorStatement
            try
            {
                pTopoOperator = (ITopologicalOperator2)pRefGeometry;
                pTopoOperator.IsKnownSimple_2 = false;
                pTopoOperator.Simplify();
                pTopoOperator2 = (ITopologicalOperator2)pOriGeometry;
                pTopoOperator2.IsKnownSimple_2 = false;
                pTopoOperator2.Simplify();
                //根据不同的需要返回点,线,面等相交的部分
                pGeometry = pTopoOperator2.Intersect((IGeometry)pTopoOperator, EnumGeometryDimension);
                pTopoOperator = (ITopologicalOperator2)pGeometry;
                pTopoOperator.IsKnownSimple_2 = false;
                pTopoOperator.Simplify();
                functionReturnValue = (IGeometry)pTopoOperator;
                return functionReturnValue;
            }
            catch (Exception)
            {
                functionReturnValue = null;
                return functionReturnValue;
            }
        }

    }
}
