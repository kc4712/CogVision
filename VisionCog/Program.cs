using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace VisionCog
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool CreateNew;
            Mutex dup = new Mutex(true, "VisionCog", out CreateNew);

            if (CreateNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain());
                dup.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("The program is already running");
            }
        }
    }
}
