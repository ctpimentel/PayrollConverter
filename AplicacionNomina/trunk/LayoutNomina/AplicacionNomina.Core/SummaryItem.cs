using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplicacionNomina.Core
{
    public class SummaryItem
    {
        public string Number { get; set; }
        public string Banco { get; set; }
        public int Empleados { get; set; }
        //public string Empleados { get; set; }
        public decimal Monto { get; set; }
        public string MontoString { get { return Monto.ToString("N"); } }
        public string ChannelType { get; set; }

    }
}
