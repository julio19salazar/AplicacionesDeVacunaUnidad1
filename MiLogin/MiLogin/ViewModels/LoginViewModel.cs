using MarcTron.Plugin;
using MiLogin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MiLogin.ViewModels
{
   public class LoginViewModel: INotifyPropertyChanged
    {
        public LoginModel LoginModel { get; set; }
        public bool Indicador { get; set; }
        public string Error { get; set; }
        public ICommand IniciarSesionCommand { get; set; }

        public LoginViewModel()
        {
            CrossMTAdmob.Current.LoadRewardedVideo("ca-app-pub-9712565769296684/2175393507");
            LoginModel = new LoginModel();
            IniciarSesionCommand = new Command(IniciarSesion);
        }
        private void Recompensar()
        {
            if (CrossMTAdmob.Current.IsRewardedVideoLoaded())
                CrossMTAdmob.Current.ShowRewardedVideo();

        }
        private async void IniciarSesion()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
              
                Indicador = true;
                if (string.IsNullOrWhiteSpace(LoginModel.NombreUsuario) || string.IsNullOrWhiteSpace(LoginModel.Contraseña))
                {
                    Error = "Escriba las credenciales para iniciar sesión.";
                }
                else
                {
                    var result = await App.User.IniciarSesion(LoginModel);

                    if (!result)
                    {
                        Error = "Usuario o contraseña incorrectas.";
                    }
                    else
                    {
                        Recompensar();
                        App.User.RedirigirPrincipal();
                    }


                }
                Indicador = false;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Atención 🚫", "Acceda a una red wifi para poder realizar la operación", "OK");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
