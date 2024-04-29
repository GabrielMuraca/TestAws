using ClassLibrary;
using System;
using System.Windows.Forms;

namespace PuntosTateti.GestionDePuntos
{
    public partial class PuntosEditar : Form
    {
        PuntosxCliente puntosxCliente = new PuntosxCliente();
        Usuario usuario;
        public PuntosEditar(PuntosxCliente _puntosxUsuario, Usuario _usuario)
        {
            InitializeComponent();
            puntosxCliente = _puntosxUsuario;
            usuario = _usuario;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (rbAgregar.Checked)
                puntosxCliente.Puntos = puntosxCliente.Puntos + decimal.Parse(txtPuntos.Text);
            else if (rbQuitar.Checked)
                puntosxCliente.Puntos = puntosxCliente.Puntos - decimal.Parse(txtPuntos.Text);

            try
            {
                string cmd = string.Format("Exec UpdatePuntos '{0}', '{1}', '{2}'", puntosxCliente.IdCliente, puntosxCliente.Puntos, usuario.Alias);
                Utilidades.Ejecutar(cmd);

                MessageBox.Show("Puntos Editados con EXITO!");

                this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al actualizar los Puntos del Cliente: " + error.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPuntos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
