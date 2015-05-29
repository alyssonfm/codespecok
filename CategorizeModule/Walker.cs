using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Commons;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    class Walker
    {
        private ReachableMethodList _methods;

        public Walker(Solution sln) {
            // The Reachable methods will be initialized on search through Solution.
            this._methods = new ReachableMethodList(sln);
        }

        public Score WalkOnTest(MethodDeclarationSyntax test, ReachableMethodList methodsAvailable)
        {
            methodsAvailable.ResetScore();

            SyntaxList<StatementSyntax> block = test.Body.Statements;
            
            foreach (StatementSyntax s in block)
            {
                if (s is ExpressionStatementSyntax)
                {
                    ExpressionStatementSyntax e = (ExpressionStatementSyntax) s;
                    StepOnFunctions(e.Expression, ReachableMethodList.TEST_RANDOOP_CLASS);
                }
            }

            return _methods.GetScores();
        }

        private Score StepOnFunctions(ExpressionSyntax line, string actualClass)
        {
            Score score = new Score();

            if (line is ParenthesizedExpressionSyntax)
            {
                StepOnFunctions(((ParenthesizedExpressionSyntax)line).Expression, actualClass);
            }
            else if (line is BinaryExpressionSyntax)
            {
                StepOnFunctions(((BinaryExpressionSyntax)line).Left, actualClass);
                StepOnFunctions(((BinaryExpressionSyntax)line).Right, actualClass);
            }
            else if (line is PrefixUnaryExpressionSyntax)
            {
                StepOnFunctions(((PrefixUnaryExpressionSyntax)line).Operand, actualClass);
            }
            else if (line is PostfixUnaryExpressionSyntax)
            {
                StepOnFunctions(((PostfixUnaryExpressionSyntax)line).Operand, actualClass);
            }
            else if (line is AssignmentExpressionSyntax)
            {
                StepOnFunctions(((AssignmentExpressionSyntax)line).Left, actualClass);
            }
            else if (line is InvocationExpressionSyntax)
            {
                InvocationExpressionSyntax inv = (InvocationExpressionSyntax)line;
                foreach(ArgumentSyntax a in inv.ArgumentList.Arguments)
                {
                    StepOnFunctions(a.Expression, actualClass);
                }
                
                MemberAccessExpressionSyntax member = inv.Expression as MemberAccessExpressionSyntax;
                string methodName = (member.Name as IdentifierNameSyntax).Identifier.Text;
                string filterHelper = (member.Expression as IdentifierNameSyntax).Identifier.Text;

                // Treat cases where function can be acessed or not.
                score.Add(WalkOn(GetMethod(methodName, actualClass, filterHelper)));
            }

            return score;
        }

        private Score WalkOn(MethodDeclarationSyntax method)
        {
            string actualClass = GetClassOf(method);


        }

        private string GetClassOf(MethodDeclarationSyntax method)
        {
            throw new NotImplementedException();
        }

        private MethodDeclarationSyntax GetMethod(string methodName, string actualClass, string filterHelper)
        {
            return _methods.SearchMethod(methodName, actualClass, filterHelper);
        }
    }
}
