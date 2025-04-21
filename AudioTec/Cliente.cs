using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTec
{
    public class Cliente
    {
        public String nombre { get; set; }
        public int dni { get; set; }
        public String direccion { get; set; }
        public String telefono { get; set; }
        public int nroOrden { get; set; }
        public String articulo { get; set; }
        public String modelo { get; set; }
        public String marca { get; set; }
        public String email { get; set; }
        public DateTime fechaLlegada { get; set; }

        public Cliente(String nombre, int dni,String direccion,String telefono)
        {
            this.nombre = nombre;
            this.dni = dni;
            this.direccion = direccion;
            this.telefono = telefono;
        
        }

        public override string ToString()
        {
            return nombre;
        }

    }
}
