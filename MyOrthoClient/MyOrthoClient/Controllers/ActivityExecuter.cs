using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyOrthoClient.Models;

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

        public void StartPlayback()
        {
            Player.StartPlayback(this.CurrentActivity.Example_wav_path);
        }

        public void StopPlayback()
        {
            Player.StopPlayback();
        }

        public void StartRecord()
        {
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
            this.CurrentActivity.Jitter = CalculateJitter(wavPath);

            this.AnalyzeSample(this.CurrentActivity.Results);
        }

        public void StartLastExercicePlayblack()
        {
            if (string.IsNullOrEmpty(this.lastExerciceWavPath))
            {
                Player.StartPlayback(this.lastExerciceWavPath);
            }
        }

        private async void AnalyzeSample(IEnumerable<DataLineItem> values)
        {
            
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

            return DataExtractor.GetInstance().GetFileValues(resultPath);
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

            double result;
            if(double.TryParse(DataExtractor.GetInstance().GetFileSingleValue(resultPath), out result))
            {
                return result;
            }

            return 0;
        }
        
    }
}
