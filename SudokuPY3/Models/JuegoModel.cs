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

        public JuegoModel()
        {

        }


        public void setCeldasIngreso(int celdasIngreso)
        {
            this.CeldasIngreso = celdasIngreso;
        }

        public void SetVerificaciones(int verificaciones)
        {
            this.Verificaciones += verificaciones;
        }

        public void SetErroresVerificacion(int errores)
        {
            this.ErroresVerificacion += errores;
        }

        public void SetSugerenciasUsadas(int sugerencias)
        {
            this.SugerenciasUsadas = sugerencias;
        }

        public void SetTipoFinalizacion(string tipo)
        {
            this.TipoFinalizacion = tipo;
        }
        public void SetDuracion(int duracion)
        {
            this.Duracion = duracion;
        }        

        public void SetMatrizOrigen(List<List<int>> matriz)
        {
            this.MatrizOrigen = matriz;
        }

        public void SetMatrizConCeros(List<List<int>> matriz)
        {
            this.MatrizConCeros = matriz;
        }

        public void SetUltimaMatriz(List<List<int>> matriz)
        {
            this.UltimaMatriz = matriz;
        }

        public void ResetarDatos()
        {
            this.Verificaciones = 0;
            this.ErroresVerificacion = 0;

            return;
        }



    }
}
