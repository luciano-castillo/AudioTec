using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioTec.Modelo;

namespace AudioTec.Logica
{
    internal class OrdenLogica
    {


        private static OrdenLogica _instancia = null;
        public static int NroOrdenActual = UltimoNroOrden();

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
        public static List<Orden> TraerOrdenes(int nroOrden, string dni, string nombre)
        {

            List<Orden> ordenes = new List<Orden>();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT o.OrdenID, o.Presupuesto, o.Fecha_reparacion, o.Fecha_retiro, o.Repuesto, " +
                    "c.DNI, c.Nombre " +
                    "FROM Orden o " +
                    "INNER JOIN Cliente c ON o.ClienteDNI = c.DNI " +
                    "WHERE 1=1 "; //Condicion siempre verdadera

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
                    query += " AND c.Nombre LIKE @nombre COLLATE NOCASE";
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
                    cmd.Parameters.Add(new SQLiteParameter("@nombre", "%" + nombre + "%"));
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

        public static List<Orden> TraerOrdenes(){

           List<Orden> ordenes = new List<Orden>();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT o.OrdenID, o.Presupuesto, o.Fecha_reparacion, o.Fecha_retiro, o.Repuesto, " +
               "c.DNI, c.Nombre " +
               "FROM Orden o " +
               "INNER JOIN Cliente c ON o.ClienteDNI = c.DNI";


                SQLiteCommand cmd = new SQLiteCommand(query, con);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        int id = int.Parse(reader["OrdenID"].ToString());
                        ordenes.Add(new Orden()
                        {
                            Cliente = ClienteLogica.TraerCliente(id),
                            OrdenID = id,                            
                            Presupuesto = int.Parse(reader["Presupuesto"].ToString()),
                            Fecha_reparacion = reader["Fecha_reparacion"].ToString(),
                            Fecha_retiro = reader["Fecha_retiro"].ToString(),
                            Repuesto = reader["Repuesto"].ToString(),
                            
                        });

                    }

                }

            }
            
            return ordenes; 
        }

        public static List<Orden> TraerOrdenesNoTerminadas()
        {

            List<Orden> ordenes = new List<Orden>();

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT o.OrdenID, o.Presupuesto, o.Fecha_reparacion, o.Fecha_retiro, o.Repuesto, " +
               "c.DNI, c.Nombre " +
               "FROM Orden o " +
               "INNER JOIN Cliente c ON o.ClienteDNI = c.DNI " +
               "WHERE Fecha_retiro IS NULL OR Fecha_retiro = ''";


                SQLiteCommand cmd = new SQLiteCommand(query, con);

                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        int id = int.Parse(reader["OrdenID"].ToString());
                        ordenes.Add(new Orden()
                        {
                            Cliente = ClienteLogica.TraerCliente(id),
                            OrdenID = id,
                            Presupuesto = int.Parse(reader["Presupuesto"].ToString()),
                            Fecha_reparacion = reader["Fecha_reparacion"].ToString(),
                            Fecha_retiro = reader["Fecha_retiro"].ToString(),
                            Repuesto = reader["Repuesto"].ToString(),

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
                string query = "UPDATE Orden " +//espacio
                    "SET Fecha_retiro = CURRENT_DATE " +
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
                string query = "UPDATE Orden " +//esapcio
                    "SET ClienteDNI = @ClienteDni, Presupuesto = @Presupuesto, Fecha_reparacion = @fr, Fecha_retiro = @fretiro, Repuesto = @repuesto " +
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

        public static bool CargarDiagnosticoPresupuesto(Orden obj)
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "UPDATE Orden " +
                    "SET Presupuesto = @presupuesto, Repuesto = @repuesto " +
                    "WHERE OrdenID = @ordenID";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.Add(new SQLiteParameter("@presupuesto", obj.Presupuesto));
                cmd.Parameters.Add(new SQLiteParameter("@repuesto", obj.Repuesto));
                cmd.Parameters.Add(new SQLiteParameter("ordenID", obj.OrdenID));

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

        private static int UltimoNroOrden()
        {
            int resultado = 0;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT MAX(OrdenID) AS max FROM Orden";

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

        //Traer orden por Nro de DNI
        public static Orden TraerNroOrden(string dni)
        {
            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT * FROM Orden WHERE ClienteDNI = @dni ORDER BY OrdenID DESC LIMIT 1";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.AddWithValue("@dni", dni);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        //int id = int.Parse(reader["OrdenID"].ToString());
                        return new Orden
                        {
                            OrdenID = Convert.ToInt32(reader["OrdenID"]),
                            //OrdenID = id,
                            //Presupuesto = int.Parse(reader["Presupuesto"].ToString()),
                            //Fecha_reparacion = reader["Fecha_reparacion"].ToString(),
                            //Fecha_retiro = reader["Fecha_retiro"].ToString(),
                            //Repuesto = reader["Repuesto"].ToString(),
                            //Cliente = ClienteLogica.TraerCliente(id)
                        };
                    }
                }
            }
            return null;
        }

        public static bool EliminarOrden(int id)
        {
            bool respuesta = true;
            
            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "DELETE FROM Orden WHERE OrdenID = @ordenID";

                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.AddWithValue("@ordenID", id);
                   
                if (cmd.ExecuteNonQuery() < 1)
                {
                    respuesta = false;
                }

            }

            return respuesta;
        }

        public static bool ExisteOrden(int id)
        {
            bool respuesta = true;

            using (SQLiteConnection con = new SQLiteConnection(Conexion.cadena))
            {
                con.Open();
                string query = "SELECT * " +
                    "FROM Orden " +
                    "WHERE OrdenID = @id";
                
                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                int cantidad = Convert.ToInt32(cmd.ExecuteScalar());
                respuesta = cantidad > 0;

            }

            return respuesta;
        }

        public static void AumentarNroOrden()
        {
            NroOrdenActual++;
        }

    }
}
