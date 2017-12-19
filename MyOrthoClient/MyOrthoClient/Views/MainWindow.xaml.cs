using MyOrthoClient.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using MyOrthoClient.Controllers;
using System.Windows.Threading;
using WpfAnimatedGif;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Xml.Linq;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Linq;
using System.IO.Compression;

namespace MyOrthoClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ActivityExecuter ac;
        ListVM activityListInstance = new ListVM();
        static string EXERCICES_FOLDER = Environment.GetEnvironmentVariable("LocalAppData") + "\\MyOrtho\\SavedExercices";
        List<System.Windows.Controls.UserControl> activityScores = new List<UserControl>();

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
            activityScores.Clear();
            activityListInstance.ActivityList.ToList().ForEach(_ => activityScores.Add(null));
        }

        private void BtnLire_Click(object sender, RoutedEventArgs e)
        {
            BtnDemarrer.IsEnabled = false;
            BtnArreter.IsEnabled = true;
            BtnLire.IsEnabled = false;

            ac.StartPlayback();
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

            Task.Factory.StartNew(() =>
            {
                //Update Text on the UI thread 
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input,
               new Action(() =>
               {
                   var currentActivityIndex = ListActivities.SelectedIndex;
                   var activity = activityListInstance.GetActivity(currentActivityIndex);
                   activityScores[currentActivityIndex] = activity.Courbe_f0_exacteEvaluated ? new Views.CurveResult(activity) : (System.Windows.Controls.UserControl)new Views.FlatResult(activity);
                   this.Results.Content = activityScores[currentActivityIndex];
               }));
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

            ac.StartLastExercicePlayblack();
        }

        private void ListActivities_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentActivityIndex = ListActivities.SelectedIndex;
            var activity = activityListInstance.GetActivity(currentActivityIndex);
            activity.SetExerciseValue(values => SetChartLine((LineSeries)IntensityChart.Series[0], (LineSeries)PitchChart.Series[0], values));
            activity.SetResultValue(values => SetChartLine((LineSeries)IntensityChart.Series[1], (LineSeries)PitchChart.Series[1], values));
            ac = new ActivityExecuter(activity, OnPlayingEnablingButton, SetFeedbackGif);
            this.Results.Content = activityScores[currentActivityIndex];
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
                frequencyLineArray[i] = new KeyValuePair<double, double>(lineItem.Time, lineItem.Intensity);
                pitchLineArray[i++] = new KeyValuePair<double, double>(lineItem.Time, lineItem.Pitch);
            }
            this.Dispatcher.Invoke(() =>
            {
                frequency.ItemsSource = frequencyLineArray;
                pitch.ItemsSource = pitchLineArray;
            });
        }

        private void OnPlayingEnablingButton(bool isPlaying)
        {
            Task.Factory.StartNew(() =>
            {
                //Update Text on the UI thread 
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input,
               new Action(() => {
                   BtnDemarrer.IsEnabled = !isPlaying;
                   BtnArreter.IsEnabled = isPlaying;
                   BtnLire.IsEnabled = !isPlaying;
               }));
            });
        }

        private void SetFeedbackGif(string path)
        {
            this.Dispatcher.Invoke((new Action(() =>
            {
                BitmapImage image = null;
                if (!string.IsNullOrEmpty(path))
                {
                    image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource = new Uri(path);
                    image.EndInit();
                }
                ImageBehavior.SetAnimatedSource(feedbackGif, image);
            })));
        }

        private void BtnExporter_Click(object sender, RoutedEventArgs e)
        {

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
                                activityListInstance.ActivityList.Select(x => new XElement("Activity",
                                    new XElement("Name", x.Name),
                                    new XElement("Exercice_wav_file_name", x.Example_wav_path?.Split(new char[] { '\\'}).Last()),
                                    new XElement("Result_wav_filename", x.Resultat_wav_path?.Split(new char[] { '\\' }).Last()),
                                    new XElement("F0_exact", x.F0_exact),
                                    new XElement("F0_stable", x.F0_stable),
                                    new XElement("Intensite_stable", x.Intensite_stable),
                                    new XElement("Courbe_f0_exacte", x.Courbe_f0_exacte),
                                    new XElement("Duree_exacte", x.Duree_exacte),
                                    new XElement("Jitter", x.Jitter),
                                    x.Exercice?.Select(y => new XElement("PointExercice",
                                        new XElement("Time", y.Time),
                                        new XElement("Intensity", y.Intensity),
                                        new XElement("Pitch", y.Pitch)
                                        )
                                        ),
                                    x.Results?.Select(y => new XElement("PointResultat",
                                        new XElement("Time", y.Time),
                                        new XElement("Intensity", y.Intensity),
                                        new XElement("Pitch", y.Pitch)
                                        )
                                        )
                                    )
                                )
                            )
                        );

                string tempPath = Directory.GetCurrentDirectory();
                Directory.CreateDirectory(tempPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                tempPath += "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");

                doc.Save(tempPath + "\\" + "resultats.xml");

                foreach (ActivityVM exercice in activityListInstance.ActivityList)
                {
                    if (!String.IsNullOrEmpty(exercice.Resultat_wav_path)) { 
                        File.Copy(exercice.Resultat_wav_path, tempPath + "\\" + exercice.Resultat_wav_path.Split(new char[] { '\\' }).Last());
                        File.Copy(exercice.Example_wav_path, tempPath + "\\" + exercice.Example_wav_path.Split(new char[] { '\\' }).Last());
                    }
                }

                ZipFile.CreateFromDirectory(tempPath, targetDirectory + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".zip");

                Directory.Delete(tempPath,true);
            }
        }
        
    }
}
