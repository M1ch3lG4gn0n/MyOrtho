using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.IO;

namespace MyOrthoClient.Controllers
{
    class WAVPlayerRecorder
    {
        static string RECORD_FORLDER = "~\\Results\\";
        private bool isRecording = false;
        private bool isPlaying = false;
        private string fileName = "";
        private string exerciseFolder = "";
        private string currentWav = "";
        private System.Media.SoundPlayer player;
        [DllImport("winmm.dll")]
        private static extern long mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);
        

    public WAVPlayerRecorder(string folderName)
        {
            exerciseFolder = folderName;
        }

        public async void StartPlayback(string wavPath)
        {
            StopPlayback();

            isPlaying = true;
            currentWav = wavPath;
            string currentDir = Environment.CurrentDirectory;
            player = new System.Media.SoundPlayer(currentDir + wavPath);
            player.Play();

           /* string playCommand = "Open \"" + currentWav + "\" type waveaudio alias example1";
            mciSendString(playCommand, null, 0, IntPtr.Zero);

            playCommand = "Play " + currentWav + " notify";
            mciSendString(playCommand, null, 0, new WindowInteropHelper(Application.Current.MainWindow).Handle);*/

        }
        

        public async void StopPlayback()
        {
            if (isPlaying)
            {
                player.Stop();
                
                isPlaying = false;
            }
            

        }

        public async void StartRecord(string filename)
        {
            //Record into RECORD_FOLDER
            isRecording = true;
            fileName = filename;

            //Microphone mic = Microphone.Default;
        }



        public async Task<string> StopRecord()
        {
            isRecording = false;
            string completePath = RECORD_FORLDER + exerciseFolder + fileName + DateTime.Now.ToLongDateString() + ".wav";

            //return Task.FromResult<string>(RECORD_FORLDER + FILENAME + DateTime.Now.ToLongDateString());
            return (completePath);
        }



        public bool IsRecording => isRecording;
    }

   
}
