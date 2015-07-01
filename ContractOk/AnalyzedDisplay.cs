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
        private HashSet<Nonconformance> invariants;
        private TreeNode nodeNamespace;
        private TreeNode nodeClass;
        private TreeNode nodeMethod;
        private bool HasAnyIndexSelected = false;
        private int numberMethods = 0;

        public AnalyzedDisplay(HashSet<Nonconformance> nonconformance)
        {
            InitializeComponent();
            InitializeInvariants(nonconformances);
            InitializeListNonconformances();

            btStackTrace.Visible = false;

            TreeNodeCollection nodes = treeView1.Nodes;
            this.nodeNamespace = treeView1.Nodes[0];
            this.nodeClass = this.nodeNamespace.Nodes[0];
            this.nodeMethod = this.nodeClass.Nodes[0];

            this.Closing += (object sender, CancelEventArgs e) =>
            {
                Controller.MakeMainVisibleAgain();
            };

            this.Show();
        }

        private void InitializeListNonconformances()
        {
            lbSetNumberNonconformances.Text = invariants.Count + "";
            for (int i = 0; i < invariants.Count; i++)
            {
                listBoxNonconformances.Items.Add(i + " - " + invariants.ElementAt(i).GetContractType());
            }
            listBoxNonconformances.SelectionMode = SelectionMode.One;
        }
        private void InitializeListProblematicMethods()
        {
            numberMethods = 0;
            if(invariants.ElementAt(listBoxNonconformances.SelectedIndex).GetLikelySources().Count > 0)
            {
                numberMethods = invariants.ElementAt(listBoxNonconformances.SelectedIndex).GetLikelySources().Count;
            }
            for (int i = 0; i < numberMethods; i++)
            {
                listBoxProblematicMethods.Items.Add(i + " - " + invariants.ElementAt(listBoxNonconformances.SelectedIndex).GetLikelySources().ElementAt(i).GetMethod() + 
                ", "   + invariants.ElementAt(listBoxProblematicMethods.SelectedIndex).GetLikelySources().ElementAt(i).GetLikelyCause());
            }
            listBoxNonconformances.SelectionMode = SelectionMode.One;
        }

        private void InitializeInvariants(HashSet<Nonconformance> nonconformances)
        {
            this.invariants = new HashSet<Nonconformance>();
            foreach(Nonconformance n in nonconformances)
            {
                if (n.GetContractType().Equals("invariant")) {
                    invariants.Add(n);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Nonconformance n = nonconformances.ElementAt(listBoxNonconformances.SelectedIndex);
            tbTextSample.Text = CodeReader.GetTestMethod(n.GetTestFileName());
            this.nodeNamespace.Text = n.GetNameSpace();
            this.nodeClass.Text = n.GetClassName();
            this.nodeMethod.Text = n.GetMethodName();
            lbSetLikelyCause.Text = n.GetLikelyCause();

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
    }
}
