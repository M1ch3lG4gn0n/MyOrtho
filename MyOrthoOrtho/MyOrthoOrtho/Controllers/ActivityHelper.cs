using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using MyOrthoOrtho.ViewModels;

namespace MyOrthoOrtho.Controllers
{
    class ActivityHelper
    {
        public async void PopulateActivityList(ListSuiviVM instance)
        {

            /*FileHelper.FileReader fr = new FileHelper.FileReader();
            var xmlDoc = XElement.Load(fr.ReadFile());
            List<XElement> activityNodes = xmlDoc.Descendants().Where(x => x.Name.ToString().Equals("Activity")).ToList();
            for (var i=0; i>activityNodes.Count; i++)
            {
                var activity = new SuiviVM();
                activity.Name = activityNodes[i].Descendants("Name").ToString();
                activity.Example_wav_path = activityNodes[i].Descendants("Example_wav_path").ToString();
                activity.Pitch = Convert.ToInt32(activityNodes[i].Descendants("Pitch").ToString());
                activity.PitchEvaluated = Convert.ToBoolean(activityNodes[i].Descendants("PitchEvaluated").ToString());
                activity.Intensity = Convert.ToInt32(activityNodes[i].Descendants("Intensity").ToString());
                activity.IntensityEvaluated = Convert.ToBoolean(activityNodes[i].Descendants("IntensityEvaluated").ToString());
                activity.F0_exact = Convert.ToInt32(activityNodes[i].Descendants("F0_exact").ToString());
                activity.F0_exactEvaluated = Convert.ToBoolean(activityNodes[i].Descendants("F0_exactEvaluated").ToString());
                activity.F0_stable = Convert.ToInt32(activityNodes[i].Descendants("F0_stable").ToString());
                activity.F0_stableEvaluated = Convert.ToBoolean(activityNodes[i].Descendants("F0_stableEvaluated").ToString());
                activity.Intensite_stable = Convert.ToInt32(activityNodes[i].Descendants("Intensite_stable").ToString());
                activity.Intensite_stableEvaluated = Convert.ToBoolean(activityNodes[i].Descendants("Intensite_stableEvaluated").ToString());
                activity.Courbe_f0_exacte = Convert.ToInt32(activityNodes[i].Descendants("Courbe_f0_exacte").ToString());
                activity.Courbe_f0_exacteEvaluated = Convert.ToBoolean(activityNodes[i].Descendants("Courbe_f0_exacteEvaluated").ToString());
                activity.Duree_exacte = Convert.ToInt32(activityNodes[i].Descendants("Duree_exacte").ToString());
                activity.Duree_exacteEvaluated = Convert.ToBoolean(activityNodes[i].Descendants("Duree_exacteEvaluated").ToString());
                activity.Jitter = Convert.ToInt32(activityNodes[i].Descendants("Jitter").ToString());
                activity.JitterEvaluated = Convert.ToBoolean(activityNodes[i].Descendants("JitterEvaluated").ToString());

                instance.Add(activity);            
            }*/

        }


    }
}