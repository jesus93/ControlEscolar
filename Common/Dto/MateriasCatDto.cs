using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class MateriasCatDto
    {
        public int IdMateriaCat { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<decimal> Costo { get; set; }
    }
}
