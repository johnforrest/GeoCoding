using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Controls;

namespace ZJGISDataExtract
{
  public sealed  class RegionExtract:BaseTool
    {
        private IHookHelper _hookHelper;
        public override void OnCreate(object hook)
        {
            if (hook == null)
                return;
            try
            {
                _hookHelper = new HookHelperClass();
                _hookHelper.Hook = hook;
                if (_hookHelper.ActiveView == null)
                    _hookHelper = null;
            }
            catch
            {
                _hookHelper = null;
            }

            if (_hookHelper == null)
                base.m_enabled = false;
            else
                base.m_enabled = true;

            FrmRegionExtract frmRegionExtract = new FrmRegionExtract(_hookHelper);
            frmRegionExtract.ShowDialog();
        }  
    }
}
