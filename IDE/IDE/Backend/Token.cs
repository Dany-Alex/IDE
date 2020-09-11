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
        private List<TipoToken> tokenItem = new List<TipoToken>();

        public void limpiarLista() {
            tokenItem.Clear();
        }
        public void listaTokens()
        {
            TipoToken palabraReservada = new TipoToken("si", "", -0, -0, -0, 0, "palabra Reservada", "inicio de un comentario de mas de una linea", "", Color.Green);
            tokenItem.Add(palabraReservada);
            TipoToken palabraReservada1 = new TipoToken("sino", "", -0, -0, -0, 1, "palabra Reservada", "inicio de un comentario de mas de una linea", "",Color.Green);
            tokenItem.Add(palabraReservada1);
            TipoToken palabraReservada2 = new TipoToken("mientras", "", -0, -0, -0, 2, "palabra Reservada", "inicio de un bloque", "", Color.Green);
            tokenItem.Add(palabraReservada2);
            TipoToken palabraReservada3 = new TipoToken("sino_si", "", -0, -0, -0, 3, "palabra Reservada", "inicio de un comentario de mas de una linea", "", Color.Green);
            tokenItem.Add(palabraReservada3);
            TipoToken palabraReservada4 = new TipoToken("hacer", "", -0, -0, -0, 4, "palabra Reservada", "inicio de un comentario de mas de una linea", "", Color.Green);
            tokenItem.Add(palabraReservada4);
            TipoToken palabraReservada5 = new TipoToken("desde", "", -0, -0, -0, 5, "palabra Reservada", "inicio de un bloque", "", Color.Green);
            tokenItem.Add(palabraReservada5);
            TipoToken palabraReservada6 = new TipoToken("hasta", "", -0, -0, -0, 6, "palabra Reservada", "inicio de un comentario de mas de una linea", "", Color.Green);
            tokenItem.Add(palabraReservada6);
            TipoToken palabraReservada7 = new TipoToken("incremento", "", -0, -0, -0, 7, "palabra Reservada", "inicio de un comentario de mas de una linea", "", Color.Green);
            tokenItem.Add(palabraReservada7);
            TipoToken inicioBloqueCodigo = new TipoToken("{", "", -0, -0, -0, 8, "bloque", "inicio de un bloque", "", Color.DarkBlue);
            tokenItem.Add(inicioBloqueCodigo);
            TipoToken finBloqueCodigo = new TipoToken("}", "", -0, -0, -0, 9, "bloque", "inicio de un bloque", "", Color.DarkBlue);
            tokenItem.Add(finBloqueCodigo);
            TipoToken inicioAgrupacion = new TipoToken("(|)", "", -0, -0, -0, 10, "parametro", "inicia peticion de parametro", "", Color.DarkBlue);
            tokenItem.Add(inicioAgrupacion);
            //TipoToken finAgrupacion = new TipoToken(")", "", -0, -0, -0, 11, "parametro", "termina peticion de parametro", "",Color.DarkBlue);
            // tokens.Add(finAgrupacion);
            TipoToken asignacion = new TipoToken("=", "", -0, -0, -0, 12, "asignacion", "simbolo de asignacion", "",Color.Pink);
            tokenItem.Add(asignacion);
            TipoToken posicionador = new TipoToken(";", "", -0, -0, -0, 13, "posicionador", "final de linea", "", Color.Pink);
            tokenItem.Add(posicionador);

            TipoToken numero = new TipoToken(@"([0-9])+([0-9])*", "", -0, -0, -0, 14, "posicionador", "final de linea", "", Color.Purple);
            tokenItem.Add(numero);
            TipoToken numeroDecimal = new TipoToken(@"(-*)+(\d*(\.)+\d*)", "", -0, -0, -0, 14, "posicionador", "final de linea", "", Color.Cyan);
            tokenItem.Add(numeroDecimal);
           // TipoToken cadena = new TipoToken(@"[a-z|A-Z]+", "", -0, -0, -0, 14, "posicionador", "final de linea", "", Color.Gray);
            //tokenItem.Add(cadena);
            TipoToken booleno = new TipoToken(@"true", "", -0, -0, -0, 14, "posicionador", "final de linea", "", Color.Brown);
            tokenItem.Add(booleno);
            TipoToken booleno1 = new TipoToken(@"false", "", -0, -0, -0, 14, "posicionador", "final de linea", "", Color.Brown);
            tokenItem.Add(booleno1);

            //TipoToken operador = new TipoToken("+", "", -0, -0, -0, 18, "operador", "suma", "", Color.DarkBlue);
            //tokenItem.Add(operador);
            TipoToken operador1 = new TipoToken("-", "", -0, -0, -0, 20, "operador", "resta", "", Color.DarkBlue);
            tokenItem.Add(operador1);
           // TipoToken operador2 = new TipoToken("*", "", -0, -0, -0, 21, "operador", "multiplicacion", "", Color.DarkBlue);
          //  tokenItem.Add(operador2);
            TipoToken operador3 = new TipoToken("/", "", -0, -0, -0, 22, "operador", "division", "", Color.DarkBlue);
            tokenItem.Add(operador3);
            TipoToken operadorRelacionador = new TipoToken("!", "", -0, -0, -0, 23, "operador relacionador", "negacion", "", Color.DarkBlue);
            tokenItem.Add(operadorRelacionador);
            TipoToken operadorRelacionador1 = new TipoToken(">", "", -0, -0, -0, 24, "operador relacionador", "mayor que", "", Color.DarkBlue);
            tokenItem.Add(operadorRelacionador1);
            TipoToken operadorRelacionador2 = new TipoToken("<", "", -0, -0, -0, 25, "operador relacionador", "menor que", "", Color.DarkBlue);
            tokenItem.Add(operadorRelacionador2);
            TipoToken operadorRelacionador3 = new TipoToken(">=", "", -0, -0, -0, 26, "operador relacionador", "mayor o igual que", "", Color.DarkBlue);
            tokenItem.Add(operadorRelacionador3);
            TipoToken operadorRelacionador4 = new TipoToken("<=", "", -0, -0, -0, 27, "operador relacionador", "menor o igual que", "", Color.DarkBlue);
            tokenItem.Add(operadorRelacionador4);
            TipoToken operadorRelacionador5 = new TipoToken("&", "", -0, -0, -0, 28, "operador relacionador", "expresion y (and)", "", Color.DarkBlue);
            tokenItem.Add(operadorRelacionador5);
            TipoToken operadorRelacionador6 = new TipoToken("||", "", -0, -0, -0, 29, "operador relacionador", "expresion or", "", Color.DarkBlue);
            tokenItem.Add(operadorRelacionador6);
            TipoToken operadorRelacionador7 = new TipoToken("==", "", -0, -0, -0, 30, "operador relacionador", "expresion igual", "", Color.DarkBlue);
            tokenItem.Add(operadorRelacionador7);
            TipoToken operadorRelacionador8 = new TipoToken("!=", "", -0, -0, -0, 31, "operador relacionador", "expresion distinto", "", Color.DarkBlue);
            tokenItem.Add(operadorRelacionador8);
            TipoToken comentario = new TipoToken(@"(//)+[a-z|A-Z]+", "", -0, -0, -0, 31, "operador relacionador", "expresion distinto", "", Color.Red);
            tokenItem.Add(comentario);
            //TipoToken comentario = new TipoToken(@"(/*)[a-z|A-Z]+(*/)", "", -0, -0, -0, 31, "operador relacionador", "expresion distinto", "", Color.Red);
            //tokenItem.Add(comentario);

        }

        public bool compararAL(string stringSeparado)
        {
            bool bandera = false;
            foreach (var palabra in tokenItem)
            {

                if (palabra.Simbolo == stringSeparado)
                {
                    bandera = true;
                    break;
                }
                else
                {
                    bandera = false;
                }

            }
            return bandera;
        }
        public int contarlineas()
        {
            int numeroId = 0;
            foreach (var numeroLinea in tokenItem)
            {
                numeroId = numeroId + 1;
            }
            return numeroId - 1;
        }

        public List<TipoToken> getListaToken()
        {
            return Tokens;
        }
        public List<TipoToken> Tokens { get => tokenItem; set => tokenItem = value; }

    }
}
