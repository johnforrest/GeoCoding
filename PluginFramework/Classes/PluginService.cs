using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace PluginFramework
{
    public class PluginService : IPluginService
    {
        private IApplication application;
        public IApplication Application
        {
            get { return application; }
            set { application = value; }
        }
        public PluginService(IApplication application)
        {
            this.application = application;
        }
        /// <summary>
        /// 加载所有插件
        /// </summary>
        public void LoadAllPlugin()
        {
            string path = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\..\\bin\\Plugin";
            //\\..\\Res\\mdb\\全国县级区域矢量化地图\\实验数据.mxd
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                FileInfo[] pluginArray = directoryInfo.GetFiles("*.dll");
                foreach (FileInfo plugin in pluginArray)
                {
                    Assembly assembly = System.Reflection.Assembly.LoadFile(plugin.FullName);
                    Type[] types = null;
                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    foreach (Type type in types)
                    {
                        if (type.GetInterface("IPlugin") != null)
                        {
                            IPlugin instance = (IPlugin)Activator.CreateInstance(type);
                            instance.Application = this.application;
                            instance.Load();
                        }
                    }
                }
            }
            catch { }
        }
    }
}
