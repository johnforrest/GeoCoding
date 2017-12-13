// Copyright 2006 ESRI
//
// All rights reserved under the copyright laws of the United States
// and applicable international laws, treaties, and conventions.
//
// You may freely redistribute and use this sample code, with or
// without modification, provided you include the original copyright
// notice and use restrictions.
//
// See use restrictions at /arcgis/developerkit/userestrictions.

using System;
using System.Drawing;
using System.Text;
//using System.Windows.Forms;
using System.Runtime.InteropServices;
//using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Controls;
using ZJGISDataMatch.Forms;
using ESRI.ArcGIS.ADF.BaseClasses;
//using ESRI.ArcGIS.Carto;
//using ESRI.ArcGIS.Geoprocessor;
//using ESRI.ArcGIS.Geoprocessing;
//using ESRI.ArcGIS.AnalysisTools;

namespace ZJGISDataMatch.Classes
{
  /// <summary>
  /// Summary description for BufferSelectedLayerCmd.
  /// </summary>
  [Guid("7dc0aa20-efe4-4714-9110-7f3c57bf00aa")]
  [ClassInterface(ClassInterfaceType.None)]
  [ProgId("GpBufferLayer.BufferSelectedLayerCmd")]
  public sealed class ClsBufferSelectedLayerCmd : BaseCommand
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
      MxCommands.Register(regKey);

    }
    /// <summary>
    /// Required method for ArcGIS Component Category unregistration -
    /// Do not modify the contents of this method with the code editor.
    /// </summary>
    private static void ArcGISCategoryUnregistration(Type registerType)
    {
      string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
      ControlsCommands.Unregister(regKey);
      MxCommands.Unregister(regKey);

    }

    #endregion
    #endregion

    private IHookHelper m_hookHelper;

    public ClsBufferSelectedLayerCmd()
    {
      base.m_category = "自定义工具集";
      base.m_caption = "缓冲区分析";
      base.m_message = "缓冲区分析";
      base.m_toolTip = "缓冲区分析";
      base.m_name = "GpBufferLayer_BufferSelectedLayerCmd";

      try
      {
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
    }

    /// <summary>
    /// Occurs when this command is clicked
    /// </summary>
    public override void OnClick()
    {
      if (null == m_hookHelper)
        return;

      if (m_hookHelper.FocusMap.LayerCount > 0)
      {
          FrmBuffer bufferDlg = new FrmBuffer(m_hookHelper);
        bufferDlg.Show();
      }
    }

    #endregion
  }
}
