using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace AudioTec.Logica
{
    static class Conexion
    {

        public static string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;
        // entra al archivo configuracion y devuelve la ubicacion de la bd

    }
}
