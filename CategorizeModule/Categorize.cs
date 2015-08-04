using System;
using System.Collections.Generic;
using Commons;
using Structures;

namespace CategorizeModule
{
    /// <summary>
    /// Class responsible to categorize all Nonconformances detected. It users an static analyser
    /// to define a set of LikelySources for each Nonconformance.
    /// </summary>
    public class Categorize
    {
        private Walker _analyser;

        /// <summary>
        /// Map a List of LikelySources for each Nonconformance.
        /// </summary>
        /// <param name="errors">The set of nonconformances</param>
        /// <param name="solutionSourceFolder">Path of source folder of the project solution.</param>
        /// <param name="solutionFileName">Project Solution filename, with it's extension.</param>
        /// <returns>The set of nonconformances already categorized.</returns>
        public HashSet<Nonconformance> categorize(HashSet<Nonconformance> errors, String solutionSourceFolder, String solutionFileName)
        {
            // Initialize Walker giving local of solution to be analyzed.
            this._analyser = Factory.CreateWalker(solutionSourceFolder, solutionFileName);
            // Iterate over all nonconformance, categorizing all.
            foreach (Nonconformance n in errors) 
                CategorizeAs(n, n.GetContractType());

            // Save results on XML, that can be saved later.
            GenerateResult.Save(errors, true);
            return errors;
        }
        /// <summary>
        /// Assign a List of Likely Sources for the nonconformance given.
        /// </summary>
        /// <param name="nonconformance">The nonconformance given.</param>
        /// <param name="category">The type of nonconformance.</param>
        private void CategorizeAs(Nonconformance nonconformance, string category)
        {
            // Resets Score depending on category of Nonconformance.
            _analyser.ResetScore(category);
            // Creates test and walk on it, receiving a list of points as result.
            RTest test = nonconformance.CreateTest();
            List<Point> points = _analyser.WalkOn(test);
            // Set the likely sources of nonconformance with points.
            nonconformance.SetLikelySources(points);
        }
    }
}
