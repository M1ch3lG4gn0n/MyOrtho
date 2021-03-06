﻿using System.Collections.Generic;
using MyOrthoOrtho.Models;
using System.IO;

namespace MyOrthoOrtho.Controllers
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

        public ICollection<DataLineItem> GetFileValues(string path)
        {
            var list = new List<DataLineItem>();

            var lines = File.ReadLines(path);
            foreach(string line in lines)
            {
                var result = ValidateValue(line);
                if(result != null)
                {
                    list.Add(result);
                }
            }

            return list;
        }

        private DataLineItem ValidateValue(string line)
        {
            if (line.Contains("undefined"))
            {
                return null;
            }
            var values = line.Split(new char[]{ ' ' });

            return new DataLineItem()
            {
                Time = double.Parse(values[0]),
                Intensity = double.Parse(values[1]),
                Pitch = double.Parse(values[2])
            };
        }
    }
}
