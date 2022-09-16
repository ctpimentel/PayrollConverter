using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Windows;

namespace AplicacionNominaWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
           
        }

        public App()
        {

        }

        public bool DoHandle { get; set; }
        private void Application_DispatcherUnhandledException(object sender,
                               System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"ErrorLog.txt", true))
            {
                string output = string.Format("{2} - Exception: {0}; StackStrace: {1}", e.Exception.Message, e.Exception.StackTrace, DateTime.Now);
                file.WriteLine(output);
                output = string.Format("{2} - Base Exception Exception: {0}; StackStrace: {1}", e.Exception.GetBaseException().Message, e.Exception.GetBaseException().StackTrace, DateTime.Now);
                file.WriteLine(output);
            }

            //Handling the exception within the UnhandledException handler.
            MessageBox.Show(e.Exception.Message + e.Exception.StackTrace, "Exception Caught",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = false;
        }

    }
}
