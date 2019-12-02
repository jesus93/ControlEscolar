using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitioWeb.Models
{
    public class MateriasCatModel
    {
        public int IdMateriaCat { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public decimal Costo { get; set; }
    }
}