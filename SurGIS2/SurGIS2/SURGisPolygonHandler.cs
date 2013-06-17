using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    class SURGisPolygonHandler
    {
    }
}
