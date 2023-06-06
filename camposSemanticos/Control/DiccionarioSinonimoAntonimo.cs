using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camposSemanticos.Control
{
    public class DiccionarioSinonimoAntonimo
    {
        private List<string> listaPalabrasFichero;
        private List<string> diccionarioSinonimosAntonimos;
        private Dictionary<int, List<string>> listasRelacionadasDiccionarioSinAnton;

        public DiccionarioSinonimoAntonimo(List<string> listaPalabrasFichero, List<string> diccionarioSinonimosAntonimos)
        {
            this.listaPalabrasFichero = listaPalabrasFichero;
            this.listasRelacionadasDiccionarioSinAnton = new Dictionary<int, List<string>>();
            this.diccionarioSinonimosAntonimos = diccionarioSinonimosAntonimos;
        }


        public Dictionary<int, List<string>> iniciarOperacionDiccionarioSinAnton()
        {
            operacionDiccionarioSinonimosAntonimos();
            return this.listasRelacionadasDiccionarioSinAnton;
        }

        private void operacionDiccionarioSinonimosAntonimos()
        {
            int newKey = listasRelacionadasDiccionarioSinAnton.Keys.Count > 0 ? listasRelacionadasDiccionarioSinAnton.Keys.Max() + 1 : 1;
            foreach (String line in diccionarioSinonimosAntonimos)
            {
                string[] words = line.Split(' ');
                List<string> sinonimos = new List<string>();
                for (int i = 1; i < words.Length; i++)
                {
                    sinonimos.Add(words[i]);
                }
                for (int i = 0; i < listaPalabrasFichero.Count; i++)
                {
                    string word = listaPalabrasFichero[i];
                    if (line.StartsWith(word + " ") || line == word)
                    {
                        List<string> relacionados = new List<string>();
                        relacionados.Add(word);
                        foreach (string sinonimo in sinonimos)
                        {
                            if (listaPalabrasFichero.Contains(sinonimo))
                            {
                                relacionados.Add(sinonimo);
                            }
                        }
                        if (relacionados.Count > 1) // Si hay al menos dos palabras en la lista de relacionados
                        {
                            listasRelacionadasDiccionarioSinAnton.Add(newKey, relacionados);
                            newKey++;
                        }
                        break;
                    }
                }
            }
            
            foreach (var lista in listasRelacionadasDiccionarioSinAnton)
            {
                if (lista.Value.Intersect(listaPalabrasFichero).Any()) // Si la lista contiene al menos una palabra de listaPalabrasFichero
                {
                    Console.WriteLine("Lista relacionada sinónimos diccionario #" + lista.Key + ": " + string.Join(", ", lista.Value.Distinct()));
                }
            }
        }

    }
}
