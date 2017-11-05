using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows;

namespace MyOrthoClient.Controllers
{
    public class WAVPlayerRecorder : IObserver<bool>
    {
        private bool _isRecording = false;
        private bool _isPlaying;
        private Action<bool> _playingAction;
        private string _fileName = "";
        private MediaPlayer _player;

        [DllImport("winmm.dll")]
        private static extern long mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);
        

        public WAVPlayerRecorder(Action<bool> playingAction)
        {
            _playingAction = playingAction;
        }

        public void StartPlayback(string wavPath)
        {
            StopPlayback();

            _isPlaying = true;
            _playingAction(true);

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _player = new MediaPlayer();
                _player.Open(new Uri(wavPath, UriKind.Absolute));
                _player.Volume = 1.0;
                _player.IsMuted = false;
                _player.Position = TimeSpan.FromMilliseconds(0);
                _player.Play();
                _player.MediaEnded += Player_MediaEnded;
            }));

            
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            _isPlaying = false;
            _playingAction(false);
        }

        public void StopPlayback()
        {
            if (_isPlaying)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => _player.Stop()));
                _playingAction(false);
            }
        }

        public void StartRecord(string filename)
        {
            _isRecording = true;
            _fileName = filename;
            long[] code =new long[3];
            
            code[0] = mciSendString("open new Type waveaudio Alias recsound", null, 0, IntPtr.Zero);

            code[1] = mciSendString("record recsound", null, 0, IntPtr.Zero);

            code[2] = 0;
        }

        public string StopRecord()
        {
            _isRecording = false;
            string completePath =  _fileName + ".wav";
            int length = 0;
            long[] code = new long[3];
           
            StringBuilder outs = new StringBuilder();
            mciSendString("stop recsound", outs, length, IntPtr.Zero);
            code[0] = mciSendString("save recsound \"" + completePath + "\"", outs, length, IntPtr.Zero);
            code[1] = mciSendString("close recsound", null, 0, IntPtr.Zero);

            code[2] = 0;
            return completePath;
        }

        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(bool value)
        {
            _playingAction(value);
        }

        public bool IsRecording => _isRecording;
        
    }

}
