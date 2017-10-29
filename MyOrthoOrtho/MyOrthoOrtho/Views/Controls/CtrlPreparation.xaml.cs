using MyOrthoOrtho.Controllers;
using MyOrthoOrtho.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;


namespace MyOrthoOrtho.Views.Controls
{
    /// <summary>
    /// Interaction logic for CtrlPreparation.xaml
    /// </summary>
    public partial class CtrlPreparation : UserControl
    {
        private PreparationExecuter ac;

        public CtrlPreparation()
        {
            InitializeComponent();
        }

        private void BtnCreerExercice_Click(object sender, RoutedEventArgs e)
        {
            PreparationVM activity = new PreparationVM
            {
                Example_wav_path = txtFileName.Text,
                Name = txtName.Text,
                PitchMin = Convert.ToInt32(txtPitchMin.Text),
                PitchMax = Convert.ToInt32(txtPitchMax.Text),
                IntensityThreshold = Convert.ToInt32(txtIntensityThreshold.Text),
                Duree_expected = Convert.ToInt32(txtDuration.Text)
            };

            activity.SetExerciseValue(values => SetChartLine((LineSeries)PitchChart.Series[0], (LineSeries)IntensityChart.Series[0], values));
            activity.SetResultValue(values => SetChartLine((LineSeries)PitchChart.Series[1], (LineSeries)IntensityChart.Series[1], values));
            ac = new PreparationExecuter(activity);
        }


        private void BtnLire_Click(object sender, RoutedEventArgs e)
        {
            ac.StartPlaybackExemple();

        }
        private void BtnArreter_Click(object sender, RoutedEventArgs e)
        {
            ac.StopPlayback();
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
            ac.StartRecord();
        }
        private void BtnTerminer_Click(object sender, RoutedEventArgs e)
        {
            ac.StopRecord();
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
    }
}
