using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AudioTec
{
    public partial class FormInicioSesion : Form
    {
        public String correo { get; private set; }
        public String claveApp { get; private set; }
        public FormInicioSesion()
        {
            InitializeComponent();

            if (File.Exists("credenciales.txt"))
            {
                var lineas = File.ReadAllLines("credenciales.txt");
                textBoxCorreo.Text = lineas[0];
                textBoxClave.Text = lineas[1];
                checkBoxRecordarme.Checked = true;
            }
        }

        private void FormInicioSesion_Load(object sender, EventArgs e)
        {

        }

        private void buttonIniciarSesion_Click(object sender, EventArgs e)
        {
            correo = textBoxCorreo.Text.Trim();
            claveApp = textBoxClave.Text.Trim();

            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(claveApp))
            {
                MessageBox.Show("Por favor, completá ambos campos.");
                return;
            }

            if (checkBoxRecordarme.Checked)
            {
                File.WriteAllLines("credenciales.txt", new[] { correo, claveApp });
            }
            else
            {
                if (File.Exists("credenciales.txt"))
                    File.Delete("credenciales.txt");
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
 }

