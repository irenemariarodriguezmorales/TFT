namespace camposSemanticos
{
    partial class FormInicial
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInicial));
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtongrupoSemantico = new System.Windows.Forms.RadioButton();
            this.radioButtonredPalabras = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Bradley Hand ITC", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(243, 597);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(285, 78);
            this.button1.TabIndex = 0;
            this.button1.Tag = "boton1";
            this.button1.Text = "Seleccionar directorio";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonSeleccionarDir_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bradley Hand ITC", 19.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(220, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 41);
            this.label1.TabIndex = 2;
            this.label1.Text = "Mundo semántico";
            // 
            // radioButtongrupoSemantico
            // 
            this.radioButtongrupoSemantico.AutoSize = true;
            this.radioButtongrupoSemantico.Checked = true;
            this.radioButtongrupoSemantico.Font = new System.Drawing.Font("Bradley Hand ITC", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtongrupoSemantico.Location = new System.Drawing.Point(39, 556);
            this.radioButtongrupoSemantico.Name = "radioButtongrupoSemantico";
            this.radioButtongrupoSemantico.Size = new System.Drawing.Size(223, 20);
            this.radioButtongrupoSemantico.TabIndex = 7;
            this.radioButtongrupoSemantico.TabStop = true;
            this.radioButtongrupoSemantico.Text = "Análisis de grupos semánticos";
            this.radioButtongrupoSemantico.UseVisualStyleBackColor = true;
            // 
            // radioButtonredPalabras
            // 
            this.radioButtonredPalabras.AutoSize = true;
            this.radioButtonredPalabras.Font = new System.Drawing.Font("Bradley Hand ITC", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonredPalabras.Location = new System.Drawing.Point(513, 556);
            this.radioButtonredPalabras.Name = "radioButtonredPalabras";
            this.radioButtonredPalabras.Size = new System.Drawing.Size(200, 20);
            this.radioButtonredPalabras.TabIndex = 8;
            this.radioButtonredPalabras.Text = "Análisis de red de palabras";
            this.radioButtonredPalabras.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.Font = new System.Drawing.Font("Bradley Hand ITC", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(90, 68);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(591, 63);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "Bienvenido/a a la aplicación que tiene como fin la detección automática de grupos" +
    " semánticos en corpus textuales.";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.Font = new System.Drawing.Font("Bradley Hand ITC", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(39, 147);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(674, 106);
            this.textBox2.TabIndex = 10;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // FormInicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(773, 749);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.radioButtonredPalabras);
            this.Controls.Add(this.radioButtongrupoSemantico);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormInicial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mundo semántico";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormInicial_FormClosed);
            this.Load += new System.EventHandler(this.FormInicial_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtongrupoSemantico;
        private System.Windows.Forms.RadioButton radioButtonredPalabras;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}

