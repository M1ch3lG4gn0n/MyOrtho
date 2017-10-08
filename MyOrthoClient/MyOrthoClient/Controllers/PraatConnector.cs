using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MyOrthoClient.Controllers
{
    class PraatConnector
    {
        private string praatExeLocation;
        private static PraatConnector instance;
        
        private PraatConnector()
        {
            praatExeLocation = Directory.GetCurrentDirectory() + "\\praat.exe";
        }

        public static PraatConnector GetConnector()
        {
            if(instance == null)
            {
                instance = new PraatConnector();
            }
            return instance;
        }

        public void GetResult(string script)
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

            p.WaitForExit();
        }
    }
}
