using PuntosTateti.GestionClientes;
using PuntosTateti.GestionDePuntos;
using System;
using System.Windows.Forms;

namespace PuntosTateti
{
    public partial class ContenedorPrincipal : Form
    {
        Usuario usuario;
        public ContenedorPrincipal(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            ventanaAdminUsuario();
         
            this.Close();
        }

        private void ventanaAdminUsuario()
        {
            if (usuario.Tipo == 1)
            {
                VentanaAdmin venAd = new VentanaAdmin(usuario);
                venAd.Show();
            }
            else
            {
                VentanaUsuario venUs = new VentanaUsuario(usuario);
                venUs.Show();
            }

        }

        private void ContenedorPrincipal_Load(object sender, EventArgs e)
        {
            GestionPuntos VenPuntos = new GestionPuntos(usuario);
            VenPuntos.MdiParent = this;
            VenPuntos.Show();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AltaCliente AltaCliente = new AltaCliente(usuario);
            AltaCliente.MdiParent = this;
            AltaCliente.Show();
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbmClientes abmClientes = new AbmClientes(usuario);
            abmClientes.MdiParent = this;
            abmClientes.Show();
        }

        private void gestionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionPuntos gestion = new GestionPuntos(usuario);
            gestion.MdiParent = this;
            gestion.Show();
        }

        private void editarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PuntosConfig puntosConfig = new PuntosConfig(usuario);
            puntosConfig.MdiParent = this;
            puntosConfig.Show();
        }

        private void ContenedorPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            ventanaAdminUsuario();
        }
    }
}
