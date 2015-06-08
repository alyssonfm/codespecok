using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    class ReachableMethod
    {
        private BaseMethodDeclarationSyntax _method;
        private Score _score;
        private string _class;
        private string _namespace;
        private List<string> _fields;
        public Score GetScore()
        {
            return _score;
        }
        public string GetClass()
        {
            return _class;
        }
        public string GetNamespace()
        {
            return _namespace;
        }
        public BaseMethodDeclarationSyntax GetMethod()
        {
            return _method;
        }
        public bool IsField(ExpressionSyntax e)
        {
            if (e is ParenthesizedExpressionSyntax)
            {
                return IsField(((ParenthesizedExpressionSyntax)e).Expression);
            }
            else if (e is PrefixUnaryExpressionSyntax)
            {
                return IsField(((PrefixUnaryExpressionSyntax)e).Operand);
            }
            else if (e is PostfixUnaryExpressionSyntax)
            {
                return IsField(((PostfixUnaryExpressionSyntax)e).Operand);
            }
            else
            {
                foreach(string field in _fields)
                {
                    if (field.Equals(e.ToString()))
                        return true;
                }
                return false;
            }
        }
    }
}
