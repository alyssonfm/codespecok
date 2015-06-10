using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.MSBuild;
using Commons;

namespace CategorizeModule
{
    public class ReachableMethodList
    {
        private Solution _sln;
        private List<ReachableMethod> _methods;
        private ReachableMethod _lastMethodFound;

        public ReachableMethodList(string solutionPath)
        {
            OpenSolutionFile(solutionPath);
            LoadMethods();
        }

        private void LoadMethods()
        {
            this._methods = new List<ReachableMethod>();
            List<Document> docs = GetDocumentsToSearchForClass();
            foreach (Document d in docs)
            {
                SearchForMethodsOnDocument(d);
            }
        }

        private void TakeMethodsFromClass(ClassDeclarationSyntax cl, string namespaceName)
        {
            string className = cl.Identifier.ToString();
            List<BaseMethodDeclarationSyntax> methods = new List<BaseMethodDeclarationSyntax>();
            foreach (MemberDeclarationSyntax m in cl.Members)
            {
                if (m is ConstructorDeclarationSyntax | m is MethodDeclarationSyntax)
                {
                    BaseMethodDeclarationSyntax baseMethod = (BaseMethodDeclarationSyntax)m;
                    ReachableMethod rm = new ReachableMethod(baseMethod, className, namespaceName, new List<string>());
                    this._methods.Add(rm);
                }
            }
        }

        private void OpenSolutionFile(string solutionPath)
        {
            // Precondition:
            // ==> Solution path must be non-null, non-empty.
            if (solutionPath == null || solutionPath.Equals(""))
            {
                throw new FileLoadException("We didn't receive any Solution File");
            }

            // Create solution file.
            this._sln = MSBuildWorkspace.Create().OpenSolutionAsync(solutionPath).Result;

            // Postcondition:
            // ==> Solution should be found, if Detection phase already was completed.
            // ==> Some projects had to be found.
            if (this._sln.FilePath == null)
            {
                throw new FileLoadException("For some reason, we couldn't load the Solution file.\n"
                                          + "See the string we receive:\n"
                                          + "SolutionPath ==" + solutionPath);
            }
            else if (!this._sln.Projects.IsAny<Project>())
            {
                throw new FileLoadException("For some reason, Solution loaded doesn't have any project.\n"
                                          + "See the string we receive:\n"
                                          + "SolutionPath ==" + solutionPath);
            }
        }
        private List<Document> GetDocumentsToSearchForClass()
        {
            List<Document> docsGeneral = new List<Document>();
            foreach (Project proj in this._sln.Projects)
            {
                foreach (Document d in proj.Documents)
                {
                    docsGeneral.Add(d);
                }
            }
            return docsGeneral;
        }

        private void SearchForMethodsOnDocument(Document doc)
        {
            // Load AST its root node.
            SyntaxTree st = doc.GetSyntaxTreeAsync().Result;
            var root = (CompilationUnitSyntax)st.GetRoot();

            List<NamespaceDeclarationSyntax> namespaces = GetNamepacesFromList(root.Members);
            foreach (NamespaceDeclarationSyntax n in namespaces)
            {
                string nameNamespace = n.Name.ToString();
                List<ClassDeclarationSyntax> cl01 = GetClassesFromList(n.Members);
                foreach (ClassDeclarationSyntax c in cl01)
                {
                    TakeMethodsFromClass(c, nameNamespace);
                }
            }
            List<ClassDeclarationSyntax> cl02 = GetClassesFromList(root.Members);
            foreach (ClassDeclarationSyntax c in cl02)
            {
                TakeMethodsFromClass(c, "");
            }
        }

        public List<NamespaceDeclarationSyntax> GetNamepacesFromList(SyntaxList<MemberDeclarationSyntax> list)
        {
            List<NamespaceDeclarationSyntax> nl = new List<NamespaceDeclarationSyntax>();
            foreach (MemberDeclarationSyntax m in list)
            {
                if (m is NamespaceDeclarationSyntax)
                {
                    NamespaceDeclarationSyntax n = (NamespaceDeclarationSyntax)m;
                    nl.Add(n);
                }

            }
            return nl;
        }

        public List<ClassDeclarationSyntax> GetClassesFromList(SyntaxList<MemberDeclarationSyntax> list)
        {
            List<ClassDeclarationSyntax> cl = new List<ClassDeclarationSyntax>();
            foreach (MemberDeclarationSyntax m in list)
            {
                if (m is ClassDeclarationSyntax)
                {
                    ClassDeclarationSyntax c = (ClassDeclarationSyntax)m;
                    cl.Add(c);
                }
            }
            return cl;
        }

        public static string TEST_RANDOOP_CLASS { get; internal set; }

        public void ResetScore()
        {
            foreach (ReachableMethod rm in _methods)
            {
                rm.ResetScore();
            }
        }

        public List<Point> GetPoints()
        {
            List<Point> sc = new List<Point>();
            foreach(ReachableMethod rm in _methods)
            {
                sc.AddRange(rm.GetPoint());
            }
            sc.Sort();
            return sc;
        }

        public ReachableMethod GetLastMethodFound()
        {
            return this._lastMethodFound;
        }

        public bool MethodIsReachable(string methodName, string actualClass, string filterHelper)
        {
            List<ReachableMethod> methodsWithSameName = new List<ReachableMethod>();
            foreach(ReachableMethod rm in _methods)
            {
                if (rm.GetMethod() is ConstructorDeclarationSyntax)
                    if (((ConstructorDeclarationSyntax)rm.GetMethod()).Identifier.Value.ToString().Equals(methodName))
                    {
                        methodsWithSameName.Add(rm);
                    }
                else
                    if(((MethodDeclarationSyntax)rm.GetMethod()).Identifier.Value.ToString().Equals(methodName))
                    {
                        methodsWithSameName.Add(rm);
                    }
            }
            List<ReachableMethod> methodsInTheSpecifiedClass = new List<ReachableMethod>();
            foreach (ReachableMethod rm in methodsWithSameName) {
                if (rm.GetClass().Equals(actualClass))
                    methodsInTheSpecifiedClass.Add(rm);
            }
            if(methodsInTheSpecifiedClass.Count > 0)
            {
                this._lastMethodFound = methodsInTheSpecifiedClass[0];
                return true;
            }
            return false;
        }

        public void CalculateStrongInv()
        {
            
        }
    }
}