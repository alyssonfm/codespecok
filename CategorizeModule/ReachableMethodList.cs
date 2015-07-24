using System;
using System.Linq;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.MSBuild;
using Commons;
using Structures;

namespace CategorizeModule
{
    public class ReachableMethodList
    {
        private Solution _sln;
        private List<ReachableMethod> _methods;
        private ReachableMethod _lastMethodFound;
        
        /// <summary>
        /// Get all methods from class and declare them as Reachable methods.
        /// </summary>
        /// <param name="cds">The class to be searched</param>
        /// <param name="nds">The namespace of class to be searched</param>
        /// <param name="model">The semantic model of AST where class was loaded</param>
        /// <summary>
        /// Open Solution file, and ensures there are projects on it.
        /// </summary>
        /// <param name="solutionPath">The path to solution needed to load</param>
        private void OpenSolutionFile(string solutionPath)
        {
            // Precondition:
            // ==> Solution path must be non-null, non-empty.
            if (solutionPath == null || solutionPath.Equals(""))
            {
                throw new FileLoadException("We didn't receive any Solution File");
            }

            // Create solution file.
            this._sln = MSBuildWorkspace.Create().OpenSolutionAsync(solutionPath).Result;

            // Postcondition:
            // ==> Solution should be found, if Detection phase already was completed.
            // ==> Some projects had to be found.
            if (this._sln.FilePath == null)
            {
                throw new FileLoadException("For some reason, we couldn't load the Solution file.\n"
                                          + "See the string we receive:\n"
                                          + "SolutionPath ==" + solutionPath);
            }
            else if (!this._sln.Projects.IsAny<Project>())
            {
                throw new FileLoadException("For some reason, Solution loaded doesn't have any project.\n"
                                          + "See the string we receive:\n"
                                          + "SolutionPath ==" + solutionPath);
            }
        }
        /// <summary>
        /// It returns all Documents in Solution to keep them available for search in classes on them.
        /// </summary>
        /// <returns>List of all Documents in local Solution</returns>
        private List<Document> GetDocumentsToSearchForClass()
        {
            List<Document> docs = new List<Document>();
            // Add all docs in all projects from Solution to list.
            foreach (Project proj in this._sln.Projects)
                foreach (Document d in proj.Documents)
                    docs.Add(d);
            return docs;
        }

        public static string TEST_RANDOOP_CLASS { get; internal set; }
        /// <summary>
        /// Reset score on all reachable methods.
        /// </summary>
        public void ResetScore()
        {
            foreach (ReachableMethod rm in _methods)
                rm.ResetScore();
        }
        /// <summary>
        /// Get the list of points from scores in all reachable methods.
        /// </summary>
        /// <returns>List of points of all reachable methods</returns>
        public List<Point> GetPoints()
        {
            List<Point> lsPoint = new List<Point>();
            foreach(ReachableMethod rm in _methods)
                lsPoint.AddRange(rm.GetPoints());
            lsPoint.Sort();
            return lsPoint;
        }
        /// <summary>
        /// Get last method found with MethodIsReachable
        /// </summary>
        /// <returns>A method found with MethodIsReachable</returns>
        public ReachableMethod GetLastMethodFound()
        {
            return this._lastMethodFound;
        }

        public bool MethodIsReachable(string methodName, string actualClass, string filterHelper)
        {
            List<ReachableMethod> methodsWithSameName = new List<ReachableMethod>();
            foreach(ReachableMethod rm in _methods)
            {
                BaseMethodDeclarationSyntax bmds = rm.GetMethod();
                if (bmds is ConstructorDeclarationSyntax)
                {
                    if (rm.GetClass().Equals(methodName))
                    {
                        methodsWithSameName.Add(rm);
                    }
                }
                else
                {
                    if (((MethodDeclarationSyntax)bmds).Identifier.Value.ToString().Equals(methodName))
                    {
                        methodsWithSameName.Add(rm);
                    }
                }
            }
            List<ReachableMethod> methodsInTheSpecifiedClass = new List<ReachableMethod>();
            foreach (ReachableMethod rm in methodsWithSameName) {
                if (filterHelper.Contains(rm.GetClass()))
                    methodsInTheSpecifiedClass.Add(rm);
            }
            if(methodsInTheSpecifiedClass.Count > 0)
            {
                this._lastMethodFound = methodsInTheSpecifiedClass[0];
                return true;
            }
            return false;
        }

        public void CalculateStrongInv()
        {
            
        }
    }
}