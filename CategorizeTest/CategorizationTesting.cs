using CategorizeModule;
using Commons;
using DetectModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structures;
using System;
using System.IO;
using System.Collections.Generic;

namespace CategorizeTest
{
    [TestClass]
    public class CategorizationTesting
    {

        enum NonconformancesSuite { Boogie = 0 };

        private string[] sourceFolderPath = { @"C:\Users\denni_000\OneDrive\Documents\ContractOK-UE\UE04-Boogie-15NC\Source" };
        private string[] solutionFile = { @"Boogie.sln" };
        private string[] testResultsPath = { @"C:\Users\denni_000\Documents\contractok\CategorizeTest\Resources\TestResultFromBoogie.xml" };
        private string[][] correctLikelyCause = { new string[] { "Strong Invariant", "Strong Invariant", "Strong Invariant", "Strong Invariant", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition" } };

        private HashSet<Nonconformance> GetNonconformancesSuite(NonconformancesSuite suite)
        {
            return (new NCCreator()).ListNonconformances(this.testResultsPath[(int)suite]);
        }

        private Nonconformance[] GetNonconformancesSuiteCategorized(NonconformancesSuite suite)
        {
            DirectoryInfo srcFolder = new DirectoryInfo(sourceFolderPath[(int)suite]);
            srcFolder.Unblock();
            HashSet<Nonconformance> nonconformances = (new Categorize()).categorize(GetNonconformancesSuite(suite), sourceFolderPath[(int)suite], solutionFile[(int)suite]);
            List<Nonconformance> toReturn = new List<Nonconformance>();
            foreach (var n in nonconformances)
            {
                toReturn.Add(n);
            }
            return toReturn.ToArray();
        }

        private void VerifyLikelyCausesForNCSuite(NonconformancesSuite suite)
        {
            Nonconformance[] nonconformances = GetNonconformancesSuiteCategorized(suite);
            for (int i = 0; i < nonconformances.Length; i++)
            {
                Assert.AreEqual(nonconformances[i].GetLikelyCause(), correctLikelyCause[0][i]);
            }
        }

        [TestMethod]
        public void TestNonconformancesLikelyCause()
        {
            foreach (NonconformancesSuite suite in Utils.GetList<NonconformancesSuite>())
            {
                VerifyLikelyCausesForNCSuite(suite);
            }

        }
    }
}
