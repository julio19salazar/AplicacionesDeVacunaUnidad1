using ApiVacunacion.Models;
using ApiVacunacion.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiVacunacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AlumnosController : ControllerBase
    {
        public itesrcne_181G0543Context Context { get; set; }
        public AlumnoRepository repository;
        public AlumnosController(itesrcne_181G0543Context context)
        {
            Context = context;
            repository = new AlumnoRepository(context);
        }
        [HttpGet]
        
        public IActionResult Get()
        {

            if (User.Identity.IsAuthenticated)
            {
                var ListaPeronas = repository.GetAll();

                return Ok(ListaPeronas.Select(x => new
                {
                    x.Id,
                    x.Nombre,
                    x.Edad,
                    x.Peso,
                    x.Area,

                }));
            }
            else
            {
                return BadRequest("Usted no tiene permisos");
            }
           

        }
    }
}
