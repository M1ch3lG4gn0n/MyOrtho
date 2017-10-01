using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using MyOrthoClient.Models;

namespace MyOrthoClient.Controllers
{
    class ActivityHelper
    {
        public async void PopulateActivityList()
        {
            FileHelper.FileReader fr = new FileHelper.FileReader();
            var xmlDoc = XElement.Load(fr.ReadFile());
            List<XElement> activityNodes = xmlDoc.Descendants().Where(x => x.Name.ToString().Equals("Activity")).ToList();
            foreach (int elem in activityNodes)
            {
                var activity = new ActivityVM();
                
            }
        }
    }
}
