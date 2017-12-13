using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;//添加System引用即可
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using DevComponents.DotNetBar;
namespace ZJGISFinishTool
{
    class ClsCreatAndGetDataset
    {
       
        //** 功能描述: 通过一个字段集,创建另外一个字段集,直接添加传入的字段集中的所有字段的话
        //             会产生高版本和低版本不兼容的问题,

        public static IFields GetFieldsByFields(IFields pFields, ref ISpatialReference pDesSpatialReference, Dictionary<string, string> pDicField)
        {
            //pDesSpatialReference = null;
            //pDicField = null;

            int i = 0;
            IField pField = default(IField);
            IFieldEdit pFieldEdit = default(IFieldEdit);
            IFieldsEdit pFieldsEdit = default(IFieldsEdit);
            IField pCreateField = default(IField);
            ISpatialReference pOriSpatialReference = default(ISpatialReference);

            IGeometryDef pGeometryDef = default(IGeometryDef);
            IGeometryDefEdit pGeometryDefEdit = default(IGeometryDefEdit);
            double ymin = 0;
            double xmin = 0;
            double xmax = 0;
            double ymax = 0;
            double mMin = 0;
            double zmin = 0;
            double zmax = 0;
            double mMax = 0;
            IEnvelope pEnvelop = default(IEnvelope);
            IGeometry pGeometry = default(IGeometry);
            IClone pClone = default(IClone);
            //标识该字段是否被添加
            bool bIsAddField = false;
            //应该把OID字段添加进去,否则会产生错误
            pFieldsEdit = new FieldsClass();

            for (i = 0; i <= pFields.FieldCount - 1; i++)
            {
                pField = pFields.get_Field(i);

                if (pField.Editable | pField.Type == esriFieldType.esriFieldTypeOID | pField.Type == esriFieldType.esriFieldTypeGlobalID | pField.Type == esriFieldType.esriFieldTypeGUID)
                {
                    

                    pClone = (IClone)pField;
                    pCreateField = (IField)pClone.Clone();

                    //如果更改字段名称:如果是OID字段的话,即便不在dic中也要进行添加
                    if ((pDicField != null))
                    {
                        if (pDicField.ContainsKey(Strings.Trim(pField.Name)))
                        {
                            bIsAddField = true;
                            //修改字段的名称
                            pFieldEdit = (IFieldEdit)pCreateField;
                            pFieldEdit.Name_2 = pDicField[Strings.Trim(pField.Name)];

                            //需不需要把别名赋过去,根据需要再看吧,现在看的话应该不要赋过去,因此一般字段名和别名是一样的
                            //如果赋过去的话,显示的是原字段的别名,即不是用户想看到的字段名
                            //pFieldEdit.AliasName = pField.AliasName
                        }
                        else if (pField.Type == esriFieldType.esriFieldTypeOID | pField.Type == esriFieldType.esriFieldTypeGlobalID | pField.Type == esriFieldType.esriFieldTypeGUID)
                        {
                            bIsAddField = true;
                        }
                        else
                        {
                            bIsAddField = false;
                        }
                    }
                    else
                    {
                        bIsAddField = true;
                    }
                    if (pField.Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pGeometryDef = pCreateField.GeometryDef;
                        pGeometryDefEdit = (IGeometryDefEdit)pGeometryDef;



                        if ((pDesSpatialReference != null))
                        {
                            pOriSpatialReference = pGeometryDef.SpatialReference;

                            //从原来的空间参考中得到domain
                            if (pOriSpatialReference.HasXYPrecision())
                            {
                                pOriSpatialReference.GetDomain(out xmin, out xmax, out ymin, out ymax);
                            }

                            pEnvelop = new EnvelopeClass();
                            pEnvelop.PutCoords(xmin, ymin, xmax, ymax);
                            pGeometry = pEnvelop;
                            pGeometry.SpatialReference = pOriSpatialReference;
                            pGeometry.Project(pDesSpatialReference);
                            xmax = pEnvelop.XMax;
                            xmin = pEnvelop.XMin;
                            ymax = pEnvelop.YMax;
                            ymin = pEnvelop.YMin;

                            pDesSpatialReference.SetDomain(xmin, xmax, ymin, ymax);

                            if (pOriSpatialReference.HasZPrecision())
                            {
                                pOriSpatialReference.GetZDomain(out zmin, out zmax);
                                pDesSpatialReference.SetZDomain(zmin, zmax);
                            }

                            if (pOriSpatialReference.HasMPrecision())
                            {
                                pOriSpatialReference.GetMDomain(out mMin, out mMax);
                                pDesSpatialReference.SetMDomain(mMin, mMax);
                            }

                            pGeometryDefEdit.SpatialReference_2 = pDesSpatialReference;

                        }
                        //修改grid的大小
                        if (pGeometryDef.GridCount != 0)
                        {
                            if (pGeometryDef.get_GridSize(0) < 100)
                            {
                                pGeometryDefEdit.set_GridSize(0, 100);
                            }
                        }
                        bIsAddField = true;
                    }

                    if (bIsAddField == true)
                    {
                        pFieldsEdit.AddField(pCreateField);
                        bIsAddField = false;
                    }
                }
            }
            return pFieldsEdit;
        }

       
        //** 功能描述: 得到shp文件的工作空间
       
        public static IWorkspace CreatOrGetSHPWorkspace(string sFilePath)
        { 
            IWorkspace functionReturnValue = default(IWorkspace);
            try
            {
               
                IWorkspaceFactory pWorkspaceFactory = default(IWorkspaceFactory);
                IWorkspace pWorkspace = default(IWorkspace);


                if (Microsoft.VisualBasic.FileIO.FileSystem.DirectoryExists(sFilePath) == false)
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(sFilePath);
                }
                pWorkspaceFactory = new ShapefileWorkspaceFactory();
                pWorkspace = pWorkspaceFactory.OpenFromFile(sFilePath, 0);
                functionReturnValue = pWorkspace;
                return functionReturnValue;
            }
            catch
            {
                functionReturnValue = null;
                return functionReturnValue;
            }
        }
        //<CSCM>
        //********************************************************************************
        //** 函 数 名: GetPDBOrGDBWorkspace
        //** 版    权: CopyRight (C)  
        //** 创 建 人: 杨旭斌
        //** 功能描述: 根据用户提供的PDB的全路径,获得对应的MDB的工作空间
        //** 创建日期:
        //** 修 改 人:
        //** 修改日期:
        //** 修改时间: 20070818
        //** 参数列表: sPDBOrFDBRouteAndName (String)pdb或FDB的全路径,即包括文件名称
        //**           bCreateFDB标识是否是创建FDB
        //** 版    本:1.0
        //*********************************************************************************
        //</CSCM>
        public static IWorkspace GetPDBOrGDBWorkspace(string sPDBOrFDBRouteAndName, bool bCreateFDB)
        {
            IWorkspace functionReturnValue = default(IWorkspace);
            try
            {
                
                string sFilePath = null;
                string sFileName = null;
                IWorkspaceFactory pWorkspaceFactory;
                IWorkspace pWorkspace = default(IWorkspace);




                if (string.IsNullOrEmpty(Strings.Trim(sPDBOrFDBRouteAndName)))
                {
                    functionReturnValue = null;
                    return functionReturnValue;
                }

                if (bCreateFDB)
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                }
                else
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                }

                //获得文件名
                sFileName = Microsoft.VisualBasic.FileIO.FileSystem.GetName(sPDBOrFDBRouteAndName);
                //20110723  HSh 注释因为文件名称未加后缀名".gdb"而不能创建的问题
                sFileName = sFileName.Substring(0, sFileName.Length - 4);

                //获得路径
                sFilePath = Microsoft.VisualBasic.FileIO.FileSystem.GetParentPath(sPDBOrFDBRouteAndName);
                //如果路径不存在，则创建
                if (Microsoft.VisualBasic.FileIO.FileSystem.DirectoryExists(sFilePath) == false)
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(sFilePath);
                }
                //判断MDB文件是否存在,如果不存在的话,则创建
                if (bCreateFDB == false)
                {
                    if (Microsoft.VisualBasic.FileIO.FileSystem.FileExists(sPDBOrFDBRouteAndName) == false)
                    {
                        pWorkspaceFactory.Create(sFilePath, sFileName, null, 0);
                    }
                }
                else
                {
                    if (Microsoft.VisualBasic.FileIO.FileSystem.DirectoryExists(sPDBOrFDBRouteAndName) == false)
                    {
                        pWorkspaceFactory.Create(sFilePath, sFileName, null, 0);
                    }
                }
                pWorkspace = pWorkspaceFactory.OpenFromFile(sPDBOrFDBRouteAndName, 0);

                ClsPublicFunction.DeleteAllFeatCls(pWorkspace);

                functionReturnValue = pWorkspace;
                return functionReturnValue;
            }
            catch
            {
           
                functionReturnValue = null;
                return functionReturnValue;
            }
        }

       
        
        //** 功能描述:根据传入的工作区或特征数据集创建要素类,如果特征数据集不为空的话,则使用特征数据集
        //** 创建要素类,否则使用工作区创建要素类
       
        //** 参数列表: sFeatClsName (String)新要素类的名称
        //         pFeatWorkSpace (IFeatureWorkspace)工作区
        //         pFields (IFields)字段
        //         pFeatDataset (IFeatureDataset)特征数据集
        //         EnumFeatType (esriFeatureType = esriFTSimple)要素类的类型
        //         sShapeFieldName (String = "shape")shape字段的名称
        
        public static IFeatureClass CreateFeatCls(ref string sFeatClsName, ref IFeatureWorkspace pFeatWorkspace, IFields pFields, ref IFeatureDataset pFeatDataset, ref esriFeatureType EnumFeatType, string sShapeFieldName)
        {
            
            IFeatureClass functionReturnValue = default(IFeatureClass);

            try
            {
                IFeatureClass pFeatCls = default(IFeatureClass);

                //如果特征数据集不为空的话,使用特征数据集创建
                if (pFeatDataset != null)
                {
                    pFeatCls = pFeatDataset.CreateFeatureClass(sFeatClsName, pFields, null, null, EnumFeatType, sShapeFieldName, "");
                }
                else
                {
                  
                    pFeatCls = pFeatWorkspace.CreateFeatureClass(sFeatClsName, pFields, null, null, EnumFeatType, sShapeFieldName, "");
                }

                functionReturnValue = pFeatCls;
                return functionReturnValue;
            }
            catch (Exception ex)
            {
                functionReturnValue = null;
                if (((System.Runtime.InteropServices.COMException)ex).ErrorCode == -2147220724)
                {
                    //g_clsErrorHandle.DisplayInformation("不能在ArcGIS9.1或更低版本创建的PDB里边使用ArcGIS9.2或更高版本创建要素类！", false);
                    MessageBoxEx.Show("不能在ArcGIS9.1或更低版本创建的PDB里边使用ArcGIS9.2或更高版本创建要素类！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (((System.Runtime.InteropServices.COMException)ex).ErrorCode == -2147219886)
                {
                    //g_clsErrorHandle.DisplayInformation("要素中字段名称不合法", false);
                    MessageBoxEx.Show("要素中字段名称不合法", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //g_clsErrorHandle.DisplayInformation(ex.Message, false);
                    MessageBoxEx.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            return functionReturnValue;


        }

        
        //** 功能描述: 获得要素类,如果已经存在的话,则直接打开,否则进行创建
       
        //** 参数列表: pFeatWorkspace (IFeatureWorkspace)
        //         sFeatClsName (String)
        //         pFields (IFields) 某个要素类的字段
        //         pSpatialRefer (ISpatialReference)'如果需要改变创建的要素类的空间参考的话,就改变
        //         pDicField    更改字段的名称,key里边放的是原字段名称,item放的是目标字段名称
      
        public static IFeatureClass CreatOrOpenFeatClsByName(ref IFeatureWorkspace pFeatWorkspace, string sFeatClsName,
                        ref IFields pFields, ISpatialReference pSpatialRefer, Dictionary<string, string> pDicField)
        {
            
            IFeatureClass functionReturnValue = default(IFeatureClass);
            IFeatureClass pFeatCls = default(IFeatureClass);
            //Dim pGeoDataset As ESRI.ArcGIS.Geodatabase.IGeoDataset
            IFields pDesFields = default(IFields);
            IWorkspace2 pWorkspace2 = default(IWorkspace2);

            IWorkspace pWorkspace = default(IWorkspace);
            bool bHasExsit = false;
            IDataset pDataset = default(IDataset);
            try
            {
                //首选判断该数据集是否存在
                if (pFeatWorkspace == null)
                {
                    functionReturnValue = null;
                    return functionReturnValue;
                }
                pWorkspace = (IWorkspace)pFeatWorkspace;

                ////印骅 20081113 判断shapefile是否存在
                if (pWorkspace.Type == esriWorkspaceType.esriFileSystemWorkspace)
                {
                    bHasExsit = File.Exists(pWorkspace.PathName + "\\" + sFeatClsName + ".shp") && File.Exists(pWorkspace.PathName + "\\" + sFeatClsName + ".shx") && File.Exists(pWorkspace.PathName + "\\" + sFeatClsName + ".dbf");
                }
                else
                {
                    pWorkspace2 = (IWorkspace2)pFeatWorkspace;
                    bHasExsit = pWorkspace2.get_NameExists(esriDatasetType.esriDTFeatureClass, sFeatClsName);
                    pFeatWorkspace = (IFeatureWorkspace)pWorkspace2;
                }


                if (bHasExsit == false)
                {
                    pDesFields = GetFieldsByFields(pFields, ref pSpatialRefer, pDicField);

                    IFeatureDataset pFeatureDataset = null;
                    esriFeatureType pEnumFeatType = esriFeatureType.esriFTSimple;
                    pFeatCls = CreateFeatCls(ref sFeatClsName, ref pFeatWorkspace, pDesFields, ref pFeatureDataset, ref pEnumFeatType, "shape");
                }
                else
                {
                    //因为在获取工作空间的时候已经删除了空间内可能存在的要素集和要素类，所以下面的语句实际上永远不会被执行 yh
                    //如果要找的要素类存在的话,则打开
                    pFeatCls = pFeatWorkspace.OpenFeatureClass(sFeatClsName);
                    pDataset = (IDataset)pFeatCls;
                    if (pDataset.CanDelete())
                    {
                        pDataset.Delete();
                    }

                    //重新创建要素类
                    pDesFields = GetFieldsByFields(pFields, ref pSpatialRefer, pDicField);
                    IFeatureDataset pFeatureDataset = null;
                    esriFeatureType pEnumFeatType = esriFeatureType.esriFTSimple;
                    pFeatCls = CreateFeatCls(ref sFeatClsName, ref pFeatWorkspace, pDesFields, ref pFeatureDataset, ref pEnumFeatType, "shape");
                }

                functionReturnValue = pFeatCls;
                return functionReturnValue;
            }
            catch (Exception ex)
            {
                
                MessageBoxEx.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                functionReturnValue = null;
                
            }
            return functionReturnValue;
        }

    }
}
