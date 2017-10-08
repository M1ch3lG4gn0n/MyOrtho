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

            ActivityVM activity = new ActivityVM();
            activity.Example_wav_path = "./Ressources/truc.wav";
            activity.Name = "Test";
            activity.Pitch = 600;
            activity.Intensity = 75;
            activityListInstance.Add(activity);

            //this.DataContext = activityListInstance;
            ac = new ActivityExecuter(activityListInstance.GetActivity(0));
            ac.StopRecord();
            var results = activityListInstance.GetActivity(0).Results;
            var dataSourceList = new List<List<KeyValuePair<double, double>>>();
            List<KeyValuePair<double, double>> intensity = new List<KeyValuePair<double, double>>();
            List<KeyValuePair<double, double>> frequency = new List<KeyValuePair<double, double>>();

            foreach (var line in results)
            {
                intensity.Add(new KeyValuePair<double, double>(line.time, line.pitch));
                frequency.Add(new KeyValuePair<double, double>(line.time, line.frequency));
            }

            dataSourceList.Add(intensity);
            dataSourceList.Add(frequency);
            mcChart.DataContext = dataSourceList;
        }

        private void BtnImporter_Click(object sender, RoutedEventArgs e)
        {
            string currentDir = Environment.CurrentDirectory;

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
            ac = new ActivityExecuter(activityListInstance.GetActivity(currentActivityIndex));
        }
    }
}
