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

using Microsoft.Surface;
using Microsoft.Surface.Presentation.Generic;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using Microsoft.Maps.MapControl.WPF;
using System.Collections;

namespace SurGIS
{
    /// <summary>
    /// Interaction logic for SURGISControl1.xaml
    /// </summary>
    public partial class SURGISControl1 : UserControl
    {
       public double Lattitude;
       public double Longitude;
       public Double CameraLevel;
       public double Orientation;
       private SurfaceWindow1 surfaceWindow1;
     

       public ArrayList Polygons = new ArrayList();




       public SURGISControl1()
       {
           InitializeComponent();

           //SURTextBox.Text = this.Name;
       }

        public SURGISControl1(SurfaceWindow1 surfaceWindow1)
        {
            // TODO: Complete member initialization
            InitializeComponent();
            this.surfaceWindow1 = surfaceWindow1;
           
        }

        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {

            Location Home = new Location();
            Home.Latitude = Lattitude;
            Home.Longitude = Longitude;


            foreach (MapPolygon Polygon in Polygons)

            {

                Polygon.Fill = new SolidColorBrush(Colors.Maroon);
                Polygon.Stroke = new SolidColorBrush(Colors.Gold);
                Polygon.StrokeThickness = 2;
                Polygon.Opacity = 0.8;

                    try
                    {
                        //Original =====
                        //LogicalTreeHelper.GetChildren);
                        MessageBox.Show ("Message here");
                        surfaceWindow1.PolygonLayer.Children.Add(Polygon); //The actual polygon creation point.
                        //==============
     

                       
                    }
                    catch
                    {
                        // don't do anything.
                    }
            }

            surfaceWindow1.myMap.SetView(Home, CameraLevel);
          
          

        } 

      
         
       
    }
}
