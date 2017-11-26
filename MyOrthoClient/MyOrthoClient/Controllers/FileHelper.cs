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
using System.Xml.Linq;

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

            public void zipToExerciceList(string file, ListVM activityList)
            {
                string zipPath = file;
                string extractPath = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
                
                ZipFile.ExtractToDirectory(zipPath, extractPath);

                populateExerciceList(extractPath, activityList);
            }

            private void populateExerciceList(string path, ListVM activityList)
            {
                XDocument xml = XDocument.Load(path + "\\config.xml");
                var activities = xml.Descendants("Activity");

                foreach (XElement activity in activities)
                {
                    ActivityVM newSuiviVM = new ActivityVM
                    {
                        Name = activity.Descendants("Name").First().Value,
                        Example_wav_path = path + "\\" + activity.Descendants("Exercice_wav_file_name").First().Value,
                        PitchMin = Convert.ToInt32(activity.Descendants("Pitch_min").First().Value),
                        PitchMax = Convert.ToInt32(activity.Descendants("Pitch_max").First().Value),
                        IntensityThreshold = Convert.ToInt32(activity.Descendants("Intensity_threshold").First().Value)
                    };
                    activityList.Add(newSuiviVM);
                }
            }
        }
    }
}
