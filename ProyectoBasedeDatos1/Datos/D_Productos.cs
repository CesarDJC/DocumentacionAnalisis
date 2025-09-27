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
    public class D_Productos
    {
        public DataTable ListarProductos() {
            SqlDataReader resultado;
            DataTable tabla=new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("select IdProducto, NombreProducto as [Nombre Producto], Descripción,Precio,Stock, FechaIngreso as [Fecha de Ingreso],Imagen from Producto", SqlCon);

                SqlCon.Open();
                resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            finally {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                
            }

            
        }



        public DataTable StockBajo(int stock) {
            SqlDataReader resultado;
            DataTable tabla=new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("select NombreProducto as [Nombre Producto], Stock from Producto where Stock<@Stock", SqlCon);
                comando.Parameters.AddWithValue("@Stock", stock);
                SqlCon.Open();
                resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            finally { 
                if(SqlCon.State == ConnectionState.Open)  SqlCon.Close(); 
            }
        }

        public DataTable Busqueda(string bus) { 
            SqlDataReader resultado;
            DataTable tabla=new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("select *from Producto where NombreProducto like '" + bus + "%'", SqlCon);
                SqlCon.Open();
                resultado = comando.ExecuteReader();
                tabla.Load(resultado);
                return tabla;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
            finally {
                if(SqlCon.State==ConnectionState.Open) SqlCon.Close();
            }
        }


        public DataTable NombresProductos()
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("select IdProducto,NombreProducto as [Nombre Producto] from Producto", SqlCon);

                SqlCon.Open();
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
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

            }


        }

        public string Guardar_Producto(E_Producto Producto)
        {
            String respuesta = "";
            SqlConnection SqlCon = new SqlConnection();
            bool productoExiste = false; // Bandera para controlar si el producto ya existe

            try
            {
                // Obtiene la conexión a la base de datos
                SqlCon = Conexion.crearInstancia().CrearConexion();

                // Verificar si el nombre del producto ya existe
                string consultaVerificacion = @"
            SELECT COUNT(*) FROM Producto WHERE NombreProducto = @NombreProducto;
        ";

                SqlCommand comandoVerificacion = new SqlCommand(consultaVerificacion, SqlCon);
                comandoVerificacion.Parameters.AddWithValue("@NombreProducto", Producto.NombreProducto);

                // Abre la conexión
                SqlCon.Open();

                // Ejecuta la consulta de verificación
                int cantidad = (int)comandoVerificacion.ExecuteScalar();

                if (cantidad > 0)
                {
                    // Si ya existe un producto con el mismo nombre, activa la bandera
                    productoExiste = true;
                    respuesta = "existente";
                }

                // Si el producto no existe, procede con la inserción
                if (!productoExiste)
                {
                    // Define la consulta SQL para insertar un nuevo producto
                    string consultaInsercion = @"
                INSERT INTO Producto (NombreProducto, Descripción, Precio, Stock, Imagen)
                VALUES (@NombreProducto, @Descripción, @Precio, @Stock,@Imagen);
            ";

                    // Crea el comando SQL para la inserción
                    SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon);

                   
                    comandoInsercion.Parameters.AddWithValue("@NombreProducto", Producto.NombreProducto);
                    comandoInsercion.Parameters.AddWithValue("@Descripción", Producto.Descripción);
                    comandoInsercion.Parameters.AddWithValue("@Precio", Producto.Precio);
                    comandoInsercion.Parameters.AddWithValue("@Stock", Producto.Stock);
                    comandoInsercion.Parameters.AddWithValue("@Imagen", Producto.Imagen);

                    // Ejecuta el comando de inserción
                    int filasAfectadas = comandoInsercion.ExecuteNonQuery();

                    // Verifica si se insertó correctamente
                    if (filasAfectadas > 0)
                    {
                        respuesta = "guardado";
                    }
                    else
                    {
                        respuesta = "no guardado";
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que ocurra
                respuesta = "Error al guardar el producto: " + ex.Message;
            }
            finally
            {
                // Cierra la conexión si está abierta
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }

            return respuesta; // Único punto de salida
        }
        public string Editar_Producto(E_Producto Producto)
        {
            String respuesta = "";
            SqlConnection SqlCon = new SqlConnection();
            bool productoExiste = false; // Bandera para controlar si el producto ya existe

            try
            {
                // Obtiene la conexión a la base de datos
                SqlCon = Conexion.crearInstancia().CrearConexion();

                // Verificar si el nombre del producto ya existe
                string consultaVerificacion = @"
            SELECT COUNT(*) FROM Producto WHERE NombreProducto = @NombreProducto AND IdProducto <> @IdProducto;";

                SqlCommand comandoVerificacion = new SqlCommand(consultaVerificacion, SqlCon);
                comandoVerificacion.Parameters.AddWithValue("@NombreProducto", Producto.NombreProducto);
                comandoVerificacion.Parameters.AddWithValue("@IdProducto", Producto.IdProducto);

                // Abre la conexión
                SqlCon.Open();

                // Ejecuta la consulta de verificación
                int cantidad = (int)comandoVerificacion.ExecuteScalar();

                if (cantidad > 0)
                {
                    // Si ya existe un producto con el mismo nombre, activa la bandera
                    productoExiste = true;
                    respuesta = "existente";
                }

                // Si el producto no existe, procede con la inserción
                if (!productoExiste)
                {
                    // Define la consulta SQL para insertar un nuevo producto
                    string consultaInsercion = @"
                        update Producto set NombreProducto=@NombreProducto,
                        Descripción=@Descripción,
                        Precio=@Precio, Stock=@Stock, Imagen=@Imagen
                        where IdProducto=@IdProducto; ";


                    // Crea el comando SQL para la inserción
                    SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon);

                    // Asigna los parámetros con los valores del objeto E_Producto
                    comandoInsercion.Parameters.AddWithValue("@IdProducto", Producto.IdProducto);
                    comandoInsercion.Parameters.AddWithValue("@NombreProducto", Producto.NombreProducto);
                    comandoInsercion.Parameters.AddWithValue("@Descripción", Producto.Descripción);
                    comandoInsercion.Parameters.AddWithValue("@Precio", Producto.Precio);
                    comandoInsercion.Parameters.AddWithValue("@Stock", Producto.Stock);
                    comandoInsercion.Parameters.AddWithValue("@Imagen", Producto.Imagen);

                    // Ejecuta el comando de inserción
                    int filasAfectadas = comandoInsercion.ExecuteNonQuery();

                    // Verifica si se insertó correctamente
                    if (filasAfectadas > 0)
                    {
                        respuesta = "actualizado";
                    }
                    else
                    {
                        respuesta = "no actualizado";
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción que ocurra
                respuesta = "Error al actualizar el producto: " + ex.Message;
            }
            finally
            {
                // Cierra la conexión si está abierta
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }

            return respuesta; // Único punto de salida
        }

        public string Eliminar_Producto(E_Producto Producto)
        {
            String respuesta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                // Obtiene la conexión a la base de datos
                SqlCon = Conexion.crearInstancia().CrearConexion();

                // Verificar si el nombre del producto ya existe
            
                // Abre la conexión
                SqlCon.Open();


                    // Define la consulta SQL para insertar un nuevo producto
              string consultaInsercion = @"
                        delete from Producto
                        where IdProducto=@IdProducto; ";


                    // Crea el comando SQL para la inserción
               SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon);

                    // Asigna los parámetros con los valores del objeto E_Producto
                    comandoInsercion.Parameters.AddWithValue("@IdProducto", Producto.IdProducto);

                    // Ejecuta el comando de inserción
                    int filasAfectadas = comandoInsercion.ExecuteNonQuery();

                    // Verifica si se insertó correctamente
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
                // Maneja cualquier excepción que ocurra
                respuesta = "Error al eliminar el producto: " + ex.Message;
            }
            finally
            {
                // Cierra la conexión si está abierta
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }

            return respuesta; // Único punto de salida
        }


        public DataTable BusquedaFechas(DateTime Fecha1, DateTime Fecha2)
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();

                // Consulta SQL corregida
                string consulta = @"
            SELECT NombreProducto, FechaIngreso 
            FROM Producto 
            WHERE FechaIngreso BETWEEN @Fecha1 AND @Fecha2;
        ";

                SqlCommand comando = new SqlCommand(consulta, SqlCon);
                comando.Parameters.AddWithValue("@Fecha1", Fecha1);
                comando.Parameters.AddWithValue("@Fecha2", Fecha2);

                SqlCon.Open();
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
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }
        }
    }
}
