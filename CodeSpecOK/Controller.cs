using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DetectModule;

namespace CodeSpecOK
{
    static class Controller
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainScreen());
        }

        static public void startDetectPhase(String srcFolder, String libFolder, String time){
            Detect d = new Detect();
            DetectConsole con = new DetectConsole(d);
            con.Show();
            d.DetectErrors(srcFolder, libFolder, time);
        }


    }
}
