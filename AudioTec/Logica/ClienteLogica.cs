using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AudioTec.Modelo;
using System.Data.SQLite;

namespace AudioTec.Logica
{
    public class ClienteLogica
    {

        //private static string cadena = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;
        // entra al archivo configuracion y devuelve la ubicacion de la bd

        // Patron Singleton
        private static ClienteLogica _instancia = null;

        public ClienteLogica() { }

        public static ClienteLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia= new ClienteLogica();
                }

                return _instancia;
            }
        }


        public static bool Guardar(Cliente obj)
        {
            bool respuesta = true;

            using(SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "insert into Cliente(DNI,Nombre,Direccion,Telefono,Email) values (@dni,@nombre,@direccion,@telefono,@email)";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@dni", obj.DNI));
                cmd.Parameters.Add(new SQLiteParameter("@nombre", obj.Nombre));
                cmd.Parameters.Add(new SQLiteParameter("@direccion", obj.Direccion));
                cmd.Parameters.Add(new SQLiteParameter("@telefono", obj.Telefono));
                cmd.Parameters.Add(new SQLiteParameter("@email", obj.Email));
                cmd.CommandType = System.Data.CommandType.Text;

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }

        // Trae todos los clientes
        public static List<Cliente> Listar()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "select * from Cliente";
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Cliente()
                        {
                            DNI = reader["DNI"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["email"].ToString(),
                        });
                    }
                }

            }


            return lista;
        }

        // modificar
        public static bool Editar(Cliente obj)
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "update Cliente " +
                    "set DNI = @dni, Nombre = @nombre, Direccion = @direccion, Telefono = @telefono, Email = @email" +
                    "where DNI = @dni";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@dni", obj.DNI));
                cmd.Parameters.Add(new SQLiteParameter("@nombre", obj.Nombre));
                cmd.Parameters.Add(new SQLiteParameter("@direccion", obj.Direccion));
                cmd.Parameters.Add(new SQLiteParameter("@telefono", obj.Telefono));
                cmd.Parameters.Add(new SQLiteParameter("@enail", obj.Email));
                cmd.CommandType = System.Data.CommandType.Text;

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }
            }
            return respuesta;

        }


        // Trae un cliente asociado a una orden
        public static Cliente TraerCliente(Orden obj)
        {
            Cliente cliente = new Cliente();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT c.DNI, c.Nombre, c.Direccion, c.Telefono, c.Email" +
                    "FROM Orden o" +
                    "JOIN Cliente c ON (o.ClienteDNI = c.DNI)" +
                    "Where o.OrdenID = @OrdenID";

                SQLiteCommand cmd = new SQLiteCommand (query, con);
                cmd.Parameters.Add(new SQLiteParameter("@OrdenID", obj.OrdenID));
                cmd.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        cliente.DNI = reader["DNI"].ToString();
                        cliente.Nombre = reader["Nombre"].ToString();
                        cliente.Direccion = reader["Direccion"].ToString();
                        cliente.Telefono = reader["Telefono"].ToString();
                        cliente.Email = reader["email"].ToString();
                    }

                }

            }

            return cliente;
        }

        // Traer cliente por su DNI
        public static Cliente TraerCliente(string dni)
        {

            Cliente cliente = new Cliente();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT DNI, Nombre, Direccion, Telefono, Email" +
                    "FROM Cliente" +
                    "Where DNI = @dni";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@dni", dni));
                cmd.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        cliente.DNI = reader["DNI"].ToString();
                        cliente.Nombre = reader["Nombre"].ToString();
                        cliente.Direccion = reader["Direccion"].ToString();
                        cliente.Telefono = reader["Telefono"].ToString();
                        cliente.Email = reader["email"].ToString();
                    }
                    
                }

            }

            return cliente;
        }

        // Traer cliente por el ordenId asociado
        public static Cliente TraerCliente(int nroOrden)
        {

            Cliente cliente = new Cliente();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT c.DNI, c.Nombre, c.Direccion, c.Telefono, c.Email" +
                    "FROM Cliente c" +
                    "INNER JOIN Orden o ON c.DNI = o.ClienteDNI" +
                    "Where o.OrdenID = @nroOrden";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@nroOrden", nroOrden));
                cmd.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        cliente.DNI = reader["DNI"].ToString();
                        cliente.Nombre = reader["Nombre"].ToString();
                        cliente.Direccion = reader["Direccion"].ToString();
                        cliente.Telefono = reader["Telefono"].ToString();
                        cliente.Email = reader["email"].ToString();
                    }

                }

            }

            return cliente;
        }

        // Comprobar si cliente existe
        public static bool Existe(string dni)
        {
            bool respuesta = false;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Cliente WHERE DNI = @dni";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@dni",dni));

                int cantidad = Convert.ToInt32(cmd.ExecuteScalar());
                respuesta = cantidad > 0;
            }

            return respuesta;
        }
    }
}
