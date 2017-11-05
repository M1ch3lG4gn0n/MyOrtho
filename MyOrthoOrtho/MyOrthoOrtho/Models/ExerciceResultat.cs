using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyOrthoOrtho.Models
{

    public class ExerciceResultat
    {
        public ExerciceResultat()
        {
            
        }
        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Exercice_wav_file_name")]
        public string Exercice_wav_file_name { get; set; }

        [XmlElement("Exercice_praat_file_name")]
        public string Exercice_praat_file_name { get; set; }

        [XmlElement("PitchMin")]
        public int PitchMin { get; set; }

        [XmlElement("PitchMax")]
        public int PitchMax { get; set; }

        [XmlElement("IntensityThreshold")]
        public int IntensityThreshold { get; set; }

        [XmlElement("DureeExacte")]
        public int DureeExacte { get; set; }
    }
}
