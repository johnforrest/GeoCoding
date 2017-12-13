using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesFile;
using System.IO;
using DevComponents.DotNetBar;
//using ESRI.ArcGIS.r

namespace ZJGISDataExtract
{
    class ClsAddData
    {
        private const string c_sModuleFileName = "WHFUtilities_modOpenData";
        //是否比较加入图层与当前MAP的空间参考是否一致 
        private static bool blnEditMapSpatial;

        //// 加载选中图层
        ////blnAddData是否加载数据到Map

        public static bool AddSelectedLayer(ref IBasicMap pMap, string pSuffix, List<IFeatureLayer> pListCol, List<IDataset> m_DatasetCol, string pCurrentFilePath, bool blnAddData, int intPyrmd)
        {
            //intPyrmd=0;

            bool functionReturnValue = false;

            functionReturnValue = false;
            //WHFErrorHandle.clsErrorHandle pfrmError = new WHFErrorHandle.clsErrorHandle();
            System.Windows.Forms.Cursor pCursor = Cursors.Default;
            int i = 0;

            //Dim pFileName As IFileName
            IWorkspaceFactory pWorkspaceFactory = default(IWorkspaceFactory);
            //Dim pRasterWorkspace As IRasterWorkspace2
            //Dim pRasterDataset As IRasterDataset
            //Dim pRasterLayer As IRasterLayer

            IFeatureWorkspace pFeatWorkspace = default(IFeatureWorkspace);
            //Dim pFeatDS As IFeatureDataset
            IDataset pDataset = default(IDataset);
            //Dim pFeatCls As IFeatureClass
            IFeatureLayer pFeatLayer = default(IFeatureLayer);
            //Dim pLayer As ILayer
            ////以下三个变量是用来获得工作区的路径和文件的名称
            //Dim sPath() As String
            string sWorkSpacePath = pCurrentFilePath;
            string sFileName = null;

            if (pListCol.Count == 0)
                return functionReturnValue;

            //要进行空间参考的比较
            blnEditMapSpatial = true;
            try
            {
                switch (pSuffix)
                {
                    case ".SHP":
                        //shp文件的处理
                        //获得工作区,并通过shp文件的名称打开文件
                        pWorkspaceFactory = new ShapefileWorkspaceFactory();
                        if (pWorkspaceFactory.IsWorkspace(sWorkSpacePath))
                        {
                            pFeatWorkspace = (IFeatureWorkspace)pWorkspaceFactory.OpenFromFile(sWorkSpacePath, 0);
                            for (i = 1; i <= pListCol.Count; i++)
                            {
                                sFileName = (string)pListCol[i].Name;
                                pDataset = (IDataset)pFeatWorkspace.OpenFeatureClass(sFileName);
                                m_DatasetCol.Add(pDataset);
                                pFeatLayer = new FeatureLayer();
                                //设置图层的名字和要素类
                                pFeatLayer.FeatureClass = (IFeatureClass)pDataset;
                                pFeatLayer.Name = sFileName.Substring(0, sFileName.Length - 4);
                                //pFeatLayer.Name = Strings.Left(sFileName, Strings.Len(sFileName) - 4);
                            }
                            functionReturnValue = true;
                        }
                        break;
                    //Case ".IMG", ".TIF", ".BMP", ".JPG"
                    //    pWorkspaceFactory = New RasterWorkspaceFactory
                    //    '创建并添加栅格图层
                    //    If pWorkspaceFactory.IsWorkspace(sWorkSpacePath) Then
                    //        pRasterWorkspace = pWorkspaceFactory.OpenFromFile(sWorkSpacePath, 0)
                    //        Dim pRasPyrmd As IRasterPyramid
                    //        Dim intRslt As Integer
                    //        Dim intSameDo As Integer
                    //        For i = 1 To pListCol.Count
                    //            sFileName = pListCol.Item(i)
                    //            pRasterDataset = pRasterWorkspace.OpenRasterDataset(sFileName)
                    //            m_DatasetCol.Add(pRasterDataset)
                    //            '//判断栅格金字塔是否存在,询问是否构建
                    //            pRasPyrmd = pRasterDataset

                    //            '//如果金字塔存在则直接添加到地图，如果不存在则提示
                    //            If Not pRasPyrmd.Present Then
                    //                Select Case intPyrmd
                    //                    Case 0
                    //                        '//intSamedo决定是否选择了"采用相同设置".0代表否,1代表是
                    //                        '//intResult 表示用户点击的按钮0,1,2分别代表"否","是","退出"
                    //                        intRslt = pfrmError.DisplayInformationEx("添加的栅格数据没有构建金字塔,是否构建?", sFileName & "金字塔构建", "采用相同设置,不再询问", 4, intSameDo, "是", "否", "退出")

                    //                        '// 0则不创建,不做任何处理
                    //                        If intRslt = 1 Then      '//创建
                    //                            Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    //                            DoEvents()
                    //                            pRasPyrmd.Create()
                    //                            If intSameDo = 1 Then
                    //                                intPyrmd = 1
                    //                            End If
                    //                            Windows.Forms.Cursor.Current = Cursors.Default
                    //                        ElseIf intRslt = 0 Then
                    //                            '//接下来的图幅不创建金字塔，直接加载到地图，设置intPyrmd为3不做判断
                    //                            If intSameDo = 1 Then
                    //                                intPyrmd = 3
                    //                            End If
                    //                        End If
                    //                    Case 1
                    //                        Windows.Forms.Cursor.Current = Cursors.WaitCursor
                    //                        DoEvents()
                    //                        pRasPyrmd.Create()
                    //                        Windows.Forms.Cursor.Current = Cursors.Default
                    //                    Case 2      '//退出创建,不加载该栅格数据
                    //                        If intSameDo = 1 Then
                    //                            intPyrmd = 2
                    //                            Exit Function
                    //                        End If
                    //                End Select
                    //            End If
                    //            pRasterLayer = New RasterLayer
                    //            pRasterLayer.CreateFromDataset(pRasterDataset)
                    //        Next
                    //        AddSelectedLayer = True
                    //    End If

                    case ".DWG":
                    case ".DXF":
                    case ".DGN":
                        functionReturnValue = AddCADLayer(ref pMap, pCurrentFilePath, pListCol, blnAddData);
                        break;
                    case ".MXD":
                        string strMxdName = null;
                        IMapDocument pMapDoc = default(IMapDocument);
                        IMap pTmpMap = default(IMap);

                        sFileName = (string)pListCol[1].Name;
                        strMxdName = sWorkSpacePath + "\\" + sFileName;
                        pMapDoc = new MapDocument();
                        pMapDoc.Open(strMxdName, null);
                        pTmpMap = pMapDoc.get_Map(0);

                        break;
                    case "IDatasetName":

                        IDatasetName pDSN = default(IDatasetName);
                        for (i = 1; i <= pListCol.Count; i++)
                        {
                            pDSN.Name = pListCol[i].Name;
                            functionReturnValue = AddDataset(ref pMap, pDSN, m_DatasetCol, blnAddData);
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
            return functionReturnValue;

        }


        // // 添加CAD,dxf,dgn图层
        public static bool AddCADLayer(ref IBasicMap pBasicMap, string vPath, List<IFeatureLayer> pSelectedCln, bool blnAddData)
        {
            bool functionReturnValue = false;

            //WHFErrorHandle.clsErrorHandle pdisplayinfo = new WHFErrorHandle.clsErrorHandle();
            IWorkspaceFactory pFact = default(IWorkspaceFactory);
            IWorkspace pWorkSpace = default(IWorkspace);
            IFeatureWorkspace pFeatureWorkspace = default(IFeatureWorkspace);
            IFeatureLayer pFtLyr = default(IFeatureLayer);
            IFeatureClass pFtCls = default(IFeatureClass);
            string pFile = null;
            pFact = new CadWorkspaceFactory();
            pWorkSpace = pFact.OpenFromFile(vPath, 0);
            pFeatureWorkspace = (IFeatureWorkspace)pWorkSpace;
            int i = 0;
            int j = 0;
            IFeatureDataset pFtDataset = default(IFeatureDataset);
            for (j = 1; j <= pSelectedCln.Count; j++)
            {
                pFile = (string)pSelectedCln[j].Name;
                pFtDataset = pFeatureWorkspace.OpenFeatureDataset(pFile);
                IFeatureClassContainer pFtClsContainer = default(IFeatureClassContainer);
                pFtClsContainer = (IFeatureClassContainer)pFtDataset;

                //检查当前目录是否是cad工作空间
                if (pFtClsContainer == null | pFtClsContainer.ClassCount == 0)
                {
                    //pdisplayinfo.DisplayInformation("CAD文件:" + pFile + "不存在数据层", false);
                    MessageBoxEx.Show("CAD文件:" + pFile + "不存在数据层", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    functionReturnValue = false;
                    return functionReturnValue;
                }

                for (i = 0; i <= pFtClsContainer.ClassCount - 1; i++)
                {
                    pFtCls = pFtClsContainer.get_Class(i);
                    if (pFtCls.FeatureType == esriFeatureType.esriFTSimple)
                    {
                        pFtLyr = new CadFeatureLayerClass();
                        pFtLyr.FeatureClass = pFtCls;
                        pFtLyr.Name = pFile + ":" + pFtCls.AliasName;

                        pSelectedCln.Add(pFtLyr);
                        functionReturnValue = true;
                    }
                    else if (pFtCls.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        pFtLyr = new CadAnnotationLayerClass();
                        pFtLyr.FeatureClass = pFtCls;

                        pFtLyr.Name = pFile + " " + pFtCls.AliasName;

                        pSelectedCln.Add(pFtLyr);

                        functionReturnValue = true;
                    }
                }
            }
            return functionReturnValue;
        }


        public static bool AddDataset(ref IBasicMap pBasicMap, IDatasetName pDatasetName, List<IDataset> m_DatasetCol, bool blnAddData)
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

            //WHFErrorHandle.clsErrorHandle pfrmError = new WHFErrorHandle.clsErrorHandle();

            if (pDatasetName is IFeatureDatasetName)
            {
                pName = (IName)pDatasetName;
                pFeatDS = (IFeatureDataset)pName.Open();

                pEnumDataSet = pFeatDS.Subsets;
                pDataset = pEnumDataSet.Next();

                m_DatasetCol.Add(pDataset);
                if (pDataset == null)
                    return functionReturnValue;
                ////根据数据集的类型,添加特征数据集中的所有要素类(拓扑,一般的,栅格目录,网络)
                while ((pDataset != null))
                {

                    if (pDataset.Type == esriDatasetType.esriDTFeatureClass)
                    {
                        pFeatLayer = null;
                        if (pFeatLayer == null)
                        {
                            pFeatLayer = new FeatureLayer();
                            pFeatCls = (IFeatureClass)pDataset;
                            pFeatLayer.Name = pFeatCls.AliasName;
                            pFeatLayer.FeatureClass = pFeatCls;
                        }

                        if (pDataset.Type == esriDatasetType.esriDTRasterCatalog)
                        {
                            // Dim pRaster
                        }

                        // pSelectedCln.Add(pFeatLayer)

                    }
                    else if (pDataset.Type == esriDatasetType.esriDTTopology)
                    {
                        pTopoLayer = new TopologyLayerClass();
                        pTopoLayer.Topology = (ITopology)pDataset;
                        pLayer = (ILayer)pTopoLayer;
                        pLayer.Name = pDataset.Name;

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
                pName = (IName)pDatasetName;
                pDataset = (IDataset)pName.Open();
                pTopoLayer = new TopologyLayerClass();
                pTopoLayer.Topology = (ITopology)pDataset;
                pLayer = (ILayer)pTopoLayer;
                pLayer.Name = pDataset.Name;
                m_DatasetCol.Add(pDataset);
                if (blnAddData == true)
                {
                    //pMap.AddLayer pLayer
                    AddLyrToBasicMap(ref pBasicMap, pLayer);
                    //SortLayer(pBasicMap, pLayer)
                }
                //pSelectedCln.Add(pLayer)

                //if (pfrmError.DisplayInformation("要把拓扑里边的所有要素类也添加到当前地图中吗?") == true)
                if (MessageBoxEx.Show("要把拓扑里边的所有要素类也添加到当前地图中吗?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    IFeatureClassContainer pFeatClsContainer = default(IFeatureClassContainer);
                    pFeatClsContainer = (IFeatureClassContainer)pTopo;
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
                            AddLyrToBasicMap(ref pBasicMap, pFeatLayer);
                            //SortLayer(pBasicMap, pFeatLayer)
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
                int i = 0;
                int j = 0;
                IDataset pGeoDataset = default(IDataset);

                pName = (IName)pDatasetName;
                pGeoDataset = (IDataset)pName.Open();
                m_DatasetCol.Add(pGeoDataset);
                if (pGeoDataset.Type == esriDatasetType.esriDTGeometricNetwork)
                {
                    ////这里对网络数据进行处理  
                    IFeatureClassContainer pFeatureClassContainer = default(IFeatureClassContainer);
                    pGeometricNetwork = (IGeometricNetwork)pGeoDataset;
                    pFeatureClassContainer = (IFeatureClassContainer)pGeometricNetwork;

                    for (i = 0; i <= pFeatureClassContainer.ClassCount - 1; i++)
                    {
                        pFeatCls = pFeatureClassContainer.get_Class(i);
                        pFeatLayer = new FeatureLayer();
                        pFeatLayer.Name = pFeatCls.AliasName;
                        pFeatLayer.FeatureClass = pFeatCls;

                        if (blnAddData == true)
                        {
                            // pMap.AddLayer pFeatLayer
                            AddLyrToBasicMap(ref pBasicMap, pFeatLayer);
                            //SortLayer(pBasicMap, pFeatLayer)
                        }
                        //pSelectedCln.Add(pFeatLayer)
                    }
                }
                else
                {
                    pFeatDS = (IFeatureDataset)pGeoDataset;
                    pNetworkCollection = (INetworkCollection)pFeatDS;
                    ////如果是用户选择一个网络技术打开的话,肯定只有一个网络在里边,其实
                    ////可以不需要循环,而用GeometricNetwork(0)代替循环
                    for (j = 0; j <= pNetworkCollection.GeometricNetworkCount - 1; j++)
                    {
                        pGeometricNetwork = pNetworkCollection.get_GeometricNetwork(j);
                        for (i = 0; i <= 3; i++)
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
                                    AddLyrToBasicMap(ref pBasicMap, pFeatLayer);
                                    //SortLayer(pBasicMap, pFeatLayer)
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
                pName = (IName)pDatasetName;
                pDataset = (IDataset)pName.Open();
                m_DatasetCol.Add(pDataset);
                pFeatLayer = new GdbRasterCatalogLayerClass();

                pFeatLayer.FeatureClass = (IFeatureClass)pDataset;
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
                pRasterCatalogPro = (IRasterCatalogDisplayProps)pFeatLayer;
                ////不用数量来控制了,而以比例尺来控制
                pRasterCatalogPro.DisplayRasters = 16;
                pRasterCatalogPro.UseScale = true;
                ////设置一个比例,在此临界栅格数据将会在框架显示与实际栅格显示之间转换
                pRasterCatalogPro.TransitionScale = 50000;

                if (blnAddData == true)
                {
                    //pMap.AddLayer pFeatLayer
                    AddLyrToBasicMap(ref pBasicMap, pFeatLayer);
                    //'SortLayer(pBasicMap, pFeatLayer)
                }
                //pSelectedCln.Add(pFeatLayer)
                functionReturnValue = true;
                // 陈昉  2009-3-22 添加单一的RasterDataset
            }
            else if (pDatasetName is IRasterDatasetName)
            {
                IRasterLayer pRasterLayer = default(IRasterLayer);
                pName = (IName)pDatasetName;
                pDataset = (IDataset)pName.Open();
                m_DatasetCol.Add(pDataset);
                pRasterLayer = new RasterLayerClass();
                pRasterLayer.CreateFromDataset(pDataset as IRasterDataset);
                pRasterLayer.Name = pDataset.Name;
                AddLyrToBasicMap(ref pBasicMap, pRasterLayer);
                functionReturnValue = true;

                ////添加TIN图层
            }
            else if (pDatasetName is ITinWorkspace)
            {
                pTinWS = (ITinWorkspace)pDatasetName;
                ITinLayer pTinLyr = default(ITinLayer);
                pTinLyr = new TinLayer();
                pTinLyr.Dataset = pTinWS.OpenTin(pDatasetName.Name);
                pTinLyr.Name = pDatasetName.Name;

                if (blnAddData == true)
                {
                    //pMap.AddLayer pTinLyr
                    AddLyrToBasicMap(ref pBasicMap, pTinLyr);
                    //SortLayer(pBasicMap, pTinLyr)
                }
                //pSelectedCln.Add(pTinLyr)
                functionReturnValue = true;

                ////添加一般的要素类,未写完。。。。。。
            }
            else
            {
                pName = (IName)pDatasetName;
                pDataset = (IDataset)pName.Open();
                pFeatCls = (IFeatureClass)pDataset;
                m_DatasetCol.Add(pDataset);
                if (pFeatCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    pFeatLayer = new FDOGraphicsLayerClass();
                }
                else if (pFeatCls.FeatureType == esriFeatureType.esriFTDimension)
                {
                    pFeatLayer = new DimensionLayerClass();

                }
                else
                {
                    pFeatLayer = new FeatureLayer();
                }
                //印骅 20081205 添加"Not"
                if ((pFeatLayer != null))
                {
                    //pFeatLayer.Name = pDataset.Name
                    pFeatLayer.Name = pFeatCls.AliasName;
                    pFeatLayer.FeatureClass = (IFeatureClass)pDataset;
                }
                if (blnAddData == true)
                {
                    //pMap.AddLayer pFeatLayer

                    AddLyrToBasicMap(ref pBasicMap, pFeatLayer);
                    //SortLayer(pBasicMap, pFeatLayer)
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

        #region "qitadaima"
        //    ' //添加Coverage图层

        //    Public Function ADDCoverageLayer(ByVal pBasicMap As IBasicMap, ByVal vPath As String, ByVal vFile As String, _
        //    ByVal pSelectedCln As Collection, ByVal blnAddData As Boolean) As Boolean
        //        '<EhHeader>
        //        On Error GoTo ErrorHandler
        //        '</EhHeader>

        //        Dim pFact As IWorkspaceFactory
        //        Dim pWorkSpace As IWorkspace
        //        Dim pFeatureWorkspace As IFeatureWorkspace
        //        Dim pFtLyr As IFeatureLayer
        //        Dim pFeatCls As IFeatureClass

        //        pFact = New ArcInfoWorkspaceFactory
        //        pWorkSpace = pFact.OpenFromFile(vPath, 0)
        //        pFeatureWorkspace = pWorkSpace

        //        '//添加Coverage个点线面要素层
        //        On Error Resume Next
        //        pFeatCls = Nothing
        //        pFeatCls = pFeatureWorkspace.OpenFeatureClass(vFile & ":polygon")
        //        If Not pFeatCls Is Nothing Then
        //            pFtLyr = New FeatureLayer
        //            pFtLyr.FeatureClass = pFeatCls
        //            pFtLyr.Name = vFile & ":polygon"

        //            If blnAddData = True Then
        //                'pBasicMap.AddLayer pFtLyr
        //                AddLyrToBasicMap(pBasicMap, pFtLyr)
        //                SortLayer(pBasicMap, pFtLyr)
        //            End If
        //            pSelectedCln.Add(pFtLyr)

        //            ADDCoverageLayer = True
        //        End If

        //        pFeatCls = Nothing
        //        pFeatCls = pFeatureWorkspace.OpenFeatureClass(vFile & ":arc")
        //        If Not pFeatCls Is Nothing Then
        //            pFtLyr = New FeatureLayer
        //            pFtLyr.FeatureClass = pFeatCls
        //            pFtLyr.Name = vFile & ":arc"

        //            If blnAddData = True Then
        //                'pBasicMap.AddLayer pFtLyr
        //                AddLyrToBasicMap(pBasicMap, pFtLyr)
        //                SortLayer(pBasicMap, pFtLyr)
        //            End If
        //            pSelectedCln.Add(pFtLyr)

        //            ADDCoverageLayer = True
        //        End If

        //        pFeatCls = Nothing
        //        pFeatCls = pFeatureWorkspace.OpenFeatureClass(vFile & ":point")
        //        If Not pFeatCls Is Nothing Then
        //            pFtLyr = New FeatureLayer
        //            pFtLyr.FeatureClass = pFeatCls
        //            pFtLyr.Name = vFile & ":point"

        //            If blnAddData = True Then
        //                'pBasicMap.AddLayer pFtLyr
        //                AddLyrToBasicMap(pBasicMap, pFtLyr)
        //                SortLayer(pBasicMap, pFtLyr)
        //            End If
        //            pSelectedCln.Add(pFtLyr)

        //            ADDCoverageLayer = True
        //        End If

        //        '待修改，添加Coverage注记层
        //        '    Set pFeatCls = Nothing
        //        '    Set pFeatCls = pFeatureWorkspace.OpenFeatureClass(vFile & ":annotation")
        //        '    If Not pFeatCls Is Nothing Then
        //        '        Dim pCovAnnoLyr As ICoverageAnnotationLayer
        //        '        Set pCovAnnoLyr = New CoverageAnnotationLayer
        //        '        Set pCovAnnoLyr.FeatureClass = pFeatCls
        //        '        pCovAnnoLyr.GenerateGraphics
        //        '        pFtLyr.Name = vFile & ":annotation"
        //        '        pBasicMap.AddLayer pFtLyr
        //        '        ADDCoverageLayer = True
        //        '    End If

        //        '<EhFooter>
        //        Exit Function

        //ErrorHandler:
        //        HandleError(True, "错误文件名：" & c_sModuleFileName & "；错误过程名：ADDCoverageLayer" & "；错误行数： " & GetErrorLineNumberString(Erl), Err.Number, Err.Source, Err.Description, g_Sys)
        //        '</EhFooter>
        //    End Function



        ////显示SDE或mdb图层列表,可以显示在树列表TreeView也可以显示在ListView中

        public static void AddDatasetToListView(IWorkspace pWorkSpace, ref ListView vListView)
        {
            if ((vListView != null))
                vListView.Items.Clear();

            IEnumDatasetName pEnumFeatureDSN = default(IEnumDatasetName);
            //要素层名
            IEnumDatasetName pEnumDSN = default(IEnumDatasetName);
            //要素集名
            IEnumDatasetName pEnumRasterDSN = default(IEnumDatasetName);
            //栅格数据集名
            IDatasetName pDSN = default(IDatasetName);
            IFeatureClassName pFtClsName = default(IFeatureClassName);

            ListViewItem vListItem = default(ListViewItem);
            int shpType = 0;
            //添加SDE数据集
            pEnumDSN = pWorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);
            pDSN = pEnumDSN.Next();

            while ((pDSN != null))
            {
                vListItem = vListView.Items.Add(pDSN.Name, "SDEDataset");
                vListItem.Tag = pDSN;

                pDSN = pEnumDSN.Next();
            }
            //添加SDE独立要素层
            pEnumFeatureDSN = pWorkSpace.get_DatasetNames(esriDatasetType.esriDTFeatureClass);
            pDSN = null;
            pDSN = pEnumFeatureDSN.Next();
            while ((pDSN != null))
            {
                if (pDSN.Type == esriDatasetType.esriDTFeatureClass)
                {
                    pFtClsName = (IFeatureClassName)pDSN;
                    //显示点线面要素层

                    if (pFtClsName.FeatureType == esriFeatureType.esriFTSimple | pFtClsName.FeatureType == esriFeatureType.esriFTSimpleJunction | pFtClsName.FeatureType == esriFeatureType.esriFTSimpleEdge | pFtClsName.FeatureType == esriFeatureType.esriFTComplexEdge | pFtClsName.FeatureType == esriFeatureType.esriFTComplexJunction)
                    {
                        shpType = (int)pFtClsName.ShapeType;
                        if (shpType == (int)esriGeometryType.esriGeometryPoint | shpType == (int)esriGeometryType.esriGeometryMultipoint)
                        {
                            vListItem = vListView.Items.Add(pDSN.Name, "SDEPointLyr");
                            vListItem.Tag = pFtClsName;
                        }
                        else if (shpType == (int)esriGeometryType.esriGeometryRing | shpType == (int)esriGeometryType.esriGeometryPolyline | shpType == (int)esriGeometryType.esriGeometryCircularArc | shpType == (int)esriGeometryType.esriGeometryEllipticArc)
                        {
                            vListItem = vListView.Items.Add(pDSN.Name, "SDELineLyr");
                            vListItem.Tag = pFtClsName;
                        }
                        else if (shpType == (int)esriGeometryType.esriGeometryPolygon | shpType == (int)esriGeometryType.esriGeometryEnvelope)
                        {
                            vListItem = vListView.Items.Add(pDSN.Name, "SDEPolyLyr");
                            vListItem.Tag = pFtClsName;
                        }

                        //显示注记层

                    }
                    else if (pFtClsName.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        vListItem = vListView.Items.Add(pDSN.Name, "SDEAnnoLyr");
                        vListItem.Tag = pFtClsName;

                        //添加尺寸图层

                    }
                    else if (pFtClsName.FeatureType == esriFeatureType.esriFTDimension)
                    {
                        vListItem = vListView.Items.Add(pDSN.Name, "SDEDimensionLyr");
                        vListItem.Tag = pFtClsName;

                    }
                }
                pDSN = pEnumFeatureDSN.Next();
            }
            //添加SDE栅格RasterDataset图层
            pEnumRasterDSN = pWorkSpace.get_DatasetNames(esriDatasetType.esriDTRasterDataset);
            pDSN = null;
            pDSN = pEnumRasterDSN.Next();

            while ((pDSN != null))
            {
                vListItem = vListView.Items.Add(pDSN.Name, "SDERasterLyr");
                vListItem.Tag = pDSN;
                pDSN = pEnumRasterDSN.Next();
                // 陈昉  2009-3-22  修改 修改原因 原先代码有误（'pDSN = pEnumFeatureDSN.Next ）
            }
            //添加SDE栅格RasterCatalog图层
            pEnumRasterDSN = pWorkSpace.get_DatasetNames(esriDatasetType.esriDTRasterCatalog);
            pDSN = null;
            pDSN = pEnumRasterDSN.Next();
            while ((pDSN != null))
            {
                //添加到listView
                vListItem = vListView.Items.Add(pDSN.Name, "SDERasterBand");
                vListItem.Tag = pDSN;

                pDSN = pEnumRasterDSN.Next();
            }

        }

        // // 将vListView中数据集显示在vListView

        public static void ListDatasetLayer(ListView vListView)
        {
            if (vListView.SelectedItems[0] == null)
                return;
            ListViewItem vListItem = default(ListViewItem);
            vListItem = vListView.SelectedItems[0];

            IDatasetName pDSN = default(IDatasetName);
            pDSN = (IDatasetName)vListItem.Tag;
            if (pDSN.Type != esriDatasetType.esriDTFeatureDataset)
                return;

            IEnumDatasetName pEnumFeatureDSN = default(IEnumDatasetName);
            //要素层名
            IDatasetName pSubDSN = default(IDatasetName);
            IFeatureClassName pFtClsName = default(IFeatureClassName);
            int shpType = 0;

            //显示数据集中的要素层
            pEnumFeatureDSN = pDSN.SubsetNames;
            vListView.Items.Clear();
            pSubDSN = pEnumFeatureDSN.Next();
            while ((pSubDSN != null))
            {
                if (pSubDSN.Type == esriDatasetType.esriDTFeatureClass)
                {
                    pFtClsName = (IFeatureClassName)pSubDSN;
                    //显示点线面要素层
                    if (pFtClsName.FeatureType == esriFeatureType.esriFTSimple)
                    {
                        shpType = (int)pFtClsName.ShapeType;

                        if (shpType == (int)esriGeometryType.esriGeometryPoint | shpType == (int)esriGeometryType.esriGeometryMultipoint)
                        {
                            vListItem = vListView.Items.Add(pSubDSN.Name, "SDEPointLyr");
                            vListItem.Tag = pFtClsName;
                        }
                        else if (shpType == (int)esriGeometryType.esriGeometryRing | shpType == (int)esriGeometryType.esriGeometryPolyline | shpType == (int)esriGeometryType.esriGeometryCircularArc | shpType == (int)esriGeometryType.esriGeometryEllipticArc)
                        {
                            vListItem = vListView.Items.Add(pSubDSN.Name, "SDELineLyr");
                            vListItem.Tag = pFtClsName;
                        }
                        else if (shpType == (int)esriGeometryType.esriGeometryPolygon | shpType == (int)esriGeometryType.esriGeometryEnvelope)
                        {
                            vListItem = vListView.Items.Add(pSubDSN.Name, "SDEPolyLyr");
                            vListItem.Tag = pFtClsName;
                        }
                        //显示注记层
                    }
                    else if (pFtClsName.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        vListItem = vListView.Items.Add(pSubDSN.Name, "SDEAnnoLyr");
                        vListItem.Tag = pFtClsName;
                        //显示尺寸注记图层
                    }
                    else if (pFtClsName.FeatureType == esriFeatureType.esriFTDimension)
                    {
                        vListItem = vListView.Items.Add(pDSN.Name, "SDEDimensionLyr");
                        vListItem.Tag = pFtClsName;
                        //显示经过network处理的图层
                    }
                    else if (pFtClsName.FeatureType == esriFeatureType.esriFTSimpleJunction | pFtClsName.FeatureType == esriFeatureType.esriFTSimpleEdge | pFtClsName.FeatureType == esriFeatureType.esriFTComplexEdge | pFtClsName.FeatureType == esriFeatureType.esriFTComplexJunction)
                    {
                        shpType = (int)pFtClsName.ShapeType;
                        if (shpType == (int)esriGeometryType.esriGeometryPoint | shpType == (int)esriGeometryType.esriGeometryMultipoint)
                        {
                            vListItem = vListView.Items.Add(pSubDSN.Name, "SDEPointLyr");
                            vListItem.Tag = pFtClsName;
                        }
                        else if (shpType == (int)esriGeometryType.esriGeometryRing | shpType == (int)esriGeometryType.esriGeometryPolyline | shpType == (int)esriGeometryType.esriGeometryCircularArc | shpType == (int)esriGeometryType.esriGeometryEllipticArc)
                        {
                            vListItem = vListView.Items.Add(pSubDSN.Name, "SDELineLyr");
                            vListItem.Tag = pFtClsName;
                        }
                        else if (shpType == (int)esriGeometryType.esriGeometryPolygon | shpType == (int)esriGeometryType.esriGeometryEnvelope)
                        {
                            vListItem = vListView.Items.Add(pSubDSN.Name, "SDEPolyLyr");
                            vListItem.Tag = pFtClsName;
                        }

                    }

                }
                else if (pSubDSN.Type == esriDatasetType.esriDTGeometricNetwork)
                {
                    vListItem = vListView.Items.Add(pSubDSN.Name, "NET");
                    vListItem.Tag = pSubDSN;
                    // pFtClsName
                }

                pSubDSN = pEnumFeatureDSN.Next();
            }

        }

        #endregion

        // // 该函数判断用户是否选择了唯一一个FDB或者PDB
        public static bool BlnSelectedPDBorFDB(ListView vListView)
        {
            bool functionReturnValue = false;
            functionReturnValue = false;
            if (vListView.SelectedItems.Count == 0)
                return functionReturnValue;
            string vFilename = null;
            ////如果只选择了一个
            if (vListView.SelectedItems.Count == 1)
            {
                vFilename = vListView.SelectedItems[0].Name;
                ////并且选择的文件名以mdb或者gdb结尾，则说明需要打开该数据

                if (vFilename.Substring(vFilename.Length - 4, vFilename.Length) == ".MDB" |  vFilename.Substring(vFilename.Length-4,vFilename.Length) == ".GDB")
                {
                    functionReturnValue = true;
                }
            }
            return functionReturnValue;

        }

        //用于往MAP中添加图层 比较空间参考是否一致  

        public static void AddLyrToBasicMap(ref IBasicMap pMap, ILayer pLayer)
        {
            //WHFErrorHandle.clsErrorHandle pdisplayinfo = new WHFErrorHandle.clsErrorHandle();
            IGeoDataset pGeoDataset = default(IGeoDataset);
            try
            {
                pMap.AddLayer(pLayer);
                //先把图层加进去再说
                //判断空间参考是否一致

                if (pLayer is IGeoFeatureLayer)
                {
                    pGeoDataset = (IGeoDataset)pLayer;
                    if (((pGeoDataset.SpatialReference != null)) & ((pMap.SpatialReference != null)))
                    {
                        //这里只是通过名称进行下比较  如果说想要比较的严格的话 可以通过克隆的方法进行
                        if ((blnEditMapSpatial == true) & (pGeoDataset.SpatialReference.Name != pMap.SpatialReference.Name))
                        {
                            //if (pdisplayinfo.DisplayInformation("当前加载图层的空间参考与地图空间参考不一致，" + "是否更改地图空间参考？", true, " 是 ", " 否 ") == true)
                            if (MessageBoxEx.Show("当前加载图层的空间参考与地图空间参考不一致，" + "是否更改地图空间参考？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                            {
                                pMap.SpatialReference = pGeoDataset.SpatialReference;
                            }
                            else
                            {
                                //不进行比较了
                                blnEditMapSpatial = false;
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }

        }
    }
}
