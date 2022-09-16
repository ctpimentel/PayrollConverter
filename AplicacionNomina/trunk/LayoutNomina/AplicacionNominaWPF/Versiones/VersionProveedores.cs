using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplicacionNominaWPF.Versiones
{
    public class VersionProveedores: IVersion
    {
        public string TitleMainWindow
        {
            get
            {
                return "Aplicación de Pagos de Proveedores";
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
                return "Archivo de proveedores exportado";
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
                return "Cargar archivo proveedores";
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
                return @"images/flagsuplidores.png";
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
