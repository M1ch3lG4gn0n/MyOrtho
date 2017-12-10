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

        public double GetJitterValue(string path)
        {
            var text = File.ReadAllText(path);
            var result = ValidateValue(text);
            double value = 0;
            if (result != null && double.TryParse(text.Split(new char[] { '%' })[0], out value))
            {
                return value;
            }
            return value;
        }

        public double GetTimeLengthValue(string path)
        {
            var text = File.ReadAllText(path);
            var result = ValidateValue(text);
            double value = 0;
            if (result != null && double.TryParse(text, out value))
            {
                return value;
            }
            return value;
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
