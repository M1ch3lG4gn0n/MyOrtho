using MyOrthoOrtho.ViewModels;
using MyOrthoOrtho.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoOrtho.Controllers
{
    class PreparationExecuter
    {
        private WAVPlayerRecorder Player;
        private PreparationVM CurrentActivity;
        private PraatScripting scripting;
        private PraatConnector connector;
        private string lastExerciceWavPath;
        private string exerciceFolderPath;
        private string currentExercicePath;

        public PreparationExecuter(PreparationVM currentActivity)
        {
            this.Player = new WAVPlayerRecorder();
            this.CurrentActivity = currentActivity;
            this.scripting = new PraatScripting(currentActivity.Name);
            this.connector = PraatConnector.GetConnector();
            this.currentExercicePath = CurrentActivity.Example_wav_path;
            this.exerciceFolderPath = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\" + this.CurrentActivity.Name + "\\";
            if (!Directory.Exists(this.exerciceFolderPath))
            {
                Directory.CreateDirectory(this.exerciceFolderPath);
            }

            File.Copy(CurrentActivity.Example_wav_path, exerciceFolderPath + CurrentActivity.Name, true);
            string currentExerciceFilePath = (this.exerciceFolderPath + "exercice" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
            string currentResultFilePath = (this.exerciceFolderPath + "resultat" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");

            Task.Run(() => this.CurrentActivity.Exercice = CalculateIntensityAndFrequency(this.CurrentActivity.Example_wav_path, currentExerciceFilePath));
        }

        public void StartPlaybackExemple()
        {
            Player.StopPlayback();
            Player.StartPlayback(this.CurrentActivity.Example_wav_path);
        }

        public void StartPlaybackResult()
        {
            Player.StopPlayback();
            Player.StartPlayback(this.CurrentActivity.Result_wav_path);
        }

        public void StopPlayback()
        {
            Player.StopPlayback();
        }

        public void StartRecord()
        {
            string currentExerciceFilePath = (this.exerciceFolderPath + "exercice" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
            Player.StartRecord(currentExerciceFilePath);
        }

        public async void StopRecord()
        {
            string filename = (this.exerciceFolderPath + "resultat" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
            if (!Player.IsRecording)
            {
                return;
            }
           
            var wavPath = lastExerciceWavPath = await Player.StopRecord();
            CurrentActivity.Example_wav_path = wavPath;

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

        private ICollection<DataLineItem> CalculateIntensityAndFrequency(string wavPath, string resultPath)
        {
            //var resultPath = currentExercicePath + ".txt";

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

    }
}
