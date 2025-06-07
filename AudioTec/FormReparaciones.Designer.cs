namespace AudioTec
{
    partial class FormReparaciones
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNroOrdenRep = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxNombreRep = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxArticuloRep = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxDatosRep = new System.Windows.Forms.TextBox();
            this.buttonGenerarComprobante = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxRepuesto = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPresupuesto = new System.Windows.Forms.TextBox();
            this.textBoxGarantia = new System.Windows.Forms.TextBox();
            this.checkBoxGarantia = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Informacion Cliente";
            // 
            // textBoxNroOrdenRep
            // 
            this.textBoxNroOrdenRep.Location = new System.Drawing.Point(305, 60);
            this.textBoxNroOrdenRep.Name = "textBoxNroOrdenRep";
            this.textBoxNroOrdenRep.Size = new System.Drawing.Size(100, 20);
            this.textBoxNroOrdenRep.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(223, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Nro Orden";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(439, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 22);
            this.label3.TabIndex = 6;
            this.label3.Text = "Nombre";
            // 
            // textBoxNombreRep
            // 
            this.textBoxNombreRep.Location = new System.Drawing.Point(510, 61);
            this.textBoxNombreRep.Name = "textBoxNombreRep";
            this.textBoxNombreRep.Size = new System.Drawing.Size(100, 20);
            this.textBoxNombreRep.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(644, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 22);
            this.label4.TabIndex = 8;
            this.label4.Text = "Articulo";
            // 
            // textBoxArticuloRep
            // 
            this.textBoxArticuloRep.Location = new System.Drawing.Point(715, 63);
            this.textBoxArticuloRep.Name = "textBoxArticuloRep";
            this.textBoxArticuloRep.Size = new System.Drawing.Size(100, 20);
            this.textBoxArticuloRep.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(54, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(252, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "Diagnostico y Datos de la Reparacion";
            // 
            // textBoxDatosRep
            // 
            this.textBoxDatosRep.Location = new System.Drawing.Point(57, 148);
            this.textBoxDatosRep.Multiline = true;
            this.textBoxDatosRep.Name = "textBoxDatosRep";
            this.textBoxDatosRep.Size = new System.Drawing.Size(416, 188);
            this.textBoxDatosRep.TabIndex = 10;
            // 
            // buttonGenerarComprobante
            // 
            this.buttonGenerarComprobante.Location = new System.Drawing.Point(57, 583);
            this.buttonGenerarComprobante.Name = "buttonGenerarComprobante";
            this.buttonGenerarComprobante.Size = new System.Drawing.Size(80, 47);
            this.buttonGenerarComprobante.TabIndex = 11;
            this.buttonGenerarComprobante.Text = "Generar Recibo";
            this.buttonGenerarComprobante.UseVisualStyleBackColor = true;
            this.buttonGenerarComprobante.Click += new System.EventHandler(this.buttonGenerarComprobante_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(575, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 25);
            this.label6.TabIndex = 12;
            this.label6.Text = "Repuestos";
            // 
            // textBoxRepuesto
            // 
            this.textBoxRepuesto.Location = new System.Drawing.Point(578, 148);
            this.textBoxRepuesto.Multiline = true;
            this.textBoxRepuesto.Name = "textBoxRepuesto";
            this.textBoxRepuesto.Size = new System.Drawing.Size(416, 188);
            this.textBoxRepuesto.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(59, 359);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 22);
            this.label7.TabIndex = 15;
            this.label7.Text = "Presupuesto";
            // 
            // textBoxPresupuesto
            // 
            this.textBoxPresupuesto.Location = new System.Drawing.Point(160, 359);
            this.textBoxPresupuesto.Name = "textBoxPresupuesto";
            this.textBoxPresupuesto.Size = new System.Drawing.Size(103, 20);
            this.textBoxPresupuesto.TabIndex = 14;
            // 
            // textBoxGarantia
            // 
            this.textBoxGarantia.Enabled = false;
            this.textBoxGarantia.Location = new System.Drawing.Point(712, 371);
            this.textBoxGarantia.Name = "textBoxGarantia";
            this.textBoxGarantia.Size = new System.Drawing.Size(282, 20);
            this.textBoxGarantia.TabIndex = 16;
            // 
            // checkBoxGarantia
            // 
            this.checkBoxGarantia.AutoSize = true;
            this.checkBoxGarantia.Location = new System.Drawing.Point(925, 397);
            this.checkBoxGarantia.Name = "checkBoxGarantia";
            this.checkBoxGarantia.Size = new System.Drawing.Size(69, 17);
            this.checkBoxGarantia.TabIndex = 17;
            this.checkBoxGarantia.Text = "Modificar";
            this.checkBoxGarantia.UseVisualStyleBackColor = true;
            this.checkBoxGarantia.CheckedChanged += new System.EventHandler(this.checkBoxGarantia_CheckedChanged);
            // 
            // FormReparaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxGarantia);
            this.Controls.Add(this.textBoxGarantia);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxPresupuesto);
            this.Controls.Add(this.textBoxRepuesto);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonGenerarComprobante);
            this.Controls.Add(this.textBoxDatosRep);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxArticuloRep);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxNombreRep);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxNroOrdenRep);
            this.Controls.Add(this.label1);
            this.Name = "FormReparaciones";
            this.Size = new System.Drawing.Size(1174, 732);
            this.Load += new System.EventHandler(this.FormReparaciones_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNroOrdenRep;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxNombreRep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxArticuloRep;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxDatosRep;
        private System.Windows.Forms.Button buttonGenerarComprobante;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxRepuesto;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPresupuesto;
        private System.Windows.Forms.TextBox textBoxGarantia;
        private System.Windows.Forms.CheckBox checkBoxGarantia;
    }
}