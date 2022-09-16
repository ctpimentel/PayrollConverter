using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AplicacionNomina
{
    public class stResultReturnList<T>
    {
        public List<T> ObjList { get; set; }
        public stResultReturn Result { get; set; }
    }
    public class stResultReturnRet<T>
    {
        public T Obj { get; set; }
        public stResultReturn Result { get; set; }
    }
    public struct stResultReturn
    {
        public bool IsValid { get; set; }
        public string Mensaje { get; set; }
    }
}
