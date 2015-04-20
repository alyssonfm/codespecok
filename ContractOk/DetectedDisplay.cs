using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Structures;
using Commons;

namespace ContractOK
{
    public partial class DetectedDisplay : Form
    {
        private HashSet<Nonconformance> nonconformances;
        private TreeNode nodeNamespace;
        private TreeNode nodeClass;
        private TreeNode nodeMethod;
        private bool HasAnyIndexSelected = false;

        public DetectedDisplay(HashSet<Nonconformance> nonconformance)
        {
            InitializeComponent();

            nonconformances = nonconformance;

            lbSetNumberNonconformances.Text = nonconformances.Count + "";
            for (int i = 0; i < nonconformances.Count; i++)
            {
                listBox.Items.Add(i + " - " + nonconformances.ElementAt(i).GetContractType());
            }
            listBox.SelectionMode = SelectionMode.One;

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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Nonconformance n = nonconformances.ElementAt(listBox.SelectedIndex);
            tbTextSample.Text = CodeReader.GetTestMethod(n.GetTestFileName());
            this.nodeNamespace.Text = n.GetNameSpace();
            this.nodeClass.Text = n.GetClassName();
            this.nodeMethod.Text = n.GetMethodName();

            this.HasAnyIndexSelected = true;
            btStackTrace.Visible = true;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private void btCategorize_Click(object sender, EventArgs e)
        {
            Controller.StartCategorizationPhase();
        }
        private void DetectedDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            Controller.MakeMainVisibleAgain();
            this.Visible = false;
        }

        private void btStackTrace_Click(object sender, EventArgs e)
        {
            if (HasAnyIndexSelected)
            {
                string toShow = "Stack Trace of Nonconformance: \n\n   ";
                foreach (var line in nonconformances.ElementAt(listBox.SelectedIndex).GetStackTrace())
                {
                    toShow += line + "\n";
                }
                MessageBox.Show(toShow);
            }
        }

        private void btSaveResults_Click(object sender, EventArgs e)
        {
            if (saveResultsBrowser.ShowDialog() == DialogResult.OK)
            {
                // Select the folder of destination.
                String destinationFolder = saveResultsBrowser.SelectedPath;

                try
                {
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

                    MessageBox.Show("The files indicating Detection result were saved correctly.");
                }
                catch (Exception excep)
                {
                    Console.WriteLine(excep.Message);
                    MessageBox.Show("The files indicating Detection result couldn't be saved.");
                }
            }

        }
    }
}
