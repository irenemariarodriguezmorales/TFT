using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace camposSemanticos
{
    public partial class FormResultado : Form
    {

        Dictionary<int, List<string>> listaFinal1punto;
        Dictionary<int, List<string>> listaFinal2punto;
        Dictionary<int, List<string>> listaFinal3punto;
        Dictionary<int, List<string>> listaFinal4punto;

        public FormResultado(Dictionary<int, List<string>> listaFinal1punto, Dictionary<int, List<string>> listaFinal2punto,
                            Dictionary<int, List<string>> listaFinal3punto, Dictionary<int, List<string>> listaFinal4punto)
        {
            InitializeComponent();
            
            this.listaFinal1punto = listaFinal1punto;
            this.listaFinal2punto = listaFinal2punto;
            this.listaFinal3punto = listaFinal3punto;
            this.listaFinal4punto = listaFinal4punto;

            this.radioButton1.CheckedChanged += new EventHandler((s, e) => RadioButtonEventHandler(s));
            this.radioButton2.CheckedChanged += new EventHandler((s, e) => RadioButtonEventHandler(s));
            this.radioButton3.CheckedChanged += new EventHandler((s, e) => RadioButtonEventHandler(s));
            this.radioButton4.CheckedChanged += new EventHandler((s, e) => RadioButtonEventHandler(s));

        }

    private void RadioButtonEventHandler(object sender)
    {
        System.Windows.Forms.RadioButton radioButton = (System.Windows.Forms.RadioButton)sender;
        switch(radioButton.Name)
        {
                case "radioButton1":
                    if(radioButton.Checked) {
                        this.textBox1.Text = string.Empty;
            
                        foreach (var kvp in listaFinal1punto)
                        {
                            this.textBox1.Text += (string.Join(", ", kvp.Value));
                            this.textBox1.AppendText("\r\n");
                        }
                    }
                break;

                case "radioButton2":
                    if (radioButton.Checked) {
                        this.textBox1.Text = string.Empty;
                        foreach (var kvp in listaFinal2punto)
                        {
                            this.textBox1.Text += (string.Join(", ", kvp.Value));
                            this.textBox1.AppendText("\r\n");
                        }
                    }
                break;

                case "radioButton3":
                    if (radioButton.Checked) {
                        this.textBox1.Text = string.Empty;
                        foreach (var kvp in listaFinal3punto)
                        {
                            this.textBox1.Text += (string.Join(", ", kvp.Value));
                            this.textBox1.AppendText("\r\n");
                        }
                    }
                break;

                case "radioButton4":
                    if (radioButton.Checked) {
                        this.textBox1.Text = string.Empty;
                        foreach (var kvp in listaFinal4punto)
                        {
                            this.textBox1.Text += (string.Join(", ", kvp.Value));
                            this.textBox1.AppendText("\r\n");
                        }
                    }
                break;
            }
        }

        private void FormResultado_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void buttonVolverInicio_Click(object sender, EventArgs e)
        {
            FormInicial formularioInicio = new FormInicial();
            this.Dispose();
            formularioInicio.ShowDialog();
        }
    }
}
