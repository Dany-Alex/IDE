using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Backend.SegundaFase
{
    class GramaticaLibreContextoRegla
    {
        public string izquieda { get; set; } = null;
        public IList<string> derecha { get; } = new List<string>();

        public GramaticaLibreContextoRegla(string izquieda, IEnumerable<string> derecha)
        {
            this.izquieda = izquieda;
            if (null != this.derecha) this.derecha = new List<string>(derecha);
        }

        public GramaticaLibreContextoRegla(string izquierda, params string[] derecha) : this(izquierda, (IEnumerable<string>)derecha)
        {

        }

        public bool esNull { get { return 0 == derecha.Count; } }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(izquieda ?? "");
            sb.Append(" ->");
            var ic = derecha.Count;
            for (var i = 0; i < ic; ++i)
            {
                sb.Append(" ");
                sb.Append(derecha[i]);
            }

            
            return sb.ToString();

        }



        //right-hand side rhs -> lado derecho
        //left-hand side lhs  -> lado izquiedo


    }
}
