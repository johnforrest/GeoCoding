using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Collections;
using System.Windows.Forms;

namespace ZJGISLayerManager
{
    /// <summary>
    /// Summary description for ClsAsortVertex.
    /// </summary>
    [Guid("2363d7d9-6257-4cdd-8fd2-f5a5a59f24a1")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ZJGISLayerManager.Classes.ClsAsortVertex")]
    public sealed class ClsAsortVertex : BaseCommand
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            ControlsCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper;
        IMapControl4 m_MapControl=null;

        public ClsAsortVertex()
        {
            //
            
            //
            base.m_category = ""; //localizable text
            base.m_caption = "";  //localizable text
            base.m_message = "";  //localizable text 
            base.m_toolTip = "";  //localizable text 
            base.m_name = "";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;

            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;

            //  Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ClsAsortVertex.OnClick implementation
            IToolbarControl2 pToolbarControl;
            if (m_hookHelper.Hook is IToolbarControl)
            {

                pToolbarControl = (m_hookHelper.Hook) as IToolbarControl2;
                m_MapControl = (pToolbarControl.Buddy) as IMapControl4;
            }
            else if (m_hookHelper.Hook is IMapControl4)
                m_MapControl = (IMapControl4)(m_hookHelper.Hook);

            IMap pMap = m_MapControl.ActiveView.FocusMap as IMap;
            ILayer2 pLayer = null;
            //m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint();
            
            for (int i = 0; i < pMap.LayerCount; i++)
            {
                pLayer = pMap.get_Layer(i) as ILayer2;
                if (pLayer is IFeatureLayer2)
                {
                    IFeatureLayer2 pFeatLyr = pLayer as IFeatureLayer2;
                    IFeatureSelection pFeatSel = pFeatLyr as IFeatureSelection;

                    ISelectionSet2 pSelSet = pFeatSel.SelectionSet as ISelectionSet2;
                    if (pSelSet.Count <= 0)
                    {
                        return;
                    }
                    else
                    {
                        ICursor pCursor = null;
                        pSelSet.Search(null, false, out pCursor);
                        IFeatureCursor pFeatCursor = pCursor as IFeatureCursor;

                        IFeature pFeat = pFeatCursor.NextFeature();
                        while (pFeat != null)
                        {
                            IGeometry5 pGeometry = pFeat.Shape as IGeometry5;
                            IPointCollection pPointCol = new Multipoint();
                            pPointCol = pGeometry as IPointCollection;

                            double[] arrayX = new double[200];
                            double[] arrayY = new double[200];

                            IPointArray pPtArray = new PointArrayClass();
                            pPtArray = AsortVertex(pPointCol, arrayX, arrayY);

                            
                            for (int k = 0; k < pPointCol.PointCount; k++)
                            {
                                IPoint pPoint = new PointClass();                         
                                pPoint = pPointCol.get_Point(k);

                                MessageBox.Show(pPointCol.get_Point(k).ID.ToString());
                                for (int l = 0; l < pPtArray.Count; l++)
                                {
                                    if (pPoint.X == pPtArray.get_Element(l).X)
                                    {
                                        int ID=pPtArray.get_Element(l).ID;
                                        pPointCol.get_Point(k).ID = ID;
                                        MessageBox.Show(pPointCol.get_Point(k).ID.ToString());
                                        break;
                                    }
                                }
                            }
                            pFeat = pFeatCursor.NextFeature();

                        }


                    }
                }
                
            }
            m_MapControl.ActiveView.Refresh();
        }  
        private IPointArray AsortVertex(IPointCollection pPointCol, double[] arrayX, double[] arrayY)
        {
            int i;
            int j;
            double tempx;
            double tempy;
            IPointArray pPtArray = new PointArrayClass();

            for (int m = 0; m < pPointCol.PointCount; m++)
            { 
                IPoint pPt=new PointClass();
                pPt=pPointCol.get_Point(m) as IPoint;
                arrayX[m] = pPt.X;
                arrayY[m] = pPt.Y;
            }

                for (i = 0; i < pPointCol.PointCount; i++)
                {
                    for (j = i+1; j < pPointCol.PointCount; j++)
                    {
                        if (arrayX[i] > arrayX[j])
                        {
                            tempx = arrayX[i];
                            arrayX[i] = arrayX[j];
                            arrayX[j] = tempx;

                            tempy = arrayY[i];
                            arrayY[i] = arrayY[j];
                            arrayY[j] = tempy;
                        }
                        else if (arrayX[i] == arrayX[j])
                        {
                            if (arrayY[i] > arrayY[j])
                            {
                                tempx = arrayX[i];
                                arrayX[i] = arrayX[j];
                                arrayX[j] = tempx;

                                tempy = arrayY[i];
                                arrayY[i] = arrayY[j];
                                arrayY[j] = tempy;
                            }
                        }
                    }
                }
                for (int n = 0; n < pPointCol.PointCount; n++)
                {
                    IPoint pPoint = new PointClass();
                    pPoint.X = arrayX[n];
                    pPoint.Y = arrayY[n];
                    pPoint.ID = n;

                    pPtArray.Add(pPoint);
                }
                return pPtArray;
        }


        //private IPointCollection AsortVertex(IPointCollection pPointCol, double[] arrayX, double[] arrayY)
        //{
        //    int i;
        //    int j;
        //    double tempx;
        //    double tempy;
        //    IPointCollection pOutPointCol =new Multipoint();

        //    for (int m = 0; m < pPointCol.PointCount; m++)
        //    {
        //        IPoint pPt = new PointClass();
        //        pPt = pPointCol.get_Point(m) as IPoint;
        //        arrayX[m] = pPt.X;
        //        arrayY[m] = pPt.Y;
        //    }

        //    for (i = 0; i < pPointCol.PointCount; i++)
        //    {
        //        for (j = i + 1; j < pPointCol.PointCount; j++)
        //        {
        //            if (arrayX[i] > arrayX[j])
        //            {
        //                tempx = arrayX[i];
        //                arrayX[i] = arrayX[j];
        //                arrayX[j] = tempx;

        //                tempy = arrayY[i];
        //                arrayY[i] = arrayY[j];
        //                arrayY[j] = tempy;
        //            }
        //            else if (arrayX[i] == arrayX[j])
        //            {
        //                if (arrayY[i] > arrayY[j])
        //                {
        //                    tempx = arrayX[i];
        //                    arrayX[i] = arrayX[j];
        //                    arrayX[j] = tempx;

        //                    tempy = arrayY[i];
        //                    arrayY[i] = arrayY[j];
        //                    arrayY[j] = tempy;
        //                }
        //            }
        //        }
        //    }
        //    for (int n = 0; n < pPointCol.PointCount; n++)
        //    {
        //        IPoint pPoint = new PointClass();
        //        pPoint.X = arrayX[n];
        //        pPoint.Y = arrayY[n];
        //        pPoint.ID = n;

        //        pOutPointCol.AddPoint(pPoint,null,null);
        //    }
        //    return pOutPointCol;
        //}
        #endregion
    }
}
