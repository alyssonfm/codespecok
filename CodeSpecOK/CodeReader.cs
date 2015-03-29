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

        public static ClassDeclarationSyntax GetClassFromList(SyntaxList<MemberDeclarationSyntax> list, string name)
        {
            foreach (ClassDeclarationSyntax c in list)
            {
                string nameOfClass = c.Identifier.Value.ToString();
                if (nameOfClass.Equals(name))
                {
                    return c;
                }
            }
            return null;
        }

        public static  NamespaceDeclarationSyntax GetNamepaceFromList(SyntaxList<MemberDeclarationSyntax> list, string name)
        {
            foreach (NamespaceDeclarationSyntax n in list)
            {
                string nameOfNameSpace = ((IdentifierNameSyntax)n.Name).Identifier.Value.ToString();
                if (nameOfNameSpace.Equals(name))
                {
                    return n;
                }
            }
            return null;
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
