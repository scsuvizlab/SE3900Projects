using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SurGIS2
{
    /// <summary>
    /// Interaction logic for SurGISContPanel1.xaml
    /// </summary>
    public partial class SurGISContPanel1 : UserControl
    {
        SurfaceWindow1 surfacewindow;
        public SurGISContPanel1(SurfaceWindow1 ProgramWindow)
        {
            surfacewindow = ProgramWindow;
            InitializeComponent();
        }

        private void PolyGonToggleButton_Click(object sender, RoutedEventArgs e)
        {
            surfacewindow.PolyPointButton_Click();
        }
    }
}
