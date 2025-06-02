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
        private readonly RegistroServices _registroService;

        /**
         * Nombre: EstadisticasController
         * 
         * Descripcion: Contructor de la clase EstadisticasServices.
         * 
         * Entradas: Los servicios que usara la clase.
         * 
         * Salidas: No posee.
         * 
         */
        public EstadisticasController(ILogger<EstadisticasController> logger, PrologServices prologService, RegistroServices registroService)
        {
            _logger = logger;
            _prologService = prologService;
            _registroService = registroService;
        }


        /**
         * Nombre: Estadisticas
         * 
         * Descripcion: Funcion para renderizar la ventana de estadisticas.
         * 
         * Entradas: No posee.
         * 
         * Salidas: La ventana de estadisticas.
         * 
         */
        public IActionResult Estadisticas()
        {

            List<JuegoModel> historialPartidas = _registroService.OptenerHistorial();
            return View(historialPartidas);
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
