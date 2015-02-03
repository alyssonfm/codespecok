using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    public class Constants
    {
        
	//Constant to get the file separator of the System.
	public static const char FILE_SEPARATOR = Path.PathSeparator;
	//Constants to folders created path.
	//public static const String TEMP_DIR = Path.GetTempPath() + FILE_SEPARATOR + "jmlOK";
	public static const String TEMP_DIR = "C:" + FILE_SEPARATOR + "CodeSpecOK";
	public static const String SOURCE_BIN = TEMP_DIR + FILE_SEPARATOR + "bin";
	public static const String RANDOOP_OUTPUT = TEMP_DIR + FILE_SEPARATOR + "RandoopTests";
	public static const String TEST_RESULTS = TEMP_DIR + FILE_SEPARATOR + "TestsResults";
	//Constant to file that has the class names. 
	public static const String CLASSES = TEMP_DIR + FILE_SEPARATOR  + "classes.txt";
	//Constants to result of Randoop execution under SUT.
	public static const String TEST_ERRORS = TEMP_DIR + FILE_SEPARATOR + "TestResult.xml";
	//Constant to file that contains the result more cleaned. The nonconformances detected by the tool.
	public static const String RESULTS = TEMP_DIR + FILE_SEPARATOR + "results.xml";
    }
}
