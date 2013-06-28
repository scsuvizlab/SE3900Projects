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

        public LibraryStack1(SurfaceWindow1 window)
        {
            InitializeComponent();
            
            surfacewindow = window;
      

        }
     
      


    }
}
