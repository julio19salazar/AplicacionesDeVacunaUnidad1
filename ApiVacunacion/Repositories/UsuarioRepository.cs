using ApiVacunacion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiVacunacion.Repositories
{
    public class UsuarioRepository: Repository<Usuario>
    {
        public UsuarioRepository(DbContext context) : base(context)
        {

        }

       public Usuario GetUserByName(string username)
        {
            return Context.Set<Usuario>().FirstOrDefault(x => x.NombreUsuario == username);
        }
    }
}
