namespace ContractOK
{
    partial class DetectConsole
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
            this.textArea = new System.Windows.Forms.TextBox();
            this.lbStage = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.btViewNonconformances = new System.Windows.Forms.Button();
            this.btCategorize = new System.Windows.Forms.Button();
            this.backgroundWorkerProgressBar = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // textArea
            // 
            this.textArea.Location = new System.Drawing.Point(31, 58);
            this.textArea.Multiline = true;
            this.textArea.Name = "textArea";
            this.textArea.ReadOnly = true;
            this.textArea.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textArea.Size = new System.Drawing.Size(860, 314);
            this.textArea.TabIndex = 0;
            this.textArea.TextChanged += new System.EventHandler(this.textArea_TextChanged);
            // 
            // lbStage
            // 
            this.lbStage.AutoSize = true;
            this.lbStage.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbStage.Location = new System.Drawing.Point(40, 21);
            this.lbStage.Name = "lbStage";
            this.lbStage.Size = new System.Drawing.Size(127, 20);
            this.lbStage.TabIndex = 1;
            this.lbStage.Text = "Detection Phase";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(34, 386);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(856, 26);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 2;
            // 
            // btViewNonconformances
            // 
            this.btViewNonconformances.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btViewNonconformances.Location = new System.Drawing.Point(404, 16);
            this.btViewNonconformances.Name = "btViewNonconformances";
            this.btViewNonconformances.Size = new System.Drawing.Size(200, 31);
            this.btViewNonconformances.TabIndex = 3;
            this.btViewNonconformances.Text = "View Nonconformances";
            this.btViewNonconformances.UseVisualStyleBackColor = true;
            this.btViewNonconformances.Click += new System.EventHandler(this.button1_Click);
            // 
            // btCategorize
            // 
            this.btCategorize.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btCategorize.Location = new System.Drawing.Point(635, 16);
            this.btCategorize.Name = "btCategorize";
            this.btCategorize.Size = new System.Drawing.Size(237, 31);
            this.btCategorize.TabIndex = 4;
            this.btCategorize.Text = "Categorize";
            this.btCategorize.UseVisualStyleBackColor = true;
            this.btCategorize.Click += new System.EventHandler(this.btCategorize_Click);
            // 
            // backgroundWorkerProgressBar
            // 
            this.backgroundWorkerProgressBar.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerProgressBar_DoWork);
            // 
            // DetectConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 430);
            this.Controls.Add(this.btCategorize);
            this.Controls.Add(this.btViewNonconformances);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lbStage);
            this.Controls.Add(this.textArea);
            this.Name = "DetectConsole";
            this.Text = "Detection Screen";
            this.Load += new System.EventHandler(this.DetectConsole_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textArea;
        private System.Windows.Forms.Label lbStage;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btViewNonconformances;
        private System.Windows.Forms.Button btCategorize;
        private System.ComponentModel.BackgroundWorker backgroundWorkerProgressBar;
    }
}