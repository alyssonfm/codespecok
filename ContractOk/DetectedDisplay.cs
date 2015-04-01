using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Structures;

namespace ContractOK
{
    public partial class DetectedDisplay : Form
    {
        private HashSet<Nonconformance> nonconformances;
        private TreeNode nodeNamespace;
        private TreeNode nodeClass;
        private TreeNode nodeMethod;

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
    }
}
