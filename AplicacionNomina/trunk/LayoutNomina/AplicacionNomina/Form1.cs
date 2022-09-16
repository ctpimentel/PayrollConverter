using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AplicacionNomina
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CargarDdls();
        }

        private void CargarDdls()
        {
            
            Dictionary<string, string> valoresDdl = new Dictionary<string, string>();
            valoresDdl.Add("Peso Dominicano", "DOP");
            valoresDdl.Add("Dólares", "USD");
            ddlMoneda.DataSource = new BindingSource(valoresDdl, null);
            ddlMoneda.DisplayMember = "Key";
            ddlMoneda.ValueMember = "Value";
            ddlMoneda.SelectedIndex = 0;
        }

        string rutaInput = "";

        private void btnCargarNomina_Click(object sender, EventArgs e)
        {
            ofdCargarNomina.FileName = string.Empty;
            ofdCargarNomina.Filter = "Excel files (*.xlsx, *.xls)|*.xlsx;*.xls|All files (*.*)|*.*";
            if (ofdCargarNomina.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                rutaInput = ofdCargarNomina.FileName;
                Procesar(rutaInput);
            }
            else
            {
                MessageBox.Show("Seleccione un archivo de Excel", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Procesar(string rutaInput)
        {
            ValidarExcel validar = new ValidarExcel();
            string moneda = ((KeyValuePair<string, string>)ddlMoneda.SelectedItem).Value;
            var dt = ReadExcelFast.ReadExcel(rutaInput);
            dt.Rows.RemoveAt(0);
            if (!validar.Validar(dt, moneda))
            {
                MessageBox.Show(validar.MensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var archivo = ArchivoAGenerar.ObtenerArchivoAGenerar(moneda, dt);

            DialogResult result = saveFileDialog1.ShowDialog();
            var RutaSeleccionada = saveFileDialog1.FileName;

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(RutaSeleccionada))
                {
                    sw.Write(archivo.ToString());
                    sw.Flush();
                }

                MessageBox.Show("Archivo de nómina o proveedores exportado.", "Fin de Proceso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
