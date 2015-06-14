using System;
using System.Collections.Generic;
using CategorizeModule;
using System.Diagnostics;

namespace CategorizeTest
{
    public class StaticAnalysisTesting
    {
        public void TestNonconformancesLikelyCause()
        {
            Walker w = new Walker(@"E:\Git\testingcategorization\TestingCategorization.sln");
            List<Point> lp = w.WalkOnTest(@"E:\Git\testingcategorization\RandoopTest82486739703059104.cs");
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
                        Debug.WriteLine("There are " + p.GetMyself() + " cases on this method and\n");
                    }
                    Debug.WriteLine("There are " + p.GetOthers() + " cases on methods called by this method\n\n");
                }
                
            }

            Debug.WriteLine("Test finalized...");
        }
    }
}
