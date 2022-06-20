using ApiVacunacion.Models;
using ApiVacunacion.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiVacunacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public itesrcne_181G0543Context Context { get; set; }
        public UsuarioRepository repository;
        public LoginController(IConfiguration configuration, itesrcne_181G0543Context context)
        {
            Context = context;
            repository = new UsuarioRepository(context);
            Configuration = configuration;
        }


        [HttpPost]
        public IActionResult Post(Usuario model)
        {


            var usuarioEncontrado = repository.GetUserByName(model.NombreUsuario);
            if (usuarioEncontrado != null && usuarioEncontrado.NombreUsuario == model.NombreUsuario && usuarioEncontrado.Contraseña == model.Contraseña)
            {

                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, usuarioEncontrado.NombreUsuario));
                claims.Add(new Claim("Id", usuarioEncontrado.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, "1"));
                claims.Add(new Claim(JwtRegisteredClaimNames.Iss,
                    Configuration["JwtAuth:Issuer"]));
                claims.Add(new Claim(JwtRegisteredClaimNames.Exp,
                    DateTime.UtcNow.AddMinutes(2).ToString()));


                var handler = new JwtSecurityTokenHandler();


                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor();
                descriptor.Issuer = Configuration["JwtAuth:Issuer"];
                descriptor.Audience = Configuration["JwtAuth:Audience"];
                descriptor.Subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                descriptor.Expires = DateTime.UtcNow.AddMinutes(2);
                descriptor.SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(Configuration["JwtAuth:Key"])),
                    SecurityAlgorithms.HmacSha256);

                var token = handler.CreateToken(descriptor);



                return Ok(handler.WriteToken(token));



            }
            else
            {
                return Unauthorized("El usuario o la contraseña son incorrectos.");
            }
        }
    }
}
