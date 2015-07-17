using System;

namespace Structures
{
    public class Point : IComparable<Point>
    {   
        private string _likelyCause;
        private string _method;
        private string _class;
        private string _namespace;
        private int _myself;
        private int _others;
        private double _percent;
            
        public Point(string lc, string m, string c, string n, int others, int myself)
        {
            setLikelyCause(lc);
            setMethod(m);
            setClass(c);
            setNamespace(n);
            setOthers(others);
            setMyself(myself);
        }

        public void SetPercent(double percent)
        {
            this._percent = percent;
        }

        public double GetPercent()
        {
            return this._percent;
        }

        private void setMyself(int myself)
        {
            this._myself = myself;
        }
        public int GetMyself()
        {
            return this._myself;
        }

        private void setOthers(int others)
        {
            this._others = others;
        }
        public int GetOthers()
        {
            return this._others;
        }
        private void setNamespace(string n)
        {
            this._namespace = n;
        }
        public string GetNamespace()
        {
            return this._namespace;
        }
        private void setClass(string c)
        {
            this._class = c;
        }
        public string GetClass()
        {
            return this._class;
        }
        private void setMethod(string m)
        {
            this._method = m;
        }
        public string GetMethod()
        {
            return this._method;
        }
        private void setLikelyCause(string lc)
        {
            this._likelyCause = lc;
        }
        public string GetLikelyCause()
        {
            return this._likelyCause;
        }

        public int CompareTo(Point other)
        {
            if (other != null)
            {
                if (this._myself != other._myself)
                {
                    return this._myself.CompareTo(other._myself);
                }
                else
                {
                    return this._others.CompareTo(other._others);
                }
            }
            else
                throw new ArgumentException("Object is not a Point");
        }
    }
}