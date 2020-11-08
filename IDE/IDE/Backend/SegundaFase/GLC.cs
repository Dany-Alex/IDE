using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Backend.SegundaFase
{
    class GLC
    {
        public List<GLC_Regla> listaGramaticaLibreContextoRegla { get; } = new List<GLC_Regla>();


        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int ic = listaGramaticaLibreContextoRegla.Count, i = 0; i < ic; ++i)
                sb.AppendLine(listaGramaticaLibreContextoRegla[i].ToString());
            return sb.ToString();
        }



    }
}
