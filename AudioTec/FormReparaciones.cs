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
            ordenActual = orden;
            textBoxNombreRep.Text = orden.Cliente.Nombre;
            textBoxNroOrdenRep.Text = orden.OrdenID.ToString();
            textBoxArticuloRep.Text = orden.Electrodomesticos[0].Articulo;

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

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF Files|*.pdf";
                saveDialog.Title = "Guardar comprobante";
                saveDialog.FileName = $"Comprobante {ordenActual.Cliente.Nombre}-{ordenActual.Cliente.DNI}.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = saveDialog.FileName;

                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        var doc = new Document();
                        PdfWriter writer = PdfWriter.GetInstance(doc, fs);

                        doc.Open();

                        //Datos del Cliente
                        doc.Add(new Paragraph("COMPROBANTE DE REPARACIÓN - AudioTec"));
                        doc.Add(new Paragraph("---------------------------------------------"));
                        doc.Add(new Paragraph("Nombre: " + ordenActual.Cliente.Nombre));
                        doc.Add(new Paragraph("DNI: " + ordenActual.Cliente.DNI));
                        doc.Add(new Paragraph("Teléfono: " + ordenActual.Cliente.Telefono));
                        doc.Add(new Paragraph("Dirección: " + ordenActual.Cliente.Direccion));
                        //doc.Add(new iTextSharp.text.Paragraph("Fecha de llegada: " + ClienteActual.fechaLlegada.ToShortDateString()));
                        
                        ////doc.Add(new iTextSharp.text.Paragraph("Articulo: "+ ClienteActual.articulo));
                        ////doc.Add(new iTextSharp.text.Paragraph("Marca: "+ClienteActual.marca));
                        ////doc.Add(new iTextSharp.text.Paragraph("Modelo: " + ClienteActual.modelo));

                        // En esta parte en ves de sacar los datos directamente del cliente deberia de ser de la orden
                        /* Ejemplo
                        doc.Add(new iTextSharp.text.Paragraph("Nombre: " + Orden.Cliente.Nombre));
                        doc.Add(new iTextSharp.text.Paragraph("DNI: " + Orden.Cliente.DNI));
                        doc.Add(new iTextSharp.text.Paragraph("Teléfono: " + Orden.Cliente.Telefono));
                        doc.Add(new iTextSharp.text.Paragraph("Dirección: " + Orden.Cliente.Direccion));
                        doc.Add(new iTextSharp.text.Paragraph("Fecha de llegada: " + Orden.Fecha_reparacion.ToShortDateString()));
                        doc.Add(new iTextSharp.text.Paragraph("Articulo: " + Orden.Electrodomestico.Articulo));
                        doc.Add(new iTextSharp.text.Paragraph("Marca: " + Orden.Electrodomestico.Articulo));
                        doc.Add(new iTextSharp.text.Paragraph("Modelo: " + Orden.Electrodomestico.Articulo));
                        */

                        // Los datos de reparación, repuestos, etc.
                        doc.Add(new Paragraph($"{textBoxDatosRep.Text}"));

                        doc.Close();

                        fs.Close();

                        MessageBox.Show("Comprobante generado exitosamente.");
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
