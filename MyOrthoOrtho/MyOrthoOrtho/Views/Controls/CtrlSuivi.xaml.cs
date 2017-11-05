using MyOrthoOrtho.Controllers;
using MyOrthoOrtho.Models;
using MyOrthoOrtho.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;

namespace MyOrthoOrtho.Views.Controls
{
    /// <summary>
    /// Interaction logic for CtrlSuivi.xaml
    /// </summary>
    public partial class CtrlSuivi : UserControl
    {
        private SuiviExecuter ac;
        ListSuiviVM activityListInstance = new ListSuiviVM();


        public CtrlSuivi()
        {
            InitializeComponent();
            DataContext = activityListInstance;
        }

        private void BtnImporter_Click(object sender, RoutedEventArgs e)
        {
            string currentDir = Environment.CurrentDirectory;
            activityListInstance.ClearItems();

            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".xml";
            
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> fileSelected = dlg.ShowDialog();

            if (fileSelected == true)
            {
                string data;
                var streamReader = new StreamReader(dlg.FileName, Encoding.UTF8);
                data = streamReader.ReadToEnd();


                ListeResultatsModel result = new ListeResultatsModel();

                var serializer = new XmlSerializer(typeof(ListeResultatsModel));

                var stream = new StringReader(data);
                var reader = XmlReader.Create(stream);
                {
                    result = (ListeResultatsModel)serializer.Deserialize(reader);
                }
               foreach(ExerciceResultat ex in result.Liste_exercices_resultats)
                {
                    SuiviVM newSuiviVM = new SuiviVM
                    {
                        Example_wav_path = currentDir + ex.Exercice_wav_file_name,
                        Result_wav_path = currentDir + ex.,
                        Name = "Exercice 1",
                        PitchMin = 70,
                        PitchMax = 800,
                        IntensityThreshold = 40
                    };

                    activityListInstance.Add(newSuiviVM);
                }

            }


            /*

            //TODO: Activities dummies import 
            SuiviVM activityEx1 = new SuiviVM
            {
                Example_wav_path = currentDir + @"\Ressources\ex1.wav",
                Result_wav_path = currentDir + @"\Ressources\re1.wav",
                Name = "Exercice 1",
                PitchMin = 70,
                PitchMax = 800,
                IntensityThreshold = 40
            };
            SuiviVM activityEx2 = new SuiviVM
            {
                Example_wav_path = currentDir + @"\Ressources\ex2.wav",
                Result_wav_path = currentDir + @"\Ressources\re2.wav",
                Name = "Exercice 2",
                PitchMin = 70,
                PitchMax = 800,
                IntensityThreshold = 40
            };
            SuiviVM activityEx3 = new SuiviVM
            {
                Example_wav_path = currentDir + @"\Ressources\ex3.wav",
                Result_wav_path = currentDir + @"\Ressources\re3.wav",
                Name = "Exercice 3",
                PitchMin = 70,
                PitchMax = 800,
                IntensityThreshold = 40
            };
            //

                activityListInstance.Add(activityEx1);
                activityListInstance.Add(activityEx2);
                activityListInstance.Add(activityEx3);
                */
        }


        private void BtnLire_Click(object sender, RoutedEventArgs e)
        {
            ac.StartPlaybackExemple();

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
            activity.SetExerciseValue(values => SetChartLine((LineSeries)PitchChart.Series[0], (LineSeries)IntensityChart.Series[0], values));
            activity.SetResultValue(values => SetChartLine((LineSeries)PitchChart.Series[1], (LineSeries)IntensityChart.Series[1], values));
            ac = new SuiviExecuter(activity);
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
            this.Dispatcher.Invoke(() =>
            {
                frequency.ItemsSource = frequencyLineArray;
                pitch.ItemsSource = pitchLineArray;
            });
        }

        private void BtnLireResult_Click(object sender, RoutedEventArgs e)
        {
            ac.StartPlaybackResult();
        }
    }
}
