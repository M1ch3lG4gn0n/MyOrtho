using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MyOrthoOrtho.Models
{
    public class ApplicationVM : VMBase
    {
        SuiviVM VMPageSuivi = new SuiviVM();
        PreparationVM VMPagePreparation = new PreparationVM();
    }
}
