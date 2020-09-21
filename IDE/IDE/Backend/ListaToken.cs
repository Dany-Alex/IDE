﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE
{
    class ListaToken
    {

        private List<Token> listaTokenResultado = new List<Token>();
        private List<Token> listaTokenErrores= new List<Token>();
        public void limpiarLista()
        {
            listaTokenResultado.Clear();
            listaTokenErrores.Clear();
        }

        public void agregarListaTokenResultado(Token nuevo)
        {
            listaTokenResultado.Add(nuevo);
        }

        public List<Token> getListaTokenResultado()
        {
            return listaTokenResultado;
        }

        public void agregarListaTokenErrores(Token nuevo)
        {
            listaTokenErrores.Add(nuevo);
        }

        public List<Token> getListaTokenErrores()
        {
            return listaTokenErrores;
        }
    }
}
