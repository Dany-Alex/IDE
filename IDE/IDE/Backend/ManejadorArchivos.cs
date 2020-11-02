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

        public void guardar(string direccionArchivo, String texto) {
            try {
       
                    escribir(texto, direccionArchivo);

                
            } catch(Exception) {
                MessageBox.Show("Error al Guardar");
            }
        }

        public void guardarComo(SaveFileDialog saveFileDialog, String texto)
        {
            propiedadesSaveFileDiaog(saveFileDialog);
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(saveFileDialog.FileName))
                    {

                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);
                        setFileName(saveFileDialog.FileName);
                    }
                    else
                    {
                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);
                        setFileName(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Guardar");
            }
        }

        public void guardarErrorComo(SaveFileDialog saveFileDialog, String texto)
        {
            propiedadesSaveFileDiaogError(saveFileDialog);
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(saveFileDialog.FileName))
                    {

                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);
                        setFileName(saveFileDialog.FileName);
                    }
                    else
                    {
                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);
                        setFileName(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Guardar");
            }
        }



        string direccionArchivoJPG = null;
        public void setFileNameJPG(string direccionArchivo) { this.direccionArchivoJPG = direccionArchivo; }
        public string getFileNameJPG() { return this.direccionArchivoJPG; }
        public void guardarJPGComo(SaveFileDialog saveFileDialog, String texto)
        {
            propiedadesSaveFileDiaogExportJPG(saveFileDialog);
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(saveFileDialog.FileName))
                    {

                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);
                        setFileName(saveFileDialog.FileName);
                    }
                    else
                    {
                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);
                        setFileName(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error al Guardar");
            }
        }



        string direccionArchivoGraphviz = null;
        public void setFileNameGraphviz(string direccionArchivo) { this.direccionArchivoGraphviz = direccionArchivo; }
        public string getFileNameGraphviz() { return this.direccionArchivoGraphviz; }
        public void guardarGraphvizComo(SaveFileDialog saveFileDialog, String texto)
        {
            propiedadesSaveFileDiaogGraphviz(saveFileDialog);
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(saveFileDialog.FileName))
                    {

                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);
                        setFileNameGraphviz(saveFileDialog.FileName);
                    }
                    else
                    {
                        String nombreArchivo = saveFileDialog.FileName;
                        escribir(texto, nombreArchivo);
                        setFileNameGraphviz(saveFileDialog.FileName);
                    }
                }
            }
            catch (Exception)
            {
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
                        setFileName( openFileDialog.FileName);
                       // MessageBox.Show("Archivo: "+nombreArchivo+" cargado exitosamente");
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
        string direccionArchivo = null;
        public void setFileName(string direccionArchivo) { this.direccionArchivo = direccionArchivo; }

        public string getFileName() { return this.direccionArchivo; }

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

        private void propiedadesSaveFileDiaogGraphviz(SaveFileDialog saveFileDialog)
        {
            saveFileDialog.Filter = "Code File (.txt)|*.txt";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;
        }
        private void propiedadesSaveFileDiaogExportJPG(SaveFileDialog saveFileDialog)
        {
            saveFileDialog.Filter = "IMG File (.jpg)|*.jpg";
            saveFileDialog.DefaultExt = "jpg";
            saveFileDialog.AddExtension = true;
        }
        private void propiedadesSaveFileDiaogError(SaveFileDialog saveFileDialog)
        {
            saveFileDialog.Filter = "Code File (.gtE)|*.gtE";
            saveFileDialog.DefaultExt = "gtE";
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
