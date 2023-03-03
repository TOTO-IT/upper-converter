using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upper_case_Converter.Model;

namespace upper_case_Converter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Cliente> clientes = Metodos.Metodos.TraigoClientes();

            int actualizados = 0;
            int noActualizados = 0;
            Console.WriteLine($"Se han traído {clientes.Count} clientes. \nProcedo a la actualización.");
            clientes.ForEach(cliente =>
            {
                if (Metodos.Metodos.InsertTotoLink(cliente))
                {

                    actualizados++;
                }
                else
                {
                    noActualizados++;
                }

            });

            Console.WriteLine("Termina el script." + "\n" + actualizados + " Actualizado/s." + "\n" + noActualizados + " Rechazados.");
        }
    }
}
