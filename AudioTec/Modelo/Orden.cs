using AudioTec.Logica;
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
        //public string ClienteDNI { get; set; }
        public Cliente Cliente { get; set; }
        public List<Electrodomestico> Electrodomesticos { get; set; }
        public float Presupuesto { get; set; }
        public string Fecha_reparacion { get; set; }
        public string Fecha_retiro { get; set; }

        public string Repuesto { get; set; }


        public Orden()
        {
            Electrodomesticos = new List<Electrodomestico>();
        }

        // Crea la orden y lo guarda en la base de datos
        public bool CrearOrden()
        {
            bool respuesta = true;

            if (OrdenID == 0)
            {
                OrdenLogica.CrearOrden(this);

                // Crea los electrodomesticos
                foreach (var item in Electrodomesticos)
                {
                    if (item.Dueno == null)
                    {
                        item.Dueno = Cliente;
                    }
                    
                    item.Crear();
                }
            }
            else
            {
                respuesta = false;
            }

            return respuesta;
        }

        // Agrega el electrodomestico a la lista
        public bool AgregarElectrodomestico(Electrodomestico obj)
        {
            bool respuesta = true;

            Electrodomesticos.Add(obj);

            return respuesta;
        }

        // Terminar orden
        public bool TerminarOrden()
        {
            bool respuesta = OrdenLogica.TerminarOrden(this);

            return respuesta;
        }

        // Editar orden

        // Traer una orden vacia y cargar los datos desde la BD
        public void TraerOrden(int id)
        {

            Orden datos = OrdenLogica.TraerOrden(id);
            OrdenID = id;

            if (datos != null)
            {
                Cliente = datos.Cliente;
                Presupuesto = datos.Presupuesto;
                Fecha_reparacion = datos.Fecha_reparacion;
                Fecha_retiro = datos.Fecha_retiro;
                Repuesto = datos.Repuesto;

                List<Electrodomestico> ElectroTraido = ElectrodomesticoLogica.TraerElectrodomesticos(this);

                foreach (var item in ElectroTraido)
                {
                    Electrodomesticos.Add(item);
                }

            }          

        }

        public override string ToString()
        {
            return $"Orden N° {OrdenID} - {Cliente?.Nombre} - {Cliente?.DNI}";
        }

    }
}
