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


        public bool Guardar(Cliente obj)
        {
            bool respuesta = true;

            using(SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "insert into Cliente(DNI,Nombre,Direccion,Telefono) values (@dni,@nombre,@direccion,@telefono)";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@dni", obj.DNI));
                cmd.Parameters.Add(new SQLiteParameter("@nombre", obj.Nombre));
                cmd.Parameters.Add(new SQLiteParameter("@direccion", obj.Direccion));
                cmd.Parameters.Add(new SQLiteParameter("@telefono", obj.Telefono));
                cmd.CommandType = System.Data.CommandType.Text;

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }

        // Trae todos los clientes
        public List<Cliente> Listar()
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
                        });
                    }
                }

            }


            return lista;
        }

        // modificar
        public bool Editar(Cliente obj)
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "update Cliente " +
                    "set DNI = @dni, Nombre = @nombre, Direccion = @direccion, Telefono = @telefono" +
                    "where DNI = @dni";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@dni", obj.DNI));
                cmd.Parameters.Add(new SQLiteParameter("@nombre", obj.Nombre));
                cmd.Parameters.Add(new SQLiteParameter("@direccion", obj.Direccion));
                cmd.Parameters.Add(new SQLiteParameter("@telefono", obj.Telefono));
                cmd.CommandType = System.Data.CommandType.Text;

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }
            }
            return respuesta;

        }


        // Trae un cliente asociado a una orden
        public Cliente TraerCliente(Orden obj)
        {
            Cliente cliente = new Cliente();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT c.DNI, c.Nombre. Direccion, c.Telefono" +
                    "FROM Orden o" +
                    "JOIN Cliente c ON (o.ClienteDNI = c.DNI)" +
                    "Where o.OrdenID = @OrdenID";

                SQLiteCommand cmd = new SQLiteCommand (query, con);
                cmd.Parameters.Add(new SQLiteParameter("@OrdenID", obj.OrdenID));
                cmd.CommandType = System.Data.CommandType.Text;

                if (cmd.ExecuteNonQuery() > 0)
                {

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        cliente.DNI = reader["DNI"].ToString();
                        cliente.Nombre = reader["Nombre"].ToString();
                        cliente.Direccion = reader["Direccion"].ToString();
                        cliente.Telefono = reader["Telefono"].ToString();
                    }

                }
            }

            return cliente;
        }
    }
}
