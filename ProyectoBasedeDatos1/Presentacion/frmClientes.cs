using Newtonsoft.Json;
using ProyectoBasedeDatos1.Datos;
using ProyectoBasedeDatos1.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBasedeDatos1.Presentacion
{
    public partial class frmClientes : Form
    {
        public frmClientes()
        {
            InitializeComponent();
        }
        #region "Variables"
        int iCodigoCliente = 0;
        bool bEstadoEliminar = false;
        bool bEstadoGuardar = true;
        #endregion

        #region "Métodos"
        private void CargarClientes()
        {
            D_Clientes datos = new D_Clientes();
            dgvClientes.DataSource = datos.ListarClientes();

            // Configurar columnas del DataGridView
            if (dgvClientes.Columns["Id"] != null)
            {
                dgvClientes.Columns["Id"].Visible = false;
            }

            // Agregar columnas personalizadas para mostrar datos del JSON
            if (dgvClientes.Columns["datos_cliente"] != null)
            {
                dgvClientes.Columns["datos_cliente"].Visible = false;

                // Agregar columnas para mostrar datos específicos
                if (dgvClientes.Columns["Nombre"] == null)
                {
                    DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn();
                    colNombre.Name = "Nombre";
                    colNombre.HeaderText = "Nombre";
                    dgvClientes.Columns.Add(colNombre);
                }

                if (dgvClientes.Columns["Nit"] == null)
                {
                    DataGridViewTextBoxColumn colNit = new DataGridViewTextBoxColumn();
                    colNit.Name = "Nit";
                    colNit.HeaderText = "NIT";
                    dgvClientes.Columns.Add(colNit);
                }

                if (dgvClientes.Columns["Telefono"] == null)
                {
                    DataGridViewTextBoxColumn colTelefono = new DataGridViewTextBoxColumn();
                    colTelefono.Name = "Telefono";
                    colTelefono.HeaderText = "Teléfono";
                    dgvClientes.Columns.Add(colTelefono);
                }

                // Llenar las columnas con datos del JSON
                foreach (DataGridViewRow row in dgvClientes.Rows)
                {
                    if (row.Cells["datos_cliente"].Value != null)
                    {
                        dynamic cliente = JsonConvert.DeserializeObject(row.Cells["datos_cliente"].Value.ToString());
                        row.Cells["Nombre"].Value = cliente.nombre;
                        row.Cells["Nit"].Value = cliente.nit;
                        row.Cells["Telefono"].Value = cliente.telefono;
                    }
                }
            }
        }

        private void BusquedaClientes(string bus)
        {
            D_Clientes datos = new D_Clientes();
            dgvClientes.DataSource = datos.Busqueda(bus);
        }

        private void ActivarCampos(bool bEstado)
        {
            txtNit.Enabled = bEstado;
            txtNombre.Enabled = bEstado;
            txtTelefono.Enabled = bEstado;
            txtCorreo.Enabled = bEstado;
            txtDireccion.Enabled = bEstado;
            textBuscar.Enabled = !bEstado;
        }

        private void ActivarBotones(bool bEstado)
        {
            btnAgrega.Enabled = bEstado;
            btnEdita.Enabled = bEstado;
            btnElimina.Enabled = bEstado;
            btnMenu.Enabled = bEstado;

            btnGuardar.Enabled = !bEstado;
            btnCancelar.Enabled = !bEstado;
        }

        private void SeleccionarClientes()
        {
            if (dgvClientes.CurrentRow != null)
            {
                var row = dgvClientes.CurrentRow;
                iCodigoCliente = row.Cells["Id"].Value != null ? (int)row.Cells["Id"].Value : 0;

                if (row.Cells["datos_cliente"].Value != null)
                {
                    CargarDatosDesdeJson(row.Cells["datos_cliente"].Value.ToString());
                }
                else
                {
                    LimpiarControles();
                }
            }
            else
            {
                LimpiarControles();
            }
        }

        private void CargarDatosDesdeJson(string json)
        {
            try
            {
                dynamic cliente = JsonConvert.DeserializeObject(json);
                txtNit.Text = cliente.nit;
                txtNombre.Text = cliente.nombre;
                txtDireccion.Text = cliente.direccion;
                txtTelefono.Text = cliente.telefono;
                txtCorreo.Text = cliente.correo;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos del cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimpiarControles();
            }
        }

        private string ConstruirJsonCliente()
        {
            try
            {
                var cliente = new
                {
                    nit = txtNit.Text,
                    nombre = txtNombre.Text,
                    direccion = txtDireccion.Text,
                    telefono = txtTelefono.Text,
                    correo = txtCorreo.Text
                };

                return JsonConvert.SerializeObject(cliente);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al construir los datos del cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        private void LimpiarControles()
        {
            iCodigoCliente = 0;
            txtNit.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtDireccion.Text = string.Empty;
        }

        private void GuardarClientes()
        {
            if (validarTextos())
            {
                MessageBox.Show("Hay campos vacíos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jsonCliente = ConstruirJsonCliente();
            if (string.IsNullOrEmpty(jsonCliente))
            {
                return;
            }

            E_Clientes cliente = new E_Clientes();
            cliente.datos_cliente = jsonCliente;

            D_Clientes Datos = new D_Clientes();
            string resultado = Datos.Guardar_Cliente(cliente);

            if (resultado == "existente")
            {
                MessageBox.Show("El cliente con este NIT ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "guardado")
            {
                CargarClientes();
                LimpiarControles();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("El cliente se guardó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no guardado")
            {
                MessageBox.Show("No se guardó el cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditarClientes()
        {
            if (validarTextos())
            {
                MessageBox.Show("Hay campos vacíos", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jsonCliente = ConstruirJsonCliente();
            if (string.IsNullOrEmpty(jsonCliente))
            {
                return;
            }

            E_Clientes cliente = new E_Clientes();
            cliente.datos_cliente = jsonCliente;

            D_Clientes Datos = new D_Clientes();
            string resultado = Datos.Editar_Cliente(iCodigoCliente, cliente);

            if (resultado == "existente")
            {
                MessageBox.Show("El cliente con este NIT ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "actualizado")
            {
                CargarClientes();
                LimpiarControles();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("El cliente se actualizó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no actualizado")
            {
                MessageBox.Show("No se actualizó el cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarCliente()
        {
            D_Clientes Datos = new D_Clientes();
            string resultado = Datos.Eliminar_Cliente(iCodigoCliente);

            if (resultado == "eliminado")
            {
                CargarClientes();
                LimpiarControles();
                ActivarCampos(false);
                ActivarBotones(true);
                MessageBox.Show("El cliente se eliminó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bEstadoEliminar = false;
            }
            else if (resultado == "no eliminado")
            {
                MessageBox.Show("No se eliminó el cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool validarTextos()
        {
            bool TextoVacio = false;
            if (string.IsNullOrEmpty(txtNit.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtNombre.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtDireccion.Text)) TextoVacio = true;
            return TextoVacio;
        }
        #endregion


        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmClientes_Load(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu menu= new frmMenu();
            menu.ShowDialog();
            this.Close();

        }

        private void btnAgrega_Click(object sender, EventArgs e)
        {
            bEstadoGuardar = true;
            bEstadoEliminar = false;
            iCodigoCliente = 0;
            ActivarCampos(true);
            ActivarBotones(false);
            LimpiarControles();
            txtNit.Focus();
        }

        private void btnEdita_Click(object sender, EventArgs e)
        {
            if (iCodigoCliente == 0)
            {
                MessageBox.Show("Selecciona un registro", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                bEstadoGuardar = false;
                ActivarCampos(true);
                ActivarBotones(false);
            }
        }

        private void btnElimina_Click(object sender, EventArgs e)
        {
            if (iCodigoCliente == 0)
            {
                MessageBox.Show("Selecciona un registro", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (MessageBox.Show("¿Estás seguro de eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bEstadoGuardar = false;
                    bEstadoEliminar = true;
                    ActivarCampos(false);
                    ActivarBotones(false);
                    EliminarCliente();
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (bEstadoGuardar)
            {
                GuardarClientes();
            }
            else if (bEstadoEliminar)
            {
                EliminarCliente();
            }
            else
            {
                EditarClientes();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bEstadoGuardar = true;
            bEstadoEliminar = false;
            iCodigoCliente = 0;
            ActivarCampos(false);
            ActivarBotones(true);
            LimpiarControles();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SeleccionarClientes();
        }
    }
}
