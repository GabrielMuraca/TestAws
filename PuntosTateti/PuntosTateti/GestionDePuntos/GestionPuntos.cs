using ClassLibrary;
using PuntosTateti.GestionDePuntos;
using System;
using System.Data;
using System.Windows.Forms;

namespace PuntosTateti
{
    public partial class GestionPuntos : Form
    {
        Usuario usuario;
        public GestionPuntos(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;

            ocultarAcreditado();
        }

        private void ocultarAcreditado()
        {
            lblAcreditado.Visible = false;
            lblFelicitaciones.Visible = false;
            lblSehaAcreditado.Visible = false;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                try
                {
                    string cmd = string.Format("Select c.nombre as Nombre, c.apellido as Apellido, pc.puntos as Puntos from Clientes c left join PuntosxCliente pc on c.idCliente = pc.idCliente where c.codigo = '{0}' and c.Audit_Delete_Date is null and pc.Audit_Delete_Date is null", txtCodigoCliente.Text.Trim());

                    DataSet ds = Utilidades.Ejecutar(cmd);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblNombre.Text = ds.Tables[0].Rows[0]["Nombre"].ToString();
                        lblApellido.Text = ds.Tables[0].Rows[0]["Apellido"].ToString();
                        lblPuntos.Text = ds.Tables[0].Rows[0]["Puntos"] == null ? "0" : ds.Tables[0].Rows[0]["puntos"].ToString();

                        lblNombre.Visible = true;
                        lblApellido.Visible = true;
                        lblPuntos.Visible = true;
                    }
                    else
                        MessageBox.Show("El Cliente indicado no existe");

                    ocultarAcreditado();
                }
                catch (Exception error)
                {
                    MessageBox.Show("Error al conectarse a la Base de Puntos: " + error.Message);
                }
                
            }
        }

        private void txtImporte_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtCodigoCliente.Text != string.Empty && decimal.Parse(txtImporte.Text) > 0)
                    guardarPuntos();
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if(txtCodigoCliente.Text != string.Empty && decimal.Parse(txtImporte.Text) > 0)
                guardarPuntos();
        }

        private void guardarPuntos()
        {
            string cmd = string.Format("Select c.idCliente, pc.puntos from Clientes c left join PuntosxCliente pc on c.idCliente = pc.idCliente where c.codigo = '{0}' and c.Audit_Delete_Date is null and pc.Audit_Delete_Date is null", txtCodigoCliente.Text.Trim());
            DataSet ds = new DataSet();
            try
            {
                ds = Utilidades.Ejecutar(cmd);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al conectarse a la Base de Puntos: " + error.Message);
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("El usuario indicado no Exite");
                return;
            }
            decimal puntoValor = 0;
            try
            {
                cmd = string.Format("select valorxUnidad from PuntosValor p where p.Audit_Insert_Date = (select MAX(Audit_Insert_Date) from PuntosValor) and p.activo = 1");

                puntoValor = decimal.Parse(Utilidades.Ejecutar(cmd).Tables[0].Rows[0][0].ToString());
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al obtener el valor de un Punto: " + error.Message);
                return;
            }

            if(puntoValor <= 0)
            {
                MessageBox.Show("Error al obtener el valor de un Punto: Valores inválidos. Corregir en Configuración");
                return;
            }

            decimal PuntosAAgregar = decimal.Parse(txtImporte.Text) / puntoValor;

            if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["puntos"].ToString()))
                cmd = string.Format("Exec InsertPuntos '{0}', '{1}', '{2}'", ds.Tables[0].Rows[0]["idCliente"].ToString(), PuntosAAgregar.ToString(), usuario.Alias);
            else
            {
                decimal totalPuntos = decimal.Parse(ds.Tables[0].Rows[0]["puntos"].ToString()) + PuntosAAgregar;
                cmd = string.Format("Exec UpdatePuntos '{0}', '{1}', '{2}'", ds.Tables[0].Rows[0]["idCliente"].ToString(), totalPuntos.ToString(), usuario.Alias);
            }

            try
            {
                Utilidades.Ejecutar(cmd);
                lblAcreditado.Text = PuntosAAgregar.ToString();
                lblAcreditado.Visible = true;
                lblFelicitaciones.ForeColor = System.Drawing.Color.Magenta;
                lblFelicitaciones.Visible = true;
                lblSehaAcreditado.ForeColor = System.Drawing.Color.Magenta;
                lblSehaAcreditado.Visible = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al realizar la operacion: " + e.Message);
            }
        }

        private void picConfiguracion_Click(object sender, EventArgs e)
        {
            PuntosConfig PunConf = new PuntosConfig(usuario);
            PunConf.Show();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string cmd;
            try
            {
                cmd = string.Format("Select c.idCliente as idCliente, pc.puntos as Puntos from Clientes c join PuntosxCliente pc on c.idCliente = pc.idCliente where c.codigo = '{0}' and c.Audit_Delete_Date is null and pc.Audit_Delete_Date is null", txtCodigoCliente.Text.Trim());
                ds = Utilidades.Ejecutar(cmd);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al obtener los Puntos del Cliente: " + error.Message);
            }

            if (ds.Tables.Count > 0)
            {
                if(ds.Tables[0].Rows.Count > 0)
                {
                    PuntosxCliente puntosxUsuario = new PuntosxCliente()
                    {
                        IdCliente = (int)ds.Tables[0].Rows[0]["idCliente"],
                        Puntos = (decimal)ds.Tables[0].Rows[0]["puntos"]
                    };

                    PuntosEditar puntosEditar = new PuntosEditar(puntosxUsuario, usuario);
                    puntosEditar.Show();
                }
            }
        }

        private void txtImporte_KeyPress(object sender, KeyPressEventArgs e)
        {
            validadorNumerico(sender, e);
        }

        private void validadorNumerico(object sender, KeyPressEventArgs e)
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

        private void txtCodigoCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            validadorNumerico(sender, e);
        }

        private void btnCanjear_Click(object sender, EventArgs e)
        {
            string cmd = string.Format("Select c.idCliente as idCliente, pc.puntos as Puntos from Clientes c left join PuntosxCliente pc on c.idCliente = pc.idCliente where c.codigo = '{0}' and c.Audit_Delete_Date is null and pc.Audit_Delete_Date is null", txtCodigoCliente.Text.Trim());
            DataSet ds = new DataSet();
            try
            {
                ds = Utilidades.Ejecutar(cmd);
            }
            catch (Exception error)
            {
                MessageBox.Show("Error al conectarse a la Base de Puntos: " + error.Message);
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("El usuario indicado no Exite");
                return;
            }

            PuntosxCliente puntosCliente = new PuntosxCliente()
            {
                IdCliente = (int)ds.Tables[0].Rows[0]["idCliente"],
                Puntos = (decimal)ds.Tables[0].Rows[0]["Puntos"]
            };

            PuntosCanjear puntosCanjear = new PuntosCanjear(puntosCliente, usuario);
            puntosCanjear.Show();
        }
    }
}
