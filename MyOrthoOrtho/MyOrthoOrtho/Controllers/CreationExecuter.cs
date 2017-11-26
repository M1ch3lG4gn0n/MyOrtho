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
        static string TEMP_PATH = Path.GetTempPath() + "MyOrtho\\";
        private WAVPlayerRecorder Player;
        private CreationVM CurrentActivity;
        private PraatScripting scripting;
        private PraatConnector connector;
        private string tempRecordingsLocation;

        public string TempExWavPath { get; set; }
        public string TempExPraatResultsPath { get; set; }
        

        public CreationExecuter(CreationVM currentActivity)
        {
            this.Player = new WAVPlayerRecorder();
            this.CurrentActivity = currentActivity;
            this.scripting = new PraatScripting(currentActivity.Name);
            this.connector = PraatConnector.GetConnector();

            TempExWavPath = CurrentActivity.Example_wav_path;

            this.tempRecordingsLocation = TEMP_PATH;
            if (!Directory.Exists(TEMP_PATH))
            {
                Directory.CreateDirectory(TEMP_PATH);
            }
            
                string currentExerciceFilePath = (TEMP_PATH + "TempPraatResults_" + currentActivity.Date + ".txt");
                string praatScriptTargetPath = (TEMP_PATH + "TempScript_" + currentActivity.Date + ".praat");
            //string currentExerciceFilePath2 = (this.tempRecordingsLocation + "_" + CurrentActivity.Date + ".txt");

            Task.Run(() => this.CurrentActivity.Exercice = CalculateIntensityAndFrequency(this.CurrentActivity.Example_wav_path, currentExerciceFilePath, praatScriptTargetPath));
            
            
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
        
        private ICollection<DataLineItem> CalculateIntensityAndFrequency(string wavPath, string resultPath, string targetPath)
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

            var scriptPath = this.scripting.WriteIntensityFrequencyScript(wavPath, this.CurrentActivity.PitchMin, this.CurrentActivity.PitchMax, this.CurrentActivity.IntensityThreshold, resultPath, targetPath);

            this.connector.GetResult(scriptPath);

            TempExPraatResultsPath = resultPath;

            return DataExtractor.GetInstance().GetFileValues(resultPath);
        }

    }
}
