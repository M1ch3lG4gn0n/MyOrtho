using MyOrthoOrtho.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyOrthoOrtho.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowState = WindowState.Normal;
            
        }

        private void Navigate_Preparation(object sender, RoutedEventArgs e)
        {
            ctrlSuivi.Visibility = Visibility.Hidden;
            ctrlPreparation.Visibility = Visibility.Visible;
        }

        private void Navigate_Suivi(object sender, RoutedEventArgs e)
        {
            ctrlPreparation.Visibility = Visibility.Hidden;
            ctrlSuivi.Visibility = Visibility.Visible;
        }

        private void OpenHelp(object sender, RoutedEventArgs e)
        {

        }
    }
   
}
