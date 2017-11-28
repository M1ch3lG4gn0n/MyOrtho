using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoOrtho.ViewModels
{
    class ListPreparationVM
    {
        public ObservableCollection<PreparationVM> ActivityList { get; set; }
        public ObservableCollection<PreparationVM> SelectedActivityList { get; set; }
        public ListPreparationVM()
        {
            ActivityList = new ObservableCollection<PreparationVM>();
            SelectedActivityList = new ObservableCollection<PreparationVM>();
        }

        public void Add(object activity)
        {
            ActivityList.Add((PreparationVM)activity);
        }

        public void Remove(object activity)
        {
            ActivityList.Remove((PreparationVM)activity);
        }

        public void AddSelection(object selectedActivity)
        {
            SelectedActivityList.Add((PreparationVM)selectedActivity);
        }

        public void RemoveSelection(object selectedActivity)
        {
            SelectedActivityList.Remove((PreparationVM)selectedActivity);
        }

        public PreparationVM GetActivity(int index)
        {
            return ActivityList[index];
        }

        public PreparationVM GetSelectedActivity(int index)
        {
            return SelectedActivityList[index];
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
