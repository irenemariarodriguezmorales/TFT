namespace camposSemanticos
{
    partial class FormBarraCarga
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBarraCarga));
            this.pictureBoxReloj = new System.Windows.Forms.PictureBox();
            this.barraCarga = new System.Windows.Forms.ProgressBar();
            this.textoBarraCarga = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReloj)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxReloj
            // 
            this.pictureBoxReloj.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxReloj.Image")));
            this.pictureBoxReloj.Location = new System.Drawing.Point(28, 24);
            this.pictureBoxReloj.Name = "pictureBoxReloj";
            this.pictureBoxReloj.Size = new System.Drawing.Size(104, 107);
            this.pictureBoxReloj.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxReloj.TabIndex = 0;
            this.pictureBoxReloj.TabStop = false;
            this.pictureBoxReloj.WaitOnLoad = true;
            // 
            // barraCarga
            // 
            this.barraCarga.Location = new System.Drawing.Point(156, 72);
            this.barraCarga.Maximum = 400;
            this.barraCarga.Name = "barraCarga";
            this.barraCarga.Size = new System.Drawing.Size(458, 23);
            this.barraCarga.TabIndex = 3;
            // 
            // textoBarraCarga
            // 
            this.textoBarraCarga.AutoSize = true;
            this.textoBarraCarga.Font = new System.Drawing.Font("Bradley Hand ITC", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textoBarraCarga.Location = new System.Drawing.Point(152, 24);
            this.textoBarraCarga.Name = "textoBarraCarga";
            this.textoBarraCarga.Size = new System.Drawing.Size(473, 21);
            this.textoBarraCarga.TabIndex = 4;
            this.textoBarraCarga.Text = "Realizando el análisis. Este proceso puede requerir unos minutos...";
            // 
            // FormBarraCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(703, 169);
            this.Controls.Add(this.textoBarraCarga);
            this.Controls.Add(this.barraCarga);
            this.Controls.Add(this.pictureBoxReloj);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormBarraCarga";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cargando su mundo semántico...";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormBarraCarga_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxReloj)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxReloj;
        private System.Windows.Forms.ProgressBar barraCarga;
        private System.Windows.Forms.Label textoBarraCarga;
    }
}