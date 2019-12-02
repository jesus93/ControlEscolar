using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitioWeb.Models
{
    public class Alumnos
    {
        public int IdAlumno { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public bool Activo { get; set; }
    }
}