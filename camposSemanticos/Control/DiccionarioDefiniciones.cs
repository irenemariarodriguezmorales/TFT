using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camposSemanticos.Control
{
    public class DiccionarioDefiniciones
    {
        private List<string> listaPalabrasFichero;
        private List<string> diccionarioDefiniciones;
        private Dictionary<int, List<string>> listasRelacionadasDefiniciones;
        private Boolean estaMarcadoRadioRedPalabras;
        private int palabrasComunesEnDefiniciones = 15; // Indica las palabras comunes que existen entre definiciones de palabras que se están tratando

        public DiccionarioDefiniciones(List<string> listaPalabrasFichero, List<string> diccionarioDefiniciones, bool estaMarcadoRadioRedPalabras)
        {
            this.listaPalabrasFichero = listaPalabrasFichero;
            this.diccionarioDefiniciones= diccionarioDefiniciones;
            this.listasRelacionadasDefiniciones = new Dictionary<int, List<string>>();
            this.estaMarcadoRadioRedPalabras = estaMarcadoRadioRedPalabras;
        }

        public Dictionary<int, List<string>> iniciarOperacionDiccionarioDef()
        {
            operacionDiccionarioDefiniciones();
            return this.listasRelacionadasDefiniciones;
        }


        private void operacionDiccionarioDefiniciones()
        {
            string contenidoArchivo = string.Join(Environment.NewLine, diccionarioDefiniciones);

            // Dividir el contenido del archivo en definiciones separadas
            string[] definiciones = contenidoArchivo.Split(new[] { "###" }, StringSplitOptions.RemoveEmptyEntries);

            // Recorrer cada palabra en la lista de palabras del archivo
            foreach (string palabra in listaPalabrasFichero.Distinct())
            {
                // Crear una lista para almacenar las palabras relacionadas con la palabra actual
                List<string> listasRelacionadasDefinicionesActual = new List<string>();

                // Buscar la palabra en las definiciones
                foreach (string definicion in definiciones)
                {
                    if (definicion.Contains(palabra))
                    {
                        // Extraer las palabras que tienen los códigos :1000, :1100 y :3000
                        List<string> palabrasCodigo = new List<string>();

                        foreach (string linea in definicion.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (linea.Contains(":1000"))
                            {
                                string[] partesLinea = linea.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                                if (partesLinea.Length >= 2)
                                {
                                    string codigo = partesLinea[0].Trim();

                                    // Verificar si la palabra código está en la lista de palabras del archivo
                                    if (listaPalabrasFichero.Contains(codigo) && codigo != palabra)
                                    {
                                        palabrasCodigo.Add(codigo);
                                    }
                                }
                            }


                            if (this.estaMarcadoRadioRedPalabras)
                            {
                                if (linea.Contains(":1100") || linea.Contains(":3000"))
                                {
                                    string[] partesLinea = linea.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                                    if (partesLinea.Length >= 2)
                                    {
                                        string codigo = partesLinea[0].Trim();

                                        // Verificar si la palabra código está en la lista de palabras del archivo
                                        if (listaPalabrasFichero.Contains(codigo) && codigo != palabra)
                                        {
                                            palabrasCodigo.Add(codigo);
                                        }
                                    }
                                }
                            }
                        }

                        // Contar cuántas palabras en común hay entre las palabras código y la palabra actual
                        int palabrasEnComun = palabrasCodigo.Count(palabras => definicion.Contains(palabras));

                        // Si hay al menos X palabras en común, agregar la palabra actual y las palabras código a la lista actual
                        if (palabrasEnComun >= palabrasComunesEnDefiniciones)
                        {
                            listasRelacionadasDefinicionesActual.Add(palabra);
                            listasRelacionadasDefinicionesActual.AddRange(palabrasCodigo);
                        }
                    }
                }

                // Si la lista actual contiene más de una palabra, agregarla al diccionario de palabras relacionadas
                if (listasRelacionadasDefinicionesActual.Count > 1)
                {
                    listasRelacionadasDefinicionesActual.Sort(); // Ordenar la lista de palabras relacionadas
                    listasRelacionadasDefiniciones[listasRelacionadasDefiniciones.Count + 1] = listasRelacionadasDefinicionesActual.Distinct().ToList();
                }
            }

            // Imprimir las relaciones encontradas
            Console.WriteLine("Relaciones encontradas:");

            foreach (var relacion in listasRelacionadasDefiniciones)
            {
                Console.WriteLine("Relación #{0}: {1}", relacion.Key, string.Join(", ", relacion.Value));
            }
        }


    }
}
