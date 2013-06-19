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

    class GISMapPoint
    {

        public Location PointLocation;
        public Rectangle pointrect;
              
        public GISMapPoint() 
        {
            pointrect = new Rectangle();
            pointrect.Fill = new SolidColorBrush(Colors.Maroon);
            pointrect.Stroke = new SolidColorBrush(Colors.Gold);
            pointrect.StrokeThickness = 1;
            pointrect.Width = 8;
            pointrect.Height = 8;                        
        }

        

    }

    class GISMapPoints
    {
        public ArrayList MapPoints = new ArrayList();
        public LocationCollection PointsList = new LocationCollection();
        public bool AddPolyPoint = false;
        public GISMapPoint SelectedPoint = new GISMapPoint();        
        
        public void AddPoint(Location PushPinLocation,SurfaceWindow1 surfaceWindow1)
         {
            if (AddPolyPoint)
            {
                GISMapPoint GMPoint = new GISMapPoint();
                GMPoint.PointLocation = PushPinLocation;                
                surfaceWindow1.PolyPointLayer.AddChild(GMPoint.pointrect, PushPinLocation);
                MapPoints.Add(GMPoint);
                PointsList.Add(PushPinLocation);                
                GMPoint.pointrect.TouchDown += new EventHandler<TouchEventArgs>(Point_TouchDown);
                              
            }
        }

       

         public void Point_TouchDown(object sender, TouchEventArgs e)
         {             
             Rectangle TouchPoint = sender as Rectangle;

             if (TouchPoint.Equals(SelectedPoint.pointrect))
             {
                 DeselectAll();
                 SelectedPoint.pointrect = null;
             }
             else
             {
                 SelectedPoint.pointrect = TouchPoint;
                 UpdateColors(TouchPoint);                 
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
             this.SelectedPoint = new GISMapPoint();             
             GISMapPoint Dummy = new GISMapPoint();
             UpdateColors(Dummy.pointrect);
         }

    }
}