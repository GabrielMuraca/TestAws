using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataModel;
using DataModel.Clases;


namespace PruebaObjetos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CatalogoClientes CatCli = new CatalogoClientes();
            List<Cliente> Clientes = CatCli.GetClientes();

            ListadoProductos ListPro = new ListadoProductos();

            List<Articulo> Productos = ListPro.GetProductos();

            List<Articulo> productosFiltrados = Productos.Where(x => x.Rubro == "Art. Limpieza" && x.Nom_pro.Contains("a")).ToList();

            Articulo A = Productos.FirstOrDefault(x => x.Rubro == "Art. Limpieza" && x.Nom_pro.Contains("a"));

            
        }
    }
}
