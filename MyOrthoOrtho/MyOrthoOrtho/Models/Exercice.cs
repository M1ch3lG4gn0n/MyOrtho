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

        [XmlElement("F0_exacteEvaluated")]
        public bool F0_exacte_evaluated { get; set; }
        
        [XmlElement("F0_stableEvaluated")]
        public bool F0_stable_evaluated { get; set; }
        
        [XmlElement("Intensite_stableEvaluated")]
        public bool Intensite_stable_evaluated { get; set; }

        [XmlElement("Courbe_f0_exacteEvaluated")]
        public bool F0_courbe_exact_evaluated { get; set; }

        [XmlElement("Duree_exacteEvaluated")]
        public bool Duree_exacte_evaluated { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
