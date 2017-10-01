using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoClient.Controllers
{
    class ActivityExecuter
    {
        private WANPlayerRecorder Player = new WANPlayerRecorder();

        public async void StartPlayback(string wavPath)
        {
            Player.StartPlayback(wavPath);
        
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
            Player.StopRecord();
        }
    }
}
