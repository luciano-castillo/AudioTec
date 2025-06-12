using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AudioTec.Logica;
using AudioTec.Modelo;

namespace AudioTec
{
    public partial class VentanaEmergenteDatos : Form
    {

        public Cliente empresa;
        private string idEmpresa = "1";

        public VentanaEmergenteDatos(Cliente emp)
        {
            InitializeComponent();
            empresa = emp;
            CargarDatos();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBoxCambios_CheckedChanged(object sender, EventArgs e)
        {
            CambiarEstadoDeInputs();
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (ClienteLogica.Existe(idEmpresa))
            {
                if (HayCambios())
                {
                    Cliente nuevaEmpresa = DatosEmpresa();

                    empresa.EditarCliente(nuevaEmpresa);
                }
            }
            else
            {
                empresa = DatosEmpresa();
                empresa.crear();
            }

            MessageBox.Show("Se guardo la informacion");
            this.Close();
        }

        private void CargarDatos()
        {
            if (empresa != null) 
            {
                RellenarCampos();
            } 
            else if ( ClienteLogica.Existe(idEmpresa))
            {
                empresa = ClienteLogica.TraerCliente(idEmpresa);
                RellenarCampos();
            }

        }

        private void RellenarCampos()
        {
            textBoxDireccion.Text = empresa.Direccion;
            textBoxEmail.Text = empresa.Email;
            textBoxTelefono.Text = empresa.Telefono;
        }

        private void CambiarEstadoDeInputs()
        {
            textBoxDireccion.Enabled = !textBoxDireccion.Enabled;
            textBoxEmail.Enabled = !textBoxEmail.Enabled;
            textBoxTelefono.Enabled = !textBoxTelefono.Enabled;
            buttonGuardar.Enabled = !buttonGuardar.Enabled;
        }

        private bool HayCambios()
        {
            bool respuesta = false;

            if (empresa.Direccion != textBoxDireccion.Text ||
                empresa.Telefono != textBoxTelefono.Text ||
                empresa.Email != textBoxEmail.Text)
            {
                respuesta = true;
            }

            return respuesta;
        }

        private Cliente DatosEmpresa()
        {
            return new Cliente()
            {
                Nombre = "Empresa",
                DNI = "1",
                Direccion = textBoxDireccion.Text,
                Email = textBoxEmail.Text,
                Telefono = textBoxTelefono.Text,
            };

        }

        
    }
}
