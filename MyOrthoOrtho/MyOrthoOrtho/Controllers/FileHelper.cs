using MyOrthoOrtho.ViewModels;
using MyOrthoOrtho.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyOrthoOrtho.Controllers
{
    class FileHelper
    {
        public class FileWriter
        {
            public FileWriter()
            {

            }
        }

        public class FileReader
        {
            public FileReader()
            {
                


            }

            public void ReadAllExercicesIntoExerciceVMList(string filePath, ObservableCollection<ExerciceVM> list)
            {
                foreach (string file in Directory.GetFiles(filePath))
                {
                    if (Path.GetExtension(file) == ".xml")
                    {
                        XDocument xml = XDocument.Load(file);

                        var exercice = xml.Descendants("Activity");
                        var isMyotype = exercice.Count();//Vérifie si le .xml est un exercice MyOrtho

                        if(isMyotype > 0)
                        {
                            ExerciceVM newExercice = new ExerciceVM
                                {
                                    Name = exercice.Descendants("Name").First().Value,
                                    Example_wav_path = exercice.Descendants("Exercice_wav_file_name").First().Value,

                                    PitchMin = int.Parse(exercice.Descendants("Pitch_min").First().Value),
                                    PitchMax = int.Parse(exercice.Descendants("Pitch_max").First().Value),
                                    IntensityThreshold = int.Parse(exercice.Descendants("Intensity_threshold").First().Value),
                                    Duree_exacte = int.Parse(exercice.Descendants("Duree_exacte").First().Value),

                                    F0_exactEvaluated = exercice.Descendants("F0_exactEvaluated").First().Value == "True"?true : false,
                                    F0_stableEvaluated = exercice.Descendants("F0_stableEvaluated").First().Value == "True" ? true : false,
                                    Courbe_f0_exacteEvaluated = exercice.Descendants("Courbe_f0_exacteEvaluated").First().Value == "True" ? true : false,
                                    Intensite_stableEvaluated = exercice.Descendants("Intensite_stableEvaluated").First().Value == "True" ? true : false,
                                    Duree_exacteEvaluated = exercice.Descendants("Duree_exacteEvaluated").First().Value == "True" ? true : false,
                                    JitterEvaluated = exercice.Descendants("JitterEvaluated").First().Value == "True" ? true : false,

                                    F0_exact_good_max = decimal.Parse(exercice.Descendants("F0_exacte_evaluation").First().Descendants("Good").Descendants("Max").First().Value),
                                    F0_exact_good_min = decimal.Parse(exercice.Descendants("F0_exacte_evaluation").First().Descendants("Good").Descendants("Min").First().Value),
                                    F0_exact_okay_max = decimal.Parse(exercice.Descendants("F0_exacte_evaluation").First().Descendants("Okay").Descendants("Max").First().Value),
                                    F0_exact_okay_min = decimal.Parse(exercice.Descendants("F0_exacte_evaluation").First().Descendants("Okay").Descendants("Min").First().Value),
                                    F0_exact_bad_max = decimal.Parse(exercice.Descendants("F0_exacte_evaluation").First().Descendants("Bad").Descendants("Max").First().Value),
                                    F0_exact_bad_min = decimal.Parse(exercice.Descendants("F0_exacte_evaluation").First().Descendants("Bad").Descendants("Min").First().Value),

                                    F0_stable_good_max = decimal.Parse(exercice.Descendants("F0_stable_evaluation").First().Descendants("Good").Descendants("Max").First().Value),
                                    F0_stable_good_min = decimal.Parse(exercice.Descendants("F0_stable_evaluation").First().Descendants("Good").Descendants("Min").First().Value),
                                    F0_stable_okay_max = decimal.Parse(exercice.Descendants("F0_stable_evaluation").First().Descendants("Okay").Descendants("Max").First().Value),
                                    F0_stable_okay_min = decimal.Parse(exercice.Descendants("F0_stable_evaluation").First().Descendants("Okay").Descendants("Min").First().Value),
                                    F0_stable_bad_max = decimal.Parse(exercice.Descendants("F0_stable_evaluation").First().Descendants("Bad").Descendants("Max").First().Value),
                                    F0_stable_bad_min = decimal.Parse(exercice.Descendants("F0_stable_evaluation").First().Descendants("Bad").Descendants("Min").First().Value),

                                    Courbe_F0_exact_good_max = decimal.Parse(exercice.Descendants("Courbe_F0_exacte_evaluation").First().Descendants("Good").Descendants("Max").First().Value),
                                    Courbe_F0_exact_good_min = decimal.Parse(exercice.Descendants("Courbe_F0_exacte_evaluation").First().Descendants("Good").Descendants("Min").First().Value),
                                    Courbe_F0_exact_okay_max = decimal.Parse(exercice.Descendants("Courbe_F0_exacte_evaluation").First().Descendants("Okay").Descendants("Max").First().Value),
                                    Courbe_F0_exact_okay_min = decimal.Parse(exercice.Descendants("Courbe_F0_exacte_evaluation").First().Descendants("Okay").Descendants("Min").First().Value),
                                    Courbe_F0_exact_bad_max = decimal.Parse(exercice.Descendants("Courbe_F0_exacte_evaluation").First().Descendants("Bad").Descendants("Max").First().Value),
                                    Courbe_F0_exact_bad_min = decimal.Parse(exercice.Descendants("Courbe_F0_exacte_evaluation").First().Descendants("Bad").Descendants("Min").First().Value),

                                    Intensite_stable_good_max = decimal.Parse(exercice.Descendants("Intensite_stable_evaluation").First().Descendants("Good").Descendants("Max").First().Value),
                                    Intensite_stable_good_min = decimal.Parse(exercice.Descendants("Intensite_stable_evaluation").First().Descendants("Good").Descendants("Min").First().Value),
                                    Intensite_stable_okay_max = decimal.Parse(exercice.Descendants("Intensite_stable_evaluation").First().Descendants("Okay").Descendants("Max").First().Value),
                                    Intensite_stable_okay_min = decimal.Parse(exercice.Descendants("Intensite_stable_evaluation").First().Descendants("Okay").Descendants("Min").First().Value),
                                    Intensite_stable_bad_max = decimal.Parse(exercice.Descendants("Intensite_stable_evaluation").First().Descendants("Bad").Descendants("Max").First().Value),
                                    Intensite_stable_bad_min = decimal.Parse(exercice.Descendants("Intensite_stable_evaluation").First().Descendants("Bad").Descendants("Min").First().Value),

                                    Jitter_good_max = decimal.Parse(exercice.Descendants("Jitter_evaluation").First().Descendants("Good").Descendants("Max").First().Value),
                                    Jitter_good_min = decimal.Parse(exercice.Descendants("Jitter_evaluation").First().Descendants("Good").Descendants("Min").First().Value),
                                    Jitter_okay_max = decimal.Parse(exercice.Descendants("Jitter_evaluation").First().Descendants("Okay").Descendants("Max").First().Value),
                                    Jitter_okay_min = decimal.Parse(exercice.Descendants("Jitter_evaluation").First().Descendants("Okay").Descendants("Min").First().Value),
                                    Jitter_bad_max = decimal.Parse(exercice.Descendants("Jitter_evaluation").First().Descendants("Bad").Descendants("Max").First().Value),
                                    Jitter_bad_min = decimal.Parse(exercice.Descendants("Jitter_evaluation").First().Descendants("Bad").Descendants("Min").First().Value),

                                    Duree_good_max = decimal.Parse(exercice.Descendants("Duree_exacte_evaluation").First().Descendants("Good").Descendants("Max").First().Value),
                                    Duree_good_min = decimal.Parse(exercice.Descendants("Duree_exacte_evaluation").First().Descendants("Good").Descendants("Min").First().Value),
                                    Duree_okay_max = decimal.Parse(exercice.Descendants("Duree_exacte_evaluation").First().Descendants("Okay").Descendants("Max").First().Value),
                                    Duree_okay_min = decimal.Parse(exercice.Descendants("Duree_exacte_evaluation").First().Descendants("Okay").Descendants("Min").First().Value),
                                    Duree_bad_max = decimal.Parse(exercice.Descendants("Duree_exacte_evaluation").First().Descendants("Bad").Descendants("Max").First().Value),
                                    Duree_bad_min = decimal.Parse(exercice.Descendants("Duree_exacte_evaluation").First().Descendants("Bad").Descendants("Min").First().Value),
                            
                                };
                                newExercice.Exercice = exercice.Descendants("Exercice_praat_results").First().Descendants("point").Select(x => new DataLineItem {time = double.Parse(x.Descendants("time").First().Value), pitch = double.Parse(x.Descendants("pitch").First().Value), frequency = double.Parse(x.Descendants("frequency").First().Value) }).ToList();
                        
                                list.Add(newExercice);
                            
                        }
                    }
                }
                
            }

            public string ReadFile()
            {
                

                return string.Empty;
            }
        }
    }
}
