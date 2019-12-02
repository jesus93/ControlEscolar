using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class MateriasDto
    {
        [JsonProperty]
        public int IdMateria { get; set; }
        [JsonProperty]
        public int IdAlumno { get; set; }
        [JsonProperty]
        public int IdMateriaCat { get; set; }
        [JsonProperty]
        public bool Activo { get; set; }
    }
}
