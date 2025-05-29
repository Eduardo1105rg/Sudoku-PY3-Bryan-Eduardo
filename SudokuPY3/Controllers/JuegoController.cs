using System.Collections.Generic;
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

            return View();
        }

        [HttpPost]
        public IActionResult solicitarMatriz([FromBody] InciarJuegoRequest tamano_request)
        {
            List<List<List<int>>> matrizSudoku = _prologService.ObtenerMatrizSudokuV2(tamano_request.TamanoMatriz);
            List<List<int>> matrizResuelta = matrizSudoku[0]; 
            List<List<int>> matrizJuego = matrizSudoku[1];


            return Json(new { MatrizResuelta = matrizResuelta, MatrizJuego = matrizJuego }); //Json(new { matriz = matrizSudoku });
        }


        [HttpPost]
        public JsonResult jugadaSudoku([FromBody] DatosMovimientoRequest data_resquest)
        {
            int fila = data_resquest.Fila;

            //_logger.LogInformation($"Numero de la fila de la jugada: {fila}");

            int columna = data_resquest.Columna;

            int valor = data_resquest.Valor;

            List<List< int >> matrizRecibida = data_resquest.Tablero;

            List<List<int>> matrizOrigen = data_resquest.MatrizOrigen;

            //_logger.LogInformation($"Cantidad de filas en la matriz: {matrizRecibida.Count}");
            //foreach (var fila2 in matrizRecibida)
            //{
            //    _logger.LogInformation(string.Join(", ", fila2));
            //}
            var datosEnvio = _prologService.verificarMovimiento(fila, columna, valor, matrizRecibida);

            //_prologService.OptenerSugerencia();

            return datosEnvio;//Json(new { matriz = "matrizSudoku" });

        }

        [HttpGet]
        public JsonResult SolicitarSugerencias()
        {

            List<List<int>> matriz_con_sugerencias = _prologService.OptenerSugerencia();

            return Json(new { MatrizConSugerencias = matriz_con_sugerencias });
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

            public List<List<int>> MatrizOrigen { get; set; }

        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
