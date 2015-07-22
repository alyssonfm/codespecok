using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;

namespace CategorizeModule
{
    class ReachableClass
    {
        private ClassDeclarationSyntax _classDeclaration;
        private ReachableNamespace _originNamespace;
        private ReachableClass _originClass;
        private List<string> _fields;
        private string _nameOfClass;
        private string originClassName;
        private ClassDeclarationSyntax cds;
        private SemanticModel model;
        private bool _isInnerClass = false;

        public ReachableClass(ClassDeclarationSyntax classDecl, SemanticModel model, ReachableNamespace origin)
        {
            SetClassDeclaration(classDecl, model);
            _originNamespace = origin;
        }

        public ReachableClass(ReachableClass originClass, ClassDeclarationSyntax classDecl, SemanticModel model, ReachableNamespace originNamespace)
        {
            _isInnerClass = true;
            SetClassDeclaration(classDecl, model);
            _originNamespace = originNamespace;
            _originClass = _originClass;
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

        public List<string> GetFieldsOfClass()
        {
            return _fields;
        }
    }
}
