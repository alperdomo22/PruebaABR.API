using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaABR.Class
{
    public class ConsultaProfesores {
        public bool Profesores { get; set; }
    }

    public class ConsultaProfesoresRetorno
    {
        public int? PersonaID { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identificacion { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public int? Edad { get; set; }
        public string Activo { get; set; }
    }

    public class ConsultaAsignaturasRetorno
    {
        public int? AsignaturaID { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int? PersonaID { get; set; }
        public string Persona { get; set; }
        public string Activo { get; set; }
    }
}
