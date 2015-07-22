using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Structures;

namespace CategorizeModule
{
    /// <summary>
    /// Class made to turn Method manipulation and search easier.
    /// </summary>
    public class ReachableMethod
    {
        private BaseMethodDeclarationSyntax _method;
        private Score _score;
        private string _class;
        private string _namespace;
        private List<string> _fields;
        
        public ReachableMethod(BaseMethodDeclarationSyntax methodDecl, string nameOfClass, string nameOfNamespace, List<string> fieldsOfClass)
        {
            _method = methodDecl;
            _class = nameOfClass;
            _namespace = nameOfNamespace;
            SetFields(fieldsOfClass);
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
            else if (_method is DestructorDeclarationSyntax)
                return "dtor";
            else if (_method is OperatorDeclarationSyntax)
                return "op_" + ((OperatorDeclarationSyntax)_method).OperatorKeyword.Text;
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
                    if (("this." + field).Equals(e.ToString()))
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
