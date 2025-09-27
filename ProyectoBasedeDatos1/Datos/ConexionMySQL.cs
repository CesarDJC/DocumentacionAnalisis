using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoBasedeDatos1.Datos
{
    public class ConexionMySQL
    {
        private string Servidor;
        private string BaseDatos;
        private string Usuario;
        private string Password;
        private string Puerto; // Nuevo campo para el puerto

        private static ConexionMySQL Instancia = null;

        private ConexionMySQL()
        {
            // Valores por defecto (ajústalos a tu configuración)
            this.Servidor = "localhost";
            this.BaseDatos = "clientes";
            this.Usuario = "root";
            this.Password = "root1234";
            this.Puerto = "3306"; // Puerto por defecto de MySQL
        }

        public static ConexionMySQL ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new ConexionMySQL();
            }
            return Instancia;
        }

        public void Configurar(string servidor, string basedatos, string usuario, string password, string puerto = "3306")
        {
            this.Servidor = servidor; // Ahora sí usa los parámetros
            this.BaseDatos = basedatos;
            this.Usuario = usuario;
            this.Password = password;
            this.Puerto = puerto;
        }

        public MySqlConnection CrearConexion()
        {
            string cadenaConexion = $"Server={Servidor};Port={Puerto};Database={BaseDatos};Uid={Usuario};Pwd={Password};";
            return new MySqlConnection(cadenaConexion);
        }

        public bool ProbarConexion()
        {
            using (MySqlConnection conexion = CrearConexion())
            {
                try
                {
                    conexion.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    // Opcional: Puedes mostrar el error en consola para debug
                    Console.WriteLine($"Error de conexión: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
