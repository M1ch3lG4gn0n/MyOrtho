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
        public ObservableCollection<ExerciceVM> availableExercices;
        public ObservableCollection<ExerciceVM> selectedExercices;

        private ICollection<DataLineItem> _exercice;
        private Action<ICollection<DataLineItem>> _setExercise;
        

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
        
        public void ClearAvailable()
        {
            availableExercices.Clear();
        }

        public void AddAvailable(ExerciceVM item)
        {
            availableExercices.Add(item);
        }
        public void AddSelected(ExerciceVM item)
        {
            selectedExercices.Add(item);
        }

        public CreationVM() {
            availableExercices = new ObservableCollection<ExerciceVM>();
            selectedExercices = new ObservableCollection<ExerciceVM>();
        }


        public void SetExerciseValue(Action<ICollection<DataLineItem>> action)
        {
            _setExercise = action;
        }
        
    }
}
