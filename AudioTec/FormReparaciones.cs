using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioTec.Modelo;
using AudioTec.Logica;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace AudioTec
{
    public partial class FormReparaciones : UserControl
    {
        private Cliente ClienteActual;
        private Electrodomestico eletroActual;
        private Orden ordenActual;
        public FormReparaciones(Orden orden)
        {
            InitializeComponent();

            textBoxGarantia.Text = "30 dias de Garantia";


            //ordenActual = orden;
            //textBoxNombreRep.Text = orden.Cliente.Nombre;
            //textBoxNroOrdenRep.Text = orden.OrdenID.ToString();
            //textBoxArticuloRep.Text = orden.Electrodomesticos[0].Articulo;

            //Para que no se detenga el Programa por si se selecciona el Boton Repraciones sin una Orden
            try
            {
                if (orden == null)
                {
                    MessageBox.Show("No se pasó una orden válida.");
                    return;
                }

                ordenActual = orden;

                if (orden.Cliente != null)
                {
                    textBoxNombreRep.Text = orden.Cliente.Nombre;
                }

                textBoxNroOrdenRep.Text = orden.OrdenID.ToString();

                if (orden.Electrodomesticos != null && orden.Electrodomesticos.Count > 0)
                {
                    textBoxArticuloRep.Text = orden.Electrodomesticos[0].Articulo;
                }
                else
                {
                    textBoxArticuloRep.Text = "Sin artículo";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }



            //textBoxArticuloRep.Text = cliente.articulo;
        }

        private void FormReparaciones_Load(object sender, EventArgs e)
        {

        }


        private void buttonGenerarComprobante_Click(object sender, EventArgs e)
        {
            if (ordenActual == null)
            {
                MessageBox.Show("No hay cliente asignado.");
                return;
            }

            string pagina_texto = Properties.Resources.PlantillaAudioTec.ToString();
            pagina_texto = pagina_texto.Replace("@FACTURANRO",ordenActual.OrdenID.ToString());
            pagina_texto = pagina_texto.Replace("@FECHA",DateTime.Now.ToString("dd/MM/yyyy"));
            pagina_texto = pagina_texto.Replace("@CLIENTE", ordenActual.Cliente.Nombre);
            pagina_texto = pagina_texto.Replace("@DNI", ordenActual.Cliente.DNI);
            pagina_texto = pagina_texto.Replace("@DIRECCION", ordenActual.Cliente.Direccion);
            pagina_texto = pagina_texto.Replace("@TELEFONO", ordenActual.Cliente.Telefono);
            pagina_texto = pagina_texto.Replace("@EMAIL", ordenActual.Cliente.Email);
            pagina_texto = pagina_texto.Replace("@ARTICULO", ordenActual.Electrodomesticos[0].Articulo);
            pagina_texto = pagina_texto.Replace("@DIAGNOSTICO", textBoxDatosRep.Text);
            pagina_texto = pagina_texto.Replace("@REPUESTOS", textBoxRepuesto.Text);
            pagina_texto = pagina_texto.Replace("@TOTAL", textBoxPresupuesto.Text);
            pagina_texto = pagina_texto.Replace("@GARANTIA", textBoxGarantia.Text);

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF Files|*.pdf";
                saveDialog.Title = "Guardar comprobante";
                saveDialog.FileName = $"Comprobante {ordenActual.Cliente.Nombre}-{ordenActual.OrdenID}.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {

                    using (FileStream fs = new FileStream(saveDialog.FileName, FileMode.Create))
                    {
                        //Document doc = new Document(PageSize.A4, 25, 25, 25, 25);
                        Document doc = new Document();
                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                        doc.Open();

                        using (StringReader  reader = new StringReader(pagina_texto)) {
                        
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer,doc,reader);
                        }

                        doc.Close();

                        fs.Close();

                        MessageBox.Show("Comprobante generado exitosamente.");

                        // Guardar Info Repuesto, Diagnostico y Presupuesto
                        OrdenLogica.CargarDiagnosticoPresupuesto(ordenActual);
                        ElectrodomesticoLogica.CargarObservacionElectro(ordenActual.Electrodomesticos[0]);
                        OrdenLogica.TerminarOrden(ordenActual);
                    }

                    // Preguntar si desea enviar por email
                    if (!string.IsNullOrWhiteSpace(ordenActual.Cliente.Email))
                    {
                        DialogResult enviar = MessageBox.Show("¿Deseás enviar el comprobante al correo del cliente?", "Enviar por correo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (enviar == DialogResult.Yes)
                        {
                            try
                            {
                                // Leer credenciales
                                if (!File.Exists("credenciales.txt"))
                                {
                                    MessageBox.Show("No se encontraron las credenciales de Gmail. Iniciá sesión primero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }

                                var lineas = File.ReadAllLines("credenciales.txt");
                                string correoPropio = lineas[0];
                                string claveApp = lineas[1];

                                // Crear mensaje
                                MailMessage mensaje = new MailMessage();
                                mensaje.From = new MailAddress(correoPropio);
                                mensaje.To.Add(ordenActual.Cliente.Email);
                                mensaje.Subject = "Comprobante de reparación - AudioTec";
                                mensaje.Body = "Adjunto te enviamos el comprobante de tu reparación. ¡Gracias por confiar en nosotros!";
                                mensaje.Attachments.Add(new Attachment(saveDialog.FileName));

                                // Configurar SMTP
                                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                                smtp.EnableSsl = true;
                                smtp.Credentials = new NetworkCredential(correoPropio, claveApp);

                                smtp.Send(mensaje);

                                MessageBox.Show("Correo enviado exitosamente.");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error al enviar el correo:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("El cliente no tiene un email registrado. No se puede enviar el comprobante.", "Sin correo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                
            }

        }

        private void checkBoxGarantia_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxGarantia.Checked)
            {
                textBoxGarantia.Enabled = true;
            }
            else
            {
                textBoxGarantia.Enabled=false;
            }
        }
    }
}
