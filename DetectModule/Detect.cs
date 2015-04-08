#define DETECT_READY
using Commons;
using Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace DetectModule
{
    /// <summary>
    /// The type of Event that Detect calls on each stage of its execution.
    /// </summary>
    /// <param name="text">Text that will be shown on DetectScreen console.</param>
    public delegate void DetectEvent(String text);

    /// <summary>
    /// Class responsible to Detect nonconformances on a C#/CodeContracts project.
    /// </summary>
    public class Detect
    {
        // Events thrown to GUI purposes (they update the user on which 
        // stage is currently running on Detect phase).
        public event DetectEvent DirectoriesCreated;
        public event DetectEvent ProjectCompiled;
        public event DetectEvent TestsGenerated;
        public event DetectEvent TestsExecuted;
        public event DetectEvent ErrorOnDetectPhase;

        // Information necessary to Detect phase.
        private String _srcFolder;
        private String _solutionFile;
        private String _libFolder;
        private String _projName;
        private String _timeout;

        // A watch to analyse the time needed for each stage.
        private Stopwatch _watch;

        /// <summary>
        /// Object to help recording actual Stage of Detect phase.
        /// </summary>
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
            // The watch needed to record time is created.
            this._watch = new Stopwatch();
        }
        /// <summary>
        /// Detect phase begins, and the Detect object will use source location, and time
        /// to generate tests to detect nonconformances on project given.
        /// </summary>
        /// <param name="source">The source folder path for the project needed to be tested.</param>
        /// <param name="lib">The libraries folder path fot the libraries needed for project to run.</param>
        /// <param name="timeout">Time to generate tests.</param>
        /// <returns>Set of nonconformances founded.</returns>
        public HashSet<Nonconformance> DetectErrors(String source, String solutionFile, String lib, String timeout)
        {
#if DETECT_READY
            try
            {
#endif
                // Execute all scripts, one for stage.
                Execute(source, solutionFile, lib, timeout);
                // List Errors, save results and return nonconformances.
                NCCreator ncfinder = new NCCreator();
                return GenerateResult.Save(ncfinder.ListNonconformances(), false);
#if DETECT_READY
            }
            catch (Exception e)
            {
                // On error, inform the user.
                TriggersEvent(Stages.ERROR_ON_DETECTION, e.Message);
                return new HashSet<Nonconformance>();
            }
#endif
        }
            /// <summary>
            /// Call each stage of Detection phase.
            /// </summary>
            /// <param name="src">Source folder path where the project are.</param>
            /// <param name="lib">Libraries folder path where libraries needed to project run are.</param>
            /// <param name="time">Time for test generation.</param>
        public void Execute(String src, String sln, String lib, String time)
        {
                // Initialize information to run the stages.
                this._srcFolder = src;
                this._solutionFile = sln;
                if (lib == null || lib.Equals(""))
                {
                    this._libFolder = Constants.TEST_RESULTS;
                } else {
                    this._libFolder = lib;
                }
                this._projName = sln.Substring(0, sln.LastIndexOf(".sln")).Trim();
                this._timeout = time;

                // Starts counting time.
                InitTimer();

                // Call each stage of Detection phase.
                RunStage("Creating directories", "\nDirectories created in", Stages.CREATED_DIRECTORIES);
                UnblockFolder(this._srcFolder);
                RunStage("\nCompiling the project", "Project compiled in", Stages.COMPILED_PROJECT);
                UnblockFolder(Constants.SOURCE_BIN);
                RunStage("Generating tests", "Tests generated in", Stages.GENERATED_TESTS);
                UnblockFolder(Constants.TEST_OUTPUT);
                RunStage("Running test into contract-based code", "Tests ran in", Stages.EXECUTED_TESTS);
                // Move test results, to make nonconformances creation easier.
                MoveResultFiles();
        }
        /// <summary>
        /// Run determined stage from Detection Phase, call the necessary scripts.
        /// </summary>
        /// <param name="iniMsg">The text to introduce text generated with current stage execution.</param>
        /// <param name="finMsg">The text ending text generated with current stage execution.</param>
        /// <param name="stagesDetect">Current stage to be called.</param>
        private void RunStage(String iniMsg, String finMsg, Stages stagesDetect)
        {
            String text = iniMsg + "...\n";
            // Call correct algorithm to each stage.
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

            // Send whole text generated with execution of current stage to user.
		    TriggersEvent(stagesDetect, text);
	    }
        /// <summary>
        /// Create directories needed on temporary folder to process data.
        /// </summary>
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
        /// <summary>
        /// Clean the directories created, to ensures that data will not be corrupted
        /// also, save on these folders the extra programs needed for Detect phase.
        /// </summary>
        private void CleanDirectories()
        {
            var di = new DirectoryInfo(Constants.TEMP_DIR);
            foreach (var file in di.GetFiles("*", SearchOption.AllDirectories))
                //File.SetAttributes(file.DirectoryName, FileAttributes.Normal);
                file.Attributes &= ~FileAttributes.ReadOnly;
            foreach (var path in new string []{Constants.SOURCE_BIN, Constants.TEST_OUTPUT, Constants.TEST_RESULTS}){
                Array.ForEach(Directory.GetFiles(path), File.Delete);
            }

            SaveResourcesOnTemp();
        }
        /// <summary>
        /// Save necessary programs to Detect phase on temporary folder.
        /// </summary>
        private void SaveResourcesOnTemp()
        {
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "build.exe");
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "nant.exe");
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "randoop.exe");
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "vstest.exe");
            Process.Start("Resources" + Constants.FILE_SEPARATOR + "testdll.exe");
        }
        /// <summary>
        /// Execute script that will compile project with the contracts, using MSBuild.
        /// </summary>
        /// <returns>Text generated when compiling project.</returns>
        private string CompileProject()
        {
            ProcessStartInfo startInfo = PrepareProcess("compileProject.build");
            String arg = " ";
            arg += "-D:source_folder=\"" + this._srcFolder + "\" ";
            arg += "-D:lib_folder=\"" + this._libFolder + "\" ";
            arg += "-D:build_dir=\"" + Constants.SOURCE_BIN + "\" ";
            arg += "-D:project_sln=" + this._solutionFile + " ";
            arg += "compile_project";
            startInfo.Arguments += arg;

            return RunProcess(startInfo);
        }
        /// <summary>
        /// Execute script that will generate Tests, using Randoop.NET.
        /// </summary>
        /// <returns>Text generated when generating tests.</returns>
        private string GenerateTests()
        {
            //string listDLLs = LookForDLLToTest();
            //string listEXEs = LookForEXEToTest();

            ProcessStartInfo startInfo = PrepareProcess("generateTests.build");
            String arg = " ";
            arg += "-D:toTest.Folder=\"" + Constants.SOURCE_BIN + "\" ";
            arg += "-D:timeout=" + this._timeout + " ";
            arg += "-D:output.dir=\"" + Constants.TEST_OUTPUT + "\" ";
            arg += "-D:randoop_dir=\"" + Constants.RANDOOP_LIB + "\" ";
            arg += "-D:lib_folder=\"" + this._libFolder + "\" ";
            arg += "filterTests";
            startInfo.Arguments += arg;

            return RunProcess(startInfo);
        }

        private string LookForDLLToTest()
        {
            string toReturn = "";
            var di = new DirectoryInfo(Constants.SOURCE_BIN);
            foreach (FileInfo file in di.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
                toReturn += file.Name + " ;";
            return toReturn.Trim();
        }

        private string LookForEXEToTest()
        {
            string toReturn = "";
            var di = new DirectoryInfo(Constants.SOURCE_BIN);
            foreach (FileInfo file in di.GetFiles("*.exe", SearchOption.TopDirectoryOnly))
                toReturn += file.Name + " ;";
            return toReturn.Trim();
        }

        /// <summary>
        /// Execute script that will run tests generated, using VsTest.Console.
        /// </summary>
        /// <returns>Text generated when running tests.</returns>
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

            return text;
        }
        /// <summary>
        /// Move TestResult file, to turn nonconformance creation easier for NCCreator.
        /// </summary>
        private void MoveResultFiles()
        {
            var matches = Directory.GetFiles(Constants.TEMP_DIR + Constants.FILE_SEPARATOR + "TestResults").Where(path => Regex.Match(path, @".trx").Success);
            foreach(string file in matches){
                File.Move(file, Constants.TEST_RESULTS + Constants.FILE_SEPARATOR + "TestResult.xml");
            }
            Directory.Delete(Constants.TEMP_DIR + Constants.FILE_SEPARATOR + "TestResults", true);
        }
        /// <summary>
        /// Prepare a process command to run determined script.
        /// </summary>
        /// <param name="ant">Script needed to run.</param>
        /// <returns>Return the process information.</returns>
        private ProcessStartInfo PrepareProcess(String ant)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Constants.NANT_EXE);
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            startInfo.Arguments = "-buildfile:\"" + Constants.BUILDS_LIB + Constants.FILE_SEPARATOR + ant + "\"";
            return startInfo;
        }
        /// <summary>
        /// Run a process command, executing a script.
        /// </summary>
        /// <param name="startInfo">Process information needed for execution.</param>
        /// <returns>The output of process execution.</returns>
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
        /// <summary>
        /// Initializer timer.
        /// </summary>
        private void InitTimer()
        {
            this._watch.Reset();
            this._watch.Start();
        }
        /// <summary>
        /// Stop timer and calculate the time it has passed.
        /// </summary>
        /// <returns>The time it has passed.</returns>
        private double CountTime()
        {
            this._watch.Stop();
            return this._watch.ElapsedMilliseconds / 1000.0;
        }
        private void UnblockFolder(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            d.Unblock();
        }

        /// <summary>
        /// Call event, updating user about Detection execution.
        /// </summary>
        /// <param name="stage">Last stage that were ran.</param>
        /// <param name="text">Text generated with Stage execution.</param>
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
        /// <summary>
        /// Register events to be executed on each Stage of Detection phase.
        /// </summary>
        /// <param name="dc">Event telling about Directories creation process.</param>
        /// <param name="pc">Event telling about Project compilation process.</param>
        /// <param name="tg">Event telling about Test generation process.</param>
        /// <param name="te">Event telling about Test execution process.</param>
        /// <param name="ed">Event telling about any error that can occur on Detection phase.</param>
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
