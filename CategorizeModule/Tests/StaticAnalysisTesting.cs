using System;
using System.Collections.Generic;
using CategorizeModule;

namespace CategorizeTest
{
    public class StaticAnalysisTesting
    {
        public void TestNonconformancesLikelyCause()
        {
            Walker w = new Walker(@"E:\Git\testingcategorization\TestingCategorization.sln");
            List<Point> lp = w.WalkOnTest(@"E:\Git\testingcategorization\RandoopTest82486739703059104.cs");

            foreach (Point p in lp)
            {
                String m = p.GetMethod();
                String lc = p.GetLikelyCause();
                int ot = p.GetOthers();
                int my = p.GetMyself();
                int a = 10;
            }
        }
    }
}
