using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace IDE
{
    class ManejadorArchivos
    {
        public ManejadorArchivos(){
            
        }

        public void guardar(SaveFileDialog saveFileDialog, String texto) {
            try {
                if (saveFileDialog.ShowDialog() == DialogResult.OK){
                    if (File.Exists(saveFileDialog.FileName)) {

                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);

                    }
                    else{
                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);
                    }
                }
            } catch { 
            
            }
        }

        private void escribir(String texto, String nombreArchivo) {
            TextWriter archivo = new StreamWriter(nombreArchivo);
            archivo.WriteLine(texto);
            archivo.Close();
        }
    }
}
