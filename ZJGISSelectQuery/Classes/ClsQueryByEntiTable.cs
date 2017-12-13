using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Collections;
using System.Resources;
using System.Reflection;
using Microsoft.VisualBasic;
using System.Windows.Forms;
namespace SelectQuery 
{
    /// <summary>
    /// Command that works in ArcMap/Map/PageLayout, ArcScene/SceneControl
    /// or ArcGlobe/GlobeControl
    /// </summary>
    [Guid("0a3488a9-f18b-4a6f-95db-9cbaf6c8b9c9")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("SelectQuery.clsQueryByAttribute")]
    public sealed class ClsQueryByEntiTable : BaseCommand
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
            GMxCommands.Register(regKey);
            MxCommands.Register(regKey);
            SxCommands.Register(regKey);
            ControlsCommands.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            GMxCommands.Unregister(regKey);
            MxCommands.Unregister(regKey);
            SxCommands.Unregister(regKey);
            ControlsCommands.Unregister(regKey);
        }

        #endregion
        #endregion

        private IHookHelper m_hookHelper = null;
        //private Form FrmMain;
        //public Form FrmActive
        //{
        //    get
        //    {
        //        return FrmMain;
        //    }
        //    set
        //    {
        //        FrmMain = value;
        //    }
        //}

        //FrmQueryByAttribute m_frmQueryByAttribute;
        FrmQueryByEntiTable m_frmQueryByEntiTable;
        //Collection m_FeatClsCln; 

        public ClsQueryByEntiTable()
        {
            //
            
            //
            //base.m_category = "SelectQuery"; //localizable text
            //base.m_caption = "属性查询";  //localizable text
            //base.m_message = "属性查询";   //localizable text 
            //base.m_toolTip = "属性查询";  //localizable text 
            //base.m_name = "A10408E";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")
            base.m_category = "SelectQuery"; //localizable text
            base.m_caption = "实体表查询";  //localizable text
            base.m_message = "实体表查询";   //localizable text 
            base.m_toolTip = "实体表查询";  //localizable text 
            base.m_name = "A10408E";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                
                //
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), "Resources.AttriQuery1.bmp");
                //ResourceManager resource = new ResourceManager("SelectQuery.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("AttriQuery");
                //ResourceManager rm = new ResourceManager("ZJGISSelectQuery.Properties.Resources", Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)rm.GetObject("AttriQuery1");

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
            if (m_hookHelper==null)
            {
                m_hookHelper = new HookHelperClass();
            }
            if (hook!=null)
            {
                m_hookHelper.Hook = hook;
            }
            ClsDeclare.g_pMap = m_hookHelper.FocusMap;
            //ClsDeclare.g_Sys.FrmMain = FrmMain;
            
            //ClsDeclare.g_Sys.FrmMain = (Form)pFrmMain.FrmActive;
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {

                
            try
            {
                if (m_frmQueryByEntiTable == null)
                    m_frmQueryByEntiTable = new FrmQueryByEntiTable();
                else
                {
                    if (m_frmQueryByEntiTable.IsDisposed)
                        m_frmQueryByEntiTable = new FrmQueryByEntiTable();
                }
                m_frmQueryByEntiTable.Show();
            }
            catch (Exception)
            {
            }

        }

        #endregion
    }
}
