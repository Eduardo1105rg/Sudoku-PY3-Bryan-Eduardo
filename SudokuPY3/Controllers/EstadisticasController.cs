using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SudokuPY3.Models;
using SudokuPY3.Services;

namespace SudokuPY3.Controllers
{
    public class EstadisticasController : Controller
    {

        private readonly ILogger<EstadisticasController> _logger;
        private readonly PrologServices _prologService;

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
        public EstadisticasController(ILogger<EstadisticasController> logger, PrologServices prologService)
        {
            _logger = logger;
            _prologService = prologService;
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
        public IActionResult Estadisticas()
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
