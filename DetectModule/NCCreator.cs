using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using Structures;
using Commons;

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
            HashSet<Nonconformance> result = new HashSet<Nonconformance>();
            XDocument doc = XDocument.Load(Constants.TEST_ERRORS);
            var testUnits = doc.Descendants().ElementAt(7).Descendants();
            for(int i = 0; i < testUnits.Count(); i += 5)
            {
                string message = testUnits.ElementAt(i + 3).Value;
                string stackTrace = testUnits.ElementAt(i + 4).Value;
                result.Add(new Nonconformance(message, stackTrace));
            }
            return result;
        }

    }
}
