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

        public List<List<int>> solicitarMatriz()
        {
            List<List<int>> matrizSudoku = _prologService.ObtenerMatrizSudoku();
            return matrizSudoku;
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
