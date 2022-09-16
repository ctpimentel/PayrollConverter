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
using System.Windows.Shapes;

namespace AplicacionNominaWPF
{
    /// <summary>
    /// Interaction logic for CustomPopup.xaml
    /// </summary>
    public partial class CustomPopup : Window
    {
        public CustomPopup()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(ObtenerRuta(Icono), UriKind.Relative);
            bi3.EndInit();
            imgIcono.Source = bi3;
            CustomPopupWindow.Title = Header;
            lblMensaje.Text = Message;
        }

        public CustomPopup(Iconos icono, string message, string header)
        {            
            InitializeComponent();
            Icono = icono;
            Message = message;
            Header = header;
        }       

        public Iconos Icono { get; set; }
        public string Message { get; set; }
        public string Header { get; set; }

        private void btnAceptar_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string ObtenerRuta(Iconos icono)
        {
            if (icono == Iconos.Error)
            {
                return @"Images/iconError.png";
            }
            else if (icono == Iconos.Exclamacion)
            {
                return @"Images/IconExclamation.png";
            }

            throw new ApplicationException("Icono no reconocido");
        }
    }
}
