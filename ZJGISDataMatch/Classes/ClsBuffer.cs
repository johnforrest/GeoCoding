using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;

namespace ZJGISDataMatch.Classes
{
    class ClsBuffer
    {
        public string ReturnshpPath(string layername)
        {
            // string returnPath = null;

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.CheckPathExists = true;
            saveDlg.Filter = "Shapefile (*.shp)|*.shp|File Geodatabase (*.gdb)|*.gdb";
            saveDlg.OverwritePrompt = true;
            saveDlg.Title = "Output Layer";
            saveDlg.RestoreDirectory = true;
            saveDlg.FileName = layername + "_Buffer.shp";

            // returnPath = saveDlg.FileName;
            return saveDlg.FileName;
            //DialogResult dr = saveDlg.ShowDialog();
            //if (dr == DialogResult.OK)
            //    textBoxX2.Text = saveDlg.FileName;
        }
        public void ReturnBufferFeatureClass(IFeatureLayer pInputlayer, string pOutputPath, double bufferDistance, string units)
        {
            //判断缓冲区距离
            if (bufferDistance == 0.0)
            {
                MessageBox.Show("缓冲区距离无效!");
                //return null;
            }

            //get an instance of the geoprocessor
            Geoprocessor gp = new Geoprocessor();
            gp.OverwriteOutput = true;
            //create a new instance of a buffer tool
            ESRI.ArcGIS.AnalysisTools.Buffer buffer = new ESRI.ArcGIS.AnalysisTools.Buffer(pInputlayer, pOutputPath, Convert.ToString(bufferDistance) + " " + units);
            //buffer.dissolve_option = "ALL";//这个要设成ALL,否则相交部分不会融合
            buffer.dissolve_option = "NONE";//这个要设成ALL,否则相交部分不会融合
            //buffer.line_side = "FULL";//默认是"FULL",最好不要改否则出错
            //buffer.line_end_type = "ROUND";//默认是"ROUND",最好不要改否则出错
            try
            {
                //results = (IGeoProcessorResult)gp.Execute(buffer, null);
                gp.Execute(buffer, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        


        /// <summary>
        /// 在内存中创建图层
        /// </summary>
        /// <param name="DataSetName">数据集名称</param>
        /// <param name="AliaseName">别名</param>
        /// <param name="SpatialRef">空间参考</param>
        /// <param name="GeometryType">几何类型</param>
        /// <param name="PropertyFields">属性字段集合</param>
        /// <returns>IfeatureLayer</returns>
        public static IFeatureLayer CreateFeatureLayerInmemeory(string DataSetName, string AliaseName, ISpatialReference SpatialRef, esriGeometryType GeometryType, IFields PropertyFields)
        {
            IWorkspaceFactory workspaceFactory = new InMemoryWorkspaceFactoryClass();
            ESRI.ArcGIS.Geodatabase.IWorkspaceName workspaceName = workspaceFactory.Create("", "MyWorkspace", null, 0);
            ESRI.ArcGIS.esriSystem.IName name = (IName)workspaceName;
            ESRI.ArcGIS.Geodatabase.IWorkspace inmemWor = (IWorkspace)name.Open();
            IField oField = new FieldClass();
            IFields oFields = new FieldsClass();
            IFieldsEdit oFieldsEdit = null;
            IFieldEdit oFieldEdit = null;
            IFeatureClass oFeatureClass = null;
            IFeatureLayer oFeatureLayer = null;
            try
            {
                oFieldsEdit = oFields as IFieldsEdit;
                oFieldEdit = oField as IFieldEdit;
                for (int i = 0; i < PropertyFields.FieldCount; i++)
                {
                    oFieldsEdit.AddField(PropertyFields.get_Field(i));
                }
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                geometryDefEdit.AvgNumPoints_2 = 5;
                geometryDefEdit.GeometryType_2 = GeometryType;
                geometryDefEdit.GridCount_2 = 1;
                geometryDefEdit.HasM_2 = false;
                geometryDefEdit.HasZ_2 = false;
                geometryDefEdit.SpatialReference_2 = SpatialRef;
                oFieldEdit.Name_2 = "SHAPE";
                oFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                oFieldEdit.GeometryDef_2 = geometryDef;
                oFieldEdit.IsNullable_2 = true;
                oFieldEdit.Required_2 = true;
                oFieldsEdit.AddField(oField);
                oFeatureClass = (inmemWor as IFeatureWorkspace).CreateFeatureClass(DataSetName, oFields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                (oFeatureClass as IDataset).BrowseName = DataSetName;
                oFeatureLayer = new FeatureLayerClass();
                oFeatureLayer.Name = AliaseName;
                oFeatureLayer.FeatureClass = oFeatureClass;
            }
            catch
            {
            }
            finally
            {
                try
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oField);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFields);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFieldsEdit);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFieldEdit);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(name);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspaceFactory);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workspaceName);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(inmemWor);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFeatureClass);
                }
                catch { }

                GC.Collect();
            }
            return oFeatureLayer;
        }

    }
}
