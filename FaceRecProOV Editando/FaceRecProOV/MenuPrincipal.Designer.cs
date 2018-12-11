namespace MultiFaceRec
{
    partial class MenuPrincipal
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
            this.btnSocio = new System.Windows.Forms.Button();
            this.btnActividades = new System.Windows.Forms.Button();
            this.btnEmpleado = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSocio
            // 
            this.btnSocio.Location = new System.Drawing.Point(39, 39);
            this.btnSocio.Name = "btnSocio";
            this.btnSocio.Size = new System.Drawing.Size(87, 46);
            this.btnSocio.TabIndex = 0;
            this.btnSocio.Text = "Socio";
            this.btnSocio.UseVisualStyleBackColor = true;
            this.btnSocio.Click += new System.EventHandler(this.btnSocio_Click);
            // 
            // btnActividades
            // 
            this.btnActividades.Location = new System.Drawing.Point(258, 39);
            this.btnActividades.Name = "btnActividades";
            this.btnActividades.Size = new System.Drawing.Size(87, 46);
            this.btnActividades.TabIndex = 1;
            this.btnActividades.Text = "Actividades";
            this.btnActividades.UseVisualStyleBackColor = true;
            // 
            // btnEmpleado
            // 
            this.btnEmpleado.Location = new System.Drawing.Point(147, 117);
            this.btnEmpleado.Name = "btnEmpleado";
            this.btnEmpleado.Size = new System.Drawing.Size(87, 46);
            this.btnEmpleado.TabIndex = 2;
            this.btnEmpleado.Text = "Empleado";
            this.btnEmpleado.UseVisualStyleBackColor = true;
            // 
            // MenuPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 185);
            this.Controls.Add(this.btnEmpleado);
            this.Controls.Add(this.btnActividades);
            this.Controls.Add(this.btnSocio);
            this.Name = "MenuPrincipal";
            this.Text = "MenuPrincipal";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSocio;
        private System.Windows.Forms.Button btnActividades;
        private System.Windows.Forms.Button btnEmpleado;
    }
}