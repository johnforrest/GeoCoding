using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ZJGISDataMatch.Classes;

namespace ZJGISDataMatch.Forms
{
    public partial class FrmOneKeyCoding : DevComponents.DotNetBar.Office2007Form
    {

        IMapControl2 pMapControl;
        Dictionary<string, string> pdic = new Dictionary<string, string>();
        Dictionary<string, ILayer> pdicLayer = new Dictionary<string, ILayer>();
        Dictionary<string, IFeatureClass> pdicFeatureClass = new Dictionary<string, IFeatureClass>();
        //key是待匹配图层的fid，value是源图层的fid
        Dictionary<int, int> pdicOid = new Dictionary<int, int>();
        Dictionary<int, string> pdiOri = new Dictionary<int, string>();


        public FrmOneKeyCoding()
        {
            InitializeComponent();
        }
        public FrmOneKeyCoding(IMapControl2 pmapcontrol)
        {
            InitializeComponent();
            this.pMapControl = pmapcontrol;
        }
        private void FrmOneKeyCoding_Load(object sender, EventArgs e)
        {
            InitialComboxExDistance(comboBoxExDistance);
            InitialComboxEx(comboBoxExOriLayer);
            InitialComboxEx(comboBoxExMatLayer);
            //默认选中第一项
            comboBoxExDistance.SelectedItem = comboBoxExDistance.Items[0];
        }

        private void InitialComboxExDistance(ComboBoxEx pComboBoxEx)
        {
            pComboBoxEx.Items.Add("Meters");
            pComboBoxEx.Items.Add("Kilometers");
            pComboBoxEx.Items.Add("Decimal degrees");

        }
        private void InitialComboxEx(ComboBoxEx cobox)
        {
            IMap pMap = pMapControl.Map;
            IEnumLayer pEnumLayer = pMap.get_Layers(null, true);
            pEnumLayer.Reset();
            ILayer pLayer = pEnumLayer.Next();
            while (pLayer != null)
            {
                cobox.Items.Add(pLayer.Name);
                //获取layer的完整路径
                IDataLayer2 pDatalayer = pLayer as IDataLayer2;
                IDatasetName pDatasetName = pDatalayer.DataSourceName as IDatasetName;
                IWorkspaceName pWPName = pDatasetName.WorkspaceName;

                if (!pdic.ContainsKey(pLayer.Name))
                {
                    pdic.Add(pLayer.Name, pWPName.PathName + "\\" + pLayer.Name + ".shp");
                }
                if (!pdicLayer.ContainsKey(pLayer.Name))
                {
                    pdicLayer.Add(pLayer.Name, pLayer);
                }
                pLayer = pEnumLayer.Next();

            }

        }

        private void buttonXOneKeyCoding_Click(object sender, EventArgs e)
        {

            bool isContainNullCoding = false;
            string temPath = @"D:\temp";
            string nameWithExtentionBuffer = null;
            string nameWithExtentionIndentity = null;

            //判断输出路径是否合法
            try
            {
                if (!System.IO.Directory.Exists(temPath))
                {
                    Directory.CreateDirectory(temPath);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("路径无效!");

            }
            try
            {
                isContainNullCoding = ClsCheck.CheckNullField(pdicLayer[this.comboBoxExOriLayer.SelectedItem.ToString()], "ENTIID");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("没有选择源图层，请选择源图层");
            }
            //首先对源图层A的编码进行空值检查
            if (!isContainNullCoding)
            {
                //对源图层进行缓冲区分析生成缓冲区图层B
                ClsBuffer pBuffer = new ClsBuffer();
                nameWithExtentionBuffer = pBuffer.ReturnshpPath(pdicLayer[this.comboBoxExOriLayer.SelectedItem.ToString()].Name);
                temPath = temPath + "\\" + nameWithExtentionBuffer;
                pBuffer.ReturnBufferFeatureClass(pdicLayer[this.comboBoxExOriLayer.Text] as IFeatureLayer, temPath, double.Parse(this.textBoxX1.Text.ToString()), this.comboBoxExDistance.SelectedItem.ToString());
                if (!pdic.ContainsKey(nameWithExtentionBuffer.Substring(0, nameWithExtentionBuffer.LastIndexOf("."))))
                {
                    pdic.Add(nameWithExtentionBuffer.Substring(0, nameWithExtentionBuffer.LastIndexOf(".")), temPath);
                }
                if (!pdicFeatureClass.ContainsKey(nameWithExtentionBuffer.Substring(0, nameWithExtentionBuffer.LastIndexOf("."))))
                {
                    pdicFeatureClass.Add(nameWithExtentionBuffer.Substring(0, nameWithExtentionBuffer.LastIndexOf(".")), ClsCommon.ReadFeatureClass(temPath));
                }

                //让缓冲区图层B和待匹配编码图层C作标识叠加分析得到图层D
                ClsIndentity pIndentity = new ClsIndentity();
                nameWithExtentionIndentity = pIndentity.ReturnshpPath(pdicLayer[this.comboBoxExMatLayer.SelectedItem.ToString()].Name);
                temPath = @"D:\temp";
                temPath = temPath + "\\" + nameWithExtentionIndentity;

                pIndentity.ReturnIndentityFeatureClass((pdicLayer[this.comboBoxExMatLayer.Text] as IFeatureLayer).FeatureClass, pdicFeatureClass[nameWithExtentionBuffer.Substring(0, nameWithExtentionBuffer.LastIndexOf("."))], temPath);
                pdic.Add(nameWithExtentionIndentity.Substring(0, nameWithExtentionIndentity.LastIndexOf(".")), temPath);
                pdicFeatureClass.Add(nameWithExtentionIndentity.Substring(0, nameWithExtentionIndentity.LastIndexOf(".")), ClsCommon.ReadFeatureClass(temPath));
                //遍历D图层字段，找到图层D和图层C的对应关系，也就是图层B和图层A的对应关系，从而找到A和B几何匹配成功的要素
                GetMatchedOID(pdicFeatureClass[nameWithExtentionIndentity.Substring(0, nameWithExtentionIndentity.LastIndexOf("."))], "ORIG_FID", "FID");
                //不进行属性检查就编码
                ClsCoding pCode = new ClsCoding();
                GetMatchedOIDENTIID(pdicLayer[this.comboBoxExOriLayer.Text] as IFeatureLayer, pdicOid, "ENTIID");
                pCode.PointCoding(pdicLayer[this.comboBoxExMatLayer.Text] as IFeatureLayer, pdicOid, pdiOri, "ENTIID");
                //对匹配成功的要素进行属性匹配，只要有一条属性匹配成功，那么就认为是同一个地理实体，把图层A的地理编码赋值给同名要素

                //遍历B图层，对没有匹配成功的字段进行编码
                pCode.PointCodingRest(pdicLayer[this.comboBoxExMatLayer.Text] as IFeatureLayer, pdiOri, "ENTIID");
            }
            else
            {
                MessageBox.Show("源图层编码含有空值，请重新编码后，再进行匹配操作！");
            }
            MessageBox.Show("点实体编码成功！");

        }

        private void buttonXClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 获取源图层和待匹配图层之间的fid对应关系
        /// </summary>
        /// <param name="pFeatureClass">Indentify分析之后的图层</param>
        /// <param name="pFieldOri">源图层FID</param>
        /// <param name="pFieldMat">待匹配图层的fid</param>
        public void GetMatchedOID(IFeatureClass pFeatureClass, string pFieldOri, string pFieldMat)
        {
            //遍历Feature
            IDataset pDataset = pFeatureClass as IDataset;
            IWorkspaceEdit pWorkspaceEdit = null;
            if (pDataset != null)
            {
                pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                {
                    pWorkspaceEdit.StartEditing(true);
                    pWorkspaceEdit.StartEditOperation();
                }
                IFeatureCursor pFeatureCursor = pFeatureClass.Search(new QueryFilterClass(), false);
                //test
                int test = pFeatureClass.FeatureCount(null);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {

                    int pIndex2 = pFeature.Fields.FindField(pFieldMat);
                    int pIndex = pFeature.Fields.FindField(pFieldOri);

                    ////test
                    //string test1 = pFeature.get_Value(pIndex2).ToString();
                    //string test2 = pFeature.get_Value(pIndex).ToString();
                    if (pFeature.get_Value(pIndex).ToString().Trim() != "0")
                    {
                        pdicOid.Add((int)pFeature.get_Value(pIndex2), (int)pFeature.get_Value(pIndex));
                    }
                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();

            }

        }
        /// <summary>
        ///  获取源图层的fid和ENTIID的对应关系
        /// </summary>
        /// <param name="pOriLayer"></param>
        /// <param name="pdicOid"></param>
        /// <param name="pEntiid"></param>
        public void GetMatchedOIDENTIID(IFeatureLayer pOriLayer, Dictionary<int, int> pdicOid, string pEntiid)
        {
            //Dictionary<int, string> pdiOri = new Dictionary<int, string>();
            //Dictionary<int, string> pdiMat = new Dictionary<int, string>();
            //遍历Feature
            IDataset pDataset = pOriLayer.FeatureClass as IDataset;
            IWorkspaceEdit pWorkspaceEdit = null;
            if (pDataset != null)
            {
                pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                {
                    pWorkspaceEdit.StartEditing(true);
                    pWorkspaceEdit.StartEditOperation();
                }
                IFeatureCursor pFeatureCursor = pOriLayer.FeatureClass.Search(new QueryFilterClass(), false);
                //test
                int test = pOriLayer.FeatureClass.FeatureCount(null);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    foreach (int value in pdicOid.Values)
                    {
                        if (pFeature.OID == value)
                        {
                            int pIndex = pFeature.Fields.FindField(pEntiid);
                            pdiOri.Add(value, pFeature.get_Value(pIndex).ToString());
                        }

                    }
                    pFeature = pFeatureCursor.NextFeature();
                }
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();
            }
        }


    }
}
