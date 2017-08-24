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
    [Route("api/CapacitacionesAreasConocimientos")]
    public class CapacitacionesAreasConocimientosController : Controller
    {
        private readonly SwTHDbContext db;

        public CapacitacionesAreasConocimientosController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/BasesDatos
        [HttpGet]
        [Route("ListarCapacionesAreasConocimientos")]
        public async Task<List<CapacitacionAreaConocimiento>> GetCapacionesAreasConocimientos()
        {
            try
            {
                return await db.CapacitacionAreaConocimiento.OrderBy(x => x.Descripcion).ToListAsync();
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new List<CapacitacionAreaConocimiento>();
            }
        }

        // GET: api/BasesDatos/5
        [HttpGet("{id}")]
        public async Task<Response> GetCapacitacionAreaConocimiento([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo no v�lido",
                    };
                }

                var CapacitacionAreaConocimiento = await db.CapacitacionAreaConocimiento.SingleOrDefaultAsync(m => m.IdCapacitacionAreaConocimiento == id);

                if (CapacitacionAreaConocimiento == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No encontrado",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Resultado = CapacitacionAreaConocimiento,
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        // PUT: api/BasesDatos/5
        [HttpPut("{id}")]
        public async Task<Response> PutCapacitacionAreaConocimiento([FromRoute] int id, [FromBody] CapacitacionAreaConocimiento CapacitacionAreaConocimiento)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo inv�lido"
                    };
                }

                var CapacitacionAreaConocimientoActualizar = await db.CapacitacionAreaConocimiento.Where(x => x.IdCapacitacionAreaConocimiento == id).FirstOrDefaultAsync();
                if (CapacitacionAreaConocimientoActualizar != null)
                {
                    try
                    {

                        CapacitacionAreaConocimientoActualizar.Descripcion = CapacitacionAreaConocimiento.Descripcion;
                        db.CapacitacionAreaConocimiento.Update(CapacitacionAreaConocimientoActualizar);
                        await db.SaveChangesAsync();

                        return new Response
                        {
                            IsSuccess = true,
                            Message = "Ok",
                        };

                    }
                    catch (Exception ex)
                    {
                        await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                        {
                            ApplicationName = Convert.ToString(Aplicacion.SwTH),
                            ExceptionTrace = ex,
                            Message = "Se ha producido una excepci�n",
                            LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                            LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                            UserName = "",

                        });
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "Error ",
                        };
                    }
                }




                return new Response
                {
                    IsSuccess = false,
                    Message = "Existe"
                };
            }
            catch (Exception)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Excepci�n"
                };
            }
        }

        // POST: api/BasesDatos
        [HttpPost]
        [Route("InsertarCapacitacionAreaConocimiento")]
        public async Task<Response> PostCapacitacionAreaConocimiento([FromBody] CapacitacionAreaConocimiento CapacitacionAreaConocimiento)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo inv�lido"
                    };
                }

                var respuesta = Existe(CapacitacionAreaConocimiento);
                if (!respuesta.IsSuccess)
                {
                    db.CapacitacionAreaConocimiento.Add(CapacitacionAreaConocimiento);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = "OK"
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = "Existe una capacitaci�n de una �rea de conocimiento con igual descripci�n"
                };

            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        // DELETE: api/BasesDatos/5
        [HttpDelete("{id}")]
        public async Task<Response> DeleteCapacitacionAreaConocimiento([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "M�delo no v�lido ",
                    };
                }

                var respuesta = await db.CapacitacionAreaConocimiento.SingleOrDefaultAsync(m => m.IdCapacitacionAreaConocimiento == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "No existe ",
                    };
                }
                db.CapacitacionAreaConocimiento.Remove(respuesta);
                await db.SaveChangesAsync();

                return new Response
                {
                    IsSuccess = true,
                    Message = "Eliminado ",
                };
            }
            catch (Exception ex)
            {
                await GuardarLogService.SaveLogEntry(new LogEntryTranfer
                {
                    ApplicationName = Convert.ToString(Aplicacion.SwTH),
                    ExceptionTrace = ex,
                    Message = "Se ha producido una excepci�n",
                    LogCategoryParametre = Convert.ToString(LogCategoryParameter.Critical),
                    LogLevelShortName = Convert.ToString(LogLevelParameter.ERR),
                    UserName = "",

                });
                return new Response
                {
                    IsSuccess = false,
                    Message = "Error ",
                };
            }
        }

        private Response Existe(CapacitacionAreaConocimiento CapacitacionAreaConocimiento)
        {
            var bdd = CapacitacionAreaConocimiento.Descripcion.ToUpper().TrimEnd().TrimStart();
            var CapacitacionAreaConocimientorespuesta = db.CapacitacionAreaConocimiento.Where(p => p.Descripcion.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            if (CapacitacionAreaConocimientorespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = "Existe un capacitaci�n de un �rea de conocimiento de igual descripci�n",
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = CapacitacionAreaConocimientorespuesta,
            };
        }
    }
}