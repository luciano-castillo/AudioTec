using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AudioTec.Modelo;

namespace AudioTec.Logica
{
    internal class OrdenLogica
    {


        private static OrdenLogica _instancia = null;

        public OrdenLogica() { }

        public static OrdenLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new OrdenLogica();
                }

                return _instancia;
            }
        }


        public static bool CrearOrden(Orden obj)
        {
            bool respuesta = true;

            using(SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "insert into Orden (ClienteDNI, Presupuesto, Fecha_reparacion, Fecha_retiro, Repuesto)" +
                    "Values (@clientedni, @presupuesto, @fehca_reparacion, @fecha_retiro, @repuesto)";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@clientedni", obj.Cliente.DNI));
                //cmd.Parameters.Add(new SQLiteParameter("@clientedni", obj.ClienteDNI));
                cmd.Parameters.Add(new SQLiteParameter("@presupuesto", obj.Presupuesto));
                cmd.Parameters.Add(new SQLiteParameter("@fehca_reparacion", obj.Fecha_reparacion));
                cmd.Parameters.Add(new SQLiteParameter("@fecha_retiro", obj.Fecha_retiro));
                cmd.Parameters.Add(new SQLiteParameter("@repuesto", obj.Repuesto));
                cmd.CommandType = System.Data.CommandType.Text;

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }

            }

            return respuesta;
        }


        // Traer ordenes que tenga un cliente en especifico
        public static List<Orden> OrdenesdeCliente(Cliente obj)
        {
            List<Orden> ordenes = new List<Orden>();

            using(SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string queary = "SELECT * from Orden WHERE ClienteDNI = @dni";

                SQLiteCommand cmd = new SQLiteCommand(queary, con);
                cmd.Parameters.Add(new SQLiteParameter("@dni", obj.DNI));
                cmd.CommandType = System.Data.CommandType.Text;
                    
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ordenes.Add(new Orden()
                        {
                            OrdenID = int.Parse(reader["OrdenID"].ToString()),
                            //ClienteDNI = reader["ClienteDNI"].ToString(),
                            Cliente = obj,
                            Presupuesto = int.Parse(reader["Presupuesto"].ToString()),
                            Fecha_reparacion = reader["Fecha_reparacion"].ToString(),
                            Fecha_retiro = reader["Fecha_retiro"].ToString(),
                            Repuesto = reader["Repuesto"].ToString()
                        });
                    }
                }

            }

            return ordenes;
        }

        // Buscar orden por nro de orden
        public static Orden TraerOrden(int nroOrden)
        {
            Orden orden = new Orden();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string queary = "SELECT * from Orden WHERE OrdenID = @nroOrden";

                SQLiteCommand cmd = new SQLiteCommand(queary, con);
                cmd.Parameters.Add(new SQLiteParameter("@nroOrden", nroOrden));
                cmd.CommandType = System.Data.CommandType.Text;

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        orden.OrdenID = int.Parse(reader["OrdenID"].ToString());
                        //orden.ClienteDNI = reader["ClienteDNI"].ToString();
                        orden.Presupuesto = int.Parse(reader["Presupuesto"].ToString());
                        orden.Fecha_reparacion = reader["Fecha_reparacion"].ToString();
                        orden.Fecha_retiro = reader["Fecha_retiro"].ToString();
                        orden.Repuesto = reader["Repuesto"].ToString();

                        // Traer cliente
                        Cliente cliente = ClienteLogica.TraerCliente(orden);
                        orden.Cliente = cliente;
                    }

                }

            }

            return orden;
        }


        // Traer NroOrden|Dni|Cliente nombre todas las ordenes
        public List<Orden> TraerOrdenes(int nroOrden, string dni, string nombre)
        {

            List<Orden> ordenes = new List<Orden>();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT o.OrdenID, c.DNI, c.Nombre" +
                    "FROM Orden o" +
                    "INNER JOIN Cliente c ON o.ClienteDNI = c.DNI" +
                    "WHERE 1=1"; //Condicion siempre verdadera

                // Filtros dinamico
                if (nroOrden != 0)
                {
                    query += " AND o.OrdenID = @nroOrden";
                }
                if (!string.IsNullOrEmpty(dni))
                {
                    query += " AND c.DNI = @dni";
                }
                if (!string.IsNullOrEmpty(nombre))
                {
                    query += " AND c.Nombre LIKE @nombre";
                }

                // Agregar clausula de ordenamiento
                query += " ORDER BY c.Nombre ASC";

                SQLiteCommand cmd = new SQLiteCommand(query, con);

                // Asignar valores a los parametros
                if (nroOrden != 0)
                {
                    cmd.Parameters.Add(new SQLiteParameter("@nroOrden", nroOrden));
                }
                if (!string.IsNullOrEmpty(dni))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@dni", dni));
                }
                if (!string.IsNullOrEmpty(nombre))
                {
                    cmd.Parameters.Add(new SQLiteParameter("@nombre", nombre));
                }

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        int id = int.Parse(reader["OrdenID"].ToString());
                        ordenes.Add(new Orden()
                        {
                            OrdenID = id,                        
                            Presupuesto = int.Parse(reader["Presupuesto"].ToString()),
                            Fecha_reparacion = reader["Fecha_reparacion"].ToString(),
                            Fecha_retiro = reader["Fecha_retiro"].ToString(),
                            Repuesto = reader["Repuesto"].ToString(),
                            Cliente = ClienteLogica.TraerCliente(id)
                        });

                    }

                }
            }

            return ordenes;

        }


        // Dar por terminada una orden -- Pone la fecha actual a la orden
        public static bool TerminarOrden(Orden obj)
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "UPDATE Orden" +
                    "SET Fecha_retiro = CURRENT_DATE" +
                    "WHERE OrdenID = @nroOrden";

                SQLiteCommand cmd = new SQLiteCommand(query,con);
                cmd.Parameters.Add(new SQLiteParameter("@nroOrden",obj.OrdenID));

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }
            }


            return respuesta;
        }

        // Editar orden
        public static bool Editar(Orden obj)
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "UPDATE Orden" +
                    "SET ClienteDNI = @ClienteDni, Presupuesto = @Presupuesto, Fecha_reaparacion = @fr, Fecha_retiro = @fretiro, Repuesto = @repuesto" +
                    "WHERE OrdenID = @ordenID";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                //cmd.Parameters.Add(new SQLiteParameter("@ClienteDni", obj.ClienteDNI));
                cmd.Parameters.Add(new SQLiteParameter("@ClienteDni", obj.Cliente.DNI));
                cmd.Parameters.Add(new SQLiteParameter("@Presupuesto", obj.Presupuesto));
                cmd.Parameters.Add(new SQLiteParameter("@fr", obj.Fecha_reparacion));
                cmd.Parameters.Add(new SQLiteParameter("@fretiro", obj.Fecha_retiro));
                cmd.Parameters.Add(new SQLiteParameter("@ordenID", obj.OrdenID));
                cmd.Parameters.Add(new SQLiteParameter("@repuesto", obj.Repuesto));

                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }

            }

            return respuesta;
        }

        public static int CantOrden()
        {
            int resultado = 0;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT COUNT(*) AS TotalOrdenes FROM Orden";

                SQLiteCommand cmd = new SQLiteCommand(query, con);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        resultado = int.Parse(reader["TotalOrdenes"].ToString());
                    }
                    
                }

            }

                return resultado;
        }


    }
}
