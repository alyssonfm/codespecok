//#define MAKE_TESTS
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DetectModule;
using CategorizeModule;
using Structures;
using System.Text.RegularExpressions;

namespace ContractOK
{
    static class Controller
    {
        private static MainScreen mainSc;
        private static DetectConsole dconSc;
        private static DetectedDisplay ddisSc;
        private static AnalyzedDisplay cdisSc;
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
            (new CategorizeTest.StaticAnalysisTesting()).TestNonconformancesLikelyCause();
        }

        static public void StartDetectPhase(String srcFolder, String slnFile, String libFolder, String time){
            sourceFolder = srcFolder;
            solutionFile = slnFile;

            Detect d = new Detect();

            dconSc = new DetectConsole(d);
            Application.DoEvents();

            nonconformances = d.DetectErrors(srcFolder, slnFile, libFolder, time);
        }

        static public bool checkProblemsWithInput(String srcFolder, String slnFile, String time)
        {
            return checkSlnField(slnFile) && checkSrcFolderField(srcFolder) && checkTimeField(time);
        }

        static public bool checkSrcFolderField(String srcFolder)
        {
            return (srcFolder != null && !srcFolder.Equals(""));
        }

        static public bool checkSlnField(String slnFile)
        {
            return (slnFile != null && !slnFile.Equals(""));
        }

        static public bool checkTimeField(String time)
        {
            Regex r = new Regex("\\d+");
            Match m = r.Match(time);
            return m.Success;
        }

        static public void StartCategorizationPhase()
        {
            Categorize c = new Categorize();
            nonconformances = c.categorize(nonconformances, sourceFolder, solutionFile);

            if (dconSc.Visible)
                dconSc.Visible = false;
            else if (ddisSc.Visible)
                ddisSc.Visible = false;
            cdisSc = new AnalyzedDisplay(nonconformances);
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
