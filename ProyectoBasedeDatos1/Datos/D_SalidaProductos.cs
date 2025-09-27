using MySql.Data.MySqlClient;
using ProyectoBasedeDatos1.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoBasedeDatos1.Datos
{
   public class D_SalidaProductos
    {

        public DataTable MostrarSalidas() {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();
            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                string consulta = @"select sp.IdSalida ,p.NombreProducto as Producto,sp.Cantidad,sp.FechaSalida, sp.NitCliente, sp.Total
                                    from SalidaProductos sp inner join Producto p on sp.IdProducto=p.IdProducto";
                SqlCommand comando = new SqlCommand(consulta,SqlCon);

                SqlCon.Open();
                resultado=comando.ExecuteReader();
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
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            }
        }

        public string GuardarSalida(E_SalidaProductos Salida)
        {
            string respuesta = "";
            SqlConnection SqlCon = new SqlConnection();
            bool StockS = false;
            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();

                // 1. Verificar stock y obtener precio del producto
                string consultaverficacion = @"SELECT Stock, Precio FROM Producto WHERE IdProducto=@IdProducto";
                SqlCommand comando = new SqlCommand(consultaverficacion, SqlCon);
                comando.Parameters.AddWithValue("@IdProducto", Salida.IdProducto);
                SqlCon.Open();


                SqlDataReader reader = comando.ExecuteReader();
                decimal precioProducto = 0;
                int stockActual = 0;

                if (reader.Read())
                {
                    stockActual = reader.GetInt32(0);
                    precioProducto = reader.GetDecimal(1);
                }
                reader.Close();

                if (stockActual < Salida.Cantidad)
                {
                    respuesta = "insuficiente";
                }
                else
                {
                    // Calcular el total
                    decimal total = Salida.Cantidad * precioProducto;

                    string Insercion = @"Insert into SalidaProductos(IdProducto, Cantidad, Total, NitCliente)
                            values(@IdProducto, @Cantidad,@Total, @NitCliente)";

                    SqlCommand comandoI = new SqlCommand(Insercion, SqlCon);
                    comandoI.Parameters.AddWithValue("@IdProducto", Salida.IdProducto);
                    comandoI.Parameters.AddWithValue("@Cantidad", Salida.Cantidad);
                    comandoI.Parameters.AddWithValue("@NitCliente", Salida.NitCliente);
                    comandoI.Parameters.AddWithValue("@Total", total);

                    int filasAfectadas = comandoI.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        string actualizar = @"Update Producto set Stock=Stock-@Cantidad
                                where IdProducto=@IdProducto";

                        SqlCommand comandoA = new SqlCommand(actualizar, SqlCon);
                        comandoA.Parameters.AddWithValue("@Cantidad", Salida.Cantidad);
                        comandoA.Parameters.AddWithValue("@IdProducto", Salida.IdProducto);
                        int filasActualizadas = comandoA.ExecuteNonQuery();

                        if (filasActualizadas > 0)
                        {
                            respuesta = "Salida guardada";
                        }
                        else
                        {
                            respuesta = "Salida no guardada";
                        }
                    }
                    else
                    {
                        respuesta = "Error";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error al guardar la salida: " + ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
            return respuesta;
        }

        public string EditarSalida(E_SalidaProductos Salida)
        {
            string respuesta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCon.Open();

                // 1. Obtener los datos originales de la salida y el precio del producto
                string consultaOriginal = @"SELECT sp.IdProducto, sp.Cantidad, p.Precio 
                                FROM SalidaProductos sp
                                INNER JOIN Producto p ON sp.IdProducto = p.IdProducto
                                WHERE sp.IdSalida = @IdSalida";

                SqlCommand comandoOriginal = new SqlCommand(consultaOriginal, SqlCon);
                comandoOriginal.Parameters.AddWithValue("@IdSalida", Salida.IdSalida);

                SqlDataReader reader = comandoOriginal.ExecuteReader();
                int idProductoOriginal = 0;
                int cantidadOriginal = 0;
                decimal precioOriginal = 0;

                if (reader.Read())
                {
                    idProductoOriginal = reader.GetInt32(0);
                    cantidadOriginal = reader.GetInt32(1);
                    precioOriginal = reader.GetDecimal(2);
                }
                reader.Close();

                // 2. Obtener el precio del nuevo producto si es diferente
                decimal precioNuevoProducto = precioOriginal;
                if (Salida.IdProducto != idProductoOriginal)
                {
                    string consultaPrecio = "SELECT Precio FROM Producto WHERE IdProducto = @IdProducto";
                    SqlCommand comandoPrecio = new SqlCommand(consultaPrecio, SqlCon);
                    comandoPrecio.Parameters.AddWithValue("@IdProducto", Salida.IdProducto);
                    precioNuevoProducto = Convert.ToDecimal(comandoPrecio.ExecuteScalar());
                }

                // 3. Calcular el nuevo total
                Salida.Total = Salida.Cantidad * precioNuevoProducto;

                // Resto del código de edición permanece igual, pero asegúrate de incluir el Total en la actualización
                string consultaActualizacion = @"
        UPDATE SalidaProductos 
        SET IdProducto = @IdProducto, 
            Cantidad = @Cantidad,
            FechaSalida = @FechaSalida,
            Total = @Total,
            NitCliente=@NitCliente
        WHERE IdSalida = @IdSalida";

                SqlCommand comandoActualizacion = new SqlCommand(consultaActualizacion, SqlCon);
                comandoActualizacion.Parameters.AddWithValue("@IdSalida", Salida.IdSalida);
                comandoActualizacion.Parameters.AddWithValue("@IdProducto", Salida.IdProducto);
                comandoActualizacion.Parameters.AddWithValue("@Cantidad", Salida.Cantidad);
                comandoActualizacion.Parameters.AddWithValue("@FechaSalida", Salida.FechaSalida);
                comandoActualizacion.Parameters.AddWithValue("@NitCliente", Salida.NitCliente);
                comandoActualizacion.Parameters.AddWithValue("@Total", Salida.Total);

                // Resto del método permanece igual...
            }
            catch (Exception ex)
            {
                respuesta = "Error al editar la salida: " + ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }

            return respuesta;
        }


        public DataTable BuscarSalidaPorId(int IdSalida)
        {
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                string consulta = @"
            SELECT sp.IdSalida, sp.IdProducto, p.NombreProducto, sp.Cantidad, sp.FechaSalida
            FROM SalidaProductos sp
            INNER JOIN Producto p ON sp.IdProducto = p.IdProducto
            WHERE sp.IdSalida = @IdSalida";

                SqlCommand comando = new SqlCommand(consulta, SqlCon);
                comando.Parameters.AddWithValue("@IdSalida", IdSalida);

                SqlCon.Open();
                SqlDataReader resultado = comando.ExecuteReader();
                tabla.Load(resultado);
            }
            catch (Exception ex)
            {
                tabla = null;
                Console.WriteLine("Error al buscar salida: " + ex.Message);
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }

            return tabla;
        }
        public DataTable BuscarSalidasPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();

                // Consulta SQL para obtener salidas en el rango de fechas
                string consulta = @"
        SELECT 
            sp.IdSalida,
            p.NombreProducto as Producto,
            sp.Cantidad,
            sp.FechaSalida
        FROM 
            SalidaProductos sp
        INNER JOIN 
            Producto p ON sp.IdProducto = p.IdProducto
        WHERE 
            sp.FechaSalida BETWEEN @FechaInicio AND @FechaFin
        ORDER BY 
            sp.FechaSalida DESC";

                SqlCommand comando = new SqlCommand(consulta, SqlCon);
                comando.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                comando.Parameters.AddWithValue("@FechaFin", fechaFin.Date.AddDays(1).AddSeconds(-1)); 

                SqlCon.Open();
                resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar salidas por fecha: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
        }
        public DataTable ObtenerClientes()
        {
            DataTable tabla = new DataTable();
            MySqlConnection SqlCon = ConexionMySQL.ObtenerInstancia().CrearConexion();

            try
            {
                // Consulta corregida - versión definitiva
                string consulta = @"SELECT 
                      Id, 
                      JSON_UNQUOTE(JSON_EXTRACT(datos_cliente, '$.nit')) as Nit,
                      JSON_UNQUOTE(JSON_EXTRACT(datos_cliente, '$.nombre')) as Nombre
                   FROM Clientes";


                MySqlCommand comando = new MySqlCommand(consulta, SqlCon);
                SqlCon.Open();
                MySqlDataReader resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener clientes: " + ex.Message);
                return null;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
        }



        public Dictionary<string, string> ObtenerDatosCliente(int idCliente)
        {
            var datos = new Dictionary<string, string>();
            MySqlConnection SqlCon = ConexionMySQL.ObtenerInstancia().CrearConexion();

            try
            {
                string consulta = "SELECT datos_cliente FROM Clientes WHERE Id = @Id";
                MySqlCommand comando = new MySqlCommand(consulta, SqlCon);
                comando.Parameters.AddWithValue("@Id", idCliente);

                SqlCon.Open();
                string json = comando.ExecuteScalar()?.ToString();

                if (!string.IsNullOrEmpty(json))
                {
                    dynamic cliente = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                    datos["nit"] = cliente.nit;
                    datos["nombre"] = cliente.nombre;
                    datos["direccion"] = cliente.direccion;
                    datos["telefono"] = cliente.telefono;
                    datos["correo"] = cliente.correo;
                }

                return datos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener datos del cliente: " + ex.Message);
                return null;
            }
            finally
            {
                if (SqlCon.State == System.Data.ConnectionState.Open) SqlCon.Close();
            }
        }


        public Dictionary<string, string> BuscarClientePorNit(string nit)
        {
            var datos = new Dictionary<string, string>();
            MySqlConnection SqlCon = ConexionMySQL.ObtenerInstancia().CrearConexion();

            try
            {
                string consulta = @"SELECT 
                          Id, 
                          JSON_UNQUOTE(JSON_EXTRACT(datos_cliente, '$.nit')) as Nit,
                          JSON_UNQUOTE(JSON_EXTRACT(datos_cliente, '$.nombre')) as Nombre,
                          JSON_UNQUOTE(JSON_EXTRACT(datos_cliente, '$.direccion')) as Direccion,
                          JSON_UNQUOTE(JSON_EXTRACT(datos_cliente, '$.telefono')) as Telefono,
                          JSON_UNQUOTE(JSON_EXTRACT(datos_cliente, '$.correo')) as Correo
                       FROM Clientes
                       WHERE JSON_EXTRACT(datos_cliente, '$.nit') = @Nit";

                MySqlCommand comando = new MySqlCommand(consulta, SqlCon);
                comando.Parameters.AddWithValue("@Nit", nit);

                SqlCon.Open();
                MySqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    datos["id"] = reader["Id"].ToString();
                    datos["nit"] = reader["Nit"].ToString();
                    datos["nombre"] = reader["Nombre"].ToString();
                    datos["direccion"] = reader["Direccion"].ToString();
                    datos["telefono"] = reader["Telefono"].ToString();
                    datos["correo"] = reader["Correo"].ToString();
                }

                return datos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar cliente por NIT: " + ex.Message);
                return null;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }
        }





    }
}
