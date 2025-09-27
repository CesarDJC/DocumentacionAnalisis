using MySql.Data.MySqlClient;
using ProyectoBasedeDatos1.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ProyectoBasedeDatos1.Datos
{
    public class D_Clientes
    {
        public DataTable ListarClientes()
        {
            MySqlDataReader resultado;
            DataTable tabla = new DataTable();
            MySqlConnection MySqlCon = new MySqlConnection();

            try
            {
                MySqlCon = ConexionMySQL.ObtenerInstancia().CrearConexion();
                MySqlCommand comando = new MySqlCommand("SELECT Id, datos_cliente FROM Clientes", MySqlCon);

                MySqlCon.Open();
                resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            finally
            {
                if (MySqlCon.State == System.Data.ConnectionState.Open) MySqlCon.Close();
            }
        }

        public DataTable Busqueda(string bus)
        {
            MySqlDataReader resultado;
            DataTable tabla = new DataTable();
            MySqlConnection MySqlCon = new MySqlConnection();

            try
            {
                MySqlCon = ConexionMySQL.ObtenerInstancia().CrearConexion();
                MySqlCommand comando = new MySqlCommand(
                    "SELECT Id, datos_cliente FROM Clientes " +
                    "WHERE JSON_EXTRACT(datos_cliente, '$.nombre') LIKE @busqueda", MySqlCon);

                comando.Parameters.AddWithValue("@busqueda", $"%{bus}%");

                MySqlCon.Open();
                resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            finally
            {
                if (MySqlCon.State == System.Data.ConnectionState.Open) MySqlCon.Close();
            }
        }

        public string Guardar_Cliente(E_Clientes Cliente)
        {
            string respuesta = "";
            MySqlConnection MySqlCon = new MySqlConnection();

            try
            {
                MySqlCon = ConexionMySQL.ObtenerInstancia().CrearConexion();

               
                string consultaVerificacion =
                    "SELECT COUNT(*) FROM Clientes " +
                    "WHERE JSON_EXTRACT(datos_cliente, '$.nit') = @nit";

                MySqlCommand comandoVerificacion = new MySqlCommand(consultaVerificacion, MySqlCon);
                comandoVerificacion.Parameters.AddWithValue("@nit", ObtenerValorJson(Cliente.datos_cliente, "nit"));

                MySqlCon.Open();
                int cantidad = Convert.ToInt32(comandoVerificacion.ExecuteScalar());

                if (cantidad > 0)
                {
                    return "existente";
                }

               
                string consultaInsercion = "INSERT INTO Clientes (datos_cliente) VALUES (@datos_cliente)";
                MySqlCommand comandoInsercion = new MySqlCommand(consultaInsercion, MySqlCon);
                comandoInsercion.Parameters.AddWithValue("@datos_cliente", Cliente.datos_cliente);

                int filasAfectadas = comandoInsercion.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    respuesta = "guardado";
                }
                else
                {
                    respuesta = "no guardado";
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error al guardar el cliente: " + ex.Message;
            }
            finally
            {
                if (MySqlCon.State == System.Data.ConnectionState.Open)
                {
                    MySqlCon.Close();
                }
            }

            return respuesta;
        }

        public string Editar_Cliente(int id, E_Clientes Cliente)
        {
            string respuesta = "";
            MySqlConnection MySqlCon = new MySqlConnection();

            try
            {
                MySqlCon = ConexionMySQL.ObtenerInstancia().CrearConexion();

         
                string consultaVerificacion =
                    "SELECT COUNT(*) FROM Clientes " +
                    "WHERE JSON_EXTRACT(datos_cliente, '$.nit') = @nit AND Id <> @id";

                MySqlCommand comandoVerificacion = new MySqlCommand(consultaVerificacion, MySqlCon);
                comandoVerificacion.Parameters.AddWithValue("@nit", ObtenerValorJson(Cliente.datos_cliente, "nit"));
                comandoVerificacion.Parameters.AddWithValue("@id", id);

                MySqlCon.Open();
                int cantidad = Convert.ToInt32(comandoVerificacion.ExecuteScalar());

                if (cantidad > 0)
                {
                    return "existente";
                }

              
                string consultaActualizacion = "UPDATE Clientes SET datos_cliente = @datos_cliente WHERE Id = @id";
                MySqlCommand comandoActualizacion = new MySqlCommand(consultaActualizacion, MySqlCon);
                comandoActualizacion.Parameters.AddWithValue("@datos_cliente", Cliente.datos_cliente);
                comandoActualizacion.Parameters.AddWithValue("@id", id);

                int filasAfectadas = comandoActualizacion.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    respuesta = "actualizado";
                }
                else
                {
                    respuesta = "no actualizado";
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error al actualizar el cliente: " + ex.Message;
            }
            finally
            {
                if (MySqlCon.State == System.Data.ConnectionState.Open)
                {
                    MySqlCon.Close();
                }
            }

            return respuesta;
        }

        public string Eliminar_Cliente(int id)
        {
            string respuesta = "";
            MySqlConnection MySqlCon = new MySqlConnection();

            try
            {
                MySqlCon = ConexionMySQL.ObtenerInstancia().CrearConexion();
                MySqlCommand comando = new MySqlCommand("DELETE FROM Clientes WHERE Id = @id", MySqlCon);
                comando.Parameters.AddWithValue("@id", id);

                MySqlCon.Open();
                int filasAfectadas = comando.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    respuesta = "eliminado";
                }
                else
                {
                    respuesta = "no eliminado";
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error al eliminar el cliente: " + ex.Message;
            }
            finally
            {
                if (MySqlCon.State == System.Data.ConnectionState.Open)
                {
                    MySqlCon.Close();
                }
            }

            return respuesta;
        }

        // Método auxiliar para extraer valores del JSON
        private string ObtenerValorJson(string json, string propiedad)
        {
            try
            {
                var jsonObj = Newtonsoft.Json.Linq.JObject.Parse(json);
                return jsonObj[propiedad]?.ToString() ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
