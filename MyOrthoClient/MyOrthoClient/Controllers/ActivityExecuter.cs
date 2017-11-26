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
        private Action<string> setFeedback;

        public ActivityExecuter(ActivityVM currentActivity, Action<bool> playingAction, Action<string> setFeedback)
        {
            this.Player = new WAVPlayerRecorder(new Action<bool>(x =>
            {
                playingAction(x);
                if (x)
                {
                    this.setFeedback(Environment.CurrentDirectory + "\\" + RessourceService.PlayingGifPath);
                }else
                {
                    this.setFeedback("");
                }
            }));
            this.CurrentActivity = currentActivity;
            this.scripting = new PraatScripting(currentActivity.Name);
            this.connector = PraatConnector.GetConnector();
            this.analyser = new SoundAnalyser();
            this.setFeedback = setFeedback;
            this.exerciceFolderPath = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\" + this.CurrentActivity.Name+ "\\";
            if (!Directory.Exists(this.exerciceFolderPath))
            {
                Directory.CreateDirectory(this.exerciceFolderPath);
            }
            currentExercicePath = this.exerciceFolderPath + DateTime.Now.ToString("yyyyMMddHHmmss");
            /*
            Task.Run(() =>
            {
                this.setFeedback(Environment.CurrentDirectory + "\\" + RessourceService.LoadingGifPath);
                this.CurrentActivity.Exercice = CalculateIntensityAndFrequency(this.CurrentActivity.Example_wav_path);
                this.setFeedback("");
            });*/
        }

        public void StartPlayback()
        {
            Task.Run(() =>
            {
                Player.StartPlayback(this.CurrentActivity.Example_wav_path);
            });
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

            this.setFeedback(Environment.CurrentDirectory + "\\" + RessourceService.RecordingGifPath);

            this.CurrentActivity.Results = new List<DataLineItem>();
            this.CurrentActivity.Jitter = 0;

            currentExercicePath = this.exerciceFolderPath + DateTime.Now.ToString("yyyyMMddHHmmss");
            Player.StartRecord(currentExercicePath);
        }

        public void StopRecord()
        {
            if (!Player.IsRecording)
            {
                return;
            }

            var wavPath = lastExerciceWavPath = Player.StopRecord();

            this.setFeedback(Environment.CurrentDirectory + "\\" + RessourceService.LoadingGifPath);

            this.CurrentActivity.Results = CalculateIntensityAndFrequency(wavPath);

            this.AnalyzeSample(CurrentActivity.Exercice, CurrentActivity.Results);

            this.EvaluateExercice(wavPath);

            this.setFeedback("");


        }

        public void StartLastExercicePlayblack()
        {
            if (!string.IsNullOrEmpty(this.lastExerciceWavPath))
            {
                Task.Run(() =>
                {
                    Player.StartPlayback(this.lastExerciceWavPath);
                });
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

        private double CalculateJitter(string wavPath)
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

            return DataExtractor.GetInstance().GetJitterValue(resultPath);
        }

        private void EvaluateExercice(string wavPath)
        {
            var score = 0;
            var random = new Random();
            var count = 0;

            if (this.CurrentActivity.F0_exactEvaluated)
            {
                count++;
                var value = random.Next(0, 100);
                this.CurrentActivity.F0_exact = value;
                score += value;
            }

            if (this.CurrentActivity.F0_stableEvaluated)
            {
                count++;
                var value = random.Next(0, 100);
                this.CurrentActivity.F0_stable = value;
                score += value;
            }

            if (this.CurrentActivity.Intensite_stableEvaluated)
            {
                count++;
                var value = random.Next(0, 100);
                this.CurrentActivity.Intensite_stable = value;
                score += value;
            }

            if (this.CurrentActivity.Courbe_f0_exacteEvaluated)
            {
                count++;
                var value = random.Next(0, 100);
                this.CurrentActivity.Courbe_f0_exacte = value;
                score += value;
            }

            
            if (this.CurrentActivity.Duree_exacteEvaluated)
            {
                count++;
                var value = random.Next(0, 100);
                this.CurrentActivity.Duree_exacte = value;
                score += value;
            }
            
            if (this.CurrentActivity.JitterEvaluated)
            {
                count++;
                var value = CalculateJitter(wavPath);
                this.CurrentActivity.Jitter = value;
                score += ScoreProvider.EvaluateJitter(value);
            }

            if(count == 0)
            {
                return;
            }

            score = score / count;

            string imagePath = ScoreProvider.ImageResult(score);

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
