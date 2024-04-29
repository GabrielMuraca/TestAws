using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Clases;

namespace DataModel
{
    public class ListadoProductos
    {
        public List<Articulo> GetProductos()
        {
            DataTable dataTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["PruebaObjetos"].ConnectionString;
            string query = "select id_pro, Nom_pro from Articulo";

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            // create data adapter
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            // this will query your database and return the result to your datatable
            da.Fill(dataTable);
            conn.Close();
            da.Dispose();

            List<Articulo> articulos = new List<Articulo>();

            foreach (DataRow rFila in dataTable.Rows)
            {
                Articulo articulo = new Articulo();
                articulo.id_producto = rFila["id_pro"].ToString();
                articulo.Nom_pro = rFila["Nom_pro"].ToString();
                articulo.Rubro = rFila["Rubro"].ToString();

                articulos.Add(articulo);
            }

            return articulos;
        }
    }
}
