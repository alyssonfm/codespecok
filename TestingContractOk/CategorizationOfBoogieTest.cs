using DetectModule;
using CategorizeModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structures;
using System;
using System.Collections.Generic;

namespace TestingContractOk
{
    public static class EnumUtil
    {
        public static List<T> GetList<T>()
        {
            return new List<T>((T[])Enum.GetValues(typeof(T)));
        }
    }

    [TestClass]
    public class CategorizationOfBoogieTest
    {

        enum NonconformancesSuite { Boogie = 0 };

        private string[] sourceFolderPath = { @"C:\Users\denni_000\OneDrive\Documents\ContracOK UE\UE04 - Boogie - 25 NC\Source" };
        private string[] solutionFile = { @"Boogie.sln" };
        private string[] testResultsPath = { @"C:\Users\denni_000\Documents\contractok\TestingContractOk\Resources\TestResultFromBoogie.xml" };
        private string[][] correctLikelyCause = { new string[] { "Strong Invariant", "Strong Invariant" , "Strong Invariant", "Strong Invariant", "Strong Precondition", "Strong Precondition" , "Strong Precondition", "Strong Precondition" , "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition", "Strong Precondition" } };

        private HashSet<Nonconformance> GetNonconformancesSuite(NonconformancesSuite suite)
        {
            return (new NCCreator()).ListNonconformances(this.testResultsPath[(int)suite]);
        }

        private Nonconformance [] GetNonconformancesSuiteCategorized(NonconformancesSuite suite)
        {
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
            Nonconformance [] nonconformances = GetNonconformancesSuiteCategorized(suite);
            for (int i = 0; i < nonconformances.Length; i++)
            {
                Assert.AreEqual(nonconformances[i].GetLikelyCause(), correctLikelyCause[i]);
            }
        }

        [TestMethod]
        public void TestNonconformancesLikelyCause()
        {
            foreach(NonconformancesSuite suite in EnumUtil.GetList<NonconformancesSuite>())
            {
                VerifyLikelyCausesForNCSuite(suite);
            }

        }
    }
}
