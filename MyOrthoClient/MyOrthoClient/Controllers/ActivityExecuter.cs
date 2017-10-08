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

        public ActivityExecuter(ActivityVM currentActivity)
        {
            this.Player = new WAVPlayerRecorder();
            this.CurrentActivity = currentActivity;
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
            Player.StartRecord();
        }

        public async void StopRecord()
        {
            if (!Player.IsRecording)
            {
                return;
            }

            var result path = Player.StopRecord();
        }

        public async void AnalyseSample()
        {
            
        }
        
    }
}
