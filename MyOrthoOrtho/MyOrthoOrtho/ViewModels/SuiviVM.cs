using MyOrthoOrtho.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;

namespace MyOrthoOrtho.ViewModels
{
    class SuiviVM : VMBase
    {

        public ObservableCollection<ExerciceVM> ActivityList { get; set; }
        public SuiviVM()
        {
            ActivityList = new ObservableCollection<ExerciceVM>();
        }

        public void Add(ExerciceVM activity)
        {
            ActivityList.Add(activity);
        }

        public ExerciceVM GetActivity(int index)
        {
            return ActivityList[index];
        }

        public void ClearItems()
        {
            ActivityList.Clear();
        }
    }
}
