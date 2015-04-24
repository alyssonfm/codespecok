using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using DetectModule;

namespace ContractOK
{
    public delegate void ProgressEvent();
    public partial class DetectConsole : Form
    {
        private Detect _detect;
        private bool _detectionSuceeded;
        private bool _pauseProgressBar = false;
        private int _steps = 0;
        private double _velocity = 2;

        private const double _SECONDS = 1000;

        public DetectConsole(Detect d)
        {
            this._detect = d;
            InitializeComponent();

            backgroundWorkerProgressBar.WorkerReportsProgress = true;
            backgroundWorkerProgressBar.WorkerSupportsCancellation = false;

            lbStage.Text = "Current Stage: "+ "Creating Directories";
            progressBar.Value = 0;
            progressBar.Step = 1;
            progressBar.Maximum = 100;
            textArea.Text = "";

            btCategorize.Visible = false;
            btViewNonconformances.Visible = false;

            this._detect.RegisterEvents(DirectoriesCreated, ProjectCompiled, TestsGenerated, TestsExecuted, ErrorDetected);

            this.Closing += (object sender, CancelEventArgs e) =>
            {
                Controller.MakeMainVisibleAgain();
            };

            if (backgroundWorkerProgressBar.IsBusy != true)
            {
                backgroundWorkerProgressBar.RunWorkerAsync();
            }

            this.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Controller.ShowDetectDisplay();
        }

        private void DirectoriesCreated(String text)
        {   
            RestartProgress(20, text);
            lbStage.Text = "Current Stage: " + "Compiling Project";
        }
        private void ProjectCompiled(String text)
        {
            RestartProgress(60, text);
            lbStage.Text = "Current Stage: " + "Generating Tests";
        }
        private void TestsGenerated(String text)
        {
            RestartProgress(80, text);
            lbStage.Text = "Current Stage: " + "Executing Tests";
        }
        private void TestsExecuted(String text)
        {
            if (backgroundWorkerProgressBar.WorkerSupportsCancellation == true)
            {
                backgroundWorkerProgressBar.CancelAsync();
            }

            RestartProgress(100, text);
            this._detectionSuceeded = true;
            lbStage.Text = "Detection Phase finished.";

            ModifyButton();
        }
        private void ErrorDetected(String text)
        {
            if (backgroundWorkerProgressBar.WorkerSupportsCancellation == true)
            {
                backgroundWorkerProgressBar.CancelAsync();
            }

            textArea.Text += text;
            this._detectionSuceeded = false;
            lbStage.Text = "Detection Phase finished with errors.";

            ModifyButton();
        }
        private void ModifyButton()
        {
            if (this._detectionSuceeded == false)
            {
                btCategorize.Text = "Exit";
                btCategorize.Visible = true;
            }
            else
            {
                btCategorize.Visible = true;
                btViewNonconformances.Visible = true;
            }
        }
        public void StopProgress()
        {
            this._pauseProgressBar = true;
        }
        public void RestartProgress(int progressNumber, string text)
        {
            StopProgress();
            textArea.Text += text;
            textArea.Invalidate();
            textArea.Update();
            textArea.Refresh();
            progressBar.Value = progressNumber;
            StartProgress();
        }
        private void StartProgress()
        {
            progressBar.Invalidate();
            progressBar.Update();
            progressBar.Refresh();
            Application.DoEvents();
            this._velocity = 2;
            this._steps = 0;
            this._pauseProgressBar = false;
        }
        private void backgroundWorkerProgressBar_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            while (true)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }else if(this._pauseProgressBar == true){
                    ;
                }
                else if (this._steps < 20)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(_SECONDS / this._velocity));
                    IncrementBar();
                }
            }
        }

        private void IncrementBar()
        {
            if(this.progressBar.InvokeRequired){
                ProgressEvent p = new ProgressEvent(IncrementBar);
                this.Invoke(p, new object[] { });
            }else{
                progressBar.PerformStep();
                progressBar.Invalidate();
                progressBar.Update();
                progressBar.Refresh();
                Application.DoEvents();
                this._velocity /= 2;
                this._steps++;
            }
        }

        private void btCategorize_Click(object sender, EventArgs e)
        {
            if (this._detectionSuceeded)
            {
                Controller.StartCategorizationPhase();
            }
            else
            {
                this.Visible = false;
                Controller.MakeMainVisibleAgain();
            }
        }

        private void textArea_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
