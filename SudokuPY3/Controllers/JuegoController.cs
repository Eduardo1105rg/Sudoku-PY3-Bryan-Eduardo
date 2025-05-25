using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SudokuPY3.Models;
using SudokuPY3.Services;

namespace SudokuPY3.Controllers
{
    public class JuegoController : Controller
    {

        private readonly ILogger<JuegoController> _logger;
        private readonly PrologServices _prologService;

        public JuegoController(ILogger<JuegoController> logger, PrologServices prologService)
        {
            _logger = logger;
            _prologService = prologService;
        }

        public IActionResult Juego()
        {

            //List<List<int>> matrizSudoku = _prologService.ObtenerMatrizSudoku();
            string matrizSudoku2 = _prologService.EnviarConsulta("sudoku_con_pistas(MatrizConCeros, MatrizResuelta), write(MatrizConCeros), nl, halt.");
            //string resultado = _prologService.EnviarConsulta("sudoku(Rows, M), writeln(M), nl, halt.");
            //string resultado = _prologService.ObtenerErroresProlog();

            _logger.LogInformation(matrizSudoku2);

            //_logger.LogInformation($"Cantidad de filas en la matriz: {matrizSudoku.Count}");

            //foreach (var fila in matrizSudoku)
            //{
            //    _logger.LogInformation(string.Join(", ", fila));
            //}

            return View();
        }

        [HttpPost]
        public IActionResult solicitarMatriz([FromBody] InciarJuegoRequest tamano_request)
        {
            List<List<int>> matrizSudoku = _prologService.ObtenerMatrizSudoku(tamano_request.TamanoMatriz);
            //var matrizSudoku = _prologService.ObtenerMatrizSudoku(tamano_request.TamanoMatriz);
            //_logger.LogInformation(matrizSudoku);
            //_logger.LogInformation($"Cantidad de filas en la matriz: {matrizSudoku.Count}");
            //foreach (var fila in matrizSudoku)
            //{
            //    _logger.LogInformation(string.Join(", ", fila));
            //}
            return Ok(matrizSudoku); //Json(new { matriz = matrizSudoku });
        }


        [HttpPost]
        public JsonResult jugadaSudoku([FromBody] DatosMovimientoRequest data_resquest)
        {
            int fila = data_resquest.Fila;

            //_logger.LogInformation($"Numero de la fila de la jugada: {fila}");

            int columna = data_resquest.Columna;

            int valor = data_resquest.Valor;

            List<List< int >> matrizRecibida = data_resquest.Tablero;

            //_logger.LogInformation($"Cantidad de filas en la matriz: {matrizRecibida.Count}");
            //foreach (var fila2 in matrizRecibida)
            //{
            //    _logger.LogInformation(string.Join(", ", fila2));
            //}

            return Json(new { matriz = "matrizSudoku" });

        }



        // Estos serian las modelos de datos:

        public class InciarJuegoRequest
        {

            public int TamanoMatriz { get; set; }

        }


        public class DatosMovimientoRequest
        {
            public int Fila { get; set; }

            public int Columna { get; set; }

            public int Valor { get; set; }

            public List<List<int>> Tablero { get; set; }

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
