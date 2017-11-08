using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyOrthoOrtho.Models
{
    [XmlType("ExerciceResultat")]
    public class ExerciceResultat
    {
       
        [XmlElement("Date")]
        public string Date { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Exercice_wav_file_name")]
        public string Exercice_wav_file_name { get; set; }

        [XmlElement("Exercice_praat_file_name")]
        public string Exercice_praat_file_name { get; set; }

        [XmlElement("Resultat_wav_file_name")]
        public string Resultat_wav_file_name { get; set; }

        [XmlElement("Resultat_praat_file_name")]
        public string Resultat_praat_file_name { get; set; }

        [XmlElement("Pitch_min")]
        public int PitchMin { get; set; }

        [XmlElement("Pitch_max")]
        public int PitchMax { get; set; }

        [XmlElement("Intensity_threshold")]
        public int IntensityThreshold { get; set; }
        
    }
}
