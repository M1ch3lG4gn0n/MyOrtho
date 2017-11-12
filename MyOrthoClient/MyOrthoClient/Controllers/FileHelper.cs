using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace MyOrthoClient.Controllers
{
    class FileHelper
    {
        public class FileWriter
        {
            public FileWriter()
            {

            }
        }

        public class FileReader
        {
            public FileReader()
            {
                
            }

            public string ReadFile()
            {
                return string.Empty;
            }

            public void zipToExerciceList(string file)
            {
                string zipPath = file;
                string extractPath = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
                
                ZipFile.ExtractToDirectory(zipPath, extractPath);

                populateExerciceList(extractPath);
            }

            private void populateExerciceList(string path)
            {
                //TODO create all the exercices from the xml file in the path
            }
        }
    }
}
