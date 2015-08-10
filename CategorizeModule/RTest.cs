using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Commons;
using Structures;

namespace CategorizeModule
{
    public class RTest
    {
        private CompilationUnitSyntax _compilation;

        public RTest(string namefile)
        {
            SetCompilation(namefile);
        }

        private void SetCompilation(string namefile)
        {
            _compilation = Collector.GetCompilationUnit(namefile);
        }

        public SyntaxList<StatementSyntax> GetStatements()
        {
            return _compilation.GetFirstClass().GetFirstMethod().GetBlock();
        }
    }
}
