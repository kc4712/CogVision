using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace VisionCog
{
    class ini
    {
    }
    public class IniControl
    {
        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
    }


    public class CodeINI
    {
        public static String MTree;
        public static int TCount;


        public static void IniFileCreate(string FileName, string title)
        {
            FileStream IniStream = new FileStream(FileName, FileMode.Create);
            if (!IniStream.CanWrite)
            {
                IniStream.Close();
                return;
            }
            StreamWriter writer = new StreamWriter(IniStream);
            writer.Write("[" + title + "]");
            writer.Flush();
            writer.Close();

        }

        public static void WriteIniFilePath(string FileName, string title, string subtitle, string value)
        {

            IniControl.WritePrivateProfileString(title, subtitle, value, FileName);
        }

        public static string ReadIniFilePath(string FileName, string title, string subtitle)
        {
            StringBuilder rINI;
            rINI = new StringBuilder("", 1024);

            IniControl.GetPrivateProfileString(title, subtitle, "", rINI, 1024, FileName);

            return rINI.ToString();
        }
    }
}
