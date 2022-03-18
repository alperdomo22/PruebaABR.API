using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaABR.Class
{
    public class RetornoGeneral
    {
        public bool? Exitoso { get; set; }
        public int? Codigo { get; set; }
        public string Descripcion { get; set; }
    }

    public class Persona_CRUD
    {
        public int? PersonaID { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int? Edad { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public int? RolID { get; set; }
        public bool? Activo { get; set; }
        public bool Profesor { get; set; }
    }

    public class ClaseEliminarAlumno
    {
        public int PersonaID { get; set; }
    }

    public class Asignatura_CRUD
    {
        public int? AsignaturaID { get; set; }
        public int? PersonaID { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool? Activo { get; set; }
    }
}
