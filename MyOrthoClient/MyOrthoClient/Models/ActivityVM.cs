using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoClient.Models
{
    class ActivityVM
    {
        public string Name { get; set; }
        public string Example_wav_path { get; set; }
        public int Pitch { get; set; }
        public bool PitchEvaluated { get; set; }
        public int Intensity { get; set; }
        public bool IntensityEvaluated { get; set; }
        public int F0_exact { get; set; }
        public bool F0_exactEvaluated { get; set; }
        public int F0_stable { get; set; }
        public bool F0_stableEvaluated { get; set; }
        public int Intensite_stable { get; set; }
        public bool Intensite_stableEvaluated { get; set; }
        public int Courbe_f0_exacte { get; set; }
        public bool Courbe_f0_exacteEvaluated { get; set; }
        public int Duree_exacte { get; set; }
        public bool Duree_exacteEvaluated { get; set; }
        public int Jitter { get; set; }
        public bool JitterEvaluated { get; set; }        

        public ActivityVM() { }
    }
}
