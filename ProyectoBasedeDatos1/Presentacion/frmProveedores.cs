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
    public partial class frmProveedores : Form
    {
        public frmProveedores()
        {
            InitializeComponent();
        }
        #region "Variables"
        int iCodigoProveedor = 0;
        bool bEstadoEliminar = false;
        bool bEstadoGuardar = true;
        #endregion
        #region "Metodos"
        private void CargarProveedores() {
            D_Proveedores datos=new D_Proveedores();    
            dgvproveedores.DataSource = datos.ListarProveedores();

            if (dgvproveedores.Columns["IdProveedor"] !=null) {
                dgvproveedores.Columns["IdProveedor"].Visible = false;
            }
        }


        private void BusquedaProveedores(string bus) {
            D_Proveedores datos = new D_Proveedores();
            dgvproveedores.DataSource = datos.Busqueda(bus);
        }


        private void ActivarCampos(bool bEstado) { 
            txtNombre.Enabled= bEstado;
            txtTelefono.Enabled = bEstado;
            txtCorreo.Enabled = bEstado;
            txtDireccion.Enabled = bEstado;
            textBuscar.Enabled = !bEstado;
            
        }

        private void ActivarBotones(bool bEstado) {

            btnAgrega.Enabled= bEstado;
            btnEdita.Enabled= bEstado;
            btnElimina.Enabled= bEstado;
            btnMenu.Enabled= bEstado;

            btnGuardar.Enabled = !bEstado;
            btnCancelar.Enabled = !bEstado;
        }

        private void SeleccionarProveedores()
        {
            // Verifica si hay una fila seleccionada
            if (dgvproveedores.CurrentRow != null)
            {
                // Obtiene la fila seleccionada
                var row = dgvproveedores.CurrentRow;


                bool filaEnBlanco = true;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        filaEnBlanco = false;
                        break;
                    }
                }


                if (!filaEnBlanco)
                {
                    iCodigoProveedor = row.Cells["IdProveedor"].Value != null ? (int)row.Cells["IdProveedor"].Value : 0;
                    txtNombre.Text = row.Cells["Nombre Proveedor"].Value != null ? row.Cells["Nombre Proveedor"].Value.ToString() : string.Empty;
                    txtDireccion.Text = row.Cells["Direccion"].Value != null ? row.Cells["Direccion"].Value.ToString() : string.Empty;
                    txtTelefono.Text = row.Cells["Telefono"].Value != null ? row.Cells["Telefono"].Value.ToString() : "";
                    txtCorreo.Text = row.Cells["Correo Electronico"].Value != null ? row.Cells["Correo Electronico"].Value.ToString() : "";
                }
                else
                {
                    // Si la fila está en blanco, limpia los controles
                    LimpiarControles();
                }
            }
            else
            {
                // Si no hay fila seleccionada, limpia los controles
                LimpiarControles();
            }
        }

        private void LimpiarControles()
        {
            iCodigoProveedor = 0;
            txtNombre.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtTelefono.Text = "";
            txtDireccion.Text = "";
        }

        private void GuardarProveedores()
        {
           E_Proveedor Proveedor= new E_Proveedor();

            // Asignar valores directamente (no requieren conversión)

            Proveedor.NombreProveedor=txtNombre.Text;
            Proveedor.Direccion=txtDireccion.Text;
            Proveedor.CorreoElectronico=txtCorreo.Text;

            if (txtTelefono.Text.Length == 8)
            {
                if (long.TryParse(txtTelefono.Text, out long telefono))
                {
                    Proveedor.Telefono = telefono;
                }
                else
                {
                    MessageBox.Show("El telefono no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else {
                MessageBox.Show("Pon un teléfono válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            D_Proveedores Datos= new D_Proveedores();
            string resultado = Datos.Guardar_Proveedor(Proveedor);

            if (resultado == "existente")
            {
                MessageBox.Show("El proveedor ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado=="t existe") {
                MessageBox.Show("El telefono ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "guardado")
            {
                CargarProveedores();
                LimpiarControles();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("El proveedor se guardó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no guardado") {
                MessageBox.Show("No se guardó el proveedor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void EditarProveedores()
        {
            E_Proveedor Proveedor = new E_Proveedor();

            // Asignar valores directamente (no requieren conversión)
            Proveedor.IdProveedor = iCodigoProveedor;
            Proveedor.NombreProveedor = txtNombre.Text;
            Proveedor.Direccion = txtDireccion.Text;
            Proveedor.CorreoElectronico = txtCorreo.Text;

            if (txtTelefono.Text.Length == 8)
            {
                if (long.TryParse(txtTelefono.Text, out long telefono))
                {
                    Proveedor.Telefono = telefono;
                }
                else
                {
                    MessageBox.Show("El telefono no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Pon un teléfono válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            D_Proveedores Datos = new D_Proveedores();
            string resultado = Datos.Editar_Proveedor(Proveedor);

            if (resultado == "existente")
            {
                MessageBox.Show("El proveedor ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "t existe")
            {
                MessageBox.Show("El telefono ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "actualizado")
            {
                CargarProveedores();
                LimpiarControles();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("El proveedor se actualizó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no actualizado")
            {
                MessageBox.Show("No se actualizó el proveedor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void EliminarProveedor()
        {
            E_Proveedor Proveedor = new E_Proveedor();

            // Asignar valores directamente (no requieren conversión)
            Proveedor.IdProveedor = iCodigoProveedor;


            D_Proveedores Datos = new D_Proveedores();

            // Llamar al método para guardar el producto en la base de datos
            string resultado = Datos.Eliminar_Proveedor(Proveedor);

            // Evaluar el resultado usando else if para evitar múltiples MessageBox
            if (resultado == "eliminado")
            {
                CargarProveedores();
                LimpiarControles();
                ActivarCampos(false);
                ActivarBotones(true);
                MessageBox.Show("El proveedor se eliminó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bEstadoEliminar = false;
            }
            else if (resultado == "no eliminado")
            {
                MessageBox.Show("No se eliminó", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool validarTextos()
        {
            bool TextoVacio = false;
            if (string.IsNullOrEmpty(txtNombre.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtCorreo.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtDireccion.Text)) TextoVacio = true;
            return TextoVacio;
        }

        #endregion



        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu menu= new frmMenu();
            menu.ShowDialog();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void frmProveedores_Load(object sender, EventArgs e)
        {
            CargarProveedores();
        }

        private void textBuscar_TextChanged(object sender, EventArgs e)
        {
            BusquedaProveedores(textBuscar.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bEstadoGuardar = true;
            bEstadoEliminar = false;
            iCodigoProveedor = 0;
            ActivarCampos(true);
            ActivarBotones(false);
            LimpiarControles();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bEstadoGuardar = true;
            bEstadoEliminar = false;
            iCodigoProveedor = 0;
            ActivarCampos(false);
            ActivarBotones(true);
            LimpiarControles();
        }

        private void btnEdita_Click(object sender, EventArgs e)
        {
            if (iCodigoProveedor == 0)
            {
                MessageBox.Show("Selecciona un registro", "Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                bEstadoGuardar = false;
                ActivarCampos(true);
                ActivarBotones(false);

            }
        }

        private void dgvproveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SeleccionarProveedores();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarTextos())
            {
                MessageBox.Show("Hay campos vacios", "Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                if (bEstadoGuardar)
                {
                    GuardarProveedores();
                }
                else if (bEstadoEliminar)
                {
                    EliminarProveedor();

                }
                else {
                    EditarProveedores();
                }
            }
        }

        private void btnElimina_Click(object sender, EventArgs e)
        {
            if (iCodigoProveedor == 0)
            {
                MessageBox.Show("Selecciona un registro", "Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                bEstadoGuardar = false;
                bEstadoEliminar = true;
                ActivarCampos(false);
                ActivarBotones(false);

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dgvproveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtCorreo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtDireccion_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
