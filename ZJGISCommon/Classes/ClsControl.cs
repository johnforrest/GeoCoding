using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Controls;
using DevComponents.DotNetBar;
namespace ZJGISCommon
{
    public class ClsControl
    {
        //传递主控件
        private static IMapControl4 mapControlMain;

        private static IMapControl4 MapControlFrom;
        private static IMapControl4 MapControlTo;
        private static IMapControl4 MapControlOverlap;

        private static TabControl MainTabControl;
        private static DevComponents.DotNetBar.Controls.DataGridViewX DGVFrom;
        private static DevComponents.DotNetBar.Controls.DataGridViewX DGVTo;
        private static DevComponents.DotNetBar.Bar BottomStandBar;

        public static IMapControl4 MapControlMain
        {
            get { return mapControlMain; }
            set { mapControlMain = value; }
        }
        
        public static IMapControl4 m_MapControlFrom
        {
            get
            {
                return MapControlFrom;
            }
            set
            {
                MapControlFrom = value;
            }
        }
        public static TabControl m_MainTabControl
        {
            get
            {
                return MainTabControl;
            }
            set
            {
                MainTabControl = value;
            }
        }
        public static IMapControl4 m_MapControlTo
        {
            get
            {
                return MapControlTo;
            }
            set
            {
                MapControlTo = value;
            }
        }
        public static IMapControl4 m_MapControlOverlap
        {
            get
            {
                return MapControlOverlap;
            }
            set
            {
                MapControlOverlap = value;
            }
        }
        public static DevComponents.DotNetBar.Controls.DataGridViewX m_DGVFrom 
        {
            get
            {
                return DGVFrom;
            }
            set
            {
                DGVFrom = value;
            }
        }
        public static DevComponents.DotNetBar.Controls.DataGridViewX m_DGVTo
        {
            get
            {
                return DGVTo;
            }
            set
            {
                DGVTo = value;
            }
        }
        public static DevComponents.DotNetBar.Bar m_BottomStandBar
        {
            get
            {
                return BottomStandBar;
            }
            set
            {
                BottomStandBar = value;
            }
        }
      
    }
}
