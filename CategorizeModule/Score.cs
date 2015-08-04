using Structures;
using System;
using System.Collections.Generic;

namespace CategorizeModule
{

    /// <summary>
    /// Class responsible to calculate Score for each Likely Source in each ReachableMethod.
    /// </summary>
    public class Score 
    {
        private ScoreTable _myself;
        private ScoreTable _others;
        private string _category;

        public Score(string category)
        {
            InitScore(category);
        }

        public void InitScore(string category)
        {
            _category = category;
            _myself = new ScoreTable();
            _others = new ScoreTable();
        }

        public string GetCategory()
        {
            return this._category;
        }

        public ScoreTable GetMyself()
        {
            return this._myself;
        }
        public ScoreTable GetOthers()
        {
            return this._others;
        }
        public void Add(Score score)
        {
            _others.IncrementCodeError(score.GetOthers().GetCodeError());
            _others.IncrementWeakPre(score.GetOthers().GetWeakPre());
            _others.IncrementWeakPos(score.GetOthers().GetWeakPos());
            _others.IncrementStrongPre(score.GetOthers().GetStrongPre());
            _others.IncrementStrongPos(score.GetOthers().GetStrongPos());
        }
        public void IncrementCodeError()
        {
            IncrementCodeError(1);
        }
        public void IncrementCodeError(int value)
        {
            _myself.IncrementCodeError(value);
            _others.IncrementCodeError(value);
        }
        public void IncrementWeakPre()
        {
            IncrementWeakPre(1);
        }
        public void IncrementWeakPre(int value)
        {
            _myself.IncrementWeakPre(value);
            _others.IncrementWeakPre(value);
        }
        public void IncrementWeakPos()
        {
            IncrementWeakPos(1);
        }
        public void IncrementWeakPos(int value)
        {
            _myself.IncrementWeakPos(value);
            _others.IncrementWeakPos(value);
        }
        public void IncrementStrongPre()
        {
            IncrementStrongPre(1);
        }
        public void IncrementStrongPre(int value)
        {
            _myself.IncrementStrongPre(value);
            _others.IncrementStrongPre(value);
        }
        public void IncrementStrongPos()
        {
            IncrementStrongPos(1);
        }
        public void IncrementStrongPos(int value)
        {
            _myself.IncrementStrongPre(value);
            _others.IncrementStrongPre(value);
        }
        public void IncrementStrongInv()
        {
            IncrementStrongInv(1);
        }
        public void IncrementStrongInv(int value)
        {
            _myself.IncrementStrongInv(value);
            _others.IncrementStrongInv(value);
        }
        private bool VerifyPositiveValues(int a, int b)
        {
            return a > 0 || b > 0;
        }

        public List<Point> GetPoints(RMethod rm)
        {
            List<Point> lp = new List<Point>();
            AddCodeErrorPoints(rm, lp);
            AddWeakPrePoints(rm, lp);
            AddWeakPosPoints(rm, lp);
            AddStrongPrePoints(rm, lp);
            AddStrongPosPoints(rm, lp);
            AddStrongInvPoints(rm, lp);
            return lp;
        }

        private void AddStrongInvPoints(RMethod rm, List<Point> lp)
        {
            if (_category.Equals(CategoryType.INVARIANT) && VerifyPositiveValues(_others.GetStrongInv(), _myself.GetStrongInv()))
                lp.AddPoint(Cause.STRONG_INV, rm, _others.GetStrongInv(), _myself.GetStrongInv());
        }

        private void AddStrongPosPoints(RMethod rm, List<Point> lp)
        {
            if (_category.Equals(CategoryType.POSTCONDITION) && VerifyPositiveValues(_others.GetStrongPos(), _myself.GetStrongPos()))
                lp.AddPoint(Cause.STRONG_POST, rm, _others.GetStrongPos(), _myself.GetStrongPos());
        }

        private void AddStrongPrePoints(RMethod rm, List<Point> lp)
        {
            if (_category.Equals(CategoryType.PRECONDITION) && VerifyPositiveValues(_others.GetStrongPre(), _myself.GetStrongPre()))
                lp.AddPoint(Cause.STRONG_PRE, rm, _others.GetStrongPre(), _myself.GetStrongPre());
        }

        private void AddWeakPosPoints(RMethod rm, List<Point> lp)
        {
            if (VerifyPositiveValues(_others.GetWeakPos(), _myself.GetWeakPos()))
                lp.AddPoint(Cause.WEAK_POST, rm, _others.GetWeakPos(), _myself.GetWeakPos());
        }

        private void AddWeakPrePoints(RMethod rm, List<Point> lp)
        {
            if (VerifyPositiveValues(_others.GetWeakPre(), _myself.GetWeakPre()))
                lp.AddPoint(Cause.WEAK_PRE, rm, _others.GetWeakPre(), _myself.GetWeakPre());
        }

        private void AddCodeErrorPoints(RMethod rm, List<Point> lp)
        {
            if (VerifyPositiveValues(_others.GetCodeError(), _myself.GetCodeError()))
                lp.AddPoint(Cause.CODE_ERROR, rm, _others.GetCodeError(), _myself.GetCodeError());
        }
    }
    public static class Cause
    {
        public const string STRONG_PRE = "Strong Precondition";
        public const string WEAK_PRE = "Weak Precondition";
        public const string STRONG_POST = "Strong Postcondition";
        public const string WEAK_POST = "Weak Postcondition";
        public const string STRONG_INV = "Strong Invariant";
        public const string CODE_ERROR = "Code Error";
    }

    public class ScoreTable
    {
        private int _CodeError;
        private int _WeakPre;
        private int _WeakPos;
        private int _StrongPre;
        private int _StrongPos;
        private int _StrongInv;

        public ScoreTable()
        {
            SetCodeError(0);
            SetWeakPre(0);
            SetWeakPos(0);
            SetStrongPre(0);
            SetStrongPos(0);
            SetStrongInv(0);
        }

        public int GetCodeError()
        {
            return _CodeError;
        }

        public void SetCodeError(int value){
            _CodeError = value;
        }
        public void IncrementCodeError(int value) {
            _CodeError += value;
        }
        public int GetWeakPre()
        {
            return _WeakPre;
        }

        public void SetWeakPre(int value)
        {
            _WeakPre = value;
        }
        public void IncrementWeakPre(int value)
        {
            _WeakPre += value;
        }
        public int GetWeakPos()
        {
            return _WeakPos;
        }

        public void SetWeakPos(int value)
        {
            _WeakPos = value;
        }
        public void IncrementWeakPos(int value)
        {
            _WeakPos += value;
        }

        public int GetStrongPre()
        {
            return _StrongPre;
        }

        public void SetStrongPre(int value)
        {
            _StrongPre = value;
        }
        public void IncrementStrongPre(int value)
        {
            _StrongPre += value;
        }

        public int GetStrongPos()
        {
            return _StrongPos;
        }

        public void SetStrongPos(int value)
        {
            _StrongPos = value;
        }
        public void IncrementStrongPos(int value)
        {
            _StrongPos += value;
        }

        public int GetStrongInv()
        {
            return _StrongInv;
        }

        public void SetStrongInv(int value)
        {
            _StrongInv = value;
        }
        public void IncrementStrongInv(int value)
        {
            _StrongInv += value;
        }
    }
}
