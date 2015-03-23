using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Structures;

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

        private void DetectedDisplay_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }
    }
}
