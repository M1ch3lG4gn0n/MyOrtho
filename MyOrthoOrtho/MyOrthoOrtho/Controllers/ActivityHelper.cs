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
    public class ActivityHelper
    {
       
        public static string FormatDateString(string dateString)
        {
            string formattedDate = "";
            string year = dateString.Substring(0, 4);
            string month = dateString.Substring(4, 2);
            int day = int.Parse(dateString.Substring(6, 2));
            int hour = int.Parse(dateString.Substring(8, 2));
            string minute = dateString.Substring(10, 2);
            string seconds = dateString.Substring(12, 2);
            

            formattedDate = "Le " + day.ToString() + " " + getMonth(month) + " " + year + " à " + hour.ToString() + "h" + minute;
            return formattedDate;
        }

        private static string getMonth(string number)
        {
            switch(number){
                case "01":
                    return "Janvier";
                    break;
                case "02":
                    return "Février";
                    break;
                case "03":
                    return "Mars";
                    break;
                case "04":
                    return "Avril";
                    break;
                case "05":
                    return "Mai";
                    break;
                case "06":
                    return "Juin";
                    break;
                case "07":
                    return "Juillet";
                    break;
                case "08":
                    return "Août";
                    break;
                case "09":
                    return "Septembre";
                    break;
                case "10":
                    return "Octobre";
                    break;
                case "11":
                    return "Novembre";
                    break;
                case "12":
                    return "Décembre";
                    break;
                default:
                    return "";
                    break;
            }
        }

    }
}