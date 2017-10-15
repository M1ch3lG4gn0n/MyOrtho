using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;

namespace MyOrthoOrtho.Models
{
    class SuiviVM : VMBase
    {

        private ICollection<DataLineItem> _exercice;
        private Action<ICollection<DataLineItem>> _setExercise;
        private ICollection<DataLineItem> _results;
        private Action<ICollection<DataLineItem>> _setResult;
        private LineSeries _frequencyExpected;
        private LineSeries _frequencyResult;
        private LineSeries _pitchExpected;
        private LineSeries _pitchResult;

        public string Name { get; set; }
        public string Example_wav_path { get; set; }
        public string Result_wav_path { get; set; }
        public int PitchMin { get; set; }
        public int PitchMax { get; set; }
        public int IntensityThreshold { get; set; }
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

        public ICollection<DataLineItem> Exercice
        {
            get
            {
                return this._exercice;
            }
            set
            {
                this._exercice = value;
                this._setExercise(value);
            }
        }

        public ICollection<DataLineItem> Results
        {
            get
            {
                return this._results;
            }
            set
            {
                this._results = value;
                this._setResult(value);
            }
        }      

        public SuiviVM() { }

        public override string ToString()
        {
            return this.Name;
        }

        public void SetExerciseValue(Action<ICollection<DataLineItem>> action)
        {
            _setExercise = action;
        }

        public void SetResultValue(Action<ICollection<DataLineItem>> action)
        {
            _setResult = action;
        }
    }
}
