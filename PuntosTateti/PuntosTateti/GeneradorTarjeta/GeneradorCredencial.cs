using System;
using System.Windows.Forms;

namespace PuntosTateti.GeneradorTarjeta
{
    public partial class GeneradorCredencial : Form
    {
        public GeneradorCredencial()
        {
            InitializeComponent();
        }

        private void BaseGenerador_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }
    }
}
