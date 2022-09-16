using AplicacionNomina.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplicacionNomina
{
    public class CPayRollValidate
    {
        /// <summary>
        /// Metodo para validar si existe un codigo  swift asociado a este nombre del banco
        /// </summary>
        /// <param name="Description">Nombre del banco</param>
        /// <returns></returns>
        public stResultReturn cExistLBTRDescription(string Description)
        {
            var result = new stResultReturn();

            try
            {
                using (var context = new BANESCO_DEVEntities())
                {
                    var LBTRExist = context.RedLBTRs.Any(a => a.BankName.Trim() == Description.Trim());
                    if (LBTRExist)
                    {
                        result.IsValid = true;
                        
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Mensaje = "No existe un codigo swift asociado a este LBTR: " + Description;
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
        /// <summary>
        /// Metodo para validar si existe unA Ruta y tránsito O digito de chequeo asociado a estA entidad
        /// </summary>
        /// <param name="Description">Nombre de la entidad</param>
        /// <returns></returns>
        public stResultReturn cExistACHDescription(string Description)
        {
            var result = new stResultReturn();

            try
            {
                using (var context = new BANESCO_DEVEntities())
                {
                    var LBTRExist = context.RedACHes.Any(a => a.Entidad.Trim() == Description.Trim());
                    if (LBTRExist)
                    {
                        result.IsValid = true;                        
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Mensaje = "No existe RUTA Y TRANSITO O DIGITO DE CHEQUEO asociado a esta entidad : " + Description;
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
                innerMessage = " " + ex.Message;
                result.IsValid = false;
                result.Mensaje = innerMessage;
            }
            return result;
        }
    }
}
