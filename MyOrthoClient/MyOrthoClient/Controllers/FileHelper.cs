using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using MyOrthoClient.Models;
using MyOrthoOrtho.Models;

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

                //populateExerciceList(extractPath);
            }

            private void populateExerciceList(string path)
            {
                //TODO create all the exercices from the xml file in the path

                ActivityVM activityListInstance = new ActivityVM();

                string currentDir = Environment.CurrentDirectory;

                // Create OpenFileDialog 
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

                // Set filter for file extension and default file extension 
                dlg.DefaultExt = ".xml";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> fileSelected = dlg.ShowDialog();

                if (fileSelected == true)
                {
                    string data;
                    //Create a StreamReader to read selected xml file
                    var streamReader = new StreamReader(dlg.FileName, Encoding.UTF8);

                    //Trim and clean the read data to ease parsing
                    data = streamReader.ReadToEnd();
                    data.Trim();
                    data = data.Replace("\n", String.Empty).Replace("\t", String.Empty).Replace("\r", String.Empty);

                    //create instance of our model
                    ListeResultatsModel result = new ListeResultatsModel();

                    //Setup our xml serializer and read xml data into our class
                    var serializer = new XmlSerializer(typeof(ListeResultatsModel));
                    var stream = new StringReader(data);
                    var reader = XmlReader.Create(stream);
                    {
                        result = (ListeResultatsModel)serializer.Deserialize(reader);
                    }

                    //create an object of type SuiviVM from the collected data
                    foreach (ExerciceResultat ex in result.Liste_exercices_resultats)
                    {
                        ActivityVM newSuiviVM = new ActivityVM
                        {
                            Example_wav_path = currentDir + @"\Ressources\" + ex.Exercice_wav_file_name,
                            //Result_wav_path = currentDir + @"\Ressources\" + ex.Resultat_wav_file_name,
                            Name = ex.Name,
                            PitchMin = ex.PitchMin,
                            PitchMax = ex.PitchMax,
                            IntensityThreshold = ex.IntensityThreshold
                        };

                       // activityListInstance.Add(newSuiviVM);
                    }

                }
            }
        }
    }
}
