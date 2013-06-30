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
        string MyMapMode = "Roads";
        string BingMapsKey = "AkrxDoVF81cNDLiNXWiVCzeOT3hrtspPyJUiMHxQcO0wd-tLAX7p8GCBaWPwI-PO";
        SurfaceWindow1 surfaceWindow;
            
        public SurGISContPanel1(SurfaceWindow1 ProgramWindow)
        {
            surfacewindow = ProgramWindow;
            InitializeComponent();




        }


        private void PolyGonToggleButton_Click(object sender, RoutedEventArgs e)
        {
            surfacewindow.PolyPointButton_Click();
        }



        private void btnMode_Click(object sender, RoutedEventArgs e)
        {

            if (MyMapMode == "ArialWithLabels")
            {

                surfacewindow.MainMap.MapTileOverlay.Mode = new RoadMode();
                MyMapMode = "Roads";
               // ModeButton.Content = "Arial";

            }
            else
            {
                surfacewindow.MainMap.MapTileOverlay.Mode = new AerialMode(true);
                MyMapMode = "ArialWithLabels";
                //ModeButton.Content = "Roads";
            }


        }
        public XmlDocument Geocode(string addressQuery)
        {
            //Create REST Services geocode request using Locations API
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=" + BingMapsKey;

            //Make the request and get the response
            XmlDocument geocodeResponse = GetXmlResponse(geocodeRequest);

            return (geocodeResponse);
        }

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



        private void Search_Click(object sender, RoutedEventArgs e)
        {

            surfacewindow.MainMap.Search_Click(sender, e);


            //Clear prior search
            surfacewindow.MainMap.Visibility = Visibility.Hidden;
            //surfacewindow.MainMap.myMapLabel.Visibility = Visibility.Collapsed;
            surfacewindow.MainMap.AddressList.Children.Clear();
            SearchResults.Visibility = Visibility.Collapsed;
            AddressList.Children.Clear();
            ErrorMessage.Visibility = Visibility.Collapsed;


            //Get latitude and longitude coordinates for specified location
             XmlDocument searchResponse = Geocode(SearchNearby.Text);

            //Find and display points of interest near the specified location
            //FindandDisplayNearbyPOI(searchResponse);
        }
        private void AddLabel(Panel parent, string labelString)
        {
            Label dname = new Label();
            dname.Content = labelString;
            dname.Style = (Style)FindResource("AddressStyle");
            parent.Children.Add(dname);
        }
        private void AddPushpinToMap(double latitude, double longitude, string pinLabel)
        {
            Location location = new Location(latitude, longitude);
            Pushpin pushpin = new Pushpin();
            pushpin.Content = pinLabel;
            pushpin.Location = location;
           // surfaceWindow.MainMap.Children.Add(pushpin);
        }

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


                surfaceWindow.MainMap.Visibility = Visibility.Visible;
                //myMapLabel.Visibility = Visibility.Visible;
                surfaceWindow.MainMap.Focus(); //allows '+' and '-' to zoom the map
            }



        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            surfacewindow.Exit();
        }
    }
}
