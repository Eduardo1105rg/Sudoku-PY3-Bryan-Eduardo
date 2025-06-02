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
        private readonly RegistroServices _registroService;

        /**
         * Nombre: JuegoController
         * 
         * Descripcion: Constructor de la clase Juego.
         * 
         * Entradas: Recibe los servicios que usara la clase.
         * 
         * Salidas: No posee.
         * 
         */
        public JuegoController(ILogger<JuegoController> logger, PrologServices prologService, RegistroServices registroService)
        {
            _logger = logger;
            _prologService = prologService;
            _registroService = registroService;
        }

        /**
         * Nombre:
         * 
         * Descripcion:
         * 
         * Entradas: No posee.
         * 
         * Salidas: Renderizara la ventana correspondiente al juego.
         * 
         */
        public IActionResult Juego()
        {

            return View();
        }

        /**
         * Nombre: solicitarMatriz
         * 
         * Descripcion: Funcion para  solicitar el tablero del juego al servicios de prolog y devolverlo hacia la interfaz. Esta funcion puede ser llamada desde la interfaz mediante un metodo POST.
         * 
         * Entradas: InciarJuegoRequest tamano_request: Un modelos de datos.
         * 
         * Salidas: Un objeto tipo json con los datos que devolvera la consulta.
         * 
         */
        [HttpPost]
        public IActionResult solicitarMatriz([FromBody] InciarJuegoRequest tamano_request)
        {
            List<List<List<int>>> matrizSudoku = _prologService.ObtenerMatrizSudokuV2(tamano_request.TamanoMatriz);
            List<List<int>> matrizResuelta = matrizSudoku[0]; 
            List<List<int>> matrizJuego = matrizSudoku[1];


            return Json(new { MatrizResuelta = matrizResuelta, MatrizJuego = matrizJuego }); //Json(new { matriz = matrizSudoku });
        }

        /**
         * Nombre:
         * 
         * Descripcion: Esta funcion puede ser llamada desde la interfaz mediante un metodo POST.
         * 
         * Entradas:  Un modelos de datos.
         * 
         * Salidas: Un objeto tipo json con los datos que devolvera la consulta.
         * 
         */
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

        /**
         * Nombre:
         * 
         * Descripcion: Esta funcion puede ser llamada desde la interfaz mediante un metodo POST.
         * 
         * Entradas: DatosMovimientoRequest data_resquest: Un modelos de datos.
         * 
         * Salidas: Un objeto tipo json con los datos que devolvera la consulta.
         * 
         */
        [HttpPost]
        public JsonResult SolicitarSugerencias([FromBody] DatosSugerenciasReques data_resquest)
        {

            List<List<int>> tableroJugador = data_resquest.Tablero; // Recibir el tablero, esto seria para actualizarlo y modificarlo.



            List<List<int>> matriz_con_sugerencias = _prologService.OptenerSugerencia(tableroJugador);

            // >> En esta parte de aqui se haria algo para que se valide el movimiento.

            List<int> listaNumeros = _prologService.Optener_Cant_Erores_Y_Vacios(matriz_con_sugerencias);

            return Json(new { MatrizConSugerencias = matriz_con_sugerencias, CantErrores = listaNumeros[0], CantVacios = listaNumeros[1], Finalizado = listaNumeros[2] });
        }


        /**
         * Nombre: FinalizacionPartida
         * 
         * Descripcion: Esta funcion puede ser llamada desde la interfaz mediante un metodo POST.
         * 
         * Entradas: DatosFinalizacionRequest data_resques: Un modelos de datos.
         * 
         * Salidas:  Un objeto tipo json con los datos que devolvera la consulta.
         * 
         */
        [HttpPost]
        public JsonResult FinalizacionPartida([FromBody] DatosFinalizacionRequest data_resquest)
        {

            // Optener los datos de la consulta:

            int duracion = data_resquest.Duracion;

            List<List<int>> ultimaMatriz = data_resquest.Tablero;

            string tipoFinalizacion = data_resquest.TipoFinalizacion;

            int canSugerencias = data_resquest.SugerenciasUtilizadas;


            // Registrar los datos.
            _registroService.FinalizarPartida(tipoFinalizacion, canSugerencias, duracion, ultimaMatriz);


            return Json(new { Estado = "Finalizacion partida" });

        }


        /**
         * Nombre: ReiniciarPartida
         * 
         * Descripcion: Funcion para solicitar el reinicio de los datos al servicios de registro. Esta funcion puede ser llamada desde un metodo GET en la vista.
         * 
         * Entradas: No posee.
         * 
         * Salidas: Un objeto tipo json con los datos que devolvera la consulta.
         * 
         */
        [HttpGet]
        public JsonResult ReiniciarPartida()
        {

            _registroService.ReinicarDatosJuego(); // Reiniciar los datos del juego actual.
            Console.WriteLine("Datos de la partida actual reiniciados.");
            

            return Json(new { Estado = "Reinicio realizado" });
        }





        // Estos serian las modelos de datos:

        /* Modelo de datos para pasar el tamaño de una matriz al controlador */
        public class InciarJuegoRequest
        {

            public int TamanoMatriz { get; set; }

        }

        /* Modelo de datos para pasar los datos de un movimiento en el tablero al controlador.*/
        public class DatosMovimientoRequest
        {
            public int Fila { get; set; }

            public int Columna { get; set; }

            public int Valor { get; set; }

            public List<List<int>> Tablero { get; set; }

            public List<List<int>> MatrizOrigen { get; set; }

        }

        /* Modelo de datos  para pasar los datos de finalizacion de una partida al controlador. */
        public class DatosFinalizacionRequest
        {
            public int Duracion { get; set; }

            public string TipoFinalizacion { get; set; }

            public int SugerenciasUtilizadas { get; set; }

            public List<List<int>> Tablero { get; set; }
        }

        /* Modelo de datos para pasar los datos de la atriz del usuario al controlador */
        public class DatosSugerenciasReques
        {

            public List<List<int>> Tablero { get; set; }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
