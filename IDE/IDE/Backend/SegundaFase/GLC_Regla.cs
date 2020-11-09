using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Backend.SegundaFase
{
    class GLC_Regla
    {
        /// <summary>
        /// este es el no terminal
        /// </summary>
        public string izquierda { get; set; } = null;
        public List<string> derecha { get; } = new List<string>();

        /// <summary>
        /// esta es la produccion del no terminal
        /// </summary>
        public GLC_Regla(string izquierda, params string[] derecha)
        {
            this.izquierda = izquierda;
            if (null != this.derecha) this.derecha = new List<string>(derecha);
        }

        /// <summary>
        /// verifica si la produccion es nula osea epsion ( si la produccion es vacia)
        /// </summary>
        public bool esNull { get { return 0 == derecha.Count; } }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(izquierda ?? "");
            sb.Append(" ->");
            var ic = derecha.Count;
            for (var i = 0; i < ic; ++i)
            {
                sb.Append(" ");
                sb.Append(derecha[i]);
            }
            return sb.ToString();
        }



    }

}
