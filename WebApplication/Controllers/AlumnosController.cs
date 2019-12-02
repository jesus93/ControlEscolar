
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
    public class AlumnosController : System.Web.Http.ApiController
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        [Route("GetStudents")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var users = unitOfWork.AlumnoRepository.Get(
                    null,
                    orderBy: item => item.OrderBy(i => i.Nombre));
                return Ok(JsonConvert.SerializeObject(users));
            }
            catch (Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
            }
        }
        [Route("GetStudentByName")]
        public async Task<IHttpActionResult> GetAlumnoByName(string name, string apePat, string apeMat)
        {
            try
            {
                if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(apePat) && string.IsNullOrEmpty(apeMat))
                    return Ok(new { responseMsg = "Llene alguno de los campos de búsqueda", HasError = true });

                //TODO: Validate nulls on filters
                var user = unitOfWork.AlumnoRepository.Get(
                    filter: item => item.Nombre.ToUpper().Trim() == name.ToUpper().Trim() &&
                            item.ApellidoPaterno.ToUpper().Trim() == apePat.ToUpper().Trim() &&
                            item.ApellidoMaterno.ToUpper().Trim() == apeMat.ToUpper().Trim(),
                    includeProperties: "Materias",
                    orderBy: item => item.OrderBy(i => i.Nombre)).Select(j => new AlumnosDto{ 
                        Activo = j.Activo,
                        ApellidoMaterno = j.ApellidoMaterno,
                        ApellidoPaterno = j.ApellidoPaterno,
                        IdAlumno = j.IdAlumno,
                        Nombre = j.Nombre
                    }).FirstOrDefault();

                if (user == null)
                    return Ok(new { responseMsg = "No hay coincidencias", HasError = true });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
                //TODO: Implement log and custom response 
            }
        }

        [Route("GetStudentByID")]
        public async Task<IHttpActionResult> GetAlumnoById(int id)
        {
            try
            {
                if (id == 0)
                    return Ok(new { responseMsg = "Seleccione un elemento", HasError = true });

                var user = unitOfWork.AlumnoRepository.Get(
                    filter: item => item.IdAlumno == id,
                    includeProperties: "Materias",
                    orderBy: item => item.OrderBy(i => i.Nombre));

                if (user == null || user.Count() == 0)
                    return Ok(new { responseMsg = "No hay coincidencias", HasError = true });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
            }
        }

        [Route("InsertStudent")]
        public async Task<IHttpActionResult> InsertAlumno(ALUMNOS alumno)
        {
            try
            {
                if (alumno == null)
                    return Ok(new { responseMsg = "Datos vacíos", HasError = true });
                else
                {
                    alumno.Nombre.ValidateStringData();
                    alumno.ApellidoPaterno.ValidateStringData();
                    alumno.ApellidoMaterno.ValidateStringData();
                }

                if (ModelState.IsValid)
                {
                    unitOfWork.AlumnoRepository.Insert(alumno);
                    unitOfWork.Save();
                    return Ok(new { responseMsg = "Usuario agregado correctamente", HasError = false });
                }
                else
                    return Ok(new { responseMsg = "Datos no validos", HasError = true });
            }
            catch (Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
            }
        }

        //[Http]mo
        [Route("UpdateStudent")]
        public async Task<IHttpActionResult> UpdateAlumno(ALUMNOS alumno)
        {
            try
            {
                if (alumno == null)
                    return Ok(new { responseMsg = "Datos vacíos", HasError = true });
                else
                {
                    alumno.Nombre.ValidateStringData();
                    alumno.ApellidoPaterno.ValidateStringData();
                    alumno.ApellidoMaterno.ValidateStringData();
                }

                unitOfWork.AlumnoRepository.Update(alumno);
                unitOfWork.Save();
                return Ok(new { responseMsg = "Usuario modificado correctamente", HasError = false });
            }
            catch (Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
            }
        }

        [Route("DeleteStudent")]
        public async Task<IHttpActionResult> DeleteUser(ALUMNOS alumno)
        {
            try
            {
                unitOfWork.AlumnoRepository.Delete(alumno);
                unitOfWork.Save();
                return Ok(new { responseMsg = "Usuario borrado correctamente", HasError = false });
            }
            catch (Exception ex)
            {
                return Ok(new { responseMsg = $"Error: {ex.Message}", HasError = true });
            }
        }

    }
}