//<CSCC>
//-------------------------------------------------------------------------------------
//    部件名：FrmLyrExtent
//    工程名：LayerManager
//    版权: CopyRight (c) 2010
//    创建人：
//    描述：显示图层的范围和数据源信息
//    创建日期：
//    修改人：
//    修改说明：
//    修改日期：
//-------------------------------------------------------------------------------------
//</CSCC>


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesRaster;


namespace ZJGISLayerManager
{
    public partial class FrmLyrExtent :DevComponents.DotNetBar.Office2007Form
    {
        private ILayer m_pLayer;

        public ILayer Layer
        {
            set
            {
                m_pLayer = value;
            }
        }

        public FrmLyrExtent()
        {
            InitializeComponent();
        }

        private void FrmLyrExtent_Load(object sender, EventArgs e)
        {
            this.DGVSource.ReadOnly = true;         //只读控件不允许修改
            ShowLayerExtent();                      //显示数据源范围
            ShowLayerDataSource(m_pLayer);          //显示数据源信息
        }

        //显示图层范围
        public void ShowLayerExtent()
        {
            IEnvelope pExtent;
            double dTop, dLeft, dRight, dButtom;

            //获取当前图层的范围
            pExtent = GetLayerExtent(m_pLayer);
            dTop = pExtent.YMax;
            dButtom = pExtent.YMin;
            dLeft = pExtent.XMin;
            dRight = pExtent.XMax;

            //格式化，6位小数
            dTop = Convert.ToDouble(string.Format("{0:F6}", dTop));
            dLeft = Convert.ToDouble(string.Format("{0:F6}", dLeft));
            dRight = Convert.ToDouble(string.Format("{0:F6}", dRight));
            dButtom = Convert.ToDouble(string.Format("{0:F6}", dButtom));

            //dTop = Strings.FormatNumber(dTop, 6, TriState.True);
            //dLeft = Strings.FormatNumber(dLeft, 6, TriState.True);
            //dRight = Strings.FormatNumber(dRight, 6, TriState.True);
            //dButtom = Strings.FormatNumber(dButtom, 6, TriState.True);

            //显示范围取值
            lblTop.Text = "上：" + dTop;
            lblLeft.Text = "左：" + dLeft;
            lblRight.Text = "右：" + dRight;
            lblBottom.Text = "下：" + dButtom;
        }

        //获取矢量或栅格图层的范围
        public IEnvelope GetLayerExtent(ILayer pLayer)
        {
            IRasterLayer pRasterLayer;
            IFeatureLayer pFeatureLayer;
            IGeoDataset pGeoDS;

            //QI the geodataset:
            if (pLayer is IRasterLayer)
            {
                pRasterLayer = (IRasterLayer)pLayer;
                pGeoDS = (IGeoDataset)pRasterLayer;
            }
            else if (pLayer is IFeatureLayer)
            {
                pFeatureLayer = (IFeatureLayer)pLayer;
                pGeoDS = (IGeoDataset)pFeatureLayer;
            }
            else
            {
                pGeoDS = null;
            }

            //return extent if found
            if (pGeoDS!=null)
            {
                return pGeoDS.Extent;
            }
            else
            {
                return null;
            }
        }

        //自定义过程，显示指定图层的数据源信息，pLayer 为指定图层名称
        //支持RasterLayer,TinLayer,FeatureLayer
        public void ShowLayerDataSource(ILayer pLayer)
        {
            IRasterLayer pRasterLayer;
            ITinLayer pTinLayer;
            IFeatureLayer pFeatureLayer;

            int i;
            //获取图层的空间参考
            ISpatialReference pSR;
            pSR = GetLayerSourceSpatialRef(pLayer);

            IDataset pDataSet;
            IWorkspace pWorkspace;
            DataGridViewRow vRow;

            this.DGVSource.Rows.Clear();        //先清空

            //根据图层的类型获取该图层的数据集信息
            if (pLayer is IRasterLayer)             //Raster数据
            {
                pRasterLayer = (IRasterLayer)pLayer;

                //数据所在路径名称
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "文件路径：";
                vRow.Cells[1].Value = pRasterLayer.FilePath;

                //行列数
                int nColumns, nRows;
                nColumns = pRasterLayer.ColumnCount;
                nRows = pRasterLayer.RowCount;

                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "列数和行数:";
                vRow.Cells[1].Value = nColumns + "," + nRows;

                //波段数
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "波段数：";
                vRow.Cells[1].Value = pRasterLayer.BandCount;

                IRaster pRaster = pRasterLayer.Raster;
                IRasterBandCollection pBandCol = (IRasterBandCollection)pRaster;
                IRasterBand pBand = pBandCol.Item(0);
                bool bHasColormap;
                pBand.HasColormap(out bHasColormap);                //判断是否含有颜色图

                IRasterProps pRasBProps = (IRasterProps)pBand;                    //Raster属性接口

                //获取格网大小
                double cX, cY;
                cX = pRasBProps.MeanCellSize().X;
                cY = pRasBProps.MeanCellSize().Y;

                //填充格网大小
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "格网大小(X,Y)：";
                vRow.Cells[1].Value = cX + "," + cY;

                IRasterDataset pRDS = (IRasterDataset)pBand;                        //获取数据集

                //获取数据格式
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "数据格式：";
                vRow.Cells[1].Value = pRDS.Format;

                //获取像素数据类型和byte位数
                string[] tempItem = new string[2];
                vRow=this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "像素类型：";
                tempItem[0] = "像素深度：";
                switch ((int)pRasBProps.PixelType)
                {
                    case 0:
                        vRow.Cells[1].Value = "Byte";
                        tempItem[1] = "1 Bit";
                        break;
                    case 1:
                        vRow.Cells[1].Value = "Byte";
                        tempItem[1] = "2 Bit";
                        break;
                    case 2:
                        vRow.Cells[1].Value = "Byte";
                        tempItem[1] = "4 Bit";
                        break;
                    case 3:
                        vRow.Cells[1].Value = "unsigned integer";
                        tempItem[1] = "8 Bit";
                        break;
                    case 4:
                        vRow.Cells[1].Value = "integer";
                        tempItem[1] = "8 Bit";
                        break;
                    case 5:
                        vRow.Cells[1].Value = "unsigned integer";
                        tempItem[1] = "16 Bit";
                        break;
                    case 6:
                        vRow.Cells[1].Value = "integer";
                        tempItem[1] = "16 Bit";
                        break;
                    case 7:
                        vRow.Cells[1].Value = "unsigned integer";
                        tempItem[1] = "32 Bit";
                        break;
                    case 8:
                        vRow.Cells[1].Value = "interger";
                        tempItem[1] = "32 Bit";
                        break;
                    case 9:
                        vRow.Cells[1].Value = "single";
                        tempItem[1] = "32 Bit";
                        break;
                    case 10:
                        vRow.Cells[1].Value = "double";
                        tempItem[1] = "64 Bit";
                        break;
                    case 11:
                        vRow.Cells[1].Value = "Complex";
                        tempItem[1] = "unknown";
                        break;
                    default:
                        break;
                }

                //填充空值信息
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "空值：";
                vRow.Cells[1].Value = pRasBProps.NoDataValue;

                //填充是否含有颜色图
                vRow.Cells[0].Value = "颜色图";
                if (bHasColormap==true)
                {
                    vRow.Cells[1].Value = "存在";
                }
                else
                {
                    vRow.Cells[1].Value = "不存在";
                }

                //是否存在金字塔
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "金字塔：";
                if (pRasterLayer.PyramidPresent)
                {
                    vRow.Cells[1].Value = "存在";
                }
                else
                {
                    vRow.Cells[1].Value = "不存在";
                }

                //判断数据压缩类型
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "压缩类型：";
                vRow.Cells[1].Value = pRDS.CompressionType;                 //Raster数据压缩类型


                //*************  获取每个波段的统计值  ************************
                //lvwSource.Items.Add("波段统计")

                bool bHasStatistic;         //是否含有统计量
                IRasterStatistics pRasStatistic;
                for (i = 0; i <= pRasterLayer.BandCount-1; i++)
                {
                    pBand = pBandCol.Item(i);

                    //波段名称

                    //判断是否含有统计量
                    pBand.HasStatistics(out bHasStatistic);
                    if (bHasStatistic==true)
                    {
                        //获取统计接口
                        pRasStatistic = pBand.Statistics;

                        //填充创建band的参数信息
                        vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                        vRow.Cells[0].Value = "创建参数：";
                        string strTemp;
                        strTemp = "横轴跳跃因子:" + pRasStatistic.SkipFactorX;          //X方向跳跃因子
                        strTemp = strTemp + ",纵轴跳跃因子:" + pRasStatistic.SkipFactorY;       //Y方向跳跃因子
                        strTemp = strTemp + ",忽略值:";                                 //忽略值
                        if ((string)pRasStatistic.IgnoredValues!="")
                        {
                            strTemp = strTemp + pRasStatistic.IgnoredValues;
                        }
                        vRow.Cells[1].Value = strTemp;

                        //最小灰度值
                        vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                        vRow.Cells[0].Value = "最小值：";
                        vRow.Cells[1].Value = pRasStatistic.Minimum;

                        //最大灰度值
                        vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                        vRow.Cells[0].Value = "最大值：";
                        vRow.Cells[1].Value = pRasStatistic.Maximum;

                        //平均值
                        vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                        vRow.Cells[0].Value = "平均值：";
                        vRow.Cells[1].Value = pRasStatistic.Mean;

                        //标准方差
                        vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                        vRow.Cells[0].Value = "标准方差：";
                        vRow.Cells[1].Value = pRasStatistic.StandardDeviation;
                    }
                    else
                    {
                        vRow.Cells[0].Value = "无统计值";
                    }

                }
                //**********************************************************************
                pRaster = null;
                pBandCol = null;
                pBand = null;
                pRasBProps = null;
                pRasStatistic = null;
            }
            else if (pLayer is ITinLayer)           //TIN数据
            {
                pTinLayer = (ITinLayer)pLayer;
                ITin pTin = pTinLayer.Dataset;

                //获取数据集所在的工作区间
                pDataSet = (IDataset)pTin;
                pWorkspace = pDataSet.Workspace;

                //获取Z值的范围
                double Zmin, Zmax;
                Zmin = pTinLayer.AreaOfInterest.ZMin;
                Zmax = pTinLayer.AreaOfInterest.ZMax;

                //保留6位小数
                Zmin = Convert.ToDouble(string.Format("{0:F6}", Zmin));
                Zmax = Convert.ToDouble(string.Format("{0:F6}", Zmax));

                //填充数据类型
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "数据类型：";
                vRow.Cells[1].Value = "TIN";

                //填充数据存放路径
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "数据目录：";
                vRow.Cells[1].Value = @pWorkspace.PathName;

                //填充TIN中结点总数
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "结点总数：";
                vRow.Cells[1].Value = pTin.DataNodeCount;

                //填充TIN中三角形总数
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "三角形总数：";
                vRow.Cells[1].Value = pTin.DataTriangleCount;

                //填充TIN中高程范围
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "高程范围：";
                vRow.Cells[1].Value = "(" + Zmin + "," + Zmax + ")";

            }

            else if (pLayer is IFeatureLayer)           //shape数据
            {
                pFeatureLayer = (IFeatureLayer)pLayer;
                IFeatureClass pFeatClass = pFeatureLayer.FeatureClass;

                //获取工作区间
                pDataSet = (IDataset)pFeatClass;
                pWorkspace = pDataSet.Workspace;

                //数据类型
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "数据类型：";
                vRow.Cells[1].Value = pFeatureLayer.DataSourceType;

                //图层路径全称
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "文件路径：";
                if (pFeatureLayer.DataSourceType=="Shapefile Feature Class")
                {
                    vRow.Cells[1].Value =@pWorkspace.PathName + "\\" + pFeatClass.AliasName + ".shp";
                }
                else
                {
                    vRow.Cells[1].Value = @pWorkspace.PathName + "\\" + pFeatClass.AliasName;
                }

                //要素类名称
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "要素类名称：";
                vRow.Cells[1].Value = pFeatureLayer.FeatureClass.AliasName;

                //要素类型
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "要素类型：";
                vRow.Cells[1].Value = GetFeatureType(pFeatureLayer);

                //判断几何对象形状类型
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "几何形状：";
                switch ((int)pFeatClass.ShapeType)          //获取形状取值
                {
                    case 1:
                    case 2:
                        vRow.Cells[1].Value = "Point";
                        break;
                    case 3:
                    case 6:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 20:
                        vRow.Cells[1].Value = "Line";
                        break;
                    case 4:
                    case 5:
                    case 9:
                    case 11:
                    case 18:
                    case 19:
                    case 21:
                    case 22:
                        vRow.Cells[1].Value = "Polygon";
                        break;
                    default:
                        vRow.Cells[1].Value = "Unknown";
                        break;
                }

                pFeatClass = null;
            }

            ILinearUnit pLU;
            IGeographicCoordinateSystem pGCS;                   //地理坐标系

            //获取空间参考的更多信息

            //投影坐标系，则获取投影的相关信息
            if (pSR is IProjectedCoordinateSystem)
            {
                IProjectedCoordinateSystem pPCS;
                pPCS = (IProjectedCoordinateSystem)pSR;
                pLU = pPCS.CoordinateUnit;                      //坐标单位

                //投影坐标系名称
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "投影坐标系：";
                vRow.Cells[1].Value = pPCS.Name;

                //获取投影
                IProjection pProjection;
                pProjection = (IProjection)pPCS.Projection;

                //投影名称
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "投影名称：";
                vRow.Cells[1].Value = pProjection.Name;

                //东伪偏移值和北伪偏移值
                double dFalseEasting, dFalseNorthing;
                dFalseEasting = pPCS.FalseEasting;
                dFalseNorthing = pPCS.FalseNorthing;

                //东伪偏移值
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "东伪偏移值：";
                vRow.Cells[1].Value = dFalseEasting.ToString();

                //北伪偏移值
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "北伪偏移值：";
                vRow.Cells[1].Value = dFalseNorthing.ToString();

                double dCentralMeridian = pPCS.get_CentralMeridian(true);
                //中央经线
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "中央经线：";
                vRow.Cells[1].Value = dCentralMeridian.ToString();

                //获取地理坐标系统相关信息
                pGCS = pPCS.GeographicCoordinateSystem;

                //地理坐标系统名称
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "地理坐标系：";
                vRow.Cells[1].Value = pGCS.Name;

                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "大地基准面：";
                vRow.Cells[1].Value = pGCS.Datum.Name;


                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "本初子午线经度：";
                vRow.Cells[1].Value = pGCS.PrimeMeridian.Longitude;

                pPCS = null;
                pProjection = null;
                pGCS = null;

            }

            //地理坐标系，则获取地理相关信息
            else if (pSR is IGeographicCoordinateSystem)        //地理坐标系统名称
            {
                pGCS = (IGeographicCoordinateSystem)pSR;

                pLU = pGCS.ZCoordinateUnit;                     //坐标单位

                //坐标系名称
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "地理坐标系：";
                vRow.Cells[1].Value = pGCS.Name;

                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "大地基准面：";
                vRow.Cells[1].Value = pGCS.Datum.Name;

                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "本初子午线经度：";
                vRow.Cells[1].Value = pGCS.PrimeMeridian.Longitude;
            }
            else
            {
                pLU = pSR.ZCoordinateUnit;                      //获取Z坐标单位
            }

            //Linear Unit
            if (pLU!=null)
            {
                vRow = this.DGVSource.Rows[this.DGVSource.Rows.Add()];
                vRow.Cells[0].Value = "线性单位：";
                vRow.Cells[1].Value = pLU.Name;
            }

            //释放变量
            pRasterLayer = null;
            pTinLayer = null;
            pFeatureLayer = null;
            pSR = null;
            pLU = null;
            pGCS = null;
            pDataSet = null;
            pWorkspace=null;

        }

        //获取矢量或栅格图层的数据源的空间参考
        public ISpatialReference GetLayerSourceSpatialRef(ILayer pLayer)
        {
            IGeoDataset pGDS = null;
            if (pLayer is IFeatureLayer)
            {
                IFeatureLayer pFLayer;
                pFLayer = (IFeatureLayer)pLayer;
                pGDS = (IGeoDataset)pFLayer.FeatureClass;
            }
            else if (pLayer is ITinLayer)
            {
                ITinLayer pTLayer;
                pTLayer = (ITinLayer)pLayer;
                pGDS = (IGeoDataset)pTLayer.Dataset;
            }
            else if (pLayer is IRasterLayer)
            {
                IRasterLayer pRLayer;
                pRLayer = (IRasterLayer)pLayer;
                IRasterBandCollection pRasterBands;
                pRasterBands = (IRasterBandCollection)pRLayer.Raster;
                IRasterBand pRasterBand;
                pRasterBand = pRasterBands.Item(0);
                pGDS = (IGeoDataset)pRasterBand.RasterDataset;
            }

            if (pGDS!=null)
            {
                return pGDS.SpatialReference;
            }
            else
            {
                return null;
            }
        }

        //获取矢量图层的要素类型
        public string GetFeatureType(IFeatureLayer pFeatLayer)
        {
            IFeatureClass pFeatClass;
            pFeatClass = pFeatLayer.FeatureClass;

            switch (pFeatClass.FeatureType)
            {
                case esriFeatureType.esriFTSimple:
                    return "Simple";
                case esriFeatureType.esriFTSimpleEdge:
                    return "SimpleEdge";
                case esriFeatureType.esriFTSimpleJunction:
                    return "SimpleJunction";
                case esriFeatureType.esriFTComplexEdge:
                    return "ComplexEdge";
                case esriFeatureType.esriFTComplexJunction:
                    return "ComplexJunction";
                case esriFeatureType.esriFTAnnotation:
                    return "Annotation";
                case esriFeatureType.esriFTCoverageAnnotation:
                    return "CoverageAnnotation";
                case esriFeatureType.esriFTDimension:
                    return "Dimension";
                case esriFeatureType.esriFTRasterCatalogItem:
                    return "RasterCatalog";
                default:
                    return "Unknown";
            }

            pFeatClass = null;
        }

    }
}
