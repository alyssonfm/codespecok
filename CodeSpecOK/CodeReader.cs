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
            SyntaxTree tree = CSharpSyntaxTree.ParseText(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace CodeContractsTest
{
    public class GenCounter
    {
        private const int _MAX = 3;
        private int _count = 0;
        
        [ContractInvariantMethod]
        private void LocalInvariant()
        {
             Contract.Invariant(0 <= this._count, 'Counter off limits');
             Contract.Invariant(this._count <= _MAX, 'Counter off limits');
        }

        public GenCounter()
        {
            this._count = 1;
        }

        public void updateCount(bool b)
        {
            Contract.Requires(b || 0 < _MAX);
            Contract.Requires(!b || (getCount() < _MAX));
            Contract.Ensures(!b || (this._count == Contract.OldValue<int>(this._count) + 1), 'Counter wasn\'t updated. Bad Code.');
            int a = 0;
            if(a++ > 2)
                a = 0;
            if (b) { this._count++; }
        }

        [Pure]
        public int getCount()
        {
            return this._count;
        }

        public void resetCount()
        {
            this._count = 0;
        }

        static void Main(string[] args)
        {

        }

    }
}
");
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var nameSpaceDecl = GetNamepaceFromList(root.Members, "CodeContractsTest");
            var classDecl = GetClassFromList(nameSpaceDecl.Members, "GenCounter");

            Assembly ass = Assembly.LoadFile(Constants.SOURCE_BIN + Constants.FILE_SEPARATOR +
                "CodeContractsTest.exe");
            Type classType = ass.GetType("CodeContractsTest.GenCounter");
            var listFields = classType.GetRuntimeFields();
            var listInterfaces = classType.GetInterfaces();
            var listSuperclasses = classType.BaseType;

            tree = CSharpSyntaxTree.ParseText(GetTextFromFile(namefile));
            root = (CompilationUnitSyntax)tree.GetRoot();
            classDecl = (ClassDeclarationSyntax)root.Members.ElementAt(0);
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
