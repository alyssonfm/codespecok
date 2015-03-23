using Commons;
using Structures;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DetectModule
{
    /// <summary>
    /// Load file with test results, then create nonconformances that were founded.
    /// </summary>
    class NCCreator
    {
        /// <summary>
        /// Constructor of NCCreator class.
        /// </summary>
        public NCCreator()
        {
        }
        
        /// <summary>
        /// Return a list of distinct nonconformances founded with tests.
        /// </summary>
        /// <returns>List of distinct nonconformances founded with tests.</returns>
        public HashSet<Nonconformance> ListNonconformances()
        {
            HashSet<Nonconformance> result = new HashSet<Nonconformance>();
            // Load test results.
            XDocument doc = XDocument.Load(Constants.TEST_ERRORS);
            var testUnits = doc.Descendants().ElementAt(7).Descendants();
            for(int i = 0; i < testUnits.Count(); i += 5) {
                // Read from XML, the needed values.
                string message = testUnits.ElementAt(i + 3).Value;
                string stackTrace = testUnits.ElementAt(i + 4).Value;
                // Create the nonconformance.
                Nonconformance n = new Nonconformance(message, stackTrace);
                if(!result.Contains(n))
                    result.Add(new Nonconformance(message, stackTrace));
            }
            return result;
        }

    }
}
