using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoOrtho.Controllers
{
    public class PraatHelper
    {
        private PraatConnector connector;
        private PraatScripting scripter;

        public PraatHelper(string activityName)
        {
            connector = PraatConnector.GetConnector();
            scripter = new PraatScripting(activityName);
        }

        public string WriteIntensityFrequencyScript(string wavPath, int pitchMin, int pitchMax, int intensityThreshold, string resultPath)
        {
            return scripter.WriteIntensityFrequencyScript(wavPath, pitchMin, pitchMax, intensityThreshold, resultPath);
        }

        public void GetResult(string script)
        {
            connector.GetResult(script);
        }
    }
}
