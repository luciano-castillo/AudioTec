using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioTec.Modelo;
using AudioTec.Logica;
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.X500;

namespace AudioTec
{
    public partial class Form1 : Form
    {
        List<Cliente> listaClientes = ClienteLogica.Listar();
        List<Orden> listaOrdenes = OrdenLogica.TraerOrdenes();
        

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
            MessageBox.Show(listaOrdenes.Count().ToString());
            MessageBox.Show(listaClientes.Count().ToString());

            listBoxClientes.DataSource = listaOrdenes;
            listBoxClientes.DisplayMember = null;
            panelContenedor.Visible = false;

            dataGridView1.DataSource = listaOrdenes;
            
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
                //List<Orden> ordenes = OrdenLogica.OrdenesdeCliente(clienteSeleccionado);

                Orden orden = OrdenLogica.TraerNroOrden(clienteSeleccionado.DNI);
                // orden.Electrodomesticos = ElectrodomesticoLogica.TraerElectrodomesticos(orden);


                //if (orden.Electrodomesticos != null && orden.Electrodomesticos.Any())
                //{
                //    var primero = orden.Electrodomesticos[0];
                //    textBoxArticulo.Text = primero.Articulo;
                //    textBoxMarca.Text = primero.Marca;
                //    textBoxModelo.Text = primero.Modelo;
                //}

                textBoxNombre.Text = orden.Cliente.Nombre;
                textBoxDni.Text = orden.Cliente.DNI;
                textBoxTelefono.Text = orden.Cliente.Telefono;
                textBoxDireccion.Text = orden.Cliente.Direccion;
                textBoxEmail.Text = orden.Cliente.Email;
                textBoxNroOrden.Text = orden.OrdenID.ToString();
                
                //textBoxNombre.Text = clienteSeleccionado.Nombre;
                //textBoxDni.Text = clienteSeleccionado.DNI.ToString();
                //textBoxTelefono.Text = clienteSeleccionado.Telefono.ToString();
                //textBoxDireccion.Text = clienteSeleccionado.Direccion;

                //dateTimePicker1.Value = clienteSeleccionado.fechaLlegada;
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


            string dniBuscar = textBoxBuscarDni.Text.Trim();
            string ordenBuscar = textBoxBuscarNroOrden.Text.Trim();
            string nombreBuscar = textBoxBuscarNombre.Text.Trim();
            List<Cliente> resultado = new List<Cliente>();

            Cliente clienteEncontrado = null;

            if (!string.IsNullOrWhiteSpace(dniBuscar))
            {
                clienteEncontrado = ClienteLogica.TraerCliente(dniBuscar);
            }
            else if (!string.IsNullOrWhiteSpace(ordenBuscar) && int.TryParse(ordenBuscar, out int nroOrden))
            {
                clienteEncontrado = ClienteLogica.TraerCliente(nroOrden);
            }
            else if(!string.IsNullOrWhiteSpace(nombreBuscar))
            {
                resultado = ClienteLogica.BuscarPorNombre(nombreBuscar);
            }

            if (clienteEncontrado != null || resultado.Count > 0)
            {
                listBoxClientes.DataSource = null;
                listBoxClientes.DataSource = new List<Cliente> { clienteEncontrado };
                listBoxClientes.DisplayMember = "Nombre"; // o usar ToString() en la clase Cliente
                listBoxClientes.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("No se encontró ningún cliente con los datos proporcionados.");
                listBoxClientes.DataSource = null;
            }
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
                //Se genera el cliente
                Cliente cliente = new Cliente
                {
                    DNI = textBoxDni.Text.Trim(),//el Trim quita espacios en blanco 
                    Nombre = textBoxNombre.Text.Trim(),
                    Direccion = textBoxDireccion.Text.Trim(),
                    Telefono = textBoxTelefono.Text.Trim(),
                    Email = textBoxEmail.Text.Trim()
                };

                bool existe = ClienteLogica.Existe(cliente.DNI);

                bool exito = false;

                if (existe)
                {
                    exito = ClienteLogica.Editar(cliente);
                    if (exito)
                        MessageBox.Show("Los datos del cliente fueron actualizados correctamente.", "Cliente actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("No se pudo actualizar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    exito = ClienteLogica.Guardar(cliente);
                    if (exito)
                        MessageBox.Show("El cliente fue registrado correctamente.", "Cliente agregado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("No se pudo guardar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //Electrodomestico
                Electrodomestico electro = new Electrodomestico
                {
                    Dueno = cliente,
                    Articulo = textBoxArticulo.Text.Trim(),
                    Marca = textBoxMarca.Text.Trim(),
                    Modelo = textBoxModelo.Text.Trim(),
                    Observacion = textBoxObservaciones.Text.Trim(),

                };

                bool guardoElectro = ElectrodomesticoLogica.GuardarElectrodomestico(electro);

                if (!guardoElectro)
                {
                    MessageBox.Show("El cliente fue guardado, pero no se pudo guardar el electrodoméstico.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Orden orden = new Orden
                {
                    Cliente = cliente,
                    Fecha_reparacion = dateTimePicker1.Value.ToString(),
                };
                //----------------------------------------------------------------------------------
                //orden.AgregarElectrodomestico(electro);

                bool guardoOrden = OrdenLogica.CrearOrden(orden);

                if (guardoOrden)
                {
                    MessageBox.Show("Se Creo una orden");
                }

                //listBoxClientes.DataSource = null;
                //listBoxClientes.DataSource = ClienteLogica.Listar();
                //listBoxClientes.DisplayMember = "Nombre";
                //listBoxClientes.SelectedIndex = -1;

                listBoxClientes.DataSource = null;
                listBoxClientes.DataSource = OrdenLogica.TraerOrdenes();
                listBoxClientes.DisplayMember = null;
                listBoxClientes.SelectedIndex = -1;

            }

            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar los datos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonNuevo_Click(object sender, EventArgs e)
        {
            textBoxNombre.Text = null;
            textBoxDni.Text = null;
            textBoxDireccion.Text = null;
            textBoxTelefono.Text = null;
            textBoxEmail.Text = null;
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

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            var listaClientes = ClienteLogica.Listar();

            if (listaClientes == null || listaClientes.Count == 0)
            {
                MessageBox.Show("No hay clientes registrados en la base de datos.", "Lista vacía", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBoxClientes.DataSource = null;
                return;
            }

            listBoxClientes.DataSource = null;
            listBoxClientes.DataSource = listaClientes;
            listBoxClientes.DisplayMember = "Nombre"; // o ToString()
            listBoxClientes.SelectedIndex = -1;
            
        }
    }
}
