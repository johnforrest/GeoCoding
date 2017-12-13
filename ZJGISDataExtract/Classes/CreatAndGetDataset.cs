using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;

namespace ZJGISDataExtract
{
    class CreatAndGetDataset
    {
        public static IWorkspace CreatOrGetSHPWorkspace(string strFilePath)
        {
            if (Directory.Exists(strFilePath) == false)
                Directory.CreateDirectory(strFilePath);
            IWorkspaceFactory workspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspace workspace = workspaceFactory.OpenFromFile(strFilePath, 0);
            return workspace;
        }
        public static IWorkspace GetPDBOrGDBWorkspace(string strPath, bool isCreateFDB)
        {
            IWorkspace workspace;
            IWorkspaceFactory workspaceFactory;         
            if (string.IsNullOrEmpty(strPath.Trim()))
                return null;
            if (isCreateFDB)
                workspaceFactory = new FileGDBWorkspaceFactory();
            else
                workspaceFactory = new AccessWorkspaceFactory();
            string strFilePath=Directory.GetDirectoryRoot(strPath);
            string strFileName = Path.GetFileNameWithoutExtension(strPath);
            if (Directory.Exists(strPath) == false)
                Directory.CreateDirectory(strPath);
            //if (isCreateFDB == false)
            //{
            //    if (Directory.Exists(strPath) == false)
            //        workspaceFactory.Create(strFilePath, strFileName, null, 0);
            //}
            //else 
            //{
            //    if (Directory.Exists(strPath) == false)
            //        workspaceFactory.Create(strFilePath, strFileName, null, 0);
            //}
            workspace = workspaceFactory.OpenFromFile(strPath, 0);
            return null;
        }
    }
}
