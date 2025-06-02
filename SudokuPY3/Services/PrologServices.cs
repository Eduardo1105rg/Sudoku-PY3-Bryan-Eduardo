using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace SudokuPY3.Services
{
    public class PrologServices
    {

        private Process prolog; // Instancia de prolog para todo el juego.

        private List<int> TableroOrigen; // Lista original creada para el juego actual.

        private List<int> TableroIncial; // Lista con ceros creada para el juego actual.

        //private List<List<int>> MatrizDeCerosOriginal;

        private readonly RegistroServices _registroService; // Esto seria para el servicio de registro de los datos de las partidas de los juegos.

        /**
         * Nombre: PrologServices
         * 
         * Descripcion: Contructor de la clase.
         * 
         * Entradas: RegistroServices registroService: La clase de registro.
         * 
         * Salidas: No posee.
         * 
         */
        public PrologServices(RegistroServices registroService)
        {
            IniciarProlog();
            _registroService = registroService;
        }

        /**
         * Nombre: IniciarProlog
         * 
         * Descripcion: Funcion para incializar el proceso de porlog, establece la conexion inical a prolog para que se pueda hacer consultas a este.
         * 
         * Entradas: No posee.
         * 
         * Salidas: No posee.
         * 
         * Nota: Cambiar la ruta de 'Arguments' antes de usar el programa en otra computadora, debe se usar este formato: "-s \"{En esta parte va la ruta}\""
         * 
         */
        private void IniciarProlog()
        {
            if (prolog == null || prolog.HasExited)
            {
                prolog = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "swipl",
                        Arguments = "-s \"C:\\Users\\edurg\\OneDrive\\Escritorio\\Proyecto 3\\Sudoku-PY3-Bryan-Eduardo\\SudokuPY3\\Prolog\\Sudoku.pl\"",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                prolog.Start();
                Console.WriteLine("Prolog iniciado...");
            }
        }

        /**
         * Nombre: EnviarConsulta
         * 
         * Descripcion: Funcion para enviar las consultas a Prolog y recibir las respuestas que este gera
         * 
         * Entradas: string consulta: Los datos de la consulta a realizar.
         * 
         * Salidas: Un string con las respuesta a la consulta realizada.
         * 
         */
        public string EnviarConsulta(string consulta)
        {
            IniciarProlog();

            prolog.StandardInput.WriteLine(consulta);
            prolog.StandardInput.Flush();

            System.Threading.Thread.Sleep(3000);
            return prolog.StandardOutput.ReadLine();
        }

        /**
         * Nombre: CerrarProlog
         * 
         * Descripcion: Funcion para detener el proceso de prolog.
         * 
         * Entradas: No posee.
         * 
         * Salidas: No posee.
         * 
         */
        public void CerrarProlog()
        {
            if (prolog != null && !prolog.HasExited)
            {
                prolog.StandardInput.WriteLine("halt.");
                prolog.WaitForExit();
                prolog.Close();
            }
        }

        /**
         * Nombre: ObtenerMatrizSudokuV2
         * 
         * Descripcion: Funcion para solicitar el tablero de Sudoku a prolog.
         * 
         * Entradas: int tamano: Tamño de la matriz a crear.
         * 
         * Salidas: Los tableros de Sudoku generados para el juego actual, uno de esos tamblero contiene la solucion y el otro tiene elementos oculto que
         * seran los que completara el usuario.
         * 
         */
        public List<List<List<int>>> ObtenerMatrizSudokuV2(int tamano)
        {
            // Consulta a Prolog para generar la matriz
            string consulta = "sudoku(MatrizResuelta, ListaResuelta), write(MatrizResuelta), nl, halt.";
            string resultado = EnviarConsulta(consulta);

            Console.WriteLine("Matriz Completa: " + resultado);

            resultado = resultado.Trim('[', ']');


            List<List<int>> matrizSudokuResuelta = resultado
                .Split("],[")
                .Select(sublista => sublista
                    .Split(',')
                    .Select(num => int.Parse(num.Trim()))
                    .ToList()
                )
                .ToList();

            // pasar la matriz a una lista.
            List<int> listaSudoku = matrizSudokuResuelta.SelectMany(sublista => sublista).ToList();
            //Console.WriteLine("Lista completa: " + string.Join(", ", listaSudoku));
            // Consultar para optener la ahora la matriz con ceros.
            string listaProlog = "[" + string.Join(",", listaSudoku) + "]";
            //Console.WriteLine ("Datos de la lista a enviar: " + listaProlog);


            string consultaMatrizConCeros = $"pistas_en_matriz({listaProlog}, MatrizResulta), write(MatrizResulta), nl, halt.";
            string resultadoConsulta = EnviarConsulta(consultaMatrizConCeros);

            Console.WriteLine($"Matriz con ceros: {resultadoConsulta}");

            resultadoConsulta = resultadoConsulta.Trim('[', ']');

            List<List<int>> matrizSudokuJuego = resultadoConsulta
                .Split("],[")
                .Select(sublista => sublista
                    .Split(',')
                    .Select(num => int.Parse(num.Trim()))
                    .ToList()
                )
                .ToList();

            List<int> listaSudokuJuego = matrizSudokuJuego.SelectMany(sublista => sublista).ToList();

            // Esto seria para almacenar los datos de las matrices en la clase.
            this.TableroOrigen = listaSudoku;

            this.TableroIncial = listaSudokuJuego;

            //this.MatrizDeCerosOriginal = matrizSudokuJuego;



            // Ahora guardar los datos de las matrices iniciales en el servicio de registro

            int cantidadCeros = listaSudokuJuego.Count(valor => valor == 0); // Esto es para contar los elementos que cumplan cierta condicion dentro de una lista.

            _registroService.RegistrarDatosIniciales(cantidadCeros, matrizSudokuResuelta, matrizSudokuJuego);

            return [matrizSudokuResuelta, matrizSudokuJuego];
        }


        // Para el envio de un movimiento.
        // 1. Fila - Columna.
        // 2. Que valor se va a colocar.
        // resolver(Fila, Columna, Valor, Matriz)



        // Para verificar si se puede agregar un elemento en una posicion especifica: (Esta si se debe de hacer en una consulta separada.)
        // Pasar: Fila, Columna, Valor, ListaConCeros.

        // Cantidad de errores:
        // Pasar Fila, Columna, Valor, ListaOrigial, ListaActual(Ya lleva el valor modificado)

        // Cantidad de Vacios:
        // Pasar Fila, Columna, Valor, ListaActual(Ya lleva el valor modificado)

        // Finalizado?
        // Pasar: CantidadDeErrores, CantidadDeVacios| Si  CantidadDeErrores = 0 y CantidadDeVacios = 0 => Ya se finalizo. Caso contrario no ha finalizado.

        // Si se decide hacer toda las consultas a la vez, entonces devolver una lista que tenga: [CantErrores, CantVacios, Finalizado]

        /**
         * Nombre: verificarMovimiento
         * 
         * Descripcion: Funciona para realizar las consultas a porlog para verificar si se puede realizar un movimiento y optener las cantidad de casillas erronea y la cantidad de casillas vacias 
         * que hay actualmente en el tablero.
         * 
         * Entradas:
         * 
         * Salidas:
         * 
         */
        public JsonResult verificarMovimiento(int fila, int columna, int valor, List<List<int>> tableroEnJuego)
        {

            string listaConCerosString = "[" + string.Join(",", TableroIncial) + "]";

            //Console.WriteLine($"\nLista conceros: {listaConCerosString} \n");

            string listaOrigenString = "[" + string.Join(",", TableroOrigen) + "]";
            //Console.WriteLine($"\nLista origen: {listaOrigenString} \n");




            //Console.WriteLine(listaConCerosString);
            // Lo primero es verificar si se puede agregar ese dato en la posicion indicada.
            string consultaVerificacion = $"verifica_posicion({fila}, {columna}, {valor}, {listaConCerosString}, Variable), write(Variable), nl, halt.";
            string respuestaVerificacion = EnviarConsulta(consultaVerificacion);
            //Console.WriteLine($"Verificación del movimiento: {respuestaVerificacion}");

            // Verificar si es podible modificar esa posicion.
            if (respuestaVerificacion.Contains("true"))
            {
                tableroEnJuego[fila - 1][columna - 1] = valor;
            }

            // Pasar el tablero en juego a una lista
            List<int> listaTableroEnJuego = tableroEnJuego.SelectMany(sublista => sublista).ToList();
            string listaTableroJuegoString = "[" + string.Join(",", listaTableroEnJuego) + "]";
            Console.WriteLine($"\nLista usuario: {listaTableroJuegoString} \n");


            // Consultar para ver cuantos errores y elementos vacios hay, ademas, se revisa si ya termino el juego.
            string consultaResultados = $"juego_final({listaOrigenString}, {listaTableroJuegoString}, Resultado), write(Resultado), nl, halt.";
            string perro = EnviarConsulta(consultaResultados);


            Console.WriteLine($"Lista resultado consulta: {perro}");

            // Parar la lista de string a numeros.
            List<int> listaNumeros = perro.Trim('[', ']')
                                    .Split(',')
                                    .Select(int.Parse)
                                    .ToList();


            // En esta parte de aqui se guardarian los datos de la verificacion.
            _registroService.VerificacionesRealizadas(); // Registrar una nueva verificacion

            _registroService.ErroresVerificacion(listaNumeros[0]); // Sumar la cantidad de errores que hubo en esta verificacion.

            return new JsonResult(new { Tablero = tableroEnJuego, CantErrores = listaNumeros[0], CantVacios = listaNumeros[1], Finalizado = listaNumeros[2] });
        }


        /**
         * Nombre: Optener_Cant_Erores_Y_Vacios
         * 
         * Descripcion: Esta funcion se encarga de validar la cantidad de errores y de casillas vacias que posea el tablero actual del jugador. Se envia una consulta a prolog para realizar la verificacion.
         * 
         * Entradas: List<List<int>> tableroEnJuego: El tablero de Sudoku del jugador.
         * 
         * Salidas: Una lista de numeros con 3 elementos, en la posicion [0] esta la cantidad de errores, en la [1] la cantidad de casillas vacias, y en la [2] el estado actual del juego.
         * 
         */
        public List<int> Optener_Cant_Erores_Y_Vacios(List<List<int>> tableroEnJuego)
        {
            string listaOrigenString = "[" + string.Join(",", TableroOrigen) + "]"; // Pasar a string la lista con el sudoku resuelto.

            // Pasar el tablero en juego a una lista
            List<int> listaTableroEnJuego = tableroEnJuego.SelectMany(sublista => sublista).ToList();

            string listaTableroJuegoString = "[" + string.Join(",", listaTableroEnJuego) + "]";
            //Console.WriteLine($"\nLista usuario: {listaTableroJuegoString} \n");


            string consultaResultados = $"juego_final({listaOrigenString}, {listaTableroJuegoString}, Resultado), write(Resultado), nl, halt.";
            string perro = EnviarConsulta(consultaResultados);


            //Console.WriteLine($"Lista resultado consulta: {perro}");


            List<int> listaNumeros = perro.Trim('[', ']')
                                    .Split(',')
                                    .Select(int.Parse)
                                    .ToList();

            // En esta parte de aqui se guardarian los datos de la verificacion.
            _registroService.VerificacionesRealizadas(); // Registrar una nueva verificacion

            _registroService.ErroresVerificacion(listaNumeros[0]); // Sumar la cantidad de errores que hubo en esta verificacion.


            Console.WriteLine("Se optuvieron los datos de verificacion.");

            return listaNumeros;
        }


        /**
         * Nombre: OptenerSugerencia
         * 
         * Descripcion: Funcion para optener una sugerencia de movimiento desde prolog, este devolvera una lista  la que posteriormente se modificara para poderla devolver 
         * al usuario y generar la sugerencia.
         * 
         * Entradas:  List<List<int>> tableroJugador: El tablero del jugador, el tablero actual.
         * 
         * Salidas: Una matriz de enteros con la sugerencia optenida desde prolog.
         * 
         */
        public List<List<int>> OptenerSugerencia(List<List<int>> tableroJugador)
        {

            string listaOrigenString = "[" + string.Join(",", TableroOrigen) + "]";
            //Console.WriteLine($"\nLista origen: {listaOrigenString} \n");
            string listaConCerosString = "[" + string.Join(",", TableroIncial) + "]";

            //Console.WriteLine($"\nLista conceros: {listaConCerosString} \n");


            // Realizar la consulta para optener las sugerencia.
            string consultaSugerencia = $"hacer_sugerencia({listaConCerosString}, {listaOrigenString}, LActualizado, MatrizActualizada), write(MatrizActualizada), nl, halt.";
            string respuestaSugerencias = EnviarConsulta(consultaSugerencia);

            Console.WriteLine($"\nRespuesta de la matriz con sugerencias: {respuestaSugerencias} \n");

            // Pasar la matriz string a una matriz  de enteros.
            List<List<int>> matriz_sugerencia = MatrizStringToInt(respuestaSugerencias);


            // Combinar las dos matrices, para que se puedan mantener los datos actuales del jugador.

            //List<List<int>> matrizDeCerosOriginal = _registroService.OptenerMatrizConCeros();

            List<List<int>> matriz_con_sugerencias = ReemplazarMatriz(tableroJugador, matriz_sugerencia);


            return matriz_con_sugerencias;
        }

        /**
         * Nombre: VerificarCargaArchivo
         * 
         * Descripcion: Funcion para verificar la carga del archivo que contiene las instrucciones de prolog.
         * 
         * Entradas: No posee.
         * 
         * Salidas: El stirng con los datos del error de la carga del archivo.
         * 
         */
        public string VerificarCargaArchivo()
        {
            return EnviarConsulta("writeln('Intentando abrir archivo...'), consult('C:\\Users\\edurg\\OneDrive\\Escritorio\\Prueba-Windows-forms\\PruebaMVC\\Prolog\\Sudoku.pl'), writeln('Archivo cargado correctamente.'), halt.");
        }

        /**
         * Nombre: ObtenerErroresProlog
         * 
         * Descripcion: Funcion para optener los errores que se generan al realizar una consult a prolog.
         * 
         * Entradas: No posee.
         * 
         * Salidas: Los datos de un error (En caso de haberlos).
         * 
         */
        public string ObtenerErroresProlog()
        {
            Console.WriteLine("Errores optenidos: ", prolog.StandardError.ReadToEnd());
            return prolog.StandardError.ReadToEnd();
        }



        /**
         * Nombre: MatrizStringToInt
         * 
         * Descripcion: Funcion para pasar una matriz que esta en string, a una matriz de enteros.
         * 
         * Entradas: string matriz_string: Una matriz en formato string.
         * 
         * Salidas: Una matriz de enteros formada a partir de la matriz string.
         * 
         */
        public List<List<int>> MatrizStringToInt(string matriz_string)
        {
            matriz_string = matriz_string.Trim('[', ']');
            List<List<int>> matriz_resultado = matriz_string
                .Split("],[")
                .Select(sublista => sublista
                    .Split(',')
                    .Select(num => int.Parse(num.Trim()))
                    .ToList()
                )
                .ToList();

            return matriz_resultado;
        }

        /**
         * Nombre: ReemplazarMatriz
         * 
         * Descripcion: Funcion para reemplazar los elemenos de una matriz con los elementos de otra matriz, posicion por posicion, exeptuando los elementos en donde el valor de la segunda 
         * matriz sea cero. 
         * 
         * Entradas: List<List<int>> matrizJugador: La matriz a la cual se le reemplazaran los elementos, List<List<int>> matrizConCeros: La matriz que sera copiada en la primer matriz.
         * 
         * Salidas: Una matriz.
         * 
         */
        public List<List<int>> ReemplazarMatriz(List<List<int>> matrizJugador, List<List<int>> matrizConCeros)
        {

            // Creamos la matriz resultante copiando la original
            List<List<int>> matrizResultado = matrizJugador.Select(fila => new List<int>(fila)).ToList();

            // Recorremos ambas listas para ir copiando los elementos.
            for (int i = 0; i < matrizJugador.Count; i++)
            {
                for (int j = 0; j < matrizJugador[i].Count; j++)
                {
                    // Los elementos que son ceros se dejan como estaba.
                    //Console.WriteLine($"Data de las matrices en unificacion: Ceros->{matrizConCeros[i][j].ToString()} -- Jugador->{matrizResultado[i][j].ToString()}");
                    if (matrizConCeros[i][j] != 0)
                    {
                        matrizResultado[i][j] = matrizConCeros[i][j];
                    }
                }
            }

            //Console.WriteLine("Matrices unificadas.");

            // devolver la matriz copiada.
            return matrizResultado;
        }




    }
}