using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTBTeam
{
    public class AccessConnection 
    {

        private static AccessConnection instance;

        private AccessConnection() { }

        public static AccessConnection getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccessConnection();
                }
                return instance;
            }
        }

        //public static string CONNECTION_STRING = "Provider=Microsoft.ACE.Sql.12.0;" +
                          //    "Data Source=" + Server.MapPath("~/CTBWebsiteData.accdb") + ";";
    }
}