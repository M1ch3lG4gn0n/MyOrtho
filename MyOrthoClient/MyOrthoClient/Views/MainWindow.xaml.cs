using MyOrthoClient.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyOrthoClient.Controllers;

namespace MyOrthoClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ActivityExecuter ac;
        ListVM activityListInstance = new ListVM();        

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowState = WindowState.Normal;
            DataContext = activityListInstance;
        }

        private void BtnImporter_Click(object sender, RoutedEventArgs e)
        {
            string currentDir = Environment.CurrentDirectory;
            activityListInstance.ClearItems();

            //TODO: Activities dummies import 
            ActivityVM activityEx1 = new ActivityVM
            {
                Example_wav_path = currentDir + @"\Ressources\ex1.wav",
                Name = "Exercice 1",
                Pitch = 600,
                Intensity = 75
            };
            ActivityVM activityEx2 = new ActivityVM
            {
                Example_wav_path = currentDir + @"\Ressources\ex2.wav",
                Name = "Exercice 2",
                Pitch = 600,
                Intensity = 75
            };
            ActivityVM activityEx3 = new ActivityVM
            {
                Example_wav_path = currentDir + @"\Ressources\ex3.wav",
                Name = "Exercice 3",
                Pitch = 600,
                Intensity = 75
            };
            //

            activityListInstance.Add(activityEx1);
            activityListInstance.Add(activityEx2);
            activityListInstance.Add(activityEx3);
        }

        private void BtnLire_Click(object sender, RoutedEventArgs e)
        {
            ac.StartPlayback();

        }
        private void BtnArreter_Click(object sender, RoutedEventArgs e)
        {
            ac.StopPlayback();
        }
        private void BtnDemarrer_Click(object sender, RoutedEventArgs e)
        {
            ac.StartRecord();
        }
        private void BtnTerminer_Click(object sender, RoutedEventArgs e)
        {
            ac.StopRecord();
        }

        private void ListActivities_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentActivityIndex = ListActivities.SelectedIndex;
            var activity = activityListInstance.GetActivity(currentActivityIndex);
            activity.SetExerciseValue(values => SetChartLine((LineSeries)FrequencyChart.Series[0], (LineSeries)PitchChart.Series[0], values));
            activity.SetResultValue(values => SetChartLine((LineSeries)FrequencyChart.Series[1], (LineSeries)PitchChart.Series[1], values));
            ac = new ActivityExecuter(activity);
        }

        private void SetChartLine(LineSeries frequency, LineSeries pitch, ICollection<DataLineItem> values)
        {
            var frequencyLineArray = new KeyValuePair<double, double>[values.Count];
            var pitchLineArray = new KeyValuePair<double, double>[values.Count];
            int i = 0;
            foreach (var lineItem in values)
            {
                frequencyLineArray[i] = new KeyValuePair<double, double>(lineItem.time, lineItem.frequency);
                pitchLineArray[i++] = new KeyValuePair<double, double>(lineItem.time, lineItem.pitch);
            }
            frequency.ItemsSource = frequencyLineArray;
            pitch.ItemsSource = pitchLineArray;
        }
    }
}
