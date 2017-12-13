using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginFramework
{
    public interface IPlugin
    {
     //   string Name { get; set; }
        IApplication Application { get; set; }
        void Load();
        void UnLoad();
    }
}
