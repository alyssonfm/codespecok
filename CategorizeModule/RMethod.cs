﻿using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Structures;

namespace CategorizeModule
{
    /// <summary>
    /// Class made to turn Method manipulation and search easier.
    /// </summary>
    public class RMethod
    {
        private BaseMethodDeclarationSyntax _method;
        private RNamespace _originNamespace;
        private RClass _originClass;
        private RMethod _baseMethod;
        private Score _score;
        private List<string> _fields;
        private bool _wasVisited = false;
        private bool _wasStrongInvCalculated = false;

        public RMethod(BaseMethodDeclarationSyntax methodDecl, RClass originClass, RNamespace originNamespace)
        {
            _method = methodDecl;
            _originNamespace = originNamespace;
            _originClass = originClass;
        }
        public Score GetScore()
        {
            return _score;
        }
        public string GetClass()
        {
            return _originClass.GetName();
        }
        public string GetNamespace()
        {
            return _originNamespace.GetName();
        }
        public BaseMethodDeclarationSyntax GetMethod()
        {
            return _method;
        }
        public string GetName()
        {
            if (_method is ConstructorDeclarationSyntax)
                return "ctor";
            else if (_method is DestructorDeclarationSyntax)
                return "dtor";
            else if (_method is OperatorDeclarationSyntax)
                return "op_" + ((OperatorDeclarationSyntax)_method).OperatorKeyword.Text;
            else if (_method is ConversionOperatorDeclarationSyntax)
                return "op_" + ((ConversionOperatorDeclarationSyntax)_method).OperatorKeyword.Text;
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
                foreach(string field in GetFields())
                {
                    if (field.Equals(e.ToString()))
                        return true;
                    if (("this." + field).Equals(e.ToString()))
                        return true;
                }
                return false;
            }
        }
        public void CalculateStrongInv()
        {
            if (!_wasStrongInvCalculated && !_originClass.WasStrongInvCalculated())
            {
                _originClass.CalculateStrongInv(this);
                _wasStrongInvCalculated = true;
            }
        }

        public void AssignBaseMethod(RMethod bm)
        {
            _baseMethod = bm;
        }

        private IEnumerable<string> GetFields()
        {
            return _originClass.GetFieldsOfClass();
        }

        public void SetFields(List<string> fieldsList)
        {
            _fields = fieldsList;
        }

        internal void ResetScore(string category)
        {
            _score = new Score(category);
        }
        public bool WasVisited()
        {
            return _wasVisited;
        }
        public void Visit()
        {
            _wasVisited = true;
        }
        public List<Point> GetPoints()
        {
            return _score.GetPoints(this);   
        }
    }
}
