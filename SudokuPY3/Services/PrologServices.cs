using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions;

namespace SudokuPY3.Services
{
    public class PrologServices
    {

        private Process prolog;

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

        public List<List<int>> RecibirListaDesdeProlog()
        {
            string consulta = "generarLista(X), write(X), nl, halt.";
            string resultado = EnviarConsulta(consulta);

            resultado = resultado.Replace("[", "").Replace("]", "").Trim();
            var listas = resultado.Split(',')
                                  .Select(s => s.Split(' ').Select(int.Parse).ToList())
                                  .ToList();

            return listas;
        }


        //public List<List<int>> ObtenerMatrizSudoku(int tamano)
        //{
        //    // Consulta a Prolog para generar la matriz
        //    string consulta = "sudoku_con_pistas(MatrizConCeros, MatrizResuelta), write(MatrizConCeros), nl, halt.";
        //    string resultado = EnviarConsulta(consulta);
        //    //Console.WriteLine("Respuesta: ");

        //    //Console.WriteLine(resultado);
        //    // Convertir la respuesta de Prolog en una lista de listas en C#
        //    resultado = resultado.Replace("[", "").Replace("]", "").Trim();
        //    Console.WriteLine(resultado);
        //    var listas = resultado.Split(',')
        //                          .Select(sublista => sublista.Split(' ')
        //                          .Select(int.Parse).ToList())
        //                          .ToList();

        //    return listas;
        //}



        public List<List<int>> ObtenerMatrizSudoku(int tamano)
        {
            // Consulta a Prolog para generar la matriz
            string consulta = "sudoku_con_pistas(MatrizConCeros, MatrizResuelta), write(MatrizConCeros), nl, halt.";
            string resultado = EnviarConsulta(consulta);

            Console.WriteLine("Respuesta de Prolog: " + resultado);

            resultado = resultado.Trim('[', ']');

           
            List<List<int>> matrizSudoku = resultado
                .Split("],[")
                .Select(sublista => sublista
                    .Split(',') 
                    .Select(num => int.Parse(num.Trim())) 
                    .ToList()
                )
                .ToList();

            Console.WriteLine($"Cantidad de filas en la matriz: {matrizSudoku.Count}");


            return matrizSudoku;
        }





        // Para el envio de un movimiento.
        // 1. Fila - Columna.
        // 2. Que valor se va a colocar.
        // resolver(Fila, Columna, Valor, Matriz)


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
