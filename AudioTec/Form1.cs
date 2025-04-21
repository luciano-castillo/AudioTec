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
        private string correoGmail;
        private string claveAppGmail;

        public Form1()
        {
            FormInicioSesion loginForm = new FormInicioSesion();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                correoGmail = loginForm.correo;
                claveAppGmail = loginForm.claveApp;
            }
            else
            {
                MessageBox.Show("No se puede iniciar sin ingresar credenciales.");
                Application.Exit();
                return;
            }
            InitializeComponent();
            listaClientes.Add(new Cliente("Juan Pérez",111111111, "Calle Ficticia 123", "123-456-789"));
            listaClientes.Add(new Cliente("Ana Gómez", 222222222,"Avenida Principal 456", "987-654-321"));
            listaClientes.Add(new Cliente("Luis Martínez",333333333, "Calle Secundaria 789", "555-555-555"));
            listBoxClientes.DataSource = listaClientes;
            panelContenedor.Visible = false;
            listaClientes[0].fechaLlegada = new DateTime(2025, 1, 22);
            listaClientes[1].fechaLlegada = new DateTime(2025, 3, 5);
            listaClientes[2].fechaLlegada = new DateTime(2024, 12, 27);
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
            if (listBoxClientes.SelectedItems != null) 
            {
                Cliente clienteSeleccinado = (Cliente)listBoxClientes.SelectedItem;
                FormReparaciones reparaciones = new FormReparaciones(clienteSeleccinado);
                panelContenedor.Visible = true;
                panelContenedor.BringToFront();
                panelContenedor.Dock = DockStyle.Fill;
                CargarFormulario(reparaciones);
            }
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
            listBoxClientes.SelectedIndex = -1; // evita seleccion automatica del listboxCliente
            listBoxClientes.Refresh();
            listBoxClientes.Update();
        }

        private void buttonGuardarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones mínimas
                if (string.IsNullOrWhiteSpace(textBoxNombre.Text) ||
                    string.IsNullOrWhiteSpace(textBoxDni.Text) ||
                    string.IsNullOrWhiteSpace(textBoxDireccion.Text) ||
                    string.IsNullOrWhiteSpace(textBoxTelefono.Text))
                {
                    MessageBox.Show("Por favor, completá todos los campos antes de guardar.");
                    return;
                }

                string nombre = textBoxNombre.Text.Trim(); //el Trim quita espacios en blanco 
                int dni = int.Parse(textBoxDni.Text.Trim());
                string direccion = textBoxDireccion.Text.Trim();
                String telefono = textBoxTelefono.Text.Trim();
                String email = textBoxEmail.Text.Trim();

                String articulo = textBoxArticulo.Text.Trim();
                String marca = textBoxMarca.Text.Trim();
                String modelo = textBoxModelo.Text.Trim();
                DateTime fechaLlegada = dateTimePicker1.Value;

                // Buscar si ya existe un cliente con ese DNI
                Cliente clienteExistente = listaClientes.FirstOrDefault(c => c.dni == dni);

                if (clienteExistente != null)
                {
                    // Sobrescribir datos del cliente existente
                    clienteExistente.nombre = nombre;
                    clienteExistente.direccion = direccion;
                    clienteExistente.telefono = telefono;
                    clienteExistente.fechaLlegada = fechaLlegada;
                    clienteExistente.email = email;

                    clienteExistente.articulo = articulo;                    
                    clienteExistente.modelo = modelo;
                    clienteExistente.marca = marca;

                    MessageBox.Show("Los datos del cliente fueron actualizados correctamente.", "Cliente actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Crear nuevo cliente
                    Cliente nuevoCliente = new Cliente(nombre, dni, direccion, telefono);
                    nuevoCliente.email = email;
                    nuevoCliente.fechaLlegada = fechaLlegada;

                    nuevoCliente.articulo = articulo;
                    nuevoCliente.modelo = modelo;
                    nuevoCliente.marca = marca;
                    listaClientes.Add(nuevoCliente);
                }

                // Actualizar el ListBox
                listBoxClientes.DataSource = null;
                listBoxClientes.DataSource = listaClientes;
            }
            catch (FormatException)
            {
                MessageBox.Show("Verificá que el DNI y el teléfono contengan solo números.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el cliente:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            if (listBoxClientes.SelectedItem != null)
            {
                // Confirmación
                var resultado = MessageBox.Show("¿Estás seguro que deseas eliminar este cliente?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    Cliente clienteSeleccionado = (Cliente)listBoxClientes.SelectedItem;

                    listaClientes.Remove(clienteSeleccionado);

                    // Actualizar el ListBox
                    listBoxClientes.DataSource = null;
                    listBoxClientes.DataSource = listaClientes;

                    // Limpiar los campos
                    textBoxNombre.Text = "";
                    textBoxDni.Text = "";
                    textBoxTelefono.Text = "";
                    textBoxDireccion.Text = "";
                    dateTimePicker1.Value = DateTime.Now;

                    MessageBox.Show("Cliente eliminado con éxito.");
                }
            }
            else
            {
                MessageBox.Show("Seleccioná un cliente para eliminar.");
            }
        }
    }
}
