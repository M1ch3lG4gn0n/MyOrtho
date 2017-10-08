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
        static bool isPlaying = false;
        static string FILENAME = "";
        static string EXERCISE_FOLDER = "";

        [DllImport("winmm.dll")]
        private static extern long mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);
        

    public WAVPlayerRecorder(string folderName)
        {
            EXERCISE_FOLDER = folderName;
        }

        public async void StartPlayback(string wavPath)
        {
            isPlaying = true;
            string playCommand = "Play " + wavPath + " notify";
            //mciSendString(playCommand, null, 0, notifyForm.Handle);

        }

        public async void StopPlayback()
        {
            

        }

        public async void StartRecord(string filename)
        {
            //Record into RECORD_FOLDER
            isRecording = true;
            FILENAME = filename;

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
