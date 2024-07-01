using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Common
{
    public static class LogFileException
    {

        #region"Logs code"

        public static object Write_Log_Exception(string mappath, dynamic strMsg)
        {
            string strPath = mappath + "\\" + DateTime.Now.ToString("MMddyyyy");
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            string path2 = strPath + "\\" + "submittedData" + DateTime.Now.ToString("yyyyMMddhhmmssmmm");
            StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
            swLog.WriteLine(DateTime.Now.ToString("ddMMyyHHmmssttt") + ":" + strMsg);
            swLog.Close();
            swLog.Dispose();
            return "";



        }
        public static object Write_Log(string mappath, dynamic strMsg)
        {
            string strPath = mappath + "\\" + DateTime.Now.ToString("MMddyyyy");
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            string path2 = strPath + "\\" + "submittedData" + DateTime.Now.ToString("yyyyMMddhhmmssmmm");
            StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
            swLog.WriteLine(DateTime.Now.ToString("ddMMyyHHmmssttt") + ":" + strMsg);
            swLog.Close();
            swLog.Dispose();
            return "";



        }

        public static object Write_Log_Timer(string mappath, dynamic strMsg)
        {
            string strPath = mappath + "\\" + DateTime.Now.ToString("MMddyyyy");
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            string path2 = strPath + "\\" + "Timer" + DateTime.Now.ToString("yyyyMMddhhmmssmmm");
            StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
            swLog.WriteLine(DateTime.Now.ToString("ddMMyyHHmmssttt") + ":" + strMsg);
            swLog.Close();
            swLog.Dispose();
            return "";



        }
        #endregion

    }
}
