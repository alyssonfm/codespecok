using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using DetectModule;

namespace CodeSpecOK
{
    public partial class DetectConsole : Form
    {
        private Detect _detect;
        private Worker _worker;
        private bool _detectionSuceeded;
        public DetectConsole(Detect d)
        {
            this._detect = d;
            InitializeComponent();
            lbStage.Text = "Current Stage: "+ "Creating Directories";
            progressBar.Value = 0;
            progressBar.Text = "0%";
            textArea.Text = "";

            this._detect.RegisterEvents(DirectoriesCreated, ProjectCompiled, TestsGenerated, TestsExecuted, ErrorDetected);

            //this._worker = new Worker(2, progressBar);
            //Thread workThread = new Thread(this._worker.DoWork);
            //workThread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void DirectoriesCreated(String text)
        {
            textArea.Text += text;
            RestartProgress(20);
            lbStage.Text = "Current Stage: " + "Compiling Project";
        }
        private void ProjectCompiled(String text)
        {
            textArea.Text += text;
            RestartProgress(60);
            lbStage.Text = "Current Stage: " + "Generating Tests";
        }
        private void TestsGenerated(String text)
        {
            textArea.Text += text;
            RestartProgress(80);
            lbStage.Text = "Current Stage: " + "Executing Tests";
        }
        private void TestsExecuted(String text)
        {
            textArea.Text += text;
            RestartProgress(100);
            this._detectionSuceeded = true;
            lbStage.Text = "Detection Phase finished.";

            ModifyButton();
        }
        private void ErrorDetected(String text)
        {
            textArea.Text += text;
            this._detectionSuceeded = false;
            lbStage.Text = "Detection Phase finished.";

            ModifyButton();
        }
        private void ModifyButton()
        {

        }
        public void RestartProgress(int progressNumber)
        {
            progressBar.Value = progressNumber;
        }

        public class Worker{
            private volatile bool _shouldStop = false;
            private volatile bool _shouldWait = false;
            private volatile ProgressBar _bar;
            private volatile int _velocity;
            private volatile int _steps = 0;

            public Worker(int vel, ProgressBar bar){
                this._velocity = vel;
                this._bar = bar;
            }
            public void DoWork(){
                while(!this._shouldStop){
                    if(this._shouldWait){
                        ;
                    }else if(this._steps < 20){
                        Thread.Sleep(TimeSpan.FromMilliseconds(1000 / this._velocity));
                        this._bar.Increment(1);
                        this._bar.Text = this._bar.Value + "%";
                        this._velocity /= 2;
                        this._steps++;
                    }
                }
            }
            public void restartProgress(int progressNumber){
                stopProgress();
                this._bar.Value = progressNumber;
                this._shouldWait = false;
            }
            public void stopProgress(){
                this._shouldWait = true;
            }
            public void stopWork(){
                this._shouldStop = true;
            }

        }

    }
}
