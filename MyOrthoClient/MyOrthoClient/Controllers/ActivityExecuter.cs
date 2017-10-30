using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyOrthoClient.Models;
using System.Windows;
using System.Windows.Threading;

namespace MyOrthoClient.Controllers
{
    class ActivityExecuter
    {
        private WAVPlayerRecorder Player;
        private ActivityVM CurrentActivity;
        private PraatScripting scripting;
        private PraatConnector connector;
        private SoundAnalyser analyser;
        private string lastExerciceWavPath;
        private string exerciceFolderPath;
        private string currentExercicePath;

        public ActivityExecuter(ActivityVM currentActivity)
        {
            this.Player = new WAVPlayerRecorder();
            this.CurrentActivity = currentActivity;
            this.scripting = new PraatScripting(currentActivity.Name);
            this.connector = PraatConnector.GetConnector();
            this.analyser = new SoundAnalyser();
            this.exerciceFolderPath = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\" + this.CurrentActivity.Name+ "\\";
            if (!Directory.Exists(this.exerciceFolderPath))
            {
                Directory.CreateDirectory(this.exerciceFolderPath);
            }
            currentExercicePath = this.exerciceFolderPath + DateTime.Now.ToString("yyyyMMddHHmmss");

            Task.Run(() => this.CurrentActivity.Exercice = CalculateIntensityAndFrequency(this.CurrentActivity.Example_wav_path));
        }

        public void StartPlayback(Action playDone)
        {
            Task.Run(() => Player.StartPlayback(this.CurrentActivity.Example_wav_path, playDone));
        }

        public void StopPlayback()
        {
            Player.StopPlayback();
        }

        public void StartRecord()
        {
            if (Player.IsRecording)
            {
                return;
            }

            this.CurrentActivity.Results = new List<DataLineItem>();
            this.CurrentActivity.Jitter = 0;

            currentExercicePath = this.exerciceFolderPath + DateTime.Now.ToString("yyyyMMddHHmmss");
            Player.StartRecord(currentExercicePath);
        }

        public async void StopRecord()
        {
            if (!Player.IsRecording)
            {
                return;
            }

            var wavPath = lastExerciceWavPath = await Player.StopRecord();

            this.CurrentActivity.Results = CalculateIntensityAndFrequency(wavPath);

            this.AnalyzeSample(CurrentActivity.Exercice, CurrentActivity.Results);

            this.EvaluateExercice(wavPath);
        }

        public void StartLastExercicePlayblack(Action playDone)
        {
            if (!string.IsNullOrEmpty(this.lastExerciceWavPath))
            {
                Task.Run(() => Player.StartPlayback(this.lastExerciceWavPath, playDone));
            }
        }

        private void AnalyzeSample(ICollection<DataLineItem> baseline, ICollection<DataLineItem> values)
        {
            var correlation = analyser.CalculateCorrelation(baseline, values);
            
        }

        private ICollection<DataLineItem> CalculateIntensityAndFrequency(string wavPath)
        {
            var resultPath = currentExercicePath + "IntensityFrequency.txt";
            if (!File.Exists(resultPath))
            {
                File.Create(resultPath).Close();
            }
            else
            {
                File.WriteAllText(resultPath, string.Empty);
            }
            var scriptPath = this.scripting.WriteIntensityFrequencyScript(wavPath, this.CurrentActivity.PitchMin, this.CurrentActivity.PitchMax, this.CurrentActivity.IntensityThreshold, resultPath);

            this.connector.GetResult(scriptPath);

            return DataExtractor.GetInstance().GetIntensityFrequencyValues(resultPath);
        }

        private ICollection<JitterIntervalItem> CalculateJitter(string wavPath)
        {
            var resultPath = currentExercicePath + "Jitter.txt";
            if (!File.Exists(resultPath))
            {
                File.Create(resultPath).Close();
            }
            else
            {
                File.WriteAllText(resultPath, string.Empty);
            }
            var scriptPath = this.scripting.WriteJitterScript(wavPath, this.CurrentActivity.PitchMin, this.CurrentActivity.PitchMax, resultPath);

            this.connector.GetResult(scriptPath);

            return DataExtractor.GetInstance().GetJitterValues(resultPath);
        }

        private void EvaluateExercice(string wavPath)
        {
            var score = 90;

            if (this.CurrentActivity.F0_exactEvaluated) {

            }

            if (this.CurrentActivity.F0_stableEvaluated) {

            }

            if (this.CurrentActivity.Intensite_stableEvaluated) {

            }

            if (this.CurrentActivity.Courbe_f0_exacteEvaluated) {

            }

            var jitters = CalculateJitter(wavPath);
            if (this.CurrentActivity.Duree_exacteEvaluated) {
                this.CurrentActivity.Duree_exacte = (int)Math.Round(jitters.Select(x => x.EndTime).Max() - jitters.Select(x => x.StartTime).Min());
            }

            if (this.CurrentActivity.JitterEvaluated)
            {
                this.CurrentActivity.Jitter = jitters.OrderByDescending(x => x.EndTime - x.StartTime).First().Jitter;
            }

            string imagePath;
            if (score < 60)
            {
                imagePath = "Ressources\\FailedFace.png";
            }
            else if (score < 80)
            {
                imagePath = "Ressources\\PassedFace.png";
            }
            else
            {
                imagePath = "Ressources\\SucceededFace.png";
            }

            Task.Factory.StartNew(() =>
            {
                //Update Text on the UI thread 
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input,
               new Action(() => {
                   var splash = new SplashScreen(imagePath);
                   splash.Show(false);
                   splash.Close(new TimeSpan(0, 0, 5));
               }));
            });
        }
        
    }
}
