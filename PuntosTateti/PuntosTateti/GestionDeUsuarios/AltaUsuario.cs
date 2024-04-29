using ClassLibrary;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace PuntosTateti.GestionDeUsuarios
{
    public partial class AltaUsuario : Form
    {
        Usuario usuario;
        Usuario usuarioAModificar;
        bool modificacion;
        public AltaUsuario(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;
            modificacion = false;
        }
        public AltaUsuario(Usuario _usuario, Usuario _usuarioAModificar)
        {
            InitializeComponent();
            usuario = _usuario;
            usuarioAModificar = _usuarioAModificar;
            modificacion = true;
        }
        private void AltaUsuario_Load(object sender, EventArgs e)
        {
            try
            {
                string cmd = "Select idTipo, descripcion from TiposUsuario where Audit_Delete_Date is Null";

                DataSet ds = Utilidades.Ejecutar(cmd);

                cbxTupoUsuario.DataSource = ds.Tables[0];
                cbxTupoUsuario.DisplayMember = "descripcion";
                cbxTupoUsuario.ValueMember = "idTipo";

                cbxTupoUsuario.SelectedIndex = -1;
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al conectarse a la base de Tipos de Usuario: " + error.Message);
            }

            if(modificacion)
            {
                txtNombre.Text = usuarioAModificar.Nombre;
                txtApellido.Text = usuarioAModificar.Apellido;
                cbxTupoUsuario.SelectedValue = usuarioAModificar.Tipo;
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (!validarDatos())
            {
                MessageBox.Show("Error en los datos ingresados");
                return;
            }
                
            if (modificacion)
            {
                try
                {
                    string alias = (txtNombre.Text[0] + txtApellido.Text).ToLower();
                    string cmd = string.Format("Exec UpdateUsuario '{0}', '{1}', '{2}', '{3}', {4}, {5}", usuarioAModificar.idUsuario.ToString(), txtNombre.Text, txtApellido.Text, alias, cbxTupoUsuario.SelectedValue.ToString(), usuario.Alias);

                    Utilidades.Ejecutar(cmd);

                    MessageBox.Show("El usuairo se ha actualizado con Exito!");

                    this.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error al actualizar el nuevo Usuario: " + error.Message);
                }
            }
            else
            {
                try
                {
                    string alias = (txtNombre.Text[0] + txtApellido.Text).ToLower();
                    string cmd = string.Format("Exec AltaUsuario '{0}', '{1}', '{2}', '{3}', {4}", txtNombre.Text, txtApellido.Text, alias, cbxTupoUsuario.SelectedValue.ToString(), usuario.Alias);
                    Utilidades.Ejecutar(cmd);

                    MessageBox.Show("El usuairo se ha creado con Exito! \n USUARIO: " + alias.ToUpper() + " \n CONTRASEÑA: Tateti");
                    this.Close();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error al Guardar el nuevo Usuario: " + error.Message);
                }
            }            
        }

        private bool validarDatos()
        {
            bool response = true;
            //Verifica Apellido
            if (txtApellido.Text == string.Empty || !Regex.IsMatch(txtApellido.Text, @"^[a-zA-Z]+$"))
            {
                lblErrorApellido.Visible = true;
                response = false;
            }
            else
            {
                lblErrorApellido.Visible = false;
            }
            //Verifica Nombre
            if (txtNombre.Text == string.Empty || !Regex.IsMatch(txtNombre.Text, @"^[a-zA-Z]+$"))
            {
                lblErrorNombre.Visible = true;
                response = false;
            }
            else
            {
                lblErrorNombre.Visible = false;
            }
            //Verifica Nombre
            if (cbxTupoUsuario.SelectedIndex == -1)
            {
                lblErrorCombo.Visible = true;
                response = false;
            }
            else
            {
                lblErrorCombo.Visible = false;
            }

            return response;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
