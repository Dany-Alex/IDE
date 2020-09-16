using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDE
{
    class Analizador
    {
        ListaToken listaToken = new ListaToken();

        private int estado;
        private String LexemaAuxiliar;
        String textoEntrada;
        int numeroLinea;
        public void analizar(RichTextBox richTextBox, DataGridView dataGridView)
        {
            textoEntrada = richTextBox.Text;
            textoEntrada = textoEntrada + "#";

            estado = 0;
            LexemaAuxiliar = "";
            Char c;
            List<char> operadoresAritmenticos = new List<char>() { '+', '-', '*', '/' };

            for (int i = 0; i <= textoEntrada.Length - 1; i++)
            {
                c = textoEntrada.ElementAt(i);
                if (c == ' ' || c == '\n' || c == '\t') { numeroLinea++; }
                switch (estado)
                {
                    case 0:
                        if (Char.IsDigit(c))
                        {
                            estado = 1;
                            LexemaAuxiliar += c;
                        }
                        else if (operadoresAritmenticos.Contains(c))
                        {
                            LexemaAuxiliar += c;

                            listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Operador Artimetico", "", Color.Green));
                            pintar(richTextBox, LexemaAuxiliar, i + 1, Color.DarkBlue);
                            resetEstado();
                        }
                        /* else if (c.CompareTo('-') == 0)
                         {
                             LexemaAuxiliar += c;
                             listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Operador", "", Color.Green));
                             pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                             resetEstado();
                         }
                         else if (c.CompareTo('*') == 0)
                         {
                             LexemaAuxiliar += c;
                             listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Operador", "", Color.Green));
                             pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                             resetEstado();
                         }
                         else if (c.CompareTo('/') == 0)
                         {
                             LexemaAuxiliar += c;
                             listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Operador", "", Color.Green));
                             pintar(richTextBox, LexemaAuxiliar, i, Color.DarkBlue);
                             resetEstado();
                         }*/
                        else
                        {
                            if (c.CompareTo('#') == 0 && i == textoEntrada.Length - 1)
                            {
                                Console.WriteLine("fin de la cadena");
                            }
                            else
                            {
                                Console.WriteLine("error --- " + c);
                                estado = 0;
                            }
                        }

                        break;
                    case 1:
                        if (Char.IsDigit(c))
                        {
                            estado = 1;
                            LexemaAuxiliar += c;
                        }
                        else if (c.CompareTo('.') == 0)
                        {
                            estado = 2;
                            LexemaAuxiliar += c;
                        }
                        else
                        {
                            listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, numeroLinea, "Entero", "", Color.Green));
                            pintar(richTextBox, LexemaAuxiliar, i, Color.Purple);
                            resetEstado();
                            i -= 1;
                        }
                        break;
                    case 2:

                        if (Char.IsDigit(c))
                        {
                            estado = 3;
                            LexemaAuxiliar += c;
                        }
                        else
                        {
                            Console.WriteLine("error --- " + c + " despues de un puento se esperaban mas numeros");
                            listaToken.agregarListaTokenResultado(new Token(123, c.ToString(), numeroLinea, "Se esperaba mas numeros ", "", Color.Yellow));
                            pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                            estado = 0;
                        }
                        break;
                    case 3:
                        if (Char.IsDigit(c))
                        {
                            estado = 3;
                            LexemaAuxiliar += c;
                        }
                        else if (c.CompareTo('.') == 0)
                        {
                            Console.WriteLine("error --- " + c + " despues de un puento se esperaban mas numeros");
                            listaToken.agregarListaTokenResultado(new Token(123, c.ToString(), numeroLinea, "No se puede coloar dos puntos un un doble ", "", Color.Yellow));
                            pintar(richTextBox, LexemaAuxiliar, i, Color.Yellow);
                            estado = 0;
                        }
                        else
                        {
                            listaToken.agregarListaTokenResultado(new Token(1, LexemaAuxiliar, i, "Decimal", "", Color.Green));
                            pintar(richTextBox, LexemaAuxiliar, i, Color.Cyan);
                            resetEstado();
                            i -= 1;
                        }
                        break;

                    default:
                        break;

                }
            }
            dataGridView.DataSource = (listaToken.getListaTokenResultado());
        }

        public void resetEstado()
        {
            LexemaAuxiliar = "";
            estado = 0;
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


    }
}
