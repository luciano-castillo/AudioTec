using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTec
{
    internal class Cliente
    {
        public String nombre { get; set; }
        public int dni { get; set; }
        public String direccion { get; set; }
        public int telefono { get; set; }

        public Cliente(String nombre, int dni,String direccion,int telefono)
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
