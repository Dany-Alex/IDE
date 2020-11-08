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

        public string valor { get; set; }

        NodoArbol padre,izquierdo, derecho;
        public IList<NodoArbol> hijo { get; } = new List<NodoArbol>();

        private static int correlativo = 1;
        private int id;

        public NodoArbol(NodoArbol padre ,string valor)
        {
            this.padre = padre;
            this.valor = valor;
            this.izquierdo = null;
            this.derecho = null;
            this.id = correlativo++;
        }

        void insertar(string valor)
        {
           
        }


        string ruta;
        StringBuilder grafo;
        public void graficar(SaveFileDialog saveFileDialog1)
        {
            ManejadorArchivos manejador = new ManejadorArchivos();
            manejador.guardarGraphvizComo(saveFileDialog1, getCodigoGraphviz());
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

        private String getCodigoGraphviz()
        {
            return "digraph grafica{\n" +
                   "rankdir=TB;\n" +
                   "node [shape = record, style=filled, fillcolor=seashell2];\n" +
                    getCodigoInterno() +
                    "}\n";
        }

        private String getCodigoInterno()
        {
            String etiqueta;
            if (izquierdo == null && derecho == null)
            {
                etiqueta = "nodo" + id + " [ label =\"" + valor + "\"];\n";
            }
            else
            {
                etiqueta = "nodo" + id + " [ label =\"<C0>|" + valor + "|<C1>\"];\n";
            }
            if (izquierdo != null)
            {
                etiqueta = etiqueta + izquierdo.getCodigoInterno() +
                   "nodo" + id + ":C0->nodo" + izquierdo.id + "\n";
            }
            if (derecho != null)
            {
                etiqueta = etiqueta + derecho.getCodigoInterno() +
                   "nodo" + id + ":C1->nodo" + derecho.id + "\n";
            }
            return etiqueta;
        }
    }
}
