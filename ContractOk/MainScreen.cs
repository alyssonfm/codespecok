using System;
using System.Windows.Forms;

namespace ContractOK
{
    public partial class MainScreen : Form
    {
        private String _srcFolder;
        private String _solutionFile;
        private String _libFolder;
        private String _time;

        public MainScreen()
        {
            InitializeComponent();
        }

        private void btClean_Click(object sender, EventArgs e)
        {
            this.lbSetSrc.Text = "";
            this.lbSetLib.Text = "";
            this.tbSeconds.Text = "";
            _srcFolder = "";
            _libFolder = "";
            _time = "";
        }

        private void btBrSrc_Click(object sender, EventArgs e)
        {
            if (solutionFileBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                String path = solutionFileBrowserDialog.FileName;
                String solution = solutionFileBrowserDialog.SafeFileName;
                this._srcFolder = path.Substring(0, path.IndexOf("\\" + solution));
                this._solutionFile = solution;
                this.lbSetSrc.Text = solution;
            }
        }

        private void btBrLib_Click(object sender, EventArgs e)
        {
            if (folderBrowserLibDialog.ShowDialog() == DialogResult.OK)
            {
                String text = folderBrowserLibDialog.SelectedPath;
                this._libFolder = text;
                this.lbSetLib.Text = text.Substring(text.LastIndexOf('\\') + 1);
            }
        }

        private void btRun_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            if (this._libFolder == null) { this._libFolder = ""; }
            if(this.tbSeconds.Text == "")
            {
                this._time = "10";
            } else
            {
                this._time = this.tbSeconds.Text;
            }
            if (!Controller.checkProblemsWithInput(this._srcFolder, this._solutionFile, this._time))
            {
                DialogResult result = MessageBox.Show("Please select at least the solution file before running...", "Important Note", MessageBoxButtons.OKCancel);
                if(result == DialogResult.OK)
                {
                    System.Windows.Forms.Application.Restart();
                }
                
            }
            else
            {
                Controller.StartDetectPhase(this._srcFolder, this._solutionFile, this._libFolder, this._time);
            }
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {

        }

        private void folderBrowserLibDialog_HelpRequest(object sender, EventArgs e)
        {

        }
    }
}
