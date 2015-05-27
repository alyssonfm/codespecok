using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Commons;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule.Tests
{
    class StaticAnalysisTesting
    {
        public void TestNonconformancesLikelyCause()
        {
            MethodDeclarationSyntax test;
            List<ReachableMethod> methodsAvailable;

            //WalkOnTest(test, methodsAvailable);
        }
    }
}
