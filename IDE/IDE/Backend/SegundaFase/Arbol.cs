using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Backend.SegundaFase
{
    class Arbol
    {
        private NodoArbol raiz=new NodoArbol();

        public List<NodoArbol> nodos { get; } = new List<NodoArbol>();
        public Arbol()
        {
        }
        public void insertar(String val)
        {
           // if (raiz == null)
             //   raiz = new NodoArbol(val);
            //else
               // raiz.insertar(val);
        }
        public StringBuilder graficar()
        {
            return raiz.getCodigoGraphviz();
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
