using Commons;
using Structures;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    class RSolution
    {
        private RMethod _lastMethodFound;

        private List<RNamespace> _namespaces;
        public RSolution(string solutionPath)
        {
            IEnumerable<Project> projects = OpenSolutionFile(solutionPath);
            LoadMethods(projects);
        }
        private void LoadMethods(IEnumerable<Project> projects)
        {
            this._namespaces = new List<RNamespace>();
            foreach (Document d in GetDocumentsToSearchForClass(projects))
            {
                SearchForMethodsOnDocument(d);
            }
        }
        private void SearchForMethodsOnDocument(Document doc)
        {
            // Load AST and SemanticModel from Doc.
            SyntaxTree st = doc.GetSyntaxTreeAsync().Result;
            SemanticModel model = doc.GetSemanticModelAsync().Result;
            var root = (CompilationUnitSyntax)st.GetRoot();

            // Through all namespaces, get Methods from each class
            foreach (NamespaceDeclarationSyntax nds in root.Members.OfType<NamespaceDeclarationSyntax>())
            {
                RNamespace rn = new RNamespace(nds, model);
                _namespaces.Add(rn);
            }
        }
        private List<Document> GetDocumentsToSearchForClass(IEnumerable<Project> projects)
        {
            List<Document> docs = new List<Document>();
            // Add all docs in all projects from Solution to list.
            foreach (Project proj in projects)
                foreach (Document d in proj.Documents)
                    docs.Add(d);
            return docs;
        }
        private IEnumerable<Project> OpenSolutionFile(string solutionPath)
        {
            Solution sln;
            // Precondition:
            // ==> Solution path must be non-null, non-empty.
            if (solutionPath == null || solutionPath.Equals(""))
            {
                throw new FileLoadException("We didn't receive any Solution File");
            }

            // Create solution file.
            sln = MSBuildWorkspace.Create().OpenSolutionAsync(solutionPath).Result;

            // Postcondition:
            // ==> Solution should be found, if Detection phase already was completed.
            // ==> Some projects had to be found.
            if (sln.FilePath == null)
            {
                throw new FileLoadException("For some reason, we couldn't load the Solution file.\n"
                                          + "See the string we receive:\n"
                                          + "SolutionPath ==" + solutionPath);
            }
            else if (!sln.Projects.IsAny<Project>())
            {
                throw new FileLoadException("For some reason, Solution loaded doesn't have any project.\n"
                                          + "See the string we receive:\n"
                                          + "SolutionPath ==" + solutionPath);
            }

            return sln.Projects;
        }

        private int GetNumberOfNamepaces()
        {
            return _namespaces.Count;
        }
        private RNamespace GetNamepaceAt(int index)
        {
            return _namespaces.ElementAt(index);
        }
        private RNamespace SearchNamespace(string nameOfNamespace)
        {
            for(int i = 0; i < _namespaces.Count; i++)
            {
                if (nameOfNamespace.Contains(_namespaces.ElementAt(i).GetName()))
                {
                    return _namespaces.ElementAt(i);
                }
            }
            return null;
        }

        internal void CalculateStrongInv()
        {
            for (int i = 0; i < GetNumberOfNamepaces(); i++)
            {
                for (int j = 0; j < GetNamepaceAt(i).GetNumberOfClasses(); j++)
                {
                    for (int k = 0; k < GetNamepaceAt(i).GetClassAt(j).GetNumberOfMethods(); k++)
                    {
                        RMethod rm = GetNamepaceAt(i).GetClassAt(j).GetMethodAt(k);
                        for(int l = 0; l < GetNamepaceAt(i).GetClassAt(j).GetNumberOfInvariants(); l++)
                        {
                            rm.GetScore().IncrementStrongInv();
                        }
                    }
                }
            }
        }

        public bool MethodIsReachable(string methodName, string actualClass, string filterHelper)
        {
            if (filterHelper.Equals("this"))
            {
                filterHelper = actualClass;
            }
            List<RMethod> methodsWithSameName = new List<RMethod>();

            RNamespace rn = SearchNamespace(filterHelper);
            if(rn != null)
            {
                RClass rc = rn.SearchClass(filterHelper);
                if(rc != null)
                {
                    RMethod rm = rc.SearchMethod(methodName);
                    if(rm != null)
                    {
                        _lastMethodFound = rm;
                        return true;
                    }
                }
            }
            for(int i = 0; i < GetNumberOfNamepaces(); i++)
            {
                RClass rc = GetNamepaceAt(i).SearchClass(filterHelper);
                if (rc != null)
                {
                    RMethod rm = rc.SearchMethod(methodName);
                    if (rm != null)
                    {
                        _lastMethodFound = rm;
                        return true;
                    }
                }
            }
            for (int i = 0; i < GetNumberOfNamepaces(); i++)
            {
                for (int j = 0; j < GetNamepaceAt(i).GetNumberOfClasses(); j++)
                {
                    RMethod rm = GetNamepaceAt(i).GetClassAt(j).SearchMethod(methodName);
                    if(rm != null)
                    {
                        _lastMethodFound = rm;
                        return true;
                    }
                    
                }
            }
            return false;
        }

        public RMethod GetLastMethodFound()
        {
            return this._lastMethodFound;
        }

        public void ResetScore(string category)
        {
            foreach (RNamespace rn in _namespaces)
                rn.ResetScore(category);
        }

        public List<Point> GetPoints()
        {
            List<Point> lsPoint = new List<Point>();
            for (int i = 0; i < GetNumberOfNamepaces(); i++)
            {
                for (int j = 0; j < GetNamepaceAt(i).GetNumberOfClasses(); j++)
                {
                    for (int k = 0; k < GetNamepaceAt(i).GetClassAt(j).GetNumberOfMethods(); k++)
                    {
                        RMethod rm = GetNamepaceAt(i).GetClassAt(j).GetMethodAt(k);
                        if(rm.WasVisited())
                            lsPoint.AddRange(rm.GetPoints());
                    }
                }
            }
            lsPoint.Sort();
            return lsPoint;
        }
    }
}
