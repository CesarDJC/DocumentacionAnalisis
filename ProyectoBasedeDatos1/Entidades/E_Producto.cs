using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoBasedeDatos1.Entidades
{
    public class E_Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Descripción { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string Imagen { get; set; }


    }
}
