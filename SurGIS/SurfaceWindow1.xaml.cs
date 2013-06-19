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

namespace SurGIS
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        ///        

        GISMapPolygon GMPolygon = new GISMapPolygon();
        SurXML_IO XML_IO = new SurXML_IO();
        //GISMapPoints GMPoint = new GISMapPoints();
        //GMP.Parent = this;

        string MyMapMode;
        string AddStrokesMode="NONE";
        //bool AddPolyPoint = false;
       // bool EditPolyPoint = false;
        bool AddPushPinMode = false;
     //   int PolyGonCount = 0;

     //   public ArrayList ARList;
        public ArrayList FileNames;

    //    public ArrayList LocalARList;
        public ArrayList LocalFileNames;

       //rrayList PointsList;
        LocationCollection PointsList = new LocationCollection();
        public ArrayList MapPolygons = new ArrayList();
        public MapPolygon SelectedPolygon = new MapPolygon();
        // The user defined polygon to add to the map.       
        
        // The map layer containing the polygon points defined by the user.
        MapLayer PushPinLayer = new MapLayer();
       public MapLayer PolygonLayer = new MapLayer();
       public MapLayer PolyPointLayer = new MapLayer();
        MapLayer InkLayer = new MapLayer();
        SurfaceInkCanvas GISInkCanvas = new SurfaceInkCanvas();
        bool WeatherMode = false;

        // Create a new map layer to add the tile overlay to.
          MapTileLayer  tileLayer = new MapTileLayer();

        public FileSystemWatcher FSWatcher;
        public FileSystemWatcher LocalWatcher;
     
        public SurfaceWindow1()
        {
            try
            {
                InitializeComponent();
                AddWindowAvailabilityHandlers();
            }
            catch (NullReferenceException i)
            {
                MessageBoxResult result = MessageBox.Show("InitError", i.Message);
            }
            // Add handlers for window availability events
            


            MyMapMode = "ArialWithLabels";
            myMap.Children.Add(PushPinLayer);   // add the polygon layer to the map
            myMap.Children.Add(PolygonLayer);
            GISInkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
            GISInkCanvas.GotTouchCapture+=new EventHandler<TouchEventArgs>(GISInkCanvas_TouchDown);
            GISInkCanvas.DefaultDrawingAttributes.Height = 15;
            GISInkCanvas.DefaultDrawingAttributes.Width = 15;
            InkLayer.Children.Add(GISInkCanvas);

           
            myMap.Focus();


            StartFSWatcher();

        }

        private void StartFSWatcher()
        {

            FSWatcher = new FileSystemWatcher("./Images");
            LocalWatcher = new FileSystemWatcher("C:/LocalGISImages");
            FSWatcher.SynchronizingObject = NetBMPList as ISynchronizeInvoke;
            LocalWatcher.SynchronizingObject = LocalBMPList as ISynchronizeInvoke;

            FSWatcher.Filter = "*.xml";
            LocalWatcher.Filter = "*.xml";


            FSWatcher.Created += new FileSystemEventHandler(FSWatcher_Created);
            LocalWatcher.Created += new FileSystemEventHandler(FSWatcher_Created);

            FSWatcher.EnableRaisingEvents = true;
            LocalWatcher.EnableRaisingEvents = true;
           
        }

        public void FSWatcher_Created(object sender, FileSystemEventArgs e)
        {

           

            try
            {

                this.Dispatcher.Invoke((Action)(() =>
                {

                    System.Threading.Thread.Sleep(1000);

                    RefreshLists();

                }));

            }
            catch (IOException i)
            {
               
            }
        }

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

        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
           // string MapMode = myMap.Mode.ToString();
            if (MyMapMode == "ArialWithLabels")
            {
                myMap.Mode = new RoadMode();
                MyMapMode = "Roads";
                ModeButton.Content = "Arial";

            }
            else
            {
                myMap.Mode = new AerialMode(true);
                MyMapMode = "ArialWithLabels";
                ModeButton.Content = "Roads";
            }
        }

        private void BingScatter1_Initialized(object sender, EventArgs e)
        {
            BingScatter1.IsTopmostOnActivation = false;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {


            bool GoodLOC = true;

            double Lat = 0.0;
            double Long = 0.0;
            try
            {
                Lat = double.Parse(latTextBox.Text);
            }
            catch (FormatException)
            {

                MessageBox.Show("Improper Lat Value");
                GoodLOC = false;
            }

            try
            {
                Long = double.Parse(longTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Improper Long Value");
                GoodLOC = false;
            }

            Location Loc = new Location();

            Loc.Latitude = Lat;
            Loc.Longitude = Long;

            if (GoodLOC)
            {
                myMap.Center = Loc;
                myMap.ZoomLevel = 14;
                Pushpin p = new Pushpin();
                p.Location = myMap.Center;
                myMap.Children.Add(p);
            }
        }

        private void AddressButton_Click(object sender, RoutedEventArgs e)
        {

        //    MessageBox.Show("To do:  make this work");
            
            XmlDocument xmlDoc = new XmlDocument();

            string addressQuery = AddresTextBox.Text;
            string BingMapsKey = "AmdIDw9xv6RGyi7933XvJmGss4vILOyzK2q52JTmOcdgnyDZ1g8KLB5QCDSMZsW-";
            string geocodeRequest = "http://dev.virtualearth.net/REST/v1/Locations/" + addressQuery + "?o=xml&key=" + BingMapsKey;

            HttpWebRequest request = WebRequest.Create(geocodeRequest) as HttpWebRequest;
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                xmlDoc.Load(response.GetResponseStream());

            }

            //Create namespace manager
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("rest", "http://schemas.microsoft.com/search/local/ws/rest/v1");

            //Get all geocode locations in the response 
            XmlNodeList locationElements = xmlDoc.SelectNodes("//rest:Location", nsmgr);


            XmlNodeList displayGeocodePoints =
                         locationElements[0].SelectNodes(".//rest:GeocodePoint/rest:UsageType[.='Display']/parent::node()", nsmgr);
            string latitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Latitude", nsmgr).InnerText;
            string longitude = displayGeocodePoints[0].SelectSingleNode(".//rest:Longitude", nsmgr).InnerText;

            myMap.Center.Latitude = (Convert.ToDouble(latitude));
            myMap.Center.Longitude = (Convert.ToDouble(longitude));
            myMap.SetView(myMap.Center, 16.0);
            
            Pushpin pushpin = new Pushpin();
            pushpin.Location = myMap.Center;
            pushpin.Content = AddresTextBox.Text;
           
            myMap.Children.Add(pushpin);


        }
                          
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddStrokesMode == "ADD")
            {

                InkLayer.Children.Remove(GISInkCanvas);
                GISInkCanvas = new SurfaceInkCanvas();
                InkLayer.Children.Add(GISInkCanvas);
                GISInkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
               GISInkCanvas.GotTouchCapture+=new EventHandler<TouchEventArgs>(GISInkCanvas_TouchDown);
               
                GISInkCanvas.DefaultDrawingAttributes.Height = 15;
                GISInkCanvas.DefaultDrawingAttributes.Width = 15;
                

            }
        }

        private void GISInkCanvas_TouchDown(object sender, TouchEventArgs e)
        {
           TouchPoint point = e.TouchDevice.GetTouchPoint(this.GISInkCanvas);
           double orient = e.Device.GetOrientation(this);
         if (orient >= 225 && orient <= 315)
           {
               GISInkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
           }
           
               else
           if (orient >= 45 && orient <= 135)
           {
               GISInkCanvas.DefaultDrawingAttributes.Color = Colors.Yellow;
           }
           else
           if (orient >= 135 && orient <= 225)
           {
               GISInkCanvas.DefaultDrawingAttributes.Color = Colors.Green;
           }else
               
               {
                   GISInkCanvas.DefaultDrawingAttributes.Color = Colors.Blue;
               }

           
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            //Center="47.7742,-96.60787"
            Location Home = new Location();
            Home.Latitude = 47.8009009048924;
            Home.Longitude = -96.6082316181025;

            myMap.Center = Home;
            myMap.ZoomLevel = 16;
            myMap.SetView(Home, 16);
        }

        private void BMPCapButton_Click(object sender, RoutedEventArgs e)
        {
            int width = (int)myMap.ActualWidth;
            int height = (int)myMap.ActualHeight;
            
            // turn off the home button and text box.
            btnHome.Visibility = Visibility.Hidden;
            txtDescription.Visibility = Visibility.Hidden;


           
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(myMap);

            // turn the home button and text box back on.
            btnHome.Visibility = Visibility.Visible;
            txtDescription.Visibility = Visibility.Visible;

            PngBitmapEncoder png = new PngBitmapEncoder();

            png.Frames.Add(BitmapFrame.Create(bmp));


            DateTime time = DateTime.Now;
            string timestamp = time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() +
                time.Minute.ToString() + time.Second.ToString();

            if (!Directory.Exists("C:/LocalGISImages"))
            {
                Directory.CreateDirectory("C:/LocalGISImages");

            }
            string Filename = @"C:/LocalGISImages/" + "LocalImage" + timestamp + ".png";
            LocalFileNames.Add(Filename);

            using (Stream stm = File.Create(Filename))
            {
                png.Save(stm);
            }

            LocalFileNames.Add(Filename);
          //  AddLocalListItem(Filename);
            XML_IO.WriteXMLData(@"C:/LocalGISImages/" + "LocalImage" + timestamp, myMap.Center, myMap.ZoomLevel, GMPolygon );


        }

      /*  public void WriteXMLData(string Filename)
        {
            
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode GISDataNode = xmlDoc.CreateElement("GISData");
            xmlDoc.AppendChild(GISDataNode);

            XmlNode ImageNode = xmlDoc.CreateElement("GISImage");
            XmlAttribute ImageFileAttribute = xmlDoc.CreateAttribute("file");
            ImageFileAttribute.Value = Filename + ".png";
            ImageNode.Attributes.Append(ImageFileAttribute);

            XmlNode LocationNode = xmlDoc.CreateElement("MapLoc");
            XmlAttribute MapLatAttribute = xmlDoc.CreateAttribute("lat");
            XmlAttribute MapLongAttribute = xmlDoc.CreateAttribute("long");
            XmlAttribute MapCameraLevel = xmlDoc.CreateAttribute("alt");

            MapLatAttribute.Value = myMap.Center.Latitude.ToString();
            MapLongAttribute.Value = myMap.Center.Longitude.ToString();
            MapCameraLevel.Value = myMap.ZoomLevel.ToString();

            
            XmlNode PolyNode = xmlDoc.CreateElement("Polygons");

            foreach(MapPolygon MapPoly in  GMPolygon.MapPolygons)
            {

            XmlNode PolygonNode = xmlDoc.CreateElement("Polygon");
            XmlAttribute PolyName = xmlDoc.CreateAttribute("name");
            PolyName.Value = MapPoly.Name;
            PolygonNode.Attributes.Append(PolyName);
                
            foreach (Location Point in MapPoly.Locations)
            {
                XmlElement PolyPoint = xmlDoc.CreateElement("Point");
                XmlAttribute PointLat = xmlDoc.CreateAttribute("lat");
                XmlAttribute PointLong = xmlDoc.CreateAttribute("long");
                PointLat.Value = Point.Latitude.ToString();
                PointLong.Value = Point.Longitude.ToString();

                PolyPoint.Attributes.Append(PointLat);
                PolyPoint.Attributes.Append(PointLong);

                PolygonNode.AppendChild(PolyPoint);
            }
            PolyNode.AppendChild(PolygonNode);
            }

            LocationNode.Attributes.Append(MapLatAttribute);
            LocationNode.Attributes.Append(MapLongAttribute);
            LocationNode.Attributes.Append(MapCameraLevel);

            GISDataNode.AppendChild(LocationNode);
            GISDataNode.AppendChild(ImageNode);
            GISDataNode.AppendChild(PolyNode);


            try
            {
                xmlDoc.Save(Filename + ".xml");
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("XMLWriteError", "XML Write Error");
            }
        } */

        private void ShareBMPButton_Click(object sender, RoutedEventArgs e)
        {
            int width = (int)myMap.ActualWidth;
            int height = (int)myMap.ActualHeight;

            // turn off the home button and text box.
            btnHome.Visibility = Visibility.Hidden;
            txtDescription.Visibility = Visibility.Hidden;



            RenderTargetBitmap bmp = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(myMap);

            // turn the home button and text box back on.
            btnHome.Visibility = Visibility.Visible;
            txtDescription.Visibility = Visibility.Visible;

            PngBitmapEncoder png = new PngBitmapEncoder();

            png.Frames.Add(BitmapFrame.Create(bmp));


            DateTime time = DateTime.Now;
            string timestamp = time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() +
                time.Minute.ToString() + time.Second.ToString();

            if (!Directory.Exists("./Images"))
            {
                Directory.CreateDirectory("./Images");

            }
            string Filename = @"./Images/" + "Image" + timestamp + ".png";
            LocalFileNames.Add(Filename);

            using (Stream stm = File.Create(Filename))
            {
                png.Save(stm);
            }

            FileNames.Add(Filename);
          //  AddListItem(Filename);
            XML_IO.WriteXMLData(@"./Images/" + "Image" + timestamp, myMap.Center, myMap.ZoomLevel, GMPolygon);
        }

        public void newScatterViewItem_SizeChanged(object sender, RoutedEventArgs e)
        {
            ScatterViewItem Obj = sender as ScatterViewItem;
            if (Obj.Width <= 120 || Obj.Height <= 120)
            {
                Obj.Visibility = Visibility.Collapsed;
            }
        }

        private void DHPoly_TouchDown(object sender, TouchEventArgs e)
        {
           // MessageBox.Show("Dowell Hall \n University of Minnesota, Crookston");
            
            txtDescription.Text = "Dowell Hall -- Math, Science and Technology";
            
        }

        private void KeihlePoly_TouchDown(object sender, TouchEventArgs e)
        {
            txtDescription.Text = "Keihle Hall -- Auditorium, Help Desk, PR Offices";
        }

        private void SurfaceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GISlayer.Opacity = (e.NewValue/10);
        }
       
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PPModeButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddPushPinMode)
            {
                AddPushPinMode = false;
                PPModeButton.Content = "Pins";
            }
            else
            {
                AddPushPinMode = true;
                PPModeButton.Content = "Stop";
                

                if (AddStrokesMode == "ADD")
                {
                    AddStrokesMode = "NONE";
                }
            }
        }

        private void myMap_GotTouchCapture(object sender, TouchEventArgs e)
        {
            TouchPoint point = e.TouchDevice.GetTouchPoint(this.GISInkCanvas);
            double orient = e.Device.GetOrientation(this.GISInkCanvas);

        }

        //public void Point_TouchMove(object sender, TouchEventArgs e)
        //{
        //    Rectangle TouchPoint = sender as Rectangle;


        //        GMPolygon.GMPoint.SelectedPoint.pointrect = new Rectangle();
        //        GMPolygon.GMPoint.SelectedPoint.pointrect = TouchPoint;
        //        TouchPoint = null;
                
            
        //}

        private void AddPoint(object sender, TouchEventArgs e)
        {

            
            Location PointLocation = new Location();
            TouchPoint TouchP = e.GetTouchPoint(myMap);
            Point TPosition = TouchP.Position;
            Location PushPinLocation = myMap.ViewportPointToLocation(TPosition);
            GMPolygon.GMPoint.AddPoint(PushPinLocation, this);

            //if (!GMPolygon.GMPoint.AddPolyPoint)
            //{

            //    GMPolygon.GMPoint.SelectedPoint.pointrect.TouchMove += new EventHandler<TouchEventArgs>(Point_TouchMove);
            //}

        }

        private void PolyButton_TouchDown(object sender, TouchEventArgs e)
        {

        }

        private void PolyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!AddPushPinMode)
            {
                if (AddStrokesMode == "NONE")
                {
                    AddStrokesMode = "ADD";
                    myMap.Children.Add(InkLayer);
                    GISInkCanvas.UsesTouchShape = false;
                    GISInkCanvas.DefaultDrawingAttributes.Color = Colors.Red;
                    GISInkCanvas.DefaultDrawingAttributes.Height = 15;
                    GISInkCanvas.DefaultDrawingAttributes.Width = 15;
                }
                else
                    if (AddStrokesMode == "ADD")
                    {
                        myMap.Focus();
                        //    PolyButton.Content = "Draw";
                        myMap.Children.Remove(InkLayer);
                        //    CancelButton.Content = " ";

                        AddStrokesMode = "NONE";

                    }
            }
        }

       private void PolyPointButton_Click(object sender, RoutedEventArgs e)
        {
            // Change the labels
            if (!GMPolygon.GMPoint.AddPolyPoint)
            {
                if (!myMap.Children.Contains(PolyPointLayer))
                {
                    myMap.Children.Add(PolyPointLayer);
                }                               
                PolyPointButton.Content = "Add";
                PolyCreateButton.Content = "Create";                
                GMPolygon.GMPoint.AddPolyPoint = true;                               
            } 
        }

       private void PolyCreateButton_Click(object sender, RoutedEventArgs e)
        {
            GMPolygon.AddPolygon(this);
            GMPolygon.GMPoint.AddPolyPoint = false;
            PolyPointButton.Content = "Polygon";
            PolyCreateButton.Content = "";
        } 

        private void LibPoly_TouchDown(object sender, TouchEventArgs e)
        {
            txtDescription.Text = "Library";
        }

        private void SahlPoly_TouchDown(object sender, TouchEventArgs e)
        {
            txtDescription.Text = "Sahlstrom  Conference Center";
        }

     /*   private void LoadShapeButton_Click(object sender, RoutedEventArgs e)
        {
           // ShapeLayer.Children.Clear();
            

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select an ESRI Shapefile";
            dialog.Filter = "ESRI Shapefile (*.shp) |*.shp;";

            bool? valid = dialog.ShowDialog();

            if (valid.HasValue && valid.Value)
            {
                using (Shapefile shapefile = new Shapefile(dialog.FileName))
                {
                    //Set the map view for the data set

                    myMap.SetView(RectangleDToLocationRect(shapefile.BoundingBox));

                    foreach (Catfood.Shapefile.Shape s in shapefile)
                    {
                        RenderShapeOnLayer(s, GISlayer);
                    }
                }
            }
        } */

        private void ClearMap_Clicked(object sender, RoutedEventArgs e)
        {
            GISlayer.Children.Clear();
           // GISLayer.Children.Clear();
        }
    
        /*private LocationRect RectangleDToLocationRect(RectangleD bBox)
        {
            return new LocationRect(bBox.Top, bBox.Left, bBox.Bottom, bBox.Right);
        }*/

        /*
        private void RenderShapeOnLayer(Catfood.Shapefile.Shape shape, MapLayer layer)
        {
            switch (shape.Type)
            {
                case ShapeType.Point:
                    ShapePoint point = shape as ShapePoint;
                    layer.Children.Add(new Pushpin()
                    {
                        Location = new Location(point.Point.Y, point.Point.X)
                    });
                    break;
                case ShapeType.PolyLine:
                    ShapePolyLine polyline = shape as ShapePolyLine;
                    for (int i = 0; i < polyline.Parts.Count; i++)
                    {
                        layer.Children.Add(new MapPolyline()
                        {
                            Locations = PointDArrayToLocationCollection(polyline.Parts[i]),
                            Stroke = new SolidColorBrush(Color.FromArgb(150, 255, 0, 0))
                        });
                    }
                    break;
                case ShapeType.Polygon:
                    ShapePolygon polygon = shape as ShapePolygon;
                    if (polygon.Parts.Count > 0)
                    {
                        //Only render the exterior ring of polygons for now.
                        for (int i = 0; i < polygon.Parts.Count; i++)
                        {
                            //Note that the exterior rings in a ShapePolygon have a Clockwise order
                            if (!IsCCW(polygon.Parts[i]))
                            {
                                layer.Children.Add(new MapPolygon()
                                {
                                    Locations = PointDArrayToLocationCollection(polygon.Parts[i]),
                                    Fill = new SolidColorBrush(Color.FromArgb(150, 0, 0, 255)),
                                    Stroke = new SolidColorBrush(Color.FromArgb(150, 255, 0, 0))
                                });
                            }
                        }
                    }
                    break;
                case ShapeType.MultiPoint:
                    ShapeMultiPoint multiPoint = shape as ShapeMultiPoint;
                    for (int i = 0; i < multiPoint.Points.Length; i++)
                    {
                        layer.Children.Add(new Pushpin()
                        {
                            Location = new Location(multiPoint.Points[i].Y, multiPoint.Points[i].X)
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        private LocationCollection PointDArrayToLocationCollection(PointD[] points)
        {
            LocationCollection locations = new LocationCollection();
            int numPoints = points.Length;
            for (int i = 0; i < numPoints; i++)
            {
                locations.Add(new Location(points[i].Y, points[i].X));
            }
            return locations;
        }

        /// <summary>
        /// Determines if the coordinates in an array are in a counter clockwise order.
        /// </summary>
        /// <returns>A boolean indicating if the coordinates are in a counter clockwise order</returns>
        public bool IsCCW(PointD[] points)
        {
            int count = points.Length;

            PointD coordinate = points[0];
            int index1 = 0;

            for (int i = 1; i < count; i++)
            {
                PointD coordinate2 = points[i];
                if (coordinate2.Y > coordinate.Y)
                {
                    coordinate = coordinate2;
                    index1 = i;
                }
            }

            int num4 = index1 - 1;

            if (num4 < 0)
            {
                num4 = count - 2;
            }

            int num5 = index1 + 1;

            if (num5 >= count)
            {
                num5 = 1;
            }

            PointD coordinate3 = points[num4];
            PointD coordinate4 = points[num5];

            double num6 = ((coordinate4.X - coordinate.X) * (coordinate3.Y - coordinate.Y)) -
                ((coordinate4.Y - coordinate.Y) * (coordinate3.X - coordinate.X));

            if (num6 == 0.0)
            {
                return (coordinate3.X > coordinate4.X);
            }

            return (num6 > 0.0);
        }
        */
        private void LocalBMPList_Initialized(object sender, EventArgs e)
        {
           // LocalARList = new ArrayList();
            LocalFileNames = new ArrayList();
            // double width, height;

            string DirectoryName = "C:/LocalGISImages";
            string[] LocalImageNames;
            if (Directory.Exists(DirectoryName))
            {
                LocalImageNames = Directory.GetFiles(DirectoryName);
                for (int x = 0; x < LocalImageNames.Length; x++)
                {
                    if (LocalImageNames[x].EndsWith(".xml"))
                    {
                        LocalFileNames.Add(LocalImageNames[x]);
                        AddLocalListItem(LocalImageNames[x]);
                    }
                }  // for
            } // if 

        }

        private void NetBMPList_Initialized(object sender, EventArgs e)
        {
     
            FileNames = new ArrayList();
            // double width, height;

            string DirectoryName = "./Images";
            string[] ImageNames;
            if (Directory.Exists(DirectoryName))
            {
                ImageNames = Directory.GetFiles(DirectoryName);
                for (int x = 0; x < ImageNames.Length; x++)
                {
                    if (ImageNames[x].EndsWith(".xml"))
                    {
                        FileNames.Add(ImageNames[x]);
                        AddListItem(ImageNames[x]);
                    }
                }  // for
            } // if 
        }

        void AddListItem(string FullPath)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XML_IO.ReadXMLData(xmlDoc, FullPath);

            XmlNode ImageNode = xmlDoc.SelectSingleNode("//GISData/GISImage");
            string ImageFile = ImageNode.Attributes["file"].Value;
            BitmapImage BMPimage = new BitmapImage();
            double width, height;

            try
            {
                BMPimage.BeginInit();
                BMPimage.UriSource = new Uri(ImageFile, UriKind.RelativeOrAbsolute);
                BMPimage.EndInit();
            }
            catch (FileNotFoundException i)
            {

                MessageBoxResult Result = MessageBox.Show("Network Error", "Please Manually Refresh");
            }

            width = BMPimage.Width;
            height = BMPimage.Height;

            SurfaceListBoxItem NewImageItem = new SurfaceListBoxItem();

            int x = FileNames.Count;

            if (x <= 9)
                NewImageItem.Name = "Image0" + x.ToString();
            else
                NewImageItem.Name = "Image" + x.ToString();

           // ARList.Add(width / height);

            NewImageItem.Background = new ImageBrush(BMPimage);
            NewImageItem.Height = 80;
            NewImageItem.Padding = new Thickness(5.0);

            NewImageItem.Selected += new RoutedEventHandler(ImageItem_selected);

            NetBMPList.Items.Add(NewImageItem);
        }

        void AddLocalListItem(string FullPath)
        {

            XmlDocument xmlDoc = new XmlDocument();

            XML_IO.ReadXMLData(xmlDoc, FullPath);

            XmlNode ImageNode = xmlDoc.SelectSingleNode("//GISData/GISImage");
            string ImageFile = ImageNode.Attributes["file"].Value;
            BitmapImage BMPimage = new BitmapImage();
            double width, height;

            try
            {
                BMPimage.BeginInit();
                BMPimage.UriSource = new Uri(ImageFile, UriKind.RelativeOrAbsolute);
                BMPimage.EndInit();
            }
            catch (FileNotFoundException i)
            {

                MessageBoxResult Result = MessageBox.Show("Network Error", "Please Manually Refresh");
            }
            width = BMPimage.Width;
            height = BMPimage.Height;

            SurfaceListBoxItem NewImageItem = new SurfaceListBoxItem();

            int x = LocalFileNames.Count;

            if (x <= 9)
                NewImageItem.Name = "Local0" + x.ToString();
            else
                NewImageItem.Name = "Local" + x.ToString();

           // ARList.Add(width / height);

            NewImageItem.Background = new ImageBrush(BMPimage);
            NewImageItem.Height = 80;
            NewImageItem.Padding = new Thickness(5.0);

            NewImageItem.Selected += new RoutedEventHandler(ImageItem_selected);

            LocalBMPList.Items.Add(NewImageItem);

        }

       /* public XmlDocument ReadXMLData(XmlDocument xmlDoc, string FullPath)
        {
            xmlDoc.Load(FullPath);
            
            return xmlDoc;
        } */
        
        public void ImageItem_selected(object sender, RoutedEventArgs e)
        {

            SurfaceListBoxItem Item = e.Source as SurfaceListBoxItem;
            System.Windows.Point Center = new System.Windows.Point(950.0, 450.0);
            string ScatterName = Item.Name;
            ScatterViewItem Obj = LogicalTreeHelper.FindLogicalNode(scatterView1, ScatterName) as ScatterViewItem;
            
            if (Obj == null)  // if the object does not already exists create it
            {   
                XmlDocument xmlDoc = new XmlDocument();

                int index = Convert.ToInt32(Item.Name.Substring(5, 2));

                if (Item.Name.Substring(0, 5) == "Local")
                {
                    XML_IO.ReadXMLData(xmlDoc, LocalFileNames[index - 1].ToString());
                   
                }
                else
                {
                    XML_IO.ReadXMLData(xmlDoc, FileNames[index - 1].ToString());
                }

                ScatterViewItem newScatterViewItem = new ScatterViewItem();
                newScatterViewItem.Name = Item.Name;
                newScatterViewItem.Background = Item.Background;
                newScatterViewItem.Height = 270;
                newScatterViewItem.Width = 360;

                newScatterViewItem.Center = Center;

                // add a resize event to scale the grid 
                newScatterViewItem.SizeChanged += new SizeChangedEventHandler(newScatterViewItem_SizeChanged);

                //  Grid newGrid = new Grid();
                SURGISControl1 newGrid = new SURGISControl1(this);

               XmlNode GISData = xmlDoc.SelectSingleNode("//GISData/MapLoc");

               newGrid.Longitude = Convert.ToDouble(GISData.Attributes["long"].Value);
               newGrid.Lattitude = Convert.ToDouble(GISData.Attributes["lat"].Value);
               newGrid.CameraLevel = Convert.ToDouble(GISData.Attributes["alt"].Value);

               XmlNodeList Polygons = xmlDoc.SelectNodes("//GISData/Polygons/Polygon");

               foreach (XmlNode Polygon in Polygons)
               {
                   
                   MapPolygon NewGridPoly = new MapPolygon();
                   NewGridPoly.Locations  = new LocationCollection();
                   NewGridPoly.Name = Item.Name + Polygon.Attributes.GetNamedItem("name").Value;
                 
                   XmlNodeList Points = Polygon.ChildNodes;
                  
                   foreach (XmlNode Point in Points)
                   {
                       Location NewGridPolyPoint = new Location();

                       NewGridPolyPoint.Latitude = Convert.ToDouble(Point.Attributes.GetNamedItem("lat").Value);
                       NewGridPolyPoint.Longitude = Convert.ToDouble(Point.Attributes.GetNamedItem("long").Value);
                      
                       NewGridPoly.Locations.Add(NewGridPolyPoint);

                   }
                   
                   newGrid.Polygons.Add(NewGridPoly);
               }

                newGrid.Background = Item.Background;

                newScatterViewItem.Content = newGrid;

                scatterView1.Items.Add(newScatterViewItem);

            }// if obj== null

            else /// object exists
            {
                if (Obj.Visibility == Visibility.Collapsed)
                {
                    Obj.Visibility = Visibility.Visible;
                    Obj.Width = 360;
                    Obj.Height = 270;
                }

            }
            Item.IsSelected = false;

        }

        private void RefreshLists()
        {
            string DirectoryName = "./Images";
            string[] ImageNames;

            if (Directory.Exists(DirectoryName))
            {
                ImageNames = Directory.GetFiles(DirectoryName);
                for (int x = 0; x < ImageNames.Length; x++)
                {
                    if (!FileNames.Contains(ImageNames[x]) && (ImageNames[x].EndsWith(".xml")))
                    {
                        FileNames.Add(ImageNames[x]);
                        AddListItem(ImageNames[x]);
                    }

                }  // for
            } // if 

            string LocalDirectoryName = "C:/LocalGISImages";
            string[] LocalImageNames;

            if (Directory.Exists(LocalDirectoryName))
            {
                LocalImageNames = Directory.GetFiles(LocalDirectoryName);
                for (int x = 0; x < LocalImageNames.Length; x++)
                {
                    if (!LocalFileNames.Contains(LocalImageNames[x]) && (LocalImageNames[x].EndsWith(".xml")))
                    {
                        LocalFileNames.Add(LocalImageNames[x]);
                        AddLocalListItem(LocalImageNames[x]);
                    }

                }  // for
            } // if 

        }
                  
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshLists();
        }
    
        private void MaxButton_Click(object sender, RoutedEventArgs e)
        {
            BingScatter1.Orientation = 0;
            BingScatter1.Height = 1080;
            BingScatter1.Width = 1404;
            Point CenterPoint = new Point ((1920/2),(1080/2));
            BingScatter1.Center = CenterPoint;

        }

        private void ClearPButton_Click(object sender, RoutedEventArgs e)
        {
            // wipe out the polygons on the map
            GMPolygon.ClearMapPolies();
        }

        private void WeatherButton_Click(object sender, RoutedEventArgs e)
        
        {

            if (!WeatherMode)
            {

                MessageBoxResult Result =  MessageBox.Show("Make the Weather Map Work","To Do:");
                GetWeatherData();
                if (!myMap.Children.Contains(tileLayer))
                {
                    myMap.Children.Add(tileLayer);
                }
                tileLayer.Visibility = Visibility.Visible;
                WeatherMode = true;
            }
            else{

                myMap.Children.Remove(tileLayer);
                tileLayer.Visibility = Visibility.Collapsed;
               // tileLayer.dispose();
                WeatherMode = false;
            }

        }

        void GetWeatherData()
        {
            TileSource tSource = new TileSource();

            int MapWidth = (int)myMap.ActualWidth;
            int MapHeight =(int) myMap.ActualHeight;

            string north = myMap.BoundingRectangle.North.ToString().Substring(0,6);
            string east = myMap.BoundingRectangle.East.ToString().Substring(0, 6);
            string south = myMap.BoundingRectangle.South.ToString().Substring(0, 6);
            string west = myMap.BoundingRectangle.West.ToString().Substring(0, 6);
            
            string UriString = "{UriScheme}://nowcoast.noaa.gov/wms/com.esri.wms.Esrimap/obs?service=wms&version=1.1.1&request=GetMap&";
            string format = "format=png&";
          
            string BBox = "BBOX=" + west+ "," + south + "," + east + "," + north;
       
            string SRS = "&SRS=EPSG:4269&";
            string WidthHeight = "width="+MapWidth.ToString()+"&height="+MapHeight.ToString()+"&";
            string Transparent = "transparent=true&";
            string Layers = "Layers=world_countries,RAS_RIDGE_NEXRAD";
           // string Layers = "Layers=world_countries,RAS_GOES,RAS_RIDGE_NEXRAD";

            tSource.UriFormat = UriString + format + BBox + SRS + WidthHeight + Transparent + Layers;
                    
            tileLayer.TileSource = tSource;
                    
            tileLayer.Opacity = 0.7;
        }

        private void TrafficButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BingScatter1_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}