using System;
using System.IO;
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

namespace LibraryTest1
{
    /// <summary>
    /// Interaction logic for LibraryContainer1.xaml
    /// </summary>
    /// 
    public partial class LibraryContainer1 : UserControl
    {
        SurfaceWindow1 surfacewindow;
        public LibraryContainer1(SurfaceWindow1 programwindow)
        {
            surfacewindow = programwindow;
            InitializeComponent();

            AddLibraryContainerItems();
        }

        private void AddLibraryContainerItems()
        {
            foreach (string file in Directory.GetFiles(@".\Images", "*.png"))
            {
                Image img = new Image();
                BitmapImage BMP = new BitmapImage();
                BMP.BeginInit();
                BMP.UriSource = new Uri(file, UriKind.RelativeOrAbsolute);
                BMP.EndInit();

                LibraryBarItem LBI = new LibraryBarItem();
                
                LBI.Width = 100;
                LBI.Height = 100;

                LBI.Background = new ImageBrush(BMP);

                
               
              

            }
        }
    }
}
