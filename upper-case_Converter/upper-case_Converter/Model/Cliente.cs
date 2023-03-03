using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upper_case_Converter.Model
{
    public class Cliente
    {

        public long IdClient { get; set; }
        public string Name { get; set; }
        public string CI { get; set; }
        public string DateOfBirth { get; set; }
        public string Telephone { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Genre { get; set; }
        public string IdClientClass { get; set; }
        public string Address { get; set; }
        public string Locality { get; set; }
        public string Locality2 { get; set; }
        public string IdDepartamento { get; set; }   
    }
}
