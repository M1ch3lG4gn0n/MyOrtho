﻿using System.Windows.Media.Imaging;
using MyOrthoClient.Models;
using MyOrthoClient.Controllers;
using System.Windows.Controls;

namespace MyOrthoClient.Views
{
    /// <summary>
    /// Interaction logic for FlatResult.xaml
    /// </summary>
    public partial class FlatResult : UserControl
    {
        private ActivityVM activity;
        private BitmapImage _F0ExacteImage;
        public BitmapImage F0ExacteImage { get { return _F0ExacteImage; } }
        private BitmapImage _F0StableImage;
        public BitmapImage F0StableImage { get { return _F0StableImage; } }
        private BitmapImage _IntensiteStableImage;
        public BitmapImage IntensiteStableImage { get { return _IntensiteStableImage; } }
        private BitmapImage _DureeExacteImage;
        public BitmapImage DureeExacteImage { get { return _DureeExacteImage; } }
        private BitmapImage _JitterImage;
        public BitmapImage JitterImage { get { return _JitterImage; } }

        public FlatResult(ActivityVM activity)
        {
            if (activity.Courbe_f0_exacteEvaluated)
            {
                return;
            }

            this.activity = activity;
            
            _F0ExacteImage = this.ConvertToBitMap(activity.F0_exact);
            _F0StableImage = this.ConvertToBitMap(activity.F0_stable);
            _IntensiteStableImage = this.ConvertToBitMap(activity.Intensite_stable);
            _DureeExacteImage = this.ConvertToBitMap(activity.Duree_exacte);
            _JitterImage = this.ConvertToBitMap(ScoreProvider.EvaluateJitter(activity.Jitter));

            InitializeComponent();

            this.F0ExacteResult.Source = F0ExacteImage;
            this.F0StableResult.Source = F0StableImage;
            this.IntensiteStableResult.Source = IntensiteStableImage;
            this.DureeExacteResult.Source = DureeExacteImage;
            this.JitterResult.Source = JitterImage;
        }
    }
}