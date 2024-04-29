using ClassLibrary;
using System;
using System.Windows.Forms;

namespace PuntosTateti.GestionDePuntos
{
    public partial class PuntosConfig : Form
    {
        Usuario usuario;
        public PuntosConfig(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;
        }

        private void PuntosConfig_Load(object sender, EventArgs e)
        {
            try
            {
                string cmd = string.Format("select valorxUnidad from PuntosValor p where p.Audit_Insert_Date = (select MAX(Audit_Insert_Date) from PuntosValor) and p.activo = 1");

                lblValor.Text = "$" + Utilidades.Ejecutar(cmd).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al consultar la Base de Valores de Puntos: " + error.Message);
            }

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string cmd = string.Format("select idPunto from PuntosValor p where p.Audit_Insert_Date = (select MAX(Audit_Insert_Date) from PuntosValor) and p.activo = 1");

                string idPunto = Utilidades.Ejecutar(cmd).Tables[0].Rows[0][0].ToString();

                cmd = string.Format("Exec UpdatePuntosValor '{0}', '{1}', '{2}'", idPunto, txtNuevoValor.Text, usuario.Alias);

                Utilidades.Ejecutar(cmd);

                MessageBox.Show("Nuevo Valor actualizado con EXITO!");

                this.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al consultar la Base de Valores de Puntos: " + error.Message);
            }
            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNuevoValor_KeyPress(object sender, KeyPressEventArgs e)
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
