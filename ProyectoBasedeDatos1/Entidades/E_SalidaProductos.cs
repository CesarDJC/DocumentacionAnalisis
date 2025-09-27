using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoBasedeDatos1.Entidades
{
    public class E_SalidaProductos
    {
       public int IdSalida {  get; set; }
       public int IdProducto { get; set; }
       public int Cantidad { get; set; }
       public DateTime FechaSalida { get; set; }

        public string NitCliente { get; set; }

        public decimal Total { get; set; }


    }
}
