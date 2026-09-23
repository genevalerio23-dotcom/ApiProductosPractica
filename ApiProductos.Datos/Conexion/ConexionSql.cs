using Microsoft.Data.SqlClient;

namespace ApiProductos.Datos.Conexion
{
    public class ConexionSql : IConexion
    {
        private readonly string _cadenaConexion;

        public ConexionSql(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public SqlConnection CrearConexion()
        {
            return new SqlConnection(_cadenaConexion);
        }
    }
}