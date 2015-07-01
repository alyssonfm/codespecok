using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Commons;
using Structures;

namespace CategorizeModule
{
    public static class Cause
    {
        public const String STRONG_PRE = "Strong Precondition";
        public const String WEAK_PRE = "Weak Precondition";
        public const String STRONG_POST = "Strong Postcondition";
        public const String WEAK_POST = "Weak Postcondition";
        public const String STRONG_INV = "Strong Invariant";
    }

    public class Categorize
    {
        private Examinator _examiner;
        private Walker _analyser;

        public HashSet<Nonconformance> categorize(HashSet<Nonconformance> errors, String sourceFolder, String solutionPath)
        {
            this._examiner = new Examinator(sourceFolder + Constants.FILE_SEPARATOR + solutionPath);
            this._analyser = new Walker(sourceFolder + Constants.FILE_SEPARATOR + solutionPath);
            for (int i = 0; i < errors.Count; i++)
            {
                Nonconformance n = errors.ElementAt(i);
                switch (n.GetContractType())
                {
                    case Structures.CategoryType.PRECONDITION:
                        n.SetLikelyCause(CategorizePrecondition(n));
                        break;
                    case Structures.CategoryType.POSTCONDITION:
                        n.SetLikelyCause(CategorizePostcondition(n));
                        break;
                    case Structures.CategoryType.INVARIANT:
                        n.SetLikelySources(this._analyser.WalkOnTest(Constants.TEST_OUTPUT + n.GetTestFileName()));
                        break;
                    default:
                        break;
                }
            }

            GenerateResult.Save(errors, true);
            return errors;
        }

        public string CategorizePrecondition(Nonconformance n)
        {
                this._examiner.SetPrincipalClassName(n.GetNameSpace(), n.GetClassName());

                if (this._examiner.CheckStrongPrecondition(n.GetMethodName(), n.GetParametersArray()))
                    return Cause.STRONG_PRE;
                else
                    return Cause.WEAK_POST;
        }

        public string CategorizePostcondition(Nonconformance n)
        {
                this._examiner.SetPrincipalClassName(n.GetNameSpace(), n.GetClassName());

                if (this._examiner.CheckWeakPrecondition(n.GetMethodName(), n.GetParametersArray()))
                    return Cause.WEAK_PRE;
                if (this._examiner.CheckWeakPostcondition(n.GetMethodName(), n.GetParametersArray()))
                    return Cause.WEAK_PRE;
                else
                    return Cause.STRONG_POST;
        }

        public string CategorizeInvariant(Nonconformance n)
        {
                this._examiner.SetPrincipalClassName(n.GetNameSpace(), n.GetClassName());

                if (this._examiner.CheckWeakPrecondition(n.GetMethodName(), n.GetParametersArray()))
                    return Cause.WEAK_PRE;
                else
                    return Cause.STRONG_INV;
        }
    }
}
