using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    public class ReachableMethodList
    {
        private Solution sln;

        public ReachableMethodList(Solution sln)
        {
            this.sln = sln;
        }

        public static string TEST_RANDOOP_CLASS { get; internal set; }

        internal MethodDeclarationSyntax SearchMethod(string methodName, string actualClass, string filterHelper)
        {
            throw new NotImplementedException();
        }

        internal void ResetScore()
        {
            throw new NotImplementedException();
        }

        internal Score GetScores()
        {
            throw new NotImplementedException();
        }

        internal ReachableMethod GetLastMethodFound()
        {
            throw new NotImplementedException();
        }

        internal bool MethodIsReachable(string methodName, string actualClass, string filterHelper)
        {
            throw new NotImplementedException();
        }

        internal void CalculateStrongInv()
        {
            throw new NotImplementedException();
        }
    }
}