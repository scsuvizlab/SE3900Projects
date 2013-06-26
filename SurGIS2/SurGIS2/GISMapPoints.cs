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
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.Collections;
//using Catfood.Shapefile;
using Microsoft.Win32;
using System.Windows.Threading;
using System.ComponentModel;

namespace SurGIS2
{

    public class GISMapPoint
    {

        public Location PointLocation;
        public Rectangle pointrect;

        public GISMapPoint()
        {
            pointrect = new Rectangle();
            pointrect.Fill = new SolidColorBrush(Colors.Maroon);
            pointrect.Stroke = new SolidColorBrush(Colors.Gold);
            pointrect.StrokeThickness = 1;
            pointrect.Width = 16;
            pointrect.Height = 16;            
        }



    }

   public class GISMapPoints
    {
        public ArrayList MapPoints = new ArrayList();
        public bool AddPolyPoint = false;
        public GISMapPoint SelectedPoint = new GISMapPoint();
        public bool PointSelected = false; 

        // Global handler back to the main program window.
        private SurfaceWindow1 surfacewindow;

        //constructor
      

        public GISMapPoints(SurfaceWindow1 ProgramWindow)
        {
            // TODO: Complete member initialization
            surfacewindow = ProgramWindow;
        }
        //used for plotting the points for drawing a polygon.
        public void AddPoint(Location MapPointLocation)
        {
            if (AddPolyPoint)
            {
                GISMapPoint GMPoint = new GISMapPoint();
                GMPoint.PointLocation = MapPointLocation;
                surfacewindow.MainMap.PointLayer.AddChild(GMPoint.pointrect, MapPointLocation);
                MapPoints.Add(GMPoint);
                GMPoint.pointrect.TouchDown += new EventHandler<TouchEventArgs>(Point_TouchDown);                
            }
        }

       /*
        * trying to get a point to move by useing two different methods. one is an actualy event methods and one is a mehtods that says
        * touch a point and change point to recently touched point.
        */

        public void Point_TouchMove(Location MapLocation)
        {            
            surfacewindow.MainMap.PointLayer.Children.Remove(SelectedPoint.pointrect);
            surfacewindow.MapPolygon.GMPoint.SelectedPoint.PointLocation = MapLocation;
            surfacewindow.MapPolygon.GMPoint.AddPoint(SelectedPoint.PointLocation);            
            PointSelected = false;           
        }

        public void Point_TouchDown(object sender, TouchEventArgs e)
        {
            Rectangle TouchPoint = sender as Rectangle;            

            if (TouchPoint == SelectedPoint.pointrect)
            {
                DeselectAll();                
            }
            else
            {                
                SelectedPoint.pointrect = TouchPoint;
                UpdateColors(TouchPoint);
                PointSelected = true;
            }            
        }

        public void UpdateColors(Rectangle pointrect)
        {

            for (int i = 0; i < MapPoints.Count; i++)
            {
                GISMapPoint test = MapPoints[i] as GISMapPoint;
                if (pointrect.Equals(test.pointrect) && pointrect != null)
                {
                    test.pointrect.Stroke = new SolidColorBrush(Colors.Wheat);
                    test.pointrect.Fill = new SolidColorBrush(Colors.Blue);                    
                }
                else // not the selected polygon... reset it's color
                {
                    test.pointrect.Stroke = new SolidColorBrush(Colors.Gold);
                    test.pointrect.Fill = new SolidColorBrush(Colors.Maroon);                    
                }
            }
        }
        public void DeselectAll()
        {
            SelectedPoint = new GISMapPoint();
            GISMapPoint Dummy = new GISMapPoint();
            UpdateColors(Dummy.pointrect);            
        }

    }
}