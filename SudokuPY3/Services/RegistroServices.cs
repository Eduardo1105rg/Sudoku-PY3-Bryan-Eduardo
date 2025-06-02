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
         * Entradas: Los datos de finalizacion de una partida.
         * 
         * Salidas: No posee.
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
         * Nombre: RegistrarDatosIniciales
         * 
         * Descripcion: Funcion para registrar los datos iniciales de una partida.
         * 
         * Entradas: Los datos que se ocupan para registrar una partida.
         * 
         * Salidas: No posee.
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
         * Nombre: VerificacionesRealizadas
         * 
         * Descripcion: Funcion para aumentar el numero de verificaciones realizadas.
         * 
         * Entradas: No posee.
         * 
         * Salidas: No posee.
         * 
         */
        public void VerificacionesRealizadas()
        {
            juegoActual.SetVerificaciones(1);

            return;
        }

        /**
         * Nombre: ErroresVerificacion
         * 
         * Descripcion: Funcion para modificar la cantidad de errores que que se tiene actualmente en una partida.
         * 
         * Entradas: int cantErrores: Cantidad de errores en una verificacion.
         * 
         * Salidas: No posee.
         * 
         */
        public void ErroresVerificacion(int cantErrores) 
        {
            juegoActual.SetErroresVerificacion(cantErrores);

            return;
        }

        /**
         * Nombre: ReinicarDatosJuego
         * 
         * Descripcion: Funcion para llamar a la funcion que reiniciara ciertos valores de la clase JuegoModel.
         * 
         * Entradas: No posee.
         * 
         * Salidas: No posee.
         * 
         */
        public void ReinicarDatosJuego()
        {
            juegoActual.ResetarDatos();

            return;
        }


        /**
         * Nombre: OptenerMatrizConCeros
         * 
         * Descripcion: Funcion para optener la matriz con ceros del juego actual.
         * 
         * Entradas: No posee.
         * 
         * Salidas: La matriz con ceros creada para el juego actual.
         * 
         */
        public List<List<int>> OptenerMatrizConCeros() {

            return juegoActual.MatrizConCeros;
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
    }
}
