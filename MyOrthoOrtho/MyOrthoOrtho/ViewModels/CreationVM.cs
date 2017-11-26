using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;
using MyOrthoOrtho.Models;
using System.Collections.ObjectModel;

namespace MyOrthoOrtho.ViewModels
{
    class CreationVM : VMBase
    {
        public ObservableCollection<Exercice> availableExercices;
        public ObservableCollection<Exercice> selectedExercices;

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
        public string Date { get; set; }
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
        public int Duree_expected { get; set; }
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

        public void ClearAvailable()
        {
            availableExercices.Clear();
        }

        public void AddAvailable(Exercice item)
        {
            availableExercices.Add(item);
        }
        public void AddSelected(Exercice item)
        {
            selectedExercices.Add(item);
        }

        public CreationVM() {
            availableExercices = new ObservableCollection<Exercice>();
            selectedExercices = new ObservableCollection<Exercice>();
        }

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
