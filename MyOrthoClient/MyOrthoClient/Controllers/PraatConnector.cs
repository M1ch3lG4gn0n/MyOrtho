using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyOrthoClient.Controllers
{
    class PraatConnector
    {
        private string praatExeLocation;


        public PraatConnector()
        {
            praatExeLocation = Directory.GetCurrentDirectory() + "\\praat.exe";
        }

        public async void GetResult(string script)
        {
            if (!File.Exists(script))
            {
                throw new FileNotFoundException("Script not found");
            }

            ProcessStartInfo si = new ProcessStartInfo();
            si.FileName = praatExeLocation;

            si.RedirectStandardOutput = true;
            si.WindowStyle = ProcessWindowStyle.Hidden;
            si.UseShellExecute = false;

            si.Arguments = string.Format("--run {0}", script);

            Process p = new Process();
            p.StartInfo = si;
            p.Start();

            await Task.Run(() => p.WaitForExit());
        }
    }
}
