using MarcTron.Plugin;
using MiLogin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MiLogin.ViewModels
{
   public class AlumnosViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<AlumnoModel> ListaAlumnos { get; set; } = new ObservableCollection<AlumnoModel>();
        public ICommand CerrarSesionCommand { get; set; }
        public AlumnosViewModel()
        {
            CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-9712565769296684/3488475179");
            //CrossMTAdmob.Current.LoadRewardedVideo("ca-app-pub-9712565769296684/2175393507");

            CerrarSesionCommand = new Command(CerrarMiSesion);
            CargarDatos();
            //CrossMTAdmob.Current.OnRewardedVideoAdLoaded += Current_OnRewardedVideoAdLoaded;
            //CrossMTAdmob.Current.OnRewardedVideoAdCompleted += Current_OnRewardedVideoAdCompleted;
            //CrossMTAdmob.Current.OnRewardedVideoAdClosed += Current_OnRewardedVideoAdClosed;
           
        }

       


        public void CerrarMiSesion()
        {
            if (CrossMTAdmob.Current.IsInterstitialLoaded())
                CrossMTAdmob.Current.ShowInterstitial();
            App.User.CerrarSesion();
        }


        async void CargarDatos()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://181g0543.81g.itesrc.net/");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {await App.User.GetToken()}");

            var result = await client.GetAsync("api/alumnos");

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();

                var lista = JsonConvert.DeserializeObject<List<AlumnoModel>>(json);
                var contenido = lista;
                lista.ForEach(x => ListaAlumnos.Add(x));
            }
            else
            {
                App.User.CerrarSesion();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void Lanzar(string propiedad = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propiedad));
        }
    }
}
