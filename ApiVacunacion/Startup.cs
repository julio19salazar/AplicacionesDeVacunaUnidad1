using ApiVacunacion.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiVacunacion
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private readonly string _MyCors = "MyCors";
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

             services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                //Para que no me pida el token y entre al Login.
                options.LoginPath = "/Login/Post";
                //options.LogoutPath = "/Home/Logout";
                options.Cookie.Name = "jwtsitio";

            });

            services.AddDbContext<itesrcne_181G0543Context>(optionsBuilder =>
                          optionsBuilder.UseMySql("server=204.93.216.11;user=itesrcne_julio;password=181G0543;database=itesrcne_181G0543", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.29-mariadb")));



            services.AddCors(options =>
            {
                options.AddPolicy(name: _MyCors, builder =>
                 {
                     builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                     .AllowAnyHeader().AllowAnyMethod();
                 });
            }
            );



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>

            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.
                TokenValidationParameters
                {
                    ValidIssuer = Configuration["JwtAuth:Issuer"],
                    ValidAudience = Configuration["JwtAuth:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.
                    UTF8.GetBytes(Configuration["JwtAuth:Key"])),
                    ValidateIssuerSigningKey = true,
                };
            }
            );
            services.AddControllers();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRequestLocalization("es-MX");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(_MyCors);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
