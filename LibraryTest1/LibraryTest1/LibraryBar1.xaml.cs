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

namespace LibraryTest1
{
    /// <summary>
    /// Interaction logic for LibraryBar1.xaml
    /// </summary>
    public partial class LibraryBar1 : UserControl
    {
        SurfaceWindow1 surfacewindow;

        public LibraryBar1(SurfaceWindow1 programwindow)
        {
            surfacewindow = programwindow;
            InitializeComponent();
          
        }
    

    }
}
