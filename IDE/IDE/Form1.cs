using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDE
{
    public partial class IDE : Form
    {
        public IDE()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "Code Files (.gt)|*.gt";
            saveFileDialog1.DefaultExt = "gt";
            saveFileDialog1.AddExtension = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ManejadorArchivos manejador = new ManejadorArchivos();
            manejador.guardar(saveFileDialog1,"hola");
            
        }

     
    }
}
