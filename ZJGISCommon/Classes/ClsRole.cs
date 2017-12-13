using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISCommon
{
    public class ClsRole
    {
        private long m_RoleID;
        private string m_RoleName;
        private string m_RoleDesc;
        private string m_Belong;



        public long RoleID
        {
            get
            {
                return m_RoleID;
            }
            set
            {
                m_RoleID = value;
            }
        }


        public string RoleName
        {
            get
            {
                return m_RoleName;
            }
            set
            {
                m_RoleName = value;
            }
        }


        public string RoleDesc
        {
            get
            {
                return m_RoleDesc;
            }
            set
            {
                m_RoleDesc = value;
            }
        }


        public string Belong
        {
            get
            {
                return m_Belong;
            }
            set
            {
                m_Belong = value;
            }
        }

    }
}
