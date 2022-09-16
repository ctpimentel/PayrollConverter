using AplicacionNomina.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplicacionNomina
{
    public enum enAccountType
    {
        Ahorro,
        Corriente,
        Tarjeta,
        Préstamo

    }
    public class CIndicatorBank
    {

        /// <summary>
        /// Metodo para validar si existe un codigo  swift asociado a este nombre del banco
        /// </summary>
        /// <param name="Description">Nombre del banco</param>
        /// <returns></returns>
        public stResultReturn cExistIndicatorDescription(string Description)
        {
            var result = new stResultReturn();

            try
            {
                using (var context = new BANESCO_DEVEntities())
                {
                    var IndicatorExist = context.IndicatorBanks.Any(a => a.IndicatorName.Trim() == Description.Trim());
                    if (IndicatorExist)
                    {
                        result.IsValid = true;

                    }
                    else
                    {
                        result.IsValid = false;
                        result.Mensaje = "No existe un Indicador de Banco con esta descripcion : " + Description;
                    }

                }
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException;
                var innerMessage = string.Empty;
                if (innerException != null)
                {
                    innerMessage = innerException.Message;
                }
                innerMessage = " Ha ocurrido un error con este banco destino " + Description + " " + ex.Message;
                result.IsValid = false;
                result.Mensaje = innerMessage;

            }
            return result;
        }
    }
}
