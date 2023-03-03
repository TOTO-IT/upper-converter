using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_case_Converter.Conexion
{
    public class ConexionBD
    {
        private SqlConnection conexion = new SqlConnection();


        public SqlConnection Pconexion
        {
            get
            {
                return this.conexion;
            }
            set
            {
                conexion = value;
            }
        }

        public void ConectoBase()
        {
            if (Pconexion.State == 0)
            {
                try
                {
                    Pconexion.ConnectionString = DoyConexion();
                    // Abro conexión
                    Pconexion.Open();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void DesconectoBase()
        {
            if (Pconexion.State > 0)
            {
                try
                {
                    Pconexion.Close();
                    Pconexion.Dispose();
                }
                catch (Exception ex)
                {
                }
            }
        }

        public bool EstadoDeConexion()
        {
            bool estado = false;
            if (Pconexion.State > 0)
                estado = true;
            return estado;
        }

        public string DoyConexion()
        {
            return "pooling=true;max pool size=100;data source = 10.100.59.21; initial catalog = Stock; user id = sa; password = adm1404";
            //return "pooling=true;max pool size=100;data source = 192.168.41.160; initial catalog = Stock; user id = sa; password = adm1404";
        }

    }
}
