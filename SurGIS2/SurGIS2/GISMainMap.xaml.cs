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
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Globalization;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Design;
using System.Xml;
using System.Net;
using System.Xml.XPath;




/*
 *  +++++++++++++++++++++++++===  GIS Main Map ++++++++++++++++++++++++++
 *  
 * This is the control where the main Bing Map will reside.   It should have several
 * maplayers for displaying polygon and other data.
 * 
 * There should be controls for clearing the map, saving the current map data and adding other kinds of data that the map will need
 * 
 * Image overlays.  The map should have controls for finding and loading map overlays (new terrain imagery) 
 * 
 * The main map will also have controls for controling the over-all opacity for image overlays and polygon data.
 * 
 * The main map will also provide any new interfaces and controls that are needed for editing polygon data and metadata.
 * 
 * 
 * 
 * */

namespace SurGIS2
{
    /// <summary>
    /// Interaction logic for GISMainMap.xaml
    /// </summary>
    public partial class GISMainMap : UserControl
    {
        LocationConverter locConverter = new LocationConverter();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// 

        // The user defined polygon to add to the map.
       // MapPolygon newPolygon = null;
        // The map layer containing the polygon points defined by the user.
       // public MapLayer polygonPointLayer = new MapLayer();

        // A collection of key/value pairs containing the event name 
        // and the text block to display the event to.
        Dictionary<string, TextBlock> eventBlocks = new Dictionary<string, TextBlock>();
        // A collection of key/value pairs containing the event name  
        // and the number of times the event fired.
        Dictionary<string, int> eventCount = new Dictionary<string, int>();

        MapTileLayer tileLayer;
        private double tileOpacity = 0.50;

        string BingMapsKey = "AkrxDoVF81cNDLiNXWiVCzeOT3hrtspPyJUiMHxQcO0wd-tLAX7p8GCBaWPwI-PO";
        SurfaceWindow1 surfaceWindow;

        public GISMainMap(SurfaceWindow1 ProgramWindow)
        {
          
            InitializeComponent();
            //Set focus on the map
            MapTileOverlay.Focus();

          //  SetUpNewPolygon();
            // Adds location points to the polygon for every single mouse click
            MapTileOverlay.MouseDoubleClick += new MouseButtonEventHandler(
               MapWithPolygon_MouseDoubleClick);

            // Adds the layer that contains the polygon points
           // NewPolygonLayer.Children.Add(polygonPointLayer);

            // Displays the current latitude and longitude as the map animates.
            MapTileOverlay.ViewChangeOnFrame += new EventHandler<MapEventArgs>(viewMap_ViewChangeOnFrame);
            // The default animation level: navigate between different map locations.
            //viewMap.AnimationLevel = AnimationLevel.Full;

            // surfacewindow is a global handle back to the main program.
            surfaceWindow = ProgramWindow;

        }
         private void viewMap_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            // Gets the map object that raised this event.
            Map map = sender as Map;
            // Determine if we have a valid map object.
            if (map != null)
            {
                // Gets the center of the current map view for this particular frame.
                Location mapCenter = map.Center;

                // Updates the latitude and longitude values, in real time,
                // as the map animates to the new location.
                txtLatitude.Text = string.Format(CultureInfo.InvariantCulture,
                  "{0:F5}", mapCenter.Latitude);
                txtLongitude.Text = string.Format(CultureInfo.InvariantCulture,
                    "{0:F5}", mapCenter.Longitude);
            }
        }


        private void ChangeMapView_Click(object sender, RoutedEventArgs e)
        {
            // Parse the information of the button's Tag property
            string[] tagInfo = ((Button)sender).Tag.ToString().Split(' ');
            Location center = (Location)locConverter.ConvertFrom(tagInfo[0]);
            double zoom = System.Convert.ToDouble(tagInfo[1]);

            // Set the map view
            MapTileOverlay.SetView(center, zoom);
        }


        private void AnimationLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)(((ComboBox)sender).SelectedItem);
            string v = cbi.Content as string;
            if (!string.IsNullOrEmpty(v) && MapTileOverlay != null)
            {
                AnimationLevel newLevel = (AnimationLevel)Enum.Parse(typeof(AnimationLevel), v, true);
                MapTileOverlay.AnimationLevel = newLevel;
            }
        }


        private void SetUpNewPolygon()  /// remove this method.  All polgyon stuff will be handled by the polygon class
        {
            //newPolygon = new MapPolygon();
            //// Defines the polygon fill details
            //newPolygon.Locations = new LocationCollection();
            //newPolygon.Fill = new SolidColorBrush(Colors.Blue);
            //newPolygon.Stroke = new SolidColorBrush(Colors.Green);
            //newPolygon.StrokeThickness = 3;
            //newPolygon.Opacity = 0.8;
            ////Set focus back to the map so that +/- work for zoom in/out
            //MapTileOverlay.Focus();
        }

        private void MapWithPolygon_MouseDoubleClick(object sender, MouseButtonEventArgs e)  // remove this method, all the polygon function will go in the polygon class
        {
            //e.Handled = true;
            //// Creates a location for a single polygon point and adds it to
            //// the polygon's point location list.
            //Point mousePosition = e.GetPosition(this);
            ////Convert the mouse coordinates to a location on the map
            //Location polygonPointLocation = MapTileOverlay.ViewportPointToLocation(
            //    mousePosition);
            //newPolygon.Locations.Add(polygonPointLocation);

            //// A visual representation of a polygon point.
            //Rectangle r = new Rectangle();
            //r.Fill = new SolidColorBrush(Colors.Red);
            //r.Stroke = new SolidColorBrush(Colors.Yellow);
            //r.StrokeThickness = 1;
            //r.Width = 8;
            //r.Height = 8;

            //// Adds a small square where the user clicked, to mark the polygon point.
            //polygonPointLayer.AddChild(r, polygonPointLocation);
            ////Set focus back to the map so that +/- work for zoom in/out
            //MapTileOverlay.Focus();

        }

        private void btnCreatePolygon_Click(object sender, RoutedEventArgs e)  // remove this method.  blah blah polygon class.
        {
            //If there are two or more points, add the polygon layer to the map
            //if (newPolygon.Locations.Count >= 2)
            //{
            //    // Removes the polygon points layer.
            //    polygonPointLayer.Children.Clear();

            //    // Adds the filled polygon layer to the map.
            //    NewPolygonLayer.Children.Add(newPolygon);
            //    SetUpNewPolygon();
            //}
        }
        // the methods in the following region are all default stubs.
        #region DEFAULT 
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

        private void addImageToMap(object sender, RoutedEventArgs e)
        {
            MapLayer imageLayer = new MapLayer();


            Image image = new Image();
            image.Height = 150;
            //Define the URI location of the image
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri("http://upload.wikimedia.org/wikipedia/commons/d/d4/Golden_Gate_Bridge10.JPG");
            // To save significant application memory, set the DecodePixelWidth or  
            // DecodePixelHeight of the BitmapImage value of the image source to the desired 
            // height or width of the rendered image. If you don't do this, the application will 
            // cache the image as though it were rendered as its normal size rather then just 
            // the size that is displayed.
            // Note: In order to preserve aspect ratio, set DecodePixelWidth
            // or DecodePixelHeight but not both.
            //Define the image display properties
            myBitmapImage.DecodePixelHeight = 150;
            myBitmapImage.EndInit();
            image.Source = myBitmapImage;
            image.Opacity = 0.6;
            image.Stretch = System.Windows.Media.Stretch.None;

            //The map location to place the image at
            Location location = new Location() { Latitude = 37.8197222222222, Longitude = -122.478611111111 };
            //Center the image around the location specified
            PositionOrigin position = PositionOrigin.Center;

            //Add the image to the defined map layer
            imageLayer.AddChild(image, location, position);
            //Add the image layer to the map
            MapTileOverlay.Children.Add(imageLayer);
        }


        private void MapWithPushpins_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(this);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = MapTileOverlay.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;

            // Adds the pushpin to the map.
            MapTileOverlay.Children.Add(pin);
        }

        public void MapEvents()
        {
            InitializeComponent();
            //Set focus on map
            MapTileOverlay.Focus();

            // Fires every animated frame from one location to another.
            MapTileOverlay.ViewChangeOnFrame +=
                new EventHandler<MapEventArgs>(MapWithEvents_ViewChangeOnFrame);
            // Fires when the map view location has changed.
            MapTileOverlay.TargetViewChanged +=
                new EventHandler<MapEventArgs>(MapWithEvents_TargetViewChanged);
            // Fires when the map view starts to move to its new target view.
            MapTileOverlay.ViewChangeStart +=
                new EventHandler<MapEventArgs>(MapWithEvents_ViewChangeStart);
            // Fires when the map view has reached its new target view.
            MapTileOverlay.ViewChangeEnd +=
                new EventHandler<MapEventArgs>(MapWithEvents_ViewChangeEnd);
            // Fires when a different mode button on the navigation bar is selected.
            MapTileOverlay.ModeChanged +=
                new EventHandler<MapEventArgs>(MapWithEvents_ModeChanged);
            // Fires when the mouse is double clicked
            MapTileOverlay.MouseDoubleClick +=
                new MouseButtonEventHandler(MapWithEvents_MouseDoubleClick);
            // Fires when the mouse wheel is used to scroll the map
            MapTileOverlay.MouseWheel +=
                new MouseWheelEventHandler(MapWithEvents_MouseWheel);
            // Fires when the left mouse button is depressed
            MapTileOverlay.MouseLeftButtonDown +=
                new MouseButtonEventHandler(MapWithEvents_MouseLeftButtonDown);
            // Fires when the left mouse button is released
            MapTileOverlay.MouseLeftButtonUp +=
                new MouseButtonEventHandler(MapWithEvents_MouseLeftButtonUp);
        }

        void MapWithEvents_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            // Updates the count of single mouse clicks.
            ShowEvent("MapWithEvents_MouseLeftButtonUp");
        }

        void MapWithEvents_MouseWheel(object sender, MouseEventArgs e)
        {
            // Updates the count of mouse drag boxes created.
            ShowEvent("MapWithEvents_MouseWheel");
        }

        void MapWithEvents_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            // Updates the count of mouse pans.
            ShowEvent("MapWithEvents_MouseLeftButtonDown");
        }

        void MapWithEvents_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Updates the count of mouse double clicks.
            ShowEvent("MapWithEvents_MouseDoubleClick");
        }

        void MapWithEvents_ViewChangeEnd(object sender, MapEventArgs e)
        {
            //Updates the number of times the map view has changed.
            ShowEvent("ViewChangeEnd");
        }

        void MapWithEvents_ViewChangeStart(object sender, MapEventArgs e)
        {
            //Updates the number of times the map view started changing.
            ShowEvent("ViewChangeStart");
        }

        void MapWithEvents_ViewChangeOnFrame(object sender, MapEventArgs e)
        {
            // Updates the number of times a map view has changed 
            // during an animation from one location to another.
            ShowEvent("ViewChangeOnFrame");
        }
        void MapWithEvents_TargetViewChanged(object sender, MapEventArgs e)
        {
            // Updates the number of map view changes that occured during
            // a zoom or pan.
            ShowEvent("TargetViewChange");
        }

        void MapWithEvents_ModeChanged(object sender, MapEventArgs e)
        {
            // Updates the number of times the map mode changed.
            ShowEvent("ModeChanged");
        }

        void ShowEvent(string eventName)
        {
            // Updates the display box showing the number of times 
            // the wired events occured.
            if (!eventBlocks.ContainsKey(eventName))
            {
                TextBlock tb = new TextBlock();
                tb.Foreground = new SolidColorBrush(
                    Color.FromArgb(255, 128, 255, 128));
                tb.Margin = new Thickness(5);
                eventBlocks.Add(eventName, tb);
                eventCount.Add(eventName, 0);
                ////eventsPanel.Children.Add(tb);
            }

            eventCount[eventName]++;
            eventBlocks[eventName].Text = String.Format(
                "{0}: [{1} times] {2} (HH:mm:ss:ffff)",
                eventName, eventCount[eventName].ToString(), DateTime.Now.ToString());
        }

        private void AddTileOverlay()
        {

            // Create a new map layer to add the tile overlay to.
            tileLayer = new MapTileLayer();

            // The source of the overlay.
            TileSource tileSource = new TileSource();
            tileSource.UriFormat = "{UriScheme}://ecn.t0.tiles.virtualearth.net/tiles/r{quadkey}.jpeg?g=129&mkt=en-us&shading=hill&stl=H";

            // Add the tile overlay to the map layer
            tileLayer.TileSource = tileSource;

            // Add the map layer to the map
            if (!MapTileOverlay.Children.Contains(tileLayer))
            {
                MapTileOverlay.Children.Add(tileLayer);
            }
            tileLayer.Opacity = tileOpacity;
        }

        private void btnAddCities_Click( object sender, RoutedEventArgs e)
        {
            Button b = new Button();
      
           

         List<Button> myDinamicButtonsList = new List<Button>();
  




        }

        private void btnRemoveCities_Click(object sender, RoutedEventArgs e)
        {


   
        }

        ////////REST service

        // Geocode an address and return a latitude and longitude
        public XmlDocument Geocode(string addressQuery)
        {
            //Create REST Services geocode request using Locations API
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=" + BingMapsKey;

            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);

            return (geocodeResponse);
        }


        // Submit a REST Services or Spatial Data Services request and return the response
        private XmlDocument GetXmlResponse(string requestUrl)
        {
            System.Diagnostics.Trace.WriteLine("Request URL (XML): " + requestUrl);
            HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;
            }
        }

        //Search for POI near a point
        private void FindandDisplayNearbyPOI(XmlDocument xmlDoc)
        {
            //Get location information from geocode response 

            //Create namespace manager
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            //Get all geocode locations in the response 
            XmlNodeList locationElements = xmlDoc.SelectNodes("//rest:Location", nsmgr);
            if (locationElements.Count == 0)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Content = "The location you entered could not be geocoded.";
            }
            else
            {
                //Get the geocode location points that are used for display (UsageType=Display)
                XmlNodeList displayGeocodePoints =
                        locationElements[0].SelectNodes(".//rest:GeocodePoint/rest:UsageType[.='Display']/parent::node()", nsmgr);
                string latitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Latitude", nsmgr).InnerText;
                string longitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Longitude", nsmgr).InnerText;
                /////ComboBoxItem entityTypeID = (ComboBoxItem)EntityType.SelectedItem;
                ////ComboBoxItem distance = (ComboBoxItem)Distance.SelectedItem;

                //Create the Bing Spatial Data Services request to get the user-specified POI entity type near the selected point  
                string findNearbyPOIRequest = "http://spatial.virtualearth.net/REST/v1/data/f22876ec257b474b82fe2ffcb8393150/NavteqNA/NavteqPOIs?spatialfilter=nearby("
                    ///// + latitude + "," + longitude + "," + distance.Content + ")"
                    ///// + "&$filter=EntityTypeID%20EQ%20'" + entityTypeID.Tag + "'&$select=EntityID,DisplayName,__Distance,Latitude,Longitude,AddressLine,Locality,AdminDistrict,PostalCode&$top=10"
                + "&key=" + BingMapsKey;

                //Submit the Bing Spatial Data Services request and retrieve the response
                XmlDocument nearbyPOI = GetXmlResponse(findNearbyPOIRequest);

                //Center the map at the geocoded location and display the results
                /////myMap.Center = new Location(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
                ////// myMap.ZoomLevel = 12;
                DisplayResults(nearbyPOI);

            }
        }


        //Add label element to application
        private void AddLabel(Panel parent, string labelString)
        {
            Label dname = new Label();
            dname.Content = labelString;
            dname.Style = (Style)FindResource("AddressStyle");
            parent.Children.Add(dname);
        }

        //Add a pushpin with a label to the map
        private void AddPushpinToMap(double latitude, double longitude, string pinLabel)
        {
            Location location = new Location(latitude, longitude);
            Pushpin pushpin = new Pushpin();
            pushpin.Content = pinLabel;
            pushpin.Location = location;
            /////myMap.Children.Add(pushpin);
        }

        //Show the POI address information and insert pushpins on the map
        private void DisplayResults(XmlDocument nearbyPOI)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nearbyPOI.NameTable);
            nsmgr.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
            nsmgr.AddNamespace("m", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
            nsmgr.AddNamespace("a", "http://www.w3.org/2005/Atom");

            //Get the the entityID for each POI entity in the response
            XmlNodeList displayNameList = nearbyPOI.SelectNodes("//d:DisplayName", nsmgr);

            //Provide entity information and put a pushpin on the map.
            if (displayNameList.Count == 0)
            {
                ErrorMessage.Content = "No results were found for this location.";
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                XmlNodeList addressLineList = nearbyPOI.SelectNodes("//d:AddressLine", nsmgr);
                XmlNodeList localityList = nearbyPOI.SelectNodes("//d:Locality", nsmgr);
                XmlNodeList adminDistrictList = nearbyPOI.SelectNodes("//d:AdminDistrict", nsmgr);
                XmlNodeList postalCodeList = nearbyPOI.SelectNodes("//d:PostalCode", nsmgr);
                XmlNodeList latitudeList = nearbyPOI.SelectNodes("//d:Latitude", nsmgr);
                XmlNodeList longitudeList = nearbyPOI.SelectNodes("//d:Longitude", nsmgr);
                for (int i = 0; i < displayNameList.Count; i++)
                {
                    AddLabel(AddressList, "[" + Convert.ToString(i + 1) + "] " + displayNameList[i].InnerText);
                    AddLabel(AddressList, addressLineList[i].InnerText);
                    AddLabel(AddressList, localityList[i].InnerText + ", " + adminDistrictList[i].InnerText);
                    AddLabel(AddressList, postalCodeList[i].InnerText);
                    AddLabel(AddressList, "");
                    AddPushpinToMap(Convert.ToDouble(latitudeList[i].InnerText), Convert.ToDouble(longitudeList[i].InnerText), Convert.ToString(i + 1));
                }
                SearchResults.Visibility = Visibility.Visible;
                ///// myMap.Visibility = Visibility.Visible;
                /////myMapLabel.Visibility = Visibility.Visible;
                /////myMap.Focus(); //allows '+' and '-' to zoom the map
            }

        }

        //Search for POI elements when the Search button is clicked
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //Clear prior search
            //// myMap.Visibility = Visibility.Hidden;
            ////myMapLabel.Visibility = Visibility.Collapsed;
            //// myMap.Children.Clear();
            SearchResults.Visibility = Visibility.Collapsed;
            AddressList.Children.Clear();
            ErrorMessage.Visibility = Visibility.Collapsed;


            //Get latitude and longitude coordinates for specified location
            //////// XmlDocument searchResponse = Geocode(SearchNearby.Text);

            //Find and display points of interest near the specified location
            //// FindandDisplayNearbyPOI(searchResponse);
        }



        ////////////////dynamic
 /*       private void addButtons()
        {
            TextBox dynamictexbox = new TextBox();
            dynamictexbox.Text = "(Enter some text)";
            Button dynamicbutton = new Button();
            dynamicbutton.Click += new System.EventHandler(this.btnAddCities_Click);
            Panel1.Controls.Add(dynamicbutton);

        }

        private void removeButtons(object sender, System.EventArgs e)
        {

         if(panel1.Controls.Contains(dynamicbutton)
           {
             this.newPanelButton.Click -= new System.EventHandler(this.btnRemoveCities_Click);
             panel1.Controls.Remove(dynamicbutton);
             newPanelButton.Dispose();
           }

         }

            
        private void btnAddCities_Click(Object sender, System.EventArgs e)
        {
            TextBox tb = new TextBox();
            tb = (TextBox)(Panel1.FindControl("dynamictextbox"));
            Label1.Text = tb.Text;
        }
*///////
    }
  }

