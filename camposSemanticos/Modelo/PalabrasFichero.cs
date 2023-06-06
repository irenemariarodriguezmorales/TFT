using camposSemanticos.Almacenamiento;
using camposSemanticos.Control;
using camposSemanticos.ServicioLematizacion;
using ProcesarTextos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace camposSemanticos.Modelo
{
    public class PalabrasFichero
    {
        private string ruta;
        ServicioLematizacionClient servicioLematizacion;
        private ProcesarTextos.Text texto;
        private List<String> listaPalabrasFichero;
        private const int sustantivo = 1000;
        private const int verbo = 3000;
        private const int adjetivo = 1100;
        private string radioButtonMarcado;
        private CargaDiccionarios cargaDiccionarios;
        private HashSet<string> palabrasNoDeseadas = new HashSet<string> { "a", "e", "i", "o", "u", "ser", "estar", "tener", "hacer", "ir", "ver", "dar",
            "decir", "poder", "querer", "saber", "haber", "abordar", "acudir", "adquirir", "afectar", "agradecer", "alcanzar", "alinear", "aplicar",
            "apreciar", "aprobar", "asistir", "atender", "atraer", "aumentar", "averiguar", "cambiar", "causar", "celebrar", "comenzar", "comunicar",
            "conocer", "considerar", "construir", "contar", "crear", "cumplir", "debatir", "decidir", "definir", "demostrar", "descubrir", "describir",
            "desempeñar", "destacar", "determinar", "desarrollar", "difundir", "dirigir", "disminuir", "distribuir", "divulgar", "durar", "ejecutar",
            "elaborar", "emitir", "emplear", "encontrar", "entender", "establecer", "evaluar", "examinar", "explicar", "expresar", "facilitar",
            "fortalecer", "generar", "gestionar", "identificar", "implementar", "impulsar", "incluir", "incrementar", "indicar", "informar", "iniciar",
            "innovar", "inscribir", "invertir", "investigar", "lograr", "mantener", "mejorar", "motivar", "negociar", "obtener", "ofrecer", "organizar",
            "participar", "perder", "perfeccionar", "planificar", "potenciar", "preparar", "presentar", "presidir", "producir", "promover", "propiciar",
            "proponer", "proteger", "publicar", "realizar", "recibir", "reconocer", "recopilar", "reducir", "referir", "reforzar", "regular", "relcionar",
            "resolver", "responer", "resultar", "revisar", "satisfacer", "seleccionar", "solucionar", "sostener", "suponer", "sustentar", "trabajar",
            "transformar", "transmitir", "utilizar", "validar", "valorar", "vender", "venir", "visitar"};

        private FormInicial formInicial;
        private FormBarraCarga formBarraCarga;
        private FormResultado formResultado;

        public PalabrasFichero (FormInicial formInicial, string radioButtonMarcado, CargaDiccionarios cargaDiccionarios, string ruta)
        {
            this.formInicial = formInicial;
            this.servicioLematizacion = new ServicioLematizacionClient("BasicHttpsBinding_IServicioLematizacion");
            this.listaPalabrasFichero = new List<String>();
            this.radioButtonMarcado = radioButtonMarcado;
            this.cargaDiccionarios = cargaDiccionarios;
            this.ruta = ruta;
        }

        public void iniciar()
        {
            // Cargar barra de progreso
            this.formBarraCarga = new FormBarraCarga();

            // Lanza hilo secundario 
            formBarraCarga.Shown += (sender, e) =>
            {
                // Hilo secundario
                Task.Run(() =>
                {
                    // Obtener contenido de fichero
                    String[] files = Directory.GetFiles(ruta);
                    String content = "";
                    foreach (String filename in files)
                    {
                        content += System.IO.File.ReadAllText(filename);
                    }
                    obtenerPalabras(content);

                    // Actualizar barra de progreso
                    gestionarBarraCarga(100);

                    var taskWordnet = Task.Run(() =>
                    {
                        Wordnet wordnet = new Wordnet(listaPalabrasFichero);
                        return wordnet.iniciarOperacionWordnet();
                    });

                    gestionarBarraCarga(150);

                    var taskDiccionarioSinAnton = Task.Run(() =>
                    {
                        DiccionarioSinonimoAntonimo diccionarioSinonAnton = new DiccionarioSinonimoAntonimo(listaPalabrasFichero, cargaDiccionarios.DiccionarioSinonimosAntonimos);
                        return diccionarioSinonAnton.iniciarOperacionDiccionarioSinAnton();
                    });

                    // Actualizar barra de progreso
                    gestionarBarraCarga(200);

                    var taskServicioFamilias = Task.Run(() =>
                    {
                        ServicioFamiliasTratamiento servicioFam = new ServicioFamiliasTratamiento(listaPalabrasFichero, cargaDiccionarios.DiccionarioSinonimosAntonimos);
                        return servicioFam.iniciarOperacionServicioFamilias();
                    });

                    gestionarBarraCarga(250);

                    var taskDiccionarioIdeasAfines = Task.Run(() =>
                    {
                        DiccionarioIdeasAfines diccionarioIdeasAfines = new DiccionarioIdeasAfines(listaPalabrasFichero, cargaDiccionarios.DiccionarioIdeasAfinesPrimero, cargaDiccionarios.DiccionarioIdeasAfinesSegundo);
                        return diccionarioIdeasAfines.iniciarOperacionDiccionarioIdeasAfines();
                    });

                    // Actualizar barra de progreso
                    gestionarBarraCarga(300);

                    var taskDiccionarioDefiniciones = Task.Run(() =>
                    {
                        DiccionarioDefiniciones diccionarioDefiniciones;
                        if (this.radioButtonMarcado.Equals("REDPALABRAS"))
                            diccionarioDefiniciones = new DiccionarioDefiniciones(listaPalabrasFichero, cargaDiccionarios.DiccionarioDefiniciones, true);
                        else
                            diccionarioDefiniciones = new DiccionarioDefiniciones(listaPalabrasFichero, cargaDiccionarios.DiccionarioDefiniciones, false);

                        return diccionarioDefiniciones.iniciarOperacionDiccionarioDef();
                    });

                    
                    // Esperar a que todas las tareas se completen
                    Task.WaitAll(taskWordnet, taskDiccionarioSinAnton, taskServicioFamilias, taskDiccionarioIdeasAfines, taskDiccionarioDefiniciones);

                    // Obtener los resultados de las tareas
                    var listaFinalWordnet = taskWordnet.Result;
                    var listaFinalSinAnton = taskDiccionarioSinAnton.Result;
                    var listaComprobarPrimitivasDerivadas = taskServicioFamilias.Result;
                    var listaFinalIdeasAfines = taskDiccionarioIdeasAfines.Result;
                    var listaFinalDefiniciones = taskDiccionarioDefiniciones.Result;

                    // Crear las uniones
                    // Botón 1: wordnet + diccionario sinónimos y antónimos
                    Uniones union1 = new Uniones();
                    Dictionary<int, List<string>> listaFinal1 = union1.iniciarOperacionUnion1(listaFinalWordnet, listaFinalSinAnton);

                    gestionarBarraCarga(340);

                    // Botón 2: botón 1 + servicio de familias
                    Uniones union2 = new Uniones();
                    Dictionary<int, List<string>> listaFinal2 = union2.iniciarOperacionUnion2(listaFinal1, listaComprobarPrimitivasDerivadas);

                    gestionarBarraCarga(360);

                    // Botón 3: botón 2 + ideas afines
                    Uniones union3 = new Uniones();
                    Dictionary<int, List<string>> listaFinal3 = union3.iniciarOperacionUnion3(listaFinal2, listaFinalIdeasAfines);

                    gestionarBarraCarga(380);

                    // Botón 4: botón 3 + definiciones
                    Uniones union4 = new Uniones();
                    Dictionary<int, List<string>> listaFinal4 = union4.iniciarOperacionUnion4(listaFinal3, listaFinalDefiniciones);

                    // Actualizar barra de progreso
                    gestionarBarraCarga(400);

                    // Mostrar resultados en la ventana formResultado
                    formBarraCarga.Invoke(new Action(() =>
                    {
                        formResultado = new FormResultado(listaFinal1, listaFinal2, listaFinal3, listaFinal4);

                        formBarraCarga.Hide();
                        formResultado.Show();
                    }));
                    // Guardar resultados en un fichero de salida
                    GuardarSalidaEnFichero(listaFinal1, listaFinal2, listaFinal3, listaFinal4);
                });
            };

            // Camino hilo principal
            formBarraCarga.Show(); // Cuando se ejecute se lanza el hilo secundario
            formInicial.Hide();
            System.Windows.Forms.Application.DoEvents();
        }

        // Actualiza estado de la barra de carga
        private void gestionarBarraCarga(int porcentaje)
        {
            formBarraCarga.Invoke(new Action(() =>
            {
                formBarraCarga.Hide();
                formBarraCarga.setProgreso(porcentaje);
                if (porcentaje == 400)
                {
                    formBarraCarga.Hide();
                }
                else
                {
                    formBarraCarga.Show();
                }
            }));
            System.Windows.Forms.Application.DoEvents();
        }

        private void obtenerPalabras(string content)
        {
            StringReader stringReader = new StringReader(content);
            this.texto = new ProcesarTextos.Text("", content);
            List<InfoUnaFrase> frases = new List<InfoUnaFrase>(); // Lista para almacenar las frases a enviar al servicio
            int contadorFrases = 0; // Contador de frases

            Parallel.ForEach(texto.GetParagraphs(), paragraph =>
            {
                string parrafo = paragraph.getText() + "\r\n";

                Parallel.ForEach(paragraph.GetSentences(), sentence =>
                {
                    InfoUnaFrase infoUnaFrase = new InfoUnaFrase();
                    infoUnaFrase.Frase = sentence.getText();
                    lock (frases)
                    {
                        frases.Add(infoUnaFrase);
                        contadorFrases++;
                    }

                    // Verificar si se alcanzó el límite de frases por grupo (500)
                    if (contadorFrases >= 500)
                    {
                        // Enviar las frases al servicio
                        List<InfoUnaFrase> frasesSalidas = servicioLematizacion.ReconocerFrases(frases, "es", false);

                        // Procesar las frases devueltas por el servicio
                        ProcesarFrasesSalidas(frasesSalidas);

                        // Limpiar la lista de frases y reiniciar el contador
                        lock (frases)
                        {
                            frases.Clear();
                            contadorFrases = 0;
                        }
                    }
                });
            });

            // Verificar si quedan frases pendientes por enviar al servicio
            if (frases.Count > 0)
            {
                // Enviar las frases restantes al servicio
                List<InfoUnaFrase> frasesSalidas = servicioLematizacion.ReconocerFrases(frases, "es", false);

                // Procesar las frases devueltas por el servicio
                ProcesarFrasesSalidas(frasesSalidas);
            }
        }

        private void ProcesarFrasesSalidas(List<InfoUnaFrase> frasesSalidas)
        {
            Parallel.ForEach(frasesSalidas, fraseSalida =>
            {
                Parallel.ForEach(fraseSalida.Palabras, palabra =>
                {
                    if (palabra.IdCategoria == sustantivo && !listaPalabrasFichero.Contains(palabra.FormaCanonica))
                    {
                        lock (listaPalabrasFichero)
                        {
                            listaPalabrasFichero.Add(palabra.FormaCanonica);
                        }
                    }

                    if (this.radioButtonMarcado.Equals("REDPALABRAS"))
                    {
                        if (palabra.IdCategoria == verbo && !listaPalabrasFichero.Contains(palabra.FormaCanonica) && !palabrasNoDeseadas.Contains(palabra.FormaCanonica))
                        {
                            lock (listaPalabrasFichero)
                            {
                                listaPalabrasFichero.Add(palabra.FormaCanonica);
                            }
                        }
                        if (palabra.IdCategoria == adjetivo && !listaPalabrasFichero.Contains(palabra.FormaCanonica))
                        {
                            lock (listaPalabrasFichero)
                            {
                                listaPalabrasFichero.Add(palabra.FormaCanonica);
                            }
                        }
                    }
                });
            });
        }


        private void GuardarSalidaEnFichero(Dictionary<int, List<string>> listaFinal1,
                                       Dictionary<int, List<string>> listaFinal2,
                                       Dictionary<int, List<string>> listaFinal3,
                                       Dictionary<int, List<string>> listaFinal4)
        {
            string directorioSalida = Path.GetDirectoryName(ruta + "\\Salida\\");
            // Verificar si el directorio de salida no existe y luego crearlo
            if (!Directory.Exists(directorioSalida))
            {
                Directory.CreateDirectory(directorioSalida);
            }

            string fechaHora = DateTime.Now.ToString("yyyyMMddHHmmss");
            string rutaFichero = ruta + "\\Salida\\" + fechaHora + ".txt";

            using (StreamWriter sw = new StreamWriter(rutaFichero))
            {
                sw.WriteLine("Resultados botón 1:");
                ImprimirLista(sw, listaFinal1);

                sw.WriteLine();
                sw.WriteLine("Resultados botón 2:");
                ImprimirLista(sw, listaFinal2);

                sw.WriteLine();
                sw.WriteLine("Resultados botón 3:");
                ImprimirLista(sw, listaFinal3);

                sw.WriteLine();
                sw.WriteLine("Resultados botón 4:");
                ImprimirLista(sw, listaFinal4);
            }
        }

        private void ImprimirLista(StreamWriter sw, Dictionary<int, List<string>> lista)
        {
            foreach (var item in lista)
            {
                sw.WriteLine($"Lista #{item.Key}: {string.Join(", ", item.Value)}");
            }
        }


    }
}
