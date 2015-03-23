namespace ContractOK
{
    partial class DetectedDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Method");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Class", new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Namespace", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.lbFixed01 = new System.Windows.Forms.Label();
            this.lbSetNumberNonconformances = new System.Windows.Forms.Label();
            this.lbFixed02 = new System.Windows.Forms.Label();
            this.lbFixed03 = new System.Windows.Forms.Label();
            this.listBox = new System.Windows.Forms.ListBox();
            this.lbFixed04 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tbTextSample = new System.Windows.Forms.TextBox();
            this.lbFixed05 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbFixed01
            // 
            this.lbFixed01.AutoSize = true;
            this.lbFixed01.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbFixed01.Location = new System.Drawing.Point(17, 13);
            this.lbFixed01.Name = "lbFixed01";
            this.lbFixed01.Size = new System.Drawing.Size(114, 20);
            this.lbFixed01.TabIndex = 0;
            this.lbFixed01.Text = "Were detected";
            // 
            // lbSetNumberNonconformances
            // 
            this.lbSetNumberNonconformances.AutoSize = true;
            this.lbSetNumberNonconformances.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbSetNumberNonconformances.Location = new System.Drawing.Point(137, 13);
            this.lbSetNumberNonconformances.Name = "lbSetNumberNonconformances";
            this.lbSetNumberNonconformances.Size = new System.Drawing.Size(18, 20);
            this.lbSetNumberNonconformances.TabIndex = 1;
            this.lbSetNumberNonconformances.Text = "0";
            // 
            // lbFixed02
            // 
            this.lbFixed02.AutoSize = true;
            this.lbFixed02.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbFixed02.Location = new System.Drawing.Point(161, 13);
            this.lbFixed02.Name = "lbFixed02";
            this.lbFixed02.Size = new System.Drawing.Size(141, 20);
            this.lbFixed02.TabIndex = 2;
            this.lbFixed02.Text = "nonconformances.";
            // 
            // lbFixed03
            // 
            this.lbFixed03.AutoSize = true;
            this.lbFixed03.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbFixed03.Location = new System.Drawing.Point(43, 43);
            this.lbFixed03.Name = "lbFixed03";
            this.lbFixed03.Size = new System.Drawing.Size(139, 20);
            this.lbFixed03.TabIndex = 3;
            this.lbFixed03.Text = "Nonconformances";
            // 
            // listBox
            // 
            this.listBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 20;
            this.listBox.Location = new System.Drawing.Point(18, 67);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(192, 264);
            this.listBox.TabIndex = 4;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // lbFixed04
            // 
            this.lbFixed04.AutoSize = true;
            this.lbFixed04.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbFixed04.Location = new System.Drawing.Point(247, 43);
            this.lbFixed04.Name = "lbFixed04";
            this.lbFixed04.Size = new System.Drawing.Size(70, 20);
            this.lbFixed04.TabIndex = 5;
            this.lbFixed04.Text = "Location";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(230, 67);
            this.treeView1.Name = "treeView1";
            treeNode4.Name = "SetMethod";
            treeNode4.Text = "Method";
            treeNode5.Name = "SetClass";
            treeNode5.Text = "Class";
            treeNode6.Name = "SetNamespace";
            treeNode6.Text = "Namespace";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.treeView1.Size = new System.Drawing.Size(182, 142);
            this.treeView1.TabIndex = 6;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // tbTextSample
            // 
            this.tbTextSample.Location = new System.Drawing.Point(437, 67);
            this.tbTextSample.Multiline = true;
            this.tbTextSample.Name = "tbTextSample";
            this.tbTextSample.Size = new System.Drawing.Size(355, 260);
            this.tbTextSample.TabIndex = 7;
            // 
            // lbFixed05
            // 
            this.lbFixed05.AutoSize = true;
            this.lbFixed05.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbFixed05.Location = new System.Drawing.Point(452, 43);
            this.lbFixed05.Name = "lbFixed05";
            this.lbFixed05.Size = new System.Drawing.Size(98, 20);
            this.lbFixed05.TabIndex = 8;
            this.lbFixed05.Text = "Test Sample";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(37, 356);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 32);
            this.button1.TabIndex = 9;
            this.button1.Text = "Save Results";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(256, 356);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 31);
            this.button2.TabIndex = 10;
            this.button2.Text = "StackTrace";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(541, 356);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(150, 32);
            this.button3.TabIndex = 12;
            this.button3.Text = "Categorize";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // DetectedDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 410);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbFixed05);
            this.Controls.Add(this.tbTextSample);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.lbFixed04);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.lbFixed03);
            this.Controls.Add(this.lbFixed02);
            this.Controls.Add(this.lbSetNumberNonconformances);
            this.Controls.Add(this.lbFixed01);
            this.Name = "DetectedDisplay";
            this.Text = "Detection Screen";
            this.Load += new System.EventHandler(this.DetectedDisplay_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbFixed01;
        private System.Windows.Forms.Label lbSetNumberNonconformances;
        private System.Windows.Forms.Label lbFixed02;
        private System.Windows.Forms.Label lbFixed03;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label lbFixed04;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox tbTextSample;
        private System.Windows.Forms.Label lbFixed05;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}