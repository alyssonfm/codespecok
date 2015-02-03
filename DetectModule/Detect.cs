using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons;
using Structures;

namespace DetectModule
{
    public delegate void DetectEvent();

    public class Detect
    {
        public event DetectEvent DirectoriesCreated;
        public event DetectEvent ProjectCompiled;
        public event DetectEvent TestsGenerated;
        public event DetectEvent TestsExecuted;
        public event DetectEvent ErrorOnDetectPhase;

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
            if(!Directory.Exists(Constants.TEMP_DIR)){
                Directory.CreateDirectory(Constants.TEMP_DIR);
            }
        }

        public HashSet<Nonconformance> Detect(String source, String lib, String timeout)
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
                System.Console.Write(e.Message);
                TriggersEvent(Stages.ERROR_ON_DETECTION);
                return new HashSet<Nonconformance>();
            }
        }

        public void Execute(String src, String lib, String time)
        {
            try
            {
                this._srcFolder = src;
                this._libFolder = lib;
                this._projName = src.Substring(src.LastIndexOf(Constants.FILE_SEPARATOR)).Trim();

                InitTimer();

                this._timeout = time;
                RunStage("Creating directories", "\nDirectories created in", Stages.CREATED_DIRECTORIES);
            }
        }

        private void runStage(String iniMsg, String finMsg, Stages stagesDetect)
        {
            System.Console.Write(iniMsg + "...");
		    switch (stagesDetect) {
		        case Stages.CREATED_DIRECTORIES:
			        createDirectories();
			        cleanDirectories();			
			        break;
		        case Stages.COMPILED_PROJECT:
			        compileProject(sourceFolder, librariesFolder);
			        break;
		        case Stages.GENERATED_TESTS:
			        generateTests(librariesFolder, timeout);
			        break;
		        case Stages.EXECUTED_TESTS:
			        runTests(librariesFolder);
			        break;
		        case Stages.ERROR_ON_DETECTION:
			        break;
		        default:
			        break;
		    }   
            System.Console.Write(finMsg + " " + CountTime() + " seconds");
		    TriggersEvent(stagesDetect);
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

        private void TriggersEvent(Stages stage)
        {
            switch (stage)
            {
                case Stages.CREATED_DIRECTORIES:
                    DirectoriesCreated();
                    break;

                case Stages.COMPILED_PROJECT:
                    ProjectCompiled();
                    break;

                case Stages.GENERATED_TESTS:
                    TestsGenerated();
                    break;

                case Stages.EXECUTED_TESTS:
                    TestsExecuted();
                    break;

                case Stages.ERROR_ON_DETECTION:
                    ErrorOnDetectPhase();
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
