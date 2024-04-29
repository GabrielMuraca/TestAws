using System.Data;
using System.Data.SqlClient;

namespace ClassLibrary
{
    public class Utilidades
    {
        public static DataSet Ejecutar(string cmd)
        {
            SqlConnection con = new SqlConnection(ConstantesConeccion.ConectionString);
            con.Open();

            DataSet DS = new DataSet();
            SqlDataAdapter DP = new SqlDataAdapter(cmd, con);

            DP.Fill(DS);

            con.Close();

            return DS;
        }
    }
}
