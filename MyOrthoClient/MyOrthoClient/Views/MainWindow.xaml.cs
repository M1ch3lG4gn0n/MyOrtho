using MyOrthoClient.Models;
using System;
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

            ((LineSeries)mcChart.Series[0]).ItemsSource = new KeyValuePair<DateTime, int>[]{
            new KeyValuePair<DateTime, int>(DateTime.Now, 100),
            new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(1), 130),
            new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(2), 150),
            new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(3), 125),
            new KeyValuePair<DateTime, int>(DateTime.Now.AddMonths(4),155) };

            DataContext = activityListInstance;
        }

        private void BtnImporter_Click(object sender, RoutedEventArgs e)
        {
        //TODO: Activities dummies import 
            ActivityVM activityEx1 = new ActivityVM
            {
                Example_wav_path = @"\Ressources\ex1.wav",
                Name = "Exercice 1"
            };
            ActivityVM activityEx2 = new ActivityVM
            {
                Example_wav_path = @"\Ressources\ex2.wav",
                Name = "Exercice 2"
            };
            ActivityVM activityEx3 = new ActivityVM
            {
                Example_wav_path = @"\Ressources\ex3.wav",
                Name = "Exercice 3"
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
