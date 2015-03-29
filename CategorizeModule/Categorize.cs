using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public HashSet<Nonconformance> categorize(HashSet<Nonconformance> errors, String sourceFolder)
        {
            this._examiner = new Examinator(sourceFolder);
            for (int i = 0; i < errors.Count; i++ )
            {
                Nonconformance n = errors.ElementAt(i);
                switch(n.GetContractType()){
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
            this._examiner.EndExamination();
            return errors;
        }

        public string CategorizePrecondition(Nonconformance n)
        {
            if (n.GetNameSpace() == "")
                this._examiner.SetPrincipalClassName(n.GetClassName());
            else
                this._examiner.SetPrincipalClassName(n.GetNameSpace() + "." + n.GetClassName());
            if (this._examiner.CheckStrongPrecondition(n.GetMethodName()))
                return Cause.STRONG_PRE;
            else
                return Cause.WEAK_POST;
        }

        public string CategorizePostcondition(Nonconformance n)
        {
            if (n.GetNameSpace() == "")
                this._examiner.SetPrincipalClassName(n.GetClassName());
            else
                this._examiner.SetPrincipalClassName(n.GetNameSpace() + "." + n.GetClassName());
            if (this._examiner.CheckWeakPrecondition(n.GetMethodName()))
                return Cause.WEAK_PRE;
            else
                return Cause.STRONG_POST;
        }

        public string CategorizeInvariant(Nonconformance n)
        {
            if (n.GetNameSpace() == "")
                this._examiner.SetPrincipalClassName(n.GetClassName());
            else
                this._examiner.SetPrincipalClassName(n.GetNameSpace() + "." + n.GetClassName());

            /* if (this._examiner.CheckNull(n.GetMethodName()))
                    return Cause.NULL_RELATED;
            else */ if (this._examiner.CheckWeakPrecondition(n.GetMethodName()))
                    return Cause.WEAK_PRE;
            else
                    return Cause.STRONG_INV;
        }
    }
}
