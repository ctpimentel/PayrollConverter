using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
           var BancosPesos = new List<long>(){
            111011108,
            101010708,
            101012308,
            101011105,
            101010106,
            101010300,
            101010601,
            101013705,
            101013404,
            479409009,
            101013006,
            101013909,
            101013608,
            444059002,
            102310342,
            101013802,
            101013501,
            111023280,
            101712284,
            489912007,
            251410122,
            111212143,
            101712255
            };

           var BancosDolares = new List<long>()
            {
                801010707,
801012307,
801011104,
801010105,
801010309,
801010600,
801013704,
801013403,
879409007,
801013908,
801013607,
844059000,
801013801,
801013500,
811023289,
801712283,
889912005,
801013005,
802310341,
851410124,
811212142,
801712254,
811010124,
802324230,
801712144,
801727142,
811421331,
811011107

            };

           string template = "diccionarioBancosDolares.Add({0}, \"\");";
           StringBuilder sb = new StringBuilder();
           foreach (var item in BancosDolares)
           {
               sb.AppendLine(string.Format(template, item));
           }

           var output = sb.ToString();
           var a = 5;

            //var m1 = 5000.1;
            //var m2 = 5000;
            //var m3 = 5000.35;
            //var formato = "F";
            //Console.WriteLine(m1.ToString(formato));
            //Console.WriteLine(m2.ToString(formato));
            //Console.WriteLine(m3.ToString(formato));
            //Console.Read();


            //string rutaInput = @"C:\Users\feg0548\Desktop\Archivos\Proyectos\Layout nomina y proveedores\Pesos 2.xlsx";
            //var dt = ReadExcelFast.ReadExcel(rutaInput, 5);
            //dt.Rows.RemoveAt(0);
            //var moneda = "DOP";
            //if (!ValidarExcel(dt, moneda))
            //{
            //    Console.Read();
            //    return;
            //}

            //var totalRegistros = dt.Rows.Count;
            //var montoTotal = 0m;
            
            //var archivo = new ArchivoAGenerar();
            
            //foreach (DataRow drow in dt.Rows)
            //{
            //    montoTotal += decimal.Parse(drow[4].ToString());
            //    archivo.AsignarDetalle(drow, moneda);
            //}

            //archivo.AsignarCabecera(totalRegistros, montoTotal, moneda);
            //archivo.AsignarPie(totalRegistros, montoTotal);

            //using (StreamWriter sw = new StreamWriter(@"C:\Users\feg0548\Desktop\Archivos\Proyectos\Layout nomina y proveedores\output prueba peso 1.txt"))
            //{
            //    sw.Write(archivo.ToString());
            //    sw.Flush();
            //}

            //Console.WriteLine(archivo.ToString());
            Console.Read();
        }
        static RepositorioBanco repo = new RepositorioBanco();
        private static bool ValidarExcel(DataTable dt, string moneda)
        {  
            foreach (DataRow drow in dt.Rows)
            {
                decimal monto;
                if (!decimal.TryParse(drow[4].ToString(), out monto))
                {
                    // Existe un monto que no es un número válido.
                    Console.WriteLine("El monto {0} no tiene formato correcto", drow[4].ToString());
                    return false;
                }

                long codigoBanco;
                if (!long.TryParse(drow[2].ToString(), out codigoBanco))
                {
                    // Existe un código de banco que no es un número válido.
                    Console.WriteLine("Código de banco {0} tiene formato incorrecto", drow[2].ToString());
                    return false;
                }
                else
                {
                    if (!ValidarCodigoBanco(codigoBanco, moneda))
                    {
                        Console.WriteLine("El código de banco {0} no existe en nuestro sistema", codigoBanco);
                        return false;
                    }                   
                }
            }

            return true;
        }

        private static bool ValidarCodigoBanco(long codigoBanco, string moneda)
        {
            var existe = false;

            if (moneda == "DOP")
            {
                existe = repo.BancosPesos.Contains(codigoBanco);
            }
            else
            {
                existe = repo.BancosDolares.Contains(codigoBanco);
            }

            return existe;
        }
    }

    public static class ReadExcelFast
    {
        public static DataTable ReadExcel(string path, int columnas = 0)
        {
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(path, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            Microsoft.Office.Interop.Excel._Worksheet xlWorksheet = (Microsoft.Office.Interop.Excel._Worksheet)xlWorkbook.Sheets[1];
            Microsoft.Office.Interop.Excel.Range xlRange = xlWorksheet.UsedRange;

            object[,] values = (object[,])xlRange.Value2;

            var dt = new DataTable();

            var totalColumnas = (columnas == 0) ? values.GetLength(1) : columnas;

            for (int i = 0; i < totalColumnas; i++)
            {
                dt.Columns.Add();
            }

            for (int i = 2; i <= values.GetLength(0); i++)
            {
                if (values[i, 1] != null)
                {
                    var row = values.GetRow(i, totalColumnas);
                    
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }
    }

    public static class ArrayExt
    {
        public static object[] GetRow(this object[,] array, int row, int colsMax = 0)
        {
            int cols = (colsMax == 0) ? array.GetUpperBound(1) : colsMax;
            object[] result = new object[cols];

            for (int i = 0; i < cols; i++)
            {
                result[i] = array[row, i + 1];
            }

            return result;
        }

    }

    class ArchivoAGenerar
    {
        public ArchivoAGenerar()
        {
            Detalle = new List<string>();
            _repo = new RepositorioBanco();
        }

        private RepositorioBanco _repo;
        public string Cabecera { get; private set; }
        public string Pie { get; private set; }
        public List<String> Detalle { get; private set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Cabecera);
            foreach (var detalle in Detalle)
            {
                sb.AppendLine(detalle);
            }
            sb.Append(Pie);
            return sb.ToString();
        }

        public void AsignarCabecera(int totalRegistros, decimal montoTotal, string Moneda)
        {
            string codigoTipoRegistro = "C";
            var totalRegistrosString = FormatoTotalRegistros(totalRegistros);
            var montoTotalString = FormatearMontoTotal(montoTotal);
            var cabecera = string.Format("{0}{1}{2}{3}", codigoTipoRegistro, totalRegistrosString, montoTotalString, Moneda);
            Cabecera = cabecera;
        }

        public void AsignarPie(int totalRegistros, decimal montoTotal)
        {
            string codigoTipoRegistro = "T";
            var totalRegistrosString = FormatoTotalRegistros(totalRegistros);
            var montoTotalString = FormatearMontoTotal(montoTotal);
            var pie = string.Format("{0}{1}{2}", codigoTipoRegistro, totalRegistrosString, montoTotalString);
            Pie = pie;
        }

        public void AsignarDetalle(DataRow drow, string moneda)
        {
            string codigoTipoRegistro = "D";
            string numeroCuenta = drow[0].ToString().Trim().PadRight(17, ' ');
            string nombre = drow[1].ToString().Trim().PadRight(30, ' ');
            string codigoBanco = drow[2].ToString().Trim();
            string digitoChequeo = ObtenerDigitoChequeo(codigoBanco);
            string tipoCuenta = ObtenerTipoCuentaDestino(drow[3].ToString().Trim());
            string monto = FormatearMontoTotal(Decimal.Parse(drow[4].ToString()));
            string indicadorBanco = ObtenerIndicadorBanco(codigoBanco);
            codigoBanco = codigoBanco.Substring(0, codigoBanco.Length - 1);
            string detalle = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}", codigoTipoRegistro, indicadorBanco,
                numeroCuenta, nombre, tipoCuenta, codigoBanco, digitoChequeo, monto);

            Detalle.Add(detalle);

        }

        

        private string FormatoTotalRegistros(int totalRegistros)
        {
            return totalRegistros.ToString().PadLeft(9, '0');
        }

        private string FormatearMontoTotal(decimal montoTotal)
        {
            return montoTotal.ToString("F").Replace(".", "").PadLeft(12, '0');
        }


        private string ObtenerDigitoChequeo(string codigoBanco)
        {
            return codigoBanco.Substring(codigoBanco.Length - 1);
        }

        private string ObtenerTipoCuentaDestino(string tipoCuenta)
        {
            if (tipoCuenta == "A")
            {
                // Cuenta de ahorros
                return "S";
            }

            // Cuenta Corriente
            return "C";
        }

        private string ObtenerIndicadorBanco(string codigoBanco)
        {
            if (_repo.CodigosBanesco.Contains(long.Parse(codigoBanco)))
            {
                // C = cuenta de banesco
                return "C";
            }

            // A = ACH
            return "A";
        }
    }

    class RepositorioBanco
    {
        public List<long> CodigosBanesco { get; private set; }
        public List<long> BancosPesos { get; private set; }
        public List<long> BancosDolares { get; private set; }
        public RepositorioBanco()
        {

            CodigosBanesco = new List<long>()
            {
                811023289,
                111023280
            };

            BancosPesos = new List<long>(){
            111011108,
            101010708,
            101012308,
            101011105,
            101010106,
            101010300,
            101010601,
            101013705,
            101013404,
            479409009,
            101013006,
            101013909,
            101013608,
            444059002,
            102310342,
            101013802,
            101013501,
            111023280,
            101712284,
            489912007,
            251410122,
            111212143,
            101712255
            };

            BancosDolares = new List<long>()
            {
                801010707,
801012307,
801011104,
801010105,
801010309,
801010600,
801013704,
801013403,
879409007,
801013908,
801013607,
844059000,
801013801,
801013500,
811023289,
801712283,
889912005,
801013005,
802310341,
851410124,
811212142,
801712254,
811010124,
802324230,
801712144,
801727142,
811421331,
811011107

            };

        }
    }
}
