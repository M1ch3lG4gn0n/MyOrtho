using MyOrthoOrtho.Controllers;
using MyOrthoOrtho.Models;
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

namespace MyOrthoOrtho.Views.Controls
{
    /// <summary>
    /// Interaction logic for CtrlSuivi.xaml
    /// </summary>
    public partial class CtrlSuivi : UserControl
    {
        private ActivityExecuter ac;
        SuiviVM activityListInstance = new SuiviVM();


        public CtrlSuivi()
        {
            InitializeComponent();
            DataContext = activityListInstance;
        }

        private void ListActivities_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*var currentActivityIndex = ListActivities.SelectedIndex;
            var activity = activityListInstance.GetActivity(currentActivityIndex);
            activity.SetExerciseValue(values => SetChartLine((LineSeries)FrequencyChart.Series[0], (LineSeries)PitchChart.Series[0], values));
            activity.SetResultValue(values => SetChartLine((LineSeries)FrequencyChart.Series[1], (LineSeries)PitchChart.Series[1], values));
            ac = new ActivityExecuter(activity);*/
        }
    }
}
