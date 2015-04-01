using System;
using System.IO;

namespace Commons
{
    /// <summary>
    /// Class containing path to all important folders on ContractOK execution.
    /// </summary>
    public static class Constants
    {
        
	//Constant to get the file separator of the System.
	public static readonly char FILE_SEPARATOR = Path.DirectorySeparatorChar;
	//Constants to folders created path.
	public static readonly String TEMP_DIR = Path.GetTempPath() + FILE_SEPARATOR + "ContractOk";
    public static readonly String SOURCE_BIN = TEMP_DIR + FILE_SEPARATOR + "bin";
    public static readonly String TEST_OUTPUT = TEMP_DIR + FILE_SEPARATOR + "tests";
    public static readonly String LIB_FOLDER = TEMP_DIR + FILE_SEPARATOR + "resources";
    public static readonly String NANT_LIB = LIB_FOLDER + FILE_SEPARATOR + "nant";
    public static readonly String BUILDS_LIB = LIB_FOLDER + FILE_SEPARATOR + "build";
    public static readonly String RANDOOP_LIB = LIB_FOLDER + FILE_SEPARATOR + "randoop" + FILE_SEPARATOR + "bin";
    public static readonly String VSTEST_LIB = LIB_FOLDER + FILE_SEPARATOR + "vstest";
    public static readonly String TEST_RESULTS = TEMP_DIR + FILE_SEPARATOR + "results";
	//Constant to important files.
    public static readonly String NANT_EXE = NANT_LIB + FILE_SEPARATOR + "Nant.exe";
    public static readonly String VSTEST_EXE = VSTEST_LIB + FILE_SEPARATOR + "vstest.console.exe";
    public static readonly String CLASSES = TEMP_DIR + FILE_SEPARATOR + "classes.txt";
	//Constants to result of Randoop execution under SUT.
    public static readonly String TEST_ERRORS = TEST_RESULTS + FILE_SEPARATOR + "TestResult.xml";
	//Constant to file that contains the result more cleaned. The nonconformances detected by the tool.
    public static readonly String RESULTS_DETECTED = TEST_RESULTS + FILE_SEPARATOR + "ResultsDetected.xml";
    public static readonly String RESULTS_CATEGORIZED = TEST_RESULTS + FILE_SEPARATOR + "ResultsCategorized.xml";

    }
}
