using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_AppBaterias.Model
{
    public class Modelo
    {
    }

    public class Distribuidor
    {
       public string codigo_distribuidor { get; set; }
       public string mode { get; set; }
    }

    public class Inventario
    {
        public string codigoDistribuidor { get; set; }
        public string serial { get; set; }
        public Int32 id_inv { get; set; }
        public string codEmpleado { get; set; }
        public string locEmpleado { get; set; }
        public Double latitude { get; set; }
        public Double longitude { get; set; }
        public string mode { get; set; }

    }


    public class Reporte
    {
       public string  localBateria { get; set; }
       public string codigoDistribuidor { get; set; }
       public string cedula { get; set; }
       public string nombre { get; set; }
       public string serial { get; set; }
       public string articulo { get; set; }
       public string factura_bateria { get; set; }
       public string  numCla { get; set; }
       public string  comentario_observacion { get; set; }
       public string  maxNum { get; set; }
       public string num_alm { get; set; }
       public string mode { get; set; }
    }

}
