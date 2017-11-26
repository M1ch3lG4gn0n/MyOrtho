﻿using MyOrthoClient.Models;
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

namespace MyOrthoClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string SelectedItemJitter
        {
            get
            {
                var value = activityListInstance.GetActivity(0)?.Jitter.ToString();
                return value ?? string.Empty;
            }
        }        

        private ActivityExecuter ac;
        ListVM activityListInstance = new ListVM();        

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
            JitterTxt.IsEnabled = false;
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
                   JitterTxt.Text = SelectedItemJitter;
                   var currentActivityIndex = ListActivities.SelectedIndex;
                   var activity = activityListInstance.GetActivity(currentActivityIndex);
                   this.Results.Content = activity.Courbe_f0_exacteEvaluated? new Views.CurveResult(activity) : (System.Windows.Controls.UserControl)new Views.FlatResult(activity);
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
    }
}
