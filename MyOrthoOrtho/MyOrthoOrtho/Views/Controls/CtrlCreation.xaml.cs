using MyOrthoOrtho.Controllers;
using MyOrthoOrtho.ViewModels;
using MyOrthoOrtho.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Linq;

namespace MyOrthoOrtho.Views.Controls
{
    /// <summary>
    /// Interaction logic for CtrlPreparation.xaml
    /// </summary>
    public partial class CtrlCreation : UserControl
    {
        private CreationExecuter ce;
        WAVPlayerRecorder RecordPlayer;
        CreationVM ActivityListInstance = new CreationVM();
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
            DataContext = ActivityListInstance;
            ImportExistingExercices();
        }

        private void ImportExistingExercices()
        {
            ListActivities.SelectedIndex = -1;

            ActivityListInstance.ClearItems();

            if (Directory.Exists(EXERCICES_FOLDER))
            {
                FileHelper.FileReader fileReader = new FileHelper.FileReader();
                fileReader.ReadAllExercicesIntoExerciceVMList(EXERCICES_FOLDER, ActivityListInstance.getActivityList());
                
            }
            


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

            string type = (bool)radTypeLigne.IsChecked ? "Droite" : "Courbe";

            int pitchMax;
            int pitchMin;
            int intensityThreshold;
            int duree;
            int.TryParse(txtPitchMax.Text, out pitchMax);
            int.TryParse(txtPitchMin.Text, out pitchMin);
            int.TryParse(txtIntensityThreshold.Text, out intensityThreshold);
            int.TryParse(txtDuration.Text, out duree);

            if (!Directory.Exists(EXERCICES_FOLDER))
            {
                Directory.CreateDirectory(EXERCICES_FOLDER);
            }

            File.Copy(tempExerciceWavPath, savedWavPath);
            File.Copy(tempExercicePraatResultsPath, savedPraatResultsPath);

            var activityVM = new ExerciceVM
            {
                Name = txtName.Text,
                Example_wav_path = wavFileName,
                Exercice = ce.EnumerateCurrentPoints(),
                PitchMin = pitchMin,
                PitchMax = pitchMax,
                IntensityThreshold = intensityThreshold,
                F0_exactEvaluated = chkF0ExacteEvaluated.IsChecked.Value,
                Courbe_f0_exacteEvaluated = chkCourbeF0ExacteEvaluated.IsChecked.Value,
                F0_stableEvaluated = chkF0StableEvaluated.IsChecked.Value,
                Intensite_stableEvaluated = chkIntensiteStableEvaluated.IsChecked.Value,
                Duree_exacteEvaluated = chkDurationEvaluated.IsChecked.Value,
                Duree_exacte = duree,
                JitterEvaluated = chkJitterEvaluated.IsChecked.Value,
                F0_exact_good_max = decimal.Parse(txtF0ExactGoodMax.Text),
                F0_exact_good_min = decimal.Parse(txtF0ExactGoodMin.Text),
                F0_exact_okay_max = decimal.Parse(txtF0ExactOkayMax.Text),
                F0_exact_okay_min = decimal.Parse(txtF0ExactOkayMin.Text),
                F0_exact_bad_max = decimal.Parse(txtF0ExactBadMax.Text),
                F0_exact_bad_min = decimal.Parse(txtF0ExactBadMin.Text),
                F0_stable_good_max = decimal.Parse(txtF0StableGoodMax.Text),
                F0_stable_good_min = decimal.Parse(txtF0StableGoodMin.Text),
                F0_stable_okay_max = decimal.Parse(txtF0StableOkayMax.Text),
                F0_stable_okay_min = decimal.Parse(txtF0StableOkayMin.Text),
                F0_stable_bad_max = decimal.Parse(txtF0StableBadMax.Text),
                F0_stable_bad_min = decimal.Parse(txtF0StableBadMin.Text),
                Intensite_stable_good_max = decimal.Parse(txtIntensiteStableGoodMax.Text),
                Intensite_stable_good_min = decimal.Parse(txtIntensiteStableGoodMin.Text),
                Intensite_stable_okay_max = decimal.Parse(txtIntensiteStableOkayMax.Text),
                Intensite_stable_okay_min = decimal.Parse(txtIntensiteStableOkayMin.Text),
                Intensite_stable_bad_max = decimal.Parse(txtIntensiteStableBadMax.Text),
                Intensite_stable_bad_min = decimal.Parse(txtIntensiteStableBadMin.Text),
                Courbe_F0_exact_good_max = decimal.Parse(txtCourbeF0ExactGoodMax.Text),
                Courbe_F0_exact_good_min = decimal.Parse(txtCourbeF0ExactGoodMin.Text),
                Courbe_F0_exact_okay_max = decimal.Parse(txtCourbeF0ExactOkayMax.Text),
                Courbe_F0_exact_okay_min = decimal.Parse(txtCourbeF0ExactOkayMin.Text),
                Courbe_F0_exact_bad_max = decimal.Parse(txtCourbeF0ExactBadMax.Text),
                Courbe_F0_exact_bad_min = decimal.Parse(txtCourbeF0ExactBadMin.Text),
                Duree_good_max = decimal.Parse(txtDurationGoodMax.Text),
                Duree_good_min = decimal.Parse(txtDurationGoodMin.Text),
                Duree_okay_max = decimal.Parse(txtDurationOkayMax.Text),
                Duree_okay_min = decimal.Parse(txtDurationOkayMin.Text),
                Duree_bad_max = decimal.Parse(txtDurationBadMax.Text),
                Duree_bad_min = decimal.Parse(txtDurationBadMin.Text),
                Jitter_good_max = decimal.Parse(txtJitterGoodMax.Text),
                Jitter_good_min = decimal.Parse(txtJitterGoodMin.Text),
                Jitter_okay_max = decimal.Parse(txtJitterOkayMax.Text),
                Jitter_okay_min = decimal.Parse(txtJitterOkayMin.Text),
                Jitter_bad_max = decimal.Parse(txtJitterBadMax.Text),
                Jitter_bad_min = decimal.Parse(txtJitterBadMin.Text)
            };

            var configFile = new XmlHelper(false);

            configFile.ImportNode(configFile.GetRoot() ,XmlHelper.MakeActivityNode(activityVM));

            configFile.Save(targetXMLPath);
            
            ImportExistingExercices();
            ActivityListInstance.SetDefaultExerciceValues();
            DataContext = null;
            DataContext = ActivityListInstance;
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
                recordStartDate = DateTime.Now.ToString("yyyyMMddHHmmss");

                string[] directories = filename.Split('\\');
                currentExerciceFileName = directories[directories.Length - 1];
                currentExerciceFilePath = filename;

                tempExerciceWavPath = filename;

                UpdateChartsAndActivity();

                BtnCreerExercice.IsEnabled = true;
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

            BtnCreerExercice.IsEnabled = true;
        }

        private void UpdateChartsAndActivity()
        {
            ExerciceVM activity = ActivityListInstance.CurrentExercice;
            activity.Date = recordStartDate;
            activity.Example_wav_path = txtFileName.Text;

            /*ExerciceVM activity = new ExerciceVM
            {
                Example_wav_path = txtFileName.Text,
                Date = recordStartDate,
                Name = txtName.Text,
                PitchMin = Convert.ToInt32(txtPitchMin.Text),
                PitchMax = Convert.ToInt32(txtPitchMax.Text),
                IntensityThreshold = Convert.ToInt32(txtIntensityThreshold.Text),
                Duree_exacte = Convert.ToInt32(txtDuration.Text)
            };*/

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
                frequencyLineArray[i] = new KeyValuePair<double, double>(lineItem.Time, lineItem.Intensity);
                pitchLineArray[i++] = new KeyValuePair<double, double>(lineItem.Time, lineItem.Pitch);
            }
            this.Dispatcher.Invoke(() =>
            {
                frequency.ItemsSource = frequencyLineArray;
                pitch.ItemsSource = pitchLineArray;
            });
        }

        private void ListActivities_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ListActivities.SelectedIndex == -1)
            {
                btnDeleteExercice.IsEnabled = false;
                btnModifExercice.IsEnabled = false;
                return;
            }
            
            btnDeleteExercice.IsEnabled = true;
            btnModifExercice.IsEnabled = true;
        }

        private void TxtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadTypeLigne_Checked(object sender, RoutedEventArgs e)
        {
            chkCourbeF0ExacteEvaluated.IsEnabled = false;
            chkF0ExacteEvaluated.IsEnabled = true;
            chkF0StableEvaluated.IsEnabled = true;

            txtCourbeF0ExactBadMax.IsEnabled = false;
            txtCourbeF0ExactOkayMax.IsEnabled = false;
            txtCourbeF0ExactGoodMax.IsEnabled = false;
            txtCourbeF0ExactBadMin.IsEnabled = false;
            txtCourbeF0ExactOkayMin.IsEnabled = false;
            txtCourbeF0ExactGoodMin.IsEnabled = false;

            txtF0ExactBadMax.IsEnabled = true;
            txtF0ExactOkayMax.IsEnabled = true;
            txtF0ExactGoodMax.IsEnabled = true;
            txtF0ExactBadMin.IsEnabled = true;
            txtF0ExactOkayMin.IsEnabled = true;
            txtF0ExactGoodMin.IsEnabled = true;

            txtF0StableBadMax.IsEnabled = true;
            txtF0StableOkayMax.IsEnabled = true;
            txtF0StableGoodMax.IsEnabled = true;
            txtF0StableBadMin.IsEnabled = true;
            txtF0StableOkayMin.IsEnabled = true;
            txtF0StableGoodMin.IsEnabled = true;
        }

        private void RadTypeCourbe_Checked(object sender, RoutedEventArgs e)
        {
            
            chkF0ExacteEvaluated.IsEnabled = false;
            chkF0StableEvaluated.IsEnabled = false;
            chkCourbeF0ExacteEvaluated.IsEnabled = true;

            txtF0ExactBadMax.IsEnabled = false;
            txtF0ExactOkayMax.IsEnabled = false;
            txtF0ExactGoodMax.IsEnabled = false;
            txtF0ExactBadMin.IsEnabled = false;
            txtF0ExactOkayMin.IsEnabled = false;
            txtF0ExactGoodMin.IsEnabled = false;

            txtF0StableBadMax.IsEnabled = false;
            txtF0StableOkayMax.IsEnabled = false;
            txtF0StableGoodMax.IsEnabled = false;
            txtF0StableBadMin.IsEnabled = false;
            txtF0StableOkayMin.IsEnabled = false;
            txtF0StableGoodMin.IsEnabled = false;

            txtCourbeF0ExactBadMax.IsEnabled = true;
            txtCourbeF0ExactOkayMax.IsEnabled = true;
            txtCourbeF0ExactGoodMax.IsEnabled = true;
            txtCourbeF0ExactBadMin.IsEnabled = true;
            txtCourbeF0ExactOkayMin.IsEnabled = true;
            txtCourbeF0ExactGoodMin.IsEnabled = true;

        }

        private void BtnDeleteExercice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnModifExercice_Click(object sender, RoutedEventArgs e)
        {

            var currentActivityIndex = ListActivities.SelectedIndex;
            var activity = ActivityListInstance.GetActivity(currentActivityIndex);
            
            ActivityListInstance.UpdateActivity(activity);

            ActivityListInstance.CurrentExercice.SetExerciseValue(values => SetChartLine((LineSeries)IntensityChart.Series[0], (LineSeries)PitchChart.Series[0], values));
            DataContext = null;
            DataContext = ActivityListInstance;
            //ce = new CreationExecuter(activity);
        }
    }
}
