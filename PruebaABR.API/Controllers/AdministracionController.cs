using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PruebaABR.Class;
using PruebaABR.API.Models;
using System.Net;

namespace PruebaABR.API.Controllers
{
    public class AdministracionController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Persona_CRUD(Persona_CRUD data)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string nomapierror = System.Reflection.MethodBase.GetCurrentMethod().Name + "Error";
            RetornoGeneral salida = new RetornoGeneral
            {
                Exitoso = false,
                Codigo = 0
            };
            try
            {
                if (ModelState.IsValid)
                {
                    using (PruebaABREntities db = new PruebaABREntities())
                    {
                        var consultaPersona = db.Persona.FirstOrDefault(x => x.PersonaID == data.PersonaID);
                        if (consultaPersona == null)
                        {
                            // Registro de nueva Persona
                            var consultaUsuarioIdentificacion = db.Persona.FirstOrDefault(x => x.Identificacion == data.Identificacion.Trim());
                            if (consultaUsuarioIdentificacion == null)
                            {
                                int? RolID = null;
                                if (data.Profesor)
                                {
                                    var consultaLlave = db.Configuracion.FirstOrDefault(x => x.Key == "RolProfesor");
                                    RolID = Convert.ToInt32(consultaLlave.Valor);
                                }
                                else
                                {
                                    var consultaLlave = db.Configuracion.FirstOrDefault(x => x.Key == "RolEstudiante");
                                    RolID = Convert.ToInt32(consultaLlave.Valor);
                                }
                                var nuevaPersona = new Persona
                                {
                                    Nombre = data.Nombre.Trim(),
                                    Apellido = data.Apellido.Trim(),
                                    Edad = data.Edad,
                                    Identificacion = data.Identificacion.Trim(),
                                    Direccion = data.Direccion.Trim(),
                                    Telefono = data.Telefono.Trim(),
                                    RolID = RolID,
                                    Activo = data.Activo
                                };

                                db.Persona.Add(nuevaPersona);
                                db.SaveChanges();

                                salida.Exitoso = true;
                                salida.Descripcion = "Registro exitoso";
                                salida.Codigo = nuevaPersona.PersonaID;
                            }
                            else
                            {
                                salida.Descripcion = $"Ya existe una persona registrada con la identificación {consultaUsuarioIdentificacion.Identificacion}";
                            }
                            goto retorno;
                        }
                        else
                        {
                            //Actualizar persona
                            if (data.Nombre != null && consultaPersona.Nombre != data.Nombre.Trim()) { consultaPersona.Nombre = data.Nombre.Trim(); }
                            if (data.Apellido != null && consultaPersona.Apellido != data.Apellido.Trim()) { consultaPersona.Apellido = data.Apellido.Trim(); }
                            if (data.Edad != null && consultaPersona.Edad != data.Edad) { consultaPersona.Edad = data.Edad; }
                            if (data.Identificacion != null && consultaPersona.Identificacion != data.Identificacion.Trim()) { consultaPersona.Identificacion = data.Identificacion.Trim(); }
                            if (data.Direccion != null && consultaPersona.Direccion != data.Direccion.Trim()) { consultaPersona.Direccion = data.Direccion.Trim(); }
                            if (data.Telefono != null && consultaPersona.Telefono != data.Telefono.Trim()) { consultaPersona.Telefono = data.Telefono.Trim(); }
                            if (data.RolID != null && consultaPersona.RolID != data.RolID) { consultaPersona.RolID = data.RolID; }
                            if (data.Activo != null && consultaPersona.Activo != data.Activo) { consultaPersona.Activo = data.Activo; }

                            db.SaveChanges();

                            salida.Exitoso = true;
                            salida.Descripcion = "Valor actualizado correctamente";
                            salida.Codigo = consultaPersona.PersonaID;
                            goto retorno;
                        }
                    }
                retorno:
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, salida);
                    }
                }
                else
                {
                    salida.Descripcion = "Ocurrió un error en la ejecución de la petición";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, salida);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, salida);
            }
        }

        [HttpPost]
        public HttpResponseMessage EliminarAlumno(ClaseEliminarAlumno data)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string nomapierror = System.Reflection.MethodBase.GetCurrentMethod().Name + "Error";
            RetornoGeneral salida = new RetornoGeneral
            {
                Exitoso = false,
                Codigo = 0
            };
            try
            {
                if (ModelState.IsValid)
                {
                    using (PruebaABREntities db = new PruebaABREntities())
                    {
                        var consultaPersona = db.Persona.FirstOrDefault(x => x.PersonaID == data.PersonaID);
                        if (consultaPersona != null)
                        {
                            var consultaAsignaturasAlumno = db.EstudianteAsignatura.Where(x => x.PersonaID == consultaPersona.PersonaID).ToList();
                            if (consultaAsignaturasAlumno.Count < 1)
                            {
                                db.Persona.Remove(consultaPersona);
                                db.SaveChanges();
                                salida.Exitoso = true;
                                salida.Descripcion = "Alumno eliminado correctamente";
                            }
                            else
                            {
                                salida.Descripcion = "No se puede eliminar el alumno porque tiene materias asignadas";
                            }
                        }
                        else
                        {
                            salida.Descripcion = "No se encontró la persona solicitada";
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
                    salida.Descripcion = "Ocurrió un error en la ejecución de la petición";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, salida);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, salida);
            }
        }

        [HttpPost]
        public HttpResponseMessage Asignatura_CRUD(Asignatura_CRUD data)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            string nomapierror = System.Reflection.MethodBase.GetCurrentMethod().Name + "Error";
            RetornoGeneral salida = new RetornoGeneral
            {
                Exitoso = false,
                Codigo = 0
            };
            try
            {
                if (ModelState.IsValid)
                {
                    using (PruebaABREntities db = new PruebaABREntities())
                    {
                        var consultaAsignatura = db.Asignatura.FirstOrDefault(x => x.AsignaturaID == data.AsignaturaID);
                        if (consultaAsignatura == null)
                        {
                            // Registro de nueva Asignatura
                            var consultaAsignaturaCodigo = db.Asignatura.FirstOrDefault(x => x.Codigo == data.Codigo);
                            if (consultaAsignaturaCodigo == null)
                            {
                                var nuevaAsignatura = new Asignatura
                                {
                                    Nombre = data.Nombre.Trim(),
                                    Codigo = data.Codigo.Trim(),
                                    Activo = data.Activo
                                };

                                db.Asignatura.Add(nuevaAsignatura);
                                db.SaveChanges();

                                if (data.PersonaID != null)
                                {
                                    var consultaPersona = db.Persona.FirstOrDefault(x => x.PersonaID == data.PersonaID);
                                    if (consultaPersona != null)
                                    {
                                        var nuevoProfesorAsignatura = new ProfesorAsignatura
                                        {
                                            PersonaID = consultaPersona.PersonaID,
                                            AsignaturaID = nuevaAsignatura.AsignaturaID,
                                            FechaAsignacion = DateTime.UtcNow,
                                            Activo = true
                                        };

                                        db.ProfesorAsignatura.Add(nuevoProfesorAsignatura);
                                        db.SaveChanges();
                                    }
                                }

                                salida.Exitoso = true;
                                salida.Descripcion = "Registro exitoso";
                                salida.Codigo = nuevaAsignatura.AsignaturaID;
                            }
                            else
                            {
                                salida.Descripcion = $"Ya existe una asignatura con el código {consultaAsignaturaCodigo.Codigo}";
                            }
                            goto retorno;
                        }
                        else
                        {
                            //Actualizar asignatura
                            if (data.Nombre != null && consultaAsignatura.Nombre != data.Nombre.Trim()) { consultaAsignatura.Nombre = data.Nombre.Trim(); }
                            if (data.Codigo != null && consultaAsignatura.Codigo != data.Codigo.Trim()) { consultaAsignatura.Codigo = data.Codigo.Trim(); }
                            if (data.Activo != null && consultaAsignatura.Activo != data.Activo) { consultaAsignatura.Activo = data.Activo; }
                            if (data.PersonaID != null)
                            {
                                bool registroProfesorAsignatura = true;
                                var consultaProfesorAsignatura = db.ProfesorAsignatura.FirstOrDefault(x => x.AsignaturaID == consultaAsignatura.AsignaturaID && x.Activo == true);
                                if (consultaProfesorAsignatura != null)
                                {
                                    if (data.PersonaID == consultaProfesorAsignatura.PersonaID)
                                    {
                                        registroProfesorAsignatura = false;
                                    }
                                    else
                                    {
                                        consultaProfesorAsignatura.Activo = false;
                                    }
                                }

                                if (registroProfesorAsignatura)
                                {
                                    var nuevoProfesorAsignatura = new ProfesorAsignatura
                                    {
                                        PersonaID = data.PersonaID,
                                        AsignaturaID = consultaAsignatura.AsignaturaID,
                                        FechaAsignacion = DateTime.UtcNow,
                                        Activo = true
                                    };
                                    db.ProfesorAsignatura.Add(nuevoProfesorAsignatura);
                                }
                            }
                            db.SaveChanges();

                            salida.Exitoso = true;
                            salida.Descripcion = "Valor actualizado correctamente";
                            salida.Codigo = consultaAsignatura.AsignaturaID;
                            goto retorno;
                        }
                    }
                retorno:
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, salida);
                    }
                }
                else
                {
                    salida.Descripcion = "Ocurrió un error en la ejecución de la petición";
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