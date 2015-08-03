using Structures;
using System.Collections.Generic;

namespace CategorizeModule
{
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
            IncrementOthersCodeError(score.GetOthers().CodeError);
            IncrementOthersWeakPos(score.GetOthers().WeakPos);
            IncrementOthersWeakPre(score.GetOthers().WeakPre);
            IncrementOthersStrongPos(score.GetOthers().StrongPos);
            IncrementOthersStrongPre(score.GetOthers().StrongPre);
        }

        public void IncrementWeakPre()
        {
            IncrementMyselfWeakPre(1);
            IncrementOthersWeakPre(1);
        }
        public void IncrementStrongPre()
        {
            IncrementMyselfStrongPre(1);
            IncrementOthersStrongPre(1);
        }
        public void IncrementWeakPre(int v)
        {
            IncrementMyselfWeakPre(v);
            IncrementOthersWeakPre(v);
        }
        public void IncrementStrongPre(int v)
        {
            IncrementMyselfStrongPre(v);
            IncrementOthersStrongPre(v);
        }
        private void IncrementMyselfWeakPre(int points)
        {
            _myself.WeakPre += points;
        }
        private void IncrementMyselfStrongPre(int points)
        {
            _myself.StrongPre += points;
        }
        private void IncrementOthersWeakPre(int points)
        {
            _others.WeakPre += points;
        }
        private void IncrementOthersStrongPre(int points)
        {
            _others.StrongPre += points;
        }
        public void IncrementWeakPos()
        {
            IncrementMyselfWeakPos(1);
            IncrementOthersWeakPos(1);
        }
        public void IncrementStrongPos()
        {
            IncrementMyselfStrongPos(1);
            IncrementOthersStrongPos(1);
        }
        public void IncrementWeakPos(int v)
        {
            IncrementMyselfWeakPos(v);
            IncrementOthersWeakPos(v);
        }
        public void IncrementStrongPos(int v)
        {
            IncrementMyselfStrongPos(v);
            IncrementOthersStrongPos(v);
        }
        private void IncrementMyselfWeakPos(int points)
        {
            _myself.WeakPos += points;
        }
        private void IncrementMyselfStrongPos(int points)
        {
            _myself.StrongPos += points;
        }
        private void IncrementOthersWeakPos(int points)
        {
            _others.WeakPos += points;
        }
        private void IncrementOthersStrongPos(int points)
        {
            _others.StrongPos += points;
        }
        public void IncrementCodeError()
        {
            IncrementMyselfCodeError(1);
            IncrementOthersCodeError(1);
        }
        public void IncrementCodeError(int v)
        {
            IncrementMyselfCodeError(v);
            IncrementOthersCodeError(v);
        }

        private void IncrementMyselfCodeError(int points)
        {
            _myself.CodeError += points;
        }
        private void IncrementOthersCodeError(int points)
        {
            _others.CodeError += points;
        }
        public void IncrementStrongInv()
        {
            IncrementMyselfStrongInv(1);
            IncrementOthersStrongInv(1);
        }
        public void IncrementStrongInv(int v)
        {
            IncrementMyselfStrongInv(v);
            IncrementOthersStrongInv(v);
        }
        private void IncrementOthersStrongInv(int v)
        {
            _others.StrongInv += v;
        }
        private void IncrementMyselfStrongInv(int v)
        {
            _myself.StrongInv += v;
        }

        public List<Point> GetPoints(ReachableMethod rm)
        {
            List<Point> lp = new List<Point>();
            if(_others.CodeError > 0 || _myself.CodeError > 0)
                lp.Add(new Point("Code Error", rm.GetName(), rm.GetClass(), rm.GetNamespace(), _others.CodeError, _myself.CodeError));
            if(_others.WeakPre > 0 || _myself.WeakPre > 0)
                lp.Add(new Point("Weak Precondition", rm.GetName(), rm.GetClass(), rm.GetNamespace(), _others.WeakPre, _myself.WeakPre));
            if(_others.WeakPos > 0 || _myself.WeakPos > 0)
                lp.Add(new Point("Weak Postcondition", rm.GetName(), rm.GetClass(), rm.GetNamespace(), _others.WeakPos, _myself.WeakPos));
            if (_category.Equals(CategoryType.PRECONDITION) && (_others.StrongPre > 0 || _myself.StrongPre > 0))
                lp.Add(new Point("Strong Precondition", rm.GetName(), rm.GetClass(), rm.GetNamespace(), _others.StrongPre, _myself.StrongPre));
            if (_category.Equals(CategoryType.POSTCONDITION) && (_others.StrongPos > 0 || _myself.StrongPos > 0))
                lp.Add(new Point("Strong Postcondition", rm.GetName(), rm.GetClass(), rm.GetNamespace(), _others.StrongPos, _myself.StrongPos));
            if (_category.Equals(CategoryType.INVARIANT) && (_others.StrongInv > 0 || _myself.StrongInv > 0))
                lp.Add(new Point("Strong Invariant", rm.GetName(), rm.GetClass(), rm.GetNamespace(), _others.StrongInv, _myself.StrongInv));
            return lp;
        }
    }

    public class ScoreTable
    {
        public int CodeError;
        public int WeakPre;
        public int WeakPos;
        public int StrongPre;
        public int StrongPos;
        public int StrongInv;

        public ScoreTable()
        {
            CodeError = 0;
            WeakPre = 0;
            WeakPos = 0;
            StrongPre = 0;
            StrongPos = 0;
            StrongInv = 0;
        }
    }
}
