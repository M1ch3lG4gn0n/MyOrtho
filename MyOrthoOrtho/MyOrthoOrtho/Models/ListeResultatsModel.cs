using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyOrthoOrtho.Models
{
    [XmlType("Resultats")]
    class ListeResultatsModel
    {
        public ListeResultatsModel()
        {
            Liste_exercices_resultats = new List<ExerciceResultat>();
        }
        [XmlElement("Date_sauvegarde")]
        public string Date_sauvegarde { get; set; }

        [XmlElement("ExerciceResultat")]
        public List<ExerciceResultat> Liste_exercices_resultats { get; set; }
    }
}
