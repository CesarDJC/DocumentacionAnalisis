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
    public partial class frmProductos : Form
    {
        public frmProductos()
        {
            InitializeComponent();
        }

        #region "Variables"
        int iCodigoProducto = 0;
        bool bEstadoGuardar = true;
        bool bEstadoEliminar = false;
        #endregion

        #region "Metodos"
        private void CargarProductos() { 
            D_Productos datos=new D_Productos();
            dgvproductos.DataSource = datos.ListarProductos();

            if (dgvproductos.Columns["IdProducto"] !=null) {
                dgvproductos.Columns["IdProducto"].Visible = false;
               // dgvproductos.Columns["Imagen"].Visible = false;
            }
        }

        private void BusquedaProductos(string bus) {
            D_Productos datos = new D_Productos();
            dgvproductos.DataSource = datos.Busqueda(bus);
        }


        private void ActivarTextos(bool bEstado)
        {
            txtNombre.Enabled = bEstado;
            txtDescripcion.Enabled = bEstado;
            txtPrecio.Enabled = bEstado;
            txtImagen.Enabled = bEstado;
            txtStock.Enabled = bEstado;
            btnGuardar.Enabled = bEstado;
            btnCancelar.Enabled = bEstado;
            
            textBuscar.Enabled = !bEstado;


        }


        private void ActivarBotones(bool bEstado) {
            btnAgregar.Enabled = bEstado;
            btnEditar.Enabled = bEstado;
            btnEliminar.Enabled = bEstado;
            btnMenu.Enabled = bEstado;
        }
        private void SeleccionarProductos()
        {
            // Verifica si hay una fila seleccionada
            if (dgvproductos.CurrentRow != null)
            {
                // Obtiene la fila seleccionada
                var row = dgvproductos.CurrentRow;

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
                    iCodigoProducto = row.Cells["IdProducto"].Value != null ? (int)row.Cells["IdProducto"].Value : 0;
                    txtNombre.Text = row.Cells["Nombre Producto"].Value != null ? row.Cells["Nombre Producto"].Value.ToString() : string.Empty;
                    txtDescripcion.Text = row.Cells["Descripción"].Value != null ? row.Cells["Descripción"].Value.ToString() : string.Empty;
                    txtPrecio.Text = row.Cells["Precio"].Value != null ? row.Cells["Precio"].Value.ToString() : "0.00";
                    txtStock.Text = row.Cells["Stock"].Value != null ? row.Cells["Stock"].Value.ToString() : "0";
                    txtImagen.Text = row.Cells["Imagen"].Value != null ? row.Cells["Imagen"].Value.ToString() : string.Empty;

                    // Cargar la imagen en el PictureBox
                    string imagenUrl = row.Cells["Imagen"].Value?.ToString();
                    if (!string.IsNullOrEmpty(imagenUrl))
                    {
                        try
                        {
                            // Usar WebClient para descargar la imagen desde la URL
                            using (System.Net.WebClient webClient = new System.Net.WebClient())
                            {
                                byte[] imageBytes = webClient.DownloadData(imagenUrl);
                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes))
                                {
                                    pictureBoxImagen.Image = Image.FromStream(ms);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Si hay un error al cargar la imagen, mostrar una imagen por defecto o limpiar el PictureBox
                            pictureBoxImagen.Image = null;
                            MessageBox.Show("Error al cargar la imagen: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        // Si no hay URL de imagen, limpiar el PictureBox
                        pictureBoxImagen.Image = null;
                    }
                }
                else
                {
                    // Si la fila está en blanco, limpia los controles
                    LimpiarControles();
                    pictureBoxImagen.Image = null;
                }
            }
            else
            {
                // Si no hay fila seleccionada, limpia los controles
                LimpiarControles();
                pictureBoxImagen.Image = null;
            }
        }

        private void LimpiarControles()
        {
            iCodigoProducto = 0;
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtImagen.Text = string.Empty;
            txtPrecio.Text = "";
            txtStock.Text = "";
            pictureBoxImagen.Image = null;
        }


        private void GuardarProductos()
        {
            E_Producto Producto = new E_Producto();

            // Asignar valores directamente (no requieren conversión)
            Producto.NombreProducto = txtNombre.Text;
            Producto.Descripción = txtDescripcion.Text;
            Producto.Imagen = txtImagen.Text;

            // Convertir y asignar Precio (de string a decimal)
            if (decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                Producto.Precio = precio;
            }
            else
            {
                MessageBox.Show("El precio no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            //if (txtStock.Text.Length >= 5)
            //{
            //    MessageBox.Show("Ingrese una cantidad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return; // Detener el método si el stock no es válido
            //}
            //else {
            //    // Convertir y asignar Stock (de string a int)
            //    if (int.TryParse(txtStock.Text, out int stock))
            //    {

            //        Producto.Stock = stock;
            //    }
            //    else
            //    {
            //        MessageBox.Show("El stock no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return; // Detener el método si el stock no es válido
            //    }

            //}
       

            D_Productos Datos = new D_Productos();

            // Llamar al método para guardar el producto en la base de datos
            string resultado = Datos.Guardar_Producto(Producto);

            // Evaluar el resultado usando else if para evitar múltiples MessageBox
            if (resultado == "existente")
            {
                MessageBox.Show("El producto ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "guardado")
            {
                CargarProductos();
                LimpiarControles();
                ActivarTextos(false);
                ActivarBotones(true);
                MessageBox.Show("El producto se guardó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no guardado")
            {
                MessageBox.Show("No se guardó el producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void EditarProductos()
        {
            E_Producto Producto = new E_Producto();

            // Asignar valores directamente (no requieren conversión)
            Producto.IdProducto = iCodigoProducto;
            Producto.NombreProducto = txtNombre.Text;
            Producto.Descripción = txtDescripcion.Text;
            Producto.Imagen = txtImagen.Text;   
            // Convertir y asignar Precio (de string a decimal)
            if (decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                Producto.Precio = precio;
            }
            else
            {
                MessageBox.Show("El precio no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Detener el método si el precio no es válido
            }

            // Convertir y asignar Stock (de string a int)
            if (int.TryParse(txtStock.Text, out int stock))
            {
                Producto.Stock = stock;
            }
            else
            {
                MessageBox.Show("El stock no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Detener el método si el stock no es válido
            }

            D_Productos Datos = new D_Productos();

            // Llamar al método para guardar el producto en la base de datos
            string resultado = Datos.Editar_Producto(Producto);

            // Evaluar el resultado usando else if para evitar múltiples MessageBox
            if (resultado == "existente")
            {
                MessageBox.Show("El producto ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "actualizado")
            {
                CargarProductos();
                LimpiarControles();
                ActivarTextos(false);
                ActivarBotones(true);
                MessageBox.Show("El producto se guardó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no actualizado")
            {
                MessageBox.Show("No se guardó el producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarProductos()
        {
            E_Producto Producto = new E_Producto();

            // Asignar valores directamente (no requieren conversión)
            Producto.IdProducto = iCodigoProducto;
   

            D_Productos Datos = new D_Productos();

            // Llamar al método para guardar el producto en la base de datos
            string resultado = Datos.Eliminar_Producto(Producto);

            // Evaluar el resultado usando else if para evitar múltiples MessageBox
           if (resultado == "eliminado")
            {
                CargarProductos();
                LimpiarControles();
                ActivarTextos(false);
                ActivarBotones(true);
                MessageBox.Show("El producto se eliminó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        private bool validarTextos() {
            bool TextoVacio = false;
            if(string.IsNullOrEmpty(txtNombre.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtPrecio.Text)) TextoVacio = true;
            return TextoVacio;
        }


        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu menu=new frmMenu();
            menu.ShowDialog();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            BusquedaProductos(textBuscar.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bEstadoGuardar = true;
            bEstadoEliminar = false;
            iCodigoProducto = 0;
            ActivarTextos(true);
            ActivarBotones(false);
            LimpiarControles();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bEstadoGuardar = true;
            bEstadoEliminar = false;
            iCodigoProducto = 0;
            ActivarTextos(false);
            ActivarBotones(true);
            LimpiarControles();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dgvproductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SeleccionarProductos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (iCodigoProducto == 0)
            {
                MessageBox.Show("Selecciona un registro", "Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else {
                bEstadoGuardar=false;
                ActivarTextos(true);
                ActivarBotones(false);
               
                
            }
            
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarTextos())
            {
                MessageBox.Show("Hay campos vacios", "Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else {
                if (bEstadoGuardar)
                {
                    GuardarProductos();
                }
                else if (bEstadoEliminar) {
                    EliminarProductos();
                }
                else {
                    EditarProductos();
                }
                
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (iCodigoProducto == 0)
            {
                MessageBox.Show("Selecciona un registro", "Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                bEstadoGuardar = false;
                bEstadoEliminar = true;
                ActivarTextos(true);
                ActivarBotones(false);


            }

        }

        private void pictureBoxImagen_Click(object sender, EventArgs e)
        {

        }
    }
}
