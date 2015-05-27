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

        private string[] sourceFolderPath = { @"E:\OneDrive\Documents\ContractOK-UE\UE04-Boogie-15NC\Source" };
        private string[] binFolderPath = { @"E:\OneDrive\Documents\ContractOK-UE\UE04-Boogie-15NC\Binaries" };
        private string[] solutionFile = { @"Boogie.sln" };
        private string[] testResultsPath = { @"E:\Git\contractok\CategorizeModule\Resources\TestResultFromBoogie.xml" };
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
        private void CleanDirectories()
        {
            var di = new DirectoryInfo(Constants.TEMP_DIR);
            // This assures that files non-executing can be deleted.
            foreach (var file in di.GetFiles("*", SearchOption.AllDirectories))
                file.Attributes &= ~FileAttributes.ReadOnly;

            // Deletes all files in SOURCE_BIN dir, where all bin files will be stored.
            foreach (var path in new string[] { Constants.SOURCE_BIN })
            {
                Array.ForEach(Directory.GetFiles(path), File.Delete);
            }
        }
        private void CopyBinFiles(NonconformancesSuite suite)
        {
            // Currently, this only add files directly on SOURCE_BIN dir.

            string[] files = System.IO.Directory.GetFiles(binFolderPath[(int)suite]);

            // Copy the files and overwrite destination files if they already exist.
            foreach (string f in files)
            {
                // Use static Path methods to extract only the file name from the path.
                string fileName = System.IO.Path.GetFileName(f);
                string destFile = System.IO.Path.Combine(Constants.SOURCE_BIN, fileName);
                System.IO.File.Copy(f, destFile, true);
            }
        }
        private void PrepareSuiteForTests(NonconformancesSuite suite)
        {
            CreateDirectories();
            CleanDirectories();
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
