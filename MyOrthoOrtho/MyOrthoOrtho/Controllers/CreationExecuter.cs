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
    class CreationExecuter
    {
        static string TEMP_PATH = Path.GetTempPath() + "MyOrtho";
        private WAVPlayerRecorder Player;
        private CreationVM CurrentActivity;
        private PraatScripting scripting;
        private PraatConnector connector;
        private string lastExerciceWavPath;
        private string currentExercicePath;
        private string tempRecordingsLocation;

        public CreationExecuter(CreationVM currentActivity)
        {
            this.Player = new WAVPlayerRecorder();
            this.CurrentActivity = currentActivity;
            this.scripting = new PraatScripting(currentActivity.Name);
            this.connector = PraatConnector.GetConnector();
            this.currentExercicePath = CurrentActivity.Example_wav_path;
            this.tempRecordingsLocation = TEMP_PATH + "\\" + this.CurrentActivity.Name;
            if (!Directory.Exists(this.tempRecordingsLocation))
            {
                Directory.CreateDirectory(this.tempRecordingsLocation);
            }

            //File.Copy(CurrentActivity.Example_wav_path, tempRecordingsLocation + CurrentActivity.Name, true);
            string currentExerciceFilePath = (this.tempRecordingsLocation + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");

            Task.Run(() => this.CurrentActivity.Exercice = CalculateIntensityAndFrequency(this.CurrentActivity.Example_wav_path, currentExerciceFilePath));
        }

        public void StartPlaybackExemple()
        {
            Player.StopPlayback();
            Player.StartPlayback(this.CurrentActivity.Example_wav_path);
        }
        
        public void StopPlayback()
        {
            Player.StopPlayback();
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
