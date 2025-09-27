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
    public partial class frmSalidaProductos : Form
    {
        public frmSalidaProductos()
        {
            InitializeComponent();
        }

        bool bEstadoAsignar = false;
        private int IdSalidaEditar = 0;

        #region "Metodos"
        private void CargarProductos()
        {
            D_Productos datos = new D_Productos();
            cbxProductos.DataSource = datos.NombresProductos();
            cbxProductos.ValueMember = "IdProducto";
            cbxProductos.DisplayMember = "Nombre Producto";
        }

        private void MostrarSalidas() {
            D_SalidaProductos datos =new  D_SalidaProductos();
            dgvSalidas.DataSource = datos.MostrarSalidas();
            if (dgvSalidas.Columns["IdSalida"]!=null) {
                dgvSalidas.Columns["IdSalida"].Visible = false;
            }
        }


        private void GuardarSalida() {
            E_SalidaProductos Salida = new E_SalidaProductos();

            if (cbxProductos.SelectedValue !=null) {
                Salida.IdProducto = (int)cbxProductos.SelectedValue;
            }
            else
            {
                MessageBox.Show("Selecciona un producto válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(txtCantidad.Text, out int cantidad))
            {
                Salida.Cantidad= cantidad;
            }
            else
            {
                MessageBox.Show("La cantidad no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //if (cbxClientes.SelectedItem != null)
            //{
            //    DataRowView row = (DataRowView)cbxClientes.SelectedItem;
            //    Salida.NitCliente = row["Nit"].ToString(); // Obtenemos el NIT directamente
            //}
            string nit = txtNIT.Text.Trim();
            if (string.IsNullOrEmpty(nit))
            {
                MessageBox.Show("El NIT no puede estar vacío.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            D_SalidaProductos datosCliente = new D_SalidaProductos();
            var cliente = datosCliente.BuscarClientePorNit(nit);
            if (cliente == null || cliente.Count == 0)
            {
                nit = "CF"; // Forzar NIT de Consumidor Final
            }

            Salida.NitCliente = nit;

            D_SalidaProductos datos = new D_SalidaProductos();
            string resultado = datos.GuardarSalida(Salida);
            if (resultado == "insuficiente")
            {
                MessageBox.Show("El stock es insuficiente para esa cantidad", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "Salida guardada")
            {
                LimpiarCampos();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("La Salida de productos se guardó correctamente");
                MostrarSalidas();
                bEstadoAsignar = false;

            }
            else if (resultado == "Salida no guardada")
            {
                MessageBox.Show("No se realizó la Salida de productos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void EditarSalida() {
            if (dgvSalidas.SelectedRows.Count > 0)
            {
                DataGridViewRow fila = dgvSalidas.SelectedRows[0];
                IdSalidaEditar = Convert.ToInt32(fila.Cells["IdSalida"].Value);

            
                D_SalidaProductos datos = new D_SalidaProductos();
                DataTable dtSalida = datos.BuscarSalidaPorId(IdSalidaEditar);

                if (dtSalida != null && dtSalida.Rows.Count > 0)
                {
                    DataRow row = dtSalida.Rows[0];

                 
                    cbxProductos.SelectedValue = row["IdProducto"];
                    txtCantidad.Text = row["Cantidad"].ToString();

                    ActivarCampos(true);
                    ActivarBotones(false);
                    bEstadoAsignar = false; 
                }
            }
            else
            {
                MessageBox.Show("Seleccione una salida para editar", "Advertencia",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ActualizarSalida()
        {
            E_SalidaProductos Salida = new E_SalidaProductos();
            Salida.IdSalida = IdSalidaEditar;

            if (cbxProductos.SelectedValue != null)
            {
                Salida.IdProducto = (int)cbxProductos.SelectedValue;
            }
            else
            {
                MessageBox.Show("Selecciona un producto válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(txtCantidad.Text, out int cantidad))
            {
                Salida.Cantidad = cantidad;
            }
            else
            {
                MessageBox.Show("La cantidad no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Salida.FechaSalida = DateTime.Now;

            D_SalidaProductos datos = new D_SalidaProductos();
            string resultado = datos.EditarSalida(Salida);

            ProcesarResultadoOperacion(resultado);
        }

        private void ProcesarResultadoOperacion(string resultado)
        {
            if (resultado == "insuficiente")
            {
                MessageBox.Show("El stock es insuficiente para esa cantidad", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado.Contains("guardada") || resultado.Contains("actualizada"))
            {
                LimpiarCampos();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("Operación realizada correctamente");
                bEstadoAsignar = false;
                MostrarSalidas();
            }
            else if (resultado == "Salida no guardada")
            {
                MessageBox.Show("No se realizó la Salida de productos", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuscarSalidasPorFechas()
        {
            DateTime fechaInicio = dtpFecha1.Value;
            DateTime fechaFin = dtpFecha2.Value;

          
            if (fechaInicio > fechaFin)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha final",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            D_SalidaProductos datos = new D_SalidaProductos();
            DataTable resultados = datos.BuscarSalidasPorFecha(fechaInicio, fechaFin);

            if (resultados != null && resultados.Rows.Count > 0)
            {
                dgvSalidas.DataSource = resultados;
                if (dgvSalidas.Columns["IdSalida"] != null)
                {
                    dgvSalidas.Columns["IdSalida"].Visible = false;
                }
            }
            else
            {
                MessageBox.Show("No se encontraron salidas en el rango de fechas especificado",
                               "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ActivarCampos(bool bEstado)
        {
            cbxProductos.Enabled = bEstado;
            txtCantidad.Enabled = bEstado;
            //cbxClientes.Enabled = bEstado;
            txtNIT.Enabled = bEstado;

        }

        private void ActivarFechas(bool bEstado) {
            dtpFecha1.Enabled = bEstado;
            dtpFecha2.Enabled= bEstado;
        }

        private void LimpiarCampos()
        {
            cbxProductos.SelectedIndex = 0;
            txtCantidad.Text = string.Empty;
            txtNIT.Text = string.Empty;
            txtNombreC.Text = string.Empty;
            txtDireccionC.Text = string.Empty;
            txtTelefonoC.Text = string.Empty;
            txtCorreoC.Text = string.Empty;
        }



        private void ActivarBotones(bool bEstado)
        {

            btnAsigna.Enabled = bEstado;
            btnEdita.Enabled = bEstado;
            btnFechas.Enabled = bEstado;
            btnMenu.Enabled = bEstado;


            btnGuardar.Enabled = !bEstado;
            btnCancelar.Enabled = !bEstado;
            btnBuscar.Enabled = !bEstado;
        }

        private bool ValidarTextos() {
            bool TextoVacio = false;
            if (string.IsNullOrEmpty(txtCantidad.Text)) TextoVacio = true;
            return TextoVacio;
            
        }

        private void CargarClientes()
        {
            D_SalidaProductos datos = new D_SalidaProductos();
            DataTable dtClientes = datos.ObtenerClientes();

            //if (dtClientes != null)
            //{


            //    cbxClientes.DataSource = dtClientes;
            //    cbxClientes.ValueMember = "Id";
            //    cbxClientes.DisplayMember = "Nit";
            //}
        }

        private void MostrarDatosCliente(int idCliente)
        {
            D_SalidaProductos datos = new D_SalidaProductos();
            var datosCliente = datos.ObtenerDatosCliente(idCliente);

            if (datosCliente != null)
            {
                txtNombreC.Text = datosCliente["nombre"];
                txtDireccionC.Text = datosCliente["direccion"];
                txtTelefonoC.Text = datosCliente["telefono"];
                txtCorreoC.Text = datosCliente["correo"];
            }
        }


        #endregion

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu menu=new frmMenu();
            menu.ShowDialog();
        }

        private void dgvDistribucion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmSalidaProductos_Load(object sender, EventArgs e)
        {
            MostrarSalidas();
            ActivarCampos(false);
            ActivarBotones(true);
            ActivarFechas(false);
            CargarProductos();
            CargarClientes();
        }

        private void btnAsigna_Click(object sender, EventArgs e)
        {
            ActivarCampos(true);
            ActivarBotones(false);
            bEstadoAsignar = true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (dtpFecha1.Enabled || dtpFecha2.Enabled)
            {
                // Si está activa la búsqueda por fechas
                ActivarFechas(false);

            }

            ActivarCampos(false);
            ActivarBotones(true);
            LimpiarCampos();
        }

        private void btnFechas_Click(object sender, EventArgs e)
        {
            ActivarFechas(true);
            ActivarCampos(false);
            ActivarBotones(false);

            // Establecer fechas por defecto (últimos 7 días)
            dtpFecha2.Value = DateTime.Now;
            dtpFecha1.Value = DateTime.Now;

           
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (btnFechas.Enabled == false && dtpFecha1.Enabled && dtpFecha2.Enabled)
            {
                // Estamos en modo búsqueda por fechas
                BuscarSalidasPorFechas();
                ActivarFechas(false);
                ActivarBotones(true);
            }
            else if (ValidarTextos())
            {
                MessageBox.Show("Hay campos vacios", "Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                if (bEstadoAsignar)
                {
                    GuardarSalida();
                }
                else
                {
                    ActualizarSalida();
                }
            }
        }

        private void btnEdita_Click(object sender, EventArgs e)
        {
            EditarSalida();
        }

        //private void cbxClientes_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cbxClientes.SelectedItem != null)
        //    {
        //        try
        //        {

        //            // Obtener el DataRowView seleccionado
        //            DataRowView row = (DataRowView)cbxClientes.SelectedItem;

        //            // Obtener el ID del cliente (valor real)
        //            int idCliente = Convert.ToInt32(row["Id"]);

        //            // Obtener el NIT (valor mostrado)
        //            string nit = row["Nit"].ToString();

        //            // Mostrar datos del cliente
        //            MostrarDatosCliente(idCliente);


        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error al obtener cliente: {ex.Message}", "Error");
        //        }
        //    }
        //}

        private void BuscarClientePorNit()
        {
            string nit = txtNIT.Text.Trim();

            if (string.IsNullOrEmpty(nit))
            {
                MessageBox.Show("Por favor ingrese un NIT para buscar", "Advertencia",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            D_SalidaProductos datos = new D_SalidaProductos();
            var cliente = datos.BuscarClientePorNit(nit);

            if (cliente != null && cliente.Count > 0)
            {
                // Mostrar los datos del cliente encontrado
                txtNombreC.Text = cliente["nombre"];
                txtDireccionC.Text = cliente["direccion"];
                txtTelefonoC.Text = cliente["telefono"];
                txtCorreoC.Text = cliente["correo"];
            }
            else
            {
                // Si no se encuentra, usar datos de "Consumidor Final"
                MessageBox.Show("Cliente no encontrado. Se usarán los datos de Consumidor Final",
                              "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Datos predeterminados de Consumidor Final
                txtNIT.Text = "CF";
                txtNombreC.Text = "CONSUMIDOR FINAL";
                txtDireccionC.Text = "CIUDAD";
                txtTelefonoC.Text = "00000000";
                txtCorreoC.Text = "consumidorfinal@example.com";
            }
        }

        //private void btnID_Click(object sender, EventArgs e)
        //{
        //    if (cbxClientes.SelectedValue != null)
        //    {
        //        int idCliente = (int)cbxClientes.SelectedValue;
        //        MessageBox.Show($"El ID del cliente seleccionado es: {idCliente}",
        //                       "ID Cliente",
        //                       MessageBoxButtons.OK,
        //                       MessageBoxIcon.Information);
        //    }
        //    else
        //    {
        //        MessageBox.Show("No hay ningún cliente seleccionado",
        //                       "Advertencia",
        //                       MessageBoxButtons.OK,
        //                       MessageBoxIcon.Warning);
        //    }
        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarClientePorNit();
        }
    }
}
