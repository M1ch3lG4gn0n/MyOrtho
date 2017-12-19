using Microsoft.Win32;
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
        SuiviVM activityListInstance = new SuiviVM();


        public CtrlSuivi()
        {
            InitializeComponent();
            DataContext = activityListInstance;
        }

        private void BtnImporter_Click(object sender, RoutedEventArgs e)
        {
            
                string path = "";
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() != null)
                {
                    path = file.FileName;
                }

                FileHelper.FileReader fileReader = new FileHelper.FileReader();
                fileReader.zipToExerciceList(path, activityListInstance);
            

/*

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
                //Create a StreamReader to read selected xml file
                var streamReader = new StreamReader(dlg.FileName, Encoding.UTF8);

                //Trim and clean the read data to ease parsing
                data = streamReader.ReadToEnd();
                data.Trim();
                data = data.Replace("\n", String.Empty).Replace("\t", String.Empty).Replace("\r", String.Empty);

                //create instance of our model
                ListeResultatsModel result = new ListeResultatsModel();

                //Setup our xml serializer and read xml data into our class
                var serializer = new XmlSerializer(typeof(ListeResultatsModel));
                var stream = new StringReader(data);
                var reader = XmlReader.Create(stream);
                {
                    result = (ListeResultatsModel)serializer.Deserialize(reader);
                }

                //create an object of type SuiviVM from the collected data
               foreach(ExerciceResultat ex in result.Liste_exercices_resultats)
                {
                    ExerciceVM newExerciceVM = new ExerciceVM
                    {
                        Example_wav_path = currentDir + @"\Ressources\" + ex.Exercice_wav_file_name,
                        Result_wav_path = currentDir + @"\Ressources\" + ex.Resultat_wav_file_name,
                        Name = ex.Name,
                        PitchMin = ex.PitchMin,
                        PitchMax = ex.PitchMax,
                        IntensityThreshold = ex.IntensityThreshold
                    };

                    activityListInstance.Add(newExerciceVM);
                }

            }
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
                frequencyLineArray[i] = new KeyValuePair<double, double>(lineItem.Time, lineItem.Intensity);
                pitchLineArray[i++] = new KeyValuePair<double, double>(lineItem.Time, lineItem.Pitch);
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
