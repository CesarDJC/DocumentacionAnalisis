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
    public partial class frmDistribucion : Form
    {
        public frmDistribucion()
        {
            InitializeComponent();
        }

        #region "Variables"
        int iCodigoProducto=0;
        int iCodigoProveedor = 0;
        bool bEstadoGuardar = true;
        bool bEstadoEliminar = false;
        #endregion

        #region "Metodos"
        private void CargarDistribucion() {
            D_ProductoProveedor datos=new D_ProductoProveedor();
            dgvDistribucion.DataSource = datos.ListarDistribucion();
            if (dgvDistribucion.Columns["IdProducto"]!=null && dgvDistribucion.Columns["IdProveedor"]!=null) {
                dgvDistribucion.Columns["IdProducto"].Visible = false;
                dgvDistribucion.Columns["IdProveedor"].Visible = false;

            }
        }

        private void CargarProductos() { 
            D_Productos datos=new D_Productos();
            cbxProductos.DataSource = datos.NombresProductos();
            cbxProductos.ValueMember = "IdProducto";
            cbxProductos.DisplayMember = "Nombre Producto";
        }

        private void CargarProveedores()
        {
            D_Proveedores datos = new D_Proveedores();
            cbxProveedores.DataSource = datos.NombresProveedores();
            cbxProveedores.ValueMember = "IdProveedor";
            cbxProveedores.DisplayMember = "Nombre Proveedor";
        }

        private void ActivarCampos(bool bEstado)
        {
            cbxProductos.Enabled = bEstado;
            cbxProveedores.Enabled= bEstado;
            txtCantidad.Enabled = bEstado;

        }

        private void ActivarBotones(bool bEstado)
        {

            btnAsigna.Enabled = bEstado;
            btnEdita.Enabled = bEstado;
            btnElimina.Enabled = bEstado;
            btnMenu.Enabled = bEstado;

            btnGuardar.Enabled = !bEstado;
            btnCancelar.Enabled = !bEstado;
        }

        private void SeleccionarDistribucion()
        {
            if (dgvDistribucion.CurrentRow != null)
            {
                var row = dgvDistribucion.CurrentRow;

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
                    // Obtener IdProducto (siempre debería existir)
                    iCodigoProducto = row.Cells["IdProducto"].Value != null ? Convert.ToInt32(row.Cells["IdProducto"].Value) : 0;
                    cbxProductos.SelectedValue = iCodigoProducto;

                    // Manejar IdProveedor (puede ser NULL)
                    if (row.Cells["IdProveedor"].Value != null && row.Cells["IdProveedor"].Value != DBNull.Value)
                    {
                        iCodigoProveedor = Convert.ToInt32(row.Cells["IdProveedor"].Value);
                        cbxProveedores.SelectedValue = iCodigoProveedor;
                    }
                    else
                    {
                        // Si no hay proveedor, deselecciona el ComboBox
                        cbxProveedores.SelectedIndex = -1; // Opción 1: Dejar en blanco
                                                           // O bien: cbxProveedores.SelectedValue = -1; // Si tienes un valor por defecto para "Sin proveedor"
                    }

                    // Manejar Cantidad Suministrada (puede ser NULL)
                    txtCantidad.Text = row.Cells["Cantidad Suministrada"].Value?.ToString() ?? "0";
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
        private void Guardar_Relacion()
        {
            E_ProductoProveedor ProdProv = new E_ProductoProveedor();

           
            if (cbxProductos.SelectedValue != null && cbxProveedores.SelectedValue != null)
            {
                ProdProv.IdProducto = (int)cbxProductos.SelectedValue;
                ProdProv.IdProveedor = (int)cbxProveedores.SelectedValue;

               
            }
            else
            {
                MessageBox.Show("Selecciona un producto y un proveedor válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            if (int.TryParse(txtCantidad.Text, out int cantidad))
            {
                ProdProv.CantidadSuministrada = cantidad;
            }
            else
            {
                MessageBox.Show("La cantidad no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            D_ProductoProveedor datos = new D_ProductoProveedor();
            string resultado = datos.GuardarRelacion(ProdProv);

            
            if (resultado == "existente")
            {
                MessageBox.Show("La relación ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "guardado")
            {
                CargarDistribucion();
                LimpiarControles();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("La relación se guardó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no guardado")
            {
                MessageBox.Show("No se guardó la relación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ActualizarRelacion()
        {
            E_ProductoProveedor ProdProv = new E_ProductoProveedor();

            // Verificar que se hayan seleccionado un producto y un proveedor válidos
            if (cbxProductos.SelectedValue == null || cbxProveedores.SelectedValue == null)
            {
                MessageBox.Show("Selecciona un producto y un proveedor válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Obtener los valores seleccionados
            ProdProv.IdProducto = Convert.ToInt32(cbxProductos.SelectedValue);
            ProdProv.IdProveedor = Convert.ToInt32(cbxProveedores.SelectedValue);

            // Obtener la cantidad
            if (!int.TryParse(txtCantidad.Text, out int cantidad))
            {
                MessageBox.Show("La cantidad no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ProdProv.CantidadSuministrada = cantidad;

            // Obtener los valores originales (deben estar almacenados en variables o controles)
            int idProductoOriginal = iCodigoProducto; // Valor original de IdProducto
            int idProveedorOriginal = iCodigoProveedor; // Valor original de IdProveedor

            // Llamar al método para editar la relación
            D_ProductoProveedor datos = new D_ProductoProveedor();
            string resultado = datos.Editar_Relacion(ProdProv, idProductoOriginal, idProveedorOriginal);

            // Mostrar el resultado
            if (resultado == "existente")
            {
                MessageBox.Show("La distribución ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "actualizado")
            {
                CargarDistribucion();
                LimpiarControles();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("La distribución se actualizó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no actualizado")
            {
                MessageBox.Show("No se actualizó la distribución", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Eliminar_Relacion()
        {
            E_ProductoProveedor ProdProv = new E_ProductoProveedor();


            if (cbxProductos.SelectedValue != null && cbxProveedores.SelectedValue != null)
            {
                ProdProv.IdProducto = (int)cbxProductos.SelectedValue;
                ProdProv.IdProveedor = (int)cbxProveedores.SelectedValue;


            }
            else
            {
                MessageBox.Show("Selecciona un producto y un proveedor válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (int.TryParse(txtCantidad.Text, out int cantidad))
            {
                ProdProv.CantidadSuministrada = cantidad;
            }
            else
            {
                MessageBox.Show("La cantidad no es válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            D_ProductoProveedor datos = new D_ProductoProveedor();
            string resultado = datos.Eliminar_Relacion(ProdProv);


             if (resultado == "eliminado")
            {
                CargarDistribucion();
                LimpiarControles();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("La relación se eliminó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bEstadoEliminar = false;
            }
            else if (resultado == "no eliminado")
            {
                MessageBox.Show("No se eliminó la relación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool validarTextos() {
            bool TextoVacio = false;
            if (string.IsNullOrEmpty(txtCantidad.Text)) TextoVacio = true;
            return TextoVacio;
        }


        private void LimpiarControles()
        {
            iCodigoProducto = 0;
            iCodigoProveedor = 0;
            cbxProductos.SelectedIndex = 0;
            cbxProveedores.SelectedIndex = 0;
            txtCantidad.Text = string.Empty;
          
        }
        #endregion

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu menu=new frmMenu();
            menu.ShowDialog();
        }

        private void frmDistribucion_Load(object sender, EventArgs e)
        {
            CargarDistribucion();
            CargarProductos();
            CargarProveedores();
            ActivarCampos(false);
            ActivarBotones(true);
        }

        private void button5_Click(object sender, EventArgs e)
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
                    Guardar_Relacion();
                }
                else if (bEstadoEliminar)
                {
                    Eliminar_Relacion();
                }
                else {
                    ActualizarRelacion();
                }
               
                
               
            }
        }

        private void btnAsigna_Click(object sender, EventArgs e)
        {
            bEstadoGuardar = true;
            bEstadoEliminar = false;
            iCodigoProducto = 0;
            iCodigoProveedor = 0;
            ActivarCampos(true);
            ActivarBotones(false);
            LimpiarControles();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ActivarCampos(false);
            ActivarBotones(true);
            LimpiarControles();
            iCodigoProducto = 0;
            iCodigoProveedor = 0;
            bEstadoGuardar = true;
            bEstadoEliminar = false;
        }

        private void dgvDistribucion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SeleccionarDistribucion();
        }

        private void btnEdita_Click(object sender, EventArgs e)
        {
            if (iCodigoProducto == 0 && iCodigoProveedor==0)
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

        private void dgvDistribucion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                ActivarCampos(true);
                ActivarBotones(false);

            }
        }
    }
}
