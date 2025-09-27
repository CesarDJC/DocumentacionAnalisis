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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            txtContraseña.PasswordChar = '*';
        }

        string resultado = "";
        private (string resultado, bool esAdmin) IniciarSesion()
        {
            E_Usuario Usuario = new E_Usuario();
            Usuario.NombreUsuario = txtNombre.Text;
            Usuario.Contraseña = txtContraseña.Text;

            D_Usuarios datos = new D_Usuarios();
            return datos.Login_Usuario(Usuario);
        }


        private bool Camposvacios() {
            bool vacio = false;
            if (string.IsNullOrEmpty(txtNombre.Text)) vacio = true;
            if (string.IsNullOrEmpty(txtContraseña.Text)) vacio = true;
            return vacio;

        }
       


        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            if (Camposvacios())
            {
                MessageBox.Show("Hay campos vacios", "Sistema", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            }
            else
            {
                var (resultado, esAdmin) = IniciarSesion();

                if (resultado == "existente")
                {
                 
                    SesionUsuario.NombreUsuario = txtNombre.Text;
                    SesionUsuario.EsAdministrador = esAdmin;

                    this.Hide();
                    txtContraseña.Text = string.Empty;
                    txtNombre.Text = string.Empty;

                    frmMenu menu = new frmMenu();
                    menu.ShowDialog();
                }
                else if (resultado == "no existente")
                {
                    MessageBox.Show("Usuario incorrecto o inexistente, registrese primero", "Sistema",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRegistrarse_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmRegistrarse registro=new frmRegistrarse();
            registro.ShowDialog();
            txtNombre.Text= string.Empty;
            txtContraseña.Text = string.Empty;
           
        }

        private void txtContraseña_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
