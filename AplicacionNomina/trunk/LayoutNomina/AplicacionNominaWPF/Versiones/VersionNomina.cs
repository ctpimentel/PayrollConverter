using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplicacionNominaWPF.Versiones
{
    public class VersionNomina : IVersion
    {
        public string TitleMainWindow
        {
            get
            {
                var version = System.Configuration.ConfigurationManager.AppSettings["versionNumeric"];
                //return "Aplicación de Pagos de Nóminas";
                return "Convertidor de nóminas y proveedores versión: " + version;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string MensajeExito
        {
            get
            {
                return "Archivo de nóminas exportado";
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public string TextoBotonCargarArchivo
        {
            get
            {
                return "Cargar archivo";
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public string rutaFlag
        {
            get
            {
                return "";
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
