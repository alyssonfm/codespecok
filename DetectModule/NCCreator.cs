using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structures;

namespace DetectModule
{
    class NCCreator
    {
        private int _ncCount;

        public NCCreator()
        {
            this._ncCount = 0;
        }

        public int GetNCTotal()
        {
            return this._ncCount;
        }

        public HashSet<Nonconformance> ListNonconformances()
        {
            return null;
        }

    }
}
