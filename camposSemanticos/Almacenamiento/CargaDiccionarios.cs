using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camposSemanticos.Almacenamiento
{
    public class CargaDiccionarios
    {
        private List<String> diccionarioSinonimosAntonimos;
        private List<String> diccionarioIdeasAfinesPrimero;
        private List<String> diccionarioIdeasAfinesSegundo;
        private List<String> diccionarioDefiniciones;

        public CargaDiccionarios() { 
            diccionarioSinonimosAntonimos = new List<String>();
            diccionarioIdeasAfinesPrimero = new List<String>();
            diccionarioIdeasAfinesSegundo= new List<String>();
            DiccionarioDefiniciones= new List<String>();
        }

        public List<string> DiccionarioSinonimosAntonimos { get => diccionarioSinonimosAntonimos; set => diccionarioSinonimosAntonimos = value; }
        public List<string> DiccionarioIdeasAfinesPrimero { get => diccionarioIdeasAfinesPrimero; set => diccionarioIdeasAfinesPrimero = value; }
        public List<string> DiccionarioIdeasAfinesSegundo { get => diccionarioIdeasAfinesSegundo; set => diccionarioIdeasAfinesSegundo = value; }
        public List<string> DiccionarioDefiniciones { get => diccionarioDefiniciones; set => diccionarioDefiniciones = value; }
        

        public void cargarDiccionarios()
        {
            //Se llaman funciones auxiliares para hacer la carga de los diccionarios
            cargarDiccionarioSinonimosAntonimos();
            cargarDiccionarioIdeasAfinesPrimero();
            cargarDiccionarioIdeasAfinesSegundo();
            cargarDiccionarioIdeasDefiniciones();           
        }

        private void cargarDiccionarioSinonimosAntonimos()
        {

            using (StreamReader sr = new StreamReader("..\\..\\Recursos\\diccionario sinonimos antonimos.txt"))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    diccionarioSinonimosAntonimos.Add(linea);
                }
            }

        }

        private void cargarDiccionarioIdeasAfinesPrimero()
        {
            using (StreamReader sr = new StreamReader("..\\..\\Recursos\\Diccionario_Afines_Limpio.txt"))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    diccionarioIdeasAfinesPrimero.Add(linea);
                }
            }
        }

        private void cargarDiccionarioIdeasAfinesSegundo()
        {
            using (StreamReader sr = new StreamReader("..\\..\\Recursos\\Diccionario ideas 2.txt"))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    diccionarioIdeasAfinesSegundo.Add(linea);
                }
            }
        }

        private void cargarDiccionarioIdeasDefiniciones()
        {
            using (StreamReader sr = new StreamReader("..\\..\\Recursos\\Todos_los_diccionarios_lematizados.txt"))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    diccionarioDefiniciones.Add(linea);
                }
            }
        }

    }
}
