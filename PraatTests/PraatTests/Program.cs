using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace PraatTests
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessStartInfo si = new ProcessStartInfo();
            si.FileName = @"C:\Users\Michel\Source\Repos\MyOrtho\MyOrthoClient\MyOrthoClient\praat.exe";

            si.RedirectStandardOutput = true;
            si.WindowStyle = ProcessWindowStyle.Hidden;
            si.UseShellExecute = false;

            si.Arguments = @"--run C:\Users\Michel\Downloads\script3.praat";

            Process p = new Process();
            p.StartInfo = si;
            p.Start();

            p.WaitForExit();

            Console.Read();
        }
    }
}
