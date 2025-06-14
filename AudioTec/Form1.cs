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
using System.Net.Mail;
using System.Net;

namespace AudioTec
{
    public partial class Form1 : Form
    {
        List<Cliente> listaClientes = ClienteLogica.Listar();
        List<Orden> listaOrdenes = OrdenLogica.TraerOrdenes();
        Orden ordenSeleccionada = new Orden();
        Cliente empresa = new Cliente();
        //List<Orden> listaOrdenes = new List<Orden>();

        private string correoGmail;
        private string claveAppGmail;

        public Form1()
        {
            /*
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
            } */



            InitializeComponent();

            
            panelContenedor.Visible = false;

            CargarEmpresa();
            CargarNroOrden();
            CargarOrdenes(listaOrdenes);
            dataGridViewOrdenes.ClearSelection();

            toolStrip1.ImageScalingSize = new Size(32, 32); // tamaño imagen antes de cargar

            toolStripButtonIdentificacion.Image = Image.FromFile(@"Iconos/iconoIdentificacion.png");
            toolStripButtonReparaciones.Image = Image.FromFile(@"Iconos/imagenReparacion.png");
            toolStripButtonOpciones.Image = Image.FromFile(@"Iconos/imagenOpciones.png");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            if(dataGridViewOrdenes.SelectedRows != null)
            {
                //MessageBox.Show($"Cantidad de electrodomésticos: {ordenSeleccionada.Electrodomesticos?.Count}");

                FormReparaciones reparaciones = new FormReparaciones(ordenSeleccionada, empresa);
                panelContenedor.Visible = true;
                panelContenedor.BringToFront();
                panelContenedor.Dock = DockStyle.Fill;
                CargarFormulario(reparaciones);
            }

        }

        private void buttonBuscar_Click(object sender, EventArgs e)
        {

            string dniBuscar = textBoxBuscarDni.Text.Trim();
            int ordenBuscar;
            string nombreBuscar = textBoxBuscarNombre.Text.Trim();

            if (textBoxBuscarNroOrden.Text.Trim() == "")
            {
                ordenBuscar = 0;
            }
            else
            {
                ordenBuscar = int.Parse(textBoxBuscarNroOrden.Text.Trim());
            }

            List<Orden> resultadoBusqueda = new List<Orden>();
            resultadoBusqueda = OrdenLogica.TraerOrdenes(ordenBuscar, dniBuscar, nombreBuscar);
            CargarOrdenes(resultadoBusqueda);
            LimpiarDatos();

        }

        private void buttonGuardarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones mínimas
                if (!ValidarCampos())
                {
                    MessageBox.Show("Por favor, completá todos los campos antes de guardar.");
                    return;
                }
                else
                {

                    if (OrdenLogica.ExisteOrden(int.Parse(textBoxNroOrden.Text)))
                    {
                        Cliente clienteActualizar = CrearCliente();
                        Electrodomestico electroActualizar;

                        if (int.Parse(textBoxNroOrden.Text) != ordenSeleccionada.OrdenID)
                        {
                            ordenSeleccionada = OrdenLogica.TraerOrden(int.Parse(textBoxNroOrden.Text));
                        }

                        if (ordenSeleccionada.TieneElectrodomestico())
                        {
                            electroActualizar = new Electrodomestico
                            {
                                Articulo = textBoxArticulo.Text,
                                Modelo = textBoxModelo.Text,
                                Marca = textBoxMarca.Text,
                                Observacion = textBoxObservaciones.Text
                            };
                        }
                        else
                        {
                            electroActualizar = CrearElectrodomestico(clienteActualizar);
                        }

                        ordenSeleccionada.EditarOrden(clienteActualizar, electroActualizar);
                        
                        MessageBox.Show("Los datos del cliente fueron actualizados correctamente.", "Cliente actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                    else
                    {
                        //Se genera el cliente
                        Cliente cliente = CrearCliente();

                        ExisteClienteComprobacion(cliente);
                        //Electrodomestico
                        Electrodomestico electro = CrearElectrodomestico(cliente);

                        // Crear Orden
                        Orden orden = CrearOrden(cliente);
                        orden.OrdenID = int.Parse(textBoxNroOrden.Text);
                        //----------------------------------------------------------------------------------
                        orden.AgregarElectrodomestico(electro);

                        bool guardoOrden = OrdenLogica.CrearOrden(orden);
                        if (guardoOrden)
                        {
                            MessageBox.Show("Se Creo una orden");
                            OrdenLogica.AumentarNroOrden();
                        }

                        bool guardoElectro = ElectrodomesticoLogica.GuardarElectrodomestico(electro);
                        if (!guardoElectro)
                        {
                            MessageBox.Show("El cliente fue guardado, pero no se pudo guardar el electrodoméstico.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            ElectrodomesticoLogica.AumentarNroElectrodomestico();
                            bool unir = ElectrodomesticoLogica.RelacionarOrdenElectrodomestico(electro, orden);
                            if (!unir)
                            {
                                MessageBox.Show("El cliente fue guardado, pero no se pudo unir los elementos.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                    }
 
                    CargarListaOrdenes();
                    CargarOrdenes(listaOrdenes);
                    LimpiarDatos();
                }                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar los datos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonNuevo_Click(object sender, EventArgs e)
        {
            //ordenSeleccionada = null;
            LimpiarDatos();
            dataGridViewOrdenes.ClearSelection();
            buttonEliminar.Enabled = false;
        }

        private void buttonEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrdenes.SelectedRows != null)
            {
                var resultado = MessageBox.Show("¿Estás seguro que deseas eliminar este cliente?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resultado == DialogResult.Yes)
                {
                    int ordenID = Convert.ToInt32(dataGridViewOrdenes.SelectedRows[0].Cells["OrdenID"].Value);

                    OrdenLogica.EliminarOrden(ordenID);
                    ElectrodomesticoLogica.EliminarElectrodomesticosSinOrden();
                    CargarListaOrdenes();
                    CargarOrdenes(listaOrdenes);
                    LimpiarDatos();
                    int NuevoId = Convert.ToInt32(dataGridViewOrdenes.Rows[0].Cells[0].Value);
                    dataGridViewOrdenes.Rows[0].Selected = true;
                    ordenSeleccionada.TraerOrden(NuevoId);
                }
                else
                {
                    MessageBox.Show("Seleccioná un cliente para eliminar.");
                }
            }

        }

        private void buttonActualizar_Click(object sender, EventArgs e)
        {
            //var listaClientes = ClienteLogica.Listar();

            //if (listaClientes == null || listaClientes.Count == 0)
            //{
            //    MessageBox.Show("No hay clientes registrados en la base de datos.", "Lista vacía", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    listBoxClientes.DataSource = null;
            //    return;
            //}

            //listBoxClientes.DataSource = null;
            //listBoxClientes.DataSource = listaClientes;
            //listBoxClientes.DisplayMember = "Nombre"; // o ToString()
            //listBoxClientes.SelectedIndex = -1;

            CargarListaOrdenes();
            CargarOrdenes(listaOrdenes);
            LimpiarDatos();
            
            
        }

        // Metodos propios
        private void CargarNroOrden()
        {
            textBoxNroOrden.Text = (OrdenLogica.NroOrdenActual + 1).ToString();
        }

        private void CargarOrdenes(List<Orden> listaOrdenes)
        {
            dataGridViewOrdenes.Rows.Clear();
            foreach (var item in listaOrdenes)
            {
                dataGridViewOrdenes.Rows.Add(new object[] { item.OrdenID, item.Cliente.Nombre, item.Cliente.DNI });
            }
            dataGridViewOrdenes.CurrentCell = null;
            dataGridViewOrdenes.ClearSelection();
        }

        private void CargarDatos(Orden orden)
        {
            textBoxNroOrden.Text = orden.OrdenID.ToString();
            textBoxNombre.Text = orden.Cliente.Nombre;
            textBoxDni.Text = orden.Cliente.DNI;
            textBoxTelefono.Text = orden.Cliente.Telefono;
            textBoxDireccion.Text = orden.Cliente.Direccion;
            textBoxEmail.Text = orden.Cliente.Email;

            if (orden.Electrodomesticos.Count > 0)
            {
                textBoxArticulo.Text = orden.Electrodomesticos[0].Articulo;
                textBoxMarca.Text = orden.Electrodomesticos[0].Marca;
                textBoxModelo.Text = orden.Electrodomesticos[0].Modelo;
                textBoxObservaciones.Text = orden.Electrodomesticos[0].Observacion;
            }
            
        }

        // Limpia todos los textos y lo deja vacio, trae el nro de orden actual y fecha
        private void LimpiarDatos()
        {
            CargarNroOrden();
            textBoxNombre.Clear();
            textBoxDni.Clear();
            textBoxTelefono.Clear();
            textBoxDireccion.Clear();
            textBoxEmail.Clear();
            textBoxArticulo.Clear();
            textBoxMarca.Clear();
            textBoxModelo.Clear();
            textBoxObservaciones.Clear();
            dateTimePicker1.Value = DateTime.Now;
        }

        private void CargarListaOrdenes()
        {
            //listaOrdenes = OrdenLogica.TraerOrdenes();

            if (checkBox1.Checked)
            {
                listaOrdenes = OrdenLogica.TraerOrdenesNoTerminadas();
            }
            else
            {
                listaOrdenes = OrdenLogica.TraerOrdenes();
            }
        }

        // Al hacer click un elemento de la tabla, carga los datos
        private void dataGridViewOrdenes_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrdenes.SelectedRows.Count > 0)
            {
                int ordenID = Convert.ToInt32(dataGridViewOrdenes.SelectedRows[0].Cells["OrdenID"].Value);

                //Orden ordenSeleccionada = new Orden();
                ordenSeleccionada.TraerOrden(ordenID);
                ordenSeleccionada.Electrodomesticos = ElectrodomesticoLogica.TraerElectrodomesticos(ordenSeleccionada);
                LimpiarDatos();
                CargarDatos(ordenSeleccionada);
                buttonEliminar.Enabled = true;
            }
        }

        private bool ValidarCampos()
        {
            return !(string.IsNullOrWhiteSpace(textBoxNombre.Text) ||
                     string.IsNullOrWhiteSpace(textBoxDni.Text) ||
                     string.IsNullOrWhiteSpace(textBoxDireccion.Text) ||
                     string.IsNullOrWhiteSpace(textBoxTelefono.Text) ||
                     string.IsNullOrWhiteSpace(textBoxArticulo.Text));
        }

        private Cliente CrearCliente()
        {
            return new Cliente {
                DNI = textBoxDni.Text.Trim(),//el Trim quita espacios en blanco 
                Nombre = textBoxNombre.Text.Trim(),
                Direccion = textBoxDireccion.Text.Trim(),
                Telefono = textBoxTelefono.Text.Trim(),
                Email = textBoxEmail.Text.Trim()
            };
        }

        private Electrodomestico CrearElectrodomestico(Cliente cliente)
        {
            return new Electrodomestico
            {
                ElectrodomesticoID = (ElectrodomesticoLogica.electroIDActual + 1).ToString(),
                Dueno = cliente,
                Articulo = textBoxArticulo.Text.Trim(),
                Marca = textBoxMarca.Text.Trim(),
                Modelo = textBoxModelo.Text.Trim(),
                Observacion = textBoxObservaciones.Text.Trim(),
            };
        }

        private bool ExisteClienteComprobacion(Cliente cliente)
        {
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

            return existe;
        }

        private Orden CrearOrden(Cliente cliente)
        {
            return new Orden {
                OrdenID = int.Parse(textBoxNroOrden.Text.Trim()),
                Cliente = cliente,
                Fecha_reparacion = dateTimePicker1.Value.ToString()
            };
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Devuelve true si esta activado
            CargarListaOrdenes();
            CargarOrdenes(listaOrdenes);
        }

        private void toolStripButtonOpciones_Click(object sender, EventArgs e)
        {
            VentanaEmergenteDatos ventana = new VentanaEmergenteDatos(empresa);
            ventana.StartPosition = FormStartPosition.CenterParent;
            ventana.ShowDialog();
            empresa = ventana.empresa;
        }

        private void CargarEmpresa()
        {
            if (ClienteLogica.Existe("1"))
            {
                empresa = ClienteLogica.TraerCliente("1");
            }
        }

        private void textBoxDni_Leave(object sender, EventArgs e)
        {
            string dni = textBoxDni.Text;

            if (!string.IsNullOrEmpty(dni))
            {
                if (ClienteLogica.Existe(dni))
                {
                    DialogResult enviar = MessageBox.Show("El Cliente ya existe. ¿Deseas autocompletar el formulario?", 
                        "Enviar por correo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (enviar == DialogResult.Yes)
                    {
                        try
                        {
                            Cliente nuevoCliente = ClienteLogica.TraerCliente(dni);

                            textBoxNombre.Text = nuevoCliente.Nombre;
                            textBoxTelefono.Text = nuevoCliente.Telefono;
                            textBoxEmail.Text = nuevoCliente.Email;
                            textBoxDireccion.Text = nuevoCliente.Direccion;

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al completar los campos:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
        }
    }
}
