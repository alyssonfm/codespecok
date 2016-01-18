using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Structures;

namespace Commons
{
    /// <summary>
    /// Class needed to generate and XML containing result for each Phase of ContractOk execution.
    /// </summary>
    public class GenerateResult
    {
        /// <summary>
        /// Creates an element for XML containing results.
        /// </summary>
        /// <param name="n">Nonconformance </param>
        /// <param name="isCategorized"></param>
        /// <returns></returns>
        private static XElement CreatesElement(Nonconformance n, bool isCategorized)
        {
            XElement nonconformance = new XElement("Nonconformance");
            nonconformance.SetElementValue("class", n.GetClassName());
            nonconformance.SetElementValue("method", n.GetMethodName());
            nonconformance.SetElementValue("namespace", n.GetNameSpace());
            nonconformance.SetElementValue("type", n.GetContractType());
            if (isCategorized)
            {
                XElement likelyCauses = new XElement("LikelySouces");
                
                List<Point> likelySources = n.GetLikelySources();
                foreach (Point p in likelySources)
                {
                    XElement likelyCause = new XElement("problematicMethod");
                    likelyCause.SetElementValue("class", p.GetClass());
                    likelyCause.SetElementValue("method", p.GetMethod());
                    likelyCause.SetElementValue("namespace", p.GetNamespace());
                    likelyCause.SetElementValue("likelycause", p.GetLikelyCause());
                    likelyCause.SetElementValue("likelihood", p.GetPercent());
                    likelyCauses.Add(likelyCause);
                }

                nonconformance.Add(likelyCauses);
            }

            XElement error = new XElement("Error");
            error.SetElementValue("testname", n.GetNumberedTest());
            error.SetElementValue("testfile", n.GetTestFileName());
            error.SetElementValue("message", n.GetErrorMessage());
            error.SetElementValue("stacktrace", GetStackTraceString(n.GetStackTrace()));

            nonconformance.Add(error);
            return nonconformance;
        }

        private static String GetStackTraceString(String[] st)
        {
            String toShow = "";
            toShow += "Error appeared in " + st[0] + "\n";
            for (int i = 1; i < st.Length; i++ )
            {
                toShow += "----> at " + st[i] + "\n";
            }
            return toShow;
        }

        public static HashSet<Nonconformance> Save(HashSet<Nonconformance> nonconformances, bool isCategorized)
        {
            XElement [] elements = new XElement[nonconformances.Count];
            for(int i = 0; i < nonconformances.Count; i++){
                XElement e = CreatesElement(nonconformances.ElementAt(i), isCategorized);
                elements[i] = e;
            }
            XDocument doc = new XDocument(new XElement("Results", elements));
            doc.Save(isCategorized ? Constants.RESULTS_CATEGORIZED : Constants.RESULTS_DETECTED);

            return nonconformances;
        }

    }
}
