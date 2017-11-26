using MyOrthoOrtho.Controllers;
using MyOrthoOrtho.ViewModels;
using MyOrthoOrtho.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.IO;

namespace MyOrthoOrtho.Views.Controls
{
    /// <summary>
    /// Interaction logic for CtrlPreparation.xaml
    /// </summary>
    public partial class CtrlCreation : UserControl
    {
        private CreationExecuter ce;
        WAVPlayerRecorder RecordPlayer;
        static string EXERCICES_FOLDER = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\SavedExercices";
        static string TEMP_PATH = Path.GetTempPath() + "MyOrtho";

        string recordStartDate;

        string currentExerciceFilePath;
        string currentExerciceFileName;

        string tempExerciceWavPath;
        string tempExercicePraatResultsPath;

        public CtrlCreation()
        {
            InitializeComponent();
           
        }

        private void BtnCreerExercice_Click(object sender, RoutedEventArgs e)
        {
            //TODO: enregistrer le fichier comme exercice

            tempExercicePraatResultsPath = ce.TempExPraatResultsPath;

            string xmlFileName = (txtName.Text + "_" + recordStartDate + ".xml").Replace(' ', '_');
            string wavFileName = (txtName.Text + "_recording_" + recordStartDate + ".wav").Replace(' ', '_');
            string txtFileName = (txtName.Text + "_praat_" + recordStartDate + ".txt").Replace(' ', '_');

            string targetXMLPath = EXERCICES_FOLDER + "\\" + xmlFileName;
            string savedWavPath = EXERCICES_FOLDER + "\\" + wavFileName;
            string savedPraatResultsPath = EXERCICES_FOLDER + "\\" + txtFileName;

            int pitchMin = 0;
            int pitchMax = 0;
            int intensityThreshold = 0;

            int.TryParse(txtPitchMax.Text, out pitchMax);
            int.TryParse(txtPitchMin.Text, out pitchMin);
            int.TryParse(txtIntensityThreshold.Text, out intensityThreshold);

            File.Copy(tempExerciceWavPath, savedWavPath);
            File.Copy(tempExercicePraatResultsPath, savedPraatResultsPath);

            Exercice nouvelExercice = new Exercice
            {
                Date = recordStartDate,
                Name = txtName.Text,
                Exercice_wav_file_name = wavFileName,
                Exercice_praat_file_name = txtFileName,
                PitchMax = pitchMax,
                PitchMin = pitchMin,
                IntensityThreshold = intensityThreshold
            };


            System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(Exercice));

            var path = targetXMLPath;
            FileStream file = File.Create(targetXMLPath);

            writer.Serialize(file, nouvelExercice);
            file.Close();

        }


        private void BtnLire_Click(object sender, RoutedEventArgs e)
        {
            ce.StartPlaybackExemple();

        }
        private void BtnArreter_Click(object sender, RoutedEventArgs e)
        {
            ce.StopPlayback();
        }

        private void btnImporterExercice_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".wav",
                Filter = "WAV File (.wav)|*.wav"
            };
            if (fileDialog.ShowDialog() == true)
            {
                string filename = fileDialog.FileName;
                txtFileName.Text = filename;
            }
        }

        private void BtnDemarrer_Click(object sender, RoutedEventArgs e)
        {
            RecordPlayer = new WAVPlayerRecorder();
            if (!Directory.Exists(TEMP_PATH))
            {
                Directory.CreateDirectory(TEMP_PATH);
            }
            recordStartDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            currentExerciceFileName = "\\TempRecording_" + recordStartDate;
            currentExerciceFilePath = (TEMP_PATH + currentExerciceFileName);
            

            RecordPlayer.StartRecord(currentExerciceFilePath);
            imgRec.Visibility = Visibility.Visible;

            BtnDemarrer.IsEnabled = false;
            BtnTerminer.IsEnabled = true;
        }
        private async void BtnTerminer_Click(object sender, RoutedEventArgs e)
        {
            var wavPath = await RecordPlayer.StopRecord();
            tempExerciceWavPath = wavPath;
            txtFileName.Text = wavPath;
            imgRec.Visibility = Visibility.Hidden;
            UpdateChartsAndActivity();

            BtnTerminer.IsEnabled = false;
            BtnDemarrer.IsEnabled = true;
        }

        private void UpdateChartsAndActivity()
        {
            CreationVM activity = new CreationVM
            {
                Example_wav_path = txtFileName.Text,
                Date = recordStartDate,
                Name = txtName.Text,
                PitchMin = Convert.ToInt32(txtPitchMin.Text),
                PitchMax = Convert.ToInt32(txtPitchMax.Text),
                IntensityThreshold = Convert.ToInt32(txtIntensityThreshold.Text),
                Duree_expected = Convert.ToInt32(txtDuration.Text)
            };

            activity.SetExerciseValue(values => SetChartLine((LineSeries)PitchChart.Series[0], (LineSeries)IntensityChart.Series[0], values));
            ce = new CreationExecuter(activity);

            tempExercicePraatResultsPath = ce.TempExPraatResultsPath;
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

        private void ListActivities_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
