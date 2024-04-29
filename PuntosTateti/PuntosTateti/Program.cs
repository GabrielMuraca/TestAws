using System;
using System.Windows.Forms;

namespace PuntosTateti
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VentanaLogin());
            //Application.Run(new GestionPuntos(new Usuario(){ Alias = "gmuraca" }));
        }
    }
}
