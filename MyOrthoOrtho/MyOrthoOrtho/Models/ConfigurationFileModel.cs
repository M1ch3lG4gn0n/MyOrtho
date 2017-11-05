using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoOrtho.Models
{
    [Serializable]
    public class ConfigurationFileModel
    {
        string Date_sauvegarde { get; set; }
        List<Exercice> Liste_exercices { get; set; }
        
    }
}
