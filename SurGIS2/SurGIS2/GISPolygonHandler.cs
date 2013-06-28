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
        public ArrayList MapPolygons = new ArrayList();    // collection of WPF Polygons
        public MapPolygon SelectedPolygon = new MapPolygon();

       // the naming of GMPoint is confusing.  GMPoint (singular) is of type GISMapPoints (plural)  Think of a better name.
        public GISMapPoints GMPoint;
        public MapLayer PointLayer = new MapLayer();  

        // this is a global variable to handle the main program window.  It should make it available to all methods in this class.
        // so we don't have to go searching so hard for it or passing it around like a frisbee

         private SurfaceWindow1 surfacewindow;

       // The Constructor for all classes should receive a handle to the main program window
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
            MapPolygon TouchPolygon = sender as MapPolygon;    // The polygon that was actually touched.          

            if (TouchPolygon.Equals(SelectedPolygon))          // if it's the same as 'selectedpolygon' then it's already selected
            {                
                surfacewindow.MapPolygon.DeselectAll();        // deselect the polygon
            }
            else
            {
                //SelectedPolygon.Equals(TouchPolygon);
   
                surfacewindow.MapPolygon.DeselectAll();        // clear out the selection
                
                UpdateColors(TouchPolygon);                    // color the new polygon

                                
                    surfacewindow.MapPolygon.GMPoint.AddPolyPoint = true;    // set the draw point mode to true
                    
                    foreach (Location MapPoints in TouchPolygon.Locations)  // step through the points in the selected polygon
                    {
                        surfacewindow.MapPolygon.GMPoint.AddPoint(MapPoints);  // draw the point from the polygon on the map
                    }

                    surfacewindow.MapPolygon.GMPoint.AddPolyPoint = false;       // turn off the point drawing.         

                SelectedPolygon = TouchPolygon;   // make the new polygon the selected polygon.
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

        public void RemovePolygon()
        {
            surfacewindow.MainMap.GISlayer.Children.Remove(SelectedPolygon);
        }

        public void ClearMapPolies()    // deletes all polygons from the map
        {
            surfacewindow.MainMap.GISlayer.Children.Clear();
            MapPolygon Dummy = new MapPolygon();
            SelectedPolygon = null;
        }
    }
}
