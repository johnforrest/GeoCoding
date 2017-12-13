using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using DevComponents.DotNetBar;

namespace PluginFramework
{
  public class Application:IApplication
    {
      IMapControl4 mapControlMain;
      public IMapControl4 MapControMain
      {
          get
          {
              return mapControlMain;
          }
          set 
          {
              mapControlMain = value;
          }
      }
      RibbonControl ribbonControlMain;
      public RibbonControl RibbonControlMain
      {
          get
          {
              return ribbonControlMain;
          }
          set 
          {
              ribbonControlMain = value;
          }
      }

    

    }
}
