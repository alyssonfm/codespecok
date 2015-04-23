namespace ContractOK
{
    partial class MainScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainScreen));
            this.lbChooseSrc = new System.Windows.Forms.Label();
            this.btBrSrc = new System.Windows.Forms.Button();
            this.btBrLib = new System.Windows.Forms.Button();
            this.lbChooseLib = new System.Windows.Forms.Label();
            this.tbSeconds = new System.Windows.Forms.TextBox();
            this.lbSeconds = new System.Windows.Forms.Label();
            this.pb3 = new System.Windows.Forms.PictureBox();
            this.pb2 = new System.Windows.Forms.PictureBox();
            this.pb1 = new System.Windows.Forms.PictureBox();
            this.lbSecondsLit = new System.Windows.Forms.Label();
            this.lbSetLib = new System.Windows.Forms.Label();
            this.lbSetSrc = new System.Windows.Forms.Label();
            this.btClean = new System.Windows.Forms.Button();
            this.btRun = new System.Windows.Forms.Button();
            this.folderBrowserLibDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.solutionFileBrowserDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pb3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbChooseSrc
            // 
            this.lbChooseSrc.AutoSize = true;
            this.lbChooseSrc.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbChooseSrc.Location = new System.Drawing.Point(93, 26);
            this.lbChooseSrc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbChooseSrc.Name = "lbChooseSrc";
            this.lbChooseSrc.Size = new System.Drawing.Size(185, 20);
            this.lbChooseSrc.TabIndex = 0;
            this.lbChooseSrc.Text = "Choose project solution";
            // 
            // btBrSrc
            // 
            this.btBrSrc.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btBrSrc.Location = new System.Drawing.Point(335, 17);
            this.btBrSrc.Margin = new System.Windows.Forms.Padding(4);
            this.btBrSrc.Name = "btBrSrc";
            this.btBrSrc.Size = new System.Drawing.Size(124, 42);
            this.btBrSrc.TabIndex = 1;
            this.btBrSrc.Text = "Browse";
            this.btBrSrc.UseVisualStyleBackColor = true;
            this.btBrSrc.Click += new System.EventHandler(this.btBrSrc_Click);
            // 
            // btBrLib
            // 
            this.btBrLib.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btBrLib.Location = new System.Drawing.Point(335, 66);
            this.btBrLib.Margin = new System.Windows.Forms.Padding(4);
            this.btBrLib.Name = "btBrLib";
            this.btBrLib.Size = new System.Drawing.Size(124, 42);
            this.btBrLib.TabIndex = 2;
            this.btBrLib.Text = "Browse";
            this.btBrLib.UseVisualStyleBackColor = true;
            this.btBrLib.Click += new System.EventHandler(this.btBrLib_Click);
            // 
            // lbChooseLib
            // 
            this.lbChooseLib.AutoSize = true;
            this.lbChooseLib.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbChooseLib.Location = new System.Drawing.Point(25, 75);
            this.lbChooseLib.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbChooseLib.Name = "lbChooseLib";
            this.lbChooseLib.Size = new System.Drawing.Size(242, 20);
            this.lbChooseLib.TabIndex = 3;
            this.lbChooseLib.Text = "Choose external libraries folder";
            // 
            // tbSeconds
            // 
            this.tbSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.tbSeconds.Location = new System.Drawing.Point(335, 119);
            this.tbSeconds.Margin = new System.Windows.Forms.Padding(4);
            this.tbSeconds.Name = "tbSeconds";
            this.tbSeconds.Size = new System.Drawing.Size(123, 26);
            this.tbSeconds.TabIndex = 4;
            // 
            // lbSeconds
            // 
            this.lbSeconds.AutoSize = true;
            this.lbSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbSeconds.Location = new System.Drawing.Point(80, 123);
            this.lbSeconds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSeconds.Name = "lbSeconds";
            this.lbSeconds.Size = new System.Drawing.Size(196, 20);
            this.lbSeconds.TabIndex = 5;
            this.lbSeconds.Text = "Time for tests generation";
            // 
            // pb3
            // 
            this.pb3.Image = global::ContractOK.Properties.Resources.ClockIcon;
            this.pb3.InitialImage = ((System.Drawing.Image)(resources.GetObject("pb3.InitialImage")));
            this.pb3.Location = new System.Drawing.Point(467, 116);
            this.pb3.Margin = new System.Windows.Forms.Padding(4);
            this.pb3.Name = "pb3";
            this.pb3.Size = new System.Drawing.Size(43, 39);
            this.pb3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb3.TabIndex = 8;
            this.pb3.TabStop = false;
            // 
            // pb2
            // 
            this.pb2.Image = global::ContractOK.Properties.Resources.FolderIcon;
            this.pb2.InitialImage = ((System.Drawing.Image)(resources.GetObject("pb2.InitialImage")));
            this.pb2.Location = new System.Drawing.Point(467, 69);
            this.pb2.Margin = new System.Windows.Forms.Padding(4);
            this.pb2.Name = "pb2";
            this.pb2.Size = new System.Drawing.Size(43, 39);
            this.pb2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb2.TabIndex = 7;
            this.pb2.TabStop = false;
            // 
            // pb1
            // 
            this.pb1.Image = global::ContractOK.Properties.Resources.FolderIcon;
            this.pb1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pb1.InitialImage")));
            this.pb1.Location = new System.Drawing.Point(467, 20);
            this.pb1.Margin = new System.Windows.Forms.Padding(4);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(43, 39);
            this.pb1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb1.TabIndex = 6;
            this.pb1.TabStop = false;
            // 
            // lbSecondsLit
            // 
            this.lbSecondsLit.AutoSize = true;
            this.lbSecondsLit.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbSecondsLit.Location = new System.Drawing.Point(517, 123);
            this.lbSecondsLit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSecondsLit.Name = "lbSecondsLit";
            this.lbSecondsLit.Size = new System.Drawing.Size(72, 20);
            this.lbSecondsLit.TabIndex = 9;
            this.lbSecondsLit.Text = "seconds";
            // 
            // lbSetLib
            // 
            this.lbSetLib.AutoSize = true;
            this.lbSetLib.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbSetLib.Location = new System.Drawing.Point(517, 75);
            this.lbSetLib.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSetLib.Name = "lbSetLib";
            this.lbSetLib.Size = new System.Drawing.Size(0, 20);
            this.lbSetLib.TabIndex = 10;
            // 
            // lbSetSrc
            // 
            this.lbSetSrc.AutoSize = true;
            this.lbSetSrc.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbSetSrc.Location = new System.Drawing.Point(517, 26);
            this.lbSetSrc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSetSrc.Name = "lbSetSrc";
            this.lbSetSrc.Size = new System.Drawing.Size(0, 20);
            this.lbSetSrc.TabIndex = 11;
            // 
            // btClean
            // 
            this.btClean.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btClean.Location = new System.Drawing.Point(291, 162);
            this.btClean.Margin = new System.Windows.Forms.Padding(4);
            this.btClean.Name = "btClean";
            this.btClean.Size = new System.Drawing.Size(105, 32);
            this.btClean.TabIndex = 13;
            this.btClean.Text = "Clean";
            this.btClean.UseVisualStyleBackColor = true;
            this.btClean.Click += new System.EventHandler(this.btClean_Click);
            // 
            // btRun
            // 
            this.btRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btRun.Location = new System.Drawing.Point(404, 162);
            this.btRun.Margin = new System.Windows.Forms.Padding(4);
            this.btRun.Name = "btRun";
            this.btRun.Size = new System.Drawing.Size(105, 32);
            this.btRun.TabIndex = 14;
            this.btRun.Text = "Run";
            this.btRun.UseVisualStyleBackColor = true;
            this.btRun.Click += new System.EventHandler(this.btRun_Click);
            // 
            // solutionFileBrowserDialog
            // 
            this.solutionFileBrowserDialog.FileName = "solutionFileBrowserDialog";
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 203);
            this.Controls.Add(this.btRun);
            this.Controls.Add(this.btClean);
            this.Controls.Add(this.lbSetSrc);
            this.Controls.Add(this.lbSetLib);
            this.Controls.Add(this.lbSecondsLit);
            this.Controls.Add(this.pb3);
            this.Controls.Add(this.pb2);
            this.Controls.Add(this.pb1);
            this.Controls.Add(this.lbSeconds);
            this.Controls.Add(this.tbSeconds);
            this.Controls.Add(this.lbChooseLib);
            this.Controls.Add(this.btBrLib);
            this.Controls.Add(this.btBrSrc);
            this.Controls.Add(this.lbChooseSrc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainScreen";
            this.Text = "ContractOK";
            this.Load += new System.EventHandler(this.MainScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbChooseSrc;
        private System.Windows.Forms.Button btBrSrc;
        private System.Windows.Forms.Button btBrLib;
        private System.Windows.Forms.Label lbChooseLib;
        private System.Windows.Forms.TextBox tbSeconds;
        private System.Windows.Forms.Label lbSeconds;
        private System.Windows.Forms.PictureBox pb1;
        private System.Windows.Forms.PictureBox pb2;
        private System.Windows.Forms.PictureBox pb3;
        private System.Windows.Forms.Label lbSecondsLit;
        private System.Windows.Forms.Label lbSetLib;
        private System.Windows.Forms.Label lbSetSrc;
        private System.Windows.Forms.Button btClean;
        private System.Windows.Forms.Button btRun;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserLibDialog;
        private System.Windows.Forms.OpenFileDialog solutionFileBrowserDialog;
    }
}

