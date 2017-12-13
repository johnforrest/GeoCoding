using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ZJGISOpenData.Classes
{
    /// <summary>
    /// 
    /// </summary>
    enum PathType
    {
        /// <summary>
        /// Windows用户桌面路径
        /// </summary>
        DeskTopPath,  
        /// <summary>
        /// Windows用户字体目录路径
        /// </summary>
        FontsPath,
        /// <summary>
        /// Windows用户网络邻居路径
        /// </summary>
        NethoodPath,
        /// <summary>
        ///  Windows用户我的文档路径
        /// </summary>
        PersonalPath,
        /// <summary>
        /// Windows用户开始菜单程序路径
        /// </summary>
        ProgramsPath,
        /// <summary>
        /// Windows用户存放用户最近访问文档快捷方式的目录路径
        /// </summary>
        RecentPath,
        /// <summary>
        ///  Windows用户发送到目录路径
        /// </summary>
        SendtoPath,
        /// <summary>
        ///  Windows用户开始菜单目录路径
        /// </summary>
        StartmenuPath,  
        /// <summary>
        ///  Windows用户开始菜单启动项目录路径
        /// </summary>
        StartupPath,   
        /// <summary>
        ///  Windows用户收藏夹目录路径
        /// </summary>
        FavoritesPath,   
        /// <summary>
        /// Windows用户网页历史目录路径
        /// </summary>
        HistoryPath,   
        /// <summary>
        /// Windows用户Cookies目录路径
        /// </summary>
        CookiesPath,  
        /// <summary>
        /// Windows用户Cache目录路径
        /// </summary>
        CachePath,
        /// <summary>
        ///  Windows用户应用程式数据目录路径
        /// </summary>
        AppdataPath, 
        /// <summary>
        /// Windows用户打印目录路径
        /// </summary>
        PrinthoodPath 
    }
    class ClsGetSysPath
    {
        public static string GetPath(PathType pathType){
            RegistryKey folders;
            folders = OpenRegistryPath(Registry.CurrentUser, @"/software/microsoft/windows/currentversion/explorer/shell folders");
            switch (pathType)
            {
                case PathType.DeskTopPath:
                    return folders.GetValue("Desktop").ToString();
                case PathType.FontsPath:
                    return folders.GetValue("Fonts").ToString();
                case PathType.NethoodPath:
                    return folders.GetValue("Nethood").ToString();
                case PathType.PersonalPath:
                    return folders.GetValue("Personal").ToString();
                case PathType.ProgramsPath:
                    return folders.GetValue("Programs").ToString();
                case PathType.RecentPath:
                    return folders.GetValue("Recent").ToString();
                case PathType.SendtoPath:
                    return folders.GetValue("Sendto").ToString();
                case PathType.StartmenuPath:
                    return folders.GetValue("Startmenu").ToString();
                case PathType.StartupPath:
                    return folders.GetValue("Startup").ToString();
                case PathType.FavoritesPath:
                    return folders.GetValue("Favorites").ToString();
                case PathType.HistoryPath:
                    return folders.GetValue("History").ToString();
                case PathType.CookiesPath:
                    return folders.GetValue("Cookies").ToString();
                case PathType.CachePath:
                    return folders.GetValue("Cache").ToString();
                case PathType.AppdataPath:
                    return folders.GetValue("Appdata").ToString();
                case PathType.PrinthoodPath:
                    return folders.GetValue("Printhood").ToString();
                default:
                    return string.Empty;
            }
        }
        private static RegistryKey OpenRegistryPath(RegistryKey root, string s)
        {
            s = s.Remove(0, 1) + @"/";

            while (s.IndexOf(@"/") != -1)
            {
                root = root.OpenSubKey(s.Substring(0, s.IndexOf(@"/")));
                s = s.Remove(0, s.IndexOf(@"/") + 1);
            }

            return root;
        }
    }
}
