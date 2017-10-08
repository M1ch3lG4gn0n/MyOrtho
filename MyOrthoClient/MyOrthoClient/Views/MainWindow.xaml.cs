using MyOrthoClient.Models;
using System;
using System.Collections.Generic;
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

namespace MyOrthoClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();

            ListVM activityListInstance = new ListVM();
            this.DataContext = activityListInstance;

            List<KeyValuePair<int, int>> intensityData = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, int>> frequencyData = new List<KeyValuePair<int, int>>();

            intensityData.Add(new KeyValuePair<int, int>(100, 110));
            
            frequencyData.Add(new KeyValuePair<int, int>(100, 100));

            var dataSourceList = new List<List<KeyValuePair<int, int>>>();
            dataSourceList.Add(intensityData);
            dataSourceList.Add(frequencyData);

            mcChart.DataContext = dataSourceList;
        }
    }
}
