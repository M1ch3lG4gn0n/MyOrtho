﻿using MyOrthoOrtho.Controllers;
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
    /// Interaction logic for CtrlPreparation.xaml
    /// </summary>
    public partial class CtrlPreparation : UserControl
    {
        private PreparationExecuter pe;
        ListPreparationVM activityListInstance = new ListPreparationVM();
        
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

                        PreparationVM prep = new PreparationVM
                        {
                            Name = exercice.Name,
                            Example_wav_path = exercice.Exercice_wav_file_name,
                        };
                        activityListInstance.Add(prep);

                    }   
                }
            }
            else
            {
                //TODO: message indiquand qu'aucun exercice n'existe dans l'application
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
            //TODO: lire le fichier Praat et afficher/remplacer les graphiques
        }

        private void ListSelected_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO: lire le fichier Praat et afficher/remplacer les graphiques
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
        }
    }
}
