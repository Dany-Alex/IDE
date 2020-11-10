using IDE.Backend.SegundaFase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDE
{
    class Analizador
    {
        ListaToken listaToken = new ListaToken();

        AnalizadorLL1 AnalizadorLL1;

        String textoEntrada;

        int numeroErrores, numeroToken;
        public void analizar(RichTextBox richTextBox,RichTextBox richTextBox2, DataGridView resultados, DataGridView errores)
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

               
                automataPalabrasReservadas(richTextBox, c, i, numeroLinea);

                automataNumeros(richTextBox, c, i, numeroLinea);
                automataVariables(richTextBox, c, i, numeroLinea);
                automataOperadores(richTextBox, c, i, numeroLinea);
                automataCadenas(richTextBox, c, i, numeroLinea);
                automataChar(richTextBox, c, i, numeroLinea);
                // automataComentario(richTextBox, c, i, numeroLinea);

            }

            resultados.DataSource = (listaToken.getListaTokenResultado());
            setNumeroToken(resultados.RowCount);
            resultados.AutoResizeColumn(3);
            resultados.AutoResizeColumn(4);
            errores.DataSource = (listaToken.getListaTokenErrores());
            setNumeroErrores(errores.RowCount);

           
            AnalizadorLL1 = new AnalizadorLL1("E", listaToken.getListaTokenResultado(), listaToken.getListaTokenErrores());

            AnalizadorLL1.iniciarGramatica(richTextBox2);
            setCodigoDot( AnalizadorLL1.getCodigoDot());
            Console.WriteLine( "Analizador ll1");

            // while (AnalizadorLL1.analizar()) { Console.WriteLine("its ok"); }
            AnalizadorLL1.analizar();
        }

        string codigoDot = null;
        public void setCodigoDot(string codigoDot) { this.codigoDot = codigoDot; }
        public string getCodigoDot() { return this.codigoDot; }

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
        private String LexemaAuxiliar, LexemaAuxiliarPalabrasReservadas, LexemaAuxiliarVariables, LexemaAuxiliarCadenas, LexemaAuxiliarChar;
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
                        listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Entero", "Entero", Color.Purple));
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
                        
                        listaToken.agregarListaTokenErrores(new Token(2, c.ToString(), numeroLinea, "Error", "Se esperaba mas numeros", Color.Yellow));
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
                        listaToken.agregarListaTokenErrores(new Token(3, c.ToString(), numeroLinea, "Error", "No se puede coloar dos puntos un un doble", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                    }
                    else
                    {
                        listaToken.agregarListaTokenResultado(new Token(4, LexemaAuxiliar, numeroLinea, "Decimal", "Decimal", Color.Cyan));
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

                        listaToken.agregarListaTokenResultado(new Token(5, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Resta", Color.DarkBlue));
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

                        listaToken.agregarListaTokenErrores(new Token(6, c.ToString(), numeroLinea, "Error", "No se pueden colocar mas de dos guiones seguidos", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(7, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Decremento", Color.DarkBlue));
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

                        listaToken.agregarListaTokenResultado(new Token(8, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Suma", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;

                    }
                    break;

                case 7:
                    listaToken.agregarListaTokenResultado(new Token(9, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Incremento", Color.DarkBlue));
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
                        listaToken.agregarListaTokenResultado(new Token(10, LexemaAuxiliar, numeroLinea, "Operador Artimetico", " Multiplicacion", Color.DarkBlue));
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
                        listaToken.agregarListaTokenResultado(new Token(11, LexemaAuxiliar, numeroLinea, "Finalizador de linea", "Punto y coma", Color.Pink));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.Magenta);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else if (c.CompareTo('(') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(12, LexemaAuxiliar, numeroLinea, "Signos de agrupación", "Parentesis que abre", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else if (c.CompareTo(')') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(13, LexemaAuxiliar, numeroLinea, "Signos de agrupación", "Parentesis que cierra", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else if (c.CompareTo('{') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(14, LexemaAuxiliar, numeroLinea, "Signos de agrupación", "Llave que abre", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else if (c.CompareTo('}') == 0)
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(15, LexemaAuxiliar, numeroLinea, "Signos de agrupación", "Llave que cierra", Color.DarkBlue));
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
                        listaToken.agregarListaTokenResultado(new Token(9, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Incremento", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;

                    }
                    else
                    {

                        listaToken.agregarListaTokenResultado(new Token(8, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Suma", Color.DarkBlue));
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

                        listaToken.agregarListaTokenResultado(new Token(5, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Resta", Color.DarkBlue));
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

                        listaToken.agregarListaTokenErrores(new Token(16, c.ToString(), numeroLinea, "Error", "No se pueden colocar mas de dos guiones seguidos", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(7, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Decremento", Color.DarkBlue));
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

                        listaToken.agregarListaTokenResultado(new Token(17, LexemaAuxiliar, numeroLinea, "Operadores relacionales", "Menor", Color.DarkBlue));
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

                        listaToken.agregarListaTokenErrores(new Token(18, c.ToString(), numeroLinea, "Error", "No se pueden colocar tres singos seguidos", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(19, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Menor Igual", Color.DarkBlue));
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

                        listaToken.agregarListaTokenResultado(new Token(20, LexemaAuxiliar, numeroLinea, "Operadores relacionales", "Mayor", Color.DarkBlue));
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

                        listaToken.agregarListaTokenErrores(new Token(21, c.ToString(), numeroLinea, "Error", "No se pueden colocar tres singos seguidos", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(22, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Mayor Igual", Color.DarkBlue));
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

                        listaToken.agregarListaTokenResultado(new Token(23, LexemaAuxiliar, numeroLinea, "Operador Relacional", "Igual", Color.DarkBlue));
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

                        listaToken.agregarListaTokenErrores(new Token(24, c.ToString(), numeroLinea, "Error", "No se pueden colocar mas de dos iguales seguidos", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(25, LexemaAuxiliar, numeroLinea, "Operador Relacional", "Comparacion", Color.DarkBlue));
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

                        listaToken.agregarListaTokenResultado(new Token(26, LexemaAuxiliar, numeroLinea, "Operadores Logico", "Negacion", Color.DarkBlue));
                        pintar(richTextBox, LexemaAuxiliar, i , Color.DarkBlue);
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

                        listaToken.agregarListaTokenErrores(new Token(27, c.ToString(), numeroLinea, "No se pueden colocar tres singos seguidos", "", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataNumeros = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(28, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "Diferente", Color.DarkBlue));
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
                        listaToken.agregarListaTokenErrores(new Token(29, LexemaAuxiliar, numeroLinea, "Error", "Se esperaba otro &", Color.Yellow));
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

                        listaToken.agregarListaTokenErrores(new Token(30, c.ToString(), numeroLinea, "Error", "No se pueden colocar tres singos seguidos", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(31, LexemaAuxiliar, numeroLinea, "Operador Logico", "Y", Color.DarkBlue));
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
                        listaToken.agregarListaTokenErrores(new Token(32, LexemaAuxiliar, numeroLinea, "Error", "Se esperaba otro |", Color.Yellow));
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

                        listaToken.agregarListaTokenErrores(new Token(33, c.ToString(), numeroLinea, "Error", "No se pueden colocar tres singos seguidos", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    else
                    {
                        LexemaAuxiliar += c;
                        listaToken.agregarListaTokenResultado(new Token(34, LexemaAuxiliar, numeroLinea, "Operador Logico", "O", Color.DarkBlue));
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
                        listaToken.agregarListaTokenResultado(new Token(35, LexemaAuxiliar, numeroLinea, "Operador Aritmetico", "Divicion", Color.DarkBlue));
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
                        listaToken.agregarListaTokenResultado(new Token(36, LexemaAuxiliar, numeroLinea, "Comentario", "Corto", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.Red);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;

                    }
                    else
                    {
                        listaToken.agregarListaTokenErrores(new Token(37, c.ToString(), numeroLinea, "Error", "Se espera cierre de cometario", Color.Yellow));
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
                        listaToken.agregarListaTokenErrores(new Token(38, c.ToString(), numeroLinea, "Error", "Se espera cierre de cometario", Color.Yellow));
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
                        listaToken.agregarListaTokenResultado(new Token(39, LexemaAuxiliar, numeroLinea, "Comentario", "Largo", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i + 1, Color.Red);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;

                    }
                    else
                    {

                        listaToken.agregarListaTokenErrores(new Token(40, LexemaAuxiliar, numeroLinea, "Error", "Se espera cierre de cometario", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                        LexemaAuxiliar = "";
                        estadoAutomataOperadores = 0;
                    }
                    break;

                #endregion

            }

        }

   
        private String[] reservedWord = new String[] { "SI", "SINO", "SINO_SI", "MIENTRAS", "HACER", "DESDE", "HASTA", "INCREMENTO", "ENTONCES" };

     private int estadoAutomataCadenas;
        public void automataCadenas(RichTextBox richTextBox, Char c, int i, int numeroLinea)
        {

            switch (estadoAutomataCadenas)
            {
                case 0:
                    if (c.CompareTo('\"') == 0)
                    {
                        estadoAutomataCadenas = 1;
                        LexemaAuxiliarCadenas += c;
                        

                    }
                    else
                    {
                        if (c.CompareTo('#') == 0 && i == textoEntrada.Length - 1)
                        {
                            Console.WriteLine("fin de la cadena");
                        }
                        else
                        {
                            estadoAutomataCadenas = 0;
                        }
                    }

                    break;

                #region Cadena
                case 1:

                    if (Char.IsLetter(c) || Char.IsDigit(c))
                    {
                        estadoAutomataCadenas = 1;
                        LexemaAuxiliarCadenas += c;
                    }
                    else if (c.CompareTo('\"') == 0)
                    {
                        LexemaAuxiliarCadenas += c;
                        listaToken.agregarListaTokenResultado(new Token(41, LexemaAuxiliarCadenas, numeroLinea, "Cadena", "Cadena", Color.Gray));
                        pintar(richTextBox, LexemaAuxiliarCadenas, i +1, Color.Gray);
                        LexemaAuxiliarCadenas = "";
                        estadoAutomataCadenas = 0;

                    }
                    else
                    {
                        listaToken.agregarListaTokenErrores(new Token(42, c.ToString(), numeroLinea, "Error", "Se  espera cierre de cadena", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliarCadenas, i, Color.Yellow);
                        LexemaAuxiliarCadenas = "";
                        estadoAutomataCadenas = 0;
                    }
                    break;
                #endregion



            }

        }


        private int estadoAutomataChar;
        public void automataChar(RichTextBox richTextBox, Char c, int i, int numeroLinea)
        {

            switch (estadoAutomataChar)
            {
                case 0:
                    if (c.CompareTo('\'') == 0)
                    {
                        estadoAutomataChar = 1;
                        LexemaAuxiliarChar += c;


                    }
                    else
                    {
                        if (c.CompareTo('#') == 0 && i == textoEntrada.Length - 1)
                        {
                            Console.WriteLine("fin de la cadena");
                        }
                        else
                        {
                            estadoAutomataChar = 0;
                        }
                    }

                    break;

                #region char
                case 1:

                    if (Char.IsLetter(c) || Char.IsDigit(c))
                    {
                        estadoAutomataChar = 2;
                        LexemaAuxiliarChar += c;
                    }
                    else
                    {
                        listaToken.agregarListaTokenErrores(new Token(43, c.ToString(), numeroLinea, "Error", "Se espera cierre de caracter", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliarChar, i, Color.Yellow);
                        LexemaAuxiliarChar = "";
                        estadoAutomataChar = 0;
                    }
                    break;
                case 2:
                    if (c.CompareTo('\'') == 0)
                    {
                        LexemaAuxiliarChar += c;
                        listaToken.agregarListaTokenResultado(new Token(44, LexemaAuxiliarChar, numeroLinea, "Caracter", "Caracter", Color.Brown));
                        pintar(richTextBox, LexemaAuxiliarChar, i + 1, Color.Brown);
                        LexemaAuxiliarChar = "";
                        estadoAutomataChar = 0;

                    }
                    else
                    {

                        LexemaAuxiliarChar = "";
                        estadoAutomataChar = 0;
                    }
                    break;
                    #endregion



            }

        }


        private int estadoAutomataVariables = 0;
        public void automataVariables(RichTextBox richTextBox, Char c, int i, int numeroLinea)
        {
            switch (estadoAutomataVariables)
            {
                // \u0020 es espacio en blanco
                case 0:
                    if (c.CompareTo('\u0020') == 0|| c.CompareTo('\t') == 0)
                    {
                        estadoAutomataVariables = 1;

                    }
                    else
                    {
                        if (c.CompareTo('#') == 0 && i == textoEntrada.Length - 1)
                        {
                            Console.WriteLine("fin de la cadena");
                        }
                        else
                        {
                            estadoAutomataVariables = 0;
                        }
                    }

                    break;
                // \u005F es un guion bajo
                case 1:
                    if (c.CompareTo('\u005F') == 0)
                    {
                        estadoAutomataVariables = 2;
                        LexemaAuxiliarVariables += c;

                    }
                    else
                    { estadoAutomataVariables = 0; }

                    break;

                case 2:
                       if (Char.IsDigit(c))
                    {
                        LexemaAuxiliarVariables += c;
                        estadoAutomataVariables = 3;

                    }
                    else if (Char.IsLetter(c))
                    {

                        LexemaAuxiliarVariables += c;
                        estadoAutomataVariables = 3;

                    }
                    else
                    {

                        listaToken.agregarListaTokenErrores(new Token(45, LexemaAuxiliarVariables, numeroLinea, "Error", "Se espera un nombre para la variable", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliarVariables, i, Color.Yellow);
                        LexemaAuxiliarVariables = "";
                        estadoAutomataVariables = 0;

                    }
                    break;

                case 3:
                 if (Char.IsLetter(c) || Char.IsDigit(c) )
                    {
                        estadoAutomataVariables = 3;
                        LexemaAuxiliarVariables += c;
                    }
                    else
                    {

                            listaToken.agregarListaTokenResultado(new Token(46, LexemaAuxiliarVariables, numeroLinea, "Variable", " ", Color.GreenYellow));
                            pintar(richTextBox, LexemaAuxiliarVariables, i, Color.GreenYellow);
                        LexemaAuxiliarVariables = "";
                            estadoAutomataVariables = 0;
                    }
                    break;
            }
        }

        private int estadoAutomataPalabrasReresrvadas=0;
        public void automataPalabrasReservadas(RichTextBox richTextBox, Char c, int i, int numeroLinea)
        {

            switch (estadoAutomataPalabrasReresrvadas)
            {
                case 0:
                    if (Char.IsLetter(c))
                    {
                        estadoAutomataPalabrasReresrvadas = 1;
                        LexemaAuxiliarPalabrasReservadas += c;
                    }
                    else if (c.CompareTo(' ') == 0)
                    {
                       estadoAutomataPalabrasReresrvadas = 1;

                    }

                    else
                    {
                        if (c.CompareTo('#') == 0 && i == textoEntrada.Length - 1)
                        {
                            Console.WriteLine("fin de la cadena");
                        }
                        else
                        {
                            estadoAutomataPalabrasReresrvadas = 0;
                        }
                    }

                    break;

                #region palabra reservada 2
                case 1:
                    if (Char.IsLetter(c))
                    {
                        estadoAutomataPalabrasReresrvadas = 1;
                        LexemaAuxiliarPalabrasReservadas += c;
                    }
                    else if (c.CompareTo('_') == 0)
                    {
                        estadoAutomataPalabrasReresrvadas = 2;
                        LexemaAuxiliarPalabrasReservadas += c;
                    }

                    else if (c.CompareTo(' ') == 0)
                    {
                        estadoAutomataPalabrasReresrvadas = 3;

                    }
                    else
                    {
                        verificarPalabraReservada(richTextBox, i, numeroLinea);

                    }
                    break;
                case 2:
                    if (Char.IsLetter(c))
                    {
                        estadoAutomataPalabrasReresrvadas = 2;
                        LexemaAuxiliarPalabrasReservadas += c;
                    }
                    else if (c.CompareTo('_') == 0)
                    {

                        listaToken.agregarListaTokenErrores(new Token(47, c.ToString(), numeroLinea, "Error", "No se puede coloar dos guiones seguido", Color.Yellow));
                        pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Yellow);
                        LexemaAuxiliarPalabrasReservadas = "";
                        estadoAutomataPalabrasReresrvadas = 0;

                    }
                    else if (c.CompareTo(' ') == 0)
                    {
                        estadoAutomataPalabrasReresrvadas = 3;

                    }
                    else
                    {

                        verificarPalabraReservada(richTextBox, i, numeroLinea);

                    }
                    break;
                case 3:
                    verificarPalabraReservada(richTextBox, i, numeroLinea);
                    break;
                    #endregion
            }

        }

        public void verificarPalabraReservada(RichTextBox richTextBox,int i, int numeroLinea) {

            if ("verdadero" == LexemaAuxiliarPalabrasReservadas || "falso" == LexemaAuxiliarPalabrasReservadas)
            {
                listaToken.agregarListaTokenResultado(new Token(48, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Boolean", LexemaAuxiliarPalabrasReservadas, Color.Orange));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Orange);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("SI" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(49, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "SI", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("SINO" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(50, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "SINO", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("SINO_SI" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(51, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "SINO_SI", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("MIENTRAS" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(52, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "MIENTRAS", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("HACER" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(53, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "HACER", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("DESDE" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(54, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "DESDE", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("HASTA" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(55, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "HASTA", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("INCREMENTO" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(56, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "INCREMENTO", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("entero" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(57, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "entero", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("cadena" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(58, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "cadena", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("decimal" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(59, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "decimal", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("caracter" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(60, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "caracter", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("boolean" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(61, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "boolean", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("principal" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(62, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "principal", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("leer" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(63, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "leer", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else if ("escribir" == (LexemaAuxiliarPalabrasReservadas))
            {
                listaToken.agregarListaTokenResultado(new Token(64, LexemaAuxiliarPalabrasReservadas, numeroLinea, "Palabra Rervada", "escribir", Color.Green));
                pintar(richTextBox, LexemaAuxiliarPalabrasReservadas, i, Color.Green);
                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
            else
            {

                LexemaAuxiliarPalabrasReservadas = "";
                estadoAutomataPalabrasReresrvadas = 0;
                i -= 1;
            }
        }



        public Boolean verificarVariableRepetida(String lexema) {
            foreach (var token in listaToken.getListaTokenResultado()) {
                if (token.Simbolo==lexema)
                {
                    return true;
                }

            }
            return false;

        }

        int estadoAutomataComentario =0;
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
