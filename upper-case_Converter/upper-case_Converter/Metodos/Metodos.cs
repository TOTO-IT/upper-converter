using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upper_case_Converter.Conexion;
using upper_case_Converter.Model;

namespace upper_case_Converter.Metodos
{
    public class Metodos
    {

        public static ConexionBD c = new ConexionBD();

        public static List<Cliente> TraigoClientes()
        {
            List<Cliente> clientes = ConsultaClientes();
            return clientes;
        }



        internal static bool InsertTotoLink(Cliente unCliente)
        {

            c.ConectoBase();

            SqlTransaction sqlTran = c.Pconexion.BeginTransaction();

            SqlCommand cmd = c.Pconexion.CreateCommand();

            cmd.Transaction = sqlTran;


            string query1 = "INSERT INTO Totolink.DBO.Clientes(IdCliente, Nombre, FechaNacimiento, Teléfonos, Celular, EMail, Sexo, IdClienteAlternativo, AudFA, AudUA, Fch_Reg, Estado, Habilitado, Domicilio, Localidad2, IdDepartamento, IdClientesClase) " +
                             "values(@IdCliente, @Nombre, @FechaNacimiento, @Teléfonos, " +
                             "@Celular, @EMail, @Sexo, @IdClienteAlternativo, " +
                             "CONVERT(VARCHAR(10), GETDATE(), 112), 'Sistema', " +
                             "getdate(), 'N', 'S', @Domicilio, " +
                             "@Localidad1, @IdDepartamento, @idcliclase)";

            string query2 = @"
                              INSERT INTO Totolink.dbo.LogClienteParseMayus(idCliente, FechaLog)
                              VALUES(@IdCliente, GETDATE())                                
                            ";


            try
            {
                cmd.CommandText = query1;
                cmd.Parameters.AddWithValue("@IdCliente", unCliente.IdClient);
                cmd.Parameters.AddWithValue("@Nombre", unCliente.Name);
                cmd.Parameters.AddWithValue("@FechaNacimiento", unCliente.DateOfBirth);
                cmd.Parameters.AddWithValue("@Teléfonos", unCliente.Telephone);
                cmd.Parameters.AddWithValue("@Celular", unCliente.Phone);
                cmd.Parameters.AddWithValue("@EMail", unCliente.Email);
                cmd.Parameters.AddWithValue("@Sexo", unCliente.Genre);
                cmd.Parameters.AddWithValue("@IdClienteAlternativo", unCliente.CI);
                cmd.Parameters.AddWithValue("@Domicilio", unCliente.Address);
                cmd.Parameters.AddWithValue("@Localidad1", unCliente.Locality);
                cmd.Parameters.AddWithValue("@IdDepartamento", unCliente.IdDepartamento);
                cmd.Parameters.AddWithValue("@idcliclase", unCliente.IdClientClass);
                int filas = cmd.ExecuteNonQuery();

                if (filas > 0)
                {
                    cmd.CommandText = query2;
                    int fila = cmd.ExecuteNonQuery();
                    if (fila > 0)
                    {
                        sqlTran.Commit();
                        return true;
                    }
                    else
                    {
                        sqlTran.Rollback();
                        return false;
                    }
                }
                sqlTran.Rollback();
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                try
                {
                    sqlTran.Rollback();
                }
                catch (Exception rollBackException)
                {
                    Console.WriteLine(rollBackException.Message);
                }
            }
            finally
            {
                c.DesconectoBase();
            }
            return false;
        }

        internal static List<Cliente> ParseoUpper(List<Cliente> clientes)
        {
            List<Cliente> ret = new List<Cliente>();

            foreach (var item in clientes)
            {
                bool entra = false;
                if (item.Name != "")
                {
                    string aux = item.Name;
                    item.Name = item.Name.ToUpper();
                    if (aux != item.Name)
                        entra = true;
                }
                if (item.Address != "")
                {
                    string aux2 = item.Address;
                    item.Address = item.Address.ToUpper();
                    if (aux2 != item.Address)
                        entra = true;
                }
                if (item.Locality != "")
                {
                    string aux3 = item.Locality;
                    item.Locality = item.Locality.ToUpper();
                    if (aux3 != item.Locality)
                        entra = true;
                }
                if (entra)
                {
                    ret.Add(item);
                }
            }
            return ret;

            //foreach (var item in ret)
            //{
            //    string nombre = "";
            //    string domicilio = "";
            //    string localidad = "";
            //    string localidad2 = "";

            //    if (item.Name != "")
            //    {
            //        nombre = item.Name.ToUpper();
            //    }
            //    if (item.Address != "")
            //    {
            //        domicilio = item.Address.ToUpper();
            //    }
            //    if (item.Locality != "")
            //    {
            //        localidad = item.Locality.ToUpper();
            //    }
            //    if (item.Locality2 != "")
            //    {
            //        localidad2 = item.Locality2.ToUpper();
            //    }
            //    if (localidad2 != localidad)
            //    {
            //        localidad2 = localidad;
            //    }
            //    if (localidad == "" && localidad2 == "")
            //    {
            //        localidad = TraigoDepById(item.IdDepartamento);
            //    }
            //    ret.Add(new Cliente
            //    {
            //        IdClient = item.IdClient,
            //        Name = nombre,
            //        Address = domicilio,
            //        Locality = localidad,
            //        Locality2 = localidad2,
            //        IdDepartamento= item.IdDepartamento,
            //        Email= item.Email,
            //        CI = item.CI,
            //        DateOfBirth= item.DateOfBirth,
            //        Genre= item.Genre,
            //        IdClientClass = item.IdClientClass,
            //        Phone= item.Phone,
            //        Telephone= item.Telephone,
            //    });

            //}

        }



        #region queries
        internal static string TraigoDepById(int idDepartamento)
        {
            string query = "select Nombre " +
                           "from poscomm.dbo.Departamentos " +
                           "where IdDepartamento = @id";
            try
            {
                c.ConectoBase();
                SqlCommand cmd = new SqlCommand(query, c.Pconexion);

                cmd.Parameters.AddWithValue("@id", idDepartamento);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader["Nombre"].ToString();
                    }
                }
                return "";
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                c.DesconectoBase();
            }
        }

        private static List<Cliente> ConsultaClientes()
        {
            List<Cliente> ret = new List<Cliente>();
            Console.WriteLine("Trayendo clientes...");

            string query = @"
                            SELECT distinct top 10 cli.IdCliente, cli.Nombre, cli.IdClienteAlternativo, 
                            cli.FechaNacimiento, cli.Teléfonos, cli.Celular, 
                            cli.EMail, cli.Sexo, cliP.IdClientesClase, cli.Domicilio, 
                            cli.Localidad2, cli.IdDepartamento
                            FROM totolink.dbo.clientes cli 
                            join poscomm.dbo.clientes cliP on clip.IdCliente = cli.IdCliente
                            WHERE cli.IdCliente not in (SELECT isnull(IdCliente,0) FROM TOTOLINK.DBO.LogClienteParseMayus) AND
							ISNULL(cli.Domicilio, '*****') <> '*****' AND
                            ISNULL(cli.Localidad2, '*****') <> '*****' AND
                            cliP.IdClientesClase > 200 AND
                            cli.IdClientesClase = clip.IdClientesClase AND
							cli.IdRegistro = (SELECT TOP 1 IdRegistro 
												FROM totolink.dbo.clientes 
												WHERE IdCliente = cli.IdCliente 
												ORDER BY IdRegistro DESC) AND
							UPPER(cli.Nombre) != cli.Nombre COLLATE SQL_Latin1_General_CP1_CS_AS AND
							UPPER(cli.Domicilio) != cli.Domicilio COLLATE SQL_Latin1_General_CP1_CS_AS AND 
							UPPER(cli.Localidad2) != cli.Localidad2 COLLATE SQL_Latin1_General_CP1_CS_AS
                            ";

            try
            {
                c.ConectoBase();

                SqlCommand cmd = new SqlCommand(query, c.Pconexion);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ret.Add(new Cliente
                        {
                            IdClient = Convert.ToInt32(reader["IdCliente"].ToString()),
                            Name = reader["Nombre"].ToString().ToUpper(),
                            CI = reader["IdClienteAlternativo"].ToString(),
                            DateOfBirth = reader["FechaNacimiento"].ToString(),
                            Telephone = reader["Teléfonos"].ToString(),
                            Phone = reader["Celular"].ToString(),
                            Email = reader["EMail"].ToString(),
                            Genre = reader["Sexo"].ToString(),
                            IdClientClass = reader["IdClientesClase"].ToString(),
                            Address = reader["Domicilio"].ToString().ToUpper(),
                            Locality = reader["Localidad2"].ToString().ToUpper(),
                            IdDepartamento = reader["IdDepartamento"].ToString()
                        });
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                c.DesconectoBase();
            }

        }
        #endregion


    }
}
