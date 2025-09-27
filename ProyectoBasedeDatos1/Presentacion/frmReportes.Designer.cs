namespace ProyectoBasedeDatos1.Presentacion
{
    partial class frmReportes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnMostrar = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dtmFecha2 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.dtmFecha = new System.Windows.Forms.DateTimePicker();
            this.cbxProveedores = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvStock = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnGráficos = new System.Windows.Forms.Button();
            this.btnDistribucion = new System.Windows.Forms.Button();
            this.btnFechas = new System.Windows.Forms.Button();
            this.btnStock = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnCancelar);
            this.panel2.Controls.Add(this.txtStock);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.btnMostrar);
            this.panel2.Controls.Add(this.chart1);
            this.panel2.Controls.Add(this.dtmFecha2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dtmFecha);
            this.panel2.Controls.Add(this.cbxProveedores);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.dgvStock);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(255, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(927, 575);
            this.panel2.TabIndex = 5;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Enabled = false;
            this.btnCancelar.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Image = global::ProyectoBasedeDatos1.Properties.Resources.cancelar;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(684, 90);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(188, 70);
            this.btnCancelar.TabIndex = 21;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // txtStock
            // 
            this.txtStock.Location = new System.Drawing.Point(143, 99);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(124, 22);
            this.txtStock.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 24);
            this.label5.TabIndex = 19;
            this.label5.Text = "Stock";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // btnMostrar
            // 
            this.btnMostrar.Enabled = false;
            this.btnMostrar.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMostrar.Image = global::ProyectoBasedeDatos1.Properties.Resources.ojo;
            this.btnMostrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMostrar.Location = new System.Drawing.Point(465, 90);
            this.btnMostrar.Name = "btnMostrar";
            this.btnMostrar.Size = new System.Drawing.Size(188, 70);
            this.btnMostrar.TabIndex = 18;
            this.btnMostrar.Text = "Mostrar";
            this.btnMostrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMostrar.UseVisualStyleBackColor = true;
            this.btnMostrar.Click += new System.EventHandler(this.button5_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(508, 190);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(387, 324);
            this.chart1.TabIndex = 17;
            this.chart1.Text = "Chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // dtmFecha2
            // 
            this.dtmFecha2.CustomFormat = "yyyy-MM-dd";
            this.dtmFecha2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtmFecha2.Location = new System.Drawing.Point(145, 211);
            this.dtmFecha2.Name = "dtmFecha2";
            this.dtmFecha2.ShowUpDown = true;
            this.dtmFecha2.Size = new System.Drawing.Size(245, 22);
            this.dtmFecha2.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 24);
            this.label4.TabIndex = 15;
            this.label4.Text = "Fecha";
            // 
            // dtmFecha
            // 
            this.dtmFecha.CustomFormat = "yyyy-MM-dd ";
            this.dtmFecha.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtmFecha.Location = new System.Drawing.Point(145, 159);
            this.dtmFecha.Name = "dtmFecha";
            this.dtmFecha.ShowUpDown = true;
            this.dtmFecha.Size = new System.Drawing.Size(245, 22);
            this.dtmFecha.TabIndex = 14;
            // 
            // cbxProveedores
            // 
            this.cbxProveedores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProveedores.FormattingEnabled = true;
            this.cbxProveedores.Location = new System.Drawing.Point(143, 269);
            this.cbxProveedores.Name = "cbxProveedores";
            this.cbxProveedores.Size = new System.Drawing.Size(165, 24);
            this.cbxProveedores.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Palatino Linotype", 28.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(311, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(213, 63);
            this.label7.TabIndex = 11;
            this.label7.Text = "Reportes";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // dgvStock
            // 
            this.dgvStock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvStock.BackgroundColor = System.Drawing.Color.White;
            this.dgvStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStock.Location = new System.Drawing.Point(28, 362);
            this.dgvStock.Name = "dgvStock";
            this.dgvStock.RowHeadersWidth = 51;
            this.dgvStock.RowTemplate.Height = 24;
            this.dgvStock.Size = new System.Drawing.Size(446, 173);
            this.dgvStock.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 269);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 24);
            this.label3.TabIndex = 3;
            this.label3.Text = "Proveedor";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fecha";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnMenu);
            this.panel1.Controls.Add(this.btnGráficos);
            this.panel1.Controls.Add(this.btnDistribucion);
            this.panel1.Controls.Add(this.btnFechas);
            this.panel1.Controls.Add(this.btnStock);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(255, 575);
            this.panel1.TabIndex = 6;
            // 
            // btnMenu
            // 
            this.btnMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMenu.ForeColor = System.Drawing.Color.Black;
            this.btnMenu.Image = global::ProyectoBasedeDatos1.Properties.Resources.menu;
            this.btnMenu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMenu.Location = new System.Drawing.Point(0, 480);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(255, 83);
            this.btnMenu.TabIndex = 6;
            this.btnMenu.Text = "Menu";
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnGráficos
            // 
            this.btnGráficos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGráficos.FlatAppearance.BorderSize = 0;
            this.btnGráficos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGráficos.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGráficos.ForeColor = System.Drawing.Color.Black;
            this.btnGráficos.Image = global::ProyectoBasedeDatos1.Properties.Resources.diagrama;
            this.btnGráficos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGráficos.Location = new System.Drawing.Point(0, 385);
            this.btnGráficos.Name = "btnGráficos";
            this.btnGráficos.Size = new System.Drawing.Size(255, 95);
            this.btnGráficos.TabIndex = 5;
            this.btnGráficos.Text = "Graficos";
            this.btnGráficos.UseVisualStyleBackColor = true;
            this.btnGráficos.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnDistribucion
            // 
            this.btnDistribucion.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDistribucion.FlatAppearance.BorderSize = 0;
            this.btnDistribucion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDistribucion.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDistribucion.ForeColor = System.Drawing.Color.Black;
            this.btnDistribucion.Image = global::ProyectoBasedeDatos1.Properties.Resources.proveedor__1_;
            this.btnDistribucion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDistribucion.Location = new System.Drawing.Point(0, 299);
            this.btnDistribucion.Name = "btnDistribucion";
            this.btnDistribucion.Size = new System.Drawing.Size(255, 86);
            this.btnDistribucion.TabIndex = 4;
            this.btnDistribucion.Text = "        Proveedor \r\n        y Productos";
            this.btnDistribucion.UseVisualStyleBackColor = true;
            this.btnDistribucion.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnFechas
            // 
            this.btnFechas.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnFechas.FlatAppearance.BorderSize = 0;
            this.btnFechas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechas.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFechas.ForeColor = System.Drawing.Color.Black;
            this.btnFechas.Image = global::ProyectoBasedeDatos1.Properties.Resources.fecha_limite;
            this.btnFechas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFechas.Location = new System.Drawing.Point(0, 207);
            this.btnFechas.Name = "btnFechas";
            this.btnFechas.Size = new System.Drawing.Size(255, 92);
            this.btnFechas.TabIndex = 3;
            this.btnFechas.Text = "      Busqueda";
            this.btnFechas.UseVisualStyleBackColor = true;
            this.btnFechas.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnStock
            // 
            this.btnStock.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStock.FlatAppearance.BorderSize = 0;
            this.btnStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStock.Font = new System.Drawing.Font("Constantia", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStock.ForeColor = System.Drawing.Color.Black;
            this.btnStock.Image = global::ProyectoBasedeDatos1.Properties.Resources.almacenes;
            this.btnStock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStock.Location = new System.Drawing.Point(0, 115);
            this.btnStock.Name = "btnStock";
            this.btnStock.Size = new System.Drawing.Size(255, 92);
            this.btnStock.TabIndex = 2;
            this.btnStock.Text = "Stock";
            this.btnStock.UseVisualStyleBackColor = true;
            this.btnStock.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(-3, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "_______________________________________";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::ProyectoBasedeDatos1.Properties.Resources.Logo1;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(255, 115);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // frmReportes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 575);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmReportes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmReportes";
            this.Load += new System.EventHandler(this.frmReportes_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStock)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbxProveedores;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvStock;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGráficos;
        private System.Windows.Forms.Button btnDistribucion;
        private System.Windows.Forms.Button btnFechas;
        private System.Windows.Forms.Button btnStock;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DateTimePicker dtmFecha;
        private System.Windows.Forms.DateTimePicker dtmFecha2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btnMostrar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnCancelar;
    }
}