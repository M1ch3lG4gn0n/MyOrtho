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
            si.FileName = @"C:\Users\Michel\Source\Repos\MyOrtho\MyOrthoClient\praat.exe";

            si.RedirectStandardOutput = true;
            si.WindowStyle = ProcessWindowStyle.Hidden;
            si.UseShellExecute = false;

            si.Arguments = @"--run C:\Users\Michel\Downloads\script.praat";

            Process p = new Process();
            p.StartInfo = si;
            p.Start();
            string s = p.StandardOutput.ReadToEnd();

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Michel\Downloads\example.txt"))
            {
                file.WriteLine(s);
            }

            Console.WriteLine(s);
            Console.Read();
        }
    }
}
