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
using System.IO;

namespace AudioTec
{
    public partial class FormReparaciones : UserControl
    {
        private Cliente ClienteActual;
        public FormReparaciones(Cliente cliente)
        {
            InitializeComponent();
            this.ClienteActual = cliente;
            textBoxNombreRep.Text = cliente.nombre;
            textBoxArticuloRep.Text = cliente.articulo;
        }

        private void FormReparaciones_Load(object sender, EventArgs e)
        {

        }


        private void buttonGenerarComprobante_Click(object sender, EventArgs e)
        {
            if (ClienteActual == null)
            {
                MessageBox.Show("No hay cliente asignado.");
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF Files|*.pdf";
                saveDialog.Title = "Guardar comprobante";
                saveDialog.FileName = $"Comprobante {ClienteActual.nombre}-{ClienteActual.dni}.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = saveDialog.FileName;

                    using (var fs = new System.IO.FileStream(path, System.IO.FileMode.Create))
                    {
                        var doc = new iTextSharp.text.Document();
                        iTextSharp.text.pdf.PdfWriter.GetInstance(doc, fs);

                        doc.Open();

                        //Datos del Cliente
                        doc.Add(new iTextSharp.text.Paragraph("COMPROBANTE DE REPARACIÓN - AudioTec"));
                        doc.Add(new iTextSharp.text.Paragraph("---------------------------------------------"));
                        doc.Add(new iTextSharp.text.Paragraph("Nombre: " + ClienteActual.nombre));
                        doc.Add(new iTextSharp.text.Paragraph("DNI: " + ClienteActual.dni));
                        doc.Add(new iTextSharp.text.Paragraph("Teléfono: " + ClienteActual.telefono));
                        doc.Add(new iTextSharp.text.Paragraph("Dirección: " + ClienteActual.direccion));
                        doc.Add(new iTextSharp.text.Paragraph("Fecha de llegada: " + ClienteActual.fechaLlegada.ToShortDateString()));
                        doc.Add(new iTextSharp.text.Paragraph("Articulo: "+ ClienteActual.articulo));
                        doc.Add(new iTextSharp.text.Paragraph("Marca: "+ClienteActual.marca));
                        doc.Add(new iTextSharp.text.Paragraph("Modelo: " + ClienteActual.modelo));
                        


                        // Los datos de reparación, repuestos, etc.
                        doc.Add(new iTextSharp.text.Paragraph($"{textBoxDatosRep.Text}"));

                        doc.Close();

                        MessageBox.Show("Comprobante generado exitosamente.");
                    }

                    // Preguntar si desea enviar por email
                    if (!string.IsNullOrWhiteSpace(ClienteActual.email))
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
                                mensaje.To.Add(ClienteActual.email);
                                mensaje.Subject = "Comprobante de reparación - AudioTec";
                                mensaje.Body = "Adjunto te enviamos el comprobante de tu reparación. ¡Gracias por confiar en nosotros!";
                                mensaje.Attachments.Add(new Attachment(path));

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
    }
}
