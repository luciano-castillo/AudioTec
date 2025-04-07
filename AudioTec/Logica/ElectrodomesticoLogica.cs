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
        public List<Electrodomestico> TraerElectrodomesticos(Orden obj)
        {
            List<Electrodomestico> lista = new List<Electrodomestico>();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT e.ElectrodomesticoID, e.Articulo, e.Modelo, e.Marca, e.Observacion" +
                    "FROM Orden_Electrodomestico oe" +
                    "JOIN Electrodomestico e ON (oe.ElectrodomesticoID = e.ElectrodomesticoID)" +
                    "WHERE oe.OrdenID = @nroOrden";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@nroOrden", obj.OrdenID));

                if (cmd.ExecuteNonQuery() > 0)
                {
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
                                Observacion = reader["Observacion"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

    }
}
