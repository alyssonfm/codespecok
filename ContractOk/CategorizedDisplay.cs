using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Structures;
using Commons;

namespace ContractOK
{
    public partial class CategorizedDisplay : Form
    {
        private HashSet<Nonconformance> nonconformances;
        private TreeNode nodeNamespace;
        private TreeNode nodeClass;
        private TreeNode nodeMethod;

        public CategorizedDisplay(HashSet<Nonconformance> nonconformance)
        {
            InitializeComponent();

            nonconformances = nonconformance;

            lbSetNumberNonconformances.Text = nonconformances.Count + "";
            for (int i = 0; i < nonconformances.Count; i++)
            {
                listBox.Items.Add(i + " - " + nonconformances.ElementAt(i).GetContractType());
            }
            listBox.SelectionMode = SelectionMode.One;

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
                lbSetLikelyCause.Text = n.GetLikelyCause();
         
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
    }
}
