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
            propiedadesSaveFileDiaog(saveFileDialog);
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
            } catch(Exception) {
                MessageBox.Show("Error al Guardar");
            }
        }

        public void cargar(OpenFileDialog openFileDialog, RichTextBox entradaRichTextBox) {
            propiedadesOpenFileDialog(openFileDialog);
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK){
                    if (File.Exists(openFileDialog.FileName)){

                        String nombreArchivo = openFileDialog.FileName;
                        leer(entradaRichTextBox,nombreArchivo);
                        MessageBox.Show("Archivo: "+nombreArchivo+" cargado exitosamente");
                    }
                    else{
                       
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Cargar");
            }
        }
        private void escribir(String texto, String nombreArchivo) {
            TextWriter archivo = new StreamWriter(nombreArchivo);
            archivo.WriteLine(texto);
            archivo.Flush();
            archivo.Close();
            MessageBox.Show("Archivo: " + nombreArchivo + " guardado exitosamente");
        }

        private void leer(RichTextBox entradaRichTextBox, String nombreArchivo)
        {
            TextReader archivo = new StreamReader(nombreArchivo);
            entradaRichTextBox.Text=(archivo.ReadToEnd());
            archivo.Close();
        }


        private void propiedadesSaveFileDiaog(SaveFileDialog saveFileDialog) {
            saveFileDialog.Filter = "Code File (.gt)|*.gt";
            saveFileDialog.DefaultExt = "gt";
            saveFileDialog.AddExtension = true;
        }
         private void propiedadesOpenFileDialog(OpenFileDialog openFileDialog) {
            openFileDialog.Title=("Busca tu archivo .gt");
            openFileDialog.Filter = "Code File (.gt)|*.gt";
            openFileDialog.DefaultExt = "gt";
            openFileDialog.AddExtension = true;
        }
    }
}
