using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    public class RNamespace
    {
        private List<RClass> _classes;
        private string _nameOfNamespace;

        public RNamespace(NamespaceDeclarationSyntax namespaceDeclaration, SemanticModel model)
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
            _nameOfNamespace = v;
        }
        

        public string GetName()
        {
            return _nameOfNamespace;
        }

        public void InitializeClasses(SyntaxList<MemberDeclarationSyntax> members, SemanticModel model) 
        {
            _classes = new List<RClass>();

            foreach (ClassDeclarationSyntax cds in members.OfType<ClassDeclarationSyntax>())
            {
                RClass rc = new RClass(cds, model, this);
                _classes.Add(rc);
                InitializeInnerClasses(rc, cds.Members, model);
            }
        }

        public void InitializeInnerClasses(RClass originClass, SyntaxList<MemberDeclarationSyntax> members, SemanticModel model)
        {
            foreach (ClassDeclarationSyntax cds in members.OfType<ClassDeclarationSyntax>())
            {
                RClass rc = new RClass(originClass, cds, model, this);
                _classes.Add(rc);
                InitializeInnerClasses(rc, cds.Members, model);
            }
        }

        public RClass SearchClass(string nameOfClass)
        {
            for (int i = 0; i < _classes.Count; i++)
            {
                if (nameOfClass.Contains(_classes.ElementAt(i).GetName()))
                {
                    return _classes.ElementAt(i);
                }
            }
            return null;
        }

        public int GetNumberOfClasses()
        {
            return _classes.Count;
        }

        public RClass GetClassAt(int index)
        {
            return _classes.ElementAt(index);
        }

        public void ResetScore(string category)
        {
            foreach (RClass rc in _classes)
                rc.ResetScore(category);
        }
    }
}
