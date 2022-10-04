
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AplicacionNomina
{
    public class ValidarExcel
    {
        RepositorioBanco repo = new RepositorioBanco();
        public string MensajeError { get; private set; }

        public bool Validarbk(DataTable dt, string moneda)
        {

            int limiteCaracteresCuenta = 17;
            int limiteCaracteresNombreCliente = 30;
            decimal montoLimite = 999999999999m;
            foreach (DataRow drow in dt.Rows)
            {
                decimal monto;
                if (!decimal.TryParse(drow[4].ToString(), out monto))
                {
                    // Existe un monto que no es un número válido.
                    MensajeError = string.Format("El monto {0} no tiene formato correcto", drow[4].ToString());
                    return false;
                }

                long codigoBanco;
                if (!long.TryParse(drow[2].ToString(), out codigoBanco))
                {
                    // Existe un código de banco que no es un número válido.
                    MensajeError = string.Format("Código de banco {0} tiene formato incorrecto", drow[2].ToString());
                    return false;
                }
                else
                {
                    if (!ValidarCodigoBanco(codigoBanco, moneda))
                    {
                        MensajeError = string.Format("El código de banco {0} no existe en nuestro sistema", codigoBanco);
                        return false;
                    }
                }

                // valido longitudes máximas de los campos

                string numeroCuenta = drow[0].ToString();

                if (numeroCuenta.Length > limiteCaracteresCuenta)
                {
                    MensajeError = string.Format("El código de cuenta {0} excede el máximo permitido de caracteres", numeroCuenta);
                    return false;
                }

                string nombreCliente = drow[1].ToString();

                if (nombreCliente.Length > limiteCaracteresNombreCliente)
                {
                    MensajeError = string.Format("El nombre {0} excede el máximo permitido de caracteres", nombreCliente);
                    return false;
                }

                string tipoCuenta = drow[3].ToString();

                if (tipoCuenta.Length > 1)
                {
                    MensajeError = string.Format("El tipo cuenta {0} excede el máximo permitido de caracteres", tipoCuenta);
                    return false;
                }

                if (monto > montoLimite)
                {
                    MensajeError = string.Format("El monto {0} excede el monto máximo permitido", monto);
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> Validar(DataTable dt, string moneda)
        {
            //var appConfig = new CCallApi();
            //var Response = await appConfig.GetApiManagerParams();
            //if (string.IsNullOrEmpty(CCallApi.ConnString))
            //{
            //    MensajeError = string.Format("No se pudo encontrar el parametro de conección  de la aplicación para poder procesar este archivo");
            //    return false;
            //}

            //if (CCallApi.Params == null || CCallApi.Params.Count < 1)
            //{
            //    MensajeError = string.Format("No se pudo encontrar los parametros iniciales de la aplicación para poder procesar este archivo");
            //}

            var limiteCaracteresCuenta = int.Parse(ConfigurationManager.AppSettings["limiteCaracteresCuenta"]);
            var limiteCaracteresNombreCliente = int.Parse(ConfigurationManager.AppSettings["limiteCaracteresNombreCliente"]);

            var montoLimite = decimal.Parse(ConfigurationManager.AppSettings["montoLimite"]);

            var HeaderFirstIndex = int.Parse(ConfigurationManager.AppSettings["HeaderFirstIndex"]);
            var HeaderSecond = int.Parse(ConfigurationManager.AppSettings["HeaderSecond"]);
            var HeaderThird = int.Parse(ConfigurationManager.AppSettings["HeaderThird"]);
            var HeaderFourth = int.Parse(ConfigurationManager.AppSettings["HeaderFourth"]);


            //var limiteCaracteresCuenta = int.Parse(CCallApi.Params.Where(w => w.reference == "limiteCaracteresCuenta").FirstOrDefault().paramValue);
            //var limiteCaracteresNombreCliente = int.Parse(CCallApi.Params.Where(w => w.reference == "limiteCaracteresNombreCliente").FirstOrDefault().paramValue);

            //var montoLimite = decimal.Parse(CCallApi.Params.Where(w => w.reference == "montoLimite").FirstOrDefault().paramValue);
            //var HeaderFirstIndex = int.Parse(CCallApi.Params.Where(w => w.reference == "HeaderFirstIndex").FirstOrDefault().paramValue);
            //var HeaderSecond = int.Parse(CCallApi.Params.Where(w => w.reference == "HeaderSecond").FirstOrDefault().paramValue);
            //var HeaderThird = int.Parse(CCallApi.Params.Where(w => w.reference == "HeaderThird").FirstOrDefault().paramValue);
            //var HeaderFourth = int.Parse(CCallApi.Params.Where(w => w.reference == "HeaderFourth").FirstOrDefault().paramValue);


            //var montoLimite = decimal.Parse(limitamount);
            //int limiteCaracteresCuenta = 17;
            //int limiteCaracteresNombreCliente = 30;
            //decimal montoLimite = 999999999999m;
            var index = 0;
            foreach (DataRow drow in dt.Rows)
            {

                if (index > 4)
                {
                    decimal monto;
                    if (!decimal.TryParse(drow[5].ToString(), out monto))
                    {
                        // Existe un monto que no es un número válido.
                        MensajeError = string.Format("El monto {0} no tiene formato correcto", drow[5].ToString());
                        return false;
                    }
                    var channelType = drow[2].ToString().Trim();


                    if (string.IsNullOrEmpty(channelType))
                    {
                        // Existe un monto que no es un número válido.
                        MensajeError = string.Format("Indicador de Banco   {0} no puede estar en blanco", channelType);
                        return false;
                    }
                    //var identification = drow[6].ToString();
                    //if (string.IsNullOrEmpty(identification.Trim()))
                    //{                       
                    //    // Existe un monto que no es un número válido.
                    //    MensajeError = string.Format("Identificación no puede estar en blanco");
                    //    return false;
                    //}

                    var channelTypeEnumValue = enChannelType.AACH.ToString();
                    //if (!Regex.IsMatch(DescriptionEntity.Trim(), @"^[a-zA-Z0-9\s]*$"))
                    //{
                    //    MensajeError = string.Format("Banco Destino no puedee estar en blanco ,favor revisar");
                    //    return false;
                    //}

                    var indicatorBank = new CIndicatorBank();
                    var accountTypeResult = indicatorBank.cExistIndicatorDescription(channelType);
                    if (!accountTypeResult.IsValid)
                    {
                        MensajeError = string.Format("Favor verificar este  Indicador de Banco   {0} tiene formato incorrecto", channelType + " detalle: " + accountTypeResult.Mensaje);
                        return false;
                    }
                    channelType = channelType.Replace("-", "");

                    var DescriptionEntity = drow[3].ToString();

                    if (string.IsNullOrEmpty(DescriptionEntity) && channelType != enChannelType.BBanesco.ToString())
                    {
                        // Existe un monto que no es un número válido.
                        MensajeError = string.Format("Banco Destino no puedee estar en blanco ,favor revisar");
                        return false;
                    }
                    if (channelType == enChannelType.LLBTR.ToString() || channelType == enChannelType.AACH.ToString())
                    {
                        if (channelType == channelTypeEnumValue && moneda != enCurrency.DOP.ToString())
                        {
                            MensajeError = string.Format("No puede seleccionar un tipo de moneda distinto a DOP para el indicador {0}  ", drow[2].ToString().Trim().Replace("-", ""));
                            return false;
                        }

                        var payRollValidate = new CPayRollValidate();
                        if (channelType == enChannelType.LLBTR.ToString())
                        {
                            var resultlbtr = payRollValidate.cGetLBTRDescription(DescriptionEntity);
                            if (!resultlbtr.IsValid)
                            {
                                MensajeError = string.Format("Favor verificar este  Banco Destino   {0} tiene formato incorrecto", DescriptionEntity + " detalle: " + resultlbtr.Mensaje);
                                return false;
                            }
                            else
                            {
                                if (resultlbtr.Mensaje.Length > 12)
                                {
                                    MensajeError = string.Format("El codigo swift asociado a este  Banco Destino   {0} tiene formato incorrecto", DescriptionEntity + " es mayor de 12 posiciones, favor arreglar: " + resultlbtr.Mensaje);
                                    return false;
                                }

                            }
                        }
                        else
                        {
                            var resultlbtr = payRollValidate.cGetACHDescription(DescriptionEntity);
                            if (!resultlbtr.IsValid)
                            {
                                MensajeError = string.Format("Favor verificar este  Banco Destino   {0} tiene formato incorrecto", DescriptionEntity + " detalle: " + resultlbtr.Mensaje);
                                return false;
                            }
                            else
                            {
                                if (resultlbtr.Mensaje.Length > 12)
                                {
                                    MensajeError = string.Format("RUTA Y TRANSITO O DIGITO DE CHEQUEO asociado a este  Banco Destino   {0} tiene formato incorrecto", DescriptionEntity + " es mayor de 12 posiciones , el cual es {1}: " + resultlbtr.Mensaje);
                                    return false;
                                }
                            }
                        }
                    }
                    // valido longitudes máximas de los campos
                    string numeroCuenta = drow[0].ToString();

                    if (string.IsNullOrEmpty(numeroCuenta))
                    {
                        MensajeError = string.Format("El No.de Cuenta Beneficiario  {0} no puede estar en blanco", numeroCuenta);
                        return false;
                    }
                    if (numeroCuenta.Length > limiteCaracteresCuenta)
                    {
                        MensajeError = string.Format("El código de cuenta {0} excede el máximo permitido de caracteres", numeroCuenta);
                        return false;
                    }
                    if (!Regex.IsMatch(numeroCuenta.Trim(), @"^[a-zA-Z0-9]*$"))
                    {
                        MensajeError = string.Format("El No.de Cuenta Beneficiario  {0} no puede tener caracteres especiales", numeroCuenta);
                        return false;
                    }

                    string nombreCliente = drow[1].ToString();


                    if (string.IsNullOrEmpty(nombreCliente))
                    {
                        MensajeError = string.Format("El Nombre del Beneficiario  {0} no puede ser en blanco", nombreCliente);
                        return false;
                    }
                    if (nombreCliente.Length > limiteCaracteresNombreCliente)
                    {
                        MensajeError = string.Format("El Nombre del Beneficiario {0} excede el máximo permitido de caracteres", nombreCliente);
                        return false;
                    }

                    if (!Regex.IsMatch(nombreCliente.Trim(), @"^[a-zA-Z0-9\s]*$"))//FALTA TRABAJAR LO DEL UNDERSCORE QUITARLO
                    {
                        MensajeError = string.Format("El Nombre del Beneficiario {0} no debe tener caracteres expeciales", nombreCliente);
                        return false;


                    }
                    string tipoCuenta = drow[4].ToString();

                    if (string.IsNullOrEmpty(tipoCuenta))
                    {

                        MensajeError = string.Format("Tipo Cuenta Destino no puede estar en blanco");
                        return false;
                    }
                    if (tipoCuenta.Trim() != enAccountType.Ahorro.ToString() && tipoCuenta.Trim() != enAccountType.Corriente.ToString() && tipoCuenta.Trim() != enAccountType.Préstamo.ToString() && tipoCuenta.Trim() != enAccountType.Tarjeta.ToString())
                    {
                        MensajeError = string.Format("Tipo Cuenta Destino debe ser igual a las establecidas en su momento y el digitado fue: {0} ", tipoCuenta);
                        return false;
                    }
                    if (monto < 1)
                    {
                        MensajeError = string.Format("El monto no puede ser menor que 1 ", monto);
                    }
                    if (monto > montoLimite)
                    {
                        MensajeError = string.Format("El monto {0} excede el monto máximo permitido", monto);
                        return false;
                    }

                    if (drow[5].ToString().Length > 12)
                    {
                        MensajeError = string.Format("El monto {0} excede la cantidad de posiciones permitidas ", monto);
                        return false;
                    }
                    var identification = drow[6].ToString();

                    if (string.IsNullOrEmpty(identification))
                    {
                        MensajeError = string.Format("La Identificación no puede estar en blanco");
                        return false;
                    }
                }
                else
                {
                    if (index == HeaderFirstIndex)
                    {
                        var AccountOfDebit = drow[1].ToString();

                        if (string.IsNullOrEmpty(AccountOfDebit))
                        {
                            MensajeError = string.Format("Cuenta a Debitar no puede estar en blanco al igual que todas las informaciones del header");
                            return false;
                        }
                    }
                    else if (index == HeaderSecond)
                    {
                        var PaymentExecutionDate = drow[1].ToString();

                        if (string.IsNullOrEmpty(PaymentExecutionDate))
                        {
                            MensajeError = string.Format("Fecha de Ejecución no puede estar en blanco al igual que todas las informaciones del header");
                            return false;
                        }
                        try
                        {
                            var paymentExecutionday = int.Parse(PaymentExecutionDate.Split('/')[0]);
                            var paymentExecutionMonth = int.Parse(PaymentExecutionDate.Split('/')[1]);
                            var paymentExecutionYear = int.Parse(PaymentExecutionDate.Split('/')[2]);
                            DateTime dateExecutionConvert = new DateTime(paymentExecutionYear, paymentExecutionMonth, paymentExecutionday);
                            if (dateExecutionConvert.Date < DateTime.Now.Date)
                            {
                                MensajeError = string.Format("Fecha de Ejecución: {0} no puedo ser  menor a la actual", PaymentExecutionDate);
                                return false;
                            }
                        }
                        catch (Exception exDate)
                        {
                            var innerException = exDate.InnerException;
                            var innerMessage = string.Empty;
                            if (innerException != null)
                            {
                                innerMessage = innerException.Message;
                            }
                            MensajeError = string.Format("Favor verificar la Fecha de Ejecución ya que al momento de evaluar la misma ha ocurrido un error ");

                            MensajeError = MensajeError + " " + exDate.Message + "===>" + innerMessage;

                            //appConfig = new CCallApi();
                            //appConfig.insertToLogApi(MensajeError);
                            return false;
                        }
                    }

                    else if (index == HeaderThird)
                    {
                        var DescriptionDebit = drow[1].ToString();

                        if (string.IsNullOrEmpty(DescriptionDebit))
                        {
                            MensajeError = string.Format("Descripción del Débito no puede estar en blanco al igual que todas las informaciones del header");
                            return false;
                        }

                        if (!Regex.IsMatch(DescriptionDebit.Trim(), @"^[a-zA-Z0-9\s]*$"))
                        {
                            MensajeError = string.Format("Descripción del Débito no permite caracteres especiales al igual que todas las informaciones del header");
                            return false;
                        }

                    }

                    else if (index == HeaderFourth)
                    {
                        var DescriptionCodeTrans = drow[1].ToString();

                        if (string.IsNullOrEmpty(DescriptionCodeTrans))
                        {
                            MensajeError = string.Format("Descripción Código de Transacción no puede estar en blanco al igual que todas las informaciones del header");
                            return false;
                        }
                        var transactionConvert = new CTransactionConvert();
                        transactionConvert.Description = DescriptionCodeTrans.Trim();
                        var transactionConvertResult = await transactionConvert.cExistTransacctionDescription();
                        if (!transactionConvertResult.IsValid)
                        {
                            MensajeError = string.Format("Descripción Código de Transacción no tiene un codigo asociado en las informaciones del header");
                            return false;
                        }
                    }

                    index++;
                }

            }
            return true;
        }

        private bool ValidarCodigoBanco(long codigoBanco, string moneda)
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
}
