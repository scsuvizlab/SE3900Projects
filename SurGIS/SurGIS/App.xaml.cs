using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SurGIS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            Exception CurrentException = e.Exception;
            while (CurrentException != null)
            {
                MessageBox.Show(CurrentException.Message);
                CurrentException = CurrentException.InnerException;
            }
        }
    }
}