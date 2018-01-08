using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ZJGISCommon.Forms;

namespace ZJGISGCoding.Class
{
    public class ClsTuYuanEnti
    {
        ClsCommon pClsCom = new ClsCommon();
        FrmProgressBar progressbar;
        /// <summary>
        /// 对图元编码
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="cbxLayerName"></param>
        public void PrimitiveCode(IMap pMapControl, ComboBoxEx cbxLayerName)
        {
            IFeatureLayer pFeatureLayer = (IFeatureLayer)pClsCom.GetLayerByName(pMapControl, cbxLayerName.Text);

            string elemid="ELEMID";
            //遍历字段
            if (pFeatureLayer!=null)
            {
                progressbar = new FrmProgressBar(pFeatureLayer.FeatureClass.FeatureCount(null) * 2);
                progressbar.Show();

                IField pField = null;
                IFields pFields = pFeatureLayer.FeatureClass.Fields;

                bool pPrimitiveCode = false;

                for (int i = 0; i < pFields.FieldCount - 1; i++)
                {
                    pField = pFields.get_Field(i);
                    if (pField.Name == elemid)
                    {
                        pPrimitiveCode = true;
                    }
                }

                if (!pPrimitiveCode)
                {
                    MessageBox.Show("不存在图元标识码字段！");
                }

                //对图元进行编码
                CreatePrimitiveCode(pFeatureLayer, elemid, progressbar);
                MessageBox.Show("图元编码成功！"); 
            }
            else
            {
                MessageBox.Show("没有选中任何图层，请选择图层！");
            }
        }

        /// <summary>
        /// 对街道、房屋实体进行编码
        /// </summary>
        /// <param name="pFeatureLayer">需要编码的图层</param>
        /// <param name="pENTIID">需要编码的字段名称</param>
        private void CreatePrimitiveCode(IFeatureLayer pFeatureLayer, string pELEMID,FrmProgressBar pgBar)
        {
            #region 针对GUID编码来讲
            //遍历Feature
            IDataset pDataset = pFeatureLayer.FeatureClass as IDataset;
            IWorkspaceEdit pWorkspaceEdit = null;
            if (pDataset != null)
            {
                pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                if (pWorkspaceEdit != null || pWorkspaceEdit.IsBeingEdited() == false)
                {
                    pWorkspaceEdit.StartEditing(true);
                    pWorkspaceEdit.StartEditOperation();
                }
                IFeatureCursor pFeatureCursor = pFeatureLayer.Search(null, false);
                IFeature pFeature = pFeatureCursor.NextFeature();
                while (pFeature != null)
                {
                    //获取单条Feature的某个字段值
                    int test2 = pFeature.Fields.FindField(pELEMID);
                    //如果要素对应的图元标识码为空，则进行GUID编码
                    if (pFeature.get_Value(pFeature.Fields.FindField(pELEMID)).ToString().Length == 0)
                    {
                        string pGUID = System.Guid.NewGuid().ToString("N");

                        pFeature.set_Value(pFeature.Fields.FindField(pELEMID), pGUID);
                        pFeature.Store();
                    }
                    pgBar.GoOneStep();
                    pFeature = pFeatureCursor.NextFeature();
                }
                pgBar.CloseForm();
                pWorkspaceEdit.StopEditing(true);
                pWorkspaceEdit.StopEditOperation();
            }
            #endregion
        }
    }
}
