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
        public const String NOT_EVAL_EXP = "Cannot be Evaluated";
        public const String BAD_FORMMED_EXP = "Incorrect Expression";
        public const String NULL_RELATED = "Null-Related - Code Error";
    }

    public class Categorize
    {
        private Examinator _examiner;

        public HashSet<Nonconformance> categorize(HashSet<Nonconformance> errors, String sourceFolder, String solutionPath)
        {
            this._examiner = new Examinator(sourceFolder + Constants.FILE_SEPARATOR + solutionPath);
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
                        n.SetLikelyCause(CategorizeInvariant(n));
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
            try
            {
                this._examiner.SetPrincipalClassName(n.GetNameSpace(), n.GetClassName());

                if (this._examiner.CheckStrongPrecondition(n.GetMethodName()))
                    return Cause.STRONG_PRE;
                else
                    return Cause.WEAK_POST;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return Cause.NOT_EVAL_EXP;
            }
        }

        public string CategorizePostcondition(Nonconformance n)
        {
            try
            {
                this._examiner.SetPrincipalClassName(n.GetNameSpace(), n.GetClassName());

                if (this._examiner.CheckWeakPrecondition(n.GetMethodName()))
                    return Cause.WEAK_PRE;
                else
                    return Cause.STRONG_POST;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return Cause.NOT_EVAL_EXP;
            }
        }

        public string CategorizeInvariant(Nonconformance n)
        {
            try
            {
                this._examiner.SetPrincipalClassName(n.GetNameSpace(), n.GetClassName());

                if (this._examiner.CheckWeakPrecondition(n.GetMethodName()))
                    return Cause.WEAK_PRE;
                else
                    return Cause.STRONG_INV;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return Cause.NOT_EVAL_EXP;
            }
        }
    }
}
