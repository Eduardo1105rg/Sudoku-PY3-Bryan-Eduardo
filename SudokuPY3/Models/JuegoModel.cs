namespace SudokuPY3.Models
{
    public class JuegoModel
    {
        public int CeldasIngreso = 0;

        public int Verificaciones = 0;

        public int ErroresVerificacion = 0;

        public int SugerenciasUsadas = 0;

        public string TipoFinalizacion  = "";

        public int Duracion = 0;

        public List<List<int>> MatrizOrigen;

        public List<List<int>> MatrizConCeros;

        public List<List<int>> UltimaMatriz;


        /**
         * Nombre: JuegoModel
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: No posee.
         * 
         * Salidas: No posee.
         * 
         */
        public JuegoModel()
        {

        }

        /**
         * Nombre: setCeldasIngreso
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: int celdasIngreso: Cantidad de celdas vacias del tablero.
         * 
         * Salidas: No posee.
         * 
         */
        public void setCeldasIngreso(int celdasIngreso)
        {
            this.CeldasIngreso = celdasIngreso;
        }


        /**
         * Nombre:
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: int verificaciones: Cantidad de verificaciones relizadas.
         * 
         * Salidas: No posee.
         * 
         */
        public void SetVerificaciones(int verificaciones)
        {
            this.Verificaciones += verificaciones;
        }


        /**
         * Nombre: SetErroresVerificacion
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: int errores: Cantidad de errores en cada verificacion realizada.
         *  
         * Salidas: No posee.
         * 
         */
        public void SetErroresVerificacion(int errores)
        {
            this.ErroresVerificacion += errores;
        }


        /**
         * Nombre:
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: int sugerencias: Cantidad de sugerencias utilizadas al final de la partida.
         * 
         * Salidas: No posee.
         * 
         */
        public void SetSugerenciasUsadas(int sugerencias)
        {
            this.SugerenciasUsadas = sugerencias;
        }


        /**
         * Nombre:
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: string tipo: Tipo de finalizacion realizada (Exitosa, Abandono, Autocompletado)
         * 
         * Salidas: No posee.
         * 
         */
        public void SetTipoFinalizacion(string tipo)
        {
            this.TipoFinalizacion = tipo;
        }


        /**
         * Nombre:
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: int duracion: Duracion de la partida.
         * 
         * Salidas: No posee.
         * 
         */
        public void SetDuracion(int duracion)
        {
            this.Duracion = duracion;
        }


        /**
         * Nombre:
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: List<List<int>> matriz: La matriz solucion del juego.
         * 
         * Salidas: No posee.
         * 
         */
        public void SetMatrizOrigen(List<List<int>> matriz)
        {
            this.MatrizOrigen = matriz;
        }


        /**
         * Nombre:
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: La matriz con las con algunas casillas cuyo valor cera 0.
         * 
         * Salidas: No posee.
         * 
         */
        public void SetMatrizConCeros(List<List<int>> matriz)
        {
            this.MatrizConCeros = matriz;
        }


        /**
         * Nombre: SetUltimaMatriz
         * 
         * Descripcion: Funcion para asignar un valor a uno de los atributos de la clase.
         * 
         * Entradas: List<List<int>> matriz: El tablero final del jugador al momento de finalizar el juego.
         * 
         * Salidas: No posee.
         * 
         */
        public void SetUltimaMatriz(List<List<int>> matriz)
        {
            this.UltimaMatriz = matriz;
        }


        /**
         * Nombre: ResetarDatos
         * 
         * Descripcion: Funcion para resetear los valores de 'Verificaciones' y 'ErroresVerificacion' a 0.
         * 
         * Entradas: No posee.
         * 
         * Salidas: No posee.
         * 
         */
        public void ResetarDatos()
        {
            this.Verificaciones = 0;
            this.ErroresVerificacion = 0;

            return;
        }



    }
}
