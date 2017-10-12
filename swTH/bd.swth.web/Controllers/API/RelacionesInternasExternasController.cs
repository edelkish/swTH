using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bd.swth.datos;
using bd.swth.entidades.Negocio;
using bd.log.guardar.Servicios;
using bd.log.guardar.ObjectTranfer;
using bd.swth.entidades.Enumeradores;
using bd.log.guardar.Enumeradores;
using bd.swth.entidades.Utils;

namespace bd.swth.web.Controllers.API
{
    [Produces("application/json")]
    [Route("api/RelacionesInternasExternas")]
    public class RelacionesInternasExternasController : Controller
    {
        private readonly SwTHDbContext db;

        public RelacionesInternasExternasController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/BasesDatos
        [HttpGet]
        [Route("ListarRelacionesInternasExternas")]
        public async Task<List<RelacionesInternasExternas>> GetRelacionesInternasExternas()
        {
            try
            {
                return await db.RelacionesInternasExternas.OrderBy(x => x.Descripcion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<RelacionesInternasExternas>();
            }
        }




        [HttpPost]
        [Route("ListarElementosRIE")]
        public async Task<List<RelacionesInternasExternasIndiceOcupacional>> ListarElementosRIE([FromBody]IndiceOcupacional indiceOcupacional)
        {
            try
            {

                List<RelacionesInternasExternasIndiceOcupacional> ListaRIE = await db.RelacionesInternasExternasIndiceOcupacional
                                                   .Where(a => a.IndiceOcupacional.IdIndiceOcupacional == indiceOcupacional.IdIndiceOcupacional)
                                                   .ToListAsync();


                return ListaRIE;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<RelacionesInternasExternasIndiceOcupacional>();
            }
        }


        [HttpPost]
        [Route("ListarRIENoAsignadasIndiceOcupacional")]
        public async Task<List<RelacionesInternasExternas>> ListarRIENoAsignadasIndiceOcupacional([FromBody]IndiceOcupacional indiceOcupacional)
        {
            try
            {
                var ListaRIE = await db.RelacionesInternasExternas
                                   .Where(m => !db.RelacionesInternasExternasIndiceOcupacional
                                                   .Where(a => a.IndiceOcupacional.IdIndiceOcupacional == indiceOcupacional.IdIndiceOcupacional)
                                                   .Select(iom => iom.IdRelacionesInternasExternas)
                                                   .Contains(m.IdRelacionesInternasExternas))
                                          .ToListAsync();



                return ListaRIE;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<RelacionesInternasExternas>();
            }
        }



        [HttpPost]
        [Route("ListarRelacionesInternasExternasAsignadasIndiceOcupacional")]
        public async Task<RelacionesInternasExternasIndiceOcupacional> ListarRelacionesInternasExternasoAsignadasIndiceOcupacional2([FromBody]IndiceOcupacional indiceOcupacional)
        {
            try
            {

                var ListaRelacionesInternasExternas = await db.RelacionesInternasExternasIndiceOcupacional.Where(x => x.IdIndiceOcupacional == indiceOcupacional.IdIndiceOcupacional)
                                    .Include(x => x.RelacionesInternasExternas)
                                    .FirstOrDefaultAsync();
                

                return ListaRelacionesInternasExternas;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new RelacionesInternasExternasIndiceOcupacional();
            }
        }


        [HttpPost]
        [Route("ListarRelacionesInternasExternasNoAsignadasIndiceOcupacional")]
        public async Task<List<RelacionesInternasExternas>> ListarRelacionesInternasExternasoAsignadasIndiceOcupacional([FromBody]IndiceOcupacional indiceOcupacional)
        {
            try
            {
                var ListaRelacionesInternasExternas = await db.RelacionesInternasExternas
                                   .Where(rie => !db.RelacionesInternasExternasIndiceOcupacional
                                                   .Where(a => a.IndiceOcupacional.IdIndiceOcupacional == indiceOcupacional.IdIndiceOcupacional)
                                                   .Select(iorie => iorie.IdRelacionesInternasExternas)
                                                   .Contains(rie.IdRelacionesInternasExternas))
                                          .ToListAsync();
                return ListaRelacionesInternasExternas;
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<RelacionesInternasExternas>();
            }
        }


        [HttpPost]
        [Route("EliminarIncideOcupacionalRelacionesInternasExternas")]
        public async Task<Response> EliminarIncideOcupacionalRelacionesInternasExternas([FromBody] RelacionesInternasExternasIndiceOcupacional relacionesInternasExternasIndiceOcupacional)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var respuesta = await db.RelacionesInternasExternasIndiceOcupacional.SingleOrDefaultAsync(m => m.IdRelacionesInternasExternas == relacionesInternasExternasIndiceOcupacional.IdRelacionesInternasExternas
                                      && m.IdIndiceOcupacional == relacionesInternasExternasIndiceOcupacional.IdIndiceOcupacional);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.RelacionesInternasExternasIndiceOcupacional.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }



        // GET: api/BasesDatos/5
        [HttpGet("{id}")]
        public async Task<Response> GetRelacionesInternasExternas([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var RelacionesInternasExternas = await db.RelacionesInternasExternas.SingleOrDefaultAsync(m => m.IdRelacionesInternasExternas == id);

                if (RelacionesInternasExternas == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                    Resultado = RelacionesInternasExternas,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        // PUT: api/BasesDatos/5
        [HttpPut("{id}")]
        public async Task<Response> PutRelacionesInternasExternas([FromRoute] int id, [FromBody] RelacionesInternasExternas RelacionesInternasExternas)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido
                    };
                }

                var existe = Existe(RelacionesInternasExternas);
                if (existe.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ExisteRegistro,
                    };
                }

                var RelacionesInternasExternasActualizar = await db.RelacionesInternasExternas.Where(x => x.IdRelacionesInternasExternas == id).FirstOrDefaultAsync();

                if (RelacionesInternasExternasActualizar != null)
                {
                    try
                    {

                        RelacionesInternasExternasActualizar.Nombre = RelacionesInternasExternas.Nombre;
                        RelacionesInternasExternasActualizar.Descripcion = RelacionesInternasExternas.Descripcion;
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = Mensaje.Satisfactorio,
                        };

                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.SwTH),
                            ExceptionTrace = ex,
                            Message = Mensaje.Excepcion,
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                            UserName = "",

                        });
                        return new Response
                        {
                            IsSuccess = false,
                            Message = Mensaje.Error,
                        };
                    }
                }

                


                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ExisteRegistro
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Excepcion
                };
            }
        }

        // POST: api/BasesDatos
        [HttpPost]
        [Route("InsertarRelacionesInternasExternas")]
        public async Task<Response> PostRelacionesInternasExternas([FromBody] RelacionesInternasExternas RelacionesInternasExternas)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido
                    };
                }

                var respuesta = Existe(RelacionesInternasExternas);
                if (!respuesta.IsSuccess)
                {
                    db.RelacionesInternasExternas.Add(RelacionesInternasExternas);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.ExisteRegistro
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        // DELETE: api/BasesDatos/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteRelacionesInternasExternas([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido,
                    };
                }

                var respuesta = await db.RelacionesInternasExternas.SingleOrDefaultAsync(m => m.IdRelacionesInternasExternas == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.RelacionesInternasExternas.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.Satisfactorio,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = Mensaje.Excepcion,
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Error,
                };
            }
        }

        private Response Existe(RelacionesInternasExternas RelacionesInternasExternas)
        {
            var bdd = RelacionesInternasExternas.Descripcion;
            var RelacionesInternasExternasrespuesta = db.RelacionesInternasExternas.Where(p => p.Descripcion == bdd).FirstOrDefault();
            if (RelacionesInternasExternasrespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = Mensaje.ExisteRegistro,
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = RelacionesInternasExternasrespuesta,
            };
        }

    }
}
