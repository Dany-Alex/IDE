using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE
{
    class Token
    {
        private int id;
        private string lexema;
        private int numeroLinea;
        private string tipo;
        private string descripcion;
        private Color color;

        public Token()
        {
        }

        public Token(int id, string lexema, int numeroLinea, string tipo, string descripcion, Color color)
        {
            this.Id = id;
            this.Simbolo = lexema;
            this.NumeroLinea = numeroLinea;
            this.Tipo = tipo;
            this.Descripcion = descripcion;
            this.Color = color;
        }

        public int Id { get => id; set => id = value; }
        public string Simbolo { get => lexema; set => lexema = value; }
        public int NumeroLinea { get => numeroLinea; set => numeroLinea = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public Color Color { get => color; set => color = value; }
    }
}
