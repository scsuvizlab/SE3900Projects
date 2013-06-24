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


/*++++++++++++++++++=====  SURGIS Polygons +++++++++++++++++
 * 
 * This one's going to be big.  It handles all the working polygons in the GIS Main Map (But not neccearrily all the polygons available in the 
 * program assets listed in the slider windows on he sides of the main interface) 
 * A lot of the data on the terrain will be displayed as polygon data on the map.
 * the polygon data will come from one of two sources, either drawn on the map or read from a file
 * 
 * Polygons should have options for fill color, line color, opacity.
 * Polygons should also be able to accomodate a wide variety of metadata such as:
 * 
 * Area under the polygon,
 * Usage data for the polygon (crop type, demographics etc)
 * lat/long coordinates of each point in the polygon
 * Unique polygone identifier
 * 
 * Functionality for editing an exiting polygon will be implemented.  That could mean moving the entire
 * polygon, deleting the polygon, changing any of the polygon metadata, or moving/creating/deleting any of the 
 * points that make up the polygon.
 * 
 * Polygons and Points should be handled in different classes.
 * 
 * 
 * */

namespace SurGIS2
{

   public class GISMapPolygon
    {
        public ArrayList MapPolygons = new ArrayList();
        MapLayer PushPinLayer = new MapLayer();
        SurfaceInkCanvas GISInkCanvas = new SurfaceInkCanvas();
        MapLayer InkLayer = new MapLayer();
        public MapPolygon SelectedPolygon = new MapPolygon();
        public GISMapPoints GMPoint;
        public MapLayer PointLayer = new MapLayer();

        // this is a global variable to handle the main program window.  It should make it available to all methods in this class.
        // so we don't have to go searching so hard for it or passing it around like a frisbee

         private SurfaceWindow1 surfacewindow;


        public GISMapPolygon(SurfaceWindow1 ProgramWindow)
        {
            surfacewindow = ProgramWindow;
            // pass the program window to the GISMapPoints class
            GMPoint = new GISMapPoints(surfacewindow);
        }

        // This is for creating polygons from touches on the map

        public void ConvertPointLocations(MapPolygon NewPolygon, ArrayList GMPointLocations)
        {
            foreach (GISMapPoint testPoint in GMPointLocations)
            {
                NewPolygon.Locations.Add(testPoint.PointLocation);
            }
        }

        /*
         * this was used to take the Points from the SelectedPolygon and say that that are indeed connected to the SelectedPolygon, and nothing else.
         * it also is suppose to allow you to move a selected point if that point is connected to the polygon.
         */

        //public void FindPoint(GISMapPoint SelectedPoint)
        //{
        //    foreach (MapPolygon Polygon in MapPolygons)
        //    {
        //        foreach (Location GMPoint in Polygon.Locations)
        //        {
        //            if (SelectedPoint.PointLocation.Equals(GMPoint))
        //            {
        //                MovePoint(GMPoint, Polygon, SelectedPoint);
        //                break;
        //            }
        //        }
        //    }

        //}

        /*
         * this is where we tried to make it so that a selected point is able to be moved. but we only want the points that are connected to a SelectedPolygon to move
         * which leads us to the commented out methods and errors in the Polygon_TouchDown part and the AddPoint part.
         */

        internal void MovePoint(GISMapPoint GMPoint, GISMapPolygon Polygon, GISMapPoint SelectedPoint)
        {
            GMPoint.PointLocation = SelectedPoint.PointLocation;
            Polygon.AddPolygon();
        }

    
        public void AddPolygon()
        {


            MapPolygon NewPolygon = new MapPolygon();
            

            NewPolygon.Locations = new LocationCollection();            
            ConvertPointLocations(NewPolygon, GMPoint.MapPoints);

            NewPolygon.Fill = new SolidColorBrush(Colors.Maroon);
            NewPolygon.Stroke = new SolidColorBrush(Colors.Gold);
            NewPolygon.StrokeThickness = 2;
            NewPolygon.Opacity = 0.8;
            surfacewindow.MainMap.GISlayer.Children.Add(NewPolygon);

            MapPolygons.Add(NewPolygon);
            NewPolygon.Name = "MapPoly" + MapPolygons.Count.ToString();
            NewPolygon.TouchDown += new EventHandler<TouchEventArgs>(Polygon_TouchDown);                          
            
            surfacewindow.MapPolygon.GMPoint.MapPoints = new ArrayList();
            surfacewindow.MainMap.PointLayer.Children.Clear();

        }

        public void Polygon_TouchDown(object sender, TouchEventArgs e)
        {
            MapPolygon TouchPolygon = sender as MapPolygon;            

            if (TouchPolygon.Equals(SelectedPolygon))
            {                
                surfacewindow.MapPolygon.DeselectAll();
            }
            else
            {
                //surfacewindow.MapPolygon.DeselectAll();
                //SelectedPolygon = TouchPolygon;
                UpdateColors(TouchPolygon);

                if (TouchPolygon.Equals(SelectedPolygon))
                {
                    surfacewindow.MapPolygon.GMPoint.AddPolyPoint = true;
                    
                    foreach (Location TempLocation in SelectedPolygon.Locations)
                    {
                        surfacewindow.MapPolygon.GMPoint.AddPoint(TempLocation);
                    }

                    surfacewindow.MapPolygon.GMPoint.AddPolyPoint = false;
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
        public void DeselectAll()
        {
            surfacewindow.MapPolygon.SelectedPolygon = new MapPolygon();
            surfacewindow.MapPolygon.SelectedPolygon = null;
            MapPolygon Dummy = new MapPolygon();
            UpdateColors(Dummy);
            surfacewindow.MapPolygon.GMPoint.MapPoints = new ArrayList(); 
            surfacewindow.MainMap.PointLayer.Children.Clear();                       
        }

        public void ClearMapPolies()
        {
            surfacewindow.MainMap.GISlayer.Children.Clear();
            MapPolygon Dummy = new MapPolygon();
            SelectedPolygon = null;
        }
    }
}
