using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDE.Backend.SegundaFase
{
    class NodoArbol
    {

        public string raiz { get; set; }

        public List<string> hijo { get; } = new List<string>();

        private static int correlativo = 1;
        private int id;

        public NodoArbol()
        {

        }

        public NodoArbol(string raiz, List<string> hijo)
        {
            this.raiz = raiz;
            if (null != this.hijo) this.hijo = new List<string>(hijo);
        }


        string ruta;
        StringBuilder grafo;
        public void graficar(SaveFileDialog saveFileDialog1)
        {
            ManejadorArchivos manejador = new ManejadorArchivos();
           // manejador.guardarGraphvizComo(saveFileDialog1, getCodigoGraphviz());
            try
            {

                try
                {
                    grafo = new StringBuilder();
                    string rutaDot = manejador.getFileNameGraphviz();
                    string salida = ruta + "\\graph1.png";
                    grafo.Append(getCodigoGraphviz());
                    this.generar(rutaDot, salida);

                    MessageBox.Show("ok generar Graph" + rutaDot + " - " + salida);
                    System.Diagnostics.Process.Start(rutaDot);
                    System.Diagnostics.Process.Start(salida);

                }
                catch (Exception x) { MessageBox.Show(x.ToString()); }

            }
            catch (Exception ex)
            {
               Console.WriteLine("Error al generar la imagen para el archivo aux_grafico.dot");
            }
        }
        public void generar(string rutaDot, String salidaIMG)
        {

            try
            {
                System.IO.File.WriteAllText(rutaDot, grafo.ToString());
                string comando = "dot.exe -Tpng " + rutaDot + " -o " + salidaIMG + " ";
                var formatoComando = string.Format(comando);
                var processStart = new System.Diagnostics.ProcessStartInfo("cmd", "/C" + formatoComando);
                var process = new System.Diagnostics.Process();
                process.StartInfo = processStart;
                process.Start();
                process.WaitForExit();
                MessageBox.Show("ok generar" + formatoComando);

            }
            catch (Exception x) { MessageBox.Show(x.ToString()); }


        }

        public StringBuilder getCodigoGraphviz()
        {
            StringBuilder b = new StringBuilder();
            b.AppendFormat("digraph grafica{" +Environment.NewLine+
                   
                   getCodigoInterno().ToString() +
                    "}");
            return b;
        }
 
        private StringBuilder getCodigoInterno()
        {
            StringBuilder b = new StringBuilder();
            if (raiz != null)
            {
                if (hijo.Count > 0) {
                    for (int j = 0; j < hijo.Count; j++)
                    {

                        var nodo = hijo[j];
                        b.AppendFormat(" \"{0}\"->\"{1}{2}\"{3}", raiz, nodo, id, Environment.NewLine);

                    }
                }
        

               
            }
            return b;

        }

    }
}
