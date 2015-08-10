using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    public class RNamespace
    {
        private List<RClass> _classes;
        private List<string> _imports;
        private string _nameOfNamespace;
        private string _projectName;

        public RNamespace(NamespaceDeclarationSyntax namespaceDeclaration, SemanticModel model, string projectName, List<string> imports) 
        {
            SetNamespaceDeclaration(namespaceDeclaration, projectName, imports);
            InitializeClasses(namespaceDeclaration.Members, model);
        }

        private void SetNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclaration, string projectName, List<string> imports)
        {
            SetName(namespaceDeclaration.Name.ToString());
            SetProjectName(projectName);
            SetImports(imports);
        }

        private void SetImports(List<string> imports)
        {
            _imports = imports;
        }

        public List<string> GetImports()
        {
            return _imports;
        }

        private void SetProjectName(string projectName)
        {
            _projectName = projectName;
        }

        public string GetProjectName()
        {
            return _projectName;
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
