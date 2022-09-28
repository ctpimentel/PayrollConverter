using AplicacionNomina;
using AplicacionNomina.Core;
using AplicacionNominaWPF.Versiones;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AplicacionNominaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _repoVersiones = Factoria.Crear(ConfigurationManager.AppSettings["NominaOProveedores"]);
            bwProcesar = new BackgroundWorker();
            bwProcesar.DoWork += ProcesarAsync;
            bwProcesar.RunWorkerCompleted += ProcesarCompleted;
            Title = _repoVersiones.TitleMainWindow;
            btnCargarNominaText.Text = _repoVersiones.TextoBotonCargarArchivo;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(_repoVersiones.rutaFlag, UriKind.Relative);
            bi3.EndInit();
            Flag.Source = bi3;
        }

        private IVersion _repoVersiones;
        BackgroundWorker bwProcesar;
        
        private string _mensajePostValidacion = "Favor revise los detalles de su archivo antes de exportar";
        private string _headerExito = "Fin de Proceso";
        private string _mensajeSeleccionarExcel = "Seleccione un archivo de Excel";
        private string _headerError = "Error";
        string rutaInput = "";
        string _moneda = "";
        string _mensajeError = "";
        DataTable _dt;
        ResultadoProcesarAsync _resultado;
        ArchivoAGenerar _archivoAGenerar;

        void Limpiar()
        {
            ListSummary.Items.Clear();
            btnExportar.IsEnabled = false;
            btnCargarNomina.IsEnabled = true;
        }

        private void ProcesarCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            imgLoading.Visibility = System.Windows.Visibility.Hidden;
            CustomPopup w = new CustomPopup();
            if (_resultado == ResultadoProcesarAsync.Exitoso)
            {
                _archivoAGenerar = ArchivoAGenerar.ObtenerArchivoAGenerar(_moneda, _dt);
                EscribirSummaryEnGrid(_archivoAGenerar.ObtenerListadoAgregado());                
                btnExportar.IsEnabled = true;
                w.Message = _mensajePostValidacion;
                w.Header = _headerExito;
                w.Icono = Iconos.Exclamacion;
                w.ShowDialog();                
            }
            else if (_resultado == ResultadoProcesarAsync.Invalido)
            {                
                w.Message = _mensajeError;
                w.Header = _headerError;
                w.Icono = Iconos.Error;
                w.ShowDialog();
            }

            _dt = null;
        }

        private void EscribirSummaryEnGrid(List<SummaryItem> listado)
        {
            for (int i = 0; i < listado.Count; i++)
            {
                listado[i].Number = (i + 1).ToString();
                ListSummary.Items.Add(listado[i]);
            }

            var summaryTotal = new SummaryItem()
            {
                Number = "Total",
                Banco = "",
                Monto = listado.Sum(x => x.Monto),
                Empleados = listado.Sum(x => x.Empleados)
            };
            ListSummary.Items.Add(summaryTotal);
        }

        private void ProcesarAsync(object sender, DoWorkEventArgs e)
        {           
            ValidarExcel validar = new ValidarExcel();

            _dt = ReadExcelFast.ReadExcel(rutaInput);
            _dt.Rows.RemoveAt(0);
            if (!validar.Validarbk(_dt, _moneda))
            {
                _mensajeError = validar.MensajeError;
                _resultado = ResultadoProcesarAsync.Invalido;
                return;
            }

            _resultado = ResultadoProcesarAsync.Exitoso;
            
        }
               
        private void btnCargarNomina_Click_1(object sender, RoutedEventArgs e)
        {
            Limpiar();
            OpenFileDialog ofdCargarNomina = new OpenFileDialog();
            
            ofdCargarNomina.FileName = string.Empty;
            ofdCargarNomina.Filter = "Excel files (*.xlsx, *.xls)|*.xlsx;*.xls|All files (*.*)|*.*";

            var resultTentativo = ofdCargarNomina.ShowDialog();

            imgLoading.Visibility = System.Windows.Visibility.Visible;

            var result = false;
            if (resultTentativo.HasValue)
            {
                result = resultTentativo.Value;
            }           

            if (result)
            {
                rutaInput = ofdCargarNomina.FileName;
                _moneda =((ComboBoxItem)ddlMoneda.SelectedValue).Tag.ToString();
                bwProcesar.RunWorkerAsync();
            }
            else
            {
                imgLoading.Visibility = System.Windows.Visibility.Hidden;
                CustomPopup w = new CustomPopup();
                w.Message = _mensajeSeleccionarExcel;
                w.Header = _headerError;
                w.Icono = Iconos.Error;
                w.ShowDialog();
            }

        }

        private enum ResultadoProcesarAsync
        {
            Exitoso,
            Invalido
        }

        private void btnExportar_Click_1(object sender, RoutedEventArgs e)
        {
            CustomPopup w = new CustomPopup();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Archivo Texto|*.txt";
            saveFileDialog1.FileName = string.Format("Nomina Banesco {0}.txt", DateTime.Today.ToString("dd-MM-yyyy"));
            var resultTentativo = saveFileDialog1.ShowDialog();

            var result = false;
            if (resultTentativo.HasValue)
            {
                result = resultTentativo.Value;
            }

            var RutaSeleccionada = saveFileDialog1.FileName;

            if (result)
            {
                using (StreamWriter sw = new StreamWriter(RutaSeleccionada))
                {
                    sw.Write(_archivoAGenerar.ToString());
                    sw.Flush();
                }

                w.Message = _repoVersiones.MensajeExito;
                w.Header = _headerExito;
                w.Icono = Iconos.Exclamacion;
                w.ShowDialog();
                Limpiar();
            }
        }
    }

    
}
