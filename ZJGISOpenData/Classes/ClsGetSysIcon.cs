using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;


namespace ZJGISOpenData.Classes
{

    /// <summary>
    /// 保存文件信息的结构体
    /// </summary>
    ///
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }


    class ClsGetSysIcon
    {
        [DllImport("Shell32.dll", EntryPoint = "SHGetFileInfo", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);

        #region API 参数的常量定义

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; //大图标 32×32
        public const uint SHGFI_SMALLICON = 0x1; //小图标 16×16
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;

        #endregion


        public static Icon GetIcon(string fileName, bool isLargeIcon)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            IntPtr hI;

            if (isLargeIcon)
                hI = SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), SHGFI_ICON | SHGFI_USEFILEATTRIBUTES | SHGFI_LARGEICON);
            else
                hI = SHGetFileInfo(fileName, 0, ref shfi, (uint)Marshal.SizeOf(shfi), SHGFI_ICON | SHGFI_USEFILEATTRIBUTES | SHGFI_SMALLICON);

            Icon icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;

            DestroyIcon(shfi.hIcon); //释放资源
            return icon;
        }

        public static Icon GetDirectoryIcon()
        {
            SHFILEINFO _SHFILEINFO = new SHFILEINFO();
            IntPtr _IconIntPtr = SHGetFileInfo(@"", 0, ref _SHFILEINFO, (uint)Marshal.SizeOf(_SHFILEINFO), (uint)(SHGFI_ICON | SHGFI_LARGEICON));
            if (_IconIntPtr.Equals(IntPtr.Zero)) return null;
            Icon _Icon = System.Drawing.Icon.FromHandle(_SHFILEINFO.hIcon);
            return _Icon;
        }

        /// <summary>
        /// 依据文件名读取图标，若指定文件不存在，则返回空值。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Icon GetIconByFileName(string fileName)
        {
            if (fileName == null || fileName.Equals(string.Empty)) return null;
            if (!File.Exists(fileName)) return null;

            SHFILEINFO shinfo = new SHFILEINFO();
            //Use this to get the small Icon
            Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);
            //The icon is returned in the hIcon member of the shinfo struct
            System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
            return myIcon;
        }
    }

    /// <summary>
    /// 定义调用的API方法
    /// </summary>
    class Win32
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        [DllImport("shell32.dll")]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);

    }

}
