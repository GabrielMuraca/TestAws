using System;
using System.Windows.Forms;

namespace PuntosTateti
{
    public partial class VentanaAdmin : Form
    {
        Usuario usuario = new Usuario();
        public VentanaAdmin(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;
        }

        private void VentanaAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void VentanaAdmin_Load(object sender, EventArgs e)
        {
            lblNombre.Text = usuario.Nombre + ", " + usuario.Apellido;
            lblUsuario.Text = usuario.Alias;
            lblId.Text = usuario.idUsuario.ToString();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            VentanaLogin VenLog = new VentanaLogin();
            VenLog.Show();

            this.Hide();
        }

        private void btnCambiarContraseña_Click(object sender, EventArgs e)
        {
            CambioPassword VenPass = new CambioPassword(usuario);
            VenPass.Show();
        }

        private void btnAdminitrarUsuarios_Click(object sender, EventArgs e)
        {
            AdministracionUsuarios VenAdm = new AdministracionUsuarios(usuario);
            VenAdm.Show();
        }

        private void btnPuntos_Click(object sender, EventArgs e)
        {
            ContenedorPrincipal VenContPrin = new ContenedorPrincipal(usuario);
            VenContPrin.Show();
            this.Hide();
        }
    }
}
