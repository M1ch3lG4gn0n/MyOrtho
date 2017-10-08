using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyOrthoClient.Models
{
    class ListVM
    {
        List<ActivityVM> activityList;

        public ListVM()
        {
            activityList = new List<ActivityVM>();
        }
        
        public void Add(ActivityVM activity)
        {
            activityList.Add(activity);
        }

        public ActivityVM GetActivity(int index)
        {
            return activityList[index];
        }

    }
}