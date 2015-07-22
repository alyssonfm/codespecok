using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    class ReachableNamespace
    {
        private List<ReachableClass> _classes;
        private string _name;

        public ReachableNamespace(NamespaceDeclarationSyntax namespaceDeclaration, SemanticModel model)
        {
            SetNamespaceDeclaration(namespaceDeclaration);
            InitializeClasses(namespaceDeclaration.Members, model);
        }

        private void SetNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            SetName(namespaceDeclaration.Name.ToString());
        }

        private void SetName(string v)
        {
            _name = v;
        }
        

        public string GetName()
        {
            return _name;
        }

        public void InitializeClasses(SyntaxList<MemberDeclarationSyntax> members, SemanticModel model) 
        {
            _classes = new List<ReachableClass>();

            foreach (ClassDeclarationSyntax cds in members.OfType<ClassDeclarationSyntax>())
            {
                ReachableClass rc = new ReachableClass(cds, model, this);
                _classes.Add(rc);
                InitializeInnerClasses(rc, cds.Members, model);
            }
        }

        public void InitializeInnerClasses(ReachableClass originClass, SyntaxList<MemberDeclarationSyntax> members, SemanticModel model)
        {
            foreach (ClassDeclarationSyntax cds in members.OfType<ClassDeclarationSyntax>())
            {
                ReachableClass rc = new ReachableClass(originClass, cds, model, this);
                _classes.Add(rc);
                InitializeInnerClasses(rc, cds.Members, model);
            }
        }
    }
}
