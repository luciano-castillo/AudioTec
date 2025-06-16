using AudioTec.Logica;
using Org.BouncyCastle.Asn1.X500;
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
        public bool EditarOrden(string nombreC, string dniC, string telefonoC, string direccionC, string emailC, string articulo, string marca, string modelo, string observacion)
        {
            bool respuesta = false;

            return respuesta;
        }

        public bool EditarOrden(Cliente cliente, Electrodomestico electro)
        {
            bool respuesta = false;

            if (Cliente.CompararCambios(cliente))
            {
                // aplicar cambios
                Cliente.EditarCliente(cliente); 
                respuesta = true;
            }

            if (Electrodomesticos.Count > 0)
            {
                // Si tiene un electro aplica cambio
                if (Electrodomesticos[0].CompararElectrodomestico(electro))
                {
                    Electrodomesticos[0].EditarElectrodomestico(electro);
                    respuesta = true;
                }
            } 
            else if (!electro.Vacio())
            {
                // si no tiene crea y lo une
                ElectrodomesticoLogica.GuardarElectrodomestico(electro);
                ElectrodomesticoLogica.AumentarNroElectrodomestico();
                bool unir = ElectrodomesticoLogica.RelacionarOrdenElectrodomestico(electro, this);

                respuesta = true;
            }

            return respuesta;
        }

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

        public bool TieneElectrodomestico()
        {
            bool respuesta = false;

            if (Electrodomesticos.Count > 0)
            {
                respuesta = true;
            }

            return respuesta;
        }

        public override string ToString()
        {
            return $"Orden N° {OrdenID} - {Cliente?.Nombre} - {Cliente?.DNI}";
        }

    }
}
