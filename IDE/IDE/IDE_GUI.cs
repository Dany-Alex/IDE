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

        ManejadorArchivos manejador = new ManejadorArchivos();
        ListaToken token = new ListaToken();
        
        public IDE()
        {
            InitializeComponent();

            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;

            analizador.analizar(entradaRichTextBox, dataGridView1, dataGridView2);

            String direccionArchivo;
            direccionArchivo = manejador.getFileName();


            this.Text = string.Format("IDE - {0} - {1} tokens {2} errores", direccionArchivo, analizador.getNumeroToken(), analizador.getNumeroErrores());
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {

            manejador.guardarComo(saveFileDialog1, entradaRichTextBox.Text);
            
            this.Text = string.Format("IDE - {0} - ", manejador.getFileName());
            richCambiado = false;
        }

        private void cargarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manejador.cargar(openFileDialog1, entradaRichTextBox);          
            this.Text = string.Format("IDE - {0} - ", manejador.getFileName());
            richCambiado = false;

        }


        bool richCambiado = false;
        private void entradaRichTextBox_TextChanged(object sender, EventArgs e)
        {
            richCambiado = true;

        this.Text = string.Format("IDE - {0} *- ", manejador.getFileName());
            getLineaColumna();

        }

        public void getLineaColumna() {
            int linea = entradaRichTextBox.GetLineFromCharIndex(entradaRichTextBox.SelectionStart) + 1;
            int columna = entradaRichTextBox.SelectionStart - entradaRichTextBox.GetFirstCharIndexOfCurrentLine();
            getLineaLabel.Text = linea.ToString();
            getColumnaLabel.Text = columna.ToString();
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

            if (richCambiado == true) {
                if (MessageBox.Show("¿Quieres guardar los cambios realizados en tu codigo antes de salir?", "IDE", MessageBoxButtons.YesNo) == DialogResult.No)
                {

                    if (manejador.getFileName() == null)
                    {

                        manejador.guardarComo(saveFileDialog1, entradaRichTextBox.Text);
                        richCambiado = false;
                        this.Text = string.Format("IDE - {0} - ", manejador.getFileName());
                    }
                    else
                    {
                        richCambiado = false;
                        manejador.guardar(manejador.getFileName(), entradaRichTextBox.Text);
                        this.Text = string.Format("IDE - {0} - ", manejador.getFileName());
                    }


                }
                else
                {
                    Application.Exit();
                }
            }
            Application.Exit();


        }

        Analizador analizador = new Analizador();
        private void limpiarCodigoBoton_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;

            analizador.analizar(entradaRichTextBox, dataGridView1,dataGridView2);

            String direccionArchivo;
            direccionArchivo = manejador.getFileName();

 
                this.Text = string.Format("IDE - {0} - {1} tokens {2} errores", direccionArchivo, analizador.getNumeroToken(), analizador.getNumeroErrores()); 
            
            

         }

        private void button1_Click_1(object sender, EventArgs e)
        {
            richCambiado = false;
            manejador.guardar(manejador.getFileName(), entradaRichTextBox.Text);
            this.Text = string.Format("IDE - {0} - ", manejador.getFileName());
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (richCambiado == true)
            {
                if (MessageBox.Show("¿Quieres guardar los cambios realizados en tu codigo antes de crear un nuevo documento?", "IDE", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Text = string.Format("IDE - {0} - ", manejador.getFileName());
                    manejador.guardarComo(saveFileDialog1, entradaRichTextBox.Text);
                        richCambiado = false;

                }
                else
                {
                    manejador.setFileName(null);
                    entradaRichTextBox.Text = "";
                    this.Text = string.Format("IDE - {0} - ", manejador.getFileName());
                    richCambiado = false;
                    dataGridView1.DataSource = null;
                    dataGridView2.DataSource = null;



                }
            }
            manejador.setFileName(null);
            entradaRichTextBox.Text = "";
            this.Text = string.Format("IDE - {0} - ", manejador.getFileName());
            richCambiado = false;
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
        }

        private void entradaRichTextBox_Click(object sender, EventArgs e)
        {
            getLineaColumna();

        }

        private void exportarErrorBoton_Click(object sender, EventArgs e)
        {
            int counter = 0;
            string datos = "";

            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                datos += "-> ERROR # " + counter + ": \n\t";
                counter++;
                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                {
                    datos += dataGridView2.Rows[i].Cells[j].Value.ToString();
                    if (j < dataGridView2.Columns.Count - 1)
                        datos += ",\t";
                }
                datos += "\n";
            }

            manejador.guardarErrorComo(saveFileDialog1, datos);


        }
    }
}