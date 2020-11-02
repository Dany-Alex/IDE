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
        public void iniciarGramatica(RichTextBox richTextBox)
        {



            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("E", "principal", "(", ")", "BLOQUE_CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("BLOQUE_CODIGO", "{", "CODIGO", "}"));
            listaNoTerminales.Add("BLOQUE_CODIGO");
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO", "CODIGO_PRIMA", "CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO"));
            listaNoTerminales.Add("CODIGO");

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO_PRIMA", "VARIABLE_ASIGNADA"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO_PRIMA", "ESTRUCTURA_SI"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("CODIGO_PRIMA"));
            listaNoTerminales.Add("CODIGO_PRIMA");

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("VARIABLE_ASIGNADA", "ASIGNACION_VARIABLE", ";"));
            listaNoTerminales.Add("VARIABLE_ASIGNADA");

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNACION_VARIABLE", "ESTRUCTURA_VARIABLE", "ESTRUCTURA_VARIABLE'"));
            //  GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_VARIABLE'", ",", "ESTRUCTURA_VARIABLE", "ESTRUCTURA_VARIABLE'"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_VARIABLE'"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_VARIABLE", "Variable", "ASIGNAR"));
            listaNoTerminales.Add("ESTRUCTURA_VARIABLE");

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNAR", "=", "TIPO_VARIABLE"));
            listaNoTerminales.Add("ASIGNAR");


            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "entero"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "Cadena"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "Boolean"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "Char"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("TIPO_VARIABLE", "Decimal"));
            listaNoTerminales.Add("TIPO_VARIABLE");


            #region estructura SI

            //SI(ASIGNACION_VARIABLE_ESTRUCTURA_SI){lineas de codigo}
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SI", "SI", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "ESTRUCTURA_SI_FINAL"));
            listaNoTerminales.Add("ESTRUCTURA_SI");

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "(", "VARIABLE_O_TIPO", "VARIABLE_O_TIPO'", ")"));
            listaNoTerminales.Add("ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION");


            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SI_FINAL", "SINO_SI", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "ESTRUCTURA_SI_FINAL"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SI_FINAL", "SINO_SI", "ASIGNACION_VARIABLE_ESTRUCTURA_CONDICION", "ESTRUCTURA_SI_FINAL"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SI_FINAL", "SINO", "BLOQUE_CODIGO"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("ESTRUCTURA_SI_FINAL"));
            listaNoTerminales.Add("ESTRUCTURA_SI_FINAL");

            #endregion

            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("VARIABLE_O_TIPO", "Variable"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("VARIABLE_O_TIPO", "TIPO_VARIABLE"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("VARIABLE_O_TIPO'", "LOGICO", "NEGAR", "VARIABLE_O_TIPO"));
            listaNoTerminales.Add("VARIABLE_O_TIPO");


            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("NEGAR", "!"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("NEGAR"));
            listaNoTerminales.Add("NEGAR");


            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "=="));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "!="));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", ">="));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "<="));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", ">"));
            GLC.listaGramaticaLibreContextoRegla.Add(new GLC_Regla("LOGICO", "<"));
            listaNoTerminales.Add("LOGICO");



            Console.WriteLine(" paso 1 - Gramatica Libre Contexto");
            Console.WriteLine(GLC);
            richTextBox.Text = GLC.ToString();
            setCodigoDot(ToDotGraph(GLC.listaGramaticaLibreContextoRegla));
            richTextBox.AppendText(this.codigoDot);
            
            Console.WriteLine();

        }
        string codigoDot=null;
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
         

            while (pila.Count() > 0) {
                simboloActual = listaTokenResultado[tokenActual].Simbolo;
                if (0 < pila.Count)
                {

                    var verPila = pila.Peek();

                    if (verPila.StartsWith("$"))
                    {
                        pila.Pop();
                        Console.WriteLine("Analisis sintactico realizado de manera correcta, no hay errores sintacticos");
                        realizado = true;
                    }

                    //terminal
                    if (verPila == simboloActual)
                    {
                        Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);

                        Console.WriteLine("Hice pop de terminal: " + pila.Pop());

                        imprimirPila();
                        tokenActual++;
                        Console.WriteLine("Simbolo actual: " + simboloActual + "  i: " + tokenActual);

                        imprimirPila();
                        realizado = true;
                    }

                    //analisis(verPila);


                    // no terminal

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
                            LinkedList<string> produccionActual = new LinkedList<string>();

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

            }



            return realizado;

        }

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


        public static string ToDot(GLC_Regla nodo)
        {
            string raiz="E";

            StringBuilder b = new StringBuilder();
            if (nodo.izquierda !=null)
            {
                for (int j = 0; j < nodo.derecha.Count; j++)
                {
                    raiz = nodo.derecha[j];
                    b.AppendFormat(" \"{0}\"->\"{1}\"{2}", nodo.izquierda.ToString(), raiz , Environment.NewLine);
                }

            }

            return b.ToString();
        }


 

        public void analisis(string verPila) {
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
            int produccionActualLista = 0;
            LinkedList<string> produccionActual = new LinkedList<string>();

            while (listaProduccionActual.Count < 0)
            {
                string verPilaAux;
                verPilaAux = verPila;
                for (int j = 0; j < GLC.listaGramaticaLibreContextoRegla[produccionActualLista].derecha.Count; j++)
                {
                    verPilaAux = GLC.listaGramaticaLibreContextoRegla[produccionActualLista].derecha[j];
                    produccionActual.AddLast(verPilaAux);
                    Console.WriteLine("produccion aux: " + GLC.listaGramaticaLibreContextoRegla[produccionActualLista].derecha[j]);
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
                else {
                    produccionActualLista++;
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
        public void imprimirListaTokens() {
            for (int i = 0; i < listaTokenResultado.Count(); i++)
            {

                int idActual = listaTokenResultado[i].Id;
                string simboloActual1 = listaTokenResultado[i].Simbolo;

                Console.WriteLine("i: {0}\tSimbolo actual: {1}", i, simboloActual1);
            }
        }

    }
}
