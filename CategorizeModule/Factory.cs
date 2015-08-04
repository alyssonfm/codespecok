using Commons;
using Structures;
using System.Collections.Generic;

namespace CategorizeModule
{
    public static class Factory
    {
        public static RTest CreateTest(this Nonconformance N)
        {
            return new RTest(Constants.TEST_OUTPUT + Constants.FILE_SEPARATOR + N.GetTestFileName() + ".cs");
        }
        public static Walker CreateWalker(string sourceFolder, string solutionPath)
        {
            return new Walker(sourceFolder + Constants.FILE_SEPARATOR + solutionPath);
        }

        public static void AddPoint(this List<Point> list, string cause, RMethod method, int others, int myself)
        {
            list.Add(new Point(cause, method.GetName(), method.GetClass(), method.GetNamespace(), others, myself));
        }
    }
}
