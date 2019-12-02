using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitioWeb.Models
{
    public class MateriasModel
    {
        public int IdMateria { get; set; }
        public int? IdAlumno { get; set; }
        public bool Activo { get; set; }
        public int? IdMateriaCat { get; set; }
        public string MateriaNombreCat { get; set; }
        public decimal CostoMateriaCat { get; set; }
    }
}