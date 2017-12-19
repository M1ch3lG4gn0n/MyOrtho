using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MyOrthoOrtho.ViewModels;

namespace MyOrthoOrtho.Controllers
{
    public class XmlHelper
    {
        private XmlDocument document = new XmlDocument();
        private XmlElement root;

        public XmlHelper(bool withHeader)
        {
            root = document.DocumentElement;
            if (withHeader)
            {
                XmlDeclaration xmlDeclaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                document.InsertBefore(xmlDeclaration, root);
            }
        }

        public XmlElement GetRoot()
        {
            return document.DocumentElement;
        }

        public XmlElement AddToRoot(string name, string value)
        {
            XmlElement node = document.CreateElement(string.Empty, name, string.Empty);
            if (!string.IsNullOrEmpty(value))
            {
                XmlText textNode = document.CreateTextNode(value);
                node.AppendChild(textNode);
            }
            document.AppendChild(node);
            return node;
        }

        public XmlElement AppendToNode(XmlElement node, string name, string value)
        {
            XmlElement childNode = document.CreateElement(string.Empty, name, string.Empty);
            if (!string.IsNullOrEmpty(value))
            {
                XmlText textValue = document.CreateTextNode(value);
                childNode.AppendChild(textValue);
            }
            node.AppendChild(childNode);
            return childNode;
        }

        public void ImportNode(XmlElement parent, XmlNode child)
        {
            XmlNode importedDocument = document.ImportNode(child, true);
            if (parent != null)
            {
                parent.AppendChild(importedDocument);
            }else
            {
                document.AppendChild(importedDocument);
            }
        }

        public void Save(string path)
        {
            document.Save(path);
        }

        public static XmlNode MakeActivityNode(ExerciceVM activity)
        {
            var configFile = new XmlHelper(false);
            XmlElement activityNode = configFile.AddToRoot("Activity", string.Empty);

            configFile.AppendToNode(activityNode, "Name", activity.Name);
            configFile.AppendToNode(activityNode, "Exercice_wav_file_name", activity.Example_wav_path);
            var exerciceResults = configFile.AppendToNode(activityNode, "Exercice_praat_results", string.Empty);

            XmlElement pointNode;
            foreach (var point in activity.Exercice)
            {
                pointNode = configFile.AppendToNode(exerciceResults, "point", string.Empty);
                configFile.AppendToNode(pointNode, "time", point.Time.ToString("F2"));
                configFile.AppendToNode(pointNode, "frequency", point.Intensity.ToString("F2"));
                configFile.AppendToNode(pointNode, "pitch", point.Pitch.ToString("F2"));
            }

            configFile.AppendToNode(activityNode, "Pitch_min", activity.PitchMin.ToString());
            configFile.AppendToNode(activityNode, "Pitch_max", activity.PitchMax.ToString());
            configFile.AppendToNode(activityNode, "Intensity_threshold", activity.IntensityThreshold.ToString());
            configFile.AppendToNode(activityNode, "F0_exactEvaluated", activity.F0_exactEvaluated.ToString());
            configFile.AppendToNode(activityNode, "Courbe_f0_exacteEvaluated", activity.Courbe_f0_exacteEvaluated.ToString());
            configFile.AppendToNode(activityNode, "F0_stableEvaluated", activity.F0_stableEvaluated.ToString());
            configFile.AppendToNode(activityNode, "Intensite_stableEvaluated", activity.Intensite_stableEvaluated.ToString());
            configFile.AppendToNode(activityNode, "Duree_exacteEvaluated", activity.Duree_exacteEvaluated.ToString());
            configFile.AppendToNode(activityNode, "Duree_exacte", activity.Duree_exacte.ToString());
            configFile.AppendToNode(activityNode, "JitterEvaluated", activity.JitterEvaluated.ToString());

            var f0_exacte_evaluation = configFile.AppendToNode(activityNode, "F0_exacte_evaluation", string.Empty);
            var good = configFile.AppendToNode(f0_exacte_evaluation, "Good", string.Empty);
            configFile.AppendToNode(good, "Max", activity.F0_exact_good_max.ToString());
            configFile.AppendToNode(good, "Min", activity.F0_exact_good_min.ToString());
            var okay = configFile.AppendToNode(f0_exacte_evaluation, "Okay", string.Empty);
            configFile.AppendToNode(okay, "Max", activity.F0_exact_okay_max.ToString());
            configFile.AppendToNode(okay, "Min", activity.F0_exact_okay_min.ToString());
            var bad = configFile.AppendToNode(f0_exacte_evaluation, "Bad", string.Empty);
            configFile.AppendToNode(bad, "Max", activity.F0_exact_bad_max.ToString());
            configFile.AppendToNode(bad, "Min", activity.F0_exact_bad_min.ToString());

            var f0_stable_evaluation = configFile.AppendToNode(activityNode, "F0_stable_evaluation", string.Empty);
            good = configFile.AppendToNode(f0_stable_evaluation, "Good", string.Empty);
            configFile.AppendToNode(good, "Max", activity.F0_stable_good_max.ToString());
            configFile.AppendToNode(good, "Min", activity.F0_stable_good_min.ToString());
            okay = configFile.AppendToNode(f0_stable_evaluation, "Okay", string.Empty);
            configFile.AppendToNode(okay, "Max", activity.F0_stable_okay_max.ToString());
            configFile.AppendToNode(okay, "Min", activity.F0_stable_okay_min.ToString());
            bad = configFile.AppendToNode(f0_stable_evaluation, "Bad", string.Empty);
            configFile.AppendToNode(bad, "Max", activity.F0_stable_bad_max.ToString());
            configFile.AppendToNode(bad, "Min", activity.F0_stable_bad_min.ToString());

            var intensite_stable_evaluation = configFile.AppendToNode(activityNode, "Intensite_stable_evaluation", string.Empty);
            good = configFile.AppendToNode(intensite_stable_evaluation, "Good", string.Empty);
            configFile.AppendToNode(good, "Max", activity.Intensite_stable_good_max.ToString());
            configFile.AppendToNode(good, "Min", activity.Intensite_stable_good_min.ToString());
            okay = configFile.AppendToNode(intensite_stable_evaluation, "Okay", string.Empty);
            configFile.AppendToNode(okay, "Max", activity.Intensite_stable_okay_max.ToString());
            configFile.AppendToNode(okay, "Min", activity.Intensite_stable_okay_min.ToString());
            bad = configFile.AppendToNode(intensite_stable_evaluation, "Bad", string.Empty);
            configFile.AppendToNode(bad, "Max", activity.Intensite_stable_bad_max.ToString());
            configFile.AppendToNode(bad, "Min", activity.Intensite_stable_bad_min.ToString());

            var courbe_F0_exacte_evaluation = configFile.AppendToNode(activityNode, "Courbe_F0_exacte_evaluation", string.Empty);
            good = configFile.AppendToNode(courbe_F0_exacte_evaluation, "Good", string.Empty);
            configFile.AppendToNode(good, "Max", activity.Courbe_F0_exact_good_max.ToString());
            configFile.AppendToNode(good, "Min", activity.Courbe_F0_exact_good_min.ToString());
            okay = configFile.AppendToNode(courbe_F0_exacte_evaluation, "Okay", string.Empty);
            configFile.AppendToNode(okay, "Max", activity.Courbe_F0_exact_okay_max.ToString());
            configFile.AppendToNode(okay, "Min", activity.Courbe_F0_exact_okay_min.ToString());
            bad = configFile.AppendToNode(courbe_F0_exacte_evaluation, "Bad", string.Empty);
            configFile.AppendToNode(bad, "Max", activity.Courbe_F0_exact_bad_max.ToString());
            configFile.AppendToNode(bad, "Min", activity.Courbe_F0_exact_bad_min.ToString());

            var duree_exacte_evaluation = configFile.AppendToNode(activityNode, "Duree_exacte_evaluation", string.Empty);
            good = configFile.AppendToNode(duree_exacte_evaluation, "Good", string.Empty);
            configFile.AppendToNode(good, "Max", activity.Duree_good_max.ToString());
            configFile.AppendToNode(good, "Min", activity.Duree_good_min.ToString());
            okay = configFile.AppendToNode(duree_exacte_evaluation, "Okay", string.Empty);
            configFile.AppendToNode(okay, "Max", activity.Duree_okay_max.ToString());
            configFile.AppendToNode(okay, "Min", activity.Duree_okay_min.ToString());
            bad = configFile.AppendToNode(duree_exacte_evaluation, "Bad", string.Empty);
            configFile.AppendToNode(bad, "Max", activity.Duree_bad_max.ToString());
            configFile.AppendToNode(bad, "Min", activity.Duree_bad_min.ToString());

            var jitter_evaluation = configFile.AppendToNode(activityNode, "Jitter_evaluation", string.Empty);
            good = configFile.AppendToNode(jitter_evaluation, "Good", string.Empty);
            configFile.AppendToNode(good, "Max", activity.Jitter_good_max.ToString());
            configFile.AppendToNode(good, "Min", activity.Jitter_good_min.ToString());
            okay = configFile.AppendToNode(jitter_evaluation, "Okay", string.Empty);
            configFile.AppendToNode(okay, "Max", activity.Jitter_okay_max.ToString());
            configFile.AppendToNode(okay, "Min", activity.Jitter_okay_min.ToString());
            bad = configFile.AppendToNode(jitter_evaluation, "Bad", string.Empty);
            configFile.AppendToNode(bad, "Max", activity.Jitter_bad_max.ToString());
            configFile.AppendToNode(bad, "Min", activity.Jitter_bad_min.ToString());

            return activityNode;
        }
    }
}
