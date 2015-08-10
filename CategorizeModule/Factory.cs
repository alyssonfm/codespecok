using Commons;
using Structures;
using System.Collections.Generic;

namespace CategorizeModule
{
    /// <summary>
    /// Class with some extensions related to creation of some objects.
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// Create the test related with the nonconformance.
        /// </summary>
        /// <param name="N">The nonconformance with test information.</param>
        /// <returns>Test related with the nonconformance.</returns>
        public static RTest CreateTest(this Nonconformance N)
        {
            return new RTest(Constants.TEST_OUTPUT + Constants.FILE_SEPARATOR + N.GetTestFileName() + ".cs");
        }
        /// <summary>
        /// Create the analyser whom will walk on the Solution on needed line to analyse tests.
        /// </summary>
        /// <param name="sourceFolder">Full path of folder containing the solution.</param>
        /// <param name="solutionPath">The name of solution file with it's extension.</param>
        /// <returns>Walker created using solution file.</returns>
        public static Walker CreateWalker(string sourceFolder, string solutionPath)
        {
            return new Walker(sourceFolder + Constants.FILE_SEPARATOR + solutionPath);
        }
        /// <summary>
        /// Create the point and add to a list of points.
        /// </summary>
        /// <param name="list">List of points to be adding.</param>
        /// <param name="cause">Likely Source of the point added.</param>
        /// <param name="method">Method of the point added.</param>
        /// <param name="others">Score related with others methods.</param>
        /// <param name="myself">Score related with method itself.</param>
        public static void AddPoint(this List<Point> list, string cause, RMethod method, int others, int myself)
        {
            list.Add(new Point(cause, method.GetName(), method.GetClass(), method.GetNamespace(), others, myself));
        }
    }
}
