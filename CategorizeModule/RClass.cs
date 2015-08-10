using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    /// <summary>
    /// Class defining class that are reachable from the solution analysed on Walker.
    /// </summary>
    public class RClass
    {
        private RNamespace _originNamespace;
        private RClass _originClass;
        private RClass _superClass;
        private List<RMethod> _methods;
        private List<string> _fields;
        private List<string> _invariants;
        private string _nameOfClass;
        private string _nameOfSuperClass;
        private bool _isInnerClass = false;
        private bool _isSuperClassAvailable = false;
        private bool _wasStrongInvCalculated = false;

        private bool _wasVisited = false;

        public string GetProject()
        {
            return _originNamespace.GetProjectName();
        }

        public RClass(ClassDeclarationSyntax classDecl, SemanticModel model, RNamespace originNamespace)
        {
            SetClassDeclaration(classDecl, model);
            InitializeNamespace(originNamespace);
            InitializeMethods(classDecl, model);
        }
        public RClass(RClass originClass, ClassDeclarationSyntax classDecl, SemanticModel model, RNamespace originNamespace)
        {
            InitializeInnerClass(originClass);
            SetClassDeclaration(classDecl, model);
            InitializeNamespace(originNamespace);
            InitializeMethods(classDecl, model);
        }
        private void InitializeMethods(ClassDeclarationSyntax classDecl, SemanticModel model)
        {
            _methods = new List<RMethod>();
            _invariants = new List<string>();
            foreach (BaseMethodDeclarationSyntax baseMethod in classDecl.DescendantNodes().OfType<BaseMethodDeclarationSyntax>())
            {
                RMethod rm = new RMethod(baseMethod, this, _originNamespace);
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
        public void Revise(RSolution sln)
        {
            if (!_nameOfSuperClass.Equals("")) {
                if(sln.ClassIsReachable(this, _nameOfSuperClass, _originNamespace.GetImports()))
                {
                    AssignSuperClass(sln);
                    AssignBaseMethods(sln);
                }
            }
        }

        private void AssignBaseMethods(RSolution sln)
        {
            foreach(RMethod m in _methods)
            {
                RMethod bm = _superClass.SearchMethod(GetName());
                if (bm != null)
                {
                    m.AssignBaseMethod(bm);
                }
            }
        }

        private void AssignSuperClass(RSolution sln)
        {
            _superClass = sln.GetLastClassFound();
            _isSuperClassAvailable = true;
            _fields.AddRange(_superClass.GetFieldsOfClass());
            _invariants.AddRange(_superClass.GetInvariants());
        }

        private List<string> GetInvariants()
        {
            return _invariants;
        }

        private RNamespace GetOriginNamespace()
        {
            return _originNamespace;
        }

        private void InitializeInvariants(RMethod rm)
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

        private void InitializeNamespace(RNamespace origin)
        {
            _originNamespace = origin;
        }
        private void InitializeInnerClass(RClass originClass)
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
            if(classDecl.BaseList != null && classDecl.BaseList.Types.Count > 0)
            {
                TypeSyntax type = ((SimpleBaseTypeSyntax)classDecl.BaseList.Types.ElementAt(0)).Type;
                if (type is IdentifierNameSyntax)
                {
                    _nameOfSuperClass = ((IdentifierNameSyntax)type).Identifier.Text;
                }
                else if (type is GenericNameSyntax)
                {
                    _nameOfSuperClass = ((GenericNameSyntax)type).Identifier.Text;
                }
                else
                {
                    _nameOfSuperClass = "";
                }
            } else {
                _nameOfSuperClass = "";
            }
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

        public RMethod SearchMethod(string nameOfMethod)
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

        public RMethod GetMethodAt(int index)
        {
            return _methods.ElementAt(index);
        }

        public void ResetScore(string category)
        {
            _wasStrongInvCalculated = false;
            foreach (RMethod rm in _methods)
                rm.ResetScore(category);
        }
        public bool WasStrongInvCalculated()
        {
            return _wasStrongInvCalculated;
        }
        public void CalculateStrongInv(RMethod method)
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
