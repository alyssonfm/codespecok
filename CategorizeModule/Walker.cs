using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Structures;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CategorizeModule
{
    class ContractArguments
    {
        public ContractArguments()
        {
            this.Requires = new List<ArgumentSyntax>();
            this.Ensures = new List<ArgumentSyntax>();
        }
        public List<ArgumentSyntax> Requires { get; set; }
        public List<ArgumentSyntax> Ensures { get; set; }
    }

    public class Walker
    {
        private const string TEST_RANDOOP_CLASS = "RandoopTest";
        private RSolution _sln;

        public Walker(string solutionPath) {
            // The Reachable methods will be initialized on search through Solution.
            this._sln = new RSolution(solutionPath);
        }

        public void ResetScore(string category) {
            _sln.ResetScore(category);
        }
        public List<Point> WalkOn(RTest test)
        {   
            foreach (StatementSyntax s in test.GetStatements())
            {
                if (s is ExpressionStatementSyntax)
                {
                    ExpressionStatementSyntax e = (ExpressionStatementSyntax) s;
                    List<InvocationExpressionSyntax> funs = SearchFunctions(e.Expression);
                    foreach (InvocationExpressionSyntax inv in funs) {
                        // Verify if its reachable
                        MemberAccessExpressionSyntax maesMethod = inv.Expression as MemberAccessExpressionSyntax;
                        string methodName = (maesMethod.Name as IdentifierNameSyntax).Identifier.Text;
                        WalkOnGeneralMethodCall(maesMethod, methodName);
                    }
                }
                if(s is LocalDeclarationStatementSyntax)
                {
                    LocalDeclarationStatementSyntax ldss = (LocalDeclarationStatementSyntax)s;
                    SeparatedSyntaxList<VariableDeclaratorSyntax> lvds = ldss.Declaration.Variables;
                    foreach (VariableDeclaratorSyntax vds in lvds)
                    {
                        if (vds.Initializer != null)
                        {
                            if (vds.Initializer.Value is ObjectCreationExpressionSyntax)
                            {
                                ObjectCreationExpressionSyntax oces = (ObjectCreationExpressionSyntax)vds.Initializer.Value;
                                QualifiedNameSyntax qns = (QualifiedNameSyntax)oces.Type;
                               // GenericNameSyntax qns = (GenericNameSyntax)oces.Type;
                                // string className = qns.Right.ToString();
                                // string namespaceName = qns.Left.ToString();
                                string className = qns.GetFirstToken(true, true, true, true).ToString();
                                string namespaceName = qns.GetLastToken(true, true, true, true).ToString();
                                if (MethodIsReachable(".ctor", TEST_RANDOOP_CLASS, namespaceName + "." + className))
                                {
                                    RMethod rm = GetMethodFound();
                                    WalkOn(rm);
                                }
                            }
                            else if (vds.Initializer.Value is InvocationExpressionSyntax)
                            {
                                InvocationExpressionSyntax ies = (InvocationExpressionSyntax)vds.Initializer.Value;
                                if (ies.Expression is MemberAccessExpressionSyntax)
                                {
                                    MemberAccessExpressionSyntax maesMethod = (MemberAccessExpressionSyntax)ies.Expression;
                                    string methodName = maesMethod.Name.Identifier.Text;
                                    // Cast Problem maesMethod.Expression 
                                    if (maesMethod.Expression is MemberAccessExpressionSyntax)
                                    {
                                        MemberAccessExpressionSyntax maesClass = (MemberAccessExpressionSyntax)maesMethod.Expression;
                                        string className = maesClass.Name.Identifier.Text;
                                        string namespaceName = maesClass.Expression.ToString();
                                        if (MethodIsReachable(methodName, TEST_RANDOOP_CLASS, namespaceName + "." + className))
                                        {
                                            RMethod rm = GetMethodFound();
                                            WalkOn(rm);
                                        }
                                    }
                                    else if (maesMethod.Expression is IdentifierNameSyntax)
                                    {
                                        IdentifierNameSyntax maesClass = (IdentifierNameSyntax)maesMethod.Expression;
                                        string className = maesClass.Identifier.Text;
                                        if (MethodIsReachable(methodName, TEST_RANDOOP_CLASS, className))
                                        {
                                            RMethod rm = GetMethodFound();
                                            WalkOn(rm);
                                        }
                                    }
                                    else
                                    {
                                        WalkOnGeneralMethodCall(maesMethod, methodName);
                                    }
                                }
                                else if (ies.Expression is IdentifierNameSyntax)
                                {
                                    IdentifierNameSyntax ins = (IdentifierNameSyntax)ies.Expression;
                                    string methodName = ins.Identifier.Text;
                                    if (MethodIsReachable(methodName, TEST_RANDOOP_CLASS, ""))
                                    {
                                        RMethod rm = GetMethodFound();
                                        WalkOn(rm);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            List<Point> temp = _sln.GetPoints();
            temp.Reverse();
            List<Point> toReturn = new List<Point>();
            for(int i = 0; i < 10; i++)
            {
                if(temp.Count > i)
                    toReturn.Add(temp.ElementAt(i));
            }
            return toReturn;
        }

        private bool WalkOnGeneralMethodCall(MemberAccessExpressionSyntax maesMethod, string methodName)
        {
            // Verify if its reachable
            string filterHelper = GetExpressionIdentifier(maesMethod.Expression);

            if (MethodIsReachable(methodName, TEST_RANDOOP_CLASS, filterHelper))
            {
                RMethod m = GetMethodFound();
                WalkOn(m);
                return true;
            }
            return false;
        }

        private bool WalkOnGeneralMethodCall(RMethod method, MemberAccessExpressionSyntax maesMethod, string methodName)
        {
            if(WalkOnGeneralMethodCall(maesMethod, methodName))
            {
                return true;
            }
            else
            {
                method.GetScore().IncrementCodeError();
                return false;
            }
            
        }

        private string GetExpressionIdentifier(ExpressionSyntax expressionSyntax)
        {
            if (expressionSyntax is ParenthesizedExpressionSyntax)
            {
                return GetExpressionIdentifier(((ParenthesizedExpressionSyntax)expressionSyntax).Expression);
            }
            else if(expressionSyntax is IdentifierNameSyntax)
            {
                return (expressionSyntax as IdentifierNameSyntax).Identifier.Text;
            }
            return "";
        }

        private bool MethodIsReachable(string methodName, string actualClass, string filterHelper)
        {
            return this._sln.MethodIsReachable(methodName, actualClass, filterHelper);
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

        private Score WalkOn(RMethod method)
        {
            method.CalculateStrongInv();
            method.Visit();
            string actualClass = method.GetClass();
            string actualNamespace = method.GetNamespace();

            ContractArguments contracts = GetContractsPreAndPostFromMethod(method.GetMethod());
            if (contracts.Requires.Count == 0) { 
                method.GetScore().IncrementWeakPre();
            } else {
                method.GetScore().IncrementStrongPre(contracts.Requires.Count);
            }
            if (contracts.Ensures.Count == 0) {
                method.GetScore().IncrementWeakPos();
            } else {
                method.GetScore().IncrementStrongPos(contracts.Ensures.Count);
            }

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
                        string methodName = "";
                        string filterHelper = "";
                        if (inv.Expression is IdentifierNameSyntax)
                        {
                            methodName = (inv.Expression as IdentifierNameSyntax).Identifier.Text;
                            filterHelper = actualNamespace + "." + actualClass;
                        }
                        else
                        {
                            MemberAccessExpressionSyntax member = inv.Expression as MemberAccessExpressionSyntax;

                            if (member.Name is IdentifierNameSyntax)
                                methodName = (member.Name as IdentifierNameSyntax).Identifier.Text;
                            else if (member.Name is GenericNameSyntax)
                                methodName = (member.Name as GenericNameSyntax).Identifier.Text;
                            if (member.Expression is IdentifierNameSyntax)
                            {
                                filterHelper = (member.Expression as IdentifierNameSyntax).Identifier.Text;
                            }
                            else if (member.Expression is ThisExpressionSyntax)
                            {
                                filterHelper = actualNamespace + "." + actualClass;
                            }
                            else
                            {
                                filterHelper = actualNamespace + "." + actualClass;
                            }
                        }

                        if (MethodIsReachable(methodName, actualClass, filterHelper))
                        {
                            RMethod m = GetMethodFound();
                            method.GetScore().Add(WalkOn(m));
                        }
                        else
                        {
                            if(!filterHelper.Equals("Contract"))
                                method.GetScore().IncrementCodeError();
                        }
                    }
                }
                if (line is LocalDeclarationStatementSyntax)
                {
                    LocalDeclarationStatementSyntax ldss = (LocalDeclarationStatementSyntax)line;
                    SeparatedSyntaxList<VariableDeclaratorSyntax> lvds = ldss.Declaration.Variables;
                    foreach (VariableDeclaratorSyntax vds in lvds)
                    {
                        
                        if (vds.Initializer != null)
                        {
                            if (vds.Initializer.Value is ObjectCreationExpressionSyntax)
                            {
                                // ObjectCreationExpressionSyntax oces = (ObjectCreationExpressionSyntax)vds.Initializer.Value;
                                //IdentifierNameSyntax qns = (IdentifierNameSyntax)oces.Type;
                                string className = vds.GetType().GetProperties().ToString();
                                string namespaceName = vds.GetType().Namespace;//ToString(); //qns..ToString();
                                if (MethodIsReachable(".ctor", actualClass, namespaceName + "." + className))
                                {
                                    RMethod rm = GetMethodFound();
                                    WalkOn(rm);
                                }
                            }
                            else if (vds.Initializer.Value is InvocationExpressionSyntax)
                            {
                                InvocationExpressionSyntax ies = (InvocationExpressionSyntax)vds.Initializer.Value;
                                if (ies.Expression is MemberAccessExpressionSyntax)
                                {
                                    MemberAccessExpressionSyntax maesMethod = (MemberAccessExpressionSyntax)ies.Expression;
                                    string methodName = maesMethod.Name.Identifier.Text;
                                    if (maesMethod.Expression is MemberAccessExpressionSyntax)
                                    {
                                        MemberAccessExpressionSyntax maesClass = (MemberAccessExpressionSyntax)maesMethod.Expression;
                                        string className = maesClass.Name.Identifier.Text;
                                        string namespaceName = maesClass.Expression.ToString();
                                        if (MethodIsReachable(methodName, actualClass, namespaceName + "." + className))
                                        {
                                            RMethod rm = GetMethodFound();
                                            WalkOn(rm);
                                        }
                                    }
                                    else if (maesMethod.Expression is IdentifierNameSyntax)
                                    {
                                        IdentifierNameSyntax maesClass = (IdentifierNameSyntax)maesMethod.Expression;
                                        string className = maesClass.Identifier.Text;
                                        if (MethodIsReachable(methodName, actualClass, className))
                                        {
                                            RMethod rm = GetMethodFound();
                                            WalkOn(rm);
                                        }
                                    }
                                }
                                else if (ies.Expression is IdentifierNameSyntax)
                                {
                                    IdentifierNameSyntax ins = (IdentifierNameSyntax)ies.Expression;
                                    string methodName = ins.Identifier.Text;
                                    if (MethodIsReachable(methodName, actualClass, ""))
                                    {
                                        RMethod rm = GetMethodFound();
                                        WalkOn(rm);
                                    }
                                }
                            }
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
                        string contractType = "";
                        InvocationExpressionSyntax ex = (InvocationExpressionSyntax)e.Expression;

                        if (ex.Expression is MemberAccessExpressionSyntax) contractType = ((MemberAccessExpressionSyntax)ex.Expression).Name.Identifier.Value.ToString();

                        if (ex.Expression is IdentifierNameSyntax) contractType = ((IdentifierNameSyntax)ex.Expression).Identifier.Value.ToString();

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
        private RMethod GetMethodFound()
        {
            return _sln.GetLastMethodFound();
        }

    }
    
}
