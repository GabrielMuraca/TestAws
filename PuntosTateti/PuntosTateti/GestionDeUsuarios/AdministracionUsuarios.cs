using ClassLibrary;
using PuntosTateti.GestionDeUsuarios;
using System;
using System.Data;
using System.Windows.Forms;

namespace PuntosTateti
{
    public partial class AdministracionUsuarios : Form
    {
        DataSet ds;
        Usuario usuario;
        public AdministracionUsuarios(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;
        }
        private void AdministracionUsuarios_Load(object sender, EventArgs e)
        {
            cargarTabla();
        }

        private void cargarTabla()
        {
            string cmd = "Select u.idUsuario as ID, u.nombre as Nombre, u.apellido as Apellido, u.alias as Usuario, t.descripcion as Tipo, t.idTipo as idTipo from Usuarios u inner join TiposUsuario t on u.tipoUsuario = t.idTipo where u.Audit_Delete_Date is null and t.Audit_Delete_Date is null";
            try
            {
                ds = Utilidades.Ejecutar(cmd);

                generarTabla(ds.Tables[0]);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al cargar los Usuairos: " + error.Message);
            }
        }

        private void generarTabla(DataTable dataTable)
        {
            dgvUsuarios.Rows.Clear();

            dgvUsuarios.ColumnCount = 4;
            dgvUsuarios.Columns[0].Name = "ID";
            dgvUsuarios.Columns[0].Width = 70;
            dgvUsuarios.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUsuarios.Columns[1].Name = "Nombre";
            dgvUsuarios.Columns[1].Width = 150;
            dgvUsuarios.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUsuarios.Columns[2].Name = "Apellido";
            dgvUsuarios.Columns[2].Width = 150;
            dgvUsuarios.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUsuarios.Columns[3].Name = "Tipo";
            dgvUsuarios.Columns[3].Width = 100;
            
            foreach (DataRow r in dataTable.Rows)
            {
                string[] row = new string[] {
                    r["ID"].ToString(),
                    r["Nombre"].ToString(),
                    r["Apellido"].ToString(),
                    r["Tipo"].ToString()
                    };
                dgvUsuarios.Rows.Add(row);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            AltaUsuario altaUsuario = new AltaUsuario(usuario);
            altaUsuario.Show();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if(txtNombre.Text != string.Empty)
                ds.Tables[0].DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", "Nombre", txtNombre.Text);

            if (txtApellido.Text != string.Empty)
                ds.Tables[0].DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", "Apellido", txtApellido.Text);

            if (txtUsuario.Text != string.Empty)
                ds.Tables[0].DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", "Usuario", txtUsuario.Text);

            dgvUsuarios.DataSource = ds.Tables[0];
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;

            ds.Tables[0].DefaultView.RowFilter = null;

            dgvUsuarios.DataSource = ds.Tables[0];
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if(dgvUsuarios.SelectedRows.Count > 0)
            {
                DataRow r = ds.Tables[0].Select("ID = " + dgvUsuarios.SelectedRows[0].Cells[0].Value.ToString())[0];

                Usuario usuarioAModificar = new Usuario()
                {
                    idUsuario = int.Parse(r["ID"].ToString()),
                    Nombre = r["Nombre"].ToString(),
                    Apellido = r["Apellido"].ToString(),
                    Tipo = int.Parse(r["idTipo"].ToString())
                };
                AltaUsuario altaUsuario = new AltaUsuario(usuario, usuarioAModificar);
                altaUsuario.Show();

                cargarTabla();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Seguro desea Eliminar el Usuario seleccionado?", "Eliminar Usuario", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string cmd = string.Format("Exec EliminarUsuario '{0}', '{1}'", dgvUsuarios.SelectedRows[0].Cells[0].Value.ToString(), usuario.Alias);

                try
                {
                    Utilidades.Ejecutar(cmd);

                    cargarTabla();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error al eliminar el usuario: " + error.Message);
                }
            }            
        }
    }
}
