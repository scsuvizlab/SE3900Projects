using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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

namespace LibraryTest1
{
    /// <summary>
    /// Interaction logic for LibraryStack1.xaml
    /// </summary>
    /// 
   

    public partial class LibraryStack1 : UserControl
    {
        
        SurfaceWindow1 surfacewindow;
     //   private RoutedEventHandler LSI_Selected;

        public LibraryStack1(SurfaceWindow1 window)
        {
            InitializeComponent();
            
            surfacewindow = window;

            AddLibraryStackItems();
      

        }

        private void AddLibraryStackItems()
        {

            foreach (string file in Directory.GetFiles(@".\Images", "*.png"))
            {
                Image img = new Image();
                BitmapImage BMP = new BitmapImage();
                BMP.BeginInit();
                BMP.UriSource = new Uri(file, UriKind.RelativeOrAbsolute);
                BMP.EndInit();

                LibraryStackItem LSI = new LibraryStackItem();

              //  LSI.Name = file;

                LSI.Background = new ImageBrush(BMP);
               LSI.TouchUp +=new EventHandler<TouchEventArgs>(LSI_TouchUp);

               LibraryImageStack.Items.Add(LSI);
                
            }
        }


        private void LSI_TouchUp(object sender, TouchEventArgs e)
        {
            if (true)
            {
            }
        }

        private void LibraryImageStack_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

          
        }

    }
}
