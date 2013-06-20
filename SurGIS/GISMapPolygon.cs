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

    class GISMapPolygon
    {                                   
        public ArrayList MapPolygons = new ArrayList();               
        MapLayer PushPinLayer = new MapLayer();             
        SurfaceInkCanvas GISInkCanvas = new SurfaceInkCanvas();       
        MapLayer InkLayer = new MapLayer();
        public MapPolygon SelectedPolygon = new MapPolygon();
        public GISMapPoints GMPoint = new GISMapPoints();      

        // This is for creating polygons from touches on the map

        public void ConvertPointLocations(MapPolygon NewPolygon, ArrayList GMPointLocations)
        {
            foreach(GISMapPoint testPoint in GMPointLocations)
            {
                NewPolygon.Locations.Add(testPoint.PointLocation);
            }
        }

        /*
         * this was used to take the Points from the SelectedPolygon and say that that are indeed connected to the SelectedPolygon, and nothing else.
         * it also is suppose to allow you to move a selected point if that point is connected to the polygon.
         */

        internal static void FindPoint(GISMapPoint SelectedPoint, SurfaceWindow1 mapwindow)
        {
            foreach(GISMapPolygon Polygon in mapwindow.MapPolygons)
            {
                foreach(GISMapPoint GMPoint in mapwindow.MapPolygons)
                {
                    if (SelectedPoint.Equals(GMPoint))
                    {
                        MovePoint(GMPoint, Polygon, SelectedPoint,mapwindow);
                        break;
                    }
                }
            }

        }

        /*
         * this is where we tried to make it so that a selected point is able to be moved. but we only want the points that are connected to a SelectedPolygon to move
         * which leads us to the commented out methods and errors in the Polygon_TouchDown part and the AddPoint part.
         */

        internal static void MovePoint(GISMapPoint GMPoint, GISMapPolygon Polygon, GISMapPoint SelectedPoint, SurfaceWindow1 mapwindow)
        {
            GMPoint.PointLocation = SelectedPoint.PointLocation;
            Polygon.AddPolygon(mapwindow);

        }

        public void AddPolygon(SurfaceWindow1 mapwindow)
        {

            MapPolygon NewPolygon = new MapPolygon();

            NewPolygon.Locations = new LocationCollection();           
            //NewPolygon.Locations = GMPoint.PointsList;
            ConvertPointLocations(NewPolygon, GMPoint.MapPoints);

            NewPolygon.Fill = new SolidColorBrush(Colors.Maroon);
            NewPolygon.Stroke = new SolidColorBrush(Colors.Gold);
            NewPolygon.StrokeThickness = 2;
            NewPolygon.Opacity = 0.8;
            mapwindow.PolygonLayer.Children.Add(NewPolygon);

            
            MapPolygons.Add(NewPolygon);
            NewPolygon.Name = "MapPoly" + MapPolygons.Count.ToString();
           // NewPolygon.TouchDown += new EventHandler<TouchEventArgs>(Polygon_TouchDown);            
            /*
             * this is where we are trying to make it so that we can pass in SurfaceWindow1 so that we can make the selected Polygon show the points it is made from. 
             * right now the polygon is drawn from plotted points, then the points are erased, since they no longer have a purpose to be there.
             * see if statement inside Polygon_TouchDown Method for more detail
             * until this error is fixed the Polygon_Touchdown methods does nothing so the user can NOT select a polygon.
             */
            //NewPolygon.TouchDown += new EventHandler(void Polygon_TouchDown(mapwindow, this, ));
            GMPoint.MapPoints = new ArrayList();
            
        }        
    
        public void Polygon_TouchDown(SurfaceWindow1 mapwindow, object sender, TouchEventArgs e)
        {
            MapPolygon TouchPolygon = sender as MapPolygon;            
            
            if (TouchPolygon.Equals(SelectedPolygon))
            {
                DeselectAll();
            }
            else
            {
                SelectedPolygon = TouchPolygon;
                UpdateColors(TouchPolygon);

                /*
                 * this is the if statement where we are trying to make it so that when a polygon is selected it redraws its points so that the points, in the future,
                 * the points and polygons can be edited.
                 * right now the idea is that the foreach loop takes the locations of the SelectedPolygons points and redraws the points using the locations from the SelectedPolygon.
                 * See GISMapPoints Class under AddPoints Method.
                 */

                //if (TouchPolygon.Equals(SelectedPolygon))
                //{
                //    foreach (Location TempLocation in SelectedPolygon.Locations)
                //    {                        
                //        GMPoint.AddPoint(TempLocation, mapwindow);
                //    }
                //}
                
                int pointCounter = 1;

                foreach (Location p in TouchPolygon.Locations)
                {                  
                    pointCounter++;
                }
            }
        }

        private void UpdateColors(MapPolygon Polygon)
        {

            for (int i = 0; i < MapPolygons.Count; i++)
            {

                MapPolygon test = MapPolygons[i] as MapPolygon;
                if (Polygon.Equals(MapPolygons[i]) && Polygon != null)
                {                    
                    test.Stroke = new SolidColorBrush(Colors.Wheat);
                    test.Fill = new SolidColorBrush(Colors.Blue);                    
                }
                else // not the selected polygon... reset it's color
                {
                    test.Stroke = new SolidColorBrush(Colors.Gold);
                    test.Fill = new SolidColorBrush(Colors.Maroon);
                }
            }
        }
        private void DeselectAll()
        {
            this.SelectedPolygon = new MapPolygon();
            this.SelectedPolygon = null;
            MapPolygon Dummy = new MapPolygon();
            UpdateColors(Dummy);
        }

        public void ClearMapPolies()
        {
            MapPolygons.Clear();           
            MapPolygon Dummy = new MapPolygon();
            SelectedPolygon = null;
        }      
    }
}
