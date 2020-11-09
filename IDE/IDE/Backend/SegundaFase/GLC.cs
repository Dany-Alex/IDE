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

        public List<string> llenarNoTerminales(List<string> resultado = null)
        {
            if (null == resultado) resultado = new List<string>();
            // para cada regla en el GramaticaLibreContextoRegla, agregue el lado izquierdo si aún no se ha agregado
            int ic = listaGramaticaLibreContextoRegla.Count;
            for (int i = 0; i < ic; ++i)
            {
                var regla = listaGramaticaLibreContextoRegla[i];
                if (!resultado.Contains(regla.izquierda))
                    resultado.Add(regla.izquierda);
            }
            return resultado;
        }

        public List<string> llenarTerminales(List<string> resultado = null)
        {
            if (null == resultado) resultado = new List<string>();
            //buscar los no terminales en una colección

            var simboloNoTerminal = new HashSet<string>();
            int ic = listaGramaticaLibreContextoRegla.Count;
            for (int i = 0; i < ic; ++i)
                simboloNoTerminal.Add(listaGramaticaLibreContextoRegla[i].izquierda);

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
            // add FIN DE LA ENTRADA (End Of Stream) y error
            if (!resultado.Contains("#EOS"))
                resultado.Add("#EOS");
            if (!resultado.Contains("#ERROR"))
                resultado.Add("#ERROR");
            return resultado;
        }

        public List<string> llenarSimbolos(List<string> resultado = null)
        {
            if (null == resultado)
                resultado = new List<string>();
            llenarNoTerminales(resultado);
            llenarTerminales(resultado);
            return resultado;
        }

        IEnumerable<string> enumTerminales()
        {
            // reúne los no terminales en una colección
            var nts = new HashSet<string>();
            for (int ic = listaGramaticaLibreContextoRegla.Count, i = 0; i < ic; ++i)
                nts.Add(listaGramaticaLibreContextoRegla[i].izquierda);
            var seen = new HashSet<string>();
            // simplemente escanea las reglas en busca de cualquier cosa que no sea una terminal
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
            // agregar EOS y error
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
                if (visto.Add(regla.izquierda))
                    yield return regla.izquierda;
            }
        }

        public IDictionary<string, ICollection<(GLC_Regla regla, string simbolo)>> llenarPrimeros(IDictionary<string, ICollection<(GLC_Regla regla, string simbolo)>> resultado = null)
        {
            if (null == resultado)
                resultado = new Dictionary<string, ICollection<(GLC_Regla regla, string simbolo)>>();
            // Primero agrega las terminales al resultado
            foreach (var t in enumTerminales())
            {
                var l = new List<(GLC_Regla regla, string simbolo)>();
                l.Add((null, t));
                resultado.Add(t, l);
            }
            // ahora, para cada regla, busque el primer lado derecho y agréguelo al resultado no terminal izquierdo de la regla

            for (int ic = listaGramaticaLibreContextoRegla.Count, i = 0; i < ic; ++i)
            {
                var rule = listaGramaticaLibreContextoRegla[i];
                ICollection<(GLC_Regla regla, string simbolo)> col;
                if (!resultado.TryGetValue(rule.izquierda, out col))
                {
                    col = new HashSet<(GLC_Regla regla, string simbolo)>();
                    resultado.Add(rule.izquierda, col);
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
                    (GLC_Regla regla, string simbolo) e = (rule, null);
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
                    foreach (var item in new List<(GLC_Regla regla, string simbolo)>(kvp.Value))
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

        string simboloInicial = null;

        public string simboloInicialMetodo
        {

            get
            {
                if (0 < listaGramaticaLibreContextoRegla.Count && string.IsNullOrEmpty(simboloInicial))
                { return listaGramaticaLibreContextoRegla[0].izquierda; }
                return simboloInicial;
            }
            set { this.simboloInicial = value; }

        }

        public IDictionary<string, ICollection<string>> llenarSiguentes(IDictionary<string, ICollection<string>> resultado = null)
        {
            if (resultado == null)
                resultado = new Dictionary<string, ICollection<string>>();

            // necesitaremos la tabla de primeros
            var predict = llenarPrimeros();

            var ss = simboloInicialMetodo;
            for (int ic = listaGramaticaLibreContextoRegla.Count, i = -1; i < ic; ++i)
            {
                // aquí aumentamos la gramática insertando Inicio'-> Inicio #EOS como primera regla.
                var rule = (-1 < i) ? listaGramaticaLibreContextoRegla[i] : new GLC_Regla(trasformarID(ss), ss, "#EOS");
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
                                    if (!col.Contains(f.regla.izquierda))
                                        col.Add(f.regla.izquierda);
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
                        if (!col.Contains(rule.izquierda))
                            col.Add(rule.izquierda);
                    }
                }
                else //regla es nula
                {
                    // lo que sigue es el propio no terminal izquierdo de la regla
                    if (!resultado.TryGetValue(rule.izquierda, out col))
                    {
                        col = new HashSet<string>();
                        resultado.Add(rule.izquierda, col);
                    }

                    if (!col.Contains(rule.izquierda))
                        col.Add(rule.izquierda);
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

        public IDictionary<string, IDictionary<string, GLC_Regla>> tablaAnalizar()
        {
            // Aquí llenamos el diccionario externo con un no terminal para cada clave
            // llenamos cada diccionario interno con los terminales de resultado y asociados
            // reglas de las tablas de predicción excepto en el caso en que la tabla de primeros
            // contiene nulo. En ese caso, usamos lo siguiente para obtener los terminales y
            // la regla asociada con la primeros nula para calcular el diccionario

            var primeros = llenarPrimeros();
            var siguientes = llenarSiguentes();
            var resultado = new Dictionary<string, IDictionary<string, GLC_Regla>>();
            var enumNoterminal = enumNoTerminales();
            foreach (var nt in enumNoterminal)
            {
                var d = new Dictionary<string, GLC_Regla>();
                foreach (var f in primeros[nt]) {  
                    if (null != f.simbolo) { 
                        d.Add(f.simbolo, f.regla);
                    }
                    else
                    {
                        var ff = siguientes[nt];
                        foreach (var fe in ff) {
                            
                                if ( d.ContainsKey(fe))
                                {
                                    Console.WriteLine("el valor duplicado: " + fe);
                                }
                                else {
                                    d.Add(fe, f.regla);

                                }
                            
                       

                        }
                            
                    } 
                }
                  

                resultado.Add(nt, d);
            }
            return resultado;
        }


    }
}
