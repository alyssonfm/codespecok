using Structures;
using System.Collections.Generic;
using CategorizeModule;
using System.Diagnostics;
using System;

namespace CategorizeTest
{
    public class StaticAnalysisTesting
    {
        public void TestNonconformancesLikelyCause()
        {
            DoTest();
        }

        private static void DoTest()
        {
            Walker w = new Walker(@"E:\Git\testingcategorization\TestingCategorization.sln");
            w.ResetScore(CategoryType.INVARIANT);
            List<Point> lp = w.WalkOn(new RTest(@"E:\Git\testingcategorization\RandoopTest82486739703059104.cs"));
            int counter = 1;
            Debug.WriteLine("Test initialized...\n\n");
            foreach (Point p in lp)
            {
                if (!(p.GetOthers() == 0 && p.GetMyself() == 0))
                {
                    Debug.WriteLine(counter++ + "º " + p.GetNamespace() + "." + p.GetClass() + "." + p.GetMethod()
                        + "\nLikely Source: " + p.GetLikelyCause() + "\n");
                    if (!(p.GetMyself() == 0))
                    {
                        Debug.WriteLine("There are " + p.GetMyself() + " cases on this method" + ((p.GetOthers() - p.GetMyself() == 0) ? "" : " and\n"));
                    }
                    if (!(p.GetOthers() - p.GetMyself() == 0))
                    {
                        Debug.WriteLine("There are " + (p.GetOthers() - p.GetMyself()) + " cases on methods called by this method");
                    }
                    Debug.WriteLine("\n");
                }

            }

            Debug.WriteLine("Test finalized...");
        }
    }
}
