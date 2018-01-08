/*-------------------------------------------------------------------------
 * 版权所有：武汉大学
 * 创建人：johnforrest
 * 联系方式：571716604@qq.com
 * 创建时间： 1/8/2018 9:37:05 AM
 * 类名称：LicenseInitializer
 * 本类主要用途描述：
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;

namespace ZJGIS.Classes
{
    class LicenseInitializer
    {
        private IAoInitialize m_AoInitialize = new AoInitializeClass();

        /// <summary>
        /// 初始化许可
        /// </summary>
        /// <returns></returns>
        public bool CheckLicenses()
        {
            if (m_AoInitialize == null)
            {
                MessageBox.Show("不能初始化", "ArcGIS Engine许可错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            esriLicenseStatus licenseStatus = (esriLicenseStatus)m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);

            if (licenseStatus == esriLicenseStatus.esriLicenseAvailable)
            {
                licenseStatus = (esriLicenseStatus)m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
                if (licenseStatus != esriLicenseStatus.esriLicenseAlreadyInitialized)
                {
                    if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut)
                    {
                        MessageBox.Show("初始化失败，应用程序不能运行!", "ArcGIS Engine许可错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("程序运行期间重复初始化！");
                }
            }
            else
            {
                MessageBox.Show("初始化失败，应用程序不能运行!", "ArcGIS Engine许可错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

    }
}
