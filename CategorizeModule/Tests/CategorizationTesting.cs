using CategorizeModule;
using Commons;
using DetectModule;
using Structures;
using System;
using System.IO;
using System.Collections.Generic;

namespace CategorizeTest
{
    public class CategorizationTesting
    {
        // These tests, still depend that the bin file are on bin folder.
        enum NonconformancesSuite { Boogie = 0 };

        private string[] sourceFolderPath = { @"C:\Users\denni_000\OneDrive\Documents\ContractOK-UE\UE04-Boogie-15NC\Source" };
        private string[] solutionFile = { @"Boogie.sln" };
        private string[] testResultsPath = { @"C:\Users\denni_000\Documents\contractok\CategorizeModule\Resources\TestResultFromBoogie.xml" };
        private string[][] correctLikelyCause = { new string[] { "Weak Precondition", "Strong Invariant", "Strong Invariant", "Strong Invariant", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition" } };

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
                if (!nonconformances[i].GetLikelyCause().Equals(correctLikelyCause[0][i]))
                {
                    throw new Exception("A wrong result was found: \n" +
                    "==> i=" + i + " ==>Expected: " + correctLikelyCause[0][i]
                    + "==>And received: " + nonconformances[i].GetLikelyCause());
                }
            }
        }
        private void CreateDirectories()
        {
            while (!Directory.Exists(Constants.SOURCE_BIN))
            {
                Directory.CreateDirectory(Constants.SOURCE_BIN);
            }
        }
        private void CopyBinFiles(NonconformancesSuite suite)
        {

        }
        private void PrepareSuiteForTests(NonconformancesSuite suite)
        {
            CreateDirectories();
            CopyBinFiles(suite);
        }
        public void TestNonconformancesLikelyCause()
        {
            foreach (NonconformancesSuite suite in Utils.GetList<NonconformancesSuite>())
            {
                PrepareSuiteForTests(suite);
                VerifyLikelyCausesForNCSuite(suite);
            }

        }
    }
}
