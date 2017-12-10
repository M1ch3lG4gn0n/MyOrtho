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
    class ExerciceVM : VMBase
    {
        private ICollection<DataLineItem> _exercice;
        private Action<ICollection<DataLineItem>> _setExercise;
        private ICollection<DataLineItem> _results;
        private Action<ICollection<DataLineItem>> _setResult;
        
        public string Name { get; set; }
        public string Example_wav_path { get; set; }
        public string Example_praat_path { get; set; }
        public string Result_wav_path { get; set; }
        public string Result_praat_path { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }

        public int PitchMin { get; set; }
        public int PitchMax { get; set; }
        public int IntensityThreshold { get; set; }
        public decimal Duree_exacte { get; set; }

        public bool F0_exactEvaluated { get; set; }
        public bool F0_stableEvaluated { get; set; }
        public bool Courbe_f0_exacteEvaluated { get; set; }
        public bool Intensite_stableEvaluated { get; set; }
        public bool Duree_exacteEvaluated { get; set; }
        public bool JitterEvaluated { get; set; }

        public decimal F0_exact_good_max { get; set; }
        public decimal F0_exact_good_min { get; set; }
        public decimal F0_exact_okay_max { get; set; }
        public decimal F0_exact_okay_min { get; set; }
        public decimal F0_exact_bad_max { get; set; }
        public decimal F0_exact_bad_min { get; set; }

        public decimal F0_stable_good_max { get; set; }
        public decimal F0_stable_good_min { get; set; }
        public decimal F0_stable_okay_max { get; set; }
        public decimal F0_stable_okay_min { get; set; }
        public decimal F0_stable_bad_max { get; set; }
        public decimal F0_stable_bad_min { get; set; }

        public decimal Courbe_F0_exact_good_max { get; set; }
        public decimal Courbe_F0_exact_good_min { get; set; }
        public decimal Courbe_F0_exact_okay_max { get; set; }
        public decimal Courbe_F0_exact_okay_min { get; set; }
        public decimal Courbe_F0_exact_bad_max { get; set; }
        public decimal Courbe_F0_exact_bad_min { get; set; }

        public decimal Intensite_stable_good_max { get; set; }
        public decimal Intensite_stable_good_min { get; set; }
        public decimal Intensite_stable_okay_max { get; set; }
        public decimal Intensite_stable_okay_min { get; set; }
        public decimal Intensite_stable_bad_max { get; set; }
        public decimal Intensite_stable_bad_min { get; set; }

        public decimal Jitter_good_max { get; set; }
        public decimal Jitter_good_min { get; set; }
        public decimal Jitter_okay_max { get; set; }
        public decimal Jitter_okay_min { get; set; }
        public decimal Jitter_bad_max { get; set; }
        public decimal Jitter_bad_min { get; set; }

        public decimal Duree_good_max { get; set; }
        public decimal Duree_good_min { get; set; }
        public decimal Duree_okay_max { get; set; }
        public decimal Duree_okay_min { get; set; }
        public decimal Duree_bad_max { get; set; }
        public decimal Duree_bad_min { get; set; }

        public ICollection<DataLineItem> Exercice
        {
            get
            {
                return this._exercice;
            }
            set
            {
                this._exercice = value;
                if (_setExercise != null)
                {
                    this._setExercise(value);
                }
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
                if (this._setResult != null)
                {
                    this._setResult(value);
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void SetExerciseValue(Action<ICollection<DataLineItem>> action)
        {
            _setExercise = action;
            if (this._exercice != null)
            {
                this._setExercise(this._exercice);
            }
            else
            {
                this._setExercise(new List<DataLineItem>());
            }
        }

        public void SetResultValue(Action<ICollection<DataLineItem>> action)
        {
            _setResult = action;
            if (this._results != null)
            {
                this._setResult(this._results);
            }
            else
            {
                this._setResult(new List<DataLineItem>());
            }
        }
    }
}
