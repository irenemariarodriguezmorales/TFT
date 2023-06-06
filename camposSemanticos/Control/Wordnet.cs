using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camposSemanticos.Control
{
    public class Wordnet
    {
        private Dictionary<int, List<string>> listasRelacionadasSinonimos;
        private Dictionary<int, List<string>> listasRelacionadasAntonimos;
        private Dictionary<int, List<string>> listasRelacionadasHiperHipo;
        private Dictionary<int, List<string>> listaFinalWordnet;
        private List<String> listaPalabrasFichero;

        public Wordnet(List<String> listaPalabrasFichero) { 
            this.listaPalabrasFichero= listaPalabrasFichero;
            this.listasRelacionadasSinonimos = new Dictionary<int, List<string>>();
            this.listasRelacionadasAntonimos = new Dictionary<int, List<string>>();
            this.listasRelacionadasHiperHipo = new Dictionary<int, List<string>>();
            this.listaFinalWordnet = new Dictionary<int, List<string>>();
        }

        public Dictionary<int, List<string>> iniciarOperacionWordnet()
        {
            // Crear tres tareas para los tres primeros métodos
            Task taskSinonimos = Task.Run(() => wordnetSinonimos());
            Task taskAntonimos = Task.Run(() => wordnetAntonimos());
            Task taskHiperonimosHiponimos = Task.Run(() => wordnetHiperonimosHiponimos());

            // Esperar a que todas las tareas finalicen
            Task.WaitAll(taskSinonimos, taskAntonimos, taskHiperonimosHiponimos);

            // Ejecutar los dos últimos métodos
            unionSinonimosAntonimosHiperHipo();

            return this.listaFinalWordnet;
        }


        private void wordnetSinonimos()
        {
            int listaRelacionadaIndex = 1;
            var processedWords = new HashSet<string>();

            Parallel.ForEach(listaPalabrasFichero, (argument, state, i) =>
            {
                if (processedWords.Contains(argument)) return;

                string scriptPath = "..\\..\\Recursos\\sinon.py";

                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "python.exe";
                start.Arguments = $"{scriptPath} \"{argument}\"";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.CreateNoWindow = true;

                using (Process process = Process.Start(start))
                {
                    using (System.IO.StreamReader reader = process.StandardOutput)
                    {
                        string synonymResult = reader.ReadToEnd();

                        // Comprueba si alguno de los sinónimos aparecen en el resto de la lista
                        string[] synonyms = synonymResult.Split(',');
                        List<string> listaSinonimos = new List<string>();
                        listaSinonimos.Add(argument); // Se añade la palabra original

                        // Agrega todas las palabras que sean sinónimos de al menos una palabra de la lista original
                        for (int k = 0; k < synonyms.Length; k++)
                        {
                            string sinonimo = synonyms[k].Trim();
                            if (!processedWords.Contains(sinonimo))
                            {
                                for (int j = (int)i + 1; j < listaPalabrasFichero.Count; j++)
                                {
                                    string otherArgument = listaPalabrasFichero[j];
                                    if (sinonimo == otherArgument)
                                    {
                                        listaSinonimos.Add(otherArgument);
                                        processedWords.Add(otherArgument);
                                    }
                                }
                            }
                        }

                        // Si la lista de sinónimos no está vacía, se añade a la lista de listas relacionadas
                        if (listaSinonimos.Count > 1)
                        {
                            bool existeListaRelacionada = false;
                            int listaRelacionadaKey = -1;

                            // Se comprueba si la lista relacionada ya existe en el diccionario de listas relacionadas
                            foreach (KeyValuePair<int, List<string>> kvp in listasRelacionadasSinonimos)
                            {
                                if (listaSinonimos.All(kvp.Value.Contains) && listaSinonimos.Count == kvp.Value.Count)
                                {
                                    existeListaRelacionada = true;
                                    listaRelacionadaKey = kvp.Key;
                                    break;
                                }
                            }

                            // Si la lista relacionada no existe en el diccionario de listas relacionadas, se añade
                            if (!existeListaRelacionada)
                            {
                                lock (listasRelacionadasSinonimos)
                                {
                                    listasRelacionadasSinonimos.Add(listaRelacionadaIndex, listaSinonimos);
                                    listaRelacionadaIndex++;
                                }
                            }
                        }
                    }
                }
            });

            // Imprime las listas relacionadas
            foreach (KeyValuePair<int, List<string>> kvp in listasRelacionadasSinonimos)
            {
                Console.WriteLine("Lista relacionada sinonimos wordnet #" + kvp.Key + ": " + string.Join(", ", kvp.Value));
            }
        }

        private void wordnetAntonimos()
        {
            int listaRelacionadaIndex = 1;
            var processedWords = new HashSet<string>();

            Parallel.ForEach(listaPalabrasFichero, (argument, state, i) =>
            {
                if (processedWords.Contains(argument)) return;

                string scriptPath = "..\\..\\Recursos\\anton.py";

                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "python.exe";
                start.Arguments = $"{scriptPath} \"{argument}\"";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.CreateNoWindow = true;

                using (Process process = Process.Start(start))
                {
                    using (System.IO.StreamReader reader = process.StandardOutput)
                    {
                        string antonymResult = reader.ReadToEnd();

                        // Comprueba si alguno de los antónimos aparecen en el resto de la lista
                        string[] antonyms = antonymResult.Split(',');
                        List<string> listaAntonimos = new List<string>();
                        listaAntonimos.Add(argument); // Se añade la palabra original

                        // Agrega todas las palabras que sean antónimos de al menos una palabra de la lista original
                        for (int k = 0; k < antonyms.Length; k++)
                        {
                            string antonimo = antonyms[k].Trim();
                            if (!processedWords.Contains(antonimo))
                            {
                                for (int j = (int)i + 1; j < listaPalabrasFichero.Count; j++)
                                {
                                    string otherArgument = listaPalabrasFichero[j];
                                    if (antonimo == otherArgument)
                                    {
                                        listaAntonimos.Add(otherArgument);
                                        processedWords.Add(otherArgument);
                                    }
                                }
                            }
                        }

                        // Si la lista de antónimos no está vacía, se añade a la lista de listas relacionadas
                        if (listaAntonimos.Count > 1)
                        {
                            bool existeListaRelacionada = false;
                            int listaRelacionadaKey = -1;

                            // Se comprueba si la lista relacionada ya existe en el diccionario de listas relacionadas
                            foreach (KeyValuePair<int, List<string>> kvp in listasRelacionadasAntonimos)
                            {
                                if (listaAntonimos.All(kvp.Value.Contains) && listaAntonimos.Count == kvp.Value.Count)
                                {
                                    existeListaRelacionada = true;
                                    listaRelacionadaKey = kvp.Key;
                                    break;
                                }
                            }

                            // Si la lista relacionada no existe en el diccionario de listas relacionadas, se añade
                            if (!existeListaRelacionada)
                            {
                                lock (listasRelacionadasAntonimos)
                                {
                                    listasRelacionadasAntonimos.Add(listaRelacionadaIndex, listaAntonimos);
                                    listaRelacionadaIndex++;
                                }
                            }
                        }
                    }
                }
            });

            // Imprime las listas relacionadas
            foreach (KeyValuePair<int, List<string>> kvp in listasRelacionadasAntonimos)
            {
                Console.WriteLine("Lista relacionada antónimos wordnet #" + kvp.Key + ": " + string.Join(", ", kvp.Value));
            }
        }

        private void wordnetHiperonimosHiponimos()
        {
            int listaRelacionadaIndex = 1;
            var processedWords = new HashSet<string>();

            Parallel.ForEach(listaPalabrasFichero, (argument, state, i) =>
            {
                if (processedWords.Contains(argument)) return;

                string scriptPath = "..\\..\\Recursos\\hiperhipo.py";

                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = "python.exe";
                start.Arguments = $"{scriptPath} \"{argument}\"";
                start.UseShellExecute = false;
                start.RedirectStandardOutput = true;
                start.CreateNoWindow = true;

                using (Process process = Process.Start(start))
                {
                    using (System.IO.StreamReader reader = process.StandardOutput)
                    {
                        string relationResult = reader.ReadToEnd();

                        // Comprueba si alguno de los hipónimos o hiperónimos aparecen en el resto de la lista
                        string[] relations = relationResult.Split(',');
                        List<string> listaRelacionados = new List<string>();
                        listaRelacionados.Add(argument); // Se añade la palabra original

                        // Agrega todas las palabras que sean hipónimos o hiperónimos de al menos una palabra de la lista original
                        for (int k = 0; k < relations.Length; k++)
                        {
                            string relation = relations[k].Trim();
                            if (!processedWords.Contains(relation))
                            {
                                for (int j = (int)i + 1; j < listaPalabrasFichero.Count; j++)
                                {
                                    string otherArgument = listaPalabrasFichero[j];
                                    if (relation == otherArgument)
                                    {
                                        listaRelacionados.Add(otherArgument);
                                        processedWords.Add(otherArgument);
                                    }
                                }
                            }
                        }

                        // Si la lista de relacionados no está vacía, se añade a la lista de listas relacionadas
                        if (listaRelacionados.Count > 1)
                        {
                            bool existeListaRelacionada = false;
                            int listaRelacionadaKey = -1;

                            // Se comprueba si la lista relacionada ya existe en la lista de listas relacionadas
                            foreach (KeyValuePair<int, List<string>> kvp in listasRelacionadasHiperHipo)
                            {
                                if (listaRelacionados.All(kvp.Value.Contains) && listaRelacionados.Count == kvp.Value.Count)
                                {
                                    existeListaRelacionada = true;
                                    listaRelacionadaKey = kvp.Key;
                                    break;
                                }
                            }

                            // Si la lista relacionada no existe en la lista de listas relacionadas, se añade
                            if (!existeListaRelacionada)
                            {
                                lock (listasRelacionadasHiperHipo)
                                {
                                    listasRelacionadasHiperHipo.Add(listaRelacionadaIndex, listaRelacionados);
                                    listaRelacionadaIndex++;
                                }
                            }
                        }
                    }
                }
            });

            // Imprime las listas relacionadas
            foreach (KeyValuePair<int, List<string>> kvp in listasRelacionadasHiperHipo)
            {
                Console.WriteLine("Lista relacionada hiperónimos/hipónimos wordnet #" + kvp.Key + ": " + string.Join(", ", kvp.Value));
            }
        }

        public void unionSinonimosAntonimosHiperHipo()
        {

            foreach (var kvpSinonimos in listasRelacionadasSinonimos)
            {
                List<string> listaFinalEncontrada = null;
                foreach (var kvpListaFinal in listaFinalWordnet)
                {
                    var palabrasEncontradas = 0;
                    foreach (var palabra in kvpSinonimos.Value)
                    {
                        if (kvpListaFinal.Value.Contains(palabra))
                        {
                            palabrasEncontradas++;
                        }
                    }

                    if (palabrasEncontradas >= 2)
                    {
                        listaFinalEncontrada = kvpListaFinal.Value;
                        break;
                    }
                }

                if (listaFinalEncontrada != null)
                {
                    listaFinalEncontrada.AddRange(kvpSinonimos.Value.Except(listaFinalEncontrada));
                }
                else
                {
                    int newKey = listaFinalWordnet.Keys.Count > 0 ? listaFinalWordnet.Keys.Max() + 1 : 1;
                    listaFinalWordnet.Add(newKey, kvpSinonimos.Value);
                }
            }

            foreach (var kvpAntonimos in listasRelacionadasAntonimos)
            {
                List<string> listaFinalEncontrada = null;
                foreach (var kvpListaFinal in listaFinalWordnet)
                {
                    var palabrasEncontradas = 0;
                    foreach (var palabra in kvpAntonimos.Value)
                    {
                        if (kvpListaFinal.Value.Contains(palabra))
                        {
                            palabrasEncontradas++;
                        }
                    }

                    if (palabrasEncontradas >= 2)
                    {
                        listaFinalEncontrada = kvpListaFinal.Value;
                        break;
                    }
                }

                if (listaFinalEncontrada != null)
                {
                    listaFinalEncontrada.AddRange(kvpAntonimos.Value.Except(listaFinalEncontrada));
                }
                else
                {
                    int newKey = listaFinalWordnet.Keys.Count > 0 ? listaFinalWordnet.Keys.Max() + 1 : 1;
                    listaFinalWordnet.Add(newKey, kvpAntonimos.Value);
                }
            }

            foreach (var kvpHiperHipo in listasRelacionadasHiperHipo)
            {
                List<string> listaFinalEncontrada = null;
                foreach (var kvpListaFinal in listaFinalWordnet)
                {
                    var palabrasEncontradas = 0;
                    foreach (var palabra in kvpHiperHipo.Value)
                    {
                        if (kvpListaFinal.Value.Contains(palabra))
                        {
                            palabrasEncontradas++;
                        }
                    }

                    if (palabrasEncontradas >= 2)
                    {
                        listaFinalEncontrada = kvpListaFinal.Value;
                        break;
                    }
                }

                if (listaFinalEncontrada != null)
                {
                    listaFinalEncontrada.AddRange(kvpHiperHipo.Value.Except(listaFinalEncontrada));
                }
                else
                {
                    int newKey = listaFinalWordnet.Keys.Count > 0 ? listaFinalWordnet.Keys.Max() + 1 : 1;
                    listaFinalWordnet.Add(newKey, kvpHiperHipo.Value);
                }
            }

            int listaNum = 1;
            foreach (var kvpListaFinal in listaFinalWordnet)
            {
                Console.WriteLine("Lista relacionada aux #{0}: {1}", kvpListaFinal.Key, string.Join(", ", kvpListaFinal.Value));
                listaNum++;
            }
        }
    }
}
