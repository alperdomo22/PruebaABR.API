using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PruebaABR.API.Models;
using PruebaABR.Class;
using Newtonsoft.Json;

namespace PruebaABR.API.Controllers
{
    public class ConsultasController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ConsultaPersonas(ConsultaProfesores data)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string nomapierror = System.Reflection.MethodBase.GetCurrentMethod().Name + "Error";
            List<ConsultaProfesoresRetorno> salida = new List<ConsultaProfesoresRetorno>();
            try
            {
                if (ModelState.IsValid)
                {
                    using (PruebaABREntities db = new PruebaABREntities())
                    {
                        var consultaRolProfesor = db.Configuracion.FirstOrDefault(x => x.Key == "RolProfesor");
                        var consultaRolEstudiante = db.Configuracion.FirstOrDefault(x => x.Key == "RolEstudiante");
                        if (consultaRolProfesor != null && consultaRolEstudiante != null)
                        {
                            int rolSeleccionado = 0;
                            if (data.Profesores)
                            {
                                rolSeleccionado = Convert.ToInt32(consultaRolProfesor.Valor);
                            }
                            else
                            {
                                rolSeleccionado = Convert.ToInt32(consultaRolEstudiante.Valor);
                            }
                            salida = db.Persona.Where(x => x.RolID == rolSeleccionado).Select(y => new ConsultaProfesoresRetorno
                            { 
                                PersonaID = y.PersonaID,
                                Nombre = y.Nombre,
                                Apellido = y.Apellido,
                                Identificacion = y.Identificacion,
                                Telefono = y.Telefono,
                                Direccion = y.Direccion,
                                Edad = y.Edad,
                                Activo = y.Activo == true ? "Activo" : "Inactivo"
                            }).ToList();
                        }
                        goto retorno;
                    }
                retorno:
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, salida);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, salida);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, salida);
            }
        }

        [HttpPost]
        public HttpResponseMessage ConsultaAsignaturas(ConsultaProfesores data)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string nomapierror = System.Reflection.MethodBase.GetCurrentMethod().Name + "Error";
            List<ConsultaAsignaturasRetorno> salida = new List<ConsultaAsignaturasRetorno>();
            try
            {
                if (ModelState.IsValid)
                {
                    using (PruebaABREntities db = new PruebaABREntities())
                    {
                        salida = (from a in db.Asignatura
                                  from pa in db.ProfesorAsignatura.Where(x => x.AsignaturaID == a.AsignaturaID)
                                  from p in db.Persona.Where(x => x.PersonaID == pa.PersonaID)
                                  where (pa.Activo == true)
                                  select new ConsultaAsignaturasRetorno
                                  {
                                      AsignaturaID = a.AsignaturaID,
                                      Codigo = a.Codigo,
                                      Nombre = a.Nombre,
                                      PersonaID = p.PersonaID,
                                      Persona = p.Nombre,
                                      Activo = a.Activo == true ? "Activo" : "Inactivo"
                                  }).ToList();
                        goto retorno;
                    }
                retorno:
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, salida);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, salida);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, salida);
            }
        }
    }
}