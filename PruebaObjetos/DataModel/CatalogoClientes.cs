using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;

namespace DataModel
{
    public class CatalogoClientes
    {
        

        public List<Cliente> GetClientes()
        {
            DataTable dataTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["PruebaObjetos"].ConnectionString;
            string query = "select id_clientes, Nom_cli from Clientes";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            List<Cliente> clientes = new List<Cliente>();

            foreach(DataRow rFila in dataTable.Rows)
            {
                Cliente cliente = new Cliente();
                cliente.codigo = int.Parse(rFila["id_Clientes"].ToString());
                cliente.Nombre = rFila["Nom_cli"].ToString();

                clientes.Add(cliente);
            }

            return clientes;
        }
    }
}
