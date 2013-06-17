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
        public void AddPolygon(SurfaceWindow1 mapwindow)
        {

            MapPolygon NewPolygon = new MapPolygon();

            NewPolygon.Locations = new LocationCollection();
            NewPolygon.Locations = GMPoint.PointsList;

            //  PolyGonCount++;
            NewPolygon.Fill = new SolidColorBrush(Colors.Maroon);
            NewPolygon.Stroke = new SolidColorBrush(Colors.Gold);
            NewPolygon.StrokeThickness = 2;
            NewPolygon.Opacity = 0.8;
            mapwindow.PolygonLayer.Children.Add(NewPolygon);
            //GMPoint.AddPolyPoint = false;
            

            MapPolygons.Add(NewPolygon);
            NewPolygon.Name = "MapPoly" + MapPolygons.Count.ToString();
            NewPolygon.TouchDown += new EventHandler<TouchEventArgs>(Polygon_TouchDown);
            GMPoint.PointsList = new LocationCollection();

            //try
            //{
            //    aPolygon.TouchDown += new EventHandler<TouchEventArgs>(Polygon_TouchDown);
            //    mapwindow.PolygonLayer.Children.Add(aPolygon);
            //    MapPolygons.Add(aPolygon);
            //}
            //catch
            //{
            //    // don't do anything.
            //}
        }

    
        public void Polygon_TouchDown(object sender, TouchEventArgs e)
        {
            MapPolygon TouchPolygon = sender as MapPolygon;

            if (TouchPolygon.Equals(SelectedPolygon))
            {
                DeselectAll();

                //txtDescription.Text = "Select a Polygon";
            }
            else
            {
                SelectedPolygon = TouchPolygon;
                UpdateColors(TouchPolygon);

                //txtDescription.Text = "Polygon " + TouchPolygon.Name + " Selected.\n";
                int pointCounter = 1;



                foreach (Location p in TouchPolygon.Locations)
                {
                  //  txtDescription.Text += "Point " + pointCounter.ToString() + ": Lat:" + p.Latitude.ToString()
                    //    + ", Long:" + p.Longitude.ToString() + "\n";
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
                    //  PolygonLayer.Children.Remove(Polygon);
                    test.Stroke = new SolidColorBrush(Colors.Wheat);
                    test.Fill = new SolidColorBrush(Colors.Blue);
                    //   PolygonLayer.Children.Add(Polygon);                    
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
           // PolygonLayer.Children.Clear();
            MapPolygon Dummy = new MapPolygon();
            SelectedPolygon = null;
        }

        
    }
}
