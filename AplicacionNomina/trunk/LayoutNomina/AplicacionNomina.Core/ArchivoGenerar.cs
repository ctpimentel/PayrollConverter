using AplicacionNomina.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                archivo.AsignarDetallebk(drow, moneda);
            }

            archivo.Total = montoTotal;
            archivo.AsignarCabecerabk(totalRegistros, montoTotal, moneda);
            archivo.AsignarPie(totalRegistros, montoTotal);
            return archivo;
        }

        public static ArchivoAGenerar ObtenerArchivoAGenerar(string moneda, DataTable dt)
        {
            var totalRegistros = 0;
            var montoTotal = 0m;
            var archivo = new ArchivoAGenerar();

            var index = 0;

            //var HeaderFirstIndex = int.Parse(ConfigurationManager.AppSettings["HeaderFirstIndex"]);
            //var HeaderSecond = int.Parse(ConfigurationManager.AppSettings["HeaderSecond"]);
            //var HeaderThird = int.Parse(ConfigurationManager.AppSettings["HeaderThird"]);
            //var HeaderFourth = int.Parse(ConfigurationManager.AppSettings["HeaderFourth"]);





            var HeaderFirstIndex = int.Parse(CCallApi.Params.Where(w => w.reference == "HeaderFirstIndex").FirstOrDefault().paramValue);
            var HeaderSecond = int.Parse(CCallApi.Params.Where(w => w.reference == "HeaderSecond").FirstOrDefault().paramValue);
            var HeaderThird = int.Parse(CCallApi.Params.Where(w => w.reference == "HeaderThird").FirstOrDefault().paramValue);
            var HeaderFourth = int.Parse(CCallApi.Params.Where(w => w.reference == "HeaderFourth").FirstOrDefault().paramValue);

            var AccountOfDebit = string.Empty;
            var PaymentExecutionDate = string.Empty;
            var DescriptionDebit = string.Empty;
            var DescriptionCodeTrans = string.Empty;
            foreach (DataRow drow in dt.Rows)
            {
                if (index > 4)
                {
                    totalRegistros++;
                    montoTotal += decimal.Parse(drow[5].ToString());
                    archivo.AsignarDetalle(drow, moneda);
                }
                else
                {
                    if (index == HeaderFirstIndex)
                    {
                        AccountOfDebit = drow[1].ToString();
                    }
                    else if (index == HeaderSecond)
                    {
                        PaymentExecutionDate = drow[1].ToString();
                    }
                    else if (index == HeaderThird)
                    {
                        DescriptionDebit = drow[1].ToString();
                    }
                    else if (index == HeaderFourth)
                    {
                        DescriptionCodeTrans = drow[1].ToString();
                    }
                    index++;
                }

            }

            archivo.Total = montoTotal;
            archivo.AsignarCabecera(totalRegistros, montoTotal, moneda, DescriptionDebit, DescriptionCodeTrans, AccountOfDebit, PaymentExecutionDate);
            //archivo.AsignarPie(totalRegistros, montoTotal);
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
        public void AsignarCabecera(int totalRegistros, decimal montoTotal, string Moneda, string DescriptionDebit, string DescriptionCodeTrans, string AccountOfDebit, string PaymentExecutionDate)
        {
            string codigoTipoRegistro = "C";
            var totalRegistrosString = FormatoTotalRegistros(totalRegistros);
            var montoTotalString = FormatearMontoTotal(montoTotal);
            if (DescriptionDebit.Trim().Length < 30)
            {
                DescriptionDebit = DescriptionDebit.Trim().PadRight(30, ' ');
            }

            var transactionConvert = new CTransactionConvert();
            transactionConvert.Description = DescriptionCodeTrans;
            var transactionConvertResult = transactionConvert.cGeTransaccionByDescription();
            if (transactionConvertResult.IsValid)
            {
                DescriptionCodeTrans = transactionConvert.Code;
                if (DescriptionCodeTrans.Length < 2)
                {
                    DescriptionCodeTrans = DescriptionCodeTrans.Trim().PadRight(2, ' ');
                }
            }
            else
            {
                //llamar log
            }


            if (AccountOfDebit.Length < 12)
            {
                AccountOfDebit = AccountOfDebit.Trim().PadRight(12, ' ');
            }
            if (PaymentExecutionDate.Length < 12)
            {
                PaymentExecutionDate = PaymentExecutionDate.Trim().PadRight(10, ' ');
            }

            if (Moneda.Length < 3)
            {
                Moneda = Moneda.Trim().PadRight(3, ' ');
            }
            if (montoTotalString.Length < 12)
            {
                montoTotalString = montoTotalString.Trim().PadLeft(12, '0');
            }

            if (totalRegistrosString.Length < 9)
            {
                totalRegistrosString = totalRegistrosString.Trim().PadLeft(9, '0');
            }
            var cabecera = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", codigoTipoRegistro, DescriptionDebit, DescriptionCodeTrans, AccountOfDebit, PaymentExecutionDate, Moneda, montoTotalString, totalRegistrosString);
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

            var channelType = drow[2].ToString().Trim();

            var indicatorBank = new CIndicatorBank();
            var accountTypeResult = indicatorBank.cGetIndicatorByDescription(channelType);
            if (!accountTypeResult.IsValid)
            {
                //llamar al log para guardar el error
            }

            string nombreCliente = drow[1].ToString().Trim().PadRight(30, ' ');

            //string identification = drow[6].ToString().Trim().PadRight(15, ' '); ;
            if (string.IsNullOrEmpty(nombreCliente))
            {
                //llamar al log para guardar el error
            }

            string tipoCuenta = drow[4].ToString();

            if (string.IsNullOrEmpty(tipoCuenta))
            {

                //llamar al log para guardar el error
            }
            var accountType = new CAccountType();
            accountType.Description = tipoCuenta;
            var accountTyperesult = accountType.cGetAccTypeByDescription();
            if (!accountTyperesult.IsValid)
            {
                //llamar al log para guardar el error
            }
            string numeroCuenta = drow[0].ToString().Trim().PadRight(20, ' ');


            var DescriptionEntity = drow[3].ToString();

            if (string.IsNullOrEmpty(DescriptionEntity))
            {
                //llamar al log para guardar el error
            }
            channelType = channelType.Replace("-", "");
            var channelTypeEnumValue = enChannelType.AACH.ToString();
            var codigoBanco = string.Empty;


            if (channelType == enChannelType.LLBTR.ToString() || channelType == enChannelType.AACH.ToString())
            {
                var payRollValidate = new CPayRollValidate();
                if (channelType == enChannelType.LLBTR.ToString())
                {
                    var resultlbtr = payRollValidate.cGetLBTRDescription(DescriptionEntity);
                    if (!resultlbtr.IsValid)
                    {
                        //Llamar al log para guardar el error
                    }
                    else
                    {
                        codigoBanco = resultlbtr.Mensaje;
                    }
                }
                else
                {
                    var resultlbtr = payRollValidate.cGetACHDescription(DescriptionEntity);
                    if (!resultlbtr.IsValid)
                    {
                        //Llamar al log para guardar el error
                    }
                    else
                    {
                        codigoBanco = resultlbtr.Mensaje; //debo agregar cero a la izquierda o espacios?
                    }
                }
            }
            if (string.IsNullOrEmpty(codigoBanco) || codigoBanco.Length < 12)
            {
                codigoBanco = codigoBanco.Trim().PadRight(12, ' ');
            }
            var monto = string.Empty;
            if (monto.Length < 12)
            {
                //Validamos que si tiene un . es porque vino decimal de lo contrario agregaremos el .00 tal cual acordamos
                if (drow[5].ToString().Contains("."))
                {

                    monto = drow[5].ToString().Replace(".", "").PadLeft(12, '0');
                }
                else
                {
                    StringBuilder sbAmount = new StringBuilder(drow[5].ToString());
                    sbAmount.Append("00");
                    monto = sbAmount.ToString().PadLeft(12, '0');

                }
            }

            var identification = drow[6].ToString();
            var DescriptionCredit = drow[7].ToString();
            var Email = drow[8].ToString();
            var Phone = drow[9].ToString();
            if (string.IsNullOrEmpty(identification) || identification.Length < 15)
            {
                identification = identification.PadRight(15, ' ');
                //MensajeError = string.Format("La Identificación no puede estar en blanco");
                //return false;
            }
            if (string.IsNullOrEmpty(DescriptionCredit) || DescriptionCredit.Length < 30)
            {
                DescriptionCredit = DescriptionCredit.PadRight(30, ' ');
                //MensajeError = string.Format("La Identificación no puede estar en blanco");
                //return false;
            }

            if (string.IsNullOrEmpty(Email) || Email.Length < 50)
            {
                Email = Email.PadRight(50, ' ');
                //MensajeError = string.Format("La Identificación no puede estar en blanco");
                //return false;
            }
            if (string.IsNullOrEmpty(Phone) || Phone.Length < 15)
            {
                Phone = Phone.PadRight(15, ' ');
                //MensajeError = string.Format("La Identificación no puede estar en blanco");
                //return false;
            }
            //string nombre = drow[1].ToString().Trim().PadRight(30, ' ');
            //string codigoBanco = drow[2].ToString().Trim();
            //var codigoBancoConDigitoDeChequeoLong = long.Parse(codigoBanco);
            //string digitoChequeo = ObtenerDigitoChequeo(codigoBanco);
            ////  string tipoCuenta = ObtenerTipoCuentaDestino(drow[3].ToString().Trim());
            //string monto = FormatearMontoTotal(Decimal.Parse(drow[4].ToString()));
            //string indicadorBanco = ObtenerIndicadorBanco(codigoBanco);
            //codigoBanco = codigoBanco.Substring(0, codigoBanco.Length - 1);
            string detalle = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", codigoTipoRegistro, indicatorBank.IndicatorCode,
                nombreCliente, numeroCuenta, accountType.Code.Trim(), codigoBanco, monto, identification, DescriptionCredit, Email, Phone);

            //string nombreBanco = ObtenerNombreDeLBanco(codigoBancoConDigitoDeChequeoLong);
            var registroIndividual = new SummaryItem();
            registroIndividual.Banco = DescriptionEntity;
            registroIndividual.Empleados = 1;
            registroIndividual.Monto = Decimal.Parse(drow[5].ToString());
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
