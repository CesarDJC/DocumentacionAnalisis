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
    public partial class frmRegistrarse : Form
    {


        public frmRegistrarse()
        {
            InitializeComponent();
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
               
                LimpiarControles();
               
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

        private void LimpiarControles()
        {
            
            txtNombre.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtContraseña.Text = "";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        private bool validarTextos()
        {
            bool TextoVacio = false;
            if (string.IsNullOrEmpty(txtNombre.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtCorreo.Text)) TextoVacio = true;
            if (string.IsNullOrEmpty(txtContraseña.Text)) TextoVacio = true;
            return TextoVacio;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LimpiarControles();
            this.Hide();
            frmLogin login = new frmLogin();
            login.ShowDialog();
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            if (validarTextos())
            {
                MessageBox.Show("Hay campos vacios", "Sistema", MessageBoxButtons.OK,
            MessageBoxIcon.Warning);
            }
            else
            {
                GuardarUsuarios();
            }
        }

        private void frmRegistrarse_Load(object sender, EventArgs e)
        {

        }
    }
}
