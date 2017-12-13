using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using System.Resources;
using System.Windows.Forms;

namespace ZJGISFinishTool
{
   
    public sealed class ClsExtractAfterProjection : BaseCommand
    {
       

        private IHookHelper m_hookHelper = null;
      

        public ClsExtractAfterProjection()
        {
          

            base.m_category = "DataExtract"; //localizable text
            base.m_caption = "投影转换";  //localizable text 
            base.m_message = "投影转换";  //localizable text
            base.m_toolTip = "投影转换";  //localizable text
            base.m_name = "A10704E";   //unique id, non-localizable (e.g. "MyCategory_MyCommand")

            try
            {
                //
                
                //
                //ResourceManager resource = new ResourceManager("ZJGISDataExtract.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
                //base.m_bitmap = (Bitmap)resource.GetObject("profile");
                //string bitmapResourceName = GetType().Name + ".bmp";
                //base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
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

            try
            {
                m_hookHelper = new HookHelperClass();
                m_hookHelper.Hook = hook;
                if (m_hookHelper.ActiveView == null)
                    m_hookHelper = null;
            }
            catch
            {
                m_hookHelper = null;
            }

            if (m_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            // Add other initialization code
        }

        /// <summary>
        /// Occurs when this command is clicked
        /// </summary>
        public override void OnClick()
        {
            // TODO: Add ClsExtractAfterProjection.OnClick implementation
            FrmNewProj frmPrj;
            if (m_hookHelper != null)
            {
                frmPrj = new FrmNewProj(m_hookHelper.FocusMap);
                frmPrj.ShowDialog();
            }
        }

        #endregion
    }
}
