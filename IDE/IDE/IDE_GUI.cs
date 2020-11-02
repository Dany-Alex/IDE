using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            analizador.analizar(entradaRichTextBox,richTextBoxConsola, dataGridView1, dataGridView2);

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
                
            getLineaLabel.Text = string.Format("Linea: {0}  Columna: {1}", linea, columna);
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

            analizador.analizar(entradaRichTextBox,richTextBoxConsola, dataGridView1,dataGridView2);



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

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonGenerarArbol_Click(object sender, EventArgs e)
        {
            manejador.guardarGraphvizComo(saveFileDialog1, analizador.getCodigoDot());
            graficador();
            GenerateGraph(analizador.getCodigoDot());
            abrirGrafo();
            
        }

        string ruta;
        StringBuilder grafo;
        public void graficador() { ruta = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory); }
        public void GenerateGraph(string codigoDot)
        {
            
            try
            {
                grafo = new StringBuilder();
                string rutaDot = ruta + "\\img\\graph.txt";
                string salida = ruta + "\\img\\graph1.png";
                grafo.Append(codigoDot);
                this.generar(rutaDot, salida);

                MessageBox.Show("ok generar Graph" + rutaDot + " - " + salida);
            }
            catch (Exception x) { MessageBox.Show(x.ToString()); }
        }
        public void abrirGrafo() {
            if (File.Exists(ruta))
            {
                try {
                    System.Diagnostics.Process.Start(ruta);
                    MessageBox.Show("ok abrir" + ruta );

                }
                catch (Exception x){ MessageBox.Show(x.ToString()); }
            }
            else { 
            }
        }
        public void  generar(string rutaDot, String salidaIMG)
        {

            try
            {
                System.IO.File.WriteAllText(rutaDot, grafo.ToString());
                string comando = "dot.exe -Tpng " + rutaDot + " -o " + salidaIMG+" ";
                var formatoComando = string.Format(comando);
                var processStart = new System.Diagnostics.ProcessStartInfo("cmd", "/C" + formatoComando);
                var process = new System.Diagnostics.Process();
                process.StartInfo = processStart;
                process.Start();
                process.WaitForExit();
                    MessageBox.Show("ok generar" + formatoComando );

            }
            catch (Exception x) { MessageBox.Show(x.ToString()); }

           
        }
    }
}