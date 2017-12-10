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
        public ObservableCollection<ExerciceVM> ActivityList { get; set; }
        
        public ExerciceVM CurrentExercice { get; set; }

        private ICollection<DataLineItem> _exercice;
        private Action<ICollection<DataLineItem>> _setExercise;

        public CreationVM()
        {
            ActivityList = new ObservableCollection<ExerciceVM>();
            CurrentExercice = new ExerciceVM();
            SetDefaultExerciceValues();
            SetDefaultExerciceValues();
        }

        public void SetDefaultExerciceValues()
        {
            CurrentExercice.Name = Properties.Settings.Default.name;
            CurrentExercice.PitchMin = Properties.Settings.Default.pitchMin;
            CurrentExercice.PitchMax = Properties.Settings.Default.pitchMax;
            CurrentExercice.Date = DateTime.Now.ToString("yyyyMMddHHmmss");
            CurrentExercice.IntensityThreshold = Properties.Settings.Default.intensityThreshold;
            CurrentExercice.Duree_exacte = Properties.Settings.Default.duration;

            CurrentExercice.Type = Properties.Settings.Default.type;

            CurrentExercice.F0_exactEvaluated = Properties.Settings.Default.f0ExacteEvaluated;
            CurrentExercice.F0_stableEvaluated = Properties.Settings.Default.f0StableEvaluated;
            CurrentExercice.Courbe_f0_exacteEvaluated = Properties.Settings.Default.courbeF0ExacteEvaluated;
            CurrentExercice.Duree_exacteEvaluated = Properties.Settings.Default.DurationEvaluated;
            CurrentExercice.Intensite_stableEvaluated = Properties.Settings.Default.IntensiteStableEvaluated;
            CurrentExercice.JitterEvaluated = Properties.Settings.Default.JitterEvaluated;

            CurrentExercice.F0_exact_good_max = Properties.Settings.Default.f0ExacteGoodMax;
            CurrentExercice.F0_exact_good_min = Properties.Settings.Default.f0ExacteGoodMin;
            CurrentExercice.F0_exact_okay_max = Properties.Settings.Default.f0ExacteOkayMax;
            CurrentExercice.F0_exact_okay_min = Properties.Settings.Default.f0ExacteOkayMin;
            CurrentExercice.F0_exact_bad_max = Properties.Settings.Default.f0ExacteBadMax;
            CurrentExercice.F0_exact_bad_min = Properties.Settings.Default.f0ExacteBadMin;

            CurrentExercice.F0_stable_good_max = Properties.Settings.Default.f0StableGoodMax;
            CurrentExercice.F0_stable_good_min = Properties.Settings.Default.f0StableGoodMin;
            CurrentExercice.F0_stable_okay_max = Properties.Settings.Default.f0StableOkayMax;
            CurrentExercice.F0_stable_okay_min = Properties.Settings.Default.f0StableOkayMin;
            CurrentExercice.F0_stable_bad_max = Properties.Settings.Default.f0StableBadMax;
            CurrentExercice.F0_stable_bad_min = Properties.Settings.Default.f0StableBadMin;

            CurrentExercice.Courbe_F0_exact_good_max = Properties.Settings.Default.courbeF0ExacteGoodMax;
            CurrentExercice.Courbe_F0_exact_good_min = Properties.Settings.Default.courbeF0ExacteGoodMin;
            CurrentExercice.Courbe_F0_exact_okay_max = Properties.Settings.Default.courbeF0ExacteOkayMax;
            CurrentExercice.Courbe_F0_exact_okay_min = Properties.Settings.Default.courbeF0ExacteOkayMin;
            CurrentExercice.Courbe_F0_exact_bad_max = Properties.Settings.Default.courbeF0ExacteBadMax;
            CurrentExercice.Courbe_F0_exact_bad_min = Properties.Settings.Default.courbeF0ExacteBadMin;

            CurrentExercice.Duree_good_max = Properties.Settings.Default.DurationGoodMax;
            CurrentExercice.Duree_good_min = Properties.Settings.Default.DurationGoodMin;
            CurrentExercice.Duree_okay_max = Properties.Settings.Default.DurationOkayMax;
            CurrentExercice.Duree_okay_min = Properties.Settings.Default.DurationOkayMin;
            CurrentExercice.Duree_bad_max = Properties.Settings.Default.DurationBadMax;
            CurrentExercice.Duree_bad_min = Properties.Settings.Default.DurationBadMin;

            CurrentExercice.Intensite_stable_good_max = Properties.Settings.Default.IntensiteStableGoodMax;
            CurrentExercice.Intensite_stable_good_min = Properties.Settings.Default.IntensiteStableGoodMin;
            CurrentExercice.Intensite_stable_okay_max = Properties.Settings.Default.IntensiteStableOkayMax;
            CurrentExercice.Intensite_stable_okay_min = Properties.Settings.Default.IntensiteStableOkayMin;
            CurrentExercice.Intensite_stable_bad_max = Properties.Settings.Default.IntensiteStableBadMax;
            CurrentExercice.Intensite_stable_bad_min = Properties.Settings.Default.IntensiteStableBadMin;

            CurrentExercice.Jitter_good_max = Properties.Settings.Default.JitterGoodMax;
            CurrentExercice.Jitter_good_min = Properties.Settings.Default.JitterGoodMin;
            CurrentExercice.Jitter_okay_max = Properties.Settings.Default.JitterOkayMax;
            CurrentExercice.Jitter_okay_min = Properties.Settings.Default.JitterOkayMin;
            CurrentExercice.Jitter_bad_max = Properties.Settings.Default.JitterBadMax;
            CurrentExercice.Jitter_bad_min = Properties.Settings.Default.JitterBadMin;

        }

        public void UpdateActivity(ExerciceVM activity)
        {
            CurrentExercice.PitchMin = activity.PitchMin;
            CurrentExercice.PitchMax = activity.PitchMax;
            CurrentExercice.Date = activity.Date;
            CurrentExercice.IntensityThreshold = activity.IntensityThreshold;
            CurrentExercice.Duree_exacte = activity.Duree_exacte;

            CurrentExercice.Exercice = activity.Exercice;
            CurrentExercice.Name = activity.Name;
            CurrentExercice.Example_wav_path = activity.Example_wav_path;
            CurrentExercice.Example_praat_path = activity.Example_praat_path;

            CurrentExercice.F0_exactEvaluated = activity.F0_exactEvaluated;
            CurrentExercice.F0_stableEvaluated = activity.F0_stableEvaluated;
            CurrentExercice.Courbe_f0_exacteEvaluated = activity.Courbe_f0_exacteEvaluated;
            CurrentExercice.Duree_exacteEvaluated = activity.Duree_exacteEvaluated;
            CurrentExercice.Intensite_stableEvaluated = activity.Intensite_stableEvaluated;
            CurrentExercice.JitterEvaluated = activity.JitterEvaluated;

            CurrentExercice.F0_exact_good_max = activity.F0_exact_good_max;
            CurrentExercice.F0_exact_good_min = activity.F0_exact_good_min;
            CurrentExercice.F0_exact_okay_max = activity.F0_exact_okay_max;
            CurrentExercice.F0_exact_okay_min = activity.F0_exact_okay_min;
            CurrentExercice.F0_exact_bad_max = activity.F0_exact_bad_max;
            CurrentExercice.F0_exact_bad_min = activity.F0_exact_bad_min;

            CurrentExercice.F0_stable_good_max = activity.F0_stable_good_max;
            CurrentExercice.F0_stable_good_min = activity.F0_stable_good_min;
            CurrentExercice.F0_stable_okay_max = activity.F0_stable_okay_max;
            CurrentExercice.F0_stable_okay_min = activity.F0_stable_okay_min;
            CurrentExercice.F0_stable_bad_max = activity.F0_stable_bad_max;
            CurrentExercice.F0_stable_bad_min = activity.F0_stable_bad_min;

            CurrentExercice.Courbe_F0_exact_good_max = activity.Courbe_F0_exact_good_max;
            CurrentExercice.Courbe_F0_exact_good_min = activity.Courbe_F0_exact_good_min;
            CurrentExercice.Courbe_F0_exact_okay_max = activity.Courbe_F0_exact_okay_max;
            CurrentExercice.Courbe_F0_exact_okay_min = activity.Courbe_F0_exact_okay_min;
            CurrentExercice.Courbe_F0_exact_bad_max = activity.Courbe_F0_exact_bad_max;
            CurrentExercice.Courbe_F0_exact_bad_min = activity.Courbe_F0_exact_bad_min;

            CurrentExercice.Duree_good_max = activity.Duree_good_max;
            CurrentExercice.Duree_good_min = activity.Duree_good_min;
            CurrentExercice.Duree_okay_max = activity.Duree_okay_max;
            CurrentExercice.Duree_okay_min = activity.Duree_okay_min;
            CurrentExercice.Duree_bad_max = activity.Duree_bad_max;
            CurrentExercice.Duree_bad_min = activity.Duree_bad_min;

            CurrentExercice.Intensite_stable_good_max = activity.Intensite_stable_good_max;
            CurrentExercice.Intensite_stable_good_min = activity.Intensite_stable_good_min;
            CurrentExercice.Intensite_stable_okay_max = activity.Intensite_stable_okay_max;
            CurrentExercice.Intensite_stable_okay_min = activity.Intensite_stable_okay_min;
            CurrentExercice.Intensite_stable_bad_max = activity.Intensite_stable_bad_max;
            CurrentExercice.Intensite_stable_bad_min = activity.Intensite_stable_bad_min;

            CurrentExercice.Jitter_good_max = activity.Jitter_good_max;
            CurrentExercice.Jitter_good_min = activity.Jitter_good_min;
            CurrentExercice.Jitter_okay_max = activity.Jitter_okay_max;
            CurrentExercice.Jitter_okay_min = activity.Jitter_okay_min;
            CurrentExercice.Jitter_bad_max = activity.Jitter_bad_max;
            CurrentExercice.Jitter_bad_min = activity.Jitter_bad_min;
        }

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

        public ObservableCollection<ExerciceVM> getActivityList()
        {
            return ActivityList;
        }

        public void ClearItems()
        {
            ActivityList.Clear();
        }

        public ExerciceVM GetActivity(int index)
        {
            if (index < ActivityList.Count && index >= 0)
            {
                return ActivityList[index];
            }
            return new ExerciceVM();
        }

        public void AddAvailable(ExerciceVM item)
        {
            ActivityList.Add(item);
        }
        
        public void SetExerciseValue(Action<ICollection<DataLineItem>> action)
        {
            _setExercise = action;
        }
        
    }
}
