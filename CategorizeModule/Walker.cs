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
        public static Score WalkOnTest(MethodDeclarationSyntax test, List<ReachableMethod> methodsAvailable)
        {
            SyntaxList<StatementSyntax> block = test.Body.Statements;
            
            foreach (StatementSyntax s in block)
            {
                if (s is ExpressionStatementSyntax)
                {
                    ExpressionStatementSyntax e = (ExpressionStatementSyntax) s;
                    StepOnFunctions(e.Expression);
                }
            }

            return null;
        }

        private static void StepOnFunctions(ExpressionSyntax line)
        {
            if (line is ParenthesizedExpressionSyntax)
            {
                StepOnFunctions(((ParenthesizedExpressionSyntax)line).Expression);
            }
            else if (line is BinaryExpressionSyntax)
            {
                StepOnFunctions(((BinaryExpressionSyntax)line).Left);
                StepOnFunctions(((BinaryExpressionSyntax)line).Right);
            }
            else if (line is PrefixUnaryExpressionSyntax)
            {
                StepOnFunctions(((PrefixUnaryExpressionSyntax)line).Operand);
            }
            else if (line is PostfixUnaryExpressionSyntax)
            {
                StepOnFunctions(((PostfixUnaryExpressionSyntax)line).Operand);
            }
            else if (line is AssignmentExpressionSyntax)
            {
                StepOnFunctions(((AssignmentExpressionSyntax)line).Left);
            }
            else if (line is IdentifierNameSyntax)
            {
                return;
            }
            else if (line is MemberAccessExpressionSyntax)
            {
                return;
            }
            else if (line is InvocationExpressionSyntax)
            {
                InvocationExpressionSyntax inv = (InvocationExpressionSyntax)line;
                foreach(ArgumentSyntax a in inv.ArgumentList.Arguments)
                {
                    StepOnFunctions(a.Expression);
                }
                var member = inv.Expression as MemberAccessExpressionSyntax;
                var name = (member.Expression as IdentifierNameSyntax).Identifier;
                //inv.Expression;
            }
        }

    }
}
