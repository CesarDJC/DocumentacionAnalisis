using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoBasedeDatos1.Entidades
{
    public class E_Proveedor
    {

        public int IdProveedor {  get; set; }
        public string NombreProveedor { get; set; }

        public long Telefono { get; set; }

        public string CorreoElectronico { get; set; }

        public string Direccion {  get; set; }
      

    }
}
