using ApiVacunacion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiVacunacion.Repositories
{
    public class AlumnoRepository : Repository<Alumno>
    {
        public AlumnoRepository(DbContext context) : base(context)
        {

        }
    }
}
