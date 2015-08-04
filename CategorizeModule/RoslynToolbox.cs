using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CategorizeModule
{
    public static class RoslynToolbox
    {
        public static MethodDeclarationSyntax GetFirstMethod(this ClassDeclarationSyntax root) {
            return root.Members.OfType<MethodDeclarationSyntax>().ElementAt(0);
        }

        public static ClassDeclarationSyntax GetFirstClass(this CompilationUnitSyntax root)
        {
            return root.Members.OfType<ClassDeclarationSyntax>().ElementAt(0);
        }

        public static CompilationUnitSyntax GetCompilationUnit(string namefile)
        {
            return (CompilationUnitSyntax) CSharpSyntaxTree.ParseText(GetTextFromFile(namefile)).GetRoot();   
        }

        public static String GetTextFromFile(string namefile)
        {
            using (StreamReader sr = new StreamReader(namefile))
            {
                return sr.ReadToEnd();
            }
        }

        public static SyntaxList<StatementSyntax> GetBlock(this MethodDeclarationSyntax method)
        {
            return method.Body.Statements;
        }
    }
}
