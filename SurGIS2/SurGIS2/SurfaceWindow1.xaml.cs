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
using System.IO;

/*
 * ++++++++++++++++++  SurfaceWindow1 +++++++++++++++
 * 
 * This is the main program interface class for SurGIS2  It will have a few main components
 * 2 SurfaceSliderLists which go up and down the two sides of the main window.  This will contain the
 * assets that are available to the main window.
 * 
 * The sliders on the sides will watch user-defined directories and present all the data that is available 
 * 
 * There will be a control which can exist as a single scatterview object or as two controls which are docked
 * to the top and bottom edges. There should be a toggle to go between these two control modes.  The main program control will have
 * the following features:
 * 
 * Help/About 
 * Exit
 * Address Lookup
 * Lat/Long go-to
 * configuration
 * 
 * The configuration will allow the user to set the watch directories, configure the network connection to the server and other as-yet
 * determined options.  The config will be written to an xml file which can modified and saved through this main interface.  The config XML
 * will be read at program startup as well.
 * 
 * 
 * 
 * */

namespace SurGIS2
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    /// 
       


    public partial class SurfaceWindow1 : SurfaceWindow
    {
       
        #region GLOBALS

        bool AddPolyMode = false;
       
        public GISMapPolygon MapPolygon;
        public GISMainMap MainMap;
        public SurGISContPanel1 ControlPanel;

        #endregion

        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
            MainMap = new GISMainMap(this);
            ControlPanel = new SurGISContPanel1(this);

           
            // adds the map to the main window
            
            AddMap();       
            AddControl();
           
            MainMap.MapTileOverlay.TouchDown += new EventHandler<TouchEventArgs>(Map_Touch_Event);

            // Adds a polygon handler class.  This should handle all the polygon functions
            MapPolygon = new GISMapPolygon(this);
           
            

        }
        // This next region has all the methods to build needed components

        #region BUILD_COMPONENTS

        void AddControl()
        {
            ScatterViewItem ControlScatterViewItem = new ScatterViewItem();
            ControlScatterViewItem.Width = 680;
            ControlScatterViewItem.Height = 90;
            ControlScatterViewItem.CanScale = false;
            ControlScatterViewItem.ZIndex = 4;
            ControlScatterViewItem.Content = ControlPanel;

           
            
            ControlScatterViewItem.IsTopmostOnActivation = true;
            ControlScatterViewItem.IsContainerActive = true;

            ControlScatterViewItem.ContainerStaysActive = true;
            ScatterView ControlSV = new ScatterView();
            MainGrid.Children.Add(ControlSV);
            ControlSV.Items.Add(ControlScatterViewItem);


        }
        void AddMap()
        {
            // build a scatterviewitem to put the map in.
       /* 
        * The following code creates a scatterview item for the map.  Without it the map is full screen
        * 
        * 
         ScatterViewItem MapScatterViewItem = new ScatterViewItem();
            MapScatterViewItem.Width = 900;
            MapScatterViewItem.Height = 600;
            MapScatterViewItem.Padding = new Thickness(20);

            MapScatterViewItem.Content = MainMap;
            MapScatterViewItem.ZIndex = 1;

            MainScatterView.Items.Add(MapScatterViewItem);
         */
            MainGrid.Children.Add(MainMap);


        }
        #endregion

        // All the default availability handlers.
        #region DEFAULT

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }
        #endregion 


        // All the polygon stuff goes in this region.
        #region PolygonStuff  

        public void Map_Touch_Event(object sender, TouchEventArgs e)
        {
            Location PointLocation = new Location();                                                        // variable for the new touch point location in map coords
            TouchPoint TouchP = e.GetTouchPoint(MainMap.MapTileOverlay);                                    // get the touch point in UI pixels
            Point TPosition = TouchP.Position;                                                              // need a point object to do the conversion
            Location MapTouchPointLocation =   MainMap.MapTileOverlay.ViewportPointToLocation(TPosition);   // convert the point to a map coord location
            MapPolygon.GMPoint.AddPoint(MapTouchPointLocation);                                             // add the point to the map.            
            
                if (MapPolygon.GMPoint.PointSelected)    // if there is a point selected
                {     
                    switch (MapPolygon.GMPoint.TouchCounter){

                        case 0:  
                        {
                            MapPolygon.GMPoint.TouchCounter++;
                            MapPolygon.GMPoint.SelectedPoint.PointLocation = MapTouchPointLocation;
                            MapPolygon.GMPoint.g = MapPolygon.GMPoint.SelectedPoint;
                            MapPolygon.GMPoint.PointDistance(MapTouchPointLocation);
                            break; // do nothing till next time through
                        }
                        case 1:
                        {
                            // move the actual point.  It should take two touches to get to this point
                            //MapPolygon.GMPoint.SelectedPoint.PointLocation = MapTouchPointLocation;
                            MapPolygon.GMPoint.AddPolyPoint = true;
                            MapPolygon.GMPoint.Point_TouchMove(MapTouchPointLocation);
                            MapPolygon.GMPoint.AddPolyPoint = false;
                            MapPolygon.GMPoint.TouchCounter = 0;
                            break;
                        }
                }
                                             
            } // if

              

        }

        //  This toggles the polycreation mode of the map
        // not sure where this method needs to go.  Is it in the main program, the map or the polygon class?

        public void PolyPointButton_Click()
        {
            if (AddPolyMode)   // if we are drawing adding points when this button is clicked... finish drawing
            {
                ControlPanel.PolyGonToggleButton.Content = "Add Poly";   // change the label back
                
                MapPolygon.AddPolygon();   // create the polygon
                AddPolyMode = false;   
                MapPolygon.GMPoint.AddPolyPoint = false;
            }
            else   // if we're not currently drawing a polygon... start drawing.
            {
                MapPolygon.DeselectAll();   // clear out the currently selected polygons and points, if any exit.
                ControlPanel.PolyGonToggleButton.Content = "Create";   // change the button label.
                AddPolyMode = true;
                MapPolygon.GMPoint.AddPolyPoint = true;                
            }

        }


        #endregion





        internal void Exit()
        {
            this.Close();
        }
    }



}