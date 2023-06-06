using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace camposSemanticos
{
    public partial class FormBarraCarga : Form
    {
        private int progreso = 0;

        public FormBarraCarga()
        {
            InitializeComponent();
        }

        public int getProgreso()
        {
            return progreso;
        }

        public void setProgreso(int progreso)
        {
            this.progreso = progreso;
            barraCarga.Value = progreso;
            Refresh();
        }

        private void FormBarraCarga_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
