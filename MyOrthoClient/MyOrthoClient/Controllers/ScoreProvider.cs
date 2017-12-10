using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoClient.Controllers
{
    public static class ScoreProvider
    {
        private static Random random = new Random();

        public static int EvaluateJitter(double score)
        {
            return (int)((1d - score) * 100d);
        }

        public static int EvaluateTimeLength(double activityTime, double resultTime)
        {
            return (int)((resultTime * 100d) / (activityTime * 100d));
        }

        public static string ImageResult(int score)
        {
            var low = random.Next(0, 60);
            var high = random.Next(low, 100);

            if(score < low)
            {
                return RessourceService.FailImagePath;
            }

            if(score > high)
            {
                return RessourceService.GoodImagePath;
            }

            return RessourceService.OkImagePath;
        }
    }
}
