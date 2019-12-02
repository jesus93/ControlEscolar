using Common.Dto;

using Newtonsoft.Json;
using RestSharp;
using SitioWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SitioWeb.Controllers
{
    public class AlumnoController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult ShowCoursesByStudent(int id)
        {
            try
            {
                string path = ConfigurationManager.AppSettings["ApiServie"];
                if (string.IsNullOrEmpty(path))
                    throw new Exception("No se encontró la ruta del servicio");

                var client = new RestClient($"{path}");
                var request = new RestRequest($"GetCoursesByStudentId?id={id}", Method.GET);

                //request.AddParameter("id", id.ToString());
                IRestResponse<MateriasDto> response = client.Execute<MateriasDto>(request);
                var content = response.Content;


                var deserializeObj = JsonConvert.DeserializeObject<IEnumerable<MateriasDto>>(content, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                var materiasDes = deserializeObj.Select(item => new MateriasModel()
                {
                    Activo = item.Activo,
                    IdAlumno = item.IdAlumno,
                    IdMateria = item.IdMateria,
                    IdMateriaCat = item.IdMateriaCat
                });

                var requestCatalog = new RestRequest($"GetCatCourses", Method.GET);
                IRestResponse<MateriasCatDto> responseCat = client.Execute<MateriasCatDto>(requestCatalog);
                content = responseCat.Content;
                var catalogoDes = JsonConvert.DeserializeObject<IEnumerable<MateriasCatDto>>(content);
                var result = from cat in catalogoDes
                             join mat in materiasDes on cat.IdMateriaCat equals mat.IdMateriaCat into catGrp
                             from item in catGrp.DefaultIfEmpty(new MateriasModel { Activo = false })
                             select new MateriasModel()
                             {
                                 Activo = item.Activo,
                                 CostoMateriaCat = cat.Costo ?? decimal.Zero,
                                 IdAlumno = item.IdAlumno,
                                 IdMateria = item.IdMateria,
                                 IdMateriaCat = cat.IdMateriaCat,
                                 MateriaNombreCat = cat.Nombre
                             };
                //materiasDes?.ToList()?.ForEach(item =>
                //{

                //    item.MateriaNombreCat = catalogoDes.FirstOrDefault(i => i.IdMateriaCat == (int)item.IdMateriaCat )?.Nombre;
                //    item.CostoMateriaCat = catalogoDes.FirstOrDefault(i => i.IdMateriaCat == (int)item.IdMateriaCat )?.Costo ?? decimal.Zero;
                //}
                //);

                return View(new MateriasViewModel() { Materias = result });
            }
            catch (Exception ex)
            {
                //TODO: Return to index 
                log.Error(ex.ToString());
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult SaveCourses(int? model)
        {
            try
            {
                return View(new { idStudent = model });
            }
            catch (Exception ex)
            {

                //TODO: Return to index 
                log.Error(ex.ToString());
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult SaveMaterias(IEnumerable<MateriasDto> Materias)
        {
            try
            {
                string path = ConfigurationManager.AppSettings["ApiServie"];
                if (string.IsNullOrEmpty(path))
                    throw new Exception("No se encontró la ruta del servicio");

                var client = new RestClient($"{path}");
                var request = new RestRequest($"SaveCourses", Method.POST);
                request.AddParameter("request", new SaveMateriasDto()
                {
                    Materias = Materias
                });

                IRestResponse response = client.Execute(request);
                var content = response.Content;

                return RedirectToAction("ShowCoursesByStudent", "Alumno", new { id = Materias.FirstOrDefault(i => i.IdAlumno != 0).IdAlumno });
            }
            catch (Exception ex)
            {

                //TODO: Return to index 
                log.Error(ex.ToString());
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult CalculateCost(MateriasViewModel model)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                //TODO: Return to index 
                log.Error(ex.ToString());
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult LogIn(LoginModel model)
        {
            try
            {
                string path = ConfigurationManager.AppSettings["ApiServie"];
                if (string.IsNullOrEmpty(path))
                    throw new Exception("No se encontró la ruta del servicio");

                var client = new RestClient($"{path}");
                var request = new RestRequest($"GetStudentByName", Method.GET);
                request.AddParameter("name", model.Name);
                request.AddParameter("apePat", model.ApellidoPaterno);
                request.AddParameter("apeMat", model.ApellidoMaterno);

                //request.AddParameter("id", id.ToString());
                IRestResponse<AlumnosDto> response = client.Execute<AlumnosDto>(request);
                var content = response.Content;
                var deserializedObj = JsonConvert.DeserializeObject<AlumnosDto>(content);
                if (deserializedObj != null)
                {
                    return RedirectToAction("ShowCoursesByStudent", "Alumno", new { id = deserializedObj.IdAlumno });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return RedirectToAction("Index", "Home");
            }
        }
    }
}