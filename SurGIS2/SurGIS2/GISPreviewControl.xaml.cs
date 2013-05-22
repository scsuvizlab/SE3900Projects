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


/*
 * ++++++++++++++++++++++++++=========  GIS Preview window 
 * 
 * This is a user control that comes up as a scatterview object when an item is selected from the side slider lists 
 * in the main program interface.   It can be a preview for either a set of our own data, a kml file, or an esri shape file.
 * 
 * 
 * If the item is produced (saved) from SURGis then there will be a preview image of the terrain used as the background
 * of both the slider list item, as well as this preview control.  If there's not an image, then a default background can be used.
 * this would be the case for KML or ESRI shapefile data.
 * 
 * There will be controls on the preview window control to load the data to the main map window, which would set the view of the map 
 * so that the data from the preview window will be visible.  or dismiss the window.
 * 
 * 
 * 
 * 
 * */
namespace SurGIS2
{
    /// <summary>
    /// Interaction logic for GISPreviewControl.xaml
    /// </summary>
    public partial class GISPreviewControl : UserControl
    {
        public GISPreviewControl()
        {
            InitializeComponent();
        }
    }
}
