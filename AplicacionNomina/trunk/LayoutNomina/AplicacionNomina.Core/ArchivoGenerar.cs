using AplicacionNomina.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AplicacionNomina
{
    public class ArchivoAGenerar
    {
        
        public List<SummaryItem> registrosIndividuales { get; set; }

        public decimal Total { get; set; }
        
        public ArchivoAGenerar()
        {
            Detalle = new List<string>();
            _repo = new RepositorioBanco();
            registrosIndividuales = new List<SummaryItem>();
        }

        public static ArchivoAGenerar ObtenerArchivoAGenerarbk(string moneda, DataTable dt)
        {
            var totalRegistros = dt.Rows.Count;
            var montoTotal = 0m;
            var archivo = new ArchivoAGenerar();

            foreach (DataRow drow in dt.Rows)
            {
                montoTotal += decimal.Parse(drow[4].ToString());
                archivo.AsignarDetalle(drow, moneda);
            }

            archivo.Total = montoTotal;
            archivo.AsignarCabecera(totalRegistros, montoTotal, moneda);
            archivo.AsignarPie(totalRegistros, montoTotal);
            return archivo;
        }

        public static ArchivoAGenerar ObtenerArchivoAGenerar(string moneda, DataTable dt)
        {
            var totalRegistros = dt.Rows.Count;
            var montoTotal = 0m;
            var archivo = new ArchivoAGenerar();

            foreach (DataRow drow in dt.Rows)
            {
                montoTotal += decimal.Parse(drow[4].ToString());
                archivo.AsignarDetalle(drow, moneda);
            }

            archivo.Total = montoTotal;
            archivo.AsignarCabecera(totalRegistros, montoTotal, moneda);
            archivo.AsignarPie(totalRegistros, montoTotal);
            return archivo;
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

        public void AsignarCabecerabk(int totalRegistros, decimal montoTotal, string Moneda)
        {
            string codigoTipoRegistro = "C";
            var totalRegistrosString = FormatoTotalRegistros(totalRegistros);
            var montoTotalString = FormatearMontoTotal(montoTotal);
            var cabecera = string.Format("{0}{1}{2}{3}", codigoTipoRegistro, totalRegistrosString, montoTotalString, Moneda);
            Cabecera = cabecera;
        }
        public void AsignarCabecera(int totalRegistros, decimal montoTotal, string Moneda)
        {
            string codigoTipoRegistro = "C";
            var totalRegistrosString = FormatoTotalRegistros(totalRegistros);
            var montoTotalString = FormatearMontoTotal(montoTotal);
            var cabecera = string.Format("{0}{1}{2}{3}", codigoTipoRegistro, totalRegistrosString, montoTotalString, Moneda);
            Cabecera = cabecera;
        }

        public void AsignarPiebk(int totalRegistros, decimal montoTotal)
        {
            string codigoTipoRegistro = "T";
            var totalRegistrosString = FormatoTotalRegistros(totalRegistros);
            var montoTotalString = FormatearMontoTotal(montoTotal);
            var pie = string.Format("{0}{1}{2}", codigoTipoRegistro, totalRegistrosString, montoTotalString);
            Pie = pie;
        }
        public void AsignarPie(int totalRegistros, decimal montoTotal)
        {
            string codigoTipoRegistro = "T";
            var totalRegistrosString = FormatoTotalRegistros(totalRegistros);
            var montoTotalString = FormatearMontoTotal(montoTotal);
            var pie = string.Format("{0}{1}{2}", codigoTipoRegistro, totalRegistrosString, montoTotalString);
            Pie = pie;
        }

        public void AsignarDetallebk(DataRow drow, string moneda)
        {
            string codigoTipoRegistro = "D";
            string numeroCuenta = drow[0].ToString().Trim().PadRight(17, ' ');
            string nombre = drow[1].ToString().Trim().PadRight(30, ' ');
            string codigoBanco = drow[2].ToString().Trim();
            var codigoBancoConDigitoDeChequeoLong = long.Parse(codigoBanco);
            string digitoChequeo = ObtenerDigitoChequeo(codigoBanco);
            string tipoCuenta = ObtenerTipoCuentaDestino(drow[3].ToString().Trim());
            string monto = FormatearMontoTotal(Decimal.Parse(drow[4].ToString()));
            string indicadorBanco = ObtenerIndicadorBanco(codigoBanco);
            codigoBanco = codigoBanco.Substring(0, codigoBanco.Length - 1);
            string detalle = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}", codigoTipoRegistro, indicadorBanco,
                numeroCuenta, nombre, tipoCuenta, codigoBanco, digitoChequeo, monto);

            string nombreBanco = ObtenerNombreDeLBanco(codigoBancoConDigitoDeChequeoLong);
            var registroIndividual = new SummaryItem();
            registroIndividual.Banco = nombreBanco;
            registroIndividual.Empleados = 1;
            registroIndividual.Monto = Decimal.Parse(drow[4].ToString());
            registrosIndividuales.Add(registroIndividual);

            Detalle.Add(detalle);

        }
        public void AsignarDetalle(DataRow drow, string moneda)
        {
            string codigoTipoRegistro = "D";
            string numeroCuenta = drow[0].ToString().Trim().PadRight(17, ' ');
            string nombre = drow[1].ToString().Trim().PadRight(30, ' ');
            string codigoBanco = drow[2].ToString().Trim();
            var codigoBancoConDigitoDeChequeoLong = long.Parse(codigoBanco);
            string digitoChequeo = ObtenerDigitoChequeo(codigoBanco);
            string tipoCuenta = ObtenerTipoCuentaDestino(drow[3].ToString().Trim());
            string monto = FormatearMontoTotal(Decimal.Parse(drow[4].ToString()));
            string indicadorBanco = ObtenerIndicadorBanco(codigoBanco);
            codigoBanco = codigoBanco.Substring(0, codigoBanco.Length - 1);
            string detalle = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}", codigoTipoRegistro, indicadorBanco,
                numeroCuenta, nombre, tipoCuenta, codigoBanco, digitoChequeo, monto);

            string nombreBanco = ObtenerNombreDeLBanco(codigoBancoConDigitoDeChequeoLong);
            var registroIndividual = new SummaryItem();
            registroIndividual.Banco = nombreBanco;
            registroIndividual.Empleados = 1;
            registroIndividual.Monto = Decimal.Parse(drow[4].ToString());
            registrosIndividuales.Add(registroIndividual);

            Detalle.Add(detalle);

        }


        private string ObtenerNombreDeLBanco(long codigoBancoConDigitoDeChequeo)
        {
            string nombreBanco = "";

            if (_repo.diccionarioBancosDolares.Keys.Contains(codigoBancoConDigitoDeChequeo))
            {
                nombreBanco = _repo.diccionarioBancosDolares[codigoBancoConDigitoDeChequeo];
            }
            else
            {
                nombreBanco = _repo.diccionarioBancosPesos[codigoBancoConDigitoDeChequeo];
            }
            return nombreBanco;
        }

        private string FormatoTotalRegistros(int totalRegistros)
        {
            return totalRegistros.ToString().PadLeft(9, '0');
        }

        private string FormatearMontoTotal(decimal montoTotal)
        {
            return montoTotal.ToString("F").Replace(".", "").PadLeft(12, '0');
        }

        public List<SummaryItem> ObtenerListadoAgregado()
        {
            List<SummaryItem> agregado = new List<SummaryItem>();

            agregado = registrosIndividuales.GroupBy(x => x.Banco)
                .Select(x => new SummaryItem
                {
                    Banco = x.FirstOrDefault().Banco,
                    Empleados = x.Count(),
                    Monto = x.Sum(y => y.Monto),
                    Number = "1"
                }).ToList();

            return agregado;
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
}
