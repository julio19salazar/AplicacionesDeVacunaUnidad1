using MiLogin.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MiLogin.Services
{
  public  class UserServices
    {
        public async Task<string> GetToken()
        {
            var x = await SecureStorage.GetAsync("MiToken");
            return x;
        }

        public DateTime ExpirationDate { get; set; }

        public bool EstoyAutenticado
        {
            get
            {
                var result = GetToken().Result;

                if (result == null) return false;

                var thandler = new JwtSecurityTokenHandler();
                var descriptor = thandler.ReadJwtToken(result);

                Identity = new ClaimsIdentity(descriptor.Claims);
                ExpirationDate = descriptor.ValidTo;
                var salida = result != null && DateTime.UtcNow < ExpirationDate;
                return salida;
            }
        }


        public ClaimsIdentity Identity { get; set; }

        public async Task<bool> Renovar()
        {
            //obtener user y password de securestorage

            var user = await SecureStorage.GetAsync("User");
            var pwd = await SecureStorage.GetAsync("Password");

            if (user != null && pwd != null)
            {
                return await IniciarSesion(new LoginModel
                {
                    NombreUsuario = user,
                    Contraseña = pwd
                });
            }

            return false;
        }


        public async Task<bool> IniciarSesion(LoginModel lm)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://181g0543.81g.itesrc.net/");

            var json = JsonConvert.SerializeObject(lm);

            var result = await client.PostAsync("api/login",
                new StringContent(json, Encoding.UTF8, "application/json"));

            SecureStorage.RemoveAll();


            if (result.IsSuccessStatusCode)
            {
                //recuperar el token

                var token = await result.Content.ReadAsStringAsync();

                await SecureStorage.SetAsync("MiToken", token);
                //Para renovación
                await SecureStorage.SetAsync("User", lm.NombreUsuario);
                await SecureStorage.SetAsync("Password", lm.Contraseña);

                var thandler = new JwtSecurityTokenHandler();
                var descriptor = thandler.ReadJwtToken(token);

                Identity = new ClaimsIdentity(descriptor.Claims);
                ExpirationDate = descriptor.ValidTo;

                return true;

            }
            else if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return false;
            }


            return true;
        }
     
        public void RedirigirPrincipal()
        {
            //Identity.RoleClaimType
            if (EstoyAutenticado)
            {
                App.Current.MainPage = new Views.VistaAlumnos();
            }
            else
            {
                App.Current.MainPage = new MiLogin.Views.LoginVIew();
            }

            var x = 2;
       
           
        }

        public void CerrarSesion()
        {
            SecureStorage.RemoveAll();
            Identity = null;
            ExpirationDate = DateTime.MinValue;
            App.Current.MainPage = new Views.LoginVIew();

        }
    }
}
