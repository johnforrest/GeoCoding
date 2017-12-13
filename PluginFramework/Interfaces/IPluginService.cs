using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework
{
   public  interface IPluginService
    {
       IApplication Application { get; set; }
       void LoadAllPlugin();
    }
}
