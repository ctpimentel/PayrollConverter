
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

                if (string.IsNullOrEmpty(drow[4].ToString()))
                {
                    // Existe un monto que no es un número válido.
                    MensajeError = string.Format("El monto  no puede estar en blanco ");
                    return false;
                }
                if (!decimal.TryParse(drow[4].ToString(), out monto))
                {
                    // Existe un monto que no es un número válido.
                    MensajeError = string.Format("El monto {0} no tiene formato correcto", drow[4].ToString());
                    return false;
                }
                if (monto <= 0)
                {
                    // Existe un monto que no es un número válido.
                    MensajeError = string.Format("El monto debe ser mayor que cero");
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

                    var montoAmountstring = drow[5].ToString();
                    if (string.IsNullOrEmpty(montoAmountstring)) 
                    {
                        MensajeError = string.Format("El campo monto no puede estar en blanco ");
                        return false;
                    }
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
                        MensajeError = string.Format("Indicador de banco no puede estar en blanco ");
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
                        MensajeError = string.Format("Favor verificar este indicador de banco {0} tiene formato incorrecto", channelType + " detalle: " + accountTypeResult.Mensaje);
                        return false;
                    }
                    channelType = channelType.Replace("-", "");

                    var DescriptionEntity = drow[3].ToString();

                    if (string.IsNullOrEmpty(DescriptionEntity) && channelType != enChannelType.BBanesco.ToString())
                    {
                        // Existe un monto que no es un número válido.
                        MensajeError = string.Format("Banco destino no puedee estar en blanco ,favor revisar");
                        return false;
                    }
                    if (channelType == enChannelType.LLBTR.ToString() || channelType == enChannelType.AACH.ToString())
                    {
                        if (channelType == channelTypeEnumValue && moneda != enCurrency.DOP.ToString())
                        {
                            MensajeError = string.Format("No puede seleccionar un tipo de moneda distinto a dop para el indicador ACH ");
                            return false;
                        }

                        var payRollValidate = new CPayRollValidate();
                        if (channelType == enChannelType.LLBTR.ToString())
                        {
                            var resultlbtr = payRollValidate.cGetLBTRDescription(DescriptionEntity);
                            if (!resultlbtr.IsValid)
                            {
                                MensajeError = string.Format("Favor verificar este banco destino {0} tiene formato incorrecto", DescriptionEntity + " detalle: " + resultlbtr.Mensaje);
                                return false;
                            }
                            else
                            {
                                if (resultlbtr.Mensaje.Length > 12)
                                {
                                    MensajeError = string.Format("El codigo swift asociado a este banco destino {0} tiene formato incorrecto", DescriptionEntity + " es mayor de 12 posiciones, favor arreglar: " + resultlbtr.Mensaje);
                                    return false;
                                }

                            }
                        }
                        else
                        {
                            var resultlbtr = payRollValidate.cGetACHDescription(DescriptionEntity);
                            if (!resultlbtr.IsValid)
                            {
                                MensajeError = string.Format("Favor verificar este banco destino {0} tiene formato incorrecto", DescriptionEntity + " detalle: " + resultlbtr.Mensaje);
                                return false;
                            }
                            else
                            {
                                if (resultlbtr.Mensaje.Length > 12)
                                {
                                    MensajeError = string.Format("Ruta y tránsito O dígito de chequeo asociado a este  banco destino {0} tiene formato incorrecto", DescriptionEntity + " es mayor de 12 posiciones , el cual es {1}: " + resultlbtr.Mensaje);
                                    return false;
                                }
                            }
                        }
                    }
                    // valido longitudes máximas de los campos
                    string numeroCuenta = drow[0].ToString();

                    if (string.IsNullOrEmpty(numeroCuenta))
                    {
                        MensajeError = string.Format("El no.de cuenta beneficiario no puede estar en blanco");
                        return false;
                    }
                    if (numeroCuenta.Length > limiteCaracteresCuenta)
                    {
                        MensajeError = string.Format("El código de cuenta {0} excede el máximo permitido de caracteres", numeroCuenta);
                        return false;
                    }
                    if (!Regex.IsMatch(numeroCuenta.Trim(), @"^[a-zA-Z0-9]*$"))
                    {
                        MensajeError = string.Format("El no.de cuenta beneficiario {0} no puede tener caracteres especiales", numeroCuenta);
                        return false;
                    }

                    string nombreCliente = drow[1].ToString();


                    if (string.IsNullOrEmpty(nombreCliente))
                    {
                        MensajeError = string.Format("El nombre del beneficiario  no puede estar en blanco ");
                        return false;
                    }
                    if (nombreCliente.Length > limiteCaracteresNombreCliente)
                    {
                        MensajeError = string.Format("El nombre del beneficiario {0} excede el máximo permitido de caracteres", nombreCliente);
                        return false;
                    }

                    if (!Regex.IsMatch(nombreCliente.Trim(), @"^[a-zA-Z0-9\s]*$"))//FALTA TRABAJAR LO DEL UNDERSCORE QUITARLO
                    {
                        MensajeError = string.Format("El nombre del beneficiario {0} no debe tener caracteres expeciales", nombreCliente);
                        return false;

                    }
                    string tipoCuenta = drow[4].ToString();

                    if (string.IsNullOrEmpty(tipoCuenta))
                    {
                        MensajeError = string.Format("Tipo cuenta destino no puede estar en blanco");
                        return false;
                    }
                    if (tipoCuenta.Replace(" ", "").Trim() == enAccountType.Corriente.ToString() && moneda == enCurrency.USD.ToString())
                    {
                        MensajeError = string.Format("No puede haber un tipo cuenta destino corriente  y seleccionar como moneda de conversión dólares");
                        return false;

                    }
                    if (tipoCuenta.Replace(" ", "").Trim() != enAccountType.Ahorro.ToString() && tipoCuenta.Trim() != enAccountType.Corriente.ToString() && tipoCuenta.Trim() != enAccountType.Préstamo.ToString() && tipoCuenta.Replace(" ", "").Trim() != enAccountType.Tarjetadecrédito.ToString())
                    {
                        MensajeError = string.Format("Tipo cuenta destino debe ser igual a las establecidas en su momento y el digitado fue: {0} ", tipoCuenta);
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

                    if (string.IsNullOrEmpty(identification) && channelType != enChannelType.BBanesco.ToString())
                    {
                        MensajeError = string.Format("La identificación no puede estar en blanco");
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
                            MensajeError = string.Format("Cuenta a debitar no puede estar en blanco al igual que todas las informaciones del header");
                            return false;
                        }
                    }
                    else if (index == HeaderSecond)
                    {
                        var PaymentExecutionDate = drow[1].ToString();

                        if (string.IsNullOrEmpty(PaymentExecutionDate))
                        {
                            MensajeError = string.Format("Fecha de ejecución no puede estar en blanco al igual que todas las informaciones del header");
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
                                MensajeError = string.Format("Fecha de ejecución: {0} no puedo ser  menor a la actual", PaymentExecutionDate);
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
                            MensajeError = string.Format("Favor verificar la fecha de ejecución ya que al momento de evaluar la misma ha ocurrido un error ");

                            MensajeError = MensajeError + " " + exDate.Message + "===>" + innerMessage;
                            
                            return false;
                        }
                    }

                    else if (index == HeaderThird)
                    {
                        var DescriptionDebit = drow[1].ToString();

                        if (string.IsNullOrEmpty(DescriptionDebit))
                        {
                            MensajeError = string.Format("Descripción del débito no puede estar en blanco al igual que todas las informaciones del header");
                            return false;
                        }

                        if (!Regex.IsMatch(DescriptionDebit.Trim(), @"^[a-zA-Z0-9\s]*$"))
                        {
                            MensajeError = string.Format("Descripción del débito no permite caracteres especiales al igual que todas las informaciones del header");
                            return false;
                        }

                    }

                    else if (index == HeaderFourth)
                    {
                        var DescriptionCodeTrans = drow[1].ToString();

                        if (string.IsNullOrEmpty(DescriptionCodeTrans))
                        {
                            MensajeError = string.Format("Descripción código de transacción no puede estar en blanco al igual que todas las informaciones del header");
                            return false;
                        }
                        var transactionConvert = new CTransactionConvert();
                        transactionConvert.Description = DescriptionCodeTrans.Trim();
                        var transactionConvertResult = await transactionConvert.cExistTransacctionDescription();
                        if (!transactionConvertResult.IsValid)
                        {
                            MensajeError = string.Format("Descripción código de transacción no tiene un codigo asociado en las informaciones del header");
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
