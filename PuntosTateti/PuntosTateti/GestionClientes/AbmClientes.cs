using ClassLibrary;
using System;
using System.Data;
using System.Windows.Forms;

namespace PuntosTateti.GestionClientes
{
    public partial class AbmClientes : Form
    {
        Usuario usuario;
        DataSet ds;
        public AbmClientes(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AbmClientes_Load(object sender, EventArgs e)
        {
            cargarTabla();

            string cmd = string.Format("Select * from CategoriaCliente");

            try
            {
                DataSet Categorias = Utilidades.Ejecutar(cmd);

                cbxCategoria.DataSource = Categorias.Tables[0];
                cbxCategoria.DisplayMember = "descripcion";
                cbxCategoria.ValueMember = "idCategoria";
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al cargar las Categorias: " + error.Message);
            }
        }

        private void cargarTabla()
        {
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtMail.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            cbxCategoria.Text = string.Empty;
            lblPuntos.Text = string.Empty;

            try
            {
                string cmd = "Select c.NroSocio as Socio, c.nombre as Nombre, c.apellido as Apellido, c.telefono as Telefono, c.mail as EMail, c.direccion as Direccion, c.categoria as Categoria, p.puntos as Puntos, c.idCliente as ID, c.codigo as Codigo from Clientes c left join PuntosxCliente p on c.idCliente = p.idCliente where c.Audit_Delete_Date is null and p.Audit_Delete_Date is null";

                ds = Utilidades.Ejecutar(cmd);

                generarTabla(ds.Tables[0]);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al Cargar la Tabla inicial: " + error.Message);
            }
        }

        private void generarTabla(DataTable dataTable)
        {
            dgvClientes.Rows.Clear();
            
            dgvClientes.ColumnCount = 8;
            dgvClientes.Columns[0].Name = "N°";
            dgvClientes.Columns[0].Width = 70;
            dgvClientes.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.Columns[1].Name = "Nombre";
            dgvClientes.Columns[1].Width = 150;
            dgvClientes.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.Columns[2].Name = "Apellido";
            dgvClientes.Columns[2].Width = 150;
            dgvClientes.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.Columns[3].Name = "Puntos";
            dgvClientes.Columns[3].Width = 100;
            dgvClientes.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.Columns[4].Name = "Telefono";
            dgvClientes.Columns[4].Width = 150;
            dgvClientes.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.Columns[5].Name = "Mail";
            dgvClientes.Columns[5].Width = 300;
            dgvClientes.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.Columns[6].Name = "Direccion";
            dgvClientes.Columns[6].Width = 150;
            dgvClientes.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.Columns[7].Name = "Cat.";
            dgvClientes.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.Columns[7].Width = 50;

            dataTable.DefaultView.Sort = "Puntos Desc";
            
            foreach (DataRow r in dataTable.DefaultView.ToTable().Rows)
            {
                string[] row = new string[] {
                    r["Socio"].ToString(),
                    r["Nombre"].ToString(),
                    r["Apellido"].ToString(),
                    r["Puntos"].ToString(),
                    r["Telefono"].ToString(),
                    r["EMail"].ToString(),
                    r["Direccion"].ToString(),
                    r["Categoria"].ToString() };
                dgvClientes.Rows.Add(row);
            }
        }

        private void dgvClientes_MouseClick(object sender, MouseEventArgs e)
        {
            if (dgvClientes.SelectedRows != null)
            {
                DataRow r = ds.Tables[0].Select("Socio = " + dgvClientes.SelectedRows[0].Cells[0].Value.ToString())[0];

                txtNombre.Text = r["Nombre"].ToString();
                txtApellido.Text = r["Apellido"].ToString();
                txtTelefono.Text = r["Telefono"].ToString();
                txtMail.Text = r["EMail"].ToString();
                txtDireccion.Text = r["Direccion"].ToString();
                cbxCategoria.Text = r["Categoria"].ToString();
                lblPuntos.Text = r["Puntos"].ToString();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            filtrarTabla();
        }

        private void filtrarTabla()
        {
            string cmd = "Select c.NroSocio as Socio, c.nombre as Nombre, c.apellido as Apellido, c.telefono as Telefono, c.mail as EMail, c.direccion as Direccion, c.categoria as Categoria, p.puntos as Puntos, c.idCliente as ID from Clientes c left join PuntosxCliente p on c.idCliente = p.idCliente where c.Audit_Delete_Date is null and p.Audit_Delete_Date is null";

            if (txtCodigo.Text != string.Empty)
                cmd += " and c.codigo = " + txtCodigo.Text.Trim();
            else
            {
                if (txtApellido.Text != string.Empty)
                {
                    cmd += " and c.apellido like '%" + txtApellido.Text.Trim() + "%'";

                    if (txtNombre.Text != string.Empty)
                        cmd += " and c.nombre like '%" + txtNombre.Text.Trim() + "%'";

                    if (cbxCategoria.SelectedValue != null)
                        cmd += " and c.categoria = " + cbxCategoria.SelectedValue.ToString();
                }
                else
                {
                    if (txtNombre.Text != string.Empty)
                    {
                        cmd += " and c.nombre llike '%" + txtNombre.Text.Trim() + "%'";

                        if (cbxCategoria.SelectedValue != null)
                            cmd += " and c.categoria = " + cbxCategoria.SelectedValue.ToString();
                    }
                    else
                        if (cbxCategoria.SelectedValue != null)
                        cmd += " and c.categoria = " + cbxCategoria.SelectedValue.ToString();
                }
            }

            try
            {
                DataSet ds = Utilidades.Ejecutar(cmd);

                dgvClientes.DataSource = ds.Tables[0];
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al llamar a la Base de Clientes: " + error.Message);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            cargarTabla();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows != null)
            {
                DataRow r = ds.Tables[0].Select("Socio = " + dgvClientes.SelectedRows[0].Cells[0].Value.ToString())[0];
                Cliente cliente = new Cliente()
                {
                    IdCliente = int.Parse(r["ID"].ToString()),
                    Nombre = r["Nombre"].ToString(),
                    Apellido = r["Apellido"].ToString(),
                    Telefono = r["Telefono"].ToString(),
                    Mail = r["EMail"].ToString(),
                    Direccion = r["Direccion"].ToString(),
                    Categoria = int.Parse(r["Categoria"].ToString())
                };

                AltaCliente abmCliente = new AltaCliente(usuario, cliente);
                abmCliente.Show();
            }
            else
                MessageBox.Show("Debe seleccionar un Cliente a modificar");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string cmd;
            if (dgvClientes.SelectedRows != null)
            {
                try
                {
                    cmd = string.Format("Exec EliminarCliente '{0}', '{1}'", dgvClientes.SelectedRows[0].Cells[8].Value.ToString(), usuario.Alias);
                    Utilidades.Ejecutar(cmd);

                    MessageBox.Show("Cliente Eliminado con EXITO!");

                    cargarTabla();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error al Eliminar el Cliente: " + error.Message);
                }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            AltaCliente altaCliente = new AltaCliente(usuario);
            altaCliente.Show();
        }

        private void btnCredencial_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows != null)
            {
                DataRow r = ds.Tables[0].Select("Socio = "  + dgvClientes.SelectedRows[0].Cells[0].Value.ToString())[0];
                Cliente cliente = new Cliente()
                {
                    IdCliente = int.Parse(r["ID"].ToString()),
                    Nombre = r["Nombre"].ToString(),
                    Apellido = r["Apellido"].ToString(),
                    NroSocio = int.Parse(r["Socio"].ToString()),
                    Codigo = r["Codigo"].ToString()
                };

                new MetodosCredencial().MostrarCredencial(cliente);
            }
            else
                MessageBox.Show("Debe seleccionar un Cliente");
        }
    }
}
