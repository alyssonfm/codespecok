//#define MAKE_TESTS
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DetectModule;
using CategorizeModule;
using Structures;

namespace ContractOK
{
    static class Controller
    {
        private static MainScreen mainSc;
        private static DetectConsole dconSc;
        private static DetectedDisplay ddisSc;
        private static CategorizedDisplay cdisSc;
        private static HashSet<Nonconformance> nonconformances;
        private static String sourceFolder;
        private static String solutionFile;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #if MAKE_TESTS
                MakeTests();
            #endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainSc = new MainScreen();
            Application.Run(mainSc);
        }

        static public void MakeTests(){
            (new CategorizeTest.CategorizationTesting()).TestNonconformancesLikelyCause();
        }

        static public void StartDetectPhase(String srcFolder, String slnFile, String libFolder, String time){
            sourceFolder = srcFolder;
            solutionFile = slnFile;

            Detect d = new Detect();

            dconSc = new DetectConsole(d);
            Application.DoEvents();

            nonconformances = d.DetectErrors(srcFolder, slnFile, libFolder, time);
        }

        static public void StartCategorizationPhase()
        {
            Categorize c = new Categorize();
            nonconformances = c.categorize(nonconformances, sourceFolder, solutionFile);

            if (dconSc.Visible)
                dconSc.Visible = false;
            else if (ddisSc.Visible)
                ddisSc.Visible = false;
            cdisSc = new CategorizedDisplay(nonconformances);
            Application.DoEvents();
        }

        static public void ShowDetectDisplay()
        {
            dconSc.Visible = false;
            ddisSc = new DetectedDisplay(nonconformances);
        }

        static public void MakeMainVisibleAgain()
        {
            mainSc.Visible = true;
        }
    }
}
