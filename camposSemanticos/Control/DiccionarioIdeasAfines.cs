using camposSemanticos.ServicioFamilias;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camposSemanticos.Control
{
    public class DiccionarioIdeasAfines
    {
        private List<string> listaPalabrasFichero;
        private List<string> diccionarioIdeasAfinesPrimero;
        private List<string> diccionarioIdeasAfinesSegundo;
        private Dictionary<int, List<string>> relacionesIdeasAfines;
        private Dictionary<int, List<string>> relacionesIdeasAfines2;
        private Dictionary<int, List<string>> listaDosDiccionariosIdeasAfines;


        public DiccionarioIdeasAfines(List<string> listaPalabrasFichero, List<string> diccionarioIdeasAfinesPrimero, List<string> diccionarioIdeasAfinesSegundo)
        {
            this.listaPalabrasFichero = listaPalabrasFichero;
            this.relacionesIdeasAfines = new Dictionary<int, List<string>>();
            this.relacionesIdeasAfines2 = new Dictionary<int, List<string>>();
            this.listaDosDiccionariosIdeasAfines = new Dictionary<int, List<string>>();
            this.diccionarioIdeasAfinesPrimero = diccionarioIdeasAfinesPrimero;
            this.diccionarioIdeasAfinesSegundo = diccionarioIdeasAfinesSegundo;
        }

        public Dictionary<int, List<string>> iniciarOperacionDiccionarioIdeasAfines()
        {
            operacionDiccionarioIdeasAfines();
            operacionDiccionarioIdeasAfines2();
            unionDosDiccionariosIdeasAfines(relacionesIdeasAfines, relacionesIdeasAfines2);
            return this.listaDosDiccionariosIdeasAfines;
        }

        private void operacionDiccionarioIdeasAfines()
        {

            string[] lineasDiccionario = this.diccionarioIdeasAfinesPrimero.ToArray();
            List<string> relacionActual = null;
            string palabraEncontrada = "";
            int indexRelacion = 1;
            foreach (string linea in lineasDiccionario)
            {
                if (relacionActual == null)
                {
                    foreach (string palabra in listaPalabrasFichero)
                    {
                        if (linea.StartsWith(palabra + "."))
                        {
                            relacionActual = new List<string>() { palabra };
                            palabraEncontrada = palabra;
                            break;
                        }
                    }
                }
                else
                {
                    if (linea.StartsWith("=========="))
                    {
                        if (relacionActual.Count > 1)
                        {
                            relacionesIdeasAfines.Add(indexRelacion, relacionActual);
                            indexRelacion++;
                        }
                        relacionActual = null;
                    }
                    else
                    {
                        string[] palabrasLinea = linea.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string palabraLinea in palabrasLinea)
                        {
                            string[] palabras = palabraLinea.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string palabra in palabras)
                            {
                                if (listaPalabrasFichero.Contains(palabra))
                                {
                                    if (relacionActual == null)
                                    {
                                        relacionActual = new List<string>() { palabraEncontrada, palabra };
                                    }
                                    else
                                    {
                                        relacionActual.Add(palabra);
                                    }
                                }
                            }
                            if (palabras.Contains(palabraEncontrada))
                            {
                                foreach (string palabra in palabras)
                                {
                                    if (listaPalabrasFichero.Contains(palabra))
                                    {
                                        if (relacionActual == null)
                                        {
                                            relacionActual = new List<string>() { palabraEncontrada, palabra };
                                        }
                                        else
                                        {
                                            relacionActual.Add(palabra);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (relacionActual != null && relacionActual.Count > 1)
            {
                relacionesIdeasAfines.Add(indexRelacion, relacionActual);
            }
            int i = 1;
            foreach (var relacion in relacionesIdeasAfines)
            {
                Console.WriteLine($"Lista relaciones ideas afines #{relacion.Key}: {string.Join(", ", relacion.Value.Distinct())}");
                i++;
            }
        }


        private void operacionDiccionarioIdeasAfines2()
        {
            string contenidoArchivo = string.Join(Environment.NewLine, diccionarioIdeasAfinesSegundo);

            // Recorremos cada palabra de listaPalabrasFichero
            for (int i = 0; i < listaPalabrasFichero.Count; i++)
            {
                string palabra = listaPalabrasFichero[i];

                // Si la palabra ya ha sido encontrada y relacionada, pasamos a la siguiente palabra de listaPalabrasFichero
                if (relacionesIdeasAfines2.Any(l => l.Value.Contains(palabra)))
                {
                    continue;
                }

                List<string> palabrasRelacionadas = new List<string>();

                // Buscamos la palabra en el archivo de texto con el formato #palabra
                string palabraBuscada = "#" + palabra;
                int indicePalabraBuscada = contenidoArchivo.IndexOf(palabraBuscada);

                // Si la palabra no se encuentra en el archivo de texto, pasamos a la siguiente palabra de listaPalabrasFichero
                if (indicePalabraBuscada == -1)
                {
                    continue;
                }

                // Buscamos el siguiente # después de la palabra buscada
                int indiceSiguienteNumeral = contenidoArchivo.IndexOf("#", indicePalabraBuscada + 1);

                // Obtenemos el contenido entre los dos #
                string contenidoEntreNumerales = contenidoArchivo.Substring(indicePalabraBuscada + palabraBuscada.Length,
                                                                             indiceSiguienteNumeral - (indicePalabraBuscada + palabraBuscada.Length));

                // Separamos el contenido por palabras y lo almacenamos en una lista
                List<string> palabrasEntreNumerales = contenidoEntreNumerales.Split(new char[] { ' ', ',', ';', '.' },
                                                                                     StringSplitOptions.RemoveEmptyEntries)
                                                                               .ToList();

                // Recorremos cada palabra encontrada entre los numerales y verificamos si coinciden con alguna palabra de listaPalabrasFichero
                foreach (string palabraEncontrada in palabrasEntreNumerales)
                {
                    if (listaPalabrasFichero.Contains(palabraEncontrada) && !palabrasRelacionadas.Contains(palabraEncontrada) && palabra != palabraEncontrada)
                    {
                        // Si la palabra coincide con alguna de listaPalabrasFichero, la añadimos a la lista de palabras relacionadas
                        palabrasRelacionadas.Add(palabraEncontrada);
                    }
                }

                // Si hay palabras relacionadas, las añadimos al diccionario y las imprimimos
                if (palabrasRelacionadas.Count > 0)
                {
                    palabrasRelacionadas.Insert(0, palabra);
                    relacionesIdeasAfines2[i + 1] = palabrasRelacionadas;
                    Console.WriteLine("Lista relaciones ideas afines2 diccionario #{0}: {1}", i + 1, string.Join(", ", palabrasRelacionadas));
                }
            }
        }


        public void unionDosDiccionariosIdeasAfines(Dictionary<int, List<string>> relacionesIdeasAfines, Dictionary<int, List<string>> relacionesIdeasAfines2)
        {
            Dictionary<int, List<string>> newLists = new Dictionary<int, List<string>>(listaDosDiccionariosIdeasAfines);

            // Iterar sobre cada lista en relacionesIdeasAfines2
            foreach (var kvp in relacionesIdeasAfines2) 
            {
                bool found = false;
                foreach (var newList in newLists)
                {
                    int sharedWordsCount = newList.Value.Intersect(kvp.Value).Count();
                    if (sharedWordsCount >= 2)
                    {
                        found = true;
                        foreach (var nuevaPalabra in kvp.Value)
                        {
                            if (!newList.Value.Contains(nuevaPalabra))
                            {
                                newList.Value.Add(nuevaPalabra);
                            }
                        }
                        break;
                    }
                }
                if (!found)
                {
                    try
                    {
                        newLists.Add(kvp.Key, new List<string>(kvp.Value));
                    }
                    catch (ArgumentException)
                    {
                        // La clave ya existe, ignorar
                    }
                }
            }

            // Eliminar las listas duplicadas y las palabras individuales
            List<List<string>> mergedLists = newLists.Values.GroupBy(x => string.Join(",", x.OrderBy(s => s))).Select(x => x.First()).Where(x => x.Count >= 2).ToList();

            // Imprimir resultado
            int i = 1;
            foreach (var list in mergedLists)
            {
                Console.WriteLine("Lista #{0}: {1}", i, string.Join(", ", list));
                i++;
            }

            listaDosDiccionariosIdeasAfines = mergedLists.Select((value, index) => new { index, value })
                                             .ToDictionary(pair => pair.index + 1, pair => pair.value);
        }


    }
}
