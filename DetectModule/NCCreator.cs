using Commons;
using Structures;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
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
            XmlDocument docXml = new XmlDocument();
            docXml.Load(Constants.TEST_ERRORS);

            XmlNodeList nodes = docXml.GetElementsByTagName("UnitTestResult");

            IEnumerator ienum = nodes.GetEnumerator();
            while (ienum.MoveNext())
            {
                // Read from XML, the needed values.
                XmlNode unitTestResult = (XmlNode)ienum.Current;
                XmlNode output = unitTestResult.FirstChild;
                XmlNode errorInfo = ((XmlElement)output).GetElementsByTagName("ErrorInfo").Item(0);
                XmlNode mess = errorInfo.FirstChild;
                XmlNode stac = errorInfo.LastChild;
                string message = mess.InnerText.ToString();
                string stackTrace = stac.InnerText.ToString();

                // Create the nonconformance.
                Nonconformance n = new Nonconformance(message, stackTrace);
                if (!result.Contains(n))
                    result.Add(new Nonconformance(message, stackTrace));
            }

            return result;
        }

    }
}
