using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camposSemanticos.Control
{
    public class Uniones
    {
        private Dictionary<int, List<string>> listaFinal1punto;
        private Dictionary<int, List<string>> listaFinal2punto;
        private Dictionary<int, List<string>> listaFinal3punto;
        private Dictionary<int, List<string>> listaFinal4punto;
        private int compartirPalabrasEnRelaciones = 2; // Número de palabras comunes entre listas para que se fusionen en una sola 
                                                       // EL VALOR 2 ES EL MEJOR PARA ENCONTRAR RELACIONES (¡MUY RECOMENDADO!)

        public Uniones() {
            this.listaFinal1punto = new Dictionary<int, List<string>>();
            this.listaFinal2punto = new Dictionary<int, List<string>>();
            this.listaFinal3punto = new Dictionary<int, List<string>>();
            this.listaFinal4punto = new Dictionary<int, List<string>>();
        }

        public Dictionary<int, List<string>> iniciarOperacionUnion1(Dictionary<int, List<string>> listaFinalWordnet, Dictionary<int, List<string>> listasRelacionadasDiccionarioSinAnton)
        {
            union1Punto(listaFinalWordnet, listasRelacionadasDiccionarioSinAnton);
            return this.listaFinal1punto;
        }

        public Dictionary<int, List<string>> iniciarOperacionUnion2(Dictionary<int, List<string>> listaFinal1punto, Dictionary<int, List<string>> listaComprobarPrimitivasDerivadas)
        {
            union2Punto(listaFinal1punto, listaComprobarPrimitivasDerivadas);
            return this.listaFinal2punto;
        }

        public Dictionary<int, List<string>> iniciarOperacionUnion3(Dictionary<int, List<string>> listaFinal2punto, Dictionary<int, List<string>> listaDosDiccionariosIdeasAfines)
        {
            union3Punto(listaFinal2punto, listaDosDiccionariosIdeasAfines);
            return this.listaFinal3punto;
        }

        public Dictionary<int, List<string>> iniciarOperacionUnion4(Dictionary<int, List<string>> listaFinal3punto, Dictionary<int, List<string>> palabrasRelacionadas)
        {
            union4Punto(listaFinal3punto, palabrasRelacionadas);
            return this.listaFinal4punto;
        }

        public void union1Punto(Dictionary<int, List<string>> listaFinalWordnet, Dictionary<int, List<string>> listasRelacionadasDiccionarioSinAnton)
        {
            // Agregar todas las listas de listaFinalWordnet a listaFinal1Punto
            foreach (var lista in listaFinalWordnet.Values)
            {
                if (!listaFinal1punto.Values.Any(l => l.SequenceEqual(lista)) && !lista.Any(s => lista.Count(str => str == s) > 1))
                {
                    listaFinal1punto.Add(listaFinal1punto.Keys.Count > 0 ? listaFinal1punto.Keys.Max() + 1 : 1, new List<string>(lista));
                }
            }

            // Iterar sobre cada lista en listasRelacionadasDiccionarioSinAnton
            foreach (var listaRelacionada in listasRelacionadasDiccionarioSinAnton.Values)
            {
                // Encontrar si hay una lista en listaFinal1Punto que comparta dos palabras con listaRelacionada
                bool found = false;
                foreach (var listaFinal in listaFinal1punto.Values)
                {
                    int count = 0;
                    foreach (var palabra in listaRelacionada)
                    {
                        if (listaFinal.Contains(palabra))
                        {
                            count++;
                        }
                    }

                    if (count >= compartirPalabrasEnRelaciones)
                    {
                        // Si encontramos una lista, agregamos todas las palabras de listaRelacionada a esa lista
                        found = true;
                        foreach (var nuevaPalabra in listaRelacionada)
                        {
                            if (!listaFinal.Contains(nuevaPalabra))
                            {
                                listaFinal.Add(nuevaPalabra);
                            }
                        }
                        break;
                    }
                }

                // Si no encontramos ninguna lista que comparta dos palabras con listaRelacionada, agregamos listaRelacionada a listaFinal1Punto
                if (!found && !listaFinal1punto.Values.Any(l => l.SequenceEqual(listaRelacionada)))
                {
                    listaFinal1punto.Add(listaFinal1punto.Keys.Count > 0 ? listaFinal1punto.Keys.Max() + 1 : 1, new List<string>(listaRelacionada));
                }
            }

            // Eliminar subgrupos de grupos
            var gruposFinales = listaFinal1punto.Values.ToList();
            for (int i = 0; i < gruposFinales.Count; i++)
            {
                var grupoActual = gruposFinales[i];
                bool esSubgrupo = false;
                for (int j = 0; j < gruposFinales.Count; j++)
                {
                    if (i != j)
                    {
                        var grupoComparacion = gruposFinales[j];
                        if (grupoActual.Count >= grupoComparacion.Count &&
                            grupoActual.Intersect(grupoComparacion).Count() == grupoComparacion.Count)
                        {
                            esSubgrupo = true;
                            break;
                        }
                    }
                }
                if (esSubgrupo)
                {
                    listaFinal1punto.Remove(listaFinal1punto.FirstOrDefault(x => x.Value.SequenceEqual(grupoActual)).Key);
                }
            }

            // Imprimir resultado
            foreach (var lista in listaFinal1punto)
            {
                Console.Write("Lista union 1pto #{0}: ", lista.Key);
                Console.WriteLine(string.Join(", ", lista.Value));
            }
        }

        public void union2Punto(Dictionary<int, List<string>> listaFinal1Punto, Dictionary<int, List<string>> listaComprobarPrimitivasDerivadas)
        {
            // Agregar todas las listas de listaFinal1Punto a listaFinal2Punto
            foreach (var lista in listaFinal1Punto.Values)
            {
                if (!listaFinal2punto.Values.Any(l => l.SequenceEqual(lista)) && !lista.Any(s => lista.Count(str => str == s) > 1))
                {
                    listaFinal2punto.Add(listaFinal2punto.Keys.Count > 0 ? listaFinal2punto.Keys.Max() + 1 : 1, new List<string>(lista));
                }
            }

            // Iterar sobre cada lista en listaComprobarPrimitivasDerivadas
            foreach (var listaComprobar in listaComprobarPrimitivasDerivadas.Values)
            {
                // Encontrar si hay una lista en listaFinal2Punto que comparta dos palabras con listaComprobar
                bool found = false;
                foreach (var listaFinal in listaFinal2punto.Values)
                {
                    int count = 0;
                    foreach (var palabra in listaComprobar)
                    {
                        if (listaFinal.Contains(palabra))
                        {
                            count++;
                        }
                    }

                    if (count >= compartirPalabrasEnRelaciones)
                    {
                        // Si encontramos una lista, agregamos todas las palabras de listaComprobar a esa lista
                        found = true;
                        foreach (var nuevaPalabra in listaComprobar)
                        {
                            if (!listaFinal.Contains(nuevaPalabra))
                            {
                                listaFinal.Add(nuevaPalabra);
                            }
                        }
                        break;
                    }
                }

                // Si no encontramos ninguna lista que comparta dos palabras con listaComprobar, agregamos listaComprobar a listaFinal2Punto
                if (!found && !listaFinal2punto.Values.Any(l => l.SequenceEqual(listaComprobar)))
                {
                    listaFinal2punto.Add(listaFinal2punto.Keys.Count > 0 ? listaFinal2punto.Keys.Max() + 1 : 1, new List<string>(listaComprobar));
                }
            }

            // Eliminar subgrupos de grupos
            var gruposFinales = listaFinal2punto.Values.ToList();
            for (int i = 0; i < gruposFinales.Count; i++)
            {
                var grupoActual = gruposFinales[i];
                bool esSubgrupo = false;
                for (int j = 0; j < gruposFinales.Count; j++)
                {
                    if (i != j)
                    {
                        var grupoComparacion = gruposFinales[j];
                        if (grupoActual.Count >= grupoComparacion.Count &&
                            grupoActual.Intersect(grupoComparacion).Count() == grupoComparacion.Count)
                        {
                            esSubgrupo = true;
                            break;
                        }
                    }
                }
                if (esSubgrupo)
                {
                    listaFinal2punto.Remove(listaFinal2punto.FirstOrDefault(x => x.Value.SequenceEqual(grupoActual)).Key);
                }
            }

            // Imprimir resultado
            foreach (var lista in listaFinal2punto)
            {
                Console.Write("Lista union 2pto #{0}: ", lista.Key);
                Console.WriteLine(string.Join(", ", lista.Value));
            }
        }


        public void union3Punto(Dictionary<int, List<string>> listaFinal2punto, Dictionary<int, List<string>> listaDosDiccionariosIdeasAfines)
        {
            // Agregar todas las listas de listaFinal2punto a listaFinal3punto
            foreach (var lista in listaFinal2punto.Values)
            {
                listaFinal3punto.Add(listaFinal3punto.Count, new List<string>(lista));
            }

            // Iterar sobre cada lista en listaDosDiccionariosIdeasAfines
            foreach (var listaRelacionada in listaDosDiccionariosIdeasAfines.Values)
            {
                // Encontrar si hay una lista en listaFinal3punto que comparta al menos dos palabras con listaRelacionada
                bool found = false;
                foreach (var listaFinal in listaFinal3punto.Values)
                {
                    int sharedWordsCount = 0;
                    foreach (var palabra in listaRelacionada)
                    {
                        if (listaFinal.Contains(palabra))
                        {
                            sharedWordsCount++;
                        }
                    }
                    if (sharedWordsCount >= compartirPalabrasEnRelaciones)
                    {
                        // Si encontramos una lista, agregamos todas las palabras de listaRelacionada a esa lista
                        found = true;
                        foreach (var nuevaPalabra in listaRelacionada)
                        {
                            if (!listaFinal.Contains(nuevaPalabra))
                            {
                                listaFinal.Add(nuevaPalabra);
                            }
                        }
                        break;
                    }
                }

                // Si no encontramos ninguna lista que comparta al menos dos palabras con listaRelacionada, agregamos listaRelacionada a listaFinal3punto
                if (!found)
                {
                    listaFinal3punto.Add(listaFinal3punto.Count, new List<string>(listaRelacionada));
                }
            }

            // Eliminar subgrupos de grupos
            var gruposFinales = listaFinal3punto.Values.ToList();
            for (int i = 0; i < gruposFinales.Count; i++)
            {
                var grupoActual = gruposFinales[i];
                bool esSubgrupo = false;
                for (int j = 0; j < gruposFinales.Count; j++)
                {
                    if (i != j)
                    {
                        var grupoComparacion = gruposFinales[j];
                        if (grupoActual.Count >= grupoComparacion.Count &&
                            grupoActual.Intersect(grupoComparacion).Count() == grupoComparacion.Count)
                        {
                            esSubgrupo = true;
                            break;
                        }
                    }
                }
                if (esSubgrupo)
                {
                    listaFinal3punto.Remove(listaFinal3punto.FirstOrDefault(x => x.Value.SequenceEqual(grupoActual)).Key);
                }
            }

            // Imprimir resultado
            foreach (var listaFinal in listaFinal3punto.Values)
            {
                Console.Write("Lista union3pto #{0}: ", listaFinal3punto.FirstOrDefault(x => x.Value.SequenceEqual(listaFinal)).Key + 1);
                Console.WriteLine(string.Join(", ", listaFinal));
            }
        }


        public void union4Punto(Dictionary<int, List<string>> listaFinal3punto, Dictionary<int, List<string>> palabrasRelacionadas)
        {
            List<int> keysToRemove = new List<int>();

            // Iterar sobre cada lista en listaFinal3punto
            foreach (var lista in listaFinal3punto.Values)
            {
                bool found = false;

                // Iterar sobre cada lista en palabrasRelacionadas
                foreach (var listaRelacionada in palabrasRelacionadas.Values)
                {
                    int sharedWordsCount = 0;

                    // Contar cuántas palabras en común tienen las dos listas
                    foreach (var palabra in lista)
                    {
                        if (listaRelacionada.Contains(palabra))
                        {
                            sharedWordsCount++;
                        }
                    }

                    // Si encontramos una lista en palabrasRelacionadas que comparta al menos dos palabras con lista, agregamos todas las palabras de lista a esa lista
                    if (sharedWordsCount >= compartirPalabrasEnRelaciones)
                    {
                        found = true;

                        foreach (var nuevaPalabra in lista)
                        {
                            if (!listaRelacionada.Contains(nuevaPalabra))
                            {
                                listaRelacionada.Add(nuevaPalabra);
                            }
                        }

                        break;
                    }
                }

                // Si no encontramos ninguna lista en palabrasRelacionadas que comparta al menos dos palabras con lista, agregamos lista a palabrasRelacionadas
                if (!found)
                {
                    palabrasRelacionadas.Add(palabrasRelacionadas.Count + 1, new List<string>(lista));
                }
            }

            // Eliminar subgrupos de grupos
            var gruposFinales = palabrasRelacionadas.Values.ToList();
            for (int i = 0; i < gruposFinales.Count; i++)
            {
                var grupoActual = gruposFinales[i];
                bool esSubgrupo = false;
                for (int j = 0; j < gruposFinales.Count; j++)
                {
                    if (i != j)
                    {
                        var grupoComparacion = gruposFinales[j];
                        if (grupoActual.Count >= grupoComparacion.Count &&
                            grupoActual.Intersect(grupoComparacion).Count() == grupoComparacion.Count)
                        {
                            esSubgrupo = true;
                            break;
                        }
                    }
                }
                if (esSubgrupo)
                {
                    palabrasRelacionadas.Remove(palabrasRelacionadas.FirstOrDefault(x => x.Value.SequenceEqual(grupoActual)).Key);
                }
            }

            // Eliminar listas vacías de palabrasRelacionadas
            foreach (var kvp in palabrasRelacionadas)
            {
                if (kvp.Value.Count == 0)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (int key in keysToRemove)
            {
                palabrasRelacionadas.Remove(key);
            }

            // Imprimir resultado
            foreach (var kvp in palabrasRelacionadas)
            {
                Console.Write("Lista union4pto #{0}: ", kvp.Key);
                Console.WriteLine(string.Join(", ", kvp.Value));
            }

            listaFinal4punto = new Dictionary<int, List<string>>(palabrasRelacionadas);
        }

    }
}
