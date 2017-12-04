using MyOrthoOrtho.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoOrtho.Models
{
    public class ExerciseEvaluationGridRow : VMBase
    {
        public string Name { get; set; }
        public string Evaluated { get; set; }
        public string GoodMax { get; set; }
        public string GoodMin { get; set; }
        public string OkayMax { get; set; }
        public string OkayMin { get; set; }
        public string BadMax { get; set; }
        public string BadMin { get; set; }
    }
}
