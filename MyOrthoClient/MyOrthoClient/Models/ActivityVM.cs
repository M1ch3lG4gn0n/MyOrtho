using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoClient.Models
{
    class ActivityVM
    {
        private int Pitch { get; set; }
        private int Intensity { get; set; }
        private string Example_wav_path { get; set; }
        private int F0_exact { get; set; }
        private int F0_stable { get; set; }
        private int Intensite_stable { get; set; }
        private int Courbe_f0_exacte { get; set; }
        private int Duree_exacte { get; set; }
        private int Jitter { get; set; }
        private string Name { get; set; }

        public ActivityVM() { }
    }
}
