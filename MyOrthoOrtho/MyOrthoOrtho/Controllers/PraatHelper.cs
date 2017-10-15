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

        public async Task<string> WriteScript(string wavPath, int pitch, int intensity, string resultPath)
        {
            return await(scripter.WriteScript(wavPath, pitch, intensity, resultPath));
        }

        public void GetResult(string script)
        {
            connector.GetResult(script);
        }
    }
}
