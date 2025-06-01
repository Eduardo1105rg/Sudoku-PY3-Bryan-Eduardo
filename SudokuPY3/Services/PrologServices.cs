using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;

namespace SudokuPY3.Services
{
    public class PrologServices
    {

        private Process prolog;

        private List<int> TableroOrigen;

        private List<int> TableroIncial;

        private readonly RegistroServices _registroService; // Esto seria para el servicio de registro de los datos de las partidas de los juegos.

        /**
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
         * 
         */
        public PrologServices(RegistroServices registroService)
        {
            IniciarProlog();
            _registroService = registroService;
        }

        /**
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
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
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
         * 
         */
        public string EnviarConsulta(string consulta)
        {
            IniciarProlog(); 

            prolog.StandardInput.WriteLine(consulta);
            prolog.StandardInput.Flush();

            return prolog.StandardOutput.ReadLine();
        }

        /**
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
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
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
         * 
         */
        public List<List<int>> ObtenerMatrizSudoku(int tamano)
        {
            // Consulta a Prolog para generar la matriz
            string consulta = "sudoku_con_pistas(MatrizConCeros, MatrizResuelta), write(MatrizConCeros), nl, halt.";
            string resultado = EnviarConsulta(consulta);

            //Console.WriteLine("Respuesta de Prolog: " + resultado);

            resultado = resultado.Trim('[', ']');

           
            List<List<int>> matrizSudoku = resultado
                .Split("],[")
                .Select(sublista => sublista
                    .Split(',') 
                    .Select(num => int.Parse(num.Trim())) 
                    .ToList()
                )
                .ToList();

            //Console.WriteLine($"Cantidad de filas en la matriz: {matrizSudoku.Count}");


            return matrizSudoku;
        }


        /**
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
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
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
         * 
         */
        public JsonResult verificarMovimiento(int fila, int columna, int valor, List<List<int>> tableroEnJuego)
        {

            string listaConCerosString = "[" + string.Join(",", TableroIncial) + "]";

            Console.WriteLine($"\nLista conceros: {listaConCerosString} \n");

            string listaOrigenString = "[" + string.Join(",", TableroOrigen) + "]";
            Console.WriteLine($"\nLista origen: {listaOrigenString} \n");




            //Console.WriteLine(listaConCerosString);
            // Lo primero es verificar si se puede agregar ese dato en la posicion indicada.
            string consultaVerificacion = $"verifica_posicion({fila}, {columna}, {valor}, {listaConCerosString}, Variable), write(Variable), nl, halt.";
            string respuestaVerificacion = EnviarConsulta(consultaVerificacion);
            Console.WriteLine($"Verificación del movimiento: {respuestaVerificacion}");
            //verifica_posicion(1, 6, 4, [5,4,3,7,0,0,6,1,8,9,0,1,6,0,0,2,0,5,0,6,0,3,1,5,4,9,7,0,2,0,0,5,3,7,8,0,3,5,7,9,6,8,1,0,0,1,0,9,0,2,0,3,5,6,8,1,5,2,7,0,0,4,3,7,3,2,5,4,9,0,6,1,4,0,0,8,3,1,5,0,0]), write(Valido), nl, halt.
            
            if (respuestaVerificacion.Contains("true"))
            {
                // Aqui se deberia de insertar un valor en la posicion indicada por el usuario.
                tableroEnJuego[fila - 1][columna - 1] = valor;
            }

            // Pasar el tablero en juego a una lista
            List<int> listaTableroEnJuego = tableroEnJuego.SelectMany(sublista => sublista).ToList();
            string listaTableroJuegoString = "[" + string.Join(",", listaTableroEnJuego) + "]";
            Console.WriteLine($"\nLista usuario: {listaTableroJuegoString} \n");

            // >> En caso de hacer todas en unos solo todas estas partes deberan quitarse.



            // Aqui seria para optener la cantidad de fallos que hay.
            //string consultaErrores = $"cantidad_errores({TableroOrigen}, {listaTableroJuegoString}, CantErrores), write(CantErrores), nl, halt.";
            //string respuestaErrores = EnviarConsulta(consultaErrores);
            //int cantidadErrores = int.Parse(respuestaErrores.Trim());
            //Console.WriteLine("CantErrores: ", cantidadErrores);

            //// Aqui seria para optener la cantidad de elementos vacios (Casillas con ceros).
            //string consultaVacios = $"cantidad_vacios({listaTableroJuegoString}, CantVacios), write(CantVacios), nl, halt.";
            //string respuestaVacios = EnviarConsulta(consultaVacios);
            //int cantidadVacios = int.Parse(respuestaVacios.Trim());
            //Console.WriteLine("CantVacios: ", cantidadVacios);

            //ObtenerErroresProlog();
            string consultaResultados = $"juego_final({listaOrigenString}, {listaTableroJuegoString}, Resultado), write(Resultado), nl, halt.";
            string perro = EnviarConsulta(consultaResultados);
            //ObtenerErroresProlog();
            //string na = "";
            //perro = perro.Trim();

            Console.WriteLine($"Lista resultado consulta: {perro}");
            //		"juego_final([9,5,3,1,6,2,4,8,7,6,1,7,4,5,8,3,2,9,4,8,2,7,3,9,1,5,6,1,9,6,8,2,5,7,3,4,5,7,8,9,4,3,6,1,2,3,2,4,6,1,7,5,9,8,8,3,5,2,7,6,9,4,1,7,4,9,5,8,1,2,6,3,2,6,1,3,9,4,8,7,5], [9,5,3,0,6,2,4,0,7,1,0,7,4,5,8,0,2,0,4,8,2,7,3,9,1,0,0,1,0,6,0,2,5,0,3,4,0,7,0,9,4,3,6,1,2,3,2,0,6,1,7,0,9,8,0,3,5,2,0,6,0,4,1,7,4,9,5,8,0,0,6,3,2,6,0,3,9,4,8,0,0], Resultado), write(Resultado), nl, halt."


            //juego_final([3,1,8,2,6,5,4,9,7,7,5,2,1,4,9,3,6,8,6,9,4,7,3,8,1,2,5,5,4,7,6,1,2,9,8,3,1,8,9,5,7,3,2,4,6,2,3,6,9,8,4,7,5,1,9,2,1,3,5,6,8,7,4,4,7,5,8,9,1,6,3,2,8,6,3,4,2,7,5,1,9], [3,1,0,0,6,0,0,9,7,7,5,2,1,0,9,3,6,8,0,0,4,7,0,0,1,2,5,5,0,7,6,0,2,9,8,3,1,8,9,5,7,3,2,0,0,0,0,0,9,8,4,0,5,0,9,2,0,0,5,6,0,7,4,4,7,5,0,9,1,6,3,0,8,6,0,4,2,7,0,1,9], X), write(X), nl, halt.

            // Aqui seria la consulta para ver si ya se termino el juego.

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
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
         * 
         */
        public List<List<int>> OptenerSugerencia()
        {

            string listaOrigenString = "[" + string.Join(",", TableroOrigen) + "]";
            //Console.WriteLine($"\nLista origen: {listaOrigenString} \n");
            string listaConCerosString = "[" + string.Join(",", TableroIncial) + "]";

            //Console.WriteLine($"\nLista conceros: {listaConCerosString} \n");


            // Realizar la consulta para optener las sugerencia.
            string consultaSugerencia = $"hacer_sugerencia({listaConCerosString}, {listaOrigenString}, LActualizado, MatrizActualizada), write(MatrizActualizada), nl, halt.";
            string respuestaSugerencias = EnviarConsulta(consultaSugerencia);

            Console.WriteLine($"\nRespuesta de la matriz con sugerencias: {respuestaSugerencias} \n");

            List<List<int>> matriz_sugerencia = MatrizStringToInt(respuestaSugerencias);

            // Pasar la matriz string a una matriz  de enteros.


            return matriz_sugerencia;
        }

        /**
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
         * 
         */
        public string VerificarCargaArchivo()
        {
            return EnviarConsulta("writeln('Intentando abrir archivo...'), consult('C:\\Users\\edurg\\OneDrive\\Escritorio\\Prueba-Windows-forms\\PruebaMVC\\Prolog\\Sudoku.pl'), writeln('Archivo cargado correctamente.'), halt.");
        }

        /**
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
         * 
         */
        public string ObtenerErroresProlog()
        {
            Console.WriteLine("Errores optenidos: ", prolog.StandardError.ReadToEnd());
            return prolog.StandardError.ReadToEnd();
        }



        /**
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas:
         * 
         * Salidas:
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

    }
}
