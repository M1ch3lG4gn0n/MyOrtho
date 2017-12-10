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
    class PreparationVM : VMBase
    {
        public ObservableCollection<ExerciceVM> ActivityList { get; set; }
        public ObservableCollection<ExerciceVM> SelectedActivityList { get; set; }
        public ObservableCollection<ExerciseEvaluationGridRow> ListOfEvaluatedParameters { get; set; }
        public PreparationVM()
        {
            ActivityList = new ObservableCollection<ExerciceVM>();
            SelectedActivityList = new ObservableCollection<ExerciceVM>();
            ListOfEvaluatedParameters = new ObservableCollection<ExerciseEvaluationGridRow>();
        }

        public ObservableCollection<ExerciceVM> getActivityList()
        {
            return ActivityList;
        }
        public ObservableCollection<ExerciceVM> getSelectedActivityList()
        {
            return SelectedActivityList;
        }

        public void Add(object activity)
        {
            ActivityList.Add((ExerciceVM)activity);
        }

        public void Remove(object activity)
        {
            ActivityList.Remove((ExerciceVM)activity);
        }

        public void AddSelection(object selectedActivity)
        {
            SelectedActivityList.Add((ExerciceVM)selectedActivity);
        }

        public void RemoveSelection(object selectedActivity)
        {
            SelectedActivityList.Remove((ExerciceVM)selectedActivity);
        }

        public void AddEvaluated(object evaluatedInformationRow)
        {
            ListOfEvaluatedParameters.Add((ExerciseEvaluationGridRow)evaluatedInformationRow);
        }
        public void ClearEvaluated()
        {
            ListOfEvaluatedParameters.Clear();
        }

        public ExerciceVM GetActivity(int index)
        {
            if(index < ActivityList.Count && index >= 0)
            {
                return ActivityList[index];
            }
            return new ExerciceVM();
        }

        public ExerciceVM GetSelectedActivity(int index)
        {
            if (index < SelectedActivityList.Count && index >= 0)
            {
                return SelectedActivityList[index];
            }
            return new ExerciceVM();
        }

        public void ClearItems()
        {
            ActivityList.Clear();
        }

        public void ClearSelectedItems()
        {
            ActivityList.Clear();
        }

    }
}
