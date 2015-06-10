using System.Collections.Generic;

namespace CategorizeModule
{
    public class Score 
    {
        private InvariantTable _myself;
        private InvariantTable _others;

        public Score()
        {
            InitScore();
        }

        public void InitScore()
        {
            _myself = new InvariantTable();
            _others = new InvariantTable();
        }

        public InvariantTable GetMyself()
        {
            return this._myself;
        }
        public InvariantTable GetOthers()
        {
            return this._others;
        }
        public void Add(Score score)
        {
            IncrementOthersCodeError(score.GetOthers().CodeError);
            IncrementOthersWeakPos(score.GetOthers().WeakPos);
            IncrementOthersWeakPre(score.GetOthers().WeakPre);
        }

        public void IncrementWeakPre()
        {
            IncrementMyselfWeakPre(1);
            IncrementOthersWeakPre(1);
        }
        public void IncrementWeakPre(int v)
        {
            IncrementMyselfWeakPre(v);
            IncrementOthersWeakPre(v);
        }
        private void IncrementMyselfWeakPre(int points)
        {
            _myself.WeakPre += points;
        }
        private void IncrementOthersWeakPre(int points)
        {
            _others.WeakPre += points;
        }

        public void IncrementWeakPos()
        {
            IncrementMyselfWeakPos(1);
            IncrementOthersWeakPos(1);
        }
        public void IncrementWeakPos(int v)
        {
            IncrementMyselfWeakPos(v);
            IncrementOthersWeakPos(v);
        }
        private void IncrementMyselfWeakPos(int points)
        {
            _myself.WeakPos += points;
        }
        private void IncrementOthersWeakPos(int points)
        {
            _others.WeakPos += points;
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

        public List<Point> getPoints(ReachableMethod rm)
        {
            List<Point> lp = new List<Point>();
            lp.Add(new Point("Code Error", rm.GetMethodName(), rm.GetClass(), rm.GetNamespace(), _others.CodeError, _myself.CodeError));
            lp.Add(new Point("Weak Pre", rm.GetMethodName(), rm.GetClass(), rm.GetNamespace(), _others.WeakPre, _myself.WeakPre));
            lp.Add(new Point("Weak Pos", rm.GetMethodName(), rm.GetClass(), rm.GetNamespace(), _others.WeakPos, _myself.WeakPos));
            lp.Add(new Point("Strong Inv", rm.GetMethodName(), rm.GetClass(), rm.GetNamespace(), _others.StrongInv, _myself.StrongInv));
            return lp;
        }
    }

    public class InvariantTable
    {
        public int CodeError = 0;
        public int WeakPre = 0;
        public int WeakPos = 0;
        public int StrongInv = 0;
    }
}
