using ClinicaJulio.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ClinicaJulio.Services
{
    public class SincronizadorServices
    {
        public static event Action ActualizacionRealizada;
        public List<PersonasEntity> Buffer { get; set; }



        static HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("https://181g0543.81g.itesrc.net/")
        };
        public CatalogoPersonas Catalogo { get; set; }
        public SincronizadorServices(CatalogoPersonas p)
        {

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            Catalogo = p;
            DeserializarBuffer();
            //Verificar si ya se ha descargado algo antes

            //if (!Preferences.ContainsKey("FechaUltimaActualizacion"))
            //{ //Si no: Verifico si tengo conexion y descargo toda la bd, inicio el hilo de sincronizacion

            //    _ = DescargarPrimeraVezAsync();
            //    Sincronizar();
            //}
            //else
            //{
            //    Sincronizar();
            //    //Si: inicio el hilo de sincronización
            //}
            if (!Preferences.ContainsKey("FechaUltimaActualizacion"))
            {
                _ = DescargarPrimeraVezAsync();
                Sincronizar();

            }
            else
            {
                Sincronizar();
            }

        }
        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {

            await Descargar();




        }
        async Task DescargarPrimeraVezAsync()
        {

            //Verificar si existe conexión a internet
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var fecha = DateTime.Now;

                var result = await client.GetAsync("api/personas");

                if (result.IsSuccessStatusCode)
                {
                    Preferences.Set("FechaUltimaActualizacion", fecha);

                    string json = await result.Content.ReadAsStringAsync();
                    List<Personas> contactos = JsonConvert.DeserializeObject<List<Personas>>(json);

                    contactos.ForEach(x => Catalogo.InsertOrReplace(x));

                    if (contactos.Count > 0)
                    {

                        LanzarEvento();
                    }
                }


            }

        }
        Thread hs;
        void Sincronizar()
        {
            hs = new Thread(new ThreadStart(hiloSincronizador));
            hs.Start();
        }
        async void hiloSincronizador()
        {
            while (true)
            {

                await Descargar();

                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        public bool SeguirEnviandoEnBuffer { get; set; } = false;
        async Task CargarBuffer()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                SeguirEnviandoEnBuffer = true;
                if (Buffer.Count > 0)
                {
                    foreach (var persona in Buffer.ToArray())
                    {

                        switch (persona.Estado)
                        {
                            case Estado.Agregado:
                                
                               
                                var result = await EnviarAPI(persona.Personas, HttpMethod.Post);

                                if (result != null) Buffer.Remove(persona);
                                //Buffer.Remove(ce);
                                break;
                            case Estado.Modificado:
                                await EnviarAPI(persona.Personas, HttpMethod.Put);
                                Buffer.Remove(persona);
                                break;
                            case Estado.Eliminado:
                                await EnviarAPI(persona.Personas, HttpMethod.Delete);
                                Buffer.Remove(persona);
                                break;
                        }
                    }
                    SerializarBuffer();
                }
            }
        }
        private async Task Descargar()
        {

            if (Connectivity.NetworkAccess == NetworkAccess.Internet
                && Preferences.ContainsKey("FechaUltimaActualizacion")
                )
            {


                await CargarBuffer();


                Console.WriteLine("Descargar Iniciado");


                var json = JsonConvert.SerializeObject(
                Preferences.Get("FechaUltimaActualizacion",
                DateTime.MinValue));

                var fecha = DateTime.Now;


                var response = await client.PostAsync("api/Personas/sincronizar",
                 new StringContent(json, Encoding.UTF8, "application/json"));


                if (response.IsSuccessStatusCode)
                {
                    var datos = await response.Content.ReadAsStringAsync();

                    List<Personas> lista = JsonConvert.DeserializeObject<List<Personas>>(datos);

                    foreach (var item in lista)
                    {
                        Catalogo.InsertOrReplace(item);
                    }

                    Preferences.Set("FechaUltimaActualizacion", fecha);

                    if (lista.Count > 0)
                        LanzarEvento();

                }

                Console.WriteLine("Descargar Terminado");


            }

        }

        public async Task<List<string>> Agregar(Personas c)
        {
            //Validar
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return await EnviarAPI(c, HttpMethod.Post);
            }
            else
            {
                PersonasEntity ce = new PersonasEntity();
                ce.Personas = c;
                ce.Estado = Estado.Agregado;

                Buffer.Add(ce);
                SerializarBuffer();

                LanzarEvento();

                return null;
            }
        }

        public async Task<List<string>> Editar(Personas c)
        {
            //Validar
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return await EnviarAPI(c, HttpMethod.Put);
            }
            else
            {
                PersonasEntity ce = new PersonasEntity();
                ce.Personas = c;
                ce.Estado = Estado.Modificado;

                Buffer.Add(ce);
                SerializarBuffer();
                LanzarEvento();
                return null;
            }
        }

        public async Task<List<string>> Eliminar(Personas c)
        {
            //Validar
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return await EnviarAPI(c, HttpMethod.Delete);
            }
            else
            {
                PersonasEntity ce = new PersonasEntity();
                ce.Personas = c;
                ce.Estado = Estado.Eliminado;

                Buffer.Add(ce);
                SerializarBuffer();
                LanzarEvento();
                return null;
            }
        }

        private async Task<List<string>> EnviarAPI(Personas c, HttpMethod method)
        {
            List<string> errores = new List<string>();

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {

                    string json = JsonConvert.SerializeObject(c);

                    HttpRequestMessage request = new HttpRequestMessage();
                    request.Method = method;
                    request.RequestUri = new Uri(client.BaseAddress + "api/personas");
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");


                    var result = await client.SendAsync(request);

                    if (result.IsSuccessStatusCode)
                    {
                        await Descargar();
                        return null;
                    }

                    if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        json = await result.Content.ReadAsStringAsync();
                        var lista = JsonConvert.DeserializeObject<List<string>>(json);
                        return lista;
                    }

                    if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        errores.Add("La persona no existe.");
                        return errores;
                    }

                    if (result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        json = await result.Content.ReadAsStringAsync();
                        var mensaje = JsonConvert.DeserializeObject<string>(json);
                        errores.Add(mensaje);
                        return errores;
                    }

                    return null;
                }
                catch (Exception ex)
                {

                    errores.Add(ex.Message);
                    return errores;
                }

            }
            else
            {
                //trabajo desconectado
                errores.Add("No hay conexión a internet.");
                return null;
            }


        }


        private void LanzarEvento()
        {
            Xamarin.Forms.Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                ActualizacionRealizada?.Invoke();
            });
        }

        void SerializarBuffer()
        {
            var json = JsonConvert.SerializeObject(Buffer);
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/buffer.json";
            File.WriteAllText(ruta, json);
        }

        void DeserializarBuffer()
        {
            var ruta = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/buffer.json";
            try
            {
                if (File.Exists(ruta))
                {
                    var json = File.ReadAllText(ruta);
                    Buffer = JsonConvert.DeserializeObject<List<PersonasEntity>>(json);
                }
                else
                {
                    Buffer = new List<PersonasEntity>();
                }
            }
            catch
            {
                Buffer = new List<PersonasEntity>();
            }
        }

    }

}