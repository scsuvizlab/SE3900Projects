using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using Microsoft.Maps.MapControl.WPF;


namespace SurGIS
{
    class SurXML_IO
    {



        public void WriteXMLData(string Filename, Location Center, double ZoomLevel, GISMapPolygon IO_MapPolygons)
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

            MapLatAttribute.Value = Center.Latitude.ToString();
            MapLongAttribute.Value = Center.Longitude.ToString();
            MapCameraLevel.Value = ZoomLevel.ToString();


            XmlNode PolyNode = xmlDoc.CreateElement("Polygons");

            foreach (MapPolygon MapPoly in IO_MapPolygons.MapPolygons)
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
        }


        public XmlDocument ReadXMLData(XmlDocument xmlDoc, string FullPath)
        {
            xmlDoc.Load(FullPath);

            return xmlDoc;
        }


    }
}
