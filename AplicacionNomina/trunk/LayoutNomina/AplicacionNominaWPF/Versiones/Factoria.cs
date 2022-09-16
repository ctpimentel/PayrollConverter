using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplicacionNominaWPF.Versiones
{
    public class Factoria
    {
        public static IVersion Crear(string version)
        {
            if (version.ToUpper() == "PROVEEDORES")
            {
                return new VersionProveedores();
            }
            else
            {
                return new VersionNomina();
            }
        }
    }
}
