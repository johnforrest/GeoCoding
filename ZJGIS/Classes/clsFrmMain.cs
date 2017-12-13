using System.Collections.Generic;
using System.Drawing;
using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Data;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
namespace ZJGIS
{
	public class clsFrmMain
	{
		
		
		private frmMain m_FrmActive; //传进来要处理的窗体
      
		private IMapControl3 m_MapControl;
		private static string m_UserName;
		////类的相关属性
		////传递本类要处理的窗体，当然这个窗体就是frmMain
        public frmMain FrmActive
		{
            get
            {
                return m_FrmActive;
            }
            set
            {
                m_FrmActive = value;
            }
		}
		
		public IMapControl3 MapControl
		{
			get
			{
				return m_MapControl;
			}
			set
			{
				m_MapControl = value;
			}
		}
		
		public static string UserName
		{
			set
			{
				m_UserName = value;
			}
		}
       
		//构造函数
		public clsFrmMain()
		{
			
		}
		
		~clsFrmMain()
		{
			m_FrmActive = null;
		
		}
		
		public void ShowAbout()
		{
			//frmAbout.TopMost = True
			//frmAbout.Show()
		}
		
		public void ShowHelp()
		{
			SendKeys.Send("{F1}");
		}
		
	}
	
}
