using System.Collections.Generic;
using MyOrthoClient.Models;
using System.IO;

namespace MyOrthoClient.Controllers
{
    class DataExtractor
    {
        private static DataExtractor instance;

        private DataExtractor()
        {
        }

        public static DataExtractor GetInstance()
        {
            if (instance == null)
            {
                instance = new DataExtractor();
            }

            return instance;
        }

        public ICollection<DataLineItem> GetIntensityFrequencyValues(string path)
        {
            var list = new List<DataLineItem>();

            var lines = File.ReadLines(path);
            foreach(string line in lines)
            {
                var result = ValidateValue(line);
                if(result != null)
                {
                    list.Add(new DataLineItem()
                    {
                        Time = double.Parse(result[0]),
                        Pitch = double.Parse(result[1]),
                        Intensity = double.Parse(result[2])
                    });
                }
            }

            return list;
        }

        public ICollection<JitterIntervalItem> GetJitterValues(string path)
        {
            var list = new List<JitterIntervalItem>();

            var lines = File.ReadLines(path);
            foreach (string line in lines)
            {
                var result = ValidateValue(line);
                if (result != null)
                {
                    list.Add(new JitterIntervalItem()
                    {
                        StartTime = double.Parse(result[0]),
                        EndTime = double.Parse(result[1]),
                        Jitter = double.Parse(result[2])
                    });
                }
            }

            return list;
        }

        private string[] ValidateValue(string line)
        {
            //TODO Interpoler les valeurs
            if (line.Contains("undefined"))
            {
                return null;
            }
            return line.Split(new char[]{ ' ' });
        }
    }
}
