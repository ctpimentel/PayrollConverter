using AplicacionNomina.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplicacionNomina
{
    public class CAccountType
    {
        public string Code { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Metodo para validar si existe un codigo  swift asociado a este nombre del banco
        /// </summary>
        /// <param name="Description">Nombre del banco</param>
        /// <returns></returns>
        public stResultReturn cGetAccTypeByDescription()
        {
            var result = new stResultReturn();

            try
            {
                using (var context = new BANESCO_DEVEntities(CCallApi.ConnString))
                {
                    var AccountTypeObj = context.AccountTypes.Where(a => a.Description.Trim() == this.Description.Trim()).FirstOrDefault();
                    if (AccountTypeObj != null)
                    {
                        result.IsValid = true;
                        this.Code= AccountTypeObj.Code;
                        this.Description= AccountTypeObj.Description;
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Mensaje = "No existe  el tipo de cuenta : " + Description;
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
                innerMessage = " Ha ocurrido un error en el tipo de cuenta " + Description + " " + ex.Message;
                result.IsValid = false;
                result.Mensaje = innerMessage;

            }
            return result;
        }
    }
}
