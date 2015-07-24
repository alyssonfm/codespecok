namespace ContractOK
{
    partial class AnalyzedDisplay
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Method");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Class", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Namespace", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Method");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Class", new System.Windows.Forms.TreeNode[] {
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Namespace", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            this.lbFixed01 = new System.Windows.Forms.Label();
            this.lbSetNumberNonconformances = new System.Windows.Forms.Label();
            this.lbFixed02 = new System.Windows.Forms.Label();
            this.lbFixed03 = new System.Windows.Forms.Label();
            this.listBoxNonconformances = new System.Windows.Forms.ListBox();
            this.lbFixed04 = new System.Windows.Forms.Label();
            this.tvNonconformanceLocation = new System.Windows.Forms.TreeView();
            this.tbTextSample = new System.Windows.Forms.TextBox();
            this.lbFixed05 = new System.Windows.Forms.Label();
            this.btSaveResults = new System.Windows.Forms.Button();
            this.btStackTrace = new System.Windows.Forms.Button();
            this.lbLikelySource = new System.Windows.Forms.Label();
            this.lbSetLikelySource = new System.Windows.Forms.Label();
            this.saveResultsBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.listBoxProblematicMethods = new System.Windows.Forms.ListBox();
            this.labelProblematicMethods = new System.Windows.Forms.Label();
            this.tvProblematicMethodLocation = new System.Windows.Forms.TreeView();
            this.labelProblematicMethodLocation = new System.Windows.Forms.Label();
            this.labelProbability = new System.Windows.Forms.Label();
            this.lbSetProbability = new System.Windows.Forms.Label();
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
            // listBoxNonconformances
            // 
            this.listBoxNonconformances.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxNonconformances.FormattingEnabled = true;
            this.listBoxNonconformances.Location = new System.Drawing.Point(18, 67);
            this.listBoxNonconformances.Name = "listBoxNonconformances";
            this.listBoxNonconformances.Size = new System.Drawing.Size(192, 108);
            this.listBoxNonconformances.TabIndex = 4;
            this.listBoxNonconformances.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // lbFixed04
            // 
            this.lbFixed04.AutoSize = true;
            this.lbFixed04.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbFixed04.Location = new System.Drawing.Point(247, 44);
            this.lbFixed04.Name = "lbFixed04";
            this.lbFixed04.Size = new System.Drawing.Size(196, 20);
            this.lbFixed04.TabIndex = 5;
            this.lbFixed04.Text = "Nonconformance Location";
            this.lbFixed04.Click += new System.EventHandler(this.lbFixed04_Click);
            // 
            // tvNonconformanceLocation
            // 
            this.tvNonconformanceLocation.Location = new System.Drawing.Point(230, 67);
            this.tvNonconformanceLocation.Name = "tvNonconformanceLocation";
            treeNode1.Name = "SetMethod";
            treeNode1.Text = "Method";
            treeNode2.Name = "SetClass";
            treeNode2.Text = "Class";
            treeNode3.Name = "SetNamespace";
            treeNode3.Text = "Namespace";
            this.tvNonconformanceLocation.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.tvNonconformanceLocation.Size = new System.Drawing.Size(257, 108);
            this.tvNonconformanceLocation.TabIndex = 6;
            // 
            // tbTextSample
            // 
            this.tbTextSample.Location = new System.Drawing.Point(515, 67);
            this.tbTextSample.Multiline = true;
            this.tbTextSample.Name = "tbTextSample";
            this.tbTextSample.Size = new System.Drawing.Size(396, 137);
            this.tbTextSample.TabIndex = 7;
            // 
            // lbFixed05
            // 
            this.lbFixed05.AutoSize = true;
            this.lbFixed05.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbFixed05.Location = new System.Drawing.Point(536, 43);
            this.lbFixed05.Name = "lbFixed05";
            this.lbFixed05.Size = new System.Drawing.Size(98, 20);
            this.lbFixed05.TabIndex = 8;
            this.lbFixed05.Text = "Test Sample";
            // 
            // btSaveResults
            // 
            this.btSaveResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btSaveResults.Location = new System.Drawing.Point(37, 332);
            this.btSaveResults.Name = "btSaveResults";
            this.btSaveResults.Size = new System.Drawing.Size(144, 32);
            this.btSaveResults.TabIndex = 9;
            this.btSaveResults.Text = "Save Results";
            this.btSaveResults.UseVisualStyleBackColor = true;
            this.btSaveResults.Click += new System.EventHandler(this.btSaveResults_Click);
            // 
            // btStackTrace
            // 
            this.btStackTrace.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btStackTrace.Location = new System.Drawing.Point(256, 332);
            this.btStackTrace.Name = "btStackTrace";
            this.btStackTrace.Size = new System.Drawing.Size(134, 31);
            this.btStackTrace.TabIndex = 10;
            this.btStackTrace.Text = "StackTrace";
            this.btStackTrace.UseVisualStyleBackColor = true;
            this.btStackTrace.Click += new System.EventHandler(this.btStackTrace_Click);
            // 
            // lbLikelySource
            // 
            this.lbLikelySource.AutoSize = true;
            this.lbLikelySource.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbLikelySource.Location = new System.Drawing.Point(536, 216);
            this.lbLikelySource.Name = "lbLikelySource";
            this.lbLikelySource.Size = new System.Drawing.Size(103, 20);
            this.lbLikelySource.TabIndex = 12;
            this.lbLikelySource.Text = "Likely Source";
            // 
            // lbSetLikelySource
            // 
            this.lbSetLikelySource.AutoSize = true;
            this.lbSetLikelySource.Location = new System.Drawing.Point(537, 247);
            this.lbSetLikelySource.Name = "lbSetLikelySource";
            this.lbSetLikelySource.Size = new System.Drawing.Size(0, 13);
            this.lbSetLikelySource.TabIndex = 13;
            // 
            // listBoxProblematicMethods
            // 
            this.listBoxProblematicMethods.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxProblematicMethods.FormattingEnabled = true;
            this.listBoxProblematicMethods.Location = new System.Drawing.Point(18, 207);
            this.listBoxProblematicMethods.Name = "listBoxProblematicMethods";
            this.listBoxProblematicMethods.Size = new System.Drawing.Size(192, 108);
            this.listBoxProblematicMethods.TabIndex = 14;
            this.listBoxProblematicMethods.SelectedIndexChanged += new System.EventHandler(this.listBoxProblematicMethods_SelectedIndexChanged);
            // 
            // labelProblematicMethods
            // 
            this.labelProblematicMethods.AutoSize = true;
            this.labelProblematicMethods.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.labelProblematicMethods.Location = new System.Drawing.Point(33, 184);
            this.labelProblematicMethods.Name = "labelProblematicMethods";
            this.labelProblematicMethods.Size = new System.Drawing.Size(158, 20);
            this.labelProblematicMethods.TabIndex = 15;
            this.labelProblematicMethods.Text = "Problematic methods";
            // 
            // tvProblematicMethodLocation
            // 
            this.tvProblematicMethodLocation.Location = new System.Drawing.Point(230, 207);
            this.tvProblematicMethodLocation.Name = "tvProblematicMethodLocation";
            treeNode4.Name = "SetMethod";
            treeNode4.Text = "Method";
            treeNode5.Name = "SetClass";
            treeNode5.Text = "Class";
            treeNode6.Name = "SetNamespace";
            treeNode6.Text = "Namespace";
            this.tvProblematicMethodLocation.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.tvProblematicMethodLocation.Size = new System.Drawing.Size(257, 108);
            this.tvProblematicMethodLocation.TabIndex = 17;
            // 
            // labelProblematicMethodLocation
            // 
            this.labelProblematicMethodLocation.AutoSize = true;
            this.labelProblematicMethodLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.labelProblematicMethodLocation.Location = new System.Drawing.Point(247, 183);
            this.labelProblematicMethodLocation.Name = "labelProblematicMethodLocation";
            this.labelProblematicMethodLocation.Size = new System.Drawing.Size(215, 20);
            this.labelProblematicMethodLocation.TabIndex = 16;
            this.labelProblematicMethodLocation.Text = "Problematic Method Location";
            this.labelProblematicMethodLocation.Click += new System.EventHandler(this.labelProblematicMethodLocation_Click);
            // 
            // labelProbability
            // 
            this.labelProbability.AutoSize = true;
            this.labelProbability.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.labelProbability.Location = new System.Drawing.Point(536, 267);
            this.labelProbability.Name = "labelProbability";
            this.labelProbability.Size = new System.Drawing.Size(80, 20);
            this.labelProbability.TabIndex = 18;
            this.labelProbability.Text = "Likelihood";
            // 
            // lbSetProbability
            // 
            this.lbSetProbability.AutoSize = true;
            this.lbSetProbability.Location = new System.Drawing.Point(537, 296);
            this.lbSetProbability.Name = "lbSetProbability";
            this.lbSetProbability.Size = new System.Drawing.Size(0, 13);
            this.lbSetProbability.TabIndex = 19;
            // 
            // AnalyzedDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 381);
            this.Controls.Add(this.lbSetProbability);
            this.Controls.Add(this.labelProbability);
            this.Controls.Add(this.tvProblematicMethodLocation);
            this.Controls.Add(this.labelProblematicMethodLocation);
            this.Controls.Add(this.labelProblematicMethods);
            this.Controls.Add(this.listBoxProblematicMethods);
            this.Controls.Add(this.lbSetLikelySource);
            this.Controls.Add(this.lbLikelySource);
            this.Controls.Add(this.btStackTrace);
            this.Controls.Add(this.btSaveResults);
            this.Controls.Add(this.lbFixed05);
            this.Controls.Add(this.tbTextSample);
            this.Controls.Add(this.tvNonconformanceLocation);
            this.Controls.Add(this.lbFixed04);
            this.Controls.Add(this.listBoxNonconformances);
            this.Controls.Add(this.lbFixed03);
            this.Controls.Add(this.lbFixed02);
            this.Controls.Add(this.lbSetNumberNonconformances);
            this.Controls.Add(this.lbFixed01);
            this.MaximizeBox = false;
            this.Name = "AnalyzedDisplay";
            this.Text = "Categorization Screen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbFixed01;
        private System.Windows.Forms.Label lbSetNumberNonconformances;
        private System.Windows.Forms.Label lbFixed02;
        private System.Windows.Forms.Label lbFixed03;
        private System.Windows.Forms.ListBox listBoxNonconformances;
        private System.Windows.Forms.Label lbFixed04;
        private System.Windows.Forms.TreeView tvNonconformanceLocation;
        private System.Windows.Forms.TextBox tbTextSample;
        private System.Windows.Forms.Label lbFixed05;
        private System.Windows.Forms.Button btSaveResults;
        private System.Windows.Forms.Button btStackTrace;
        private System.Windows.Forms.Label lbLikelySource;
        private System.Windows.Forms.Label lbSetLikelySource;
        private System.Windows.Forms.FolderBrowserDialog saveResultsBrowser;
        private System.Windows.Forms.ListBox listBoxProblematicMethods;
        private System.Windows.Forms.Label labelProblematicMethods;
        private System.Windows.Forms.TreeView tvProblematicMethodLocation;
        private System.Windows.Forms.Label labelProblematicMethodLocation;
        private System.Windows.Forms.Label labelProbability;
        private System.Windows.Forms.Label lbSetProbability;
    }
}