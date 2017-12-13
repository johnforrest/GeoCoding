using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using DevComponents.DotNetBar;

namespace PluginFramework
{
  public  interface IApplication
    {
      IMapControl4 MapControMain { get; set; }
      RibbonControl RibbonControlMain { get; set; }
    }
}
