using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectModule
{
    public class Detect
    {
        public event EventHandler DirectoriesCreated;
        public event EventHandler ProjectCompiled;
        public event EventHandler TestsGenerated;
        public event EventHandler TestsExecuted;
        public event EventHandler ErrorOnDetectPhase;

        private File tempDir = new 

        enum StagesDetect : long
        {
            CREATED_DIRECTORIES, COMPILED_PROJECT, GENERATED_TESTS, EXECUTED_TESTS, ERROR_ON_DETECTION
        };

        public Detect()
        {
            //IO.File;
        }

    }
}
