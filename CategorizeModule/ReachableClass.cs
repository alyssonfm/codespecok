using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    public class ReachableClass
    {
        private ReachableNamespace _originNamespace;
        private ReachableClass _originClass;
        private List<ReachableMethod> _methods;
        private List<string> _fields;
        private List<string> _invariants;
        private string _nameOfClass;
        private bool _isInnerClass = false;
        private bool _wasStrongInvCalculated = false;
        private bool _wasVisited = false;

        public ReachableClass(ClassDeclarationSyntax classDecl, SemanticModel model, ReachableNamespace originNamespace)
        {
            SetClassDeclaration(classDecl, model);
            InitializeNamespace(originNamespace);
            InitializeMethods(classDecl, model);
        }
        public ReachableClass(ReachableClass originClass, ClassDeclarationSyntax classDecl, SemanticModel model, ReachableNamespace originNamespace)
        {
            InitializeInnerClass(originClass);
            SetClassDeclaration(classDecl, model);
            InitializeNamespace(originNamespace);
            InitializeMethods(classDecl, model);
        }
        private void InitializeMethods(ClassDeclarationSyntax classDecl, SemanticModel model)
        {
            _methods = new List<ReachableMethod>();
            _invariants = new List<string>();
            foreach (BaseMethodDeclarationSyntax baseMethod in classDecl.DescendantNodes().OfType<BaseMethodDeclarationSyntax>())
            {
                ReachableMethod rm = new ReachableMethod(baseMethod, this, _originNamespace);
                if (rm.GetName().Equals("LocalInvariant"))
                {
                    InitializeInvariants(rm);
                }
                else
                {
                    this._methods.Add(rm);
                }
            }
        }
        private void InitializeInvariants(ReachableMethod rm)
        {
            foreach (StatementSyntax s in rm.GetMethod().Body.Statements)
            {
                if (s is ExpressionStatementSyntax)
                {
                    ExpressionStatementSyntax e = (ExpressionStatementSyntax)s;

                    if (e.Expression is InvocationExpressionSyntax)
                    {
                        string contractType = ((MemberAccessExpressionSyntax)((InvocationExpressionSyntax)e.Expression).Expression).Name.Identifier.Value.ToString();
                        if (contractType.Equals("Invariant"))
                        {
                            _invariants.Add(((InvocationExpressionSyntax)e.Expression).ArgumentList.Arguments[0].ToString());
                        }
                    }
                }
            }
        }
        public int GetNumberOfInvariants()
        {
            return _invariants.Count;
        }

        private void InitializeNamespace(ReachableNamespace origin)
        {
            _originNamespace = origin;
        }
        private void InitializeInnerClass(ReachableClass originClass)
        {
            _isInnerClass = true;
            _originClass = originClass;
        }
        private void SetClassDeclaration(ClassDeclarationSyntax classDecl, SemanticModel model)
        {
            if (classDecl == null)
                throw new ArgumentException();
            InitializeClass(classDecl);
            InitializeFields(classDecl,  model);
        }
        private void InitializeFields(ClassDeclarationSyntax classDeclaration, SemanticModel model)
        {
            List<String> fields = new List<String>();

            // Get all fields from class.
            foreach (FieldDeclarationSyntax fieldNode in classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>())
                foreach (VariableDeclaratorSyntax variableNode in fieldNode.Declaration.Variables)
                {
                    ISymbol fieldSymbol = model.GetDeclaredSymbol(variableNode);
                    fields.Add(fieldSymbol.Name.ToString());
                }

            _fields = fields;
        }

        private void InitializeClass(ClassDeclarationSyntax classDecl)
        {
            _nameOfClass = classDecl.Identifier.ToString();
        }

        public string GetName()
        {
            return _nameOfClass;
        }

        public string GetNameOfNamespace()
        {
            return _originNamespace.GetName();
        }

        public string GetFullName()
        {
            if (_isInnerClass)
            {
                return _originClass.GetFullName() + "." + GetName();
            }
            return GetNameOfNamespace() + "." + GetName();
        }

        public List<string> GetFieldsOfClass()
        {
            return _fields;
        }

        public ReachableMethod SearchMethod(string nameOfMethod)
        {
            for (int i = 0; i < _methods.Count; i++)
            {
                if (nameOfMethod.Contains(_methods.ElementAt(i).GetName()))
                {
                    return _methods.ElementAt(i);
                }
            }
            return null;
        }
        public int GetNumberOfMethods()
        {
            return _methods.Count;
        }

        public ReachableMethod GetMethodAt(int index)
        {
            return _methods.ElementAt(index);
        }

        public void ResetScore(string category)
        {
            foreach (ReachableMethod rm in _methods)
                rm.ResetScore(category);
        }
        public bool WasStrongInvCalculated()
        {
            return _wasStrongInvCalculated;
        }
        public void CalculateStrongInv(ReachableMethod method)
        {
            if (!_wasStrongInvCalculated)
            {
                for (int i = 0; i < GetNumberOfInvariants(); i++)
                {
                    method.GetScore().IncrementStrongInv();
                }
                _wasStrongInvCalculated = true;
            }
        }
    }
}
