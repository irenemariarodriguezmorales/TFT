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
using System.Xml.Linq;
using camposSemanticos.ServicioLematizacion;
using ProcesarTextos;
using camposSemanticos.ServicioFamilias;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Collections.Concurrent;
using static IronPython.Modules._ast;
using camposSemanticos.Modelo;
using camposSemanticos.Almacenamiento;

namespace camposSemanticos
{
    public partial class FormInicial : Form
    {
        public FormInicial()
        {
            InitializeComponent();
        }

        private void FormInicial_Load(object sender, EventArgs e)
        {
        }

        private void FormInicial_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }


        private void buttonSeleccionarDir_Click(object sender, EventArgs e)
        {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    DialogResult result = MessageBox.Show("¿Confirmar directorio?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        buttonSeleccionarDir_Click(sender, e);
                    }
                    else if (result == DialogResult.Yes)
                    {
                        string radioButtonMarcado;

                        if (this.radioButtongrupoSemantico.Checked) radioButtonMarcado = "GRUPOSEMANTICO";
                        else radioButtonMarcado = "REDPALABRAS";

                        CargaDiccionarios cargaDiccionarios = new CargaDiccionarios();

                        cargaDiccionarios.cargarDiccionarios();

                        PalabrasFichero palabrasFichero = new PalabrasFichero(this, radioButtonMarcado, cargaDiccionarios, folderBrowserDialog1.SelectedPath);

                        palabrasFichero.iniciar();

                    }
                }
        }

    }
}