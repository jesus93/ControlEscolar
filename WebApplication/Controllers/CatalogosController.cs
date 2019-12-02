using Common;
using Common.Dto;
using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApplication.Controllers
{
    [RoutePrefix(Constants.RoutePrefix)]
    public class CatalogosController : System.Web.Http.ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        
        [Route("GetCatCourses")]
        public async Task<IHttpActionResult> GetCatMaterias()
        {
            try
            {
                var catMaterias = unitOfWork.CatMateriasRepository.Get()?.Select(i => new MateriasCatDto()
                {
                    Activo = i.Activo,
                    Costo = i.Costo,
                    IdMateriaCat = i.IdMateriaCat,
                    Nombre = i.Nombre
                });
                return Ok(catMaterias);
            }
            catch(Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
            }
        }
    }
}