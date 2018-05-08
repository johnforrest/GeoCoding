using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.VisualBasic;
using ESRI.ArcGIS.DataSourcesGDB;
using DevComponents.DotNetBar;
namespace ZJGISCommon
{
    class ClsSDE
    {
        /// <summary>
        /// 得到IPropertySet对象
        /// </summary>
        /// <param name="pStrProp"></param>
        /// <returns></returns>
        public static IPropertySet GetPropSetFromArr(string[] pStrProp)
        {
            IPropertySet propertySet = new PropertySetClass();
            propertySet.SetProperty("Server", pStrProp[0]);
            propertySet.SetProperty("Instance", pStrProp[1]);
            propertySet.SetProperty("Database", pStrProp[2]);
            propertySet.SetProperty("user", pStrProp[3]);
            propertySet.SetProperty("password", pStrProp[4]);
            propertySet.SetProperty("version", pStrProp[5]);
            propertySet.SetProperty("AUTHENTICATION_MODE", pStrProp[6]);

            return propertySet;
        }
        /// <summary>
        /// 检查IPropertySet对象
        /// </summary>
        /// <param name="pStrProp"></param>
        /// <returns></returns>
        public static bool CheckTxtComplete(string[] pStrProp)
        {
            for (int i = 0; i < pStrProp.Length; i++)
            {
                if (string.IsNullOrEmpty(pStrProp[i]) && i != 2)
                {
                    MessageBoxEx.Show("设置未完成，请检查设置!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 测试连接状态
        /// </summary>
        /// <param name="pPropertSet"></param>
        /// <returns></returns>
        public static IWorkspace TestSDELinkState(IPropertySet pPropertSet)
        {
            IWorkspaceFactory workspaceFactory = new SdeWorkspaceFactoryClass();
            IWorkspace workspace;

            workspace = workspaceFactory.Open(pPropertSet, 0);
            if (workspace == null)
            {
                MessageBox.Show("连接SDE失败,请检查连接参数!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            else
                return workspace;
        }
        /// <summary>
        /// 保存上次的数据库连接设置
        /// </summary>
        /// <param name="pStrProp"></param>
        public static void SavePropSetting(string[] pStrProp)
        {
            //保存数据库连接设置
            Interaction.SaveSetting(Application.CompanyName, "SDESeting", "Server", pStrProp[0]);
            Interaction.SaveSetting(Application.CompanyName, "SDESeting", "Instance", pStrProp[1]);
            Interaction.SaveSetting(Application.CompanyName, "SDESeting", "Database", pStrProp[2]);
            Interaction.SaveSetting(Application.CompanyName, "SDESeting", "user", pStrProp[3]);
            Interaction.SaveSetting(Application.CompanyName, "SDESeting", "password", pStrProp[4]);
            Interaction.SaveSetting(Application.CompanyName, "SDESeting", "Version", pStrProp[5]);

        }
    }
}
