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

namespace SurGIS
{
    class GISMapPoints
    {
        public ArrayList MapPoints = new ArrayList();
        public LocationCollection PointsList = new LocationCollection();
        public bool AddPolyPoint = false;
        public Point SelectedPoint = new Point();

         public void AddPoint(Location PushPinLocation,SurfaceWindow1 surfaceWindow1)
        {
            if (AddPolyPoint)
            {
                Rectangle r = new Rectangle();
                r.Fill = new SolidColorBrush(Colors.Maroon);
                r.Stroke = new SolidColorBrush(Colors.Gold);
                r.StrokeThickness = 1;
                r.Width = 8;
                r.Height = 8;
                surfaceWindow1.PolyPointLayer.AddChild(r, PushPinLocation);
                PointsList.Add(PushPinLocation);
            }
        }

         //public void Point_TouchDown(object sender, TouchEventArgs e)
         //{
         //    //Still Working on this so comment out
         //    //Point TouchPoint = sender as Point;

         //    if (TouchPoint.Equals(SelectedPoint))
         //    {
         //        DeselectAll();                 
         //    }
         //    else
         //    {
         //        SelectedPoint = TouchPoint;
         //        UpdateColors(TouchPoint);
                 
         //        int pointCounter = 1;

         //        foreach (Location p in TouchPoint)
         //        {
         //            //  txtDescription.Text += "Point " + pointCounter.ToString() + ": Lat:" + p.Latitude.ToString()
         //            //    + ", Long:" + p.Longitude.ToString() + "\n";
         //            pointCounter++;
         //        }
         //    }
         //}

         //private void UpdateColors(Point Point)
         //{

         //    for (int i = 0; i < MapPoints.Count; i++)
         //    {

         //        Rectangle r = new Rectangle();
         //        //Point test = MapPoints[i] as Point;
         //        if (Point.Equals(MapPoints[i]) && Point != null)
         //        {
         //            //  PolygonLayer.Children.Remove(Polygon);
         //            r.Stroke = new SolidColorBrush(Colors.Wheat);
         //            r.Fill = new SolidColorBrush(Colors.Blue);
         //            //   PolygonLayer.Children.Add(Polygon);                    
         //        }
         //        else // not the selected polygon... reset it's color
         //        {
         //            r.Stroke = new SolidColorBrush(Colors.Gold);
         //            r.Fill = new SolidColorBrush(Colors.Maroon);
         //        }


         //    }
         //}
         //private void DeselectAll()
         //{
         //    this.SelectedPoint = new Point();
         //   // this.SelectedPoint = null;
         //    Point Dummy = new Point();
         //    UpdateColors(Dummy);
         //}

    }
}
