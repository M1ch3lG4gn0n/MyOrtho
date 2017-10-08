using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;

namespace MyOrthoClient.Controllers
{
    class WAVPlayerRecorder
    {
        static string RECORD_FORLDER = "~\\Results\\";
        static bool isRecording = false;
        static string FILENAME = "";

        public WANPlayerRecorder(string filename)
        {
            FILENAME = filename;
        }

        public async void StartPlayback(string wavPath)
        {
            

        }

        public async void StopPlayback()
        {
            

        }

        public async void StartRecord()
        {
            //Record into RECORD_FOLDER
            isRecording = true;

            //Microphone mic = Microphone.Default;
        }



        public async Task<string> StopRecord()
        {
            isRecording = false;

            //return Task.FromResult<string>(RECORD_FORLDER + FILENAME + DateTime.Now.ToLongDateString());
            return (RECORD_FORLDER + FILENAME + DateTime.Now.ToLongDateString());
        }

        public bool IsRecording()
        {
            return isRecording;
        }
    }
}
