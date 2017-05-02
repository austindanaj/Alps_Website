using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Date = System.DateTime;

namespace CTBTeam
{
    public class Log
    {

        private static Log instance;

        private Log() { }

        public static Log getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Log();
                }
                return instance;
            }
        }
        public void WriteToLog(string functionName, Exception exception, HttpServerUtility Server)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"" + Server.MapPath("~/Debug/StackTrace.txt")))
            {
                file.WriteLine(Date.Today.ToString() + "--" + functionName + "--" + exception.ToString());
                file.WriteLine();
                file.Close();
            }
        }
    }
}