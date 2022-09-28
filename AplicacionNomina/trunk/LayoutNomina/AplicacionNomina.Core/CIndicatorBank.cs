using AplicacionNomina.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public int Id { get; set; }
        public string IndicatorName { get; set; }
        public string IndicatorCode { get; set; }


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
                //var connectionString = "metadata=res://*/PayRollModel.csdl|res://*/PayRollModel.ssdl|res://*/PayRollModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=10.3.10.7\\inst01;initial catalog=BANESCO_DEV;user id=usr_laynomdev;password=6M&j4cxHP@Kn7EF4H2bc;MultipleActiveResultSets=True;App=EntityFramework\"";
                //var connectionString = ConfigurationManager.ConnectionStrings["BANESCO_DEVEntitiesprueba"].ConnectionString;
                
                using (var context = new BANESCO_DEVEntities(CCallApi.ConnString))
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



        /// <summary>
        /// Metodo para validar si existe un codigo  swift asociado a este nombre del banco
        /// </summary>
        /// <param name="Description">Nombre del banco</param>
        /// <returns></returns>
        public stResultReturn cGetIndicatorByDescription(string Description)
        {
            var result = new stResultReturn();

            try
            {
                
                using (var context = new BANESCO_DEVEntities(CCallApi.ConnString))
                {
                    var IndicatorObj = context.IndicatorBanks.Where(a => a.IndicatorName.Trim() == Description.Trim()).FirstOrDefault();
                    if (IndicatorObj != null)
                    {
                        result.IsValid = true;
                        this.IndicatorCode = IndicatorObj.IndicatorCode;
                        this.IndicatorName = IndicatorObj.IndicatorName;
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
