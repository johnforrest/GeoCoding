using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISCommon
{
    public class ClsUser
    {
        private long m_userID;
        private string m_userName;//用户姓名
        private string m_userPsw; //用户密码
        private string m_userStatus;//用户状态
        private string m_userDepartment;//用户单位



        public long UserID
        {
            get
            {
                return m_userID;
            }
            set
            {
                m_userID = value;
            }
        }

        public string UserName
        {
            get
            {
                return m_userName;
            }
            set
            {
                m_userName = value;
            }
        }

        public string UserPsw
        {
            get
            {
                return m_userPsw;
            }
            set
            {
                m_userPsw = value;
            }
        }

        public string UserStatus
        {
            get
            {
                return m_userStatus;
            }
            set
            {
                m_userStatus = value;
            }
        }

        public string UserDepartment
        {
            get
            {
                return m_userDepartment;
            }
            set
            {
                m_userDepartment = value;
            }
        }

    }
}
