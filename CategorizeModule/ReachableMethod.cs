using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    public class ReachableMethod
    {
        private BaseMethodDeclarationSyntax _method;
        private Score _score;
        private string _class;
        private string _namespace;
        private List<string> _fields;

        public ReachableMethod(BaseMethodDeclarationSyntax m, string c, string n, List<string> fs)
        {
            _method = m;
            _class = c;
            _namespace = n;
            SetFields(fs);
        }

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
        public string GetMethodName()
        {
            if (_method is ConstructorDeclarationSyntax)
                return "ctor";
            else
                return ((MethodDeclarationSyntax)_method).Identifier.Value.ToString();
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
        public void SetFields(List<string> fieldsList)
        {
            _fields = fieldsList;
        }

        internal void ResetScore()
        {
            _score = new Score();
        }

        public List<Point> GetPoints()
        {
            return _score.GetPoints(this);   
        }
    }
}
