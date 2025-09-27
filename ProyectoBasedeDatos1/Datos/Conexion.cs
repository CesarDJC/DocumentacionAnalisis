using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoBasedeDatos1.Datos
{
    public class Conexion
    {
        private String Base;
        private String Servidor;
        private String Usuario;
        private String Clave;

        private static Conexion Con = null;

        private Conexion() {
            this.Servidor = "cesar2005";
            this.Base = "Proyecto1BD";
            this.Usuario = "root";
            this.Clave = "root";
        }

        public SqlConnection CrearConexion() { 
            SqlConnection Cadena = new SqlConnection();
            try
            {
                Cadena.ConnectionString = "Server=" + this.Servidor + "; Database=" + this.Base
                                           + "; User Id=" + this.Usuario + "; Password=" + this.Clave
                                            + ";TrustServerCertificate=True";
            }
            catch (Exception ex)
            {
                Cadena = null;
                throw ex;
            }
            return Cadena;
        }



        public static Conexion crearInstancia() {
            if (Con==null) {
                Con = new Conexion();
            }
            return Con;
        }

    }
}
