using ClassLibrary;
using System;
using System.Windows.Forms;

namespace PuntosTateti
{
    public partial class CambioPassword : Form
    {
        Usuario usuario;
        public CambioPassword(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(txtNuevaPsw.Text != txtRepiteNuevaPsw.Text)
            {
                MessageBox.Show("La Nueva Contraseña y su confirmación no coinciden");
                return;
            }

            if (usuario.Password != txtPassword.Text)
            {
                MessageBox.Show("Contraseña incorrecta");
                return;
            }

            try
            {
                string cmd = string.Format("Exec UpdatePassword {0}, {1}", usuario.idUsuario, txtNuevaPsw.Text);
                Utilidades.Ejecutar(cmd);

                MessageBox.Show("Contraseña actualizada con EXITO!");

                this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al actualizar la contraseña: " + error.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
