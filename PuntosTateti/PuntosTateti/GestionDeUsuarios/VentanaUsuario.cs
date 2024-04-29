using System;
using System.Windows.Forms;

namespace PuntosTateti
{
    public partial class VentanaUsuario : Form
    {
        Usuario usuario = new Usuario();
        public VentanaUsuario(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;
        }

        private void VentanaUsuario_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void VentanaUsuario_Load(object sender, EventArgs e)
        {
            lblNombre.Text = usuario.Nombre + ", " + usuario.Apellido;
            lblUsuario.Text = usuario.Alias;
            lblId.Text = usuario.idUsuario.ToString();
        }

        private void btnCambiarContraseña_Click(object sender, EventArgs e)
        {
            CambioPassword VenPass = new CambioPassword(usuario);
            VenPass.Show();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            VentanaLogin VenLog = new VentanaLogin();
            VenLog.Show();

            this.Hide();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnPuntos_Click(object sender, EventArgs e)
        {
            ContenedorPrincipal VenContPrin = new ContenedorPrincipal(usuario);
            VenContPrin.Show();
            this.Hide();
        }
    }
}
