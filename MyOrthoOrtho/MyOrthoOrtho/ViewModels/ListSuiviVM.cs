using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoOrtho.ViewModels
{
    class ListSuiviVM
    {
        public ObservableCollection<SuiviVM> ActivityList { get; set; }
        public ListSuiviVM()
        {
            ActivityList = new ObservableCollection<SuiviVM>();
        }

        public void Add(SuiviVM activity)
        {
            ActivityList.Add(activity);
        }

        public SuiviVM GetActivity(int index)
        {
            return ActivityList[index];
        }

        public void ClearItems()
        {
            ActivityList.Clear();
        }


    }
}
