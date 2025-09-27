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
    public class D_Usuarios
    {
        public DataTable ListarUsuarios() {
            SqlDataReader resultado;
            DataTable tabla=new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("select IdUsuario, NombreUsuario as [Nombre Usuario], CorreoElectronico as [Correo Electronico], " +
                "CASE WHEN TipoUsuario = 1 THEN 'Administrador' ELSE 'Usuario' END as [Tipo de Usuario], " +
                "FechaRegistro as [Fecha de Registro] from Usuarios", SqlCon);
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
        public DataTable BusquedaUsuarios(string bus)
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                SqlCommand comando = new SqlCommand("select *from Usuarios where NombreUsuario like '" + bus + "%'", SqlCon);
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

        public (string resultado, bool esAdmin) Login_Usuario(E_Usuario Usuario)
        {
            string respuesta = "";
            bool esAdministrador = false;
            SqlConnection SqlCon = new SqlConnection();

            try
            {
                SqlCon = Conexion.crearInstancia().CrearConexion();
                string contraseña = Cifrado.HashPassword(Usuario.Contraseña);

                string consultaVerificacion = @"
        SELECT TipoUsuario FROM Usuarios 
        WHERE NombreUsuario = @NombreUsuario AND Contraseña = @Contraseña;";

                SqlCommand comandoVerificacion = new SqlCommand(consultaVerificacion, SqlCon);
                comandoVerificacion.Parameters.AddWithValue("@NombreUsuario", Usuario.NombreUsuario);
                comandoVerificacion.Parameters.AddWithValue("@Contraseña", contraseña);

                SqlCon.Open();

                object resultado = comandoVerificacion.ExecuteScalar();

                if (resultado != null)
                {
                    esAdministrador = (bool)resultado;
                    respuesta = "existente";
                }
                else
                {
                    respuesta = "no existente";
                }
            }
            catch (Exception ex)
            {
                respuesta = "Error al verificar: " + ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
            }

            return (respuesta, esAdministrador);
        }

        public string Guardar_Usuario(E_Usuario Usuario)
        {
            string respuesta = "";
            SqlConnection SqlCon = new SqlConnection();
            bool UsuarioExiste = false; 
           

            try
            {
              
                SqlCon = Conexion.crearInstancia().CrearConexion();
                Usuario.Contraseña = Cifrado.HashPassword(Usuario.Contraseña);
        
                string consultaVerificacion = @"
                SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @NombreUsuario;";

               
                SqlCommand comandoVerificacion = new SqlCommand(consultaVerificacion, SqlCon);
                comandoVerificacion.Parameters.AddWithValue("@NombreUsuario", Usuario.NombreUsuario);

                SqlCon.Open();

                
                int cantidad = (int)comandoVerificacion.ExecuteScalar();

                if (cantidad > 0)
                {
                  
                    UsuarioExiste = true;
                    respuesta = "existente";
                }

                

                if (!UsuarioExiste)
                {
                    string consultaInsercion = @"
                INSERT INTO Usuarios (NombreUsuario, Contraseña, CorreoElectronico)
                VALUES (@NombreUsuario, @Contraseña, @CorreoElectronico);";


                 
                    SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon);

                   
                    comandoInsercion.Parameters.AddWithValue("@NombreUsuario", Usuario.NombreUsuario);
                    comandoInsercion.Parameters.AddWithValue("@Contraseña", Usuario.Contraseña);
                    comandoInsercion.Parameters.AddWithValue("@CorreoElectronico", Usuario.CorreoElectronico);


          
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
                respuesta = "Error al guardar el usuario: " + ex.Message;
            }
            finally
            {
                // Cierra la conexión si está abierta
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }

            return respuesta; 
        }



        public string Editar_Usuario(E_Usuario Usuario)
        {
            string respuesta = "";
            SqlConnection SqlCon = new SqlConnection();
            bool proveedorExiste = false; // Bandera para controlar si el producto ya existe
           

            try
            {
                // Obtiene la conexión a la base de datos
                SqlCon = Conexion.crearInstancia().CrearConexion();

                // Verificar si el nombre del producto ya existe
                string consultaVerificacion = @"
                SELECT COUNT(*) FROM Usuarios WHERE NombreUsuario = @NombreUsuario and IdUsuario<>@IdUsuario;";

           
                SqlCommand comandoVerificacion = new SqlCommand(consultaVerificacion, SqlCon);
                comandoVerificacion.Parameters.AddWithValue("@NombreUsuario", Usuario.NombreUsuario);
                comandoVerificacion.Parameters.AddWithValue("@IdUsuario", Usuario.IdUsuario);



                SqlCon.Open();

                int cantidad = (int)comandoVerificacion.ExecuteScalar();

                if (cantidad > 0)
                {

                    proveedorExiste = true;
                    respuesta = "existente";
                }

                


                if (!proveedorExiste)
                {

                    string consultaInsercion = @"
                             update Usuarios set NombreUsuario=@NombreUsuario,
                            CorreoElectronico=@CorreoElectronico, TipoUsuario=@TipoUsuario where IdUsuario=@IdUsuario;";



                    SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon);


                    comandoInsercion.Parameters.AddWithValue("@IdUsuario",Usuario.IdUsuario);
                    comandoInsercion.Parameters.AddWithValue("@NombreUsuario", Usuario.NombreUsuario);
                    comandoInsercion.Parameters.AddWithValue("@CorreoElectronico", Usuario.CorreoElectronico);
                    comandoInsercion.Parameters.AddWithValue("@TipoUsuario",Usuario.TipoUsuario);
    

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

                respuesta = "Error al actualizar el usuario: " + ex.Message;
            }
            finally
            {
                // Cierra la conexión si está abierta
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }
            }

            return respuesta; 
        }


        public string Eliminar_Usuario(E_Usuario Usuario)
        {
            String respuesta = "";
            SqlConnection SqlCon = new SqlConnection();

            try
            {

                SqlCon = Conexion.crearInstancia().CrearConexion();

                SqlCon.Open();


                // Define la consulta SQL para insertar un nuevo producto
                string consultaInsercion = @"
                        delete from Usuarios
                        where IdUsuario=@IdUsuario; ";


                SqlCommand comandoInsercion = new SqlCommand(consultaInsercion, SqlCon);

                comandoInsercion.Parameters.AddWithValue("@IdUsuario", Usuario.IdUsuario);


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

                respuesta = "Error al eliminar el Usuario: " + ex.Message;
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

    }
}
