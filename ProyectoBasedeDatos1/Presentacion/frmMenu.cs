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
    public partial class frmMenu : Form
    {
        private bool EsAdministrador { get;  set; }
        public frmMenu()
        {
            InitializeComponent();
            ConfigurarAccesos();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void ConfigurarAccesos()
        {
            // Usar la clase estática
            button6.Enabled = SesionUsuario.EsAdministrador;

            if (SesionUsuario.EsAdministrador)
            {
                this.Text += " - Administrador";
            }
            else
            {
                this.Text += " - Usuario";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmProductos frmproductos=new frmProductos();
            frmproductos.ShowDialog();
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSalidaProductos frameSalida=new frmSalidaProductos();
            frameSalida.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmProveedores frameproveedores=new frmProveedores();
            frameproveedores.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDistribucion distribucion=new frmDistribucion(); 
            distribucion.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmReportes reportes = new frmReportes();
            reportes.ShowDialog();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmUsuarios usuarios= new frmUsuarios();    
            usuarios.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmClientes clientes = new frmClientes();
            clientes.ShowDialog();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
           



            SesionUsuario.NombreUsuario = null;
            SesionUsuario.EsAdministrador = false;
            SesionUsuario.IdUsuario = 0;

            this.Hide();
            frmLogin login = new frmLogin();
            login.ShowDialog();
            this.Close();
        }
    }
}
