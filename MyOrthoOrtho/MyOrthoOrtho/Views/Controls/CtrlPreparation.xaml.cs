using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MyOrthoOrtho.Controllers;
using MyOrthoOrtho.Models;
using MyOrthoOrtho.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MyOrthoOrtho.Views.Controls
{
    /// <summary>
    /// Interaction logic for CtrlPreparation.xaml
    /// </summary>
    public partial class CtrlPreparation : UserControl
    {
        private PreparationExecuter pe;
        PreparationVM activityListInstance = new PreparationVM();
        
        
        WAVPlayerRecorder RecordPlayer;
        static string EXERCICES_FOLDER = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\SavedExercices";

        public CtrlPreparation()
        {
            InitializeComponent();
            DataContext = activityListInstance;
            ImportExistingExercices();
        }
        
        private void ImportExistingExercicesSerialized()
        {

            activityListInstance.ClearItems();
            string data;
            if (Directory.Exists(EXERCICES_FOLDER))
            {
                foreach (string filePath in Directory.GetFiles(EXERCICES_FOLDER))
                {
                    if (System.IO.Path.GetExtension(filePath) == ".xml")
                    {
                        var streamReader = new StreamReader(filePath, Encoding.UTF8);
                        //Trim and clean the read data to ease parsing
                        data = streamReader.ReadToEnd();
                        data.Trim();
                        data = data.Replace("\n", String.Empty).Replace("\t", String.Empty).Replace("\r", String.Empty);

                        //create instance of our model
                        Exercice exercice = new Exercice();

                        //Setup our xml serializer and read xml data into our class
                        var serializer = new XmlSerializer(typeof(Exercice));
                        var stream = new StringReader(data);
                        var reader = XmlReader.Create(stream);
                        {
                            exercice = (Exercice)serializer.Deserialize(reader);
                        }

                        //TODO: ajouter le path du fichier praat
                        ICollection<DataLineItem> praatResultValues = DataExtractor.GetInstance().GetFileValues(EXERCICES_FOLDER + "\\" + exercice.Exercice_praat_file_name);
                        ExerciceVM prep = new ExerciceVM
                        {
                            Name = exercice.Name,
                            Example_wav_path = exercice.Exercice_wav_file_name,
                            Exercice = praatResultValues,
                            
                        };
                        activityListInstance.Add(prep);

                    }   
                }
            }
            else
            {
                //TODO: message indiquant qu'aucun exercice n'existe dans l'application
            }
            
            
        }

        private void ImportExistingExercices()
        {
            ListAvailable.SelectedIndex = -1;
            activityListInstance.ClearItems();
            
            if (Directory.Exists(EXERCICES_FOLDER))
            {
                FileHelper.FileReader fileReader = new FileHelper.FileReader();
                fileReader.ReadAllExercicesIntoPreparationVMList(EXERCICES_FOLDER, activityListInstance.getActivityList());
                
            }
            else
            {
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
                frequencyLineArray[i] = new KeyValuePair<double, double>(lineItem.time, lineItem.frequency);
                pitchLineArray[i++] = new KeyValuePair<double, double>(lineItem.time, lineItem.pitch);
            }
            this.Dispatcher.Invoke(() =>
            {
                frequency.ItemsSource = frequencyLineArray;
                pitch.ItemsSource = pitchLineArray;
            });
        }
        

        private void ListAvailable_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentActivityIndex = ListAvailable.SelectedIndex;
            var activity = activityListInstance.GetActivity(currentActivityIndex);
            activity.SetExerciseValue(values => SetChartLine((LineSeries)IntensityChart.Series[0], (LineSeries)PitchChart.Series[0], values));
            updateDataGrid(activity);
            
        }

        private void ListSelected_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentActivityIndex = ListSelected.SelectedIndex;
            var activity = activityListInstance.GetSelectedActivity(currentActivityIndex);
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
            activityListInstance.AddSelection(ListAvailable.SelectedItem);
            activityListInstance.Remove(ListAvailable.SelectedItem);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            activityListInstance.Add(ListSelected.SelectedItem);
            activityListInstance.RemoveSelection(ListSelected.SelectedItem);
        }

        private void btnExporter_Click(object sender, RoutedEventArgs e)
        {
            //TODO: exporter les exercices sélectionnées en incluant leur fichier xml, wav et txt, avec le xml les énumérant et le toute dans un zip.
            string targetDirectory = "";
            CommonOpenFileDialog fd = new CommonOpenFileDialog();
            fd.Title = "Exporter une série d'exercices";
            fd.IsFolderPicker = true;
            fd.InitialDirectory = EXERCICES_FOLDER;

            if (fd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                targetDirectory = fd.FileName;
           

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }
            
            XDocument doc =
                new XDocument(
                    new XElement("Activities",
                        new XElement("Date", DateTime.Now.ToString("yyyyMMddHHmmss")),
                            activityListInstance.SelectedActivityList.Select(x => new XElement("Activity",
                                new XElement("Name", x.Name),
                                new XElement("Exercice_wav_file_name", x.Example_wav_path),
                                new XElement("Exercice_praat_file_name", x.Example_praat_path),
                                new XElement("Pitch_min", x.PitchMin),
                                new XElement("Pitch_max", x.PitchMax),
                                new XElement("Intensity_threshold", x.IntensityThreshold),
                                new XElement("Duree", x.Duree_exacte),
                                new XElement("F0_exactEvaluated", x.F0_exactEvaluated),
                                new XElement("F0_stableEvaluated", x.F0_stableEvaluated),
                                new XElement("Intensite_stableEvaluated", x.Intensite_stableEvaluated),
                                new XElement("Courbe_f0_exacteEvaluated", x.Courbe_f0_exacteEvaluated),
                                new XElement("Duree_exacteEvaluated", x.Duree_exacteEvaluated),
                                new XElement("JitterEvaluated", x.JitterEvaluated),
                                new XElement("F0_exacte_evaluation",
                                    new XElement("Good",
                                        new XElement("Max", x.F0_exact_good_max),
                                        new XElement("Min", x.F0_exact_good_min)
                                        ),
                                    new XElement("Okay",
                                        new XElement("Max", x.F0_exact_okay_max),
                                        new XElement("Min", x.F0_exact_okay_min)
                                        ),
                                    new XElement("Bad",
                                        new XElement("Max", x.F0_exact_bad_max),
                                        new XElement("Min", x.F0_exact_bad_min)
                                        )
                                    ),
                                new XElement("F0_stable_evaluation",
                                    new XElement("Good",
                                        new XElement("Max", x.F0_stable_good_max),
                                        new XElement("Min", x.F0_stable_good_min)
                                        ),
                                    new XElement("Okay",
                                        new XElement("Max", x.F0_stable_okay_max),
                                        new XElement("Min", x.F0_stable_okay_min)
                                        ),
                                    new XElement("Bad",
                                        new XElement("Max", x.F0_stable_bad_max),
                                        new XElement("Min", x.F0_stable_bad_min)
                                        )
                                    ),
                                new XElement("Intensite_stable_evaluation",
                                    new XElement("Good",
                                        new XElement("Max", x.Intensite_stable_good_max),
                                        new XElement("Min", x.Intensite_stable_good_min)
                                        ),
                                    new XElement("Okay",
                                        new XElement("Max", x.Intensite_stable_okay_max),
                                        new XElement("Min", x.Intensite_stable_okay_min)
                                        ),
                                    new XElement("Bad",
                                        new XElement("Max", x.Intensite_stable_bad_max),
                                        new XElement("Min", x.Intensite_stable_bad_min)
                                        )
                                    ),
                                new XElement("Courbe_F0_exacte_evaluation",
                                    new XElement("Good",
                                        new XElement("Max", x.Courbe_F0_exact_good_max),
                                        new XElement("Min", x.Courbe_F0_exact_good_min)
                                        ),
                                    new XElement("Okay",
                                        new XElement("Max", x.Courbe_F0_exact_okay_max),
                                        new XElement("Min", x.Courbe_F0_exact_okay_min)
                                        ),
                                    new XElement("Bad",
                                        new XElement("Max", x.Courbe_F0_exact_bad_max),
                                        new XElement("Min", x.Courbe_F0_exact_bad_min)
                                        )
                                    ),
                                new XElement("Duree_exacte_evaluation",
                                    new XElement("Good",
                                        new XElement("Max", x.Duree_good_max),
                                        new XElement("Min", x.Duree_good_min)
                                        ),
                                    new XElement("Okay",
                                        new XElement("Max", x.Duree_okay_max),
                                        new XElement("Min", x.Duree_okay_min)
                                        ),
                                    new XElement("Bad",
                                        new XElement("Max", x.Duree_bad_max),
                                        new XElement("Min", x.Duree_bad_min)
                                        )
                                    ),
                                new XElement("Jitter_evaluation",
                                    new XElement("Good",
                                        new XElement("Max", x.Jitter_good_max),
                                        new XElement("Min", x.Jitter_good_min)
                                        ),
                                    new XElement("Okay",
                                        new XElement("Max", x.Jitter_okay_max),
                                        new XElement("Min", x.Jitter_okay_min)
                                        ),
                                    new XElement("Bad",
                                        new XElement("Max", x.Jitter_bad_max),
                                        new XElement("Min", x.Jitter_bad_min)
                                        )
                                    )
                                )
                            )
                        )
                    );

            doc.Save(targetDirectory + "\\" + "test.xml");
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
