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
    public class D_Proveedores
    {
        public DataTable ListarProveedores() {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("select IdProveedor, NombreProveedor as [Nombre Proveedor], Telefono, CorreoElectronico as [Correo Electronico], Direccion from Proveedor", SqlCon);

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


        public DataTable NombresProveedores()
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("select IdProveedor,NombreProveedor as [Nombre Proveedor] from Proveedor", SqlCon);

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




        public DataTable Busqueda(string bus)
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("select *from Proveedor where NombreProveedor like '" + bus + "%'", SqlCon);
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

        public string Guardar_Proveedor(E_Proveedor Proveedor)
        {
            string respuesta = "";
            SqlConnection SqlCon = new SqlConnection();
            bool proveedorExiste = false; // Bandera para controlar si el producto ya existe
            bool TelefonoExiste = false;

            try
            {
                // Obtiene la conexión a la base de datos
                SqlCon = Conexion.crearInstancia().CrearConexion();

                // Verificar si el nombre del producto ya existe
                string consultaVerificacion = @"
                SELECT COUNT(*) FROM Proveedor WHERE NombreProveedor = @NombreProveedor;";

                string consultaVerificacion2 = @"
                SELECT COUNT(*) FROM Proveedor WHERE Telefono = @Telefono";

                SqlCommand comandoVerificacion = new SqlCommand(consultaVerificacion, SqlCon);
                comandoVerificacion.Parameters.AddWithValue("@NombreProveedor", Proveedor.NombreProveedor);

                SqlCommand comando2=new SqlCommand(consultaVerificacion2, SqlCon);
                comando2.Parameters.AddWithValue("@Telefono",Proveedor.Telefono);
                // Abre la conexión
                SqlCon.Open();

                // Ejecuta la consulta de verificación
                int cantidad = (int)comandoVerificacion.ExecuteScalar();

                if (cantidad > 0)
                {
                    // Si ya existe un producto con el mismo nombre, activa la bandera
                    proveedorExiste = true;
                    respuesta = "existente";
                }

                int cantidad2 = (int)comando2.ExecuteScalar();
                if (cantidad2 > 0) {
                    TelefonoExiste = true;
                    respuesta = "t existe";
                }

                // Si el producto no existe, procede con la inserción
                if (!proveedorExiste && !TelefonoExiste)
                {
                    // Define la consulta SQL para insertar un nuevo producto
                    string consultaInsercion = @"
                INSERT INTO Proveedor (NombreProveedor, Telefono, CorreoElectronico, Direccion)
                VALUES (@NombreProveedor, @Telefono, @CorreoElectronico, @Direccion);";
            

                    // Crea el comando SQL para la inserción
                    SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon);

                    // Asigna los parámetros con los valores del objeto E_Producto
                    comandoInsercion.Parameters.AddWithValue("@NombreProveedor", Proveedor.NombreProveedor);
                    comandoInsercion.Parameters.AddWithValue("@Telefono", Proveedor.Telefono);
                    comandoInsercion.Parameters.AddWithValue("@CorreoElectronico", Proveedor.CorreoElectronico);
                    comandoInsercion.Parameters.AddWithValue("@Direccion", Proveedor.Direccion);

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
                respuesta = "Error al guardar el proveedor: " + ex.Message;
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

        public string Editar_Proveedor(E_Proveedor Proveedor)
        {
            string respuesta = "";
            SqlConnection SqlCon = new SqlConnection();
            bool proveedorExiste = false; // Bandera para controlar si el producto ya existe
            bool TelefonoExiste = false;

            try
            {
                // Obtiene la conexión a la base de datos
                SqlCon = Conexion.crearInstancia().CrearConexion();

                // Verificar si el nombre del producto ya existe
                string consultaVerificacion = @"
                SELECT COUNT(*) FROM Proveedor WHERE NombreProveedor = @NombreProveedor and IdProveedor<>@IdProveedor;";

                string consultaVerificacion2 = @"
                SELECT COUNT(*) FROM Proveedor WHERE Telefono = @Telefono and IdProveedor<>@IdProveedor";

                SqlCommand comandoVerificacion = new SqlCommand(consultaVerificacion, SqlCon);
                comandoVerificacion.Parameters.AddWithValue("@NombreProveedor", Proveedor.NombreProveedor);
                comandoVerificacion.Parameters.AddWithValue("@IdProveedor", Proveedor.IdProveedor);

                SqlCommand comando2 = new SqlCommand(consultaVerificacion2, SqlCon);
                comando2.Parameters.AddWithValue("@Telefono", Proveedor.Telefono);
                comando2.Parameters.AddWithValue("@IdProveedor", Proveedor.IdProveedor);
                
                SqlCon.Open();

                int cantidad = (int)comandoVerificacion.ExecuteScalar();

                if (cantidad > 0)
                {
         
                    proveedorExiste = true;
                    respuesta = "existente";
                }

                int cantidad2 = (int)comando2.ExecuteScalar();
                if (cantidad2 > 0)
                {
                    TelefonoExiste = true;
                    respuesta = "t existe";
                }

      
                if (!proveedorExiste && !TelefonoExiste)
                {
                   
                    string consultaInsercion = @"
                             update Proveedor set NombreProveedor=@NombreProveedor,Telefono=@Telefono,
                            CorreoElectronico=@CorreoElectronico,Direccion=@Direccion where IdProveedor=@IdProveedor;";



                    SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon);

               
                    comandoInsercion.Parameters.AddWithValue("@IdProveedor",Proveedor.IdProveedor);
                    comandoInsercion.Parameters.AddWithValue("@NombreProveedor", Proveedor.NombreProveedor);
                    comandoInsercion.Parameters.AddWithValue("@Telefono", Proveedor.Telefono);
                    comandoInsercion.Parameters.AddWithValue("@CorreoElectronico", Proveedor.CorreoElectronico);
                    comandoInsercion.Parameters.AddWithValue("@Direccion", Proveedor.Direccion);

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
                respuesta = "Error al actualizar el proveedor: " + ex.Message;
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

        public string Eliminar_Proveedor(E_Proveedor Proveedor)
        {
            String respuesta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {
            
                SqlCon = Conexion.crearInstancia().CrearConexion();

                SqlCon.Open();


                // Define la consulta SQL para insertar un nuevo producto
                string consultaInsercion = @"
                        delete from Proveedor
                        where IdProveedor=@IdProveedor; ";


                SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon);

                comandoInsercion.Parameters.AddWithValue("@IdProveedor",Proveedor.IdProveedor);

               
                int filasAfectadas = comandoInsercion.ExecuteNonQuery();

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
               
                respuesta = "Error al eliminar el proveedor: " + ex.Message;
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

        public DataTable BusquedaProductos(E_Proveedor Proveedor)
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand(@"select p.NombreProducto as Producto, pr.NombreProveedor as Proveedor from Producto_Proveedor as pp inner join Producto as p on pp.IdProducto=p.IdProducto
                                                        inner join Proveedor as pr on pp.IdProveedor=pr.IdProveedor where pr.IdProveedor=@IdProveedor", SqlCon);
                comando.Parameters.AddWithValue("@IdProveedor", Proveedor.IdProveedor);
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
    }
}
