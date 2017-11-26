using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyOrthoClient.Models;

namespace MyOrthoClient.Views
{
    /// <summary>
    /// Interaction logic for CurveResult.xaml
    /// </summary>
    public partial class CurveResult : UserControl
    {
        private ActivityVM activity;
        private BitmapImage _CourbeF0ExacteImage;
        public BitmapImage CourbeF0ExacteImage { get { return _CourbeF0ExacteImage; } }

        public CurveResult(ActivityVM activity)
        {
            if (!activity.Courbe_f0_exacteEvaluated)
            {
                return;
            }

            this.activity = activity;

            _CourbeF0ExacteImage = this.ConvertToBitMap(activity.Courbe_f0_exacte);

            InitializeComponent();

            this.CourbeF0ExacteResult.Source = CourbeF0ExacteImage;
        }
    }
}
