using ApiVacunacion.Helpers;
using ApiVacunacion.Models;
using ApiVacunacion.Repositories;
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
    public class PersonasController : ControllerBase
    {
        public itesrcne_181G0543Context Context { get; set; }
        public PersonaRepository repository;
        public PersonasController(itesrcne_181G0543Context context)
        {
            Context = context;
            repository = new PersonaRepository(context);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var ListaPeronas = repository.GetAll();

            return Ok(ListaPeronas.Select(x => new
            {
                x.Id,
                x.Nombre,
                x.Edad,
                x.Sexo,
                x.Vacuna,
                x.Lote
            }));

        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var persona = repository.Get(id);
            if (persona==null)
            {
                return NotFound();
            }
            else 
            {
                return Ok(new
                {
                    persona.Id,
                    persona.Nombre,
                    persona.Edad,
                    persona.Sexo,
                    persona.Vacuna,
                    persona.Lote
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
                x.Nombre,
                x.Edad,
                x.Sexo,
                x.Vacuna,
                x.Lote
            }));


        }

        [HttpPost]
        public IActionResult Post([FromBody] Persona p)
        {
            try
            {
                p.Id = 0;
                if (repository.IsValid(p, out List<string> errores))
                {
                    repository.Insert(p);
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
        public IActionResult Put([FromBody] Persona p)
        {
            try
            {

                var persona = repository.Get(p.Id);
                if (persona == null)
                    return NotFound();

                if (repository.IsValid(p, out List<string> errores))
                {

                    persona.Nombre = p.Nombre;
                    persona.Edad = p.Edad;
                    persona.Sexo = p.Sexo;
                    persona.Vacuna = p.Vacuna;
                    persona.Lote = p.Lote;

                    repository.Update(persona);
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
        public IActionResult Delete([FromBody] Persona c)
        {
            try
            {

                var contacto = repository.Get(c.Id);
                if (contacto == null)
                    return NotFound();

                repository.Delete(contacto);
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
