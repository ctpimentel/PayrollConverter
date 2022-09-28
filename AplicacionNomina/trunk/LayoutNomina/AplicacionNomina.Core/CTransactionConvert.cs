using AplicacionNomina.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionNomina
{
    public class CTransactionConvert
    {
        public string Code { get; set; }
        public string Description { get; set; }


        /// <summary>
        /// Metodo para traer un  codigo de transaccion dado una descripción
        /// </summary>
        /// <param name="Description">Nombre del banco</param>
        /// <returns></returns>
        public stResultReturn cGeTransaccionByDescription()
        {
            var result = new stResultReturn();

            try
            {
                using (var context = new BANESCO_DEVEntities(CCallApi.ConnString))
                {
                    var TransactionConvertObj = context.TransactionConverts.Where(a => a.Description.Trim() == this.Description.Trim()).FirstOrDefault();
                    if (TransactionConvertObj != null)
                    {
                        result.IsValid = true;
                        this.Code = TransactionConvertObj.Code;
                        this.Description = TransactionConvertObj.Description;
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Mensaje = "No existe  codigo de transaccion asociada a esta descripcion : " + Description;
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
                innerMessage = " Ha ocurrido un error en el codigo de transacción de descripción: " + Description + " " + ex.Message;
                result.IsValid = false;
                result.Mensaje = innerMessage;

            }
            return result;
        }




        /// <summary>
        /// Metodo para validar si existe un codigo  swift asociado a este nombre del banco
        /// </summary>
        /// <param name="Description">Nombre del banco</param>
        /// <returns></returns>
        public async Task<stResultReturn> cExistTransacctionDescription()
        {
            var result = new stResultReturn();

            try
            {
                var appConfig = new CCallApi();


                var Response = await appConfig.GetApiManagerParams();


                //"metadata=res://*/PayRollModel.csdl|res://*/PayRollModel.ssdl|res://*/PayRollModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=10.3.10.7\\inst01;initial catalog=BANESCO_DEV;user id=usr_laynomdev;password=6M&amp;j4cxHP@Kn7EF4H2bc;MultipleActiveResultSets=True;App=EntityFramework&quot;" api
                //var connectionString = "metadata=res://*/PayRollModel.csdl|res://*/PayRollModel.ssdl|res://*/PayRollModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=10.3.10.7\\inst01;initial catalog=BANESCO_DEV;user id=usr_laynomdev;password=6M&j4cxHP@Kn7EF4H2bc;MultipleActiveResultSets=True;App=EntityFramework\""; //tomado desde el webconfig y funcionó
                //var connectionString = "metadata=res://*/PayRollModel.csdl|res://*/PayRollModel.ssdl|res://*/PayRollModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=10.3.10.7\\inst01;initial catalog=BANESCO_DEV;user id=usr_laynomdev;password=6M&j4cxHP@Kn7EF4H2bc;MultipleActiveResultSets=True;App=EntityFramework\"";

                
                using (var context = new BANESCO_DEVEntities(CCallApi.ConnString))
                {
                    // 
                    
                    var IndicatorExist = context.TransactionConverts.Any(a => a.Description.Trim() == Description.Trim());
                    if (IndicatorExist)
                    {
                        result.IsValid = true;
                    }
                    else
                    {
                        result.IsValid = false;
                        result.Mensaje = "No existe un código de transacción asociado a esta descripción  : " + Description;
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
