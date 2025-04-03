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
            panelContenedor.Visible = false;
            //panelContenedor.Dock = DockStyle.Fill;
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
                dateTimePicker1.Value = clienteSeleccionado.fechaLlegada;
            }
        }

        private void CargarFormulario(UserControl nuevoFormulario)
        {
            // Limpiar el contenido del panel antes de agregar un nuevo formulario
            panelContenedor.Controls.Clear();

            // Ajustar el nuevo formulario para que llene el panel
            nuevoFormulario.Dock = DockStyle.Fill;

            // Agregar el nuevo formulario al panel
            panelContenedor.Controls.Add(nuevoFormulario);
            nuevoFormulario.BringToFront();
        }

        private void toolStripButtonIdentificacion_Click(object sender, EventArgs e)
        {
            //CargarFormulario(new Form1());
            panelContenedor.Visible = false;
        }

        private void toolStripButtonReparaciones_Click(object sender, EventArgs e)
        {
            panelContenedor.Visible = true;
            panelContenedor.BringToFront();
            panelContenedor.Dock = DockStyle.Fill;
            CargarFormulario(new FormReparaciones());
        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            String nombreBuscar = textBoxBuscarNombre.Text.Trim().ToLower();
            String dniBuscar = textBoxBuscarDni.Text.Trim();

            var clientesFiltrados = listaClientes.Where(cliente => 
            (String.IsNullOrWhiteSpace(nombreBuscar) || cliente.nombre.Trim().ToLower().Contains(nombreBuscar)) &&
             (String.IsNullOrWhiteSpace(dniBuscar) || cliente.dni.ToString().Contains(dniBuscar))).ToList();

            listBoxClientes.DataSource = null;
            listBoxClientes.DataSource = clientesFiltrados;
            listBoxClientes.SelectedIndex = -1; // evita selecion automatica del listboxCliente
            listBoxClientes.Refresh();
            listBoxClientes.Update();
        }

        private void buttonGuardarCliente_Click(object sender, EventArgs e)
        {
            String nombre = textBoxNombre.Text;
            int dni = int.Parse(textBoxDni.Text);
            String direccion = textBoxDireccion.Text;
            int telefono = int.Parse(textBoxTelefono.Text);

            Cliente nuevoCliente = new Cliente(nombre,dni,direccion,telefono);

            nuevoCliente.fechaLlegada = DateTime.Now;
            listaClientes.Add(nuevoCliente);

            listBoxClientes.DataSource = null;
            listBoxClientes.DataSource = listaClientes;
        }

        private void buttonNuevo_Click(object sender, EventArgs e)
        {
            textBoxNombre.Text = null;
            textBoxDni.Text = null;
            textBoxDireccion.Text = null;
            textBoxTelefono.Text = null;
            dateTimePicker1.Value= DateTime.Now;
            textBoxArticulo.Text = null;
            textBoxMarca.Text = null;
            textBoxModelo.Text = null;
            textBoxObservaciones.Text = null;
        }
    }
}
