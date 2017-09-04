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
    [Route("api/EscalasEvaluacionesTotales")]
    public class EscalasEvaluacionesTotalesController : Controller
    {
        private readonly SwTHDbContext db;

        public EscalasEvaluacionesTotalesController(SwTHDbContext db)
        {
            this.db = db;
        }

        // GET: api/BasesDatos
        [HttpGet]
        [Route("ListarEscalasEvaluacionesTotales")]
        public async Task<List<EscalaEvaluacionTotal>> GetEscalasEvaluacionesTotales()
        {
            try
            {
                return await db.EscalaEvaluacionTotal.OrderBy(x => x.Descripcion).ToListAsync();
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
                return new List<EscalaEvaluacionTotal>();
            }
        }

        // GET: api/BasesDatos/5
        [HttpGet("{id}")]
        public async Task<Response> GetEscalaEvaluacionTotal([FromRoute] int id)
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

                var EscalaEvaluacionTotal = await db.EscalaEvaluacionTotal.SingleOrDefaultAsync(m => m.IdEscalaEvaluacionTotal == id);

                if (EscalaEvaluacionTotal == null)
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
                    Resultado = EscalaEvaluacionTotal,
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
        public async Task<Response> PutEscalaEvaluacionTotal([FromRoute] int id, [FromBody] EscalaEvaluacionTotal EscalaEvaluacionTotal)
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


                if (EscalaEvaluacionTotal.PorcientoDesde > EscalaEvaluacionTotal.PorcientoHasta)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "El porcentaje desde no puede ser mayor el porcentaje hasta"
                    };
                }

                var existe = Existe(EscalaEvaluacionTotal);
                if (existe.IsSuccess)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ExisteRegistro,
                    };
                }

                var EscalaEvaluacionTotalActualizar = await db.EscalaEvaluacionTotal.Where(x => x.IdEscalaEvaluacionTotal == id).FirstOrDefaultAsync();
                if (EscalaEvaluacionTotalActualizar != null)
                {
                    try
                    {

                        EscalaEvaluacionTotalActualizar.Descripcion = EscalaEvaluacionTotal.Descripcion;
                        EscalaEvaluacionTotalActualizar.Nombre = EscalaEvaluacionTotal.Nombre;
                        EscalaEvaluacionTotalActualizar.PorcientoDesde = EscalaEvaluacionTotal.PorcientoDesde;
                        EscalaEvaluacionTotalActualizar.PorcientoHasta = EscalaEvaluacionTotal.PorcientoHasta;
                        db.EscalaEvaluacionTotal.Update(EscalaEvaluacionTotalActualizar);
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
        [Route("InsertarEscalaEvaluacionTotal")]
        public async Task<Response> PostEscalaEvaluacionTotal([FromBody] EscalaEvaluacionTotal EscalaEvaluacionTotal)
        {
            try
            {
            
                if (EscalaEvaluacionTotal.PorcientoDesde> EscalaEvaluacionTotal.PorcientoHasta)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "El porcentaje desde no puede ser mayor el porcentaje hasta"
                    };
                }

                var respuesta = Existe(EscalaEvaluacionTotal);      
                if (!respuesta.IsSuccess)
                {
                    db.EscalaEvaluacionTotal.Add(EscalaEvaluacionTotal);
                    await db.SaveChangesAsync();
                    return new Response
                    {
                        IsSuccess = true,
                        Message = Mensaje.Satisfactorio
                    };
                }

                if (!ModelState.IsValid)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.ModeloInvalido
                    };
                }

                return new Response
                {
                    IsSuccess = false,
                    Message = Mensaje.Satisfactorio
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
        public async Task<Response> DeleteEscalaEvaluacionTotal([FromRoute] int id)
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

                var respuesta = await db.EscalaEvaluacionTotal.SingleOrDefaultAsync(m => m.IdEscalaEvaluacionTotal == id);
                if (respuesta == null)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = Mensaje.RegistroNoEncontrado,
                    };
                }
                db.EscalaEvaluacionTotal.Remove(respuesta);
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

        private Response Existe(EscalaEvaluacionTotal EscalaEvaluacionTotal)
        {
            var bdd = EscalaEvaluacionTotal.Nombre.ToUpper().TrimEnd().TrimStart();
            var EscalaEvaluacionTotalrespuesta = db.EscalaEvaluacionTotal.Where(p => p.Nombre.ToUpper().TrimStart().TrimEnd() == bdd).FirstOrDefault();
            if (EscalaEvaluacionTotalrespuesta != null)
            {
                return new Response
                {
                    IsSuccess = true,
                    Message = "Existe una escala de evaluación total de igual nombre",
                    Resultado = null,
                };

            }

            return new Response
            {
                IsSuccess = false,
                Resultado = EscalaEvaluacionTotalrespuesta,
            };
        }
    }
}