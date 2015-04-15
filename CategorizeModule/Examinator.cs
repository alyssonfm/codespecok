using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Commons;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CategorizeModule
{
    class ContractArguments
    {
        public List<ArgumentSyntax> Requires { get; set; } = new List<ArgumentSyntax>();
        public List<ArgumentSyntax> Ensures { get; set; } = new List<ArgumentSyntax>();
    }
    class Examinator 
    {
        private const int _VAR_FALSE = int.MinValue;
        private string _sourceFolder;
        private string _principalClass;
        private string _CONSTRUCTOR_ALIAS = "<init>";
        private List<String> _variables;
        private Assembly _assembly;

        public enum Operations
        {
            ATR_VAR_IN_PRECONDITION, REQUIRES_TRUE, ATR_MOD, ENSURES_TRUE, WITHOUT_ASSEMBLY
        }

        public Examinator(string srcFolder)
        {
            this._sourceFolder = srcFolder;
            string projName = srcFolder.Substring(srcFolder.LastIndexOf(Constants.FILE_SEPARATOR) + 1);
            TestWorkspaceExp();
        }

        public void TestWorkspaceExp()
        {
            string solutionsStr = @"C:\Users\denni_000\OneDrive\Documents\ContractOK-UE\UE04-Boogie-15NC\Source\Boogie.sln";
            var solution = MSBuildWorkspace.Create().OpenSolutionAsync(solutionsStr).Result;
            

            var projects = solution.Projects;
            foreach(Project p in projects)
            {
                string nome = p.AssemblyName;
                foreach(Document d in p.Documents)
                {
                    SyntaxTree s = d.GetSyntaxTreeAsync().Result;
                    CompilationUnitSyntax root = (CompilationUnitSyntax)s.GetRoot();
                    foreach(MemberDeclarationSyntax m in root.Members)
                    {

                    }
                    var i = 2;


                }

            }


        }

        private bool TryToLoadAssembly(Assembly assembly, string path)
        {
            try
            {
                assembly = Assembly.LoadFrom(path);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private Assembly GetCorrectAssembly(string className) {
            Assembly toReturn = null;
            foreach (FileInfo f in new DirectoryInfo(Constants.SOURCE_BIN).GetFiles("*.dll"))
            {
                if (TryToLoadAssembly(toReturn, f.FullName))
                {
                    toReturn = Assembly.LoadFrom(f.FullName);
                }
                if (toReturn != null && toReturn.GetType(className) != null)
                    return toReturn;
            }
            foreach (FileInfo f in new DirectoryInfo(Constants.SOURCE_BIN).GetFiles("*.exe"))
            {
                if (TryToLoadAssembly(toReturn, f.FullName))
                {
                    toReturn = Assembly.LoadFrom(f.FullName);
                }
                if (toReturn != null && toReturn.GetType(className) != null)
                    return toReturn;
            }
            return null;
        }


        public void SetPrincipalClassName(string className){
            this._principalClass = className;
            _assembly = GetCorrectAssembly(className);

            this._variables = new List<String>();
            foreach(FieldInfo f in GetVariablesFromClass(className))
            {
                if (!this._variables.Contains(f.Name.ToString()))
                    this._variables.Add(f.Name.ToString());
            }
        }
        public string GetPrincipalClassName()
        {
            return this._principalClass;
        }

        private String GetOnlyClassName(String className)
        {
            if (className.Contains("."))
                return className.Substring(className.LastIndexOf(".") + 1);
            else
                 return className;
        }

        private String GetCSPathFromFile(String className)
        {
            String[] classNameArr = className.Split('.');
            String classStr = classNameArr[1];
            classStr += ".cs";
            var fileList = new DirectoryInfo(this._sourceFolder).GetFiles(classStr, SearchOption.AllDirectories);
            foreach(FileInfo f in fileList){
                if(f.FullName.Substring(0, f.FullName.LastIndexOf("\\"+ classStr)).EndsWith(classNameArr[0]))
                {
                    return f.FullName;
                }
            }
            foreach (FileInfo f in fileList)
            {
                return f.FullName;
            }
            return "";
        }

        private void UpdateVariables(string className)
        {
            if (!className.Equals(this.GetPrincipalClassName()))
                foreach (FieldInfo s in GetVariablesFromClass(className))
                    if (!this._variables.Contains(s.Name.ToString()))
                        this._variables.Add(s.ToString());
        }

        private IEnumerable<FieldInfo> GetVariablesFromClass(string className)
        {
            if (_assembly == null) { 
                return new List<FieldInfo>();
            }
            else
            {
                Type classType = _assembly.GetType(className);
                return classType.GetRuntimeFields();
            }
        }

        public bool CheckStrongPrecondition(string methodName)
        {
            if (methodName.Equals(GetOnlyClassName(this.GetPrincipalClassName())))
                methodName = this._CONSTRUCTOR_ALIAS;
            return ExamineCSharpCode(this.GetPrincipalClassName(), methodName, Operations.ATR_VAR_IN_PRECONDITION);
        }
        public bool CheckWeakPrecondition(string method)
        {
            if (method.Equals(GetOnlyClassName(this.GetPrincipalClassName())))
                method = this._CONSTRUCTOR_ALIAS;            
            if (ExamineCSharpCode(this.GetPrincipalClassName(), method, Operations.REQUIRES_TRUE))
                return true;
            if (ExamineCSharpCode(this.GetPrincipalClassName(), method, Operations.ATR_MOD))
                return true;
            if (ExamineCSharpCode(this.GetPrincipalClassName(), method, Operations.ENSURES_TRUE))
                return true;            
            return false;
        }
        private bool ExamineCSharpCode(String className, String methodName, Operations typeOfExamination)
        {
            ClassDeclarationSyntax ourClass = TakeClassFromFile(GetCSPathFromFile(className), className);
		    if(ourClass == null)
			    return false;
            UpdateVariables(className);
		    if(ExamineAllClassAssociated(className, methodName, typeOfExamination))
			    return true;
		    return ExamineMethods(TakeMethodsFromClass(ourClass, methodName), typeOfExamination); 
        }
        private List<MethodDeclarationSyntax> TakeMethodsFromClass(ClassDeclarationSyntax cl, string methodName)
        {
            List<MethodDeclarationSyntax> methods = new List<MethodDeclarationSyntax>();
            foreach(MemberDeclarationSyntax m in cl.Members)
            {
                if (m is MethodDeclarationSyntax)
                {
                    if (((MethodDeclarationSyntax)m).Identifier.Value.ToString().Equals(methodName))
                        methods.Add((MethodDeclarationSyntax)m);
                }
            }
            return methods;
        }
        private bool ExamineMethods(List<MethodDeclarationSyntax> methods, Operations typeOfOperation)
        {
            if (methods != null)
            {
                foreach(MethodDeclarationSyntax m in methods)
                {
                    if (typeOfOperation.Equals(Operations.ATR_MOD))
                    {
                        if (ExamineCodeFromMethod(m))
                            return true;
                    }
                    ContractArguments contracts = GetContractsPreAndPostFromMethod(m);
                    if (contracts.Requires.Count + contracts.Ensures.Count != 0)
                    {
                        if (typeOfOperation.Equals(Operations.ATR_VAR_IN_PRECONDITION) || typeOfOperation.Equals(Operations.REQUIRES_TRUE) || typeOfOperation.Equals(Operations.ENSURES_TRUE))
                        {
                            if (ExaminePrePostClauses(typeOfOperation, m, contracts))
                                return true;
                        }
                    }
                    else if (typeOfOperation.Equals(Operations.REQUIRES_TRUE) || typeOfOperation.Equals(Operations.ENSURES_TRUE))
                        return true;
                }
            }
            return false;
        }

        private bool ExaminePrePostClauses(Operations typeOfOperation, MethodDeclarationSyntax method, ContractArguments contracts)
        {
            bool isRequiresClauseNotFounded = true, isEnsuresClauseNotFounded = true;
            if(contracts.Requires.Count != 0)
            {
                isRequiresClauseNotFounded = false;
                switch (typeOfOperation)
                {
                    case Operations.ATR_VAR_IN_PRECONDITION:
                        if (IsAtrAndVarInPrecondition(method.ParameterList, contracts.Requires))
                            return true;
                        break;
                    case Operations.REQUIRES_TRUE:
                        if (IsClauseTrue(contracts.Requires))
                            return true;
                        break;
                    default:
                        break;
                }
            }
            if(contracts.Ensures.Count != 0)
            {
                isEnsuresClauseNotFounded = false;
                if (typeOfOperation.Equals(Operations.ENSURES_TRUE)){
                    if (IsClauseTrue(contracts.Ensures))
                        return true;
                }
            }
            if (typeOfOperation.Equals(Operations.REQUIRES_TRUE))
                return isRequiresClauseNotFounded;
            if (typeOfOperation.Equals(Operations.ENSURES_TRUE))
                return isEnsuresClauseNotFounded;
            return false;
        }

        private bool IsAtrAndVarInPrecondition(ParameterListSyntax parameters, List<ArgumentSyntax> requires)
        {
            List<String> vars = GetListOfVarsToVerify(parameters);

            foreach (ArgumentSyntax a in requires)
            {
                if (IsAtrAndVarInPrecondition(vars, a.Expression))
                    return true;
            }
            return false;
        }
        private List<String> GetListOfVarsToVerify(ParameterListSyntax parameters)
        {
            List<String> toReturn = new List<String>();
            foreach(ParameterSyntax p in parameters.Parameters)
            {
                toReturn.Add(p.Identifier.Value.ToString());
            }
            foreach(String v in this._variables)
            {
                toReturn.Add(v);
                toReturn.Add("this." + v);
            }
            return toReturn;
        }
        private bool IsAtrAndVarInPrecondition(List<String> vars, ExpressionSyntax exp)
        {
            if (exp is ParenthesizedExpressionSyntax)
            {
                return IsAtrAndVarInPrecondition(vars, ((ParenthesizedExpressionSyntax)exp).Expression);
            }
            else if (exp is BinaryExpressionSyntax)
            {
                return IsAtrAndVarInPrecondition(vars, ((BinaryExpressionSyntax)exp).Left) ||
                    IsAtrAndVarInPrecondition(vars, ((BinaryExpressionSyntax)exp).Right);
            }
            else if (exp is PrefixUnaryExpressionSyntax)
            {
                return IsAtrAndVarInPrecondition(vars, ((PrefixUnaryExpressionSyntax)exp).Operand);
            }
            else if (exp is PostfixUnaryExpressionSyntax)
            {
                return IsAtrAndVarInPrecondition(vars, ((PostfixUnaryExpressionSyntax)exp).Operand);
            }
            else if (exp is IdentifierNameSyntax)
            {
                string expName = ((IdentifierNameSyntax)exp).Identifier.Value.ToString();
                foreach(String s in vars)
                {
                    if (s.ToString().Equals(expName))
                        return true;
                }
            }
            // else NumericLiteralExpression || InvocationExpressionSyntax.
            return false;
        }
        private bool IsClauseTrue(List<ArgumentSyntax> clauses)
        {
            foreach(ArgumentSyntax a in clauses)
            {
                ExpressionSyntax e = a.Expression;
                if (HasTrueValue(e) != 0 && HasTrueValue(e) != _VAR_FALSE)
                    return true;
            }
            return false;
        }
        private int HasTrueValue(ExpressionSyntax exp)
        {
            if(exp is ParenthesizedExpressionSyntax)
            {
                if(HasTrueValue(((ParenthesizedExpressionSyntax)exp).Expression) != 0)
                    return 1;
            }
            else if(exp is LiteralExpressionSyntax)
            {
                return (int) ((LiteralExpressionSyntax)exp).Token.Value;
            }
            else if (exp is BinaryExpressionSyntax)
            {
                int rigth = HasTrueValue(((BinaryExpressionSyntax)exp).Right);
                int left = HasTrueValue(((BinaryExpressionSyntax)exp).Left);
                string op = ((BinaryExpressionSyntax)exp).OperatorToken.Value.ToString();
                switch (op)
                {
                    case "||":
                        if ((rigth != 0) && (rigth != _VAR_FALSE) || (left != 0) && (left != _VAR_FALSE))
                            return 1;
                        return 0;
                    case "&&":
                        if ((rigth == 0) && (rigth == _VAR_FALSE) || (left == 0) && (left == _VAR_FALSE))
                            return 0;
                        return 1;

                    case "==":
                        if (rigth == _VAR_FALSE || left == _VAR_FALSE)
                            return _VAR_FALSE;
                        return ((rigth != 0) == (left != 0))? 1 : 0;

                    case "!=":
                        if (rigth == _VAR_FALSE || left == _VAR_FALSE)
                            return _VAR_FALSE;
                        return ((rigth != 0) != (left != 0)) ? 1 : 0;

                    case "<":
                        if (rigth == _VAR_FALSE || left == _VAR_FALSE)
                            return _VAR_FALSE;
                        return (rigth < left) ? 1 : 0;

                    case ">":
                        if (rigth == _VAR_FALSE || left == _VAR_FALSE)
                            return _VAR_FALSE;
                        return (rigth > left) ? 1 : 0;

                    case "<=":
                        if (rigth == _VAR_FALSE || left == _VAR_FALSE)
                            return _VAR_FALSE;
                        return (rigth <= left) ? 1 : 0;

                    case ">=":
                        if (rigth == _VAR_FALSE || left == _VAR_FALSE)
                            return _VAR_FALSE;
                        return (rigth >= left) ? 1 : 0;

                    default:
                        return 0;
                }

            }
            return _VAR_FALSE;
        }

        private ContractArguments GetContractsPreAndPostFromMethod(MethodDeclarationSyntax method)
        {
            ContractArguments contracts = new ContractArguments();
            foreach(StatementSyntax s in method.Body.Statements)
            {
                if (s is ExpressionStatementSyntax) {
                    ExpressionStatementSyntax e = (ExpressionStatementSyntax) s;

                    if(e.Expression is InvocationExpressionSyntax)
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

        private bool ExamineCodeFromMethod(MethodDeclarationSyntax method)
        {
            SyntaxList<StatementSyntax> block = method.Body.Statements;
            ParameterListSyntax parameters = method.ParameterList;
            if (IsSomeVarOrAtrGettingAttribution(parameters, block))
                return true;
            return false;
        }

        private bool IsSomeVarOrAtrGettingAttribution(ParameterListSyntax parameters, SyntaxList<StatementSyntax> block)
        {
            // Really complicated Stuff, will have to assume cases like If Clauses, and Many others.
            // Will Need Recursion.
            List<String> vars = GetListOfVarsToVerify(parameters);

            foreach(StatementSyntax s in block)
            {
                if(s is ExpressionStatementSyntax)
                {
                    ExpressionStatementSyntax e = (ExpressionStatementSyntax) s;
                    if (IsSomeVarOrAtrGettingAttribution(vars, e.Expression))
                        return true;
                }
            }
            return false;
        }
        private bool IsSomeVarOrAtrGettingAttribution(List<String> vars, ExpressionSyntax line)
        {
            if(line is PrefixUnaryExpressionSyntax)
            {
                if (IsSomeVarOrAtrGettingAttribution(vars, ((PrefixUnaryExpressionSyntax)line).Operand))
                    return true;
            }
            else if(line is PostfixUnaryExpressionSyntax)
            {
                if (IsSomeVarOrAtrGettingAttribution(vars, ((PostfixUnaryExpressionSyntax)line).Operand))
                    return true;
            }
            else if(line is AssignmentExpressionSyntax)
            {
                if (IsSomeVarOrAtrGettingAttribution(vars, ((AssignmentExpressionSyntax)line).Left))
                    return true;
            }
            else if(line is IdentifierNameSyntax)
            {
                string expName = ((IdentifierNameSyntax)line).Identifier.Value.ToString();
                foreach(String s in vars)
                {
                    if (expName.Equals(s))
                        return true;
                }
            }
            return false;
        }
        private bool ExamineAllClassAssociated(string className, string methodName, Operations typeOfOperation)
        {
            if(typeOfOperation.Equals(Operations.ATR_VAR_IN_PRECONDITION) || typeOfOperation.Equals(Operations.REQUIRES_TRUE)){
                List<String> interfacesOfClass = GetInterfacesPathFromClass(className);
                if(interfacesOfClass.Count != 0)
                {
                    foreach(String i in interfacesOfClass){
                        if (ExamineCSharpCode(i, methodName, typeOfOperation))
                            return true;
                    }
                }
            }
            String superClass = GetSuperclassPathFromClass(className);
            if (!superClass.Equals(""))
            {
                try
                {
                    if (ExamineCSharpCode(superClass, methodName, typeOfOperation))
                        return true;
                }
                catch(Exception e)
                {

                }
            }
            return false;
        }

        private List<String> GetInterfacesPathFromClass(string className)
        {
            if(this._assembly != null)
            {
                Type classToLoad = this._assembly.GetType(className);
                List<String> toReturn = new List<String>();
                foreach(Type t in classToLoad.GetInterfaces())
                {
                    toReturn.Add(t.FullName.ToString());
                }
                return toReturn;
            }
            return new List<String>();
        }

        private String GetSuperclassPathFromClass(string className)
        {
            if(this._assembly != null){
                Type classToLoad = this._assembly.GetType(className);
                return classToLoad.BaseType.FullName.ToString();
            }
            return "";
        }

        private ClassDeclarationSyntax TakeClassFromFile(string path, string classString)
        {
            try{
                SyntaxTree st = CSharpSyntaxTree.ParseText(GetTextFromFile(path));
                string className, nameSpaceName;
                ClassDeclarationSyntax classDecl;

                className = GetOnlyClassName(classString);
                if (className.Equals(classString))
                {
                    nameSpaceName = "";
                } else {
                    nameSpaceName = classString.Substring(0, classString.IndexOf("."));
                }
                
                var root = (CompilationUnitSyntax) st.GetRoot();

                if (nameSpaceName.Equals(""))
                {
                    classDecl = GetClassFromList(root.Members, className);
                } else {
                    var nameSpaceDecl = GetNamepaceFromList(root.Members, nameSpaceName);
                    classDecl = GetClassFromList(nameSpaceDecl.Members, className);
                }

                return classDecl;
            }
            catch (Exception e)
            {
                if (classString.Equals(this._principalClass))
                {
                    throw new FileNotFoundException(e.Message + "Principal class couldn't be load.");
                }
                else
                {
                    return null;
                }
            }
        }

        public NamespaceDeclarationSyntax GetNamepaceFromList(SyntaxList<MemberDeclarationSyntax> list, string name)
        {
            foreach (MemberDeclarationSyntax m in list)
            {
                if(m is NamespaceDeclarationSyntax)
                {
                    NamespaceDeclarationSyntax n = (NamespaceDeclarationSyntax)m;

                    string nameOfNameSpace = ((IdentifierNameSyntax)n.Name).Identifier.Value.ToString();
                    if (nameOfNameSpace.Equals(name))
                    {
                        return n;
                    }
                }
                
            }
            return null;
        }

        public ClassDeclarationSyntax GetClassFromList(SyntaxList<MemberDeclarationSyntax> list, string name)
        {
            foreach (MemberDeclarationSyntax m in list)
            {
                if(m is ClassDeclarationSyntax)
                {
                    ClassDeclarationSyntax c = (ClassDeclarationSyntax)m;
                    string nameOfClass = c.Identifier.Value.ToString();
                    if (nameOfClass.Equals(name))
                    {
                        return c;
                    }
                }
            }
            return null;
        }

        private String GetTextFromFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                String line = sr.ReadToEnd();
                return line;
            }
        }
    }
}
