using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTec.Modelo
{
    public class Orden
    {

        public int OrdenID { get; set; }
        public string ClienteDNI { get; set; }
        public List<Electrodomestico> Electrodomesticos { get; set; }
        public float Presupuesto { get; set; }
        public string Fecha_reparacion { get; set; }
        public string Fecha_retiro { get; set; }

        public string Repuesto { get; set; }



    }
}
