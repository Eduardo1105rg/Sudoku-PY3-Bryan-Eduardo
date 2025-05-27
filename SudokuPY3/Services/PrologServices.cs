using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;

namespace SudokuPY3.Services
{
    public class PrologServices
    {

        private Process prolog;

        private List<int> TableroOrigen;

        private List<int> TableroIncial;

        public PrologServices()
        {
            IniciarProlog();
        }

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

        public string EnviarConsulta(string consulta)
        {
            IniciarProlog(); 

            prolog.StandardInput.WriteLine(consulta);
            prolog.StandardInput.Flush();

            return prolog.StandardOutput.ReadLine();
        }

        public void CerrarProlog()
        {
            if (prolog != null && !prolog.HasExited)
            {
                prolog.StandardInput.WriteLine("halt.");
                prolog.WaitForExit();
                prolog.Close();
            }
        }

        public string EnviarListaDeListas(List<List<int>> lista)
        {
            string listaFormateada = "[" + string.Join(",", lista.Select(sublista => "[" + string.Join(",", sublista) + "]")) + "]";
            string consulta = $"procesarLista({listaFormateada}), write('Resultado: '), nl, halt.";

            return EnviarConsulta(consulta);
        }

        //public List<List<int>> RecibirListaDesdeProlog()
        //{
        //    string consulta = "generarLista(X), write(X), nl, halt.";
        //    string resultado = EnviarConsulta(consulta);

        //    resultado = resultado.Replace("[", "").Replace("]", "").Trim();
        //    var listas = resultado.Split(',')
        //                          .Select(s => s.Split(' ').Select(int.Parse).ToList())
        //                          .ToList();

        //    return listas;
        //}

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

        void verificarMovimiento(int fila, int columna, int valor, List<List<int>> tableroEnJuego)
        {

            // Lo primero es verificar si se puede agregar ese dato en la posicion indicada.
            string listaConCerosString = "[" + string.Join(",", TableroIncial) + "]";
            string consultaVerificacion = $"verifica_posicion({fila}, {columna}, {valor}, {listaConCerosString}), write(Valido), nl, halt.";
            string respuestaVerificacion = EnviarConsulta(consultaVerificacion);
            Console.WriteLine($"Verificación del movimiento: {respuestaVerificacion}");

            if (!respuestaVerificacion.Contains("fail"))
            {
                // Aqui se deberia de insertar un valor en la posicion indicada por el usuario.
            }

            // >> En caso de hacer todas en unos solo todas estas partes deberan quitarse.

            // Pasar el tablero en juego a una lista
            List<int> listaTableroEnJuego = tableroEnJuego.SelectMany(sublista => sublista).ToList();
            string listaTableroJuegoString= "[" + string.Join(",", listaTableroEnJuego) + "]";

            // Aqui seria para optener la cantidad de fallos que hay.
            string consultaErrores = $"cantidad_errores({TableroOrigen}, {listaTableroJuegoString}, CantErrores), write(CantErrores), nl, halt.";
            string respuestaErrores = EnviarConsulta(consultaErrores);
            int cantidadErrores = int.Parse(respuestaErrores.Trim());

            // Aqui seria para optener la cantidad de elementos vacios (Casillas con ceros).
            string consultaVacios = $"cantidad_vacios({listaTableroJuegoString}, CantVacios), write(CantVacios), nl, halt.";
            string respuestaVacios = EnviarConsulta(consultaVacios);
            int cantidadVacios = int.Parse(respuestaVacios.Trim());

            // Aqui seria la consulta para ver si ya se termino el juego.

        }



        public string VerificarCargaArchivo()
        {
            return EnviarConsulta("writeln('Intentando abrir archivo...'), consult('C:\\Users\\edurg\\OneDrive\\Escritorio\\Prueba-Windows-forms\\PruebaMVC\\Prolog\\Sudoku.pl'), writeln('Archivo cargado correctamente.'), halt.");
        }

        public string ObtenerErroresProlog()
        {
            return prolog.StandardError.ReadToEnd();
        }

    }
}
