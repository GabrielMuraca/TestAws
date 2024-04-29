using System.Data.SqlClient.dll<>
using System.Data;
using System.Data.SqlTypes;


namespace Libreria
{
    public class Utilidades
    {
        public static DataSet Ejecutar(string cmd)
        {
            SqlConnection con = new SqlConnection("Data Source=(localdb)\\Servidor;Initial Catalog=Libreria;Integrated Security=True");
            con
        }
    }
}
