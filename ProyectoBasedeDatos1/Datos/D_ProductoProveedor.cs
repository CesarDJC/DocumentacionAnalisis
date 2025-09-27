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
    public class D_ProductoProveedor
    {
        public DataTable ListarDistribucion()
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand(
                    "SELECT p.IdProducto, pp.IdProveedor, p.NombreProducto AS [Producto], pr.NombreProveedor AS [Proveedor], pp.CantidadSuministrada AS [Cantidad Suministrada] " +
                    "FROM Producto AS p " +
                    "LEFT JOIN Producto_Proveedor AS pp ON p.IdProducto = pp.IdProducto " +
                    "LEFT JOIN Proveedor AS pr ON pp.IdProveedor = pr.IdProveedor",
                    SqlCon);

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


        public string GuardarRelacion(E_ProductoProveedor ProdProv)
        {
            string respuesta = "";
            SqlConnection SqlCon = new SqlConnection();
            bool relacionexiste = false;

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCon.Open();

                // Verificar si la relación ya existe
                string consulta = @"SELECT COUNT(*) FROM Producto_Proveedor 
                          WHERE IdProducto=@IdProducto AND IdProveedor=@IdProveedor";
                SqlCommand comando = new SqlCommand(consulta, SqlCon);
                comando.Parameters.AddWithValue("@IdProducto", ProdProv.IdProducto);
                comando.Parameters.AddWithValue("@IdProveedor", ProdProv.IdProveedor);

                int cantidad = (int)comando.ExecuteScalar();
                if (cantidad > 0)
                {
                    return "existente";
                }

                // Iniciar transacción para asegurar consistencia
                SqlTransaction transaction = SqlCon.BeginTransaction();

                try
                {
                    // Insertar la relación Producto_Proveedor
                    string consultaInsercion = @"INSERT INTO Producto_Proveedor 
                                      (IdProducto, IdProveedor, CantidadSuministrada) 
                                      VALUES(@IdProducto, @IdProveedor, @CantidadSuministrada)";
                    SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon, transaction);
                    comandoInsercion.Parameters.AddWithValue("@IdProducto", ProdProv.IdProducto);
                    comandoInsercion.Parameters.AddWithValue("@IdProveedor", ProdProv.IdProveedor);
                    comandoInsercion.Parameters.AddWithValue("@CantidadSuministrada", ProdProv.CantidadSuministrada);

                    int filas = comandoInsercion.ExecuteNonQuery();
                    if (filas <= 0)
                    {
                        transaction.Rollback();
                        return "no guardado";
                    }

                    // Actualizar el stock del producto
                    string actualizarStock = @"UPDATE Producto 
                                    SET Stock = ISNULL(Stock, 0) + @CantidadSuministrada
                                    WHERE IdProducto = @IdProducto";
                    SqlCommand cmdStock = new SqlCommand(actualizarStock, SqlCon, transaction);
                    cmdStock.Parameters.AddWithValue("@CantidadSuministrada", ProdProv.CantidadSuministrada);
                    cmdStock.Parameters.AddWithValue("@IdProducto", ProdProv.IdProducto);

                    int filasStock = cmdStock.ExecuteNonQuery();
                    if (filasStock <= 0)
                    {
                        transaction.Rollback();
                        return "error actualizando stock";
                    }

                    transaction.Commit();
                    respuesta = "guardado";
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error al guardar: " + ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
            }
            return respuesta;
        }

        public string Editar_Relacion(E_ProductoProveedor ProdProv, int idProductoOriginal, int idProveedorOriginal)
        {
            string respuesta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCon.Open();

                // Verificar si la nueva relación ya existe (excepto la que estamos editando)
                string consultaVerificacion = @"
            SELECT COUNT(*) 
            FROM Producto_Proveedor 
            WHERE IdProducto = @IdProducto 
              AND IdProveedor = @IdProveedor
              AND NOT (IdProducto = @IdProductoOriginal AND IdProveedor = @IdProveedorOriginal)";

                SqlCommand comandoVerificacion = new SqlCommand(consultaVerificacion, SqlCon);
                comandoVerificacion.Parameters.AddWithValue("@IdProducto", ProdProv.IdProducto);
                comandoVerificacion.Parameters.AddWithValue("@IdProveedor", ProdProv.IdProveedor);
                comandoVerificacion.Parameters.AddWithValue("@IdProductoOriginal", idProductoOriginal);
                comandoVerificacion.Parameters.AddWithValue("@IdProveedorOriginal", idProveedorOriginal);

                int cantidad = (int)comandoVerificacion.ExecuteScalar();
                if (cantidad > 0)
                {
                    return "existente";
                }

                // Iniciar transacción
                SqlTransaction transaction = SqlCon.BeginTransaction();

                try
                {
                    // 1. Obtener la cantidad anterior de la relación original
                    string obtenerAnterior = @"
                SELECT CantidadSuministrada 
                FROM Producto_Proveedor
                WHERE IdProducto = @IdProductoOriginal 
                  AND IdProveedor = @IdProveedorOriginal";

                    SqlCommand cmdAnterior = new SqlCommand(obtenerAnterior, SqlCon, transaction);
                    cmdAnterior.Parameters.AddWithValue("@IdProductoOriginal", idProductoOriginal);
                    cmdAnterior.Parameters.AddWithValue("@IdProveedorOriginal", idProveedorOriginal);

                    int cantidadAnterior = Convert.ToInt32(cmdAnterior.ExecuteScalar());

                    // 2. Reducir el stock del producto original (si el producto ha cambiado)
                    if (ProdProv.IdProducto != idProductoOriginal)
                    {
                        string reducirStockOriginal = @"
                    UPDATE Producto 
                    SET Stock = ISNULL(Stock, 0) - @CantidadAnterior
                    WHERE IdProducto = @IdProductoOriginal";

                        SqlCommand cmdReducirOriginal = new SqlCommand(reducirStockOriginal, SqlCon, transaction);
                        cmdReducirOriginal.Parameters.AddWithValue("@CantidadAnterior", cantidadAnterior);
                        cmdReducirOriginal.Parameters.AddWithValue("@IdProductoOriginal", idProductoOriginal);
                        cmdReducirOriginal.ExecuteNonQuery();
                    }

                    // 3. Actualizar la relación
                    string consultaActualizacion = @"
                UPDATE Producto_Proveedor 
                SET IdProducto = @IdProducto, 
                    IdProveedor = @IdProveedor, 
                    CantidadSuministrada = @CantidadSuministrada
                WHERE IdProducto = @IdProductoOriginal 
                  AND IdProveedor = @IdProveedorOriginal";

                    SqlCommand comandoActualizacion = new SqlCommand(consultaActualizacion, SqlCon, transaction);
                    comandoActualizacion.Parameters.AddWithValue("@IdProducto", ProdProv.IdProducto);
                    comandoActualizacion.Parameters.AddWithValue("@IdProveedor", ProdProv.IdProveedor);
                    comandoActualizacion.Parameters.AddWithValue("@CantidadSuministrada", ProdProv.CantidadSuministrada);
                    comandoActualizacion.Parameters.AddWithValue("@IdProductoOriginal", idProductoOriginal);
                    comandoActualizacion.Parameters.AddWithValue("@IdProveedorOriginal", idProveedorOriginal);

                    int filas = comandoActualizacion.ExecuteNonQuery();
                    if (filas <= 0)
                    {
                        transaction.Rollback();
                        return "no actualizado";
                    }

                    // 4. Ajustar el stock del nuevo producto
                    if (ProdProv.IdProducto != idProductoOriginal)
                    {
                        // Si cambió el producto, sumamos la nueva cantidad al nuevo producto
                        string actualizarStockNuevo = @"
                    UPDATE Producto 
                    SET Stock = ISNULL(Stock, 0) + @CantidadSuministrada
                    WHERE IdProducto = @IdProducto";

                        SqlCommand cmdStockNuevo = new SqlCommand(actualizarStockNuevo, SqlCon, transaction);
                        cmdStockNuevo.Parameters.AddWithValue("@CantidadSuministrada", ProdProv.CantidadSuministrada);
                        cmdStockNuevo.Parameters.AddWithValue("@IdProducto", ProdProv.IdProducto);
                        cmdStockNuevo.ExecuteNonQuery();
                    }
                    else
                    {
                        // Si es el mismo producto, ajustamos la diferencia
                        string actualizarStock = @"
                    UPDATE Producto 
                    SET Stock = ISNULL(Stock, 0) + (@CantidadSuministrada - @CantidadAnterior)
                    WHERE IdProducto = @IdProducto";

                        SqlCommand cmdStock = new SqlCommand(actualizarStock, SqlCon, transaction);
                        cmdStock.Parameters.AddWithValue("@CantidadSuministrada", ProdProv.CantidadSuministrada);
                        cmdStock.Parameters.AddWithValue("@CantidadAnterior", cantidadAnterior);
                        cmdStock.Parameters.AddWithValue("@IdProducto", ProdProv.IdProducto);
                        cmdStock.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    respuesta = "actualizado";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    respuesta = "Error al actualizar: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error al actualizar: " + ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
            }
            return respuesta;
        }

        public string Eliminar_Relacion(E_ProductoProveedor ProdProv)
        {
            string respuesta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();


                SqlCon.Open();
                  
                    string consultaActualizacion = @"
                delete from Producto_Proveedor
                WHERE IdProducto = @IdProducto 
                  AND IdProveedor = @IdProveedor;
            ";

                    SqlCommand comandoActualizacion = new SqlCommand(consultaActualizacion, SqlCon);
                    comandoActualizacion.Parameters.AddWithValue("@IdProducto", ProdProv.IdProducto);
                    comandoActualizacion.Parameters.AddWithValue("@IdProveedor", ProdProv.IdProveedor);
                 

                    int filas = comandoActualizacion.ExecuteNonQuery();

                    if (filas > 0)
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
                respuesta = "Error al eliminar: " + ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }

            return respuesta;
        }


        public DataTable ObtenerCantidad() {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                // Obtiene la conexión a la base de datos
                SqlCon = Conexion.crearInstancia().CrearConexion();

        
                string consulta = @"
                    SELECT 
                        Proveedor.NombreProveedor, 
                        SUM(Producto_Proveedor.CantidadSuministrada) AS TotalSuministrado
                    FROM 
                        Producto_Proveedor
                    INNER JOIN 
                        Proveedor ON Proveedor.IdProveedor = Producto_Proveedor.IdProveedor
                    GROUP BY 
                        Proveedor.NombreProveedor;
                ";


             
                SqlCommand comando = new SqlCommand(consulta, SqlCon);

             
                SqlCon.Open();

                // Ejecuta la consulta y llena el DataTable
                resultado = comando.ExecuteReader();
                tabla.Load(resultado);

                return tabla;
            }
            catch (Exception ex)
            {
   
                MessageBox.Show("Error al obtener la cantidad suministrada por proveedor: " + ex.Message);
                return null;
            }
            finally
            {
                // Cierra la conexión si está abierta
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }
        }
    }
}
