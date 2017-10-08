using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyOrthoClient.Models
{
    class ListVM
    {
        public ObservableCollection<ActivityVM> ActivityList { get; set; }
        public ListVM()
        {
            ActivityList = new ObservableCollection<ActivityVM>();
        }

        public void Add()
        {
                       
        }

        public ActivityVM GetActivity(int index)
        {
            return activityList[index];
        }

    }
}