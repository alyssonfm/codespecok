using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Commons;
using Structures;
using NAnt.Core;

namespace DetectModule
{
    public delegate void DetectEvent(String text);

    public class CustomWriter : TextWriter
    {
        private StringWriter _buff;

        public CustomWriter(StringWriter b)
        {
            this._buff = b;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public override void Write(int b)
        {
            this._buff.GetStringBuilder().Append(b);
        }
    }

    public class Detect
    {
        public event DetectEvent DirectoriesCreated;
        public event DetectEvent ProjectCompiled;
        public event DetectEvent TestsGenerated;
        public event DetectEvent TestsExecuted;
        public event DetectEvent ErrorOnDetectPhase;

        public static void ExMethod()
        {

        }

        private String _srcFolder;
        private String _libFolder;
        private String _projName;
        private String _timeout;

        private Stopwatch _watch;

        enum Stages : int { 
            CREATED_DIRECTORIES, 
            COMPILED_PROJECT, 
            GENERATED_TESTS, 
            EXECUTED_TESTS, 
            ERROR_ON_DETECTION
        }

        /// <summary>
        /// This class constructs Detect object and initializes Temporary dir to be used.
        /// </summary>
        public Detect()
        {
            while (!Directory.Exists(Constants.TEMP_DIR)){
                Directory.CreateDirectory(Constants.TEMP_DIR);
            }
            this._watch = new Stopwatch();
        }

        public HashSet<Nonconformance> DetectErrors(String source, String lib, String timeout)
        {
            try
            {
                // Execute scripts division starts here
                Execute(source, lib, timeout);
                // List Errors
                NCCreator ncfinder = new NCCreator();
                return ncfinder.ListNonconformances();
            }
            catch (Exception e)
            {
                TriggersEvent(Stages.ERROR_ON_DETECTION, e.Message);
                return new HashSet<Nonconformance>();
            }
        }

        public void Execute(String src, String lib, String time)
        {
                this._srcFolder = src;
                this._libFolder = lib;
                this._projName = src.Substring(src.LastIndexOf(Constants.FILE_SEPARATOR) + 1).Trim();

                InitTimer();

                this._timeout = time;
                RunStage("Creating directories", "\nDirectories created in", Stages.CREATED_DIRECTORIES);
                RunStage("\nCompiling the project", "Project compiled in", Stages.COMPILED_PROJECT);
                RunStage("Generating tests", "Tests generated in", Stages.GENERATED_TESTS);
                RunStage("Running test into contract-based code", "Tests ran in", Stages.EXECUTED_TESTS);
        }

        private void RunStage(String iniMsg, String finMsg, Stages stagesDetect)
        {
            String text = iniMsg + "...\n";
		    switch (stagesDetect) {
		        case Stages.CREATED_DIRECTORIES:
			        CreateDirectories();
			        CleanDirectories();			
			        break;
		        case Stages.COMPILED_PROJECT:
			        text += CompileProject();
			        break;
		        case Stages.GENERATED_TESTS:
			        text += GenerateTests();
			        break;
		        case Stages.EXECUTED_TESTS:
			        text += RunTests();
			        break;
		        case Stages.ERROR_ON_DETECTION:
			        break;
		        default:
			        break;
		    }   
            text += finMsg + " " + CountTime() + " seconds";
		    TriggersEvent(stagesDetect, text);
	    }
        private void CreateDirectories()
        {
            while (!Directory.Exists(Constants.SOURCE_BIN))
            {
                Directory.CreateDirectory(Constants.SOURCE_BIN);
            }
            while (!Directory.Exists(Constants.TEST_OUTPUT))
            {
                Directory.CreateDirectory(Constants.TEST_OUTPUT);
            }
            while (!Directory.Exists(Constants.TEST_RESULTS))
            {
                Directory.CreateDirectory(Constants.TEST_RESULTS);
            }
            while (!Directory.Exists(Constants.LIB_FOLDER))
            {
                Directory.CreateDirectory(Constants.LIB_FOLDER);
            }
        }
        private void CleanDirectories()
        {
            Array.ForEach(Directory.GetFiles(Constants.SOURCE_BIN), File.Delete);
            Array.ForEach(Directory.GetFiles(Constants.TEST_OUTPUT), File.Delete);
            Array.ForEach(Directory.GetFiles(Constants.TEST_RESULTS), File.Delete);

            SaveResourcesOnTemp();
        }
        private void SaveResourcesOnTemp()
        {
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "build.exe");
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "nant.exe");
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "randoop.exe");
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "vstest.exe");
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "testdll.exe");
        }
        private string CompileProject()
        {
            ProcessStartInfo startInfo = PrepareProcess("compileProject.build");
            String arg = " ";
            arg += "-D:source_folder=\"" + this._srcFolder + "\" ";
            arg += "-D:build_dir=\"" + Constants.SOURCE_BIN + "\" ";
            arg += "-D:project_name=" + this._projName + " ";
            arg += "compile_project";
            startInfo.Arguments += arg;

            return RunProcess(startInfo);
        }
        private string GenerateTests()
        {
            ProcessStartInfo startInfo = PrepareProcess("generateTests.build");
            String arg = " ";
            arg += "-D:build_dir=\"" + Constants.SOURCE_BIN + "\" ";
            arg += "-D:timeout=" + this._timeout + " ";
            arg += "-D:output.dir=\"" + Constants.TEST_OUTPUT + "\" ";
            arg += "-D:randoop_dir=\"" + Constants.RANDOOP_LIB + "\" ";
            arg += "-D:project_name=" + this._projName + " ";
            arg += "generateTests";
            startInfo.Arguments += arg;

            return RunProcess(startInfo);
        }
        private string RunTests()
        {
            ProcessStartInfo startInfo = PrepareProcess("compileTests.build");
            String arg = " ";
            arg += "-D:DirBin_toCopy=\"" + Constants.SOURCE_BIN + "\" ";
            arg += "-D:dir_target=\"" + Constants.TEST_OUTPUT + "\" ";
            arg += "-D:name_project=" + this._projName + " ";
            arg += "-D:path_Vstest=\"" + Constants.VSTEST_EXE + "\" ";
            arg += "run_tests";
            startInfo.Arguments += arg;
            startInfo.WorkingDirectory = Constants.TEMP_DIR;

            string text = RunProcess(startInfo);
            MoveResultFiles();

            return text;
        }

        private void MoveResultFiles()
        {
            var matches = Directory.GetFiles(Constants.TEMP_DIR + Constants.FILE_SEPARATOR + "TestResults").Where(path => Regex.Match(path, @".trx").Success);
            foreach(string file in matches){
                File.Move(file, Constants.TEST_RESULTS + Constants.FILE_SEPARATOR + "TestResult.xml");
            }
            Directory.Delete(Constants.TEMP_DIR + Constants.FILE_SEPARATOR + "TestResults", true);
        }
        
        private ProcessStartInfo PrepareProcess(String ant)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Constants.NANT_EXE);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.Arguments = "-buildfile:\"" + Constants.BUILDS_LIB + Constants.FILE_SEPARATOR + ant + "\"";
            return startInfo;
        }
        private String RunProcess(ProcessStartInfo startInfo)
        {
            Process p = new Process();
            p.StartInfo = startInfo;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            p.WaitForExit();
            return output + error;
        }
        private void RunProject(StringWriter buff, Project p, string mainTarget, DefaultLogger consoleLogger)
        {
            p.BuildListeners.Add(consoleLogger);
            p.Run();
            try{
                p.Execute(mainTarget);
            } catch (Exception e){
			    System.Console.Write(buff.ToString());
			    throw new Exception(e.Message);
		    }
            System.Console.Write(buff.ToString());
        }

        

        private String GetEXEPath()
        {
            return System.Reflection.Assembly.GetEntryAssembly().Location;
        }

        private void InitTimer()
        {
            this._watch.Reset();
            this._watch.Start();
        }

        private double CountTime()
        {
            this._watch.Stop();
            return this._watch.ElapsedMilliseconds / 1000.0;
        }

        private void TriggersEvent(Stages stage, String text)
        {
            switch (stage)
            {
                case Stages.CREATED_DIRECTORIES:
                    DirectoriesCreated(text);
                    break;

                case Stages.COMPILED_PROJECT:
                    ProjectCompiled(text);
                    break;

                case Stages.GENERATED_TESTS:
                    TestsGenerated(text);
                    break;

                case Stages.EXECUTED_TESTS:
                    TestsExecuted(text);
                    break;

                case Stages.ERROR_ON_DETECTION:
                    ErrorOnDetectPhase(text);
                    break;
            }
        }

        public void RegisterEvents(DetectEvent dc, DetectEvent pc, DetectEvent tg, DetectEvent te, DetectEvent ed)
        {
            DirectoriesCreated += dc;
            ProjectCompiled += pc;
            TestsGenerated += tg;
            TestsExecuted += te;
            ErrorOnDetectPhase += ed;
        }
    }
}
