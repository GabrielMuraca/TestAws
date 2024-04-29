using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PuntosTateti.GestionClientes
{
    public partial class AltaCliente : Form
    {
        string direccionImagen;
        Usuario usuario;
        int idClienteExterno = 0;
        public AltaCliente(Usuario _usuario)
        {
            InitializeComponent();
            usuario = _usuario;
        }
        public AltaCliente(Usuario _usuario, Cliente cliente)
        {
            InitializeComponent();
            usuario = _usuario;

            cbxCategoria.SelectedValue = cliente.Categoria;

            txtNombre.Text = cliente.Nombre;
            txtApellido.Text = cliente.Apellido;
            txtTelefono.Text = cliente.Telefono;
            txtMail.Text = cliente.Mail;
            txtDireccion.Text = cliente.Direccion;

            idClienteExterno = cliente.IdCliente;
        }
        private void AltaCliente_Load(object sender, EventArgs e)
        {
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
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (!Control())
                MessageBox.Show("Los campos indicados son incorrectos.");

            Cliente nuevoCliente = new Cliente()
            {
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Telefono = txtTelefono.Text.Trim(),
                Mail = txtMail.Text.Trim(),
                Direccion = txtDireccion.Text.Trim(),
                Categoria = int.Parse(cbxCategoria.SelectedValue.ToString()),
                Imagen = direccionImagen
            };

            if (idClienteExterno == 0)
            {
                DataSet baseClientes = baseClientesActual();

                nuevoCliente.Codigo = nuevoCodigo(baseClientes.Tables[0].AsEnumerable().Select(x => x[2].ToString()).ToList());

                bool clienteNoRepetido = buscarClienteRepetido(baseClientes);

                DialogResult dialogResult = new DialogResult();

                if (!clienteNoRepetido)
                {
                    dialogResult = MessageBox.Show("Ya existe un Cliente con el Nombre y Apellido Especificado. Desea actualizarlo?", "Cliente Repetido", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes)
                    {
                        int idCliente = baseClientes.Tables[0].AsEnumerable().Where(x => x[0].ToString().Trim().ToUpper() == txtNombre.Text.ToString().Trim().ToUpper() && x[1].ToString().Trim().ToUpper() == txtApellido.Text.ToString().Trim().ToUpper()).Select(x => (int)x[0]).FirstOrDefault();

                        nuevoCliente.IdCliente = idCliente;

                        if (actualizarCliente(nuevoCliente))
                            MessageBox.Show("Usuario Actualizado con Exito!");
                    }
                }
                else
                {
                    nuevoCliente = guardarUsuario(nuevoCliente);
                    if (nuevoCliente.IdCliente > 0)
                    {
                        MessageBox.Show("Usuario dado de Alta con Exito!");

                        new MetodosCredencial().MostrarCredencial(nuevoCliente);
                    }
                }
            }
            else
            {
                nuevoCliente.IdCliente = idClienteExterno;
                if (actualizarCliente(nuevoCliente))
                {
                    MessageBox.Show("Usuario Actualizado con Exito!");
                    this.Close();
                }  
            }
        }

        private bool actualizarCliente(Cliente cliente)
        {
            try
            {
                string cmd = string.Format("EXEC ActualizarCliente '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}'", cliente.IdCliente, cliente.Nombre, cliente.Apellido, cliente.Telefono, cliente.Mail, cliente.Direccion, cliente.Categoria, cliente.Imagen, usuario.Alias);

                Utilidades.Ejecutar(cmd);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al Guardar el Usuario: " + e.Message);
                return false;
            };
        }

        private Cliente guardarUsuario(Cliente cliente)
        {
            try
            {
                
                string cmd = string.Format("EXEC AltaCliente '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}'", cliente.Nombre, cliente.Apellido, cliente.Telefono, cliente.Mail, cliente.Direccion, cliente.Categoria, cliente.Codigo, cliente.Imagen, usuario.Alias);

                DataTable nuevoCliente = Utilidades.Ejecutar(cmd).Tables[0];

                cliente.IdCliente = int.Parse(nuevoCliente.Rows[0]["idCliente"].ToString());
                cliente.NroSocio = int.Parse(nuevoCliente.Rows[0]["nroSocio"].ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al Guardar el Usuario: " + e.Message);
            };
            return cliente;
        }

        private bool buscarClienteRepetido(DataSet baseClientes)
        {
            if (baseClientes.Tables[0].AsEnumerable().Where(x => x[1].ToString().Trim().ToUpper() == txtNombre.Text.ToString().Trim().ToUpper() && x[2].ToString().Trim().ToUpper() == txtApellido.Text.ToString().Trim().ToUpper()).Count() > 0)
                return false;

            return true;
        }

        private DataSet baseClientesActual()
        {
            DataSet response = new DataSet();
            try
            {
                string cmd = string.Format("Select idCliente, nombre, apellido, codigo from Clientes");
                response = Utilidades.Ejecutar(cmd);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al llamar a la base de Clientes: " + e.Message);
            }
            return response;
        }

        private string nuevoCodigo(List<string> codigosActuales)
        {
            Random generator = new Random();
            String codigo = generator.Next(0, 999999).ToString("D6");

            while(codigosActuales.Where(x => x == codigo).Count() < 0)
                codigo = generator.Next(0, 999999).ToString("D6");

            return codigo;
        }

        private bool Control()
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
            //Verifica Telefono
            if (txtTelefono.Text == string.Empty)
            {
                lblErrorTelefono.Visible = true;
                response = false;
            }
            else
            {
                lblErrorTelefono.Visible = false;
            }
            //Verifica Mail
            if (txtMail.Text == string.Empty || !Regex.IsMatch(txtMail.Text, @"(@)(.+)$"))
            {
                lblErrorMail.Visible = true;
                response = false;
            }
            else
            {
                lblErrorMail.Visible = false;
            }
            //Verifica Direccion
            if (txtDireccion.Text == string.Empty)
            {
                lblErrorDireccion.Visible = true;
                response = false;
            }
            else
            {
                lblErrorDireccion.Visible = false;
            }
            return response;
        }
    }
}
