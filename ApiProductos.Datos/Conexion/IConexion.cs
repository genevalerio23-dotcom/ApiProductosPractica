using Microsoft.Data.SqlClient;

namespace ApiProductos.Datos.Conexion
{
    public interface IConexion
    {
        SqlConnection CrearConexion();
    }
}