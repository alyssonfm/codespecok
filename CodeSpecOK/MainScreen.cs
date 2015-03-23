using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContractOK
{
    public partial class MainScreen : Form
    {
        private String _srcFolder;
        private String _libFolder;

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
        }

        private void btBrSrc_Click(object sender, EventArgs e)
        {
            if (folderBrowserSrcDialog.ShowDialog() == DialogResult.OK)
            {
                String text = folderBrowserSrcDialog.SelectedPath;
                this._srcFolder = text;
                this.lbSetSrc.Text = text.Substring(text.LastIndexOf('\\') + 1);
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
            Controller.StartDetectPhase(this._srcFolder, this._libFolder, this.tbSeconds.Text);
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {

        }

        private void MainScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
