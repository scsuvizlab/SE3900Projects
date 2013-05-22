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
        public GISMainMap()
        {
            InitializeComponent();
        }
    }
}
