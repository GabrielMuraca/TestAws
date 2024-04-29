using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using ClassLibrary;

namespace PuntosTateti
{
    public partial class VentanaLogin : Form
    {
        public VentanaLogin()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                Usuario usuario = buscarUsuario();

                if (usuario.Alias.Trim().ToUpper() == txtUsuario.Text.Trim().ToUpper() && usuario.Password == txtPassword.Text)
                {
                    if (usuario.Tipo == Constantes.TIPO_ADMIN)
                    {
                        VentanaAdmin VenAdm = new VentanaAdmin(usuario);
                        this.Hide();
                        VenAdm.Show();
                    }
                    else
                    {
                        VentanaUsuario VenUser = new VentanaUsuario(usuario);
                        this.Hide();
                        VenUser.Show();
                    }
                }
                else
                    MessageBox.Show("Usuario o Contraseña incorrecto");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Usuario o Contraseña incorrecto");
            }

        }

        private Usuario buscarUsuario()
        {
            string cmd = string.Format("Select * from Usuarios where alias = '{0}' and password = '{1}'", txtUsuario.Text.Trim().ToUpper(), txtPassword.Text);

            DataSet DS = Utilidades.Ejecutar(cmd);

            return new Usuario()
            {
                idUsuario = (int)DS.Tables[0].Rows[0]["idUsuario"],
                Nombre = DS.Tables[0].Rows[0]["nombre"].ToString().Trim(),
                Apellido = DS.Tables[0].Rows[0]["apellido"].ToString().Trim(),
                Password = DS.Tables[0].Rows[0]["password"].ToString().Trim(),
                Alias = DS.Tables[0].Rows[0]["alias"].ToString().Trim(),
                Tipo = (int)DS.Tables[0].Rows[0]["tipoUsuario"]
            };
        }

        private void VentanaLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
