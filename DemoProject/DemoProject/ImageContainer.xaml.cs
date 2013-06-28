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
using Microsoft.Maps.MapControl.WPF;

namespace DemoProject
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ImageContainer : UserControl
    {
        
        public ImageContainer()
        {
            InitializeComponent();
            PopulateImages(); //Test call, will come from surface window eventually
        }

        //Method to load images into surface window at runtime
        public void PopulateImages()
        {
            //Load some images here, something like this:
            //LibraryStackItem LStackItem = new LibraryStackItem();
            //LStackItem.Background = (whatever, image, resource, etc);
            //LStackItem.Touchdown += new TouchDownEvent // add the event handlers
            //LibraryStackItem.Items.Add(LStackItem);
            
        }
    }
}
