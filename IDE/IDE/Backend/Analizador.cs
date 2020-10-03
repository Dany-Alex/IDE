using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDE
{
    class Analizador
    {
        ListaToken listaToken = new ListaToken();



        String textoEntrada;

        int numeroErrores, numeroToken;
        public void analizar(RichTextBox richTextBox, DataGridView resultados, DataGridView errores)
        {
            int numeroLinea = 1;
            listaToken.iniciarListaErroeres();
            listaToken.limpiarLista();
            textoEntrada = richTextBox.Text;
            textoEntrada = textoEntrada + " #";

            estadoAutomataNumeros = 0;

            LexemaAuxiliar = "";
            Char c;

            for (int i = 0; i <= textoEntrada.Length - 1; i++)
            {
                c = textoEntrada.ElementAt(i);
                if (c.CompareTo('\n') == 0) { numeroLinea++; }


                automataCadenas(richTextBox, c, i, numeroLinea);
                automataNumeros(richTextBox, c, i, numeroLinea);
                automataOperadores(richTextBox, c, i, numeroLinea);
                
                
               // automataComentario(richTextBox, c, i, numeroLinea);

            }

            resultados.DataSource = (listaToken.getListaTokenResultado());
           setNumeroToken(resultados.RowCount);

            errores.DataSource = (listaToken.getListaTokenErrores());
            setNumeroErrores(errores.RowCount);
  

        }



        public void pintar(RichTextBox richTextBox, String palabla, int startIndex, Color color)
        {
            int StartCursorPosition = richTextBox.Text.Length;

            richTextBox.Select(startIndex - palabla.Length, palabla.Length);
            richTextBox.SelectionColor = color;
            richTextBox.SelectionStart = StartCursorPosition;
            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectionStart = StartCursorPosition;

        }

        int estadoAutomataNumeros = 0;
        private String LexemaAuxiliar;
        public void automataNumeros(RichTextBox richTextBox, Char c, int i, int numeroLinea)
        {

            switch (estadoAutomataNumeros)
            {
                case 0:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataNumeros = 1;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('-') == 0)
                    {
                        estadoAutomataNumeros = 4;
                        LexemaAuxiliar += c;

                    }
                    else if (c.CompareTo('+') == 0)
                    {
                        estadoAutomataNumeros = 6;
                        LexemaAuxiliar += c;


                    }
                    else
                    {
                        if (c.CompareTo('#') == 0 && i == textoEntrada.Length - 1)
                        {
                            Console.WriteLine("fin de la cadena");
                        }
                        else
                        {
                            Console.WriteLine("error --- " + c);
                            estadoAutomataNumeros = 0;
                        }
                    }

                    break;
                #region verificar si es entero
                case 1:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataNumeros = 1;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('.') == 0)
                    {
                        estadoAutomataNumeros = 2;
                        LexemaAuxiliar += c;
                    }

                    else
                    {
                        listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Entero", "", Color.Purple));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Purple);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                        i -= 1;
                    }
                    break;
                #endregion

                #region verifica errores y decimal
                case 2:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataNumeros = 3;
                        LexemaAuxiliar += c;
                    }
                    else
                    {
                        Console.WriteLine("error --- " + c + " despues de un puento se esperaban mas numeros");
                        listaToken.agregarListaTokenErrores(new Token(1, c.ToString(), numeroLinea, "Se esperaba mas numeros ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                    }
                    break;
                case 3:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataNumeros = 3;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('.') == 0)
                    {
                        Console.WriteLine("error --- " + c + " despues de un puento se esperaban mas numeros");
                        listaToken.agregarListaTokenErrores(new Token(2, c.ToString(), numeroLinea, "No se puede coloar dos puntos un un doble ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                    }
                    else
                    {
                        listaToken.agregarListaTokenResultado(new Token(2, LexemaAuxiliar, numeroLinea, "Decimal", "", Color.Cyan));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Cyan);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                        i -= 1;
                    }
                    break;
                #endregion

                #region estado 4 y 5: agregar operador con el signo '-'

                case 4:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataNumeros = 1;
                        LexemaAuxiliar += c;

                    }
                    else if (c.CompareTo('-') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataNumeros = 5;

                    }
                    else
                    {

                        listaToken.agregarListaTokenResultado(new Token(3, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Resta", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                    }

                    break;

                case 5:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataNumeros = 0;

                    }
                    else if (c.CompareTo('-') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(2, c.ToString(), numeroLinea, "No se pueden colocar mas de dos guiones seguidos", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(4, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Decremento", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                    }
                    break;
                #endregion

                #region estado 6 y 7: agregar operador con el signo '+'

                case 6:

                    if (c.CompareTo('+') == 0)
                    {
                        estadoAutomataNumeros = 7;
                        LexemaAuxiliar += c;


                    }
                    else
                    {

                        listaToken.agregarListaTokenResultado(new Token(5, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Suma", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;

                    }
                    break;

                case 7:
                    listaToken.agregarListaTokenResultado(new Token(6, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Incremento", Color.DarkBlue));
                    pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                    LexemaAuxiliar = "";
                    estadoAutomataNumeros = 0;
                    break;

                #endregion

              

            }

        }


        int estadoAutomataOperadores = 0;

        public void automataOperadores(RichTextBox richTextBox, Char c, int i, int numeroLinea)
        {

            switch (estadoAutomataOperadores)
            {
                case 0:
                     if (c.CompareTo('*') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(7, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else if (c.CompareTo('<') == 0)
                    {

                        estadoAutomataOperadores = 4;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('>') == 0)
                    {
                        estadoAutomataOperadores = 6;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('=') == 0)
                    {
                        estadoAutomataOperadores = 8;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('!') == 0)
                    {
                        estadoAutomataOperadores = 10;
                        LexemaAuxiliar += c;

                    }
                    else if (c.CompareTo('&') == 0)
                    {
                        estadoAutomataOperadores = 12;
                        LexemaAuxiliar += c;

                    }
                    else if (c.CompareTo('|') == 0)
                    {
                        estadoAutomataOperadores = 14;
                        LexemaAuxiliar += c;


                    }
                    else if (c.CompareTo(';') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(8, LexemaAuxiliar, numeroLinea, "Finalizador de linea", "", Color.Pink));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.Magenta);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else if (c.CompareTo('(') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(9, LexemaAuxiliar, numeroLinea, "Signos de agrupación", "", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else if (c.CompareTo(')') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(10, LexemaAuxiliar, numeroLinea, "Signos de agrupación", "", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else if (c.CompareTo('/') == 0)
                    {
                        estadoAutomataOperadores = 16;
                        LexemaAuxiliar += c;

                    }
                    else
                    {
                        #region fin de cadena
                        if (c.CompareTo('#') == 0 && i == textoEntrada.Length - 1)
                        {
                            Console.WriteLine("fin de la cadena");
                        }
                        else
                        {
                            Console.WriteLine("error --- " + c);
                            estadoAutomataOperadores = 0;
                        }
                        #endregion
                    }

                    break;
                #region estado 1: agregar operador incremento o suma
                case 1:

                    if (c.CompareTo('+') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(11, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Incremento", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;

                    }
                    else
                    {

                        listaToken.agregarListaTokenResultado(new Token(12, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Suma", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;

                    }
                    break;
                #endregion

                #region estado 2 y 3: agregar operador con el signo '-'
                case 2:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('-') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataOperadores = 3;

                    }
                    else
                    {

                        listaToken.agregarListaTokenResultado(new Token(13, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Resta", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }

                    break;

                case 3:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('-') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(3, c.ToString(), numeroLinea, "No se pueden colocar mas de dos guiones seguidos", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(14, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Decremento", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;
                #endregion

                #region estado 4 y 5: agregar operador con el signo '<' y '<='
                case 4:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('=') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataOperadores = 5;

                    }
                    else
                    {

                        listaToken.agregarListaTokenResultado(new Token(15, LexemaAuxiliar, numeroLinea, "Operadores relacionales", "Menor", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;

                case 5:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('=') == 0 || c.CompareTo('<') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(4, c.ToString(), numeroLinea, "No se pueden colocar tres singos seguidos", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(16, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Menor Igual", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;
                #endregion

                #region estado 6 y 7: agregar operador con el signo '>' y '>='
                case 6:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('=') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataOperadores = 7;

                    }
                    else
                    {

                        listaToken.agregarListaTokenResultado(new Token(17, LexemaAuxiliar, numeroLinea, "Operadores relacionales", "Mayor", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;

                case 7:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('=') == 0 || c.CompareTo('>') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(5, c.ToString(), numeroLinea, "No se pueden colocar tres singos seguidos", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(18, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Mayor Igual", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;
                #endregion

                #region estado 8 y 9: agregar operador con el signo '=' y '=='
                case 8:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('=') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataOperadores = 9;

                    }
                    else
                    {

                        listaToken.agregarListaTokenResultado(new Token(19, LexemaAuxiliar, numeroLinea, "Operador Relacional", "Igual", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i , Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }

                    break;

                case 9:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('=') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(6, c.ToString(), numeroLinea, "No se pueden colocar mas de dos iguales seguidos", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(20, LexemaAuxiliar, numeroLinea, "Operador Relacional", "Comparacion", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;
                #endregion

                #region estado 10 y 11: agregar operador con el signo '!' y '!='
                case 10:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('=') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataOperadores = 11;

                    }
                    else
                    {

                        listaToken.agregarListaTokenResultado(new Token(21, LexemaAuxiliar, numeroLinea, "Operadores Logico", "Negacion", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;

                case 11:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('=') == 0 || c.CompareTo('!') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(7, c.ToString(), numeroLinea, "No se pueden colocar tres singos seguidos", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(22, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Diferente", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;
                #endregion

                #region estado 12 y 13: agregar operador con el signo '&' y '&&'
                case 12:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }

                    else if (c.CompareTo('&') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataOperadores = 13;

                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenErrores(new Token(23, LexemaAuxiliar, numeroLinea, "Se esperaba otro &", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;

                case 13:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('&') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(8, c.ToString(), numeroLinea, "No se pueden colocar tres singos seguidos", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(24, LexemaAuxiliar, numeroLinea, "Operador Logico", "Y", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;
                #endregion

                #region estado 14 y 15: agregar operador con el signo '|' y '||'
                case 14:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }

                    else if (c.CompareTo('|') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataOperadores = 15;

                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenErrores(new Token(9, LexemaAuxiliar, numeroLinea, "Se esperaba otro |", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;

                case 15:
                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }
                    else if (c.CompareTo('|') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(10, c.ToString(), numeroLinea, "No se pueden colocar tres singos seguidos", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(25, LexemaAuxiliar, numeroLinea, "Operador Logico", "O", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;
                #endregion

               #region estado 16 y 17: agregar operador con el signo '/' y '//'
                case 16:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataOperadores = 0;

                    }

                    else if (c.CompareTo('/') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataOperadores = 17;

                    }
                    else if (c.CompareTo('*') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataOperadores = 18;

                    }
                    else
                    {
                        listaToken.agregarListaTokenResultado(new Token(26, LexemaAuxiliar, numeroLinea, "Operador Aritmetico", "Divicion", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;

                case 17:
                    if (Char.IsLetter(c) || Char.IsDigit(c) || c.CompareTo(' ') == 0)
                    {
                        estadoAutomataOperadores = 17;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('\n') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(27, LexemaAuxiliar, numeroLinea, "Comentario", "Corto", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.Red);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;

                    }
                    else
                    {
                        listaToken.agregarListaTokenErrores(new Token(11, c.ToString(), numeroLinea, "Se cierre de cometario ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;

                case 18:
                    if (Char.IsLetter(c) || Char.IsDigit(c) || c.CompareTo(' ') == 0 || c.CompareTo('\n') == 0)
                    {
                        estadoAutomataOperadores = 17;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('*') == 0)
                    {
                        estadoAutomataOperadores = 18;
                        LexemaAuxiliar += c;

                    }
                    else
                    {
                        listaToken.agregarListaTokenErrores(new Token(12, c.ToString(), numeroLinea, "Se cierre de cometario ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }

                    break;
                case 19:
                    if (Char.IsLetter(c) || Char.IsDigit(c) || c.CompareTo(' ') == 0)
                    {
                        estadoAutomataOperadores = 17;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('/') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(28, LexemaAuxiliar, numeroLinea, "Comentario", "Largo", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.Red);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;

                    }
                    else
                    {

                        listaToken.agregarListaTokenErrores(new Token(13, LexemaAuxiliar, numeroLinea, "Se esperaba cierre de cometario ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;

                #endregion

            }

        }

        private int estadoAutomataCadenas;
        private String[] reservedWord = new String[] { "SI", "SINO", "SINO_SI", "MIENTRAS", "HACER", "DESDE", "HASTA", "INCREMENTO", "ENTONCES" };
        public void automataCadenas(RichTextBox richTextBox, Char c, int i, int numeroLinea)
        {

            switch (estadoAutomataCadenas)
            {
                case 0:
                    if (c.CompareTo('"') == 0)
                    {
                        estadoAutomataCadenas = 1;
                        LexemaAuxiliar += c;
                        

                    }
                    else if (Char.IsLetter(c))
                    {
                        estadoAutomataCadenas = 2;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo(' ') == 0)
                    {
                        estadoAutomataCadenas = 2;

                    }

                    else
                    {
                        if (c.CompareTo('#') == 0 && i == textoEntrada.Length - 1)
                        {
                            Console.WriteLine("fin de la cadena");
                        }
                        else
                        {
                            Console.WriteLine("error --- " + c);
                            estadoAutomataCadenas = 0;
                        }
                    }

                    break;

                #region Cadena
                case 1:

                    if (Char.IsLetter(c) || Char.IsDigit(c))
                    {
                        estadoAutomataCadenas = 1;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('"') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(29, LexemaAuxiliar, numeroLinea, "Cadena", "", Color.Gray));
                        pintar(richTextBox, LexemaAuxiliar, i , Color.Gray);
                        LexemaAuxiliar = "";
                        estadoAutomataCadenas = 0;

                    }
                    else
                    {
                        listaToken.agregarListaTokenErrores(new Token(14, c.ToString(), numeroLinea, "Se cierre de cometario ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataCadenas = 0;
                    }
                    break;
                #endregion

                #region palabra reservada
                case 2:
                    if (Char.IsLetter(c))
                    {
                        estadoAutomataCadenas = 2;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('_') == 0)
                    {
                        estadoAutomataCadenas = 3;
                        LexemaAuxiliar += c;
                    }

                    else if (c.CompareTo(' ') == 0)
                    {
                        estadoAutomataCadenas = 4;

                    }
                    else
                    {

                        if (reservedWord.Contains(LexemaAuxiliar))
                        {
                            listaToken.agregarListaTokenResultado(new Token(30, LexemaAuxiliar, numeroLinea, "Palabra Rervada", "", Color.Green));
                            pintar(richTextBox, LexemaAuxiliar, i, Color.Green);
                            LexemaAuxiliar = "";
                            estadoAutomataCadenas = 0;
                            i -= 1;
                        }
                        else if ("true" == LexemaAuxiliar || "false" == LexemaAuxiliar)
                        {
                            listaToken.agregarListaTokenResultado(new Token(31, LexemaAuxiliar, numeroLinea, "Boolean", "", Color.Orange));
                            pintar(richTextBox, LexemaAuxiliar, i, Color.Orange);
                            LexemaAuxiliar = "";
                            estadoAutomataCadenas = 0;
                            i -= 1;
                        }
                        else
                        {
                            //listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Indentificador", "", Color.Magenta));
                           // pintar(richTextBox, LexemaAuxiliar, i, Color.Magenta);
                            LexemaAuxiliar = "";
                            estadoAutomataCadenas = 0;
                            i -= 1;
                        }



                    }
                    break;
                case 3:
                    if (Char.IsLetter(c))
                    {
                        estadoAutomataCadenas = 3;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('_') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(15, c.ToString(), numeroLinea, "No se puede coloar dos guiones seguido ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataCadenas = 0;

                    }
                    else if (c.CompareTo(' ') == 0)
                    {
                        estadoAutomataCadenas = 4;

                    }
                    else
                    {

                        if (reservedWord.Contains(LexemaAuxiliar))
                        {
                            listaToken.agregarListaTokenResultado(new Token(32, LexemaAuxiliar, numeroLinea, "Palabra Rervada", "", Color.Green));
                            pintar(richTextBox, LexemaAuxiliar, i, Color.Green);
                            LexemaAuxiliar = "";
                            estadoAutomataCadenas = 0;
                            i -= 1;
                        }
                        else if ("true" == LexemaAuxiliar || "false" == LexemaAuxiliar)
                        {
                            listaToken.agregarListaTokenResultado(new Token(33, LexemaAuxiliar, numeroLinea, "Boolean", "", Color.Orange));
                            pintar(richTextBox, LexemaAuxiliar, i, Color.Orange);
                            LexemaAuxiliar = "";
                            estadoAutomataCadenas = 0;
                            i -= 1;
                        }
                        else
                        {
                            //listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Indentificador", "", Color.Magenta));
                            //pintar(richTextBox, LexemaAuxiliar, i, Color.Magenta);
                            LexemaAuxiliar = "";
                            estadoAutomataCadenas = 0;
                            i -= 1;
                        }


                    }
                    break;
                case 4:
                    if (reservedWord.Contains(LexemaAuxiliar))
                    {
                        listaToken.agregarListaTokenResultado(new Token(34, LexemaAuxiliar, numeroLinea, "Palabra Rervada", "", Color.Green));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Green);
                        LexemaAuxiliar = "";
                        estadoAutomataCadenas = 0;
                        i -= 1;
                    }
                    else if ("true" == LexemaAuxiliar || "false" == LexemaAuxiliar)
                    {
                        listaToken.agregarListaTokenResultado(new Token(35, LexemaAuxiliar, numeroLinea, "Boolean", "", Color.Orange));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Orange);
                        LexemaAuxiliar = "";
                        estadoAutomataCadenas = 0;
                        i -= 1;
                    }
  
                    else
                    {
                      //  listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Indentificador", "", Color.Magenta));
                        //pintar(richTextBox, LexemaAuxiliar, i, Color.Magenta);
                        LexemaAuxiliar = "";
                        estadoAutomataCadenas = 0;
                        i -= 1;
                    }
                    break;
                #endregion

            }

        }

        int estadoAutomataComentario=0;
        public void automataComentario(RichTextBox richTextBox, Char c, int i, int numeroLinea)
        {
            switch (estadoAutomataComentario)
            {
 
                case 0:
                    if (c.CompareTo('/') == 0)
                    {
                        estadoAutomataComentario = 1;
                        LexemaAuxiliar += c;

                    }
                    else
                    {
                        if (c.CompareTo('#') == 0 && i == textoEntrada.Length - 1)
                        {
                            Console.WriteLine("fin de la cadena");
                        }
                        else
                        {
                            Console.WriteLine("error --- " + c);
                            estadoAutomataComentario = 0;
                        }
                    }

                    break;
                case 1:

                    if (Char.IsDigit(c))
                    {
                        estadoAutomataComentario = 0;

                    }

                    else if (c.CompareTo('/') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataComentario = 2;

                    }
                    else if (c.CompareTo('*') == 0)
                    {
                        LexemaAuxiliar += c;
                        estadoAutomataComentario = 3;

                    }
                    else
                    {
                        listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Operador Aritmetico", "Divicion", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataComentario = 0;
                    }
                    break;

                case 2:
                    if (Char.IsLetter(c) || Char.IsDigit(c) || c.CompareTo(' ') == 0)
                    {
                        estadoAutomataComentario = 2;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('\n') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(123, LexemaAuxiliar, numeroLinea, "Comentario", "Corto", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.Red);
                        LexemaAuxiliar = "";
                        estadoAutomataComentario = 0;

                    }
                    else
                    {
                        listaToken.agregarListaTokenErrores(new Token(123, c.ToString(), numeroLinea, "Se cierre de cometario ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataComentario = 0;
                    }
                    break;

                case 3:
                    if (Char.IsLetter(c) || Char.IsDigit(c) || c.CompareTo(' ') == 0 || c.CompareTo('\n') == 0)
                    {
                        estadoAutomataComentario = 3;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('*') == 0)
                    {
                        estadoAutomataComentario = 4;
                        LexemaAuxiliar += c;

                    }
                    else
                    {
                        listaToken.agregarListaTokenErrores(new Token(123, c.ToString(), numeroLinea, "Se cierre de cometario ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataComentario = 0;
                    }

                    break;
                case 4:
                    if (Char.IsLetter(c) || Char.IsDigit(c) || c.CompareTo(' ') == 0)
                    {
                        estadoAutomataComentario = 2;
                        LexemaAuxiliar += c;
                    }
                    else if (c.CompareTo('/') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(123, LexemaAuxiliar, numeroLinea, "Comentario", "Largo", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.Red);
                        LexemaAuxiliar = "";
                        estadoAutomataComentario = 0;

                    }
                    else
                    {

                        listaToken.agregarListaTokenErrores(new Token(123, LexemaAuxiliar, numeroLinea, "Se esperaba cierre de cometario ", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataComentario = 0;
                    }
                    break;


            }
        }

        public void setNumeroErrores(int numeroErrores) { this.numeroErrores = numeroErrores; }
        public void setNumeroToken(int numeroToken) { this.numeroToken = numeroToken; }
        public int getNumeroErrores() { return numeroErrores; }
        public int getNumeroToken() { return numeroToken; }

    }
}
