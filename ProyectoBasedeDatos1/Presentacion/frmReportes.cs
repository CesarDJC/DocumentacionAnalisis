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
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;

namespace ProyectoBasedeDatos1.Presentacion
{
    public partial class frmReportes : Form
    {
        public frmReportes()
        {
            InitializeComponent();
        }
        #region "Variables"
        private bool EInicio=true;
        private bool EStock = false;
        private bool EFechas = false;
        private bool Edistribución = false;
    
        private bool Egraficos = false;

        #endregion
        #region
        private void CargarStock() {

            D_Productos datos = new D_Productos();

            
            
                if (int.TryParse(txtStock.Text, out int Stock))
                {
                    dgvStock.DataSource = datos.StockBajo(Stock);
                txtStock.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("El stock no tiene un formato válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            
           
            
            
        }

        private void LimpiarTabla() {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            dgvStock.DataSource = null;
        }

        private void CargarProveedores() {
            D_Proveedores datos = new D_Proveedores();
            cbxProveedores.DataSource = datos.NombresProveedores();
            cbxProveedores.ValueMember = "IdProveedor";
            cbxProveedores.DisplayMember = "Nombre Proveedor";
        }

        private void BusquedaProv() {
            E_Proveedor Proveedor=new E_Proveedor();
            if (cbxProveedores.SelectedValue != null) {
                Proveedor.IdProveedor = (int)cbxProveedores.SelectedValue;

            }
            D_Proveedores datos=new D_Proveedores();
            dgvStock.DataSource = datos.BusquedaProductos(Proveedor);
        }

        private void BusquedaFechas()
        {
            D_Productos datos = new D_Productos();
            DateTime fecha1 = dtmFecha.Value; // Obtiene la fecha del DateTimePicker
            DateTime fecha2 = dtmFecha2.Value; // Obtiene la fecha del DateTimePicker

            dgvStock.DataSource = datos.BusquedaFechas(fecha1, fecha2);
        }

        private void configurarChart() {
            chart1.Series.Clear();
            chart1.Titles.Clear();

            chart1.Titles.Add("Cantidad de Productos suministrados por proveedor");
            Series serie = new Series();
            serie.Name="Proveedores";
            serie.ChartType = SeriesChartType.Column;
            serie.IsValueShownAsLabel = true;

            chart1.Series.Add(serie);
        }

        private void LlenarDatosChart(DataTable datos)
        {
            // Limpia el Chart antes de agregar nuevos datos
            chart1.Series["Proveedores"].Points.Clear();

            // Recorre el DataTable y agrega los datos al Chart
            foreach (DataRow fila in datos.Rows)
            {
                string nombreProveedor = fila["NombreProveedor"].ToString();
                int totalSuministrado = Convert.ToInt32(fila["TotalSuministrado"]);

                // Agrega el punto al Chart
                chart1.Series["Proveedores"].Points.AddXY(nombreProveedor, totalSuministrado);
            }
        }

        private void CargarDatos()
        {
            // Crea una instancia de la clase de datos
            D_ProductoProveedor datos = new D_ProductoProveedor();

            // Obtiene los datos de la cantidad suministrada por proveedor
            DataTable tabla = datos.ObtenerCantidad();

            // Verifica si se obtuvieron datos
            if (tabla != null && tabla.Rows.Count > 0)
            {
                // Asigna el DataTable al DataGridView
                dgvStock.DataSource = tabla;

                // Configura y llena el Chart con los datos
                configurarChart();
                LlenarDatosChart(tabla); // Pasa el DataTable, no el DataGridView
            }
            else
            {
                MessageBox.Show("No se encontraron datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void HabilitarBotones(bool Estado) {
            btnMostrar.Enabled=!Estado;
            btnCancelar.Enabled=!Estado;
            btnStock.Enabled=Estado;
            btnFechas.Enabled = Estado;
            btnDistribucion.Enabled = Estado;
            btnGráficos.Enabled = Estado;
            btnMenu.Enabled = Estado;
        }

        private void RestaurarEstadoInicial()
        {
            // Reinicia todas las variables de estado
            EInicio = true;
            EStock = false;
            EFechas = false;
            Edistribución = false;
            Egraficos = false;

            // Habilita los botones
            HabilitarBotones(true);

            // Deshabilita los controles de entrada
            HabilitarControles();

            // Limpia el DataGridView
            LimpiarTabla();
        }
        private void HabilitarControles()
        {
            // Reinicia todos los controles
            txtStock.Enabled = false;
            dtmFecha.Enabled = false;
            dtmFecha2.Enabled = false;
            cbxProveedores.Enabled = false;

            // Habilita los controles según el estado actual
            if (EStock)
            {
                txtStock.Enabled = true;
            }
            else if (EFechas)
            {
                dtmFecha.Enabled = true;
                dtmFecha2.Enabled = true;
            }
            else if (Edistribución)
            {
                cbxProveedores.Enabled = true;
            }
        }


        #endregion
        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            LimpiarTabla();
            EStock = false;
            EInicio = false;
            EFechas = false;
            Edistribución = false;
            Egraficos = true;
            HabilitarBotones(false);
            HabilitarControles();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LimpiarTabla();
            EStock = true;
            EInicio = false;
            EFechas = false;
            Edistribución = false;
            Egraficos = false;
            HabilitarBotones(false);
            HabilitarControles();   
            
        }

        private void frmReportes_Load(object sender, EventArgs e)
        {
            
            CargarProveedores();
            HabilitarControles();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LimpiarTabla();
            EStock =false;
            EInicio = false;
            EFechas = false;
            Edistribución = true;
            Egraficos = false;
            HabilitarBotones(false);
            HabilitarControles();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            LimpiarTabla();
            EStock = false;
            EInicio = false;
            EFechas = true;
            Edistribución = false;
            Egraficos = false;
            HabilitarBotones(false);
            HabilitarControles();

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (EStock)
            {
                if (string.IsNullOrEmpty(txtStock.Text))
                {
                    MessageBox.Show("Ingrese stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    LimpiarTabla();
                    CargarStock();
                    EInicio = true;
                    EStock = false;
                    HabilitarBotones(true);
                    HabilitarControles();
                }
            }
            else if (EFechas)
            {
                LimpiarTabla();
                BusquedaFechas();
                EInicio = true;
                EFechas = false;
                HabilitarBotones(true);
                HabilitarControles();
            }
            else if (Edistribución)
            {
                LimpiarTabla();
                BusquedaProv();
                EInicio = true;
                Edistribución = false;
                HabilitarBotones(true);
                HabilitarControles();
            }
            else if (Egraficos)
            {
                LimpiarTabla();
                CargarDatos();
                EInicio = true;
                Egraficos = false;
                HabilitarBotones(true);
                HabilitarControles();
            }



        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            frmMenu menu = new frmMenu();
            menu.ShowDialog();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            RestaurarEstadoInicial();
        }
    }
}
