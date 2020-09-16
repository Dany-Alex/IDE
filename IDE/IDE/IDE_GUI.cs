using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDE
{
    public partial class IDE : Form
    {
        int cantLineas = 0;
        ManejadorArchivos manejador = new ManejadorArchivos();
        ListaToken token = new ListaToken();
        
        public IDE()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {

            manejador.guardar(saveFileDialog1, entradaRichTextBox.Text);
            entradaRichTextBox.Text = "";
        }

        private void cargarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manejador.cargar(openFileDialog1, entradaRichTextBox);
        }

        private void entradaRichTextBox_TextChanged(object sender, EventArgs e)
        {


        }
       
        
       
        private void selectRichText(RichTextBox entradaRichTextBox, string token,Color color){
            Regex rex = new Regex(token);

            MatchCollection mc = rex.Matches(entradaRichTextBox.Text);

            int StartCursorPosition = entradaRichTextBox.SelectionStart;

            foreach (Match m in mc)
            {
                int startIndex = m.Index;
                int StopIndex = m.Length;
                entradaRichTextBox.Select(startIndex, StopIndex);
                entradaRichTextBox.SelectionColor = color;
                entradaRichTextBox.SelectionStart = StartCursorPosition;
                entradaRichTextBox.SelectionColor = Color.Black;

            }
        }

        private void IDE_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void IDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            /*  if (MessageBox.Show("¿Quieres guardar cambios en tu texto?", "IDE",
                               MessageBoxButtons.YesNo) == DialogResult.No)
               {

                          e.Cancel = true;
                      manejador.guardar(saveFileDialog1, entradaRichTextBox.Text);
                      entradaRichTextBox.Text = "";
               }
              */
        }

        private void limpiarCodigoBoton_Click(object sender, EventArgs e)
        {
            Analizador analizador = new Analizador();
            analizador.analizar(entradaRichTextBox, dataGridView1);

            //   token.iniciarListaToken();
            //  entradaRichTextBox.Select(entradaRichTextBox.TextLength, 0);

            /*     foreach (var token in token.Tokens) {
                      selectRichText(entradaRichTextBox, token.Simbolo, token.Color);
                  }

                dataGridView1.DataSource = token.Tokens;*/


        }
    }
}