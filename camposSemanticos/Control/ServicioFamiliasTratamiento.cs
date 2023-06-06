using camposSemanticos.ServicioFamilias;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace camposSemanticos.Control
{
    public class ServicioFamiliasTratamiento
    {
        private List<string> listaPalabrasFichero;
        private Dictionary<int, List<string>> listaComprobarPrimitivasDerivadas;
        ServicioFamiliasClient servicioFamilias;
        private Dictionary<int, List<string>> listasRelacionadasDiccionarioSinAnton2;
        private List<string> diccionarioSinonimosAntonimos;

        public ServicioFamiliasTratamiento(List<string> listaPalabrasFichero, List<string> diccionarioSinonimosAntonimos)
        {
            this.listaPalabrasFichero = listaPalabrasFichero;
            this.listaComprobarPrimitivasDerivadas = new Dictionary<int, List<string>>();
            this.servicioFamilias = new ServicioFamiliasClient("BasicHttpsBinding_IServicioFamilias");
            this.listasRelacionadasDiccionarioSinAnton2 = new Dictionary<int, List<string>>();
            this.diccionarioSinonimosAntonimos = diccionarioSinonimosAntonimos;
        }

        public Dictionary<int, List<string>> iniciarOperacionServicioFamilias()
        {
            Task sfTask = Task.Run(() => servicioFamiliasBuscarDerPrim());
            Task diccionarioTask = Task.Run(() => servicioFamiliasBuscarEnDiccionarioDerPrim());

            Task.WaitAll(sfTask, diccionarioTask);

            unionListaComprobacionConDiccionarioSinAnton(); // Unir primitivas y derivadas encontradas en SF + diccionario y guardar en listaComprobarPrimitivasDerivadas
            return listaComprobarPrimitivasDerivadas;
        }


        private void servicioFamiliasBuscarDerPrim()
        {
            // Diccionarios para almacenar las primitivas y derivadas de cada palabra
            Dictionary<string, List<string>> primitivas = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> derivadas = new Dictionary<string, List<string>>();

            // Busca las primitivas y derivadas de cada palabra
            int index = 1;
            foreach (string word in listaPalabrasFichero)
            {
                // Busca las primitivas y derivadas de la palabra actual
                List<ServicioFamilias.InfoPrimitiva> familiaPrimitiva = servicioFamilias.ConsultaPrimitivas(word);
                List<ServicioFamilias.InfoDerivada> familiaDerivada = servicioFamilias.ConsultaDerivadas(word);

                // Obtiene las palabras primitivas y las agrega a primitivasList si familiaPrimitiva no es nulo
                List<string> primitivasList = familiaPrimitiva != null ? familiaPrimitiva.Select(p => p.Primitiva).ToList() : new List<string>();

                // Obtiene las palabras derivadas y las agrega a derivadasList si familiaDerivada no es nulo
                List<string> derivadasList = familiaDerivada != null ? familiaDerivada.Select(d => d.Derivada).ToList() : new List<string>();

                // Busca palabras relacionadas y las agrega a relacionadasList
                List<string> relacionadasList = new List<string>();

                // Agrega la palabra actual al principio de relacionadasList
                relacionadasList.Add(word);

                // Verifica si hay alguna palabra relacionada en la lista de palabras primitivas o derivadas
                foreach (string primitiva in primitivasList)
                {
                    if (listaPalabrasFichero.Contains(primitiva) && !relacionadasList.Contains(primitiva) && !primitivasList.Any(p => relacionadasList.Contains(p)))
                    {
                        relacionadasList.Add(primitiva);
                    }
                }

                foreach (string derivada in derivadasList)
                {
                    if (listaPalabrasFichero.Contains(derivada) && !relacionadasList.Contains(derivada) && !derivadasList.Any(d => relacionadasList.Contains(d)))
                    {
                        relacionadasList.Add(derivada);
                    }
                }

                // Verifica si la lista relacionada ya existe en el diccionario
                bool listaExistente = false;
                foreach (KeyValuePair<int, List<string>> relacionada in listaComprobarPrimitivasDerivadas)
                {
                    if (SonListasIguales(relacionada.Value, relacionadasList))
                    {
                        listaExistente = true;
                        break;
                    }
                }

                // Agrega relacionadasList a listaComprobarPrimitivasDerivadas si no existe una lista igual y tiene más de un elemento
                if (!listaExistente && relacionadasList.Count > 1)
                {
                    listaComprobarPrimitivasDerivadas[index] = relacionadasList;
                    index++;
                }
            }

            // Imprime los resultados
            foreach (KeyValuePair<int, List<string>> relacionada in listaComprobarPrimitivasDerivadas)
            {
                Console.WriteLine("Lista relacionada SF #" + relacionada.Key + ": " + string.Join(", ", relacionada.Value));
            }
        }

        // Función para verificar si dos listas son iguales independientemente del orden
        private bool SonListasIguales(List<string> lista1, List<string> lista2)
        {
            if (lista1.Count != lista2.Count)
            {
                return false;
            }

            foreach (string palabra in lista1)
            {
                if (!lista2.Contains(palabra))
                {
                    return false;
                }
            }

            return true;
        }

        private void servicioFamiliasBuscarEnDiccionarioDerPrim()
        {
            // Itera de forma paralela sobre la lista de palabras del fichero
            Parallel.ForEach(listaPalabrasFichero, word =>
            {
                // Consulta las familias primitivas y derivadas para la palabra actual
                List<ServicioFamilias.InfoPrimitiva> familiaPrimitiva = servicioFamilias.ConsultaPrimitivas(word);
                List<ServicioFamilias.InfoDerivada> familiaDerivada = servicioFamilias.ConsultaDerivadas(word);

                // Si existen familias primitivas para la palabra actual
                if (familiaPrimitiva != null)
                {
                    // Itera de forma paralela sobre las familias primitivas
                    Parallel.ForEach(familiaPrimitiva, info =>
                    {
                        // Llama al método diccionarioSinonimosAntonimos2 con los parámetros correspondientes
                        diccionarioSinonimosAntonimos2(info.Primitiva, word, listaPalabrasFichero, listasRelacionadasDiccionarioSinAnton2);
                    });
                }

                // Si existen familias derivadas para la palabra actual
                if (familiaDerivada != null)
                {
                    // Itera de forma paralela sobre las familias derivadas
                    Parallel.ForEach(familiaDerivada, info =>
                    {
                        // Llama al método diccionarioSinonimosAntonimos2 con los parámetros correspondientes
                        diccionarioSinonimosAntonimos2(info.Derivada, word, listaPalabrasFichero, listasRelacionadasDiccionarioSinAnton2);
                    });
                }
            });

            // Imprime las listas relacionadas de sinónimos al finalizar el recorrido de todas las palabras
            for (int i = 0; i < listasRelacionadasDiccionarioSinAnton2.Count; i++)
            {
                Console.WriteLine("Lista relacionada sinónimos DICCIONARIO SF #" + (i + 1) + ": " + string.Join(", ", listasRelacionadasDiccionarioSinAnton2[i]));
            }
        }


        private void diccionarioSinonimosAntonimos2(string pal, string word, List<string> listaPalabrasFichero, Dictionary<int, List<string>> listasRelacionadasDiccionarioSinAnton2)
        {
                
            foreach (String line in diccionarioSinonimosAntonimos)
            {
                if (line.StartsWith(pal))
                {
                    string[] words = line.Split(' ');
                    List<string> sinonimos = new List<string>();
                    for (int i = 1; i < words.Length; i++)
                    {
                        sinonimos.Add(words[i]);
                    }

                    bool currentWordFound = listaPalabrasFichero.Contains(pal);

                    List<string> relacionados = new List<string>();
                    foreach (string sinonimo in sinonimos)
                    {
                        if (listaPalabrasFichero.Contains(sinonimo))
                        {
                            relacionados.Add(sinonimo);
                        }
                    }

                    if (currentWordFound && relacionados.Count > 0)
                    {
                        relacionados.Insert(0, pal);
                        listasRelacionadasDiccionarioSinAnton2.Add(listasRelacionadasDiccionarioSinAnton2.Count, relacionados);
                    }
                }
            }
            
        }

        public void unionListaComprobacionConDiccionarioSinAnton()
        {
            Parallel.ForEach(listasRelacionadasDiccionarioSinAnton2, kvp =>
            {
                bool found = false;
                Parallel.ForEach(listaComprobarPrimitivasDerivadas, kvp2 =>
                {
                    int matchingElements = 0;
                    Parallel.ForEach(kvp.Value, palabra =>
                    {
                        if (kvp2.Value.Contains(palabra))
                        {
                            Interlocked.Increment(ref matchingElements);
                        }
                    });
                    if (matchingElements == 2)
                    {
                        found = true;
                        Parallel.ForEach(kvp.Value, palabra =>
                        {
                            if (!kvp2.Value.Contains(palabra))
                            {
                                lock (kvp2.Value)
                                {
                                    kvp2.Value.Add(palabra);
                                }
                            }
                        });
                    }
                });

                if (!found)
                {
                    lock (listaComprobarPrimitivasDerivadas)
                    {
                        listaComprobarPrimitivasDerivadas.Add(listaComprobarPrimitivasDerivadas.Count + 1, kvp.Value);
                    }
                }
            });

            // Eliminamos las comas después de la última palabra
            Parallel.ForEach(listaComprobarPrimitivasDerivadas, kvp =>
            {
                string lastWord = kvp.Value.LastOrDefault();
                if (lastWord != null)
                {
                    kvp.Value[kvp.Value.Count - 1] = lastWord.TrimEnd(',');
                }
            });

            Parallel.ForEach(listaComprobarPrimitivasDerivadas, kvp =>
            {
                Console.Write("Lista unionAuxAux2pto SF #{0}: ", kvp.Key);
                Console.WriteLine(string.Join(", ", kvp.Value));
            });
        }


    }
}
