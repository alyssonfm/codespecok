using System;
using System.IO;
using System.Reflection;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Commons;

namespace ContractOK
{
    class CodeReader
    {
        public static String GetTestMethod(String namefile)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(GetTextFromFile(namefile));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var classDecl = (ClassDeclarationSyntax)root.Members.ElementAt(0);
            MethodDeclarationSyntax methodDecl = (MethodDeclarationSyntax)classDecl.Members.ElementAt(0);

            return methodDecl.ToString();
        }
        public static String GetTextFromFile(string namefile)
        {
            using (StreamReader sr = new StreamReader(Constants.TEST_OUTPUT + Constants.FILE_SEPARATOR +  namefile + ".cs"))
            {
                String line = sr.ReadToEnd();
                return line;
            }
        }
    }
}
