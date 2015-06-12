using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using Commons;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace CategorizeModule
{
    public class Walker
    {
        private ReachableMethodList _methods;

        public Walker(string solutionPath) {
            // The Reachable methods will be initialized on search through Solution.
            this._methods = new ReachableMethodList(solutionPath);
        }

        public static String GetTextFromFile(string namefile)
        {
            using (StreamReader sr = new StreamReader(namefile))
            {
                String line = sr.ReadToEnd();
                return line;
            }
        }
        public static MethodDeclarationSyntax GetTestMethod(String namefile)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(GetTextFromFile(namefile));
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var classDecl = (ClassDeclarationSyntax)root.Members.ElementAt(0);
            MethodDeclarationSyntax methodDecl = (MethodDeclarationSyntax)classDecl.Members.ElementAt(0);

            return methodDecl;
        }

        public List<Point> WalkOnTest(string testlocation)
        {
            MethodDeclarationSyntax test = GetTestMethod(testlocation);
            this._methods.ResetScore();

            SyntaxList<StatementSyntax> block = test.Body.Statements;
            
            foreach (StatementSyntax s in block)
            {
                if (s is ExpressionStatementSyntax)
                {
                    ExpressionStatementSyntax e = (ExpressionStatementSyntax) s;
                    List<InvocationExpressionSyntax> funs = SearchFunctions(e.Expression);
                    foreach (InvocationExpressionSyntax inv in funs) {
                        // Verify if its reachable
                        MemberAccessExpressionSyntax member = inv.Expression as MemberAccessExpressionSyntax;
                        string methodName = (member.Name as IdentifierNameSyntax).Identifier.Text;
                        string filterHelper = (member.Expression as IdentifierNameSyntax).Identifier.Text;

                        if (MethodIsReachable(methodName, ReachableMethodList.TEST_RANDOOP_CLASS, filterHelper)) {
                            ReachableMethod m = GetMethodFound();
                            WalkOn(m);
                        }
                    }
                }
                if(s is LocalDeclarationStatementSyntax)
                {
                    LocalDeclarationStatementSyntax ldss = (LocalDeclarationStatementSyntax)s;
                    SeparatedSyntaxList<VariableDeclaratorSyntax> lvds = ldss.Declaration.Variables;
                    foreach(VariableDeclaratorSyntax vds in lvds)
                    {
                        if (vds.Initializer.Value is ObjectCreationExpressionSyntax)
                        {
                            ObjectCreationExpressionSyntax oces = (ObjectCreationExpressionSyntax) vds.Initializer.Value;
                            QualifiedNameSyntax qns = (QualifiedNameSyntax)oces.Type;
                            string className = qns.Right.ToString();
                            string namespaceName = qns.Left.ToString();
                            if(MethodIsReachable(className, ReachableMethodList.TEST_RANDOOP_CLASS, namespaceName + "." + className))
                            {
                                ReachableMethod rm = GetMethodFound();
                                WalkOn(rm);
                            }
                        }
                    }
                }
            }
            _methods.CalculateStrongInv();
            return _methods.GetPoints();
        }

        private bool MethodIsReachable(string methodName, string actualClass, string filterHelper)
        {
            return this._methods.MethodIsReachable(methodName, actualClass, filterHelper);
        }

        private List<InvocationExpressionSyntax> SearchFunctions(ExpressionSyntax line)
        {
            List<InvocationExpressionSyntax> funs = new List<InvocationExpressionSyntax>();
            if (line is ParenthesizedExpressionSyntax)
            {
                funs.AddRange(SearchFunctions(((ParenthesizedExpressionSyntax)line).Expression));
            }
            else if (line is BinaryExpressionSyntax)
            {
                funs.AddRange(SearchFunctions(((BinaryExpressionSyntax)line).Left));
                funs.AddRange(SearchFunctions(((BinaryExpressionSyntax)line).Right));
            }
            else if (line is PrefixUnaryExpressionSyntax)
            {
                funs.AddRange(SearchFunctions(((PrefixUnaryExpressionSyntax)line).Operand));
            }
            else if (line is PostfixUnaryExpressionSyntax)
            {
                funs.AddRange(SearchFunctions(((PostfixUnaryExpressionSyntax)line).Operand));
            }
            else if (line is AssignmentExpressionSyntax)
            {
                funs.AddRange(SearchFunctions(((AssignmentExpressionSyntax)line).Left));
            }
            else if (line is InvocationExpressionSyntax)
            {
                InvocationExpressionSyntax inv = (InvocationExpressionSyntax)line;
                foreach(ArgumentSyntax a in inv.ArgumentList.Arguments)
                {
                    funs.AddRange(SearchFunctions(a.Expression));
                }
                funs.Add((InvocationExpressionSyntax) line);
            }
            return funs;
        }

        private Score WalkOn(ReachableMethod method)
        {
            string actualClass = method.GetClass();
            string actualNamespace = method.GetNamespace();

            ContractArguments contracts = GetContractsPreAndPostFromMethod(method.GetMethod());
            if (contracts.Requires.Count == 0)
                method.GetScore().IncrementWeakPre();
            if (contracts.Ensures.Count == 0)
                method.GetScore().IncrementWeakPos();

            SyntaxList<StatementSyntax> block = method.GetMethod().Body.Statements;

            foreach (StatementSyntax line in block)
            {
                if (line is ExpressionStatementSyntax)
                {
                    ExpressionStatementSyntax e = (ExpressionStatementSyntax)line;
                    List<ExpressionSyntax> fieldsWhereValueChanged = GetAttibutionVictims(e.Expression);
                    foreach (ExpressionSyntax exp in fieldsWhereValueChanged)
                    {
                        if (method.IsField(exp))
                        {
                            method.GetScore().IncrementCodeError();
                        }
                    }
                    List<InvocationExpressionSyntax> funs = SearchFunctions(e.Expression);
                    foreach (InvocationExpressionSyntax inv in funs)
                    {
                        // Verify if its reachable
                        MemberAccessExpressionSyntax member = inv.Expression as MemberAccessExpressionSyntax;
                        string methodName = (member.Name as IdentifierNameSyntax).Identifier.Text;
                        string filterHelper = "";
                        if (member.Expression is IdentifierNameSyntax)
                        {
                            filterHelper = (member.Expression as IdentifierNameSyntax).Identifier.Text;
                        }
                        else if(member.Expression is ThisExpressionSyntax)
                        {
                            filterHelper = actualNamespace + "." + actualClass;
                        }
                        else
                        {
                            filterHelper = actualNamespace + "." + actualClass;
                        }


                        if (MethodIsReachable(methodName, actualClass, filterHelper))
                        {
                            ReachableMethod m = GetMethodFound();
                            method.GetScore().Add(WalkOn(m));
                        }
                        else
                        {
                            method.GetScore().IncrementCodeError();
                        }
                    }
                }
            }
            return method.GetScore();
        }

        private List<ExpressionSyntax> GetAttibutionVictims(ExpressionSyntax line)
        {
            List<ExpressionSyntax> attibutes = new List<ExpressionSyntax>();
            if (line is ParenthesizedExpressionSyntax)
            {
                attibutes.AddRange(GetAttibutionVictims(((ParenthesizedExpressionSyntax)line).Expression));
            }
            else if (line is BinaryExpressionSyntax)
            {
                attibutes.AddRange(GetAttibutionVictims(((BinaryExpressionSyntax)line).Left));
                attibutes.AddRange(GetAttibutionVictims(((BinaryExpressionSyntax)line).Right));
            }
            else if (line is PrefixUnaryExpressionSyntax)
            {
                attibutes.Add(((PrefixUnaryExpressionSyntax)line).Operand);
            }
            else if (line is PostfixUnaryExpressionSyntax)
            {
                attibutes.Add(((PostfixUnaryExpressionSyntax)line).Operand);
            }
            else if (line is AssignmentExpressionSyntax)
            {
                attibutes.Add(((AssignmentExpressionSyntax)line).Left);
            }
            else if (line is InvocationExpressionSyntax)
            {
                InvocationExpressionSyntax inv = (InvocationExpressionSyntax)line;
                foreach (ArgumentSyntax a in inv.ArgumentList.Arguments)
                {
                    attibutes.AddRange(GetAttibutionVictims(a.Expression));
                }
            }
            return attibutes;
        }

        private ContractArguments GetContractsPreAndPostFromMethod(BaseMethodDeclarationSyntax method)
        {
            ContractArguments contracts = new ContractArguments();
            foreach (StatementSyntax s in method.Body.Statements)
            {
                if (s is ExpressionStatementSyntax)
                {
                    ExpressionStatementSyntax e = (ExpressionStatementSyntax)s;

                    if (e.Expression is InvocationExpressionSyntax)
                    {
                        string contractType = ((MemberAccessExpressionSyntax)((InvocationExpressionSyntax)e.Expression).Expression).Name.Identifier.Value.ToString();
                        if (contractType.Equals("Requires"))
                        {
                            contracts.Requires.Add(((InvocationExpressionSyntax)e.Expression).ArgumentList.Arguments[0]);
                        }
                        else if (contractType.Equals("Ensures"))
                        {
                            contracts.Ensures.Add(((InvocationExpressionSyntax)e.Expression).ArgumentList.Arguments[0]);
                        }
                    }
                }
            }
            return contracts;
        }
        private ReachableMethod GetMethodFound()
        {
            return _methods.GetLastMethodFound();
        }

    }
    
}
