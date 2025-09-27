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
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
            cbxTipo.Items.Add("Usuario");
            cbxTipo.Items.Add("Administrador");
            cbxTipo.SelectedIndex = 0;
        }
        #region "Variables"
        int iCodigoUsuario = 0;
        bool bEstadoEliminar = false;
        bool bEstadoGuardar = true;
        #endregion

        #region "Metodos"
        private void CargarUsuarios()
        {
            D_Usuarios datos = new D_Usuarios();
            dgvproveedores.DataSource = datos.ListarUsuarios();

            if (dgvproveedores.Columns["IdUsuario"] != null)
            {
                dgvproveedores.Columns["IdUsuario"].Visible = false;
            }
        }

        private void Busqueda(string bus) { 
            D_Usuarios datos=new D_Usuarios();
            dgvproveedores.DataSource = datos.BusquedaUsuarios(bus);
        }

        private bool validarTextos()
        {
            bool TextoVacio = false;
            if (string.IsNullOrEmpty(txtNombre.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtCorreo.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtContraseña.Text)) TextoVacio = true;
            return TextoVacio;
        }
        private bool validarTextosContraseña()
        {
            bool TextoVacio = false;
            if (string.IsNullOrEmpty(txtNombre.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtCorreo.Text)) TextoVacio = true;
            return TextoVacio;
        }

        private void ActivarCampos(bool bEstado)
        {

            txtNombre.Enabled = bEstado;
            txtCorreo.Enabled = bEstado;
            txtContraseña.Enabled = bEstado;
            txtBuscar.Enabled = !bEstado;
            cbxTipo.Enabled = !bEstado;

        }

        private void ActivarCamposEditar(bool bEstado)
        {
            txtNombre.Enabled = bEstado;
            txtCorreo.Enabled = bEstado;
            txtContraseña.Enabled = !bEstado;
            txtBuscar.Enabled = !bEstado;
            cbxTipo.Enabled= bEstado;

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

        private void LimpiarControles()
        {
            iCodigoUsuario = 0;
            txtNombre.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtContraseña.Text = "";
            cbxTipo.SelectedIndex = 0;
        }
        private void SeleccionarUsuarios()
        {
          
            if (dgvproveedores.CurrentRow != null)
            {
                
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
                    iCodigoUsuario = row.Cells["IdUsuario"].Value != null ? (int)row.Cells["IdUsuario"].Value : 0;
                    txtNombre.Text = row.Cells["Nombre Usuario"].Value != null ? row.Cells["Nombre Usuario"].Value.ToString() : string.Empty;
                    txtCorreo.Text = row.Cells["Correo Electronico"].Value != null ? row.Cells["Correo Electronico"].Value.ToString() : "";
                    if (row.Cells["Tipo de Usuario"].Value != null)
                    {
                        string tipo = row.Cells["Tipo de Usuario"].Value.ToString();
                        cbxTipo.SelectedIndex = tipo == "Administrador" ? 1 : 0;
                    }
                    else {
                        cbxTipo.SelectedIndex = 0;
                    }
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


        private void GuardarUsuarios()
        {
            E_Usuario Usuario = new E_Usuario();



            if (txtContraseña.Text.Length < 4)
            {
                MessageBox.Show("La contraseña es demasiado corta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else {
                Usuario.Contraseña = txtContraseña.Text;
            }
            

            Usuario.NombreUsuario = txtNombre.Text;
           
            Usuario.CorreoElectronico = txtCorreo.Text;

           

            D_Usuarios Datos = new D_Usuarios();
            string resultado = Datos.Guardar_Usuario(Usuario);


            if (resultado == "existente")
            {
                MessageBox.Show("El usuario ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            else if (resultado == "guardado")
            {
                CargarUsuarios();
                LimpiarControles();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("El usuario se guardó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no guardado")
            {
                MessageBox.Show("No se guardó el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }





        private void EditarUsuarios()
        {
            E_Usuario Usuario = new E_Usuario();

            // Asignar valores directamente (no requieren conversión)
            Usuario.IdUsuario = iCodigoUsuario;
            Usuario.NombreUsuario = txtNombre.Text;
            Usuario.CorreoElectronico = txtCorreo.Text;
            Usuario.TipoUsuario = cbxTipo.SelectedIndex == 1;
            

            D_Usuarios Datos = new D_Usuarios();
            string resultado = Datos.Editar_Usuario(Usuario);

            if (resultado == "existente")
            {
                MessageBox.Show("El usuario ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (resultado == "actualizado")
            {
                CargarUsuarios();
                LimpiarControles();
                ActivarBotones(true);
                ActivarCampos(false);
                MessageBox.Show("El usuario se actualizó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (resultado == "no actualizado")
            {
                MessageBox.Show("No se actualizó el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        private void EliminarUsuario()
        {
            E_Usuario Usuario = new E_Usuario();

            // Asignar valores directamente (no requieren conversión)
            Usuario.IdUsuario = iCodigoUsuario;


            D_Usuarios Datos = new D_Usuarios();

            // Llamar al método para guardar el producto en la base de datos
            string resultado = Datos.Eliminar_Usuario(Usuario);

            // Evaluar el resultado usando else if para evitar múltiples MessageBox
            if (resultado == "eliminado")
            {
                CargarUsuarios();
                LimpiarControles();
                ActivarCampos(false);
                ActivarBotones(true);
                MessageBox.Show("El usuario se eliminó correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        #endregion

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu menu = new frmMenu();
            menu.ShowDialog(this);
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void dgvproveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SeleccionarUsuarios();

        }

        private void btnAgrega_Click(object sender, EventArgs e)
        {
            bEstadoGuardar = true;
            bEstadoEliminar = false;
            iCodigoUsuario = 0;
            ActivarCampos(true);
            ActivarBotones(false);
            LimpiarControles();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            if (bEstadoGuardar)
            {
                if (validarTextos())
                {
                    MessageBox.Show("Hay campos vacios", "Sistema", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                }
                else {
                    GuardarUsuarios();
                }

            }
            else if (bEstadoEliminar)
            {
                if (validarTextosContraseña())
                {
                    MessageBox.Show("Hay campos vacios", "Sistema", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                }
                else
                {
                     EliminarUsuario();
                }


            }
            else
            {
                if (validarTextosContraseña())
                {
                    MessageBox.Show("Hay campos vacios", "Sistema", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                }
                else
                {
                    EditarUsuarios();
                }
            }
               
            


            
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            bEstadoGuardar = true;
            bEstadoEliminar = false;
            iCodigoUsuario = 0;
            ActivarCampos(false);
            ActivarBotones(true);
            LimpiarControles();
        }

        private void btnEdita_Click(object sender, EventArgs e)
        {
            if (iCodigoUsuario == 0)
            {
                MessageBox.Show("Selecciona un registro", "Sistema", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                bEstadoGuardar = false;
                ActivarCamposEditar(true);
                ActivarBotones(false);

            }
        }

        private void btnElimina_Click(object sender, EventArgs e)
        {
            if (iCodigoUsuario == 0)
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

        private void textBuscar_TextChanged(object sender, EventArgs e)
        {
            Busqueda(txtBuscar.Text);
        }
    }
}
