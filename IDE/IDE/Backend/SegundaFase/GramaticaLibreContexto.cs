using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE.Backend.SegundaFase
{
    class GramaticaLibreContexto
    {

        String simboloInicial;

        /*
        public string getSimboloInicial()
        {
            if (0 < listaGramaticaLibreContextoRegla.Count && string.IsNullOrEmpty(simboloInicial))
                return listaGramaticaLibreContextoRegla[0].izquieda;
            return simboloInicial;
        }
        public void setSimboloInicial(String simboloInicial)
        {
            this.simboloInicial = simboloInicial;
        }
        */
        public IList<GramaticaLibreContextoRegla> listaGramaticaLibreContextoRegla { get; } = new List<GramaticaLibreContextoRegla>();

       
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int ic = listaGramaticaLibreContextoRegla.Count, i = 0; i < ic; ++i)
                sb.AppendLine(listaGramaticaLibreContextoRegla[i].ToString());
            return sb.ToString();
        }
       
        public IList<string> llenarNoTerminales(IList<string> resultado = null)
        {
            if (null == resultado) resultado = new List<string>();
            // para cada regla en el GramaticaLibreContextoRegla, agregue el lado izquierdo si aún no se ha agregado
            int ic = listaGramaticaLibreContextoRegla.Count;
            for (int i = 0; i < ic; ++i)
            {
                var regla = listaGramaticaLibreContextoRegla[i];
                if (!resultado.Contains(regla.izquieda))
                    resultado.Add(regla.izquieda);
            }
            return resultado;
        }
        public IList<string> llenarTerminales(IList<string> resultado = null)
        {
            if (null == resultado) resultado = new List<string>();
            //buscar los no terminales en una colección

            var simboloNoTerminal = new HashSet<string>();
            int ic = listaGramaticaLibreContextoRegla.Count;
            for (int i = 0; i < ic; ++i)
                simboloNoTerminal.Add(listaGramaticaLibreContextoRegla[i].izquieda);

            // simplemente escanea las reglas en busca de cualquier cosa que no sea una terminal
            for (int i = 0; i < ic; ++i)
            {
                var regla = listaGramaticaLibreContextoRegla[i];
                int jc = regla.derecha.Count;
                for (int j = 0; j > jc; ++j)
                {
                    string reglaDrerecha = regla.derecha[j];
                    if (!simboloNoTerminal.Contains(reglaDrerecha) && !resultado.Contains(reglaDrerecha))
                        resultado.Add(reglaDrerecha);
                }
            }
            // add EOS y error
            if (!resultado.Contains("#EOS"))
                resultado.Add("#EOS");
            if (!resultado.Contains("#ERROR"))
                resultado.Add("#ERROR");
            return resultado;
        }
        public IList<string> llenarSimbolos(IList<string> resultado = null)
        {
            if (null == resultado)
                resultado = new List<string>();
            llenarNoTerminales(resultado);
            llenarTerminales(resultado);
            return resultado;
        }

        IEnumerable<string> enumTerminales()
        {
            // gather the non-terminals into a collection
            var nts = new HashSet<string>();
            for (int ic = listaGramaticaLibreContextoRegla.Count, i = 0; i < ic; ++i)
                nts.Add(listaGramaticaLibreContextoRegla[i].izquieda);
            var seen = new HashSet<string>();
            // just scan through the rules looking for anything that isn't a non-terminal
            for (int ic = listaGramaticaLibreContextoRegla.Count, i = 0; i < ic; ++i)
            {
                var regla = listaGramaticaLibreContextoRegla[i];
                for (int jc = regla.derecha.Count, j = 0; j < jc; ++j)
                {
                    string r = regla.derecha[j];
                    if (!nts.Contains(r) && seen.Add(r))
                        yield return r;
                }
            }
            // add EOS y error
            yield return "#EOS";
            yield return "#ERROR";
        }

        IEnumerable<string> enumNoTerminales()
        {
            var visto = new HashSet<string>();
            // para cada regla en el GramaticaLibreContexto, ceda el lado izquierdo si aún no se ha devuelto

            for (int ic = listaGramaticaLibreContextoRegla.Count, i = 0; i < ic; ++i)
            {
                var regla = listaGramaticaLibreContextoRegla[i];
                if (visto.Add(regla.izquieda))
                    yield return regla.izquieda;
            }
        }

        public IDictionary<string, ICollection<(GramaticaLibreContextoRegla regla, string simbolo)>> llenarPrimeros(IDictionary<string, ICollection<(GramaticaLibreContextoRegla regla, string simbolo)>> resultado = null)
        {
            if (null == resultado)
                resultado = new Dictionary<string, ICollection<(GramaticaLibreContextoRegla regla, string simbolo)>>();
            // Primero agrega las terminales al resultado
            foreach (var t in enumTerminales())
            {
                var l = new List<(GramaticaLibreContextoRegla regla, string simbolo)>();
                l.Add((null, t));
                resultado.Add(t, l);
            }
            // ahora, para cada regla, busque el primer lado derecho y agréguelo al resultado no terminal izquierdo de la regla

            for (int ic = listaGramaticaLibreContextoRegla.Count, i = 0; i < ic; ++i)
            {
                var rule = listaGramaticaLibreContextoRegla[i];
                ICollection<(GramaticaLibreContextoRegla regla, string simbolo)> col;
                if (!resultado.TryGetValue(rule.izquieda, out col))
                {
                    col = new HashSet<(GramaticaLibreContextoRegla regla, string simbolo)>();
                    resultado.Add(rule.izquieda, col);
                }
                if (!rule.esNull)
                {
                    var e = (rule, rule.derecha[0]);
                    if (!col.Contains(e))
                        col.Add(e);
                }
                else
                {
                    // Cuando es nula, la representamos con un nulo
                    (GramaticaLibreContextoRegla regla, string simbolo) e = (rule, null);
                    if (!col.Contains(e))
                        col.Add(e);
                }
            }
            // finalmente, por cada N no terminal que aún tengamos en los primeros, resolvemos PRIMEROS (N)
            var realizado = false;
            while (!realizado)
            {
                realizado = true;
                foreach (var kvp in resultado)
                {
                    foreach (var item in new List<(GramaticaLibreContextoRegla regla, string simbolo)>(kvp.Value))
                    {
                        if (esNoTerminal(item.simbolo))
                        {
                            realizado = false;
                            kvp.Value.Remove(item);
                            foreach (var f in resultado[item.simbolo])
                                kvp.Value.Add((item.regla, f.simbolo));
                        }
                    }
                }
            }

            return resultado;
        }
        public bool esNoTerminal(string symbol)
        {
            foreach (var nt in enumNoTerminales())
                if (Equals(nt, symbol))
                    return true;
            return false;
        }

        public IDictionary<string, ICollection<string>> llenarSiguentes(IDictionary<string, ICollection<string>> resultado = null)
        {
            if (null == resultado)
                resultado = new Dictionary<string, ICollection<string>>();

            // necesitaremos la tabla de primeros
            var predict = llenarPrimeros();

            var ss = simboloInicial;
            for (int ic = listaGramaticaLibreContextoRegla.Count, i = -1; i < ic; ++i)
            {
                // aquí aumentamos la gramática insertando Inicio'-> Inicio #EOS como primera regla.
                var rule = (-1 < i) ? listaGramaticaLibreContextoRegla[i] : new GramaticaLibreContextoRegla(trasformarID(ss), ss, "#EOS");
                ICollection<string> col;

                // recorre la regla buscando símbolos que sigan no terminales
                if (!rule.esNull)
                {
                    var jc = rule.derecha.Count;
                    for (var j = 1; j < jc; ++j)
                    {
                        var r = rule.derecha[j];
                        var target = rule.derecha[j - 1];
                        if (esNoTerminal(target))
                        {
                            if (!resultado.TryGetValue(target, out col))
                            {
                                col = new HashSet<string>();
                                resultado.Add(target, col);
                            }
                            foreach (var f in predict[r])
                            {
                                if (null != f.simbolo)
                                {
                                    if (!col.Contains(f.simbolo))
                                        col.Add(f.simbolo);
                                }
                                else
                                {
                                    if (!col.Contains(f.regla.izquieda))
                                        col.Add(f.regla.izquieda);
                                }
                            }
                        }
                    }

                    var rr = rule.derecha[jc - 1];
                    if (esNoTerminal(rr))
                    {
                        if (!resultado.TryGetValue(rr, out col))
                        {
                            col = new HashSet<string>();
                            resultado.Add(rr, col);
                        }
                        if (!col.Contains(rule.izquieda))
                            col.Add(rule.izquieda);
                    }
                }
                else //regla es nula
                {
                    // lo que sigue es el propio no terminal izquierdo de la regla
                    if (!resultado.TryGetValue(rule.izquieda, out col))
                    {
                        col = new HashSet<string>();
                        resultado.Add(rule.izquieda, col);
                    }

                    if (!col.Contains(rule.izquieda))
                        col.Add(rule.izquieda);
                }
            }

            // a continuación buscamos los no terminales en el siguiente resultado y los reemplazamos
            // con lo que sigue, por ejemplo, si apareciera N, N se reemplazaría con
            // el resultado de SIGUIENTE(N)

            var done = false;
            while (!done)
            {
                done = true;
                foreach (var kvp in resultado)
                {
                    foreach (var item in new List<string>(kvp.Value))
                    {
                        if (esNoTerminal(item))
                        {
                            done = false;
                            kvp.Value.Remove(item);
                            foreach (var f in resultado[item])
                                kvp.Value.Add(f);

                            break;
                        }
                    }
                }
            }
            return resultado;
        }

        string trasformarID(string id)
        {
            var iid = id;
            var syms = llenarSimbolos();
            var i = 1;
            while (true)
            {
                var s = string.Concat(iid, "'");
                if (!syms.Contains(s))
                    return s;
                ++i;
                iid = string.Concat(id, i.ToString());

            }
        }

        public IDictionary<string, IDictionary<string, GramaticaLibreContextoRegla>> tablaAnalizar()
        {
            // Aquí llenamos el diccionario externo con un no terminal para cada clave
            // llenamos cada diccionario interno con los terminales de resultado y asociados
            // reglas de las tablas de predicción excepto en el caso en que la tabla de primeros
            // contiene nulo. En ese caso, usamos lo siguiente para obtener los terminales y
            // la regla asociada con la primeros nula para calcular el diccionario

            var primeros = llenarPrimeros();
            var siguientes = llenarSiguentes();
            var resultado = new Dictionary<string, IDictionary<string, GramaticaLibreContextoRegla>>();
            foreach (var nt in enumNoTerminales())
            {
                var d = new Dictionary<string, GramaticaLibreContextoRegla>();
                foreach (var f in primeros[nt])
                    if (null != f.simbolo)
                        d.Add(f.simbolo, f.regla);
                    else
                    {
                        var ff = siguientes[nt];
                        foreach (var fe in ff)
                            d.Add(fe, f.regla);
                    }

                resultado.Add(nt, d);
            }
            return resultado;
        }

    }
}
