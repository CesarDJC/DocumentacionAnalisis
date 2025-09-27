using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoBasedeDatos1.Entidades
{
    public class E_Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public string CorreoElectronico { get; set; }

        public DateTime FechaRegistro   { get; set; }
        public bool TipoUsuario { get; set; }
    }
}
