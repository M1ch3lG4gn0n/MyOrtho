using System;
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

            var resultPath = string.Empty;
            var scriptPath = await this.scripting.WriteScript(wavPath, this.CurrentActivity.Pitch, this.CurrentActivity.Intensity, resultPath);

            this.connector.GetResult(scriptPath);

            var resultValues = DataExtractor.GetInstance().GetFileValues(resultPath);

            //this.CurrentActivity.

            this.AnalyzeSample(resultValues);
        }

        private async void AnalyzeSample(IEnumerable<DataLineItem> values)
        {
            
        }
        
    }
}
