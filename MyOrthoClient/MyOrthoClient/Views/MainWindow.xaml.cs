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
using System.Windows.Threading;

namespace MyOrthoClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SelectedItemJitter
        {
            get
            {
                var value = activityListInstance.GetActivity(0)?.Jitter.ToString();
                return value ?? string.Empty;
            }
        }

        private ActivityExecuter ac;
        ListVM activityListInstance = new ListVM();        

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowState = WindowState.Normal;
            DataContext = activityListInstance;

            BtnDemarrer.IsEnabled = false;
            BtnArreter.IsEnabled = false;
            BtnLire.IsEnabled = false;
            BtnTerminer.IsEnabled = false;
            BtnEcouter.IsEnabled = false;
            JitterTxt.IsEnabled = false;
        }

        private void BtnImporter_Click(object sender, RoutedEventArgs e)
        {
            BtnImporter.IsEnabled = false;
            //LoadingAdorner.IsAdornerVisible = !LoadingAdorner.IsAdornerVisible;
            string currentDir = Environment.CurrentDirectory;
            activityListInstance.ClearItems();

            //TODO: Activities dummies import 
            ActivityVM activityEx1 = new ActivityVM
            {
                Example_wav_path = currentDir + @"\Ressources\ex1.wav",
                Name = "Exercice 1",
                PitchMin = 70,
                PitchMax = 800,
                IntensityThreshold = 40
            };
            ActivityVM activityEx2 = new ActivityVM
            {
                Example_wav_path = currentDir + @"\Ressources\ex2.wav",
                Name = "Exercice 2",
                PitchMin = 70,
                PitchMax = 800,
                IntensityThreshold = 40
            };
            ActivityVM activityEx3 = new ActivityVM
            {
                Example_wav_path = currentDir + @"\Ressources\ex3.wav",
                Name = "Exercice 3",
                PitchMin = 70,
                PitchMax = 800,
                IntensityThreshold = 40
            };
            //

            activityListInstance.Add(activityEx1);
            activityListInstance.Add(activityEx2);
            activityListInstance.Add(activityEx3);
        }

        private void BtnLire_Click(object sender, RoutedEventArgs e)
        {
            BtnDemarrer.IsEnabled = false;
            BtnArreter.IsEnabled = true;
            BtnLire.IsEnabled = false;

            ac.StartPlayback(DisableButtons);
        }
        private void BtnArreter_Click(object sender, RoutedEventArgs e)
        {
            BtnDemarrer.IsEnabled = true;
            BtnArreter.IsEnabled = false;
            BtnLire.IsEnabled = true;

            ac.StopPlayback();
        }
        private void BtnDemarrer_Click(object sender, RoutedEventArgs e)
        {
            BtnDemarrer.IsEnabled = false;
            BtnLire.IsEnabled = false;
            BtnTerminer.IsEnabled = true;

            ac.StartRecord();
        }
        private void BtnTerminer_Click(object sender, RoutedEventArgs e)
        {
            ac.StopRecord();
            SoundAnalyser sa = new SoundAnalyser();
            string currentDir = Environment.CurrentDirectory;
            sa.CalculateCorrelation(activityListInstance.GetActivity(0).Exercice, activityListInstance.GetActivity(0).Results);

            Task.Factory.StartNew(() =>
            {
                //Update Text on the UI thread 
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input,
               new Action(() => { JitterTxt.Text = SelectedItemJitter; }));
            });

            BtnDemarrer.IsEnabled = true;
            BtnLire.IsEnabled = true;
            BtnTerminer.IsEnabled = false;
            BtnEcouter.IsEnabled = true;
        }
        private void BtnEcouter_Click(object sender, RoutedEventArgs e)
        {
            BtnDemarrer.IsEnabled = false;
            BtnArreter.IsEnabled = true;
            BtnLire.IsEnabled = false;
            BtnEcouter.IsEnabled = false;

            ac.StartLastExercicePlayblack(DisableButtons);
        }

        private void ListActivities_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentActivityIndex = ListActivities.SelectedIndex;
            var activity = activityListInstance.GetActivity(currentActivityIndex);
            activity.SetExerciseValue(values => SetChartLine((LineSeries)PitchChart.Series[0], (LineSeries)IntensityChart.Series[0], values));
            activity.SetResultValue(values => SetChartLine((LineSeries)PitchChart.Series[1], (LineSeries)IntensityChart.Series[1], values));
            ac = new ActivityExecuter(activity);
            BtnDemarrer.IsEnabled = true;
            BtnLire.IsEnabled = true;
        }

        private void SetChartLine(LineSeries frequency, LineSeries pitch, ICollection<DataLineItem> values)
        {
            var frequencyLineArray = new KeyValuePair<double, double>[values.Count];
            var pitchLineArray = new KeyValuePair<double, double>[values.Count];
            int i = 0;
            foreach (var lineItem in values)
            {
                frequencyLineArray[i] = new KeyValuePair<double, double>(lineItem.time, lineItem.Intensity);
                pitchLineArray[i++] = new KeyValuePair<double, double>(lineItem.time, lineItem.pitch);
            }
            this.Dispatcher.Invoke(() =>
            {
                frequency.ItemsSource = frequencyLineArray;
                pitch.ItemsSource = pitchLineArray;
            });
        }

        private void DisableButtons()
        {
            Task.Factory.StartNew(() =>
            {
                //Update Text on the UI thread 
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input,
               new Action(() => {
                   BtnDemarrer.IsEnabled = true;
                   BtnArreter.IsEnabled = false;
                   BtnLire.IsEnabled = true;
               }));
            });
        }
    }
}
