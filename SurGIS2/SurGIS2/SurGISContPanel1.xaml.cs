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


namespace SurGIS2
{
    /// <summary>
    /// Interaction logic for SurGISContPanel1.xaml
    /// </summary>
    public partial class SurGISContPanel1 : UserControl
    {
        SurfaceWindow1 surfacewindow;
        LocationConverter locConverter = new LocationConverter();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// 

        // The user defined polygon to add to the map.
         //MapPolygon newPolygon = null;
        // The map layer containing the polygon points defined by the user.
        // public MapLayer polygonPointLayer = new MapLayer();

        // A collection of key/value pairs containing the event name 
        // and the text block to display the event to.
        Dictionary<string, TextBlock> eventBlocks = new Dictionary<string, TextBlock>();
        // A collection of key/value pairs containing the event name  
        // and the number of times the event fired.
        Dictionary<string, int> eventCount = new Dictionary<string, int>();

       // MapTileLayer tileLayer;
      //  private double tileOpacity = 0.50;

       // string BingMapsKey = "AkrxDoVF81cNDLiNXWiVCzeOT3hrtspPyJUiMHxQcO0wd-tLAX7p8GCBaWPwI-PO";
      //  SurfaceWindow1 surfaceWindow;
        public SurGISContPanel1(SurfaceWindow1 ProgramWindow)
        {
            surfacewindow = ProgramWindow;
            InitializeComponent();



        }

        private void ChangeMapView_Click(object sender, RoutedEventArgs e)
        {
            // Parse the information of the button's Tag property
            string[] tagInfo = ((Button)sender).Tag.ToString().Split(' ');
            Location center = (Location)locConverter.ConvertFrom(tagInfo[0]);
            double zoom = System.Convert.ToDouble(tagInfo[1]);

            // Set the map view
           // MapTileOverlay.SetView(center, zoom);
        }

        private void AnimationLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)(((ComboBox)sender).SelectedItem);
            string v = cbi.Content as string;
        /*    if (!string.IsNullOrEmpty(v) && MapTileOverlay != null)
            {
                AnimationLevel newLevel = (AnimationLevel)Enum.Parse(typeof(AnimationLevel), v, true);
                MapTileOverlay.AnimationLevel = newLevel;
            }*/
        }

        private void PolyGonToggleButton_Click(object sender, RoutedEventArgs e)
        {
            surfacewindow.PolyPointButton_Click();
        }

        private void btnAddCities_Click(object sender, RoutedEventArgs e)
        {
            Button b = new Button();



            List<Button> myDinamicButtonsList = new List<Button>();

        }


        private void btnRemoveCities_Click(object sender, RoutedEventArgs e)
        {
           // surfacewindow.MainMap;



        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

            surfacewindow.MainMap.Search_Click(sender, e);
            //Clear prior search
            //surfacewindow.MainMap.Visibility = Visibility.Hidden;
           // surfacewindow.MainMap.myMapLabel.Visibility = Visibility.Collapsed;
           // surfacewindow.MainMap.AddressList.Children.Clear();
           //// SearchResults.Visibility = Visibility.Collapsed;
           //// AddressList.Children.Clear();
           //// ErrorMessage.Visibility = Visibility.Collapsed;


           // //Get latitude and longitude coordinates for specified location
           // XmlDocument searchResponse = Geocode(SearchNearby.Text);

            //Find and display points of interest near the specified location
            //// FindandDisplayNearbyPOI(searchResponse);
        }
    }
}
