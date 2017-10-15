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

namespace MyOrthoOrtho.Controllers
{
    class WAVPlayerRecorder
    {
        static string RECORD_FORLDER = Directory.GetCurrentDirectory() + "\\Results\\";
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
            player = new System.Media.SoundPlayer(wavPath);
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
            long[] code =new long[3];
            string completePath = RECORD_FORLDER + exerciseFolder + "\\" + fileName + ".wav";
            int length = 0;
            
            code[0] = mciSendString("open new Type waveaudio Alias recsound", null, 0, IntPtr.Zero);

            code[1] = mciSendString("record recsound", null, 0, IntPtr.Zero);

            code[2] = 0;
        }



        public async Task<string> StopRecord()
        {
            isRecording = false;
            string completePath =  RECORD_FORLDER + exerciseFolder + "\\" + fileName + ".wav";
            int length = 0;
            long[] code = new long[3];
            if (!Directory.Exists(RECORD_FORLDER + exerciseFolder))
            {
                Directory.CreateDirectory(RECORD_FORLDER + exerciseFolder);
            }
           
            StringBuilder outs = new StringBuilder();
            mciSendString("stop recsound", outs, length, IntPtr.Zero);
            code[0] = mciSendString("save recsound \"" + completePath + "\"", outs, length, IntPtr.Zero);
            code[1] = mciSendString("close recsound", null, 0, IntPtr.Zero);

            code[2] = 0;
            //return Task.FromResult<string>(RECORD_FORLDER + FILENAME + DateTime.Now.ToLongDateString());
            return (completePath);
        }



        public bool IsRecording => isRecording;
    }

   
}
