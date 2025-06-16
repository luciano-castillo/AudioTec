using AudioTec.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioTec.Modelo
{
    public class Cliente
    {

        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }


        public bool crear()
        {
            bool respuesta = true;

            // Comprueba si no esta vacio
            if (DNI != null &&  Nombre != null)
            {
                if (!ClienteLogica.Existe(DNI))
                {
                    respuesta = ClienteLogica.Guardar(this);
                }
                
            }

            return respuesta;
        }

        // este metodo se usara para traer un cliente desde la base de datos
        public void CargarCliente(string dni)
        {
            Cliente datos = ClienteLogica.TraerCliente(dni);

            if (datos != null)
            {
                DNI = datos.DNI;
                Nombre = datos.Nombre;
                Direccion = datos.Direccion;
                Telefono = datos.Telefono;
                Email = datos.Email;
            }
            
        }

        public bool CompararCambios(Cliente cliente)
        {
            bool respuesta = false;

            if (DNI != cliente.DNI || 
                Nombre != cliente.Nombre || 
                Direccion != cliente.Direccion || 
                Telefono != cliente.Telefono ||
                Email != cliente.Email)
            {
                respuesta = true;
            }

            return respuesta;
        }

        public void EditarCliente(Cliente cliente)
        {
            if (ClienteLogica.Editar(cliente, DNI))
            {
                DNI = cliente.DNI;
                Nombre = cliente.Nombre;
                Direccion = cliente.Direccion;
                Telefono = cliente.Telefono;
                Email = cliente.Email;
            }
            else
            {
                MessageBox.Show("No se pudo editar el cliente");
            }      

        }

    }
}
