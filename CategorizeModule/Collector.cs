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
    public static class Collector
    {
        public static MethodDeclarationSyntax GetFirstMethod(this ClassDeclarationSyntax root)
        {
            return root.Members.OfType<MethodDeclarationSyntax>().ElementAt(0);
        }

        public static ClassDeclarationSyntax GetFirstClass(this CompilationUnitSyntax root)
        {
            return root.Members.OfType<ClassDeclarationSyntax>().ElementAt(0);
        }

        public static CompilationUnitSyntax GetCompilationUnit(string namefile)
        {
            return (CompilationUnitSyntax)CSharpSyntaxTree.ParseText(GetTextFromFile(namefile)).GetRoot();
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

        public static CompilationUnitSyntax GetCompilationUnit(this Document doc)
        {
            SyntaxTree st = doc.GetSyntaxTreeAsync().Result;
            return (CompilationUnitSyntax)st.GetRoot();
        }

        public static SemanticModel GetSemanticModel(this Document doc)
        {
            return doc.GetSemanticModelAsync().Result;
        }

        public static List<RClass> GetAllClasses(this RSolution sln)
        {
            List<RClass> list = new List<RClass>();
            for (int i = 0; i < sln.GetNumberOfNamespaces(); i++)
            {
                list.AddRange(sln.GetNamespaceAt(i).GetAllClasses());
            }
            return list;
        }
        public static List<RClass> GetAllClasses(this RNamespace n)
        {
            List<RClass> list = new List<RClass>();
            for (int i = 0; i < n.GetNumberOfClasses(); i++)
            {
                list.Add(n.GetClassAt(i));
            }
            return list;
        }
        public static List<string> GetNames(this IEnumerable<Project> projects)
        {
            List<string> names = new List<string>();
            foreach(Project p in projects)
            {
                names.Add(p.Name.ToString());
            }
            return names;
        }
    }
}
