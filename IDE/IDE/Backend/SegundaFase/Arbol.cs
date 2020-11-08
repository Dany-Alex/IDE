using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Backend.SegundaFase
{
    class Arbol
    {
        private NodoArbol raiz;

        public Arbol()
        {
            this.raiz = null;
        }
        public void insertar(String val)
        {
           // if (raiz == null)
             //   raiz = new NodoArbol(val);
            //else
               // raiz.insertar(val);
        }
        public void graficar(String path)
        {
         //   raiz.graficar(path);
        }
        public void inorden()
        {
            Console.WriteLine("Recorrido inorden del árbol binario de búsqueda:");
            inorden(raiz);
            Console.WriteLine();
        }
        private void inorden(NodoArbol a)
        {
            if (a == null)
                return;
           // inorden(a.izquierdo);
          //  Console.WriteLine(a.valor + ",");
          //  inorden(a.derecho);
        }
    }
}
