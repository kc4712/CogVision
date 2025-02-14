﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Forms;

namespace VisionCog
{
    public delegate void LogEventHandler(string _strItem, string _strMessage);
    public class Log
    {
        
        public static event LogEventHandler LogEvent;

        private static bool bSaveEnable = true;

        private static string GetLogFileName()
        {
            string sDir;

            sDir = Application.StartupPath + @"\Log";
            Directory.CreateDirectory(sDir);
            sDir = sDir + @"\" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            return sDir;
        }

        public static void SetSaveOption(bool _bSaveEnable)
        {
            bSaveEnable = _bSaveEnable;
        }

        public static bool LogStr(string _strItem, string _strMessage)
        {
            if (LogEvent != null) LogEvent(_strItem, _strMessage);
            if (bSaveEnable != true) return true;
            try
            {
                FileStream LogStream = new FileStream(GetLogFileName(), FileMode.Append);
                if (!LogStream.CanWrite)
                {
                    LogStream.Close();
                    return false;
                }

                StreamWriter LogWriter = new StreamWriter(LogStream);
                LogWriter.Write(DateTime.Now.ToString("[HH:mm:ss.fff] ") + _strItem + "\t" + _strMessage + "\r\n");
                LogWriter.Flush();
                LogWriter.Close();
                LogStream.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
