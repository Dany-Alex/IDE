using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDE.Backend.SegundaFase
{
    public enum analizarTipoNodo
    {
        /// <summary>
        /// estado inicial
        /// </summary>
        inicial = 0,
        /// <summary>
        /// analiza si es un no terminal
        /// </summary>
        noTerminal = 1,
        /// <summary>
        /// analiza si es un no final no terminal
        /// </summary>
        finNoTerminal = 2,
        /// <summary>
        /// analiza si es un terminal
        /// </summary>
        terminal = 3,
        /// <summary>
        /// analiza si es un error
        /// </summary>
        error = 4,
        /// <summary>
        /// final de docuemnto
        /// </summary>
        finDocumento = 5
    }
    class AnalizadorLL1
    {
        GramaticaLibreContexto GramaticaLibreContexto;
        string simboloInicial1;
        GLC GLC = new GLC();



        private List<Token> listaTokenResultado = new List<Token>();
        IDictionary<string, IDictionary<string, GLC_Regla>> tablaAnalisis;

        private List<string> listaNoTerminales = new List<string>();
        Stack<string> pila;

        public analizarTipoNodo tipoNodo
        {
            get
            {

                if (pila.Count > 0)
                {
                    var s = pila.Peek();
                    if (s.StartsWith("$"))
                        return analizarTipoNodo.finNoTerminal;
                    if (s == simboloActual)
                        return analizarTipoNodo.terminal;
                    return analizarTipoNodo.noTerminal;
                }
                try
                {
                    if (pila.Count() == 0)
                        return analizarTipoNodo.finDocumento;
                }
                catch { }
                return analizarTipoNodo.inicial;
            }
        }

        public Stack<string> getPila()
        {
            return this.pila;
        }

        public void gramaticaEnCodigo()
        {
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("E", "principal", "(", ")", "BLOQUE_CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("BLOQUE_CODIGO", "{", "CODIGO", "}"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "VARIABLE_ASIGNADA", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_SI", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_MIENTRAS", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_HACER_MIENTRAS", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_DESDE", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_LEER_ESCRIBIR", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO"));


            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("VARIABLE_ASIGNADA", "TIPO_VARIABLE", "Variable", "ASIGNAR", ";"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNAR", "=", "TIPO_DATO", "ASIGNAR"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNAR", ",", "Variable", "ASIGNAR"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNAR"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "57Entero"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "58Cadena"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "61Boolean"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "60Caracter"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "59Decimal"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "1Entero"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "41Cadena"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "48Boolean"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "44Caracter"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "4Decimal"));


            #region estructura Leer escribir

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_LEER_ESCRIBIR", "TIPO_LEER_ESCRIBIR", "(", ")", "BLOQUE_CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_LEER_ESCRIBIR", "leer"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_LEER_ESCRIBIR", "escribir"));

            #endregion

            #region estructura SI

            //SI(ASIGNACION_VARIABLE_ESTRUCTURA_SI){lineas de codigo}
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SI", "SI", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "BLOQUE_CODIGO", "ESTRUCTURA_SINO_SI", "ESTRUCTURA_SINO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SINO_SI", "SINO_SI", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "BLOQUE_CODIGO", "ESTRUCTURA_SINO_SI", "ESTRUCTURA_SINO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SINO_SI"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SINO", "SINO", "BLOQUE_CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SINO"));
            #endregion


            #region estructura MIENTRAS

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_MIENTRAS", "MIENTRAS", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "BLOQUE_CODIGO"));

            #endregion

            #region estructura HACER_MIENTRAS

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_HACER_MIENTRAS", "HACER", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "BLOQUE_CODIGO", "MIENTRAS", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION"));

            #endregion

            #region estructura HACER_MIENTRAS

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_DESDE", "DESDE", "SENTENCIA_LOGICA", ";", "HASTA", "SENTENCIA_LOGICA", "INCREMENTO", "Identificador", "BLOQUE_CODIGO"));

            #endregion

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "(", "SENTENCIA_LOGICA", ")"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("SENTENCIA_LOGICA", "TIPO_DATO", "CONECTOR_LOGICO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("SENTENCIA_LOGICA", "TIPO_VARIABLE", "CONECTOR_LOGICO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("SENTENCIA_LOGICA", "NEGAR", "TIPO_DATO"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CONECTOR_LOGICO", "LOGICO", "TIPO_DATO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CONECTOR_LOGICO"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("NEGAR", "!"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("NEGAR"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "=="));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "!="));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", ">="));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "<="));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", ">"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "<"));
        }

        public void iniciarGramatica(RichTextBox richTextBox)
        {
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("E", "62", "12", "13", "BLOQUE_CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("BLOQUE_CODIGO", "14", "CODIGO", "15"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "VARIABLE_ASIGNADA", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_SI", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_MIENTRAS", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_HACER_MIENTRAS", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_DESDE", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "ESTRUCTURA_LEER_ESCRIBIR", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO"));


            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("VARIABLE_ASIGNADA", "TIPO_VARIABLE", "46", "ASIGNAR", "11"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNAR", "23", "TIPO_DATO", "ASIGNAR"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNAR", ",", "46", "ASIGNAR"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNAR"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "57"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "58"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "61"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "60"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "59"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "1"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "41"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "48"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "44"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_DATO", "4"));


            #region estructura Leer escribir

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_LEER_ESCRIBIR", "TIPO_LEER_ESCRIBIR", "12", "13", "BLOQUE_CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_LEER_ESCRIBIR", "63"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_LEER_ESCRIBIR", "64"));

            #endregion

            #region estructura SI

            //SI(ASIGNACION_VARIABLE_ESTRUCTURA_SI){lineas de codigo}
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SI", "49", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "BLOQUE_CODIGO", "ESTRUCTURA_SINO_SI", "ESTRUCTURA_SINO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SINO_SI", "51", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "BLOQUE_CODIGO", "ESTRUCTURA_SINO_SI", "ESTRUCTURA_SINO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SINO_SI"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SINO", "50", "BLOQUE_CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SINO"));
            #endregion


            #region estructura MIENTRAS

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_MIENTRAS", "52", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "BLOQUE_CODIGO"));

            #endregion

            #region estructura HACER_MIENTRAS

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_HACER_MIENTRAS", "53", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "BLOQUE_CODIGO", "52", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION"));

            #endregion

            #region estructura HACER_MIENTRAS

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_DESDE", "54", "SENTENCIA_LOGICA", "11", "55", "SENTENCIA_LOGICA", "11", "56", "1", "BLOQUE_CODIGO"));

            #endregion

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "12", "SENTENCIA_LOGICA", "13"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("SENTENCIA_LOGICA", "TIPO_DATO", "CONECTOR_LOGICO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("SENTENCIA_LOGICA", "TIPO_VARIABLE", "CONECTOR_LOGICO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("SENTENCIA_LOGICA", "NEGAR", "TIPO_DATO"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CONECTOR_LOGICO", "LOGICO", "TIPO_DATO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CONECTOR_LOGICO"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("NEGAR", "26"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("NEGAR"));

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "17"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "19"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "20"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "21"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "25"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "28"));


            Console.WriteLine(" paso 1 - Gramatica Libre Contexto");
            Console.WriteLine(GLC);
            richTextBox.Text = GLC.ToString();
            tablaAnalisis = GLC.tablaAnalizar();
            setCodigoDot(ToDotGraph(GLC.listaGramaticaLibreContextoRegla));
            richTextBox.AppendText(this.codigoDot);

            Console.WriteLine();

        }

        string codigoDot = null;
        public void setCodigoDot(string codigoDot) { this.codigoDot = codigoDot; }
        public string getCodigoDot() { return this.codigoDot; }

        public AnalizadorLL1(string simboloInicial1,
                            List<Token> listaTokenResultado,
                            List<Token> listaTokenErrores)
        {
            GramaticaLibreContexto = new GramaticaLibreContexto();

            this.simboloInicial1 = simboloInicial1;
            this.listaTokenResultado = listaTokenResultado;
            this.pila = new Stack<string>();
        }

        String imprimir = null;
        public void imprimirPila()
        {

            /* while (analizar())
             {
                 imprimir=string.Format("{0}\t", simbolo);
             }
            */
            foreach (string number in pila)
            {
                Console.WriteLine(number);
            }
            Console.WriteLine("{0} {1} {0}", "----------------------------", "Fin de la impresion de pila");
        }

        int inicial = 62;
        string simboloActual;
        string s = null;

        int tokenActual = 0, idActual = 0;
        /// <summary>
        /// se encarga del analisis sintactico por medio de los tokes que fueron encontrados por el analizador lexico, asi como tambien en este metodo se trabaja con el autamata de pila
        /// </summary>
        /// <returns></returns>
        public bool analizar()
        {

            bool realizado = false;


            if (listaTokenResultado.Count() > 0)
            {
                simboloActual = listaTokenResultado[tokenActual].Simbolo;
                idActual = listaTokenResultado[tokenActual].Id;
            }

            if (inicial == idActual && tokenActual == 0)
            {
                pila.Push("$");
                pila.Push(simboloInicial1);
                Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);
                imprimirPila();
                realizado = true;
                inicial = -1;

            }


            while (pila.Count>0)
            {
                simboloActual = listaTokenResultado[tokenActual].Simbolo;
                idActual = listaTokenResultado[tokenActual].Id;

                if (0 < pila.Count)
                {
                    var verPila = pila.Peek();

                    if (verPila.StartsWith("$ "))
                    {
                        Console.WriteLine("Hice pop de $: " + pila.Pop());
                        realizado = true;
                    }

                    //terminal
                    if (verPila == idActual.ToString())
                    {
                        Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);
                        Console.WriteLine("Hice pop de terminal: " + pila.Pop());
                        imprimirPila();

                        tokenActual++;

                        Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);
                        imprimirPila();
                        realizado = true;
                    }

                    // no terminal

                    IDictionary<string, GLC_Regla> d;
                    if (tablaAnalisis.TryGetValue(verPila, out d))
                    {
                        GLC_Regla rule;
                        if (d.TryGetValue(idActual.ToString(), out rule))
                        {
                            Console.WriteLine("Hice pop no terminal de:" + pila.Pop());
                            imprimirPila();
                            // mete el marcador final no terminal para más tarde
                            //pila.Push(string.Concat("$", verPila));

                            // mete la derivacion en orden inverso
                            var ic = rule.derecha.Count;
                            for (var i = ic - 1; 0 <= i; --i)
                            {
                                verPila = rule.derecha[i];
                                pila.Push(verPila);
                                imprimirPila();
                            }
                            //   realizado = true;
                        }
                        realizado = true;
                    }


                }
                // if (!simboloActual.Equals(pila.Peek())) {  Console.WriteLine("Hice pop de While if: " + pila.Pop()); }

            }



            return realizado;

        }

        public void provicional(string verPila)
        {
            for (int k = 0; k < GLC.listaGramaticaLibreContextoRegla.Count - 1; k++)
            {



                if (GLC.listaGramaticaLibreContextoRegla[k].izquierda == (verPila))
                {


                    Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);
                    string noTerminalAux;
                    noTerminalAux = pila.Pop();
                    Console.WriteLine("Hice pop de no terminal: " + noTerminalAux);

                    imprimirPila();
                    // mete el fin no terminal para mas tarde
                    pila.Push(string.Concat("$ ", verPila));
                    // imprimirPila();
                    // mete la derivación de la regla a la pila en orden inverso

                    var countRegla = GLC.listaGramaticaLibreContextoRegla[k].derecha.Count;
                    LinkedList<string> produccionActual = new LinkedList<string>();

                    string verPilaAux;
                    verPilaAux = verPila;
                    for (int j = 0; j < countRegla; j++)
                    {
                        verPilaAux = GLC.listaGramaticaLibreContextoRegla[k].derecha[j];
                        produccionActual.AddLast(verPilaAux);
                        Console.WriteLine("produccion aux: " + GLC.listaGramaticaLibreContextoRegla[k].derecha[j]);
                    }


                    for (int j = produccionActual.Count() - 1; 0 <= j; j--)
                    {
                        Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);
                        verPila = produccionActual.ElementAt(j);
                        pila.Push(verPila);
                        imprimirPila();

                    }



                }


                //while (pila.Contains((s = simboloActual)) && pila.Peek() != s) { Console.WriteLine("Hice pop de While: " + pila.Pop()); }
                // realizado = true;
            }
        }


        /// <summary>
        /// este metodo se encarga de crear el codigo de graphviz
        /// </summary>
        /// <param name="nodo">recibe una lista de reglas gramaticales libres de contexto</param>
        /// <returns></returns>
        public string ToDotGraph(List<GLC_Regla> nodo)
        {
            StringBuilder b = new StringBuilder();
            b.Append("digraph G {" + Environment.NewLine);

            for (int k = 0; k < GLC.listaGramaticaLibreContextoRegla.Count - 1; k++)
            {
                b.Append(ToDot(GLC.listaGramaticaLibreContextoRegla[k]));
            }


            b.Append("}");
            return b.ToString();
        }

        int correlativo;
        string raiz = "E";
        /// <summary>
        /// este metodo se encarga de crear el interior del codigo de graphviz
        /// </summary>
        /// <param name="nodo">recibe un regla gramatical libre de contexto</param>
        /// <returns></returns>
        public string ToDot(GLC_Regla nodo)
        {

            StringBuilder b = new StringBuilder();
            if (nodo.izquierda != null)
            {

                for (int j = 0; j < nodo.derecha.Count; j++)
                {

                    raiz = nodo.derecha[j];
                    b.AppendFormat(" \"{0}\"->\"{1}\"{2}", nodo.izquierda.ToString(), raiz, Environment.NewLine);

                }

            }

            return b.ToString();
        }


        static void _AppendTreeTo(StringBuilder result, NodoArbol node)
        {
            List<NodoArbol> firstStack = new List<NodoArbol>();
            firstStack.Add(node);

            List<List<NodoArbol>> childListStack = new List<List<NodoArbol>>();
            childListStack.Add(firstStack);

            while (childListStack.Count > 0)
            {
                List<NodoArbol> childStack = childListStack[childListStack.Count - 1];

                if (childStack.Count == 0)
                {
                    childListStack.RemoveAt(childListStack.Count - 1);
                }
                else
                {
                    node = childStack[0];
                    childStack.RemoveAt(0);

                    string indent = "";
                    for (int i = 0; i < childListStack.Count - 1; i++)
                    {
                        indent += (childListStack[i].Count > 0) ? "|  " : "   ";
                    }
                    var s = node.valor;
                    result.Append(string.Concat(indent, "└> ", s, " ", "").TrimEnd());
                    result.AppendLine();// string.Concat(" at line ", node.Line, ", column ", node.Column, ", position ", node.Position, ", length of ", node.Length));
                    if (node.hijo.Count > 0)
                    {
                        childListStack.Add(new List<NodoArbol>(node.hijo));
                    }
                }
            }
        }

        public void analisis(string verPila)
        {
            // no terminal

            List<GLC_Regla> listaProduccionActual = new List<GLC_Regla>();
            foreach (var produccion in GLC.listaGramaticaLibreContextoRegla)
            {
                if (verPila == produccion.izquierda)
                {
                    listaProduccionActual.Add(produccion);
                }

            }

            Console.WriteLine("Lista de producciones repetidas");
            foreach (var produccion in listaProduccionActual)
            {
                var sb = new StringBuilder();
                sb.Append(produccion.izquierda ?? "");
                sb.Append(" ->");
                var ic = produccion.derecha.Count;
                for (var i = 0; i < ic; ++i)
                {
                    sb.Append(" ");
                    sb.Append(produccion.derecha[i]);
                }
                Console.WriteLine(sb);
            }
            int iteradorProduccionActualLista = 0;
            LinkedList<string> produccionActual = new LinkedList<string>();

            while (listaProduccionActual.Count < 0)
            {
                string verPilaAux;
                verPilaAux = verPila;
                for (int j = 0; j < GLC.listaGramaticaLibreContextoRegla[iteradorProduccionActualLista].derecha.Count; j++)
                {
                    verPilaAux = GLC.listaGramaticaLibreContextoRegla[iteradorProduccionActualLista].derecha[j];
                    produccionActual.AddLast(verPilaAux);
                    Console.WriteLine("produccion aux: " + GLC.listaGramaticaLibreContextoRegla[iteradorProduccionActualLista].derecha[j]);
                }
                if (produccionActual.Last() == simboloActual || simboloActual == "E")
                {
                    for (int j = produccionActual.Count() - 1; 0 <= j; j--)
                    {
                        Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);
                        verPila = produccionActual.ElementAt(j);
                        pila.Push(verPila);
                        imprimirPila();

                    }
                }
                else
                {
                    iteradorProduccionActualLista++;
                }

            }
            //-----------------------------------------------------------------+

            Console.WriteLine("Lista de producciones repetidas");
            foreach (var produccion in listaProduccionActual)
            {
                var sb = new StringBuilder();
                sb.Append(produccion.izquierda ?? "");
                sb.Append(" ->");
                var ic = produccion.derecha.Count;
                for (var i = 0; i < ic; ++i)
                {
                    sb.Append(" ");
                    sb.Append(produccion.derecha[i]);
                }
                Console.WriteLine(sb);
            }

            for (int k = 0; k < GLC.listaGramaticaLibreContextoRegla.Count - 1; k++)
            {

                if (GLC.listaGramaticaLibreContextoRegla[k].izquierda == (verPila))
                {


                    Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);
                    string noTerminalAux;
                    noTerminalAux = pila.Pop();
                    Console.WriteLine("Hice pop de no terminal: " + noTerminalAux);

                    imprimirPila();
                    // mete el fin no terminal para mas tarde
                    // pila.Push(string.Concat("$", verPila));
                    // imprimirPila();
                    // mete la derivación de la regla a la pila en orden inverso

                    var countRegla = GLC.listaGramaticaLibreContextoRegla[k].derecha.Count;

                    string verPilaAux;
                    verPilaAux = verPila;
                    for (int j = 0; j < countRegla; j++)
                    {
                        verPilaAux = GLC.listaGramaticaLibreContextoRegla[k].derecha[j];
                        produccionActual.AddLast(verPilaAux);
                        Console.WriteLine("produccion aux: " + GLC.listaGramaticaLibreContextoRegla[k].derecha[j]);
                    }

                    if (produccionActual.Contains(simboloActual) || simboloActual == "E")
                    {

                        for (int j = produccionActual.Count() - 1; 0 <= j; j--)
                        {
                            Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);
                            verPila = produccionActual.ElementAt(j);
                            pila.Push(verPila);
                            imprimirPila();

                        }

                    }


                }

                // realizado = true;
            }

        }
        public void imprimirListaTokens()
        {
            for (int i = 0; i < listaTokenResultado.Count(); i++)
            {

                int idActual = listaTokenResultado[i].Id;
                string simboloActual1 = listaTokenResultado[i].Simbolo;

                Console.WriteLine("i: {0}\tSimbolo actual: {1}", i, simboloActual1);
            }
        }

    }
}
