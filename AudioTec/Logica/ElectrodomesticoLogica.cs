using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AudioTec.Modelo;
using System.Data.SQLite;

namespace AudioTec.Logica
{
    internal class ElectrodomesticoLogica
    {
        private static ElectrodomesticoLogica _instancia = null;
        public static int electroIDActual = UltimoNroElectrodomestico();

        public ElectrodomesticoLogica() { }

        public static ElectrodomesticoLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new ElectrodomesticoLogica();
                }

                return _instancia;
            }
        }

        // Traer los electrodomesticos de una orden en una lista
        public static List<Electrodomestico> TraerElectrodomesticos(Orden obj)
        {
            List<Electrodomestico> lista = new List<Electrodomestico>();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT e.ElectrodomesticoID, e.Articulo, e.Modelo, e.Marca, e.Observacion, e.ClienteDNI " +
                    "FROM Orden_Electrodomestico o " +
                    "JOIN Electrodomestico e ON (o.ElectrodomesticoID = e.ElectrodomesticoID) " +
                    "WHERE o.OrdenID = @nroOrden";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@nroOrden", obj.OrdenID));

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Electrodomestico()
                        {
                            ElectrodomesticoID = reader["ElectrodomesticoID"].ToString(),
                            Articulo = reader["Articulo"].ToString(),
                            Modelo = reader["Modelo"].ToString(),
                            Marca = reader["Marca"].ToString(),
                            Observacion = reader["Observacion"].ToString(),
                            Dueno = ClienteLogica.TraerCliente(reader["ClienteDNI"].ToString())
                            
                        });
                    }
                }

            }

            return lista;
        }

        // Guardar electrodomestico
        public static bool GuardarElectrodomestico(Electrodomestico obj)
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "insert into Electrodomestico (ClienteDNI, Articulo, Modelo, Marca, Observacion)" +
                    "Values (@clientedni, @articulo, @modelo, @marca, @observacion)";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@clientedni", obj.Dueno.DNI));
                cmd.Parameters.Add(new SQLiteParameter("@articulo", obj.Articulo));
                cmd.Parameters.Add(new SQLiteParameter("@modelo", obj.Modelo));
                cmd.Parameters.Add(new SQLiteParameter("@marca", obj.Marca));
                cmd.Parameters.Add(new SQLiteParameter("@observacion", obj.Observacion));
                cmd.CommandType = System.Data.CommandType.Text;

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }

            }

            return respuesta;
        }

        public static bool RelacionarOrdenElectrodomestico(Electrodomestico obj, Orden orden)
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "insert into Orden_Electrodomestico (OrdenID, ElectrodomesticoID) " +
                    "Values (@ordenID, @electrodomesticoID)";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@ordenID", orden.OrdenID));
                cmd.Parameters.Add(new SQLiteParameter("@electrodomesticoID", obj.ElectrodomesticoID));

                cmd.CommandType = System.Data.CommandType.Text;

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }

            }

            return respuesta;
        }

        // Eliminar electrodomesticos
        public static bool EliminarElectrodomesticosSinOrden()
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "DELETE FROM Electrodomestico " +
                    "WHERE ElectrodomesticoID NOT IN (SELECT ElectrodomesticoID FROM Orden_Electrodomestico)";

                using(SQLiteCommand cmd = new SQLiteCommand(query, con))
                {
                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        respuesta = false;
                    }
                }
            }

            return respuesta;
        }


        // Comprueba si ya existe un electrodomestico
        public static bool Existe(string id)
        {
            bool respuesta = false;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Electrodomestico WHERE ElectrodomesticoID = @id";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@id", id));

                int cantidad = Convert.ToInt32(cmd.ExecuteScalar());
                respuesta = cantidad > 0;
            }

            return respuesta;
        }

        public static int CantElectrodomesticos()
        {
            int respuesta = 0;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT COUNT(*) AS TotalElectro FROM Electrodomestico";

                SQLiteCommand cmd = new SQLiteCommand(query, con);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        respuesta = int.Parse(reader["TotalElectro"].ToString());
                    }

                }

            }

            return respuesta;
        }

        private static int UltimoNroElectrodomestico()
        {
            int resultado = 0;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT MAX(ElectrodomesticoID) AS max FROM Electrodomestico";

                SQLiteCommand cmd = new SQLiteCommand(query, con);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        resultado = int.Parse(reader["max"].ToString());
                    }

                }

            }

            return resultado;
        }

        public static bool CargarObservacionElectro(Electrodomestico obj)
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "UPDATE Electrodomestico " +
                    "SET Observacion = @observacion " +
                    "WHERE ElectrodomesticoID = @ID";

                SQLiteCommand cmd = new SQLiteCommand(query,con);
                cmd.Parameters.Add(new SQLiteParameter("@ID", obj.ElectrodomesticoID));
                cmd.Parameters.Add(new SQLiteParameter("@observacion", obj.Observacion));

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }

            }

            return respuesta;
        }

        public static void AumentarNroElectrodomestico()
        {
            electroIDActual++;
        }

    }
}
