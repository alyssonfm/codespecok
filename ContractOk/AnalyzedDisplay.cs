using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Structures;
using Commons;

namespace ContractOK
{
    public partial class AnalyzedDisplay : Form
    {
        private HashSet<Nonconformance> nonconformances;
        private TreeNode nodeNCNamespace;
        private TreeNode nodePMNamespace;
        private TreeNode nodeNCClass;
        private TreeNode nodePMClass;
        private TreeNode nodeNCMethod;
        private TreeNode nodePMMethod;
        private bool HasAnyIndexSelected = false;
        private int numberMethods = 0;

        public AnalyzedDisplay(HashSet<Nonconformance> nonconformance)
        {
            InitializeComponent();
            InitializeNonconformances(nonconformance);
            InitializeListNonconformances();

            btStackTrace.Visible = false;
            
            this.nodeNCNamespace = tvNonconformanceLocation.Nodes[0];
            this.nodeNCClass = this.nodeNCNamespace.Nodes[0];
            this.nodeNCMethod = this.nodeNCClass.Nodes[0];

            this.nodePMNamespace = tvProblematicMethodLocation.Nodes[0];
            this.nodePMClass = this.nodePMNamespace.Nodes[0];
            this.nodePMMethod = this.nodePMClass.Nodes[0];

            this.Closing += (object sender, CancelEventArgs e) =>
            {
                Controller.MakeMainVisibleAgain();
            };

            this.Show();
        }

        private void InitializeListNonconformances()
        {
            lbSetNumberNonconformances.Text = nonconformances.Count + "";
            for (int i = 0; i < nonconformances.Count; i++)
            {
                string output = (i+1) + " - " + nonconformances.ElementAt(i).GetContractType();
                listBoxNonconformances.Items.Add(output);
            }
            listBoxNonconformances.SelectionMode = SelectionMode.One;
        }
        private void InitializeListProblematicMethods(Nonconformance n)
        {
            listBoxProblematicMethods.Items.Clear();

            numberMethods = 0;
            if(n.GetLikelySources().Count > 0)
            {
                numberMethods = n.GetLikelySources().Count;
            }
            for (int i = 0; i < numberMethods; i++)
            {
                if (n.GetLikelySources().ElementAt(i).GetLikelyCause().Equals("Strong Invariant") || n.GetLikelySources().ElementAt(i).GetMethod().Equals("ctor")) {
                    string output2 = (i+1) + " - " + n.GetLikelySources().ElementAt(i).GetClass() +
                        ", " + n.GetLikelySources().ElementAt(i).GetLikelyCause();
                    listBoxProblematicMethods.Items.Add(output2);
                }
                else
                {
                    string output3 = (i+1) + " - " + n.GetLikelySources().ElementAt(i).GetMethod() +
                        ", " + n.GetLikelySources().ElementAt(i).GetLikelyCause();
                    listBoxProblematicMethods.Items.Add(output3);
                }
            }
            listBoxNonconformances.SelectionMode = SelectionMode.One;
        }

        private void InitializeNonconformances(HashSet<Nonconformance> nonconformances)
        {
            this.nonconformances = new HashSet<Nonconformance>();
            foreach(Nonconformance n in nonconformances)
            {
                this.nonconformances.Add(n);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Nonconformance n = nonconformances.ElementAt(listBoxNonconformances.SelectedIndex);
            tbTextSample.Text = CodeReader.GetTestMethod(n.GetTestFileName());
            this.nodeNCNamespace.Text = n.GetNameSpace();
            this.nodeNCClass.Text = n.GetClassName();
            if(n.GetMethodName().Contains("ctor"))
                this.nodeNCMethod.Text = n.GetClassName();
            else
                this.nodeNCMethod.Text = n.GetMethodName();
            lbSetLikelySource.Text = n.GetLikelyCause();

            InitializeListProblematicMethods(n);

            this.HasAnyIndexSelected = true;
            btStackTrace.Visible = true;
        }

        private void btSaveResults_Click(object sender, EventArgs e)
        {
            if (saveResultsBrowser.ShowDialog() == DialogResult.OK)
            {
                try { 
                    // Select the folder of destination.
                    String destinationFolder = saveResultsBrowser.SelectedPath;

                    // Currently, select files of results folder to Copy.
                    string[] files = System.IO.Directory.GetFiles(Constants.TEST_RESULTS);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string f in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(f);
                        string destFile = System.IO.Path.Combine(destinationFolder, fileName);
                        System.IO.File.Copy(f, destFile, true);
                    }

                    MessageBox.Show("The files indicating Categorization result were saved correctly.");
                }
                catch (Exception excep)
                {
                    Console.WriteLine(excep.Message);
                    MessageBox.Show("The files indicating Categorization result couldn't be saved.");
                }
            }
        }

        private void btStackTrace_Click(object sender, EventArgs e)
        {
            if (HasAnyIndexSelected)
            {
                string toShow = "Stack Trace of Nonconformance: \n\n   ";
                foreach (var line in nonconformances.ElementAt(listBoxNonconformances.SelectedIndex).GetStackTrace())
                {
                    toShow += line + "\n";
                }
                MessageBox.Show(toShow);
            }
        }

        private void listBoxProblematicMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            Nonconformance n = nonconformances.ElementAt(listBoxNonconformances.SelectedIndex);
            this.nodePMNamespace.Text = n.GetLikelySources().ElementAt(listBoxProblematicMethods.SelectedIndex).GetNamespace();
            this.nodePMClass.Text = n.GetLikelySources().ElementAt(listBoxProblematicMethods.SelectedIndex).GetClass();
            if (n.GetLikelySources().ElementAt(listBoxProblematicMethods.SelectedIndex).GetLikelyCause().Equals("Strong Invariant") || n.GetLikelySources().ElementAt(listBoxProblematicMethods.SelectedIndex).GetMethod().Equals("ctor"))
            {
                this.nodePMMethod.Text = n.GetLikelySources().ElementAt(listBoxProblematicMethods.SelectedIndex).GetClass();
            }
            else
            {
                this.nodePMMethod.Text = n.GetLikelySources().ElementAt(listBoxProblematicMethods.SelectedIndex).GetMethod();
            }
            lbSetLikelySource.Text = n.GetLikelySources().ElementAt(listBoxProblematicMethods.SelectedIndex).GetLikelyCause();
            Double percentNumber = n.GetLikelySources().ElementAt(listBoxProblematicMethods.SelectedIndex).GetPercent();
            string percentString = (Math.Round(percentNumber, 2)).ToString();
            lbSetProbability.Text = percentString + "%";
        }

        private void lbFixed04_Click(object sender, EventArgs e)
        {

        }

        private void labelProblematicMethodLocation_Click(object sender, EventArgs e)
        {
            
        }
    }
}
