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
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    /// 

    
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// 
        #region GLOBALS
        public LibraryStack1 LibStack;
        public LibraryBar1 LibBar;
        public LibraryContainer1 LibCont;

        #endregion

        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            LibStack = new LibraryStack1(this);
            LibBar = new LibraryBar1(this);
            LibCont = new LibraryContainer1(this);
           

         
          //   AddLibraryItems();
             AddLibBar();
             AddLibraryStack();
             AddLibContainer();

        }

        private void AddLibContainer()
        {
            ScatterViewItem NewSVI = new ScatterViewItem();
            NewSVI.Width = 500;
            NewSVI.Height = 300;
            NewSVI.Content = LibCont;
            LibScatterView.Items.Add(NewSVI);
        }

     
       public void AddLibraryStack ()
        {

            ScatterViewItem NewSVI = new ScatterViewItem();
            NewSVI.Width = 300;
            NewSVI.Name = "LibStackSVI";
            NewSVI.Height = 300;
            NewSVI.Background = new SolidColorBrush(Colors.Transparent);
            NewSVI.Content = LibStack;
            LibScatterView.Items.Add(NewSVI);


        }

        void AddLibBar()
        {
            ScatterViewItem libbarscatterview = new ScatterViewItem();
            libbarscatterview.Width = 500;
            libbarscatterview.Height = 250;
            libbarscatterview.Content = LibBar;
            LibScatterView.Items.Add(libbarscatterview);
            //TopImageDockPanel.Children.Add(libbarscatterview);
           

        
        }


        void AddLibraryItems()
     {
         foreach (string file in Directory.GetFiles(@".\Images", "*.png"))
         {
             Image img = new Image();
             BitmapImage BMP= new BitmapImage();
             BMP.BeginInit();
                 BMP.UriSource = new Uri(file, UriKind.RelativeOrAbsolute);
             BMP.EndInit();

             LibraryStackItem LSI = new LibraryStackItem();
             LibraryBarItem LBI = new LibraryBarItem();
             LBI.Width = 100;
             LBI.Height = 100;          
             
             LSI.Background = new ImageBrush(BMP);
             LBI.Background = new ImageBrush(BMP);
           
         

             LibStack.LibraryImageStack.Items.Add(LSI);
             LibBar.ImageLibraryBar.Items.Add(LBI);
           
         }
       
     }

        #region DEFAULT

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
        #endregion
    }
}