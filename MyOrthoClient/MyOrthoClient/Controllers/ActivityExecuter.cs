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

        public ActivityExecuter(ActivityVM currentActivity)
        {
            this.Player = new WAVPlayerRecorder(currentActivity.Name);
            this.CurrentActivity = currentActivity;
            this.scripting = new PraatScripting(currentActivity.Name);
            this.connector = PraatConnector.GetConnector();
            this.analyser = new SoundAnalyser();

            Task.Run(() => this.CurrentActivity.Exercice = GetNumericValue(this.CurrentActivity.Example_wav_path).Result);
        }

        public async void StartPlayback()
        {
            Player.StartPlayback(this.CurrentActivity.Example_wav_path);
        }

        public async void StopPlayback()
        {
            Player.StopPlayback();
        }

        public async void StartRecord()
        {
            var fileName = this.CurrentActivity.Name + DateTime.Now.ToString("yyyyMMddHHmmss");
            Player.StartRecord(fileName);
        }

        public async void StopRecord()
        {
            if (!Player.IsRecording)
            {
                return;
            }

            var wavPath = await Player.StopRecord();

            /*var selectFile = new Microsoft.Win32.OpenFileDialog();

            var selectedResult = selectFile.ShowDialog();

            if (!(selectedResult == true))
            {
                return;
            }

            var wavPath = selectFile.FileName;*/

            this.CurrentActivity.Results = await GetNumericValue(wavPath);

            this.AnalyzeSample(this.CurrentActivity.Results);
        }

        private async void AnalyzeSample(IEnumerable<DataLineItem> values)
        {
            
        }

        private async Task<ICollection<DataLineItem>> GetNumericValue(string wavPath)
        {
            var resultPath = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\" + Guid.NewGuid().ToString("N") + ".txt";
            if (!File.Exists(resultPath))
            {
                File.Create(resultPath).Close();
            }
            else
            {
                File.WriteAllText(resultPath, string.Empty);
            }
            var scriptPath = await this.scripting.WriteScript(wavPath, this.CurrentActivity.PitchMin, this.CurrentActivity.PitchMax, this.CurrentActivity.IntensityThreshold, resultPath);

            this.connector.GetResult(scriptPath);

            return DataExtractor.GetInstance().GetFileValues(resultPath);
        }
        
    }
}
