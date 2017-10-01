using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MyOrthoClient.Models
{
    class ListVM
    {
        List<ActivityVM> activityList;

        public ListVM()
        {
            activityList = new List<ActivityVM>();
        }

        private async void PopulateActivityList(string xmlDoc)
        {
            var docParsed = XDocument.Load(xmlDoc);
            

        }
    }
}
