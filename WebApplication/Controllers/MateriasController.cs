using Common;
using Common.Dto;
using DAL;
using Entities;
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
    public class MateriasController : System.Web.Http.ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [Route("GetCoursesByStudentId")]
        public async Task<IHttpActionResult> GetMateriasByAlumnoId(int id)
        {
            try
            {
                if (id == 0)
                    return Ok(new { responseMsg = "Seleccione un elemento", HasError = true });

                var result = unitOfWork.MateriasRepository.Get(filter: i => i.IdAlumno == id,
                    orderBy: j => j.OrderBy(i => i.IdMateria)).Select(item => new MateriasDto()
                    {
                        Activo = item.Activo ?? false,
                        IdAlumno = item.IdAlumno ?? 0,
                        IdMateria = item.IdMateria,
                        IdMateriaCat = item.IdMateriaCat ?? 0
                    });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
            }
        }

        [Route("CalculateCost")]
        public async Task<IHttpActionResult> CalculateMaterias(IEnumerable<MATERIAS> materias)
        {
            try
            {
                decimal precioFinal = decimal.Zero;
                materias.ToList().ForEach(item =>
                {
                    if (item.IdMateriaCat != null && item.IdMateriaCat != 0 && item.Activo != null && (bool)item.Activo)
                    {
                        var precio = unitOfWork.CatMateriasRepository.GetByID((int)item.IdMateriaCat);
                        precioFinal += precio != null ? (decimal)precio.Costo : decimal.Zero;
                    }


                });

                return Ok(new { responseMsg = $"El costo final de materias es {precioFinal.ToString("C")}", HasError = false });
            }
            catch (Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
            }
        }

        [Route("SaveCourses")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveMaterias(SaveMateriasDto request)
        {
            try
            {
                if (request?.Materias != null)
                {
                    var materias = request.Materias.Select(j => new MATERIAS()
                    {
                        Activo = j.Activo,
                        IdAlumno = j.IdAlumno,
                        IdMateria = j.IdMateria,
                        IdMateriaCat = j.IdMateriaCat
                    });

                    materias.ToList().ForEach(item =>
                    {
                        if (item.IdMateriaCat != null && item.IdMateriaCat != 0 &&
                        item.Activo != null &&
                        item.IdAlumno != null && item.IdAlumno != 0
                        )
                        {
                            //var exist = unitOfWork.CatMateriasRepository.GetByID((int)item.IdMateriaCat);
                            if (item.IdMateria == 0)
                            {
                                unitOfWork.MateriasRepository.Insert(item);
                            }
                            else
                            {
                                unitOfWork.MateriasRepository.Update(item);
                            }
                            unitOfWork.Save();
                        }

                    });
                }


                return Ok(new { responseMsg = $"Se guardaron los cambios correctamente ", HasError = false });
            }
            catch (Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
            }
        }
    }
}