using Microsoft.WindowsAPICodePack.Dialogs;
using MyOrthoOrtho.Controllers;
using MyOrthoOrtho.Models;
using MyOrthoOrtho.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Xml.Linq;

namespace MyOrthoOrtho.Views.Controls
{
    /// <summary>
    /// Interaction logic for CtrlPreparation.xaml
    /// </summary>
    public partial class CtrlPreparation : UserControl
    {
        private PreparationExecuter pe;
        PreparationVM activityListInstance = new PreparationVM();
        static string TEMP_PATH = Path.GetTempPath() + "MyOrtho\\Preparation";

        WAVPlayerRecorder RecordPlayer;
        static string EXERCICES_FOLDER = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\SavedExercices";

        public CtrlPreparation()
        {
            InitializeComponent();
            DataContext = activityListInstance;
            ImportExistingExercices();
        }
       
        private void ImportExistingExercices()
        {
            ListAvailable.SelectedIndex = -1;
            ListSelected.SelectedIndex = -1;

            activityListInstance.ClearItems();
            activityListInstance.ClearSelectedItems();
            
            if (Directory.Exists(EXERCICES_FOLDER))
            {
                FileHelper.FileReader fileReader = new FileHelper.FileReader();
                fileReader.ReadAllExercicesIntoExerciceVMList(EXERCICES_FOLDER, activityListInstance.getActivityList());
                if(activityListInstance.getActivityList().Count > 0)
                {
                    lblAucunExercice.Visibility = Visibility.Hidden;
                }
                else
                {
                    lblAucunExercice.Visibility = Visibility.Visible;
                }
            }
            else
            {
                lblAucunExercice.Visibility = Visibility.Visible;
                //TODO: message indiquant qu'aucun exercice n'existe dans l'application
            }


        }

        private void BtnLire_Click(object sender, RoutedEventArgs e)
        {
            pe.StartPlaybackExemple();

        }
        private void BtnArreter_Click(object sender, RoutedEventArgs e)
        {
            pe.StopPlayback();
        }
        
        private void BtnDemarrer_Click(object sender, RoutedEventArgs e)
        {
            RecordPlayer = new WAVPlayerRecorder();
            var exerciceFolderPath = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\enregistrement\\";
            if (!Directory.Exists(exerciceFolderPath))
            {
                Directory.CreateDirectory(exerciceFolderPath);
            }
            string currentExerciceFilePath = (exerciceFolderPath + "exercice" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
            RecordPlayer.StartRecord(currentExerciceFilePath);
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
        

        private void ListAvailable_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListAvailable.SelectedIndex == -1)
            {
                btnAdd.IsEnabled = false;
                return;
            }
                
            var currentActivityIndex = ListAvailable.SelectedIndex;
            var activity = activityListInstance.GetActivity(currentActivityIndex);

            btnAdd.IsEnabled = true;

            ListSelected.SelectedIndex = -1;

            lblNom.Content = activity.Name;
            lblDuree.Content = activity.Duree_exacte;
            lblType.Content = activity.Type;

            activity.SetExerciseValue(values => SetChartLine((LineSeries)IntensityChart.Series[0], (LineSeries)PitchChart.Series[0], values));
            updateDataGrid(activity);

            
        }

        private void ListSelected_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListSelected.SelectedIndex == -1)
            {
                btnRemove.IsEnabled = false;
                return;
            }
            var currentActivityIndex = ListSelected.SelectedIndex;
            var activity = activityListInstance.GetSelectedActivity(currentActivityIndex);

            btnRemove.IsEnabled = true;

            ListAvailable.SelectedIndex = -1;

            lblNom.Content = activity.Name;
            lblDuree.Content = activity.Duree_exacte;
            lblType.Content = activity.Type;

            activity.SetExerciseValue(values => SetChartLine((LineSeries)IntensityChart.Series[0], (LineSeries)PitchChart.Series[0], values));
            updateDataGrid(activity);
        }
        

        private void updateDataGrid(ExerciceVM selectedExercise)
        {
            activityListInstance.ClearEvaluated();

            activityListInstance.AddEvaluated(new ExerciseEvaluationGridRow
            {
                Name = "F0 exact",
                Evaluated = selectedExercise.F0_exactEvaluated.ToString(),
                GoodMax = selectedExercise.F0_exact_good_max.ToString(),
                GoodMin = selectedExercise.F0_exact_good_min.ToString(),
                OkayMax = selectedExercise.F0_exact_okay_max.ToString(),
                OkayMin = selectedExercise.F0_exact_okay_min.ToString(),
                BadMax = selectedExercise.F0_exact_bad_max.ToString(),
                BadMin = selectedExercise.F0_exact_bad_min.ToString()
            });
            activityListInstance.AddEvaluated(new ExerciseEvaluationGridRow
            {
                Name = "F0 stable",
                Evaluated = selectedExercise.F0_stableEvaluated.ToString(),
                GoodMax = selectedExercise.F0_stable_good_max.ToString(),
                GoodMin = selectedExercise.F0_stable_good_min.ToString(),
                OkayMax = selectedExercise.F0_stable_okay_max.ToString(),
                OkayMin = selectedExercise.F0_stable_okay_min.ToString(),
                BadMax = selectedExercise.F0_stable_bad_max.ToString(),
                BadMin = selectedExercise.F0_stable_bad_min.ToString()
            });
            activityListInstance.AddEvaluated(new ExerciseEvaluationGridRow
            {
                Name = "Courbe F0 exact",
                Evaluated = selectedExercise.Courbe_f0_exacteEvaluated.ToString(),
                GoodMax = selectedExercise.Courbe_F0_exact_good_max.ToString(),
                GoodMin = selectedExercise.Courbe_F0_exact_good_min.ToString(),
                OkayMax = selectedExercise.Courbe_F0_exact_okay_max.ToString(),
                OkayMin = selectedExercise.Courbe_F0_exact_okay_min.ToString(),
                BadMax = selectedExercise.Courbe_F0_exact_bad_max.ToString(),
                BadMin = selectedExercise.Courbe_F0_exact_bad_min.ToString()
            });
            activityListInstance.AddEvaluated(new ExerciseEvaluationGridRow
            {
                Name = "Intensité stable",
                Evaluated = selectedExercise.Intensite_stableEvaluated.ToString(),
                GoodMax = selectedExercise.Intensite_stable_good_max.ToString(),
                GoodMin = selectedExercise.Intensite_stable_good_min.ToString(),
                OkayMax = selectedExercise.Intensite_stable_okay_max.ToString(),
                OkayMin = selectedExercise.Intensite_stable_okay_min .ToString(),
                BadMax = selectedExercise.Intensite_stable_bad_max.ToString(),
                BadMin = selectedExercise.Intensite_stable_bad_min.ToString()
            });
            activityListInstance.AddEvaluated(new ExerciseEvaluationGridRow
            {
                Name = "Durée exacte",
                Evaluated = selectedExercise.Duree_exacteEvaluated.ToString(),
                GoodMax = selectedExercise.Duree_good_max.ToString(),
                GoodMin = selectedExercise.Duree_good_min.ToString(),
                OkayMax = selectedExercise.Duree_okay_max.ToString(),
                OkayMin = selectedExercise.Duree_okay_min.ToString(),
                BadMax = selectedExercise.Duree_bad_max.ToString(),
                BadMin = selectedExercise.Duree_bad_min.ToString()
            });
            activityListInstance.AddEvaluated(new ExerciseEvaluationGridRow
            {
                Name = "Jitter",
                Evaluated = selectedExercise.JitterEvaluated.ToString(),
                GoodMax = selectedExercise.Jitter_good_max.ToString(),
                GoodMin = selectedExercise.Jitter_good_min.ToString(),
                OkayMax = selectedExercise.Jitter_okay_max.ToString(),
                OkayMin = selectedExercise.Jitter_okay_min.ToString(),
                BadMax = selectedExercise.Jitter_bad_max.ToString(),
                BadMin = selectedExercise.Jitter_bad_min.ToString()
            });

            //evaluatedDataGrid.ItemsSource = listOfEvaluatedParameters;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ImportExistingExercices();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(ListAvailable.SelectedIndex != -1)
            {
                activityListInstance.AddSelection(ListAvailable.SelectedItem);
                activityListInstance.Remove(ListAvailable.SelectedItem);
                ListSelected.SelectedIndex = ListSelected.Items.Count - 1;
                btnExporter.IsEnabled = true;
                
            }
            
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ListSelected.SelectedIndex != -1)
            {
                activityListInstance.Add(ListSelected.SelectedItem);
                activityListInstance.RemoveSelection(ListSelected.SelectedItem);
                ListAvailable.SelectedIndex = ListAvailable.Items.Count - 1;
                if (activityListInstance.SelectedActivityList.Count() == 0)
                {
                    btnExporter.IsEnabled = false;
                }

            }
        }

        private void btnExporter_Click(object sender, RoutedEventArgs e)
        {
            //TODO: exporter les exercices sélectionnées en incluant leur fichier xml, wav et txt, avec le xml les énumérant et le toute dans un zip.
            string targetDirectory = "";
            string dateExported = DateTime.Now.ToString("yyyyMMddHHmmss");
            string zipfolder = TEMP_PATH + "\\" + dateExported;
            CommonOpenFileDialog fd = new CommonOpenFileDialog();
            fd.Title = "Exporter une série d'exercices";
            fd.IsFolderPicker = true;
            fd.InitialDirectory = EXERCICES_FOLDER;

            if (fd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                targetDirectory = fd.FileName;
                
                Directory.CreateDirectory(targetDirectory);
                Directory.CreateDirectory(zipfolder);

                var activitiesConfig = new XmlHelper(false);
                var root = activitiesConfig.AddToRoot("Activities", string.Empty);
                activitiesConfig.AppendToNode(root, "Date", dateExported);
                foreach(var activity in activityListInstance.SelectedActivityList)
                {
                    activitiesConfig.ImportNode(root, XmlHelper.MakeActivityNode(activity));
                }
                activitiesConfig.Save(zipfolder + "\\config.xml");
                
                foreach (ExerciceVM selectedFile in ListSelected.Items)
                {
                    File.Copy(EXERCICES_FOLDER + "\\" + selectedFile.Example_wav_path, zipfolder + "\\" + selectedFile.Example_wav_path);
                }

                ZipFile.CreateFromDirectory(zipfolder, targetDirectory + "\\" + txtConfigName.Text + ".zip");

            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
