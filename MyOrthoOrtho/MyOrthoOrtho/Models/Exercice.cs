using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyOrthoOrtho.Models
{
    [XmlType("Exercice")]
    public class Exercice
    {
        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Exercice_wav_file_name")]
        public string Exercice_wav_file_name { get; set; }

        [XmlElement("Exercice_praat_file_name")]
        public string Exercice_praat_file_name { get; set; }

        [XmlElement("Pitch_min")]
        public int PitchMin { get; set; }

        [XmlElement("Pitch_max")]
        public int PitchMax { get; set; }

        [XmlElement("Intensity_threshold")]
        public int IntensityThreshold { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
