using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioTec
{
    public partial class Form1 : Form
    {
        List<Cliente> listaClientes = new List<Cliente>();
        public Form1()
        {
            InitializeComponent();
            listaClientes.Add(new Cliente("Juan Pérez",111111111, "Calle Ficticia 123", 123-456-789));
            listaClientes.Add(new Cliente("Ana Gómez", 222222222,"Avenida Principal 456", 987-654-321));
            listaClientes.Add(new Cliente("Luis Martínez",333333333, "Calle Secundaria 789", 555-555-555));
            listBoxClientes.DataSource = listaClientes;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //cargar info del cliente (datos de identificacion)
        private void listBoxClientes_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(listBoxClientes.SelectedItems != null)
            {
                Cliente clienteSeleccionado = (Cliente)listBoxClientes.SelectedItem;

                textBoxNombre.Text = clienteSeleccionado.nombre;
                textBoxDni.Text = clienteSeleccionado.dni.ToString();
                textBoxTelefono.Text = clienteSeleccionado.telefono.ToString();
                textBoxDireccion.Text = clienteSeleccionado.direccion;

            }


        }
    }
}
