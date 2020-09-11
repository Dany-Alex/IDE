using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE
{
    class TipoToken
    {
        private string simbolo;
        private string valor;
        private int numeroLinea;
        private int tamaño;
        private int ambito;
        private int id;
        private string tipoVar;
        private string tipo;
        private string descripcion;
        private Color color;


        public TipoToken()
        {

        }

        public TipoToken(string simbolo, string valor, int numeroLinea, int tamaño, int ambito, int id, string tipoVar, string tipo, string descripcion, Color color)
        {
            this.Simbolo = simbolo;
            this.Valor = valor;
            this.NumeroLinea = numeroLinea;
            this.Tamaño = tamaño;
            this.Ambito = ambito;
            this.Id = id;
            this.TipoVar = tipoVar;
            this.Tipo = tipo;
            this.Descripcion = descripcion;
            this.Color = color;
        }

        public string Simbolo { get => simbolo; set => simbolo = value; }
        public string Valor { get => valor; set => valor = value; }
        public int NumeroLinea { get => numeroLinea; set => numeroLinea = value; }
        public int Tamaño { get => tamaño; set => tamaño = value; }
        public int Ambito { get => ambito; set => ambito = value; }
        public int Id { get => id; set => id = value; }
        public string TipoVar { get => tipoVar; set => tipoVar = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public Color Color { get => color; set => color = value; }
    }
}
