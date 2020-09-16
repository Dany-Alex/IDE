using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE
{
    class ListaToken
    {
        private List<Token> listaTokenItem = new List<Token>();
        private List<Token> listaTokenResultado = new List<Token>();
        public void inicializarLista()
        {
            Token tonen1 = new Token(1, "si", -0, "Palabra reservada", "Prueba", Color.Red);
            listaTokenItem.Add(tonen1);
        }

        public List<Token> getListaToken()
        {
            return listaTokenItem;
        }
        public void agregarListaTokenResultado(Token nuevo)
        {
            listaTokenResultado.Add(nuevo);
        }

        public List<Token> getListaTokenResultado()
        {
            return listaTokenResultado;
        }
        internal List<Token> TokenItem { get => listaTokenItem; set => listaTokenItem = value; }
    }
}
