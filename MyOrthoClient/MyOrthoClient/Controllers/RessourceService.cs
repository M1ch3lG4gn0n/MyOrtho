using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyOrthoClient.Controllers
{
    public static class RessourceService
    {
        public static string GoodImagePath
        {
            get { return "Ressources\\SucceededFace.png"; }
        }

        public static string OkImagePath
        {
            get { return "Ressources\\PassedFace.png"; }
        }

        public static string FailImagePath
        {
            get { return "Ressources\\FailedFace.png"; }
        }

        public static string LoadingGifPath
        {
            get { return "Ressources\\Loading.gif"; }
        }

        public static string RecordingGifPath
        {
            get { return "Ressources\\rec.gif"; }
        }

        public static string PlayingGifPath
        {
            get { return "Ressources\\playing.gif"; }
        }
    }
}
