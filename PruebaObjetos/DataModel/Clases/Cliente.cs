using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Cliente
    {
        public int codigo { get; set; }
        public string Nombre { get; set; }
        public string Cuit { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string TelAlt { get; set; }
        public string Iva { get; set; }
    }
}
