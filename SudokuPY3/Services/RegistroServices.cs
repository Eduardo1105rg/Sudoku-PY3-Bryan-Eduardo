using SudokuPY3.Models;

namespace SudokuPY3.Services
{
    public class RegistroServices
    {

        private JuegoModel juegoActual; // Instancia de la partidad actual del juego.

        public List<JuegoModel> HistorialPartidas = new List<JuegoModel>(); // Lista con el historial de partidas del juego.


        /**
         * Nombre: RegistroServices
         * 
         * Descripcion: Constructor de la clase.
         * 
         * Entradas: No posee.
         * 
         * Salidas: No posee.
         * 
         */
        public RegistroServices()
        {
            juegoActual = new JuegoModel();
        }


        /**
         * Nombre: OptenerHistorial
         * 
         * Descripcion: Funcion para optener el historial de partidas jugadas.
         * 
         * Entradas: No posee.
         * 
         * Salidas: El historial de partidas jugadas.
         * 
         */
        public List<JuegoModel> OptenerHistorial()
        {
            return HistorialPartidas;
        }


        /**
         * Nombre: FinalizarPartida
         * 
         * Descripcion: Funcion encargada de guardar los datos funales de una partida cuando se termine y agregar la instancia actual de JuegoModel al historial.
         * 
         * Entradas:
         * 
         * Salidas:
         * 
         */
        public void FinalizarPartida(string tipoFinalizacion, int cantSugerencias, int duracion, List<List<int>> ultimaMatriz)
        {
            juegoActual.SetUltimaMatriz(ultimaMatriz);
            juegoActual.SetDuracion(duracion);
            juegoActual.SetSugerenciasUsadas(cantSugerencias);
            juegoActual.SetTipoFinalizacion(tipoFinalizacion);
            HistorialPartidas.Add(juegoActual);

            juegoActual = new JuegoModel(); // Iniciar una nueva partida después de finalizar

            Console.WriteLine("Datos finales del juego registrados.");

            return;
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
        public void RegistrarDatosIniciales(int celdasIngreso, List<List<int>> matrizOrigen, List<List<int>> matrizCeros)
        {
            juegoActual.setCeldasIngreso(celdasIngreso);

            juegoActual.SetMatrizOrigen(matrizOrigen);

            juegoActual.SetMatrizConCeros(matrizCeros);

            Console.WriteLine("Datos iniciales del juego registrados.");

            return;

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
        public void VerificacionesRealizadas()
        {
            juegoActual.SetVerificaciones(1);

            return;
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
        public void ErroresVerificacion(int cantErrores) // Hay que definir como se contarian los errores de verificacion.
        {
            juegoActual.SetErroresVerificacion(cantErrores);

            return;
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
        public void ReinicarDatosJuego()
        {
            juegoActual.ResetarDatos();

            return;
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
    }
}
