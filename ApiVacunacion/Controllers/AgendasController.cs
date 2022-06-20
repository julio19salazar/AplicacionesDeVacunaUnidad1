using ApiVacunacion.Helpers;
using ApiVacunacion.Models;
using ApiVacunacion.Repositories;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiVacunacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AgendasController : ControllerBase
    {
        public itesrcne_181G0543Context Context { get; set; }
        public AgendaRepository repository;
        public AgendasController(itesrcne_181G0543Context context)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile("llave.json")
                }); ;
            }
            Context = context;
            repository = new AgendaRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var ListActividades = repository.GetAll();

            return Ok(ListActividades.Select(x => new
            {
                x.Id,
                x.Actividad,
                x.FechaDeRealizar,
                x.Importancia
            }));

        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var agenda = repository.Get(id);
            if (agenda == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new
                {
                    agenda.Id,
                    agenda.Actividad,
                    agenda.FechaDeRealizar,
                    agenda.Importancia
                });
            }
        }
        [HttpPost("sincronizar")]
        public IActionResult Post([FromBody] DateTime fecha)
        {
            var personas = repository.GetAllSinceDate(fecha.ToMexicoTime());
            return Ok(personas.Select(x => new
            {
                x.Id,
                x.Actividad,
                x.FechaDeRealizar,
                x.Importancia
            }));
        }

        [HttpPost]
        public async Task< IActionResult> Post([FromBody]Agenda a)
        {
            try
            {
                a.Id = 0;
                if (repository.IsValid(a, out List<string> errores))
                {
                    repository.Insert(a);
                    Message message = new Message();
                    message.Topic = "chat";
                    message.Data = new Dictionary<string, string>
            {
                {"Id", a.Id.ToString()},
                  {"Actividad", a.Actividad},
                {"FechaDeRealizar", a.FechaDeRealizar},
                {"Importancia", a.Importancia},
                {"Accion","Agregar" }
            };
                    await FirebaseMessaging.DefaultInstance.SendAsync(message);

                    return Ok();

                 
                 
                }
                else
                {
                    return BadRequest(errores);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Agenda a)
        {
            try
            {

                var agenda = repository.Get(a.Id);
                if (agenda == null)
                    return NotFound();

                if (repository.IsValid(a, out List<string> errores))
                {

                    agenda.Actividad = a.Actividad;
                    agenda.FechaDeRealizar = a.FechaDeRealizar;
                    agenda.Importancia = a.Importancia;


                    repository.Update(agenda);
                    Message message = new Message();
                    message.Topic = "chat";
                    message.Data = new Dictionary<string, string>
            {
                {"Id", a.Id.ToString()},
                  {"Actividad", a.Actividad},
                {"FechaDeRealizar", a.FechaDeRealizar},
                {"Importancia", a.Importancia},
                {"Accion","Editar" }
            };
                  
                    await FirebaseMessaging.DefaultInstance.SendAsync(message);
               
                    return Ok();
                }
                else
                {
                    return BadRequest(errores);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Agenda a)
        {
            try
            {

                var agenda = repository.Get(a.Id);
                if (agenda == null)
                    return NotFound();

                repository.Delete(agenda);
                Message message = new Message();
                message.Topic = "chat";
                message.Data = new Dictionary<string, string>
            {
                {"Id", a.Id.ToString()},
                  {"Actividad", a.Actividad},
                {"FechaDeRealizar", a.FechaDeRealizar},
                {"Importancia", a.Importancia},
                {"Accion","Eliminar" }
            };
                await FirebaseMessaging.DefaultInstance.SendAsync(message);

                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) return StatusCode(500, ex.InnerException.Message);

                return StatusCode(500, ex.Message);
            }
        }


    }
}
