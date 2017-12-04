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
            
            int.TryParse(txtPitchMax.Text, out int pitchMax);
            int.TryParse(txtPitchMin.Text, out int pitchMin);
            int.TryParse(txtIntensityThreshold.Text, out int intensityThreshold);
            int.TryParse(txtDuration.Text, out int duree);

            if (!Directory.Exists(EXERCICES_FOLDER))
            {
                Directory.CreateDirectory(EXERCICES_FOLDER);
            }

            File.Copy(tempExerciceWavPath, savedWavPath);
            File.Copy(tempExercicePraatResultsPath, savedPraatResultsPath);


            XDocument doc =
                new XDocument(
                    new XElement("Exercice",
                        new XElement("Date", recordStartDate),
                        new XElement("Name", txtName.Text),
                        new XElement("Exercice_wav_file_name", wavFileName),
                        new XElement("Exercice_praat_file_name", txtFileName),
                        new XElement("Pitch_min", pitchMin),
                        new XElement("Pitch_max", pitchMax),
                        new XElement("Intensity_threshold", intensityThreshold),
                        new XElement("Duree", duree),
                        new XElement("F0_exactEvaluated", chkF0ExacteEvaluated.IsChecked.ToString()),
                        new XElement("F0_stableEvaluated", chkF0StableEvaluated.IsChecked.ToString()),
                        new XElement("Intensite_stableEvaluated", chkIntensiteStableEvaluated.IsChecked.ToString()),
                        new XElement("Courbe_f0_exacteEvaluated", chkCourbeF0ExacteEvaluated.IsChecked.ToString()),
                        new XElement("Duree_exacteEvaluated", chkDurationEvaluated.IsChecked.ToString()),
                        new XElement("JitterEvaluated", chkJitterEvaluated.IsChecked.ToString()),
                        new XElement("F0_exacte_evaluation",
                            new XElement("Good", 
                                new XElement("Max", txtF0ExactGoodMax.Text),
                                new XElement("Min", txtF0ExactGoodMin.Text)
                                ),
                            new XElement("Okay",
                                new XElement("Max", txtF0ExactOkayMax.Text),
                                new XElement("Min", txtF0ExactOkayMin.Text)
                                ),
                            new XElement("Bad",
                                new XElement("Max", txtF0ExactBadMax.Text),
                                new XElement("Min", txtF0ExactBadMin.Text)
                                )
                            ),
                        new XElement("F0_stable_evaluation",
                            new XElement("Good",
                                new XElement("Max", txtF0StableGoodMax.Text),
                                new XElement("Min", txtF0StableGoodMin.Text)
                                ),
                            new XElement("Okay",
                                new XElement("Max", txtF0StableOkayMax.Text),
                                new XElement("Min", txtF0StableOkayMin.Text)
                                ),
                            new XElement("Bad",
                                new XElement("Max", txtF0StableBadMax.Text),
                                new XElement("Min", txtF0StableBadMin.Text)
                                )
                            ),
                        new XElement("Intensite_stable_evaluation",
                            new XElement("Good",
                                new XElement("Max", txtIntensiteStableGoodMax.Text),
                                new XElement("Min", txtIntensiteStableGoodMin.Text)
                                ),
                            new XElement("Okay",
                                new XElement("Max", txtIntensiteStableOkayMax.Text),
                                new XElement("Min", txtIntensiteStableOkayMin.Text)
                                ),
                            new XElement("Bad",
                                new XElement("Max", txtIntensiteStableBadMax.Text),
                                new XElement("Min", txtIntensiteStableBadMin.Text)
                                )
                            ),
                        new XElement("Courbe_F0_exacte_evaluation",
                            new XElement("Good",
                                new XElement("Max", txtCourbeF0ExactGoodMax.Text),
                                new XElement("Min", txtCourbeF0ExactGoodMin.Text)
                                ),
                            new XElement("Okay",
                                new XElement("Max", txtCourbeF0ExactOkayMax.Text),
                                new XElement("Min", txtCourbeF0ExactOkayMin.Text)
                                ),
                            new XElement("Bad",
                                new XElement("Max", txtCourbeF0ExactBadMax.Text),
                                new XElement("Min", txtCourbeF0ExactBadMin.Text)
                                )
                            ),
                        new XElement("Duree_exacte_evaluation",
                            new XElement("Good",
                                new XElement("Max", txtDurationGoodMax.Text),
                                new XElement("Min", txtDurationGoodMin.Text)
                                ),
                            new XElement("Okay",
                                new XElement("Max", txtDurationOkayMax.Text),
                                new XElement("Min", txtDurationOkayMin.Text)
                                ),
                            new XElement("Bad",
                                new XElement("Max", txtDurationBadMax.Text),
                                new XElement("Min", txtDurationBadMin.Text)
                                )
                            ),
                        new XElement("Jitter_evaluation",
                            new XElement("Good",
                                new XElement("Max", txtJitterGoodMax.Text),
                                new XElement("Min", txtJitterGoodMin.Text)
                                ),
                            new XElement("Okay",
                                new XElement("Max", txtJitterOkayMax.Text),
                                new XElement("Min", txtJitterOkayMin.Text)
                                ),
                            new XElement("Bad",
                                new XElement("Max", txtJitterBadMax.Text),
                                new XElement("Min", txtJitterBadMin.Text)
                                )
                            )

                        )
        );

            doc.Save(targetXMLPath);



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
            ExerciceVM activity = new ExerciceVM
            {
                Example_wav_path = txtFileName.Text,
                Date = recordStartDate,
                Name = txtName.Text,
                PitchMin = Convert.ToInt32(txtPitchMin.Text),
                PitchMax = Convert.ToInt32(txtPitchMax.Text),
                IntensityThreshold = Convert.ToInt32(txtIntensityThreshold.Text),
                Duree_exacte = Convert.ToInt32(txtDuration.Text)
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

        }
    }
}
